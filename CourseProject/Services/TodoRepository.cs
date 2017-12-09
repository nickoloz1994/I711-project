using System.Collections.Generic;
using CourseProject.Data;
using CourseProject.Models;
using System.Linq;

namespace CourseProject.Services
{
    public class TodoRepository : Repository<TodoItem>, ITodoRepository
    {
        public TodoRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        public void AddCategory(TodoCategory category)
        {
            _context.TodoCategories.Add(category);
        }

        public IEnumerable<TodoCategory> GetCategories()
        {
            return _context.TodoCategories.ToList();
        }

        public TodoCategory GetCategory(int id)
        {
            return _context.TodoCategories.Find(id);
        }

        public IEnumerable<TodoItem> GetTodosByCategory(int id)
        {
            return _context.TodoItems
                .Where(t => t.CategoryId == id)
                .ToList();
        }

        public void RemoveCategory(TodoCategory category)
        {
            _context.TodoCategories.Remove(category);
        }

        public void UpdateCategory(TodoCategory category)
        {
            _context.TodoCategories.Update(category);
        }
    }
}