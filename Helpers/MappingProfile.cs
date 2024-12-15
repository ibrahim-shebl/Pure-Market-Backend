using AutoMapper;
using test.Dtos;
using test.DTOs;
using test.Models;

namespace test.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDetailsDto>();
            CreateMap<Contact, ContactusDetails>();
            CreateMap<ContactUsDto, Contact>();
            CreateMap<Banner, BannerProductDetailsDto>();
            CreateMap<Review, ReviewsDetailsDto>();
            CreateMap<ProductDto, Product>()
                .ForMember(src => src.CatImgPath, opt => opt.Ignore());
            CreateMap<BannerProductDto, Banner>()
                .ForMember(src => src.CatImgPath, opt => opt.Ignore());
            CreateMap<ReviewDto, Review>()
                .ForMember(src => src.CatImgPath, opt => opt.Ignore());
        }
        
    }
}
