/********************************************************************************************
 * Project Name - Kiosk Activity Log
 * Description  - Bussiness logic of  Kiosk Activity Log
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.70        16-Jul-2019   Dakshakh raj        Modified : 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.logger
{
    /// <summary>
    /// Business logic for KioskActivityLog class.
    /// </summary>
    public class KioskActivityLogBL
    {
        #region Fields
        private KioskActivityLogDTO kioskActivityLogDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        #endregion
        #region Constructor
        ///// <summary>
        ///// Default constructor of KioskActivityLogBL class
        ///// </summary>
        //public KioskActivityLogBL()
        //{
        //    log.LogMethodEntry();
        //    kioskActivityLogDTO = null;
        //    log.LogMethodExit();
        //}

        /// <summary>
        /// Creates KioskActivityLogBL object using the KioskActivityLogDTO
        /// </summary>
        /// <param name="kioskActivityLogDTO">KioskActivityLogDTO object</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public KioskActivityLogBL(ExecutionContext executionContext, KioskActivityLogDTO kioskActivityLogDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(executionContext, kioskActivityLogDTO, sqlTransaction);
            this.executionContext = executionContext;
            this.kioskActivityLogDTO = kioskActivityLogDTO;
            log.LogMethodExit();
        }
        #endregion
        /// <summary>
        /// Saves the KioskActivityLog
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (kioskActivityLogDTO == null
                || kioskActivityLogDTO.KioskActivityLogId > -1)
            {
                log.LogMethodExit("Empty DTO or already saved record. Skipping log insert action");
                return;
            }
            KioskActivityLogDataHandler kioskActivityLogDataHandler = new KioskActivityLogDataHandler(sqlTransaction);
            kioskActivityLogDTO = kioskActivityLogDataHandler.InsertKioskActivityLog(kioskActivityLogDTO, executionContext.GetUserId(), executionContext.GetSiteId());
            kioskActivityLogDTO.TrxId = kioskActivityLogDTO.KioskActivityLogId;
            kioskActivityLogDTO.AcceptChanges();
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public KioskActivityLogDTO KioskActivityLogDTO
        {
            get
            {
                return kioskActivityLogDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of monitors
    /// </summary>
    public class KioskActivityLogListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<KioskActivityLogDTO> kioskActivityLogDTOList = new List<KioskActivityLogDTO>();
        private ExecutionContext executionContext;

        public KioskActivityLogListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="kioskActivityLogDTOList"></param>
        /// <param name="executionContext"></param>
        public KioskActivityLogListBL(ExecutionContext executionContext, List<KioskActivityLogDTO> kioskActivityLogDTOList) : this(executionContext)
        {
            log.LogMethodEntry(kioskActivityLogDTOList, executionContext);
            this.kioskActivityLogDTOList = kioskActivityLogDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the monitors list
        /// </summary>
        public List<KioskActivityLogDTO> GetKioskActivityLogList(List<KeyValuePair<KioskActivityLogDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            KioskActivityLogDataHandler kioskActivityLogDataHandler = new KioskActivityLogDataHandler(sqlTransaction);
            List<KioskActivityLogDTO> kioskActivityLogDTOList = kioskActivityLogDataHandler.GetKioskActivityLogList(searchParameters);
            log.LogMethodExit(kioskActivityLogDTOList);
            return kioskActivityLogDTOList;
        }


        /// <summary>
        /// Saves the kioskActivityLog DTO List
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public List<KioskActivityLogDTO> Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (kioskActivityLogDTOList == null ||
                kioskActivityLogDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return null;
            }
            List<KioskActivityLogDTO> result = new List<KioskActivityLogDTO>();
            for (int i = 0; i < kioskActivityLogDTOList.Count; i++)
            {
                var kioskActivityLogDTO = kioskActivityLogDTOList[i];
                try
                {
                    KioskActivityLogBL kioskActivityLog = new KioskActivityLogBL(executionContext, kioskActivityLogDTO);
                    kioskActivityLog.Save(sqlTransaction);
                    result.Add(kioskActivityLog.KioskActivityLogDTO);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving kioskActivityLogDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("kioskActivityLogDTO", kioskActivityLogDTO);
                    throw;
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        public static int GetMaxKioskTrxId(ExecutionContext machineExecutionContext, SqlTransaction sqltrx)
        {
            log.LogMethodEntry();
            int maxKioskTrxId = 0;
            KioskActivityLogDataHandler kioskActivityLogDataHandler = new KioskActivityLogDataHandler(sqltrx);
            maxKioskTrxId = kioskActivityLogDataHandler.GetMaxKioskTrxId(machineExecutionContext.GetSiteId());
            log.LogMethodExit(maxKioskTrxId);
            return maxKioskTrxId;
        }
    }
}

