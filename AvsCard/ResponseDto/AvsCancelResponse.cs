using System.Xml.Serialization;

namespace AvsCard.ResponseDto
{
    /// <summary>
    /// Represents an AVS CANCEL response returned by the AVS system.
    /// Contains the AID of the processed transaction, the reference
    /// to the original transaction, and updated balance information.
    /// </summary>
    [XmlRoot("RESPONSE")]
    public class AvsCancelResponse : AvsResponseBase
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the AVS-assigned transaction identifier (AID)
        /// for the cancellation operation.
        /// </summary>
        [XmlElement("AID")]
        public string Aid { get; set; } = default!;

        /// <summary>
        /// Gets or sets the reference to the original transaction
        /// that was cancelled.
        /// </summary>
        [XmlElement("TXREF")]
        public string TxRef { get; set; } = default!;

        /// <summary>
        /// Gets or sets the balance information after the cancellation,
        /// including currency details and updated amounts.
        /// </summary>
        [XmlElement("BALANCE")]
        public AvsBalanceInfo Balance { get; set; } = new();

        #endregion Public Properties
    }
}