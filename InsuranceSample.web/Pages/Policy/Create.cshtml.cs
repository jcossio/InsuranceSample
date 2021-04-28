using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InsuranceSample.Infrastructure.Repositories;
using InsuranceSample.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InsuranceSample.Web.Pages.Policy
{
    public class CreateModel : PageModel
    {
        public IEnumerable<CoverType> Coverages { get; set; }

        public void OnGet()
        {
            Coverages = new List<CoverType> { new CoverType { Name = CoverTypeModel.Earthquake.ToString() },
                new CoverType { Name = CoverTypeModel.Fire.ToString() },
                new CoverType { Name = CoverTypeModel.Loss.ToString() },
                new CoverType { Name = CoverTypeModel.Theft.ToString() }
            };
        }
        
    }
}
