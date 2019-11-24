using System.ComponentModel.DataAnnotations;

namespace ApiAuth.API.DtoS
{
    public class UpdateUserDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Name { get; set; }
    }
}
