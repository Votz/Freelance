using Freelance.Services.Models.Request;
using Freelance.Services.Models.Response;
using Freelance.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Freelance.Services.Interfaces
{
    public interface IRoleService
    {
        Task<ApiResponse<List<RoleViewModel>>> GetAll(RoleModel model);
        Task<ApiResponse<RoleViewModel>> Get(string id);
        Task<ApiResponse<string>> Create(RoleModel model);
        Task<ApiResponse<string>> Update(RoleModel model);
        Task<ApiResponse> Delete(string id);
    }
}
