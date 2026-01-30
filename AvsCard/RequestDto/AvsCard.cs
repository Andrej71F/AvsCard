using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AvsCard.RequestDto
{
    /// <summary>
    /// Represents the card information included in AVS requests.
    /// Contains the PAN (required) and an optional PIN field.
    /// </summary>
    public class AvsCard
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the Primary Account Number (PAN) of the card.
        /// This value is required for all AVS operations.
        /// </summary>
        [XmlElement("PAN")]
        public string Pan { get; set; } = default!;

        /// <summary>
        /// Gets or sets the optional PIN value used for specific AVS operations
        /// such as online shop transactions.
        /// </summary>
        [XmlElement("PIN")]
        public string? Pin { get; set; }

        #endregion Public Properties

        // Reserved for online shop usage
    }
}