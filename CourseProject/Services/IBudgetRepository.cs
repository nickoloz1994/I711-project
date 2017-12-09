using CourseProject.Models;
using System.Collections.Generic;

namespace CourseProject.Services
{
    public interface IBudgetRepository : IRepository<Budget>
    {
        void AddExpense(int id, Expense expense);

        IEnumerable<Expense> GetExpenses(int id);

        Expense GetExpense(int budgetId, int expenseId);

        void UpdateExpense(Expense expense);

        void DeleteExpense(Expense expense);
    }
}