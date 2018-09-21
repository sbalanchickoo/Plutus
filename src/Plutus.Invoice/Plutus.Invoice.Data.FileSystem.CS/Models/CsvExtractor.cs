using Plutus.SharedLibrary.CS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;

namespace Plutus.Invoices.Data.FileSystem.CS.Models
{
    /// <summary>
    /// Helper class to extract BankTransaction from Ofx
    /// </summary>
    public class CsvExtractor
    {
        private string _delimiter;
        /// <summary>
        /// Delimiter used in CSV file
        /// </summary>
        public string Delimiter
        {
            get
            {
                if (string.IsNullOrEmpty(_delimiter))
                {
                    _delimiter = ",";
                }
                return _delimiter;
            }
            set
            {
                if (!(string.IsNullOrEmpty(_delimiter)))
                {
                    _delimiter = value;
                }
            }
        }

        /// <summary>
        /// Controller method, takes in CSV string and returns IEnumerable
        /// </summary>
        public IEnumerable<Invoice> ExtractInvoicesFromCsvString(string csvContent)
        {
            List<Invoice> txnList = new List<Invoice>();
            try
            {
                var csv = new CsvReader(new StringReader(csvContent));
                csv.Configuration.Delimiter = Delimiter;
                var anonymousTypeDefinition = new
                {
                    Date = string.Empty,
                    Description = string.Empty,
                    ClientName = string.Empty,
                    InvoiceRef = string.Empty,
                    Total = string.Empty,
                    Amount = string.Empty,
                    Net = string.Empty,
                    Vat = string.Empty,
                };
                var txnListInput = csv.GetRecords(anonymousTypeDefinition);
                foreach (var input in txnListInput)
                {
                    Invoice row = new Invoice
                    {
                        Date = Convert.ToDateTime(input.Date),
                        Amount = Convert.ToDecimal(input.Amount),
                        ClientName = input.ClientName,
                        InvoiceReference = input.InvoiceRef,
                        Description = input.Description
                    };
                    txnList.Add(row);
                }
                return txnList;
            }
            catch (Exception ex)
            {
                throw new FieldAccessException("Badly formatted CSV string", ex);
            }
        }
    }
}
