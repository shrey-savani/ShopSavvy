using System.ComponentModel.DataAnnotations;

namespace ShopSavvy.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int? UserId { get; set; }

        public User? User { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        public string? OrderItems { get; set; }

        [Required]
        [StringLength(100)]
        public string ShippingAddress { get; set; }

        [Required]
        [StringLength(50)]
        public string City { get; set; }

        [Required]
        [StringLength(50)]
        public string State { get; set; }

        [Required]
        [StringLength(10)]
        public string ZipCode { get; set; }

        [Required]
        [RegularExpression(@"^\d{3}-\d{3}-\d{4}$", ErrorMessage = "Please enter a valid phone number i.e. 123-456-7890.")]
        public string PhoneNumber { get; set; }
    }
}
