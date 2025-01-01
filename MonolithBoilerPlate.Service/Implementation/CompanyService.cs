﻿using AutoMapper;
using MonolithBoilerPlate.Entity.Entities;
using MonolithBoilerPlate.Repository.UnitOfWork;
using MonolithBoilerPlate.Service.Base;
using MonolithBoilerPlate.Service.Interface;
using MonolithBoilerPlate.Common;
using Microsoft.Extensions.Options;

namespace MonolithBoilerPlate.Service.Implementation
{
    public class CompanyService : BaseService<Company>, ICompanyService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IOptions<AppSettings> appSettings
            ) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
      

    }
}
