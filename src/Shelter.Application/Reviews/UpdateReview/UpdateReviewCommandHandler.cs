using Shelter.Application.Abstractions.Messaging;
using Shelter.Domain.Abstractions.Persistence;
using Shelter.Domain.Abstractions;
using Shelter.Domain.Reviews;

namespace Shelter.Application.Reviews.UpdateReview;

internal sealed class UpdateReviewCommandHandler :
    ICommandHandler<UpdateReviewCommand, Guid>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateReviewCommandHandler(
        IReviewRepository reviewRepository,
        IUnitOfWork unitOfWork)
    {
        _reviewRepository = reviewRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(
        UpdateReviewCommand request,
        CancellationToken cancellationToken)
    {
        var review = await _reviewRepository.GetByIdAsync(request.ReviewId, cancellationToken);

        if (review is null)
        {
            return Result.Failure<Guid>(ReviewErrors.NotFound);
        }

        review.Update(
            Rating.Create(request.Rating).Value,
            new Comment(request.Comment));

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return review.Id;
    }
}
