using System.ComponentModel.DataAnnotations;

namespace If.InsuranceCompany.Models
{
    /// <summary>
    /// The model of risk.
    /// </summary>
    public class Risk
    {
        /// <summary>
        /// Unique name of the risk
        /// </summary>
        [Key]
        public string Name { get; set; }

        /// <summary>
        /// Risk yearly price
        /// </summary>
        public decimal YearlyPrice { get; set; }
    }
}
