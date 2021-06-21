using Freelance.Services.Models.Request;
using Freelance.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Freelance.Services.Interfaces
{
    public interface IPasswordResetService
    {
        Task<ApiResponse<bool>> ResetPassword(ResetPasswordModel model);
    }
}
