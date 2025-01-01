using AutoMapper;
using MonolithBoilerPlate.Entity.Entities;
using MonolithBoilerPlate.Repository.Interface;
using MonolithBoilerPlate.Repository.UnitOfWork;
using MonolithBoilerPlate.Service.Base;
using MonolithBoilerPlate.Service.Interface;
using MonolithBoilerPlate.Entity.Dtos;
using MonolithBoilerPlate.Common;
using MonolithBoilerPlate.Infrastructure.Utility;
using Microsoft.EntityFrameworkCore;
using MonolithBoilerPlate.Entity.ViewModels;
using MonolithBoilerPlate.Common.Interface;
using Microsoft.Extensions.Options;
using MonolithBoilerPlate.Service.Helper;
using MonolithBoilerPlate.Repository.Pagination.Model;
using MonolithBoilerPlate.Entity.Enums;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MassTransit.Initializers;
using MonolithBoilerPlate.Common.Extensions;

namespace MonolithBoilerPlate.Service.Implementation
{
    public class InvoiceService : BaseService<Invoice>, IInvoiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserInfo _currentUserInfo;
        private readonly IServerDirectoryService _serverDirectoryService;
        private readonly IMessageBrokerHelper _messageBroker;
        private readonly AppSettings _appSettings;
        private readonly IReportService _reportService;
        private readonly IInvoiceApiRequestLogService _invoiceApiRequestLogService;
        private readonly HttpClient _httpClient;
        private static SemaphoreSlim semaphore = new(1);

