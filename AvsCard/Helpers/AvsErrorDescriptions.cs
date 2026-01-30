namespace AvsCard.Helpers
{
    /// <summary>
    /// Provides human‑readable descriptions for AVS result codes,
    /// including both official AVS codes and custom client‑side error codes.
    /// </summary>
    public static class AvsErrorDescriptions
    {
        #region Private Fields

        /// <summary>
        /// Internal mapping between <see cref="AvsResultCode"/> values
        /// and their corresponding descriptive text.
        /// </summary>
        private static readonly Dictionary<AvsResultCode, string> _map =
            new()
            {
                { AvsResultCode.Success, "transaction successful" },
                { AvsResultCode.CardUnknown, "card unknown" },
                { AvsResultCode.CardExpired, "card expired" },
                { AvsResultCode.CardNotActive, "card not active" },
                { AvsResultCode.TransactionNotFound, "transaction not found" },
                { AvsResultCode.InvalidAmount, "invalid amount" },
                { AvsResultCode.NoAccountFound, "no account found for card" },
                { AvsResultCode.CardLimitExceeded, "card limit exceeded" },
                { AvsResultCode.TransactionTypeNotAllowed, "transaction type not allowed" },
                { AvsResultCode.WrongPin, "wrong PIN" },
                { AvsResultCode.WrongPinTriesExceeded, "wrong PIN - tries exceeded" },
                { AvsResultCode.SystemError9997, "wrong credentials at login (username or password)" },
                { AvsResultCode.SystemError9998, "system error: 9998" },
                { AvsResultCode.SystemError9999, "system error: 9999" },

                // Custom client-side error descriptions
                { AvsResultCode.HttpTimeout, "http timeout" },
                { AvsResultCode.HttpNetworkError, "http network error" },
                { AvsResultCode.HttpUnexpectedError, "unexpected http error" },
                { AvsResultCode.UnknownError, "unknown system error" },

                // New validation-related errors
                { AvsResultCode.DataValidationError, "data validation error" },
                { AvsResultCode.InvalidRequest, "invalid request" }
            };

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// Returns a human‑readable description for the specified AVS result code.
        /// If the code is not recognized, a generic "unknown system error" description is returned.
        /// </summary>
        /// <param name="code">The numeric AVS result code.</param>
        /// <returns>A descriptive text for the result code.</returns>
        public static string GetDescription(int code)
        {
            if (Enum.IsDefined(typeof(AvsResultCode), code))
            {
                var enumCode = (AvsResultCode)code;
                if (_map.TryGetValue(enumCode, out var desc))
                    return desc;
            }

            return _map[AvsResultCode.UnknownError];
        }

        #endregion Public Methods
    }
}