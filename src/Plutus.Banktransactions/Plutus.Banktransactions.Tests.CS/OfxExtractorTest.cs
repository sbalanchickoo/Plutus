using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plutus.Banktransactions.Data.FileSystem.CS.Models;
using Plutus.Banktransactions.Tests.CS.EqualityComparers;
using Plutus.SharedLibrary.CS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Plutus.Banktransaction.Tests.CS
{
    /// <summary>
    /// Test class to test methods in OfxExtractor class
    /// </summary>
    [TestClass]
    public class OfxExtractorTest
    {
        private List<BankTransaction> _bankTransactionList1 = new List<BankTransaction>();

        /// <summary>
        /// Test initialization step, List of fake data
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
            _bankTransactionList1.Add(bankTransaction1);
        }

        /// <summary>
        /// Happy path for the ExtractXmlFromOfx method
        /// </summary>
        [TestMethod]
        public void ExtractXmlFromOfx_OnValidOfx_ReturnValidXml()
        {
            //Arrange
            string input1 = File.ReadAllText(@"..\..\..\TestFiles1\Ofx_Valid_1.OFX");
            string output1 = File.ReadAllText(@"..\..\..\TestFiles1\Xml_Valid_1.XML");

            //Act
            OfxExtractor repo = new OfxExtractor();
            string output = repo.ExtractXmlFromOfx(input1);

            //Assert
            Assert.AreEqual(output1, output);
        }

        /// <summary>
        /// Exception test for the ExtractXmlFromOfx method
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FieldAccessException))]
        public void ExtractXmlFromOfx_OnInvalidOfx_ThrowException()
        {
            //Arrange
            string input1 = File.ReadAllText(@"..\..\..\TestFiles1\Ofx_Invalid_1.OFX");
            OfxExtractor repo = new OfxExtractor();

            //Act
            string output = repo.ExtractXmlFromOfx(input1);
        }

        /// <summary>
        /// Exception message test for the ExtractXmlFromOfx method
        /// </summary>
        [TestMethod]
        public void ExtractXmlFromOfx_OnInvalidOfx_ThrowCorrectExceptionMessage()
        {
            //Arrange
            string input1 = File.ReadAllText(@"..\..\..\TestFiles1\Ofx_Invalid_1.OFX");
            OfxExtractor repo = new OfxExtractor();

            //Act
            try
            {
                string output = repo.ExtractXmlFromOfx(input1);
            }

            //Assert
            catch (Exception ex)
            {
                Assert.AreEqual("Badly formatted OFX content", ex.Message);
            }
        }

        /// <summary>
        /// Happy path for the ExtractBankTransactionsFromXml method
        /// </summary>
        [TestMethod]
        public void ExtractBankTransactionsFromXml_OnValidXml_ReturnTxnList()
        {
            //Arrange
            string input1 = File.ReadAllText(@"..\..\..\TestFiles1\Xml_Valid_1.XML");
            
            //Act
            OfxExtractor repo = new OfxExtractor();
            List<BankTransaction> output1 = repo.ExtractBankTransactionsFromXml(input1).ToList();

            //Assert
            CollectionAssert.AreEqual(_bankTransactionList1, output1, new BankTransactionComparer());
        }

        /// <summary>
        /// Exception test for the ExtractBankTransactionsFromXml method
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FieldAccessException))]
        public void ExtractBankTransactionsFromXml_OnInvalidXml_ThrowException()
        {
            //Arrange
            string input1 = File.ReadAllText(@"..\..\..\TestFiles1\Xml_Invalid_1.XML");
            List<BankTransaction> txnList = new List<BankTransaction>();
            OfxExtractor repo = new OfxExtractor();

            //Act
            List<BankTransaction> output1 = repo.ExtractBankTransactionsFromXml(input1).ToList();
        }

        /// <summary>
        /// Exception message test for the ExtractBankTransactionsFromXml method
        /// </summary>
        [TestMethod]
        public void ExtractBankTransactionsFromXml_OnInvalidXml_ThrowCorrectExceptionMessage()
        {
            //Arrange
            string input1 = File.ReadAllText(@"..\..\..\TestFiles1\Xml_Invalid_1.XML");
            OfxExtractor repo = new OfxExtractor();

            //Act
            try
            {
                List<BankTransaction> output1 = repo.ExtractBankTransactionsFromXml(input1).ToList();
            }

            //Assert
            catch (Exception ex)
            {
                Assert.AreEqual("Badly formatted XML string", ex.Message);
            }
        }
    }
}
