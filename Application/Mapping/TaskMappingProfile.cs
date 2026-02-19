using Application.DTO.Tasks;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping;

public class TaskMappingProfile : Profile
{
    public TaskMappingProfile()
    {
        CreateMap<CreateTaskRequest, ProjectTask>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.CreatorId, opt => opt.Ignore())
            .ForMember(d => d.History, opt => opt.Ignore());

        CreateMap<UpdateTaskRequest, ProjectTask>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<ChangeTaskStatusCommand, ProjectTask>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<AssignUserRequest, ProjectTask>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<ProjectTask, TaskDetailedDto>()
            .ForMember(d => d.AssigneeId,
                opt => opt.MapFrom(s => s.Assignee != null ? s.Assignee.FullName : "Не назначен"));
        
        CreateMap<ProjectTask, TaskShortDto>()
            .ForMember(d => d.AssigneeId,
                opt => opt.MapFrom(s => s.Assignee != null ? s.Assignee.FullName : "Не назначен"));
    }
}