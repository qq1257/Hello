// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="DeflaterPending.cs">
//   
// </copyright>
// <summary>
//   This class stores the pending output of the Deflater.
//   author of the original java version : Jochen Hoenicke
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------
namespace ICSharpCode.SharpZipLib.Zip.Compression
{
    /// <summary>
    ///     This class stores the pending output of the Deflater.
    ///     author of the original java version : Jochen Hoenicke
    /// </summary>
    public class DeflaterPending : PendingBuffer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeflaterPending"/> class. 
        ///     Construct instance with default buffer size
        /// </summary>
        public DeflaterPending() : base(DeflaterConstants.PENDING_BUF_SIZE)
        {
        }
    }
}