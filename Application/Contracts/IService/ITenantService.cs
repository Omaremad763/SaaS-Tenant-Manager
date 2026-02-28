using Domain.Entities.MasterDB;
namespace Application.Contracts.IService;
public interface ITenantService
{
      Task<Tenant?> GetTenantBySlugAndDomainAsync(string slug, string domain);
      Task<Tenant?> GetTenantByDomainAsync(string domain);
      Task AddTenantAsync(Tenant tenant);
}
