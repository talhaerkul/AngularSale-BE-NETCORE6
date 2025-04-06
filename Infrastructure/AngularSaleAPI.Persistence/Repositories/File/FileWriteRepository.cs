using AngularSaleAPI.Application.Repositories.File;
using AngularSaleAPI.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularSaleAPI.Persistence.Repositories.File
{
    public class FileWriteRepository : WriteRepository<Domain.Entities.File>, IFileWriteRepository
    {
        public FileWriteRepository(AngularSaleAPIDbContext context) : base(context)
        {
        }
    }
}
