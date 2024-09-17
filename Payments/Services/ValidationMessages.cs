namespace Payments.Services
{
    public static class ValidationMessages
    {
        public const string CardNumberRequired = "Card number is required.";
        public const string InvalidCardNumber = "Invalid card number.";
        public const string CardCvcRequired = "Card cvc is required.";
        public const string InvalidCVC = "Invalid CVC.";
        public const string CardOwnerRequired = "Card owner is required.";
        public const string CardOwnerInvalid = "Card owner field should contain only letters.";
        public const string ExpiryDateRequired = "Card expiration date is required.";
        public const string InvalidExpiryDate = "Invalid card expiration date.";
        public const string CardExpired = "Card has expired.";
    }
}
