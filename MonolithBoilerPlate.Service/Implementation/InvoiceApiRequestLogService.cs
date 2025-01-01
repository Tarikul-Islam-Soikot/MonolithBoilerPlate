using AutoMapper;
using MonolithBoilerPlate.Entity.Entities;
using MonolithBoilerPlate.Repository.UnitOfWork;
using MonolithBoilerPlate.Service.Base;
using MonolithBoilerPlate.Service.Interface;
using MonolithBoilerPlate.Common;
using Microsoft.Extensions.Options;

namespace MonolithBoilerPlate.Service.Implementation
{
    public class InvoiceApiRequestLogService : BaseService<InvoiceApiRequestLog>, IInvoiceApiRequestLogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public InvoiceApiRequestLogService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IOptions<AppSettings> appSettings
            ) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }      

    }
}
