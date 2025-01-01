using MonolithBoilerPlate.Entity.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MonolithBoilerPlate.Entity.Enums;

namespace MonolithBoilerPlate.Infrastructure.EntityConfiguration
{
    public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        { 
            builder.Property(x => x.DocumentType).IsRequired().HasMaxLength(250);
            builder.Property(x => x.InvoiceType).IsRequired().HasMaxLength(250);
            builder.Property(x => x.InvoiceVersion).IsRequired().HasMaxLength(250);
            builder.Property(x => x.InvoiceNo).IsRequired().HasMaxLength(250);
            builder.HasIndex(x => x.InvoiceNo).IsUnique();
            builder.Property(x => x.IrbmUniqueIdNumber).HasMaxLength(250);
            builder.HasIndex(x => x.IrbmUniqueIdNumber);
            builder.Property(x => x.QrCode).HasMaxLength(250);
            builder.HasIndex(x => x.QrCode);
            builder.Property(x => x.OriginalInvoiceRef).HasMaxLength(250);
            builder.Property(x => x.InvoiceDate).IsRequired();
            builder.Property(x => x.ValidationDate).IsRequired();
            builder.Property(x => x.Status).HasDefaultValue(ProcessingStatus.Initial);
            builder.HasIndex(x => x.SupplierId);
            builder.HasIndex(x => x.BuyerId);
            builder.HasIndex(x => x.CompanyId);
            builder.Property(x => x.FilePath).HasMaxLength(500);
            builder.HasOne(x => x.Supplier).WithMany().HasForeignKey(x => x.SupplierId);
            builder.HasOne(x => x.Buyer).WithMany().HasForeignKey(x => x.BuyerId);
            builder.HasOne(x => x.Company).WithMany().HasForeignKey(x => x.CompanyId);
        }
    }
}
