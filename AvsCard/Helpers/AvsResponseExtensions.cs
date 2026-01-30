using AvsCard.ResponseDto;

namespace AvsCard.Helpers
{
    /// <summary>
    /// Provides extension methods for working with AVS responses,
    /// including result code conversion, error description lookup,
    /// and success state evaluation.
    /// </summary>
    public static class AvsResponseExtensions
    {
        #region Public Methods

        /// <summary>
        /// Converts the numeric AVS result value into a strongly typed <see cref="AvsResultCode"/>.
        /// Throws an exception if the result code is not recognized.
        /// </summary>
        /// <param name="response">The AVS response instance.</param>
        /// <returns>The corresponding <see cref="AvsResultCode"/>.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the result code is not defined in <see cref="AvsResultCode"/>.
        /// </exception>
        public static AvsResultCode ToResultCode(this AvsResponseBase response)
        {
            if (Enum.IsDefined(typeof(AvsResultCode), response.Result))
                return (AvsResultCode)response.Result;

            throw new InvalidOperationException(
                $"Unknown AVS result code: {response.Result}");
        }

        /// <summary>
        /// Returns a human‑readable description for the AVS result code
        /// associated with the given response.
        /// </summary>
        /// <param name="response">The AVS response instance.</param>
        /// <returns>A descriptive error message.</returns>
        public static string GetErrorDescription(this AvsResponseBase response)
        {
            return AvsErrorDescriptions.GetDescription(response.Result);
        }

        /// <summary>
        /// Determines whether the AVS response indicates a successful operation.
        /// </summary>
        /// <param name="response">The AVS response instance.</param>
        /// <returns>True if the result code equals <see cref="AvsResultCode.Success"/>; otherwise false.</returns>
        public static bool IsSuccess(this AvsResponseBase response)
        {
            return response.Result == (int)AvsResultCode.Success;
        }

        #endregion Public Methods
    }
}