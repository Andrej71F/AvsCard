using System.Xml.Serialization;

namespace AvsCard.ResponseDto
{
    /// <summary>
    /// Represents detailed balance information for a specific currency
    /// within an AVS BALANCE response. Includes current and previous
    /// balance values, reserved amounts, limits, and currency metadata.
    /// </summary>
    public class AvsBalanceCurrency
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the standard currency indicator used by AVS.
        /// </summary>
        [XmlAttribute("STANDARD")]
        public int Standard { get; set; }

        /// <summary>
        /// Gets or sets the ISO currency code (e.g., 978 for EUR).
        /// </summary>
        [XmlElement("ISOCODE")]
        public int IsoCode { get; set; }

        /// <summary>
        /// Gets or sets the human‑readable currency name.
        /// </summary>
        [XmlElement("NAME")]
        public string Name { get; set; } = default!;

        /// <summary>
        /// Gets or sets the number of fractional digits used by the currency.
        /// </summary>
        [XmlElement("FRACTION")]
        public int Fraction { get; set; }

        /// <summary>
        /// Gets or sets the current available balance (in minor units).
        /// </summary>
        [XmlElement("BALANCE")]
        public int Balance { get; set; }

        /// <summary>
        /// Gets or sets the available balance before the transaction.
        /// </summary>
        [XmlElement("BALANCE_BEFORE")]
        public int BalanceBefore { get; set; }

        /// <summary>
        /// Gets or sets the currently reserved amount (in minor units).
        /// </summary>
        [XmlElement("RESERVED")]
        public int Reserved { get; set; }

        /// <summary>
        /// Gets or sets the reserved amount before the transaction.
        /// </summary>
        [XmlElement("RESERVED_BEFORE")]
        public int ReservedBefore { get; set; }

        /// <summary>
        /// Gets or sets the minimum allowed balance.
        /// </summary>
        [XmlElement("MIN")]
        public int Min { get; set; }

        /// <summary>
        /// Gets or sets the maximum allowed balance.
        /// </summary>
        [XmlElement("MAX")]
        public int Max { get; set; }

        /// <summary>
        /// Gets or sets the minimum allowed recharge amount.
        /// </summary>
        [XmlElement("MINRECHARGE")]
        public int MinRecharge { get; set; }

        /// <summary>
        /// Gets or sets the maximum allowed recharge amount.
        /// </summary>
        [XmlElement("MAXRECHARGE")]
        public int MaxRecharge { get; set; }

        /// <summary>
        /// Gets or sets the activation amount required for the card.
        /// </summary>
        [XmlElement("ACTIVATIONAMOUNT")]
        public int ActivationAmount { get; set; }

        #endregion Public Properties
    }
}