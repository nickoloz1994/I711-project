using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using CourseProject.Models;
using CourseProject.Services;
using CourseProject.Models.TodoViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CourseProject.Authorization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using System;

namespace CourseProject.Controllers
{
    [Route("todos")]
    public class TodoController : Controller
    {
        private ITodoRepository _todoRepo;
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<ApplicationUser> _userManager;

        public TodoController(
            ITodoRepository todoRepo,
            IAuthorizationService authorizationService,
            UserManager<ApplicationUser> userManager)
        {
            _todoRepo = todoRepo;
            _authorizationService = authorizationService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> List(int? id, string filter)
        {
            var vm = new TodoListViewModel();
            IEnumerable<TodoItem> todos = new List<TodoItem>();
            var res = _todoRepo.GetCategories().FirstOrDefault(c => c.Id == 1);
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var userID = _userManager.GetUserId(User);

            if (!String.IsNullOrWhiteSpace(filter) && filter == "complete")
            {
                vm.Category = res;
                vm.TodoItems = _todoRepo.GetDone(userID);

                return View(vm);
            }
            else
            {
                if (filter == "pending")
                {
                    vm.Category = res;
                    vm.TodoItems = _todoRepo.GetPending(userID);

                    return View(vm);
                }
            }

            if (id != null && id != 1)
            {
                vm.Category = _todoRepo.GetCategory((int)id);
                vm.TodoItems = _todoRepo.GetTodosByCategory((int)id, userID);

                return View(vm);
            }

            vm.Category = res;
            vm.TodoItems = _todoRepo.GetAll(userID);

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
        public async Task<IActionResult> Create(TodoCreateEditViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var todo = new TodoItem
            {
                Title = vm.TodoItem.Title,
                Date = vm.TodoItem.Date,
                CategoryId = vm.TodoItem.CategoryId,
                Complete = vm.TodoItem.Complete,

                OwnerID = _userManager.GetUserId(User)
            };

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                                                                User, todo,
                                                                Operations.Create);

            if (!isAuthorized.Succeeded)
            {
                return new ChallengeResult();
            }

            _todoRepo.Add(todo);
            _todoRepo.Save();

            return RedirectToAction(nameof(List));
        }

        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todo = _todoRepo.Get((int)id);

            if (todo == null)
            {
                return NotFound();
            }

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                                                                 User, todo,
                                                                 Operations.Update);

            if (!isAuthorized.Succeeded)
            {
                return new ChallengeResult();
            }

            var vm = new TodoCreateEditViewModel();
            var categories = _todoRepo.GetCategories();

            vm.TodoItem = todo;
            vm.CategorySelectList = new SelectList(categories, nameof(TodoCategory.Id), nameof(TodoCategory.Name));

            return View(vm);
        }

        [HttpPost("edit/{id}")]
        public async Task<IActionResult> Edit(int id, TodoCreateEditViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var todo = _todoRepo.Get(id);
            if (todo == null)
            {
                return NotFound();
            }

            var isAuthorized = await _authorizationService.AuthorizeAsync(User, todo,
                                                                        Operations.Update);

            if (!isAuthorized.Succeeded)
            {
                return new ChallengeResult();
            }

            todo.Title = vm.TodoItem.Title;
            todo.Date = vm.TodoItem.Date;
            todo.Category = vm.TodoItem.Category;
            todo.Complete = vm.TodoItem.Complete;

            _todoRepo.Update(todo);
            _todoRepo.Save();

            return RedirectToAction(nameof(List));
        }

        [HttpPost("done/{id}")]
        public IActionResult Done(int id)
        {
            var todo = _todoRepo.Get(id);
            if (todo == null)
            {
                return NotFound();
            }

            var patchTodo = new TodoItem
            {
                Title = todo.Title,
                Date = todo.Date,
                Complete = !todo.Complete,
                Category = todo.Category,
                OwnerID = todo.OwnerID
            };

            TryValidateModel(patchTodo);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            todo.Title = patchTodo.Title;
            todo.Date = patchTodo.Date;
            todo.Complete = patchTodo.Complete;
            todo.Category = patchTodo.Category;
            todo.OwnerID = patchTodo.OwnerID;
            _todoRepo.Update(todo);
            _todoRepo.Save();
            return RedirectToAction(nameof(List));
        }

        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todo = _todoRepo.Get((int)id);
            if (todo == null)
            {
                return NotFound();
            }

            var isAuthorized = await _authorizationService.AuthorizeAsync(User, todo, Operations.Delete);
            if (!isAuthorized.Succeeded)
            {
                return new ChallengeResult();
            }

            return View(todo);
        }

        [HttpPost("delete/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var todo = _todoRepo.Get(id);

            var isAuthorized = await _authorizationService.AuthorizeAsync(User, todo, Operations.Delete);

            if (!isAuthorized.Succeeded)
            {
                return new ChallengeResult();
            }

            _todoRepo.Remove(todo);
            _todoRepo.Save();
            return RedirectToAction(nameof(List));
        }
    }
}