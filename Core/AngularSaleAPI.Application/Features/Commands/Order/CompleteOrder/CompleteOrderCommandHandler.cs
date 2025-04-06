using AngularSaleAPI.Application.Abstractions.Services.UserServices;
using AngularSaleAPI.Application.DTOs.Order;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularSaleAPI.Application.Features.Commands.Order.CompleteOrder
{
    public class CompleteOrderCommandHandler : IRequestHandler<CompleteOrderCommandRequest, CompleteOrderCommandResponse>
    {
        readonly IOrderService _orderService;
        readonly IMailService _mailService;

        public CompleteOrderCommandHandler(IOrderService orderService, IMailService mailService)
        {
            _orderService = orderService;
            _mailService = mailService;
        }

        public async Task<CompleteOrderCommandResponse> Handle(CompleteOrderCommandRequest request, CancellationToken cancellationToken)
        {
            (bool succeded, CompletedOrderResponseDTO dto) = await _orderService.CompleteOrderAsync(request.Id);
            if (succeded)
                await _mailService.SendCompletedOrderMailAsync(dto.Email, dto.UserName, dto.OrderCode,dto.OrderDate);
            return new();
        }
    }
}
