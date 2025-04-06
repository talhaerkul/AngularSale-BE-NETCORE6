using AngularSaleAPI.Application.Abstractions.Services.UserServices.Authentication;
using AngularSaleAPI.Application.Abstractions.Token;
using AngularSaleAPI.Application.DTOs;
using AngularSaleAPI.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using A = AngularSaleAPI.Domain.Entities.Identity;

namespace AngularSaleAPI.Application.Features.Commands.AppUser.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
    {
        private readonly IInternalAuthService _authService;
        private readonly IConfiguration _configuration;

        public LoginUserCommandHandler(IInternalAuthService authService, IConfiguration configuration)
        {
            _authService = authService;
            _configuration = configuration;
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            var token = await _authService.LoginAsync(request.UsernameOrEmail, request.Password, int.Parse(_configuration["TokenLifeTime"]));
            return new()
            {
                Token = token
            };
        }
    }
}
