//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Plutus.Bankmetadata.Data.FileSystem.CS.Models;
//using Plutus.Bankmetadata.Tests.CS.EqualityComparers;
//using Plutus.SharedLibrary.CS.Enums;
//using Plutus.SharedLibrary.CS.Models;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;

//namespace Plutus.Bankmetadata.Tests.CS
//{
//    [TestClass]
//    public class BankmetadataRepoTest
//    {
//        [TestMethod]
//        public void GetSourceDetails_OnNonBlankInput_ReturnInputSource()
//        {
//            //Arrange
//            BankmetadataRepo repo = new BankmetadataRepo
//            {
//                FolderName = @"C:\Temp"
//            };

//            //Act
//            InputDataSource inputSource = repo.GetSourceDetails();

//            //Assert
//            Assert.AreEqual(@"C:\Temp", inputSource.InputDataSourceName);
//            Assert.AreEqual(DataSource.FileSystem, inputSource.InputDataSourceType);
//        }

//        [TestMethod]
//        public void GetSourceDetails_OnBlankInput_ReturnInputSource()
//        {
//            //Arrange
//            BankmetadataRepo repo = new BankmetadataRepo();
            
//            //Act
//            InputDataSource inputSource = repo.GetSourceDetails();

//            //Assert
//            Assert.IsNotNull(inputSource.InputDataSourceName);
//            Assert.AreEqual(DataSource.FileSystem, inputSource.InputDataSourceType);
//        }

//        [TestMethod]
//        public void ExtractBankMetadatasFromOfx_OnInvalidOfx_ReturnEmptyTxnList()
//        {
//            //Arrange
//            List<BankMetadata> txnList = new List<BankMetadata>();
//            string input1 = File.ReadAllText(@"..\..\..\TestFiles1\Ofx_Invalid_1.OFX");

//            //Act
//            BankmetadataRepo repo = new BankmetadataRepo();
//            List<BankMetadata> output1 = repo.ExtractBankMetadatasFromOfx(input1).OrderBy(txn => txn.FITID).ToList();

//            //Assert
//            Assert.AreEqual(0, output1.Count);
//            CollectionAssert.AreEqual(txnList, output1, new BankMetadataComparer());
//        }

//        [TestMethod]
//        public void ExtractBankMetadatasFromOfx_OnInvalidXml_ReturnEmptyTxnList()
//        {
//            //Arrange
//            List<BankMetadata> txnList = new List<BankMetadata>();
//            string input1 = File.ReadAllText(@"..\..\..\TestFiles1\Ofx_Invalid_2.OFX");

//            //Act
//            BankmetadataRepo repo = new BankmetadataRepo();
//            List<BankMetadata> output1 = repo.ExtractBankMetadatasFromOfx(input1).OrderBy(txn => txn.FITID).ToList();

//            //Assert
//            Assert.AreEqual(0, output1.Count);
//            CollectionAssert.AreEqual(txnList, output1, new BankMetadataComparer());
//        }

//        [TestMethod]
//        public void ExtractBankMetadatasFromOfx_OnValidInputs_ReturnTxnList()
//        {
//            //Arrange
//            List<BankMetadata> txnList = new List<BankMetadata>
//            {
//                new BankMetadata
//                {
//                    FITID = "91234567800",
//                    Merchant = "R/P to John Doe",
//                    Amount = -500.50M,
//                    Date = Convert.ToDateTime("2011-12-30")
//                },
//                new BankMetadata
//                {
//                    FITID = "91234567801",
//                    Merchant = "R/P to Jane Doe",
//                    Amount = -1000.50M,
//                    Date = Convert.ToDateTime("2011-12-31")
//                }
//            }.OrderBy(txn => txn.FITID).ToList();

//            //Act
//            BankmetadataRepo repo = new BankmetadataRepo
//            {
//                FolderName = @"..\..\..\TestFiles2"
//            };
//            List<BankMetadata> output1 = repo.GetBankMetadatas().OrderBy(txn => txn.FITID).ToList();

//            //Assert
//            Assert.AreEqual(2, output1.Count);
//            CollectionAssert.AreEqual(txnList, output1, new BankMetadataComparer());
//        }

