using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvsCard.Helpers
{
    /// <summary>
    /// Represents the supported AVS transaction types.
    /// </summary>
    public enum AvsTransactionType
    {
        REFUND,

        REDEEM,

        BALANCE,

        CANCEL
    }

    /// <summary>
    /// Represents AVS result codes returned by the AVS system,
    /// including both official AVS codes and custom client-side error codes.
    /// </summary>
    public enum AvsResultCode
    {
        Success = 0,

        CardUnknown = 2,

        CardExpired = 4,

        CardNotActive = 6,

        TransactionNotFound = 8,

        InvalidAmount = 9,

        NoAccountFound = 10,

        CardLimitExceeded = 11,

        TransactionTypeNotAllowed = 13,

        WrongPin = 23,

        WrongPinTriesExceeded = 700,

        SystemError9997 = 9997,

        SystemError9998 = 9998,

        SystemError9999 = 9999,

        // Custom client-side error codes
        HttpTimeout = 100000,

        HttpNetworkError = 100001,

        HttpUnexpectedError = 100002,

        UnknownError = 999999
    }

    /// <summary>
    /// Contains constant values used throughout the AVS client implementation.
    /// </summary>
    public static class AvsConstants
    {
        #region Public Fields

        /// <summary>
        /// Test environment endpoint URL.
        /// </summary>
        public const string TestUrl = "https://up.test.epayworldwide.com/tgxml";

        /// <summary>
        /// Production environment endpoint URL.
        /// </summary>
        public const string ProdUrl = "https://sv.precision.epayworldwide.com/gxml";

        /// <summary>
        /// Expected PAN prefix for AVS cards.
        /// </summary>
        public const string PanPrefix = "636453";

        /// <summary>
        /// Expected PAN length for AVS cards.
        /// </summary>
        public const int PanLength = 16;

        /// <summary>
        /// Default HTTP timeout (in seconds) for AVS requests.
        /// </summary>
        public const int TimeoutSeconds = 30;

        #endregion Public Fields
    }
}