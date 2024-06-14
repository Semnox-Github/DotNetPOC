/* Project Name - MasterScheduleDataHandler 
* Description  - Data handler object of the MasterSchedule
* 
**************
**Version Log
**************
*Version     Date           Modified By             Remarks          
********************************************************************************************* 
*2.50        26-Nov-2018    Guru S A             Created for Booking enhancement changes 
*2.70        18-Mar-2019    Guru S A             Booking phase 2 enhancement changes 
*2.70        27-Jun-2019    Akshay Gulaganji     Added DeleteMasterSchedule() method
*2.70.2      10-Dec-2019    Jinto Thomas         Removed siteid from update query
*2.80.0      21-02-2020     Girish Kundar        Modified : 3 tier Changes for REST API
*2.130.4     17-Feb-2022    Nitin Pai            Creating Attraction Schedule Container                                                
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Product
{
    public class MasterScheduleDataHandler
    {
        private DataAccessHandler dataAccessHandler;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private static readonly ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        private static readonly Dictionary<MasterScheduleDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<MasterScheduleDTO.SearchByParameters, string>
            {
                {MasterScheduleDTO.SearchByParameters.MASTER_SCHEDULE_ID, "AttractionMasterScheduleId"},
                {MasterScheduleDTO.SearchByParameters.MASTER_SCHEDULE_ID_LIST, "AttractionMasterScheduleId"},
                {MasterScheduleDTO.SearchByParameters.ACTIVE_FLAG, "ActiveFlag"},
                {MasterScheduleDTO.SearchByParameters.MASTER_ENTITY_ID, "MasterEntityId"},
                {MasterScheduleDTO.SearchByParameters.SITE_ID, "site_id"}
            };

        private static readonly string atMSSelectQuery = @"SELECT *
                                                            FROM AttractionMasterSchedule ";

        /// <summary>
        /// Default constructor of  MasterScheduleDataHandler class
        /// </summary>
        public MasterScheduleDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating masterSchedule Record.
        /// </summary>
        /// <param name="masterScheduleDTO">MasterScheduleDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(MasterScheduleDTO masterScheduleDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(masterScheduleDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@AttractionMasterScheduleId", masterScheduleDTO.MasterScheduleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterScheduleName", masterScheduleDTO.MasterScheduleName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ActiveFlag", (masterScheduleDTO.ActiveFlag == true ? "Y" : "N")));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", masterScheduleDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the MasterSchedule record to the database
        /// </summary>
        /// <param name="masterScheduleDTO">masterScheduleDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public MasterScheduleDTO InsertMasterSchedule(MasterScheduleDTO masterScheduleDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(masterScheduleDTO, loginId, siteId);
            string query = @"INSERT INTO dbo.AttractionMasterSchedule
                                           (MasterScheduleName
                                           ,ActiveFlag
                                           ,Guid
                                           ,site_id 
                                           ,MasterEntityId
                                           ,CreatedBy
                                           ,CreationDate
                                           ,LastUpdatedBy
                                           ,LastUpdateDate)
                                     VALUES
                                           (@MasterScheduleName 
                                           ,@ActiveFlag 
                                           ,NEWID()
                                           ,@site_id 
                                           ,@MasterEntityId 
                                           ,@CreatedBy 
                                           ,getdate()
                                           ,@LastUpdatedBy 
                                           ,getdate()) 
                          SELECT * FROM AttractionMasterSchedule WHERE AttractionMasterScheduleId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(masterScheduleDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshMasterScheduleDTO(masterScheduleDTO, dt, loginId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting masterScheduleDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(masterScheduleDTO);
            return masterScheduleDTO;
        }

        private void RefreshMasterScheduleDTO(MasterScheduleDTO masterScheduleDTO, DataTable dt, string loginId, int siteId)
        {
            log.LogMethodEntry(masterScheduleDTO, dt, loginId, siteId);
            if (dt.Rows.Count > 0)
            {
                masterScheduleDTO.MasterScheduleId = Convert.ToInt32(dt.Rows[0]["AttractionMasterScheduleId"]);
                masterScheduleDTO.LastUpdateDate = dt.Rows[0]["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dt.Rows[0]["LastUpdateDate"]);
                masterScheduleDTO.CreationDate = dt.Rows[0]["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dt.Rows[0]["CreationDate"]);
                masterScheduleDTO.Guid = dt.Rows[0]["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dt.Rows[0]["Guid"]);
                masterScheduleDTO.LastUpdatedBy = dt.Rows[0]["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dt.Rows[0]["LastUpdatedBy"]);
                masterScheduleDTO.CreatedBy = dt.Rows[0]["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dt.Rows[0]["CreatedBy"]);
                masterScheduleDTO.SiteId = dt.Rows[0]["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dt.Rows[0]["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Updates the MasterSchedule record
        /// </summary>
        /// <param name="masterScheduleDTO">MasterScheduleDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public MasterScheduleDTO UpdateMasterSchedule(MasterScheduleDTO masterScheduleDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(masterScheduleDTO, loginId, siteId);
            string query = @"UPDATE dbo.AttractionMasterSchedule
                               SET MasterScheduleName = @MasterScheduleName 
                                  ,ActiveFlag = @ActiveFlag 
                                  -- ,site_id = @site_id 
                                  ,MasterEntityId = @MasterEntityId 
                                  ,LastUpdatedBy = @LastUpdatedBy 
                                  ,LastUpdateDate =getdate()
                             WHERE AttractionMasterScheduleId = @AttractionMasterScheduleId
                            SELECT * FROM AttractionMasterSchedule WHERE AttractionMasterScheduleId = @AttractionMasterScheduleId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(masterScheduleDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshMasterScheduleDTO(masterScheduleDTO, dt, loginId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating  masterScheduleDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(masterScheduleDTO);
            return masterScheduleDTO;
        }

        //internal bool HasValidSchedule(int facilityId, int productId)
        //{
        //    log.LogMethodEntry(facilityId, productId);
        //    bool hasValidSchedule = false;
        //    List<SqlParameter> sqlParams = new List<SqlParameter>();
        //    if(facilityId > -1)
        //    {
        //        sqlParams.Add(new SqlParameter("@facilityId", facilityId)); 
        //    }
        //    if(productId > -1)
        //    {
        //        sqlParams.Add(new SqlParameter("@productId", productId));
        //    }
        //    if(sqlParams.Count > 0)
        //    {
        //        string addProductCheck = " and p.product_Id = @productId ";
        //        string addFacilityCheck = " and fac.facilityId = @facilityId ";

        //        string sqlQry = @" select top 1 1
        //                             from products p, productsAllowedinfacility paif, checkinfacility fac, AttractionSchedules ats
        //                             where p.product_Id = paif.productsId
        //                             and paif.isactive = 1
        //                             and paif.facilityId = fac.facilityId
        //                             and fac.active_flag = 'Y'
        //                             and fac.MasterScheduleId = ats.AttractionMasterScheduleId
        //                             and ats.ActiveFlag = 'Y'" + (productId > -1? addProductCheck : "") + (facilityId > -1 ? addFacilityCheck : "");
        //        Object validSchedules = dataAccessHandler.executeScalar(sqlQry, sqlParams.ToArray(), sqlTransaction);
        //        if(validSchedules != null && Convert.ToBoolean(validSchedules))
        //        {
        //            hasValidSchedule = true;
        //        }
        //    }
        //    log.LogMethodExit(hasValidSchedule); 
        //    return hasValidSchedule;
        //}
        internal bool HasValidSchedule(int facilityMapId, int productId)
        {
            log.LogMethodEntry(facilityMapId, productId);
            bool hasValidSchedule = false;
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            if (facilityMapId > -1)
            {
                sqlParams.Add(new SqlParameter("@facilityMapId", facilityMapId));
            }
            if (productId > -1)
            {
                sqlParams.Add(new SqlParameter("@productId", productId));
            }
            if (sqlParams.Count > 0)
            {
                string addProductCheck = @" AND EXISTS (
                                                   --SELECT 1
									                 --FROM facilityMap fm , products p
													--WHERE p.facilityMapId = fm.facilityMapId
													  --AND fm.MasterScheduleId = ats.AttractionMasterScheduleId
													  --AND fm.isActive = 1
													  --AND p.product_id = @productId 
											        --UNION ALL
													SELECT 1
									                 FROM facilityMap fm , ProductsAllowedInFacility paif
													WHERE paif.facilityMapId = fm.facilityMapId
													  AND fm.MasterScheduleId = ats.AttractionMasterScheduleId
													  AND fm.isActive = 1
													  AND paif.IsActive = 1
													  AND paif.productsid = @productId )  ";
                string addFacilityCheck = @" and EXISTS (SELECT 1 
                                                          FROM facilityMap fm 
                                                         WHERE fm.facilityMapId = @facilityMapId 
                                                           AND fm.MasterScheduleId = ats.AttractionMasterScheduleId
													       AND fm.isActive = 1 ) ";

                string sqlQry = @" select top 1 1
                                     from AttractionSchedules ats
                                     where ats.ActiveFlag = 'Y' " + (productId > -1 ? addProductCheck : "") + (facilityMapId > -1 ? addFacilityCheck : "");
                Object validSchedules = dataAccessHandler.executeScalar(sqlQry, sqlParams.ToArray(), sqlTransaction);
                if (validSchedules != null && Convert.ToBoolean(validSchedules))
                {
                    hasValidSchedule = true;
                }
            }
            log.LogMethodExit(hasValidSchedule);
            return hasValidSchedule;
        }


        /// <summary>
        /// Converts the Data row object to GetMasterScheduleDTO calss type
        /// </summary>
        /// <param name="attrSchRow">MasterSchedule DataRow</param>
        /// <returns>Returns MasterScheduleDTO</returns>
        private MasterScheduleDTO GetMasterScheduleDTO(DataRow attrSchRow)
        {
            log.LogMethodEntry(attrSchRow);
            MasterScheduleDTO masterScheduleDTO = new MasterScheduleDTO(
                                                                    Convert.ToInt32(attrSchRow["AttractionMasterScheduleId"]),
                                                                    attrSchRow["MasterScheduleName"].ToString(),
                                                                    string.IsNullOrEmpty(attrSchRow["ActiveFlag"].ToString()) ? true : (attrSchRow["ActiveFlag"].ToString() == "Y" ? true : false),
                                                                    attrSchRow["Guid"].ToString(),
                                                                    string.IsNullOrEmpty(attrSchRow["site_id"].ToString()) ? -1 : Convert.ToInt32(attrSchRow["site_id"]),
                                                                    string.IsNullOrEmpty(attrSchRow["SynchStatus"].ToString()) ? false : Convert.ToBoolean(attrSchRow["SynchStatus"]),
                                                                    string.IsNullOrEmpty(attrSchRow["MasterEntityId"].ToString()) ? -1 : Convert.ToInt32(attrSchRow["MasterEntityId"]),
                                                                    string.IsNullOrEmpty(attrSchRow["CreatedBy"].ToString()) ? "" : Convert.ToString(attrSchRow["CreatedBy"]),
                                                                    attrSchRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(attrSchRow["CreationDate"]),
                                                                    string.IsNullOrEmpty(attrSchRow["LastUpdatedBy"].ToString()) ? "" : Convert.ToString(attrSchRow["LastUpdatedBy"]),
                                                                    attrSchRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(attrSchRow["LastUpdateDate"])
                                                                    );
            log.LogMethodExit(masterScheduleDTO);
            return masterScheduleDTO;
        }

        /// <summary>
        /// Gets the MasterSchedule data of passed masterSchedule Id
        /// </summary>
        /// <param name="masterScheduleId">integer type parameter</param>
        /// <returns>Returns MasterScheduleDTO</returns>
        public MasterScheduleDTO GetMasterScheduleDTO(int masterScheduleId)
        {
            log.LogMethodEntry(masterScheduleId);
            string selectMasterScheduleQuery = atMSSelectQuery + "  WHERE AttractionMasterScheduleID = @attractionMasterScheduleId";
            SqlParameter[] selectMasterScheduleParameters = new SqlParameter[1];
            selectMasterScheduleParameters[0] = new SqlParameter("@attractionMasterScheduleId", masterScheduleId);
            DataTable masterSchedule = dataAccessHandler.executeSelectQuery(selectMasterScheduleQuery, selectMasterScheduleParameters, sqlTransaction);
            MasterScheduleDTO masterScheduleDataObject = new MasterScheduleDTO();
            if (masterSchedule.Rows.Count > 0)
            {
                DataRow MasterScheduleRow = masterSchedule.Rows[0];
                masterScheduleDataObject = GetMasterScheduleDTO(MasterScheduleRow);
            }
            log.LogMethodExit(masterScheduleDataObject);
            return masterScheduleDataObject;
        }

        /// <summary>
        /// Gets the MasterScheduleDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of MasterScheduleDTO matching the search criteria</returns>
        public List<MasterScheduleDTO> GetMasterScheduleDTOList(List<KeyValuePair<MasterScheduleDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<MasterScheduleDTO> masterScheduleDTOList = new List<MasterScheduleDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            int count = 0;
            string selectQuery = atMSSelectQuery;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<MasterScheduleDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key == MasterScheduleDTO.SearchByParameters.MASTER_SCHEDULE_ID ||
                                 searchParameter.Key == MasterScheduleDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));

                        }
                        else if (searchParameter.Key == MasterScheduleDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == MasterScheduleDTO.SearchByParameters.ACTIVE_FLAG)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
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
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    MasterScheduleDTO masterScheduleDTO = GetMasterScheduleDTO(dataRow);
                    masterScheduleDTOList.Add(masterScheduleDTO);
                }
            }
            log.LogMethodExit(masterScheduleDTOList);
            return masterScheduleDTOList;
        }

        /// <summary>
        /// Based on the attractionMasterScheduleId, appropriate AttractionMasterSchedule record will be deleted
        /// </summary>
        /// <param name="attractionMasterScheduleId">attractionMasterScheduleId</param>
        /// <returns>return the int</returns>
        public int DeleteMasterSchedule(int attractionMasterScheduleId)
        {
            log.LogMethodEntry(attractionMasterScheduleId);
            try
            {
                string deleteQuery = @"delete from AttractionMasterSchedule where AttractionMasterScheduleId = @attractionMasterScheduleId";
                SqlParameter[] deleteParameters = new SqlParameter[1];
                deleteParameters[0] = new SqlParameter("@attractionMasterScheduleId", attractionMasterScheduleId);
                int deleteStatus = dataAccessHandler.executeUpdateQuery(deleteQuery, deleteParameters, sqlTransaction);
                log.LogMethodExit(deleteStatus);
                return deleteStatus;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "throwing exception");
                throw;
            }
        }

        internal DateTime? GetAttractionSchedulesLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"SELECT MAX(LastUpdatedDate) LastUpdatedDate 
                                FROM (
                                select max(LastupdateDate) LastUpdatedDate from AttractionMasterSchedule WHERE (site_id = @siteId or @siteId = -1)
                                union all
                                select max(LastupdateDate) LastUpdatedDate from AttractionSchedules WHERE (site_id = @siteId or @siteId = -1)
                                union all
                                select max(LastupdateDate) LastUpdatedDate from AttractionScheduleRules WHERE (site_id = @siteId or @siteId = -1)
                                union all
                                select max(LastupdateDate) LastUpdatedDate from FacilityMap WHERE (site_id = @siteId or @siteId = -1)
                                union all
                                select max(LastupdateDate) LastUpdatedDate from FacilityMapDetails WHERE (site_id = @siteId or @siteId = -1)
                                union all
                                select max(LastupdateDate) LastUpdatedDate from ProductsAllowedInFacility WHERE (site_id = @siteId or @siteId = -1)
                                union all
                                select max(last_updated_date) LastUpdatedDate from CheckInFacility WHERE (site_id = @siteId or @siteId = -1)
                                union all
                                select max(LastupdateDate) LastUpdatedDate from AttractionPlays WHERE (site_id = @siteId or @siteId = -1)
                                ) a";
            SqlParameter parameter = new SqlParameter("@siteId", siteId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, sqlTransaction);
            DateTime? result = null;
            if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["LastUpdatedDate"] != DBNull.Value)
            {
                result = Convert.ToDateTime(dataTable.Rows[0]["LastUpdatedDate"]);
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}
