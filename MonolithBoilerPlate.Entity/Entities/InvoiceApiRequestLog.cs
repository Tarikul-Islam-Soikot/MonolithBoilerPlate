using MonolithBoilerPlate.Entity.Enums;

namespace MonolithBoilerPlate.Entity.Entities
{
    public class InvoiceApiRequestLog: Audit
    {
        public long Id { get; set; }
        public long? InvoiceId { get; set; }
        public string? RequestBody { get; set; }
    }
}
