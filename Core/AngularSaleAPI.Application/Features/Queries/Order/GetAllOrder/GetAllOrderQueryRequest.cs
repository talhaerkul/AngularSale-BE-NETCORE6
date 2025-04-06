using AngularSaleAPI.Application.RequestParameters;
using MediatR;

namespace AngularSaleAPI.Application.Features.Queries.Order.GetAllOrder
{
    public class GetAllOrderQueryRequest : Pagination, IRequest<GetAllOrderQueryResponse>
    {
    }
}