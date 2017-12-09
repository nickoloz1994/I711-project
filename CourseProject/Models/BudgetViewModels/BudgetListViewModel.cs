using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseProject.Models.BudgetViewModels
{
    public class BudgetListViewModel
    {
        public IEnumerable<Budget> Budgets { get; set; }
        public Budget Budget { get; set; }
        public IEnumerable<Expense> Expenses { get; set; } = new List<Expense>();
    }
}