﻿using AutoMapper;
using MonolithBoilerPlate.Entity.Entities;
using MonolithBoilerPlate.Infrastructure.Context;
using MonolithBoilerPlate.Repository.Base;
using MonolithBoilerPlate.Repository.Interface;
using MonolithBoilerPlate.Repository.Pagination.Interface;

namespace MonolithBoilerPlate.Repository.Implementation
{
    public class GeneralApiRequestLogRepository : BaseRepository<GeneralApiRequestLog>, IGeneralApiRequestLogRepository
    {
        public GeneralApiRequestLogRepository(
               IApplicationDbContext dbContext,
               IMapper mapper,
               ISqlPagingManager sqlPagingManager)
               : base(dbContext, mapper, sqlPagingManager)
        {
        }
    }
}