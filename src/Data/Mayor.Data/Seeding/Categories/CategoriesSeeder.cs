namespace Mayor.Data.Seeding.Categories
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Mayor.Data.Models;

    public class CategoriesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Categories.Count() > 1)
            {
                return;
            }

            await dbContext.Categories.AddAsync(new Category { Name = "Traffic & Parking" });
            await dbContext.Categories.AddAsync(new Category { Name = "Roads" });
            await dbContext.Categories.AddAsync(new Category { Name = "Urban Facilities" });
            await dbContext.Categories.AddAsync(new Category { Name = "Waste" });
            await dbContext.Categories.AddAsync(new Category { Name = "Green Spaces" });
            await dbContext.Categories.AddAsync(new Category { Name = "Playgrounds" });
            await dbContext.Categories.AddAsync(new Category { Name = "Construction" });
            await dbContext.Categories.AddAsync(new Category { Name = "Law & Order" });
            await dbContext.Categories.AddAsync(new Category { Name = "Charity" });

            await dbContext.SaveChangesAsync();
        }
    }
}
