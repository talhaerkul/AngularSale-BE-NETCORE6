using MediatR;

namespace AngularSaleAPI.Application.Features.Commands.AppUser.AssignRoleUser
{
    public class AssignRoleUserCommandRequest : IRequest<AssignRoleUserCommandResponse>
    {
        public string UserId { get; set; }
        public string[] Roles { get; set; }
    }

}