/********************************************************************************************
 * Project Name - RequisitionTemplate Lines Data Handler
 * Description  - Data handler of the RequisitionTemplate Lines type class
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By      Remarks          
 *********************************************************************************************
 *1.00        10-Aug-2016    Suneetha.S       Created 
 *2.70        16-Jul-2019    Dakshakh raj     Modified : added GetSQLParameters() and
                                                      SQL injection Issue Fix
 *2.110.0    15-Dec-2020   Mushahid Faizan    Modified : Web Inventory Changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;


namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// Requisition Template Type Data Handler - Handles insert, update and select of Requisition type objects
    /// </summary>
    public class RequisitionTemplatesDataHandler
    {
        logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        List<SqlParameter> parameters = new List<SqlParameter>();
        private const string SELECT_QUERY = @"SELECT * FROM RequisitionTemplates AS rt ";

        /// <summary>
        /// Dictionary for searching Parameters for the RequisitionTemplates object.
        /// </summary>
        private static readonly Dictionary<RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters, string> DBSearchParameters = new Dictionary<RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters, string>
            {
                {RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters.TEMPLATE_ID, "rt.TemplateId"},
                {RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters.TEMPLATE_NAME, "rt.Name"},
                {RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters.ACTIVE_FLAG, "rt.IsActive"},
                {RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters.REQUISITION_TYPE, "rt.RequisitionType"},
                {RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters.STATUS, "rt.Status"},
                {RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters.SITEID, "rt.Site_id"},
                {RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters.MASTER_ENTITY_ID, "rt.MasterEntityId"}
            };

        /// <summary>
        ///  Default constructor of ReqTemplatesTypeDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public RequisitionTemplatesDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        ///  Builds the SQL Parameter list used for inserting and updating requisitionTemplates Record.
        /// </summary>
        /// <param name="requisitionTemplatesDTO">requisitionTemplates object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(RequisitionTemplatesDTO requisitionTemplatesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(requisitionTemplatesDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@templateId", requisitionTemplatesDTO.TemplateId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@requisitionType", requisitionTemplatesDTO.RequisitionType, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@name", string.IsNullOrEmpty(requisitionTemplatesDTO.TemplateName) ? DBNull.Value : (object)requisitionTemplatesDTO.TemplateName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@requestingDept", requisitionTemplatesDTO.RequestingDept, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@fromDepartment", requisitionTemplatesDTO.FromDepartment, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@toDepartment", requisitionTemplatesDTO.ToDepartment, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@remarks", string.IsNullOrEmpty(requisitionTemplatesDTO.Remarks) ? DBNull.Value : (object)requisitionTemplatesDTO.Remarks));
            parameters.Add(dataAccessHandler.GetSQLParameter("@status", string.IsNullOrEmpty(requisitionTemplatesDTO.Status) ? DBNull.Value : (object)requisitionTemplatesDTO.Status));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteid", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", requisitionTemplatesDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", requisitionTemplatesDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@requiredByDate", requisitionTemplatesDTO.RequiredByDate == DateTime.MinValue ? DBNull.Value : (object)requisitionTemplatesDTO.RequiredByDate));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the Req Templates type record to the database
        /// </summary>
        /// <param name="reqTemplatesDTO">RequisitionTemplatesDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>reqTemplatesDTO</returns>
        public RequisitionTemplatesDTO Insert(RequisitionTemplatesDTO reqTemplatesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(reqTemplatesDTO, loginId, siteId);
            string insertReqTemplatesQuery = @"INSERT INTO [dbo].[RequisitionTemplates] 
                                                        (                                                         
                                                        RequisitionType,
                                                        Name,
                                                        RequestingDept,
                                                        FromDepartment,
                                                        ToDepartment,
                                                        Remarks,
                                                        Status,                                                        
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastUpdatedDate,
                                                        Guid,
                                                        site_id,
                                                        IsActive,
                                                        RequiredByDate,
                                                        MasterEntityId
                                                        ) 
                                                values 
                                                        (
                                                         @requisitionType,
                                                         @name,
                                                         @requestingDept,
                                                         @fromDepartment,   
                                                         @toDepartment,
                                                         @remarks,
                                                         @status,                                                         
                                                         @createdBy,
                                                         Getdate(),                                                         
                                                         @createdBy,
                                                         Getdate(),                                                         
                                                         NEWID(),
                                                         @siteid,
                                                         @isActive,
                                                         @requiredByDate,
                                                         @masterEntityId
                                                        )SELECT * FROM RequisitionTemplates WHERE TemplateId =scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertReqTemplatesQuery, GetSQLParameters(reqTemplatesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshRequisitionTemplatesDTO(reqTemplatesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting RequisitionTemplatesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(reqTemplatesDTO);
            return reqTemplatesDTO;
        }

        /// <summary>
        /// Updates the requisition template header 
        /// </summary>
        /// <param name="reqTemplateHeadeDTO">RequisitionTemplatesDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>reqTemplatesDTO</returns>
        public RequisitionTemplatesDTO Update(RequisitionTemplatesDTO reqTemplateHeadeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(reqTemplateHeadeDTO, loginId, siteId);
            string updateReqTemplateHeaderQuery = @"update RequisitionTemplates 
		                                                set RequisitionType = @requisitionType,
		                                                Name = @name,
		                                                RequestingDept = @requestingDept,
		                                                FromDepartment = @fromDepartment,
		                                                ToDepartment = @toDepartment,
		                                                Remarks = @remarks,
		                                                IsActive = @isActive,
		                                                Status = @status,
                                                        RequiredByDate=@requiredByDate,
		                                                LastUpdatedBy = @lastUpdatedBy, 
		                                                LastupdatedDate = Getdate()
		                                                where TemplateId = @templateId
                                                        SELECT* FROM RequisitionTemplates WHERE TemplateId = @templateId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateReqTemplateHeaderQuery, GetSQLParameters(reqTemplateHeadeDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshRequisitionTemplatesDTO(reqTemplateHeadeDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating RequisitionTemplatesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(reqTemplateHeadeDTO);
            return reqTemplateHeadeDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="requisitionTemplatesDTO">requisitionTemplatesDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshRequisitionTemplatesDTO(RequisitionTemplatesDTO requisitionTemplatesDTO, DataTable dt)
        {
            log.LogMethodEntry(requisitionTemplatesDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                requisitionTemplatesDTO.TemplateId = Convert.ToInt32(dt.Rows[0]["TemplateId"]);
                requisitionTemplatesDTO.lastUpdateDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                requisitionTemplatesDTO.CreatedDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                requisitionTemplatesDTO.GUID = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                requisitionTemplatesDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                requisitionTemplatesDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                requisitionTemplatesDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Converts the Data row object to RequisitionTemplatesDTO class type
        /// </summary>
        /// <param name="requisitionDataRow">RequisitionTemplatesDTO DataRow</param>
        /// <returns>Returns RequisitionTemplatesDTO</returns>
        private RequisitionTemplatesDTO GetRequisitionTemplatesDTO(DataRow requisitionDataRow)
        {
            log.LogMethodEntry(requisitionDataRow);
            RequisitionTemplatesDTO productDataObject = new RequisitionTemplatesDTO(Convert.ToInt32(requisitionDataRow["TemplateId"]),
                                                          Convert.ToInt32(requisitionDataRow["RequisitionType"]),
                                                          requisitionDataRow["Name"].ToString(),
                                                          requisitionDataRow["RequestingDept"] == DBNull.Value ? -1 : Convert.ToInt32(requisitionDataRow["RequestingDept"]),
                                                          requisitionDataRow["FromDepartment"] == DBNull.Value ? -1 : Convert.ToInt32(requisitionDataRow["FromDepartment"]),
                                                          requisitionDataRow["ToDepartment"] == DBNull.Value ? -1 : Convert.ToInt32(requisitionDataRow["ToDepartment"]),
                                                          //Convert.ToDouble(requisitionDataRow["EstimatedValue"]),
                                                          Convert.ToBoolean(requisitionDataRow["IsActive"]),
                                                          requisitionDataRow["Remarks"] == DBNull.Value ? string.Empty : Convert.ToString(requisitionDataRow["Remarks"]),
                                                          requisitionDataRow["Status"].ToString(),
                                                          requisitionDataRow["RequiredByDate"] == DBNull.Value ? ServerDateTime.Now : Convert.ToDateTime(requisitionDataRow["RequiredByDate"]),
                                                          requisitionDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(requisitionDataRow["CreatedBy"]),
                                                          requisitionDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(requisitionDataRow["CreationDate"]),
                                                          requisitionDataRow["LastUpdatedBy"].ToString(),
                                                          requisitionDataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(requisitionDataRow["LastUpdatedDate"]),
                                                          requisitionDataRow["Guid"].ToString(),
                                                          requisitionDataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(requisitionDataRow["Site_id"]),
                                                          requisitionDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(requisitionDataRow["SynchStatus"]),
                                                          requisitionDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(requisitionDataRow["MasterEntityId"])
                                                         );
            log.LogMethodExit(productDataObject);
            return productDataObject;
        }

        /// <summary>
        /// Gets the requisition data of passed patch asset application id
        /// </summary>
        /// <param name="templateId">integer type parameter</param>
        /// <returns>Returns RequisitionTemplatesDTO</returns>
        public RequisitionTemplatesDTO GetRequisitionTemplates(int templateId)
        {
            log.LogMethodEntry(templateId);
            RequisitionTemplatesDTO result = null;
            string selectRequisitionquery = SELECT_QUERY + @" WHERE rt.TemplateId = @templateId";
            SqlParameter parameter = new SqlParameter("@templateId", templateId);

            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectRequisitionquery, new SqlParameter[] { parameter }, sqlTransaction);

            if (dataTable.Rows.Count > 0)
            {
                result = GetRequisitionTemplatesDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the RequisitionTemplatesDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of RequisitionTemplatesDTO matching the search criteria</returns>
        public List<RequisitionTemplatesDTO> GetRequisitionTemplateList(List<KeyValuePair<RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<RequisitionTemplatesDTO> requisitionTemplatesDTOList = null;
            parameters.Clear();
            string selectQuery = SELECT_QUERY;
            selectQuery = selectQuery + GetFilterQuery(searchParameters);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                requisitionTemplatesDTOList = new List<RequisitionTemplatesDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    RequisitionTemplatesDTO requisitionTemplatesDTO = GetRequisitionTemplatesDTO(dataRow);
                    requisitionTemplatesDTOList.Add(requisitionTemplatesDTO);
                }
            }
            log.LogMethodExit(requisitionTemplatesDTOList);
            return requisitionTemplatesDTOList;
        }

        private string GetFilterQuery(List<KeyValuePair<RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            StringBuilder query = new StringBuilder("");
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters.TEMPLATE_ID
                            || searchParameter.Key == RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters.REQUISITION_TYPE
                            || searchParameter.Key == RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters.SITEID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters.ACTIVE_FLAG)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
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

            }
            log.LogMethodExit();
            return query.ToString();
        }

        /// <summary>
        /// Gets the RequisitionTemplatesDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of RequisitionTemplatesDTO matching the search criteria</returns>
        public List<RequisitionTemplatesDTO> GetRequisitionTemplatesList(List<KeyValuePair<RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters, string>> searchParameters, int currentPage = 0, int pageSize = 0)//added
        {
            log.LogMethodEntry(searchParameters);
            List<RequisitionTemplatesDTO> requisitionTemplatesDTOList = new List<RequisitionTemplatesDTO>();
            parameters.Clear();
            string selectQuery = SELECT_QUERY + GetFilterQuery(searchParameters);
            if (currentPage > 0 || pageSize > 0)
            {
                selectQuery += " ORDER BY rt.TemplateId OFFSET " + (currentPage * pageSize).ToString() + " ROWS";
                selectQuery += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                requisitionTemplatesDTOList = new List<RequisitionTemplatesDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    RequisitionTemplatesDTO requisitionTemplatesDTO = GetRequisitionTemplatesDTO(dataRow);
                    requisitionTemplatesDTOList.Add(requisitionTemplatesDTO);
                }
            }
            log.LogMethodExit(requisitionTemplatesDTOList);
            return requisitionTemplatesDTOList;
        }

        /// <summary>
        /// Returns the no of RequisitionTemplates matching the search parameters
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>no of accounts matching the criteria</returns>
        public int GetRequisitionTemplatesCount(List<KeyValuePair<RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters, string>> searchParameters)
        {
            log.LogMethodEntry();
            int requisitionTemplatesDTOCount = 0;
            string selectQuery = SELECT_QUERY;
            selectQuery = selectQuery + GetFilterQuery(searchParameters);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                requisitionTemplatesDTOCount = Convert.ToInt32(dataTable.Rows.Count);
            }
            log.LogMethodExit(requisitionTemplatesDTOCount);
            return requisitionTemplatesDTOCount;
        }
    }
}
