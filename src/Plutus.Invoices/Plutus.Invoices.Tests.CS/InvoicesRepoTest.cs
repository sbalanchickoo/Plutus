using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plutus.Invoices.Data.FileSystem.CS.Models;
using Plutus.Invoices.Tests.CS.EqualityComparers;
using Plutus.SharedLibrary.CS.Enums;
using Plutus.SharedLibrary.CS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Plutus.Invoices.Tests.CS
{
    /// <summary>
    /// Test class to test methods in InvoicesRepo class
    /// </summary>
    [TestClass]
    public class InvoicesRepoTest
    {
        private List<Invoice> _invList1 = new List<Invoice>();
        private List<Invoice> _invList2 = new List<Invoice>();
        private List<Invoice> _invList3 = new List<Invoice>();
        private List<Invoice> _invList4 = new List<Invoice>();

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
            Invoice inv3 = new Invoice
            {
                Date = Convert.ToDateTime("2012-01-01"),
                Amount = 300.00M,
                ClientName = "Client 2",
                InvoiceReference = "Invoice Ref 3",
                Description = "Description 3"
            };
            Invoice inv4 = new Invoice
            {
                Date = Convert.ToDateTime("2012-01-01"),
                Amount = 400.00M,
                ClientName = "Client 3",
                InvoiceReference = "Invoice Ref 3",
                Description = "Description 3"
            };
            _invList1.Add(inv1);
            _invList1.Add(inv2);

            _invList2.Add(inv2);
            _invList2.Add(inv3);

            _invList3.Add(inv1);
            _invList3.Add(inv2);
            _invList3.Add(inv3);

            _invList4.Add(inv1);
            _invList4.Add(inv2);
            _invList4.Add(inv3);
            _invList4.Add(inv4);
        }

        /// <summary>
        /// Happy path for the GetSourceDetails method
        /// </summary>
        [TestMethod]
        public void GetSourceDetails_OnNonBlankInput_ReturnInputSource()
        {
            //Arrange
            InvoicesRepo repo = new InvoicesRepo
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
            InvoicesRepo repo = new InvoicesRepo();

            //Act
            InputDataSource inputSource = repo.GetSourceDetails();

            //Assert
            Assert.IsNotNull(inputSource.InputDataSourceName);
            Assert.AreEqual(DataSource.FileSystem, inputSource.InputDataSourceType);
        }

        /// <summary>
        /// Failure path for the ExtractInvoicesFromCsv method - Bad input CSV, return empty list
        /// </summary>
        [TestMethod]
        public void ExtractInvoicesFromCsv_OnInvalidCsv_ReturnEmptyInvoicesList()
        {
            //Arrange
            List<Invoice> txnList = new List<Invoice>();
            string input1 = File.ReadAllText(@"..\..\..\TestFiles1\Csv_Invalid_1.CSV");

            //Act
            InvoicesRepo repo = new InvoicesRepo();
            List<Invoice> output1 = repo.ExtractInvoicesFromCsv(input1).ToList();

            //Assert
            Assert.AreEqual(0, output1.Count);
            CollectionAssert.AreEqual(txnList, output1, new InvoicesComparer());
        }

        /// <summary>
        /// Happy path for the GetInvoices method
        /// </summary>
        [TestMethod]
        public void ExtractInvoicesFromCsv_OnValidInputs_ReturnInvoicesList()
        {
            //Arrange
            
            //Act
            InvoicesRepo repo = new InvoicesRepo
            {
                FolderName = @"..\..\..\TestFiles2"
            };
            List<Invoice> output1 = repo.GetInvoices().ToList();

            //Assert
            Assert.AreEqual(_invList3.Count, output1.Count);
            CollectionAssert.AreEqual(_invList3, output1, new InvoicesComparer());
        }

        /// <summary>
        /// Happy path for the ConsolidateInvoicesFromLists method
        /// </summary>
        [TestMethod]
        public void ConsolidateInvoicesFromLists_OnExecute_ReturnConsolidatedInvoicesList()
        {
            //Arrange
            List<List<Invoice>> txnInput = new List<List<Invoice>>
            {
                _invList1,
                _invList2
            };

            //Act
            InvoicesRepo repo = new InvoicesRepo();
            List<Invoice> output1 = repo.ConsolidateInvoicesFromLists(txnInput).ToList();

            //Assert
            Assert.AreEqual(3, output1.Count);
            CollectionAssert.AreEqual(_invList3, output1, new InvoicesComparer());
        }

        /// <summary>
        /// Test for the GetInvoices - Folder change functionality
        /// </summary>
        [TestMethod]
        public void GetInvoices_OnFolderChange_ReturnInvoicesList()
        {
            //Arrange
            File.Delete(@"..\..\..\TestFiles3\inv_Valid_3.CSV");

            //Act
            InvoicesRepo repo = new InvoicesRepo
            {
                FolderName = @"..\..\..\TestFiles3"
            };
            List<Invoice> output1 = repo.GetInvoices().ToList();

            File.Copy(@"..\..\..\TestFiles1\inv_Valid_3.CSV", @"..\..\..\TestFiles3\inv_Valid_3.CSV");
            System.Threading.Thread.Sleep(5000);
            output1 = repo.GetInvoices().ToList();

            //Assert
            Assert.AreEqual(_invList4.Count, output1.Count);
            CollectionAssert.AreEqual(_invList4, output1, new InvoicesComparer());
        }
    }
}
