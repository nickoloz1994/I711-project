using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using CourseProject.Models;
using CourseProject.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using CourseProject.Authorization;

namespace CourseProject.Controllers
{
    [Route("events")]
    public class EventController : Controller
    {
        private IEventRepository _evtRepo;
        private UserManager<ApplicationUser> _userManager;
        private IAuthorizationService _authorizationService;

        public EventController(IEventRepository evtRepo,
                               UserManager<ApplicationUser> userManager,
                               IAuthorizationService authorizationService)
        {
            _evtRepo = evtRepo;
            _userManager = userManager;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        public IActionResult List()
        {
            var userID = _userManager.GetUserId(User);
            IEnumerable<Event> events = _evtRepo.GetAll(userID);

            return View(events);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(
            [Bind("Name,Description,Location,StartDate,StartTime,EndDate,EndTime")] Event evt)
        {
            if (!ModelState.IsValid)
            {
                return View(evt);
            }

            var newEvent = new Event
            {
                Name = evt.Name,
                Location = evt.Location,
                Description = evt.Description,
                StartDate = evt.StartDate,
                StartTime = evt.StartTime,
                EndDate = evt.EndDate,
                EndTime = evt.EndTime,
                OwnerID = _userManager.GetUserId(User)
            };

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                                                                User, newEvent,
                                                                Operations.Create);

            if (!isAuthorized.Succeeded)
            {
                return new ChallengeResult();
            }

            _evtRepo.Add(newEvent);
            _evtRepo.Save();
            return RedirectToAction(nameof(List));
        }

        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evt = _evtRepo.Get((int)id);
            if (evt == null)
            {
                return NotFound();
            }

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                                                                User, evt,
                                                                Operations.Update);

            if (!isAuthorized.Succeeded)
            {
                return new ChallengeResult();
            }

            return View(evt);
        }

        [HttpPost("edit/{id}")]
        public async Task<IActionResult> Edit(int id,
            [Bind("Name,Description,Location,StartDate,StartTime,EndDate,EndTime")] Event evt)
        {
            if (!ModelState.IsValid)
            {
                return View(evt);
            }

            var evtToEdit = _evtRepo.Get(id);

            if (evtToEdit == null)
            {
                return NotFound();
            }

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                                                                User, evtToEdit,
                                                                Operations.Update);

            if (!isAuthorized.Succeeded)
            {
                return new ChallengeResult();
            }

            evtToEdit.Name = evt.Name;
            evtToEdit.Description = evt.Description;
            evtToEdit.Location = evt.Location;
            evtToEdit.StartDate = evt.StartDate;
            evtToEdit.StartTime = evt.StartTime;
            evtToEdit.EndDate = evt.EndDate;
            evtToEdit.EndTime = evt.EndTime;

            TryValidateModel(evtToEdit);

            if (ModelState.IsValid)
            {
                _evtRepo.Update(evtToEdit);
                _evtRepo.Save();
                return RedirectToAction(nameof(List));
            }

            return View(evt);
        }

        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evtToDelete = _evtRepo.Get((int)id);

            if (evtToDelete == null)
            {
                return NotFound();
            }

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                                                                User, evtToDelete,
                                                                Operations.Delete);

            if (!isAuthorized.Succeeded)
            {
                return new ChallengeResult();
            }

            return View(evtToDelete);
        }

        [HttpPost("delete/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var evtToDelete = _evtRepo.Get(id);

            var isAuthorized = await _authorizationService.AuthorizeAsync(
                                                                User, evtToDelete,
                                                                Operations.Delete);

            if (!isAuthorized.Succeeded)
            {
                return new ChallengeResult();
            }

            _evtRepo.Remove(evtToDelete);
            _evtRepo.Save();
            return RedirectToAction(nameof(List));
        }
    }
}