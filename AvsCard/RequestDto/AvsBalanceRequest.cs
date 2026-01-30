using System.Xml.Serialization;

namespace AvsCard.RequestDto
{
    /// <summary>
    /// Represents an AVS BALANCE request used to retrieve the current
    /// balance information for a specific card.
    /// </summary>
    [XmlRoot("REQUEST")]
    public class AvsBalanceRequest : AvsRequestBase
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="AvsBalanceRequest"/>
        /// and sets the request TYPE to "BALANCE" as required by AVS.
        /// </summary>
        public AvsBalanceRequest()
        {
            Type = "BALANCE";
        }

        #endregion Public Constructors
    }
}