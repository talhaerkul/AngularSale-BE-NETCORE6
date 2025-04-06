using T = AngularSaleAPI.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngularSaleAPI.Domain.Entities.Identity;

namespace AngularSaleAPI.Application.Abstractions.Token
{
    public interface ITokenHandler
    {
        T.Token CreateAccessToken(int seconds, AppUser user);
        string CreateRefreshToken();
    }
}
