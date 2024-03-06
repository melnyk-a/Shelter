using Shelter.Domain.Abstractions;

namespace Shelter.Domain.Reviews
{
    public static class ReviewErrors
    {
        public static readonly Error NotEligible = new(
            "Review.NotEligible",
            "The review is not eligible because the booking is not yet completed");

        public static Error NotMatch = new(
            "Review.NotMatch",
            "Booking mismatch. You cannot leave a review without having made a booking");

        public static Error NotFound = new(
            "Review.Found",
            "The review with the specified identifier was not found");
    }
}