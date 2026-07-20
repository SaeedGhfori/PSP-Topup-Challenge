using FluentValidation;

namespace PSP.Payment.Application.Features.Payments.Commands;

public sealed class CreatePurchaseCommandValidator
    : AbstractValidator<CreatePurchaseCommand>
{
    public CreatePurchaseCommandValidator()
    {
        RuleFor(x => x.Pan)
            .Length(16);

        RuleFor(x => x.Amount)
            .GreaterThan(0);

        RuleFor(x => x.PhoneNumber)
            .Length(11);

        RuleFor(x => x.IdempotencyKey)
            .NotEmpty();
    }
}
