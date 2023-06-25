using AutoMapper;
using RequestManager.Core.Domain.Entities;
using RequestManager.Web.Models.Enitities;
using System.Data;

namespace RequestManager.Web.Mappings
{
    public class ModelMappings : Profile
    {
        public ModelMappings()         
        {
            CreateMap<RequestUser, UserDTO>().ReverseMap();

            CreateMap<Department, DepartmentDTO>().ReverseMap();

            CreateMap<RequestRole, RoleDTO>().ReverseMap();

            CreateMap<SubDepartment, SubDepartmentDTO>().ReverseMap();

            CreateMap<Category, CategoryDTO>().ReverseMap();

            CreateMap<Lifecycle, LifecycleDTO>().ReverseMap();

            CreateMap<Request, RequestDto>().ReverseMap();

            CreateMap<RequestUser, RequestUserDto>().ReverseMap();
        }
    }
}