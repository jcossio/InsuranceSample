using InsuranceSample.Domain.Models;
using InsuranceSample.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceSample.Web.Controllers
{
    public class ClientController : Controller
    {
        private readonly IRepository<Client> clientRepository;

        public ClientController(IRepository<Client> clientRepository)
        {
            this.clientRepository = clientRepository;
        }

        public IActionResult Index(Guid id)
        {
            if (id == null)
            {
                var customers = clientRepository.All();

                return View(customers);
            }
            else
            {
                var customer = clientRepository.Get(id);

                return View(new[] { customer });
            }
        }


        // GET: ClientController/Details/5
        public ActionResult Details(Guid id)
        {
            var client = clientRepository.Get(id);
            return View(new[] { client });
        }

    }
}
