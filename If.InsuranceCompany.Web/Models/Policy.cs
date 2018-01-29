using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace If.InsuranceCompany.Models
{
    /// <summary>
    /// The implemenntation of policy.
    /// </summary>
    /// <seealso cref="InsuranceCompany.Models.IPolicy" />
    public class Policy : IPolicy
    {
        /// <summary>
        /// Name of insured object
        /// </summary>
        [Key]
#warning TODO: There could be several policies with the same insured object name, but different effective date
        public string NameOfInsuredObject { get; set; }

        /// <summary>
        /// Date when policy becomes active
        /// </summary>
        public DateTime ValidFrom { get; set; }

        /// <summary>
        /// Date when policy becomes inactive
        /// </summary>
        public DateTime ValidTill { get; set; }

        /// <summary>
        /// Total price of the policy. Calculate by summing up all insured risks.
        /// Take into account that risk price is given for 1 full year. Policy/risk period can be shorter.
        /// </summary>
#warning TOOD: Premium must be calculated according to risk validity period.
        public decimal Premium
        {
            get
            {
                if (this.InsuredRisks != null)
                    return this.InsuredRisks.Sum(q => q.YearlyPrice);

                return 0;
            }

            set
            {
            }
        }

        /// <summary>
        /// Initially included risks of risks at specific moment of time.
        /// </summary>
        public IList<Risk> InsuredRisks { get; set; }
    }
}
