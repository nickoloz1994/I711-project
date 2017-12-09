using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using CourseProject.Models;
using CourseProject.Services;
using CourseProject.Models.TodoViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CourseProject.Controllers
{
    [Route("todos")]
    public class TodoController : Controller
    {
        private ITodoRepository _todoRepo;

        public TodoController(ITodoRepository todoRepo)
        {
            _todoRepo = todoRepo;
        }

        [HttpGet]
        public IActionResult List(int? id)
        {
            var vm = new TodoListViewModel();
            IEnumerable<TodoItem> todos = Enumerable.Empty<TodoItem>();
            IEnumerable<TodoCategory> categories = _todoRepo.GetCategories();
            var res = _todoRepo.GetCategories().FirstOrDefault(c => c.Id == 1);

            if (id != null && id != 1)
            {
                vm.Category = _todoRepo.GetCategory((int)id);
                vm.TodoItems = _todoRepo.GetTodosByCategory((int)id);
                vm.TodoCategories = categories;

                return View(vm);
            }

            vm.Category = res;
            vm.TodoItems = _todoRepo.GetAll();
            vm.TodoCategories = categories;

            return View(vm);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            var vm = new TodoCreateEditViewModel();
            var categories = _todoRepo.GetCategories();
            vm.CategorySelectList = new SelectList(categories, nameof(TodoCategory.Id), nameof(TodoCategory.Name));
            return View(vm);
        }

        [HttpPost("create")]
        public IActionResult Create(TodoCreateEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _todoRepo.Add(vm.TodoItem);
                _todoRepo.Save();
                return RedirectToAction(nameof(List));
            }

            var categories = _todoRepo.GetCategories();
            vm.CategorySelectList = new SelectList(categories, nameof(TodoCategory.Id), nameof(TodoCategory.Name));
            return View(vm);
        }

        [HttpGet("edit/{id}")]
        public IActionResult Edit(int id)
        {
            var vm = new TodoCreateEditViewModel();
            var categories = _todoRepo.GetCategories();
            var todo = _todoRepo.Get(id);

            if (todo == null)
            {
                return RedirectToAction(nameof(List));
            }

            vm.TodoItem = todo;
            vm.CategorySelectList = new SelectList(categories, nameof(TodoCategory.Id), nameof(TodoCategory.Name));

            return View(vm);
        }

        [HttpPost("edit/{id}")]
        public IActionResult Edit(int id, TodoCreateEditViewModel vm)
        {
            var todoToUpdate = _todoRepo.Get(id);

            if (todoToUpdate == null)
            {
                return NotFound();
            }

            todoToUpdate.Title = vm.TodoItem.Title;
            todoToUpdate.Date = vm.TodoItem.Date;
            todoToUpdate.CategoryId = vm.TodoItem.CategoryId;

            if (ModelState.IsValid)
            {
                _todoRepo.Update(todoToUpdate);
                _todoRepo.Save();
                return RedirectToAction(nameof(List));
            }

            return View(vm);
        }

        [HttpGet("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var todo = _todoRepo.Get(id);

            if (todo == null)
            {
                return NotFound();
            }

            return View(todo);
        }

        [HttpPost("delete/{id}")]
        public IActionResult DeleteConfirmed(int id)
        {
            var todo = _todoRepo.Get(id);

            _todoRepo.Remove(todo);
            _todoRepo.Save();
            return RedirectToAction(nameof(List));
        }
    }
}