using CourseProject.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseProject.Components
{
    public class TodoCategoryListViewComponent : ViewComponent
    {
        private ITodoRepository _todoRepo;

        public TodoCategoryListViewComponent(ITodoRepository todoRepo)
        {
            _todoRepo = todoRepo;
        }

        public IViewComponentResult Invoke()
        {
            var categories = _todoRepo.GetCategories();
            return View(categories);
        }
    }
}