using GAP.InsuranceSample.BusinessLogic.Model;
using GAP.InsuranceSample.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GAP.InsuranceSample.BusinessLogic.Controllers
{
    /// <summary>
    /// Controller in charge of actions related to a policy
    /// </summary>
    public class PolicyController : ApiController
    {
        /// <summary>
        /// Get all available policies
        /// </summary>
        /// <returns>List of policy DTOs</returns>
        [HttpGet]
        [Route("api/policy/")]
        public IEnumerable<PolicyDTO> Get()
        {
            try
            {
                using (var db = new InsuranceEntities())
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
        /// <param name="PolicyId">Id of the policy to return</param>
        /// <returns>Policy DTO if found else 404</returns>
        [HttpGet]
        [Route("api/policy/{PolicyId:int}")]
        public PolicyDTO Get(int PolicyId)
        {
            try
            {
                using (var db = new InsuranceEntities())
                {
                    if (!db.Policy.Any(p => p.PolicyId == PolicyId && p.Deleted == false))
                        throw new HttpResponseException(HttpStatusCode.NotFound);

                    PolicyDTO policy = (from p in db.Policy
                                        join rt in db.RiskType on p.RiskTypeId equals rt.RiskTypeId
                                        where p.Deleted == false && p.PolicyId == PolicyId
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
        /// <param name="PolicyId">Id of the policy to delete</param>
        [HttpDelete]
        [Route("api/policy/{PolicyId:int}")]
        public void Delete(int PolicyId)
        {
            try
            {
                using (var db = new InsuranceEntities())
                {
                    if (!db.Policy.Any(p => p.PolicyId == PolicyId && p.Deleted == false))
                        throw new HttpResponseException(HttpStatusCode.NotFound);

                    var policy = db.Policy.Where(p => p.PolicyId == PolicyId)
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
        /// <param name="Policy">Policy DTO with covers</param>
        /// <returns>The created policy DTO</returns>
        [HttpPost]
        public PolicyDTO Add([FromBody]PolicyDTO Policy)
        {
            // Check parameters
            // Required Name and at least one cover
            if (Policy == null | string.IsNullOrEmpty(Policy.Name) || Policy.Covers == null || Policy.Covers.Count == 0)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            // [TODO] Move this to a BL layer
            // Check business logic
            // No cover higher than 50% on high risk
            if (Policy.RiskTypeId == 4)
            {
                if (Policy.Covers.Any(c => c.Percentage > 50))
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            // Add policy
            using (var db = new InsuranceEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var newPolicy = new Policy()
                        {
                            Name = Policy.Name,
                            Description = Policy.Description,
                            MonthlyPremium = Policy.MonthlyPremium,
                            RiskTypeId = Policy.RiskTypeId,
                            Deleted = false
                        };
                        db.Policy.Add(newPolicy);
                        db.SaveChanges();

                        // Add covers
                        foreach (var cover in Policy.Covers)
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

        /// <summary>
        /// Modify a policy. Deleted status not taken into accont as a modification.
        /// </summary>
        /// <param name="PolicyId">Id of the policy to delete</param>
        /// <param name="Policy">Policy DTO to modify</param>
        /// <returns>The modified policy DTO</returns>
        [HttpPut]
        [Route("api/policy/{PolicyId:int}")]
        public PolicyDTO Modify(int PolicyId, [FromBody]PolicyDTO Policy)
        {
            // Check parameters
            // Required Name and at least one cover
            if (Policy == null || string.IsNullOrEmpty(Policy.Name) || Policy.Covers == null || Policy.Covers.Count == 0)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            // [TODO] Move this to a BL layer
            // Check business logic
            // No cover higher than 50% on high risk
            if (Policy.RiskTypeId == 4)
            {
                if (Policy.Covers.Any(c => c.Percentage > 50))
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            // Modify policy
            using (var db = new InsuranceEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        // Check if policy exists
                        if (!db.Policy.Any(p => p.PolicyId == PolicyId && p.Deleted == false))
                            throw new HttpResponseException(HttpStatusCode.NotFound);

                        var dbPolicy = db.Policy.Where(p => p.PolicyId == PolicyId)
                            .FirstOrDefault();
                        dbPolicy.Name = Policy.Name;
                        dbPolicy.Description = Policy.Description;
                        dbPolicy.MonthlyPremium = Policy.MonthlyPremium;
                        dbPolicy.RiskTypeId = Policy.RiskTypeId;
                        db.SaveChanges();

                        // Delete and recreate covers
                        var policyCovers = db.PolicyCover.Where(pc => pc.PolicyId == PolicyId);
                        foreach (var policyCover in policyCovers)
                        {
                            db.PolicyCover.Remove(policyCover);
                        }
                        foreach (var cover in Policy.Covers)
                        {
                            db.PolicyCover.Add(new PolicyCover()
                            {
                                PolicyId = PolicyId,
                                CoverId = cover.CoverId,
                                Percentage = cover.Percentage
                            });
                        }

                        db.SaveChanges();
                        dbContextTransaction.Commit();
                        // Return modified policy 
                        return Get(PolicyId);
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
