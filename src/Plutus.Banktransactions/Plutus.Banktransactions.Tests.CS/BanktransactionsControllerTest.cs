using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Plutus.Banktransactions.Service.API.Controllers.V1;
using Plutus.Banktransactions.Tests.CS.EqualityComparers;
using Plutus.SharedLibrary.CS.Enums;
using Plutus.SharedLibrary.CS.Interfaces;
using Plutus.SharedLibrary.CS.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Plutus.Banktransactions.Tests.CS
{
    /// <summary>
    /// Test class to test methods in BanktransactionController controller class
    /// </summary>
    [TestClass]
    public class BanktransactionsControllerTest
    {
        private IBankTransaction _repo;
        private ILogger<BanktransactionsController> _logger;
        private List<BankTransaction> _txnList1 = new List<BankTransaction>();

        /// <summary>
        /// Test initialization step, List of fake data, Moq repo
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            BankTransaction txn1 = new BankTransaction
            {
                FITID = "91234567800",
                Merchant = "R/P to John Doe",
                Amount = -500.50M,
                Date = Convert.ToDateTime("2011-12-30")
            };
            BankTransaction txn2 = new BankTransaction
            {
                FITID = "91234567801",
                Merchant = "R/P to Jane Doe",
                Amount = -1000.50M,
                Date = Convert.ToDateTime("2011-12-31")
            };
            BankTransaction txn3 = new BankTransaction
            {
                FITID = "91234567802",
                Merchant = "R/P to Jack Doe",
                Amount = -1500.50M,
                Date = Convert.ToDateTime("2012-12-31")
            };

            _txnList1.Add(txn1);
            _txnList1.Add(txn2);
            _txnList1.Add(txn3);

            var repositoryMock = new Mock<IBankTransaction>();
            repositoryMock.Setup(r => r.GetBankTransactions()).Returns(_txnList1);
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
