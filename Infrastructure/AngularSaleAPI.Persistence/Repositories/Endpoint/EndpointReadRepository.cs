using AngularSaleAPI.Application.Repositories;
using AngularSaleAPI.Application.Repositories.File;
using AngularSaleAPI.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularSaleAPI.Persistence.Repositories
{
    public class EndpointReadRepository : ReadRepository<Domain.Entities.Endpoint>, IEndpointReadRepository
    {
        public EndpointReadRepository(AngularSaleAPIDbContext context) : base(context)
        {
        }
    }
}
