using InsuranceSample.Domain.Models;

namespace InsuranceSample.Infrastructure.Repositories
{

    public class ContractRepository : GenericRepository<Contract>
    {
        public ContractRepository(InsuranceContext context) : base(context)
        {
        }
    }
}
