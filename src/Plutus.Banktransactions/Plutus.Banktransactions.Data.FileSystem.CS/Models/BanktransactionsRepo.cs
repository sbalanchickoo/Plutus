using Plutus.SharedLibrary.CS.Enums;
using Plutus.SharedLibrary.CS.Interfaces;
using Plutus.SharedLibrary.CS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using NLog;

namespace Plutus.Banktransactions.Data.FileSystem.CS.Models
{
    /// <summary>
    /// This class is a local file-system-based repository of Bank transactions ...
    /// ... from the Business Bank account.
    /// These are obtained from OFX files
    /// </summary>
    public class BanktransactionsRepo : IBankTransaction
    {
        private bool _isDirty;
        private FileSystemWatcher _watcher;
        private IEnumerable<BankTransaction> _bankTransactionList;
        private IEnumerable<DirectoryFile> _fileList;
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            _isDirty = true;
            ClassLogger.Info("Folder changed, repository marked as dirty.");
        }
        private IEnumerable<DirectoryFile> ScanFolder()
        {
            List<DirectoryFile> fileList = new List<DirectoryFile>();

            string[] fileListString = Directory.GetFiles(FolderName);
            foreach (var file in fileListString)
            {
                DirectoryFile newFile = new DirectoryFile
                {
                    FileName = file,
                    FolderName = FolderName
                };

                string extension = Path.GetExtension(file);
                switch (extension.ToUpper())
                {
                    case ".OFX":
                        newFile.DirectoryFileType = FileType.BankTransaction;
                        break;
                    case ".CSV":
                        switch (file.Substring(0, 3).ToUpper())
                        {
                            case "INV":
                                newFile.DirectoryFileType = FileType.Invoice;
                                break;
                            case "EXP":
                                newFile.DirectoryFileType = FileType.Expense;
                                break;
                            case "BAN":
                                newFile.DirectoryFileType = FileType.BankMetadata;
                                break;
                            default:
                                newFile.DirectoryFileType = FileType.Invalid;
                                break;
                        }
                        break;
                    default:
                        newFile.DirectoryFileType = FileType.Invalid;
                        break;
                }
                if (!(newFile.DirectoryFileType == FileType.Invalid))
                {
                    fileList.Add(newFile);
                }
            }
            return fileList;
        }
        private void InitializeWatcher()
        {
            if (Directory.Exists(FolderName))
            {
                _watcher = new FileSystemWatcher
                {
                    Path = FolderName,
                    NotifyFilter = NotifyFilters.Size,
                    EnableRaisingEvents = true
                };
                _watcher.Changed += new FileSystemEventHandler(OnChanged);
            }
        }

