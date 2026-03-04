using Application.Contracts;
using Application.DTOs;

using FluentValidation;

using MediatR;

namespace Application.CQRS;

//Queries
public record GetTenantSubscriptionQuery(Guid TenantId) : IRequest<SubscriptionPlanDetailsDto>;
public record ToggleFeatureCommand(ToggleFeatureAccessDTO DTO) : IRequest<bool>;
public record GetAllFeaturesQuery(): IRequest<List<string>>;
public record GetAllTenantDataQuery(): IRequest<List<TenantManagementDto>>;

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
public class SubscriptionQueryHandler(ISaasServices Services) :
    IRequestHandler<GetTenantSubscriptionQuery, SubscriptionPlanDetailsDto>,
    IRequestHandler<ToggleFeatureCommand, bool>,
   //IRequestHandler<GetAllFeaturesQuery, List<string>>,
   IRequestHandler<GetAllTenantDataQuery, List<TenantManagementDto>>

{
    private readonly ISaasServices _Services = Services;

    public async Task<SubscriptionPlanDetailsDto> Handle(GetTenantSubscriptionQuery request, CancellationToken cancellationToken)
    {
        return await _Services.TenantSubscriptionService.GetTenantSubscriptionAsync(request.TenantId, cancellationToken);
    }
    public async Task<bool>Handle(ToggleFeatureCommand request, CancellationToken cancellationToken)
    {
        return await _Services.TenantFeatureService.ToggleFeatureAsync(request.DTO);

    }
    //public async Task<List<string>> Handle(GetAllFeaturesQuery request ,CancellationToken cancellationToken)
    //{
    //    return  _Services.FeatureService.GetAllFeatures();

    //}

    public Task<List<TenantManagementDto>> Handle(GetAllTenantDataQuery request, CancellationToken cancellationToken)
    {
        return _Services.TenantService.GetAllTenantsManagementAsync();
    }
}

