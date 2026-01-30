using System.Xml.Serialization;

namespace AvsCard.ResponseDto
{
    /// <summary>
    /// Represents card information returned in an AVS response.
    /// Includes masked PAN and optional expiry details depending
    /// on the card type and the specific AVS operation.
    /// </summary>
    public class AvsResponseCard
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the masked Primary Account Number (PAN)
        /// returned by the AVS system.
        /// </summary>
        [XmlElement("PAN")]
        public string Pan { get; set; } = default!;

        /// <summary>
        /// Gets or sets the card expiry year, if provided by AVS.
        /// </summary>
        [XmlElement("EXPIRY_YEAR")]
        public int? ExpiryYear { get; set; }

        /// <summary>
        /// Gets or sets the card expiry month, if provided by AVS.
        /// </summary>
        [XmlElement("EXPIRY_MONTH")]
        public int? ExpiryMonth { get; set; }

        /// <summary>
        /// Gets or sets the full expiry date string, if returned by AVS.
        /// Format may vary depending on the card type.
        /// </summary>
        [XmlElement("EXPIRY_DATE")]
        public string? ExpiryDate { get; set; }

        #endregion Public Properties
    }
}