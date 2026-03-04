using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.Contracts;
using Application.DTOs;

using MediatR;

namespace Application.CQRS;
//QUERIES 
public record GetApiLogQuery() : IRequest<SystemMetricsDto>;

//handler 
public class AdminQueryHandler(ISaasServices Services) :
   IRequestHandler<GetApiLogQuery, SystemMetricsDto>
{
    private readonly ISaasServices _Services = Services;

    public async Task<SystemMetricsDto> Handle(GetApiLogQuery request, CancellationToken cancellationToken)
    {
        return await _Services.ApiLogService.GetSystemMetricsAsync();
    }
}