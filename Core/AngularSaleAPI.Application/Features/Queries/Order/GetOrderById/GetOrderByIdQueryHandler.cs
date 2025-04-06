using AngularSaleAPI.Application.Abstractions.Services.UserServices;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularSaleAPI.Application.Features.Queries.Order.GetOrderById
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQueryRequest, GetOrderByIdQueryResponse>
    {
        readonly IOrderService _orderService;

        public GetOrderByIdQueryHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<GetOrderByIdQueryResponse> Handle(GetOrderByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var data = await _orderService.GetOrderById(request.Id);
            return new()
            {
                Id = data.Id,
                OrderCode = data.OrderCode,
                CreatedDate = data.CreatedDate,
                Address = data.Address,
                BasketItems = data.BasketItems,
                Description = data.Description,
                Completed = data.Completed, 
            };
        }
    }
}
