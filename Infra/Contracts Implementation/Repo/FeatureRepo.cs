using Application.Contracts.IRepo;

using Domain.Entities.MasterDB;

using Infra.Persistence.Contexts;

using Microsoft.EntityFrameworkCore;

namespace Infra.Contracts_Implementation.Repo;

public class FeatureRepo(MasterDbContext context) : IFeatureRepo
{
    public async Task<Feature?> GetFeatureByNameAsync(string featureName)
    {
        return await context.Features.FirstOrDefaultAsync(f => f.FeatureName == featureName);
    }

    public List<string> GetAllFeatures()
    {
        return context.Features.Select(x => x.FeatureName).ToList();
    }
}