        public InvoiceService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ICurrentUserInfo currentUserInfo,
             IMessageBrokerHelper messageBroker,
             IServerDirectoryService serverDirectoryService,
             IOptions<AppSettings> appSettings,
             IReportService reportService,
             IInvoiceApiRequestLogService invoiceApiRequestLogService,
             HttpClient httpClient
            ) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _currentUserInfo = currentUserInfo;
            _messageBroker = messageBroker;
            _serverDirectoryService = serverDirectoryService;
            _appSettings = appSettings.Value;
            _reportService = reportService;
            _invoiceApiRequestLogService = invoiceApiRequestLogService;
            _httpClient = httpClient;
        }

        public async Task<PagedViewModel<InvoiceVm>> GetPageAsync(ResourceQueryParameters args)
        {
            var query = _unitOfWork.Repository<Invoice>().Query();

            if (!string.IsNullOrEmpty(args.SearchText))
                query = query.Where(x => x.InvoiceNo.Contains(args.SearchText)
                                      || x.IrbmUniqueIdNumber.Contains(args.SearchText)
                                      || x.QrCode.Contains(args.SearchText)
                                      || x.Supplier.IdentificationNumber.Contains(args.SearchText)
                                      || x.Buyer.RegistrationNumber.Contains(args.SearchText));

            return (await _unitOfWork.Repository<Invoice>().GetPagesToAsync<ResourceQueryParameters, InvoiceVm>(query, args));
        }

        public async Task<Invoice> AddAsync(InvoiceDto dto)
        {
            if (dto is null)
                ArgumentNullException.ThrowIfNull(nameof(Invoice));
            ValidateInput(dto);

            Invoice entity = new Invoice();
            MapObject(dto, entity);

            await semaphore.WaitAsync();
            try
            {
                entity.InvoiceNo = await GenerateInvoiceNumber(dto.InvoiceSource);
                entity.CompanyId = _currentUserInfo.CompanyId;
                entity.Status = ProcessingStatus.InProgress;
                entity.NumberOfAttemptToSyncAgbs = 0;

                await _unitOfWork.GetRepository<IInvoiceRepository>().InsertAsync(entity);
                await _unitOfWork.CompleteAsync();
            }
            finally
            {
                semaphore.Release();
            }

            var invoiceApiRequestLog = new InvoiceApiRequestLog
            {
                InvoiceId = entity.Id,
                RequestBody = JsonConvert.SerializeObject(dto, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }),
            };

            await _invoiceApiRequestLogService.InsertAsync(invoiceApiRequestLog);
            await PushToInvoiceSyncConsumerAsync(entity.Id);

            return entity;
        }

        private void ValidateInput(InvoiceDto dto)
        {
            if(dto.InvoiceSource.IsNullOrEmpty())
                throw new BadRequestException("Please provide a valid InvoiceSource");
            if (dto.Supplier is null)
                throw new ArgumentNullException("supplier");
            if (dto.Supplier.Name.IsNullOrEmpty())
                throw new ArgumentNullException("Supplier's Name");
            if (dto.Buyer is null)
               throw new ArgumentNullException("Buyer");
            if (dto.Buyer.RegistrationNumberCategory.IsNullOrEmpty())
                throw new ArgumentNullException("Buyer's Registration number category");
            if (dto.Buyer.RegistrationNumber.IsNullOrEmpty())
                throw new ArgumentNullException("Buyer's Registration Number");
            if (dto.InvoiceLineItems is null)
                throw new ArgumentNullException("invoiceLineItems");
        }

        public async Task UpdateInvoiceStatus(long invoiceId, ProcessingStatus status)
        {
            var invoice = await _unitOfWork.GetRepository<IInvoiceRepository>().FirstOrDefaultAsync(x => x.Id == invoiceId);

           invoice.Status = status;
           invoice.NumberOfAttemptToSyncAgbs = invoice.NumberOfAttemptToSyncAgbs + 1;

           await _unitOfWork.GetRepository<IInvoiceRepository>().UpdateAsync(invoice);
           await _unitOfWork.CompleteAsync();
        }

        public async Task PushToInvoiceSyncConsumerAsync(long invoiceId)
        {
            await _messageBroker.PublishAsync<InvoiceSyncConsumerVm>(new
            {
                InvoiceId = invoiceId
            });
        }

        public async Task PushToConsumerAsync(long invoiceId)
        {
            await _messageBroker.PublishAsync<InvoicePdfConsumerVm>(new
            {
                InvoiceId = invoiceId
            });
        }

        public Invoice MapObject(InvoiceDto dto, Invoice entity)
        {
            entity.DocumentType = dto.DocumentType;
            entity.InvoiceType = dto.InvoiceType.Trim();
            entity.InvoiceVersion = dto.InvoiceVersion.Trim();
            entity.OriginalInvoiceRef = dto.OriginalInvoiceRef;
            entity.InvoiceDate = DateTime.Now;
            entity.Subtotal = dto.Subtotal;
            entity.TotalExcludingTax = dto.TotalExcludingTax;
            entity.TotalTaxAmount = dto.TotalTaxAmount;
            entity.TotalIncludingTax = dto.TotalIncludingTax;
            entity.TotalPayableAmount = dto.TotalPayableAmount;

            if (dto.Supplier != null)
            {
                entity.Supplier = new Supplier();
                entity.Supplier.IdentificationNumber = dto.Supplier.IdentificationNumber;
                entity.Supplier.Name = dto.Supplier.Name;
                entity.Supplier.MsicCode = dto.Supplier.MsicCode;
                entity.Supplier.Tin = dto.Supplier.Tin;
                entity.Supplier.Sst = dto.Supplier.Sst;
                entity.Supplier.Address = dto.Supplier.Address;
                entity.Supplier.Contact = dto.Supplier.Contact;
                entity.Supplier.Email = dto.Supplier.Email;
                entity.Supplier.BusinessActivityDescription = dto.Supplier.BusinessActivityDescription;
            }

            if (dto.Buyer != null)
            {
                entity.Buyer = new Buyer();
                entity.Buyer.RegistrationNumber = dto.Buyer.RegistrationNumber;
                entity.Buyer.Name = dto.Buyer.Name;
                entity.Buyer.MsicCode = dto.Buyer.MsicCode;
                entity.Buyer.Tin = dto.Buyer.Tin;
                entity.Buyer.Sst = dto.Buyer.Sst;
                entity.Buyer.Address = dto.Buyer.Address;
                entity.Buyer.Contact = dto.Buyer.Contact;
                entity.Buyer.Email = dto.Buyer.Email;
                entity.Buyer.BusinessActivityDescription = dto.Buyer.BusinessActivityDescription;
            }

            if (dto.InvoiceLineItems.Any())
            {
                entity.InvoiceLineItems = new List<InvoiceLineItem>();
                entity.InvoiceLineItems = dto.InvoiceLineItems.Select(x => new InvoiceLineItem
                {
                    ItemNo = x.ItemNo,
                    ItemName = x.ItemName,
                    Description = x.Description,
                    Classification = x.Classification,
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice,
                    AmountExcludingDT = x.AmountExcludingDT,
                    Discount = x.Discount,
                    TaxRate = x.TaxRate,
                    TaxAmount = x.TaxAmount,
                    AmountIncludingDT = x.AmountIncludingDT,

                }).ToList();
            }

            return entity;
        }

        public async Task<string> GenerateInvoiceNumber(string invoiceSource)
        {
            var prefixEntity = await _unitOfWork.GetRepository<IPrefixConfigRepository>().FirstOrDefaultAsync(x => x.SourceName.ToLower() == invoiceSource.ToLower().Trim());         
            if(prefixEntity == null) 
                throw new BadRequestException("Invoice Source is not valid!");

            string prefixPart = prefixEntity.Prefix + DateTime.Now.ToString("yyyyMMdd");

            string invoiceNo = await _unitOfWork.GetRepository<IInvoiceRepository>()
                                                                 .Query()
                                                                 .Where(x => x.InvoiceNo.Substring(0, 9) == prefixPart)
                                                                 .MaxAsync(x => x.InvoiceNo);

            long currentNumber = string.IsNullOrEmpty(invoiceNo) ? 0 : long.Parse(invoiceNo.Substring(0));
            prefixPart = string.IsNullOrEmpty(invoiceNo) ? prefixPart : string.Empty;
            string generatedInvoiceNo = $"{prefixPart}{(currentNumber + 1):D5}";

            return generatedInvoiceNo;
        }

        public async Task<Invoice> GetInvoiceById(long id)
        {
            var invoice = await _unitOfWork.GetRepository<IInvoiceRepository>()
                                           .FirstOrDefaultAsync(x => x.Id == id,
                                           x => x.InvoiceLineItems,
                                           x => x.Supplier,
                                           x => x.Buyer);
            if (invoice == null)
            {
                throw new NotFoundException("Invoice not found!");
            }

            return invoice;
        }

        public async Task<FileVm> ProcessInvoicePdfAsync(long invoiceId)
        {
            var invoice = await GetInvoiceById(invoiceId);
            var file = await _reportService.GenerateInvoicePdfReport(invoice);
            string filePath = await SaveInvoicePdfToDirectory(invoice, file);

            int index = filePath.IndexOf(_appSettings.DirectoryPath.Root, StringComparison.OrdinalIgnoreCase);

            invoice.FilePath = (index >= 0) ? filePath.Substring(index) : string.Empty;
            invoice.FileGeneratedDate = DateTime.Now;
            await _unitOfWork.GetRepository<IInvoiceRepository>().UpdateAsync(invoice);
            await _unitOfWork.CompleteAsync();

            return file;
        }

        public async Task<string> SaveInvoicePdfToDirectory(Invoice invoice, FileVm file)
        {
            var company = await _unitOfWork.GetRepository<ICompanyRepository>().FirstOrDefaultAsync(x => x.Id == invoice.CompanyId);
            string path = _serverDirectoryService.GetDirectory(_appSettings.DirectoryPath.Invoice);
            string folderPath = Path.Combine(path, company?.Name?.ToUpper()?.Split(' ')[0], invoice.CreatedAt.ToString("yyyy-MM-dd"));

            if (!_serverDirectoryService.FileExists(folderPath))
            {
                _serverDirectoryService.CreateDirectory(folderPath);
            }

            string filePath = Path.Combine(folderPath, file.Name);
            _serverDirectoryService.SaveFile(filePath, file.ToFormFile());

            return filePath;
        }

        public async Task SyncInvoice(long invoiceId)
        {
            GeneralApiRequestLog generalApiRequestLog = new GeneralApiRequestLog();

            var invoice = await _unitOfWork.GetRepository<IInvoiceRepository>().GetNonTrackingFirstOrDefaultAsync(x=> x.Id == invoiceId);

            try
            {
                if (invoice is null)
                    throw new NotFoundException("Invoice not found!");
                else if (invoice.Status != ProcessingStatus.InProgress || invoice.NumberOfAttemptToSyncAgbs >= _appSettings.ConstantValue.MaximumAttemptToSync)
                    throw new InternalServerException("Please Process Manually. InvoiceId: " + invoiceId);

                var invoiceApiRequestLog = await _unitOfWork.GetRepository<IInvoiceApiRequestLogRepository>().FirstOrDefaultAsync(x => x.InvoiceId == invoiceId);
                var content = new StringContent(invoiceApiRequestLog.RequestBody, Encoding.UTF8, "application/json");
                _httpClient.BaseAddress = new Uri(_appSettings.InvoiceGeneratorHostApi.BaseUrl);
                HttpResponseMessage response = await _httpClient.PostAsync(_appSettings.InvoiceGeneratorHostApi.DummyApi, content);

                if (response.IsSuccessStatusCode)
                {
                    await UpdateInvoiceStatus(invoiceId, ProcessingStatus.Completed);
                    await PushToConsumerAsync(invoiceId);
                }
                else
                {
                    await UpdateInvoiceStatus(invoiceId, ProcessingStatus.InProgress);
                }

                generalApiRequestLog.InvoiceId = invoiceId;
                generalApiRequestLog.InvoiceApiRequestLogId = invoiceApiRequestLog.Id;
                generalApiRequestLog.RequestBody = await content.ReadAsStringAsync();
                generalApiRequestLog.ResponseMessage = await response.Content.ReadAsStringAsync();

            }
            catch (Exception ex)
            {
                await UpdateInvoiceStatus(invoiceId, ProcessingStatus.Failed);

                generalApiRequestLog.ExceptionMessage = ex.Message;
                generalApiRequestLog.StackTrace = ex.StackTrace;
            }
            finally
            {
                await _unitOfWork.GetRepository<IGeneralApiRequestLogRepository>().InsertAsync(generalApiRequestLog);
                await _unitOfWork.CompleteAsync();
            }

        }

        public async Task<object> GetInvoiceStatus(string invoiceNo)
        {
            var res = await _unitOfWork.GetRepository<IInvoiceRepository>().FirstOrDefaultAsync(x => x.InvoiceNo == invoiceNo.Trim())
                .Select(x => new
                {
                    InvoiceNo = x.InvoiceNo,
                    Status = x.Status.GetEnumDescription()
                });

            return res;
        }

    }
}
