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
    public class PolicyController : Controller
    {
        private readonly IRepository<Policy> policyRepository;

        public PolicyController(IRepository<Policy> policyRepository)
        {
            this.policyRepository = policyRepository;
        }

        // GET: PolicyController
        public ActionResult Index()
        {
            var policies = policyRepository.All();
            return View(policies);
        }

        // GET: PolicyController/Details/5
        public ActionResult Details(int id)
        {
            var contract = policyRepository.Get(id);
            return View(new[] { contract });
        }

        // GET: PolicyController/Create
        public ActionResult Create()
        {
            var coverages = new List<Coverage> {
                new Coverage { CoverType = CoverTypeModel.Earthquake.ToString()},
                new Coverage { CoverType = CoverTypeModel.Fire.ToString()},
                new Coverage { CoverType = CoverTypeModel.Loss.ToString()},
                new Coverage { CoverType = CoverTypeModel.Theft.ToString()}
            };
            return View(coverages);
        }

        // POST: PolicyController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PolicyModel model)
        {
            var policy = new Policy
            {
                Name = model.Name,
                Description = model.Description,
                MonthlyPremium = model.MonthlyPremium,
                RiskType = model.RiskType.ToString(),
                Coverages = model.Coverages.Select(c => new Coverage
                {
                    CoverType = c.CoverType.ToString(),
                    Percentage = c.Percentage
                }).ToList()
            };

            policyRepository.Add(policy);
            policyRepository.SaveChanges();

            return Ok("Policy Created");
        }

        // GET: PolicyController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PolicyController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, PolicyModel model)
        {
            var policy = policyRepository.Get(id);
            policy.Description = model.Description;
            policy.MonthlyPremium = model.MonthlyPremium;
            policy.RiskType = model.RiskType.ToString();
            policyRepository.Update(policy);
            policyRepository.SaveChanges();

            return NoContent();
        }

        // GET: PolicyController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PolicyController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, PolicyModel model)
        {
            var policy = policyRepository.Get(id);
            policy.Deleted = true;
            policyRepository.Update(policy);
            policyRepository.SaveChanges();

            return NoContent();
        }
    }
}
