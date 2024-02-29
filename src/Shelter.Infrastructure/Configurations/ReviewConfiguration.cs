using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shelter.Domain.Bookings;
using Shelter.Domain.Reviews;
using Shelter.Domain.Users;
using Shelter.Domain.PetSitters;

namespace Shelter.Infrastructure.Configurations
{
    internal sealed class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("reviews");

            builder.HasKey(review => review.Id);

            builder.Property(review => review.Rating)
                .HasConversion(rating => rating.Value, value => Rating.Create(value).Value);

            builder.Property(review => review.Comment)
                .HasMaxLength(200)
                .HasConversion(comment => comment.Value, value => new Comment(value));

            builder.HasOne<PetSitter>()
                .WithMany()
                .HasForeignKey(review => review.PetSitterId);

            builder.HasOne<Booking>()
                .WithMany()
                .HasForeignKey(review => review.BookingId);

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(review => review.UserId);
        }
    }
}
