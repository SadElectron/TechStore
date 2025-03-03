using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Core.Entities.Concrete;
using TechStore.Api.Dtos;
using TechStore.Api.Models.Category;
using TechStore.Api.Models.Detail;
using TechStore.Api.Models.Product;
using TechStore.Api.Models.Property;

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
            
            CreateMap<Detail, CustomerDetailDto>().ForMember(cddto => cddto.PropName, opt => opt.MapFrom(d => d.Property!.PropName));
            CreateMap<Image, CustomerImageDto>();
            CreateMap<Image, ImageDto>();

            CreateMap<Property, PropertyDto>();
            CreateMap<Property, CustomerProductFiltersDto>().ForMember( d=> d.Values, opt => opt.MapFrom( src => src.Details));
            CreateMap<CreatePropertyModel, Property>();
            CreateMap<UpdatePropertyModel, Property>();

            CreateMap<Detail, DetailDto>()
                .ForMember(ddto => ddto.PropName, opt => opt.MapFrom(d => d.Property!.PropName))
                .ForMember(ddto => ddto.RowOrder, opt => opt.MapFrom(d => d.Property!.PropOrder));
            CreateMap<Detail, FilterValueDto>();
            CreateMap<DetailDto, Detail>().EqualityComparison((ddto, d) => ddto.Id == d.Id)
                .ForMember(d => d.PropertyId, opt => opt.Ignore())
                .ForMember(d => d.RowOrder, opt => opt.Ignore())
                .ForMember(d => d.ProductId, opt => opt.Ignore());
            CreateMap<CreateDetailModel, Detail>();
            CreateMap<UpdateDetailModel, Detail>();
            CreateMap<Detail, DetailMinimalDto>();

            CreateMap<Product, ProductDto>();
            CreateMap<Product, CustomerProductDto>()
            .ForMember(dest => dest.Details, opt => opt.MapFrom(src => src.Details
                .OrderBy(d => d.Property!.PropOrder) 
                .Select(d => new CustomerDetailDto
                {
                    PropName = d.Property!.PropName,
                    PropValue = d.PropValue
                }).ToList()))
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images
                .OrderBy(i => i.ImageOrder) 
                .Select(i => new CustomerImageDto
                {
                    File = i.File
                }).ToList()));
            CreateMap<ProductDto, Product>().ForMember(p => p.Category, opt => opt.Ignore());
            CreateMap<CreateProductModel, Product>();
            CreateMap<UpdateProductModel, Product>();
        }
    }
}
