using AngularSaleAPI.Application.Abstractions.Services.ProductServices;
using AngularSaleAPI.Application.DTOs.Product;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularSaleAPI.Application.Features.Queries.Product.GetByCategoryProduct
{
    public class GetByCategoryProductQueryHandler : IRequestHandler<GetByCategoryProductQueryRequest, GetByCategoryProductQueryResponse>
    {
        readonly IProductService _productService;

        public GetByCategoryProductQueryHandler(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<GetByCategoryProductQueryResponse> Handle(GetByCategoryProductQueryRequest request, CancellationToken cancellationToken)
        {
            ListProductDTO datas = await _productService.GetProductByCategoryAsync(request.Page, request.Size, request.Category);

            return new()
            {
                Products = datas.Products,
                TotalCount = datas.TotalCount
            };
        }
    }
}
