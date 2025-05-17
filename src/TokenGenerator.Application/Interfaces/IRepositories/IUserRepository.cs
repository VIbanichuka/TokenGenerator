using TokenGenerator.Application.Dtos;
using TokenGenerator.Domain.Entities;

namespace TokenGenerator.Application.Interfaces.IRepositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
    }
}