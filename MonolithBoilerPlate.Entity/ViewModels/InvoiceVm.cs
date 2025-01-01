

namespace MonolithBoilerPlate.Entity.ViewModels
{
    public class InvoiceVm
    {
        public long Id { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string? IrbmUniqueIdNumber { get; set; }
        public string? QrCode { get; set; }
        public string SupplierIdentificationNo {get; set;}
        public string BuyerRegistrationNo { get; set; }
        public decimal TotalPayableAmount { get; set; }
        public bool IsReportGenerated { get; set; }
        public DateTime FileGeneratedDate { get; set; }
    }
}
