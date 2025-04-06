using AngularSaleAPI.Application.Abstractions.Services.UserServices;
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
using A = AngularSaleAPI.Domain.Entities.Identity;
using System.Threading.Tasks;
using System.Text.Json;
using Google.Apis.Auth;
using AngularSaleAPI.Domain.Entities.Identity;
using AngularSaleAPI.Application.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.WebUtilities;
using AngularSaleAPI.Application.Helpers;

namespace AngularSaleAPI.Persistence.Services.UserServices
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<A.AppUser> _userManager;
        private readonly ITokenHandler _tokenHandler;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<A.AppUser> _signInManager;
        private readonly IUserService _userService;
        private readonly IMailService _mailService;


        public AuthService(UserManager<A.AppUser> userManager, ITokenHandler tokenHandler, IHttpClientFactory httpClientFactory, IConfiguration configuration, SignInManager<AppUser> signInManager, IUserService userService, IMailService mailService)
        {
            _userManager = userManager;
            _tokenHandler = tokenHandler;
            _httpClient = httpClientFactory.CreateClient();
            _configuration = configuration;
            _signInManager = signInManager;
            _userService = userService;
            _mailService = mailService;
        }
        private async Task<Token> CreateUserExternalAsync(AppUser user, string email, string name, UserLoginInfo info, int accessTokenLifeTime)
        {
            bool result = user != null;
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    user = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = email,
                        UserName = email,
                        NameSurname = name
                    };
                    var identityResult = await _userManager.CreateAsync(user);
                    result = identityResult.Succeeded;
                }
            }
            if (result)
            {
                await _userManager.AddLoginAsync(user, info);
                Token token = _tokenHandler.CreateAccessToken(accessTokenLifeTime, user);
                await _userService.UpdateRefreshTokenAsync(token.RefreshToken, user, token.Expiration, 1 * 24 * 60 * 60);

                return token;
            }
            throw new Exception("Invalid external authentication.");

        }
        public async Task<Token> FacebookLoginAsync(string authToken, int accessTokenLifeTime)
        {
            // user validation ve info
            var client_id = _configuration["ExternalLoginSettings:Facebook:FacebookClientId"];
            var client_secret = _configuration["ExternalLoginSettings:Facebook:FacebookAppSecret"];
            string accessTokenResponse = await _httpClient.GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id={client_id}&client_secret={client_secret}");
            var facebookAccessTokenResponse = JsonSerializer.Deserialize<FacebookTokenDTO>(accessTokenResponse);

            string userAccessTokenValidation = await _httpClient.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={authToken}&access_token ={facebookAccessTokenResponse}");
            var validation = JsonSerializer.Deserialize<FacebookValidationDTO>(userAccessTokenValidation);

            if (validation?.Data.IsValid != null)
            {
                string userInfoResponse = await _httpClient.GetStringAsync($"https://graph.facebook.com/me?fields=email,name&access_token={authToken}");
                var userInfo = JsonSerializer.Deserialize<FacebookUserDTO>(userInfoResponse);
                //

                var info = new UserLoginInfo("FACEBOOK", validation.Data.UserId, "FACEBOOK");

                A.AppUser user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

                return await CreateUserExternalAsync(user,userInfo.Email,userInfo.Name,info,accessTokenLifeTime);
            }
            throw new Exception("Invalid external authentication.");
        }

        public async Task<Token> GoogleLoginAsync(string idToken, int accessTokenLifeTime)
        {
            // user validation ve info
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { _configuration["ExternalLoginSettings:Google:GoogleClientId"] }
            };
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
            //

            var info = new UserLoginInfo("GOOGLE", payload.Subject, "GOOGLE");

            A.AppUser user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            return await CreateUserExternalAsync(user,payload.Email,payload.Name,info,accessTokenLifeTime);
        }

        public async Task<Token> LoginAsync(string usernameOrEmail, string password, int accessTokenLifeTime)
        {
            A.AppUser user = await _userManager.FindByNameAsync(usernameOrEmail);
            if (user == null)
                user = await _userManager.FindByEmailAsync(usernameOrEmail);
            if (user == null)
                throw new NotFoundUserException();
            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, password, true);
            if (result.Succeeded)
            {
                Token token = _tokenHandler.CreateAccessToken(accessTokenLifeTime, user);
                await _userService.UpdateRefreshTokenAsync(token.RefreshToken, user, token.Expiration, 1 * 24 * 60 * 60);

                return token;
            }
            else
                throw new AuthenticationErrorException();
        }

        public async Task<Token> RefreshTokenLoginAsync(string refreshToken)
        {
            AppUser? user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
            if (user != null && user?.RefreshTokenEndDate > DateTime.UtcNow)
            {
                Token token = _tokenHandler.CreateAccessToken(int.Parse(_configuration["TokenLifeTime"]), user);
                //Token token = _tokenHandler.CreateAccessToken(15);
                await _userService.UpdateRefreshTokenAsync(token.RefreshToken, user, token.Expiration, 1 * 24 * 60 * 60);
                //await _userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiration, 15);
                return token;
;            }
            else throw new NotFoundUserException();
        }

        public async Task PasswordResetAsync(string email)
        {
            AppUser user =  await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                resetToken = resetToken.UrlEncode();
                await _mailService.SendPasswordResetMailAsync(email, user.Id, resetToken);
            }
        }

        public async Task<bool> VerifyResetTokenAsync(string resetToken, string userId)
        {
            AppUser user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                resetToken = resetToken.UrlDecode();
                return await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider,"ResetPassword",resetToken);
            }
            return false;
        }
    }
}
