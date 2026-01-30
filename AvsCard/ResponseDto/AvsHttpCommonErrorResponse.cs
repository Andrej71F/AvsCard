namespace AvsCard.ResponseDto
{
    /// <summary>
    /// Represents a synthetic AVS response used when an HTTP‑level error occurs
    /// before a valid AVS XML response can be received or parsed.
    /// This class wraps transport‑layer failures (timeouts, network errors, etc.)
    /// into a standard <see cref="AvsResponseBase"/> structure.
    /// </summary>
    public class AvsHttpCommonErrorResponse : AvsResponseBase
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="AvsHttpCommonErrorResponse"/>
        /// using the specified result code and error description.
        /// Populates all AVS response fields with safe defaults.
        /// </summary>
        /// <param name="resultCode">The AVS-style error code representing the HTTP failure.</param>
        /// <param name="desciption">A human-readable description of the error.</param>
        public AvsHttpCommonErrorResponse(int resultCode, string desciption)
        {
            Type = "HTTP_ERROR";
            Result = resultCode; // automatically fills ResultText via base logic if applicable

            ResultText = desciption;

            ProfileName = "";
            TerminalId = "";
            TxId = "";
            LocalDateTime = "";
            ServerDateTime = "";
            Card = new AvsResponseCard();
        }

        #endregion Public Constructors
    }
}