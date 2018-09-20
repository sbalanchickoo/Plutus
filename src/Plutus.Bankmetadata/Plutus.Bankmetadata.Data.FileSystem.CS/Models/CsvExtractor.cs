using Plutus.SharedLibrary.CS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;

namespace Plutus.Bankmetadata.Data.FileSystem.CS.Models
{
    /// <summary>
    /// Helper class to extract BankMetadata from CSV
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
        /// Controller method, takes in CSV string and returns IEnumerable of BankMetadata
        /// </summary>
        public IEnumerable<BankMetadata> ExtractBankMetadataFromCsvString(string csvContent)
        {
            List<BankMetadata> txnList = new List<BankMetadata>();
            try
            {
                var csv = new CsvReader(new StringReader(csvContent));
                csv.Configuration.Delimiter = Delimiter;
                var anonymousTypeDefinition = new
                {
                    date = string.Empty,
                    bankdescription = string.Empty,
                    accountnumber = string.Empty,
                    employeename = string.Empty,
                    description = string.Empty,
                    usercomments = string.Empty,
                    amount = string.Empty,
                    net = string.Empty,
                    vat = string.Empty,
                };
                var txnListInput = csv.GetRecords(anonymousTypeDefinition);
                foreach (var input in txnListInput)
                {
                    BankMetadata row = new BankMetadata
                    {
                        Amount = Convert.ToDecimal(input.amount),
                        Merchant = input.bankdescription,
                        Date = Convert.ToDateTime(input.date),
                        UserComments = input.usercomments,
                        TransactionCategory = input.description
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
