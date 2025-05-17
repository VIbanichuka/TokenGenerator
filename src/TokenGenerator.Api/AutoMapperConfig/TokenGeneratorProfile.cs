using AutoMapper;
using TokenGenerator.Api.Models.Requests;
using TokenGenerator.Api.Models.Responses;
using TokenGenerator.Application.Dtos;
using TokenGenerator.Domain.Entities;

namespace TokenGenerator.Api.AutoMapperConfig
{
    public class TokenGeneratorProfile: Profile
    {
        public TokenGeneratorProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<RegisterUserRequest, UserDto>().ReverseMap();
            CreateMap<UpdateUserRequest, UserDto>().ReverseMap();
            CreateMap<UserResponse, UserDto>().ReverseMap();
            CreateMap<Token, TokenDto>().ReverseMap();
            CreateMap<TokenResponse, TokenDto>().ReverseMap();
            CreateMap<GenerateTokenRequest, TokenDto>().ReverseMap();
            CreateMap<VerifyTokenRequest, TokenDto>().ReverseMap();         
        }
    }
}
