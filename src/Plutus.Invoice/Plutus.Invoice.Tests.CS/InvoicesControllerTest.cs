using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Plutus.Invoices.Service.API.Controllers.V1;
using Plutus.Invoices.Tests.CS.EqualityComparers;
using Plutus.SharedLibrary.CS.Enums;
using Plutus.SharedLibrary.CS.Interfaces;
using Plutus.SharedLibrary.CS.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Plutus.Invoices.Tests.CS
{
    /// <summary>
    /// Test class to test methods in InvoicesController controller class
    /// </summary>
    [TestClass]
    public class InvoicesControllerTest
    {
        private IInvoice _repo;
        private ILogger<InvoicesController> _logger;

        private List<Invoice> _invList1 = new List<Invoice>();

        /// <summary>
        /// Initial setup, create Source List, Moq repo object
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            Invoice inv1 = new Invoice
            {
                Date = Convert.ToDateTime("2010-01-01"),
                Amount = 100.00M,
                ClientName = "Client 1",
                InvoiceReference = "Invoice Ref 1",
                Description = "Description 1"
            };
            Invoice inv2 = new Invoice
            {
                Date = Convert.ToDateTime("2011-01-01"),
                Amount = 200.00M,
                ClientName = "Client 2",
                InvoiceReference = "Invoice Ref 2",
                Description = "Description 2"
            };
            Invoice inv3 = new Invoice
            {
                Date = Convert.ToDateTime("2012-01-01"),
                Amount = 300.00M,
                ClientName = "Client 2",
                InvoiceReference = "Invoice Ref 3",
                Description = "Description 3"
            };

            _invList1.Add(inv1);
            _invList1.Add(inv2);
            _invList1.Add(inv3);

            var repositoryMock = new Mock<IInvoice>();
            repositoryMock.Setup(r => r.GetInvoices()).Returns(_invList1);
            InputDataSource inputSource = new InputDataSource
            {
                InputDataSourceType = DataSource.FileSystem,
                InputDataSourceName = @"C:\Temp"
            };
            repositoryMock.Setup(r => r.GetSourceDetails()).Returns(inputSource);
            _repo = repositoryMock.Object;

            var mock = new Mock<ILogger<InvoicesController>>();
            _logger = mock.Object;
        }

        /// <summary>
        /// Happy path for the SourceDetail action - type
        /// </summary>
        [TestMethod]
        public void SourceDetail_OnExecute_ReturnSourceDetailType()
        {
            //Arrange
            InvoicesController controller = new InvoicesController(_repo, _logger);

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
            InvoicesController controller = new InvoicesController(_repo, _logger);

            //Act
            var inputDataSource = controller.SourceDetail();
            var requestResult = inputDataSource as OkObjectResult;
            InputDataSource result = (InputDataSource)requestResult.Value;

            //Assert
            Assert.AreEqual(_repo.GetSourceDetails().InputDataSourceName, result.InputDataSourceName);
            Assert.AreEqual(_repo.GetSourceDetails().InputDataSourceType, result.InputDataSourceType);
        }

        /// <summary>
        /// Happy path for the Invoices action - type
        /// </summary>
        [TestMethod]
        public void Invoices_OnExecute_ReturnInvoicesType()
        {
            //Arrange
            InvoicesController controller = new InvoicesController(_repo, _logger);

            //Act
            var inputDataSource = controller.Invoices();
            var requestResult = inputDataSource as OkObjectResult;

            //Assert
            Assert.IsInstanceOfType(requestResult, typeof(OkObjectResult));
        }

        /// <summary>
        /// Happy path for the Invoices action - content
        /// </summary>
        [TestMethod]
        public void Invoices_OnExecute_ReturnInvoices()
        {
            //Arrange
            InvoicesController controller = new InvoicesController(_repo, _logger);

            //Act
            var inputDataSource = controller.Invoices();
            var requestResult = inputDataSource as OkObjectResult;
            List<Invoice> txnList = (List<Invoice>)requestResult.Value;

            //Assert
            Assert.AreEqual(3, txnList.Count);
            CollectionAssert.AreEqual(_repo.GetInvoices().ToList(), txnList, new InvoicesComparer());
        }
    }
}
