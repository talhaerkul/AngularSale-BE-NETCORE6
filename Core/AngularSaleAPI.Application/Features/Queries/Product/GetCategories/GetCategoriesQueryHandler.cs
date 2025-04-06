using AngularSaleAPI.Application.Abstractions.Services.ProductServices;
using AngularSaleAPI.Application.DTOs.Product;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularSaleAPI.Application.Features.Queries.Product.GetCategories
{
    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQueryRequest, GetCategoriesQueryResponse>
    {
        readonly IProductService _productService;

        public GetCategoriesQueryHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<GetCategoriesQueryResponse> Handle(GetCategoriesQueryRequest request, CancellationToken cancellationToken)
        {
            var categories = await _productService.GetCategories();
            return new()
            {
                Categories = categories
            };
        }
    }
}
