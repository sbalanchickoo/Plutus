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
    [TestClass]
    public class BanktransactionRepoTest
    {
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

        [TestMethod]
        public void ExtractBankTransactionsFromOfx_OnInvalidOfx_ReturnEmptyTxnList()
        {
            //Arrange
            List<BankTransaction> txnList = new List<BankTransaction>();
            string input1 = File.ReadAllText(@"..\..\..\TestFiles1\Ofx_Invalid_1.OFX");

            //Act
            BanktransactionRepo repo = new BanktransactionRepo();
            List<BankTransaction> output1 = repo.ExtractBankTransactionsFromOfx(input1).OrderBy(txn => txn.FITID).ToList();

            //Assert
            Assert.AreEqual(0, output1.Count);
            CollectionAssert.AreEqual(txnList, output1, new BankTransactionComparer());
        }

        [TestMethod]
        public void ExtractBankTransactionsFromOfx_OnInvalidXml_ReturnEmptyTxnList()
        {
            //Arrange
            List<BankTransaction> txnList = new List<BankTransaction>();
            string input1 = File.ReadAllText(@"..\..\..\TestFiles1\Ofx_Invalid_2.OFX");

            //Act
            BanktransactionRepo repo = new BanktransactionRepo();
            List<BankTransaction> output1 = repo.ExtractBankTransactionsFromOfx(input1).OrderBy(txn => txn.FITID).ToList();

            //Assert
            Assert.AreEqual(0, output1.Count);
            CollectionAssert.AreEqual(txnList, output1, new BankTransactionComparer());
        }

        [TestMethod]
        public void ExtractBankTransactionsFromOfx_OnValidInputs_ReturnTxnList()
        {
            //Arrange
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
                }
            }.OrderBy(txn => txn.FITID).ToList();

            //Act
            BanktransactionRepo repo = new BanktransactionRepo
            {
                FolderName = @"..\..\..\TestFiles2"
            };
            List<BankTransaction> output1 = repo.GetBankTransactions().OrderBy(txn => txn.FITID).ToList();

            //Assert
            Assert.AreEqual(2, output1.Count);
            CollectionAssert.AreEqual(txnList, output1, new BankTransactionComparer());
        }

        [TestMethod]
        public void ConsolidateTransactionsFromLists_OnExecute_ReturnConsolidatedTxnList()
        {
            //Arrange
            List<BankTransaction> txnListConsolidated = new List<BankTransaction>
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

            List<BankTransaction> txnList1 = new List<BankTransaction>
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
                }
            }.OrderBy(txn => txn.FITID).ToList();

            List<BankTransaction> txnList2 = new List<BankTransaction>
            {
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

            List<List<BankTransaction>> txnInput = new List<List<BankTransaction>>();
            txnInput.Add(txnList1);
            txnInput.Add(txnList2);

            //Act
            BanktransactionRepo repo = new BanktransactionRepo();
            List<BankTransaction> output1 = repo.ConsolidateTransactionsFromLists(txnInput).OrderBy(txn => txn.FITID).ToList();

            //Assert
            Assert.AreEqual(3, output1.Count);
            CollectionAssert.AreEqual(txnListConsolidated, output1, new BankTransactionComparer());
        }

        [TestMethod]
        public void GetBankTransactions_OnFolderChange_ReturnTxnList()
        {
            //Arrange
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
            File.Delete(@"..\..\..\TestFiles3\Ofx_Valid_3.OFX");

            //Act
            BanktransactionRepo repo = new BanktransactionRepo
            {
                FolderName = @"..\..\..\TestFiles3"
            };
            List<BankTransaction> output1 = repo.GetBankTransactions().OrderBy(txn => txn.FITID).ToList();
            File.Copy(@"..\..\..\TestFiles1\Ofx_Valid_3.OFX", @"..\..\..\TestFiles3\Ofx_Valid_3.OFX");
            System.Threading.Thread.Sleep(5000);
            output1 = repo.GetBankTransactions().OrderBy(txn => txn.FITID).ToList();

            //Assert
            Assert.AreEqual(3, output1.Count);
            CollectionAssert.AreEqual(txnList, output1, new BankTransactionComparer());
        }
    }
}
