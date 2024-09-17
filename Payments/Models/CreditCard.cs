namespace Payments.Models
{
    public class CreditCard
    {
        public string? CardOwner { get; set; }
        public string? Number { get; set; }
        public string? ExpiryDate { get; set; }
        public string? CVC { get; set; }
    }
}
