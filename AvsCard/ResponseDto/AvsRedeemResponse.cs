using System.Xml.Serialization;

namespace AvsCard.ResponseDto
{
    /// <summary>
    /// Represents an AVS REDEEM response returned by the AVS system.
    /// Contains the AID of the processed redemption transaction and
    /// updated balance information after the operation.
    /// </summary>
    [XmlRoot("RESPONSE")]
    public class AvsRedeemResponse : AvsResponseBase
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the AVS-assigned transaction identifier (AID)
        /// for the redeem operation.
        /// </summary>
        [XmlElement("AID")]
        public string Aid { get; set; } = default!;

        /// <summary>
        /// Gets or sets the balance information after the redemption,
        /// including currency details and updated amounts.
        /// </summary>
        [XmlElement("BALANCE")]
        public AvsBalanceInfo Balance { get; set; } = new();

        #endregion Public Properties
    }
}