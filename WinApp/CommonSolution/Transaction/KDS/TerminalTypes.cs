/********************************************************************************************
 * Project Name - Transaction
 * Description  - Factory class to create KDS Terminal type
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.2        18-Sep-2019   Lakshminarayana         Modified - Moved to a new file 
 ********************************************************************************************/

namespace Semnox.Parafait.Transaction.KDS
{
    /// <summary>
    /// Defines the KDS Terminal types
    /// </summary>
    public enum TerminalTypes
    {
        /// <summary>
        /// Represents a kitchen terminal
        /// </summary>
        Kitchen, 
        /// <summary>
        /// Represents a open terminal
        /// </summary>
        Open, 
        /// <summary>
        /// Represents a delivery terminal 
        /// </summary>
        Delivery, 
        /// <summary>
        /// Represents a combined kitchen and delivery terminal
        /// </summary>
        Both
    };
}
