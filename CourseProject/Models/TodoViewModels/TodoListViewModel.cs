using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseProject.Models.TodoViewModels
{
    public class TodoListViewModel
    {
        public IEnumerable<TodoItem> TodoItems { get; set; } = new List<TodoItem>();

        public TodoCategory Category { get; set; }
    }
}