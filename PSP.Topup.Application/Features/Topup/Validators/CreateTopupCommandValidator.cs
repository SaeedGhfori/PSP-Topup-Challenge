using FluentValidation;

using PSP.Features.Topup.Commands;

namespace PSP.Features.Topup.Validators;

public sealed class CreateTopupCommandValidator
    : AbstractValidator<CreateTopupCommand>
{
    public CreateTopupCommandValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .Length(11)
            .Matches(@"^09\d{9}$");

        RuleFor(x => x.Amount)
            .GreaterThan(0);

        RuleFor(x => x.IdempotencyKey)
            .NotEmpty();

        RuleFor(x => x.Operator)
            .InclusiveBetween(1, 3);
    }
}
