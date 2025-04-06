using AngularSaleAPI.Application.Repositories.ProductImageFile;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularSaleAPI.Application.Features.Commands.ProductImageFile.ChangeShowcaseImage
{
    public class ChangeShowcaseImageCommandHandler : IRequestHandler<ChangeShowcaseImageCommandRequest, ChangeShowcaseImageCommandResponse>
    {
        readonly IProductImageFileWriteRepository _productImageFileWriteRepository;

        public ChangeShowcaseImageCommandHandler(IProductImageFileWriteRepository productImageFileWriteRepository)
        {
            _productImageFileWriteRepository = productImageFileWriteRepository;
        }

        public async Task<ChangeShowcaseImageCommandResponse> Handle(ChangeShowcaseImageCommandRequest request, CancellationToken cancellationToken)
        {
            var query = _productImageFileWriteRepository.Table
                .Include(productImageFile => productImageFile.Products)
                .SelectMany(productImageFile => productImageFile.Products, (productImageFile, Products) => new
                {
                    productImageFile,
                    Products
                });

            var data = await query.FirstOrDefaultAsync(x => x.Products.Id == Guid.Parse(request.ProductId) && x.productImageFile.Showcase);
            if (data != null)
                data.productImageFile.Showcase = false;

            var image = await query.FirstOrDefaultAsync(p => p.productImageFile.Id == Guid.Parse(request.ImageId));
            if (image != null)
                image.productImageFile.Showcase = true;

            await _productImageFileWriteRepository.SaveAsync();
            
            return new();
        }
    }
}
