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
public record CreateClientCommand(ClientDTO DTO) : IRequest<Unit>;

public record UpdateClientCommand(UpdateClientDTO DTO) : IRequest<Unit>;

public record DeleteClientCommand(Guid Id) : IRequest<Unit>;

//queries 
public record GetAllClientsQuery() : IRequest<IEnumerable<ClientDTO>>;

public record GetClientByIdQuery(Guid Id) : IRequest<ClientDTO?>;
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
    IRequestHandler<GetAllClientsQuery, IEnumerable<ClientDTO>>,
    IRequestHandler<GetClientByIdQuery, ClientDTO?>
{
    private readonly ISaasServices _SaasServices = SaasServices;

    public async Task<Unit> Handle(CreateClientCommand request, CancellationToken ct)
    {
        await _SaasServices.ClientService.CreateClientAsync(request.DTO);
        return Unit.Value;

    }

    public async Task<Unit> Handle(UpdateClientCommand request, CancellationToken ct)
    {
        await _SaasServices.ClientService.UpdateClientAsync(request.DTO);
        return Unit.Value;
    }

    public async Task<Unit> Handle(DeleteClientCommand request, CancellationToken ct)
    {
        var dto = new ClientDTO { Id = request.Id };
        await _SaasServices.ClientService.DeleteClientAsync(dto);
        return Unit.Value;
    }

    public async Task<IEnumerable<ClientDTO>> Handle(GetAllClientsQuery request, CancellationToken ct)
    {
       return await _SaasServices.ClientService.GetAllClientsAsync();
    }

    public async Task<ClientDTO?> Handle(GetClientByIdQuery request, CancellationToken ct)
    {
        return await _SaasServices.ClientService.GetClientByIdAsync(request.Id);
    }
}