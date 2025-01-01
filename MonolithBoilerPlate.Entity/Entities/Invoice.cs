
using MonolithBoilerPlate.Entity.Enums;

namespace MonolithBoilerPlate.Entity.Entities
{
    public class Invoice: Audit
    {
        public long Id { get; set; }
        public string DocumentType { get; set; }
        public string InvoiceType { get; set; }
        public string InvoiceVersion { get; set; }
        public string InvoiceNo { get; set; }
        public string? IrbmUniqueIdNumber { get; set; }
        public string OriginalInvoiceRef { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal Subtotal { get; set; }
        public decimal TotalExcludingTax { get; set; }
        public decimal? TotalTaxAmount { get; set; }
        public decimal TotalIncludingTax { get; set; }
        public decimal TotalPayableAmount { get; set; }
        public DateTime ValidationDate { get; set; }
        public string? QrCode { get; set; }
        public string? FilePath { get; set; }
        public ProcessingStatus Status { get; set; }
        public int NumberOfAttemptToSyncAgbs { get; set; }
        public DateTime? FileGeneratedDate { get; set; }
        public long CompanyId { get; set; }
        public Company Company { get; set; }
        public long SupplierId { get; set; }
        public Supplier Supplier { get; set; }
        public long BuyerId { get; set; }
        public Buyer Buyer { get; set; }
        public List<InvoiceLineItem> InvoiceLineItems { get; set; }

    }
}
