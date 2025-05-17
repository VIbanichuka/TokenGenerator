
namespace TokenGenerator.Application.Dtos
{
    public class UserDto
    {
        public Guid UserId { get; set; }

        public string Email { get; set; }

        public bool IsEmailVerified { get; set; }

        public string? VerificationToken { get; set; }

        public DateTimeOffset? TokenExpirationTime { get; set; }

        public byte[] PasswordHash { get; set; } = new byte[32];

        public byte[] PasswordSalt { get; set; } = new byte[32];

    }
}
