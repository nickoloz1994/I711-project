using System.Collections.Generic;
using CourseProject.Data;
using CourseProject.Models;
using System.Linq;
using System;

namespace CourseProject.Services
{
    public class BudgetRepository : Repository<Budget>, IBudgetRepository
    {
        public BudgetRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        public IEnumerable<Budget> GetAll(string ownerID)
        {
            return _context.Budgets
                .Where(b => b.OwnerID == ownerID &&
                       DateTime.Compare(b.EndDate, DateTime.Now) > 0)
                .OrderBy(b => b.StartDate)
                .ToList();
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
                .OrderBy(e => e.DateTime)
                .ToList();
        }

        public IEnumerable<decimal> GetExpensesByMonth(string month)
        {
            return _context.Expenses
                .Where(e => e.DateTime.Year == DateTime.Now.Year && e.DateTime.ToString("MMMM") == month)
                .Select(e => e.Amount)
                .ToList();
        }

        public void UpdateExpense(Expense expense)
        {
            _context.Expenses.Update(expense);
        }
    }
}