/********************************************************************************************
 * Project Name - Auto Patch Log Data Handler
 * Description  - Data handler of the Auto patch log data handler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        02-Mar-2016   Raghuveera          Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Deployment
{
    /// <summary>
    /// Auto Patch Log Data Handler - Handles insert of auto patch log data objects
    /// </summary>
    public class AutoPatchLogDataHandler
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DataAccessHandler dataAccessHandler;
        /// <summary>
        /// Default constructor of AutoPatchLogDataHandler class
        /// </summary>
        public AutoPatchLogDataHandler()
        {
            log.Debug("Starts-AutoPatchLogDataHandler() default constructor.");
            dataAccessHandler = new DataAccessHandler();
            log.Debug("Ends-AutoPatchLogDataHandler() default constructor.");
        }

        /// <summary>
        /// Inserts the auto patch log record to the database
        /// </summary>
        /// <param name="autoPatchLog">AutoPatchLogDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertAutoPatchLog(AutoPatchLogDTO autoPatchLog, string userId, int siteId)
        {
            log.Debug("Starts-InsertAutoPatchLog(autoPatchLog, userId, siteId) Method.");
            string insertAutoPatchLogQuery = @"insert into Patch_Automation_Log 
                                                        (
                                                        TimeStamp,
                                                        ObjectName,
                                                        Description,
                                                        Type,
                                                        CreatedBy,
                                                        CreationDate,                                                       
                                                        Guid,
                                                        site_id,
                                                        SystemName,
                                                        SynchStatus
                                                        ) 
                                                values 
                                                        (
                                                        @timeStamp,
                                                        @objectName,
                                                        @description,
                                                        @type,
                                                        @createdBy,
                                                        GetDate(),
                                                        NewId(),
                                                        @siteId,
                                                        @systemName,
                                                        @synchStatus
                                                        )SELECT CAST(scope_identity() AS int)";
            List<SqlParameter> updateAutoPatchLogParameters = new List<SqlParameter>();
            if (autoPatchLog.TimeStamp.Equals(DateTime.MinValue))
            {
                updateAutoPatchLogParameters.Add(new SqlParameter("@timeStamp", DateTime.Now));
            }
            else
            {
                updateAutoPatchLogParameters.Add(new SqlParameter("@timeStamp", autoPatchLog.TimeStamp));
            }
            if (string.IsNullOrEmpty(autoPatchLog.ObjectName))
            {
                updateAutoPatchLogParameters.Add(new SqlParameter("@objectName", DBNull.Value));
            }
            else
            {
                updateAutoPatchLogParameters.Add(new SqlParameter("@objectName", autoPatchLog.ObjectName));
            }
            if (string.IsNullOrEmpty(autoPatchLog.Description))
            {
                updateAutoPatchLogParameters.Add(new SqlParameter("@description", DBNull.Value));
            }
            else
            {
                updateAutoPatchLogParameters.Add(new SqlParameter("@description", autoPatchLog.Description));
            }
            if (string.IsNullOrEmpty(autoPatchLog.Type))
            {
                updateAutoPatchLogParameters.Add(new SqlParameter("@type", DBNull.Value));
            }
            else
            {
                updateAutoPatchLogParameters.Add(new SqlParameter("@type", autoPatchLog.Type));
            }
            updateAutoPatchLogParameters.Add(new SqlParameter("@createdBy", userId));
            if (autoPatchLog.SiteId != -1)
                siteId = autoPatchLog.SiteId;
            if (siteId == -1)
            {
                updateAutoPatchLogParameters.Add(new SqlParameter("@siteid", DBNull.Value));
            }
            else
            {
                updateAutoPatchLogParameters.Add(new SqlParameter("@siteid", autoPatchLog.SiteId));
            }
            if (string.IsNullOrEmpty(autoPatchLog.SystemName))
            {
                updateAutoPatchLogParameters.Add(new SqlParameter("@systemName", DBNull.Value));
            }
            else
            {
                updateAutoPatchLogParameters.Add(new SqlParameter("@systemName", autoPatchLog.SystemName));
            }
            updateAutoPatchLogParameters.Add(new SqlParameter("@synchStatus", autoPatchLog.SynchStatus));
            int idOfRowInserted = dataAccessHandler.executeInsertQuery(insertAutoPatchLogQuery, updateAutoPatchLogParameters.ToArray());
            log.Debug("Ends-InsertAutoPatchLog(autoPatchLog, userId, siteId) Method.");
            return idOfRowInserted;
        }

    }
}
