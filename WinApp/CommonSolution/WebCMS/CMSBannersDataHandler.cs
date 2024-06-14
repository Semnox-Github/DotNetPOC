/********************************************************************************************
 * Project Name - CMSBannerItems Data Handler
 * Description  - Data handler of the CMSBanner Items Data Handler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 *1.00        06-Apr-2016   Rakshith         Created 
 *2.70.2      11-Dec-2019   Jinto Thomas     Removed siteid from update query
 *2.70        09-Jul-2019   Girish Kundar    Modified : Changed the Structure of Data Handler.
 *                                                      Fix for the SQL Injection Issue.
 *2.70.3      31-Mar-2020   Jeevan           Removed syncstatus from update query      
 *2.80        08-May-2020   Indrajeet Kumar  Modified - Insert and Update Query Added - Frequency
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.WebCMS
{
    public class CMSBannersDataHandler
    {
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction = null;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string SELECT_QUERY = "SELECT *  FROM CMSBanners  AS cb";
        ////<summary>
        ////For search parameter Specified
        ////</summary>
        private static readonly Dictionary<CMSBannersDTO.SearchByRequestParameters, string> DBSearchParameters = new Dictionary<CMSBannersDTO.SearchByRequestParameters, string>
            {
                  {CMSBannersDTO.SearchByRequestParameters.BANNER_ID, "cb.BannerId"},
                  {CMSBannersDTO.SearchByRequestParameters.MASTER_ENTITY_ID, "cb.MasterEntityId"},
                  {CMSBannersDTO.SearchByRequestParameters.ACTIVE, "cb.active"},
            };

        /// <summary>
        /// Parametrized constructor of CMSBannerItems class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public CMSBannersDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the record from the database based on  bannerId
        /// </summary>
        /// <returns>return the int </returns>
        public int bannerDelete(int bannerId)
        {
            log.LogMethodEntry(bannerId);
            try
            {
                string bannerQuery = @"delete  
                                       from CMSBanners
                                       where BannerId = @bannerId";
                SqlParameter[] bannerParameters = new SqlParameter[1];
                bannerParameters[0] = new SqlParameter("@bannerId", bannerId);
                int deleteStatus = dataAccessHandler.executeUpdateQuery(bannerQuery, bannerParameters, sqlTransaction);
                log.LogMethodExit(deleteStatus);
                return deleteStatus;
            }
            catch (Exception expn)
            {
                log.Error("Error occurred while Deleting at method bannerDelete(bannerId)", expn);
                log.LogMethodExit(null, "Throwing exception - " + expn.Message);
                throw;
            }
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating CMSBanners Record.
        /// </summary>
        /// <param name="CMSBannersDTO">CMSBannersDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(CMSBannersDTO cmsBannersDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(cmsBannersDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@bannerId", cmsBannersDTO.BannerId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@name", string.IsNullOrEmpty(cmsBannersDTO.Name) ? DBNull.Value : (object)cmsBannersDTO.Name));
            parameters.Add(dataAccessHandler.GetSQLParameter("@active", cmsBannersDTO.Active));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", cmsBannersDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@frequency", cmsBannersDTO.Frequency));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the CMSBannerDTO Items   record to the database
        /// </summary>
        /// <param name="cmsBannerDTO">CMSBannerItemsDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns CMSBannersDTO</returns>

        public CMSBannersDTO InsertBanner(CMSBannersDTO cmsBannerDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(cmsBannerDTO, loginId, siteId);

            string insertCmsBannerQuery = @"insert into CMSBanners
                                                        (                                                 
                                                         Name,
                                                         Active,
                                                         Guid,
                                                         site_id,
                                                         CreatedBy,
                                                         CreationDate,
                                                         LastUpdatedBy,
                                                         LastupdatedDate,
                                                         MasterEntityId,
                                                         Frequency
                                                        ) 
                                                values 
                                                        (
                                                          @name,
                                                          @active,
                                                          NEWID(),
                                                          @site_id,
                                                          @createdBy,
                                                          Getdate(), 
                                                          @lastUpdatedBy,
                                                          GetDate() ,
                                                          @masterEntityId,
                                                          @frequency
                                                         )SELECT * FROM CMSBanners WHERE BannerId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertCmsBannerQuery, GetSQLParameters(cmsBannerDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCMSBannerDTO(cmsBannerDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting CMSBannerDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(cmsBannerDTO);
            return cmsBannerDTO;
        }

        /// <summary>
        /// update the CMSBannersDTO Items   record to the database
        /// </summary>
        /// <param name="cmsBannerDTO">CMSBannersDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns CMSBannersDTO/returns>
        public CMSBannersDTO UpdateBanner(CMSBannersDTO cmsBannerDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(cmsBannerDTO, loginId, siteId);

            string updateCmsBannerQuery = @"update CMSBanners 
                                                          set 
                                                          Name= @name,
                                                          Active=@active, 
                                                          -- Site_id= @site_id ,
                                                          LastUpdatedBy=@lastUpdatedBy,
                                                          LastupdatedDate=GetDate() ,
                                                          MasterEntityId = @masterEntityId,
                                                          Frequency = @frequency
                                                          where BannerId = @bannerId
                                                SELECT* FROM CMSBanners WHERE BannerId = @bannerId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateCmsBannerQuery, GetSQLParameters(cmsBannerDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCMSBannerDTO(cmsBannerDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating CMSBannerDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(cmsBannerDTO);
            return cmsBannerDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="cmsBannersDTO">CMSBannersDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshCMSBannerDTO(CMSBannersDTO cmsBannersDTO, DataTable dt)
        {
            log.LogMethodEntry(cmsBannersDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                cmsBannersDTO.BannerId = Convert.ToInt32(dt.Rows[0]["BannerId"]);
                cmsBannersDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                cmsBannersDTO.Site_id = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// return the record from the database
        /// Convert the data Row to CMSBannersDTO object
        /// </summary>
        /// <returns>return the CMSBannersDTO object</returns>
        public CMSBannersDTO GetCmsbannerDTO(DataRow cmsBannerRow)
        {
            log.LogMethodEntry(cmsBannerRow);
            CMSBannersDTO cmsBannersDTO = new CMSBannersDTO(
                                              cmsBannerRow["BannerId"] == DBNull.Value ? -1 : Convert.ToInt32(cmsBannerRow["BannerId"]),
                                              cmsBannerRow["Name"] == DBNull.Value ? string.Empty : Convert.ToString(cmsBannerRow["Name"]),
                                              cmsBannerRow["Active"] == DBNull.Value ? false : Convert.ToBoolean(cmsBannerRow["Active"]),
                                              cmsBannerRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(cmsBannerRow["Name"]),
                                              cmsBannerRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(cmsBannerRow["SynchStatus"]),
                                              cmsBannerRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(cmsBannerRow["site_id"]),
                                              cmsBannerRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(cmsBannerRow["CreatedBy"]),
                                              cmsBannerRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(cmsBannerRow["CreationDate"]),
                                              cmsBannerRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(cmsBannerRow["LastUpdatedBy"]),
                                              cmsBannerRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(cmsBannerRow["LastupdatedDate"]),
                                              cmsBannerRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(cmsBannerRow["MasterEntityId"]),
                                              cmsBannerRow["Frequency"] == DBNull.Value ? -1 : Convert.ToInt32(cmsBannerRow["Frequency"])
                                               );
            log.LogMethodExit(cmsBannersDTO);
            return cmsBannersDTO;
        }

        /// <summary>
        /// return the record from the database based on  bannerId
        /// </summary>
        /// <returns>return the CMSBannersDTO object</returns>
        /// or null
        public CMSBannersDTO GetcmsBanner(int bannerId)
        {
            log.LogMethodEntry(bannerId);
            string CmsBannerItemsRequestQuery = SELECT_QUERY + "  where BannerId = @bannerId ";
            CMSBannersDTO cmsBannerDTO = new CMSBannersDTO();
            SqlParameter[] CmsBannerItemsParameters = new SqlParameter[1];
            CmsBannerItemsParameters[0] = new SqlParameter("@bannerId", bannerId);
            DataTable CmsBannerItemsRequests = dataAccessHandler.executeSelectQuery(CmsBannerItemsRequestQuery, CmsBannerItemsParameters, sqlTransaction);

            if (CmsBannerItemsRequests.Rows.Count > 0)
            {
                DataRow CmsBannerRequestRow = CmsBannerItemsRequests.Rows[0];
                cmsBannerDTO = GetCmsbannerDTO(CmsBannerRequestRow);

            }
            log.LogMethodExit(cmsBannerDTO);
            return cmsBannerDTO;
        }

        /// <summary>
        /// Gets the CMSBannersDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of Generic BannersDTO matching the search criteria</returns>
        public List<CMSBannersDTO> GetBannersList(List<KeyValuePair<CMSBannersDTO.SearchByRequestParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<CMSBannersDTO> cmsBannersDTOList = new List<CMSBannersDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            int count = 0;
            string selectCmsBannersQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<CMSBannersDTO.SearchByRequestParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (count == 0)

                        {
                            query.Append("Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else
                        {
                            query.Append(" and Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
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
                selectCmsBannersQuery = selectCmsBannersQuery + query;
            }
            DataTable bannerData = dataAccessHandler.executeSelectQuery(selectCmsBannersQuery, parameters.ToArray(), sqlTransaction);
            if (bannerData.Rows.Count > 0)
            {
                foreach (DataRow bannerDataRow in bannerData.Rows)
                {
                    CMSBannersDTO BannerObject = GetCmsbannerDTO(bannerDataRow);
                    cmsBannersDTOList.Add(BannerObject);
                }
            }
            log.LogMethodExit(cmsBannersDTOList);
            return cmsBannersDTOList;
        }
    }
}