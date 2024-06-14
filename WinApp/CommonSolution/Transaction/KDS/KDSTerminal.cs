/********************************************************************************************
 * Project Name - Transaction
 * Description  - Represents KDS Terminal types
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.2        9-Sep-2019   Lakshminarayana         Created 
 ********************************************************************************************/

using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Transaction.KDS
{
    /// <summary>
    /// Represents KDS terminal
    /// </summary>
    public abstract class KDSTerminal
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// terminal id
        /// </summary>
        protected readonly int terminalId;
        /// <summary>
        /// execution context
        /// </summary>
        protected readonly ExecutionContext executionContext;
        
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="terminalId"></param>
        protected KDSTerminal(ExecutionContext executionContext, int terminalId)
        {
            log.LogMethodEntry(executionContext, terminalId);
            this.executionContext = executionContext;
            this.terminalId = terminalId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the open orders matching the filter conditions
        /// </summary>
        /// <param name="posMachineId">pos Machine Id</param>
        /// <param name="tableNumber">table Number</param>
        /// <param name="loadChildRecords">load Child Records</param>
        /// <param name="sqlTransaction">sql Transaction</param>
        /// <returns></returns>
        public abstract List<KDSOrderDTO> GetOpenOrders(int posMachineId, string tableNumber, bool loadChildRecords = true, SqlTransaction sqlTransaction = null);
        
        /// <summary>
        /// Returns the delivered order matching the filter conditions 
        /// </summary>
        /// <param name="posMachineId">pos Machine Id</param>
        /// <param name="tableNumber">table Number</param>
        /// <param name="withinLastNMinutes">within Last N Minutes</param>
        /// <param name="loadChildRecords">load Child Records</param>
        /// <param name="sqlTransaction">sql Transaction</param>
        /// <returns></returns>
        public abstract List<KDSOrderDTO> GetDeliveredOrders(int posMachineId, string tableNumber, int withinLastNMinutes, bool loadChildRecords = true, SqlTransaction sqlTransaction = null);
        
        /// <summary>
        /// Bumps the order
        /// </summary>
        /// <param name="kdsOrderDTO">kds Order DTO</param>
        /// <param name="sqlTransaction">sql Transaction</param>
        public abstract void BumpOrder(KDSOrderDTO kdsOrderDTO, SqlTransaction sqlTransaction = null);
        
        /// <summary>
        /// Recalls the order from delivered to ordered status 
        /// </summary>
        /// <param name="kdsOrderDTO">kds Order DTO</param>
        /// <param name="sqlTransaction">sql Transaction</param>
        public abstract void RecallOrder(KDSOrderDTO kdsOrderDTO, SqlTransaction sqlTransaction = null);
        
        /// <summary>
        /// Bumps the specified order line
        /// </summary>
        /// <param name="kdsOrderDTO">kds Order DTO</param>
        /// <param name="lineId">line Id</param>
        /// <param name="sqlTransaction">sql Transaction</param>
        public abstract void BumpOrderLine(KDSOrderDTO kdsOrderDTO, int lineId, SqlTransaction sqlTransaction = null);
        
        /// <summary>
        ///  Recalls the specified order line
        /// </summary>
        /// <param name="kdsOrderDTO">kds Order DTO</param>
        /// <param name="lineId">line Id</param>
        /// <param name="sqlTransaction">sql Transaction</param>
        public abstract void RecallOrderLine(KDSOrderDTO kdsOrderDTO, int lineId, SqlTransaction sqlTransaction = null);
    }
}
