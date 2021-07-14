using Freelance.Services.Models.Request;
using Freelance.Services.Models.Response;
using Freelance.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Freelance.Services.Interfaces
{
    public interface IUserProfileService
    {
        Task<ApiResponse<PaginationResponseModel<UserProfileViewModel>>> GetAll(UserProfileModel model);
        Task<ApiResponse<UserProfileViewModel>> Get(int id);
        Task<ApiResponse<int>> Create(UserProfileModel model);
        Task<ApiResponse<int>> Update(UserProfileModel model);
        Task<ApiResponse> Delete(int id);
    }
}
