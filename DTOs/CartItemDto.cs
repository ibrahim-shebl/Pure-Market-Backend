namespace test.DTOs
{
    public class CartItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }  
        public int Quantity { get; set; }
    }

    public class CartResponseDto
    {
        public int Id { get; set; }
        public List<CartItemDto> CartItems { get; set; }
    }

    public class CreateCartItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class UpdateCartItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
