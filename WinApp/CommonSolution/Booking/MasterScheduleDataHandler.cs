/* Project Name - Semnox.Parafait.Booking.MasterScheduleDataHandler 
* Description  - Data handler object of the MasterSchedule
* 
**************
**Version Log
**************
*Version     Date           Modified By             Remarks          
********************************************************************************************* 
*2.50        26-Nov-2018    Guru S A             Created for Booking enhancement changes 
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Booking
{
    public class MasterScheduleDataHandler
    {
        DataAccessHandler dataAccessHandler;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        private static readonly Dictionary<MasterScheduleDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<MasterScheduleDTO.SearchByParameters, string>
            {
                {MasterScheduleDTO.SearchByParameters.MASTER_SCHEDULE_ID, "AttractionMasterScheduleId"},  
                {MasterScheduleDTO.SearchByParameters.ACTIVE_FLAG, "ActiveFlag"}, 
                {MasterScheduleDTO.SearchByParameters.MASTER_ENTITY_ID, "MasterEntityId"},
                {MasterScheduleDTO.SearchByParameters.SITE_ID, "site_id"} 
            };

        private static readonly string atMSSelectQuery = @"SELECT *
                                                            FROM AttractionMasterSchedule ";

        /// <summary>
        /// Default constructor of  MasterScheduleDataHandler class
        /// </summary>
        public MasterScheduleDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating masterSchedule Record.
        /// </summary>
        /// <param name="masterScheduleDTO">MasterScheduleDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(MasterScheduleDTO masterScheduleDTO, string userId, int siteId)
        { 
            log.LogMethodEntry(masterScheduleDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@AttractionMasterScheduleId", masterScheduleDTO.MasterScheduleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterScheduleName", masterScheduleDTO.MasterScheduleName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ActiveFlag", (masterScheduleDTO.ActiveFlag == true ? "Y" : "N")));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", masterScheduleDTO.MasterEntityId, true)); 
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", userId)); 
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the MasterSchedule record to the database
        /// </summary>
        /// <param name="masterScheduleDTO">masterScheduleDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertMasterSchedule(MasterScheduleDTO masterScheduleDTO, string userId, int siteId)
        {
            log.LogMethodEntry(masterScheduleDTO, userId, siteId);
            int idOfRowInserted;
            string query = @"INSERT INTO dbo.AttractionMasterSchedule
                                           (MasterScheduleName
                                           ,ActiveFlag
                                           ,Guid
                                           ,site_id 
                                           ,MasterEntityId
                                           ,CreatedBy
                                           ,CreationDate
                                           ,LastUpdatedBy
                                           ,LastUpdateDate)
                                     VALUES
                                           (@MasterScheduleName 
                                           ,@ActiveFlag 
                                           ,NEWID()
                                           ,@site_id 
                                           ,@MasterEntityId 
                                           ,@CreatedBy 
                                           ,getdate()
                                           ,@LastUpdatedBy 
                                           ,getdate())SELECT CAST(scope_identity() AS int)";
            try
            {
                idOfRowInserted = dataAccessHandler.executeInsertQuery(query, GetSQLParameters(masterScheduleDTO, userId, siteId).ToArray(), sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(idOfRowInserted);
            return idOfRowInserted;
        }

        /// <summary>
        /// Updates the MasterSchedule record
        /// </summary>
        /// <param name="masterScheduleDTO">MasterScheduleDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public int UpdateMasterSchedule(MasterScheduleDTO masterScheduleDTO, string userId, int siteId)
        {
            log.LogMethodEntry(masterScheduleDTO, userId, siteId);
            int rowsUpdated;
            string query = @"UPDATE dbo.AttractionMasterSchedule
                               SET MasterScheduleName = @MasterScheduleName 
                                  ,ActiveFlag = @ActiveFlag 
                                  --,site_id = @site_id 
                                  ,MasterEntityId = @MasterEntityId 
                                  ,LastUpdatedBy = @LastUpdatedBy 
                                  ,LastUpdateDate =getdate()
                             WHERE AttractionMasterScheduleId = @AttractionMasterScheduleId";
            try
            {
                rowsUpdated = dataAccessHandler.executeUpdateQuery(query, GetSQLParameters(masterScheduleDTO, userId, siteId).ToArray(), sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(rowsUpdated);
            return rowsUpdated;
        }



        /// <summary>
        /// Converts the Data row object to GetMasterScheduleDTO calss type
        /// </summary>
        /// <param name="attrSchRow">MasterSchedule DataRow</param>
        /// <returns>Returns MasterScheduleDTO</returns>
        private MasterScheduleDTO GetMasterScheduleDTO(DataRow attrSchRow)
        {
            log.LogMethodEntry(attrSchRow);
            MasterScheduleDTO masterScheduleDTO = new MasterScheduleDTO(
                                                                    Convert.ToInt32(attrSchRow["AttractionMasterScheduleId"]),
                                                                    attrSchRow["MasterScheduleName"].ToString(),
                                                                    string.IsNullOrEmpty(attrSchRow["ActiveFlag"].ToString()) ? false: (attrSchRow["ActiveFlag"].ToString()=="Y"? true: false),
                                                                    attrSchRow["Guid"].ToString(),
                                                                    string.IsNullOrEmpty(attrSchRow["site_id"].ToString()) ? -1 : Convert.ToInt32(attrSchRow["site_id"]),
                                                                    string.IsNullOrEmpty(attrSchRow["SynchStatus"].ToString()) ? false : Convert.ToBoolean(attrSchRow["SynchStatus"]),
                                                                    string.IsNullOrEmpty(attrSchRow["MasterEntityId"].ToString()) ? -1 : Convert.ToInt32(attrSchRow["MasterEntityId"]),
                                                                    string.IsNullOrEmpty(attrSchRow["CreatedBy"].ToString()) ? "" : Convert.ToString(attrSchRow["CreatedBy"]),
                                                                    attrSchRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(attrSchRow["CreationDate"]),
                                                                    string.IsNullOrEmpty(attrSchRow["LastUpdateBy"].ToString())? "" : Convert.ToString(attrSchRow["LastUpdateBy"]),
                                                                    attrSchRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(attrSchRow["LastUpdateDate"])
                                                                    );
            log.LogMethodExit(masterScheduleDTO);
            return masterScheduleDTO;
        }

        /// <summary>
        /// Gets the MasterSchedule data of passed masterSchedule Id
        /// </summary>
        /// <param name="masterScheduleId">integer type parameter</param>
        /// <returns>Returns MasterScheduleDTO</returns>
        public MasterScheduleDTO GetMasterScheduleDTO(int masterScheduleId)
        {
            log.LogMethodEntry(masterScheduleId);
            string selectMasterScheduleQuery = atMSSelectQuery + "  WHERE AttractionMasterScheduleID = @attractionMasterScheduleId";
            SqlParameter[] selectMasterScheduleParameters = new SqlParameter[1];
            selectMasterScheduleParameters[0] = new SqlParameter("@attractionMasterScheduleId", masterScheduleId);
            DataTable masterSchedule = dataAccessHandler.executeSelectQuery(selectMasterScheduleQuery, selectMasterScheduleParameters, sqlTransaction);
            MasterScheduleDTO masterScheduleDataObject = new MasterScheduleDTO();
            if (masterSchedule.Rows.Count > 0)
            {
                DataRow MasterScheduleRow = masterSchedule.Rows[0];
                masterScheduleDataObject = GetMasterScheduleDTO(MasterScheduleRow);
            }
            log.LogMethodExit(masterScheduleDataObject);
            return masterScheduleDataObject;
        }

        /// <summary>
        /// Gets the MasterScheduleDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of MasterScheduleDTO matching the search criteria</returns>
        public List<MasterScheduleDTO> GetMasterScheduleDTOList(List<KeyValuePair<MasterScheduleDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<MasterScheduleDTO> list = new List<MasterScheduleDTO>(); 
            int count = 0;
            string selectQuery = atMSSelectQuery;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<MasterScheduleDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == MasterScheduleDTO.SearchByParameters.MASTER_SCHEDULE_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value);
                        }
                        else if (searchParameter.Key == MasterScheduleDTO.SearchByParameters.SITE_ID ||
                                 searchParameter.Key == MasterScheduleDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                        }
                        else if (searchParameter.Key == MasterScheduleDTO.SearchByParameters.ACTIVE_FLAG )
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'N') = " + (searchParameter.Value == "1"? "Y":"N"));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + " N'%" + searchParameter.Value + "%'");
                        }
                        count++;
                    }
                    else
                    {
                        log.LogMethodExit(null, "throwing exception");
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, null, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            { 
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    MasterScheduleDTO masterScheduleDTO = GetMasterScheduleDTO(dataRow);
                    list.Add(masterScheduleDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        } 
    }
}
