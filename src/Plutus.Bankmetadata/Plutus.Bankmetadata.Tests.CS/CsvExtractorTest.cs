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
    [TestClass]
    public class CsvExtractorTest
    {
        [TestMethod]
        public void ExtractBankMetadatasFromCsv_OnValidCsv_ReturnMetadataList()
        {
            //Arrange
            string input1 = File.ReadAllText(@"..\..\..\TestFiles1\Csv_Valid_1.CSV");
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
            }
                .OrderBy(txn => txn.Date)
                .OrderBy(txn => txn.Amount)
                .OrderBy(txn => txn.Merchant)
                .ToList();

            //Act
            CsvExtractor repo = new CsvExtractor();
            List<BankMetadata> output1 = repo.ExtractBankMetadataFromCsvString(input1)
                .OrderBy(txn => txn.Date)
                .OrderBy(txn => txn.Amount)
                .OrderBy(txn => txn.Merchant)
                .ToList();

            //Assert
            CollectionAssert.AreEqual(txnList, output1, new BankMetadataComparer());
        }

        [TestMethod]
        [ExpectedException(typeof(FieldAccessException))]
        public void ExtractBankMetadatasFromCsv_OnInvalidCsv_ThrowException()
        {
            //Arrange
            string input1 = File.ReadAllText(@"..\..\..\TestFiles1\Csv_Invalid_1.CSV");
            List<BankMetadata> txnList = new List<BankMetadata>()
                .OrderBy(txn => txn.Date)
                .OrderBy(txn => txn.Amount)
                .OrderBy(txn => txn.Merchant)
                .ToList(); ;
            CsvExtractor repo = new CsvExtractor();

            //Act
            List<BankMetadata> output1 = repo.ExtractBankMetadataFromCsvString(input1)
                .OrderBy(txn => txn.Date)
                .OrderBy(txn => txn.Amount)
                .OrderBy(txn => txn.Merchant)
                .ToList();
        }

        [TestMethod]
        public void ExtractBankMetadatasFromCsv_OnInvalidXml_ThrowCorrectExceptionMessage()
        {
            //Arrange
            string input1 = File.ReadAllText(@"..\..\..\TestFiles1\Csv_Invalid_1.CSV");
            CsvExtractor repo = new CsvExtractor();

            //Act
            try
            {
                List<BankMetadata> output1 = repo.ExtractBankMetadataFromCsvString(input1)
                .OrderBy(txn => txn.Date)
                .OrderBy(txn => txn.Amount)
                .OrderBy(txn => txn.Merchant)
                .ToList(); ;
            }

            //Assert
            catch (Exception ex)
            {
                Assert.AreEqual("Badly formatted CSV string", ex.Message);
            }
        }
    }
}
