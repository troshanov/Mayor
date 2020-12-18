namespace Mayor.Web.ViewModels.Issue
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using Mayor.Services.Mapping;
    using Mayor.Web.ViewModels.Comment;

    public class SingleIssueViewModel : PagingViewModel, IMapFrom<Mayor.Data.Models.Issue>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string CreatorName { get; set; }

        public string CategoryName { get; set; }

        public string CreatorUserId { get; set; }

        public string StatusStatusCode { get; set; }

        public CommentInputModel CommentInput { get; set; }

        public DateTime CreatedOn { get; set; }

        public ICollection<string> IssueTags { get; set; }

        public string Address { get; set; }

        public IList<IssueInSidebarViewModel> SidebarIssues { get; set; }

        public string AddressCityName { get; set; }

        public int VotesCount { get; set; }

        public IList<string> Attachments { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Mayor.Data.Models.Issue, SingleIssueViewModel>()
                .ForMember(vm => vm.ImageUrl, opt =>
                    opt.MapFrom(i =>
                        "/img/" + i.Pictures.FirstOrDefault().Id + i.Pictures.FirstOrDefault().Extension))
                .ForMember(vm => vm.Address, opt =>
                    opt.MapFrom(a => a.Address.Street + " " + a.Address.StreetNumber))
                .ForMember(vm => vm.CreatorName, opt =>
                    opt.MapFrom(i => i.Creator.FirstName + " " + i.Creator.LastName))
                .ForMember(vm => vm.IssueTags, opt =>
                    opt.MapFrom(i => i.IssueTags.Select(it => "#" + it.Tag.Value).ToList()))
                .ForMember(vm => vm.CategoryName, opt =>
                    opt.MapFrom(i => i.Category.Name))
                .ForMember(vm => vm.Attachments, opt =>
                    opt.MapFrom(i => i.IssueAttachments.Select(a => a.Attachment.Id).ToList()))
                .ForMember(vm => vm.CommentInput, opt =>
                    opt.Ignore())
                .ForMember(vm => vm.HasNextPage, opt =>
                opt.Ignore())
                .ForMember(vm => vm.HasPreviousPage, opt =>
                opt.Ignore())
                .ForMember(vm => vm.IssuesCount, opt =>
                opt.Ignore())
                .ForMember(vm => vm.ItemsPerPage, opt =>
                opt.Ignore())
                .ForMember(vm => vm.NextPageNumber, opt =>
                opt.Ignore())
                .ForMember(vm => vm.PreviousPageNumber, opt =>
                opt.Ignore())
                .ForMember(vm => vm.PagesCount, opt =>
                opt.Ignore())
                .ForMember(vm => vm.PageNumber, opt =>
                opt.Ignore());
        }
    }
}
