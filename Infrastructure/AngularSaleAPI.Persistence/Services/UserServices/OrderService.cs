using AngularSaleAPI.Application.Abstractions.Services.UserServices;
using AngularSaleAPI.Application.DTOs.Order;
using AngularSaleAPI.Application.Repositories;
using AngularSaleAPI.Application.Repositories.CompletedOrder;
using AngularSaleAPI.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularSaleAPI.Persistence.Services.UserServices
{
    public class OrderService : IOrderService
    {
        readonly IOrderWriteRepository _orderWriteRepository;
        readonly IOrderReadRepository _orderReadRepository;
        readonly ICompletedOrderWriteRepository _completedOrderWriteRepository;
        readonly ICompletedOrderReadRepository _completedOrderReadRepository;

        public OrderService(IOrderWriteRepository orderWriteRepository, IOrderReadRepository orderReadRepository, ICompletedOrderWriteRepository completedOrderWriteRepository, ICompletedOrderReadRepository completedOrderReadRepository)
        {
            _orderWriteRepository = orderWriteRepository;
            _orderReadRepository = orderReadRepository;
            _completedOrderWriteRepository = completedOrderWriteRepository;
            _completedOrderReadRepository = completedOrderReadRepository;
        }
        // TODO: Siparişi tamamlanan order ların adetleri stocktan düşülsün
        // TODO: Mevcut stok miktarından daha fazla ürün girişi yapıldıysa, müşteriye uyarı yapılsın. Herhangi bir işlem yapılmasın
        // TODO: 0'dan daha düşük sipariş girilmesini engelle
        // TODO: 0 adet stock bilgisine sahip ürün Ana Sayfada => 'Ürün stokta yoktur' mesajı ile gelsin ve bu ürün sepete eklenmesin. Butonu pasif yapılabilir.

        public async Task CreateOrderAsync(CreateOrderRequestDTO order)
        {
            var orderCode = new Random().NextDouble().ToString()[3..]; //3. indexten sonrasını al (nextdouble 0.98679674 gibi bir sayı)
           
            await _orderWriteRepository.AddAsync(new()
            {
                Address = order.Address,
                Description = order.Description,
                Id = Guid.Parse(order.BasketId),
                OrderCode = orderCode
            });
            await _orderWriteRepository.SaveAsync();
        }

        public async Task<ListOrderResponseDTO> GetAllOrderAsync(int page, int size)
        {
            var query = _orderReadRepository.Table
                .Include(o => o.Basket) //orderın basketını ekle
                .ThenInclude(b => b.User) // basketın userını ekle
                    .Include(o => o.Basket) // orderın basketını ekle
                    .ThenInclude(b => b.BasketItems) // basketın basket itemını ekle
                    .ThenInclude(bi => bi.Product); // basket itemın productını ekle

            var data = query
                       .Skip(size * page)
                       .Take(size);

            var data2 = from order in data
            join CompletedOrder in _completedOrderReadRepository.Table
            on order.Id equals CompletedOrder.OrderId into co
            from _co in co.DefaultIfEmpty()
            select new
            {
                Id = order.Id,
                CreatedDate = order.CreatedDate,
                OrderCode =order.OrderCode,
                Basket = order.Basket,
                Completed = _co != null ? true : false
            };

            return new()
            {
                TotalCount = query.Count(),
                Orders = await data2.Select(o => new
                {
                    Id = o.Id,
                    CreatedDate = o.CreatedDate,
                    OrderCode = o.OrderCode,
                    TotalPrice = o.Basket.BasketItems.Sum(bi => bi.Product.Price * bi.Quantity),
                    UserName = o.Basket.User.UserName,
                    o.Completed
                }).ToListAsync()
            };
        }

        public async Task<OrderResponseDTO> GetOrderById(string id)
        {
            var data = _orderReadRepository.Table
                .Include(o => o.Basket)
                .ThenInclude(b => b.BasketItems)
                .ThenInclude(bi => bi.Product);

            var data2 = await (from order in data
                        join completedOrder in _completedOrderReadRepository.Table
                        on order.Id equals completedOrder.OrderId into co
                        from _co in co.DefaultIfEmpty()
                        select new
                        {
                            Id = order.Id,
                            CreatedDate = order.CreatedDate,
                            OrderCode = order.OrderCode,
                            Basket = order.Basket,
                            Completed = _co != null ? true : false,
                            Address = order.Address,
                            Description = order.Description,
                        }).FirstOrDefaultAsync(p => p.Id == Guid.Parse(id)); ;
            
            return new OrderResponseDTO()
            {
                Id = data2.Id.ToString(),
                BasketItems = data2.Basket.BasketItems.Select(bi => new
                {
                    bi.Product.Name,
                    bi.Product.Price,
                    bi.Quantity,
                }),
                Address = data2.Address,
                CreatedDate = data2.CreatedDate,
                OrderCode = data2.OrderCode,
                Description = data2.Description,
                Completed = data2.Completed
            };
        }
        public async Task<(bool,CompletedOrderResponseDTO)> CompleteOrderAsync(string id)
        {
            Order? order = await _orderReadRepository.Table
                .Include(o => o.Basket)
                .ThenInclude(b => b.User)
                .FirstOrDefaultAsync(o => o.Id == Guid.Parse(id));
            

            if(order != null)
            {
                await _completedOrderWriteRepository.AddAsync(new() { OrderId = order.Id });
                return (await _completedOrderWriteRepository.SaveAsync() > 0, new()
                {
                    OrderCode = order.OrderCode,
                    OrderDate = order.CreatedDate,
                    UserName = order.Basket.User.NameSurname,
                    Email = order.Basket.User.Email
                });
            }
            return (false,null);
        }
    }
}
