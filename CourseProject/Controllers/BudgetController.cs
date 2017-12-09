using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using CourseProject.Models;
using CourseProject.Services;
using CourseProject.Models.BudgetViewModels;
using System.Linq;

namespace CourseProject.Controllers
{
    [Route("budgets")]
    public class BudgetController : Controller
    {
        private IBudgetRepository _budgetRepo;

        public BudgetController(IBudgetRepository budgetRepo)
        {
            _budgetRepo = budgetRepo;
        }

        [HttpGet]
        public IActionResult List(int? id)
        {
            var vm = new BudgetListViewModel();
            IEnumerable<Budget> budgets = _budgetRepo.GetAll();

            if (id != null)
            {
                vm.Budgets = budgets;
                vm.Budget = _budgetRepo.Get((int)id);
                vm.Expenses = _budgetRepo.GetExpenses((int)id);
                return View(vm);
            }

            vm.Budgets = budgets;

            return View(vm);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        public IActionResult Create([Bind("Name,Amount,StartDate,EndDate")] Budget budget)
        {
            if (budget == null)
            {
                return BadRequest();
            }

            _budgetRepo.Add(budget);
            _budgetRepo.Save();
            return RedirectToAction(nameof(List));
        }

        [HttpGet("edit/{id}")]
        public IActionResult Edit(int id)
        {
            var budgetToEdit = _budgetRepo.Get(id);

            if (budgetToEdit == null)
            {
                return NotFound();
            }

            return View(budgetToEdit);
        }

        [HttpPost("edit/{id}")]
        public IActionResult Edit(int id, [Bind("Name,Amount,StarDate,EndDate")] Budget budget)
        {
            var budgetToEdit = _budgetRepo.Get(id);

            if (budgetToEdit == null)
            {
                return NotFound();
            }

            budgetToEdit.Name = budget.Name;
            budgetToEdit.Amount = budget.Amount;
            budgetToEdit.StartDate = budget.StartDate;
            budgetToEdit.EndDate = budget.EndDate;

            if (ModelState.IsValid)
            {
                _budgetRepo.Update(budgetToEdit);
                _budgetRepo.Save();
                return RedirectToAction(nameof(List));
            }

            return View(budget);
        }

        [HttpGet("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var budgetToDelete = _budgetRepo.Get(id);

            if (budgetToDelete == null)
            {
                return RedirectToAction(nameof(List));
            }

            return View(budgetToDelete);
        }

        [HttpPost("delete/{id}"), ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var budgetToDelete = _budgetRepo.Get(id);

            if (budgetToDelete == null)
            {
                return RedirectToAction(nameof(List));
            }

            _budgetRepo.Remove(budgetToDelete);
            _budgetRepo.Save();
            return RedirectToAction(nameof(List));
        }
    }
}