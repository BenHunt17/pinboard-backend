using FluentValidation;
using Pinboard.Domain.Inputs;

namespace Pinboard.Application.Validators
{
    public class NoteSearchInputValidator : AbstractValidator<NoteSearchInput>
    {
        public NoteSearchInputValidator()
        {
            RuleFor(x => x.Limit).GreaterThan(0).LessThan(99);
        }
    }
}
