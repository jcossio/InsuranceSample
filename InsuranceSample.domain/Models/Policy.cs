using System.Collections.Generic;

namespace InsuranceSample.Domain.Models
{
    public class Policy
    {
        public int PolicyId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal MonthlyPremium { get; set; }
        public string RiskType { get; set; }
        public List<Coverage> Coverages { get; set; }
        public bool Deleted { get; set; }
    }
}
