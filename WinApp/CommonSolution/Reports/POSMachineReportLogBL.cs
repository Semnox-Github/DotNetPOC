/********************************************************************************************
 * Project Name - Reports
 * Description  - Business Logic class of POSMachineReportLog
 *
 **************
 ** Version Log
  **************
  * Version     Date          Modified By            Remarks
 *********************************************************************************************
 *2.70         29-May-2019   Girish Kundar           Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Reports
{
    public class POSMachineReportLogBL
    {
        private POSMachineReportLogDTO posMachineReportLogDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parametrized  constructor 
        /// </summary>
        public POSMachineReportLogBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the POSMachineReportLog DTO parameter
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="POSMachineReportLogDTO">POSMachineReportLogDTO </param>
        public POSMachineReportLogBL(ExecutionContext executionContext, POSMachineReportLogDTO posMachineReportLogDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, posMachineReportLogDTO);
            this.posMachineReportLogDTO = posMachineReportLogDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the POSMachineReportLog id as the parameter
        /// Would fetch the POSMachineReportLog object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution Context</param>
        /// <param name="id"> id of POSMachineReportLog id passed as parameter</param>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public POSMachineReportLogBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            POSMachineReportLogDataHandler posMachineReportLogDataHandler = new POSMachineReportLogDataHandler(sqlTransaction);
            posMachineReportLogDTO = posMachineReportLogDataHandler.GetPOSMachineReportLogDTO(id);
            if (posMachineReportLogDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "POSMachineReportLog", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Saves the POSMachineReportLog  
        /// POSMachineReportLog   will be inserted if  Id is less than 
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            POSMachineReportLogDataHandler posMachineReportLogDataHandler = new POSMachineReportLogDataHandler(sqlTransaction);

            if (posMachineReportLogDTO.Id < 0)
            {
                posMachineReportLogDTO = posMachineReportLogDataHandler.Insert(posMachineReportLogDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                posMachineReportLogDTO.AcceptChanges();
            }
            else
            {
                if (posMachineReportLogDTO.IsChanged)
                {
                    posMachineReportLogDTO = posMachineReportLogDataHandler.Update(posMachineReportLogDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    posMachineReportLogDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Get the DTO
        /// </summary>
        public POSMachineReportLogDTO POSMachineReportLogDTO
        {
            get { return posMachineReportLogDTO; }
        }
    }

    /// <summary>
    /// Manages the list of POSMachineReportLog List
    /// </summary>
    public class POSMachineReportLogListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<POSMachineReportLogDTO> posMachineReportLogDTOList;

        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        public POSMachineReportLogListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.posMachineReportLogDTOList = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="posMachineReportLogDTOList">posMachineReportLogDTOList</param>
        public POSMachineReportLogListBL(ExecutionContext executionContext, List<POSMachineReportLogDTO> posMachineReportLogDTOList)
        {
            log.LogMethodEntry(executionContext, posMachineReportLogDTOList);
            this.executionContext = executionContext;
            this.posMachineReportLogDTOList = posMachineReportLogDTOList;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the List of POSMachineReportLogDTO
        /// </summary>
        public List<POSMachineReportLogDTO> GetAllPOSMachineReportLog(List<KeyValuePair<POSMachineReportLogDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            POSMachineReportLogDataHandler posMachineReportLogDataHandler = new POSMachineReportLogDataHandler(sqlTransaction);
            List<POSMachineReportLogDTO> posMachineReportLogDTOList = new List<POSMachineReportLogDTO>();
            posMachineReportLogDTOList = posMachineReportLogDataHandler.GetPOSMachineReportLogDTOList(searchParameters);
            log.LogMethodExit(posMachineReportLogDTOList);
            return posMachineReportLogDTOList;
        }

        /// <summary>
        /// This method should be used to Save and Update the POSMachineReportLogDTO details for Web Management Studio.
        /// </summary>
        public void Save()
        {
            log.LogMethodEntry();
            try
            {
                if (posMachineReportLogDTOList != null && posMachineReportLogDTOList.Count > 0)
                {
                    foreach (POSMachineReportLogDTO posMachineReportLogDTO in posMachineReportLogDTOList)
                    {
                        POSMachineReportLogBL posMachineReportLogBL = new POSMachineReportLogBL(executionContext, posMachineReportLogDTO);
                        posMachineReportLogBL.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw new Exception(ex.Message, ex);
            }
            log.LogMethodExit();
        }
    }
}
