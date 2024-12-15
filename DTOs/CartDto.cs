namespace test.DTOs
{
    public class CartDto
    {
        public int Id { get; set; }
        public ICollection<CartItemDto> CartItems { get; set; }
    }

    public class CreateCartDto
    {
        public ICollection<CreateCartItemDto> CartItems { get; set; }
    }

    public class UpdateCartDto
    {
        public int Id { get; set; }
        public ICollection<UpdateCartItemDto> CartItems { get; set; }
    }
}
