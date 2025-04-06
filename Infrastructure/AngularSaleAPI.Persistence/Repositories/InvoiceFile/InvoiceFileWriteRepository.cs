using AngularSaleAPI.Application.Repositories.File;
using AngularSaleAPI.Application.Repositories.InvoiceFile;
using AngularSaleAPI.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularSaleAPI.Persistence.Repositories.InvoiceFile
{
    public class InvoiceFileWriteRepository : WriteRepository<Domain.Entities.InvoiceFile>, IInvoiceFileWriteRepository
    {
        public InvoiceFileWriteRepository(AngularSaleAPIDbContext context) : base(context)
        {
        }
    }
}
