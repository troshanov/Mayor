namespace Mayor.Web.ViewModels.Request
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using Mayor.Data.Models;
    using Mayor.Services.Mapping;
    using Microsoft.AspNetCore.Http;

    public class SingleRequestViewModel : IMapTo<IssueRequest>, IHaveCustomMappings
    {
        public int IssueId { get; set; }

        public string Description { get; set; }

        public bool IsSolveRequest { get; set; }

        public bool? IsApproved { get; set; }

        public DateTime CreatedOn { get; set; }

        public int RequesterId { get; set; }

        public string RequesterName { get; set; }

        public string RequesterPicture { get; set; }

        public decimal RequesterRating { get; set; }

        public string IssueTitle { get; set; }

        public IList<string> Attachments { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<IssueRequest, SingleRequestViewModel>()
                .ForMember(vm => vm.RequesterPicture, opt =>
                opt.MapFrom(r => "/img/" + r.Requester.User.Pictures.FirstOrDefault(p => p.IssueId == null).Id
                + r.Requester.User.Pictures.FirstOrDefault(p => p.IssueId == null).Extension))
                .ForMember(vm => vm.Attachments, opt =>
                opt.MapFrom(r => r.IssueRequestAttachments.Select(x => x.Attachment.Id).ToList()));
        }
    }
}
