using AutoMapper;
using Business.Models;
using Data.Entities;
using System.Linq;

namespace Business
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Receipt, ReceiptModel>()
                .ForMember(rm => rm.ReceiptDetailsIds, r => r.MapFrom(x => x.ReceiptDetails.Select(rd => rd.Id)))
                .ReverseMap()
                .ForMember(r => r.Id, rm => rm.Ignore());

            CreateMap<Product, ProductModel>()
                .ForMember(p => p.ProductCategoryId, pm => pm.MapFrom(x => x.ProductCategoryId))
                .ForMember(pm => pm.CategoryName, p => p.MapFrom(x => x.Category.CategoryName))
                .ForMember(pm => pm.ReceiptDetailIds, p => p.MapFrom(x => x.ReceiptDetails.Select(rd => rd.Id)))
                .ReverseMap()
                .ForMember(p => p.Id, pm => pm.Ignore())
                .ForMember(p => p.Category, pm => pm.Ignore())
                .ForMember(p => p.ReceiptDetails, pm => pm.Ignore());

            CreateMap<ReceiptDetail, ReceiptDetailModel>()
                .ForMember(rdm => rdm.ReceiptId, rd => rd.MapFrom(x => x.ReceiptId))
                .ForMember(rdm => rdm.ProductId, rd => rd.MapFrom(x => x.ProductId))
                .ForMember(rdm => rdm.DiscountUnitPrice, rd => rd.MapFrom(x => x.DiscountUnitPrice))
                .ForMember(rdm => rdm.UnitPrice, rd => rd.MapFrom(x => x.UnitPrice))
                .ForMember(rdm => rdm.Quantity, rd => rd.MapFrom(x => x.Quantity))
                .ReverseMap();

            CreateMap<CustomerModel, Person>()
                .ForMember(p => p.Id, cm => cm.Ignore())
                .ForMember(p => p.Name, cm => cm.MapFrom(x => x.Name))
                .ForMember(p => p.Surname, cm => cm.MapFrom(x => x.Surname))
                .ForMember(p => p.BirthDate, cm => cm.MapFrom(x => x.BirthDate));

            CreateMap<Customer, CustomerModel>()
               .ForMember(cm => cm.Name, c => c.MapFrom(x => x.Person.Name))
               .ForMember(cm => cm.Surname, c => c.MapFrom(x => x.Person.Surname))
               .ForMember(cm => cm.BirthDate, c => c.MapFrom(x => x.Person.BirthDate))
               .ForMember(cm => cm.ReceiptsIds, c => c.MapFrom(x => x.Receipts.Select(r => r.Id)))
               .ReverseMap()
               .ForMember(c => c.Id, cm => cm.Ignore());

            CreateMap<ProductCategory, ProductCategoryModel>()
                .ForMember(pcm => pcm.ProductIds, pc => pc.MapFrom(x => x.Products.Select(p => p.Id)))
                .ReverseMap()
                .ForMember(p => p.Id, pm => pm.Ignore());
        }
    }
}