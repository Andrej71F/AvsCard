using System.Xml.Serialization;

namespace AvsCard.ResponseDto
{
    /// <summary>
    /// Represents an AVS BALANCE response returned by the AVS system.
    /// Contains detailed balance information for the requested card.
    /// </summary>
    [XmlRoot("RESPONSE")]
    public class AvsBalanceResponse : AvsResponseBase
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the balance information block, including
        /// currency details, available balance, reserved amounts,
        /// and related metadata.
        /// </summary>
        [XmlElement("BALANCE")]
        public AvsBalanceInfo Balance { get; set; } = new();

        #endregion Public Properties
    }
}