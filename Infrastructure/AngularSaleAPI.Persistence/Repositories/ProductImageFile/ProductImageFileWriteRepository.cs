using AngularSaleAPI.Application.Repositories.ProductImageFile;
using AngularSaleAPI.Domain.Entities;
using AngularSaleAPI.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularSaleAPI.Persistence.Repositories.ProductImageFileWriteRepository
{
    public class ProductImageFileWriteRepository : WriteRepository<ProductImageFile>, IProductImageFileWriteRepository
    {
        public ProductImageFileWriteRepository(AngularSaleAPIDbContext context) : base(context)
        {
        }
    }
}
