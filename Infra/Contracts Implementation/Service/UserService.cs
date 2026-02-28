using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Application;
using Application.Contracts;
using Application.Contracts.IService;
using Application.DTOs;
using Application.Enums;

using AutoMapper;

using Domain.Entities.MasterDB;

using Infra;

using MediatR;

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using SystemAdminSeedResultEnum = Application.Enums.SystemAdminSeedResultEnum;
namespace User_service_Imp;
public class UserService(IMapper _mapper, IMediator _mediator,IUnitofWork unitofwork) : IUserService
{

    private static string GenerateJwt(User user, IList<string> roles)
    {
        var claims = new List<Claim>
    {
        new(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new (ClaimTypes.Email, user.Email!),
        new ("TenantId", user.TenantId?.ToString() ?? string.Empty)
    };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var Issuer = Environment.GetEnvironmentVariable("SaasJWTIssuer");
        var audience = Environment.GetEnvironmentVariable("SaasJWTAudience");
        var JWTkey = Environment.GetEnvironmentVariable("SaasJwtKey");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTkey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: Issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    private async Task<Tenant> AddTenantAsync(TenantRegistrationDto tenant)
    {
        var baseConfig = Environment.GetEnvironmentVariable("SaasDatabaseConfig");
        var TenantDBConnection = new Npgsql.NpgsqlConnectionStringBuilder(baseConfig)
        {
            Database = $"saas_{tenant.Slug.ToLower()}"
        }.ToString();
        Tenant? mapping = _mapper.Map<Tenant>(tenant);
        mapping.ConnectionString = TenantDBConnection;
        await unitofwork.TenantRepo.AddTenantAsync(mapping);
        return mapping;
    }
    public async Task<ProvisioningStatusDto> RegisterTenantAdmin(TenantRegistrationDto dto)
    {
        var domainFromEmail = dto.Email.Split('@')[1];
        var tenant = await unitofwork.TenantRepo.GetTenantBySlugAndDomainAsync(dto.Slug, domainFromEmail);
        if (tenant != null) return new ProvisioningStatusDto(null, "Failed", "Tenant already exists");
        var dtoWithDomain = dto with { TenantDomain = domainFromEmail };
        var addTenant = await AddTenantAsync(dtoWithDomain);
        var defaultPlanEnum = (int)PlanTier.Free;
        var defaultPlanId= await unitofwork.SubscriptionRepo.GetPlanByIdAsync(defaultPlanEnum);
        var subscription = new TenantSubscription
        {
            TenantId = addTenant.Id,
            SubscriptionPlanId = Guid.Parse(defaultPlanId.Id.ToString()),
            StartDate = DateTime.UtcNow,
        };
        await unitofwork.TenantSubscriptionRepo.AddTenantSubscriptionAsync(subscription);
        var planFeatures = await unitofwork.PlanFeatureRepo.GetFeaturesByPlanIdAsync(defaultPlanId.Id);
        if (planFeatures != null && planFeatures.Any())
        {
            var tenantFeatures = planFeatures.Select(pf => new TenantFeature
            {
                TenantId = addTenant.Id,
                FeatureId = pf.FeatureId,
                CreatedAt = DateTime.UtcNow,
                IsEnabled = true
            }).ToList();
            await unitofwork.TenantFeatureRepo.AddRangeTenantFeaturAsync(tenantFeatures);
        }
            var user = new User
            {
                UserName = dto.Email,
                Email = dto.Email,
                TenantId = addTenant.Id,
                TenantDomain = domainFromEmail
            };
            await _mediator.Publish(new TenantCreatedEvent(addTenant.Id, addTenant.ConnectionString));
            await unitofwork.UserRepository.CreateUserWithRoleAsync(user, dto.Password, "TenantAdmin");
            await unitofwork.CommitAsync();
             return new ProvisioningStatusDto(addTenant.Id, "In Progress", "Database Proverisiong Started"); ;
    }
    public async Task<string> RegisterTenantUser(TenantUserRegistraionDto dto)
    {
        var domainFromEmail = dto.Email.Split('@')[1];
        var tenant = await unitofwork.TenantRepo.GetTenantByDomainAsync(domainFromEmail);
        if (tenant == null) return "Your company domain is not registered in our system.";
        var existingUser = await unitofwork.UserRepository.FindByEmailAsync(dto.Email);
        if (existingUser != null) return "User already exists. Please login.";
        var user = new User
        {
            UserName = dto.Email,
            Email = dto.Email,
            TenantId = tenant.Id,
            TenantDomain = domainFromEmail

        };
        var result = await unitofwork.UserRepository.CreateUserWithRoleAsync(user, dto.Password, "TenantUser");
        if (!result.Succeeded)
        {
            return "Failed Registration. Please try again later.";
        }

        return "User Registered Successfully and linked to " + tenant.Name;
    }
    public async Task<string> Login(LoginDto dto)
    {
        var user = await unitofwork.UserRepository.FindByEmailAsync(dto.Email);
        if (user == null || !await unitofwork.UserRepository.CheckPasswordAsync(user, dto.Password))
            throw new UnauthorizedAccessException();

        var roles = await unitofwork.UserRepository.GetRolesAsync(user);
        if (!roles.Contains("SystemAdmin"))
        {
            var domain = dto.Email.Split('@').Last().ToLower();
            var tenant = await unitofwork.TenantRepo.GetTenantByDomainAsync(domain);

            if (tenant == null || user.TenantId != tenant.Id)throw new UnauthorizedAccessException("Domain/Tenant mismatch.");
        }
        return GenerateJwt(user, roles);

    }
    public async Task<SystemAdminSeedResultEnum> EnsureSystemAdminAsync()
    {
        var ensureAdmin = await unitofwork.UserRepository.EnsureSystemAdminAsync();
        return ensureAdmin;
    }
}
                                                                                              