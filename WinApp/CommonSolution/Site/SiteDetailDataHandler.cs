/********************************************************************************************
 * Project Name - Site                                                                       
 * Description  - Site Detail Data Handler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************* 
  *2.110.0     01-Feb-2021   Girish Kundar     Created : Urban Piper changes
 ********************************************************************************************/

using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Semnox.Parafait.Site
{
    public class SiteDetailDataHandler
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM SiteDetails AS sdl ";
        private static readonly Dictionary<SiteDetailDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<SiteDetailDTO.SearchByParameters, string>
            {
                {SiteDetailDTO.SearchByParameters.DELIVERY_CHANNEL_ID, "sdl.DeliveryChannelId"},
                {SiteDetailDTO.SearchByParameters.SITE_DETAIL_ID, "sdl.SiteDetailId"},
                {SiteDetailDTO.SearchByParameters.PARENT_SITE_ID, "sdl.ParentSiteId"},
                {SiteDetailDTO.SearchByParameters.IS_ACTIVE, "sdl.IsActive"},
                {SiteDetailDTO.SearchByParameters.MASTER_ENTITY_ID, "sdl.MasterEntityId"},
                {SiteDetailDTO.SearchByParameters.SITE_ID, "sdl.site_id"},
                {SiteDetailDTO.SearchByParameters.LAST_UPDATED_DATE, "sdl.LastUpdatedDate"}
            };
        /// <summary>
        /// Default constructor of OrderDetailDataHandler class
        /// </summary>
        public SiteDetailDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }
        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating SiteDetailDTO Record.
        /// </summary>
        /// <param name="siteDetailDTO">SiteDetailDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> BuildSQLParameters(SiteDetailDTO siteDetailDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(siteDetailDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteDetailId", siteDetailDTO.SiteDetailId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ParentSiteId", siteDetailDTO.ParentSiteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DeliveryChannelId", siteDetailDTO.DeliveryChannelId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@OnlineChannelStartHour", siteDetailDTO.OnlineChannelStartHour));
            parameters.Add(dataAccessHandler.GetSQLParameter("@OnlineChannelEndHour", siteDetailDTO.OnlineChannelEndHour));
            parameters.Add(dataAccessHandler.GetSQLParameter("@OrderDeliveryType", siteDetailDTO.OrderDeliveryType, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ZipCodes", siteDetailDTO.ZipCodes));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", siteDetailDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", siteDetailDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteDetailDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public SiteDetailDTO Insert(SiteDetailDTO siteDetailDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(siteDetailDTO, loginId, siteId);
            string insertQuery = @"insert into SiteDetails 
                                                        (                                                         
                                                       ParentSiteId ,
                                                       DeliveryChannelId,
                                                       OnlineChannelStartHour,
                                                       OnlineChannelEndHour,
                                                       OrderDeliveryType,
                                                       ZipCodes,
                                                       IsActive ,
                                                       CreatedBy ,
                                                       CreationDate ,
                                                       LastUpdatedBy ,
                                                       LastUpdatedDate ,
                                                       Guid ,
                                                       site_id   ,
                                                       MasterEntityId 
                                                      ) 
                                                values 
                                                        (                                                        
                                                       @ParentSiteId ,
                                                       @DeliveryChannelId,
                                                       @OnlineChannelStartHour,
                                                       @OnlineChannelEndHour,
                                                       @OrderDeliveryType,
                                                       @ZipCodes,
                                                       @IsActive ,
                                                       @CreatedBy , 
                                                       GetDate(),
                                                       @LastUpdatedBy,
                                                       GetDate(),
                                                       NewId(),
                                                       @SiteId,
                                                       @MasterEntityId 
                                          )SELECT  * from SiteDetails where SiteDetailId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertQuery, BuildSQLParameters(siteDetailDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshSiteDetailDTO(siteDetailDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting SiteDetailDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(siteDetailDTO);
            return siteDetailDTO;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteDetailDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public SiteDetailDTO Update(SiteDetailDTO siteDetailDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(siteDetailDTO, loginId, siteId);
            string updateQuery = @"update SiteDetails 
                                         set 
                                             ParentSiteId = @ParentSiteId,
                                             DeliveryChannelId = @DeliveryChannelId,
                                             OnlineChannelStartHour = @OnlineChannelStartHour,
                                             OnlineChannelEndHour = @OnlineChannelEndHour,
                                             OrderDeliveryType = @OrderDeliveryType,
                                             IsActive = @IsActive,
                                             ZipCodes = @ZipCodes,
                                            LastUpdatedBy = @LastUpdatedBy,
                                            LastUpdatedDate = GetDate(),
                                            MasterEntityId =  @MasterEntityId 
                                               where   SiteDetailId =  @SiteDetailId  
                                        SELECT  * from SiteDetails where SiteDetailId = @SiteDetailId ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateQuery, BuildSQLParameters(siteDetailDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshSiteDetailDTO(siteDetailDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating SiteDetailDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(siteDetailDTO);
            return siteDetailDTO;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SiteDetailDTO"></param>
        /// <returns></returns>
        public SiteDetailDTO UpdateSiteDetailDTO(SiteDetailDTO SiteDetailDTO)
        {
            log.LogMethodEntry(SiteDetailDTO);
            string updateQuery = @"update SiteDetails 
                                         set 
                                             ParentSiteId = @ParentSiteId,
                                             DeliveryChannelId = @DeliveryChannelId,
                                             OnlineChannelStartHour = @OnlineChannelStartHour,
                                              IsActive = @IsActive,
                                             OnlineChannelEndHour = @OnlineChannelEndHour,
                                             OrderDeliveryType = @OrderDeliveryType,
                                             ZipCodes = @ZipCodes
                                               where   SiteDetailId =  @SiteDetailId  
                                        )SELECT  * from SiteDetails where SiteDetailId = @SiteDetailId ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateQuery, BuildSQLParameters(SiteDetailDTO, string.Empty, -1).ToArray(), sqlTransaction);
                RefreshSiteDetailDTO(SiteDetailDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating SiteDetailDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(SiteDetailDTO);
            return SiteDetailDTO;
        }
        private void RefreshSiteDetailDTO(SiteDetailDTO siteDetailDTO, DataTable dt)
        {
            log.LogMethodEntry(siteDetailDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                siteDetailDTO.SiteDetailId = Convert.ToInt32(dt.Rows[0]["SiteDetailId"]);
                siteDetailDTO.LastUpdateDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                siteDetailDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                siteDetailDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                siteDetailDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                siteDetailDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                siteDetailDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        private SiteDetailDTO GetSiteDetailDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            SiteDetailDTO siteDetailDTO = new SiteDetailDTO(Convert.ToInt32(dataRow["SiteDetailId"]),
                                                    dataRow["ParentSiteId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ParentSiteId"]),
                                                    dataRow["DeliveryChannelId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["DeliveryChannelId"]),
                                                    dataRow["OnlineChannelStartHour"] == DBNull.Value ? (decimal?) null : Convert.ToDecimal(dataRow["OnlineChannelStartHour"]),
                                                    dataRow["OnlineChannelEndHour"] == DBNull.Value ? (decimal?) null : Convert.ToDecimal(dataRow["OnlineChannelEndHour"]),
                                                    dataRow["OrderDeliveryType"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["OrderDeliveryType"]),
                                                    dataRow["ZipCodes"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ZipCodes"]),
                                                    dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                                                    dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                    dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                    dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                    dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                                    dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                    dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                    dataRow["synchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["synchStatus"]),
                                                    dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"])
                                                    );
            log.LogMethodExit(siteDetailDTO);
            return siteDetailDTO;
        }

        /// <summary>
        /// Gets the SiteDetailDTO data of passed id
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns SiteDetailDTO</returns>
        internal SiteDetailDTO GetSiteDetailDTO(int id)
        {
            log.LogMethodEntry(id);
            SiteDetailDTO siteDetailDTO = null;
            string selectUserQuery = SELECT_QUERY + "   where sdl.SiteDetailId = @SiteDetailId";
            SqlParameter[] selectUserParameters = new SqlParameter[1];
            selectUserParameters[0] = new SqlParameter("@SiteDetailId", id);
            DataTable siteDetailTable = dataAccessHandler.executeSelectQuery(selectUserQuery, selectUserParameters, sqlTransaction);
            if (siteDetailTable.Rows.Count > 0)
            {
                DataRow gameMachineLevelRow = siteDetailTable.Rows[0];
                siteDetailDTO = GetSiteDetailDTO(gameMachineLevelRow);
            }
            log.LogMethodExit(siteDetailDTO);
            return siteDetailDTO;

        }

        internal List<SiteDetailDTO> GetSiteDetailDTOList(List<int> siteIdList, bool activeRecords)
        {
            log.LogMethodEntry(siteIdList);
            List<SiteDetailDTO> siteDetailDTOList = new List<SiteDetailDTO>();
            string query = @"SELECT *
                            FROM SiteDetails, @siteIdList List
                            WHERE ParentSiteId = List.Id ";
            if (activeRecords)
            {
                query += " AND IsActive = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@siteIdList", siteIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                siteDetailDTOList = table.Rows.Cast<DataRow>().Select(x => GetSiteDetailDTO(x)).ToList();
            }
            log.LogMethodExit(siteDetailDTOList);
            return siteDetailDTOList;
        }

        /// <summary>
        /// GetDeliveryChannels
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<SiteDetailDTO> GetSiteDetails(List<KeyValuePair<SiteDetailDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<SiteDetailDTO> siteDetailDTOList = null;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<SiteDetailDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == SiteDetailDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == SiteDetailDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else if (searchParameter.Key.Equals(SiteDetailDTO.SearchByParameters.MASTER_ENTITY_ID) ||
                            searchParameter.Key.Equals(SiteDetailDTO.SearchByParameters.DELIVERY_CHANNEL_ID) ||
                            searchParameter.Key.Equals(SiteDetailDTO.SearchByParameters.PARENT_SITE_ID) ||
                                  searchParameter.Key.Equals(SiteDetailDTO.SearchByParameters.DELIVERY_CHANNEL_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(SiteDetailDTO.SearchByParameters.LAST_UPDATED_DATE))
                        {
                            query.Append(joiner +" ISNULL(" + DBSearchParameters[searchParameter.Key] + ", GETDATE()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
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
                        log.LogMethodExit(null, "throwing exception");
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }

                if (searchParameters.Count > 0)
                    selectQuery = selectQuery + query;
            }
            DataTable data = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (data.Rows.Count > 0)
            {
                siteDetailDTOList = new List<SiteDetailDTO>();
                foreach (DataRow dataRow in data.Rows)
                {
                    SiteDetailDTO SiteDetailDTO = GetSiteDetailDTO(dataRow);
                    siteDetailDTOList.Add(SiteDetailDTO);
                }
            }
            log.LogMethodExit(siteDetailDTOList);
            return siteDetailDTOList;
        }
    }
}
