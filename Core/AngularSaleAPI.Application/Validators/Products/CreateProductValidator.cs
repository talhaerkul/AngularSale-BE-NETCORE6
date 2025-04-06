using AngularSaleAPI.Application.DTOs.Product;
using FluentValidation;

namespace AngularSaleAPI.Application.Validators.Products
{
    public class CreateProductValidator : AbstractValidator<CreateProductRequestDTO>
    {
        public CreateProductValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Name Must Be Filled!")
                .MaximumLength(150)
                .MinimumLength(5)
                    .WithMessage("Length Must Be Between 5-150 Characters");

            RuleFor(p => p.Stock)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Stock Must Be Filled!")
                .Must(s => s >= 0)
                    .WithMessage("Stock Can Not Be Negative!");
            
            RuleFor(p => p.Price)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Price Must Be Filled!")
                .Must(s => s >= 0)
                    .WithMessage("Price Can Not Be Negative!");
        }

    }
}
