using GAP.InsuranceSample.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GAP.InsuranceSample.DataAccess.Controllers
{
    public class PolicyController : ApiController
    {
        /// <summary>
        /// Get all available policies
        /// </summary>
        /// <returns>List of policy DTOs</returns>
        [HttpGet]
        public IEnumerable<PolicyDTO> Get()
        {
            try
            {
                using (var db = new InsuranceDBEntities())
                {
                    List<PolicyDTO> policies = (from p in db.Policy
                                                join rt in db.RiskType on p.RiskTypeId equals rt.RiskTypeId
                                                where p.Deleted == false
                                                select new PolicyDTO()
                                                {
                                                    PolicyId = p.PolicyId,
                                                    Name = p.Name,
                                                    Description = p.Description,
                                                    MonthlyPremium = p.MonthlyPremium,
                                                    RiskTypeId = p.RiskTypeId,
                                                    RiskTypeName = rt.Name,
                                                    Covers = (from pc in db.PolicyCover
                                                              join c in db.Cover on pc.CoverId equals c.CoverId
                                                              where pc.PolicyId == p.PolicyId
                                                              select new CoverDTO()
                                                              {
                                                                  CoverId = c.CoverId,
                                                                  Name = c.Name,
                                                                  Percentage = pc.Percentage
                                                              }).ToList()
                                                })
                        .OrderBy(p => p.Name)
                        .ToList();
                    return policies;
                }
            }
            catch 
            {
                // [TODO] Log the exception
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Return single policy
        /// </summary>
        /// <param name="id">Id of the policy to return</param>
        /// <returns>Policy DTO if found else 404</returns>
        [HttpGet]
        public PolicyDTO Get(int id)
        {
            try
            {
                using (var db = new InsuranceDBEntities())
                {
                    if (!db.Policy.Any(p => p.PolicyId == id))
                        throw new HttpResponseException(HttpStatusCode.NotFound);

                    PolicyDTO policy = (from p in db.Policy
                                        join rt in db.RiskType on p.RiskTypeId equals rt.RiskTypeId
                                        where p.Deleted == false && p.PolicyId == id
                                        select new PolicyDTO()
                                        {
                                            PolicyId = p.PolicyId,
                                            Name = p.Name,
                                            Description = p.Description,
                                            MonthlyPremium = p.MonthlyPremium,
                                            RiskTypeId = p.RiskTypeId,
                                            RiskTypeName = rt.Name,
                                            Covers = (from pc in db.PolicyCover
                                                      join c in db.Cover on pc.CoverId equals c.CoverId
                                                      where pc.PolicyId == p.PolicyId
                                                      select new CoverDTO()
                                                      {
                                                          CoverId = c.CoverId,
                                                          Name = c.Name,
                                                          Percentage = pc.Percentage
                                                      }).ToList()
                                        }).FirstOrDefault();
                    return policy;
                }
            }
            catch (HttpResponseException)
            {
                throw;
            }
            catch 
            {
                // [TODO] Log the exception
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Delete a policy. This is a soft delete.
        /// </summary>
        /// <param name="id">Id of the policy to delete</param>
        [HttpDelete]
        public void Delete(int id)
        {
            try
            {
                using (var db = new InsuranceDBEntities())
                {
                    if (!db.Policy.Any(p => p.PolicyId == id && p.Deleted == false))
                        throw new HttpResponseException(HttpStatusCode.NotFound);

                    var policy = db.Policy.Where(p => p.PolicyId == id)
                        .FirstOrDefault();
                    policy.Deleted = true;
                    db.SaveChanges();
                }
            }
            catch (HttpResponseException)
            {
                throw;
            }
            catch
            {
                // [TODO] Log the exception
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Add a new policy.
        /// </summary>
        /// <param name="policy">Policy DTO with covers</param>
        /// <returns>The created policy DTO</returns>
        [HttpPost]
        public PolicyDTO Add(PolicyDTO policy)
        {
            // Check parameters
            // Required Name and at least one cover
            if (string.IsNullOrEmpty(policy.Name) || policy.Covers == null || policy.Covers.Count == 0)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            // [TODO] Move this to a BL layer
            // Check business logic
            // No cover higher than 50% on high risk
            if (policy.RiskTypeId == 4)
            {
                if (policy.Covers.Any(c => c.Percentage > 50))
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            // Add policy
            using (var db = new InsuranceDBEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var newPolicy = new Policy()
                        {
                            Name = policy.Name,
                            Description = policy.Description,
                            MonthlyPremium = policy.MonthlyPremium,
                            RiskTypeId = policy.RiskTypeId,
                            Deleted = false
                        };
                        db.Policy.Add(newPolicy);
                        db.SaveChanges();

                        // Add covers
                        foreach (var cover in policy.Covers)
                        {
                            db.PolicyCover.Add(new PolicyCover()
                            {
                                PolicyId = newPolicy.PolicyId,
                                CoverId = cover.CoverId,
                                Percentage = cover.Percentage
                            });
                        }

                        db.SaveChanges();
                        dbContextTransaction.Commit();
                        // Return policy with newly generated Id
                        return Get(newPolicy.PolicyId);
                    }
                    catch (Exception)
                    {
                        dbContextTransaction.Rollback();
                        // [TODO] Log the exception
                        throw new HttpResponseException(HttpStatusCode.InternalServerError);
                    }
                }
            }
        }
    }
}
