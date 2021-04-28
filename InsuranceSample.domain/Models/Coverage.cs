using System;

namespace InsuranceSample.Domain.Models
{
    public class Coverage
    {
        public Guid CoverageId { get; set; }
        public string CoverType { get; set; }
        public float Percentage { get; set; }

        public virtual Policy Policy { get; set; }
        public Guid PolicyId { get; set; }

        public Coverage()
        {
            CoverageId = Guid.NewGuid();
        }
    }
}
