using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseProject.Models.TodoViewModels
{
    public class TodoCreateEditViewModel
    {
        public TodoItem TodoItem { get; set; }
        public SelectList CategorySelectList { get; set; }
    }
}