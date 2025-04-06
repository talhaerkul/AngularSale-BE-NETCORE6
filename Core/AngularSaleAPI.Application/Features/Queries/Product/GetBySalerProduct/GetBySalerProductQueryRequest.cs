using AngularSaleAPI.Application.RequestParameters;
using MediatR;

namespace AngularSaleAPI.Application.Features.Queries.Product.GetBySalerProduct
{
    public class GetBySalerProductQueryRequest : Pagination, IRequest<GetBySalerProductQueryResponse>
    {
        public string Saler { get; set; }
    }
}