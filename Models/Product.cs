namespace EshopMidtrans.Models
{
    public class Product
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public required string ImageUrl { get; set; }
        public DateTime? DeletedAt { get; set; } // untuk soft delete
    }
}
