using Application.Contracts;
using Application.Contracts.IService;
using Application.DTOs;

using Domain.Entities.MasterDB;

namespace ApilogsServiceImp;

public class ApiLogService(IUnitofWork unitofWork) : IApiLogService
{
    public async Task SaveLogAsync(ApiLog log)
    {
        await unitofWork.APIlogRepo.SaveLogAsync(log);
    }

    public async Task<SystemMetricsDto> GetSystemMetricsAsync()
    {
        var dto = await unitofWork.APIlogRepo.GetSystemMetricsAsync();
        return dto;
    }
}