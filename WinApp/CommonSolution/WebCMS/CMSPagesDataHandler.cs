/********************************************************************************************
 * Project Name - CMSPages    Data Handler
 * Description  - Data handler of the CMSPages DataHandler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *1.00        06-Apr-2016   Rakshith          Created
 *2.70.2      11-Dec-2019   Jinto Thomas      Removed siteid from update query
 *2.70.3      31-Mar-2020   Jeevan            Removed syncstatus from update query  
 *2.80        08-May-2020   Indrajeet Kumar   Modified : Insert & Update Query add BannerId
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.WebCMS
{
    public class CMSPagesDataHandler
    {
        private DataAccessHandler dataAccessHandler;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string SELECT_QUERY = "SELECT *  FROM CMSPages  AS cp";
        private SqlTransaction sqlTransaction = null;
        ////<summary>
        ////For search parameter Specified
        ////</summary>
        private static readonly Dictionary<CMSPagesDTO.SearchByRequestParameters, string> DBSearchParameters = new Dictionary<CMSPagesDTO.SearchByRequestParameters, string>
            {
                {CMSPagesDTO.SearchByRequestParameters.PAGE_ID,"cp.PageId"},
                {CMSPagesDTO.SearchByRequestParameters.ACTIVE, "cp.Active"},
                {CMSPagesDTO.SearchByRequestParameters.MASTER_ENTITY_ID, "cp.MasterEntityId"},
                {CMSPagesDTO.SearchByRequestParameters.PAGE_NAME, "cp.PageName"},
                {CMSPagesDTO.SearchByRequestParameters.BANNER_ID, "cp.BannerId"},
                {CMSPagesDTO.SearchByRequestParameters.PAGE_ID_LIST, "cp.PageId"}
            };

        /// <summary>
        ///  constructor of  CMS CMSPagesDataHandler  class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public CMSPagesDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating CMSPages Record.
        /// </summary>
        /// <param name="CMSPagesDTO">CMSPagesDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(CMSPagesDTO cmsPagesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(cmsPagesDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@pageId", cmsPagesDTO.PageId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@groupId", cmsPagesDTO.GroupId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@pageName", string.IsNullOrEmpty(cmsPagesDTO.PageName) ? DBNull.Value : (object)cmsPagesDTO.PageName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@title", string.IsNullOrEmpty(cmsPagesDTO.Title) ? DBNull.Value : (object)cmsPagesDTO.Title));
            parameters.Add(dataAccessHandler.GetSQLParameter("@contentId", cmsPagesDTO.ContentId == -1 ? DBNull.Value : (object)cmsPagesDTO.ContentId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@metaTitle", string.IsNullOrEmpty(cmsPagesDTO.MetaTitle) ? DBNull.Value : (object)cmsPagesDTO.MetaTitle));
            parameters.Add(dataAccessHandler.GetSQLParameter("@metaKeywords", string.IsNullOrEmpty(cmsPagesDTO.MetaKeywords) ? DBNull.Value : (object)cmsPagesDTO.MetaKeywords));
            parameters.Add(dataAccessHandler.GetSQLParameter("@metaDesc", string.IsNullOrEmpty(cmsPagesDTO.MetaDesc) ? DBNull.Value : (object)cmsPagesDTO.MetaDesc));
            parameters.Add(dataAccessHandler.GetSQLParameter("@active", cmsPagesDTO.Active));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", cmsPagesDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@bannerId", cmsPagesDTO.BannerId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the CMSPages Data Handler Items record to the database
        /// </summary>
        /// <param name="cmsBannerDTO">CMSPages Data Handler  type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns CMSPagesDTO</returns>
        public CMSPagesDTO InsertPages(CMSPagesDTO cmsPagesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(cmsPagesDTO, loginId, siteId);
            string insertPagesQuery = @"insert into CMSPages
                                                        (                                                 
                                                          PageName,
                                                          Title,
                                                          ContentId,
                                                          Guid,
                                                          site_id,
                                                          MetaTitle,
                                                          MetaKeywords,
                                                          MetaDesc ,
                                                          GroupId,  
                                                          Active,
                                                          CreatedBy,
                                                          CreationDate,
                                                          LastUpdatedBy,
                                                          LastupdatedDate,
                                                          MasterEntityId,
                                                          BannerId
                                                         ) 
                                                values 
                                                        (
                                                          @pageName,
                                                          @title,
                                                          @contentId,
                                                          NEWID(),
                                                          @site_id,
                                                          @metaTitle,
                                                          @metaKeywords,
                                                          @metaDesc,
                                                          @groupId,
                                                          @active,
                                                          @createdBy,
                                                          Getdate(), 
                                                          @lastUpdatedBy,
                                                          GetDate() ,
                                                          @masterEntityId,
                                                          @bannerId
                                                         ) SELECT * FROM CMSPages WHERE PageId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertPagesQuery, GetSQLParameters(cmsPagesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCMSPagesDTO(cmsPagesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting cmsPagesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(cmsPagesDTO);
            return cmsPagesDTO;
        }

        /// <summary>
        /// Updates the CMSPages Data Handler  Items   record to the database
        /// </summary>
        /// <param name="cmsBannerDTO">CMSPages Data Handler  type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns CMSPagesDTO</returns>
        public CMSPagesDTO UpdatePages(CMSPagesDTO cmsPagesDTO, string loginId, int siteId)
        {
            log.Debug("Begins-UpdatePages(CMSPagesDTO, loginId, siteId) Method.");
            string updatePagesQuery = @"update CMSPages 
                                                          set 
                                                          PageName=@pageName,
                                                          Title= @title,
                                                          MetaTitle=@metaTitle,
                                                          MetaKeywords= @metaKeywords, 
                                                          MetaDesc=@metaDesc,
                                                          GroupId=@groupId,
                                                          Active=@active,
                                                          LastUpdatedBy=@lastUpdatedBy,
                                                          LastupdatedDate=GetDate(),
                                                          MasterEntityId = @masterEntityId,
                                                          BannerId = @bannerId
                                                          where PageId = @pageId
                                                 SELECT * FROM CMSPages WHERE PageId = @pageId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updatePagesQuery, GetSQLParameters(cmsPagesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCMSPagesDTO(cmsPagesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating cmsPagesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(cmsPagesDTO);
            return cmsPagesDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="cmsPagesDTO">CMSPagesDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshCMSPagesDTO(CMSPagesDTO cmsPagesDTO, DataTable dt)
        {
            log.LogMethodEntry(cmsPagesDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                cmsPagesDTO.PageId = Convert.ToInt32(dt.Rows[0]["PageId"]);
                cmsPagesDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                cmsPagesDTO.Site_id = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// return the record from the database
        /// Convert the datarow to CMSPagesDTO object
        /// </summary>
        /// <returns>return the CMSPagesDTO object</returns>
        private CMSPagesDTO GetPagesDTO(DataRow cmsPagesRow)
        {
            log.LogMethodEntry(cmsPagesRow);
            CMSPagesDTO cmsPagesDTO = new CMSPagesDTO(
                                              cmsPagesRow["PageId"] == DBNull.Value ? -1 : Convert.ToInt32(cmsPagesRow["PageId"]),
                                              cmsPagesRow["pageName"] == DBNull.Value ? string.Empty : Convert.ToString(cmsPagesRow["pageName"]),
                                              cmsPagesRow["Title"] == DBNull.Value ? string.Empty : Convert.ToString(cmsPagesRow["Title"]),
                                              cmsPagesRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(cmsPagesRow["Guid"]),
                                              cmsPagesRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(cmsPagesRow["SynchStatus"]),
                                              cmsPagesRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(cmsPagesRow["site_id"]),
                                              cmsPagesRow["MetaTitle"] == DBNull.Value ? string.Empty : Convert.ToString(cmsPagesRow["MetaTitle"]),
                                              cmsPagesRow["MetaKeywords"] == DBNull.Value ? string.Empty : Convert.ToString(cmsPagesRow["MetaKeywords"]),
                                              cmsPagesRow["MetaDesc"] == DBNull.Value ? string.Empty : Convert.ToString(cmsPagesRow["MetaDesc"]),
                                              cmsPagesRow["GroupId"] == DBNull.Value ? -1 : Convert.ToInt32(cmsPagesRow["GroupId"]),
                                              cmsPagesRow["Active"] == DBNull.Value ? false : Convert.ToBoolean(cmsPagesRow["Active"]),
                                              cmsPagesRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(cmsPagesRow["CreatedBy"]),
                                              cmsPagesRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(cmsPagesRow["CreationDate"]),
                                              cmsPagesRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(cmsPagesRow["LastUpdatedBy"]),
                                              cmsPagesRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(cmsPagesRow["LastupdatedDate"]),
                                              cmsPagesRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(cmsPagesRow["MasterEntityId"]),
                                              cmsPagesRow["BannerId"] == DBNull.Value ? -1 : Convert.ToInt32(cmsPagesRow["BannerId"])
                                            );
            log.LogMethodExit(cmsPagesDTO);
            return cmsPagesDTO;
        }

        /// <summary>
        /// return the record from the database based on  pageId
        /// </summary>
        /// <returns>return the CMSPagesDTO object</returns>
        /// or empty object
        public CMSPagesDTO GetcmsPages(int pageId)
        {
            log.LogMethodEntry(pageId);
            string CmsPagesRequestQuery = SELECT_QUERY + "     where cp.PageId = @pageId";
            SqlParameter[] CmsPagesParameters = new SqlParameter[1];
            CMSPagesDTO cmsPagesDTO = new CMSPagesDTO();
            CmsPagesParameters[0] = new SqlParameter("@pageId", pageId);
            DataTable CmsPagesRequests = dataAccessHandler.executeSelectQuery(CmsPagesRequestQuery, CmsPagesParameters, sqlTransaction);

            if (CmsPagesRequests.Rows.Count > 0)
            {
                DataRow CmsPagesRequestRow = CmsPagesRequests.Rows[0];
                cmsPagesDTO = GetPagesDTO(CmsPagesRequestRow);
            }
            log.LogMethodExit(cmsPagesDTO);
            return cmsPagesDTO;
        }

        /// <summary>
        /// Delete the record from the database based on  pageId
        /// </summary>
        /// <returns>return the int </returns>
        public int cmsPageDelete(int pageId)
        {
            try
            {
                string CmsPageQuery = @"delete from CMSPages where PageId = @pageId";
                SqlParameter[] CmsPageParameters = new SqlParameter[1];
                CmsPageParameters[0] = new SqlParameter("@pageId", pageId);
                int id = dataAccessHandler.executeUpdateQuery(CmsPageQuery, CmsPageParameters, sqlTransaction);
                log.LogMethodExit(id);
                return id;
            }
            catch (Exception expn)
            {
                log.Error("Error occurred while Deleting at method cmsPageDelete(pageId)", expn);
                log.LogMethodExit(null, "Throwing exception - " + expn.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the CMSPagesDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of Generic CMSPagesDTO matching the search criteria</returns>
        public List<CMSPagesDTO> GetPagesList(List<KeyValuePair<CMSPagesDTO.SearchByRequestParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<CMSPagesDTO> pagesList = new List<CMSPagesDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            int count = 0;
            string selectCmsPagesQuery = SELECT_QUERY;

            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<CMSPagesDTO.SearchByRequestParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        string joinOperartor = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key.Equals(CMSPagesDTO.SearchByRequestParameters.PAGE_ID)
                        || searchParameter.Key.Equals(CMSPagesDTO.SearchByRequestParameters.BANNER_ID)
                        || searchParameter.Key.Equals(CMSPagesDTO.SearchByRequestParameters.MASTER_ENTITY_ID))
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(CMSPagesDTO.SearchByRequestParameters.ACTIVE))
                        {
                            query.Append(joinOperartor + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0) =  " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == CMSPagesDTO.SearchByRequestParameters.PAGE_ID_LIST)
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + " in (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key.Equals(CMSPagesDTO.SearchByRequestParameters.PAGE_NAME))
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else
                        {
                            query.Append(joinOperartor + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
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
                selectCmsPagesQuery = selectCmsPagesQuery + query;
            }
            DataTable PagesData = dataAccessHandler.executeSelectQuery(selectCmsPagesQuery, parameters.ToArray(), sqlTransaction);

            if (PagesData.Rows.Count > 0)
            {
                foreach (DataRow PagesDataRow in PagesData.Rows)
                {
                    CMSPagesDTO cmsPagesDTO = GetPagesDTO(PagesDataRow);
                    pagesList.Add(cmsPagesDTO);
                }
            }
            log.LogMethodExit(pagesList);
            return pagesList;
        }

        /// <summary>
        /// List the CMSPages Data Items records
        /// </summary>
        /// <param name="pageid">CMSPages pageid</param>
        /// <returns>Returns List<CMSPagesDTO>  Object</returns>
        public CMSPagesDTOTree GetCMSPages(CMSPageParams cMSPageParams)
        {
            log.LogMethodEntry(cMSPageParams);
            // Get page details
            try
            {
                if (!String.IsNullOrEmpty(cMSPageParams.PageName))
                {
                    //Initialize  CMSPagesDTOTree with cMSPagesDTO object

                    List<KeyValuePair<CMSPagesDTO.SearchByRequestParameters, string>> searchParametersPage = new List<KeyValuePair<CMSPagesDTO.SearchByRequestParameters, string>>();
                    searchParametersPage.Add(new KeyValuePair<CMSPagesDTO.SearchByRequestParameters, string>(CMSPagesDTO.SearchByRequestParameters.PAGE_NAME, cMSPageParams.PageName));

                    CMSPagesDTO cMSPagesDTO = new CMSPagesDTO();
                    List<CMSPagesDTO> cMSPagesDTOList = new CMSPagesDataHandler().GetPagesList(searchParametersPage);
                    List<CMSContentDTO> cMSContentDTOList = new List<CMSContentDTO>();
                    if (cMSPagesDTOList.Count > 0)
                    {
                        cMSPagesDTO = cMSPagesDTOList[0];
                        if (cMSPageParams.ShowContents)
                        {
                            List<KeyValuePair<CMSContentDTO.SearchByRequestParameters, string>> searchParameters = new List<KeyValuePair<CMSContentDTO.SearchByRequestParameters, string>>();
                            searchParameters.Add(new KeyValuePair<CMSContentDTO.SearchByRequestParameters, string>(CMSContentDTO.SearchByRequestParameters.PAGE_ID, cMSPagesDTO.PageId.ToString()));
                            cMSContentDTOList = new CMSContentDataHandler().GetContentsList(searchParameters);

                            if (cMSContentDTOList != null && cMSContentDTOList.Count > 0)
                            {
                                foreach (var cmsContent in cMSContentDTOList)
                                {
                                    if (cmsContent.ContentTemplateId != -1)
                                    {
                                        cmsContent.CMSContentTemplateDTO = new CMSContentTemplateDataHandler(null).GetCMSContentTemplateDTO(cmsContent.ContentTemplateId);
                                    }
                                }
                            }

                        }
                    }

                    CMSPagesDTOTree cmsPagesDTOTree = new CMSPagesDTOTree(cMSPagesDTO, cMSContentDTOList);

                    // get all header data
                    CMSMenuItemDataHandler cmsMenuItemDataHandler = new CMSMenuItemDataHandler();
                    List<CMSMenuItemsTree> cmsMenuHeaderItemsTreeList = cmsMenuItemDataHandler.GetMenuListTreeType("header");
                    cmsPagesDTOTree.MenuHeader = cmsMenuHeaderItemsTreeList;

                    //  Get All Footer details  
                    List<CMSMenuItemsTree> cmsMenuFooterItemsTreeList = cmsMenuItemDataHandler.GetMenuListTreeType("footer");
                    cmsPagesDTOTree.MenuFooter = cmsMenuFooterItemsTreeList;

                    return cmsPagesDTOTree;
                }
                else
                {
                    CMSPagesDTOTree cmsPagesDTOTree = new CMSPagesDTOTree();
                    return cmsPagesDTOTree;
                }
            }
            catch (Exception expn)
            {
                log.Error("Error occurred at method GetCMSPages(CMSPageParams cMSPageParams)", expn);
                log.LogMethodExit(null, "Throwing exception - " + expn.Message);
                throw;
            }
        }
    }
}
