using AngularSaleAPI.Application.Abstractions.Services.UserServices.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularSaleAPI.Application.Abstractions.Services.UserServices
{
    public interface IAuthService : IExternalAuthService, IInternalAuthService
    {

    }
}
