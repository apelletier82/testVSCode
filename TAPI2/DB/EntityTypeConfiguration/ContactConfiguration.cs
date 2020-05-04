using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TAPI2.Entities;

namespace TAPI2.DB.EntityTypeConfiguration
{
    public class ContactConfiguration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.HasKey(k => k.ID);
            builder.Property(p => p.ID).ValueGeneratedOnAdd();
            builder.HasMany(o => o.Addresses).WithOne();
            builder.Property(p => p.Name).IsRequired().HasMaxLength(128);
            builder.Property(p => p.Name2).HasMaxLength(128);
            builder.Property(p => p.RowVersion).IsRowVersion();                
        }
    }
}