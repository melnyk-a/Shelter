using Shelter.Application.Abstractions.Messaging;

namespace Shelter.Application.Reviews.DeleteReview;
public record class DeleteReviewCommand(Guid ReviewId) : ICommand;
