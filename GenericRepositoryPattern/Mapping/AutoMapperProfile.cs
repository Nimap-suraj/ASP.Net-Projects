using AutoMapper;
using GenericRepositoryPattern.DTOs;
using GenericRepositoryPattern.Entity;
using System.Runtime.CompilerServices;

namespace GenericRepositoryPattern.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<CategoryCreateDTO, Category>();

            // Product Mappings
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.CategoryName,
                           opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null));

            CreateMap<ProductCreareDTO, Product>();


        }


    }
}
