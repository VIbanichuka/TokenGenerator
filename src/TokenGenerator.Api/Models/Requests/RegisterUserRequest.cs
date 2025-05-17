using System.ComponentModel.DataAnnotations;

namespace TokenGenerator.Api.Models.Requests
{
    public class RegisterUserRequest
    {
        [Required(ErrorMessage = "Field required")]
        [EmailAddress(ErrorMessage = "Invalid EmailAddress")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required, MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
