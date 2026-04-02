
namespace Application.Mapping;

using Domain.Entities;
using Application.DTO.Auth;
using Domain.Enums;

public class AuthMappingProfile :  AutoMapper.Profile
{
    public AuthMappingProfile()
    {
        CreateMap<RegisterRequest, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.Avatar, opt => opt.Ignore())
            .ForMember(dest => dest.Role, opt => opt.MapFrom(_ => UserRole.Employee))
            .ForMember(dest => dest.IsApproved, opt => opt.MapFrom(_ => false))
            .ForMember(dest => dest.NormalizedEmail, opt =>
                opt.MapFrom(src => src.Email.Trim().ToUpper()))
            .ForMember(dest => dest.DepartmentId, opt =>
                opt.MapFrom(src => Guid.Parse(src.DepartmentId)))
            .ForMember(dest => dest.PositionId, opt =>
                opt.MapFrom(src => Guid.Parse(src.PositionId)));
    }
}
