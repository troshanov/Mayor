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

            var picture = new Picture
            {
                AddedByUserId = userId,
                Extension = imgExtension,
            };

            Directory.CreateDirectory($"{rootPath}/img/");
            var physicalPath = $"{rootPath}/img/{picture.Id}{picture.Extension}";
            using Stream fileStream = new FileStream(physicalPath, FileMode.Create);
            await picFile.CopyToAsync(fileStream);
            return picture;
        }

        public async Task<bool> DeleteOldProfilePicAsync(string userId)
        {
            var picture = this.picRepo.All()
                .FirstOrDefault(p => p.AddedByUserId == userId && p.IssueId == null);
            if (picture == null)
            {
                return false;
            }

            this.picRepo.Delete(picture);
            await this.picRepo.SaveChangesAsync();
            return true;
        }

        public Picture GetProfilePicByUserId(string userId)
        {
            var picture = this.picRepo.All()
                .FirstOrDefault(p => p.AddedByUserId == userId && p.IssueId == null);

            if (picture == null)
            {
                return new Picture
                {
                    Id = "Anon",
                    Extension = ".png",
                };
            }

            return picture;
        }
    }
}
