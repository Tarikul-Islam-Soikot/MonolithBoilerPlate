using MonolithBoilerPlate.Entity.Entities;
using MonolithBoilerPlate.Entity.ViewModels;

namespace MonolithBoilerPlate.Service.Interface
{
    public interface IReportService
    {
        Task<FileVm> GenerateInvoicePdfReport(Invoice invoice);
    }
}
