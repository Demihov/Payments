# Credit Card Validation Service

## Overview

This project implements a credit card validation service using C# and .NET. The service provides functionality to validate various aspects of credit card information, including card number, card owner, CVC, and expiry date. The service also identifies the card type (Visa, MasterCard, or American Express).

## Features

- **Credit Card Validation:** Validates card number, CVC, expiry date, and card owner.
- **Card Type Detection:** Identifies if the card is a Visa, MasterCard, or American Express.
- **Error Handling:** Provides detailed error messages for invalid input.

## Components

### 1. `CreditCardService` Class

This class contains methods to validate credit card information. It includes:
- `ValidateCreditCard`: Validates the entire credit card information.
- `ValidateCardNumber`: Validates the card number and detects the card type.
- `ValidateCardCVC`: Validates the CVC based on card type.
- `ValidateCardOwner`: Ensures the card owner field contains only letters.
- `ValidateExpiryDate`: Checks if the expiry date is valid and not expired.

### 2. Error Messages

Error messages are used for validation feedback. They are defined in constants within the `CreditCardService` class.

### 3. Unit Tests

Unit tests are implemented using NUnit to ensure the correctness of the validation logic. The tests cover various scenarios including valid and invalid card numbers, expired cards, and incorrect CVCs.


## Contributing

Feel free to submit issues or pull requests to improve the project. Please ensure your contributions adhere to the coding standards used in this project.
