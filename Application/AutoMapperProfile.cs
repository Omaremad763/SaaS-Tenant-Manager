using Application.DTOs;

using AutoMapper;

using Domain.Entities.MasterDB;

namespace Application;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<TenantRegistrationDto, Tenant>();
        CreateMap<GetTenantDto, Tenant>().ReverseMap();
        CreateMap<TenantSubscription, SubscriptionPlanDetailsDto>()
            .ForMember(dest => dest.PlanName,
                opt => opt.MapFrom(src => src.SubscriptionPlanTable.PlanName))
            .ForMember(dest => dest.Price,
                opt => opt.MapFrom(src => src.SubscriptionPlanTable.Price))
            .ForMember(dest => dest.MaxRequestsPerMinute,
                opt => opt.MapFrom(src => src.SubscriptionPlanTable.MaxRequestsPerMinute))
            .ForMember(dest => dest.MaxUsers,
                opt => opt.MapFrom(src => src.SubscriptionPlanTable.MaxUsers))
            .ForMember(dest => dest.EndDate,
                opt => opt.MapFrom(src => src.EndDate))
            .ForMember(dest => dest.IsActive,
                opt => opt.MapFrom(src => src.IsActive));
    }
}

