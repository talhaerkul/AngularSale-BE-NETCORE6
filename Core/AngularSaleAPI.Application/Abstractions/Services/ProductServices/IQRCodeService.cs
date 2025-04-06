using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularSaleAPI.Application.Abstractions.Services.ProductServices
{
    public interface IQRCodeService
    {
        byte[] GenerateQRCode(string text);

    }
}
