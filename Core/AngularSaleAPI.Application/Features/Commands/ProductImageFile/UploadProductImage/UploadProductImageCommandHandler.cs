using AngularSaleAPI.Application.Abstractions.Storage;
using AngularSaleAPI.Application.Repositories.ProductImageFile;
using AngularSaleAPI.Application.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using P = AngularSaleAPI.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace AngularSaleAPI.Application.Features.Commands.ProductImageFile.UploadProductImage
{
    public class UploadProductImageCommandHandler : IRequestHandler<UploadProductImageCommandRequest, UploadProductImageCommandResponse>
    {
        private readonly IStorageService _storageService;
        private readonly IProductReadRepository _productReadRepository;
        private readonly IProductImageFileWriteRepository _productImageFileWriteRepository;

        public UploadProductImageCommandHandler(IStorageService storageService, IProductImageFileWriteRepository productImageFileWriteRepository, IProductReadRepository productReadRepository)
        {
            _storageService = storageService;
            _productImageFileWriteRepository = productImageFileWriteRepository;
            _productReadRepository = productReadRepository;
        }

        public async Task<UploadProductImageCommandResponse> Handle(UploadProductImageCommandRequest request, CancellationToken cancellationToken)
        {
            var datas = await _storageService.UploadAsync("photo-images", request.Files);

            P.Product product = await _productReadRepository.GetByIdAsync(request.Id);

            await _productImageFileWriteRepository.AddRangeAsync(datas.Select(d => new P.ProductImageFile()
            {
                FileName = d.fileName,
                Path = d.pathOrContainer,
                Storage = _storageService.StorageName,
                Products = new List<P.Product>() { product }
            }).ToList());
            await _productImageFileWriteRepository.SaveAsync();

            return new();
        }
    }
}
