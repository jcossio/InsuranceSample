using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InsuranceSample.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InsuranceSample.Web.Pages.Client
{
    public class IndexModel : PageModel
    {
        private readonly IRepository<Domain.Models.Client> clientRepository;
        [BindProperty]
        public IEnumerable<Domain.Models.Client> Clients { get; set; }

        public IndexModel(IRepository<Domain.Models.Client> clientRepository)
        {
            this.clientRepository = clientRepository;
        }

        public void OnGet()
        {
            Clients = clientRepository.All();
        }
    }
}
