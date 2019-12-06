using System;

namespace ScoopFramework.Exceptions
{
    /// <summary>
    /// The information exception.
    /// </summary>
    public class InformationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InformationException"/> class.
        /// </summary>
        public InformationException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InformationException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public InformationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InformationException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="inner">
        /// The inner.
        /// </param>
        public InformationException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
