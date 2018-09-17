using Plutus.SharedLibrary.CS.Enums;

namespace Plutus.SharedLibrary.CS.Models
{
    /// <summary>
    /// To denote a file in the file system plus some added details
    /// </summary>
    public class DirectoryFile
    {
        /// <summary>
        /// Full Folder Path that the file is located in
        /// </summary>
        public string FolderName { get; set; }

        /// <summary>
        /// Full path to file
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Whether or not the file is processed (in terms of its transactions
        /// </summary>
        public bool Processed { get; set; }

        /// <summary>
        /// File type from Enum for this file
        /// </summary>
        public FileType DirectoryFileType { get; set; }
    }
}
