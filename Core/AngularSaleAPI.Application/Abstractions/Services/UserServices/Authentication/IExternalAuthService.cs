using AngularSaleAPI.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularSaleAPI.Application.Abstractions.Services.UserServices.Authentication
{
    public interface IExternalAuthService
    {
        Task<DTOs.Token> GoogleLoginAsync(string idToken, int accessTokenLifeTime);

        Task<DTOs.Token> FacebookLoginAsync(string authToken, int accessTokenLifeTime);

    }
}
