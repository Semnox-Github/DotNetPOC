/********************************************************************************************
 * Project Name - Ads Data Handler
 * Description  - Data handler of the Ads handler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.60        17-May-2019   Jagan Mohana Rao        Created 
 *2.70.2      26-Jan-2020   Girish Kundar           Modified : Changed to Standard format 
 *2.90       20-May-2020   Mushahid Faizan     Modified : 3 tier changes for Rest API. 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Semnox.Parafait.ServerCore
{
    public class AdsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM Ads AS ads ";
        private static readonly Dictionary<AdsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<AdsDTO.SearchByParameters, string>
               {
                    {AdsDTO.SearchByParameters.AD_ID, "ads.AdId"},
                    {AdsDTO.SearchByParameters.AD_NAME, "ads.AdName"},
                    {AdsDTO.SearchByParameters.AD_TYPE, "ads.AdType"},
                    {AdsDTO.SearchByParameters.SITE_ID,"ads.site_id"},
                    {AdsDTO.SearchByParameters.ISACTIVE,"ads.Active"},
                    {AdsDTO.SearchByParameters.MASTER_ENTITY_ID,"ads.MasterEntityId"}
               };
        private SqlTransaction sqlTransaction = null;
        /// <summary>
        /// Default constructor of AdsDataHandler class
        /// </summary>
        public AdsDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating Ads Record.
        /// </summary>
        /// <param name="adsDTO">AdsDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(AdsDTO adsDTO, string userId, int siteId)
        {
            log.LogMethodEntry(adsDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@adId", adsDTO.AdId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@adName", adsDTO.AdName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@adImageFileUser", adsDTO.AdImageFileUser));
            parameters.Add(dataAccessHandler.GetSQLParameter("@adImageFileSystem", adsDTO.AdImageFileSystem));
            parameters.Add(dataAccessHandler.GetSQLParameter("@adText", adsDTO.AdText));
            parameters.Add(dataAccessHandler.GetSQLParameter("@active", adsDTO.IsActive == true ? 'Y' : 'N'));
            parameters.Add(dataAccessHandler.GetSQLParameter("@adType", adsDTO.AdType));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
           // parameters.Add(dataAccessHandler.GetSQLParameter("@SynchStatus", adsDTO.SynchStatus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", adsDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", userId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the Ads record to the database
        /// </summary>
        /// <param name="adsDTO">AdsDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public AdsDTO InsertAds(AdsDTO adsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(adsDTO, loginId, siteId);
            string insertAdsQuery = @"insert into Ads 
                                                        (
                                                        AdName,
                                                        AdImageFileUser,
                                                        AdImageFileSystem,
                                                        AdText,
                                                        Active,
                                                        AdType,
                                                        Guid,
                                                        site_id,
                                                        --SynchStatus,
                                                        MasterEntityId,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastUpdateDate
                                                        ) 
                                                values 
                                                        (                                                        
                                                        @adName,
                                                        @adImageFileUser,
                                                        @adImageFileSystem,
                                                        @adText,
                                                        @active,
                                                        @adType,
                                                        NewId(),
                                                        @site_id,
                                                       -- @synchStatus,
                                                        @masterEntityId,
                                                        @createdBy,
                                                        getDate(),
                                                        @lastUpdatedBy,
                                                        getDate()
                                                        )
                                   SELECT * FROM Ads WHERE AdId = scope_identity() ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertAdsQuery, GetSQLParameters(adsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAdsDTO(adsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting adsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(adsDTO);
            return adsDTO;
        }

        /// <summary>
        /// Updates the Ads record
        /// </summary>
        /// <param name="adsDTO">AdsDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public AdsDTO UpdateAds(AdsDTO adsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(adsDTO, loginId, siteId);
            string updateAdsQuery = @"update Ads 
                                         set AdName=@adName,
                                             AdImageFileUser=@adImageFileUser,
                                             AdImageFileSystem=@adImageFileSystem,
                                             AdText=@adText,
                                             Active=@active,
                                             AdType=@adType,
                                             LastUpdatedBy = @lastUpdatedBy, 
                                             LastUpdateDate = Getdate()
                                        where AdId = @adId
                                        SELECT * FROM Ads WHERE AdId=@adId ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateAdsQuery, GetSQLParameters(adsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAdsDTO(adsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating adsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(adsDTO);
            return adsDTO;
        }

        private void RefreshAdsDTO(AdsDTO adsDTO, DataTable dt)
        {
            log.LogMethodEntry(adsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                adsDTO.AdId = Convert.ToInt32(dt.Rows[0]["AdId"]);
                adsDTO.LastupdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                adsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                adsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                adsDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                adsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                adsDTO.Siteid = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to AdsDTO class type
        /// </summary>
        /// <param name="adsDataRow">AdsDTO DataRow</param>
        /// <returns>Returns AdsDTO</returns>
        private AdsDTO GetAdsDTO(DataRow adsDataRow)
        {
            log.LogMethodEntry(adsDataRow);
            AdsDTO adsDataObject = new AdsDTO(Convert.ToInt32(adsDataRow["AdId"]),
                                            adsDataRow["AdName"] == DBNull.Value ? string.Empty : Convert.ToString(adsDataRow["AdName"]),
                                            adsDataRow["AdImageFileUser"] == DBNull.Value ? string.Empty : Convert.ToString(adsDataRow["AdImageFileUser"]),
                                            adsDataRow["AdImageFileSystem"] == DBNull.Value ? string.Empty : Convert.ToString(adsDataRow["AdImageFileSystem"]),
                                            adsDataRow["AdText"] == DBNull.Value ? string.Empty : Convert.ToString(adsDataRow["AdText"]),
                                            adsDataRow["Active"] == DBNull.Value ? false : (adsDataRow["Active"].ToString() == "Y" ? true : false),
                                            adsDataRow["AdType"] == DBNull.Value ? string.Empty : Convert.ToString(adsDataRow["AdType"]),
                                            adsDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(adsDataRow["Guid"]),
                                            adsDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(adsDataRow["site_id"]),
                                            adsDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(adsDataRow["SynchStatus"]),
                                            adsDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(adsDataRow["MasterEntityId"]),
                                            adsDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(adsDataRow["CreatedBy"]),
                                            adsDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(adsDataRow["CreationDate"]),
                                            adsDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(adsDataRow["LastUpdatedBy"]),
                                            adsDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(adsDataRow["LastUpdateDate"])
                                            );
            log.LogMethodExit(adsDataObject);
            return adsDataObject;
        }

        /// <summary>
        /// Based on the adId, appropriate ads record will be deleted
        /// This is for hard deletion. In future, when we implement soft deletion this method may not be required
        /// </summary>
        /// <param name="adId">primary key of adId </param>
        /// <returns>return the int </returns>
        public int DeleteAds(int adId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(adId);
            try
            {
                string adsQuery = @"delete from Ads where AdId = @adId";
                SqlParameter[] adsParameters = new SqlParameter[1];
                adsParameters[0] = new SqlParameter("@adId", adId);

                int deleteStatus = dataAccessHandler.executeUpdateQuery(adsQuery, adsParameters, sqlTransaction);
                log.LogMethodExit(deleteStatus);
                return deleteStatus;
            }
            catch (Exception expn)
            {
                System.Console.Write(expn.Message.ToString());
                throw new System.Exception(expn.Message.ToString());
            }
        }
        /// <summary>
        /// Gets the Ads data of passed adId id
        /// </summary>
        /// <param name="adId">integer type parameter</param>
        /// <returns>Returns AdsDTO</returns>
        public AdsDTO GetAds(int adId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(adId);
            AdsDTO adsDataObject = new AdsDTO();
            string selectAdsQuery = SELECT_QUERY + " where Ads.AdId = @adId";
            SqlParameter[] selectAdsParameters = new SqlParameter[1];
            selectAdsParameters[0] = new SqlParameter("@adId", adId);
            DataTable ads = dataAccessHandler.executeSelectQuery(selectAdsQuery, selectAdsParameters, sqlTransaction);
            if (ads.Rows.Count > 0)
            {
                DataRow adsRow = ads.Rows[0];
                adsDataObject = GetAdsDTO(adsRow);
            }
            log.LogMethodExit(adsDataObject);
            return adsDataObject;
        }

        /// <summary>
        /// Gets the AdsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of AdsDTO matching the search criteria</returns>
        public List<AdsDTO> GetAdsDTOList(List<KeyValuePair<AdsDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            int counter = 0;
            List<AdsDTO> adsDTOList = new List<AdsDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectAdsQuery = SELECT_QUERY;
            if (searchParameters != null)
            {
                string joiner;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<AdsDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key.Equals(AdsDTO.SearchByParameters.AD_ID) ||
                              searchParameter.Key.Equals(AdsDTO.SearchByParameters.MASTER_ENTITY_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AdsDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AdsDTO.SearchByParameters.ISACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1" || searchParameter.Value == "Y" ? "Y" : "N"));
                        }
                        else if (searchParameter.Key == AdsDTO.SearchByParameters.AD_NAME)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        counter++;
                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                }
                if (searchParameters.Count > 0)
                    selectAdsQuery = selectAdsQuery + query;
            }

            DataTable adsData = dataAccessHandler.executeSelectQuery(selectAdsQuery, parameters.ToArray(), sqlTransaction);
            if (adsData.Rows.Count > 0)
            {
                foreach (DataRow adsDataRow in adsData.Rows)
                {
                    AdsDTO adsDataObject = GetAdsDTO(adsDataRow);
                    adsDTOList.Add(adsDataObject);
                }
            }
            log.LogMethodExit(adsDTOList);
            return adsDTOList;
        }
    }
}