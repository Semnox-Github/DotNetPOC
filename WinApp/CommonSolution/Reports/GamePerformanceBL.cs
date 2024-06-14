/********************************************************************************************
 * Project Name - Reports
 * Description  - Bussiness logic of GamePerformanceBL for Reports module
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.80        24-Jun-2020   Vikas Dwivedi        Created
 ********************************************************************************************/

using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Semnox.Parafait.Reports
{
    public class GamePerformanceBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor with ExecutionContext
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public GamePerformanceBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
    }

    public class GamePerformanceListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<System.Data.DataTable> tabsDataTableList = new List<System.Data.DataTable>();
        private System.Data.DataTable tabsDataList;
        private System.Data.DataTable gameProfileNameList;

        /// <summary>
        /// Parameterized Constructor with ExecutionContext
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public GamePerformanceListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// It will return the ProfileTabs
        /// </summary>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public System.Data.DataTable GetProfileTabsName(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            GamePerformanceDataHandler gamePerformanceDataHandler = new GamePerformanceDataHandler(sqlTransaction);
            gameProfileNameList = gamePerformanceDataHandler.GetProfileTabs(sqlTransaction);
            log.LogMethodExit(gameProfileNameList);
            return gameProfileNameList;
        }

        /// <summary>
        /// It will return the OverView Data
        /// </summary>
        /// <param name="periodId">periodId</param>
        /// <param name="loadOverViewData"loadOverViewData></param>
        /// <param name="profileName">profileName</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public System.Data.DataTable GetProfileTabsData(int periodId, bool loadOverViewData, string profileName, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(periodId, loadOverViewData, profileName, sqlTransaction);
            GamePerformanceDataHandler gamePerformanceDataHandler = new GamePerformanceDataHandler(sqlTransaction);
            if (loadOverViewData)
            {
                tabsDataList = gamePerformanceDataHandler.LoadOverView(periodId, sqlTransaction);
                log.LogMethodExit(tabsDataList);
                return tabsDataList;
            }
            else
            {
                tabsDataList = gamePerformanceDataHandler.LoadProfileData(profileName, periodId);
                log.LogMethodExit(tabsDataList);
                return tabsDataList;
            }
        }

        public int GetDaysCount(int periodId,SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(periodId);
            GamePerformanceDataHandler gamePerformanceDataHandler = new GamePerformanceDataHandler(sqlTransaction);
            int days=  gamePerformanceDataHandler.GetDaysCount(periodId);
            log.LogMethodExit(days);
            return days;

        }

    }
}
