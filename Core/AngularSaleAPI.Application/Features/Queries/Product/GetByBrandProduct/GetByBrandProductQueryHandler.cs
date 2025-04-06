using AngularSaleAPI.Application.Abstractions.Services.ProductServices;
using AngularSaleAPI.Application.DTOs.Product;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularSaleAPI.Application.Features.Queries.Product.GetByBrandProduct
{
    public class GetByBrandProductQueryHandler : IRequestHandler<GetByBrandProductQueryRequest, GetByBrandProductQueryResponse>
    {
        readonly IProductService _productService;

        public GetByBrandProductQueryHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<GetByBrandProductQueryResponse> Handle(GetByBrandProductQueryRequest request, CancellationToken cancellationToken)
        {
            
            ListProductDTO datas = await _productService.GetProductByBrandAsync(request.Page, request.Size, request.Brand);

            return new()
            {
                Products = datas.Products,
                TotalCount = datas.TotalCount
            };
        }
    }
}
