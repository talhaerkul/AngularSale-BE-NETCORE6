using AngularSaleAPI.Application.Repositories;
using AngularSaleAPI.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularSaleAPI.Persistence.Repositories
{
    public class MenuReadRepository : ReadRepository<Domain.Entities.Menu>, IMenuReadRepository
    {
        public MenuReadRepository(AngularSaleAPIDbContext context) : base(context)
        {
        }
    }
}