//        [TestMethod]
//        public void ConsolidateMetadatasFromLists_OnExecute_ReturnConsolidatedTxnList()
//        {
//            //Arrange
//            List<BankMetadata> txnListConsolidated = new List<BankMetadata>
//            {
//                new BankMetadata
//                {
//                    FITID = "91234567800",
//                    Merchant = "R/P to John Doe",
//                    Amount = -500.50M,
//                    Date = Convert.ToDateTime("2011-12-30")
//                },
//                new BankMetadata
//                {
//                    FITID = "91234567801",
//                    Merchant = "R/P to Jane Doe",
//                    Amount = -1000.50M,
//                    Date = Convert.ToDateTime("2011-12-31")
//                },
//                new BankMetadata
//                {
//                    FITID = "91234567802",
//                    Merchant = "R/P to Jack Doe",
//                    Amount = -1500.50M,
//                    Date = Convert.ToDateTime("2012-12-31")
//                }
//            }.OrderBy(txn => txn.FITID).ToList();

//            List<BankMetadata> txnList1 = new List<BankMetadata>
//            {
//                new BankMetadata
//                {
//                    FITID = "91234567800",
//                    Merchant = "R/P to John Doe",
//                    Amount = -500.50M,
//                    Date = Convert.ToDateTime("2011-12-30")
//                },
//                new BankMetadata
//                {
//                    FITID = "91234567801",
//                    Merchant = "R/P to Jane Doe",
//                    Amount = -1000.50M,
//                    Date = Convert.ToDateTime("2011-12-31")
//                }
//            }.OrderBy(txn => txn.FITID).ToList();

//            List<BankMetadata> txnList2 = new List<BankMetadata>
//            {
//                new BankMetadata
//                {
//                    FITID = "91234567801",
//                    Merchant = "R/P to Jane Doe",
//                    Amount = -1000.50M,
//                    Date = Convert.ToDateTime("2011-12-31")
//                },
//                new BankMetadata
//                {
//                    FITID = "91234567802",
//                    Merchant = "R/P to Jack Doe",
//                    Amount = -1500.50M,
//                    Date = Convert.ToDateTime("2012-12-31")
//                }
//            }.OrderBy(txn => txn.FITID).ToList();

//            List<List<BankMetadata>> txnInput = new List<List<BankMetadata>>();
//            txnInput.Add(txnList1);
//            txnInput.Add(txnList2);

//            //Act
//            BankmetadataRepo repo = new BankmetadataRepo();
//            List<BankMetadata> output1 = repo.ConsolidateMetadatasFromLists(txnInput).OrderBy(txn => txn.FITID).ToList();

//            //Assert
//            Assert.AreEqual(3, output1.Count);
//            CollectionAssert.AreEqual(txnListConsolidated, output1, new BankMetadataComparer());
//        }

//        [TestMethod]
//        public void GetBankMetadatas_OnFolderChange_ReturnTxnList()
//        {
//            //Arrange
//            List<BankMetadata> txnList = new List<BankMetadata>
//            {
//                new BankMetadata
//                {
//                    FITID = "91234567800",
//                    Merchant = "R/P to John Doe",
//                    Amount = -500.50M,
//                    Date = Convert.ToDateTime("2011-12-30")
//                },
//                new BankMetadata
//                {
//                    FITID = "91234567801",
//                    Merchant = "R/P to Jane Doe",
//                    Amount = -1000.50M,
//                    Date = Convert.ToDateTime("2011-12-31")
//                },
//                new BankMetadata
//                {
//                    FITID = "91234567802",
//                    Merchant = "R/P to Jack Doe",
//                    Amount = -1500.50M,
//                    Date = Convert.ToDateTime("2012-12-31")
//                }
//            }.OrderBy(txn => txn.FITID).ToList();
//            File.Delete(@"..\..\..\TestFiles3\Ofx_Valid_3.OFX");

//            //Act
//            BankmetadataRepo repo = new BankmetadataRepo
//            {
//                FolderName = @"..\..\..\TestFiles3"
//            };
//            List<BankMetadata> output1 = repo.GetBankMetadatas().OrderBy(txn => txn.FITID).ToList();
//            File.Copy(@"..\..\..\TestFiles1\Ofx_Valid_3.OFX", @"..\..\..\TestFiles3\Ofx_Valid_3.OFX");
//            System.Threading.Thread.Sleep(5000);
//            output1 = repo.GetBankMetadatas().OrderBy(txn => txn.FITID).ToList();

//            //Assert
//            Assert.AreEqual(3, output1.Count);
//            CollectionAssert.AreEqual(txnList, output1, new BankMetadataComparer());
//        }
//    }
//}
