namespace test.DTOs
{
    public class PatchCartDto
    {
        public ICollection<CreateCartItemDto> CartItems { get; set; }
    }
}
