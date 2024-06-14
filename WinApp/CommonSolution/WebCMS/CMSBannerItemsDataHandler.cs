/********************************************************************************************
* Project Name - CMSBanner Items Data Handler
* Description  - Data handler of the CMS BannerItems Data Handler class
* 
**************
**Version Log
**************
*Version     Date          Modified By       Remarks          
*********************************************************************************************
*1.00        06-Apr-2016   Rakshith          Created 
*2.70.2        11-Dec-2019   Jinto Thomas      Removed siteid from update query
*2.70        09-Jul-2019   Girish Kundar    Modified : Changed the Structure of Data Handler.
*                                                      Fix for the SQL Injection Issue.
*2.70.3      31-Mar-2020    Jeevan            Removed syncstatus from update query      
*2.80        25-May-2020    Indrajeet Kumar   Added a searchparameter BANNER_ID_LIST
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.WebCMS
{
    /// <summary>
    /// CMSBanner Items DataHandler - Handles insert, update and select of  Banner type objects
    /// </summary>
    public class CMSBannerItemsDataHandler
    {
        private DataAccessHandler dataAccessHandler;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction = null;
        private const string SELECT_QUERY = "SELECT *  FROM CMSBannerItems  AS cbi";
        /// <summary>
        /// Default constructor of  CMSBanner Items   DataHandler class
        /// </summary>
        public CMSBannerItemsDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        ////<summary>
        ////For search parameter Specified
        ////</summary>
        private static readonly Dictionary<CMSBannerItemsDTO.SearchByRequestParameters, string> DBSearchParameters = new Dictionary<CMSBannerItemsDTO.SearchByRequestParameters, string>
        {
              {CMSBannerItemsDTO.SearchByRequestParameters.BANNER_ITEM_ID, "cbi.BannerItemId"},
              {CMSBannerItemsDTO.SearchByRequestParameters.BANNER_ID, "cbi.BannerId"},
              {CMSBannerItemsDTO.SearchByRequestParameters.MASTER_ENTITY_ID, "cbi.MasterEnityId"},
              {CMSBannerItemsDTO.SearchByRequestParameters.ACTIVE, "cbi.active"},
              {CMSBannerItemsDTO.SearchByRequestParameters.BANNER_ID_LIST, "cbi.BannerId"},
        };

        /// <summary>
        /// Delete the record from the database based on  bannerItemId
        /// </summary>
        /// <returns>return the int </returns>
        public int bannerItemDelete(int bannerItemId)
        {
            log.LogMethodEntry(bannerItemId);
            try
            {
                string bannerItemQuery = @"delete  
                                           from CMSBannerItems
                                           where BannerItemId = @bannerItemId";

                SqlParameter[] bannerItemParameters = new SqlParameter[1];
                bannerItemParameters[0] = new SqlParameter("@bannerItemId", bannerItemId);
                int deleteStatus = dataAccessHandler.executeUpdateQuery(bannerItemQuery, bannerItemParameters, sqlTransaction);
                return deleteStatus;
            }
            catch (Exception expn)
            {
                log.Error("Error occurred while Deleting at method bannerItemDelete(bannerItemId)", expn);
                log.LogMethodExit(null, "Throwing exception - " + expn.Message);
                throw;
            }
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating CMSBannerItems Record.
        /// </summary>
        /// <param name="CMSBannerItemsDTO">CMSBannerItemsDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(CMSBannerItemsDTO cmsBannerItemsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(cmsBannerItemsDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@BannerItemId", cmsBannerItemsDTO.BannerItemId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@bannerId", cmsBannerItemsDTO.BannerId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@description", string.IsNullOrEmpty(cmsBannerItemsDTO.Description) ? DBNull.Value : (object)cmsBannerItemsDTO.Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@showLink", cmsBannerItemsDTO.ShowLink));
            parameters.Add(dataAccessHandler.GetSQLParameter("@url", string.IsNullOrEmpty(cmsBannerItemsDTO.Url) ? string.Empty : (object)cmsBannerItemsDTO.Url));
            parameters.Add(dataAccessHandler.GetSQLParameter("@target", string.IsNullOrEmpty(cmsBannerItemsDTO.Target) ? DBNull.Value : (object)cmsBannerItemsDTO.Target));
            parameters.Add(dataAccessHandler.GetSQLParameter("@bannerImage", string.IsNullOrEmpty(cmsBannerItemsDTO.BannerImage) ? DBNull.Value : (object)cmsBannerItemsDTO.BannerImage));
            parameters.Add(dataAccessHandler.GetSQLParameter("@displayOrder", cmsBannerItemsDTO.DisplayOrder == -1 ? DBNull.Value : (object)cmsBannerItemsDTO.DisplayOrder));
            parameters.Add(dataAccessHandler.GetSQLParameter("@active", cmsBannerItemsDTO.Active));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", cmsBannerItemsDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the CMSBannerItemsDTO Items   record to the database
        /// </summary>
        /// <param name="cmsBannerDTO">CMSBannerItemsDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns CMSBannerItemsDTO</returns>
        public CMSBannerItemsDTO InsertBannerItems(CMSBannerItemsDTO cmsBannerItemsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(cmsBannerItemsDTO, loginId, siteId);
            string insertCmsBannerItemQuery = @"insert into CMSBannerItems 
                                                        (  
                                                          BannerId,
                                                          Description,
                                                          ShowLink,
                                                          url,
                                                          Target,
                                                          BannerImage,
                                                          DisplayOrder,
                                                          Guid,
                                                          site_id,
                                                          Active,
                                                          CreatedBy,
                                                          CreationDate,
                                                          LastUpdatedBy,
                                                          LastupdatedDate,
                                                          MasterEntityId
                                                        ) 
                                                values 
                                                        (
                                                          @bannerId,
                                                          @description,
                                                          @showLink,
                                                          @url,
                                                          @target,
                                                          @bannerImage,
                                                          @displayOrder,
                                                          NEWID(),
                                                          @site_id ,
                                                          @active,
                                                          @createdBy,
                                                          GETDATE(), 
                                                          @lastUpdatedBy,
                                                          GETDATE() ,
                                                          @masterEntityId
                                                         )SELECT * FROM CMSBannerItems WHERE BannerItemId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertCmsBannerItemQuery, GetSQLParameters(cmsBannerItemsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCMSBannerItemsDTO(cmsBannerItemsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting cmsBannerItemsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(cmsBannerItemsDTO);
            return cmsBannerItemsDTO;

        }

        /// <summary>
        /// update the CMSBannerItemsDTO Items   record to the database
        /// </summary>
        /// <param name="cmsBannerItemsDTO">CMSBannerItemsDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public CMSBannerItemsDTO UpdateBannerItems(CMSBannerItemsDTO cmsBannerItemsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(cmsBannerItemsDTO, loginId, siteId);
            string updateBannerQuery = @"update CMSBannerItems 
                                                          set 
                                                          BannerId=@BannerId,
                                                          Description= @Description,
                                                          ShowLink=@ShowLink,
                                                          Url= @Url,
                                                          Target= @target,
                                                          BannerImage=@bannerImage,
                                                          DisplayOrder=@displayOrder, 
                                                          --Site_id= @site_id,
                                                          Active=@active,
                                                          LastUpdatedBy=@lastUpdatedBy,
                                                          LastupdatedDate= GETDATE(), 
                                                          MasterEntityId = @masterEntityId
                                                          where BannerItemId = @BannerItemId
                                          SELECT * FROM CMSBannerItems WHERE BannerItemId =  @BannerItemId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateBannerQuery, GetSQLParameters(cmsBannerItemsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCMSBannerItemsDTO(cmsBannerItemsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting cmsBannerItemsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(cmsBannerItemsDTO);
            return cmsBannerItemsDTO;
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="CMSBannerItemsDTO">CMSBannerItemsDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshCMSBannerItemsDTO(CMSBannerItemsDTO cmsBannerItemsDTO, DataTable dt)
        {
            log.LogMethodEntry(cmsBannerItemsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                cmsBannerItemsDTO.BannerItemId = Convert.ToInt32(dt.Rows[0]["BannerItemId"]);
                cmsBannerItemsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                cmsBannerItemsDTO.Site_id = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// return the record from the database
        /// Convert the datarow to CMSBannerItemsDTO object
        /// </summary>
        /// <returns>return the CMSBannerItemsDTO object</returns>
        private CMSBannerItemsDTO GetCmsbannerItemsDTO(DataRow cmsBannerItemRow)
        {
            log.LogMethodEntry(cmsBannerItemRow);
            CMSBannerItemsDTO cmsBannerItemDTO = new CMSBannerItemsDTO(
                                                 cmsBannerItemRow["BannerItemId"] == DBNull.Value ? -1 : Convert.ToInt32(cmsBannerItemRow["BannerItemId"]),
                                                 cmsBannerItemRow["BannerId"] == DBNull.Value ? -1 : Convert.ToInt32(cmsBannerItemRow["BannerId"]),
                                                 cmsBannerItemRow["Description"] == DBNull.Value ? string.Empty : Convert.ToString(cmsBannerItemRow["Description"]),
                                                 cmsBannerItemRow["ShowLink"] == DBNull.Value ? false : Convert.ToBoolean(cmsBannerItemRow["showLink"]),
                                                 cmsBannerItemRow["Url"] == DBNull.Value ? string.Empty : Convert.ToString(cmsBannerItemRow["url"]),
                                                 cmsBannerItemRow["Target"] == DBNull.Value ? string.Empty : Convert.ToString(cmsBannerItemRow["Target"]),
                                                 cmsBannerItemRow["BannerImage"] == DBNull.Value ? string.Empty : Convert.ToString(cmsBannerItemRow["BannerImage"]),
                                                 cmsBannerItemRow["DisplayOrder"] == DBNull.Value ? -1 : Convert.ToInt32(cmsBannerItemRow["DisplayOrder"]),
                                                 cmsBannerItemRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(cmsBannerItemRow["Guid"]),
                                                 cmsBannerItemRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(cmsBannerItemRow["SynchStatus"]),
                                                 cmsBannerItemRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(cmsBannerItemRow["site_id"]),
                                                 cmsBannerItemRow["Active"] == DBNull.Value ? false : Convert.ToBoolean(cmsBannerItemRow["active"]),
                                                 cmsBannerItemRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(cmsBannerItemRow["CreatedBy"]),
                                                 cmsBannerItemRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(cmsBannerItemRow["CreationDate"]),
                                                 cmsBannerItemRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(cmsBannerItemRow["LastUpdatedBy"]),
                                                 cmsBannerItemRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(cmsBannerItemRow["LastupdatedDate"]),
                                                 cmsBannerItemRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(cmsBannerItemRow["MasterEntityId"])
                                                 );
            log.LogMethodExit(cmsBannerItemDTO);
            return cmsBannerItemDTO;
        }

        /// <summary>
        /// return the record from the database based on  bannerItemId
        /// </summary>
        /// <returns>return the CMSBannerItemsDTO object</returns>
        /// or null
        public CMSBannerItemsDTO GetCmsbannerItem(int bannerItemId)
        {
            log.LogMethodEntry(bannerItemId);
            string CmsBannerItemsRequestQuery = SELECT_QUERY + "   where bannerItemId = @BannerItemId ";
            CMSBannerItemsDTO result = null;
            SqlParameter[] CmsBannerItemsParameters = new SqlParameter[1];
            CmsBannerItemsParameters[0] = new SqlParameter("@BannerItemId", bannerItemId);
            DataTable CmsBannerItemsRequests = dataAccessHandler.executeSelectQuery(CmsBannerItemsRequestQuery, CmsBannerItemsParameters, sqlTransaction);
            if (CmsBannerItemsRequests.Rows.Count > 0)
            {
                DataRow CmsBannerItemsRequestRow = CmsBannerItemsRequests.Rows[0];
                result = GetCmsbannerItemsDTO(CmsBannerItemsRequestRow);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the CMSBannerItemsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of Generic CMSBannerItemsDTO matching the search criteria</returns>
        public List<CMSBannerItemsDTO> GetBannerItemsList(List<KeyValuePair<CMSBannerItemsDTO.SearchByRequestParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<CMSBannerItemsDTO> cmsBannerItemsDTOList = new List<CMSBannerItemsDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            int count = 0;
            string selectCmsBannerItemsQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                StringBuilder query = new StringBuilder(" where ");
                string joiner = string.Empty;
                foreach (KeyValuePair<CMSBannerItemsDTO.SearchByRequestParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key == CMSBannerItemsDTO.SearchByRequestParameters.BANNER_ID
                            || searchParameter.Key == CMSBannerItemsDTO.SearchByRequestParameters.BANNER_ITEM_ID
                            || searchParameter.Key == CMSBannerItemsDTO.SearchByRequestParameters.MASTER_ENTITY_ID
                            || searchParameter.Key == CMSBannerItemsDTO.SearchByRequestParameters.ACTIVE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CMSBannerItemsDTO.SearchByRequestParameters.BANNER_ID_LIST)
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

                selectCmsBannerItemsQuery = selectCmsBannerItemsQuery + query;
            }
            DataTable bannerItemData = dataAccessHandler.executeSelectQuery(selectCmsBannerItemsQuery, parameters.ToArray(), sqlTransaction);

            if (bannerItemData.Rows.Count > 0)
            {
                foreach (DataRow bannerItemDataRow in bannerItemData.Rows)
                {
                    CMSBannerItemsDTO BannerItemObject = GetCmsbannerItemsDTO(bannerItemDataRow);
                    cmsBannerItemsDTOList.Add(BannerItemObject);
                }
            }
            log.LogMethodExit(cmsBannerItemsDTOList);
            return cmsBannerItemsDTOList;
        }
    }
}
