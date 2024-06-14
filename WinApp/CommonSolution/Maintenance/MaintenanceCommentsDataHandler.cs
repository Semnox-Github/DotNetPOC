/********************************************************************************************
 * Project Name - Maintenance
 * Description  - Data handler for Maintenance Comments
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.150.3    21-Mar-2022    Abhishek      Created 
 ********************************************************************************************/
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using Semnox.Core.Utilities;
using System.Linq;

namespace Semnox.Parafait.Maintenance
{
    /// <summary>
    /// Comments Data Handler - Handles insert, update and select of Maintenance Comments objects
    /// </summary>
    public class MaintenanceCommentsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM Comments AS c ";

        /// <summary>
        /// Dictionary for searching Parameters for the Maintenance Comments object.
        /// </summary>
        private static readonly Dictionary<MaintenanceCommentsDTO.SearchByCommentsParameters, string> DBSearchParameters = new Dictionary<MaintenanceCommentsDTO.SearchByCommentsParameters, string>
        {
                {MaintenanceCommentsDTO.SearchByCommentsParameters.COMMENT_ID, "c.CommentId"},
                {MaintenanceCommentsDTO.SearchByCommentsParameters.MAINT_CHECK_LIST_DETAIL_ID, "c.MaintChklstdetId"},
                {MaintenanceCommentsDTO.SearchByCommentsParameters.COMMENT_TYPE, "c.CommentType"},
                {MaintenanceCommentsDTO.SearchByCommentsParameters.COMMENT, "c.Comment"},
                {MaintenanceCommentsDTO.SearchByCommentsParameters.IS_ACTIVE, "c.IsActive"},
                {MaintenanceCommentsDTO.SearchByCommentsParameters.MASTER_ENTITY_ID,"c.MasterEntityId"},
                {MaintenanceCommentsDTO.SearchByCommentsParameters.SITE_ID, "c.site_id"}
        };

