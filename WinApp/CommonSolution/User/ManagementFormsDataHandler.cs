/********************************************************************************************
 * Project Name - ManagementFormsDataHandler
 * Description  - Data Handler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.2        15-Oct-2019   Jagan Mohan          Created 
 *2.150.3     27-APR-2023   Sweedol             Modified: Management form access new architecture changes
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;

namespace Semnox.Parafait.User
{
    class ManagementFormsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM ManagementForms ";

        /// <summary>
        /// Dictionary for searching Parameters for the ManagementFormsDTO object.
        /// </summary>
        private static readonly Dictionary<ManagementFormsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ManagementFormsDTO.SearchByParameters, string>
        {
            { ManagementFormsDTO.SearchByParameters.MANAGEMENT_FORM_ID,"ManagementFormId"},
            { ManagementFormsDTO.SearchByParameters.FUNCTION_GROUP,"FunctionGroup"},
            { ManagementFormsDTO.SearchByParameters.GROUP_NAME,"GroupName"},
            { ManagementFormsDTO.SearchByParameters.FORM_NAME,"FormName"},
            { ManagementFormsDTO.SearchByParameters.ISACTIVE,"IsActive"},
            { ManagementFormsDTO.SearchByParameters.SITE_ID,"site_id"},
            { ManagementFormsDTO.SearchByParameters.MASTER_ENTITY_ID,"MasterEntityId"}
        };
        /// <summary>
        /// Parameterized Constructor for ManagementForms.
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public ManagementFormsDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating ManagementForms Record.
        /// </summary>
        /// <param name="managementFormsDTO"></param>
        /// <param name="userId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        private List<SqlParameter> GetSQLParameters(ManagementFormsDTO managementFormsDTO, string userId, int siteId)
        {
            log.LogMethodEntry(managementFormsDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@ManagementFormId", managementFormsDTO.ManagementFormId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@FunctionGroup", managementFormsDTO.FunctionGroup));
            parameters.Add(dataAccessHandler.GetSQLParameter("@GroupName", managementFormsDTO.GroupName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@FormName", managementFormsDTO.FormName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@FormLookupTable", managementFormsDTO.FormLookupTable));
            parameters.Add(dataAccessHandler.GetSQLParameter("@FontImageIcon", managementFormsDTO.FontImageIcon));
            parameters.Add(dataAccessHandler.GetSQLParameter("@FormTargetPath", managementFormsDTO.FormTargetPath));
            parameters.Add(dataAccessHandler.GetSQLParameter("@EnableAccess", managementFormsDTO.EnableAccess));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DisplayOrder", managementFormsDTO.DisplayOrder, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", managementFormsDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", managementFormsDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Builds ProductKey DTO from the passed DataRow
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        private ManagementFormsDTO GetManagementFormsDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ManagementFormsDTO managementFormsDTO = new ManagementFormsDTO(dataRow["ManagementFormId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ManagementFormId"]),
                                                dataRow["FunctionGroup"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["FunctionGroup"]),
                                                dataRow["GroupName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["GroupName"]),
                                                dataRow["FormName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["FormName"]),
                                                dataRow["FormLookupTable"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["FormLookupTable"]),
                                                dataRow["FontImageIcon"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["FontImageIcon"]),
                                                dataRow["FormTargetPath"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["FormTargetPath"]),
                                                dataRow["EnableAccess"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["EnableAccess"]),
                                                dataRow["DisplayOrder"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["DisplayOrder"]),
                                                dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                                dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"])
                                                );
            return managementFormsDTO;
        }

        /// <summary>
        /// Gets the ManagementFormsDTO data of passed managementFormId
        /// </summary>
        /// <param name="managementFormId"></param>
        /// <returns>Returns ManagementFormsDTO</returns>
        public ManagementFormsDTO GetManagementFormsDTO(int managementFormId)
        {
            log.LogMethodEntry(managementFormId);
            ManagementFormsDTO result = null;
            string query = SELECT_QUERY + @" WHERE ManagementForms.ManagementFormId = @ManagementFormId";
            SqlParameter parameter = new SqlParameter("@ManagementFormId", managementFormId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetManagementFormsDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// Refreshing the ManagementFormsDTO
        /// </summary>
        /// <param name="managementFormsDTO"></param>
        /// <param name="dt"></param>
        /// <param name="userId"></param>
        /// <param name="siteId"></param>
        private void RefreshManagementFormsDTO(ManagementFormsDTO managementFormsDTO, DataTable dt, string userId, int siteId)
        {
            log.LogMethodEntry(managementFormsDTO, dt, userId, siteId);
            if (dt.Rows.Count > 0)
            {
                managementFormsDTO.ManagementFormId = Convert.ToInt32(dt.Rows[0]["ManagementFormId"]);
                managementFormsDTO.CreatedBy = userId;
                managementFormsDTO.CreationDate = String.IsNullOrEmpty(dt.Rows[0]["CreationDate"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(dt.Rows[0]["CreationDate"]);
                managementFormsDTO.Guid = Convert.ToString(dt.Rows[0]["Guid"]);
                managementFormsDTO.LastUpdatedBy = userId;
                managementFormsDTO.LastUpdatedDate = String.IsNullOrEmpty(dt.Rows[0]["LastUpdateDate"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(dt.Rows[0]["LastUpdateDate"]);
                managementFormsDTO.SiteId = siteId;
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///  Inserts the record to the managementForms Table. 
        /// </summary>
        /// <param name="managementFormsDTO"></param>
        /// <param name="userId"></param>
        /// <param name = "siteId" ></ param >
        /// <returns>Returns updated ManagementFormsDTO</returns>
        public ManagementFormsDTO Insert(ManagementFormsDTO managementFormsDTO, string userId, int siteId)
        {
            log.LogMethodEntry(managementFormsDTO, userId, siteId);
            string query = @"INSERT INTO [dbo].[ManagementForms]
                            (
                            FunctionGroup,
                            GroupName,
                            FormName,
                            FormLookupTable,
                            FontImageIcon,
                            FormTargetPath,
                            EnableAccess,
                            DisplayOrder,
                            Guid,
                            site_id,
                            MasterEntityId,
                            CreatedBy,
                            CreationDate,
                            LastUpdatedBy,
                            LastUpdateDate,
                            IsActive
                            )
                            VALUES
                            (
                            @FunctionGroup,
                            @GroupName,
                            @FormName,
                            @FormLookupTable,
                            @FontImageIcon,
                            @FormTargetPath,
                            @EnableAccess,
                            @DisplayOrder,
                            NEWID(),
                            @site_id,
                            @MasterEntityId,
                            @CreatedBy,
                            GETDATA(),
                            @LastUpdatedBy,
                            GETDATA(),
                            @IsActive          
                            )
                            SELECT * FROM ManagementForms WHERE ManagementFormId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(managementFormsDTO, userId, siteId).ToArray(), sqlTransaction);
                RefreshManagementFormsDTO(managementFormsDTO, dt, userId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while inserting ManagementFormsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(managementFormsDTO);
            return managementFormsDTO;
        }

        /// <summary>
        /// Update the record in the managementForms Table. 
        /// </summary>
        /// <param name="managementFormsDTO"></param>
        /// <param name="userId"></param>
        /// <param name="siteId"></param>
        /// <returns>Returns updated ManagementFormsDTO</returns>
        public ManagementFormsDTO Update(ManagementFormsDTO managementFormsDTO, string userId, int siteId)
        {
            log.LogMethodEntry(managementFormsDTO, userId, siteId);
            string query = @"UPDATE [dbo].[ManagementForms]
                             SET
                            FunctionGroup = @FunctionGroup,
                            GroupName = @GroupName,
                            FormName = @FormName,
                            FormLookupTable = @FormLookupTable,
                            FontImageIcon = @FontImageIcon,
                            FormTargetPath = @FormTargetPath,
                            EnableAccess = @EnableAccess,
                            DisplayOrder = @DisplayOrder,
                            site_id = @site_id,
                            MasterEntityId = @MasterEntityId,
                            LastUpdatedBy = @LastUpdatedBy,
                            LastUpdateDate = GETDATE(),
                            IsActive = @IsActive
                            WHERE ManagementFormId = @ManagementFormId
                            SELECT * FROM ManagementForms WHERE ManagementFormId = @ManagementFormId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(managementFormsDTO, userId, siteId).ToArray(), sqlTransaction);
                RefreshManagementFormsDTO(managementFormsDTO, dt, userId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while updating ManagementFormsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(managementFormsDTO);
            return managementFormsDTO;
        }

        /// <summary>
        /// Returns the List of ManagementFormsDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<ManagementFormsDTO> GetAllManagementFormsDTOList(List<KeyValuePair<ManagementFormsDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<ManagementFormsDTO> managementFormsDTOList = new List<ManagementFormsDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<ManagementFormsDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? "" : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == ManagementFormsDTO.SearchByParameters.MANAGEMENT_FORM_ID ||
                            searchParameter.Key == ManagementFormsDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ManagementFormsDTO.SearchByParameters.FUNCTION_GROUP
                                || searchParameter.Key == ManagementFormsDTO.SearchByParameters.GROUP_NAME
                                || searchParameter.Key == ManagementFormsDTO.SearchByParameters.FORM_NAME)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == ManagementFormsDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ManagementFormsDTO.SearchByParameters.ISACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1) =  " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
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
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ManagementFormsDTO managementFormsDTO = GetManagementFormsDTO(dataRow);
                    managementFormsDTOList.Add(managementFormsDTO);
                }
            }
            log.LogMethodExit(managementFormsDTOList);
            return managementFormsDTOList;
        }
        /// <summary>
        /// Inserts the managementFormAccess record to the database
        /// </summary>
        /// <param name="formName">string type object</param>
        /// <param name="functionalGuid">string type object</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void AddManagementFormAccess(string formName, string mainMenu, string functionGroup, string functionGuid, int siteId)
        {
            log.LogMethodEntry(formName, functionGuid, siteId);
            string query = @"exec InsertOrUpdateManagementFormAccess @mainMenu,@formName,@functionGroup,@siteId,@functionGuid";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@formName", formName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@mainMenu", mainMenu));
            parameters.Add(dataAccessHandler.GetSQLParameter("@functionGroup", functionGroup));
            parameters.Add(dataAccessHandler.GetSQLParameter("@functionGuid", functionGuid));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId));
            dataAccessHandler.executeScalar(query, parameters.ToArray(), sqlTransaction);
            log.LogMethodExit();
        }
        /// <summary>
        /// Rename the managementFormAccess record to the database
        /// </summary>
        /// <param name="newFormName">string type object</param>
        /// <param name="formName">string type object</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void RenameManagementFormAccess(string newFormName, string mainMenu, string formName, string functionGroup, int siteId, string functionGuid)
        {
            log.LogMethodEntry(newFormName, formName, siteId);
            string query = @"exec RenameManagementFormAccess @newFormName,'@mainMenu',@formName,@functionGroup,@siteId,@functionGuid";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@newFormName", newFormName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@formName", formName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@mainMenu", mainMenu));
            parameters.Add(dataAccessHandler.GetSQLParameter("@functionGroup", functionGroup));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@functionGuid", functionGuid));
            dataAccessHandler.executeScalar(query, parameters.ToArray(), sqlTransaction);
            log.LogMethodExit();
        }
        /// <summary>
        /// Update the managementFormAccess record to the database
        /// </summary>
        /// <param name="formName">string type object</param>
        /// <param name="mainMenu">string type object</param>
        /// <param name="functionGroup">string type object</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="updatedIsActive">Site to which the record belongs</param>
        /// <param name="functionGuid">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void UpdateManagementFormAccess(string formName, string mainMenu, string functionGroup, int siteId, bool updatedIsActive, string functionGuid)
        {
            log.LogMethodEntry(formName, mainMenu, functionGroup, siteId, updatedIsActive, functionGuid);
            string query = @"exec InsertOrUpdateManagementFormAccess @mainMenu,@formName,@functionGroup,@siteId,@functionGuid,@updatedIsActive";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@formName", formName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@mainMenu", mainMenu));
            parameters.Add(dataAccessHandler.GetSQLParameter("@functionGroup", functionGroup));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@updatedIsActive", updatedIsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@functionGuid", functionGuid));
            dataAccessHandler.executeScalar(query, parameters.ToArray(), sqlTransaction);
            log.LogMethodExit();
        }
    }
}
