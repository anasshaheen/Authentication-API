using System.ComponentModel.DataAnnotations;

namespace ApiAuth.API.DtoS
{
    public class RegisterDto
    {
        [Required]
        [MaxLength(100)]
        [Display(Name = "Office Name")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}