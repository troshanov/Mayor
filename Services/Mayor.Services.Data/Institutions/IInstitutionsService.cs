﻿using Mayor.Data.Models;
using Mayor.Web.ViewModels.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mayor.Services.Data.Institutions
{
    public interface IInstitutionsService
    {
        Task CreateAsync(AppUserInputModel input, string userId);

        Institution GetByUserId(string userId);

        IEnumerable<T> GetTopTenByRating<T>();
    }
}
