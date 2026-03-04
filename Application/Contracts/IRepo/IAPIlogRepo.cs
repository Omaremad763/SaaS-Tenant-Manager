using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.DTOs;

using Domain.Entities.MasterDB;

namespace Application.Contracts.IRepo;
public interface IAPIlogRepo
{
    Task<SystemMetricsDto> GetSystemMetricsAsync();
    Task SaveLogAsync(ApiLog log);

}
