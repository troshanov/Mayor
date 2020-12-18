using Mayor.Data.Models;
using Mayor.Services.Mapping;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Mayor.Web.ViewModels.Request
{
    public class RequestInListViewModel : IMapFrom<IssueRequest>
    {
        public int Id { get; set; }

        public int IssueId { get; set; }

        public bool IsSolveRequest { get; set; }

        public DateTime CreatedOn { get; set; }

        public int RequesterId { get; set; }

        public string RequesterName { get; set; }

        public string IssueTitle { get; set; }
    }
}
