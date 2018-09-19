namespace Plutus.SharedLibrary.CS.Enums
{
    /// <summary>
    /// Enum describing the various types of files
    /// </summary>
    public enum FileType
    {
        /// <summary>
        /// Bank transaction received from Bank
        /// </summary>
        BankTransaction = 0,

        /// <summary>
        /// Bank metadata (such as Category etc, typically received from accountant)
        /// </summary>
        BankMetadata,

        /// <summary>
        /// Expense row
        /// </summary>
        Expense,

        /// <summary>
        /// Invoice generated
        /// </summary>
        Invoice,

        /// <summary>
        /// Log file
        /// </summary>
        Log,

        /// <summary>
        /// Invalid / unrecognized file type
        /// </summary>
        Invalid
    }
}
