using TokenGenerator.Application.Dtos;

namespace TokenGenerator.Application.Interfaces.IServices
{
    public interface IAuthService
    {
        string CreateToken(UserDto user);

    }
}