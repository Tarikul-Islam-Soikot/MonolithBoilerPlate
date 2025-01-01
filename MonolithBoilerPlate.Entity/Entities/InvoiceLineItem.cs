namespace MonolithBoilerPlate.Entity.Entities
{
    public class InvoiceLineItem: Audit
    {
        public long Id { get; set; }
        public long InvoiceId { get; set; }
        public string ItemNo { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public string Classification { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal? AmountExcludingDT { get; set; }
        public decimal? Discount { get; set; }
        public decimal? TaxRate { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal? AmountIncludingDT { get; set; }
        public Invoice Invoice { get; set; }
    }
}
