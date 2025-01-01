using MonolithBoilerPlate.Entity.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace MonolithBoilerPlate.Infrastructure.EntityConfiguration
{
    public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.HasIndex(x => x.IdentificationNumber);
            builder.Property(x => x.IdentificationNumber).HasMaxLength(250);
            builder.Property(x => x.Name).HasMaxLength(250);
            builder.Property(x => x.MsicCode).HasMaxLength(250);
            builder.Property(x => x.Tin).HasMaxLength(250);
            builder.Property(x => x.Sst).HasMaxLength(250);
            builder.Property(x => x.Address).HasMaxLength(500);
            builder.Property(x => x.Contact).HasMaxLength(250);
            builder.Property(x => x.Email).HasMaxLength(250);
            builder.Property(x => x.BusinessActivityDescription).HasMaxLength(500);
        }
    }
}
