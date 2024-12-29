namespace test.payment
{
    public class PaymentRequestDto
    {
        public string Email { get; set; }
        public string CardNumber { get; set; }
        public string Date { get; set; }
        public string Password { get; set; }
        public decimal Amount { get; set; }
         
    }
}
