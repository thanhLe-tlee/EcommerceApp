using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceApp.Models
{
    public class Address
    {
        public int Id { get; set; }
        [Required]
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        [Required,StringLength(100)]
        public string Address1 {  get; set; }

        [Required(ErrorMessage = "City is required."),StringLength(50)]
        public string City { get; set; }

        [Required(ErrorMessage = "Country is required."),StringLength(50)]
        public string Country { get; set; }

    }
}
