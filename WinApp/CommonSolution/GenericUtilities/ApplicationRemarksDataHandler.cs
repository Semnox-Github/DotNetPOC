/********************************************************************************************
 * Project Name - ApplicationRemarks Data Handler
 * Description  - Data Handler
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By          Remarks          
 *********************************************************************************************
 *2.70.2        25-Jul-2019       Dakshakh raj        Modified : added GetSQLParameters(), 
 *                                                          SQL injection Issue Fix.
 *2.70.2        06-Dec-2019       Jinto Thomas            Removed siteid from update query                                                           
 *2.100.0     31-Aug-2020   Mushahid Faizan         siteId changes in GetSQLParameters().
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Application Remarks Data Handler  - Handles insert, update and select of application remarks objects
    /// </summary>
    public class ApplicationRemarksDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM ApplicationRemarks as ar ";

        /// <summary>
        /// Dictionary for searching Parameters for the ApplicationContentTranslated object.
        /// </summary>
        private static readonly Dictionary<ApplicationRemarksDTO.SearchByApplicationRemarksParameters, string> DBSearchParameters = new Dictionary<ApplicationRemarksDTO.SearchByApplicationRemarksParameters, string>
            {
                {ApplicationRemarksDTO.SearchByApplicationRemarksParameters.ID, "ar.Id"},
                {ApplicationRemarksDTO.SearchByApplicationRemarksParameters.MODULE_NAME, "ar.ModuleName"},
                {ApplicationRemarksDTO.SearchByApplicationRemarksParameters.ACTIVE_FLAG, "ar.IsActive"},
                {ApplicationRemarksDTO.SearchByApplicationRemarksParameters.SOURCE_GUID, "ar.SourceGuid"},
                {ApplicationRemarksDTO.SearchByApplicationRemarksParameters.SOURCE_NAME, "ar.SourceName"},
                {ApplicationRemarksDTO.SearchByApplicationRemarksParameters.MASTER_ENTITY_ID,"ar.MasterEntityId"},
                {ApplicationRemarksDTO.SearchByApplicationRemarksParameters.SITE_ID, "ar.site_id"}
            };

        /// <summary>
        /// Default constructor of ApplicationRemarksDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ApplicationRemarksDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        ///Builds the SQL Parameter list used for inserting and updating ApplicationRemark Reecord.
        /// </summary>
        /// <param name="applicationRemarksDTO">applicationRemarksDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns> Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(ApplicationRemarksDTO applicationRemarksDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(applicationRemarksDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@id", applicationRemarksDTO.Id));
            parameters.Add(dataAccessHandler.GetSQLParameter("@moduleName", string.IsNullOrEmpty(applicationRemarksDTO.ModuleName) ? DBNull.Value : (object)applicationRemarksDTO.ModuleName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@sourceName", string.IsNullOrEmpty(applicationRemarksDTO.SourceName) ? DBNull.Value : (object)applicationRemarksDTO.SourceName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@sourceGuid", string.IsNullOrEmpty(applicationRemarksDTO.SourceGuid) ? DBNull.Value : (object)applicationRemarksDTO.SourceGuid));
            parameters.Add(dataAccessHandler.GetSQLParameter("@remarks", string.IsNullOrEmpty(applicationRemarksDTO.Remarks) ? DBNull.Value : (object)applicationRemarksDTO.Remarks));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", applicationRemarksDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", applicationRemarksDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the application remarks record to the database
        /// </summary>
        /// <param name="applicationRemarksDTO">ApplicationRemarksDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="sqlTransaction">sqlTransaction </param>
        /// <returns>Returns inserted record id</returns>
        public ApplicationRemarksDTO InsertApplicationRemarks(ApplicationRemarksDTO applicationRemarksDTO, string loginId, int siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(applicationRemarksDTO, loginId, siteId);
            string insertApplicationRemarksQuery = @"INSERT INTO [dbo].[ApplicationRemarks] 
                                                        ( 
                                                         ModuleName
                                                        ,SourceName
                                                        ,SourceGuid
                                                        ,Remarks
                                                        ,CreatedBy
                                                        ,CreationDate
                                                        ,LastUpdatedBy
                                                        ,LastupdatedDate
                                                        ,Guid
                                                        ,site_id
                                                        ,MasterEntityId
                                                        ,IsActive
                                                        ) 
                                                values 
                                                        (                                                         
                                                         @moduleName
                                                        ,@sourceName
                                                        ,@sourceGuid
                                                        ,@remarks
                                                        ,@createdBy
                                                        ,getdate()
                                                        ,@lastUpdatedBy
                                                        ,getdate()
                                                        ,NewId()
                                                        ,@siteId
                                                        ,@masterEntityId
                                                        ,@isActive
                                                        )SELECT * FROM ApplicationRemarks WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertApplicationRemarksQuery, GetSQLParameters(applicationRemarksDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshApplicationRemarksDTO(applicationRemarksDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting ApplicationRemarks", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(applicationRemarksDTO);
            return applicationRemarksDTO;
        }

        /// <summary>
        /// Updates the application remarks record
        /// </summary>
        /// <param name="applicationRemarksDTO">ApplicationRemarksDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="sqlTransaction">SQL Transactions </param>
        /// <returns>Returns the count of updated rows</returns>
        public ApplicationRemarksDTO UpdateApplicationRemarks(ApplicationRemarksDTO applicationRemarksDTO, string loginId, int siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(applicationRemarksDTO, loginId, siteId);
            string updateApplicationRemarksQuery = @"update ApplicationRemarks 
                                         set ModuleName=@moduleName,
                                             SourceName=@sourceName,
                                             SourceGuid=@sourceGuid,
                                             Remarks=@remarks,
                                             IsActive = @isActive, 
                                             LastUpdatedBy = @lastUpdatedBy, 
                                             LastupdatedDate = Getdate(),
                                             --site_id=@siteid,
                                             MasterEntityId=@masterEntityId                                                                                       
                                       where Id = @id
                                       SELECT* FROM ApplicationRemarks WHERE  Id = @id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateApplicationRemarksQuery, GetSQLParameters(applicationRemarksDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshApplicationRemarksDTO(applicationRemarksDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating ApplicationRemarksDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(applicationRemarksDTO);
            return applicationRemarksDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="applicationRemarksDTO">applicationRemarksDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshApplicationRemarksDTO(ApplicationRemarksDTO applicationRemarksDTO, DataTable dt)
        {
            log.LogMethodEntry(applicationRemarksDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                applicationRemarksDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                applicationRemarksDTO.LastUpdatedDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
                applicationRemarksDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                applicationRemarksDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                applicationRemarksDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                applicationRemarksDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                applicationRemarksDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Converts the Data row object to ApplicationRemarksDTO class type
        /// </summary>
        /// <param name="applicationRemarksDataRow">ApplicationRemarks DataRow</param>
        /// <returns>Returns ApplicationRemarks</returns>
        private ApplicationRemarksDTO GetApplicationRemarksDTO(DataRow applicationRemarksDataRow)
        {
            log.LogMethodEntry(applicationRemarksDataRow);
            ApplicationRemarksDTO applicationRemarksDataObject = new ApplicationRemarksDTO(Convert.ToInt32(applicationRemarksDataRow["Id"]),
                                            applicationRemarksDataRow["ModuleName"].ToString(),
                                            applicationRemarksDataRow["SourceName"].ToString(),
                                            applicationRemarksDataRow["SourceGuid"].ToString(),
                                            applicationRemarksDataRow["Remarks"].ToString(),
                                            applicationRemarksDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(applicationRemarksDataRow["IsActive"]),
                                            applicationRemarksDataRow["CreatedBy"].ToString(),
                                            applicationRemarksDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(applicationRemarksDataRow["CreationDate"]),
                                            applicationRemarksDataRow["LastUpdatedBy"].ToString(),
                                            applicationRemarksDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(applicationRemarksDataRow["LastupdatedDate"]),
                                            applicationRemarksDataRow["Guid"].ToString(),
                                            applicationRemarksDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(applicationRemarksDataRow["site_id"]),
                                            applicationRemarksDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(applicationRemarksDataRow["SynchStatus"]),
                                            applicationRemarksDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(applicationRemarksDataRow["MasterEntityId"])
                                            );
            log.LogMethodExit(applicationRemarksDataObject);
            return applicationRemarksDataObject;
        }

        /// <summary>
        /// Gets the application remarks data of passed applicationRemarksId
        /// </summary>
        /// <param name="applicationRemarksId">integer type parameter</param>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        /// <returns>Returns ApplicationRemarksDTO</returns>
        public ApplicationRemarksDTO GetApplicationRemarks(int applicationRemarksId, SqlTransaction sqlTransaction = null )
        {
            log.LogMethodEntry(applicationRemarksId, sqlTransaction);
            ApplicationRemarksDTO result = null;
            string selectApplicationRemarksQuery = SELECT_QUERY + @" WHERE ar.Id = @id";
            SqlParameter parameter = new SqlParameter("@id", applicationRemarksId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectApplicationRemarksQuery, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetApplicationRemarksDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the ApplicationRemarksDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        /// <returns>Returns the list of ApplicationRemarksDTO matching the search criteria</returns>
        public List<ApplicationRemarksDTO> GetApplicationRemarksList(List<KeyValuePair<ApplicationRemarksDTO.SearchByApplicationRemarksParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<ApplicationRemarksDTO> applicationRemarksDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<ApplicationRemarksDTO.SearchByApplicationRemarksParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == ApplicationRemarksDTO.SearchByApplicationRemarksParameters.ID
                            || searchParameter.Key == ApplicationRemarksDTO.SearchByApplicationRemarksParameters.MASTER_ENTITY_ID
                            || searchParameter.Key == ApplicationRemarksDTO.SearchByApplicationRemarksParameters.ACTIVE_FLAG)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ApplicationRemarksDTO.SearchByApplicationRemarksParameters.MODULE_NAME
                                 || searchParameter.Key == ApplicationRemarksDTO.SearchByApplicationRemarksParameters.SOURCE_NAME
                                 || searchParameter.Key == ApplicationRemarksDTO.SearchByApplicationRemarksParameters.SOURCE_GUID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == ApplicationRemarksDTO.SearchByApplicationRemarksParameters.SITE_ID)
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
                selectQuery = selectQuery + query + " Order by ModuleName,CreationDate desc,SourceName,SourceGuid";
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                applicationRemarksDTOList = new List<ApplicationRemarksDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ApplicationRemarksDTO applicationRemarksDTO  = GetApplicationRemarksDTO(dataRow);
                    applicationRemarksDTOList.Add(applicationRemarksDTO);
                }
            }
            log.LogMethodExit(applicationRemarksDTOList);
            return applicationRemarksDTOList;
        }

    }
}
