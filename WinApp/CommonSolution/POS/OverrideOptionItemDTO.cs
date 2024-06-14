/********************************************************************************************
 * Project Name - OverrideOptionItem DTO
 * Description  - DTO class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By      Remarks          
 *********************************************************************************************
 *2.110.0     11-Dec-2020      Dakshakh Raj     Created for Peru Invoice Enhancement 
 ********************************************************************************************/

using System.ComponentModel;

namespace Semnox.Parafait.POS
{
    /// <summary>
    /// POSPrinterOverrideOptionItemNames
    /// </summary>
    public enum POSPrinterOverrideOptionItemNames
    {
        /// <summary>
        /// None
        /// </summary>
        [Description("None")]
        NONE,
        /// <summary>
        /// Receipt Template
        /// </summary>
        [Description("Receipt Template")]
        RECEIPT_TEMPLATE,
        /// <summary>
        /// Invoice Sequence
        /// </summary>
        [Description("Invoice Sequence")]
        INVOICE_SEQUENCE,
        /// <summary>
        /// Invoice Sequence
        /// </summary>
        [Description("Reversal Sequence")]
        REVERSAL_SEQUENCE
    }

    /// <summary>
    /// POSPrinterOverrideOptionItemCode
    /// </summary>
    public enum POSPrinterOverrideOptionItemCode
    {
        /// <summary>
        /// None item Code
        /// </summary>
        [Description("None item Code")]
        NONE,
        /// <summary>
        /// Receipt Template Item Code
        /// </summary>
        [Description("Receipt Template Item Code")]
        RECEIPT,
        /// <summary>
        /// Invoice Sequence Item Code
        /// </summary>
        [Description("Invoice Sequence Item Code")]
        SEQUENCE,
        /// <summary>
        /// Reversal Sequence Item Code
        /// </summary>
        [Description("Reversal Sequence Item Code")]
        REVERSALSEQUENCE
    }
    public class OverrideOptionItemDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///  SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {   /// <summary>
            /// Search by Option Item Name
            /// </summary>
            OPTION_ITEM_NAME,

            /// <summary>
            /// Search by Option Item Code
            /// </summary>
            OPTION_ITEM_CODE,
        }
        private POSPrinterOverrideOptionItemNames optionItemName;
        private POSPrinterOverrideOptionItemCode optionItemCode;
        private string sourceTableName;
        private string sourceColumnName;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <returns></returns>
        public OverrideOptionItemDTO()
        {
            log.LogMethodEntry();
            optionItemName = POSPrinterOverrideOptionItemNames.NONE;
            optionItemCode = POSPrinterOverrideOptionItemCode.NONE;
            sourceTableName = string.Empty;
            sourceColumnName = string.Empty;
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with all parameter
        /// </summary>
        public OverrideOptionItemDTO(POSPrinterOverrideOptionItemNames optionItemName, POSPrinterOverrideOptionItemCode optionItemCode, string sourceTableName, string sourceColumnName)
            : this()
        {
            log.LogMethodEntry(optionItemName, optionItemCode, sourceTableName, sourceColumnName);
            this.optionItemName = optionItemName;
            this.optionItemCode = optionItemCode;
            this.sourceTableName = sourceTableName;
            this.sourceColumnName = sourceColumnName;
        }

        /// <summary>
        /// Get/Set method of the Option Item Name field
        /// </summary>
        public POSPrinterOverrideOptionItemNames OptionItemName { get { return optionItemName; } set { optionItemName = value; } }

        /// <summary>
        /// Get/Set method of the option Item Code field
        /// </summary>
        public POSPrinterOverrideOptionItemCode OptionItemCode { get { return optionItemCode; } set { optionItemCode = value; } }

        /// <summary>
        /// Get/Set method of the Source Table Name field
        /// </summary>
        public string SourceTableName { get { return sourceTableName; } set { sourceTableName = value; } }

        /// <summary>
        /// Get/Set method of the Source Column Name field
        /// </summary>
        public string SourceColumnName { get { return sourceColumnName; } set { sourceColumnName = value; } }
    }
}


