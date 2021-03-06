using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InsuranceSample.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InsuranceSample.Web.Pages.Policy
{
    public class IndexModel : PageModel
    {
        private readonly IRepository<Domain.Models.Policy> policyRepository;
        [BindProperty]
        public IEnumerable<Domain.Models.Policy> Policies { get; set; }

        public IndexModel(IRepository<Domain.Models.Policy> policyRepository)
        {
            this.policyRepository = policyRepository;
        }

        public void OnGet()
        {
            Policies = policyRepository.All();
        }
    }
}
