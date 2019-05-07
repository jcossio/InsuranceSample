﻿using GAP.InsuranceSample.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GAP.InsuranceSample.DataAccess.Controllers
{
    /// <summary>
    /// Controller in charge of actions related to a Client
    /// </summary>
    public class ClientController : ApiController
    {
        /// <summary>
        /// Cancel a client contract. Only change the contract status.
        /// </summary>
        /// <param name="ClientId">Client Id</param>
        /// <param name="ContractId">Contract Id</param>
        [HttpDelete]
        public void CancelContract(int ClientId, int ContractId)
        {
            try
            {
                using (var db = new InsuranceDBEntities())
                {
                    // Check data 
                    if (!db.Client.Any(c => c.ClientId == ClientId))
                        throw new HttpResponseException(HttpStatusCode.BadRequest);
                    if (!db.ClientContract.Any(c => c.ClientContractId == ContractId && c.Cancelled == false))
                        throw new HttpResponseException(HttpStatusCode.NotFound);

                    var ClientContract = db.ClientContract.Where(c => c.ClientContractId == ContractId).FirstOrDefault();
                    ClientContract.Cancelled = true;
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

        [HttpPost]
        public HttpResponseMessage AddContract(ContractDTO Contract)
        {
            // Check parameters
            if (Contract.CoverageMonths == 0)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            // Add Contract
            using (var db = new InsuranceDBEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        // Check that client and policy exists
                        if (!db.Policy.Any(p => p.PolicyId == Contract.PolicyId && p.Deleted == false))
                            throw new HttpResponseException(HttpStatusCode.BadRequest);
                        if (!db.Client.Any(c => c.ClientId == Contract.ClientId))
                            throw new HttpResponseException(HttpStatusCode.BadRequest);

                        var policy = db.Policy.Where(p => p.PolicyId == Contract.PolicyId && p.Deleted == false).FirstOrDefault();
                        var newContract = new ClientContract()
                        {
                            ClientId = Contract.ClientId,
                            PolicyId = Contract.PolicyId,
                            EffectDate = Contract.EffectDate,
                            CoverageMonths = Contract.CoverageMonths,
                            MonthlyPremium = policy.MonthlyPremium,
                            RiskTypeId = policy.RiskTypeId,
                            Cancelled = false
                        };
                        db.ClientContract.Add(newContract);
                        db.SaveChanges();

                        var covers = db.PolicyCover.Where(c => c.PolicyId == Contract.PolicyId);
                        // Add coverage
                        foreach (var cover in covers)
                        {
                            db.ClientContractCover.Add(new ClientContractCover()
                            {
                                ClientContractCoverId = newContract.ClientContractId,
                                CoverId = cover.CoverId,
                                Percentage = cover.Percentage
                            });
                        }

                        db.SaveChanges();
                        dbContextTransaction.Commit();
                        // Return indication it has been created
                        return new HttpResponseMessage(HttpStatusCode.Created);
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
