using System.Xml.Serialization;

namespace AvsCard.RequestDto
{
    /// <summary>
    /// Base class for all AVS request types.
    /// Contains common fields required by the AVS XML protocol,
    /// including authentication data, terminal information,
    /// transaction identifiers, timestamps, and card details.
    /// </summary>
    public abstract class AvsRequestBase
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the AVS request type.
        /// This value is written as an XML attribute (e.g., BALANCE, REDEEM, REFUND, CANCEL).
        /// </summary>
        [XmlAttribute("TYPE")]
        public string Type { get; set; } = default!;

        /// <summary>
        /// Gets or sets the AVS username used for authentication.
        /// </summary>
        [XmlElement("USERNAME")]
        public string Username { get; set; } = default!;

        /// <summary>
        /// Gets or sets the AVS password used for authentication.
        /// </summary>
        [XmlElement("PASSWORD")]
        public string Password { get; set; } = default!;

        /// <summary>
        /// Gets or sets the terminal identifier associated with the request.
        /// </summary>
        [XmlElement("TERMINALID")]
        public string TerminalId { get; set; } = default!;

        /// <summary>
        /// Gets or sets the unique transaction ID for the request.
        /// Must be unique per request.
        /// </summary>
        [XmlElement("TXID")]
        public string TxId { get; set; } = default!;

        /// <summary>
        /// Gets or sets the local timestamp of the request
        /// in the format "yyyy-MM-dd HH:mm:ss".
        /// </summary>
        [XmlElement("LOCALDATETIME")]
        public string LocalDateTime { get; set; } = default!;

        /// <summary>
        /// Gets or sets the card information associated with the request.
        /// Includes PAN and optional PIN.
        /// </summary>
        [XmlElement("CARD")]
        public AvsCard Card { get; set; } = new();

        #endregion Public Properties
    }
}