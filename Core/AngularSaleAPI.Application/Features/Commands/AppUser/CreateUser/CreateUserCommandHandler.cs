using AngularSaleAPI.Application.Abstractions.Services.UserServices;
using AngularSaleAPI.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using A = AngularSaleAPI.Domain.Entities.Identity;

namespace AngularSaleAPI.Application.Features.Commands.AppUser.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
    {
        private readonly UserManager<A.AppUser> _userManager;
        private readonly IUserService _userService;


        public CreateUserCommandHandler(UserManager<A.AppUser> userManager, IUserService userService)
        {
            _userManager = userManager;
            _userService = userService;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {
            var response = await _userService.CreateAsync(new()
            {
                Email = request.Email,
                Name = request.Name,
                Password = request.Password,
                PasswordConfirm = request.PasswordConfirm,
                Username = request.Username
            });
            return new()
            {
                Message = response.Message,
                Succeeded = response.Succeeded,
            };        
        }
    }
}
