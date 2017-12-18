using CourseProject.Models;
using System.Collections.Generic;

namespace CourseProject.Services
{
    public interface IEventRepository : IRepository<Event>
    {
        IEnumerable<Event> GetAll(string ownerID);
    }
}