using System.ComponentModel.DataAnnotations;

namespace Speurzoekers.Service.Dto.User
{
    public class RegistrationDto
    {
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