        /// <summary>
        /// Parameterized Constructor for MaintenanceCommentsDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction Object</param>
        public MaintenanceCommentsDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Inserts the Maintenance Comments record to the database
        /// </summary>
        /// <param name="maintenanceCommentsDTO">MaintenanceCommentsDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted MaintenanceCommentsDTO</returns>
        public MaintenanceCommentsDTO Insert(MaintenanceCommentsDTO maintenanceCommentsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(maintenanceCommentsDTO, loginId, siteId);
            string insertCommentsQuery = @"insert into Comments 
                                                        ( 
                                                        MaintChklstdetId,
                                                        CommentType,
                                                        Comment,
                                                        IsActive,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastUpdatedDate,
                                                        MasterEntityId,
                                                        Guid,
                                                        site_id
                                                        ) 
                                                values 
                                                        (
                                                         @maintCheckListDetailId,
                                                         @commentType,
                                                         @comment,
                                                         @isActive,
                                                         @CreatedBy,
                                                         Getdate(),                                                         
                                                         @CreatedBy,
                                                         Getdate(), 
                                                         @masterEntityId,
                                                         NEWID(),
                                                         @site_id
                                                         )SELECT * FROM Comments WHERE CommentId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertCommentsQuery, GetSQLParameters(maintenanceCommentsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCommentsDTO(maintenanceCommentsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting MaintenanceCommentsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(maintenanceCommentsDTO);
            return maintenanceCommentsDTO;
        }


        /// <summary>
        /// Updates the Asset type record
        /// </summary>
        /// <param name="maintenanceCommentsDTO">MaintenanceCommentsDTO type parameter</param>
        /// <param name="loginId">Login Id</param>
        /// <param name="siteId">Site Id</param>
        /// <returns>Returns the MaintenanceCommentsDTO</returns>
        public MaintenanceCommentsDTO Update(MaintenanceCommentsDTO maintenanceCommentsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(maintenanceCommentsDTO, loginId, siteId);
            string updateCommentsQuery = @"update Comments 
                                          set MaintChklstdetId = @maintCheckListDetailId,
                                              CommentType = @commentType,
                                              Comment = @comment,
                                              MasterEntityId = @masterEntityId,
                                              IsActive = @isActive, 
                                              LastUpdatedBy = @lastUpdatedBy, 
                                              LastupdatedDate = Getdate()
                                              --site_id = @site_id                                           
                                              where  CommentId = @commentId
                                              SELECT* FROM Comments WHERE CommentId = @commentId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateCommentsQuery, GetSQLParameters(maintenanceCommentsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCommentsDTO(maintenanceCommentsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating CommentsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(maintenanceCommentsDTO);
            return maintenanceCommentsDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="maintenanceCommentsDTO">MaintenanceCommentsDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshCommentsDTO(MaintenanceCommentsDTO maintenanceCommentsDTO, DataTable dt)
        {
            log.LogMethodEntry(maintenanceCommentsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                maintenanceCommentsDTO.CommentId = Convert.ToInt32(dt.Rows[0]["CommentId"]);
                maintenanceCommentsDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                maintenanceCommentsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                maintenanceCommentsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                maintenanceCommentsDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                maintenanceCommentsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                maintenanceCommentsDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating Maintenance Comments Record.
        /// </summary>
        /// <param name="maintenanceCommentsDTO">MaintenanceCommentsDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(MaintenanceCommentsDTO maintenanceCommentsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(maintenanceCommentsDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@commentId", maintenanceCommentsDTO.CommentId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@maintCheckListDetailId", maintenanceCommentsDTO.MaintCheckListDetailId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@commentType", maintenanceCommentsDTO.CommentType, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@comment", maintenanceCommentsDTO.Comment));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", maintenanceCommentsDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", maintenanceCommentsDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Converts the Data row object to MaintenanceCommentsDTO class type
        /// </summary>
        /// <param name="commentsDataRow">Comments DataRow</param>
        /// <returns>Returns MaintenanceCommentsDTO</returns>
        private MaintenanceCommentsDTO GetMaintenanceCommentsDTO(DataRow commentsDataRow)
        {
            log.LogMethodEntry(commentsDataRow);
            MaintenanceCommentsDTO commentsDTO = new MaintenanceCommentsDTO(commentsDataRow["CommentId"] == DBNull.Value ? -1 : Convert.ToInt32(commentsDataRow["CommentId"]),
                                            commentsDataRow["MaintChklstdetId"] == DBNull.Value ? -1 : Convert.ToInt32(commentsDataRow["MaintChklstdetId"]),
                                            commentsDataRow["CommentType"] == DBNull.Value ? -1 : Convert.ToInt32(commentsDataRow["CommentType"]),
                                            commentsDataRow["Comment"].ToString(),
                                            commentsDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(commentsDataRow["IsActive"]),
                                            commentsDataRow["CreatedBy"].ToString(),
                                            commentsDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(commentsDataRow["CreationDate"]),
                                            commentsDataRow["LastUpdatedBy"].ToString(),
                                            commentsDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(commentsDataRow["LastupdatedDate"]),
                                            commentsDataRow["Guid"].ToString(),
                                            commentsDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(commentsDataRow["site_id"]),
                                            commentsDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(commentsDataRow["SynchStatus"]),
                                            commentsDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(commentsDataRow["MasterEntityId"])
                                            );
            log.LogMethodExit(commentsDTO);
            return commentsDTO;
        }

        /// <summary>
        /// Gets the Maintenance Comments data of passed comments Id
        /// </summary>
        /// <param name="commentId">integer type parameter</param>
        /// <returns>Returns CommentsDTO</returns>
        public MaintenanceCommentsDTO GetMaintenanceComments(int commentId)
        {
            log.LogMethodEntry(commentId);
            MaintenanceCommentsDTO result = null;
            string query = SELECT_QUERY + " WHERE c.CommentId = @Id";
            SqlParameter parameter = new SqlParameter("@Id", commentId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetMaintenanceCommentsDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the List of MaintenanceCommentsDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of MaintenanceCommentsDTO</returns>
        public List<MaintenanceCommentsDTO> GetMaintenanceCommentsDTOList(List<KeyValuePair<MaintenanceCommentsDTO.SearchByCommentsParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<MaintenanceCommentsDTO> maintenanceCommentsDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<MaintenanceCommentsDTO.SearchByCommentsParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == MaintenanceCommentsDTO.SearchByCommentsParameters.COMMENT_ID
                            || searchParameter.Key == MaintenanceCommentsDTO.SearchByCommentsParameters.MAINT_CHECK_LIST_DETAIL_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == MaintenanceCommentsDTO.SearchByCommentsParameters.COMMENT_TYPE
                                 || searchParameter.Key == MaintenanceCommentsDTO.SearchByCommentsParameters.COMMENT)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == MaintenanceCommentsDTO.SearchByCommentsParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == MaintenanceCommentsDTO.SearchByCommentsParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                    counter++;
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                maintenanceCommentsDTOList = new List<MaintenanceCommentsDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    MaintenanceCommentsDTO commentsDTO = GetMaintenanceCommentsDTO(dataRow);
                    maintenanceCommentsDTOList.Add(commentsDTO);
                }
            }
            log.LogMethodExit(maintenanceCommentsDTOList);
            return maintenanceCommentsDTOList;
        }

        /// <summary>
        /// Gets the MaintenanceCommentsDTO List for maintChklstdet Id List
        /// </summary>
        /// <param name="maintChklstdetIdList">integer list parameter</param>
        /// <returns>Returns List of MaintenanceCommentsDTOList</returns>
        public List<MaintenanceCommentsDTO> GetMaintenanceCommentsDTOList(List<int> maintChklstdetIdList, bool activeRecords)
        {
            log.LogMethodEntry(maintChklstdetIdList);
            List<MaintenanceCommentsDTO> list = new List<MaintenanceCommentsDTO>();
            string query = @"SELECT Comments.*
                            FROM Comments, @maintChklstdetIdList List
                            WHERE MaintChklstdetId = List.id ";
            if (activeRecords)
            {
                query += " AND isActive = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@maintChklstdetIdList", maintChklstdetIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetMaintenanceCommentsDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }
    }
}
