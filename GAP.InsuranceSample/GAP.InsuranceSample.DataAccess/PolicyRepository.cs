using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAP.InsuranceSample.DataAccess
{
    public class PolicyRepository : Repository<Policy>, IPolicyRepository
    {
        public PolicyRepository(InsuranceEntities context) : base(context)
        {
        }

        public Policy GetUndeletedPolicy(int PolicyId)
        {
            return InsuranceContext.Policy.Where(p => p.PolicyId == PolicyId && p.Deleted == false).FirstOrDefault();
        }

        public bool PolicyExists(int PolicyId)
        {
            return InsuranceContext.Policy.Any(p => p.PolicyId == PolicyId && p.Deleted == false);
        }

        public InsuranceEntities InsuranceContext
        {
            get { return Context as InsuranceEntities; }
        }
    }
}
