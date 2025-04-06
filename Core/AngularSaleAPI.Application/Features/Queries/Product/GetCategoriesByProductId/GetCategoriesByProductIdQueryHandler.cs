using AngularSaleAPI.Application.Abstractions.Services.ProductServices;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularSaleAPI.Application.Features.Queries.Product.GetCategoriesByProductId
{
    public class GetCategoriesByProductIdQueryHandler : IRequestHandler<GetCategoriesByProductIdQueryRequest, GetCategoriesByProductIdQueryResponse>
    {
        readonly IProductService _productService;

        public GetCategoriesByProductIdQueryHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<GetCategoriesByProductIdQueryResponse> Handle(GetCategoriesByProductIdQueryRequest request, CancellationToken cancellationToken)
        {
            var categories = await _productService.GetCategoriesByProductId(request.Id);
            return new()
            {
                Categories = categories
            };
        }
    }
}
