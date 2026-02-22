using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Application;
using Application.Contracts;
using Application.Contracts.Auth;
using Application.DTOs;

using AutoMapper;

using Domain.Entities;

using Infra;

using MediatR;

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using SystemAdminSeedResultEnum = Application.SystemAdminSeedResultEnum;

public class UserService(IMapper _mapper, IMediator _mediator,IUnitofWork unitofwork) : IUserService
{

    private string GenerateJwt(User user, IList<string> roles)
    {
        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email!),
        new Claim("TenantId", user.TenantId?.ToString() ?? string.Empty)
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
        await unitofwork.userRepository.AddTenantAsync(mapping);
        return mapping;
    }
    public async Task<ProvisioningStatusDto> RegisterTenantAdmin(TenantRegistrationDto dto)
    {
        var domainFromEmail = dto.Email.Split('@')[1];
        var tenant = await unitofwork.userRepository.GetTenantBySlugAndDomainAsync(dto.Slug, domainFromEmail);
        if (tenant != null) return new ProvisioningStatusDto(null, "Failed", "Tenant already exists");
        var dtoWithDomain = dto with { TenantDomain = domainFromEmail };
        var addTenat = await AddTenantAsync(dtoWithDomain);
        var user = new User
        {
            UserName = dto.Email,
            Email = dto.Email,
            TenantId = addTenat.Id,
            TenantDomain=domainFromEmail
        };
        await unitofwork.userRepository.CreateUserWithRoleAsync(user, dto.Password, "TenantAdmin");
        await unitofwork.CommitAsync();
        await _mediator.Publish(new TenantCreatedEvent(addTenat.Id, addTenat.ConnectionString));
        return new ProvisioningStatusDto(addTenat.Id, "In Progress", "Database Proverisiong Started"); ;
    }
    public async Task<string> RegisterTenantUser(TenantUserRegistraionDto dto)
    {
        var domainFromEmail = dto.Email.Split('@')[1];
        var tenant = await unitofwork.userRepository.GetTenantByDomainAsync(domainFromEmail);
        if (tenant == null) return "Your company domain is not registered in our system.";
        var existingUser = await unitofwork.userRepository.FindByEmailAsync(dto.Email);
        if (existingUser != null) return "User already exists. Please login.";
        var user = new User
        {
            UserName = dto.Email,
            Email = dto.Email,
            TenantId = tenant.Id,
            TenantDomain = domainFromEmail

        };
        var result = await unitofwork.userRepository.CreateUserWithRoleAsync(user, dto.Password, "TenantUser");
        if (!result.Succeeded)
        {
            return "Failed Registration. Please try again later.";
        }

        return "User Registered Successfully and linked to " + tenant.Name;
    }
    public async Task<string> Login(LoginDto dto)
    {
        var user = await unitofwork.userRepository.FindByEmailAsync(dto.Email);
        if (user == null || !await unitofwork.userRepository.CheckPasswordAsync(user, dto.Password))
            throw new UnauthorizedAccessException();

        var roles = await unitofwork.userRepository.GetRolesAsync(user);
        if (!roles.Contains("SystemAdmin"))
        {
            var domain = dto.Email.Split('@').Last().ToLower();
            var tenant = await unitofwork.userRepository.GetTenantByDomainAsync(domain);

            if (tenant == null || user.TenantId != tenant.Id)throw new UnauthorizedAccessException("Domain/Tenant mismatch.");
        }
        return GenerateJwt(user, roles);

    }
    public async Task<SystemAdminSeedResultEnum> EnsureSystemAdminAsync()
    {
        var ensureAdmin = await unitofwork.userRepository.EnsureSystemAdminAsync();
        return ensureAdmin;
    }
}
