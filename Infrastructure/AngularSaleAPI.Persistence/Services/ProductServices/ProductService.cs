using AngularSaleAPI.Application.Abstractions.Services.ProductServices;
using AngularSaleAPI.Application.DTOs.Product;
using AngularSaleAPI.Application.Exceptions;
using AngularSaleAPI.Application.Repositories;
using AngularSaleAPI.Application.Repositories.Category;
using AngularSaleAPI.Domain.Entities;
using AngularSaleAPI.Persistence.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web.Http;

namespace AngularSaleAPI.Persistence.Services.ProductServices
{
    public class ProductService : IProductService
    {
        readonly IProductWriteRepository _productWriteRepository;
        readonly IProductReadRepository _productReadRepository;
        readonly ICategoryWriteRepository _categoryWriteRepository;
        readonly ICategoryReadRepository _categoryReadRepository;
        readonly IQRCodeService _qrCodeService;
        readonly IHttpContextAccessor _httpContextAccessor;
        readonly IConfiguration _configuration;

        public ProductService(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository, IQRCodeService qrCodeService, ICategoryWriteRepository categoryWriteRepository, ICategoryReadRepository categoryReadRepository, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
            _qrCodeService = qrCodeService;
            _categoryWriteRepository = categoryWriteRepository;
            _categoryReadRepository = categoryReadRepository;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }


