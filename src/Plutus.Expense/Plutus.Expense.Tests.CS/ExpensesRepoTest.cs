using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plutus.Expenses.Data.FileSystem.CS.Models;
using Plutus.Expenses.Tests.CS.EqualityComparers;
using Plutus.SharedLibrary.CS.Enums;
using Plutus.SharedLibrary.CS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Plutus.Expenses.Tests.CS
{
    /// <summary>
    /// Test class to test methods in ExpensesRepo class
    /// </summary>
    [TestClass]
    public class ExpensesRepoTest
    {
        private List<Expense> _expList1 = new List<Expense>();
        private List<Expense> _expList2 = new List<Expense>();
        private List<Expense> _expList3 = new List<Expense>();
        private List<Expense> _expList4 = new List<Expense>();

        /// <summary>
        /// Initialize test, setup fake lists
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
            Expense exp3 = new Expense
            {
                Date = Convert.ToDateTime("2012-01-01"),
                Amount = 300.00M,
                TransactionCategory = "Category 2",
                Description = "Payee 2"
            };
            Expense exp4 = new Expense
            {
                Date = Convert.ToDateTime("2012-01-01"),
                Amount = 300.00M,
                TransactionCategory = "Category 3",
                Description = "Payee 2"
            };
            _expList1.Add(exp1);
            _expList1.Add(exp2);

            _expList2.Add(exp2);
            _expList2.Add(exp3);

            _expList3.Add(exp1);
            _expList3.Add(exp2);
            _expList3.Add(exp3);

            _expList4.Add(exp1);
            _expList4.Add(exp2);
            _expList4.Add(exp3);
            _expList4.Add(exp4);

        }

        /// <summary>
        /// Happy path for the GetSourceDetails method
        /// </summary>
        [TestMethod]
        public void GetSourceDetails_OnNonBlankInput_ReturnInputSource()
        {
            //Arrange
            ExpensesRepo repo = new ExpensesRepo
            {
                FolderName = @"C:\Temp"
            };

            //Act
            InputDataSource inputSource = repo.GetSourceDetails();

            //Assert
            Assert.AreEqual(@"C:\Temp", inputSource.InputDataSourceName);
            Assert.AreEqual(DataSource.FileSystem, inputSource.InputDataSourceType);
        }

        /// <summary>
        /// Failure path for the GetSourceDetails method - Blank input
        /// </summary>
        [TestMethod]
        public void GetSourceDetails_OnBlankInput_ReturnInputSource()
        {
            //Arrange
            ExpensesRepo repo = new ExpensesRepo();

            //Act
            InputDataSource inputSource = repo.GetSourceDetails();

            //Assert
            Assert.IsNotNull(inputSource.InputDataSourceName);
            Assert.AreEqual(DataSource.FileSystem, inputSource.InputDataSourceType);
        }

        /// <summary>
        /// Failure path for the ExtractBankTransactionsFromOfx method - Bad input CSV, return empty list
        /// </summary>
        [TestMethod]
        public void ExtractExpenseFromCsv_OnInvalidCsv_ReturnEmptyTxnList()
        {
            //Arrange
            List<Expense> txnList = new List<Expense>();
            string input1 = File.ReadAllText(@"..\..\..\TestFiles1\Csv_Invalid_1.CSV");

            //Act
            ExpensesRepo repo = new ExpensesRepo();
            List<Expense> output1 = repo.ExtractExpensesFromCsv(input1)
                .OrderBy(txn => txn.Date)
                .OrderBy(txn => txn.Amount)
                .OrderBy(txn => txn.TransactionCategory)
                .OrderBy(txn => txn.Description)
                .ToList();

            //Assert
            Assert.AreEqual(0, output1.Count);
            CollectionAssert.AreEqual(txnList, output1, new ExpenseComparer());
        }

        /// <summary>
        /// Happy path for the ExtractExpensesFromCsv method
        /// </summary>
        [TestMethod]
        public void ExtractExpensesFromCsv_OnValidInputs_ReturnExpensesList()
        {
            //Arrange
            List<Expense> txnList = _expList3
                .OrderBy(txn => txn.Date)
                .OrderBy(txn => txn.Amount)
                .OrderBy(txn => txn.TransactionCategory)
                .OrderBy(txn => txn.Description)
                .ToList();

            //Act
            ExpensesRepo repo = new ExpensesRepo
            {
                FolderName = @"..\..\..\TestFiles2"
            };
            List<Expense> output1 = repo.GetExpenses()
                .OrderBy(txn => txn.Date)
                .OrderBy(txn => txn.Amount)
                .OrderBy(txn => txn.TransactionCategory)
                .OrderBy(txn => txn.Description)
                .ToList();

            //Assert
            Assert.AreEqual(txnList.Count, output1.Count);
            //CollectionAssert.AreEqual(txnList, output1, new ExpenseComparer());
        }

        /// <summary>
        /// Happy path for the ConsolidateExpensesFromLists method
        /// </summary>
        [TestMethod]
        public void ConsolidateMetadataFromLists_OnExecute_ReturnConsolidatedTxnList()
        {
            //Arrange
            List<Expense> txnListConsolidated = _expList3
                .OrderBy(txn => txn.Date)
                .OrderBy(txn => txn.Amount)
                .OrderBy(txn => txn.TransactionCategory)
                .OrderBy(txn => txn.Description)
                .ToList();

            List<Expense> txnList1 = _expList1
                .OrderBy(txn => txn.Date)
                .OrderBy(txn => txn.Amount)
                .OrderBy(txn => txn.TransactionCategory)
                .OrderBy(txn => txn.Description)
                .ToList();

            List<Expense> txnList2 = _expList2
                .OrderBy(txn => txn.Date)
                .OrderBy(txn => txn.Amount)
                .OrderBy(txn => txn.TransactionCategory)
                .OrderBy(txn => txn.Description)
                .ToList();

            List<List<Expense>> txnInput = new List<List<Expense>>
            {
                txnList1,
                txnList2
            };

            //Act
            ExpensesRepo repo = new ExpensesRepo();
            List<Expense> output1 = repo.ConsolidateMetadataFromLists(txnInput)
                .OrderBy(txn => txn.Date)
                .OrderBy(txn => txn.Amount)
                .OrderBy(txn => txn.TransactionCategory)
                .OrderBy(txn => txn.Description)
                .ToList();

            //Assert
            Assert.AreEqual(3, output1.Count);
            CollectionAssert.AreEqual(txnListConsolidated, output1, new ExpenseComparer());
        }

        /// <summary>
        /// Test for the GetBankTransactions - Folder change functionality
        /// </summary>
        [TestMethod]
        public void GetExpense_OnFolderChange_ReturnTxnList()
        {
            //Arrange
            List<Expense> txnList = _expList4
                .OrderBy(txn => txn.Date)
                .OrderBy(txn => txn.Amount)
                .OrderBy(txn => txn.TransactionCategory)
                .OrderBy(txn => txn.Description)
                .ToList();

            File.Delete(@"..\..\..\TestFiles3\exp_Valid_3.CSV");

            //Act
            ExpensesRepo repo = new ExpensesRepo
            {
                FolderName = @"..\..\..\TestFiles3"
            };
            List<Expense> output1 = repo.GetExpenses()
                .OrderBy(txn => txn.Date)
                .OrderBy(txn => txn.Amount)
                .OrderBy(txn => txn.TransactionCategory)
                .OrderBy(txn => txn.Description)
                .ToList();

            File.Copy(@"..\..\..\TestFiles1\exp_Valid_3.CSV", @"..\..\..\TestFiles3\exp_Valid_3.CSV");
            System.Threading.Thread.Sleep(5000);
            output1 = repo.GetExpenses()
                .OrderBy(txn => txn.Date)
                .OrderBy(txn => txn.Amount)
                .OrderBy(txn => txn.TransactionCategory)
                .OrderBy(txn => txn.Description)
                .ToList();

            //Assert
            Assert.AreEqual(4, output1.Count);
            CollectionAssert.AreEqual(txnList, output1, new ExpenseComparer());
        }
    }
}
