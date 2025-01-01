using MonolithBoilerPlate.Entity.Dtos;
using MonolithBoilerPlate.Entity.Entities;
using MonolithBoilerPlate.Entity.ViewModels;
using MonolithBoilerPlate.Repository.Pagination.Model;
using MonolithBoilerPlate.Service.Base;

namespace MonolithBoilerPlate.Service.Interface
{
    public interface IInvoiceService : IBaseService<Invoice>
    {
        Task<PagedViewModel<InvoiceVm>> GetPageAsync(ResourceQueryParameters args);
        Task<Invoice> AddAsync(InvoiceDto dto);
        Task PushToConsumerAsync(long invoiceId);
        Task<Invoice> GetInvoiceById(long id);
        Task<FileVm> ProcessInvoicePdfAsync(long invoiceId);
        Task SyncInvoice(long invoiceId);
        Task<object> GetInvoiceStatus(string invoiceNo);
    }
}
