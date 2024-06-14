/********************************************************************************************
 * Project Name - Transaction
 * Description  - Represents a delivery terminal
 *
 **************
 **Version Log
 **************
 *Version     Date             Modified By            Remarks
 *********************************************************************************************
 *1.00        10-09-2019      lakshminarayana rao     Modified - As part of the KDS enhancement, changed from public fields to properties
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Transaction.KDS
{
    /// <summary>
    /// KDS display unit class
    /// </summary>
    public class KDSDisplayUnitClass
    {
        /// <summary>
        /// Get/Set method of DisplayContent field
        /// </summary>
        public Printer.ReceiptClass DisplayContent { get; set; }
        
        /// <summary>
        /// Get/Set method of TrxId field
        /// </summary>
        public int TrxId{ get; set; }

        /// <summary>
        /// Get/Set method of DisplayBatchId field
        /// </summary>
        public int DisplayBatchId{ get; set; }

        /// <summary>
        /// Get/Set method of TerminalId field
        /// </summary>
        public int TerminalId{ get; set; }

        /// <summary>
        /// Get/Set method of OrderedTime field
        /// </summary>
        public DateTime OrderedTime{ get; set; }

        /// <summary>
        /// Get/Set method of PreparedTime field
        /// </summary>
        public DateTime PreparedTime{ get; set; }

        /// <summary>
        /// Get/Set method of DeliveredTime field
        /// </summary>
        public DateTime DeliveredTime{ get; set; }

        /// <summary>
        /// Get/Set method of TableNumber field
        /// </summary>
        public string TableNumber{ get; set; }

        /// <summary>
        /// Get/Set method of DeliveryMode field
        /// </summary>
        public string DeliveryMode{ get; set; }

        /// <summary>
        /// Get/Set method of TrxNumber field
        /// </summary>
        public string TrxNumber{ get; set; }

        /// <summary>
        /// Get/Set method of kdsOrderDTO field
        /// </summary>
        public KDSOrderDTO KDSOrderDTO{ get; set; }

    }
}
