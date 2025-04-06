using AngularSaleAPI.Application.DTOs.Basket;
using AngularSaleAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularSaleAPI.Application.Abstractions.Services.UserServices
{
    public interface IBasketService
    {
        public Task<List<BasketItemDTO>> GetBasketItemsAsync();
        public Task AddItemToBasketAsync(CreateBasketItemRequestDTO basketItem);
        public Task UpdateQuantityAsync(UpdateBasketItemRequestDTO basketItem);
        public Task RemoveBasketItemAsync(string basketItemId);
        public Basket? GetUserActiveBasket { get; }
    }
}
