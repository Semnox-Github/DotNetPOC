/********************************************************************************************
 * Project Name - Communication
 * Description  - DataHandler of PushNotificationDevice Entity
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.100.0   15-Sep-2020   Nitin Pai               Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Communication
{
    public class PushNotificationDeviceDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * from PushNotificationDevice AS pnd";
        private DataAccessHandler dataAccessHandler;
        private static readonly Dictionary<PushNotificationDeviceDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<PushNotificationDeviceDTO.SearchByParameters, string>
            {
                {PushNotificationDeviceDTO.SearchByParameters.ID, "pnd.Id"},
                {PushNotificationDeviceDTO.SearchByParameters.CUSTOMER_ID, "pnd.CustomerId"},
                {PushNotificationDeviceDTO.SearchByParameters.PUSH_NOTIFICATION_TOKEN, "pnd.PushNotificationToken"},
                {PushNotificationDeviceDTO.SearchByParameters.DEVICE_TYPE, "pnd.DeviceType"},
                {PushNotificationDeviceDTO.SearchByParameters.IS_ACTIVE,"pnd.IsActive"},
                {PushNotificationDeviceDTO.SearchByParameters.CUSTOMER_SIGNED_IN,"pnd.CustomerSignedIn"},
                {PushNotificationDeviceDTO.SearchByParameters.GUID,"pnd.GUID"},
                {PushNotificationDeviceDTO.SearchByParameters.MASTER_ENTITY_ID,"pnd.MasterEntityId"},
                {PushNotificationDeviceDTO.SearchByParameters.SITE_ID, "pnd.site_id"}
            };


        /// <summary>
        /// Default constructor of PushNotificationDeviceDataHandler class
        /// </summary>
        public PushNotificationDeviceDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating PushNotificationDevice Record.
        /// </summary>
        /// <param name="pushNotificationDeviceDTO">PushNotificationDeviceeDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(PushNotificationDeviceDTO pushNotificationDeviceDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(pushNotificationDeviceDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", pushNotificationDeviceDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CustomerId", pushNotificationDeviceDTO.CustomerId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PushNotificationToken", pushNotificationDeviceDTO.PushNotificationToken));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DeviceType", pushNotificationDeviceDTO.DeviceType));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", pushNotificationDeviceDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CustomerSignedIn", pushNotificationDeviceDTO.CustomerSignedIn));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Guid", pushNotificationDeviceDTO.Guid));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", pushNotificationDeviceDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the PushNotificationDevice record to the database
        /// </summary>
        /// <param name="PushNotificationDeviceDTO">PushNotificationDeviceDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns PushNotificationDeviceDTO</returns>
        public PushNotificationDeviceDTO InsertPushNotificationDevice(PushNotificationDeviceDTO PushNotificationDeviceDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(PushNotificationDeviceDTO, loginId, siteId);
            string query = @"INSERT INTO PushNotificationDevice 
                                        ( 
                                            CustomerId,
                                            PushNotificationToken,
                                            DeviceType,
                                            IsActive,
                                            CustomerSignedIn,
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdatedBy,
                                            LastUpdateDate,
                                            site_id,
                                            MasterEntityId

                                        ) 
                                VALUES 
                                        (
                                            @CustomerId,
                                            @PushNotificationToken,
                                            @DeviceType,
                                            @IsActive,
                                            @CustomerSignedIn,
                                            @CreatedBy,
                                            GETDATE(),
                                            @LastUpdatedBy,
                                            GETDATE(),
                                            @site_id,
                                            @MasterEntityId
                                        ) SELECT * FROM PushNotificationDevice WHERE Id  = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(PushNotificationDeviceDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPushNotificationDeviceDTO(PushNotificationDeviceDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting PushNotificationDeviceDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(PushNotificationDeviceDTO);
            return PushNotificationDeviceDTO;
        }

        /// <summary>
        /// Updates the PushNotificationDevice record
        /// </summary>
        /// <param name="PushNotificationDeviceDTO">PushNotificationDeviceDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns PushNotificationDeviceDTO</returns>
        public PushNotificationDeviceDTO UpdatePushNotificationDevice(PushNotificationDeviceDTO PushNotificationDeviceDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(PushNotificationDeviceDTO, loginId, siteId);
            string query = @"UPDATE PushNotificationDevice 
                             SET CustomerId=@CustomerId,
                                 PushNotificationToken=@PushNotificationToken,
                                 DeviceType=@DeviceType,
                                 IsActive = @IsActive,
                                 CustomerSignedIn = @CustomerSignedIn,
                                 LastUpdatedBy = @LastUpdatedBy,
                                 LastUpdateDate=GETDATE(),
                                 MasterEntityId=@MasterEntityId
                           WHERE Id = @Id
                           SELECT * FROM PushNotificationDevice WHERE Id  =  @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(PushNotificationDeviceDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPushNotificationDeviceDTO(PushNotificationDeviceDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating PushNotificationDeviceDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(PushNotificationDeviceDTO);
            return PushNotificationDeviceDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="PushNotificationDeviceDTO">PushNotificationDeviceDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshPushNotificationDeviceDTO(PushNotificationDeviceDTO PushNotificationDeviceDTO, DataTable dt)
        {
            log.LogMethodEntry(PushNotificationDeviceDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                PushNotificationDeviceDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                PushNotificationDeviceDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString();
                PushNotificationDeviceDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                PushNotificationDeviceDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString();
                PushNotificationDeviceDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                PushNotificationDeviceDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
                PushNotificationDeviceDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString();
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Converts the Data row object to PushNotificationDeviceDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns PushNotificationDeviceDTO</returns>
        private PushNotificationDeviceDTO GetPushNotificationDeviceDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            PushNotificationDeviceDTO PushNotificationDeviceDTO = new PushNotificationDeviceDTO(Convert.ToInt32(dataRow["Id"]),
                                            dataRow["CustomerId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CustomerId"]),
                                            dataRow["PushNotificationToken"] == DBNull.Value ? string.Empty : dataRow["PushNotificationToken"].ToString(),
                                            dataRow["DeviceType"] == DBNull.Value ? string.Empty : dataRow["DeviceType"].ToString(),
                                            dataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["IsActive"]),
                                            dataRow["CustomerSignedIn"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["CustomerSignedIn"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString(),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString(),
                                            dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString()
                                            );
            log.LogMethodExit(PushNotificationDeviceDTO);
            return PushNotificationDeviceDTO;
        }

        /// <summary>
        /// Gets the PushNotificationDevice data of passed PushNotificationDevice Id
        /// </summary>
        /// <param name="PushNotificationDeviceId">integer type parameter</param>
        /// <returns>Returns PushNotificationDeviceDTO</returns>
        public PushNotificationDeviceDTO GetPushNotificationDeviceDTO(int PushNotificationDeviceId)
        {
            log.LogMethodEntry(PushNotificationDeviceId);
            PushNotificationDeviceDTO returnValue = null;
            string query = SELECT_QUERY + "   WHERE pnd.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", PushNotificationDeviceId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetPushNotificationDeviceDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Gets the PushNotificationDevice data of passed PushNotificationDevice Id
        /// </summary>
        /// <param name="PushNotificationDeviceId">integer type parameter</param>
        /// <returns>Returns PushNotificationDeviceDTO</returns>
        public PushNotificationDeviceDTO GetPushNotificationDeviceDTO(string token)
        {
            log.LogMethodEntry(token);
            PushNotificationDeviceDTO returnValue = null;
            string query = SELECT_QUERY + "   WHERE pnd.PushNotificationToken = @PushNotificationToken";
            SqlParameter parameter = new SqlParameter("@PushNotificationToken", token);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetPushNotificationDeviceDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }


        /// <summary>
        /// Gets the PushNotificationDeviceDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of PushNotificationDeviceDTO matching the search criteria</returns>
        public List<PushNotificationDeviceDTO> GetPushNotificationDeviceDTOList(List<KeyValuePair<PushNotificationDeviceDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<PushNotificationDeviceDTO> list = null;
            int count = 0;
            string selectQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<PushNotificationDeviceDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key == PushNotificationDeviceDTO.SearchByParameters.ID
                            || searchParameter.Key == PushNotificationDeviceDTO.SearchByParameters.MASTER_ENTITY_ID
                            || searchParameter.Key == PushNotificationDeviceDTO.SearchByParameters.CUSTOMER_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == PushNotificationDeviceDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == PushNotificationDeviceDTO.SearchByParameters.IS_ACTIVE ||
                                 searchParameter.Key == PushNotificationDeviceDTO.SearchByParameters.CUSTOMER_SIGNED_IN)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == PushNotificationDeviceDTO.SearchByParameters.PUSH_NOTIFICATION_TOKEN
                                || searchParameter.Key == PushNotificationDeviceDTO.SearchByParameters.DEVICE_TYPE
                                || searchParameter.Key == PushNotificationDeviceDTO.SearchByParameters.GUID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
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
            log.Error(selectQuery);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<PushNotificationDeviceDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    PushNotificationDeviceDTO PushNotificationDeviceDTO = GetPushNotificationDeviceDTO(dataRow);
                    list.Add(PushNotificationDeviceDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }
    }
}
