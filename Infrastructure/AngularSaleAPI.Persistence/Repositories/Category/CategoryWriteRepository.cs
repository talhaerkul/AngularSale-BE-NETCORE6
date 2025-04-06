using AngularSaleAPI.Application.Repositories;
using AngularSaleAPI.Application.Repositories.Category;
using AngularSaleAPI.Domain.Entities;
using AngularSaleAPI.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularSaleAPI.Persistence.Repositories.Category
{
    public class CategoryWriteRepository : WriteRepository<Domain.Entities.Category>, ICategoryWriteRepository
    {
        public CategoryWriteRepository(AngularSaleAPIDbContext context) : base(context)
        {
        }
    }
}
