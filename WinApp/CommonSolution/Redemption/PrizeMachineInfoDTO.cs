/********************************************************************************************
 * Project Name - PrizeMachineInfo DTO
 * Description  - Data object of PrizeMachineInfo
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *1.00        11-May-2017   Lakshminarayana          Created 
 *2.70.2        12-Aug-2019   Deeksha                  Added logger methods.
 ********************************************************************************************/

namespace Semnox.Parafait.Redemption
{
    /// <summary>
    /// This is the PrizeMachineInfo data object class. This acts as data holder for the PrizeMachineInfo business object
    /// </summary>
    public class PrizeMachineInfoDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string dispenseCategory;
        private string machineRef;
        private decimal stockQuantity;
        private decimal stockValue;
        private int machineCount;
        private int ticketCount;
        private string status;

        public PrizeMachineInfoDTO()
        {
            log.LogMethodEntry();
            dispenseCategory = string.Empty;
            machineRef = string.Empty;
            stockQuantity = 0;
            stockValue = 0;
            machineCount = 0;
            ticketCount = 0;
            status = string.Empty;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the DispenseCategory field
        /// </summary>
        public string DispenseCategory
        {
            get
            {
                return dispenseCategory;
            }

            set
            {
                dispenseCategory = value;
            }
        }

        /// <summary>
        /// Get/Set method of the MachineRef field
        /// </summary>
        public string MachineRef
        {
            get
            {
                return machineRef;
            }

            set
            {
                machineRef = value;
            }
        }

        /// <summary>
        /// Get/Set method of the StockQuantity field
        /// </summary>
        public decimal StockQuantity
        {
            get
            {
                return stockQuantity;
            }

            set
            {
                stockQuantity = value;
            }
        }

        /// <summary>
        /// Get/Set method of the StockValue field
        /// </summary>
        public decimal StockValue
        {
            get
            {
                return stockValue;
            }

            set
            {
                stockValue = value;
            }
        }

        /// <summary>
        /// Get/Set method of the MachineCount field
        /// </summary>
        public int MachineCount
        {
            get
            {
                return machineCount;
            }

            set
            {
                machineCount = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TicketCount field
        /// </summary>

        public int TicketCount
        {
            get
            {
                return ticketCount;
            }

            set
            {
                ticketCount = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Status field
        /// </summary>
        public string Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value;
            }
        }
    }
}
