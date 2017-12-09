using System.Collections.Generic;
using CourseProject.Data;
using CourseProject.Models;
using System.Linq;

namespace CourseProject.Services
{
    public class BudgetRepository : Repository<Budget>, IBudgetRepository
    {
        public BudgetRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        public void AddExpense(int id, Expense expense)
        {
            var budget = Get(id);
            budget.Expenses.Add(expense);
        }

        public void DeleteExpense(Expense expense)
        {
            _context.Expenses.Remove(expense);
        }

        public Expense GetExpense(int budgetId, int expenseId)
        {
            return _context.Expenses
                .Where(e => e.BudgetId == budgetId && e.Id == expenseId)
                .FirstOrDefault();
        }

        public IEnumerable<Expense> GetExpenses(int id)
        {
            return _context.Expenses
                .Where(e => e.BudgetId == id)
                .ToList();
        }

        public void UpdateExpense(Expense expense)
        {
            _context.Expenses.Update(expense);
        }
    }
}