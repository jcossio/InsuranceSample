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
    }
}
