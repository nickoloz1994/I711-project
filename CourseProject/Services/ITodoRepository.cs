using CourseProject.Models;
using System.Collections.Generic;

namespace CourseProject.Services
{
    public interface ITodoRepository : IRepository<TodoItem>
    {
        IEnumerable<TodoItem> GetAll(string ownerID);

        IEnumerable<TodoItem> GetDone(string ownerID);

        IEnumerable<TodoItem> GetPending(string ownerID);

        void AddCategory(TodoCategory category);

        IEnumerable<TodoCategory> GetCategories();

        TodoCategory GetCategory(int id);

        IEnumerable<TodoItem> GetTodosByCategory(int id, string ownerID);

        void RemoveCategory(TodoCategory category);

        void UpdateCategory(TodoCategory category);
    }
}