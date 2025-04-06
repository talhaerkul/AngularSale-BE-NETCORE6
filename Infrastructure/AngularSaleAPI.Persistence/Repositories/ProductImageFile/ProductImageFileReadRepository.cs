using AngularSaleAPI.Application.Repositories.File;
using AngularSaleAPI.Application.Repositories;
using AngularSaleAPI.Domain.Entities;
using AngularSaleAPI.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngularSaleAPI.Application.Repositories.ProductImageFile;

namespace AngularSaleAPI.Persistence.Repositories.ProductImageFileReadRepository
{
    public class ProductImageFileReadRepository : ReadRepository<ProductImageFile>, IProductImageFileReadRepository
    {
        public ProductImageFileReadRepository(AngularSaleAPIDbContext context) : base(context)
        {
        }
    }
}
