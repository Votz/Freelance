using Freelance.Services.Models.Request;
using Freelance.Services.Models.Response;
using Freelance.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Freelance.Services.Interfaces
{
    public interface IUserCategoryService
    {
        Task<ApiResponse<bool>> AddOrUpdate(UserCategoryModel model);
        Task<ApiResponse<List<UserCategoryViewModel>>> GetAll(UserCategoryFilterModel model);
    }
}
