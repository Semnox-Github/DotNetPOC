﻿/********************************************************************************************
 * Project Name - Transaction
 * Description  - Represents a combined KDS Terminal
 *
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks
 *********************************************************************************************
 *1.00        10-09-2019      lakshminarayana rao     Created
 *2.140.0     27-Jun-2021     Fiona Lishal             Modified for Delivery Order enhancements for F&B and Urban Piper
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Transaction.KDS
{
    /// <summary>
    /// Represents a both kitchen and delivery terminal  
    /// </summary>
    public class CombinedTerminal : KDSTerminal
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution Context</param>
        /// <param name="terminalId">terminal Id</param>
        public CombinedTerminal(ExecutionContext executionContext, int terminalId)
            : base(executionContext, terminalId)
        {
            log.LogMethodEntry(executionContext, terminalId);
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
        public override List<KDSOrderDTO> GetOpenOrders(int posMachineId, string tableNumber, bool loadChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(posMachineId, tableNumber);
            KDSOrderListBL kdsOrderListBL = new KDSOrderListBL(executionContext);
            List<KeyValuePair<KDSOrderDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<KDSOrderDTO.SearchByParameters, string>>();
            if (terminalId >= 0)
            {
                searchParameters.Add(new KeyValuePair<KDSOrderDTO.SearchByParameters, string>(KDSOrderDTO.SearchByParameters.TERMINAL_ID, terminalId.ToString()));
            }
            if (posMachineId >= 0)
            {
                searchParameters.Add(new KeyValuePair<KDSOrderDTO.SearchByParameters, string>(KDSOrderDTO.SearchByParameters.POS_MACHINE_ID, posMachineId.ToString()));
            }
            if (string.IsNullOrWhiteSpace(tableNumber) == false)
            {
                searchParameters.Add(new KeyValuePair<KDSOrderDTO.SearchByParameters, string>(KDSOrderDTO.SearchByParameters.TABLE_NUMBER, tableNumber));
            }
            searchParameters.Add(new KeyValuePair<KDSOrderDTO.SearchByParameters, string>(KDSOrderDTO.SearchByParameters.DELIVERED_TIME_NULL, string.Empty));
            searchParameters.Add(new KeyValuePair<KDSOrderDTO.SearchByParameters, string>(KDSOrderDTO.SearchByParameters.KDSKOT_ENTRY_TYPE, KDSOrderLineDTO.KDSKOTEntryType.KDS.ToString()));
            List<KDSOrderDTO> result = kdsOrderListBL.GetKDSOrderDTOList(searchParameters, loadChildRecords, sqlTransaction);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the delivered order matching the filter conditions 
        /// </summary>
        /// <param name="posMachineId">pos Machine Id</param>
        /// <param name="tableNumber">table Number</param>
        /// <param name="withinLastNMinutes">within Last N Minutes</param>
        /// <param name="loadChildRecords">load Child Records</param>
        /// <param name="sqlTransaction">sql Transaction</param>
        /// <returns></returns>
        public override List<KDSOrderDTO> GetDeliveredOrders(int posMachineId, string tableNumber, int withinLastNMinutes, bool loadChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(posMachineId, tableNumber);
            DateTime serverDateTime = new LookupValuesList(executionContext).GetServerDateTime();
            KDSOrderListBL kdsOrderListBL = new KDSOrderListBL(executionContext);
            List<KeyValuePair<KDSOrderDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<KDSOrderDTO.SearchByParameters, string>>();
            if (terminalId >= 0)
            {
                searchParameters.Add(new KeyValuePair<KDSOrderDTO.SearchByParameters, string>(KDSOrderDTO.SearchByParameters.TERMINAL_ID, terminalId.ToString()));
            }
            if (posMachineId >= 0)
            {
                searchParameters.Add(new KeyValuePair<KDSOrderDTO.SearchByParameters, string>(KDSOrderDTO.SearchByParameters.POS_MACHINE_ID, posMachineId.ToString()));
            }
            if (string.IsNullOrWhiteSpace(tableNumber) == false)
            {
                searchParameters.Add(new KeyValuePair<KDSOrderDTO.SearchByParameters, string>(KDSOrderDTO.SearchByParameters.TABLE_NUMBER, tableNumber));
            }
            searchParameters.Add(new KeyValuePair<KDSOrderDTO.SearchByParameters, string>(KDSOrderDTO.SearchByParameters.DELIVERED_TIME_GREATER_THAN, serverDateTime.AddMinutes(-Math.Abs(withinLastNMinutes)).ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)));
            searchParameters.Add(new KeyValuePair<KDSOrderDTO.SearchByParameters, string>(KDSOrderDTO.SearchByParameters.DELIVERED_TIME_NOT_NULL, string.Empty));
            searchParameters.Add(new KeyValuePair<KDSOrderDTO.SearchByParameters, string>(KDSOrderDTO.SearchByParameters.KDSKOT_ENTRY_TYPE, KDSOrderLineDTO.KDSKOTEntryType.KDS.ToString()));
            List<KDSOrderDTO> result = kdsOrderListBL.GetKDSOrderDTOList(searchParameters, loadChildRecords, sqlTransaction);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Bumps the order from ordered to delivered status
        /// </summary>
        /// <param name="kdsOrderDTO">kds Order DTO</param>
        /// <param name="sqlTransaction">sql Transaction</param>
        public override void BumpOrder(KDSOrderDTO kdsOrderDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(kdsOrderDTO, sqlTransaction);
            KDSOrderBL kdsOrderBL = new KDSOrderBL(executionContext, kdsOrderDTO);
            if (kdsOrderBL.IsCancelledOrder() == false && kdsOrderBL.IsDelivered())
            {
                log.LogMethodExit(null, "the order is already delivered.");
                return;
            }

            kdsOrderBL.Deliver();
            kdsOrderBL.Save(sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Recalls the order from delivered to ordered status 
        /// </summary>
        /// <param name="kdsOrderDTO">kds Order DTO</param>
        /// <param name="sqlTransaction">sql Transaction</param>
        public override void RecallOrder(KDSOrderDTO kdsOrderDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(kdsOrderDTO, sqlTransaction);
            KDSOrderBL kdsOrderBL = new KDSOrderBL(executionContext, kdsOrderDTO);
            if (kdsOrderBL.IsDelivered() == false && 
                kdsOrderBL.IsPrepared() == false)
            {
                log.LogMethodExit(null, "Order can't be recalled as it is neither delivered nor prepared.");
                return;
            }
            kdsOrderBL.RecallToKitchen();
            kdsOrderBL.Save(sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Bumps the order line from ordered to delivered status
        /// </summary>
        /// <param name="kdsOrderDTO">kds Order DTO</param>
        /// <param name="lineId">line Id</param>
        /// <param name="sqlTransaction">sql Transaction</param>
        public override void BumpOrderLine(KDSOrderDTO kdsOrderDTO, int lineId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(kdsOrderDTO, lineId, sqlTransaction);
            KDSOrderBL kdsOrderBL = new KDSOrderBL(executionContext, kdsOrderDTO);
            if (kdsOrderBL.IsDelivered(lineId))
            {
                log.LogMethodExit(null, "the order line is already delivered.");
                return;
            }
            kdsOrderBL.DeliverOrderLine(lineId);
            kdsOrderBL.Save(sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        ///  Recalls the order line from delivered to ordered status 
        /// </summary>
        /// <param name="kdsOrderDTO">kds Order DTO</param>
        /// <param name="lineId">line Id</param>
        /// <param name="sqlTransaction">sql Transaction</param>
        public override void RecallOrderLine(KDSOrderDTO kdsOrderDTO, int lineId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(kdsOrderDTO, lineId, sqlTransaction);
            KDSOrderBL kdsOrderBL = new KDSOrderBL(executionContext, kdsOrderDTO);
            if (kdsOrderBL.IsDelivered(lineId) == false &&
                kdsOrderBL.IsPrepared(lineId) == false)
            {
                log.LogMethodExit(null, "Order line can't be recalled as it is neither delivered nor prepared.");
                return;
            }
            kdsOrderBL.RecallOrderLineToKitchen(lineId);
            kdsOrderBL.Save(sqlTransaction);
            log.LogMethodExit();
        }
    }
}