namespace Mayor.Web.ViewModels.Issue
{
    using System.Linq;

    using AutoMapper;
    using Mayor.Services.Mapping;

    public class TopIssueViewModel : IMapFrom<Mayor.Data.Models.Issue>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string CreatorName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Mayor.Data.Models.Issue, TopIssueViewModel>()
                .ForMember(vm => vm.ImageUrl, opt =>
                    opt.MapFrom(i =>
                        "/img/" + i.Pictures.FirstOrDefault(p => p.IssueId != null).Id + i.Pictures.FirstOrDefault(p => p.IssueId != null).Extension))
                .ForMember(vm => vm.CreatorName, opt =>
                    opt.MapFrom(i => i.Creator.FirstName + " " + i.Creator.LastName));
        }
    }
}
