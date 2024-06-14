/********************************************************************************************
 * Project Name - Facility Seat Layout Data Handler
 * Description  - Data object of Facility Seat Layout data handler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By               Remarks          
 *********************************************************************************************
 *2.60        22-Feb-2019   Akshay Gulaganji          Created FacilitySeatLayout Data Handler class
 *2.70        29-Jun-2019   Akshay Gulaganji          Added isActive and made required changes
 *2.70.2      10-Dec-2019   Jinto Thomas              Removed siteid from update query
 *2.80.0      27-Feb-2020   Girish Kundar             Modified : 3 tier changes for API 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;


namespace Semnox.Parafait.Product
{
    public class FacilitySeatLayoutDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;
        private static readonly string SELECT_QUERY = @"SELECT* FROM FacilitySeatLayout fsl";

        private static readonly Dictionary<FacilitySeatLayoutDTO.SearchByFacilitySeatLayoutParameter, string> DBSearchParameters = new Dictionary<FacilitySeatLayoutDTO.SearchByFacilitySeatLayoutParameter, string>
        {
            {FacilitySeatLayoutDTO.SearchByFacilitySeatLayoutParameter.FACILITY_ID, "FacilityId"},
            {FacilitySeatLayoutDTO.SearchByFacilitySeatLayoutParameter.FACILITY_ID_LIST, "FacilityId"},
            {FacilitySeatLayoutDTO.SearchByFacilitySeatLayoutParameter.MASTER_ENTITY_ID, "MasterEntityId"},
            {FacilitySeatLayoutDTO.SearchByFacilitySeatLayoutParameter.ROW_COLUMN_INDEX, "RowColumnIndex"},
            {FacilitySeatLayoutDTO.SearchByFacilitySeatLayoutParameter.TYPE, "Type"},
            {FacilitySeatLayoutDTO.SearchByFacilitySeatLayoutParameter.SITE_ID, "site_id"},
            {FacilitySeatLayoutDTO.SearchByFacilitySeatLayoutParameter.LAYOUT_ID, "LayoutId"},
            {FacilitySeatLayoutDTO.SearchByFacilitySeatLayoutParameter.HAS_SEATS, "HasSeats"},
            {FacilitySeatLayoutDTO.SearchByFacilitySeatLayoutParameter.IS_ACTIVE, "IsActive"}
        };
        /// <summary>
        /// Default constructor of FacilitySeatLayoutDataHandler class
        /// </summary>
        public FacilitySeatLayoutDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating FacilitySeatLayout Record.
        /// </summary>
        /// <param name="facilitySeatLayoutDTO">FacilitySeatLayout type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(FacilitySeatLayoutDTO facilitySeatLayoutDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(facilitySeatLayoutDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@layoutId", facilitySeatLayoutDTO.LayoutId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@facilityId", facilitySeatLayoutDTO.FacilityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@rowColumnName", facilitySeatLayoutDTO.RowColumnName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@type", Convert.ToChar(facilitySeatLayoutDTO.Type)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@rowColumnIndex", facilitySeatLayoutDTO.RowColumnIndex, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@hasSeats", facilitySeatLayoutDTO.HasSeats.ToString() == null ? 'Y' : facilitySeatLayoutDTO.HasSeats));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", facilitySeatLayoutDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", facilitySeatLayoutDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the FacilitySeatLayout record to the database
        /// </summary>
        /// <param name="facilitySeatLayoutDTO">FacilitySeatLayoutDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns FacilitySeatLayoutDTO</returns>
        public FacilitySeatLayoutDTO InsertFacilitySeatLayout(FacilitySeatLayoutDTO facilitySeatLayoutDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(facilitySeatLayoutDTO, loginId, siteId);
            string query = @"insert into FacilitySeatLayout(
                                                                FacilityId, 
                                                                RowColumnName, 
                                                                Type, 
                                                                RowColumnIndex, 
                                                                HasSeats, 
                                                                Guid,
                                                                site_id,
                                                                MasterEntityId,
                                                                CreatedBy,
                                                                CreationDate,
                                                                LastUpdatedBy,
                                                                LastUpdateDate,
                                                                IsActive
                                                           )
                                                           values
                                                           (
                                                               @facilityId,
                                                               @rowColumnName,
                                                               @type,
                                                               @rowColumnIndex, --(select isnull(max(RowColumnIndex), 0) + 1 from FacilitySeatLayout where FacilityId = @facilityId and Type = @type),
                                                               @hasSeats,
                                                               NewId(),
                                                               @site_id,
                                                               @masterEntityId,
                                                               @createdBy,
                                                               GETDATE(),
                                                               @lastUpdatedBy,
                                                               GETDATE(),
                                                               @isActive
                                                            )
                                             SELECT * FROM FacilitySeatLayout WHERE LayoutId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(facilitySeatLayoutDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshFacilitySeatLayoutDTO(facilitySeatLayoutDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting facilitySeatLayoutDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(facilitySeatLayoutDTO);
            return facilitySeatLayoutDTO;
        }

        /// <summary>
        /// updates the FacilitySeatLayout record to the database
        /// </summary>
        /// <param name="facilitySeatLayoutDTO">FacilitySeatLayoutDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns FacilitySeatLayoutDTO</returns>
        public FacilitySeatLayoutDTO UpdateFacilitySeatLayout(FacilitySeatLayoutDTO facilitySeatLayoutDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(facilitySeatLayoutDTO, loginId, siteId);
            string query = @"update FacilitySeatLayout set FacilityId=@facilityId, 
                                                           RowColumnName=@rowColumnName, 
                                                           Type=@type, 
                                                           RowColumnIndex=@rowColumnIndex, 
                                                           HasSeats=@hasSeats,
                                                           MasterEntityId=@masterEntityId,
                                                           LastUpdatedBy=@lastUpdatedBy,
                                                           LastUpdateDate=GETDATE(),
                                                           IsActive=@isActive
                                                        Where 
                                                        LayoutId=@layoutId
                                       SELECT * FROM FacilitySeatLayout WHERE LayoutId = @layoutId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(facilitySeatLayoutDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshFacilitySeatLayoutDTO(facilitySeatLayoutDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating facilitySeatLayoutDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(facilitySeatLayoutDTO);
            return facilitySeatLayoutDTO;
        }

        private void RefreshFacilitySeatLayoutDTO(FacilitySeatLayoutDTO facilitySeatLayoutDTO, DataTable dt)
        {
            log.LogMethodEntry(facilitySeatLayoutDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                facilitySeatLayoutDTO.LayoutId = Convert.ToInt32(dt.Rows[0]["LayoutId"]);
                facilitySeatLayoutDTO.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                facilitySeatLayoutDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                facilitySeatLayoutDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                facilitySeatLayoutDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                facilitySeatLayoutDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString();
                facilitySeatLayoutDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Deletes the FacilitySeatLayout record of passed layout Id
        /// </summary>
        /// <param name="layoutId">integer type parameter</param>
        internal void DeleteFacilitySeatLayout(int layoutId)
        {
            log.LogMethodEntry(layoutId);
            string query = @"DELETE  
                             FROM FacilitySeatLayout
                             WHERE LayoutId = @layoutId";
            SqlParameter parameter = new SqlParameter("@layoutId", layoutId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the FacilitySeatLayout list matching the searchParameter
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of FacilitySeatLayoutDTO matching the search criteria</returns>
        public List<FacilitySeatLayoutDTO> GetFacilitySeatLayoutDTOList(List<KeyValuePair<FacilitySeatLayoutDTO.SearchByFacilitySeatLayoutParameter, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<FacilitySeatLayoutDTO> facilitySeatLayoutDTOList = null;
            int count = 0;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectFacilitySeatLayoutQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<FacilitySeatLayoutDTO.SearchByFacilitySeatLayoutParameter, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key == FacilitySeatLayoutDTO.SearchByFacilitySeatLayoutParameter.FACILITY_ID ||
                            searchParameter.Key == FacilitySeatLayoutDTO.SearchByFacilitySeatLayoutParameter.LAYOUT_ID ||
                             searchParameter.Key == FacilitySeatLayoutDTO.SearchByFacilitySeatLayoutParameter.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == FacilitySeatLayoutDTO.SearchByFacilitySeatLayoutParameter.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == FacilitySeatLayoutDTO.SearchByFacilitySeatLayoutParameter.FACILITY_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == FacilitySeatLayoutDTO.SearchByFacilitySeatLayoutParameter.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? true : false)));

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
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                }
                if (searchParameters.Count > 0)
                    selectFacilitySeatLayoutQuery = selectFacilitySeatLayoutQuery + query;
                selectFacilitySeatLayoutQuery = selectFacilitySeatLayoutQuery + " Order By RowColumnIndex, Type desc";
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectFacilitySeatLayoutQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                facilitySeatLayoutDTOList = new List<FacilitySeatLayoutDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    FacilitySeatLayoutDTO facilitySeatLayoutDTO = GetFacilitySeatLayoutDTO(dataRow);
                    facilitySeatLayoutDTOList.Add(facilitySeatLayoutDTO);
                }
            }
            log.LogMethodExit(facilitySeatLayoutDTOList);
            return facilitySeatLayoutDTOList;
        }

        /// <summary>
        /// Converts the Data row object to FacilitySeatLayoutDTO class type
        /// </summary>
        /// <param name="facilitySeatLayoutDTOdataRow">facilitySeatLayoutDTOdataRow</param>
        /// <returns>Returns FacilitySeatLayoutDTO</returns>
        private FacilitySeatLayoutDTO GetFacilitySeatLayoutDTO(DataRow facilitySeatLayoutDTOdataRow)
        {
            log.LogMethodEntry(facilitySeatLayoutDTOdataRow);
            try
            {
                FacilitySeatLayoutDTO facilitySeatLayoutDTO = new FacilitySeatLayoutDTO(Convert.ToInt32(facilitySeatLayoutDTOdataRow["LayoutId"]),
                                           facilitySeatLayoutDTOdataRow["FacilityId"] == DBNull.Value ? -1 : Convert.ToInt32(facilitySeatLayoutDTOdataRow["FacilityId"]),
                                           facilitySeatLayoutDTOdataRow["RowColumnName"] == DBNull.Value ? string.Empty : Convert.ToString(facilitySeatLayoutDTOdataRow["RowColumnName"]),
                                           Convert.ToChar(facilitySeatLayoutDTOdataRow["Type"]),
                                           facilitySeatLayoutDTOdataRow["RowColumnIndex"] == DBNull.Value ? -1 : Convert.ToInt32(facilitySeatLayoutDTOdataRow["RowColumnIndex"]),
                                           facilitySeatLayoutDTOdataRow["HasSeats"] == DBNull.Value ? 'Y' : Convert.ToChar(facilitySeatLayoutDTOdataRow["HasSeats"]),
                                           facilitySeatLayoutDTOdataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(facilitySeatLayoutDTOdataRow["Guid"]),
                                           facilitySeatLayoutDTOdataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(facilitySeatLayoutDTOdataRow["SynchStatus"]),
                                           facilitySeatLayoutDTOdataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(facilitySeatLayoutDTOdataRow["site_id"]),
                                           facilitySeatLayoutDTOdataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(facilitySeatLayoutDTOdataRow["MasterEntityId"]),
                                           facilitySeatLayoutDTOdataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(facilitySeatLayoutDTOdataRow["IsActive"]),
                                           facilitySeatLayoutDTOdataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(facilitySeatLayoutDTOdataRow["CreatedBy"]),
                                           facilitySeatLayoutDTOdataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(facilitySeatLayoutDTOdataRow["CreationDate"]),
                                           facilitySeatLayoutDTOdataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(facilitySeatLayoutDTOdataRow["LastUpdatedBy"]),
                                           facilitySeatLayoutDTOdataRow["LastupdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(facilitySeatLayoutDTOdataRow["LastupdateDate"])
                                           );

                log.LogMethodExit(facilitySeatLayoutDTO);
                return facilitySeatLayoutDTO;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }

    }
}
