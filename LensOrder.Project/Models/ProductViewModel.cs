using System.ComponentModel.DataAnnotations;

namespace LensOrder.Project.Models
{
    public class ProductViewModel
    {
        [Required]
        [StringLength(50)]
        public string FullName { get; set; }

        [Required]
        [StringLength(30)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(100)]
        public string Address { get; set; }

        [Required]
        [StringLength(30)]
        public string PaymentMethod { get; set; }

        [Required]
        [StringLength(30)]
        public string DeliveryMethod { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductName { get; set; }

        [Required]
        public int ProductPrice { get; set; }

        [Required]
        public int ProductCount { get; set; }
    }
}
