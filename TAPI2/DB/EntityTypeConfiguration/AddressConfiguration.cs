using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TAPI2.Entities;

namespace TAPI2.DB.EntityTypeConfiguration
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(k => k.ID);
            builder.Property(p => p.ID).ValueGeneratedOnAdd();
            builder.Property(p => p.Name).IsRequired().HasMaxLength(60);
            builder.Property(p => p.Line1).HasMaxLength(255);
            builder.Property(p => p.Line2).HasMaxLength(255);
            builder.HasIndex(i => new { i.ContactID, i.Name }).IsUnique(true);
        }
    }
}