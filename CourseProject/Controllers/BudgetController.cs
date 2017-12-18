using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using CourseProject.Models;
using CourseProject.Services;
using CourseProject.Models.BudgetViewModels;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using CourseProject.Authorization;

namespace CourseProject.Controllers
{
    [Route("budgets")]
    public class BudgetController : Controller
    {
        private List<string> Months = new List<string>(new string[]
        {
            "January",
            "February",
            "March",
            "April",
            "May",
            "June",
            "July",
            "August",
            "September",
            "October",
            "November",
            "December"
        });

        private IBudgetRepository _budgetRepo;
        private IAuthorizationService _authorizationService;
        private UserManager<ApplicationUser> _userManager;

        public BudgetController(
            IBudgetRepository budgetRepo,
            IAuthorizationService authorizationService,
            UserManager<ApplicationUser> userManager)
        {
            _budgetRepo = budgetRepo;
            _authorizationService = authorizationService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> List(int? id)
        {
            var vm = new BudgetListViewModel();
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var userID = _userManager.GetUserId(User);
            IEnumerable<Budget> budgets = _budgetRepo.GetAll(userID);

            if (id != null)
            {
                vm.Budgets = budgets;
                vm.Budget = _budgetRepo.Get((int)id);
                vm.Expenses = _budgetRepo.GetExpenses((int)id);
                return View(vm);
            }

            var budget = _budgetRepo.GetAll(userID).FirstOrDefault();

            if (budget != null)
            {
                vm.Budget = budget;
                vm.Expenses = _budgetRepo.GetExpenses(budget.Id);
                vm.Budgets = budgets;

                return View(vm);
            }

            return View(vm);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([Bind("Name,Amount,StartDate,EndDate")] Budget budget)
        {
            if (!ModelState.IsValid)
            {
                return View(budget);
            }

            var newBudget = new Budget
            {
                Name = budget.Name,
                Amount = budget.Amount,
                StartDate = budget.StartDate,
                EndDate = budget.EndDate,
                OwnerID = _userManager.GetUserId(User)
            };

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                                                            User, newBudget,
                                                            Operations.Create);

            if (!isAuthorized.Succeeded)
            {
                return new ChallengeResult();
            }

            _budgetRepo.Add(newBudget);
            _budgetRepo.Save();
            return RedirectToAction(nameof(List));
        }

        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var budgetToEdit = _budgetRepo.Get((int)id);

            if (budgetToEdit == null)
            {
                return NotFound();
            }

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                                                                    User, budgetToEdit,
                                                                    Operations.Update);

            if (!isAuthorized.Succeeded)
            {
                return new ChallengeResult();
            }

            return View(budgetToEdit);
        }

        [HttpPost("edit/{id}")]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Amount,StarDate,EndDate")] Budget budget)
        {
            if (!ModelState.IsValid)
            {
                return View(budget);
            }

            var budgetToEdit = _budgetRepo.Get(id);

            if (budgetToEdit == null)
            {
                return NotFound();
            }

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                                                                User, budgetToEdit,
                                                                Operations.Update);

            if (!isAuthorized.Succeeded)
            {
                return new ChallengeResult();
            }

            budgetToEdit.Name = budget.Name;
            budgetToEdit.Amount = budget.Amount;
            budgetToEdit.StartDate = budget.StartDate;
            budgetToEdit.EndDate = budget.EndDate;

            TryValidateModel(budgetToEdit);
            if (ModelState.IsValid)
            {
                _budgetRepo.Update(budgetToEdit);
                _budgetRepo.Save();
                return RedirectToAction(nameof(List));
            }

            return View(budget);
        }

        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var budgetToDelete = _budgetRepo.Get((int)id);

            if (budgetToDelete == null)
            {
                //return RedirectToAction(nameof(List));
                return NotFound();
            }

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                                                                User, budgetToDelete,
                                                                Operations.Delete);

            if (!isAuthorized.Succeeded)
            {
                return new ChallengeResult();
            }

            return View(budgetToDelete);
        }

        [HttpPost("delete/{id}"), ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var budgetToDelete = _budgetRepo.Get(id);

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                                                                User, budgetToDelete,
                                                                Operations.Delete);

            if (!isAuthorized.Succeeded)
            {
                return new ChallengeResult();
            }

            _budgetRepo.Remove(budgetToDelete);
            _budgetRepo.Save();
            return RedirectToAction(nameof(List));
        }

        [HttpGet("report")]
        public IActionResult Report()
        {
            var vm = new MonthlyReportViewModel();
            var count = 0;

            foreach (var month in Months)
            {
                var allExpenses = _budgetRepo.GetExpensesByMonth(month);

                if (allExpenses.Any())
                {
                    vm.MonthlyExpenses[count] = allExpenses.Sum();
                }

                count++;
            }

            return View(vm);
        }
    }
}