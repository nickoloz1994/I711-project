using System.Collections.Generic;
using CourseProject.Data;
using CourseProject.Models;
using System.Linq;

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
            return _context.Events.Where(e => e.OwnerID == ownerID).ToList();
        }
    }
}