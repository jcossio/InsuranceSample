using System.Collections.Generic;

namespace InsuranceSample.Web.Models
{
    public class PolicyModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal MonthlyPremium { get; set; }
        public RiskType RiskType { get; set; }
        public List<CoverageModel> Coverages { get; set; }
    }
}
