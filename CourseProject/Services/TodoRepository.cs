using System.Collections.Generic;
using CourseProject.Data;
using CourseProject.Models;
using System.Linq;
using System;

namespace CourseProject.Services
{
    public class TodoRepository : Repository<TodoItem>, ITodoRepository
    {
        public TodoRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        public IEnumerable<TodoItem> GetAll(string ownerID)
        {
            return _context.TodoItems
                .Where(t => t.OwnerID == ownerID)
                .OrderBy(t => t.Date)
                .ToList();
        }

        public IEnumerable<TodoItem> GetDone(string ownerID)
        {
            return _context.TodoItems
                .Where(t => t.OwnerID == ownerID && t.Complete == true)
                .OrderByDescending(t => t.Date)
                .ToList();
        }

        public IEnumerable<TodoItem> GetPending(string ownerID)
        {
            return _context.TodoItems
                .Where(t => t.OwnerID == ownerID && t.Complete == false)
                .OrderBy(t => t.Date)
                .ToList();
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

        public IEnumerable<TodoItem> GetTodosByCategory(int id, string ownerID)
        {
            return _context.TodoItems
                .Where(t => t.CategoryId == id && t.OwnerID == ownerID)
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