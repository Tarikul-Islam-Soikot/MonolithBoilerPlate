using MonolithBoilerPlate.Entity.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace MonolithBoilerPlate.Infrastructure.EntityConfiguration
{
    public class PrefixConfigConfiguration : IEntityTypeConfiguration<PrefixConfig>
    {
        public void Configure(EntityTypeBuilder<PrefixConfig> builder)
        {
            builder.HasIndex(x => x.SourceName).IsUnique();
            builder.Property(x => x.SourceName).IsRequired().HasMaxLength(250);
            builder.HasIndex(x => x.Prefix).IsUnique();
            builder.Property(x => x.Prefix).IsRequired().HasMaxLength(20);
        }
    }
}
