namespace Mayor.Services.Data.Pictures
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Mayor.Data.Common.Repositories;
    using Mayor.Data.Models;
    using Microsoft.AspNetCore.Http;

    public class PicturesService : IPicturesService
    {
        private readonly string[] allowedImageExtensions = new[] { "jpg", "png" };
        private readonly IDeletableEntityRepository<Picture> picRepo;

        public PicturesService(IDeletableEntityRepository<Picture> picRepo)
        {
            this.picRepo = picRepo;
        }

        public async Task<Picture> CreateFileAsync(string userId, string rootPath, IFormFile picFile)
        {
            var imgExtension = Path.GetExtension(picFile.FileName);
            if (!this.allowedImageExtensions.Any(x => imgExtension.EndsWith(x)))
            {
                throw new Exception($"Format should be .jpg or .png!");
            }

            // TODO: Refacture hardcoded variable userId
            userId = "a8be01d1-f291-4fa3-a697-2dbc30dbc8a6";
            var picture = new Picture
            {
                AddedByUserId = userId,
                Extension = imgExtension,
            };

            Directory.CreateDirectory($"{rootPath}/img/issues/");
            var physicalPath = $"{rootPath}/img/issues/{picture.Id}{picture.Extension}";
            using Stream fileStream = new FileStream(physicalPath, FileMode.Create);
            await picFile.CopyToAsync(fileStream);
            return picture;
        }
    }
}
