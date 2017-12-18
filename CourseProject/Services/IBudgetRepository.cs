using CourseProject.Models;
using System.Collections.Generic;

namespace CourseProject.Services
{
    public interface IBudgetRepository : IRepository<Budget>
    {
        IEnumerable<Budget> GetAll(string ownerID);

        void AddExpense(int id, Expense expense);

        IEnumerable<Expense> GetExpenses(int id);

        IEnumerable<decimal> GetExpensesByMonth(string month);

        Expense GetExpense(int budgetId, int expenseId);

        void UpdateExpense(Expense expense);

        void DeleteExpense(Expense expense);
    }
}