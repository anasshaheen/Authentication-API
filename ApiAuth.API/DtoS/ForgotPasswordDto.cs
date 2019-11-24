using System.ComponentModel.DataAnnotations;

namespace ApiAuth.API.DtoS
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

}
