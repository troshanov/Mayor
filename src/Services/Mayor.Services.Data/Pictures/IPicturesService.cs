using Mayor.Data.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Mayor.Services.Data.Pictures
{
    public interface IPicturesService
    {
        Task<Picture> CreateFileAsync(string userId, string rootPath, IFormFile picFile);

        Picture GetProfilePicByUserId(string userId);

        Task<bool> DeleteOldProfilePicAsync(string userId);
    }
}
