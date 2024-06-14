/********************************************************************************************
 * Project Name - DBSynchLog BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        20-Mar-2017      Lakshminarayana     Created
 *2.70.2        21-Oct-2019      Rakesh              Implemented the SynchHQSiteData() method
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.logging;
namespace Semnox.Parafait.DBSynch
{
    /// <summary>
    /// Business logic for DBSynchLog class.
    /// </summary>
    public class DBSynchLogBL
    {
        private DBSynchLogDTO dBSynchLogDTO;
        private readonly ExecutionContext executionContext;
        private static  readonly Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of DBSynchLogBL class
        /// </summary>
        public DBSynchLogBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            dBSynchLogDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates DBSynchLogBL object using the DBSynchLogDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="dBSynchLogDTO">DBSynchLogDTO object</param>
        public DBSynchLogBL(ExecutionContext executionContext, DBSynchLogDTO dBSynchLogDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, dBSynchLogDTO);
            this.dBSynchLogDTO = dBSynchLogDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates DBSynchLogBL object using input data
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="tableName"></param>
        /// <param name="entityGuid"></param>
        /// <param name="siteId"></param>
        /// <param name="syncOperation"></param>
        public DBSynchLogBL(ExecutionContext executionContext, string tableName, string entityGuid, int siteId, string syncOperation = "I")
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, tableName, entityGuid, siteId, syncOperation);
            DBSynchLogDTO dBSynchLogDTO = new DBSynchLogDTO(syncOperation, entityGuid, tableName, DateTime.Now, siteId);
            this.dBSynchLogDTO = dBSynchLogDTO;
            List<ValidationError> validationErrorList = Validate(null);
            if (validationErrorList.Count > 0)
            {
                throw new ValidationException("Validation failed", validationErrorList);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the DBSynchLog
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            DBSynchLogDataHandler dBSynchLogDataHandler = new DBSynchLogDataHandler(sqlTransaction);
            List<ValidationError> validationErrorList = Validate(sqlTransaction);
            if(validationErrorList.Count > 0)
            {
                throw new ValidationException("Validation failed", validationErrorList);
            }
            dBSynchLogDataHandler.InsertDBSynchLog(dBSynchLogDTO, executionContext.GetUserId(), executionContext.GetSiteId());
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the DbSynch log entity
        /// </summary>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            ValidationError validationError;
            if (string.IsNullOrWhiteSpace(dBSynchLogDTO.Operation))
            {
                validationError = new ValidationError("DBSynchLog", "Operation", "Operation can't be empty");
                validationErrorList.Add(validationError);
            }
            if (string.IsNullOrWhiteSpace(dBSynchLogDTO.Operation) == false
                && (dBSynchLogDTO.Operation != "I" && dBSynchLogDTO.Operation != "U" && dBSynchLogDTO.Operation != "D"))
            {
                validationError = new ValidationError("DBSynchLog", "Operation", "Invalid Operation code");
                validationErrorList.Add(validationError);
            }
            if (string.IsNullOrWhiteSpace(dBSynchLogDTO.TableName))
            {
                validationError = new ValidationError("DBSynchLog", "TableName", "TableName can't be empty");
                validationErrorList.Add(validationError);
            }
            if (string.IsNullOrWhiteSpace(dBSynchLogDTO.Guid))
            {
                validationError = new ValidationError("DBSynchLog", "Guid", "Guid can't be empty");
                validationErrorList.Add(validationError);
            }
            if (dBSynchLogDTO.TimeStamp == DateTime.MinValue)
            {
                validationError = new ValidationError("DBSynchLog", "Guid", "Time stamp can't be set to min value");
                validationErrorList.Add(validationError);
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        public void SynchHQSiteData(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            try
            {
                DBSynchLogDataHandler dBSynchLogDataHandler = new DBSynchLogDataHandler(sqlTransaction);
                dBSynchLogDataHandler.CreateMasterDataFromHQSite(executionContext.GetSiteId());
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public DBSynchLogDTO DBSynchLogDTO
        {
            get
            {
                return dBSynchLogDTO;
            }
        }

    }

    /// <summary>
    /// Manages the list of DBSynchLog
    /// </summary>
    public class DBSynchLogListBL
    {
        private static  readonly Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public DBSynchLogListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the DBSynchLog list
        /// </summary>
        public List<DBSynchLogDTO> GetDBSynchLogDTOList(List<KeyValuePair<DBSynchLogDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            DBSynchLogDataHandler dBSynchLogDataHandler = new DBSynchLogDataHandler(sqlTransaction);
            List<DBSynchLogDTO> returnValue = dBSynchLogDataHandler.GetDBSynchLogDTOList(searchParameters);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        public void CreateMasterDataDBSynchLog(int siteId)
        {
            log.LogMethodEntry();
            DBSynchLogDataHandler dBSynchLogDataHandler = new DBSynchLogDataHandler();
            dBSynchLogDataHandler.CreateMasterDataDBSynchLog(siteId);
            log.LogMethodExit();
        }
    }
}
