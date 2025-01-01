using MonolithBoilerPlate.Common;
using MonolithBoilerPlate.Entity.Entities;
using MonolithBoilerPlate.Entity.ViewModels;
using MonolithBoilerPlate.Service.Helper;
using MonolithBoilerPlate.Service.Interface;
using Microsoft.Extensions.Options;
using Microsoft.Reporting.NETCore;
namespace MonolithBoilerPlate.Service.Implementation
{
    public class ReportService : IReportService
    {
        private readonly IServerDirectoryService _serverDirectoryService;
        private readonly AppSettings _appSettings;
        private readonly ICompanyService _companyService;

        public ReportService(IServerDirectoryService serverDirectoryService,
            IOptions<AppSettings> appSettings,
            ICompanyService companyService)
        {
            _serverDirectoryService = serverDirectoryService;
            _appSettings = appSettings.Value;
            _companyService = companyService;
        }

        public void PrepareRdlcReportConfig(LocalReport report, string reportFileName)
        {
            string reportFolderName = _serverDirectoryService.GetDirectory(_appSettings.DirectoryPath.Report);
            var reportPath = Path.Combine(reportFolderName, reportFileName);

            if (!File.Exists(reportPath))
                throw new ArgumentException("Report file not found");

            using var reportDefinitionStream = File.OpenRead(reportPath);
            report.EnableExternalImages = true;
            report.LoadReportDefinition(reportDefinitionStream);
        }

        public async Task AssignCompanyReportParams(LocalReport report, long companyId)
        {
            var imagePath = "file:///" + _serverDirectoryService.GetDirectory("Images\\Logo.png").Replace("\\", "/");
            var company = await _companyService.FirstOrDefaultAsync(x=> x.Id == companyId);

            var reportParams = new Dictionary<string, string>()
            {
                { "Logo", imagePath },
                { "CompanyName", company.Name },
                { "Address", company.Address },
                { "ContactNo", company.Contact },
                { "Email", company.Email }
            };

            report.AssignReportParams(reportParams);
        }

        public void AssignInvoiceReportParams(LocalReport report, Invoice invoice)
        {
            var reportParams = new Dictionary<string, string>()
            {
                { "QrCodeImage", invoice?.QrCode?.GenerateQrCode() },
                { "SupplierTin", invoice.Supplier.Tin },
                { "SupplierName", invoice.Supplier.Name },
                { "SupplierIdentificationNumber", invoice.Supplier.IdentificationNumber },
                { "SupplierSst", invoice.Supplier.Sst},
                { "SupplierAddress", invoice.Supplier.Address },
                { "SupplierContact", invoice.Supplier.Contact },
                { "SupplierEmail", invoice.Supplier.Email },
                { "SupplierMsic", invoice.Supplier.MsicCode },
                { "SupplierBusinessActivityDescription", invoice.Supplier.BusinessActivityDescription },
                { "BuyerTin", invoice.Buyer.Tin },
                { "BuyerRegistrationNumber", invoice.Buyer.RegistrationNumber },
                { "BuyerSst", invoice.Buyer.Sst },
                { "IrbmUniqueIdNumber", invoice?.IrbmUniqueIdNumber },
                { "InvoiceType", invoice.InvoiceType },
                { "InvoiceVersion", invoice.InvoiceVersion },
                { "InvoiceNo", invoice.InvoiceNo },
                { "OriginalInvoiceRef", invoice.OriginalInvoiceRef },
                { "InvoiceDate", invoice.InvoiceDate.ToString("dd-MMM-yyyy hh:mm:ss tt") },
                { "ValidationDate", invoice.ValidationDate.ToString("dd-MMM-yyyy hh:mm:ss tt") }
            };

            report.AssignReportParams(reportParams);
        }

        public async Task<FileVm> GenerateInvoicePdfReport(Invoice invoice)
        {
            var report = new LocalReport();

            PrepareRdlcReportConfig(report, "Invoice.rdlc");
            await AssignCompanyReportParams(report, invoice.CompanyId);
            AssignInvoiceReportParams(report, invoice);

            var datasources = new Dictionary<string, object>()
            {
                {
                    "DataSet1", invoice.InvoiceLineItems.Select(x=> new
                    {
                        Classification = x.Classification,
                        Description = x.Description,
                        Quantity = x.Quantity,
                        UnitPrice = x.UnitPrice,
                        AmountExcludingDT = x.AmountExcludingDT,
                        Discount = x.Discount,
                        TaxRate = x.TaxRate,
                        TaxAmount = x.TaxAmount,
                        AmountIncludingDT = x.AmountIncludingDT
                    }).ToList()
                },

            };

            report.AssignReportDataSources(datasources);
            byte[] pdfBytes = report.Render("PDF");
            byte[] protectedPdfBytes = pdfBytes.GeneratePasswordProtectedPdf(invoice.Buyer.RegistrationNumber);

            return new FileVm
            {
                Name = invoice.InvoiceNo + ".pdf",
                ContentType = "application/pdf",
                Bytes = protectedPdfBytes,
            };
        }

    }
}
