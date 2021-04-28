using System;

namespace InsuranceSample.Web.Models
{
    public class CreateContractModel
    {
        public int ClientId { get; set; }
        public int PolicyId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public int CoverageMonths { get; set; }
        public decimal MonthlyPremium { get; set; }
    }
}
