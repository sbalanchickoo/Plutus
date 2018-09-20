using Plutus.SharedLibrary.CS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;

namespace Plutus.Expenses.Data.FileSystem.CS.Models
{
    /// <summary>
    /// Helper class to extract Expenses from CSV
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
        /// Controller method, takes in CSV string and returns IEnumerable of Expense
        /// </summary>
        public IEnumerable<Expense> ExtractExpensesFromCsvString(string csvContent)
        {
            List<Expense> txnList = new List<Expense>();
            try
            {
                var csv = new CsvReader(new StringReader(csvContent));
                csv.Configuration.Delimiter = Delimiter;
                var anonymousTypeDefinition = new
                {
                    DATE = string.Empty,
                    employeename = string.Empty,
                    description = string.Empty,
                    detail = string.Empty,
                    total = string.Empty,
                    vat = string.Empty,
                    net = string.Empty
                };
                var txnListInput = csv.GetRecords(anonymousTypeDefinition);
                foreach (var input in txnListInput)
                {
                    Expense row = new Expense
                    {
                        Date = Convert.ToDateTime(input.DATE),
                        Amount = Convert.ToDecimal(input.total),
                        TransactionCategory = input.description,
                        Description = input.detail
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
