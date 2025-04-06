using MediatR;

namespace AngularSaleAPI.Application.Features.Commands.Product.CreateCategory
{
    public class CreateCategoryCommandRequest : IRequest<CreateCategoryCommandResponse>
    {
        public string Category { get; set; }
    }
}