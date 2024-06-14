/********************************************************************************************
* Project Name - CMSContent Data Handler 
* Description  - Data handler of the CMSContent DataHandler class
* 
**************
**Version Log
**************
*Version     Date          Modified By    Remarks          
*********************************************************************************************
*1.00        06-Apr-2016   Rakshith       Created 
*2.70        09-Jul-2019   Girish Kundar  Modified : Changed the Structure of Data Handler.
*                                                      Fix for the SQL Injection Issue.
*2.70.2       11-Dec-2019   Jinto Thomas   Removed siteid from update query
*2.70.3       31-Mar-2020   Jeevan        Removed syncstatus from update query
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
    public class CMSContentDataHandler
    {
        private DataAccessHandler dataAccessHandler;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction = null;
        private const string SELECT_QUERY = "SELECT *  FROM CMSContents  AS cc";
        ////<summary>
        ////For search parameter Specified
        ////</summary>
        private static readonly Dictionary<CMSContentDTO.SearchByRequestParameters, string> DBSearchParameters = new Dictionary<CMSContentDTO.SearchByRequestParameters, string>
        {
              {CMSContentDTO.SearchByRequestParameters.CONTENT_ID, "cc.ContentId"},
              {CMSContentDTO.SearchByRequestParameters.MASTER_ENTITY_ID, "cc.MasterEntityId"},
              {CMSContentDTO.SearchByRequestParameters.PAGE_ID, "cc.PageId"},
              {CMSContentDTO.SearchByRequestParameters.ACTIVE, "cc.Active"},
        };

        /// <summary>
        ///  CMSContentDataHandler constructor with SqlTransaction object as parameter.
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public CMSContentDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the record from the database based on  contentId
        /// </summary>
        /// <returns>return the int </returns>
        public int contentDelete(int contentId)
        {
            log.LogMethodEntry(contentId);
            try
            {
                string contentQuery = @"delete  
                                        from CMSContents
                                        where ContentId = @contentId";

                SqlParameter[] contentarameters = new SqlParameter[1];
                contentarameters[0] = new SqlParameter("@contentId", contentId);
                int deleteStatus = dataAccessHandler.executeUpdateQuery(contentQuery, contentarameters, sqlTransaction);
                log.LogMethodExit(deleteStatus);
                return deleteStatus;
            }
            catch (Exception expn)
            {
                log.Error("Error occurred while Deleting at method contentDelete(contentId)", expn);
                log.LogMethodExit(null, "Throwing exception - " + expn.Message);
                throw;
            }
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating CMSContent Record.
        /// </summary>
        /// <param name="CMSContentDTO">CMSContentDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(CMSContentDTO cmsContentDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(cmsContentDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@contentId", cmsContentDTO.ContentId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@pageId", cmsContentDTO.PageId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@source", string.IsNullOrEmpty(cmsContentDTO.Source) ? DBNull.Value : (object)cmsContentDTO.Source));
            parameters.Add(dataAccessHandler.GetSQLParameter("@displaySection", string.IsNullOrEmpty(cmsContentDTO.DisplaySection) ? DBNull.Value : (object)cmsContentDTO.DisplaySection));
            parameters.Add(dataAccessHandler.GetSQLParameter("@contentTemplateId", cmsContentDTO.ContentTemplateId == -1 ? DBNull.Value : (object)cmsContentDTO.ContentTemplateId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@contentName", string.IsNullOrEmpty(cmsContentDTO.ContentName) ? DBNull.Value : (object)cmsContentDTO.ContentName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@displayOrder", cmsContentDTO.DisplayOrder == -1 ? DBNull.Value : (object)cmsContentDTO.DisplayOrder));
            parameters.Add(dataAccessHandler.GetSQLParameter("@contentKey", string.IsNullOrEmpty(cmsContentDTO.ContentKey) ? DBNull.Value : (object)cmsContentDTO.ContentKey));
            parameters.Add(dataAccessHandler.GetSQLParameter("@sectionName", string.IsNullOrEmpty(cmsContentDTO.SectionName) ? DBNull.Value : (object)cmsContentDTO.SectionName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@active", cmsContentDTO.Active));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", cmsContentDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@parentContentId", cmsContentDTO.ParentContentId == -1 ? DBNull.Value : (object)cmsContentDTO.ParentContentId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@height", cmsContentDTO.Height == null || cmsContentDTO.Height == -1 ? DBNull.Value : (object)cmsContentDTO.Height));
            parameters.Add(dataAccessHandler.GetSQLParameter("@width", cmsContentDTO.Width == null || cmsContentDTO.Width == -1 ? DBNull.Value : (object)cmsContentDTO.Width));
            parameters.Add(dataAccessHandler.GetSQLParameter("@contentURL", string.IsNullOrEmpty(cmsContentDTO.ContentURL) ? DBNull.Value : (object)cmsContentDTO.ContentURL));
            parameters.Add(dataAccessHandler.GetSQLParameter("@displayAttributes", string.IsNullOrEmpty(cmsContentDTO.DisplayAttributes) ? DBNull.Value : (object)cmsContentDTO.DisplayAttributes));
            log.LogMethodExit(parameters);
            return parameters;
        }




        /// <summary>
        /// Inserts the CMSContent Items   record to the database
        /// </summary>
        /// <param name="cmsBannerDTO">CMSContent type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns CMSContentDTO</returns>
        public CMSContentDTO InsertCmsContent(CMSContentDTO cmsContentDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(cmsContentDTO, loginId, siteId);
            string insertCmsContentQuery = @"insert into CMSContents
                                                        (                                                 
                                                          PageId,
                                                          Source,
                                                          DisplaySection,
                                                          ContentTemplateId,
                                                          ContentName,
                                                          DisplayOrder,
                                                          ContentKey,  
                                                          Active,
                                                          Guid,
                                                          site_id,
                                                          CreatedBy,
                                                          CreationDate,
                                                          LastUpdatedBy,
                                                          LastupdatedDate,
                                                          SectionName ,
                                                          MasterEntityId,
                                                          ParentContentId,
                                                          Height,
                                                          Width,
                                                          ContentURL,
                                                          DisplayAttributes
                                                        ) 
                                                values 
                                                        (
                                                          @pageId,
                                                          @source,
                                                          @displaySection,  
                                                          @contentTemplateId, 
                                                          @contentName,
                                                          @displayOrder, 
                                                          @contentKey,
                                                          @active,   
                                                          NEWID(),
                                                          @site_id,
                                                          @createdBy,
                                                          Getdate(), 
                                                          @lastUpdatedBy,
                                                          GetDate() ,
                                                          @sectionName ,
                                                          @masterEntityId,
                                                          @parentContentId,
                                                          @height,
                                                          @width,
                                                          @contentURL,
                                                          @displayAttributes
                                                         )
                                     SELECT * FROM CMSContents WHERE ContentId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertCmsContentQuery, GetSQLParameters(cmsContentDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCMSContentDTO(cmsContentDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting cmsContentDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(cmsContentDTO);
            return cmsContentDTO;

        }

        /// <summary>
        /// update the CMSContentDTO record to the database
        /// </summary>
        /// <param name="cmsBannerDTO">CMSContentDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns CMSContentDTO</returns>
        public CMSContentDTO UpdatecmsContent(CMSContentDTO cmsContentDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(cmsContentDTO, loginId, siteId);
            string updateCmsContentQuery = @"update CMSContents 
                                                          set 
                                                          PageId= @pageId,
                                                          Source=@source,
                                                          DisplaySection=@displaySection,
                                                          ContentTemplateId=@contentTemplateId,
                                                          ContentName=@contentName,
                                                          ContentKey=@contentKey,
                                                          DisplayOrder=@displayOrder,
                                                          Active = @active,  
                                                          -- Site_id= @site_id,
                                                          LastUpdatedBy=@lastUpdatedBy,
                                                          LastupdatedDate= GetDate() ,
                                                          SectionName  = @sectionName,
                                                          MasterEntityId = @masterEntityId,
                                                          ParentContentId = @parentContentId,
                                                          Height = @height,
                                                          Width = @width,
                                                          ContentURL = @contentURL,
                                                          DisplayAttributes = @displayAttributes
                                                          where ContentId = @contentId
                                            SELECT* FROM CMSContents WHERE ContentId =  @contentId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateCmsContentQuery, GetSQLParameters(cmsContentDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCMSContentDTO(cmsContentDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating cmsContentDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(cmsContentDTO);
            return cmsContentDTO;

        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="CMSContentDTO">CMSContentDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshCMSContentDTO(CMSContentDTO cmsContentDTO, DataTable dt)
        {
            log.LogMethodEntry(cmsContentDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                cmsContentDTO.ContentId = Convert.ToInt32(dt.Rows[0]["ContentId"]);
                cmsContentDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                cmsContentDTO.Site_id = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Data row as parameter
        /// Convert the datarow to CMSContentDTO object
        /// </summary>
        /// <returns>return the CMSContentDTO object</returns>
        private CMSContentDTO GetCmsContentDTO(DataRow cmsContentItemRow)
        {
            log.LogMethodEntry(cmsContentItemRow);
            CMSContentDTO cmsContentDTO = new CMSContentDTO(
                                                  cmsContentItemRow["ContentId"] == DBNull.Value ? -1 : Convert.ToInt32(cmsContentItemRow["contentId"]),
                                                  cmsContentItemRow["PageId"] == DBNull.Value ? -1 : Convert.ToInt32(cmsContentItemRow["pageId"]),
                                                  cmsContentItemRow["Source"] == DBNull.Value ? string.Empty : Convert.ToString(cmsContentItemRow["source"]),
                                                  cmsContentItemRow["DisplaySection"] == DBNull.Value ? string.Empty : cmsContentItemRow["DisplaySection"].ToString(),
                                                  cmsContentItemRow["DisplayOrder"] == DBNull.Value ? -1 : Convert.ToInt32(cmsContentItemRow["DisplayOrder"]),
                                                  cmsContentItemRow["ContentName"] == DBNull.Value ? string.Empty : cmsContentItemRow["ContentName"].ToString(),
                                                  cmsContentItemRow["ContentKey"] == DBNull.Value ? string.Empty : cmsContentItemRow["ContentKey"].ToString(),
                                                  cmsContentItemRow["ContentTemplateId"] == DBNull.Value ? -1 : Convert.ToInt32(cmsContentItemRow["ContentTemplateId"]),
                                                  cmsContentItemRow["Active"] == DBNull.Value ? false : Convert.ToBoolean(cmsContentItemRow["active"]),
                                                  cmsContentItemRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(cmsContentItemRow["guid"]),
                                                  cmsContentItemRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(cmsContentItemRow["synchStatus"]),
                                                  cmsContentItemRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(cmsContentItemRow["site_id"]),
                                                  cmsContentItemRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(cmsContentItemRow["SectionName"]),
                                                  cmsContentItemRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(cmsContentItemRow["CreationDate"]),
                                                  cmsContentItemRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(cmsContentItemRow["LastUpdatedBy"]),
                                                  cmsContentItemRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(cmsContentItemRow["LastupdatedDate"]),
                                                  cmsContentItemRow["SectionName"] == DBNull.Value ? string.Empty : Convert.ToString(cmsContentItemRow["SectionName"]),
                                                  cmsContentItemRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(cmsContentItemRow["MasterEntityId"]),
                                                  cmsContentItemRow["ParentContentId"] == DBNull.Value ? -1 : Convert.ToInt32(cmsContentItemRow["ParentContentId"]),
                                                  cmsContentItemRow["Height"] == DBNull.Value ? (int?)null : Convert.ToInt32(cmsContentItemRow["Height"]),
                                                  cmsContentItemRow["Width"] == DBNull.Value ? (int?)null : Convert.ToInt32(cmsContentItemRow["Width"]),
                                                  cmsContentItemRow["ContentURL"] == DBNull.Value ? string.Empty : cmsContentItemRow["ContentURL"].ToString(),
                                                  cmsContentItemRow["DisplayAttributes"] == DBNull.Value ? string.Empty : cmsContentItemRow["DisplayAttributes"].ToString()
                                            );
            log.LogMethodExit(cmsContentDTO);
            return cmsContentDTO;

        }

        /// <summary>
        /// return the record from the database based on  contentId
        /// </summary>
        /// <returns>return the CMSContentDTO object</returns>
        /// or null
        public CMSContentDTO GetCmsContents(int contentId)
        {
            string CmsContentRequestQuery = SELECT_QUERY + "  where ContentId = @contentId";
            SqlParameter[] CmsContentsParameters = new SqlParameter[1];
            CMSContentDTO cmsContentDTO = new CMSContentDTO();
            CmsContentsParameters[0] = new SqlParameter("@contentId", contentId);
            DataTable CmsContentsRequests = dataAccessHandler.executeSelectQuery(CmsContentRequestQuery, CmsContentsParameters, sqlTransaction);

            if (CmsContentsRequests.Rows.Count > 0)
            {
                DataRow CmsContentsRequestRow = CmsContentsRequests.Rows[0];
                cmsContentDTO = GetCmsContentDTO(CmsContentsRequestRow);
            }
            log.LogMethodExit(cmsContentDTO);
            return cmsContentDTO;
        }

        /// <summary>
        /// Gets the CMSContentDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of Generic CMSContentDTO matching the search criteria</returns>
        public List<CMSContentDTO> GetContentsList(List<KeyValuePair<CMSContentDTO.SearchByRequestParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<CMSContentDTO> cmsContentDTOList = new List<CMSContentDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            int count = 0;
            string selectCmsContentQuery = SELECT_QUERY;

            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<CMSContentDTO.SearchByRequestParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        string joinOperartor = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key.Equals(CMSContentDTO.SearchByRequestParameters.CONTENT_ID)
                            || searchParameter.Key.Equals(CMSContentDTO.SearchByRequestParameters.PAGE_ID)
                            || searchParameter.Key.Equals(CMSContentDTO.SearchByRequestParameters.MASTER_ENTITY_ID))

                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(CMSContentDTO.SearchByRequestParameters.ACTIVE))
                        {
                            query.Append(joinOperartor + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0) =  " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else
                        {
                            query.Append("Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
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
                selectCmsContentQuery = selectCmsContentQuery + query + " Order by DisplayOrder ";
            }
            DataTable ContentData = dataAccessHandler.executeSelectQuery(selectCmsContentQuery, parameters.ToArray(), sqlTransaction);
            if (ContentData.Rows.Count > 0)
            {
                foreach (DataRow ContentDataRow in ContentData.Rows)
                {
                    CMSContentDTO ContentDataObject = GetCmsContentDTO(ContentDataRow);
                    cmsContentDTOList.Add(ContentDataObject);
                }
            }
            log.LogMethodExit(cmsContentDTOList);
            return cmsContentDTOList;
        }
    }
}
