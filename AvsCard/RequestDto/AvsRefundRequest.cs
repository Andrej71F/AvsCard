using System.Xml.Serialization;

namespace AvsCard.RequestDto
{
    /// <summary>
    /// Represents an AVS REFUND request used to return funds back to the card.
    /// The request includes the refund amount (in cents) and the currency code.
    /// </summary>
    [XmlRoot("REQUEST")]
    public class AvsRefundRequest : AvsRequestBase
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="AvsRefundRequest"/>
        /// and sets the request TYPE to "REFUND" as required by AVS.
        /// </summary>
        public AvsRefundRequest()
        {
            Type = "REFUND";
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets or sets the amount (in cents) to be refunded to the card.
        /// </summary>
        [XmlElement("AMOUNT")]
        public int Amount { get; set; }

        /// <summary>
        /// Gets or sets the currency code for the refund amount.
        /// Defaults to 978 (EUR) as required by AVS.
        /// </summary>
        [XmlElement("CURRENCY")]
        public int Currency { get; set; } = 978;

        #endregion Public Properties
    }
}