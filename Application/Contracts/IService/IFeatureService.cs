using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.IService;
public interface IFeatureService
{
    List<string> GetAllFeatures();

}
