using AngularSaleAPI.Application.DTOs.Product;
using AngularSaleAPI.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AngularSaleAPI.Application.Features.Commands.Product.AddProductToCategories
{
    public class AddProductToCategoriesCommandRequest : IRequest<AddProductToCategoriesCommandResponse>
    {
        public string Id { get; set; }
        public CategoryDTO[] Categories { get; set; }
    }
}