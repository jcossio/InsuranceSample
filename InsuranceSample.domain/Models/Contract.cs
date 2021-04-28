using System;

namespace InsuranceSample.Domain.Models
{
    public class Contract
    {
        public Guid ContractId { get; private set; }
        public int ClientId { get; set; }
        public int PolicyId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime ContractDate { get; set; }
        public int CoverageMonths { get; set; }
        public decimal MonthlyPremium { get; set; }

        public Contract()
        {
            ContractId = Guid.NewGuid();
            ContractDate = DateTime.UtcNow;
        }
    }
}
