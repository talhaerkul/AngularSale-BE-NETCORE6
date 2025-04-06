using AngularSaleAPI.Application.Abstractions.Services.UserServices;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularSaleAPI.Application.Features.Queries.Basket.GetBasketItems
{
    public class GetBasketItemsQueryHandler : IRequestHandler<GetBasketItemsQueryRequest, List<GetBasketItemsQueryResponse>>
    {
        readonly IBasketService _basketService;

        public GetBasketItemsQueryHandler(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task<List<GetBasketItemsQueryResponse>> Handle(GetBasketItemsQueryRequest request, CancellationToken cancellationToken)
        {
            var basketItems = await _basketService.GetBasketItemsAsync();
            
            List<GetBasketItemsQueryResponse> response = basketItems.Select(b => new GetBasketItemsQueryResponse()
            {
                BasketItemId = b.BasketItemId,
                Name = b.Name,
                Path = b.Path,
                Price = b.Price,
                Quantity = b.Quantity
            }).ToList();

            return response;
        }
    }
}
