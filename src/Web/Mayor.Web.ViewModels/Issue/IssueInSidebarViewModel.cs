namespace Mayor.Web.ViewModels.Issue
{
    using System.Linq;

    using AutoMapper;
    using Mayor.Services.Mapping;

    public class IssueInSidebarViewModel : IMapFrom<Mayor.Data.Models.Issue>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int VotesCount { get; set; }

        public string ImageUrl { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Mayor.Data.Models.Issue, IssueInSidebarViewModel>()
               .ForMember(vm => vm.ImageUrl, opt =>
                   opt.MapFrom(i =>
                       "/img/" + i.Pictures.FirstOrDefault().Id + i.Pictures.FirstOrDefault().Extension))
               .ForMember(vm => vm.Description, opt =>
               opt.MapFrom(i => string.Join(string.Empty, i.Description.Take(20).ToList()) + "..."));
        }
    }
}
