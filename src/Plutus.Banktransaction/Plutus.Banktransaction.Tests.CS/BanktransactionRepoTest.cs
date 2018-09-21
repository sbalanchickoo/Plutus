using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plutus.Banktransaction.Data.FileSystem.CS.Models;
using Plutus.Banktransaction.Tests.CS.EqualityComparers;
using Plutus.SharedLibrary.CS.Enums;
using Plutus.SharedLibrary.CS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Plutus.Banktransaction.Tests.CS
{
    /// <summary>
    /// Test class to test methods in BanktransactionRepo class
    /// </summary>
    [TestClass]
    public class BanktransactionRepoTest
    {
        private List<BankTransaction> _bankTransactionList1 = new List<BankTransaction>();
        private List<BankTransaction> _bankTransactionList2 = new List<BankTransaction>();
        private List<BankTransaction> _bankTransactionList3 = new List<BankTransaction>();
        private List<BankTransaction> _bankTransactionList4 = new List<BankTransaction>();

        /// <summary>
        /// Initialize test, setup fake lists
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            BankTransaction bankTransaction1 = new BankTransaction
            {
                FITID = "91234567800",
                Merchant = "R/P to John Doe",
                Amount = -500.50M,
                Date = Convert.ToDateTime("2011-12-30")
            };
            BankTransaction bankTransaction2 = new BankTransaction
            {
                FITID = "91234567801",
                Merchant = "R/P to Jane Doe",
                Amount = -1000.50M,
                Date = Convert.ToDateTime("2011-12-31")
            };
            BankTransaction bankTransaction3 = new BankTransaction
            {
                FITID = "91234567802",
                Merchant = "R/P to Jack Doe",
                Amount = -1500.50M,
                Date = Convert.ToDateTime("2012-12-31")
            };
            BankTransaction bankTransaction4 = new BankTransaction
            {
                FITID = "91234567803",
                Merchant = "R/P to Jack Doe",
                Amount = -2000.50M,
                Date = Convert.ToDateTime("2012-12-31")
            };
            _bankTransactionList1.Add(bankTransaction1);
            _bankTransactionList1.Add(bankTransaction2);

            _bankTransactionList2.Add(bankTransaction2);
            _bankTransactionList2.Add(bankTransaction3);

            _bankTransactionList3.Add(bankTransaction1);
            _bankTransactionList3.Add(bankTransaction2);
            _bankTransactionList3.Add(bankTransaction3);

            _bankTransactionList4.Add(bankTransaction1);
            _bankTransactionList4.Add(bankTransaction2);
            _bankTransactionList4.Add(bankTransaction3);
            _bankTransactionList4.Add(bankTransaction4);
        }

        /// <summary>
        /// Happy path for the GetSourceDetails method
        /// </summary>
        [TestMethod]
        public void GetSourceDetails_OnNonBlankInput_ReturnInputSource()
        {
            //Arrange
            BanktransactionRepo repo = new BanktransactionRepo
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
            BanktransactionRepo repo = new BanktransactionRepo();
            
            //Act
            InputDataSource inputSource = repo.GetSourceDetails();

            //Assert
            Assert.IsNotNull(inputSource.InputDataSourceName);
            Assert.AreEqual(DataSource.FileSystem, inputSource.InputDataSourceType);
        }

        /// <summary>
        /// Failure path for the ExtractBankTransactionsFromOfx method - Bad input Ofx, return empty list
        /// </summary>
        [TestMethod]
        public void ExtractBankTransactionsFromOfx_OnInvalidOfx_ReturnEmptyTxnList()
        {
            //Arrange
            List<BankTransaction> txnList = new List<BankTransaction>();
            string input1 = File.ReadAllText(@"..\..\..\TestFiles1\Ofx_Invalid_1.OFX");

            //Act
            BanktransactionRepo repo = new BanktransactionRepo();
            List<BankTransaction> output1 = repo.ExtractBankTransactionsFromOfx(input1).ToList();

            //Assert
            Assert.AreEqual(0, output1.Count);
            CollectionAssert.AreEqual(txnList, output1, new BankTransactionComparer());
        }

        /// <summary>
        /// Failure path for the ExtractBankTransactionsFromOfx method - Bad input Xml, return empty list
        /// </summary>
        [TestMethod]
        public void ExtractBankTransactionsFromOfx_OnInvalidXml_ReturnEmptyTxnList()
        {
            //Arrange
            List<BankTransaction> txnList = new List<BankTransaction>();
            string input1 = File.ReadAllText(@"..\..\..\TestFiles1\Ofx_Invalid_2.OFX");

            //Act
            BanktransactionRepo repo = new BanktransactionRepo();
            List<BankTransaction> output1 = repo.ExtractBankTransactionsFromOfx(input1).ToList();

            //Assert
            Assert.AreEqual(0, output1.Count);
            CollectionAssert.AreEqual(txnList, output1, new BankTransactionComparer());
        }

        /// <summary>
        /// Happy path for the ExtractBankTransactionsFromOfx method
        /// </summary>
        [TestMethod]
        public void ExtractBankTransactionsFromOfx_OnValidInputs_ReturnTxnList()
        {
            //Arrange
            
            //Act
            BanktransactionRepo repo = new BanktransactionRepo
            {
                FolderName = @"..\..\..\TestFiles2"
            };
            List<BankTransaction> output1 = repo.GetBankTransactions().ToList();

            //Assert
            Assert.AreEqual(_bankTransactionList1.Count, output1.Count);
            CollectionAssert.AreEqual(_bankTransactionList1, output1, new BankTransactionComparer());
        }

        /// <summary>
        /// Happy path for the ConsolidateTransactionsFromLists method
        /// </summary>
        [TestMethod]
        public void ConsolidateTransactionsFromLists_OnExecute_ReturnConsolidatedTxnList()
        {
            //Arrange
            List<List<BankTransaction>> txnInput = new List<List<BankTransaction>>
            {
                _bankTransactionList1,
                _bankTransactionList2
            };

            //Act
            BanktransactionRepo repo = new BanktransactionRepo();
            List<BankTransaction> output1 = repo.ConsolidateTransactionsFromLists(txnInput).ToList();

            //Assert
            Assert.AreEqual(_bankTransactionList3.Count, output1.Count);
            CollectionAssert.AreEqual(_bankTransactionList3, output1, new BankTransactionComparer());
        }

        /// <summary>
        /// Test for the GetBankTransactions - Folder change functionality
        /// </summary>
        [TestMethod]
        public void GetBankTransactions_OnFolderChange_ReturnTxnList()
        {
            //Arrange
            File.Delete(@"..\..\..\TestFiles3\Ofx_Valid_3.OFX");

            //Act
            BanktransactionRepo repo = new BanktransactionRepo
            {
                FolderName = @"..\..\..\TestFiles3"
            };
            List<BankTransaction> output1 = repo.GetBankTransactions().ToList();
            File.Copy(@"..\..\..\TestFiles1\Ofx_Valid_3.OFX", @"..\..\..\TestFiles3\Ofx_Valid_3.OFX");
            System.Threading.Thread.Sleep(5000);
            output1 = repo.GetBankTransactions().ToList();

            //Assert
            Assert.AreEqual(_bankTransactionList4.Count, output1.Count);
            CollectionAssert.AreEqual(_bankTransactionList4, output1, new BankTransactionComparer());
        }
    }
}
