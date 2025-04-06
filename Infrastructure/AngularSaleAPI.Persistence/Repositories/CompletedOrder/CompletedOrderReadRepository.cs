using AngularSaleAPI.Application.Repositories;
using AngularSaleAPI.Application.Repositories.CompletedOrder;
using AngularSaleAPI.Domain.Entities;
using AngularSaleAPI.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularSaleAPI.Persistence.Repositories.CompletedOrder
{
    public class CompletedOrderReadRepository : ReadRepository<Domain.Entities.CompletedOrder>, ICompletedOrderReadRepository
    {
        public CompletedOrderReadRepository(AngularSaleAPIDbContext context) : base(context)
        {
        }
    }
}
