using Plutus.SharedLibrary.CS.Enums;
using Plutus.SharedLibrary.CS.Interfaces;
using Plutus.SharedLibrary.CS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using NLog;

namespace Plutus.Invoices.Data.FileSystem.CS.Models
{
    /// <summary>
    /// This class is a local file-system-based repository of Invoices ...
    /// ... from the Business account.
    /// These are obtained from CSV files
    /// </summary>
    public class InvoiceRepo : IInvoice
    {
        private bool _isDirty;
        private FileSystemWatcher _watcher;
        private IEnumerable<Invoice> _invoicesList;
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
                ClassLogger.Info(file.Substring(FolderName.Length+1, 3).ToUpper());
                string extension = Path.GetExtension(file);
                switch (extension.ToUpper())
                {
                    case ".OFX":
                        newFile.DirectoryFileType = FileType.Invoice;
                        break;
                    case ".CSV":
                        switch (file.Substring(FolderName.Length + 1, 3).ToUpper())
                        {
                            case "INV":
                                newFile.DirectoryFileType = FileType.Invoice;
                                break;
                            case "EXP":
                                newFile.DirectoryFileType = FileType.Expense;
                                break;
                            case "BAN":
                                newFile.DirectoryFileType = FileType.Invoice;
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
        /// Folder name that will hold CSV files
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
        /// ... scanning directory, and Invoices in them, adding them to repository, ...
        /// ... and initializing watcher method based on directory size
        /// </summary>
        public InvoiceRepo()
        {
            _invoicesList = new List<Invoice>();
            _isDirty = true;
        }

        #region Helper methods
        /// <summary>
        /// Extract text from csv file
        /// </summary>
        private string ExtractCsvFileContent(string path)
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
        /// Get Invoices from xml string
        /// </summary>
        public IEnumerable<Invoice> ExtractInvoiceFromCsv(string content)
        {
            IEnumerable<Invoice> txnList = new List<Invoice>();
            try
            {
                CsvExtractor csvExtractor = new CsvExtractor();
                txnList = csvExtractor.ExtractInvoiceFromCsvString(content);
                ClassLogger.Info("File Csv parsed");
                ClassLogger.Info("Invoices read from files");
            }
            catch (Exception ex)
            {
                if (ex is FieldAccessException)
                {
                    ClassLogger.Error(ex, ex.Message);
                }
                else
                {
                    ClassLogger.Error(ex, "Unhandled exception while trying to parse CSV");
                }
            }
            return txnList;
        }

        /// <summary>
        /// For each Invoice FileType in FileList list, send path to GetFileInvoices ...
        /// ... and for each Invoices, add to repository list if not already present
        /// </summary>
        public IEnumerable<Invoice> ConsolidateInvoicesFromLists(IEnumerable<IEnumerable<Invoice>> InvoicesLists)
        {
            List<Invoice> txnList = new List<Invoice>();
            foreach (var InvoicesList in InvoicesLists)
            {
                foreach (var Invoices in InvoicesList)
                {
                    if (!(txnList.Contains(Invoices)))
                    {
                        txnList.Add(Invoices);
                    }
                }
            }
            ClassLogger.Info("Invoices consolidated");
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
        /// Returns distinct list of all Invoices contained within files in a folder, ...
        /// return from local list if still valid or scan afresh if repository is marked dirty
        /// </summary>
        public IEnumerable<Invoice> GetInvoices()
        {
            if (_isDirty)
            {
                ClassLogger.Info("Repository dirty, repopulating repo");
                _fileList = ScanFolder();
                ClassLogger.Info($"Files found: [{_fileList.Where(f => f.DirectoryFileType == FileType.Invoice).ToList().Count()}]");
                List<List<Invoice>> Invoiceslists = new List<List<Invoice>>();
                foreach (var file in _fileList)
                {
                    ClassLogger.Info($"Start reading File: [{file.FileName}]");
                    if (file.DirectoryFileType == FileType.Invoice)
                    {
                        List<Invoice> InvoicesList = new List<Invoice>();
                        string fileContent = ExtractCsvFileContent(file.FileName);
                        InvoicesList = ExtractInvoiceFromCsv(fileContent).ToList();
                        ClassLogger.Info($"Invoices found: [{InvoicesList.Count}]");
                        Invoiceslists.Add(InvoicesList);
                    }
                    ClassLogger.Info($"Finished reading File: [{file.FileName}]");
                }
                List<Invoice> consolidatedInvoiceList = new List<Invoice>();
                consolidatedInvoiceList = ConsolidateInvoicesFromLists(Invoiceslists).ToList();
                _invoicesList = consolidatedInvoiceList;
                ClassLogger.Info("Repository repopulation complete");
                _isDirty = false;
            }
            else
            {
                ClassLogger.Info("Repository not dirty.");
            }
            ClassLogger.Info("RepositoryInvoices request complete.");
            return _invoicesList;
        }
        #endregion
    }
}
