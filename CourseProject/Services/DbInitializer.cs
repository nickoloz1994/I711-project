using System;
using System.Linq;
using CourseProject.Models;
using CourseProject.Data;

namespace CourseProject.Services
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Events.Any())
            {
                return;
            }

            var categories = new TodoCategory[]
            {
                new TodoCategory{Name="All"},
                new TodoCategory{Name="Home"},
                new TodoCategory{Name="Work"},
                new TodoCategory{Name="Other"},
                new TodoCategory{Name="Studies"}
            };

            foreach (TodoCategory category in categories)
            {
                context.TodoCategories.Add(category);
            }
            context.SaveChanges();

            var todoItems = new TodoItem[]
            {
                new TodoItem{Title="Buy bananas",Complete=false,Date=new DateTime(2017,12,10),CategoryId=2},
                new TodoItem{Title="Buy some food",Complete=false,Date=new DateTime(2017,12,10),CategoryId=2},
                new TodoItem{Title="Do Homework",Complete=true,Date=new DateTime(2017,12,10),CategoryId=5},
                new TodoItem{Title="Do Extra Task",Complete=false,Date=new DateTime(2017,12,10),CategoryId=5},
                new TodoItem{Title="Go to the gym",Complete=false,Date=new DateTime(2017,12,10),CategoryId=4}
            };

            foreach (TodoItem item in todoItems)
            {
                context.TodoItems.Add(item);
            }
            context.SaveChanges();

            var budgets = new Budget[]
            {
                new Budget{Name="January budget",Amount=300,StartDate=new DateTime(2018,1,1),EndDate=new DateTime(2018,1,31)},
                new Budget{Name="February budget",Amount=300,StartDate=new DateTime(2018,2,1),EndDate=new DateTime(2018,2,28)},
            };

            foreach (Budget budget in budgets)
            {
                context.Budgets.Add(budget);
            }
            context.SaveChanges();

            var events = new Event[]
            {
                new Event{Name="Play PS4 with friends",Description="Gaming party",Location="Home",StartDate=new DateTime(2017,11,10),StartTime="20:00:00",EndDate=new DateTime(2017,11,11),EndTime="06:00:00"},
                new Event{Name="Drink beer with friends",Description="Beer party",Location="Bar",StartDate=new DateTime(2017,11,15),StartTime="20:00:00",EndDate=new DateTime(2017,11,16),EndTime="03:00:00"},
            };

            foreach (Event evt in events)
            {
                context.Events.Add(evt);
            }
            context.SaveChanges();
        }
    }
}