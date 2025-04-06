using AngularSaleAPI.Application.DTOs.Product;
using AngularSaleAPI.Application.Features.Commands.Product.CreateProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularSaleAPI.Application.Abstractions.Services.ProductServices
{
    public interface IProductService
    {
        Task<ListProductDTO> GetAllProductAsync(int page, int size);
        Task<ProductDTO> GetProductByIdAsync(string id);
        Task<ListProductDTO> GetProductByBrandAsync(int page, int size, string brand);
        Task<ListProductDTO> GetProductBySalerAsync(int page, int size, string saler);
        Task<ListProductDTO> GetProductByCategoryAsync(int page, int size, string category);
        Task CreateProductAsync(CreateProductRequestDTO product);
        Task CreateCategoryAsync(string name);
        Task AddProductToCategories(string id, CategoryDTO[] dto);
        Task<object> GetCategories();
        Task<object> GetCategoriesByProductId(string id);
        Task RemoveProductAsync(string id);
        Task UpdateProductAsync(UpdateProductRequestDTO product);
        Task UpdateProductStockWithQRCode(string id, int stock);
        Task<byte[]> QRCodeToProductAsync(string id);
        

    }
}
