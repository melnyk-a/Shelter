using Shelter.Application.Abstractions.Messaging;
using Shelter.Domain.Abstractions.Persistence;
using Shelter.Domain.Abstractions;
using Shelter.Domain.Reviews;

namespace Shelter.Application.Reviews.DeleteReview;

internal sealed class DeleteReviewCommandHandler :
    ICommandHandler<DeleteReviewCommand>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteReviewCommandHandler(
        IReviewRepository reviewRepository,
        IUnitOfWork unitOfWork)
    {
        _reviewRepository = reviewRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        DeleteReviewCommand request,
        CancellationToken cancellationToken)
    {
        var review = await _reviewRepository.GetByIdAsync(request.ReviewId, cancellationToken);

        if (review is null)
        {
            return Result.Failure<Guid>(ReviewErrors.NotFound);
        }

        await _reviewRepository.DeleteAsync(review);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
