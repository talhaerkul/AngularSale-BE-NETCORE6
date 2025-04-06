using AngularSaleAPI.Application.Abstractions.Services.UserServices.Authentication;
using AngularSaleAPI.Application.Abstractions.Token;
using AngularSaleAPI.Application.DTOs;
using AngularSaleAPI.Application.DTOs.Facebook;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using A = AngularSaleAPI.Domain.Entities.Identity;

namespace AngularSaleAPI.Application.Features.Commands.AppUser.FacebookLogin
{
    public class FacebookLoginCommandHandler : IRequestHandler<FacebookLoginCommandRequest, FacebookLoginCommandResponse>
    {
        private readonly IExternalAuthService _authService;
        private readonly IConfiguration _configuration;

        public FacebookLoginCommandHandler(IExternalAuthService externalAuthService, IConfiguration configuration)
        {
            _authService = externalAuthService;
            _configuration = configuration;
        }

        public async Task<FacebookLoginCommandResponse> Handle(FacebookLoginCommandRequest request, CancellationToken cancellationToken)
        {
            var token = await _authService.FacebookLoginAsync(request.AuthToken, int.Parse(_configuration["TokenLifeTime"]));
            return new() {
                Token = token
            };
        }
    }
}
