using MonolithBoilerPlate.Entity.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MonolithBoilerPlate.Entity.Enums;

namespace MonolithBoilerPlate.Infrastructure.EntityConfiguration
{
    public class DocumentSettingConfiguration : IEntityTypeConfiguration<DocumentSetting>
    {
        public void Configure(EntityTypeBuilder<DocumentSetting> builder)
        {
            builder.Property(x => x.DocumentTypeName).IsRequired().HasMaxLength(250);           
            builder.Property(x => x.FilePath).IsRequired().HasMaxLength(500);
            builder.Property(x => x.ProtectionKey).IsRequired().HasMaxLength(250);
            builder.Property(x => x.DocumentFormatType).IsRequired().HasDefaultValue(DocumentFormatType.Normal);
            builder.Property(x => x.PdfSendingApiUrl).IsRequired().HasMaxLength(500);
            builder.Property(x => x.IsPdfResponseRequired).IsRequired().HasDefaultValue(true);
            builder.Property(x => x.CompanyId).IsRequired();
            builder.HasIndex(x => x.CompanyId);       
            builder.HasOne(x => x.Company).WithMany().HasForeignKey(x => x.CompanyId);
        }
    }
}
