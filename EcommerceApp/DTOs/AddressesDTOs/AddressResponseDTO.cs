namespace EcommerceApp.DTOs.AddressesDTOs
{
    public class AddressResponseDTO
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Address1 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}
