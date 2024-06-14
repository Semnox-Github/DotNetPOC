/********************************************************************************************
 * Project Name - RichContent Data Handler
 * Description  - Data Handler
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By       Remarks          
 *********************************************************************************************
 *2.70.2        25-Jul-2019      Dakshakh Raj      Modified : added GetSQLParameters(), 
 *                                                          SQL injection Issue Fix and
 *                                                          Added IsActive to insert/update method
 *2.70.2        06-Dec-2019      Jinto Thomas      Removed siteid from update query
 *2.90.0        04-Jun-2020      Girish Kundar    Modified : Phase -2 REST API related changes 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Core.GenericUtilities
{
    public class RichContentDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction = null;
        DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM RichContent as rc ";

        /// <summary>
        /// Dictionary for searching Parameters for the RichContent object.
        /// </summary>

        private static readonly Dictionary<RichContentDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<RichContentDTO.SearchByParameters, string>
            {
                {RichContentDTO.SearchByParameters.ID, "rc.ID"},
                {RichContentDTO.SearchByParameters.CONTENT_NAME, "rc.CONTENTNAME"},
                {RichContentDTO.SearchByParameters.FILE_NAME, "rc.FILENAME"},
                {RichContentDTO.SearchByParameters.SITE_ID, "rc.SITE_ID"},
                {RichContentDTO.SearchByParameters.MASTER_ENTITY_ID, "rc.MasterEntityId"},
                {RichContentDTO.SearchByParameters.ACTIVE_FLAG, "rc.IsActive"}
             };

        
        /// <summary>
        /// Default constructor of RichContentDataHandler class
        /// </summary>
        public RichContentDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        ///Builds the SQL Parameter list used for inserting and updating richContentDTO Reecord.
        /// </summary>
        /// <param name="richContentDTO">richContentDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns> Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(RichContentDTO richContentDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(richContentDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@id", richContentDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@contentName", string.IsNullOrEmpty(richContentDTO.ContentName) ? DBNull.Value : (object)(richContentDTO.ContentName)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@fileName", string.IsNullOrEmpty(richContentDTO.FileName) ? DBNull.Value : (object)(richContentDTO.FileName)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@data", richContentDTO.Data));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", richContentDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", (richContentDTO.IsActive == true ? "Y" : "N")));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the RichContent record to the database
        /// </summary>
        /// <param name="richContentDTO">richContentDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        ///<returns>Returns richContentDTO</returns>
        public RichContentDTO InsertRichContent(RichContentDTO richContentDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(richContentDTO, loginId, siteId);
            string insertRichContentQuery = @"insert into RichContent
                                                            ( 
                                                            ContentName,
                                                            FileName,
                                                            data,
                                                            CreatedBy,
                                                            CreationDate,
                                                            LastUpdatedBy,
                                                            LastUpdatedDate,
                                                            site_id,
                                                            Guid,
                                                            MasterEntityId,IsActive
                                                         )
                                                       values
                                                         ( 
                                                            @contentName,
                                                            @fileName,
                                                            @data,
                                                            @createdBy,
                                                            GETDATE(),
                                                            @lastUpdatedBy,
                                                            GETDATE(),
                                                            @siteId,
                                                            NewID(),
                                                            @masterEntityId,@isActive

                                                          )SELECT * FROM RichContent WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertRichContentQuery, GetSQLParameters(richContentDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshRichContentDTO(richContentDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting richContentDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(richContentDTO);
            return richContentDTO;
        }

        /// <summary>
        /// Updates the RichContent record to the database
        /// </summary>
        /// <param name="RichContentDTO">RichContentDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>richContentDTO</returns>
        public RichContentDTO UpdateRichContent(RichContentDTO richContentDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(richContentDTO, loginId, siteId);
            string updateRichContentDTOQuery = @"update RichContent
                                                         set
															ContentName = @contentName,
                                                            FileName = @fileName,
                                                            data = @data,
                                                            LastUpdatedBy = @lastUpdatedBy,
                                                            LastUpdatedDate = GETDATE(),
                                                            MasterEntityId = @masterEntityId,
                                                            IsActive = @isActive
                                                          where  ID = @id
                                                          SELECT* FROM RichContent WHERE  ID = @id ";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateRichContentDTOQuery, GetSQLParameters(richContentDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshRichContentDTO(richContentDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating richContentDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(richContentDTO);
            return richContentDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="richContentDTO">richContentDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshRichContentDTO(RichContentDTO richContentDTO, DataTable dt)
        {
            log.LogMethodEntry(richContentDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                richContentDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                richContentDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                richContentDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                richContentDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                richContentDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                richContentDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                richContentDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///  Converts the Data row object to RichContentDTO class type
        /// </summary>
        /// <param name="RichContentDTODataRow"></param>
        /// <returns>Returns RichContentDTO</returns>
        private RichContentDTO GetRichContent(DataRow RichContentDTODataRow)
        {
            log.LogMethodEntry(RichContentDTODataRow);
            RichContentDTO richContentDTO = new RichContentDTO(
                                 RichContentDTODataRow["Id"] == DBNull.Value ? -1 : Convert.ToInt32(RichContentDTODataRow["Id"]),
                                 RichContentDTODataRow["ContentName"] == DBNull.Value ? string.Empty : RichContentDTODataRow["ContentName"].ToString(),
                                 RichContentDTODataRow["FileName"] == DBNull.Value ? string.Empty : RichContentDTODataRow["FileName"].ToString(),
                                 RichContentDTODataRow["Data"] == DBNull.Value ? null : (byte[])RichContentDTODataRow["Data"],
                                 RichContentDTODataRow["IsActive"] == DBNull.Value ? true : (RichContentDTODataRow["IsActive"].ToString() == "Y" ? true : false),
                                 RichContentDTODataRow["CreatedBy"] == DBNull.Value ? string.Empty : RichContentDTODataRow["CreatedBy"].ToString(),
                                 RichContentDTODataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(RichContentDTODataRow["CreationDate"]),
                                 RichContentDTODataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : RichContentDTODataRow["LastUpdatedBy"].ToString(),
                                 RichContentDTODataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(RichContentDTODataRow["LastUpdatedDate"]),
                                 RichContentDTODataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(RichContentDTODataRow["Site_id"]),
                                 RichContentDTODataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(RichContentDTODataRow["SynchStatus"]),
                                 RichContentDTODataRow["Guid"].ToString(),
                                 RichContentDTODataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(RichContentDTODataRow["MasterEntityId"])
                                 );
            log.LogMethodExit(richContentDTO);
            return richContentDTO;
        }


        /// <summary>
        /// Gets the RichContentDTO data of passed id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns RichContentDTO</returns>
        public RichContentDTO GetRichContent(int id)
        {
            log.LogMethodEntry(id);
            RichContentDTO richContentDTO = null;
            string selectRichContentDTOQuery = SELECT_QUERY + @" WHERE rc.id = @id";
            SqlParameter[] selectRichContentDTOParameters = new SqlParameter[1];
            selectRichContentDTOParameters[0] = new SqlParameter("@id", id);
            DataTable selectedRichContent = dataAccessHandler.executeSelectQuery(selectRichContentDTOQuery, selectRichContentDTOParameters);
            if (selectedRichContent.Rows.Count > 0)
            {
                DataRow RichContentRow = selectedRichContent.Rows[0];
                richContentDTO = GetRichContent(RichContentRow);
            }

            log.LogMethodExit(richContentDTO);
            return richContentDTO;
        }

        /// <summary>
        /// Gets the RichContentDTO data of passed id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns RichContentDTO</returns>
        public void Delete(int id)
        {
            log.LogMethodEntry(id);
            string selectRichContentDTOQuery = "DELETE FROM RichContent WHERE id = @id";
            SqlParameter[] selectRichContentDTOParameters = new SqlParameter[1];
            selectRichContentDTOParameters[0] = new SqlParameter("@id", id);
            dataAccessHandler.executeUpdateQuery(selectRichContentDTOQuery, selectRichContentDTOParameters,sqlTransaction);
            log.LogMethodExit();
        }


        /// <summary>
        /// Gets the RichContentDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns>Returns the list of RichContentDTO matching the search criteria</returns>
        public List<RichContentDTO> GetRichContentDTOList(List<KeyValuePair<RichContentDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectRichContentDTOQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<RichContentDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        string joinOperartor = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key == RichContentDTO.SearchByParameters.ID
                            || searchParameter.Key == RichContentDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == RichContentDTO.SearchByParameters.CONTENT_NAME
                                 || searchParameter.Key == RichContentDTO.SearchByParameters.FILE_NAME)
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == RichContentDTO.SearchByParameters.ACTIVE_FLAG)
                        {
                            query.Append(joinOperartor + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
                        }
                        else if (searchParameter.Key == RichContentDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joinOperartor + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + " =-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        count++;
                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                }

                if (searchParameters.Count > 0)
                    selectRichContentDTOQuery = selectRichContentDTOQuery + query;
                selectRichContentDTOQuery = selectRichContentDTOQuery + " Order by id";
            }

            DataTable RichContentData = dataAccessHandler.executeSelectQuery(selectRichContentDTOQuery, parameters.ToArray());
            List<RichContentDTO> richContentDTOList = new List<RichContentDTO>();
            if (RichContentData.Rows.Count > 0)
            {
                foreach (DataRow dataRow in RichContentData.Rows)
                {
                    RichContentDTO richContentDTOObject = GetRichContent(dataRow);
                    richContentDTOList.Add(richContentDTOObject);
                }

            }
            log.LogMethodExit(richContentDTOList);
            return richContentDTOList;
        }

    }
}
