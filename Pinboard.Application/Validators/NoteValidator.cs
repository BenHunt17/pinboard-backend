using FluentValidation;
using Pinboard.Domain.Model;

namespace Pinboard.Application.Validators
{
    public class NoteValidator : AbstractValidator<Note>
    {
        public NoteValidator()
        { 
            RuleFor(x => x.Title).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Content).NotNull().MaximumLength(999);
        }
    }
}
