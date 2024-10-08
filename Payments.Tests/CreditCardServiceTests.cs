﻿using Payments.Services;
using Payments.Models;
using NUnit.Framework;

namespace Payments.Tests
{
    [TestFixture]
    public class CreditCardServiceTests
    {
        private CreditCardService _creditCardService;

        [SetUp]
        public void Setup()
        {
            _creditCardService = new CreditCardService();
        }

        [TestCase("4111111111111111", CardType.Visa, TestName = "ValidateCreditCard_ValidVisa_ReturnsValid")]
        [TestCase("5111111111111111", CardType.MasterCard, TestName = "ValidateCreditCard_ValidMasterCard_ReturnsValid")]
        [TestCase("371449635398431", CardType.AmericanExpress, TestName = "ValidateCreditCard_ValidAmericanExpress_ReturnsValid")]
        public void ValidateCreditCard_ValidCard_ReturnsValid(string cardNumber, CardType expectedCardType)
        {
            var validCard = new CreditCard
            {
                CardOwner = "John Doe",
                Number = cardNumber,
                ExpiryDate = "12/25",
                CVC = expectedCardType == CardType.AmericanExpress ? "1234" : "123" // Adjust CVC based on card type
            };

            var result = _creditCardService.ValidateCreditCard(validCard);

            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(expectedCardType, result.CardType);
            Assert.IsEmpty(result.Errors);
        }

        [Test]
        public void ValidateCreditCard_InvalidCardNumber_ReturnsInvalid()
        {
            var invalidCard = new CreditCard
            {
                CardOwner = "John Doe",
                Number = "1234567890123456", // Invalid
                ExpiryDate = "12/25",
                CVC = "123"
            };

            var result = _creditCardService.ValidateCreditCard(invalidCard);

            Assert.IsFalse(result.IsValid);
            Assert.Contains(ValidationMessages.InvalidCardNumber, result.Errors);
        }

        [Test]
        public void ValidateCreditCard_ExpiredCard_ReturnsInvalid()
        {
            var expiredCard = new CreditCard
            {
                CardOwner = "Jane Doe",
                Number = "4111111111111111",
                ExpiryDate = "12/20", // Expired
                CVC = "123"
            };

            var result = _creditCardService.ValidateCreditCard(expiredCard);

            Assert.IsFalse(result.IsValid);
            Assert.Contains(ValidationMessages.CardExpired, result.Errors);
        }

        [Test]
        public void ValidateCreditCard_InvalidCVCForAmex_ReturnsInvalid()
        {
            var invalidCvcCard = new CreditCard
            {
                CardOwner = "John Doe",
                Number = "371449635398431", // American Express
                ExpiryDate = "12/25",
                CVC = "123" // Amex requires 4 digits
            };

            var result = _creditCardService.ValidateCreditCard(invalidCvcCard);

            Assert.IsFalse(result.IsValid);
            Assert.Contains(ValidationMessages.InvalidCVC, result.Errors);
        }

        [Test]
        public void ValidateCreditCard_EmptyCardOwner_ReturnsInvalid()
        {
            var cardWithEmptyOwner = new CreditCard
            {
                CardOwner = "", // Empty
                Number = "4111111111111111",
                ExpiryDate = "12/25",
                CVC = "123"
            };

            var result = _creditCardService.ValidateCreditCard(cardWithEmptyOwner);

            Assert.IsFalse(result.IsValid);
            Assert.Contains("Card owner is required.", result.Errors);
        }

        [Test]
        public void ValidateCreditCard_InvalidCardOwnerWithNumbers_ReturnsInvalid()
        {
            var invalidOwnerCard = new CreditCard
            {
                CardOwner = "John Doe 4111111111111111", // Contains numbers
                Number = "4111111111111111",
                ExpiryDate = "12/25",
                CVC = "123"
            };

            var result = _creditCardService.ValidateCreditCard(invalidOwnerCard);

            Assert.IsFalse(result.IsValid);
            Assert.Contains(ValidationMessages.CardOwnerInvalid, result.Errors);
        }

        [Test]
        public void ValidateCreditCard_MissingExpiryDate_ReturnsInvalid()
        {
            // Arrange
            var cardWithMissingExpiryDate = new CreditCard
            {
                CardOwner = "John Doe",
                Number = "4111111111111111",
                ExpiryDate = "", // Missing
                CVC = "123"
            };

            // Act
            var result = _creditCardService.ValidateCreditCard(cardWithMissingExpiryDate);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.Contains("Card expiration date is required.", result.Errors);
        }
        
        [Test]
        public void ValidateCreditCard_InvalidExpiryDateFormat_ReturnsInvalid()
        {
            // Arrange
            var cardWithInvalidExpiryDate = new CreditCard
            {
                CardOwner = "John Doe",
                Number = "4111111111111111",
                ExpiryDate = "2025/12", // Invalid format
                CVC = "123"
            };

            // Act
            var result = _creditCardService.ValidateCreditCard(cardWithInvalidExpiryDate);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.Contains(ValidationMessages.InvalidExpiryDate, result.Errors);
        }
        
        [Test]
        public void ValidateCreditCard_MultipleErrors_ReturnsInvalid()
        {
            // Arrange
            var cardWithInvalidExpiryDate = new CreditCard
            {
                CardOwner = "John Doe 4111111111111111", 
                Number = "4111111111",
                ExpiryDate = "2029/09",
                CVC = "12354"
            };

            // Act
            var result = _creditCardService.ValidateCreditCard(cardWithInvalidExpiryDate);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.Contains(ValidationMessages.InvalidExpiryDate, result.Errors);
            Assert.Contains(ValidationMessages.CardOwnerInvalid, result.Errors);
            Assert.Contains(ValidationMessages.InvalidCardNumber, result.Errors);
            Assert.Contains(ValidationMessages.InvalidCVC, result.Errors);
        }
    }
}
