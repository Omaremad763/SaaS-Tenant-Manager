using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.Contracts;
using Application.Contracts.IService;
using Application.DTOs;

namespace Infra.Contracts_Implementation.Service;
public class PlanFeatureService(IUnitofWork unitofWork) : IPlanFeatureService
{

}
