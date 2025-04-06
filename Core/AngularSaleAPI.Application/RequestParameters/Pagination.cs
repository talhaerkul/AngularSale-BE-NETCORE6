using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularSaleAPI.Application.RequestParameters
{
    public abstract class Pagination
    {
        [FromQuery]
        public int Page { get; set; } = 0;
        [FromQuery]
        public int Size { get; set; } = 13;
    }
}
