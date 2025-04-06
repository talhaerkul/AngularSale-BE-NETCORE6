using AngularSaleAPI.Application.DTOs.User;
using AngularSaleAPI.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularSaleAPI.Application.Abstractions.Services.UserServices
{
    public interface IUserService
    {
        Task<CreateUserResponseDTO> CreateAsync(CreateUserRequestDTO model);
        Task UpdateRefreshTokenAsync(string refreshToken, AppUser user, DateTime accessTokenDate, int addOnAccessTokenDate);
        Task UpdatePasswordAsync(string userId, string resetToken, string newPassword);
        Task<List<UserDTO>> GetAllUsersAsync(int page, int size);
        public int TotalUsersCount { get;}
        Task AssingRoleToUserAsync(string userId, string[] roles);
        Task<string[]> GetRolesToUserAsync(string userIdOrName);
        Task<bool> HasRolePermissonToEndpointAsync(string name, string code);
    }
}
