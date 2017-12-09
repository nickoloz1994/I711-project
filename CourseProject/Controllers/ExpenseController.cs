using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CourseProject.Services;
using CourseProject.Models;

namespace CourseProject.Controllers
{
    [Route("budgets")]
    public class ExpenseController : Controller
    {
        private IBudgetRepository _budgetRepo;

        public ExpenseController(IBudgetRepository budgetRepo)
        {
            _budgetRepo = budgetRepo;
        }

        [HttpGet("{budgetId}/[action]")]
        public IActionResult Add(int? budgetId)
        {
            if (budgetId != null)
            {
                return View();
            }

            return RedirectToRoute(new
            {
                controller = "Budget",
                action = "List"
            });
        }

        [HttpPost("{budgetId}/[action]")]
        public IActionResult Add(int budgetId, [Bind("Name,Amount,DateTime,Note")] Expense expense)
        {
            if (ModelState.IsValid)
            {
                _budgetRepo.AddExpense(budgetId, expense);
                _budgetRepo.Save();
                return RedirectToRoute(new
                {
                    controller = "Budget",
                    action = "List",
                    id = budgetId
                });
            }

            return View(expense);
        }

        [HttpGet("{budgetId}/[action]/{expenseId}")]
        public IActionResult Edit(int? budgetId, int? expenseId)
        {
            if (budgetId != null && expenseId != null)
            {
                var expense = _budgetRepo.GetExpense((int)budgetId, (int)expenseId);
                return View(expense);
            }

            return RedirectToRoute(new
            {
                controller = "Budget",
                action = "List",
            });
        }

        [HttpPost("{budgetId}/[action]/{expenseId}")]
        public IActionResult Edit(int? budgetId, int? expenseId, Expense expense)
        {
            if (budgetId == null || expenseId == null || expense == null)
            {
                return BadRequest();
            }

            var expenseToUpdate = _budgetRepo.GetExpense((int)budgetId, (int)expenseId);

            expenseToUpdate.Name = expense.Name;
            expenseToUpdate.DateTime = expense.DateTime;
            expenseToUpdate.Amount = expense.Amount;
            expenseToUpdate.Note = expense.Note;

            _budgetRepo.UpdateExpense(expenseToUpdate);
            _budgetRepo.Save();
            return RedirectToRoute(new
            {
                controller = "Budget",
                action = "List",
                id = budgetId
            });
        }

        [HttpGet("{budgetId}/[action]/{expenseId}")]
        public IActionResult Delete(int? budgetId, int? expenseId)
        {
            if (budgetId == null || expenseId == null)
            {
                return RedirectToRoute(new
                {
                    controller = "Budget",
                    action = "List"
                });
            }

            var expense = _budgetRepo.GetExpense((int)budgetId, (int)expenseId);
            return View(expense);
        }

        [HttpPost("{budgetId}/delete/{expenseId}"), ActionName("Delete")]
        public IActionResult DeleteConfirmed(int? budgetId, int? expenseId)
        {
            if (budgetId == null || expenseId == null)
            {
                return RedirectToRoute(new
                {
                    controller = "Budget",
                    action = "List"
                });
            }

            var expense = _budgetRepo.GetExpense((int)budgetId, (int)expenseId);
            _budgetRepo.DeleteExpense(expense);
            _budgetRepo.Save();
            return RedirectToRoute(new
            {
                controller = "Budget",
                action = "List",
                id = budgetId
            });
        }
    }
}