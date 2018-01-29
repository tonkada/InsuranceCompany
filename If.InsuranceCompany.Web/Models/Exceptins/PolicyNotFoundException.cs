using System;

namespace If.InsuranceCompany.Models
{
    /// <summary>
    /// Policy not found exception.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class PolicyNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PolicyNotFoundException"/> class.
        /// </summary>
        public PolicyNotFoundException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PolicyNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public PolicyNotFoundException(string message)
            : base(message)
        {
        }
    }
}
