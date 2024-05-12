using AutoMapper;
using Decor_DataAccess;
using Decor_DataAccess.ViewModel;
using Decor_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decor_Business.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile() { 
            CreateMap<Category,CategoryDTO>().ReverseMap();
            CreateMap<Product,ProductDTO>().ReverseMap();
            CreateMap<ProductPrice,ProductPriceDTO>().ReverseMap();
            CreateMap<OrderDetails,OrderDetailsDTO>().ReverseMap();
            CreateMap<OrderHeader,OrderHeaderDTO>().ReverseMap();
            CreateMap<Order, OrderDTO>().ReverseMap();
        }
    }
}
