
namespace Application.Mapping;

using AutoMapper;
using Domain.Entities;
using Application.DTO.Auth;

public class AuthMappingProfile : Profile
{
    public AuthMappingProfile()
    {
        CreateMap<LoginRequest, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) // Пароль маппим сами
            .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()));

        CreateMap<RegisterRequest, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) // Пароль маппим сами
            .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
            .ForMember(dest => dest.NormalizedEmail, opt => opt.MapFrom(_ => _.Email));
    }
}