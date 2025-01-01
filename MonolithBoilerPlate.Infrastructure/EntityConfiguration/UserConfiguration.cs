using MonolithBoilerPlate.Entity.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace MonolithBoilerPlate.Infrastructure.EntityConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasIndex(x => x.UserName).IsUnique();
            builder.Property(x => x.UserName).IsRequired().HasMaxLength(250);
            builder.Property(x => x.PasswordHash).IsRequired();
            builder.HasIndex(x => x.CompanyId);
            builder.HasOne(x => x.Company).WithMany().HasForeignKey(x => x.CompanyId);
            builder.Property(x => x.RefreshToken).IsRequired().HasMaxLength(250);
        }
    }
}
