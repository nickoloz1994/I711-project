using System.Collections.Generic;
using CourseProject.Data;
using CourseProject.Models;
using System.Linq;
using System;

namespace CourseProject.Services
{
    public class EventRepository : Repository<Event>, IEventRepository
    {
        public EventRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        public IEnumerable<Event> GetAll(string ownerID)
        {
            return _context.Events
                .Where(e => e.OwnerID == ownerID).ToList();
        }

        public IEnumerable<Event> GetPrevious(string ownerID)
        {
            return _context.Events
                .Where(e => e.OwnerID == ownerID &&
                       DateTime.Compare(e.EndDate, DateTime.Now) < 0)
                .ToList();
        }

        public IEnumerable<Event> GetUpcoming(string ownerID)
        {
            return _context.Events
                .Where(e => e.OwnerID == ownerID &&
                       DateTime.Compare(e.EndDate.Date, DateTime.Now) > 0)
                .ToList();
        }
    }
}