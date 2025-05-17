using System.ComponentModel.DataAnnotations;

namespace TokenGenerator.Domain.Entities
{
    public class Token
    {
        [Key]
        public Guid TokenId { get; set; }
        
        public Guid UserId { get; set; }
        
        public string AccessToken { get; set; } = string.Empty;
        
        public DateTimeOffset AccessTokenExpiryDate { get; set; }

        public virtual User User { get; set; } = null!;
    }
}