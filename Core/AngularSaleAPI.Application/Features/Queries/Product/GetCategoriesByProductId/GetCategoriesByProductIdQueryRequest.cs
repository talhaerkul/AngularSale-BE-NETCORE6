using MediatR;

namespace AngularSaleAPI.Application.Features.Queries.Product.GetCategoriesByProductId
{
    public class GetCategoriesByProductIdQueryRequest : IRequest<GetCategoriesByProductIdQueryResponse>
    {
        public string Id { get; set; }
    }
}