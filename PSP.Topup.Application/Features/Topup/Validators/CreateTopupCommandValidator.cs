using FluentValidation;

using PSP.Topup.Application.Features.Topup.Commands;

namespace PSP.Topup.Application.Features.Topup.Validators;

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

        RuleFor(x => x.OperatorId)
            .InclusiveBetween(1, 3);

        RuleFor(x => x.IdempotencyKey)
            .NotEmpty()
            .MaximumLength(64);
    }
}
