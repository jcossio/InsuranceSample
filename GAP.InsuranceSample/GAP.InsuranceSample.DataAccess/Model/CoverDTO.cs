using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GAP.InsuranceSample.DataAccess.Model
{
    public class CoverDTO
    {
        public int CoverId { get; set; }
        public string Name { get; set; }
        public double Percentage { get; set; }
    }
}