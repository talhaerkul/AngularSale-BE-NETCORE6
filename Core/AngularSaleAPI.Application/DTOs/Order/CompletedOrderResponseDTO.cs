using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularSaleAPI.Application.DTOs.Order
{
    public class CompletedOrderResponseDTO
    {
        public string OrderCode { get; set; }
        public string UserName { get; set; }
        public DateTime OrderDate { get; set; }
        public string Email { get; set; }
    }
}
