using System;
using System.Collections.Generic;

namespace InsuranceSample.Domain.Models
{
    public class Policy
    {
        public Guid PolicyId { get; private set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal MonthlyPremium { get; set; }
        public string RiskType { get; set; }
        public virtual ICollection<Coverage> Coverages { get; set; }
        public bool Deleted { get; set; }

        public Policy()
        {
            PolicyId = Guid.NewGuid();
        }
    }
}
