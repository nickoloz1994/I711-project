using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseProject.Models.TodoViewModels
{
    public class TodoListViewModel
    {
        public IEnumerable<TodoItem> TodoItems { get; set; }
        public IEnumerable<TodoCategory> TodoCategories { get; set; }
        public TodoCategory Category { get; set; }
    }
}