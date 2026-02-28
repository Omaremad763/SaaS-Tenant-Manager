using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Entities.MasterDB;

namespace Application.Contracts.IRepo;
public interface IFeatureRepo
{
    Task<Feature?> GetFeatureByNameAsync(string featureName);
}
