namespace Mayor.Web.ViewModels.Institution
{
    using System.Linq;

    using AutoMapper;
    using Mayor.Services.Mapping;

    public class InstitutionViewModel : IMapFrom<Mayor.Data.Models.Institution>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Rating { get; set; }

        public string ImageUrl { get; set; }

        public string WebsiteUrl { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Mayor.Data.Models.Institution, InstitutionViewModel>()
                .ForMember(vm => vm.ImageUrl, opt =>
                opt.MapFrom(i => "/img/" +
                i.User.Pictures.FirstOrDefault(p => p.IssueId == null).Id +
                i.User.Pictures.FirstOrDefault(p => p.IssueId == null).Extension));
        }
    }
}
