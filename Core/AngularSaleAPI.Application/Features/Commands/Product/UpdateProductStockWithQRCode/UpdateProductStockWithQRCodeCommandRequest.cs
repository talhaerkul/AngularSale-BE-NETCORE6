using MediatR;

namespace AngularSaleAPI.Application.Features.Commands.Product.UpdateProductStockWithQRCode
{
    public class UpdateProductStockWithQRCodeCommandRequest :IRequest<UpdateProductStockWithQRCodeCommandResponse>
    {
        public string Id { get; set; }
        public int Stock{ get; set; }
    }
}