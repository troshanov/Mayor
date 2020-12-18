namespace Mayor.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Mayor.Data.Common.Repositories;
    using Mayor.Data.Models;
    using Mayor.Services.Data.Categories;
    using Moq;
    using Xunit;

    public class CategoriesServiceTests
    {
        private List<Category> categoriesList;
        private Mock<IDeletableEntityRepository<Category>> categoriesRepo;
        private CategoriesService categoriesService;

        public CategoriesServiceTests()
        {
            this.categoriesList = new List<Category>();
            this.categoriesRepo = new Mock<IDeletableEntityRepository<Category>>();
            this.categoriesRepo.Setup(x => x.All()).Returns(this.categoriesList.AsQueryable());
            this.categoriesRepo.Setup(x => x.AddAsync(It.IsAny<Category>())).Callback((Category address) => this.categoriesList.Add(address));

            this.categoriesService = new CategoriesService(this.categoriesRepo.Object);
        }

        [Fact]
        public void GetAllAsKeyValuePairsShouldReturnAllCategoriesIdAndNameAsKeyValuePairs()
        {
            this.categoriesList.Add(new Category { Id = 1, Name = "First" });
            this.categoriesList.Add(new Category { Id = 2, Name = "Second" });
            this.categoriesList.Add(new Category { Id = 3, Name = "Tird" });

            var kvps = this.categoriesService.GetAllAsKeyValuePairs();

            Assert.True(kvps.Count() == 3);
        }

        [Fact]
        public void GetAllAsKeyValuePairsShouldReturnCorrectKvpValues()
        {
            this.categoriesList.Add(new Category { Id = 1, Name = "First" });
            this.categoriesList.Add(new Category { Id = 2, Name = "Second" });
            this.categoriesList.Add(new Category { Id = 3, Name = "Third" });

            var kvps = this.categoriesService.GetAllAsKeyValuePairs().ToArray();
            var firstPair = kvps[0];
            var secondPair = kvps[1];
            var thirdPair = kvps[2];

            Assert.True(int.Parse(firstPair.Key) == 1 && firstPair.Value == "First");
            Assert.True(int.Parse(secondPair.Key) == 2 && secondPair.Value == "Second");
            Assert.True(int.Parse(thirdPair.Key) == 3 && thirdPair.Value == "Third");
        }
    }
}
