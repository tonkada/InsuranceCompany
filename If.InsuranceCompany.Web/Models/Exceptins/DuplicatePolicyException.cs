using System;

namespace If.InsuranceCompany.Models
{
    /// <summary>
    /// Duplicate policy exception.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class DuplicatePolicyException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicatePolicyException"/> class.
        /// </summary>
        public DuplicatePolicyException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicatePolicyException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public DuplicatePolicyException(string message)
            : base(message)
        {
        }

    }
}
