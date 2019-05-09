using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAP.InsuranceSample.DataAccess
{
    public interface IPolicyRepository: IRepository<Policy>
    {
        // Additional methods
        Policy GetUndeletedPolicy(int PolicyId);
        bool PolicyExists(int PolicyId);
    }
}
