using System.ComponentModel.DataAnnotations;

namespace TokenGenerator.Api.Models.Requests
{
    public class VerifyTokenRequest
    {
        [Required]
        public string AccessToken { get; set; }

    }
}
