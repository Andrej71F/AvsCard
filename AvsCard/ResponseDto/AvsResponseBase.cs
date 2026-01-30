using AvsCard.Helpers;
using System.Xml.Serialization;

namespace AvsCard.ResponseDto
{
    /// <summary>
    /// Base class for all AVS response types.
    /// Contains common fields returned by the AVS XML protocol,
    /// including result codes, timestamps, profile information,
    /// terminal identifiers, and card details.
    /// </summary>
    public abstract class AvsResponseBase
    {
        #region Private Fields

        /// <summary>
        /// Backing field for the <see cref="Result"/> property.
        /// </summary>
        private int _result;

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Gets or sets the AVS response type.
        /// This value is written as an XML attribute (e.g., BALANCE, REDEEM, REFUND, CANCEL).
        /// </summary>
        [XmlAttribute("TYPE")]
        public string Type { get; set; } = default!;

        /// <summary>
        /// Gets or sets the numeric AVS result code.
        /// Setting this property automatically updates <see cref="InternalResultText"/>
        /// using the AVS error description lookup.
        /// </summary>
        [XmlElement("RESULT")]
        public int Result
        {
            get => _result;

            set
            {
                _result = value;
                InternalResultText = AvsErrorDescriptions.GetDescription(value);
            }
        }

        /// <summary>
        /// Gets or sets the human-readable result text returned by AVS.
        /// </summary>
        [XmlElement("RESULTTEXT")]
        public string ResultText { get; set; } = default!;

        /// <summary>
        /// Gets or sets the profile name associated with the request.
        /// </summary>
        [XmlElement("PROFILENAME")]
        public string ProfileName { get; set; } = default!;

        /// <summary>
        /// Gets or sets the numeric profile identifier.
        /// </summary>
        [XmlElement("PROFILEID")]
        public int ProfileId { get; set; }

        /// <summary>
        /// Gets or sets the terminal identifier associated with the response.
        /// </summary>
        [XmlElement("TERMINALID")]
        public string TerminalId { get; set; } = default!;

        /// <summary>
        /// Gets or sets the transaction ID associated with the request.
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
        /// Gets or sets the server timestamp returned by AVS
        /// in the format "yyyy-MM-dd HH:mm:ss".
        /// </summary>
        [XmlElement("SERVERDATETIME")]
        public string ServerDateTime { get; set; } = default!;

        /// <summary>
        /// Gets or sets the card information returned by AVS,
        /// including masked PAN and other metadata.
        /// </summary>
        [XmlElement("CARD")]
        public AvsResponseCard Card { get; set; } = new();

        /// <summary>
        /// Gets the internal error description automatically derived
        /// from the numeric result code. This value is not serialized.
        /// </summary>
        [XmlIgnore]
        public string InternalResultText { get; private set; } = default!;

        #endregion Public Properties
    }
}