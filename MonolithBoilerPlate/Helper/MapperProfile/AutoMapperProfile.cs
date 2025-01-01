using MonolithBoilerPlate.Entity.Dtos;
using MonolithBoilerPlate.Entity.Entities;
using MonolithBoilerPlate.Entity.ViewModels;
using AutoMapper;

namespace MonolithBoilerPlate.Api.Helper.MapperProfile
{
    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile()
        {
            CreateMap<RuleDto, Rule>().ReverseMap();

            CreateMap<Rule, RuleVm>();

            CreateMap<InvoiceDto, Invoice>()
                .ReverseMap();
            CreateMap<InvoiceLineItemDto, InvoiceLineItem>().ReverseMap();
            CreateMap<SupplierDto, Supplier>().ReverseMap();
            CreateMap<BuyerDto, Buyer>().ReverseMap();

            CreateMap<Invoice, InvoiceVm>()
                .ForMember(v => v.IsReportGenerated, e => e.MapFrom(v=> v.FileGeneratedDate!= null))
                 .ForMember(v => v.SupplierIdentificationNo, e => e.MapFrom(v => v.Supplier.IdentificationNumber))
                 .ForMember(v => v.BuyerRegistrationNo, e => e.MapFrom(v => v.Buyer.RegistrationNumber));
        }
    }
}
