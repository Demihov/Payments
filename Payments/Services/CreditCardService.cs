﻿using Payments.Models;
using System.Text.RegularExpressions;

namespace Payments.Services
{
    public class CreditCardService : ICreditCardService
    {
        private const string CardOwnerPattern = @"^[A-Za-z\s]+$";
        private const string VisaPattern = @"^4[0-9]{12}(?:[0-9]{3})?$";
        private const string MasterCardPattern = @"^5[1-5][0-9]{14}$";
        private const string AmericanExpressPattern = @"^3[47][0-9]{13}$";
        private const string ExpiryDatePattern = @"^(0[1-9]|1[0-2])/(2[0-9])$";

        public CreditCardValidationResult ValidateCreditCard(CreditCard card)
        {
            var errors = new List<string>();
            CardType cardType = GetCardType(card.Number);

            ValidateCardOwner(card.CardOwner, errors);
            ValidateCardNumber(card.Number, cardType, errors);
            ValidateCardCVC(card.CVC, cardType, errors);
            ValidateExpiryDate(card.ExpiryDate, errors);

            return new CreditCardValidationResult()
            {
                IsValid = errors.Count == 0,
                Errors = errors.ToArray(),
                CardType = cardType
            };
        }

        private CardType GetCardType(string cardNumber)
        {
            var cardPatterns = new Dictionary<CardType, string>
            {
                { CardType.Visa, VisaPattern },
                { CardType.MasterCard, MasterCardPattern },
                { CardType.AmericanExpress, AmericanExpressPattern }
            };

            foreach (var pattern in cardPatterns)
            {
                if (Regex.IsMatch(cardNumber, pattern.Value))
                {
                    return pattern.Key;
                }
            }

            return CardType.Unknown;
        }

        private void ValidateCardNumber(string cardNumber, CardType cardType, List<string> errors)
        {
            if (FieldIsNullOrEmpty(cardNumber, "Card number", errors))
                return;

            if (cardType == CardType.Unknown)
            {
                errors.Add(ValidationMessages.InvalidCardNumber);
            }
        }

        private void ValidateCardCVC(string cardCvc, CardType cardType, List<string> errors)
        {
            if (FieldIsNullOrEmpty(cardCvc, "Card CVC", errors))
                return;

            if (!ValidateCVC(cardCvc, cardType))
            {
                errors.Add(ValidationMessages.InvalidCVC);
            }
        }

        private bool ValidateCVC(string cvc, CardType cardType)
        {
            switch (cardType)
            {
                case CardType.Visa:
                case CardType.MasterCard:
                    return Regex.IsMatch(cvc, @"^\d{3}$");
                case CardType.AmericanExpress:
                    return Regex.IsMatch(cvc, @"^\d{4}$");
                default:
                    return false;
            }
        }

        private void ValidateCardOwner(string cardOwner, List<string> errors)
        {
            if (FieldIsNullOrEmpty(cardOwner, "Card owner", errors))
                return;

            if (!Regex.IsMatch(cardOwner, CardOwnerPattern))
            {
                errors.Add(ValidationMessages.CardOwnerInvalid);
            }
        }

        private void ValidateExpiryDate(string expiryDate, List<string> errors)
        {
            if (FieldIsNullOrEmpty(expiryDate, "Card expiration date", errors))
                return;

            if (Regex.IsMatch(expiryDate, ExpiryDatePattern))
            {
                const int yearOffset = 2000;
                var currentYear = DateTime.Now.Year;
                var currentMonth = DateTime.Now.Month;

                var parts = expiryDate.Split('/');
                var month = int.Parse(parts[0]);
                var year = int.Parse(parts[1]);

                var expiryYear = yearOffset + year;

                if (expiryYear < currentYear || (expiryYear == currentYear && month < currentMonth))
                {
                    errors.Add(ValidationMessages.CardExpired);
                }
            }
            else
            {
                errors.Add(ValidationMessages.InvalidExpiryDate);
            }
        }

        private bool FieldIsNullOrEmpty(string value, string fieldName, List<string> errors)
        {
            if (string.IsNullOrEmpty(value))
            {
                errors.Add($"{fieldName} is required.");
                return true;
            }
            return false;
        }
    }
}
