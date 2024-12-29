namespace test.payment
{
    public class PaymentResponseDto
    {
        public string Email { get; set; }
        public decimal Amount { get; set; }
        public bool IsSuccessful { get; set; }
        public string TransactionId { get; set; }
        public string Message { get; set; }
    }
}
