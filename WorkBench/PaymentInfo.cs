namespace WorkBench
{
    public class PaymentInfo
    {
        public string Token { get; set; } = string.Empty;
        public string ApprovalNum { get; set; } = string.Empty;
        public string CardName { get; set; } = string.Empty;
         public string ExpiryDate { get; set; } = string.Empty;
         public string Amount { get; set; }
        public DateTime PaymentDateTime { get; set; }
    }
}
