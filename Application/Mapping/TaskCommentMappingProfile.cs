using Application.DTO.Task;
using Application.DTO.TaskComments;
using Application.DTO.Tasks;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping;

public class TaskCommentMappingProfile : Profile
{
    public TaskCommentMappingProfile()
    {
        CreateMap<AddTaskCommentRequest, TaskComment>()
            .ForMember(d => d.Id, opt => opt.Ignore());
        
        CreateMap<UpdateTaskCommentRequest, TaskComment>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

    }
}