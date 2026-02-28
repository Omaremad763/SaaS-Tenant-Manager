using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.Contracts;
using Application.DTOs;

using FluentValidation;

using MediatR;

namespace Application.CQRS;
public record RegisterTenantAdminCommand(TenantRegistrationDto Data) : IRequest<ProvisioningStatusDto>;
public record RegisterTenantUserCommand(TenantUserRegistraionDto Data) : IRequest<string>;
public record LoginCommand(LoginDto Data) : IRequest<string>;
public  class TenantRegistrationDtoValidator : AbstractValidator<TenantRegistrationDto>
{
    public TenantRegistrationDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Tenant name is required.").MaximumLength(100);
        RuleFor(x => x.Slug).NotEmpty().WithMessage("Slug is required.").MaximumLength(50).Matches("^[a-z0-9-]+$")
            .WithMessage("Slug must contain only lowercase letters, numbers, and hyphens.")
            .Must(slug => !slug.StartsWith("-") && !slug.EndsWith("-"))
            .WithMessage("Slug cannot start or end with a hyphen.");
        RuleFor(x => x.PlanId).NotEmpty().WithMessage("PlanId is required.");
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.").EmailAddress().WithMessage("Invalid Email Format.")
        .Must(email =>{var parts = email.Split('@');return parts.Length == 2 && parts[1].Length >= 2;});
    }
}
public class TenantRegistrationUserDtoValidator : AbstractValidator<TenantUserRegistraionDto>
{
    public TenantRegistrationUserDtoValidator()
    {
        RuleFor(x => x.Email)
    .NotEmpty()
    .WithMessage("Email is required.")
    .EmailAddress()
    .WithMessage("Invalid Email Format.")
    .Must(email =>
    {
        var parts = email.Split('@');
        return parts.Length == 2 && parts[1].Length >= 2;
    })
    .WithMessage("Invalid Email Domain.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 characters.");
    }
}
public class LoginValidator : AbstractValidator<LoginDto>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email)
    .NotEmpty()
    .WithMessage("Email is required.")
    .EmailAddress()
    .WithMessage("Invalid Email Format.")
    .Must(email =>
    {
        var parts = email.Split('@');
        return parts.Length == 2 && parts[1].Length >= 2;
    })
    .WithMessage("Invalid Email Domain.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 characters.");
    }
}

public class RegisterTenantHandler(ISaasServices service) :
    IRequestHandler<RegisterTenantAdminCommand, ProvisioningStatusDto>,
    IRequestHandler<RegisterTenantUserCommand, string>,
    IRequestHandler<LoginCommand, string>,
    INotificationHandler<TenantCreatedEvent>
{
    public async Task<ProvisioningStatusDto> Handle(RegisterTenantAdminCommand request, CancellationToken cancellationToken)
    {
        return await service.UserService.RegisterTenantAdmin(request.Data);
    }
    public async Task<string> Handle(RegisterTenantUserCommand request, CancellationToken cancellationToken)
    {
        return await service.UserService.RegisterTenantUser(request.Data);
    }
    public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        return await service.UserService.Login(request.Data);
    }
    public async Task Handle(TenantCreatedEvent notification, CancellationToken cancellationToken)
    {
        await service.DbMigrationService.MigrateTenantDatabaseAsync(notification.ConnectionString);
    }


}