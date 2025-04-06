using AngularSaleAPI.Application.Abstractions.Services.UserServices;
using AngularSaleAPI.Application.DTOs.User;
using AngularSaleAPI.Application.Exceptions;
using AngularSaleAPI.Application.Features.Commands.AppUser.CreateUser;
using AngularSaleAPI.Application.Helpers;
using AngularSaleAPI.Application.Repositories;
using AngularSaleAPI.Domain.Entities;
using AngularSaleAPI.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using A = AngularSaleAPI.Domain.Entities.Identity;

namespace AngularSaleAPI.Persistence.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly UserManager<A.AppUser> _userManager;
        readonly IEndpointReadRepository _endpointReadRepository;

        public UserService(UserManager<A.AppUser> userManager, IEndpointReadRepository endpointReadRepository)
        {
            _userManager = userManager;
            _endpointReadRepository = endpointReadRepository;
        }
        public async Task<CreateUserResponseDTO> CreateAsync(CreateUserRequestDTO model)
        {

            IdentityResult result = await _userManager.CreateAsync(new()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = model.Username,
                Email = model.Email,
                NameSurname = model.Name
            }, model.Password);

            CreateUserResponseDTO response = new() { Succeeded = result.Succeeded };
            if (result.Succeeded)
                response.Message = "User created.";
            else
                // throw new UserCreateFailedException();
                foreach (var error in result.Errors)
                    response.Message += $"{error.Code} - {error.Description}\n";

            return response;
        }

        public async Task UpdateRefreshTokenAsync(string refreshToken, AppUser user, DateTime accessTokenDate, int addOnAccessTokenDate)
        {
            if (user != null)
            {
                user.RefreshToken = refreshToken;
                user.RefreshTokenEndDate = accessTokenDate.AddSeconds(addOnAccessTokenDate);
                await _userManager.UpdateAsync(user);
            }
            else
                throw new NotFoundUserException();
        }

        public async Task UpdatePasswordAsync(string userId, string resetToken, string newPassword)
        {
            AppUser user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                resetToken = resetToken.UrlDecode();
                IdentityResult result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
                if (result.Succeeded)
                    await _userManager.UpdateSecurityStampAsync(user);
                else
                    throw new PasswordChangeFailedException();
            }
        }

        public async Task<List<UserDTO>> GetAllUsersAsync(int page, int size)
        {
            var users = await _userManager.Users
                .Skip(page * size)
                .Take(size)
                .ToListAsync();
            return users.Select(user => new UserDTO
            {
                Id = user.Id,
                Email = user.Email,
                NameSurname = user.NameSurname,
                UserName = user.UserName,
            }).ToList();
        }

        public async Task AssingRoleToUserAsync(string userId, string[] roles)
        {
            AppUser user =  await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, userRoles);

                await _userManager.AddToRolesAsync(user, roles);
            }
            else
                throw new NotFoundUserException();
        }

        public async Task<string[]> GetRolesToUserAsync(string userIdOrName)
        {
            AppUser user = await _userManager.FindByIdAsync(userIdOrName);
            if (user == null)
                user = await _userManager.FindByNameAsync(userIdOrName);

            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                return userRoles.ToArray();
            }
            else
                throw new NotFoundUserException();
        }

        public async Task<bool> HasRolePermissonToEndpointAsync(string name, string code)
        {
            var userRoles = await GetRolesToUserAsync(name);
            if (!userRoles.Any()) 
                return false;

            Endpoint? endpoint = await _endpointReadRepository.Table
                .Include(e => e.Roles)
                .FirstOrDefaultAsync(e => e.Code == code);
            if(endpoint == null)
                return false;

            var endpointRoles = endpoint.Roles.Select(r => r.Name);

            foreach(var userRole in userRoles)
                foreach (var endpointRole in endpointRoles)
                    if (userRole == endpointRole)
                        return true;
            return false;
        }

        public int TotalUsersCount => _userManager.Users.Count();

    }
}
