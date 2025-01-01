using MonolithBoilerPlate.Entity.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace MonolithBoilerPlate.Infrastructure.EntityConfiguration
{
    public class InvoiceLineItemConfiguration : IEntityTypeConfiguration<InvoiceLineItem>
    {
        public void Configure(EntityTypeBuilder<InvoiceLineItem> builder)
        {
            builder.HasIndex(x => x.InvoiceId);
            builder.Property(x => x.ItemNo).IsRequired().HasMaxLength(250);
            builder.Property(x => x.ItemName).IsRequired().HasMaxLength(250);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(500);
            builder.Property(x => x.Classification).IsRequired().HasMaxLength(250);
            builder.Property(x => x.Quantity).IsRequired();
            builder.Property(x => x.UnitPrice).IsRequired();
            builder.HasOne(x => x.Invoice).WithMany(x => x.InvoiceLineItems).HasForeignKey(x => x.InvoiceId);
        }
    }
}
