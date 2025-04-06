using AngularSaleAPI.Application.Abstractions.Services.UserServices.Authentication;
using AngularSaleAPI.Application.Abstractions.Token;
using AngularSaleAPI.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using A = AngularSaleAPI.Domain.Entities.Identity;

namespace AngularSaleAPI.Application.Features.Commands.AppUser.GoogleLogin
{
    public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommandRequest, GoogleLoginCommandResponse>
    {
        private readonly IExternalAuthService _authService;
        private readonly IConfiguration _configuration;

        public GoogleLoginCommandHandler(IExternalAuthService externalAuthService, IConfiguration configuration)
        {
            _authService = externalAuthService;
            _configuration = configuration;
        }

        public async Task<GoogleLoginCommandResponse> Handle(GoogleLoginCommandRequest request, CancellationToken cancellationToken)
        {
            var token = await _authService.GoogleLoginAsync(request.IdToken, int.Parse(_configuration["TokenLifeTime"]));
            //var token = await _authService.GoogleLoginAsync(request.IdToken, 15);

            return new()
            {
                Token = token
            };
        }
    }
}
