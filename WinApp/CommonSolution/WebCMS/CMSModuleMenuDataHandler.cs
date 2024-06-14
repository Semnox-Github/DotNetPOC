/********************************************************************************************
* Project Name - CMSModuleMenuDataHandler
* Description  - Mapper between Module Menu 
* 
**************
**Version Log
**************
*Version     Date           Modified By        Remarks          
*********************************************************************************************
*2.80        05-May-2020    Indrajeet K        Created  
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.WebCMS
{
    public class CMSModuleMenuDataHandler
    {
        private DataAccessHandler dataAccessHandler;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction = null;
        private const string SELECT_QUERY = @"SELECT * from CMSModuleMenu AS cmm";
        private static readonly Dictionary<CMSModuleMenuDTO.SearchByRequestParameters, string> DBSearchParameters = new Dictionary<CMSModuleMenuDTO.SearchByRequestParameters, string>
            {
                {CMSModuleMenuDTO.SearchByRequestParameters.ID, "cmm.Id"},
                {CMSModuleMenuDTO.SearchByRequestParameters.MODULE_ID, "cmm.ModuleId"},
                {CMSModuleMenuDTO.SearchByRequestParameters.MENU_ID, "cmm.MenuId"},
                {CMSModuleMenuDTO.SearchByRequestParameters.SITE_ID, "cmm.site_id"},
                {CMSModuleMenuDTO.SearchByRequestParameters.MASTER_ENTITY_ID, "cmm.MasterEntityId"},
                {CMSModuleMenuDTO.SearchByRequestParameters.IS_ACTIVE, "cmm.IsActive"}
            };

        public CMSModuleMenuDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        public CMSModuleMenuDTO GetCMSModuleMenuDTO(int id)
        {
            log.LogMethodEntry(id);
            string CmsMenusRequestQuery = SELECT_QUERY + "  where cmm.Id = @Id";
            SqlParameter[] CmsMenusParameters = new SqlParameter[1];
            CMSModuleMenuDTO cmsModuleMenuDTO = new CMSModuleMenuDTO();
            CmsMenusParameters[0] = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(CmsMenusRequestQuery, CmsMenusParameters, sqlTransaction);

            if (dataTable.Rows.Count > 0)
            {
                DataRow dataRow = dataTable.Rows[0];
                cmsModuleMenuDTO = GetCMSModuleMenuDTO(dataRow);
            }
            log.LogMethodExit(cmsModuleMenuDTO);
            return cmsModuleMenuDTO;
        }
        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating cMSModuleMenu Record.
        /// </summary>
        /// <param name="cMSModuleMenuDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(CMSModuleMenuDTO cMSModuleMenuDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(cMSModuleMenuDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@id", cMSModuleMenuDTO.Id));
            parameters.Add(dataAccessHandler.GetSQLParameter("@moduleId", cMSModuleMenuDTO.ModuleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@menuId", cMSModuleMenuDTO.MenuId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", cMSModuleMenuDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", cMSModuleMenuDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the CMSModuleMenuDTO Items record to the database
        /// </summary>
        /// <param name="cMSModuleMenuDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns>Return cMSModuleMenuDTO</returns>
        public CMSModuleMenuDTO InsertCMSModuleMenu(CMSModuleMenuDTO cMSModuleMenuDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(cMSModuleMenuDTO, loginId, siteId);
            string insertCMSModuleMenuQuery = @"insert into CMSModuleMenu 
                                                        (                                                            	
		                                                  ModuleId,
		                                                  MenuId,
		                                                  site_id,
		                                                  Guid,		 
                                                          IsActive,
		                                                  MasterEntityId,
		                                                  CreatedBy,
		                                                  CreationDate,
		                                                  LastUpdatedBy,
		                                                  LastUpdatedDate                                                          
                                                        ) 
                                                values 
                                                        (
                                                          @moduleId,
                                                          @menuId,
                                                          @site_id,
                                                          NEWID(),   
                                                          @isActive,
                                                          @masterEntityId,
                                                          @createdBy,
                                                          Getdate(), 
                                                          @lastUpdatedBy,
                                                          GetDate()                                                          
                                                        ) SELECT * FROM CMSModuleMenu WHERE Id  = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertCMSModuleMenuQuery, GetSQLParameters(cMSModuleMenuDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCMSModuleMenuDTO(cMSModuleMenuDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting cMSModuleMenuDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(cMSModuleMenuDTO);
            return cMSModuleMenuDTO;
        }

        private void RefreshCMSModuleMenuDTO(CMSModuleMenuDTO cMSModuleMenuDTO, DataTable dt)
        {
            log.LogMethodEntry(cMSModuleMenuDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                cMSModuleMenuDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                cMSModuleMenuDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                cMSModuleMenuDTO.Site_id = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// update the cMSModuleMenuDTO record to the database
        /// </summary>
        /// <param name="cMSModuleMenuDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public CMSModuleMenuDTO UpdateCMSModuleMenu(CMSModuleMenuDTO cMSModuleMenuDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(cMSModuleMenuDTO, loginId, siteId);
            string updateCMSModuleMenuQuery = @"update CMSModuleMenu 
                                                          set 
                                                          ModuleId=@moduleId,
                                                          MenuId=@menuId,  
                                                          IsActive=@isActive,
                                                          LastUpdatedBy=@lastUpdatedBy,
                                                          LastupdatedDate=GetDate() 
                                                          where Id = @id
                                    SELECT * FROM CMSModuleMenu WHERE Id  = @id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateCMSModuleMenuQuery, GetSQLParameters(cMSModuleMenuDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCMSModuleMenuDTO(cMSModuleMenuDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating cMSModuleMenuDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(cMSModuleMenuDTO);
            return cMSModuleMenuDTO;
        }

        /// <summary>
        /// Converts the Data row object to CustomerFeedbackSurveyDTO class type
        /// </summary>
        /// <param name="custFeedbackSurveyDataRow">CustomerFeedbackSurvey DataRow</param>
        /// <returns>Returns CustomerFeedbackSurvey</returns>
        private CMSModuleMenuDTO GetCMSModuleMenuDTO(DataRow cmsModuleMenuDTODataRow)
        {
            log.LogMethodEntry(cmsModuleMenuDTODataRow);
            CMSModuleMenuDTO cmsModuleMenuDTODataObject = new CMSModuleMenuDTO(
                                            Convert.ToInt32(cmsModuleMenuDTODataRow["Id"]),
                                            cmsModuleMenuDTODataRow["ModuleId"] == DBNull.Value ? -1 : Convert.ToInt32(cmsModuleMenuDTODataRow["ModuleId"]),
                                            cmsModuleMenuDTODataRow["MenuId"] == DBNull.Value ? -1 : Convert.ToInt32(cmsModuleMenuDTODataRow["MenuId"]),
                                            cmsModuleMenuDTODataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(cmsModuleMenuDTODataRow["Site_id"]),
                                            cmsModuleMenuDTODataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(cmsModuleMenuDTODataRow["Guid"]),
                                            cmsModuleMenuDTODataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(cmsModuleMenuDTODataRow["SynchStatus"]),
                                            cmsModuleMenuDTODataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(cmsModuleMenuDTODataRow["MasterEntityId"]),
                                            cmsModuleMenuDTODataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(cmsModuleMenuDTODataRow["CreatedBy"]),
                                            cmsModuleMenuDTODataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(cmsModuleMenuDTODataRow["CreationDate"]),
                                            cmsModuleMenuDTODataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(cmsModuleMenuDTODataRow["LastUpdatedBy"]),
                                            cmsModuleMenuDTODataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(cmsModuleMenuDTODataRow["LastupdatedDate"]),
                                            cmsModuleMenuDTODataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(cmsModuleMenuDTODataRow["IsActive"])
                                            );
            log.LogMethodExit(cmsModuleMenuDTODataObject);
            return cmsModuleMenuDTODataObject;
        }

        /// <summary>
        /// Gets the CMSModuleMenuDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of CMSModuleMenuDTO matching the search criteria</returns>
        public List<CMSModuleMenuDTO> GetCMSModuleMenuDTOList(List<KeyValuePair<CMSModuleMenuDTO.SearchByRequestParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectCMSModuleMenuQuery = SELECT_QUERY;
            List<CMSModuleMenuDTO> cmsModuleMenuDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if (searchParameters != null)
            {
                StringBuilder query = new StringBuilder(" where ");
                string joiner = string.Empty;
                foreach (KeyValuePair<CMSModuleMenuDTO.SearchByRequestParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key == CMSModuleMenuDTO.SearchByRequestParameters.ID
                            || searchParameter.Key == CMSModuleMenuDTO.SearchByRequestParameters.MODULE_ID
                            || searchParameter.Key == CMSModuleMenuDTO.SearchByRequestParameters.MENU_ID
                            || searchParameter.Key == CMSModuleMenuDTO.SearchByRequestParameters.MASTER_ENTITY_ID
                            || searchParameter.Key == CMSModuleMenuDTO.SearchByRequestParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CMSModuleMenuDTO.SearchByRequestParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'~') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
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
                if (searchParameters.Count > 0)
                    selectCMSModuleMenuQuery = selectCMSModuleMenuQuery + query;
            }
            DataTable cmsModuleMenuData = dataAccessHandler.executeSelectQuery(selectCMSModuleMenuQuery, parameters.ToArray(), sqlTransaction);
            if (cmsModuleMenuData.Rows.Count > 0)
            {
                cmsModuleMenuDTOList = new List<CMSModuleMenuDTO>();
                foreach (DataRow cmsModuleMenuDataRow in cmsModuleMenuData.Rows)
                {
                    CMSModuleMenuDTO cMSModuleMenuDTO = GetCMSModuleMenuDTO(cmsModuleMenuDataRow);
                    cmsModuleMenuDTOList.Add(cMSModuleMenuDTO);
                }
            }
            log.LogMethodExit(cmsModuleMenuDTOList);
            return cmsModuleMenuDTOList;
        }
    }
}
