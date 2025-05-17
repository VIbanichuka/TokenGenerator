using TokenGenerator.Application.Dtos;
using TokenGenerator.Application.Interfaces.IRepositories;
using TokenGenerator.Domain.Entities;

namespace TokenGenerator.Application.Interfaces.IServices
{
    public interface IUserService
    {
        Task<UserDto> CreateUserAsync(UserDto user);

        Task<bool> DeleteUserAsync(Guid userId);

        Task<UserDto> UpdateUserAsync(UserDto user);

        Task<UserDto> GetUserByEmailAsync(string email);

        Task<UserDto?> GetUserByVerificationTokenAsync(string token);

    }
}