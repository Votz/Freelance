using AutoMapper;
using Freelance.Api.Models.Request;
using Freelance.Services.Interfaces;
using Freelance.Services.Models.Request;
using Freelance.Services.Models.Response;
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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }


        [HttpGet("[action]")]
        public async Task<ApiResponse<PaginationResponseModel<CategoryViewModel>>> GetAll([FromQuery] CategoryFilterRequestModel model)
        {
            var mappedResult = _mapper.Map<CategoryModel>(model);
            return await _categoryService.GetAll(mappedResult);
        }

        [HttpPost("[action]")]
        public async Task<ApiResponse<int>> Create([FromBody] CreateCategoryRequestModel model)
        {
            var mappedResult = _mapper.Map<CategoryModel>(model);
            return await _categoryService.Create(mappedResult);
        }

        [HttpPost]
        public async Task<ApiResponse<int>> Update(CategoryFilterRequestModel model)
        {
            var mappedResult = _mapper.Map<CategoryModel>(model);
            return await _categoryService.Update(mappedResult);
        }

        [HttpGet]
        public async Task<ApiResponse<CategoryViewModel>> Get(int id)
        {
            return await _categoryService.Get(id);
        }

        [HttpGet("[action]")]
        public async Task<ApiResponse> Delete(int id)
        {
            return await _categoryService.Delete(id);
        }

    }
}
