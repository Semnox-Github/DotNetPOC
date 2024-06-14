/********************************************************************************************
 * Project Name - Reports
 * Description  - Bussiness logic of WirelessDashBoardBL for Reports module
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.80        15-Jun-2020   Vikas Dwivedi        Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Game;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Parafait.Reports
{
    /// <summary>
    /// Business Logic of WirelessDashBoardBL
    /// </summary>
    public class WirelessDashBoardBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor of WirelessDashBoardBL
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        private WirelessDashBoardBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the List of WirelessDashBoardBL
    /// </summary>
    public class WirelessDashBoardListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor of WirelessDashBoardListBL with ExecutionContext
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public WirelessDashBoardListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public List<WirelessDashBoardDTO> GetHubDetailsForWirelessDashBoard(DateTime fromDate, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(fromDate, sqlTransaction);
            WirelessDashBoardDataHandler wirelessDashBoardDataHandler = new WirelessDashBoardDataHandler(sqlTransaction);
            List<WirelessDashBoardDTO> wirelessDashBoardDTOList = wirelessDashBoardDataHandler.GetHubDetailsForWirelessDashBoard(fromDate, sqlTransaction);
            log.LogMethodExit(wirelessDashBoardDTOList);
            return wirelessDashBoardDTOList;
        }

        public List<WirelessDashBoardDTO> GetMachineDetailsForWirelessDashBoard(int masterId, DateTime fromDate, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(masterId, fromDate, sqlTransaction);
            WirelessDashBoardDataHandler wirelessDashBoardDataHandler = new WirelessDashBoardDataHandler(sqlTransaction);
            List<WirelessDashBoardDTO> wirelessDashBoardDTOList = wirelessDashBoardDataHandler.GetMachineDetailsForWirelessDashBoard(masterId, fromDate, sqlTransaction);
            log.LogMethodExit(wirelessDashBoardDTOList);
            return wirelessDashBoardDTOList;
        }

    }
}
