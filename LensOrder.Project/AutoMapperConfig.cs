using AutoMapper;
using LensOrder.Project.Entities;
using LensOrder.Project.Models;

namespace LensOrder.Project
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Order, ProductViewModel>().ReverseMap();
            CreateMap<Order, OrderViewModel>().ReverseMap();
        }
    }
}
