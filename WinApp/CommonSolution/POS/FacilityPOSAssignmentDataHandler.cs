/********************************************************************************************
 * Project Name - FacilityPOSAssignmentDataHandler
 * Description  - DH class for FacilityPOSAssignment
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By         Remarks          
 ********************************************************************************************* 
 *2.70        08-Mar-2019     Guru S A            Moved to POS namespace
 * 2.60        07-May-2019    Akshay Gulaganji    Added isActive
 *2.60.2      31-May-2019     Jagan Mohana        Moved from Bookings to POS and Code merge from Development to WebManagementStudio
 *2.70        28-Jun-2019     Akshay Gulaganji    Added DeleteFacilityPOSAssignment() method
 *2.70.2        10-Dec-2019   Jinto Thomas            Removed siteid from update query
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using System.Data;

namespace Semnox.Parafait.POS
{
    class FacilityPOSAssignmentDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;
        private static readonly Dictionary<FacilityPOSAssignmentDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<FacilityPOSAssignmentDTO.SearchByParameters, string>
        {
                {FacilityPOSAssignmentDTO.SearchByParameters.ID,"FacilityPOSAssignment.Id"},
                {FacilityPOSAssignmentDTO.SearchByParameters.FACILITY_ID, "FacilityPOSAssignment.FacilityId"},
                {FacilityPOSAssignmentDTO.SearchByParameters.POS_MACHINE_ID, "FacilityPOSAssignment.POSMachineId"},
                {FacilityPOSAssignmentDTO.SearchByParameters.SITE_ID, " FacilityPOSAssignment.site_id "},
                {FacilityPOSAssignmentDTO.SearchByParameters.IS_ACTIVE, " FacilityPOSAssignment.IsActive "}
        };
        private const string SELECT_QUERY = @" SELECT FacilityPOSAssignment.*
                                               FROM FacilityPOSAssignment ";

        /// <summary>
        /// Default constructor of FacilityPOSAssignmentDataHandler class
        /// </summary>
        public FacilityPOSAssignmentDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating FacilityPOSAssignment Record.
        /// </summary>
        /// <param name="facilityPOSAssignmentDTO">FacilityPOSAssignment type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(FacilityPOSAssignmentDTO facilityPOSAssignmentDTO, string userId, int siteId)
        {
            log.LogMethodEntry(facilityPOSAssignmentDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", facilityPOSAssignmentDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@POSMachineId", facilityPOSAssignmentDTO.POSMachineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@FacilityId", facilityPOSAssignmentDTO.FacilityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", facilityPOSAssignmentDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", facilityPOSAssignmentDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the FacilityPOSAssignment record to the database
        /// </summary>
        public FacilityPOSAssignmentDTO InsertFacilityPOSAssignment(FacilityPOSAssignmentDTO facilityPOSAssignmentDTO, string userId, int siteId)
        {
            log.LogMethodEntry(facilityPOSAssignmentDTO, userId, siteId);
            string query = @"INSERT INTO FacilityPOSAssignment
                                        ( 
                                            POSMachineId,
                                            FacilityId,
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdatedDate,
                                            LastUpdatedBy,
                                            GUID,
                                            site_id,
                                            MasterEntityId,
                                            IsActive
                                        )
                                        VALUES
                                        (  @POSMachineId,
                                            @FacilityId,
                                            @CreatedBy,
                                            Getdate(),
                                            Getdate(),
                                            @LastUpdatedBy,
                                            NewId(),
                                            @site_id,
                                            @MasterEntityId,
                                            @IsActive
                                        )
                                        SELECT * FROM FacilityPOSAssignment WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(facilityPOSAssignmentDTO, userId, siteId).ToArray(), sqlTransaction);
                if (dt.Rows.Count > 0)
                {
                    facilityPOSAssignmentDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                    facilityPOSAssignmentDTO.LastUpdatedDate = Convert.ToDateTime(dt.Rows[0]["LastUpdatedDate"]);
                    facilityPOSAssignmentDTO.Guid = Convert.ToString(dt.Rows[0]["guid"]);
                    facilityPOSAssignmentDTO.LastUpdatedBy = userId;
                    facilityPOSAssignmentDTO.SiteId = siteId;
                }
            }
            catch (Exception ex)
            {
                log.LogVariableState("FacilityPOSAssignmentDTO", facilityPOSAssignmentDTO);
                log.Error("Error occured while inserting the FacilityPOSAssignment record", ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(facilityPOSAssignmentDTO);
            return facilityPOSAssignmentDTO;
        }

        public FacilityPOSAssignmentDTO UpdateFacilityPOSAssignment(FacilityPOSAssignmentDTO facilityPOSAssignmentDTO, string userId, int siteId)
        {
            log.LogMethodEntry(facilityPOSAssignmentDTO, userId, siteId);
            string query = @"UPDATE FacilityPOSAssignment SET 
                            POSMachineId = @POSMachineId,
                            FacilityId = @FacilityId,
                            LastUpdatedDate = Getdate(),
                            LastUpdatedBy = @LastUpdatedBy,
                            -- site_id = @site_id,
                            IsActive = @IsActive,
                            MasterEntityId =  @MasterEntityId
                            WHERE  Id = @Id
                            SELECT * FROM FacilityPOSAssignment WHERE Id = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(facilityPOSAssignmentDTO, userId, siteId).ToArray(), sqlTransaction);
                if (dt.Rows.Count > 0)
                {
                    facilityPOSAssignmentDTO.LastUpdatedDate = Convert.ToDateTime(dt.Rows[0]["LastUpdatedDate"]);
                    facilityPOSAssignmentDTO.LastUpdatedBy = userId;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while updating the FacilityPOSAssignment record", ex);
                log.LogVariableState("FacilityPOSAssignmentDTO", facilityPOSAssignmentDTO);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(facilityPOSAssignmentDTO);
            return facilityPOSAssignmentDTO;
        }

        private List<FacilityPOSAssignmentDTO> CreateFacilityPOSAssignmentDTOList(SqlDataReader reader)
        {
            log.LogMethodEntry(reader);
            List<FacilityPOSAssignmentDTO> facilityPOSAssignmentDTOList = new List<FacilityPOSAssignmentDTO>();
            int id = reader.GetOrdinal("Id");
            int pOSMachineId = reader.GetOrdinal("POSMachineId");
            int facilityId = reader.GetOrdinal("FacilityId");
            int createdBy = reader.GetOrdinal("CreatedBy");
            int creationDate = reader.GetOrdinal("CreationDate");
            int LastUpdatedDate = reader.GetOrdinal("LastUpdatedDate");
            int lastUpdatedBy = reader.GetOrdinal("LastUpdatedBy");
            int site_id = reader.GetOrdinal("site_id");
            int masterEntityId = reader.GetOrdinal("MasterEntityId");
            int synchStatus = reader.GetOrdinal("SynchStatus");
            int isActive = reader.GetOrdinal("IsActive");
            int guid = reader.GetOrdinal("Guid");
            while (reader.Read())
            {
                FacilityPOSAssignmentDTO facilityPOSAssignmentDTO = new FacilityPOSAssignmentDTO(reader.IsDBNull(id) ? -1 : reader.GetInt32(id),
                                        reader.IsDBNull(pOSMachineId) ? -1 : reader.GetInt32(pOSMachineId),
                                        reader.IsDBNull(facilityId) ? -1 : reader.GetInt32(facilityId),
                                        reader.IsDBNull(creationDate) ? DateTime.MinValue : reader.GetDateTime(creationDate),
                                        reader.IsDBNull(createdBy) ? "" : reader.GetString(createdBy),
                                        reader.IsDBNull(LastUpdatedDate) ? DateTime.MinValue : reader.GetDateTime(LastUpdatedDate),
                                        reader.IsDBNull(lastUpdatedBy) ? "" : reader.GetString(lastUpdatedBy),
                                        reader.IsDBNull(site_id) ? -1 : reader.GetInt32(site_id),
                                        reader.IsDBNull(guid) ? "" : reader.GetGuid(guid).ToString(),
                                        reader.IsDBNull(synchStatus) ? false : reader.GetBoolean(synchStatus),
                                        reader.IsDBNull(masterEntityId) ? -1 : reader.GetInt32(masterEntityId),
                                        reader.IsDBNull(isActive) ? true : reader.GetBoolean(isActive)
                                        ); ;
                facilityPOSAssignmentDTOList.Add(facilityPOSAssignmentDTO);
            }
            log.LogMethodExit(facilityPOSAssignmentDTOList);
            return facilityPOSAssignmentDTOList;
        }

        /// <summary>
        /// Gets the FacilityPOSAssignment data of passed facilityPOSAssignment Id
        /// </summary>
        /// <param name="facilityPOSAssignmentId">integer type parameter</param>
        /// <returns>Returns FacilityPOSAssignmentDTO</returns>
        public FacilityPOSAssignmentDTO GetFacilityPOSAssignmentDTO(int facilityPOSAssignmentId)
        {
            log.LogMethodEntry(facilityPOSAssignmentId);
            FacilityPOSAssignmentDTO result = null;
            string selectQuery = SELECT_QUERY + " WHERE FacilityPOSAssignment.Id = @Id";
            SqlParameter[] selectFacilityPOSAssignmentParameters = new SqlParameter[1];
            selectFacilityPOSAssignmentParameters[0] = new SqlParameter("@Id", facilityPOSAssignmentId);
            List<FacilityPOSAssignmentDTO> list = dataAccessHandler.GetDataFromReader(selectQuery, selectFacilityPOSAssignmentParameters, sqlTransaction, CreateFacilityPOSAssignmentDTOList);
            if (list != null)
            {
                result = list[0];
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the FacilityPOSAssignmentDTO list matching the UserId
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of facilityPOSAssignmentDTO matching the search criteria</returns>
        public List<FacilityPOSAssignmentDTO> GetFacilityPOSAssignmentDTOList(List<KeyValuePair<FacilityPOSAssignmentDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            string selectQuery = SELECT_QUERY;
            selectQuery = SELECT_QUERY + GetWhereClause(searchParameters);
            List<FacilityPOSAssignmentDTO> list = dataAccessHandler.GetDataFromReader(selectQuery, null, sqlTransaction, CreateFacilityPOSAssignmentDTOList);
            log.LogMethodExit(list);
            return list;
        }

        private static string GetWhereClause(List<KeyValuePair<FacilityPOSAssignmentDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string whereClause = string.Empty;
            if (searchParameters == null || searchParameters.Count == 0)
            {
                log.LogMethodExit(string.Empty, "search parameters is empty");
                return whereClause;
            }
            string joiner = "";
            StringBuilder query = new StringBuilder(" where ");
            foreach (KeyValuePair<FacilityPOSAssignmentDTO.SearchByParameters, string> searchParameter in searchParameters)
            {
                if (DBSearchParameters.ContainsKey(searchParameter.Key))
                {
                    joiner = (count == 0) ? " " : " and ";
                    {
                        if (searchParameter.Key.Equals(FacilityPOSAssignmentDTO.SearchByParameters.ID) ||
                            searchParameter.Key.Equals(FacilityPOSAssignmentDTO.SearchByParameters.FACILITY_ID) ||
                            searchParameter.Key.Equals(FacilityPOSAssignmentDTO.SearchByParameters.POS_MACHINE_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + searchParameter.Value);
                        }
                        else if (searchParameter.Key == FacilityPOSAssignmentDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + " (" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                        }
                        else if (searchParameter.Key == FacilityPOSAssignmentDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') like " + "N'%" + searchParameter.Value + "%'");
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%" + searchParameter.Value + "%'");
                        }
                    }
                    count++;
                }
                else
                {
                    log.Error("Ends-GetAllFacilityPOSAssignmentList(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                    log.LogMethodExit("Throwing exception- The query parameter does not exist ");
                    throw new Exception("The query parameter does not exist");
                }
            }
            whereClause = query.ToString();
            log.LogMethodExit(whereClause);
            return whereClause;
        }
        /// <summary>
        /// Based on the Id, appropriate FacilityPOSAssignment details record will be deleted        
        /// </summary>
        /// <param name="Id">Id</param>
        /// <returns>return the int </returns>
        public int DeleteFacilityPOSAssignment(int Id)
        {
            try
            {
                log.LogMethodEntry(Id);
                string deleteQuery = @"delete from FacilityPOSAssignment where Id = @Id";
                SqlParameter[] deleteParameters = new SqlParameter[1];
                deleteParameters[0] = new SqlParameter("@Id", Id);

                int deleteStatus = dataAccessHandler.executeUpdateQuery(deleteQuery, deleteParameters, sqlTransaction);
                log.LogMethodExit(deleteStatus);
                return deleteStatus;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }
    }
}
