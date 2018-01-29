using If.InsuranceCompany.Models;
using If.InsuranceCompany.Web.Controllers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace If.InsuranceCompany.Tests
{
    /// <summary>
    /// Insurance company unit tests.
    /// </summary>
    public class InsuranceCompanyTest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InsuranceCompanyTest"/> class.
        /// </summary>
        public InsuranceCompanyTest()
        {
            InitContext();
        }

        /// <summary>
        /// The database context
        /// </summary>
        private InsuranceCompanyContext _dbContext;

        /// <summary>
        /// Initializes the context.
        /// </summary>
        private void InitContext()
        {
            var builder = new DbContextOptionsBuilder<InsuranceCompanyContext>().UseInMemoryDatabase();

            var context = new InsuranceCompanyContext(builder.Options);
            context.Database.EnsureDeleted();

            var risks = Enumerable.Range(1, 10)
                .Select(i => new Risk() { Name = $"Risk_{i}", YearlyPrice = i / 10m }).ToList();
            context.Risks.AddRange(risks);


            var policies = Enumerable.Range(1, 10)
                .Select(i => new Policy
                {
                    NameOfInsuredObject = $"Policy_{i}",
                    Premium = i / 100m,
                    ValidFrom = new DateTime(2020 + i, 1, 1),
                    ValidTill = new DateTime(2021 + i, 1, 1),
                    InsuredRisks = risks //new List<Risk>() { new Risk() { Name = $"Risk_{i}1", YearlyPrice = i / 10m } }
                });
            context.Policies.AddRange(policies);
            int changed = context.SaveChanges();
            _dbContext = context;
        }

        /// <summary>
        /// Tests the available risks CRUD.
        /// </summary>
        [Fact]
        public void TestAvailableRisksCRUD()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Tests the get policy.
        /// </summary>
        [Fact]
        public void TestGetPolicy()
        {
            var expectedNameOfInsuredObject = "Policy_1";
            var expectedValidFrom = new DateTime(2021, 1, 1);

            var controller = new InsuranceCompanyController(_dbContext);
            var result = controller.GetPolicy(expectedNameOfInsuredObject, expectedValidFrom);
            Assert.Equal(expectedNameOfInsuredObject, result.NameOfInsuredObject);
            Assert.Equal(expectedValidFrom, result.ValidFrom);
        }

        /// <summary>
        /// Tests the sell policy.
        /// </summary>
        [Fact]
        public void TestSellPolicy()
        {
            var expectedPolicy = new Policy()
            {
                NameOfInsuredObject = "Policy_11",
                ValidFrom = new DateTime(2021, 1, 1),
                ValidTill = new DateTime(2022, 1, 1),
                InsuredRisks = new List<Risk>() { new Risk() { Name = "Risk_11", YearlyPrice = 1.1m } }
            };

            var controller = new InsuranceCompanyController(_dbContext);
            short validMonths = 12;
            var result = controller.SellPolicy(expectedPolicy.NameOfInsuredObject, expectedPolicy.ValidFrom, validMonths, expectedPolicy.InsuredRisks);

            Assert.Equal(expectedPolicy.NameOfInsuredObject, result.NameOfInsuredObject);
            Assert.Equal(expectedPolicy.ValidFrom, result.ValidFrom);
            Assert.Equal(expectedPolicy.ValidTill, result.ValidTill);
            Assert.Equal(expectedPolicy.Premium, expectedPolicy.InsuredRisks.Sum(q => q.YearlyPrice));
            Assert.Equal(expectedPolicy.InsuredRisks.FirstOrDefault().Name, result.InsuredRisks.FirstOrDefault().Name);
            Assert.Equal(expectedPolicy.InsuredRisks.FirstOrDefault().YearlyPrice, result.InsuredRisks.FirstOrDefault().YearlyPrice);
        }

        /// <summary>
        /// Tests the add risk.
        /// </summary>
        [Fact]
        public void TestAddRisk()
        {
            var expectedNameOfInsuredObject = "Policy_1";
            var expectedValidFrom = new DateTime(2021, 1, 1);
            var expectedRisk = new Risk()
            {
                Name = "Risk_add",
                YearlyPrice = 1.2m
            };

            var controller = new InsuranceCompanyController(_dbContext);
            controller.AddRisk(expectedNameOfInsuredObject, expectedRisk, expectedValidFrom);

            var result = controller.GetPolicy(expectedNameOfInsuredObject, expectedValidFrom).InsuredRisks;

            Assert.Equal(2, result.Count);
            Assert.Equal(expectedRisk.Name, result[1].Name);
            Assert.Equal(expectedRisk.YearlyPrice, result[1].YearlyPrice);
        }

        /// <summary>
        /// Tests the remove risk.
        /// </summary>
        [Fact]
        public void TestRemoveRisk()
        {
            var expectedNameOfInsuredObject = "Policy_1";
            var expectedValidFrom = new DateTime(2021, 1, 1);
            var expectedRisk = new Risk()
            {
                Name = "Risk_1",
                YearlyPrice = 0.1m
            };

            var controller = new InsuranceCompanyController(_dbContext);
            controller.RemoveRisk(expectedNameOfInsuredObject, expectedRisk, expectedValidFrom);

            var result = controller.GetPolicy(expectedNameOfInsuredObject, expectedValidFrom).InsuredRisks;

            Assert.Equal(1, result.Count);
        }
    }
}
