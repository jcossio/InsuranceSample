using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GAP.InsuranceSample.BusinessLogic.Model
{
    public class ContractDTO
    {
        public int PolicyId { get; set; }
        public DateTime EffectDate { get; set; }
        public short CoverageMonths { get; set; }
    }
}