using Application.DTO.Projects;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping;

public class ProjectMappingProfile : Profile
{
    public ProjectMappingProfile()
    {
        // 1. Создание проекта
        CreateMap<CreateProjectRequest, Project>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
        
        // 2. Частичное обновление (Patch)
        CreateMap<UpdateProjectRequest, Project>()
            // Игнорируем Id при маппинге, чтобы случайно не сменить ID проекта
            .ForMember(dest => dest.Id, opt => opt.Ignore()) 
            // Мапим только те поля, которые не null в запросе
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
       
        // 3. Маппинг для списков и деталей (Projection)
        // Используй .ProjectTo<ProjectBriefDto>(configuration) в репозитории/хендлере
        CreateMap<Project, ProjectBriefDto>()
            .ForMember(d => d.OwnerName, opt => opt.MapFrom(s => s.Owner != null ? s.Owner.FullName : "Не назначен"));

        CreateMap<Project, ProjectDetailedDto>()
            .ForMember(d => d.OwnerName, opt => opt.MapFrom(s => s.Owner != null ? s.Owner.FullName : "Не назначен"));
    }
}