using System.ComponentModel.DataAnnotations;

namespace TokenGenerator.Api.Models.Requests
{
    public class GenerateTokenRequest
    {
        [Required]
        public DateTimeOffset AccessTokenExpiryDate { get; set; }
    }
}
