using System.Xml.Serialization;

namespace AvsCard.RequestDto
{
    /// <summary>
    /// Represents an AVS CANCEL request used to reverse a previously executed
    /// REDEEM or REFUND transaction. The request includes a reference to the
    /// original transaction along with the amount and currency.
    /// </summary>
    [XmlRoot("REQUEST")]
    public class AvsCancelRequest : AvsRequestBase
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="AvsCancelRequest"/>
        /// and sets the request TYPE to "CANCEL" as required by AVS.
        /// </summary>
        public AvsCancelRequest()
        {
            Type = "CANCEL";
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets or sets the reference to the original AVS transaction
        /// that should be cancelled.
        /// </summary>
        [XmlElement("TXREF")]
        public string TxRef { get; set; } = default!;

        /// <summary>
        /// Gets or sets the currency code for the cancellation amount.
        /// Defaults to 978 (EUR) as required by AVS.
        /// </summary>
        [XmlElement("CURRENCY")]
        public int Currency { get; set; } = 978;

        /// <summary>
        /// Gets or sets the amount (in cents) to be cancelled.
        /// </summary>
        [XmlElement("AMOUNT")]
        public int Amount { get; set; }

        #endregion Public Properties
    }
}