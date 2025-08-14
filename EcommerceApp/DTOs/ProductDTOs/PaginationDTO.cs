namespace EcommerceApp.DTOs.ProductDTOs
{
    public class PaginationDTO
    {
        private int pageSize = 10;
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; }
        public int PageSize
        {
            get => pageSize;
            set => pageSize = value > MaxPageSize ? MaxPageSize : value; 
        }
    }
}
