namespace Mayor.Data.Seeding.Statuses
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Mayor.Data.Models;

    public class StatusesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Statuses.Any())
            {
                return;
            }

            dbContext.Statuses.Add(new Status { StatusCode = "Solved" });
            dbContext.Statuses.Add(new Status { StatusCode = "In Process" });
            dbContext.Statuses.Add(new Status { StatusCode = "New" });
        }
    }
}
