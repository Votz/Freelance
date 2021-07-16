using Freelance.Services.Models.Request;
using Freelance.Services.Models.Response;
using Freelance.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Freelance.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<ApiResponse<List<CategoryViewModel>>> GetAll();
        Task<ApiResponse<CategoryViewModel>> Get(int id);
        Task<ApiResponse<int>> Create(CategoryModel model);
        Task<ApiResponse<int>> Update(CategoryModel model);
        Task<ApiResponse> Delete(int id);
    }
}
