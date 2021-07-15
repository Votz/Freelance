using AutoMapper;
using Freelance.Api.Models.Request;
using Freelance.Services.Interfaces;
using Freelance.Services.Models.Request;
using Freelance.Services.Models.Response;
using Freelance.Shared.Enumerations;
using Freelance.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Freelance.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobOfferController : ControllerBase
    {
        private readonly IJobOfferService _jobOfferService;
        private readonly IMapper _mapper;
        public JobOfferController(IJobOfferService jobOfferService, IMapper mapper)
        {
            _jobOfferService = jobOfferService;
            _mapper = mapper;
        }

        [HttpGet("[action]")]
        public async Task<ApiResponse<PaginationResponseModel<JobOfferViewModel>>> GetAll()
        {
            //var mappedResult = _mapper.Map<JobOfferModel>(model);
            return await _jobOfferService.GetAll(new JobOfferModel());
        }

        [HttpPost("[action]")]
        public async Task<ApiResponse<int>> Create([FromBody] CreateJobOfferRequestModel model)
        {
            var mappedResult = _mapper.Map<JobOfferModel>(model);
            return await _jobOfferService.Create(mappedResult);
        }

        [HttpPost]
        public async Task<ApiResponse<int>> Update(JobOfferFilterModel model)
        {
            var mappedResult = _mapper.Map<JobOfferModel>(model);
            return await _jobOfferService.Update(mappedResult);
        }

        [HttpGet]
        public async Task<ApiResponse<JobOfferViewModel>> Get(int id)
        {
            return await _jobOfferService.Get(id);
        }

        [HttpGet("[action]")]
        public async Task<ApiResponse> Delete(int id)
        {
            return await _jobOfferService.Delete(id);
        }

        [HttpGet("[action]")]
        public async Task<ApiResponse> ChangeJobStatus(ChangeJobStatusRequestModel model)
        {
            var mappedResult = _mapper.Map<ChangeJobOfferStatus>(model);
            return await _jobOfferService.ChangeJobStatus(mappedResult);
        }
    }
}
