using AngularSaleAPI.Application.Abstractions.Services.ProductServices;
using AngularSaleAPI.Application.DTOs.Product;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularSaleAPI.Application.Features.Queries.Product.GetBySalerProduct
{
    public class GetBySalerProductQueryHandler : IRequestHandler<GetBySalerProductQueryRequest, GetBySalerProductQueryResponse>
    {
        readonly IProductService _productService;

        public GetBySalerProductQueryHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<GetBySalerProductQueryResponse> Handle(GetBySalerProductQueryRequest request, CancellationToken cancellationToken)
        {
            
            ListProductDTO datas = await _productService.GetProductBySalerAsync(request.Page, request.Size, request.Saler);

            return new()
            {
                Products = datas.Products,
                TotalCount = datas.TotalCount
            };
        }
    }
}
