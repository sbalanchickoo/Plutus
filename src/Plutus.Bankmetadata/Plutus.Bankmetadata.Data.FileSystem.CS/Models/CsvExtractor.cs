﻿using Plutus.SharedLibrary.CS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;

namespace Plutus.Bankmetadata.Data.FileSystem.CS.Models
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
        public IEnumerable<BankMetadata> ExtractBankMetadataFromCsvString(string csvContent)
        {
            List<BankMetadata> txnList = new List<BankMetadata>();
            try
            {
                var csv = new CsvReader(new StringReader(csvContent));
                csv.Configuration.Delimiter = Delimiter;
                var txnListInput = csv.GetRecords<dynamic>().ToList();

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
