using MediatR;

namespace AngularSaleAPI.Application.Features.Commands.AppUser.ResetPasswordCheck
{
    public class ResetPasswordCommandRequest : IRequest<ResetPasswordCommandResponse>
    {
        public string Email { get; set; }
    }
}