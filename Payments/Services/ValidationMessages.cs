namespace Payments.Services
{
    public static class ValidationMessages
    {
        public const string InvalidCardNumber = "Invalid card number.";
        public const string InvalidCVC = "Invalid CVC.";
        public const string CardOwnerInvalid = "Card owner field should contain only letters.";
        public const string InvalidExpiryDate = "Invalid card expiration date.";
        public const string CardExpired = "Card has expired.";
    }
}
