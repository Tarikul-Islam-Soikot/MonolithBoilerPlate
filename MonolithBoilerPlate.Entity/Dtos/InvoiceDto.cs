

namespace MonolithBoilerPlate.Entity.Dtos
{
    public class InvoiceDto
    {
        public string InvoiceSource { get; set; }
        public string DocumentType { get; set; }
        public string InvoiceType { get; set; }
        public string InvoiceVersion { get; set; }
        //public string InvoiceNo { get; set; }
        //public string IrbmUniqueIdNumber { get; set; }
        public string OriginalInvoiceRef { get; set; }
        //public DateTime InvoiceDate { get; set; }
        public decimal Subtotal { get; set; }
        public decimal TotalExcludingTax { get; set; }
        public decimal? TotalTaxAmount { get; set; }
        public decimal TotalIncludingTax { get; set; }
        public decimal TotalPayableAmount { get; set; }
        //public DateTime ValidationDate { get; set; }
        //public string QrCode { get; set; }
        public SupplierDto Supplier { get; set; }
        public BuyerDto Buyer { get; set; }
        public List<InvoiceLineItemDto> InvoiceLineItems { get; set; }

    }
}
