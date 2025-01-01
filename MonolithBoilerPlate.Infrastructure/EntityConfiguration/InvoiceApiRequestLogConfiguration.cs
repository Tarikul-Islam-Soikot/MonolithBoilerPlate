using MonolithBoilerPlate.Entity.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace MonolithBoilerPlate.Infrastructure.EntityConfiguration
{
    public class InvoiceApiRequestLogConfiguration : IEntityTypeConfiguration<InvoiceApiRequestLog>
    {
        public void Configure(EntityTypeBuilder<InvoiceApiRequestLog> builder)
        {
            builder.HasIndex(x => x.InvoiceId);
        }
    }
}
