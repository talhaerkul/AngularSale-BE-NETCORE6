using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularSaleAPI.Application.DTOs.Order
{
    public class ListOrderResponseDTO
    {
        public int TotalCount { get; set; }
        public object Orders { get; set; }
        //public string OrderCode { get; set; }
        //public string UserName { get; set; }
        //public float TotalPrice { get; set; }
        //public DateTime CreatedDate { get; set; }
    }
}
