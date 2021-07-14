using Freelance.Services.Models.Request;
using Freelance.Services.Models.Response;
using Freelance.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Freelance.Services.Interfaces
{
    public interface IEmployerService
    {
        Task<ApiResponse<PaginationResponseModel<EmployerProfileViewModel>>> GetAll(EmployerProfileModel model);
        Task<ApiResponse<EmployerProfileViewModel>> Get(int id);
        Task<ApiResponse<int>> Create(EmployerProfileModel model);
        Task<ApiResponse<int>> Update(EmployerProfileModel model);
        Task<ApiResponse> Delete(int id);
    }
}
