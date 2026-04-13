using Application.Contracts;
using Application.Contracts.IService;

namespace Infra.Contracts_Implementation.Service;

public class FeatureServcie(IUnitofWork unitofWork) : IFeatureService
{
    List<string> IFeatureService.GetAllFeatures()
    {
        return unitofWork.FeatureRepo.GetAllFeatures();
    }
}