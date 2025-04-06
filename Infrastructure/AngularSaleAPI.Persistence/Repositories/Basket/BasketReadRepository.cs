using AngularSaleAPI.Application.Repositories;
using AngularSaleAPI.Domain.Entities;
using AngularSaleAPI.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularSaleAPI.Persistence.Repositories
{
    public class BasketReadRepository : ReadRepository<Basket>, IBasketReadRepository
    {
        public BasketReadRepository(AngularSaleAPIDbContext context) : base(context)
        {
        }
    }
}
