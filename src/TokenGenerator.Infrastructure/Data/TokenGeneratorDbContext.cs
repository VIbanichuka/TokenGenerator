using Microsoft.EntityFrameworkCore;
using TokenGenerator.Domain.Entities;

namespace TokenGenerator.Infrastructure.Data
{
    public class TokenGeneratorDbContext : DbContext
    {
        public TokenGeneratorDbContext(DbContextOptions<TokenGeneratorDbContext> options): base(options)
        {
        }

        public TokenGeneratorDbContext(){}
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Token> Tokens { get; set; }

    }
}
