using Shelter.Application.Abstractions.Messaging;

namespace Shelter.Application.Reviews.UpdateReview;

public record class UpdateReviewCommand(
    Guid ReviewId,
    int Rating,
    string Comment) : ICommand<Guid>;
