using System.ComponentModel.DataAnnotations;

namespace TokenGenerator.Api.Models.Requests
{
    public class UpdateUserRequest
    {
        [Required(ErrorMessage = "Field required")]
        [EmailAddress(ErrorMessage = "Invalid EmailAddress")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
