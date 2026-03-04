using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.DTOs;

using Domain.Entities.MasterDB;

namespace Application.Contracts.IService;
public interface IApiLogService
{
    Task SaveLogAsync(ApiLog log);
    Task<SystemMetricsDto> GetSystemMetricsAsync();
}
