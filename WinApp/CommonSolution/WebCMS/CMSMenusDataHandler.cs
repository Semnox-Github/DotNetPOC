/********************************************************************************************
* Project Name - CMSMenus Data Handler  
* Description  - Data handler of the CMSMenus  DataHandler class
* 
**************
**Version Log
**************
*Version     Date          Modified By       Remarks          
*********************************************************************************************
*1.00        06-Apr-2016   Rakshith          Created
*2.70.2      11-Dec-2019   Jinto Thomas      Removed siteid from update query
*2.70.3      31-Mar-2020   Jeevan            Removed syncstatus from update query  
*2.80        25-May-2020   Indrajeet Kumar   Modified - Added a SearchParamter MENU_ID_LIST 
*2.130.2     17-Feb-2022   Nitin Pai         CMS Changes for SmartFun
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.WebCMS
{
    public class CMSMenusDataHandler
    {

        private DataAccessHandler dataAccessHandler;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction = null;
        private const string SELECT_QUERY = "SELECT *  FROM CMSMenus  AS cm";
        ////<summary>
        ////For search parameter Specified
        ////</summary>
        private static readonly Dictionary<CMSMenusDTO.SearchByRequestParameters, string> DBSearchParameters = new Dictionary<CMSMenusDTO.SearchByRequestParameters, string>
        {
             {CMSMenusDTO.SearchByRequestParameters.MENU_ID, "cm.MenuId"},
             {CMSMenusDTO.SearchByRequestParameters.MASTER_ENTITY_ID, "cm.MenuId"},
             {CMSMenusDTO.SearchByRequestParameters.ACTIVE, "cm.Active"},
             {CMSMenusDTO.SearchByRequestParameters.MENU_ID_LIST, "cm.MenuId"},
        };

        /// <summary>
        ///  constructor of  CMS CMSMenusDataHandler  class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public CMSMenusDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating CMSMenus Record.
        /// </summary>
        /// <param name="CMSMenusDTO">CMSMenusDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(CMSMenusDTO cmsMenusDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(cmsMenusDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@menuId", cmsMenusDTO.MenuId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@name", string.IsNullOrEmpty(cmsMenusDTO.Name) ? DBNull.Value : (object)cmsMenusDTO.Name));
            parameters.Add(dataAccessHandler.GetSQLParameter("@type", string.IsNullOrEmpty(cmsMenusDTO.Type) ? DBNull.Value : (object)cmsMenusDTO.Type));
            parameters.Add(dataAccessHandler.GetSQLParameter("@active", cmsMenusDTO.Active));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", cmsMenusDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@height", cmsMenusDTO.Height == null || cmsMenusDTO.Height == -1 ? DBNull.Value : (object)cmsMenusDTO.Height));
            parameters.Add(dataAccessHandler.GetSQLParameter("@width", cmsMenusDTO.Width == null || cmsMenusDTO.Width == -1 ? DBNull.Value : (object)cmsMenusDTO.Width));
            parameters.Add(dataAccessHandler.GetSQLParameter("@displayAttributes", string.IsNullOrEmpty(cmsMenusDTO.DisplayAttributes) ? DBNull.Value : (object)cmsMenusDTO.DisplayAttributes));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the CMSMenusDTO Items   record to the database
        /// </summary>
        /// <param name="cmsBannerDTO">CMSMenusDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns CMSMenusDTO</returns>
        public CMSMenusDTO InsertMenus(CMSMenusDTO cmsMenusDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(cmsMenusDTO, loginId, siteId);
            string insertCmsMenusQuery = @"insert into CMSMenus 
                                                        (                                                 
                                                          Name,
                                                          Active,
                                                          Guid,
                                                          site_id,
                                                          Type,
                                                          CreatedBy,
                                                          CreationDate,
                                                          LastUpdatedBy,
                                                          LastupdatedDate,
                                                          MasterEntityId,
                                                          Height,
                                                          Width,
                                                          DisplayAttributes
                                                        ) 
                                                values 
                                                        (
                                                          @name,
                                                          @active,
                                                          NEWID(),
                                                          @site_id,
                                                          @type,
                                                          @createdBy,
                                                          Getdate(), 
                                                          @lastUpdatedBy,
                                                          GetDate(),
                                                          @masterEntityId,
                                                          @height,
                                                          @width,
                                                          @displayAttributes
                                                        ) SELECT * FROM CMSMenus WHERE MenuId  = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertCmsMenusQuery, GetSQLParameters(cmsMenusDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCMSMenusDTO(cmsMenusDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting cmsMenusDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(cmsMenusDTO);
            return cmsMenusDTO;
        }

        /// <summary>
        /// update the CMSMenusDTO record to the database
        /// </summary>
        /// <param name="cmsBannerDTO">CMSMenusDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns CMSMenusDTO</returns>
        public CMSMenusDTO Updatemenus(CMSMenusDTO cmsMenusDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(cmsMenusDTO, loginId, siteId);
            string updateMenusQuery = @"update CMSmenus 
                                                          set 
                                                          Name=@name,
                                                          Active= @active, 
                                                          -- Site_id=@site_id,
                                                          Type=@type,
                                                          LastUpdatedBy=@lastUpdatedBy,
                                                          LastupdatedDate=GetDate(),
                                                          Height = @height,
                                                          Width = @width,
                                                          DisplayAttributes = @displayAttributes
                                                          where MenuId = @menuId
                                    SELECT * FROM CMSMenus WHERE MenuId  = @menuId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateMenusQuery, GetSQLParameters(cmsMenusDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCMSMenusDTO(cmsMenusDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating cmsMenusDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(cmsMenusDTO);
            return cmsMenusDTO;


        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="CMSMenusDTO">CMSMenusDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshCMSMenusDTO(CMSMenusDTO cmsMenusDTO, DataTable dt)
        {
            log.LogMethodEntry(cmsMenusDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                cmsMenusDTO.MenuId = Convert.ToInt32(dt.Rows[0]["MenuId"]);
                cmsMenusDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                cmsMenusDTO.Site_id = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Data row as parameter
        /// Convert the datarow to CMSMenusDTO object
        /// </summary>
        /// <returns>return the CMSMenusDTO object</returns>
        private CMSMenusDTO GetcmsMenusDTO(DataRow cmsMenusRow)
        {
            log.LogMethodEntry(cmsMenusRow);
            CMSMenusDTO cmsMenusDTO = new CMSMenusDTO(
                                                    cmsMenusRow["MenuId"] == DBNull.Value ? -1 : Convert.ToInt32(cmsMenusRow["MenuId"]),
                                                    cmsMenusRow["Name"] == DBNull.Value ? string.Empty : Convert.ToString(cmsMenusRow["Name"]),
                                                    cmsMenusRow["Active"] == DBNull.Value ? false : Convert.ToBoolean(cmsMenusRow["Active"]),
                                                    cmsMenusRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(cmsMenusRow["Guid"]),
                                                    cmsMenusRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(cmsMenusRow["SynchStatus"]),
                                                    cmsMenusRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(cmsMenusRow["site_id"]),
                                                    cmsMenusRow["Type"] == DBNull.Value ? string.Empty : Convert.ToString(cmsMenusRow["Type"]),
                                                    cmsMenusRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(cmsMenusRow["CreatedBy"]),
                                                    cmsMenusRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(cmsMenusRow["CreationDate"]),
                                                    cmsMenusRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(cmsMenusRow["LastUpdatedBy"]),
                                                    cmsMenusRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(cmsMenusRow["LastupdatedDate"]),
                                                    cmsMenusRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(cmsMenusRow["MasterEntityId"]),
                                                    cmsMenusRow["Height"] == DBNull.Value ? (int?)null : Convert.ToInt32(cmsMenusRow["Height"]),
                                                    cmsMenusRow["Width"] == DBNull.Value ? (int?)null : Convert.ToInt32(cmsMenusRow["Width"]),
                                                    cmsMenusRow["DisplayAttributes"] == DBNull.Value ? string.Empty : Convert.ToString(cmsMenusRow["DisplayAttributes"])
                                                    );
            log.LogMethodExit(cmsMenusDTO);
            return cmsMenusDTO;
        }

        /// <summary>
        /// return the record from the database based on  menuId
        /// </summary>
        /// <returns>return the CMSMenusDTO object</returns>
        /// or null 
        public CMSMenusDTO GetcmsMenu(int menuId)
        {
            log.LogMethodEntry(menuId);
            string CmsMenusRequestQuery = SELECT_QUERY + "  where MenuId = @menuId";
            SqlParameter[] CmsMenusParameters = new SqlParameter[1];
            CMSMenusDTO cmsMenusDTO = new CMSMenusDTO();
            CmsMenusParameters[0] = new SqlParameter("@menuId", menuId);
            DataTable CmsBannerItemsRequests = dataAccessHandler.executeSelectQuery(CmsMenusRequestQuery, CmsMenusParameters, sqlTransaction);

            if (CmsBannerItemsRequests.Rows.Count > 0)
            {
                DataRow CmsMenusRequestRow = CmsBannerItemsRequests.Rows[0];
                cmsMenusDTO = GetcmsMenusDTO(CmsMenusRequestRow);
            }
            log.LogMethodExit(cmsMenusDTO);
            return cmsMenusDTO;
        }

        /// <summary>
        /// Delete the CMSMenusDTO based on Id
        /// </summary>
        public int menuDelete(int menuId)
        {
            log.LogMethodEntry(menuId);
            try
            {
                string CmsMenuQuery = @"delete  
                                        from CMSMenus
                                        where MenuId = @menuid";

                SqlParameter[] menuParameters = new SqlParameter[1];
                menuParameters[0] = new SqlParameter("@menuId", menuId);
                int deleteStatus = dataAccessHandler.executeUpdateQuery(CmsMenuQuery, menuParameters, sqlTransaction);
                log.LogMethodExit();
                return deleteStatus;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while deleting cmsMenus", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the GetMenuList list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of Generic GetMenuList sDTO matching the search criteria</returns>
        public List<CMSMenusDTO> GetMenusList(List<KeyValuePair<CMSMenusDTO.SearchByRequestParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<CMSMenusDTO> MenusList = new List<CMSMenusDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            int count = 0;

            string selectCmsMenuQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                StringBuilder query = new StringBuilder(" where ");
                string joiner = string.Empty;
                foreach (KeyValuePair<CMSMenusDTO.SearchByRequestParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key == CMSMenusDTO.SearchByRequestParameters.MENU_ID || searchParameter.Key == CMSMenusDTO.SearchByRequestParameters.ACTIVE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CMSMenusDTO.SearchByRequestParameters.MENU_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " in (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
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
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                }
                selectCmsMenuQuery = selectCmsMenuQuery + query;
            }
            DataTable menuData = dataAccessHandler.executeSelectQuery(selectCmsMenuQuery, parameters.ToArray(), sqlTransaction);

            if (menuData.Rows.Count > 0)
            {
                foreach (DataRow menuDataRow in menuData.Rows)
                {
                    CMSMenusDTO MenuItemObject = GetcmsMenusDTO(menuDataRow);
                    MenusList.Add(MenuItemObject);
                }
            }
            log.LogMethodExit(MenusList);
            return MenusList;
        }
    }
}
