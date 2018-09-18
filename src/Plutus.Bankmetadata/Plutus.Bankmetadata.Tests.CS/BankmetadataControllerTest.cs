//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Moq;
//using Plutus.Bankmetadata.Service.API.Controllers.V1;
//using Plutus.Bankmetadata.Tests.CS.EqualityComparers;
//using Plutus.SharedLibrary.CS.Enums;
//using Plutus.SharedLibrary.CS.Interfaces;
//using Plutus.SharedLibrary.CS.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace Plutus.Bankmetadata.Tests.CS
//{
//    [TestClass]
//    public class BankmetadataControllerTest
//    {
//        private IBankMetadata _repo;
//        private ILogger<BankmetadataController> _logger;

//        [TestInitialize]
//        public void Setup()
//        {
//            List<BankMetadata> txnList = new List<BankMetadata>
//            {
//                new BankMetadata
//                {
//                    FITID = "91234567800",
//                    Merchant = "R/P to John Doe",
//                    Amount = -500.50M,
//                    Date = Convert.ToDateTime("2011-12-30")
//                },
//                new BankMetadata
//                {
//                    FITID = "91234567801",
//                    Merchant = "R/P to Jane Doe",
//                    Amount = -1000.50M,
//                    Date = Convert.ToDateTime("2011-12-31")
//                },
//                new BankMetadata
//                {
//                    FITID = "91234567802",
//                    Merchant = "R/P to Jack Doe",
//                    Amount = -1500.50M,
//                    Date = Convert.ToDateTime("2012-12-31")
//                }
//            }.OrderBy(txn => txn.FITID).ToList();
//            var repositoryMock = new Mock<IBankMetadata>();
//            repositoryMock.Setup(r => r.GetBankMetadatas()).Returns(txnList);
//            InputDataSource inputSource = new InputDataSource
//            {
//                InputDataSourceType = DataSource.FileSystem,
//                InputDataSourceName = @"C:\Temp"
//            };
//            repositoryMock.Setup(r => r.GetSourceDetails()).Returns(inputSource);
//            _repo = repositoryMock.Object;

//            var mock = new Mock<ILogger<BankmetadataController>>();
//            _logger = mock.Object;
//        }

//        [TestMethod]
//        public void SourceDetail_OnExecute_ReturnSourceDetailType()
//        {
//            //Arrange
//            BankmetadataController controller = new BankmetadataController(_repo, _logger);

//            //Act
//            var inputDataSource = controller.SourceDetail();
//            var requestResult = inputDataSource as OkObjectResult;

//            //Assert
//            Assert.IsInstanceOfType(requestResult, typeof(OkObjectResult));
//        }

//        [TestMethod]
//        public void SourceDetail_OnExecute_ReturnSourceDetail()
//        {
//            //Arrange
//            BankmetadataController controller = new BankmetadataController(_repo, _logger);

//            //Act
//            var inputDataSource = controller.SourceDetail();
//            var requestResult = inputDataSource as OkObjectResult;
//            InputDataSource result = (InputDataSource)requestResult.Value;

//            //Assert
//            Assert.AreEqual(_repo.GetSourceDetails().InputDataSourceName, result.InputDataSourceName);
//            Assert.AreEqual(_repo.GetSourceDetails().InputDataSourceType, result.InputDataSourceType);
//        }

//        [TestMethod]
//        public void BankMetadatas_OnExecute_ReturnBankMetadatasType()
//        {
//            //Arrange
//            BankmetadataController controller = new BankmetadataController(_repo, _logger);

//            //Act
//            var inputDataSource = controller.BankMetadatas();
//            var requestResult = inputDataSource as OkObjectResult;

//            //Assert
//            Assert.IsInstanceOfType(requestResult, typeof(OkObjectResult));
//        }

//        [TestMethod]
//        public void BankMetadatas_OnExecute_ReturnBankMetadatas()
//        {
//            //Arrange
//            BankmetadataController controller = new BankmetadataController(_repo, _logger);

//            //Act
//            var inputDataSource = controller.BankMetadatas();
//            var requestResult = inputDataSource as OkObjectResult;
//            List<BankMetadata> txnList = (List<BankMetadata>)requestResult.Value;

//            //Assert
//            Assert.AreEqual(3, txnList.Count);
//            CollectionAssert.AreEqual(_repo.GetBankMetadatas().ToList(), txnList, new BankMetadataComparer());
//        }
//    }
//}
