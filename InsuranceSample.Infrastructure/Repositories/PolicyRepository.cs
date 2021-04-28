using InsuranceSample.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceSample.Infrastructure.Repositories
{
    public class PolicyRepository : GenericRepository<Policy>
    {
        public PolicyRepository(InsuranceContext context) : base(context)
        {
        }
    }
}
