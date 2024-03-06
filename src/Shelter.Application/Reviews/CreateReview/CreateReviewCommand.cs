
using Shelter.Application.Abstractions.Messaging;

namespace Shelter.Application.Reviews.CreateReview;

public record class CreateReviewCommand(
    Guid BookingId,
    Guid UserId,
    int Rating,
    string Comment) : ICommand<Guid>;
