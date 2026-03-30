using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.Contracts;
using Application.Contracts.IService;
using Application.DTOs;

using FluentValidation;

using MediatR;

namespace Application.CQRS;
//commands
public record CreateClientCommand(ClientDto DTO) : IRequest<Unit>;

public record UpdateClientCommand(UpdateClientDto DTO) : IRequest<Unit>;

public record DeleteClientCommand(Guid Id) : IRequest<Unit>;

//queries 
public record GetAllClientsQuery() : IRequest<IEnumerable<ClientDto>>;

public record GetClientByIdQuery(Guid Id) : IRequest<ClientDto?>;
public class CreateClientCommandValidator : AbstractValidator<CreateClientCommand>
{
    public CreateClientCommandValidator()
    {
        RuleFor(x => x.DTO.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.DTO.Address)
            .NotEmpty();
    }
}
public class UpdateClientCommandValidator : AbstractValidator<UpdateClientCommand>
{
    public UpdateClientCommandValidator()
    {
        RuleFor(x => x.DTO.Id)
            .NotEmpty();

        RuleFor(x => x.DTO.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.DTO.Address)
            .NotEmpty();
    }
}
public class DeleteClientCommandValidator : AbstractValidator<DeleteClientCommand>
{
    public DeleteClientCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}

public class GetClientByIdQueryValidator : AbstractValidator<GetClientByIdQuery>
{
    public GetClientByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}

public class ClientHandler(ISaasServices SaasServices) :
    IRequestHandler<CreateClientCommand, Unit>,
    IRequestHandler<UpdateClientCommand, Unit>,
    IRequestHandler<DeleteClientCommand, Unit>,
    IRequestHandler<GetAllClientsQuery, IEnumerable<ClientDto>>,
    IRequestHandler<GetClientByIdQuery, ClientDto?>
{
    private readonly ISaasServices _SaasServices = SaasServices;

    public async Task<Unit> Handle(CreateClientCommand request, CancellationToken cancellationToken)
    {
        await _SaasServices.ClientService.CreateClientAsync(request.DTO);
        return Unit.Value;

    }

    public async Task<Unit> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
    {
        await _SaasServices.ClientService.UpdateClientAsync(request.DTO);
        return Unit.Value;
    }

    public async Task<Unit> Handle(DeleteClientCommand request, CancellationToken cancellationToken)
    {
        var dto = new ClientDto { Id = request.Id };
        await _SaasServices.ClientService.DeleteClientAsync(dto);
        return Unit.Value;
    }

    public async Task<IEnumerable<ClientDto>> Handle(GetAllClientsQuery request, CancellationToken cancellationToken)
    {
       return await _SaasServices.ClientService.GetAllClientsAsync();
    }

    public async Task<ClientDto?> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
    {
        return await _SaasServices.ClientService.GetClientByIdAsync(request.Id);
    }
}