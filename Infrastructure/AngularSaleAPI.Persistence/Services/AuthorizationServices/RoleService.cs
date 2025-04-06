using AngularSaleAPI.Application.Abstractions.Services.AuthorizationServices;
using AngularSaleAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularSaleAPI.Persistence.Services.AuthorizationServices
{
    public class RoleService : IRoleService
    {
        readonly RoleManager<AppRole> _roleManager;

        public RoleService(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<bool> CreateRole(string name)
        {
            var result = await _roleManager.CreateAsync(new(){ Id = Guid.NewGuid().ToString(), Name = name});
            if (result.Succeeded)
            {
                return true;
            }
            else
                throw new Exception();
        }

        public async Task<bool> DeleteRole(string id)
        {
            AppRole role = await _roleManager.FindByIdAsync(id);
            var result = await _roleManager.DeleteAsync(role);
            return result.Succeeded;
        }

        public (object,int) GetAllRoles(int page, int size)
        {
            var query = _roleManager.Roles;
            IQueryable<AppRole> _query = null;
            if (page != -1 && size != -1)
                _query = query.Skip(page * size).Take(size);
            else
                _query = query;
            return (_query.Select(r => new { r.Id, r.Name}), query.Count());
        }

        public async Task<(string id, string name)> GetRoleById(string id)
        {
            string role = await _roleManager.GetRoleIdAsync(new() { Id = id });
            return (id, role);
        }

        public async Task<bool> UpdateRole(string id, string name)
        {
            AppRole role = await _roleManager.FindByIdAsync(id);
            role.Name = name;
            var result = await _roleManager.UpdateAsync(role);
            return result.Succeeded;
        }
    }
}
