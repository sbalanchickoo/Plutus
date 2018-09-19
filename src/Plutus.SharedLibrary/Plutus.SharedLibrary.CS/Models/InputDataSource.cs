using Plutus.SharedLibrary.CS.Enums;

namespace Plutus.SharedLibrary.CS.Models
{
    /// <summary>
    /// Class representing source of input data
    /// </summary>
    public class InputDataSource
    {
        /// <summary>
        /// Name e.g. location, Azure Data Lake details etc.
        /// </summary>
        public string InputDataSourceName { get; set; }

        /// <summary>
        /// Type of source e.g. File System, Azure Data Lake etc.
        /// </summary>
        public DataSource InputDataSourceType { get; set; }
    }
}
