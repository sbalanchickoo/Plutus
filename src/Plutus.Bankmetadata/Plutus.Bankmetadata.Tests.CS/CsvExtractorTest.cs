using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plutus.Bankmetadata.Data.FileSystem.CS.Models;
using Plutus.Bankmetadata.Tests.CS.EqualityComparers;
using Plutus.SharedLibrary.CS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Plutus.Bankmetadata.Tests.CS
{
    /// <summary>
    /// Test class to test methods in CsvExtractor class
    /// </summary>
    [TestClass]
    public class CsvExtractorTest
    {
        private List<BankMetadata> _bankMetadataList1 = new List<BankMetadata>();

        /// <summary>
        /// Test initialization step, List of fake data
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
            _bankMetadataList1.Add(bankMetadata1);
            _bankMetadataList1.Add(bankMetadata2);
        }

        /// <summary>
        /// Happy path for the ExtractBankMetadataFromCsv method
        /// </summary>
        [TestMethod]
        public void ExtractBankMetadataFromCsv_OnValidCsv_ReturnMetadataList()
        {
            //Arrange
            string input1 = File.ReadAllText(@"..\..\..\TestFiles1\Csv_Valid_1.CSV");
            
            //Act
            CsvExtractor repo = new CsvExtractor();
            List<BankMetadata> output1 = repo.ExtractBankMetadataFromCsvString(input1).ToList();

            //Assert
            CollectionAssert.AreEqual(_bankMetadataList1, output1, new BankMetadataComparer());
        }

        /// <summary>
        /// Exception test for the ExtractBankMetadatasFromCsv method
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FieldAccessException))]
        public void ExtractBankMetadatasFromCsv_OnInvalidCsv_ThrowException()
        {
            //Arrange
            string input1 = File.ReadAllText(@"..\..\..\TestFiles1\Csv_Invalid_1.CSV");
            CsvExtractor repo = new CsvExtractor();

            //Act, Assert
            List<BankMetadata> output1 = repo.ExtractBankMetadataFromCsvString(input1).ToList();
        }

        /// <summary>
        /// Exception message test for the ExtractBankMetadatasFromCsv method
        /// </summary>
        [TestMethod]
        public void ExtractBankMetadatasFromCsv_OnInvalidXml_ThrowCorrectExceptionMessage()
        {
            //Arrange
            string input1 = File.ReadAllText(@"..\..\..\TestFiles1\Csv_Invalid_1.CSV");
            CsvExtractor repo = new CsvExtractor();

            //Act
            try
            {
                List<BankMetadata> output1 = repo.ExtractBankMetadataFromCsvString(input1).ToList();
            }

            //Assert
            catch (Exception ex)
            {
                Assert.AreEqual("Badly formatted CSV string", ex.Message);
            }
        }
    }
}
