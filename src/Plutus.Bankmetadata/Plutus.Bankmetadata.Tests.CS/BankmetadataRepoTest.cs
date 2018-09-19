using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plutus.Bankmetadata.Data.FileSystem.CS.Models;
using Plutus.Bankmetadata.Tests.CS.EqualityComparers;
using Plutus.SharedLibrary.CS.Enums;
using Plutus.SharedLibrary.CS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Plutus.Bankmetadata.Tests.CS
{
    /// <summary>
    /// Test class to test methods in BankmetadataRepo class
    /// </summary>
    [TestClass]
    public class BankmetadataRepoTest
    {
        /// <summary>
        /// Happy path for the GetSourceDetails method
        /// </summary>
        [TestMethod]
        public void GetSourceDetails_OnNonBlankInput_ReturnInputSource()
        {
            //Arrange
            BankmetadataRepo repo = new BankmetadataRepo
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
            BankmetadataRepo repo = new BankmetadataRepo();

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
        public void ExtractBankMetadataFromCsv_OnInvalidCsv_ReturnEmptyTxnList()
        {
            //Arrange
            List<BankMetadata> txnList = new List<BankMetadata>();
            string input1 = File.ReadAllText(@"..\..\..\TestFiles1\Csv_Invalid_1.CSV");

            //Act
            BankmetadataRepo repo = new BankmetadataRepo();
            List<BankMetadata> output1 = repo.ExtractBankMetadataFromCsv(input1)
                .OrderBy(txn => txn.Date)
                .OrderBy(txn => txn.Amount)
                .OrderBy(txn => txn.Merchant)
                .ToList();

            //Assert
            Assert.AreEqual(0, output1.Count);
            CollectionAssert.AreEqual(txnList, output1, new BankMetadataComparer());
        }

        /// <summary>
        /// Happy path for the ExtractBankTransactionsFromOfx method
        /// </summary>
        [TestMethod]
        public void ExtractBankMetadataFromCsv_OnValidInputs_ReturnTxnList()
        {
            //Arrange
            List<BankMetadata> txnList = new List<BankMetadata>
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

            //Act
            BankmetadataRepo repo = new BankmetadataRepo
            {
                FolderName = @"..\..\..\TestFiles2"
            };
            List<BankMetadata> output1 = repo.GetBankMetadata()
                .OrderBy(txn => txn.Date)
                .OrderBy(txn => txn.Amount)
                .OrderBy(txn => txn.Merchant)
                .ToList();

            //Assert
            Assert.AreEqual(3, output1.Count);
            CollectionAssert.AreEqual(txnList, output1, new BankMetadataComparer());
        }

        /// <summary>
        /// Happy path for the ConsolidateTransactionsFromLists method
        /// </summary>
        [TestMethod]
        public void ConsolidateMetadataFromLists_OnExecute_ReturnConsolidatedTxnList()
        {
            //Arrange
            List<BankMetadata> txnListConsolidated = new List<BankMetadata>
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

            List<BankMetadata> txnList1 = new List<BankMetadata>
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
            }
                .OrderBy(txn => txn.Date)
                .OrderBy(txn => txn.Amount)
                .OrderBy(txn => txn.Merchant)
                .ToList();

            List<BankMetadata> txnList2 = new List<BankMetadata>
            {
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

            List<List<BankMetadata>> txnInput = new List<List<BankMetadata>>
            {
                txnList1,
                txnList2
            };

            //Act
            BankmetadataRepo repo = new BankmetadataRepo();
            List<BankMetadata> output1 = repo.ConsolidateMetadataFromLists(txnInput)
                .OrderBy(txn => txn.Date)
                .OrderBy(txn => txn.Amount)
                .OrderBy(txn => txn.Merchant)
                .ToList();

            //Assert
            Assert.AreEqual(3, output1.Count);
            CollectionAssert.AreEqual(txnListConsolidated, output1, new BankMetadataComparer());
        }

        /// <summary>
        /// Test for the GetBankTransactions - Folder change functionality
        /// </summary>
        [TestMethod]
        public void GetBankMetadata_OnFolderChange_ReturnTxnList()
        {
            //Arrange
            List<BankMetadata> txnList = new List<BankMetadata>
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
                ,
                new BankMetadata
                {
                    Date = Convert.ToDateTime("2013-01-01"),
                    Amount = 500.00M,
                    UserComments = "Comment, 2",
                    Merchant = "Payee 4",
                    TransactionCategory = "Cash received from clients"
                }
            }
                .OrderBy(txn => txn.Date)
                .OrderBy(txn => txn.Amount)
                .OrderBy(txn => txn.Merchant)
                .ToList();
            File.Delete(@"..\..\..\TestFiles3\ban_Valid_3.CSV");

            //Act
            BankmetadataRepo repo = new BankmetadataRepo
            {
                FolderName = @"..\..\..\TestFiles3"
            };
            List<BankMetadata> output1 = repo.GetBankMetadata()
                .OrderBy(txn => txn.Date)
                .OrderBy(txn => txn.Amount)
                .OrderBy(txn => txn.Merchant)
                .ToList();
            File.Copy(@"..\..\..\TestFiles1\ban_Valid_3.CSV", @"..\..\..\TestFiles3\ban_Valid_3.CSV");
            System.Threading.Thread.Sleep(5000);
            output1 = repo.GetBankMetadata()
                .OrderBy(txn => txn.Date)
                .OrderBy(txn => txn.Amount)
                .OrderBy(txn => txn.Merchant)
                .ToList();

            //Assert
            Assert.AreEqual(4, output1.Count);
            CollectionAssert.AreEqual(txnList, output1, new BankMetadataComparer());
        }
    }
}
