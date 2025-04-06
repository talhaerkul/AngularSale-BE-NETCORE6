using AngularSaleAPI.Application.Abstractions.Services.ProductServices;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularSaleAPI.Application.Features.Commands.Product.UpdateProductStockWithQRCode
{
    public class UpdateProductStockWithQRCodeCommandHandler : IRequestHandler<UpdateProductStockWithQRCodeCommandRequest, UpdateProductStockWithQRCodeCommandResponse>
    {
        readonly IProductService _productService;

        public UpdateProductStockWithQRCodeCommandHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<UpdateProductStockWithQRCodeCommandResponse> Handle(UpdateProductStockWithQRCodeCommandRequest request, CancellationToken cancellationToken)
        {
            await _productService.UpdateProductStockWithQRCode(request.Id, request.Stock);
            return new();
        }
    }
}
