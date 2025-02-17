using AutoMapper;
using AutoMapper.Collection;
using AutoMapper.EquivalencyExpression;
using Core.Dtos;
using Core.Entities.Concrete;
using TechStore.Api.Models;

namespace TechStore.Api.MapperProfiles
{
    public class DtoMappingProfile : Profile
    {
        public DtoMappingProfile()
        {

            CreateMap<Category, CategoryDto>();
            CreateMap<Category, CategoryMinimalDto>();
            CreateMap<Category, CustomerCategoryDto>();
            CreateMap<CreateCategoryModel, Category>();
            CreateMap<UpdateCategoryModel, Category>().ForMember(c => c.CreatedAt, opt => opt.Ignore());
            CreateMap<Product, CustomerProductDto>();
            CreateMap<Detail, CustomerDetailDto>().ForMember(cddto => cddto.PropName, opt => opt.MapFrom(d => d.Property!.PropName));
            CreateMap<Image, CustomerImageDto>();

            CreateMap<Property, PropertyDto>();
            CreateMap<Property, CustomerProductFiltersDto>().ForMember( d=> d.Values, opt => opt.MapFrom( src => src.Details));
                
            CreateMap<Detail, DetailDto>().ForMember(ddto => ddto.PropName, opt => opt.MapFrom(d => d.Property!.PropName));
            CreateMap<Detail, FilterValueDto>();

            CreateMap<DetailDto, Detail>().EqualityComparison((ddto, d) => ddto.Id == d.Id)
                .ForMember(d => d.PropertyId, opt => opt.Ignore())
                .ForMember(d => d.RowOrder, opt => opt.Ignore())
                .ForMember(d => d.ProductId, opt => opt.Ignore());
            CreateMap<CreateDetailModel, Detail>();
            CreateMap<Detail, DetailMinimalDto>();

            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>().ForMember(p => p.Category, opt => opt.Ignore());

        }
    }
}
