using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CourseProject.Models;
using CourseProject.Services;

namespace CourseProject.Controllers
{
    public class CategoryController : Controller
    {
        private ITodoRepository _todoRepo;

        public CategoryController(ITodoRepository todoRepo)
        {
            _todoRepo = todoRepo;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(TodoCategory category)
        {
            if (ModelState.IsValid)
            {
                _todoRepo.AddCategory(category);
                _todoRepo.Save();
                return RedirectToRoute(new
                {
                    controller = "Todo",
                    action = "List"
                });
            }

            return View(category);
        }

        [HttpGet("edit/{id}")]
        public IActionResult Edit(int id)
        {
            var categoryToEdit = _todoRepo.GetCategory(id);

            if (categoryToEdit == null)
            {
                return RedirectToRoute(new
                {
                    controller = "Todo",
                    action = "List"
                });
            }

            return View(categoryToEdit);
        }

        [HttpPost("edit/{id}")]
        public IActionResult Edit(int id, TodoCategory category)
        {
            var categoryToEdit = _todoRepo.GetCategory(id);

            categoryToEdit.Name = category.Name;

            _todoRepo.UpdateCategory(categoryToEdit);
            _todoRepo.Save();
            return RedirectToRoute(new
            {
                controller = "Todo",
                action = "List"
            });
        }

        [HttpGet("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var categoryToDelete = _todoRepo.GetCategory(id);

            if (categoryToDelete == null)
            {
                return RedirectToAction(nameof(TodoController.List));
            }

            return View(categoryToDelete);
        }

        [HttpPost("delete/{id}")]
        public IActionResult DeleteConfirmed(int id)
        {
            var categoryToDelete = _todoRepo.GetCategory(id);

            _todoRepo.RemoveCategory(categoryToDelete);
            _todoRepo.Save();
            return RedirectToAction(nameof(TodoController.List));
        }
    }
}