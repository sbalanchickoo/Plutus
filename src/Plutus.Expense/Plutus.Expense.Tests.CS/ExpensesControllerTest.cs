using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Plutus.Expenses.Service.API.Controllers.V1;
using Plutus.Expenses.Tests.CS.EqualityComparers;
using Plutus.SharedLibrary.CS.Enums;
using Plutus.SharedLibrary.CS.Interfaces;
using Plutus.SharedLibrary.CS.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Plutus.Expenses.Tests.CS
{
    /// <summary>
    /// Test class to test methods in BanktransactionController controller class
    /// </summary>
    [TestClass]
    public class ExpensesControllerTest
    {
        private IExpense _repo;
        private ILogger<ExpensesController> _logger;
        private List<Expense> _expList1 = new List<Expense>();

        /// <summary>
        /// Test initialization step, List of fake data, Moq repo
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            Expense exp1 = new Expense
            {
                Date = Convert.ToDateTime("2010-01-01"),
                Amount = 100.00M,
                TransactionCategory = "Category 1",
                Description = "Payee 1"
            };
            Expense exp2 = new Expense
            {
                Date = Convert.ToDateTime("2011-01-01"),
                Amount = 200.00M,
                TransactionCategory = "Category 2",
                Description = "Payee 2"
            };
            _expList1.Add(exp1);
            _expList1.Add(exp2);
            
            var repositoryMock = new Mock<IExpense>();
            repositoryMock.Setup(r => r.GetExpenses()).Returns
                (
                _expList1.OrderBy(txn => txn.Date)
                .OrderBy(txn => txn.Amount)
                .OrderBy(txn => txn.TransactionCategory)
                .OrderBy(txn => txn.Description)
                .ToList()
                );
            InputDataSource inputSource = new InputDataSource
            {
                InputDataSourceType = DataSource.FileSystem,
                InputDataSourceName = @"C:\Temp"
            };
            repositoryMock.Setup(r => r.GetSourceDetails()).Returns(inputSource);
            _repo = repositoryMock.Object;

            var mock = new Mock<ILogger<ExpensesController>>();
            _logger = mock.Object;
        }

        /// <summary>
        /// Happy path for the SourceDetail action - type
        /// </summary>
        [TestMethod]
        public void SourceDetail_OnExecute_ReturnSourceDetailType()
        {
            //Arrange
            ExpensesController controller = new ExpensesController(_repo, _logger);

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
            ExpensesController controller = new ExpensesController(_repo, _logger);

            //Act
            var inputDataSource = controller.SourceDetail();
            var requestResult = inputDataSource as OkObjectResult;
            InputDataSource result = (InputDataSource)requestResult.Value;

            //Assert
            Assert.AreEqual(_repo.GetSourceDetails().InputDataSourceName, result.InputDataSourceName);
            Assert.AreEqual(_repo.GetSourceDetails().InputDataSourceType, result.InputDataSourceType);
        }

        /// <summary>
        /// Happy path for the Expenses action - type
        /// </summary>
        [TestMethod]
        public void Expenses_OnExecute_ReturnExpensesType()
        {
            //Arrange
            ExpensesController controller = new ExpensesController(_repo, _logger);

            //Act
            var inputDataSource = controller.Expenses();
            var requestResult = inputDataSource as OkObjectResult;

            //Assert
            Assert.IsInstanceOfType(requestResult, typeof(OkObjectResult));
        }

        /// <summary>
        /// Happy path for the Expenses action - content
        /// </summary>
        [TestMethod]
        public void Expenses_OnExecute_ReturnExpenses()
        {
            //Arrange
            ExpensesController controller = new ExpensesController(_repo, _logger);

            //Act
            var inputDataSource = controller.Expenses();
            var requestResult = inputDataSource as OkObjectResult;
            List<Expense> txnList = (List<Expense>)requestResult.Value;

            //Assert
            Assert.AreEqual(3, txnList.Count);
            CollectionAssert.AreEqual(_repo.GetExpenses().ToList(), txnList, new ExpenseComparer());
        }
    }
}
