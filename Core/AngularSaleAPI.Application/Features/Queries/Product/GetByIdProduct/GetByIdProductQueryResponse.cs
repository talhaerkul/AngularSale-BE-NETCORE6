using AngularSaleAPI.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularSaleAPI.Application.Features.Queries.Product.GetByIdProduct
{
    public class GetByIdProductQueryResponse
    {
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public string SalerUsername { get; set; }
        public int Stock { get; set; }
        public float Price { get; set; }

    }
}
