using Application.Contracts;
using Application.Contracts.IService;

using Domain.Entities.MasterDB;

using Infra.Persistence.Contexts;

namespace ApilogsServiceImp;
public class ApiLogService(MasterDbContext dbContext,IUnitofWork unitofWork) : IApiLogService
{
    public async Task SaveLogAsync(ApiLog log)
    {
        dbContext.ApiLogs.Add(log);
        await unitofWork.CommitAsync();
    }
}