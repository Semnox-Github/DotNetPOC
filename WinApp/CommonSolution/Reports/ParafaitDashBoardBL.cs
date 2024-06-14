/********************************************************************************************
 * Project Name - Reports
 * Description  - Bussiness logic of ParafaitDashBoardBL for Reports module
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.90        03-Jun-2020   Vikas Dwivedi        Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Semnox.Parafait.Reports
{
    /// <summary>
    /// Bussiness Logic for ParafaitDashBoardBL
    /// </summary>
    public class ParafaitDashBoardBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        
        /// <summary>
        /// Default constructor of ParafaitDashBoardBL class
        /// </summary>
        private ParafaitDashBoardBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

    }

    /// <summary>
    /// Manages the List of ParafaitDashBoardBL
    /// </summary>
    public class ParafaitDashBoardListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
     
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public ParafaitDashBoardListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the ParafaitDashBoardDTO
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="todate"></param>
        /// <returns></returns>
        public List<ParafaitDashBoardDTO> GetCollections(DateTime fromDate, DateTime todate, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(fromDate, todate, sqlTransaction);
            ParafaitDashBoardDataHandler parafaitDashBoardDataHandler = new ParafaitDashBoardDataHandler(sqlTransaction);
            List<ParafaitDashBoardDTO> parafaitDashBoardDTOList = parafaitDashBoardDataHandler.GetCollections(fromDate, todate, executionContext.GetSiteId(), sqlTransaction);
            log.LogMethodExit(parafaitDashBoardDTOList);
            return parafaitDashBoardDTOList;
        }

        public DataTable GetGraphTable(DateTime fromDate, DateTime todate, bool loadGraphDate = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(fromDate, todate, sqlTransaction);
            DataTable parafaitDashBoardDTOList;
            ParafaitDashBoardDataHandler parafaitDashBoardDataHandler = new ParafaitDashBoardDataHandler(sqlTransaction);
            parafaitDashBoardDTOList = parafaitDashBoardDataHandler.GetGraphTable(fromDate, todate, sqlTransaction);
            log.LogMethodExit(parafaitDashBoardDTOList);
            return parafaitDashBoardDTOList;
        }
    }
}
