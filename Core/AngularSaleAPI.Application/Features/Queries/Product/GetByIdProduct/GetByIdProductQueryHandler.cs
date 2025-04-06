using AngularSaleAPI.Application.Abstractions.Services.ProductServices;
using AngularSaleAPI.Application.DTOs.Product;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using P = AngularSaleAPI.Domain.Entities;

namespace AngularSaleAPI.Application.Features.Queries.Product.GetByIdProduct
{
    public class GetByIdProductQueryHandler : IRequestHandler<GetByIdProductQueryRequest, GetByIdProductQueryResponse>
    {
        private readonly IProductService _productService;

        public GetByIdProductQueryHandler(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<GetByIdProductQueryResponse> Handle(GetByIdProductQueryRequest request, CancellationToken cancellationToken)
        {
            ProductDTO data = await _productService.GetProductByIdAsync(request.Id);
            return new()
            {
                Name = data.Name,
                Brand = data.Brand,
                Description = data.Description,
                SalerUsername = data.SalerUsername,
                Price = data.Price,
                Stock = data.Stock,
            };
        }
    }
}
