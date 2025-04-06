using AngularSaleAPI.Application.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularSaleAPI.Application.Abstractions.Services.UserServices
{
    public interface IOrderService
    {
        Task CreateOrderAsync(CreateOrderRequestDTO order);
        Task<ListOrderResponseDTO> GetAllOrderAsync(int page, int size);
        Task<OrderResponseDTO> GetOrderById(string id);
        Task<(bool, CompletedOrderResponseDTO)> CompleteOrderAsync(string id);
    }
}
