using AutoMapper;
using Freelance.Api.Models.Request;
using Freelance.Domain.Entities;
using Freelance.Services.Models.Request;
using Freelance.Services.Models.Response;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Freelance.Api.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<LoginRequest, LoginRequestModel>();
            CreateMap<LoginRequest, LoginRequestModel>().ReverseMap();

            CreateMap<CreateUserRequest, CreateuserModel>();
            CreateMap<CreateUserRequest, CreateuserModel>().ReverseMap();

            CreateMap<CreateRoleRequest, RoleModel>();
            CreateMap<CreateRoleRequest, RoleModel>().ReverseMap();

            CreateMap<AddUserInRoleRequest, AddUserInRoleModel>();
            CreateMap<AddUserInRoleRequest, AddUserInRoleModel>().ReverseMap();

            CreateMap<IdentityRole, RoleViewModel>();
            CreateMap<IdentityRole, RoleViewModel>().ReverseMap();

            CreateMap<List<IdentityRole>, List<RoleViewModel>>();
            CreateMap<List<IdentityRole>, List<RoleViewModel>>().ReverseMap();

            CreateMap<JobOffer, JobOfferViewModel>();
            CreateMap<JobOffer, JobOfferViewModel>().ReverseMap();

            CreateMap<JobOfferViewModel, JobOfferModel>();
            CreateMap<JobOfferViewModel, JobOfferModel>().ReverseMap();

            CreateMap<JobOfferFilterModel, JobOfferModel>();
            CreateMap<JobOfferFilterModel, JobOfferModel>().ReverseMap();

            CreateMap<JobOffer, JobOfferModel>();
            CreateMap<JobOffer, JobOfferModel>().ReverseMap();

            CreateMap<CategoryFilterRequestModel, CategoryModel>();
            CreateMap<CategoryFilterRequestModel, CategoryModel>().ReverseMap();

            CreateMap<CategoryFilterRequestModel, CategoryModel>();
            CreateMap<CategoryFilterRequestModel, CategoryModel>().ReverseMap();

            CreateMap<CreateCategoryRequestModel, CategoryModel>();
            CreateMap<CreateCategoryRequestModel, CategoryModel>().ReverseMap();

            CreateMap<CategoryModel, Category>();
            CreateMap<CategoryModel, Category>().ReverseMap();


            CreateMap<ChangeJobOfferStatus, ChangeJobStatusRequestModel>();
            CreateMap<ChangeJobOfferStatus, ChangeJobStatusRequestModel>().ReverseMap();

            CreateMap<BidFilterModel, BidModel>();
            CreateMap<BidFilterModel, BidModel>().ReverseMap();

            CreateMap<CreateBidRequest, BidModel>();
            CreateMap<CreateBidRequest, BidModel>().ReverseMap();
        }

    }
}
