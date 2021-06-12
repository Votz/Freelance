using Freelance.Services.Models.Request;
using Freelance.Services.Models.Response;
using Freelance.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Freelance.Services.Interfaces
{
    public interface IAuthorizationService
    {
        public ApiResponse<LoginResponseModel> Login(LoginRequestModel model);
    }
}
