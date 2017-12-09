using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using CourseProject.Models;
using CourseProject.Services;

namespace CourseProject.Controllers
{
    [Route("events")]
    public class EventController : Controller
    {
        private IEventRepository _evtRepo;

        public EventController(IEventRepository evtRepo)
        {
            _evtRepo = evtRepo;
        }

        [HttpGet]
        public IActionResult List()
        {
            IEnumerable<Event> events = events = _evtRepo.GetAll();

            return View(events);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        public IActionResult Create(
            [Bind("Name,Description,Location,StartDate,StartTime,EndDate,EndTime")] Event evt)
        {
            if (evt == null)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _evtRepo.Add(evt);
                _evtRepo.Save();
                return RedirectToAction(nameof(List));
            }

            return View(evt);
        }

        [HttpGet("edit/{id}")]
        public IActionResult Edit(int id)
        {
            var evt = _evtRepo.Get(id);

            if (evt == null)
            {
                return NotFound();
            }

            return View(evt);
        }

        [HttpPost("edit/{id}")]
        public IActionResult Edit(int id,
            [Bind("Name,Description,Location,StartDate,StartTime,EndDate,EndTime")] Event evt)
        {
            var evtToEdit = _evtRepo.Get(id);

            if (evtToEdit == null)
            {
                return NotFound();
            }

            evtToEdit.Name = evt.Name;
            evtToEdit.Description = evt.Description;
            evtToEdit.Location = evt.Location;
            evtToEdit.StartDate = evt.StartDate;
            evtToEdit.StartTime = evt.StartTime;
            evtToEdit.EndDate = evt.EndDate;
            evtToEdit.EndTime = evt.EndTime;

            if (ModelState.IsValid)
            {
                _evtRepo.Update(evtToEdit);
                _evtRepo.Save();
                return RedirectToAction(nameof(List));
            }

            return View(evt);
        }

        [HttpGet("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var evtToDelete = _evtRepo.Get(id);

            if (evtToDelete == null)
            {
                return NotFound();
            }

            return View(evtToDelete);
        }

        [HttpPost("delete/{id}")]
        public IActionResult DeleteConfirmed(int id)
        {
            var evtToDelete = _evtRepo.Get(id);

            if (evtToDelete == null)
            {
                return BadRequest();
            }

            _evtRepo.Remove(evtToDelete);
            _evtRepo.Save();
            return RedirectToAction(nameof(List));
        }
    }
}