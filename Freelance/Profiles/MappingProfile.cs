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
            CreateMap<LoginRequestModel, LoginRequest>();

            CreateMap<CreateUserRequest, CreateuserModel>();
            CreateMap<CreateuserModel, CreateUserRequest>().ForAllOtherMembers(opts => opts.Ignore()); ;

            CreateMap<CreateRoleRequest, RoleModel>().ForAllOtherMembers(opts => opts.Ignore());
            CreateMap<RoleModel, CreateRoleRequest>().ForAllOtherMembers(opts => opts.Ignore()); ;

            CreateMap<AddUserInRoleRequest, AddUserInRoleModel>();
            CreateMap<AddUserInRoleModel, AddUserInRoleRequest>();
            
            CreateMap<RoleFilterModel, RoleModel>();
            CreateMap<RoleModel, RoleFilterModel>();

            CreateMap<RoleViewModel, IdentityRole>().ForAllOtherMembers(opts => opts.Ignore());
            CreateMap<IdentityRole, RoleViewModel>().ForAllOtherMembers(opts => opts.Ignore());

            //CreateMap<List<IdentityRole>, List<RoleViewModel>>().ForAllOtherMembers(opts => opts.Ignore());
            //CreateMap<List<RoleViewModel>, List<IdentityRole>>().ForAllOtherMembers(opts => opts.Ignore());

            CreateMap<JobOffer, JobOfferViewModel>().ForAllOtherMembers(opts => opts.Ignore());
            CreateMap<JobOfferViewModel, JobOffer>().ForAllOtherMembers(opts => opts.Ignore());

            CreateMap<JobOfferViewModel, JobOfferModel>().ForAllOtherMembers(opts => opts.Ignore());
            CreateMap<JobOfferModel, JobOfferViewModel>().ForAllOtherMembers(opts => opts.Ignore());

            CreateMap<JobOfferFilterModel, JobOfferModel>().ForAllOtherMembers(opts => opts.Ignore());
            CreateMap<JobOfferModel, JobOfferFilterModel>().ForAllOtherMembers(opts => opts.Ignore());

            CreateMap<JobOffer, JobOfferModel>().ForAllOtherMembers(opts => opts.Ignore());
            CreateMap<JobOfferModel, JobOffer>().ForAllOtherMembers(opts => opts.Ignore());

            CreateMap<CategoryFilterRequestModel, CategoryModel>();
            CreateMap<CategoryModel, CategoryFilterRequestModel>();

            CreateMap<CreateCategoryRequestModel, CategoryModel>().ForAllOtherMembers(opts => opts.Ignore());
            CreateMap<CategoryModel, CreateCategoryRequestModel>().ForAllOtherMembers(opts => opts.Ignore());

            CreateMap<CategoryModel, Category>().ForAllOtherMembers(opts => opts.Ignore());
            CreateMap<Category, CategoryModel>().ForAllOtherMembers(opts => opts.Ignore());

            CreateMap<ChangeJobOfferStatus, ChangeJobStatusRequestModel>();
            CreateMap<ChangeJobStatusRequestModel, ChangeJobOfferStatus>();

            CreateMap<BidFilterModel, BidModel>();
            CreateMap<BidModel, BidFilterModel>();
            
            CreateMap<CreateBidRequest, BidModel>().ForAllOtherMembers(opts => opts.Ignore());
            CreateMap<BidModel, CreateBidRequest>().ForAllOtherMembers(opts => opts.Ignore());

            CreateMap<UserProfileFilterModel, UserProfileModel>().ForAllOtherMembers(opts => opts.Ignore());
            CreateMap<UserProfileModel, UserProfileFilterModel>().ForAllOtherMembers(opts => opts.Ignore());

            CreateMap<CreateUserProfileRequestModel, UserProfileModel>().ForAllOtherMembers(opts => opts.Ignore());
            CreateMap<UserProfileModel, CreateUserProfileRequestModel>().ForAllOtherMembers(opts => opts.Ignore());

            CreateMap<UserProfile, UserProfileModel>().ForAllOtherMembers(opts => opts.Ignore());
            CreateMap<UserProfileModel, UserProfile>().ForAllOtherMembers(opts => opts.Ignore());

            CreateMap<EmployerProfileModel, EmployerProfileViewModel>().ForAllOtherMembers(opts => opts.Ignore());
            CreateMap<EmployerProfileViewModel, EmployerProfileModel>().ForAllOtherMembers(opts => opts.Ignore());

            CreateMap<EmployerProfileModel, EmployerProfile>().ForAllOtherMembers(opts => opts.Ignore());
            CreateMap<EmployerProfile, EmployerProfileModel>().ForAllOtherMembers(opts => opts.Ignore());

            CreateMap<EmployerProfileFilterModel, EmployerProfileModel>().ForAllOtherMembers(opts => opts.Ignore());
            CreateMap<EmployerProfileModel, EmployerProfileFilterModel>().ForAllOtherMembers(opts => opts.Ignore());

            CreateMap<CreateEmployerProfileRequestModel, EmployerProfileModel>().ForAllOtherMembers(opts => opts.Ignore());
            CreateMap<EmployerProfileModel, CreateEmployerProfileRequestModel>().ForAllOtherMembers(opts => opts.Ignore());
        }

    }
}
