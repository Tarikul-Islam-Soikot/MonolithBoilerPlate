using MonolithBoilerPlate.Entity.Enums;

namespace MonolithBoilerPlate.Entity.Entities
{
    public class DocumentSetting: Audit
    {
        public long Id { get; set; }
        public string DocumentTypeName { get; set; }
        public long CompanyId { get; set; }
        public string FilePath { get; set; }
        public string ProtectionKey { get; set; }
        public DocumentFormatType DocumentFormatType { get; set; }
        public string PdfSendingApiUrl { get; set; }
        public bool IsPdfResponseRequired { get; set; }
        public Company Company { get; set; }
    }
}
