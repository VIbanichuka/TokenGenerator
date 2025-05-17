using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenGenerator.Domain.Entities
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }

        [Required]
        [MaxLength(300)]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public bool IsEmailVerified { get; set; }

        public string? VerificationToken { get; set; }

        public DateTimeOffset? TokenExpirationTime { get; set; } = DateTimeOffset.MinValue;

        [Required]
        public byte[] PasswordHash { get; set; } = new byte[32];

        [Required]
        public byte[] PasswordSalt { get; set; } = new byte[32];

        public virtual Token Token { get; set; }

    }
}
