using Freelance.Services.Models.Request;
using Freelance.Services.Models.Response;
using Freelance.Shared.Enumerations;
using Freelance.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Freelance.Services.Interfaces
{
    public interface IJobOfferService
    {
        Task<ApiResponse<List<JobOfferViewModel>>> GetAll();
        Task<ApiResponse<JobOfferViewModel>> Get(int id);
        Task<ApiResponse<int>> Create(JobOfferModel model);
        Task<ApiResponse<int>> Update(JobOfferModel model);
        Task<ApiResponse> Delete(int id);
        Task<ApiResponse> ChangeJobStatus(ChangeJobOfferStatus model);
    }
}
