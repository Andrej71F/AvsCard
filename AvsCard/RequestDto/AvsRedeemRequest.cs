using System.Xml.Serialization;

namespace AvsCard.RequestDto
{
    /// <summary>
    /// Represents an AVS REDEEM request used to deduct a specified amount
    /// from the card balance. The request includes the amount in cents and
    /// the currency code (default 978 for EUR).
    /// </summary>
    [XmlRoot("REQUEST")]
    public class AvsRedeemRequest : AvsRequestBase
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="AvsRedeemRequest"/>
        /// and sets the request TYPE to "REDEEM" as required by AVS.
        /// </summary>
        public AvsRedeemRequest()
        {
            Type = "REDEEM";
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets or sets the amount (in cents) to be redeemed from the card.
        /// </summary>
        [XmlElement("AMOUNT")]
        public int Amount { get; set; }

        /// <summary>
        /// Gets or sets the currency code for the redemption amount.
        /// Defaults to 978 (EUR) as required by AVS.
        /// </summary>
        [XmlElement("CURRENCY")]
        public int Currency { get; set; } = 978;

        #endregion Public Properties
    }
}