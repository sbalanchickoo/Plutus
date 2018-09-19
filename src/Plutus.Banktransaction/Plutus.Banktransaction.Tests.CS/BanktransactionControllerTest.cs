using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Plutus.Banktransaction.Service.API.Controllers.V1;
using Plutus.Banktransaction.Tests.CS.EqualityComparers;
using Plutus.SharedLibrary.CS.Enums;
using Plutus.SharedLibrary.CS.Interfaces;
using Plutus.SharedLibrary.CS.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Plutus.Banktransaction.Tests.CS
{
    /// <summary>
    /// Test class to test methods in BanktransactionController controller class
    /// </summary>
    [TestClass]
    public class BanktransactionControllerTest
    {
        private IBankTransaction _repo;
        private ILogger<BanktransactionsController> _logger;

        /// <summary>
        /// Test initialization step, List of fake data, Moq repo
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            List<BankTransaction> txnList = new List<BankTransaction>
            {
                new BankTransaction
                {
                    FITID = "91234567800",
                    Merchant = "R/P to John Doe",
                    Amount = -500.50M,
                    Date = Convert.ToDateTime("2011-12-30")
                },
                new BankTransaction
                {
                    FITID = "91234567801",
                    Merchant = "R/P to Jane Doe",
                    Amount = -1000.50M,
                    Date = Convert.ToDateTime("2011-12-31")
                },
                new BankTransaction
                {
                    FITID = "91234567802",
                    Merchant = "R/P to Jack Doe",
                    Amount = -1500.50M,
                    Date = Convert.ToDateTime("2012-12-31")
                }
            }.OrderBy(txn => txn.FITID).ToList();
            var repositoryMock = new Mock<IBankTransaction>();
            repositoryMock.Setup(r => r.GetBankTransactions()).Returns(txnList);
            InputDataSource inputSource = new InputDataSource
            {
                InputDataSourceType = DataSource.FileSystem,
                InputDataSourceName = @"C:\Temp"
            };
            repositoryMock.Setup(r => r.GetSourceDetails()).Returns(inputSource);
            _repo = repositoryMock.Object;

            var mock = new Mock<ILogger<BanktransactionsController>>();
            _logger = mock.Object;
        }

        /// <summary>
        /// Happy path for the SourceDetail action - type
        /// </summary>
        [TestMethod]
        public void SourceDetail_OnExecute_ReturnSourceDetailType()
        {
            //Arrange
            BanktransactionsController controller = new BanktransactionsController(_repo, _logger);

            //Act
            var inputDataSource = controller.SourceDetail();
            var requestResult = inputDataSource as OkObjectResult;

            //Assert
            Assert.IsInstanceOfType(requestResult, typeof(OkObjectResult));
        }

        /// <summary>
        /// Happy path for the SourceDetail action - content
        /// </summary>
        [TestMethod]
        public void SourceDetail_OnExecute_ReturnSourceDetail()
        {
            //Arrange
            BanktransactionsController controller = new BanktransactionsController(_repo, _logger);

            //Act
            var inputDataSource = controller.SourceDetail();
            var requestResult = inputDataSource as OkObjectResult;
            InputDataSource result = (InputDataSource)requestResult.Value;

            //Assert
            Assert.AreEqual(_repo.GetSourceDetails().InputDataSourceName, result.InputDataSourceName);
            Assert.AreEqual(_repo.GetSourceDetails().InputDataSourceType, result.InputDataSourceType);
        }

        /// <summary>
        /// Happy path for the BankTransactions action - type
        /// </summary>
        [TestMethod]
        public void BankTransactions_OnExecute_ReturnBankTransactionsType()
        {
            //Arrange
            BanktransactionsController controller = new BanktransactionsController(_repo, _logger);

            //Act
            var inputDataSource = controller.BankTransactions();
            var requestResult = inputDataSource as OkObjectResult;

            //Assert
            Assert.IsInstanceOfType(requestResult, typeof(OkObjectResult));
        }

        /// <summary>
        /// Happy path for the BankTransactions action - content
        /// </summary>
        [TestMethod]
        public void BankTransactions_OnExecute_ReturnBankTransactions()
        {
            //Arrange
            BanktransactionsController controller = new BanktransactionsController(_repo, _logger);

            //Act
            var inputDataSource = controller.BankTransactions();
            var requestResult = inputDataSource as OkObjectResult;
            List<BankTransaction> txnList = (List<BankTransaction>)requestResult.Value;

            //Assert
            Assert.AreEqual(3, txnList.Count);
            CollectionAssert.AreEqual(_repo.GetBankTransactions().ToList(), txnList, new BankTransactionComparer());
        }
    }
}
