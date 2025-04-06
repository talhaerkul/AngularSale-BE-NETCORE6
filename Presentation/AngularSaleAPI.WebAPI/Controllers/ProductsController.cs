using AngularSaleAPI.Application.Abstractions.Services.ProductServices;
using AngularSaleAPI.Application.Abstractions.Storage;
using AngularSaleAPI.Application.Consts;
using AngularSaleAPI.Application.CustomAttributes;
using AngularSaleAPI.Application.Enums;
using AngularSaleAPI.Application.Features.Commands.Product.AddProductToCategories;
using AngularSaleAPI.Application.Features.Commands.Product.CreateCategory;
using AngularSaleAPI.Application.Features.Commands.Product.CreateProduct;
using AngularSaleAPI.Application.Features.Commands.Product.RemoveProduct;
using AngularSaleAPI.Application.Features.Commands.Product.UpdateProduct;
using AngularSaleAPI.Application.Features.Commands.Product.UpdateProductStockWithQRCode;
using AngularSaleAPI.Application.Features.Commands.ProductImageFile.ChangeShowcaseImage;
using AngularSaleAPI.Application.Features.Commands.ProductImageFile.RemoveProductImage;
using AngularSaleAPI.Application.Features.Commands.ProductImageFile.UploadProductImage;
using AngularSaleAPI.Application.Features.Queries.Product.GetAllProduct;
using AngularSaleAPI.Application.Features.Queries.Product.GetByBrandProduct;
using AngularSaleAPI.Application.Features.Queries.Product.GetByCategoryProduct;
using AngularSaleAPI.Application.Features.Queries.Product.GetByIdProduct;
using AngularSaleAPI.Application.Features.Queries.Product.GetBySalerProduct;
using AngularSaleAPI.Application.Features.Queries.Product.GetCategories;
using AngularSaleAPI.Application.Features.Queries.Product.GetCategoriesByProductId;
using AngularSaleAPI.Application.Features.Queries.ProductImageFile.GetProductImages;
using Azure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AngularSaleAPI.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
 
    public class ProductsController : ControllerBase
    {
        readonly IMediator _mediator;
        readonly IProductService _productService;

        public ProductsController(IMediator mediator, IProductService productService)
        {
            _mediator = mediator;
            _productService = productService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetProducts([FromQuery] GetAllProductQueryRequest getAllProductQueryRequest)
        {
            GetAllProductQueryResponse response = await _mediator.Send(getAllProductQueryRequest);
            return Ok(response);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetCategories([FromQuery] GetCategoriesQueryRequest getCategoriesQueryRequest)
        {
            GetCategoriesQueryResponse response = await _mediator.Send(getCategoriesQueryRequest);
            return Ok(response);
        }
        [HttpGet("[action]/{Id}")]
        public async Task<IActionResult> GetCategoriesByProductId([FromRoute] GetCategoriesByProductIdQueryRequest getCategoriesByProductIdQueryRequest)
        {
            GetCategoriesByProductIdQueryResponse response = await _mediator.Send(getCategoriesByProductIdQueryRequest);
            return Ok(response);
        }

        [HttpGet("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Reading, Definition = "Get Product List")]
        public async Task<IActionResult> GetProductList([FromQuery] GetAllProductQueryRequest getAllProductQueryRequest)
        {
            GetAllProductQueryResponse response = await _mediator.Send(getAllProductQueryRequest);
            return Ok(response);
        }

        [HttpGet("[action]/{Id}")]
        public async Task<IActionResult> GetProductById([FromRoute] GetByIdProductQueryRequest getByIdProductQueryRequest)
        {
            GetByIdProductQueryResponse response = await _mediator.Send(getByIdProductQueryRequest);
            return Ok(response);
        }
        [HttpGet("[action]/{Category}")]
        public async Task<IActionResult> GetProductsByCategory([FromRoute, FromQuery] GetByCategoryProductQueryRequest getByCategoryProductQueryRequest)
        {
            GetByCategoryProductQueryResponse response = await _mediator.Send(getByCategoryProductQueryRequest);
            return Ok(response);
        }
        [HttpGet("[action]/{Brand}")]
        public async Task<IActionResult> GetProductsByBrand([FromRoute, FromQuery] GetByBrandProductQueryRequest getByBrandProductQueryRequest)
        {
            GetByBrandProductQueryResponse response = await _mediator.Send(getByBrandProductQueryRequest);
            return Ok(response);
        }
        [HttpGet("[action]/{Saler}")]
        public async Task<IActionResult> GetProductsBySaler([FromRoute, FromQuery] GetBySalerProductQueryRequest getBySalerProductQueryRequest)
        {
            GetBySalerProductQueryResponse response = await _mediator.Send(getBySalerProductQueryRequest);
            return Ok(response);
        }
        [HttpGet("[action]/{Saler}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Reading, Definition = "Get Showcase By Saler")]
        public async Task<IActionResult> GetShowcaseBySaler([FromRoute] GetBySalerProductQueryRequest getBySalerProductQueryRequest)
        {
            GetBySalerProductQueryResponse response = await _mediator.Send(getBySalerProductQueryRequest);
            return Ok(response);
        }

        [HttpGet("[action]/{Id}")]
        public async Task<IActionResult> GetQRCodeToProductById([FromRoute] string id)
        {
            var data = await _productService.QRCodeToProductAsync(id);
            return File(data,"image/png");
        }

        [HttpPost("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Updating, Definition = "Update Product Stock With QRCode")]
        public async Task<IActionResult> UpdateProductStockWithQRCode([FromBody]UpdateProductStockWithQRCodeCommandRequest updateProductStockWithQRCodeCommandRequest)
        {
            UpdateProductStockWithQRCodeCommandResponse response = await _mediator.Send(updateProductStockWithQRCodeCommandRequest);
            return Ok(response);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Writing, Definition = "Create Product")]
        public async Task<IActionResult> Post([FromBody]CreateProductCommandRequest createProductCommandRequest)
        {
            CreateProductCommandResponse response = await _mediator.Send(createProductCommandRequest);
            return StatusCode((int)HttpStatusCode.Created);
        }
        [HttpPost("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Writing, Definition = "Create Category")]
        public async Task<IActionResult> CreateCategory([FromBody]CreateCategoryCommandRequest createCategoryCommandRequest)
        {
            CreateCategoryCommandResponse response = await _mediator.Send(createCategoryCommandRequest);
            return Ok(response);
        }

        [HttpPost("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Writing, Definition = "Add Product To Categories")]
        public async Task<IActionResult> AddProductToCategories([FromBody] AddProductToCategoriesCommandRequest addProductToCategoriesCommandRequest)
        {
            AddProductToCategoriesCommandResponse response = await _mediator.Send(addProductToCategoriesCommandRequest);
            return Ok(response);
        }

        [HttpPut("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Updating, Definition = "Update Product")]
        public async Task<IActionResult> UpdateProduct([FromBody]UpdateProductCommandRequest updateProductCommandRequest)
        {
            UpdateProductCommandResponse response = await _mediator.Send(updateProductCommandRequest);
            return Ok(response);
        }

        [HttpDelete("{Id}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Deleting, Definition = "Delete Product")]
        public async Task<IActionResult> Delete([FromRoute]RemoveProductCommandRequest removeProductCommandRequest)
        {
            RemoveProductCommandResponse response = await _mediator.Send(removeProductCommandRequest);
            return Ok(response);
        }

        [HttpPost("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Writing, Definition = "Upload Product File")]
        public async Task<IActionResult> Upload([FromQuery]UploadProductImageCommandRequest uploadProductImageCommandRequest)
        {
            uploadProductImageCommandRequest.Files = Request.Form.Files;
            await _mediator.Send(uploadProductImageCommandRequest);
            
            return Ok();
        }

        [HttpGet("[action]/{Id}")]
        public async Task<IActionResult> GetProductImages([FromRoute]GetProductImagesQueryRequest getProductImagesQueryRequest) 
        {
            List<GetProductImagesQueryResponse> response = await _mediator.Send(getProductImagesQueryRequest);
            return Ok(response);
        }
        
        [HttpDelete("[action]/{Id}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Deleting, Definition = "Delete Product Image")]
        public async Task<IActionResult> DeleteProductImage([FromRoute, FromQuery]RemoveProductImageCommandRequest removeProductImageCommandRequest)
        {
            RemoveProductImageCommandResponse response = await _mediator.Send(removeProductImageCommandRequest);
            return Ok(response);
        }

        [HttpGet("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Updating, Definition = "Change Showcase Image")]
        public async Task<IActionResult> ChangeShowcaseImage([FromQuery]ChangeShowcaseImageCommandRequest changeShowcaseImageCommandRequest)
        {
            ChangeShowcaseImageCommandResponse response = await _mediator.Send(changeShowcaseImageCommandRequest);
            return Ok(response);
        }

        }
}
