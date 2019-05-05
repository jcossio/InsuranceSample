using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GAP.InsuranceSample.DataAccess.Model
{
    public class PolicyDTO
    {
        public int PolicyId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal MonthlyPremium { get; set; }
        public int RiskTypeId { get; set; }
        public string RiskTypeName { get; set; }
        public bool Deleted { get; set; }
        public List<CoverDTO> Covers { get; set; }
    }
}