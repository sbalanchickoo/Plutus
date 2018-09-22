using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plutus.Expenses.Data.FileSystem.CS.Models;
using Plutus.Expenses.Tests.CS.EqualityComparers;
using Plutus.SharedLibrary.CS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Plutus.Expenses.Tests.CS
{
    /// <summary>
    /// Test class to test methods in CsvExtractor class
    /// </summary>
    [TestClass]
    public class CsvExtractorTest
    {
        private List<Expense> _expList1 = new List<Expense>();

        /// <summary>
        /// Test initialization step, List of fake data
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
        }

        /// <summary>
        /// Happy path for the ExtractExpensesFromCsv method
        /// </summary>
        [TestMethod]
        public void ExtractExpensesFromCsv_OnValidCsv_ReturnExpensesList()
        {
            //Arrange
            string input1 = File.ReadAllText(@"..\..\..\TestFiles1\Csv_Valid_1.CSV");
            
            //Act
            CsvExtractor repo = new CsvExtractor();
            List<Expense> output1 = repo.ExtractExpensesFromCsvString(input1).ToList();

            //Assert
            CollectionAssert.AreEqual(_expList1, output1, new ExpenseComparer());
        }

        /// <summary>
        /// Exception test for the ExtractExpensesFromCsv method
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FieldAccessException))]
        public void ExtractExpensesFromCsv_OnInvalidCsv_ThrowException()
        {
            //Arrange
            string input1 = File.ReadAllText(@"..\..\..\TestFiles1\Csv_Invalid_1.CSV");
            CsvExtractor repo = new CsvExtractor();

            //Act, Assert
            List<Expense> output1 = repo.ExtractExpensesFromCsvString(input1).ToList();
        }

        /// <summary>
        /// Exception message test for the ExtractExpensesFromCsv method
        /// </summary>
        [TestMethod]
        public void ExtractExpensesFromCsv_OnInvalidXml_ThrowCorrectExceptionMessage()
        {
            //Arrange
            string input1 = File.ReadAllText(@"..\..\..\TestFiles1\Csv_Invalid_1.CSV");
            CsvExtractor repo = new CsvExtractor();

            //Act
            try
            {
                List<Expense> output1 = repo.ExtractExpensesFromCsvString(input1).ToList();
            }

            //Assert
            catch (Exception ex)
            {
                Assert.AreEqual("Badly formatted CSV string", ex.Message);
            }
        }
    }
}
