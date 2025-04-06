using AngularSaleAPI.Application.Consts;
using AngularSaleAPI.Application.CustomAttributes;
using AngularSaleAPI.Application.Enums;
using AngularSaleAPI.Application.Features.Commands.AuthorizationEndpoint.AssignRoleEndpoint;
using AngularSaleAPI.Application.Features.Queries.AuthorizationEndpoint.GetRolesToEndpoint;
using AngularSaleAPI.Application.Features.Queries.Order.GetAllOrder;
using AngularSaleAPI.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AngularSaleAPI.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthorizationEndpointsController : ControllerBase
    {
        readonly IMediator _mediator;

        public AuthorizationEndpointsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("[action]")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.AuthorizationEndpoints, ActionType = ActionType.Reading, Definition = "Get Roles To Endpoint")]
        public async Task<IActionResult> GetRolesToEndpoint(GetRolesToEndpointQueryRequest getRolesToEndpointQueryrequest)
        {
            GetRolesToEndpointQueryResponse response = await _mediator.Send(getRolesToEndpointQueryrequest);
            return Ok(response);
        }

        [HttpPost("[action]")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.AuthorizationEndpoints, ActionType = ActionType.Writing, Definition = "Assign Role Endpoint")]
        public async Task<IActionResult> AssignRoleEndpoint(AssignRoleEndpointCommandRequest assignRoleEndpointCommandRequest)
        {
            assignRoleEndpointCommandRequest.Type = typeof(Program);
            AssignRoleEndpointCommandResponse response = await _mediator.Send(assignRoleEndpointCommandRequest);
            return Ok(response);
        }
        
    }
}
