using System.ComponentModel.DataAnnotations;

namespace TokenGenerator.Api.Models.Requests
{
    public class UserLoginRequest
    {
        [Required(ErrorMessage = "Field required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}