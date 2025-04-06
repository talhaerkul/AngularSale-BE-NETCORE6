using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AngularSaleAPI.Application.Features.Commands.Role.UpdateRole
{
    public class UpdateRoleCommandRequest : IRequest<UpdateRoleCommandResponse>
    {
        [FromRoute]
        public string Id { get; set; }
        [FromBody]
        public string Name { get; set; }
    }
}