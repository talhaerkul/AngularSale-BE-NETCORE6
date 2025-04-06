using AngularSaleAPI.Application.RequestParameters;
using MediatR;

namespace AngularSaleAPI.Application.Features.Queries.Product.GetByBrandProduct
{
    public class GetByBrandProductQueryRequest : Pagination, IRequest<GetByBrandProductQueryResponse>
    {
        public string Brand { get; set; }
    }
}