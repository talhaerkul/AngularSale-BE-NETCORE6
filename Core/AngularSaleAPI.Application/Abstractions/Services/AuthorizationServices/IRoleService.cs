using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularSaleAPI.Application.Abstractions.Services.AuthorizationServices
{
    public interface IRoleService
    {
        Task<bool> CreateRole(string name);
        Task<bool> DeleteRole(string Id);
        Task<bool> UpdateRole(string id, string name);
        (object,int) GetAllRoles(int page, int size);
        Task<(string id, string name)> GetRoleById(string id);
    }
}
