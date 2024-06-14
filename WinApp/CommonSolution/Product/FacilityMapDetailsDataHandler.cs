/********************************************************************************************
 * Project Name - FacilityMapDetailsDataHandler
 * Description  - data handler file for  FacilityMapDetails
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        14-Jun-2019   Guru S A                Created 
 *2.70.2      10-Dec-2019   Jinto Thomas            Removed siteid from update query
 *2.80.0      27-Feb-2020   Girish Kundar           Modified : 3 tier changes for API 
 ********************************************************************************************/

using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// FacilityMapDetails data handler class
    /// </summary>
    public class FacilityMapDetailsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT vfd.*, fac.facilityName FROM FacilityMapDetails AS vfd left outer join checkinfacility fac on vfd.facilityId = fac.facilityId"; // Create alias names for the Table
        /// <summary>
        /// Dictionary for searching Parameters for the FacilityMapDetails object.
        /// </summary>
        private static readonly Dictionary<FacilityMapDetailsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<FacilityMapDetailsDTO.SearchByParameters, string>
        {
            { FacilityMapDetailsDTO.SearchByParameters.FACILITY_MAP_DETAIL_ID,"vfd.FacilityMapDetailId"},
            { FacilityMapDetailsDTO.SearchByParameters.FACILITY_MAP_ID,"vfd.FacilityMapId"},
            { FacilityMapDetailsDTO.SearchByParameters.FACILITY_MAP_ID_LIST,"vfd.FacilityMapId"},
            { FacilityMapDetailsDTO.SearchByParameters.FACILITY_ID,"vfd.FacilityId"},
            { FacilityMapDetailsDTO.SearchByParameters.IS_ACTIVE,"vfd.IsActive"},
            { FacilityMapDetailsDTO.SearchByParameters.SITE_ID,"vfd.site_id"},
            { FacilityMapDetailsDTO.SearchByParameters.MASTER_ENTITY_ID,"vfd.MasterEntityId"}
        };

        /// <summary>
        /// Parameterized Constructor for FacilityMapDetailsDataHandler.
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public FacilityMapDetailsDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating FacilityMapDetails Record.
        /// </summary>
        /// <param name="facilityMapDetailsDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        private List<SqlParameter> GetSQLParameters(FacilityMapDetailsDTO facilityMapDetailsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(facilityMapDetailsDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@FacilityMapDetailId", facilityMapDetailsDTO.FacilityMapDetailId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@FacilityMapId", facilityMapDetailsDTO.FacilityMapId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@FacilityId", facilityMapDetailsDTO.FacilityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", facilityMapDetailsDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", facilityMapDetailsDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        ///  Converts the Data row object to FacilityMapDetailsDTO class type
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns>facilityMapDetailsDTO</returns>
        private FacilityMapDetailsDTO GetFacilityMapDetailsDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            FacilityMapDetailsDTO facilityMapDetailsDTO = new FacilityMapDetailsDTO(dataRow["FacilityMapDetailId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["FacilityMapDetailId"]),
                                                         dataRow["FacilityMapId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["FacilityMapId"]),
                                                         dataRow["FacilityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["FacilityId"]),
                                                         dataRow["FacilityName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["FacilityName"]),
                                                         dataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["IsActive"]),
                                                         dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                         dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                         dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                         dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                         dataRow["LastupdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdateDate"]),
                                                         dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                         dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                         dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"])
                                                        );
            log.LogMethodExit(facilityMapDetailsDTO);
            return facilityMapDetailsDTO;
        }

        /// <summary>
        /// Gets the FacilityMapDetails data of passed FacilityMapDetailId 
        /// </summary>
        /// <param name="facilityMapDetailId">integer type parameter</param>
        /// <returns>Returns FacilityMapDetailsDTO</returns>
        public FacilityMapDetailsDTO GetFacilityMapDetails(int facilityMapDetailId)
        {
            log.LogMethodEntry(facilityMapDetailId);
            FacilityMapDetailsDTO result = null;
            string query = SELECT_QUERY + @" WHERE vfd.FacilityMapDetailId = @FacilityMapDetailId";
            SqlParameter parameter = new SqlParameter("@FacilityMapDetailId", facilityMapDetailId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetFacilityMapDetailsDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        ///  Inserts the record to the FacilityMapDetails Table.
        /// </summary>
        /// <param name="facilityMapDetailsDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public FacilityMapDetailsDTO Insert(FacilityMapDetailsDTO facilityMapDetailsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(facilityMapDetailsDTO, loginId, siteId);
            string query = @"INSERT INTO dbo.FacilityMapDetails
                            (
                            FacilityMapId
                           ,FacilityId 
                           ,IsActive
                           ,Guid
                           ,CreatedBy
                           ,CreationDate
                           ,LastUpdatedBy
                           ,LastUpdateDate
                           ,site_id
                           ,MasterEntityId 
                            )
                        VALUES
                            (
                            @FacilityMapId
                           ,@FacilityId 
                           ,@IsActive
                           ,NEWID()
                           ,@CreatedBy
                           ,GETDATE()
                           ,@LastUpdatedBy
                           ,GETDATE() 
                           ,@site_id
                           ,@MasterEntityId
                         )
                            SELECT * FROM FacilityMapDetails WHERE FacilityMapDetailId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(facilityMapDetailsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshFacilityMapDetailsDTO(facilityMapDetailsDTO, dt, loginId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting FacilityMapDetailsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(facilityMapDetailsDTO);
            return facilityMapDetailsDTO;
        }

        /// <summary>
        ///  Updates the record to the FacilityMapDetails Table.
        /// </summary>
        /// <param name="facilityMapDetailsDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public FacilityMapDetailsDTO Update(FacilityMapDetailsDTO facilityMapDetailsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(facilityMapDetailsDTO, loginId, siteId);
            string query = @"UPDATE dbo.FacilityMapDetails
                                SET
                                FacilityMapId =   @FacilityMapId
                               ,FacilityId = @FacilityId 
                               ,IsActive =  @IsActive
                               ,LastUpdatedBy = @LastUpdatedBy
                               ,LastUpdateDate = GETDATE()
                               ,MasterEntityId =   @MasterEntityId 
                            where FacilityMapDetailId = @FacilityMapDetailId 
                           SELECT * FROM FacilityMapDetails WHERE FacilityMapDetailId = @FacilityMapDetailId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(facilityMapDetailsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshFacilityMapDetailsDTO(facilityMapDetailsDTO, dt, loginId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating facilityMapDetailsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(facilityMapDetailsDTO);
            return facilityMapDetailsDTO;
        }

        private void RefreshFacilityMapDetailsDTO(FacilityMapDetailsDTO facilityMapDetailsDTO, DataTable dt, string loginId, int siteId)
        {
            log.LogMethodEntry(facilityMapDetailsDTO, dt, loginId, siteId);
            if (dt.Rows.Count > 0)
            {
                facilityMapDetailsDTO.FacilityMapDetailId = Convert.ToInt32(dt.Rows[0]["FacilityMapDetailId"]);
                facilityMapDetailsDTO.LastUpdatedDate = dt.Rows[0]["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dt.Rows[0]["LastUpdateDate"]);
                facilityMapDetailsDTO.CreationDate = dt.Rows[0]["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dt.Rows[0]["CreationDate"]);
                facilityMapDetailsDTO.Guid = dt.Rows[0]["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dt.Rows[0]["Guid"]);
                facilityMapDetailsDTO.LastUpdatedBy = dt.Rows[0]["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dt.Rows[0]["LastUpdatedBy"]);
                facilityMapDetailsDTO.CreatedBy = dt.Rows[0]["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dt.Rows[0]["CreatedBy"]);
                facilityMapDetailsDTO.SiteId = dt.Rows[0]["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dt.Rows[0]["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the List of facilityMapDetailsDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns>facilityMapDetailsDTOList</returns>
        public List<FacilityMapDetailsDTO> GetAllFacilityMapDetails(List<KeyValuePair<FacilityMapDetailsDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<FacilityMapDetailsDTO> facilityMapDetailsDTOList = new List<FacilityMapDetailsDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<FacilityMapDetailsDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == FacilityMapDetailsDTO.SearchByParameters.FACILITY_MAP_ID ||
                            searchParameter.Key == FacilityMapDetailsDTO.SearchByParameters.FACILITY_MAP_DETAIL_ID ||
                            searchParameter.Key == FacilityMapDetailsDTO.SearchByParameters.FACILITY_ID ||
                             searchParameter.Key == FacilityMapDetailsDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == FacilityMapDetailsDTO.SearchByParameters.FACILITY_MAP_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == FacilityMapDetailsDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }

                        else if (searchParameter.Key == FacilityMapDetailsDTO.SearchByParameters.IS_ACTIVE)  // bit
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                    counter++;
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    FacilityMapDetailsDTO facilityMapDetailsDTO = GetFacilityMapDetailsDTO(dataRow);
                    facilityMapDetailsDTOList.Add(facilityMapDetailsDTO);
                }
            }
            log.LogMethodExit(facilityMapDetailsDTOList);
            return facilityMapDetailsDTOList;
        }

    }
}
