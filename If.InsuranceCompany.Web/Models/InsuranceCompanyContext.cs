using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace If.InsuranceCompany.Models
{
    /// <summary>
    /// Insurance company db context.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
    public class InsuranceCompanyContext : DbContext
    {
        /// <summary>
        /// Name of Insurance company.
        /// </summary>
        public string Name
        {
            get
            {
                return "If";
            }
        }

        /// <summary>
        /// Gets or sets the books.
        /// </summary>
        public DbSet<Policy> Policies { get; set; }

        /// <summary>
        /// Gets or sets the risks.
        /// </summary>
        public DbSet<Risk> Risks { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InsuranceCompanyContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public InsuranceCompanyContext(DbContextOptions<InsuranceCompanyContext> options)
        : base(options) { }

        /// <summary>
        /// Gets the policy.
        /// </summary>
        /// <param name="nameOfInsuredObject">The name of insured object.</param>
        /// <param name="effectiveDate">The effective date.</param>
        /// <returns></returns>
        public IPolicy GetPolicy(string nameOfInsuredObject, DateTime effectiveDate)
        {
            return this.Policies.FirstOrDefault(q => q.NameOfInsuredObject.Equals(nameOfInsuredObject, StringComparison.OrdinalIgnoreCase) && q.ValidFrom >= effectiveDate && q.ValidTill >= effectiveDate);
        }

        /// <summary>
        /// Sells the policy.
        /// </summary>
        /// <param name="nameOfInsuredObject">The name of insured object.</param>
        /// <param name="validFrom">The valid from.</param>
        /// <param name="validMonths">The valid months.</param>
        /// <param name="selectedRisks">The selected risks.</param>
        /// <returns></returns>
        public IPolicy SellPolicy(string nameOfInsuredObject, DateTime validFrom, short validMonths, IList<Risk> selectedRisks)
        {
            if (this.GetPolicy(nameOfInsuredObject, validFrom) != null)
                throw new DuplicatePolicyException(string.Format("Error: policy '{0}' already sold on {1:dd/MM/yyyy}!", nameOfInsuredObject, validFrom));

            this.Policies.Add(
                new Policy
                {
                    NameOfInsuredObject = nameOfInsuredObject,
                    ValidFrom = validFrom,
                    ValidTill = validFrom.AddMonths(validMonths),
                    InsuredRisks = selectedRisks
                });
            this.SaveChanges();

            return this.GetPolicy(nameOfInsuredObject, validFrom);
        }

        /// <summary>
        /// Add risk to the policy of insured object.
        /// </summary>
        /// <param name="nameOfInsuredObject">Name of insured object</param>
        /// <param name="risk">Risk that must be added</param>
        /// <param name="validFrom">Date when risk becomes active. Can not be in the past</param>
        public void AddRisk(string nameOfInsuredObject, Risk risk, DateTime validFrom)
        {
            if (validFrom < DateTime.Now)
                throw new RiskAddInPastException(string.Format("Error: Risk can not be in the past {0:dd/MM/yyyy}!", validFrom));

            this.GetPolicy(nameOfInsuredObject, validFrom).InsuredRisks.Add(risk);
            this.SaveChanges();
        }

        /// <summary>
        /// Remove risk from the policy of insured object.
        /// </summary>
        /// <param name="nameOfInsuredObject">Name of insured object</param>
        /// <param name="risk">Risk that must be removed</param>
        /// <param name="validTill">Date when risk become inactive. Must be equal to or greater than date when risk become active</param>
        public void RemoveRisk(string nameOfInsuredObject, Risk risk, DateTime validTill)
        {
            var policy = this.GetPolicy(nameOfInsuredObject, validTill);
            if (policy == null)
                throw new PolicyNotFoundException(string.Format("Error: No policy found '{0}'!", nameOfInsuredObject));

            var riskToRemove = policy.InsuredRisks.FirstOrDefault(q => q.Name == risk.Name && q.YearlyPrice == risk.YearlyPrice);

            if (riskToRemove == null)
                throw new Exception(string.Format("Error: No risk to remove '{0}'!", risk.Name));

            policy.InsuredRisks.Remove(riskToRemove);
            this.SaveChanges();
        }
    }
}
