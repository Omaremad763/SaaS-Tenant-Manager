using Application.DTOs;

using AutoMapper;

using Domain.Entities;

namespace Application;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<TenantRegistrationDto, Tenant>();
        CreateMap<GetTenantDto, Tenant>().ReverseMap();

    }
}

