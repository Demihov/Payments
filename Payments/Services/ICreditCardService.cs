using Payments.Models;

namespace Payments.Services
{
    public interface ICreditCardService
    {
        CreditCardValidationResult ValidateCreditCard(CreditCard card);
    }
}
