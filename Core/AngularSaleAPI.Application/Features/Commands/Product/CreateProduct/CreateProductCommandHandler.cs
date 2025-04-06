using AngularSaleAPI.Application.Abstractions.Hubs;
using AngularSaleAPI.Application.Abstractions.Services.ProductServices;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularSaleAPI.Application.Features.Commands.Product.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommandRequest, CreateProductCommandResponse>
    {
        private readonly IProductService _productService;
        private readonly IProductHubService _productHubService;

        public CreateProductCommandHandler(IProductService productService, IProductHubService productHubService)
        {
            _productService = productService;
            _productHubService = productHubService;
        }

        public async Task<CreateProductCommandResponse> Handle(CreateProductCommandRequest request, CancellationToken cancellationToken)
        {
            await _productService.CreateProductAsync(new()
            {
                Name = request.Name,
                Price = request.Price,
                Stock = request.Stock,
                Brand = request.Brand,
                Description = request.Description
            });
            await _productHubService.ProductAddedMessageAsync($"Added product named {request.Name}");
            return new();
        }
    }
}
