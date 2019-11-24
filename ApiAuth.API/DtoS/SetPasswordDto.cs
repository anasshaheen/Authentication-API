using System.ComponentModel.DataAnnotations;

namespace ApiAuth.API.DtoS
{
    public class SetPasswordDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }
    }
}
