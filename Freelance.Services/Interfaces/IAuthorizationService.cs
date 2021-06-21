using Freelance.Services.Models.Request;
using Freelance.Services.Models.Response;
using Freelance.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Freelance.Services.Interfaces
{
    public interface IAuthorizationService
    {
        Task<ApiResponse<LoginResponseModel>> LogIn(LoginRequestModel model);
        Task<ApiResponse<bool>> Logout();
        Task<ApiResponse<CheckAuthorityResponseModel>> CheckAuthority();
        string GetUserId();
        string GetUserToken();
        GetUserTokenValidityModel GetUserTokenValidity();
    }
}
