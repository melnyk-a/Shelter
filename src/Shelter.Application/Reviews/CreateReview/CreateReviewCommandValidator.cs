using FluentValidation;

namespace Shelter.Application.Reviews.CreateReview;

internal  sealed class CreateReviewCommandValidator :
    AbstractValidator<CreateReviewCommand>
{
    public CreateReviewCommandValidator()
    {
        RuleFor(r => r.UserId).NotEmpty();

        RuleFor(r => r.BookingId).NotEmpty();

        RuleFor(r => r.Rating).InclusiveBetween(1, 5);

        RuleFor(r => r.Comment).NotEmpty();
    }
}
