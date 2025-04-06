using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularSaleAPI.Application.DTOs.Basket
{
    public class UpdateBasketItemRequestDTO
    {
        public string BasketItemId { get; set; }
        public int Quantity { get; set; }
    }
}
