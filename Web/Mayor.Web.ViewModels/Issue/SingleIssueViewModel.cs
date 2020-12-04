﻿namespace Mayor.Web.ViewModels.Issue
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using Mayor.Services.Mapping;

    public class SingleIssueViewModel : IMapFrom<Mayor.Data.Models.Issue>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string CreatorName { get; set; }

        public string CategoryName { get; set; }

        public DateTime CreatedOn { get; set; }

        public ICollection<string> IssueTags { get; set; }

        public string Address { get; set; }

        public IList<IssueInSidebarViewModel> SidebarIssues { get; set; }

        public string AddressCityName { get; set; }

        public int VotesCount { get; set; }

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
                opt.MapFrom(i => i.Category.Name));
        }
    }
}
