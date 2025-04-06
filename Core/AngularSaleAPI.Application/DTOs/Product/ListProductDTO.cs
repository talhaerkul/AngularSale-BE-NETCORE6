using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularSaleAPI.Application.DTOs.Product
{
    public class ListProductDTO
    {
        public int TotalCount { get; set; }
        public object Products { get; set; }
    }
}
