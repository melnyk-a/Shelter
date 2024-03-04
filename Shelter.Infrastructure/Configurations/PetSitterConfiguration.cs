using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shelter.Domain.PetSitters;
using Shelter.Domain.Shared;

namespace Shelter.Infrastructure.Configurations;

internal sealed class PetSitterConfiguration : IEntityTypeConfiguration<PetSitter>
{
    public void Configure(EntityTypeBuilder<PetSitter> builder)
    {
        builder.ToTable("pet_sitters");

        builder.HasKey(petSitter => petSitter.Id);

        builder.OwnsOne(petSitter => petSitter.Address);

        builder.Property(petSitter => petSitter.Name)
            .HasMaxLength(200)
            .HasConversion(name => name.Value, value => new Name(value));

        builder.Property(petSitter => petSitter.Description)
            .HasMaxLength(2000)
            .HasConversion(description => description.Value, value => new Description(value));

        builder.OwnsOne(petSitter => petSitter.Price, priceBuilder =>
        {
            priceBuilder.Property(money => money.Currency)
            .HasConversion(currency => currency.Code, code => Currency.FromCode(code));
        });

        builder.OwnsOne(petSitter => petSitter.SecurityDeposit, priceBuilder =>
        {
            priceBuilder.Property(money => money.Currency)
            .HasConversion(currency => currency.Code, code => Currency.FromCode(code));
        });

        builder.Property<uint>("version").IsRowVersion();
    }
}
