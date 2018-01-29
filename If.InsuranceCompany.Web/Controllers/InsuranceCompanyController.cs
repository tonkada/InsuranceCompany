using If.InsuranceCompany.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace If.InsuranceCompany.Web.Controllers
{
    [Route("api/[controller]")]
    public class InsuranceCompanyController : Controller, IInsuranceCompany
    {
        /// <summary>
        /// The database context.
        /// </summary>
        private readonly InsuranceCompanyContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="InsuranceCompanyController"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public InsuranceCompanyController(InsuranceCompanyContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Name of Insurance company.
        /// </summary>
        public string Name { get { return this._dbContext.Name; } }

        /// <summary>
        /// GET: api/InsuranceCompany/AvailableRisks.
        /// List of the risks that can be insured. List can be updated at any time.
        /// </summary>
        /// <returns>
        /// List of the risks.
        /// </returns>
#warning TODO: You can add or remove risks at any moment within policy period!
        public IList<Risk> AvailableRisks
        {
            get
            {
                return this._dbContext.Risks.ToList();
            }

            set
            {
            }
        }

        /// <summary>
        /// Sell the policy.
        /// </summary>
        /// <param name="nameOfInsuredObject">Name of the insured object. Must be unique in the given period.</param>
        /// <param name="validFrom">Date and time when policy starts. Can not be in the past</param>
        /// <param name="validMonths">Policy period in months</param>
        /// <param name="selectedRisks">List of risks that must be included in the policy</param>
        /// <returns>
        /// Information about policy
        /// </returns>
        [HttpGet("[action]")]
        public IPolicy SellPolicy(string nameOfInsuredObject, DateTime validFrom, short validMonths, IList<Risk> selectedRisks) => _dbContext.SellPolicy(nameOfInsuredObject, validFrom, validMonths, selectedRisks);

        /// <summary>
        /// Add risk to the policy of insured object.
        /// </summary>
        /// <param name="nameOfInsuredObject">Name of insured object</param>
        /// <param name="risk">Risk that must be added</param>
        /// <param name="validFrom">Date when risk becomes active. Can not be in the past</param>
        [HttpGet("[action]")]
        public void AddRisk(string nameOfInsuredObject, Risk risk, DateTime validFrom) => _dbContext.AddRisk(nameOfInsuredObject, risk, validFrom);

        /// <summary>
        /// Remove risk from the policy of insured object.
        /// </summary>
        /// <param name="nameOfInsuredObject">Name of insured object</param>
        /// <param name="risk">Risk that must be removed</param>
        /// <param name="validTill">Date when risk become inactive. Must be equal to or greater than date when risk become active</param>
        [HttpGet("[action]")]
        public void RemoveRisk(string nameOfInsuredObject, Risk risk, DateTime validTill) => _dbContext.RemoveRisk(nameOfInsuredObject, risk, validTill);

        /// <summary>
        /// GET: api/InsuranceCompany/GetPolicy?nameOfInsuredObject,effectiveDate
        /// Gets policy with a risks at the given point of time.
        /// </summary>
        /// <param name="nameOfInsuredObject">Name of insured object</param>
        /// <param name="effectiveDate">Point of date and time, when the policy effective</param>
        /// <returns>
        /// Policy.
        /// </returns>
        [HttpGet("[action]/{nameOfInsuredObject}/{effectiveDate}")]
        public IPolicy GetPolicy(string nameOfInsuredObject, DateTime effectiveDate) => _dbContext.GetPolicy(nameOfInsuredObject, effectiveDate);
    }
}
