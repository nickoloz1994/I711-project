using CourseProject.Models;
using System.Collections.Generic;

namespace CourseProject.Services
{
    public interface ITodoRepository : IRepository<TodoItem>
    {
        void AddCategory(TodoCategory category);

        IEnumerable<TodoCategory> GetCategories();

        TodoCategory GetCategory(int id);

        IEnumerable<TodoItem> GetTodosByCategory(int id);

        void RemoveCategory(TodoCategory category);

        void UpdateCategory(TodoCategory category);
    }
}