        public async Task CreateProductAsync(CreateProductRequestDTO product)
        {
            var username = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            await _productWriteRepository.AddAsync(new()
            {
                Name = product.Name,
                Brand = product.Brand,
                Price = product.Price,
                Stock = product.Stock,
                Description = product.Description,
                SalerUsername = username
            });
            await _productWriteRepository.SaveAsync();
        }
        public async Task CreateCategoryAsync(string name)
        {
            var control = await _categoryReadRepository.GetAll().Where(c => c.Name == name).FirstOrDefaultAsync();
            if (control != null)
            {
                throw new Exception("Category exists!");
            }
            await _categoryWriteRepository.AddAsync(new()
            {
                Name = name
            });
            await _categoryWriteRepository.SaveAsync();
        }
        public async Task AddProductToCategories(string id,CategoryDTO[] dto)
        {
            Product? product = await _productReadRepository.Table.Include(p => p.Categories).FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));
            if (product == null)
                throw new NotFoundProductException();
            product.Categories.Clear();
            foreach (var category in dto)
            {
                Category _category = await _categoryReadRepository.GetWhere(c => c.Name == category.Name).FirstAsync();
                product.Categories.Add(_category);
            }
            await _productWriteRepository.SaveAsync();
        }

        public async Task<ListProductDTO> GetAllProductAsync(int page, int size)
        {
            var totalCount = _productReadRepository.GetAll(false).Count();
            var products = _productReadRepository.GetAll(false)
                .Skip(size * page)
                .Take(size)
                .Include(p => p.ProductImageFiles)
                .Include(p => p.Categories)
                .Select(p => new { p.Id, p.Name, p.Brand, p.Price, p.Stock, p.CreatedDate, p.UpdatedDate, p.ProductImageFiles, p.Categories, p.SalerUsername }).ToList();
            return new()
            {
                Products = products,
                TotalCount  = totalCount
            };
        }
        public async Task<object> GetCategories()
        {
            var categories = _categoryReadRepository.GetAll(false).Select(c => new {c.Name}).ToList();

            return categories;
        }
        public async Task<object> GetCategoriesByProductId(string id)
        {
            var product = _productReadRepository.Table
                .Include(p => p.Categories).Where(c => c.Id == Guid.Parse(id));
            var categories = product.Select(c => new { c.Categories }).ToList();
            var _categories = new List<CategoryDTO>();
            foreach (var item in categories)
            {
                _categories = item.Categories.Select(c => new CategoryDTO{ Name = c.Name }).ToList();
            }

            return _categories;
        }

        public async Task<ListProductDTO> GetProductByBrandAsync(int page, int size, string brand)
        {
            var totalCount = _productReadRepository.GetAll(false).Count();
            var products = _productReadRepository.GetAll(false)
                .Skip(size * page)
                .Take(size)
                .Include(p => p.ProductImageFiles)
                .Include(p => p.Categories)
                .Where(p => p.Brand == brand)
                .Select(p => new { p.Id, p.Name, p.Brand, p.Price, p.Stock, p.CreatedDate, p.UpdatedDate, p.ProductImageFiles, p.Categories, p.SalerUsername }).ToList();

            return new()
            {
                Products = products,
                TotalCount = totalCount
            };
        }
        public async Task<ListProductDTO> GetProductBySalerAsync(int page, int size, string saler)
        {
            var username = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            var admin = _configuration["AuthorizeUsers:Admins:Talha"];
            if (saler != username && username != admin)
            {
                //todo unauthorized exp yolla
            }
            var totalCount = _productReadRepository.GetAll(false).Count();
            var products = _productReadRepository.GetAll(false)
                .Skip(size * page)
                .Take(size)
                .Include(p => p.ProductImageFiles)
                .Include(p => p.Categories)
                .Where(p => p.SalerUsername == saler)
                .Select(p => new { p.Id, p.Name, p.Brand, p.Price, p.Stock, p.CreatedDate, p.UpdatedDate, p.ProductImageFiles, p.Categories, p.SalerUsername }).ToList();

            return new()
            {
                Products = products,
                TotalCount = totalCount
            };
        }
        public async Task<ListProductDTO> GetProductByCategoryAsync(int page, int size, string category)
        {
            var _category = _categoryReadRepository.GetAll(false).Where(c => c.Name == category).First();
            var totalCount = _productReadRepository.GetAll(false).Count();
            var products = _productReadRepository.GetAll(false)
                .Skip(size * page)
                .Take(size)
                .Include(p => p.ProductImageFiles)
                .Include(p => p.Categories)
                .Where(p => p.Categories.Contains(_category))
                .Select(p => new { p.Id, p.Name, p.Brand, p.Price, p.Stock, p.CreatedDate, p.UpdatedDate, p.ProductImageFiles, p.Categories, p.SalerUsername }).ToList();

            return new()
            {
                Products = products,
                TotalCount = totalCount
            };
        }

        public async Task<ProductDTO> GetProductByIdAsync(string id)
        {
            var product = await _productReadRepository.GetAll(false).FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));
            if (product == null)
                throw new NotFoundProductException();
            return new ProductDTO()
            {
                Name = product.Name,
                Brand = product.Brand,
                Description = product.Description,
                SalerUsername = product.SalerUsername,
                Price = product.Price,
                Stock = product.Stock,

            };
        }
        

        public async Task RemoveProductAsync(string id)
        {
            await _productWriteRepository.RemoveAsync(id);
            await _productWriteRepository.SaveAsync();
        }

        public async Task UpdateProductAsync(UpdateProductRequestDTO productDTO)
        {
            Product product = await _productReadRepository.GetByIdAsync(productDTO.Id);
            product.Name = productDTO.Name;
            product.Brand = productDTO.Brand;
            product.Description = productDTO.Description;
            product.Price = productDTO.Price;
            product.Stock = productDTO.Stock;
            await _productWriteRepository.SaveAsync();
        }

        public async Task<byte[]> QRCodeToProductAsync(string id)
        {
            Product product = await _productReadRepository.GetByIdAsync(id);
            if (product == null)
                throw new NotFoundProductException();

            var plainObject = new
            {
                product.Id,
                product.Name, 
                product.Brand,
                product.Price,
                product.Stock, 
                product.CreatedDate,
                product.Categories
            };
            string plainText = JsonSerializer.Serialize(plainObject);
            return _qrCodeService.GenerateQRCode(plainText);
        }

        public async Task UpdateProductStockWithQRCode(string id, int stock)
        {
            Product product = await _productReadRepository.GetByIdAsync(id);
            if(product == null)
                throw new NotFoundProductException();

            product.Stock = stock;
            await _productWriteRepository.SaveAsync();
        }

        
    }
}
