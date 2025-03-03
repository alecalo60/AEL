using AEL.Application.DTOs;
using AEL.Domain.Entities;
using AutoMapper;


namespace Hotel.Infrastructure.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Employee, EmployeeDTO>()
            .ForMember(dest => dest.Department, opt => opt.MapFrom(src => new Department
            {
                Id = src.Department.Id,
                Name = src.Department.Name,
                CreatedAt = src.Department.CreatedAt,
                UpdatedAt = src.Department.UpdatedAt,
                Employees = null 
            }));

            CreateMap<Department, DepartmentDTO>()
                .ForMember(dest => dest.Employees, opt => opt.Ignore());

            CreateMap<CreateDepartmentDTO, Department>();
            CreateMap<UpdateDepartmentDTO, Department>();
            CreateMap<CreateEmployeeDTO, Employee>();
            CreateMap<UpdateEmployeeDTO, Employee>();


        }
    }
}
