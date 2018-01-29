using System;

namespace If.InsuranceCompany.Models
{
    /// <summary>
    /// Risk add in past exception.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class RiskAddInPastException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PolicyNotFoundException"/> class.
        /// </summary>
        public RiskAddInPastException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PolicyNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public RiskAddInPastException(string message)
            : base(message)
        {
        }
    }
}
