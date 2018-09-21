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
        private List<BankMetadata> _bankMetadataList1 = new List<BankMetadata>();
        private List<BankMetadata> _bankMetadataList2 = new List<BankMetadata>();
        private List<BankMetadata> _bankMetadataList3 = new List<BankMetadata>();
        private List<BankMetadata> _bankMetadataList4 = new List<BankMetadata>();

        /// <summary>
        /// Initialize test, setup fake lists
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            BankMetadata bankMetadata1 = new BankMetadata
            {
                Date = Convert.ToDateTime("2010-01-01"),
                Amount = 100.00M,
                UserComments = "Comment 1",
                Merchant = "Payee 1",
                TransactionCategory = "Money Received from Employee"
            };
            BankMetadata bankMetadata2 = new BankMetadata
            {
                Date = Convert.ToDateTime("2011-01-01"),
                Amount = 500.00M,
                UserComments = "Comment, 2",
                Merchant = "Payee 2",
                TransactionCategory = "Cash received from clients"
            };
            BankMetadata bankMetadata3 = new BankMetadata
            {
                Date = Convert.ToDateTime("2011-01-01"),
                Amount = 500.00M,
                UserComments = "Comment, 2",
                Merchant = "Payee 3",
                TransactionCategory = "Cash received from clients"
            };
            BankMetadata bankMetadata4 = new BankMetadata
            {
                Date = Convert.ToDateTime("2013-01-01"),
                Amount = 500.00M,
                UserComments = "Comment, 2",
                Merchant = "Payee 4",
                TransactionCategory = "Cash received from clients"
            };
            _bankMetadataList1.Add(bankMetadata1);
            _bankMetadataList1.Add(bankMetadata2);

            _bankMetadataList2.Add(bankMetadata2);
            _bankMetadataList2.Add(bankMetadata3);

            _bankMetadataList3.Add(bankMetadata1);
            _bankMetadataList3.Add(bankMetadata2);
            _bankMetadataList3.Add(bankMetadata3);

            _bankMetadataList4.Add(bankMetadata1);
            _bankMetadataList4.Add(bankMetadata2);
            _bankMetadataList4.Add(bankMetadata3);
            _bankMetadataList4.Add(bankMetadata4);
        }

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
        /// Failure path for the ExtractBankMetadataFromCsv method - Bad input CSV, return empty list
        /// </summary>
        [TestMethod]
        public void ExtractBankMetadataFromCsv_OnInvalidCsv_ReturnEmptyTxnList()
        {
            //Arrange
            List<BankMetadata> txnList = new List<BankMetadata>();
            string input1 = File.ReadAllText(@"..\..\..\TestFiles1\Csv_Invalid_1.CSV");

            //Act
            BankmetadataRepo repo = new BankmetadataRepo();
            List<BankMetadata> output1 = repo.ExtractBankMetadataFromCsv(input1).ToList();

            //Assert
            Assert.AreEqual(0, output1.Count);
            CollectionAssert.AreEqual(txnList, output1, new BankMetadataComparer());
        }

        /// <summary>
        /// Happy path for the GetBankMetadata method
        /// </summary>
        [TestMethod]
        public void GetBankMetadata_OnValidInputs_ReturnMetadataList()
        {
            //Arrange
            
            //Act
            BankmetadataRepo repo = new BankmetadataRepo
            {
                FolderName = @"..\..\..\TestFiles2"
            };
            List<BankMetadata> output1 = repo.GetBankMetadata().ToList();

            //Assert
            Assert.AreEqual(_bankMetadataList3.Count, output1.Count);
            CollectionAssert.AreEqual(_bankMetadataList3, output1, new BankMetadataComparer());
        }

        /// <summary>
        /// Happy path for the ConsolidateMetadataFromLists method
        /// </summary>
        [TestMethod]
        public void ConsolidateMetadataFromLists_OnExecute_ReturnConsolidatedMetadataList()
        {
            //Arrange
            List<List<BankMetadata>> txnInput = new List<List<BankMetadata>>
            {
                _bankMetadataList1,
                _bankMetadataList2
            };

            //Act
            BankmetadataRepo repo = new BankmetadataRepo();
            List<BankMetadata> output1 = repo.ConsolidateMetadataFromLists(txnInput).ToList();

            //Assert
            Assert.AreEqual(_bankMetadataList3.Count, output1.Count);
            CollectionAssert.AreEqual(_bankMetadataList3, output1, new BankMetadataComparer());
        }

        /// <summary>
        /// Test for the GetBankMetadata - Folder change functionality
        /// </summary>
        [TestMethod]
        public void GetBankMetadata_OnFolderChange_ReturnTxnList()
        {
            //Arrange
            File.Delete(@"..\..\..\TestFiles3\ban_Valid_3.CSV");

            //Act
            BankmetadataRepo repo = new BankmetadataRepo
            {
                FolderName = @"..\..\..\TestFiles3"
            };
            List<BankMetadata> output1 = repo.GetBankMetadata().ToList();
            File.Copy(@"..\..\..\TestFiles1\ban_Valid_3.CSV", @"..\..\..\TestFiles3\ban_Valid_3.CSV");
            System.Threading.Thread.Sleep(5000);
            output1 = repo.GetBankMetadata().ToList();

            //Assert
            Assert.AreEqual(_bankMetadataList4.Count, output1.Count);
            CollectionAssert.AreEqual(_bankMetadataList4, output1, new BankMetadataComparer());
        }
    }
}
