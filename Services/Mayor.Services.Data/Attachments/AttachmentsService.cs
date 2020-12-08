using Mayor.Data.Common.Repositories;
using Mayor.Data.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Mayor.Services.Data.Attachments
{
    public class AttachmentsService : IAttachmentsService
    {
        private readonly string[] allowedExtensions = new[] { "docx", "pdf", "jpg", "png" };
        private readonly IDeletableEntityRepository<Attachment> attRepo;

        public AttachmentsService(
            IDeletableEntityRepository<Attachment> attRepo)
        {
            this.attRepo = attRepo;
        }

        public async Task<Attachment> CreateAsync(string userId, string rootPath, IFormFile file)
        {
            var attExtension = Path.GetExtension(file.FileName);
            if (!this.allowedExtensions.Any(x => attExtension.EndsWith(x)))
            {
                throw new Exception($"Invalid attachment format!");
            }

            var attachment = new Attachment
            {
                AddedByUserId = userId,
                Extension = attExtension,
            };

            Directory.CreateDirectory($"{rootPath}/att/requests");
            var physicalPath = $"{rootPath}/att/requests/{attachment.Id}{attachment.Extension}";
            using Stream fileStream = new FileStream(physicalPath, FileMode.Create);
            await file.CopyToAsync(fileStream);
            return attachment;
        }

        public Attachment GetFileById(string id)
        {
            return this.attRepo.AllAsNoTracking()
                .FirstOrDefault(f => f.Id == id);
        }
    }
}