        private string _folderName;
        /// <summary>
        /// Folder name that will hold Ofx files
        /// </summary>
        public string FolderName
        {
            get
            {
                if (_folderName == null)
                {
                    string folderName = "";
                    if (File.Exists(Directory.GetCurrentDirectory() + "appsettings.json"))
                    {
                        var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json");
                        var configuration = builder.Build();
                        folderName = configuration["FileSystemRepo:FolderName"];
                    }
                    if (string.IsNullOrEmpty(folderName))
                    {
                        folderName = Directory.GetCurrentDirectory() + "\\InputData";
                    }
                    _folderName = folderName;
                    InitializeWatcher();
                }
                return _folderName;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _folderName = value;
                    InitializeWatcher();
                }
            }
        }

        private Logger _classLogger;
        /// <summary>
        /// Logging routine
        /// </summary>
        public Logger ClassLogger
        {
            get
            {
                if (_classLogger == null)
                {
                    _classLogger = LogManager.GetCurrentClassLogger();
                }
                return _classLogger;
            }
            set
            {
                _classLogger = value;
            }
        }

        /// <summary>
        /// Primary constructor, setting Folder to watch from config, ...
        /// ... scanning directory, and transactions in them, adding them to repository, ...
        /// ... and initializing watcher method based on directory size
        /// </summary>
        public BanktransactionsRepo()
        {
            _bankTransactionList = new List<BankTransaction>();
            _isDirty = true;
        }

        #region Helper methods
        /// <summary>
        /// Extract text from csv file
        /// </summary>
        private string ExtractOfxFileContent(string path)
        {
            string content = "";
            try
            {
                content = File.ReadAllText(path);
                ClassLogger.Info($"File: [{path}] read; Number of characters: [{content.Length}]");
            }
            catch (Exception ex)
            {
                ClassLogger.Error(ex, "Unhandled exception while trying to read File: [{ path}], returning blank string");
            }
            return content;
        }

        /// <summary>
        /// Get Bank transactions from xml string
        /// </summary>
        public IEnumerable<BankTransaction> ExtractBankTransactionsFromOfx(string content)
        {
            IEnumerable<BankTransaction> txnList = new List<BankTransaction>();
            try
            {
                OfxExtractor ofxExtractor = new OfxExtractor();
                txnList = ofxExtractor.ExtractBankTransactionsFromString(content);
                ClassLogger.Info("File Ofx converted read");
                ClassLogger.Info("Transactions read from file");
            }
            catch (Exception ex)
            {
                if (ex is FieldAccessException)
                {
                    ClassLogger.Error(ex, ex.Message);
                }
                else
                {
                    ClassLogger.Error(ex, "Unhandled exception while trying to parse OFX");
                }
            }
            return txnList;
        }

        /// <summary>
        /// For each BankTransaction FileType in FileList list, send path to GetFileBankTransactions ...
        /// ... and for each transaction, add to repository list if not already present
        /// </summary>
        public IEnumerable<BankTransaction> ConsolidateTransactionsFromLists(IEnumerable<IEnumerable<BankTransaction>> transactionLists)
        {
            List<BankTransaction> txnList = new List<BankTransaction>();
            foreach (var transactionList in transactionLists)
            {
                foreach (var transaction in transactionList)
                {
                    if (!(txnList.Contains(transaction)))
                    {
                        txnList.Add(transaction);
                    }
                }
            }
            ClassLogger.Info("Transactions consolidated");
            return txnList;
        }
        #endregion

        #region Return Methods
        /// <summary>
        /// Get input details name from config settings
        /// </summary>
        public InputDataSource GetSourceDetails()
        {
            InputDataSource inputDataSource = new InputDataSource
            {
                InputDataSourceName = FolderName,
                InputDataSourceType = DataSource.FileSystem
            };
            ClassLogger.Info("Source details returned");
            ClassLogger.Info($"Folder Name: [{FolderName}]");
            return inputDataSource;
        }

        /// <summary>
        /// Returns distinct list of all Bank transactions contained within files in a folder, ...
        /// return from local list if still valid or scan afresh if repository is marked dirty
        /// </summary>
        public IEnumerable<BankTransaction> GetBankTransactions()
        {
            if (_isDirty)
            {
                ClassLogger.Info("Repository dirty, repopulating repo");
                _fileList = ScanFolder();
                ClassLogger.Info($"Files found: [{_fileList.Where(f => f.DirectoryFileType == FileType.BankTransaction).ToList().Count()}]");
                List<List<BankTransaction>> transactionlists = new List<List<BankTransaction>>();
                foreach (var file in _fileList)
                {
                    ClassLogger.Info($"Start reading File: [{file.FileName}]");
                    if (file.DirectoryFileType == FileType.BankTransaction)
                    {
                        List<BankTransaction> transactionList = new List<BankTransaction>();
                        string fileContent = ExtractOfxFileContent(file.FileName);
                        transactionList = ExtractBankTransactionsFromOfx(fileContent).ToList();
                        ClassLogger.Info($"Transactions found: [{transactionList.Count}]");
                        transactionlists.Add(transactionList);
                    }
                    ClassLogger.Info($"Finished reading File: [{file.FileName}]");
                }
                List<BankTransaction> consolidatedBankTransactionList = new List<BankTransaction>();
                consolidatedBankTransactionList = ConsolidateTransactionsFromLists(transactionlists).ToList();
                _bankTransactionList = consolidatedBankTransactionList;
                ClassLogger.Info("Repository repopulation complete");
                _isDirty = false;
            }
            else
            {
                ClassLogger.Info("Repository not dirty.");
            }
            ClassLogger.Info("RepositoryTransactions request complete.");
            return _bankTransactionList;
        }
        #endregion
    }
}
