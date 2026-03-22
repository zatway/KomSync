using Application.DTO.Projects;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping
{
    public class ProjectMappingProfile : Profile
    {
        public ProjectMappingProfile()
        {
            CreateMap<CreateProjectRequest, Project>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            CreateMap<UpdateProjectRequest, Project>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Project, ProjectBriefDto>()
                .ForMember(d => d.OwnerName, opt => opt.MapFrom(s => s.Owner != null ? s.Owner.FullName : "Не назначен"));

            CreateMap<Project, MemberDto>();

            CreateMap<ProjectComment, ProjectCommentDto>()
                .ForMember(d => d.Author, opt => opt.MapFrom(s => s.Author))
                .ForMember(d => d.Replies, opt => opt.MapFrom(s => s.Children));

            CreateMap<User, AuthorDto>();

            CreateMap<ProjectHistory, ProjectHistoryEntryDto>()
                .ForMember(d => d.ChangedBy, opt => opt.MapFrom(s => s.ChangedBy));
            
            CreateMap<User, ChangedByDto>();
        }
    }
}