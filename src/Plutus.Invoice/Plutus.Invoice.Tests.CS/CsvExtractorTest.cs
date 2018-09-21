using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plutus.Invoices.Data.FileSystem.CS.Models;
using Plutus.Invoices.Tests.CS.EqualityComparers;
using Plutus.SharedLibrary.CS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Plutus.Invoices.Tests.CS
{
    /// <summary>
    /// Test class to test methods in CsvExtractor class
    /// </summary>
    [TestClass]
    public class CsvExtractorTest
    {
        private List<Invoice> _invList1 = new List<Invoice>();

        /// <summary>
        /// Test initialization step, List of fake data
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
            _invList1.Add(inv1);
            _invList1.Add(inv2);
        }

        /// <summary>
        /// Happy path for the ExtractInvoicesFromCsv method
        /// </summary>
        [TestMethod]
        public void ExtractInvoicesFromCsv_OnValidCsv_ReturnInvoicesList()
        {
            //Arrange
            string input1 = File.ReadAllText(@"..\..\..\TestFiles1\Csv_Valid_1.CSV");

            //Act
            CsvExtractor repo = new CsvExtractor();
            List<Invoice> output1 = repo.ExtractInvoicesFromCsvString(input1).ToList();

            //Assert
            CollectionAssert.AreEqual(_invList1, output1, new InvoicesComparer());
        }

        /// <summary>
        /// Exception test for the ExtractInvoicesFromCsv method
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FieldAccessException))]
        public void ExtractInvoicesFromCsv_OnInvalidCsv_ThrowException()
        {
            //Arrange
            string input1 = File.ReadAllText(@"..\..\..\TestFiles1\Csv_Invalid_1.CSV");
            CsvExtractor repo = new CsvExtractor();

            //Act, Assert
            List<Invoice> output1 = repo.ExtractInvoicesFromCsvString(input1).ToList();
        }

        /// <summary>
        /// Exception message test for the ExtractInvoicesFromCsv method
        /// </summary>
        [TestMethod]
        public void ExtractInvoicesFromCsv_OnInvalidXml_ThrowCorrectExceptionMessage()
        {
            //Arrange
            string input1 = File.ReadAllText(@"..\..\..\TestFiles1\Csv_Invalid_1.CSV");
            CsvExtractor repo = new CsvExtractor();

            //Act
            try
            {
                List<Invoice> output1 = repo.ExtractInvoicesFromCsvString(input1).ToList();
            }

            //Assert
            catch (Exception ex)
            {
                Assert.AreEqual("Badly formatted CSV string", ex.Message);
            }
        }
    }
}
