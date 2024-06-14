/********************************************************************************************
* Project Name - CMS MenuItem Data Handler 
* Description  - Data handler of the CMSMenuItem  DataHandler  class
* 
**************
**Version Log
**************
*Version     Date          Modified By      Remarks          
*********************************************************************************************
*1.00        06-Apr-2016   Rakshith          Created 
*2.70.2      11-Dec-2019   Jinto Thomas      Removed siteid from update query
*2.70.3      31-Mar-2020    Jeevan           Removed syncstatus from update query  
*2.80        25-May-2020   Indrajeet Kumar   Modified - As per 3 Tier Standard
*2.130.2     17-Feb-2022   Nitin Pai         CMS Changes for SmartFun
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Semnox.Parafait.WebCMS
{
    public class CMSMenuItemDataHandler
    {
        private DataAccessHandler dataAccessHandler;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction = null;
        private const string SELECT_QUERY = "SELECT *  FROM CMSMenuItems  AS cmi";
        ////<summary>
        ////For search parameter Specified
        ////</summary>
        private static readonly Dictionary<CMSMenuItemsDTO.SearchByRequestParameters, string> DBSearchParameters = new Dictionary<CMSMenuItemsDTO.SearchByRequestParameters, string>
        {
                {CMSMenuItemsDTO.SearchByRequestParameters.MENU_ID, "cmi.MenuId"},
                {CMSMenuItemsDTO.SearchByRequestParameters.ITEM_ID, "cmi.ItemId"},
                {CMSMenuItemsDTO.SearchByRequestParameters.MASTER_ENTITY_ID, "cmi.MasterEntityId"},
                {CMSMenuItemsDTO.SearchByRequestParameters.ACTIVE, "cmi.Active"},
                {CMSMenuItemsDTO.SearchByRequestParameters.PARENT_ITEMID, "cmi.ParentItemId"},
                {CMSMenuItemsDTO.SearchByRequestParameters.IS_HEADER, "cmi.Isheader"},
                {CMSMenuItemsDTO.SearchByRequestParameters.MENU_ID_LIST, "cmi.MenuId"}
        };

        ////<summary>
        ////For Get All target type  
        ////</summary>
        public List<CMSMenuItemsDTO.TargetType> GetTargetType()
        {
            log.LogMethodEntry();
            try
            {
                List<CMSMenuItemsDTO.TargetType> targetTypeList = new List<CMSMenuItemsDTO.TargetType>();
                foreach (CMSMenuItemsDTO.TargetType targetType in Enum.GetValues(typeof(CMSMenuItemsDTO.TargetType)))
                {
                    targetTypeList.Add(targetType);
                }
                log.LogMethodExit(targetTypeList);
                return targetTypeList;
            }
            catch (Exception expn)
            {
                log.Error("Error occurred at  GetTargetType()", expn);
                log.LogMethodExit(null, "Throwing exception - " + expn.Message);
                throw;
            }
        }

        /// <summary>
        ///  constructor of  CMS MenuItem DataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public CMSMenuItemDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the record from the database based on  menuId
        /// </summary>
        /// <returns>return the int </returns>
        public int menuItemDelete(int itemId)
        {
            log.LogMethodEntry(itemId);
            try
            {
                string menuItemQuery = @"delete  from CMSMenuItems
                                         where ItemId = @itemId";

                SqlParameter[] menuItemParameters = new SqlParameter[1];
                menuItemParameters[0] = new SqlParameter("@itemId", itemId);
                int deleteStatus = dataAccessHandler.executeUpdateQuery(menuItemQuery, menuItemParameters, sqlTransaction);
                log.LogMethodExit(deleteStatus);
                return deleteStatus;
            }
            catch (Exception expn)
            {
                log.Error("Error occurred while Deleting at method menuItemDelete(itemId)", expn);
                log.LogMethodExit(null, "Throwing exception - " + expn.Message);
                throw;
            }
        }


        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating CMSMenuItems Record.
        /// </summary>
        /// <param name="CMSMenuItemsDTO">CMSMenuItemsDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(CMSMenuItemsDTO cmsMenuItemsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(cmsMenuItemsDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@itemId", cmsMenuItemsDTO.ItemId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@menuId", cmsMenuItemsDTO.MenuId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@itemName", string.IsNullOrEmpty(cmsMenuItemsDTO.ItemName) ? DBNull.Value : (object)cmsMenuItemsDTO.ItemName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@itemUrl", string.IsNullOrEmpty(cmsMenuItemsDTO.ItemUrl) ? DBNull.Value : (object)cmsMenuItemsDTO.ItemUrl));
            parameters.Add(dataAccessHandler.GetSQLParameter("@target", string.IsNullOrEmpty(cmsMenuItemsDTO.Target) ? DBNull.Value : (object)cmsMenuItemsDTO.Target));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isHeader", cmsMenuItemsDTO.IsHeader));
            parameters.Add(dataAccessHandler.GetSQLParameter("@parentItemId", cmsMenuItemsDTO.ParentItemId == -1 ? DBNull.Value : (object)cmsMenuItemsDTO.ParentItemId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@displayOrder", cmsMenuItemsDTO.DisplayOrder == -1 ? DBNull.Value : (object)cmsMenuItemsDTO.DisplayOrder));
            parameters.Add(dataAccessHandler.GetSQLParameter("@active", cmsMenuItemsDTO.Active));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", cmsMenuItemsDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@height", cmsMenuItemsDTO.Height == null || cmsMenuItemsDTO.Height == -1 ? DBNull.Value : (object)cmsMenuItemsDTO.Height));
            parameters.Add(dataAccessHandler.GetSQLParameter("@width", cmsMenuItemsDTO.Width == null || cmsMenuItemsDTO.Width == -1 ? DBNull.Value : (object)cmsMenuItemsDTO.Width));
            parameters.Add(dataAccessHandler.GetSQLParameter("@displayName", string.IsNullOrEmpty(cmsMenuItemsDTO.DisplayName) ? DBNull.Value : (object)cmsMenuItemsDTO.DisplayName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@displayAttributes", string.IsNullOrEmpty(cmsMenuItemsDTO.DisplayAttributes) ? DBNull.Value : (object)cmsMenuItemsDTO.DisplayAttributes));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the  CMS MenuItem DataHandler  Items   record to the database
        /// </summary>
        /// <param name="cmsBannerDTO">  CMS MenuItem DataHandler type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public CMSMenuItemsDTO InsertMenuItems(CMSMenuItemsDTO cmsMenuItemDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(cmsMenuItemDTO, loginId, siteId);
            string insertMenuItemQuery = @"insert into CMSMenuItems 
                                                        (   
                                                          MenuId,
                                                          ItemName,
                                                          ItemUrl,
                                                          Target,
                                                          Active,
                                                          IsHeader,
                                                          ParentItemId,
                                                          DisplayOrder,
                                                          Guid,
                                                          site_id,
                                                          CreatedBy,
                                                          CreationDate,
                                                          LastUpdatedBy,
                                                          LastupdatedDate ,
                                                          MasterEntityId,
                                                          Height,
                                                          Width,
                                                          DisplayName,
                                                          DisplayAttributes
                                                        ) 
                                                values 
                                                        (
                                                           
                                                          @menuId,
                                                          @itemName,
                                                          @itemUrl,
                                                          @target,
                                                          @active,
                                                          @isHeader,
                                                          @parentItemId,
                                                          @displayOrder,
                                                          NEWID(),
                                                          @site_id, 
                                                          @createdBy,
                                                          Getdate(), 
                                                          @lastUpdatedBy,
                                                          GetDate()  ,
                                                          @masterEntityId,
                                                          @height,
                                                          @width,
                                                          @displayName,
                                                          @displayAttributes
                                                         ) SELECT * FROM CMSMenuItems WHERE itemId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertMenuItemQuery, GetSQLParameters(cmsMenuItemDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCMSMenuItemDTO(cmsMenuItemDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting cmsMenuItemDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(cmsMenuItemDTO);
            return cmsMenuItemDTO;
        }

        /// <summary>
        /// update the CMSMenuItemsDTO Items   record to the database
        /// </summary>
        /// <param name="cmsBannerDTO">CMSMenuItemsDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        public CMSMenuItemsDTO UpdateMenuItems(CMSMenuItemsDTO cmsMenuItemDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(cmsMenuItemDTO, loginId, siteId);
            string updateMenuItemsQuery = @"update CMSMenuItems 
                                                          set 
                                                          menuId= @MenuId,
                                                          itemName=@ItemName,
                                                          itemUrl= @ItemUrl,
                                                          target= @Target,
                                                          active=@Active,
                                                          isHeader=@IsHeader,
                                                          parentItemId=@parentItemId,
                                                          displayOrder=@DisplayOrder, 
                                                          -- site_id= @site_id,
                                                          LastUpdatedBy=@lastUpdatedBy,
                                                          LastupdatedDate=GetDate(),
                                                          Height = @height,
                                                          Width = @width,
                                                          DisplayName = @displayName,
                                                          DisplayAttributes = @displayAttributes
                                                          where itemId = @ItemId
                                            SELECT * FROM CMSMenuItems WHERE itemId = @ItemId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateMenuItemsQuery, GetSQLParameters(cmsMenuItemDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCMSMenuItemDTO(cmsMenuItemDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating cmsMenuItemDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(cmsMenuItemDTO);
            return cmsMenuItemDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="CMSMenuItemsDTO">CMSMenuItemsDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshCMSMenuItemDTO(CMSMenuItemsDTO cmsMenuItemsDTO, DataTable dt)
        {
            log.LogMethodEntry(cmsMenuItemsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                cmsMenuItemsDTO.ItemId = Convert.ToInt32(dt.Rows[0]["itemId"]);
                cmsMenuItemsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                cmsMenuItemsDTO.Site_id = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>s
        /// Data row as parameter
        /// Convert the datarow to CMSMenuItemsDTO object
        /// </summary>
        /// <returns>return the CMSMenuItemsDTO object</returns>
        private CMSMenuItemsDTO GetCmsMenuItemsDTO(DataRow cmsMenuItemRow)
        {
            log.LogMethodEntry(cmsMenuItemRow);
            CMSMenuItemsDTO cmsMenuItemDTO = new CMSMenuItemsDTO(
                                                   cmsMenuItemRow["ItemId"] == DBNull.Value ? -1 : Convert.ToInt32(cmsMenuItemRow["ItemId"]),
                                                   cmsMenuItemRow["MenuId"] == DBNull.Value ? -1 : Convert.ToInt32(cmsMenuItemRow["MenuId"]),
                                                   cmsMenuItemRow["ItemName"] == DBNull.Value ? string.Empty : Convert.ToString(cmsMenuItemRow["ItemName"]),
                                                   cmsMenuItemRow["ItemUrl"] == DBNull.Value ? string.Empty : Convert.ToString(cmsMenuItemRow["ItemUrl"]),
                                                   cmsMenuItemRow["Target"] == DBNull.Value ? string.Empty : Convert.ToString(cmsMenuItemRow["Target"]),
                                                   cmsMenuItemRow["Active"] == DBNull.Value ? false : Convert.ToBoolean(cmsMenuItemRow["Active"]),
                                                   cmsMenuItemRow["IsHeader"] == DBNull.Value ? false : Convert.ToBoolean(cmsMenuItemRow["IsHeader"]),
                                                   cmsMenuItemRow["ParentItemId"] == DBNull.Value ? -1 : Convert.ToInt32(cmsMenuItemRow["ParentItemId"]),
                                                   cmsMenuItemRow["DisplayOrder"] == DBNull.Value ? -1 : Convert.ToInt32(cmsMenuItemRow["DisplayOrder"]),
                                                   cmsMenuItemRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(cmsMenuItemRow["Guid"]),
                                                   cmsMenuItemRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(cmsMenuItemRow["SynchStatus"]),
                                                   cmsMenuItemRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(cmsMenuItemRow["site_id"]),
                                                   cmsMenuItemRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(cmsMenuItemRow["CreatedBy"]),
                                                   cmsMenuItemRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(cmsMenuItemRow["CreationDate"]),
                                                   cmsMenuItemRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(cmsMenuItemRow["LastUpdatedBy"]),
                                                   cmsMenuItemRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(cmsMenuItemRow["LastupdatedDate"]),
                                                   cmsMenuItemRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(cmsMenuItemRow["MasterEntityId"]),
                                                   cmsMenuItemRow["Height"] == DBNull.Value ? (int?)null : Convert.ToInt32(cmsMenuItemRow["Height"]),
                                                   cmsMenuItemRow["Width"] == DBNull.Value ? (int?)null : Convert.ToInt32(cmsMenuItemRow["Width"]),
                                                   cmsMenuItemRow["DisplayName"] == DBNull.Value ? string.Empty : Convert.ToString(cmsMenuItemRow["DisplayName"]),
                                                   cmsMenuItemRow["DisplayAttributes"] == DBNull.Value ? string.Empty : Convert.ToString(cmsMenuItemRow["DisplayAttributes"])
                                                );
            log.LogMethodExit(cmsMenuItemDTO);
            return cmsMenuItemDTO;
        }


        /// <summary>
        /// return the record from the database based on  bannerItemId
        /// </summary>
        /// <returns>return the CMSBannerItemsDTO object</returns>
        public CMSMenuItemsDTO GetCmsMenuItem(int menuItemId)
        {
            log.LogMethodEntry(menuItemId);
            string CmsMenuItemsRequestQuery = SELECT_QUERY + "  where  cmi.ItemId = @menuItemId";
            SqlParameter[] CmsMenuItemsParameters = new SqlParameter[1];
            CMSMenuItemsDTO cmsMenuItemsDTO = new CMSMenuItemsDTO();
            CmsMenuItemsParameters[0] = new SqlParameter("@menuItemId", menuItemId);
            DataTable CmsMenuItemsRequests = dataAccessHandler.executeSelectQuery(CmsMenuItemsRequestQuery, CmsMenuItemsParameters, sqlTransaction);
            if (CmsMenuItemsRequests.Rows.Count > 0)
            {
                DataRow CmsMenuItemsRequestRow = CmsMenuItemsRequests.Rows[0];
                cmsMenuItemsDTO = GetCmsMenuItemsDTO(CmsMenuItemsRequestRow);
            }
            log.LogMethodExit(cmsMenuItemsDTO);
            return cmsMenuItemsDTO;
        }

        /// <summary>
        /// Gets the GetMenuItemList matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of Generic CMSMenuItemsDTO matching the search criteria</returns>
        public List<CMSMenuItemsDTO> GetMenuItemsList(List<KeyValuePair<CMSMenuItemsDTO.SearchByRequestParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<CMSMenuItemsDTO> cmsMenuItemsDTOList = new List<CMSMenuItemsDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            int count = 0;
            string selectCmsMenuItemsQuery = SELECT_QUERY;

            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                StringBuilder query = new StringBuilder(" where ");
                string joiner = string.Empty;
                foreach (KeyValuePair<CMSMenuItemsDTO.SearchByRequestParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key == CMSMenuItemsDTO.SearchByRequestParameters.MENU_ID
                            || searchParameter.Key == CMSMenuItemsDTO.SearchByRequestParameters.ACTIVE
                            || searchParameter.Key == CMSMenuItemsDTO.SearchByRequestParameters.ITEM_ID
                            || searchParameter.Key == CMSMenuItemsDTO.SearchByRequestParameters.PARENT_ITEMID
                            || searchParameter.Key == CMSMenuItemsDTO.SearchByRequestParameters.MASTER_ENTITY_ID
                            || searchParameter.Key == CMSMenuItemsDTO.SearchByRequestParameters.IS_HEADER)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CMSMenuItemsDTO.SearchByRequestParameters.MENU_ID_LIST)
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

                selectCmsMenuItemsQuery = selectCmsMenuItemsQuery + query + " order by DisplayOrder asc";
            }
            DataTable menuItemData = dataAccessHandler.executeSelectQuery(selectCmsMenuItemsQuery, parameters.ToArray(), sqlTransaction);
            if (menuItemData.Rows.Count > 0)
            {
                foreach (DataRow menuItemDataRow in menuItemData.Rows)
                {
                    CMSMenuItemsDTO menuItemObject = GetCmsMenuItemsDTO(menuItemDataRow);
                    cmsMenuItemsDTOList.Add(menuItemObject);
                }
            }
            log.LogMethodExit(cmsMenuItemsDTOList);
            return cmsMenuItemsDTOList;
        }

        /// <summary>
        /// Gets the GetMenuItemList matching the search key menu id
        /// </summary>
        /// <param name="menuid">  parameters is menuid</param>
        /// <returns>Returns the list of Generic List<CMSMenuItemsTree> </returns>
        public List<CMSMenuItemsTree> GetMenuItemsTree(int menuid, bool showActive)
        {
            log.LogMethodEntry(menuid, showActive);
            int count = 0;

            List<CMSMenuItemsTree> menuItemListTree = new List<CMSMenuItemsTree>();
            try
            {
                List<KeyValuePair<CMSMenuItemsDTO.SearchByRequestParameters, string>> mainMenuItemParameters = new List<KeyValuePair<CMSMenuItemsDTO.SearchByRequestParameters, string>>();
                mainMenuItemParameters.Add(new KeyValuePair<CMSMenuItemsDTO.SearchByRequestParameters, string>(CMSMenuItemsDTO.SearchByRequestParameters.MENU_ID, menuid.ToString()));
                if (showActive)
                {
                    mainMenuItemParameters.Add(new KeyValuePair<CMSMenuItemsDTO.SearchByRequestParameters, string>(CMSMenuItemsDTO.SearchByRequestParameters.ACTIVE, "1"));
                }
                mainMenuItemParameters.Add(new KeyValuePair<CMSMenuItemsDTO.SearchByRequestParameters, string>(CMSMenuItemsDTO.SearchByRequestParameters.IS_HEADER, "1"));
                mainMenuItemParameters.Add(new KeyValuePair<CMSMenuItemsDTO.SearchByRequestParameters, string>(CMSMenuItemsDTO.SearchByRequestParameters.PARENT_ITEMID, "0"));
                List<CMSMenuItemsDTO> mainMenuList = GetMenuItemsList(mainMenuItemParameters);
                if (mainMenuList.Count > 0)
                {
                    foreach (CMSMenuItemsDTO cmsMenuItemsDTO in mainMenuList)
                    {
                        if (cmsMenuItemsDTO.GetType() == typeof(CMSMenuItemsDTO))
                        {
                            List<KeyValuePair<CMSMenuItemsDTO.SearchByRequestParameters, string>> searchSubMenuItemParameters = new List<KeyValuePair<CMSMenuItemsDTO.SearchByRequestParameters, string>>();
                            if (showActive)
                            {
                                searchSubMenuItemParameters.Add(new KeyValuePair<CMSMenuItemsDTO.SearchByRequestParameters, string>(CMSMenuItemsDTO.SearchByRequestParameters.ACTIVE, "1"));
                            }
                            searchSubMenuItemParameters.Add(new KeyValuePair<CMSMenuItemsDTO.SearchByRequestParameters, string>(CMSMenuItemsDTO.SearchByRequestParameters.PARENT_ITEMID, cmsMenuItemsDTO.ItemId.ToString()));

                            List<CMSMenuItemsDTO> subMenuItemList = GetMenuItemsList(searchSubMenuItemParameters);
                            if (subMenuItemList != null)
                            {
                                count = subMenuItemList.Count;
                            }
                            else
                            {
                                count = 0;
                            }
                            menuItemListTree.Add(new CMSMenuItemsTree(cmsMenuItemsDTO, subMenuItemList, count));
                        }
                    }
                    log.LogMethodExit(menuItemListTree);
                    return menuItemListTree;
                }
                else
                {
                    List<CMSMenuItemsTree> cmsMenuItemsTreeList = new List<CMSMenuItemsTree>();
                    return cmsMenuItemsTreeList;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred at GetMenuItemsTree(int menuid, bool showActive) method ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw; ;
            }
        }

        /// <summary>
        /// Return  List<CMSMenuItemsTree> of header type of Data
        /// </summary>
        /// <returns>Returns the list of Generic List<CMSMenuItemsTree> </returns>
        public List<CMSMenuItemsTree> GetMenuListTreeType(string headerType)
        {
            try
            {
                string selectCmsMenuItemsQuery = " SELECT top 1 MenuId,Active FROM CMSMenus where    Active=@active  and Type=@type";
                SqlParameter[] CmsHeaderMenusParameters = new SqlParameter[2];
                CmsHeaderMenusParameters[0] = new SqlParameter("@active", 1);
                CmsHeaderMenusParameters[1] = new SqlParameter("@type", headerType);

                DataTable menuItemData = dataAccessHandler.executeSelectQuery(selectCmsMenuItemsQuery, CmsHeaderMenusParameters, sqlTransaction);

                List<CMSMenuItemsTree> cmsMenuItemsTreeList = new List<CMSMenuItemsTree>();
                if (menuItemData.Rows.Count > 0)
                {
                    foreach (DataRow menuItemDataRow in menuItemData.Rows)
                    {
                        cmsMenuItemsTreeList = GetMenuItemsTree(Convert.ToInt32(menuItemDataRow["MenuId"]), Convert.ToBoolean(menuItemDataRow["Active"]));
                    }
                }
                return cmsMenuItemsTreeList;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred at GetMenuListTreeType(string headerType) method ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
        }
    }
}