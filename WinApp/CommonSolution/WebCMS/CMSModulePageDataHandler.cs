/********************************************************************************************
* Project Name - CMSModulePage DTO Programs 
* Description  - Mapper between Module Page
* 
**************
**Version Log
**************
*Version     Date           Modified By        Remarks          
*********************************************************************************************
*2.80        06-May-2020    Indrajeet K        Created  
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.WebCMS
{
    public class CMSModulePageDataHandler
    {
        private DataAccessHandler dataAccessHandler;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction = null;
        private const string SELECT_QUERY = @"SELECT * from CMSModulePage AS cmp";
        private static readonly Dictionary<CMSModulePageDTO.SearchByRequestParameters, string> DBSearchParameters = new Dictionary<CMSModulePageDTO.SearchByRequestParameters, string>
            {
                {CMSModulePageDTO.SearchByRequestParameters.ID, "cmp.Id"},
                {CMSModulePageDTO.SearchByRequestParameters.MODULE_ID, "cmp.ModuleId"},
                {CMSModulePageDTO.SearchByRequestParameters.PAGE_ID, "cmp.PageId"},
                {CMSModulePageDTO.SearchByRequestParameters.SITE_ID, "cmp.site_id"},
                {CMSModulePageDTO.SearchByRequestParameters.MASTER_ENTITY_ID, "cmp.MasterEntityId"},
                {CMSModulePageDTO.SearchByRequestParameters.IS_ACTIVE, "cmp.IsActive"}
            };

        public CMSModulePageDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        public CMSModulePageDTO GetCMSModulePageDTO(int id)
        {
            log.LogMethodEntry(id);
            string CmsMenusRequestQuery = SELECT_QUERY + "  where cmp.Id = @Id";
            SqlParameter[] CmsMenusParameters = new SqlParameter[1];
            CMSModulePageDTO cmsModulePageDTO = new CMSModulePageDTO();
            CmsMenusParameters[0] = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(CmsMenusRequestQuery, CmsMenusParameters, sqlTransaction);

            if (dataTable.Rows.Count > 0)
            {
                DataRow dataRow = dataTable.Rows[0];
                cmsModulePageDTO = GetCMSModulePageDTO(dataRow);
            }
            log.LogMethodExit(cmsModulePageDTO);
            return cmsModulePageDTO;
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating cMSModulePage Record.
        /// </summary>
        /// <param name="cMSModulePageDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(CMSModulePageDTO cMSModulePageDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(cMSModulePageDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@id", cMSModulePageDTO.Id));
            parameters.Add(dataAccessHandler.GetSQLParameter("@moduleId", cMSModulePageDTO.ModuleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@pageId", cMSModulePageDTO.PageId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", cMSModulePageDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", cMSModulePageDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the InsertCMSModulePage Items record to the database
        /// </summary>
        /// <param name="cMSModulePageDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns>Return cMSModulePageDTO</returns>
        public CMSModulePageDTO InsertCMSModulePage(CMSModulePageDTO cMSModulePageDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(cMSModulePageDTO, loginId, siteId);
            string insertCMSModulePageQuery = @"insert into CMSModulePage 
                                                        (                                                            	
		                                                  ModuleId,
		                                                  PageId,
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
                                                          @pageId,
                                                          @site_id,
                                                          NEWID(),
                                                          @isActive,
                                                          @masterEntityId,
                                                          @createdBy,
                                                          Getdate(), 
                                                          @lastUpdatedBy,
                                                          GetDate()                                                          
                                                        ) SELECT * FROM CMSModulePage WHERE Id  = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertCMSModulePageQuery, GetSQLParameters(cMSModulePageDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCMSModulePageDTO(cMSModulePageDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting cMSModulePageDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(cMSModulePageDTO);
            return cMSModulePageDTO;
        }

        private void RefreshCMSModulePageDTO(CMSModulePageDTO cMSModulePageDTO, DataTable dt)
        {
            log.LogMethodEntry(cMSModulePageDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                cMSModulePageDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                cMSModulePageDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                cMSModulePageDTO.Site_id = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// update the cMSModulePageDTO record to the database
        /// </summary>
        /// <param name="cMSModulePageDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public CMSModulePageDTO UpdateCMSModulePage(CMSModulePageDTO cMSModulePageDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(cMSModulePageDTO, loginId, siteId);
            string updateCMSModulePageQuery = @"update CMSModulePage 
                                                          set 
                                                          ModuleId=@moduleId,
                                                          PageId=@pageId,
                                                          IsActive=@isActive,
                                                          LastUpdatedBy=@lastUpdatedBy,
                                                          LastupdatedDate=GetDate() 
                                                          where Id = @id
                                    SELECT * FROM CMSModulePage WHERE Id  = @id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateCMSModulePageQuery, GetSQLParameters(cMSModulePageDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCMSModulePageDTO(cMSModulePageDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating cMSModulePageDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(cMSModulePageDTO);
            return cMSModulePageDTO;
        }

        /// <summary>
        /// Converts the Data row object to CustomerFeedbackSurveyDTO class type
        /// </summary>
        /// <param name="custFeedbackSurveyDataRow">CustomerFeedbackSurvey DataRow</param>
        /// <returns>Returns CustomerFeedbackSurvey</returns>
        private CMSModulePageDTO GetCMSModulePageDTO(DataRow cmsModulePageDTODataRow)
        {
            log.LogMethodEntry(cmsModulePageDTODataRow);
            CMSModulePageDTO cmsModulePageDTODataObject = new CMSModulePageDTO(
                                            Convert.ToInt32(cmsModulePageDTODataRow["Id"]),
                                            cmsModulePageDTODataRow["ModuleId"] == DBNull.Value ? -1 : Convert.ToInt32(cmsModulePageDTODataRow["ModuleId"]),
                                            cmsModulePageDTODataRow["PageId"] == DBNull.Value ? -1 : Convert.ToInt32(cmsModulePageDTODataRow["PageId"]),
                                            cmsModulePageDTODataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(cmsModulePageDTODataRow["Site_id"]),
                                            cmsModulePageDTODataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(cmsModulePageDTODataRow["Guid"]),
                                            cmsModulePageDTODataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(cmsModulePageDTODataRow["SynchStatus"]),
                                            cmsModulePageDTODataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(cmsModulePageDTODataRow["MasterEntityId"]),
                                            cmsModulePageDTODataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(cmsModulePageDTODataRow["CreatedBy"]),
                                            cmsModulePageDTODataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(cmsModulePageDTODataRow["CreationDate"]),
                                            cmsModulePageDTODataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(cmsModulePageDTODataRow["LastUpdatedBy"]),
                                            cmsModulePageDTODataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(cmsModulePageDTODataRow["LastupdatedDate"]),
                                            cmsModulePageDTODataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(cmsModulePageDTODataRow["IsActive"])
                                            );
            log.LogMethodExit(cmsModulePageDTODataObject);
            return cmsModulePageDTODataObject;
        }

        /// <summary>
        /// Gets the CMSModulePageDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of CMSModulePageDTO matching the search criteria</returns>
        public List<CMSModulePageDTO> GetCMSModulePageDTOList(List<KeyValuePair<CMSModulePageDTO.SearchByRequestParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectCMSModulePageQuery = SELECT_QUERY;
            List<CMSModulePageDTO> cmsModulePageDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if (searchParameters != null)
            {
                StringBuilder query = new StringBuilder(" where ");
                string joiner = string.Empty;
                foreach (KeyValuePair<CMSModulePageDTO.SearchByRequestParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key == CMSModulePageDTO.SearchByRequestParameters.ID
                            || searchParameter.Key == CMSModulePageDTO.SearchByRequestParameters.MODULE_ID
                            || searchParameter.Key == CMSModulePageDTO.SearchByRequestParameters.PAGE_ID
                            || searchParameter.Key == CMSModulePageDTO.SearchByRequestParameters.MASTER_ENTITY_ID
                            || searchParameter.Key == CMSModulePageDTO.SearchByRequestParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CMSModulePageDTO.SearchByRequestParameters.SITE_ID)
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
                    selectCMSModulePageQuery = selectCMSModulePageQuery + query;
            }
            DataTable cmsModulePageData = dataAccessHandler.executeSelectQuery(selectCMSModulePageQuery, parameters.ToArray(), sqlTransaction);
            if (cmsModulePageData.Rows.Count > 0)
            {
                cmsModulePageDTOList = new List<CMSModulePageDTO>();
                foreach (DataRow cmsModulePageDataRow in cmsModulePageData.Rows)
                {
                    CMSModulePageDTO cMSModulePageDTO = GetCMSModulePageDTO(cmsModulePageDataRow);
                    cmsModulePageDTOList.Add(cMSModulePageDTO);
                }
            }
            log.LogMethodExit(cmsModulePageDTOList);
            return cmsModulePageDTOList;
        }
    }
}
