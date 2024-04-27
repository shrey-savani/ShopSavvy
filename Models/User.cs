using System.ComponentModel.DataAnnotations;

namespace ShopSavvy.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(5)]
        public string Password { get; set; }

        [Required]
        [StringLength(10)]
        public string Role { get; set; } = "User";
    }
}
