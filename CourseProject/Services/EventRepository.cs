using CourseProject.Data;
using CourseProject.Models;

namespace CourseProject.Services
{
    public class EventRepository : Repository<Event>, IEventRepository
    {
        public EventRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}