using AngularSaleAPI.Application.RequestParameters;
using MediatR;

namespace AngularSaleAPI.Application.Features.Queries.Product.GetByCategoryProduct
{
    public class GetByCategoryProductQueryRequest : Pagination, IRequest<GetByCategoryProductQueryResponse>
    {
        public string Category { get; set; }

    }
}