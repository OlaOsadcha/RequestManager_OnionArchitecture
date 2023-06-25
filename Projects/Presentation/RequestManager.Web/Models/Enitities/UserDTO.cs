using System.ComponentModel.DataAnnotations;

namespace RequestManager.Web.Models.Enitities
{
    public class UserDTO
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [UIHint("password")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [UIHint("password")]
        [Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again!")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Position { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        [Required]
        public string Role { get; set; }
    }
}