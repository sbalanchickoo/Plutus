using Plutus.SharedLibrary.CS.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace Plutus.Banktransactions.Data.FileSystem.CS.Models
{
    /// <summary>
    /// Helper class to extract BankTransaction from Ofx
    /// </summary>
    public class OfxExtractor
    {
        /// <summary>
        /// Controller method
        /// </summary>
        public IEnumerable<BankTransaction> ExtractBankTransactionsFromString(string content)
        {
            try
            {
                string xml = ExtractXmlFromOfx(content);
                IEnumerable<BankTransaction> txnList = new List<BankTransaction>();
                txnList = ExtractBankTransactionsFromXml(xml);
                return txnList;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Extract xml from ofx text
        /// </summary>
        public string ExtractXmlFromOfx(string content)
        {
            string xmlContent = "<BANKTRANLIST>";
            xmlContent += "<DTSTART>19000101000000</DTSTART>";
            xmlContent += "<DTEND>19000101000000</DTEND>";
            xmlContent += "</BANKTRANLIST>";
            int xmlStartLocation = 0;
            int xmlEndLocation = 0;
            string tag = "BANKTRANLIST";
            xmlStartLocation = content.ToUpper().IndexOf($"<{tag}>");
            xmlEndLocation = content.ToUpper().IndexOf($"</{tag}>");

            try
            {
                if (xmlStartLocation != -1 && xmlEndLocation != -1)
                {
                    xmlContent = content.Substring(xmlStartLocation, xmlEndLocation - xmlStartLocation + tag.Length + 3);
                    return xmlContent;
                }
                else
                {
                    throw new FieldAccessException();
                }
            }
            catch(Exception ex)
            {
                if (ex is FieldAccessException)
                {
                    throw new FieldAccessException("Badly formatted OFX content",ex);
                }
                else
                {
                    throw;
                }
            }
            
        }

        /// <summary>
        /// Get Bank transactions from xml string
        /// </summary>
        public IEnumerable<BankTransaction> ExtractBankTransactionsFromXml(string xml)
        {
            List<BankTransaction> txnList = new List<BankTransaction>();
            try
            {
                XDocument xdoc = null;
                xdoc = XDocument.Parse(xml);
                IEnumerable<XElement> elementList = from el in xdoc.Elements().Elements()
                                                    select el;
                CultureInfo provider = CultureInfo.InvariantCulture;
                foreach (var item in elementList)
                {
                    if (item.Name == "STMTTRN")
                    {
                        BankTransaction newTxn = new BankTransaction
                        {
                            FITID = item.Element("FITID").Value,
                            Amount = Convert.ToDecimal(item.Element("TRNAMT").Value),
                            Date = DateTime.ParseExact(item.Element("DTPOSTED").Value
                                    , "yyyyMMddhhmmss", provider),
                            Merchant = item.Element("NAME").Value
                        };
                        txnList.Add(newTxn);
                    }
                }
                return txnList;
            }
            catch (Exception ex)
            {
                throw new FieldAccessException("Badly formatted XML string", ex);
            }
        }
    }
}
