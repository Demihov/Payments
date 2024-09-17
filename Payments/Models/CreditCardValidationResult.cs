namespace Payments.Models
{
    public class CreditCardValidationResult
    {
        public bool IsValid { get; set; }
        public CardType CardType { get; set; }
        public string[] Errors { get; set; }
    }
}
