using AngularSaleAPI.Application.RequestParameters;
using MediatR;

namespace AngularSaleAPI.Application.Features.Queries.Role.GetRoles
{
    public class GetRolesQueryRequest : Pagination, IRequest<GetRolesQueryResponse>
    {
    }
}