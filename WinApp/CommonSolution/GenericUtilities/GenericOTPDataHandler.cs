/********************************************************************************************
 * Project Name - Generic
 * Description  - Generic Data Handler
 * 
 **************
 **Version Log
 **************
 *Version      Date          Modified By        Remarks          
 ********************************************************************************************* 
 *2.130.11   16-Aug-2022    Yashodhara C H     Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Semnox.Core.GenericUtilities
{
    public class GenericOTPDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM GenericOTP as go ";

        /// <summary>
        /// Dictionary for searching Parameters for the GenericOTP object.
        /// </summary>
        private static readonly Dictionary<GenericOTPDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<GenericOTPDTO.SearchByParameters, string>
        {
            { GenericOTPDTO.SearchByParameters.ID, "go.Id" },
            { GenericOTPDTO.SearchByParameters.CODE, "go.Code" },
            { GenericOTPDTO.SearchByParameters.PHONE, "go.phone" },
            { GenericOTPDTO.SearchByParameters.EMAIL_ID, "go.emailId" },
            { GenericOTPDTO.SearchByParameters.SOURCE, "go.Source" },
            { GenericOTPDTO.SearchByParameters.IS_VERIFIED, "go.IsVerified" },
            { GenericOTPDTO.SearchByParameters.IS_ACTIVE, "go.IsActive" },
            { GenericOTPDTO.SearchByParameters.REMAINING_ATTEMPTS, "go.RemainingAttempts" },
            { GenericOTPDTO.SearchByParameters.MASTER_ENTITY_ID, "go.MasterEntityId" },
             { GenericOTPDTO.SearchByParameters.SITE_ID, "go.site_id" }
        };

        /// <summary>
        /// Parameterized constructor of GenericDataHandler class
        /// </summary>
        public GenericOTPDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating GenericOTP Record.
        /// </summary>
        /// <param name="GenericOTPDTO">GenericOTPDTO type object</param>
        /// <param name="id">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        public List<SqlParameter> GetSQLParameters(GenericOTPDTO genericOTPDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(genericOTPDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", genericOTPDTO.Id));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Code", genericOTPDTO.Code));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Phone", genericOTPDTO.Phone));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CountryCode", genericOTPDTO.CountryCode));
            parameters.Add(dataAccessHandler.GetSQLParameter("@EmailId", genericOTPDTO.EmailId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Source", genericOTPDTO.Source));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsVerified", genericOTPDTO.IsVerified));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RemainingAttempts", genericOTPDTO.RemainingAttempts));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ExpiryTime", genericOTPDTO.ExpiryTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", genericOTPDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", genericOTPDTO.MasterEntityId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the GenericOTP record to the database
        /// </summary>
        /// <param name="genericOTPDTO">GenericOTPDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted Generic record</returns>
        public GenericOTPDTO InsertRecord(GenericOTPDTO genericOTPDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(genericOTPDTO, loginId, siteId);
            string query = @"INSERT INTO GenericOTP
                                (
                                    Code,
                                    Phone,
                                    CountryCode,
                                    EmailId,
                                    Source,
                                    IsVerified,
                                    RemainingAttempts,
                                    ExpiryTime,
                                    IsActive,
                                    site_id,
                                    Guid,
                                    MasterEntityId,
                                    LastUpdatedBy,
                                    LastUpdatedDate,
                                    CreatedBy,
                                    CreationDate
                                )
                        VALUES
                                (
                                    @Code,
                                    @Phone,
                                    @CountryCode,
                                    @EmailId,
                                    @Source,
                                    @IsVerified,
                                    @RemainingAttempts,
                                    @ExpiryTime,
                                    @IsActive,
                                    @site_id,
                                    NewId(),
                                    @MasterEntityId,
                                    @LastUpdatedBy,
                                    GetDate(),
                                    @CreatedBy,
                                    GetDate()
                                )
                                SELECT * FROM  GenericOTP WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(genericOTPDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshGenericOTPDTO(genericOTPDTO, dt);
            }
            catch(Exception ex)
            {
                log.Error("Error occured while updating the Generuc record", ex);
                log.LogVariableState("GenericOTPDTO", genericOTPDTO);
                log.LogMethodExit(null, "throwing exception" + ex.Message);
                throw;
            }
            log.LogMethodExit(genericOTPDTO);
            return genericOTPDTO;
        }

        /// <summary>
        /// Updates the GenericOTP record
        /// </summary>
        /// <param name="genericOTPDTO">GenericOTPDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns updated Generic record</returns>
        public GenericOTPDTO UpdateRecord(GenericOTPDTO genericOTPDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(genericOTPDTO, loginId, siteId);
            string query = @" UPDATE GenericOTP
                                SET Code = @Code,
                                    Phone = @Phone,
                                    CountryCode = @CountryCode,
                                    EmailId = @EmailId,
                                    Source = @Source,
                                    IsVerified = @IsVerified,
                                    RemainingAttempts = @RemainingAttempts,
                                    ExpiryTime = @ExpiryTime,
                                    IsActive = @IsActive,
                                    site_id = @site_id,
                                    MasterEntityId = @MasterEntityId,
                                    LastUpdatedBy= @LastUpdatedBy,
                                    LastUpdatedDate = GetDate()
                                WHERE Id = @Id
                                SELECT * FROM GenericOTP WHERE Id = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(genericOTPDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshGenericOTPDTO(genericOTPDTO, dt);
            }
            catch(Exception ex)
            {
                log.Error("Error occurred while updating the GenericOTP record", ex);
                log.LogVariableState("GenericOTPDTO", genericOTPDTO);
                log.LogMethodExit(null, "throwing exception" + ex.Message);
                throw;
            }
            log.LogMethodExit(genericOTPDTO);
            return genericOTPDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="genericOTPDTO">GenericOTPDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshGenericOTPDTO(GenericOTPDTO genericOTPDTO, DataTable dt)
        {
            log.LogMethodEntry(genericOTPDTO, dt);
            if(dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                genericOTPDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                genericOTPDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                genericOTPDTO.Guid = dataRow["Guid"] == DBNull.Value? string.Empty : dataRow["Guid"].ToString();
                genericOTPDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString();
                genericOTPDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString();
                genericOTPDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to GenericDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns GenericDTO</returns>
        public GenericOTPDTO GetGenericOTPDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            GenericOTPDTO genericOTPDTO = new GenericOTPDTO(Convert.ToInt32(dataRow["id"]),
                                                            dataRow["Code"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Code"]),
                                                            dataRow["Phone"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Phone"]),
                                                            dataRow["CountryCode"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CountryCode"]),
                                                            dataRow["EmailId"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["EmailId"]),
                                                            dataRow["Source"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Source"]),
                                                            dataRow["IsVerified"] == DBNull.Value ? true : dataRow["IsVerified"].ToString() == "Y",
                                                            dataRow["RemainingAttempts"] == DBNull.Value ? 1 : Convert.ToInt32(dataRow["RemainingAttempts"]),
                                                            dataRow["ExpiryTime"] == DBNull.Value ? DateTime.MaxValue : Convert.ToDateTime(dataRow["ExpiryTime"]),
                                                            dataRow["IsActive"] == DBNull.Value ? true : dataRow["IsActive"].ToString() == "Y",
                                                            dataRow["Guid"] == DBNull.Value ? "" : dataRow["Guid"].ToString(),
                                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString(),
                                                            dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString(),
                                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"])
                                                            );
            log.LogMethodExit(genericOTPDTO);
            return genericOTPDTO;
        }

        /// <summary>
        /// Gets the GenericOTP data of passed Id
        /// </summary>
        /// <param name="Id">integer type parameter</param>
        /// <returns>Returns GenericOTP</returns>
        public GenericOTPDTO GetGenericOTPDTO(int Id)
        {
            log.LogMethodEntry(Id);
            GenericOTPDTO returnValue = null;
            string query = SELECT_QUERY + @" WHERE go.Id = @Id ";
            SqlParameter parameter = new SqlParameter("@Id", Id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetGenericOTPDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Returns the List of GenericOTPDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of GenericOTPDTO </returns>
        public List<GenericOTPDTO> GetGenericOTPDTOList(List<KeyValuePair<GenericOTPDTO.SearchByParameters, string>> searchParameters , SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<GenericOTPDTO> genericOTPDTOList = new List<GenericOTPDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach(KeyValuePair<GenericOTPDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " AND ";
                    if(DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == GenericOTPDTO.SearchByParameters.ID ||
                           searchParameter.Key == GenericOTPDTO.SearchByParameters.MASTER_ENTITY_ID ||
                           searchParameter.Key == GenericOTPDTO.SearchByParameters.REMAINING_ATTEMPTS)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == GenericOTPDTO.SearchByParameters.CODE ||
                            searchParameter.Key == GenericOTPDTO.SearchByParameters.EMAIL_ID ||
                            searchParameter.Key == GenericOTPDTO.SearchByParameters.SOURCE ||
                           searchParameter.Key == GenericOTPDTO.SearchByParameters.PHONE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToString(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == GenericOTPDTO.SearchByParameters.IS_VERIFIED ||
                                searchParameter.Key == GenericOTPDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == GenericOTPDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
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
            if(dataTable.Rows.Count > 0)
            {
                foreach(DataRow dataRow in dataTable.Rows)
                {
                    genericOTPDTOList.Add(GetGenericOTPDTO(dataRow));   
                }
            }
            log.LogMethodExit(genericOTPDTOList);
            return genericOTPDTOList;
        }
    }
}
