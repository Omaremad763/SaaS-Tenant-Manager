using Application.Contracts;
using Application.DTOs;

using FluentValidation;

using MediatR;

namespace Application.CQRS;

//Queries
public record GetTenantSubscriptionQuery(Guid TenantId) : IRequest<SubscriptionPlanDetailsDto>;
public record ToggleFeatureCommand(ToggleFeatureAccessDTO DTO) : IRequest<bool>;

//validators
public class GetTenantSubscriptionValidator : AbstractValidator<GetTenantSubscriptionQuery>
{
    public GetTenantSubscriptionValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty().WithMessage("TenantId is required to fetch plan details.");
    }
}
public class ToggleFeatureCommandValidator : AbstractValidator<ToggleFeatureCommand>
{
    public ToggleFeatureCommandValidator()
    {
        RuleFor(x => x.DTO.TenantId).NotEmpty();
        RuleFor(x => x.DTO.FeatureName).NotEmpty().WithMessage("Feature name must be specified.");
    }
}
//handler
public class SubscriptionQueryHandler :
    IRequestHandler<GetTenantSubscriptionQuery, SubscriptionPlanDetailsDto>,
    IRequestHandler<ToggleFeatureCommand, bool>
{
    private readonly ISaasServices _Services;

    public SubscriptionQueryHandler(ISaasServices Services)
    {
        _Services = Services;
    }

    public async Task<SubscriptionPlanDetailsDto> Handle(GetTenantSubscriptionQuery request, CancellationToken cancellationToken)
    {
        return await _Services.TenantSubscriptionService.GetTenantSubscriptionAsync(request.TenantId, cancellationToken);
    }
    public async Task<bool>Handle(ToggleFeatureCommand request, CancellationToken cancellationToken)
    {
        return await _Services.TenantFeatureService.ToggleFeatureAsync(request.DTO);

    }
}

