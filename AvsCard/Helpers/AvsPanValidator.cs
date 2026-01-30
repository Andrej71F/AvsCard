using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvsCard.Helpers
{
    /// <summary>
    /// Provides validation utilities for AVS PAN (Primary Account Number) values,
    /// including length checks, prefix validation, digit-only enforcement,
    /// and Luhn checksum verification.
    /// </summary>
    public static class AvsPanValidator
    {
        #region Private Methods

        /// <summary>
        /// Performs a Luhn checksum validation on the provided numeric string.
        /// </summary>
        /// <param name="number">The numeric string to validate.</param>
        /// <returns>True if the number passes the Luhn algorithm; otherwise false.</returns>
        private static bool IsValidLuhn(string number)
        {
            int sum = 0;
            bool doubleDigit = false;

            for (int i = number.Length - 1; i >= 0; i--)
            {
                int digit = number[i] - '0';

                if (doubleDigit)
                {
                    digit *= 2;
                    if (digit > 9)
                        digit -= 9;
                }

                sum += digit;
                doubleDigit = !doubleDigit;
            }

            return sum % 10 == 0;
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Validates the provided PAN according to AVS rules:
        /// correct length, digit-only content, required prefix,
        /// and successful Luhn checksum.
        /// </summary>
        /// <param name="pan">The PAN value to validate.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when the PAN is null, empty, has invalid length,
        /// contains non-digit characters, has an incorrect prefix,
        /// or fails the Luhn check.
        /// </exception>
        public static void ValidatePan(string pan)
        {
            if (string.IsNullOrWhiteSpace(pan))
                throw new ArgumentException("PAN is required.", nameof(pan));

            if (pan.Length != AvsConstants.PanLength)
                throw new ArgumentException($"PAN must be {AvsConstants.PanLength} digits.");

            if (!pan.All(char.IsDigit))
                throw new ArgumentException("PAN must contain digits only.");

            if (!pan.StartsWith(AvsConstants.PanPrefix))
                throw new ArgumentException($"PAN must start with {AvsConstants.PanPrefix}.");

            if (!IsValidLuhn(pan))
                throw new ArgumentException("PAN failed Luhn check.");
        }

        #endregion Public Methods
    }
}