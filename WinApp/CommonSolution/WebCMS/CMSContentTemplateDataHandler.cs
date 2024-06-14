/********************************************************************************************
 * Project Name - CMSContentTemplate Data Handler 
 * Description  - Data handler of the CMSContentTemplate DataHandler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        06-Apr-2016   Rakshith       Created 
 *2.70        09-Jul-2019   Girish Kundar    Modified : Changed the Structure of Data Handler.
 *                                                      Fix for the SQL Injection Issue.
 *2.70.3      31-Mar-2020    Jeevan            Removed syncstatus from update query  
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.WebCMS
{
    /// <summary>
    ///  CMSContentDataHandler Data Handler - Handles insert, update and select of  CMSContentDataHandler objects
    /// </summary>
    public class CMSContentTemplateDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<CMSContentTemplateDTO.SearchByCMSContentTemplateParameters, string> DBSearchParameters = new Dictionary<CMSContentTemplateDTO.SearchByCMSContentTemplateParameters, string>
            {
                {CMSContentTemplateDTO.SearchByCMSContentTemplateParameters.CONTENT_TEMPLATE_ID, "cct.ContentTemplateId"},
                {CMSContentTemplateDTO.SearchByCMSContentTemplateParameters.CONTENT_TEMPLATE_NAME, "cct.ContentTemplateName"},
                {CMSContentTemplateDTO.SearchByCMSContentTemplateParameters.CONTROL_FILE_NAME, "cct.ControlFileName"},
                {CMSContentTemplateDTO.SearchByCMSContentTemplateParameters.TEMPLATE_FILE_NAME, "cct.TemplateFileName"},
                {CMSContentTemplateDTO.SearchByCMSContentTemplateParameters.IS_ACTIVE, "cct.Active"},
                {CMSContentTemplateDTO.SearchByCMSContentTemplateParameters.CREATED_DATE, "cct.CreatedDate"},
                {CMSContentTemplateDTO.SearchByCMSContentTemplateParameters.CREATED_BY, "cct.CreatedBy"},
                {CMSContentTemplateDTO.SearchByCMSContentTemplateParameters.LAST_UPDATED_DATE, "cct.LastUpdatedDate"},
                {CMSContentTemplateDTO.SearchByCMSContentTemplateParameters.LAST_UPDATED_BY, "cct.LastUpdatedBy"},
                {CMSContentTemplateDTO.SearchByCMSContentTemplateParameters.GUID , "cct.GUID"},
                {CMSContentTemplateDTO.SearchByCMSContentTemplateParameters.SITE_ID, "cct.Site_id"},
                {CMSContentTemplateDTO.SearchByCMSContentTemplateParameters.SYNCH_STATUS, "cct.SynchStatus"},
                {CMSContentTemplateDTO.SearchByCMSContentTemplateParameters.MASTER_ENTITY_ID, "cct.MasterEntityId"},
            };
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction = null;
        private const string SELECT_QUERY = "SELECT *  FROM CMSContentTemplates  AS cct";
        /// <summary>
        /// Default constructor of CMSContentDataHandler class
        /// </summary>
        public CMSContentTemplateDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();

        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating CMSContentTemplate Record.
        /// </summary>
        /// <param name="cMSContentTemplateDTO">CMSContentTemplateDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(CMSContentTemplateDTO cMSContentTemplateDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(cMSContentTemplateDTO, loginId, siteId);

            List<SqlParameter> parameters = new List<SqlParameter>();
            ParametersHelper.ParameterHelper(parameters, "@ContentTemplateId", cMSContentTemplateDTO.ContentTemplateId, true);
            ParametersHelper.ParameterHelper(parameters, "@ContentTemplateName", cMSContentTemplateDTO.ContentTemplateName);
            ParametersHelper.ParameterHelper(parameters, "@ControlFileName", cMSContentTemplateDTO.ControlFileName);
            ParametersHelper.ParameterHelper(parameters, "@TemplateFileName", cMSContentTemplateDTO.TemplateFileName);
            ParametersHelper.ParameterHelper(parameters, "@IsActive", cMSContentTemplateDTO.IsActive);
            ParametersHelper.ParameterHelper(parameters, "@CreatedBy", loginId);
            ParametersHelper.ParameterHelper(parameters, "@LastUpdatedBy", loginId);
            ParametersHelper.ParameterHelper(parameters, "@Site_id", siteId, true);
            ParametersHelper.ParameterHelper(parameters, "@SynchStatus", cMSContentTemplateDTO.SynchStatus);
            ParametersHelper.ParameterHelper(parameters, "@MasterEntityId", cMSContentTemplateDTO.MasterEntityId, true);
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the CMSContentTemplate record to the database
        /// </summary>
        /// <param name="cMSContentTemplateDTO">CMSContentTemplateDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns CMSContentTemplateDTO</returns>
        public CMSContentTemplateDTO InsertCMSContentTemplate(CMSContentTemplateDTO cMSContentTemplateDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(cMSContentTemplateDTO, loginId, siteId);
            string query = @"INSERT INTO CMSContentTemplates 
                                        ( 
                                            ContentTemplateName,
                                            ControlFileName,
                                            TemplateFileName,
                                            Active,
                                            CreationDate,
                                            CreatedBy,
                                            LastUpdatedDate,
                                            LastUpdatedBy,
                                            Guid,
                                            site_id,
                                            MasterEntityId
                                        ) 
                                VALUES 
                                        (
                                            @ContentTemplateName,
                                            @ControlFileName,
                                            @TemplateFileName,
                                            @IsActive,
                                            GETDATE(),
                                            @LastUpdatedBy,
                                            GETDATE(),
                                            @LastUpdatedBy,
                                            NewId(),
                                            @Site_id,
                                            @MasterEntityId
                                        )    SELECT* FROM CMSContentTemplates WHERE ContentTemplateId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(cMSContentTemplateDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCMSContentTemplateDTO(cMSContentTemplateDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting cMSContentTemplateDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(cMSContentTemplateDTO);
            return cMSContentTemplateDTO;

        }


        /// <summary>
        /// Updates the CMSContentTemplate record
        /// </summary>
        /// <param name="cMSContentTemplate">cMSContentTemplate type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        ///   //MasterEntityId = @MasterEntityId

        public CMSContentTemplateDTO UpdateCMSContentTemplate(CMSContentTemplateDTO cMSContentTemplateDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(cMSContentTemplateDTO, loginId, siteId);
            string query = @"UPDATE CMSContentTemplates 
                             SET 
                                            ContentTemplateName = @ContentTemplateName,
                                            ControlFileName = @ControlFileName,
                                            TemplateFileName = @TemplateFileName,
                                            Active = @IsActive,
                                            CreationDate =   GETDATE(),
                                            CreatedBy = @CreatedBy,
                                            LastUpdatedDate =   GETDATE(),
                                            LastUpdatedBy = @LastUpdatedBy,
                                            --Site_id = @Site_id, 
                                            MasterEntityId =  @MasterEntityId
                                     WHERE ContentTemplateId = @ContentTemplateId 
                                     SELECT* FROM CMSContentTemplates WHERE ContentTemplateId = @ContentTemplateId ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(cMSContentTemplateDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCMSContentTemplateDTO(cMSContentTemplateDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating cMSContentTemplateDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(cMSContentTemplateDTO);
            return cMSContentTemplateDTO;
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="CMSContentTemplateDTO">CMSContentTemplateDTO </param>
        private void RefreshCMSContentTemplateDTO(CMSContentTemplateDTO cmsContentTemplateDTO, DataTable dt)
        {
            log.LogMethodEntry(cmsContentTemplateDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                cmsContentTemplateDTO.ContentTemplateId = Convert.ToInt32(dt.Rows[0]["ContentTemplateId"]);
                cmsContentTemplateDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                cmsContentTemplateDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                cmsContentTemplateDTO.CreatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                cmsContentTemplateDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                cmsContentTemplateDTO.CreationDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                cmsContentTemplateDTO.Site_id = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Converts the Data row object to CMSContentTemplateDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns CMSContentTemplateDTO</returns>
        private CMSContentTemplateDTO GetCMSContentTemplateDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            CMSContentTemplateDTO cMSContentTemplateDTO =new CMSContentTemplateDTO(Convert.ToInt32(dataRow["ContentTemplateId"]),
                                            dataRow["ContentTemplateName"] == DBNull.Value ? "" : Convert.ToString(dataRow["ContentTemplateName"]),
                                            dataRow["ControlFileName"] == DBNull.Value ? "" : Convert.ToString(dataRow["ControlFileName"]),
                                            dataRow["TemplateFileName"] == DBNull.Value ? "" : Convert.ToString(dataRow["TemplateFileName"]),
                                            dataRow["Active"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["Active"]),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["CreatedBy"].ToString(),
                                            dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? "" : Convert.ToString(dataRow["LastUpdatedBy"]),
                                            dataRow["Guid"] == DBNull.Value ? "" : Convert.ToString(dataRow["Guid"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"])
                                            );
            log.LogMethodExit(cMSContentTemplateDTO);
            return cMSContentTemplateDTO;
        }


        /// <summary>
        /// Gets the CMSContentTemplate data of passed CMSContentTemplate Id
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns CMSContentTemplateDTO</returns>
        public CMSContentTemplateDTO GetCMSContentTemplateDTO(int id)
        {
            log.LogMethodEntry(id);
            CMSContentTemplateDTO returnValue = new CMSContentTemplateDTO();
            string query =SELECT_QUERY + "     WHERE ContentTemplateId = @ContentTemplateId";
            SqlParameter parameter = new SqlParameter("@ContentTemplateId", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetCMSContentTemplateDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }


        /// <summary>
        /// Gets the CMSContentTemplateDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of CMSContentTemplateDTO matching the search criteria</returns>
        public List<CMSContentTemplateDTO> GetCMSContentTemplateDTOList(List<KeyValuePair<CMSContentTemplateDTO.SearchByCMSContentTemplateParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<CMSContentTemplateDTO> list = new List<CMSContentTemplateDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            int count = 0;
            string selectQuery = SELECT_QUERY ;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<CMSContentTemplateDTO.SearchByCMSContentTemplateParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key == CMSContentTemplateDTO.SearchByCMSContentTemplateParameters.CONTENT_TEMPLATE_ID
                            || searchParameter.Key == CMSContentTemplateDTO.SearchByCMSContentTemplateParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CMSContentTemplateDTO.SearchByCMSContentTemplateParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }

                        else if (searchParameter.Key == CMSContentTemplateDTO.SearchByCMSContentTemplateParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1) =  " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else
                        {
                            query.Append(joiner +  "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
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
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    CMSContentTemplateDTO cMSContentTemplateDTO = GetCMSContentTemplateDTO(dataRow);
                    list.Add(cMSContentTemplateDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }
    }
}
