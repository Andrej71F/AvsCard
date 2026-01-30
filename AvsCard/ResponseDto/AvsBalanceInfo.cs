using System.Xml.Serialization;

namespace AvsCard.ResponseDto
{
    /// <summary>
    /// Represents the balance information section of an AVS BALANCE response.
    /// Contains currency‑specific balance details such as available balance,
    /// reserved amounts, limits, and currency metadata.
    /// </summary>
    public class AvsBalanceInfo
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the currency block containing detailed balance data
        /// for a specific ISO currency.
        /// </summary>
        [XmlElement("CURRENCY")]
        public AvsBalanceCurrency Currency { get; set; } = new();

        #endregion Public Properties
    }
}