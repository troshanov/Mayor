namespace Mayor.Services.Data.Categories
{
    using System.Collections.Generic;
    using System.Linq;

    using Mayor.Data.Common.Repositories;
    using Mayor.Data.Models;

    public class CategoriesService : ICategoriesService
    {
        private readonly IDeletableEntityRepository<Category> categoriesRepo;

        public CategoriesService(IDeletableEntityRepository<Category> categoriesRepo)
        {
            this.categoriesRepo = categoriesRepo;
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs()
        {
           return this.categoriesRepo.All()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                }).ToList()
                .Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Name.ToString()));
        }
    }
}
