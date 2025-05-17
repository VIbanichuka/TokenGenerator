using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenGenerator.Application.Interfaces.IRepositories;
using TokenGenerator.Domain.Entities;
using TokenGenerator.Infrastructure.Data;

namespace TokenGenerator.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(TokenGeneratorDbContext context) : base(context)
        {
        }
    }
}
