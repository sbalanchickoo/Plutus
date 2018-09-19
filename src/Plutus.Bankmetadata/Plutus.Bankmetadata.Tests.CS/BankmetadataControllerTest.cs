using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Plutus.Bankmetadata.Service.API.Controllers.V1;
using Plutus.Bankmetadata.Tests.CS.EqualityComparers;
using Plutus.SharedLibrary.CS.Enums;
using Plutus.SharedLibrary.CS.Interfaces;
using Plutus.SharedLibrary.CS.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Plutus.Bankmetadata.Tests.CS
{
    /// <summary>
    /// Test class to test methods in BankmetadataController controller class
    /// </summary>
    [TestClass]
    public class BankmetadataControllerTest
    {
        private IBankMetadata _repo;
        private ILogger<BankmetadataController> _logger;

        List<BankMetadata> txnList = new List<BankMetadata>();

        /// <summary>
        /// Initial setup, create Source List, Moq repo object
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            txnList = new List<BankMetadata>
            {
                new BankMetadata
                {
                    Date = Convert.ToDateTime("2010-01-01"),
                    Amount = 100.00M,
                    UserComments = "Comment 1",
                    Merchant = "Payee 1",
                    TransactionCategory = "Money Received from Employee"
                },
                new BankMetadata
                {
                    Date = Convert.ToDateTime("2011-01-01"),
                    Amount = 500.00M,
                    UserComments = "Comment, 2",
                    Merchant = "Payee 2",
                    TransactionCategory = "Cash received from clients"
                }
                ,
                new BankMetadata
                {
                    Date = Convert.ToDateTime("2011-01-01"),
                    Amount = 500.00M,
                    UserComments = "Comment, 2",
                    Merchant = "Payee 3",
                    TransactionCategory = "Cash received from clients"
                }
            }
                .OrderBy(txn => txn.Date)
                .OrderBy(txn => txn.Amount)
                .OrderBy(txn => txn.Merchant)
                .ToList();

            var repositoryMock = new Mock<IBankMetadata>();
            repositoryMock.Setup(r => r.GetBankMetadata()).Returns(txnList);
            InputDataSource inputSource = new InputDataSource
            {
                InputDataSourceType = DataSource.FileSystem,
                InputDataSourceName = @"C:\Temp"
            };
            repositoryMock.Setup(r => r.GetSourceDetails()).Returns(inputSource);
            _repo = repositoryMock.Object;

            var mock = new Mock<ILogger<BankmetadataController>>();
            _logger = mock.Object;
        }

        /// <summary>
        /// Happy path for the SourceDetail action - type
        /// </summary>
        [TestMethod]
        public void SourceDetail_OnExecute_ReturnSourceDetailType()
        {
            //Arrange
            BankmetadataController controller = new BankmetadataController(_repo, _logger);

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
            BankmetadataController controller = new BankmetadataController(_repo, _logger);

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
        public void BankMetadata_OnExecute_ReturnBankMetadataType()
        {
            //Arrange
            BankmetadataController controller = new BankmetadataController(_repo, _logger);

            //Act
            var inputDataSource = controller.BankMetadata();
            var requestResult = inputDataSource as OkObjectResult;

            //Assert
            Assert.IsInstanceOfType(requestResult, typeof(OkObjectResult));
        }

        /// <summary>
        /// Happy path for the BankTransactions action - content
        /// </summary>
        [TestMethod]
        public void BankMetadata_OnExecute_ReturnBankMetadata()
        {
            //Arrange
            BankmetadataController controller = new BankmetadataController(_repo, _logger);

            //Act
            var inputDataSource = controller.BankMetadata();
            var requestResult = inputDataSource as OkObjectResult;
            List<BankMetadata> txnList = (List<BankMetadata>)requestResult.Value;

            //Assert
            Assert.AreEqual(3, txnList.Count);
            CollectionAssert.AreEqual(_repo.GetBankMetadata().ToList(), txnList, new BankMetadataComparer());
        }
    }
}
