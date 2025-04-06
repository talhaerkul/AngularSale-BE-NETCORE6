using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularSaleAPI.Application.Exceptions
{
    public class NotFoundProductException : Exception
    {
        public NotFoundProductException() : base("Not Found Product")
        {
        }

        public NotFoundProductException(string? message) : base(message)
        {
        }

        public NotFoundProductException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
