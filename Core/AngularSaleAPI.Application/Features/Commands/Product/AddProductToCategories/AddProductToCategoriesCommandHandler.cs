using AngularSaleAPI.Application.Abstractions.Services.ProductServices;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularSaleAPI.Application.Features.Commands.Product.AddProductToCategories
{
    public class AddProductToCategoriesCommandHandler : IRequestHandler<AddProductToCategoriesCommandRequest, AddProductToCategoriesCommandResponse>
    {
        readonly IProductService _productService;

        public AddProductToCategoriesCommandHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<AddProductToCategoriesCommandResponse> Handle(AddProductToCategoriesCommandRequest request, CancellationToken cancellationToken)
        {
            await _productService.AddProductToCategories(request.Id, request.Categories);
            return new();
        }
    }
}
