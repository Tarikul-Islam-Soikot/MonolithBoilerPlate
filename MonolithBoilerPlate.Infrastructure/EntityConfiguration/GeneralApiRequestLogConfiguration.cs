using MonolithBoilerPlate.Entity.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace MonolithBoilerPlate.Infrastructure.EntityConfiguration
{
    public class GeneralApiRequestLogConfiguration : IEntityTypeConfiguration<GeneralApiRequestLog>
    {
        public void Configure(EntityTypeBuilder<GeneralApiRequestLog> builder)
        {
            builder.HasIndex(x => x.InvoiceId);
            builder.HasIndex(x => x.InvoiceApiRequestLogId);
        }
    }
}
