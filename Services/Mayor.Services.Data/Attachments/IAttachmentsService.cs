using Mayor.Data.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Mayor.Services.Data.Attachments
{
    public interface IAttachmentsService
    {
        Task<Attachment> CreateAsync(string userId, string rootPath, IFormFile file);

        Attachment GetFileById(string id);
    }
}
