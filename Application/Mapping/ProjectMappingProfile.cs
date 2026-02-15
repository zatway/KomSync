using Application.Projects.Commands;
using Application.Projects.Commands.CreateProject;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping;

public class ProjectMappingProfile : Profile
{
    public ProjectMappingProfile()
    {
        CreateMap<CreateProjectCommand, Project>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
    }
}