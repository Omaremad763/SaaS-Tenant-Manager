using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.Contracts;
using Application.DTOs;

using Domain.Entities.TenantDBEntities;

using FluentValidation;

using MediatR;

namespace Application.CQRS;
public record CreateShipmentCommand(CreateShipmentDTO DTO) : IRequest<bool>;
public record UpdateShipmentStatusCommand(UpdateShipmentDTO DTO) : IRequest<bool>;

public record GetTenantShipmentsQuery() : IRequest<List<ShipmentDto?>>;
public record GetClientStatisticsQuery() : IRequest<ClientStatsDto>;
public class ShipmentValidators : AbstractValidator<CreateShipmentCommand>
{
    public ShipmentValidators()
    {
        RuleFor(x => x.DTO.ReceiverName)
            .NotEmpty().WithMessage("Receiver Name is required");
      
        RuleFor(x => x.DTO.ReceiverPhone)
    .NotEmpty().WithMessage("Receiver Phone  is required");

        RuleFor(x => x.DTO.Destination)
            .NotEmpty().WithMessage("Destination is required");

        RuleFor(x => x.DTO.ClientId)
            .NotEmpty().WithMessage("Client is required");

        RuleForEach(x => x.DTO.Items).ChildRules(items =>
        {
            items.RuleFor(i => i.ProductName)
                .NotEmpty().WithMessage("Product Name is required");

            items.RuleFor(i => i.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0");

            items.RuleFor(i => i.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0");
        });

    }
}

public class UpdateStatusValidator : AbstractValidator<UpdateShipmentStatusCommand>
{
    public UpdateStatusValidator()
    {
        RuleFor(x => x.DTO.Id).NotEmpty();
        RuleFor(x => x.DTO.Status)
            .IsInEnum()
            .WithMessage("Invalid shipment status.");
    }
}
public class ShipmentHandler(ISaasServices services) :
    IRequestHandler<CreateShipmentCommand, bool>,
    IRequestHandler<UpdateShipmentStatusCommand, bool>,
    IRequestHandler<GetTenantShipmentsQuery, List<ShipmentDto?>>,
    IRequestHandler<GetClientStatisticsQuery, ClientStatsDto>
{
    public async Task<bool> Handle(CreateShipmentCommand request, CancellationToken ct)
    {
        return await services.ShipmentService.CreateShipment(request.DTO, ct);
    }

    public async Task<bool> Handle(UpdateShipmentStatusCommand request, CancellationToken ct)
    {
        return await services.ShipmentService.UpdateShipment(request.DTO, ct);

    }

    public async Task<List<ShipmentDto?>> Handle(GetTenantShipmentsQuery request, CancellationToken ct)
    {
        return await services.ShipmentService.GetTenantShipments( ct);

    }

    public async Task<ClientStatsDto> Handle(GetClientStatisticsQuery request, CancellationToken ct)
    {
        return await services.ShipmentService.GetClientStatistics(ct);

    }
}