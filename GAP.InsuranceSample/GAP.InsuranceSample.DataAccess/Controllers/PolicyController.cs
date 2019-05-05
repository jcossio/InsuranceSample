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

    }
}
