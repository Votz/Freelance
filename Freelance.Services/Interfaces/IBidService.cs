using Freelance.Services.Models.Request;
using Freelance.Services.Models.Response;
using Freelance.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Freelance.Services.Interfaces
{
    public interface IBidService
    {
        Task<ApiResponse<PaginationResponseModel<BidViewModel>>> GetAll(BidModel model);
        Task<ApiResponse<BidViewModel>> Get(int id);
        Task<ApiResponse<int>> Create(BidModel model);
        Task<ApiResponse<int>> Update(BidModel model);
        Task<ApiResponse> Delete(int id);
    }
}
