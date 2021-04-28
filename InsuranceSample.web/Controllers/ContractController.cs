using InsuranceSample.Domain.Models;
using InsuranceSample.Infrastructure.Repositories;
using InsuranceSample.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceSample.Web.Controllers
{
    public class ContractController : Controller
    {
        private readonly IRepository<Contract> contractRepository;
        private readonly IRepository<Policy> policyRepository;

        public ContractController(IRepository<Contract> contractRepository, IRepository<Policy> policyRepository)
        {
            this.contractRepository = contractRepository;
            this.policyRepository = policyRepository;
        }

        // GET: ContractController
        public ActionResult Index()
        {
            var contracts = contractRepository.All();
            return View(contracts);
        }

        // GET: ContractController/Details/5
        public ActionResult Details(int id)
        {
            var contract = contractRepository.Get(id);
            return View(new[] { contract });
        }

        // GET: ContractController/Create
        public ActionResult Create()
        {
            var policies = policyRepository.All();
            return View(policies);
        }

        // POST: ContractController/Create
        [HttpPost]
        public ActionResult Create(CreateContractModel model)
        {
            var contract = new Contract
            {
                ClientId  = model.ClientId,
                PolicyId = model.PolicyId,
                EffectiveDate = model.EffectiveDate,
                CoverageMonths = model.CoverageMonths,
                MonthlyPremium = model.MonthlyPremium
            };

            contractRepository.Add(contract);
            contractRepository.SaveChanges();

            return Ok("Contract Created");
        }

        // GET: ContractController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ContractController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
