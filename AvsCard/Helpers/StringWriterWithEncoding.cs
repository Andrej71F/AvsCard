using System.Text;

namespace AvsCard.Helpers
{
    /// <summary>
    /// A custom <see cref="StringWriter"/> implementation that allows specifying
    /// the output text encoding. This is required because the default
    /// <see cref="StringWriter"/> always reports UTF-16, regardless of the
    /// encoding used during XML serialization.
    /// </summary>
    public class StringWriterWithEncoding : StringWriter
    {
        #region Private Fields

        /// <summary>
        /// The encoding to be reported by this writer.
        /// </summary>
        private readonly Encoding _encoding;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="StringWriterWithEncoding"/>
        /// using the specified encoding.
        /// </summary>
        /// <param name="encoding">The encoding to associate with this writer.</param>
        public StringWriterWithEncoding(Encoding encoding)
        {
            _encoding = encoding;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets the encoding specified during construction.
        /// </summary>
        public override Encoding Encoding => _encoding;

        #endregion Public Properties
    }
}