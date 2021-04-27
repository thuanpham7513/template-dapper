using System.ComponentModel.DataAnnotations;

namespace DapperTemplate.Models
{
    public class UserRequestModel
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [StringLength(20)]
        public string Email { get; set; }

        [Range(1, 100)]
        public int Age { get; set; }

        [Required]
        [StringLength(20)]
        public string Username { get; set;}

        [Required]
        [StringLength(20)]
        public string Password { get; set; }
    }
}
