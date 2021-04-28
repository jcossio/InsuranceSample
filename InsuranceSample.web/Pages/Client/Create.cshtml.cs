using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InsuranceSample.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InsuranceSample.Web.Pages.Client
{
    public class CreateModel : PageModel
    {
        public IRepository<Domain.Models.Client> clientRepository { get; }

        [BindProperty]
        public Domain.Models.Client Client { get; set; }

        public CreateModel(IRepository<Domain.Models.Client> clientRepository)
        {
            this.clientRepository = clientRepository;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            clientRepository.Add(Client);
            clientRepository.SaveChanges();

            return RedirectToPage("/Index");
        }
    }
}
