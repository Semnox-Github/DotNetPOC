/********************************************************************************************
 * Project Name - Facility Waivers Data Handler
 * Description  - Data handler of the Facility Waivers  class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.70.2       26-Sep-2019     Deeksha            Created for waiver phase 2
 *2.70.2       10-Dec-2019    Jinto Thomas        Removed siteid from update query
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;

namespace Semnox.Parafait.Product
{
    public class FacilityWaiversDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"Select * from FacilityWaiver as fw ";

        /// <summary>
        /// Dictionary for searching Parameters for the CustomerSignedWaiver object.
        /// </summary>
        private static readonly Dictionary<FacilityWaiverDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<FacilityWaiverDTO.SearchByParameters, string>
            {
                {FacilityWaiverDTO.SearchByParameters.FACILITY_ID, "fw.FacilityId"},
                {FacilityWaiverDTO.SearchByParameters.FACILITY_ID_LIST, "fw.FacilityId"},
                {FacilityWaiverDTO.SearchByParameters.FACILITY_WAIVER__ID, "fw.FacilityWaiverId"},
                {FacilityWaiverDTO.SearchByParameters.WAIVER_SET_ID,"fw.WaiverSetId"},
                {FacilityWaiverDTO.SearchByParameters.EFFECTIVE_FROM,"fw.EffectiveFrom"},
                {FacilityWaiverDTO.SearchByParameters.EFFECTIVE_TO,"fw.EffectiveTo"},
                {FacilityWaiverDTO.SearchByParameters.SITE_ID, "fw.site_id"},
                {FacilityWaiverDTO.SearchByParameters.IS_ACTIVE , "fw.IsActive"},
                {FacilityWaiverDTO.SearchByParameters.MASTER_ENTITY_ID, "fw.MasterEntityId"},
                 {FacilityWaiverDTO.SearchByParameters.PRODUCT_ID, "paf.ProductsId"}
            };

        /// <summary>
        /// Default constructor of FacilityWaiverDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public FacilityWaiversDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodEntry();
        }

        public void Delete(int facilityWaiverId)
        {
            log.LogMethodEntry(facilityWaiverId);
            string query = @"delete from FacilityWaiver where FacilityWaiverId = @FacilityWaiverId";
            SqlParameter parameter = new SqlParameter("@FacilityWaiverId", facilityWaiverId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating FacilityWaiverDTO parameters Record.
        /// </summary>
        /// <param name="FacilityWaiverDTO">FacilityWaiverDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>  Returns the list of SQL parameter </returns>
        private List<SqlParameter> GetSQLParameters(FacilityWaiverDTO facilityWaiverDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(facilityWaiverDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@facilityWaiverId", facilityWaiverDTO.FacilityWaiverId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@facilityId", facilityWaiverDTO.FacilityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@waiverSetId", facilityWaiverDTO.WaiverSetId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@effectiveFrom", facilityWaiverDTO.EffectiveFrom));
            parameters.Add(dataAccessHandler.GetSQLParameter("@effectiveTo", facilityWaiverDTO.EffectiveTo));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", facilityWaiverDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", facilityWaiverDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the FacilityWaiverDTO record to the database
        /// </summary>
        /// <param name="FacilityWaiverDTO">FacilityWaiverDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>FacilityWaiverDTO</returns>
        public FacilityWaiverDTO InsertFacilityWaiver(FacilityWaiverDTO facilityWaiverDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(facilityWaiverDTO, loginId, siteId);
            string query = @"INSERT INTO FacilityWaiver
                                        ( 
                                            FacilityId,
                                            WaiverSetId,
                                            EffectiveFrom,
                                            EffectiveTo,
                                            IsActive,
                                            Guid,
                                            site_id,
                                            MasterEntityId,
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdatedBy,
                                            LastUpdateDate
                                        ) 
                                VALUES 
                                        (
                                            @facilityId,
                                            @waiverSetId,
                                            @effectiveFrom,
                                            @effectiveTo,
                                            @isActive,            
                                            NEWID(),
                                            @site_id,
                                            @masterEntityId,
                                            @createdBy,
                                            GETDATE(),
                                            @lastUpdatedBy,
                                            GETDATE()
                                        ) SELECT * FROM FacilityWaiver WHERE FacilityWaiverId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(facilityWaiverDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshFacilityWaiverDTO(facilityWaiverDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting facilityWaiverDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(facilityWaiverDTO);
            return facilityWaiverDTO;
        }

        /// <summary>
        /// Updates the FacilityWaiver record
        /// </summary>
        /// <param name="FacilityWaiverDTO">FacilityWaiverDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>FacilityWaiverDTO</returns>
        public FacilityWaiverDTO UpdateFacilityWaiver(FacilityWaiverDTO facilityWaiverDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(facilityWaiverDTO, loginId, siteId);
            string query = @"UPDATE FacilityWaiver 
                             SET FacilityId = @facilityId,
                                 WaiverSetId = @waiverSetId,
                                 EffectiveFrom = @effectiveFrom,
                                 EffectiveTo = @effectiveTo,
                                 IsActive = @isActive,
                                 LastUpdatedBy=@lastUpdatedBy,
                                 LastUpdateDate= GETDATE(),
                                 -- site_id=@site_id,
                                 MasterEntityId=@masterEntityId
                             WHERE FacilityWaiverId = @facilityWaiverId
                             SELECT * FROM FacilityWaiver WHERE FacilityWaiverId = @facilityWaiverId ";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(facilityWaiverDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshFacilityWaiverDTO(facilityWaiverDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating facilityWaiverDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(facilityWaiverDTO);
            return facilityWaiverDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="facilityWaiverDTO">facilityWaiverDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshFacilityWaiverDTO(FacilityWaiverDTO facilityWaiverDTO, DataTable dt)
        {
            log.LogMethodEntry(facilityWaiverDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                facilityWaiverDTO.FacilityWaiverId = Convert.ToInt32(dt.Rows[0]["FacilityWaiverId"]);
                facilityWaiverDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                facilityWaiverDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                facilityWaiverDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                facilityWaiverDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                facilityWaiverDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                facilityWaiverDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to FacilityWaiverDTO class type
        /// </summary>
        /// <param name="customerSignedWaiverHeaderDTODataRow">CustomerSignedWaiverHeader DataRow</param>
        /// <returns>Returns CustomerSignedWaiver</returns>
        private FacilityWaiverDTO GetFacilityWaiverDTO(DataRow facilityWaiverDataRow)
        {
            log.LogMethodEntry(facilityWaiverDataRow);
            FacilityWaiverDTO facilityWaiverDataObject = new FacilityWaiverDTO(Convert.ToInt32(facilityWaiverDataRow["FacilityWaiverId"]),
                                            facilityWaiverDataRow["FacilityId"] == DBNull.Value ? -1 : Convert.ToInt32(facilityWaiverDataRow["FacilityId"]),
                                            facilityWaiverDataRow["WaiverSetId"] == DBNull.Value ? -1 : Convert.ToInt32(facilityWaiverDataRow["WaiverSetId"]),
                                            facilityWaiverDataRow["EffectiveFrom"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(facilityWaiverDataRow["EffectiveFrom"]),
                                            facilityWaiverDataRow["EffectiveTo"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(facilityWaiverDataRow["EffectiveTo"]),
                                            facilityWaiverDataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(facilityWaiverDataRow["IsActive"]),
                                            facilityWaiverDataRow["Guid"] == DBNull.Value ? string.Empty : facilityWaiverDataRow["Guid"].ToString(),
                                            facilityWaiverDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(facilityWaiverDataRow["site_id"]),
                                            facilityWaiverDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(facilityWaiverDataRow["SynchStatus"]),
                                            facilityWaiverDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(facilityWaiverDataRow["MasterEntityId"]),
                                            facilityWaiverDataRow["CreatedBy"] == DBNull.Value ? string.Empty : facilityWaiverDataRow["CreatedBy"].ToString(),
                                            facilityWaiverDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(facilityWaiverDataRow["CreationDate"]),
                                            facilityWaiverDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : facilityWaiverDataRow["LastUpdatedBy"].ToString(),
                                            facilityWaiverDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(facilityWaiverDataRow["LastUpdateDate"])
                                            );
            log.LogMethodExit(facilityWaiverDataObject);
            return facilityWaiverDataObject;
        }

        /// <summary>
        /// Gets the customer Signed Waiver Detail of passed facilityWaiverId
        /// </summary>
        /// <param name="facilityWaiverId">integer type parameter</param>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        /// <returns>Returns CustomerSignedWaiverHeaderDTO</returns>
        public FacilityWaiverDTO GetFacilityWaiverDTO(int facilityWaiverId)
        {
            log.LogMethodEntry(facilityWaiverId);
            FacilityWaiverDTO result = null;
            string selectCustomerSignedWaiverQuery = SELECT_QUERY + @" WHERE FacilityWaiverId = @facilityWaiverId";
            SqlParameter parameter = new SqlParameter("@facilityWaiverId", facilityWaiverId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectCustomerSignedWaiverQuery, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetFacilityWaiverDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the FacilityWaiverDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        /// <returns>Returns the list of FacilityWaiverDTO matching the search criteria</returns>
        public List<FacilityWaiverDTO> GetAllFacilityWaiverList(List<KeyValuePair<FacilityWaiverDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<FacilityWaiverDTO> facilityWaiverList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<FacilityWaiverDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == FacilityWaiverDTO.SearchByParameters.WAIVER_SET_ID
                            || searchParameter.Key == FacilityWaiverDTO.SearchByParameters.FACILITY_WAIVER__ID
                            || searchParameter.Key == FacilityWaiverDTO.SearchByParameters.FACILITY_ID
                            || searchParameter.Key == FacilityWaiverDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == FacilityWaiverDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'0')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else if (searchParameter.Key == FacilityWaiverDTO.SearchByParameters.EFFECTIVE_TO)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",DATEADD(minute, 5,getdate())) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == FacilityWaiverDTO.SearchByParameters.EFFECTIVE_FROM)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",DATEADD(minute, -5,getdate())) <= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == FacilityWaiverDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == FacilityWaiverDTO.SearchByParameters.PRODUCT_ID)
                        {
                            query.Append(joiner + @" EXISTS (select 1
                                                                  from ProductsAllowedInFacility paf,
                                                                       FacilityMapDetails fmd
                                                                 where paf.ProductsId = "+ dataAccessHandler.GetParameterName(searchParameter.Key) +
                                                                 @" and paf.FacilityMapId = fmd.FacilityMapId
                                                                    and paf.IsActive = 1
                                                                    and fmd.IsActive = 1
                                                                    and fmd.FacilityId = fw.FacilityId)
                                                ");
                             parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == FacilityWaiverDTO.SearchByParameters.FACILITY_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
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
                facilityWaiverList = new List<FacilityWaiverDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    FacilityWaiverDTO facilityWaiverDTO = GetFacilityWaiverDTO(dataRow);
                    facilityWaiverList.Add(facilityWaiverDTO);
                }
            }
            log.LogMethodExit(facilityWaiverList);
            return facilityWaiverList;
        }
    }
}


