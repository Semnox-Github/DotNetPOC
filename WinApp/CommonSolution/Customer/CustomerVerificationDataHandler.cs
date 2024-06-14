/********************************************************************************************
 * Project Name - CustomerVerification Data Handler
 * Description  - Data handler of the CustomerVerification class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        06-Feb-2017   Lakshminarayana     Created 
 *2.70.2       19-Jul-2019     Girish Kundar       Modified :Structure of data Handler - insert /Update methods
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    ///  CustomerVerification Data Handler - Handles insert, update and select of  CustomerVerification objects
    /// </summary>
    public class CustomerVerificationDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private static readonly Dictionary<CustomerVerificationDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<CustomerVerificationDTO.SearchByParameters, string>
            {
                {CustomerVerificationDTO.SearchByParameters.ID, "cv.Id"},
                {CustomerVerificationDTO.SearchByParameters.CUSTOMER_ID, "cv.CustomerId"},
                {CustomerVerificationDTO.SearchByParameters.PROFILE_ID, "cv.ProfileId"},
                {CustomerVerificationDTO.SearchByParameters.VERIFICATION_CODE, "cv.VerificationCode"},
                {CustomerVerificationDTO.SearchByParameters.IS_ACTIVE,"cv.IsActive"},
                {CustomerVerificationDTO.SearchByParameters.MASTER_ENTITY_ID,"cv.MasterEntityId"},
                {CustomerVerificationDTO.SearchByParameters.SITE_ID, "cv.site_id"}
            };
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * from CustomerVerification AS cv";
        /// <summary>
        /// Default constructor of CustomerVerificationDataHandler class
        /// </summary>
        public CustomerVerificationDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating CustomerVerification Record.
        /// </summary>
        /// <param name="customerVerificationDTO">CustomerVerificationDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(CustomerVerificationDTO customerVerificationDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(customerVerificationDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", customerVerificationDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CustomerId", customerVerificationDTO.CustomerId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProfileId", customerVerificationDTO.ProfileId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Source", customerVerificationDTO.Source));
            parameters.Add(dataAccessHandler.GetSQLParameter("@VerificationCode", customerVerificationDTO.VerificationCode));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ExpiryDate", customerVerificationDTO.ExpiryDate == DateTime.MinValue ? DBNull.Value : (object)customerVerificationDTO.ExpiryDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", customerVerificationDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", customerVerificationDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the CustomerVerification record to the database
        /// </summary>
        /// <param name="customerVerificationDTO">CustomerVerificationDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns CustomerVerificationDTO</returns>
        public CustomerVerificationDTO InsertCustomerVerification(CustomerVerificationDTO customerVerificationDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(customerVerificationDTO, loginId, siteId);
            string query = @"INSERT INTO CustomerVerification 
                                        ( 
                                            CustomerId,
                                            ProfileId,
                                            Source,
                                            VerificationCode,
                                            ExpiryDate,
                                            IsActive,
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdateUser,
                                            LastUpdateDate,
                                            site_id,
                                            MasterEntityId

                                        ) 
                                VALUES 
                                        (
                                            @CustomerId,
                                            @ProfileId,
                                            @Source,
                                            @VerificationCode,
                                            @ExpiryDate,
                                            @IsActive,
                                            @CreatedBy,
                                            GETDATE(),
                                            @LastUpdatedBy,
                                            GETDATE(),
                                            @site_id,
                                            @MasterEntityId
                                        ) SELECT * FROM CustomerVerification WHERE Id  = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(customerVerificationDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCustomerVerificationDTO(customerVerificationDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting customerVerificationDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(customerVerificationDTO);
            return customerVerificationDTO;
        }

        /// <summary>
        /// Updates the CustomerVerification record
        /// </summary>
        /// <param name="customerVerificationDTO">CustomerVerificationDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns CustomerVerificationDTO</returns>
        public CustomerVerificationDTO UpdateCustomerVerification(CustomerVerificationDTO customerVerificationDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(customerVerificationDTO, loginId, siteId);
            string query = @"UPDATE CustomerVerification 
                             SET CustomerId=@CustomerId,
                                 ProfileId=@ProfileId,
                                 Source=@Source,
                                 VerificationCode=@VerificationCode,
                                 ExpiryDate=@ExpiryDate,
                                 IsActive = @IsActive,
                                 LastUpdateUser = @LastUpdatedBy,
                                 LastUpdateDate=GETDATE(),
                                 MasterEntityId=@MasterEntityId
                              WHERE Id = @Id  
                             SELECT * FROM CustomerVerification WHERE Id  = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(customerVerificationDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCustomerVerificationDTO(customerVerificationDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating customerVerificationDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(customerVerificationDTO);
            return customerVerificationDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="customerVerificationDTO">CustomerVerificationDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshCustomerVerificationDTO(CustomerVerificationDTO customerVerificationDTO, DataTable dt)
        {
            log.LogMethodEntry(customerVerificationDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                customerVerificationDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                customerVerificationDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                customerVerificationDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                customerVerificationDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                customerVerificationDTO.LastUpdatedBy = dataRow["LastUpdateUser"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdateUser"]);
                customerVerificationDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                customerVerificationDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }



        /// <summary>
        /// Converts the Data row object to CustomerVerificationDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns CustomerVerificationDTO</returns>
        private CustomerVerificationDTO GetCustomerVerificationDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            CustomerVerificationDTO customerVerificationDTO = new CustomerVerificationDTO(Convert.ToInt32(dataRow["Id"]),
                                            dataRow["CustomerId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CustomerId"]),
                                            dataRow["ProfileId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ProfileId"]),
                                            dataRow["Source"] == DBNull.Value ? "" : Convert.ToString(dataRow["Source"]),
                                            dataRow["VerificationCode"] == DBNull.Value ? "" : dataRow["VerificationCode"].ToString(),
                                            dataRow["ExpiryDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["ExpiryDate"]),
                                            dataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["IsActive"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? "" : dataRow["CreatedBy"].ToString(),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdateUser"] == DBNull.Value ? "" : dataRow["LastUpdateUser"].ToString(),
                                            dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["Guid"] == DBNull.Value ? "" : dataRow["Guid"].ToString()
                                            );
            log.LogMethodExit(customerVerificationDTO);
            return customerVerificationDTO;
        }

        /// <summary>
        /// Gets the CustomerVerification data of passed CustomerVerification Id
        /// </summary>
        /// <param name="customerVerificationId">integer type parameter</param>
        /// <returns>Returns CustomerVerificationDTO</returns>
        public CustomerVerificationDTO GetCustomerVerificationDTO(int customerVerificationId)
        {
            log.LogMethodEntry(customerVerificationId);
            CustomerVerificationDTO returnValue = null;
            string query = SELECT_QUERY + "    WHERE cv.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", customerVerificationId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetCustomerVerificationDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }


        /// <summary>
        /// Gets the CustomerVerificationDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of CustomerVerificationDTO matching the search criteria</returns>
        public List<CustomerVerificationDTO> GetCustomerVerificationDTOList(List<KeyValuePair<CustomerVerificationDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<CustomerVerificationDTO> list = null;
            string selectQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<CustomerVerificationDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == CustomerVerificationDTO.SearchByParameters.ID ||
                            searchParameter.Key == CustomerVerificationDTO.SearchByParameters.CUSTOMER_ID ||
                            searchParameter.Key == CustomerVerificationDTO.SearchByParameters.PROFILE_ID||
                            searchParameter.Key == CustomerVerificationDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {

                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));

                        }
                        else if (searchParameter.Key == CustomerVerificationDTO.SearchByParameters.SITE_ID)
                        {

                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CustomerVerificationDTO.SearchByParameters.IS_ACTIVE)
                        {

                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == CustomerVerificationDTO.SearchByParameters.VERIFICATION_CODE)
                        {

                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else
                        {

                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                    }
                    else
                    {
                        log.LogMethodExit(null, "throwing exception");
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                    counter++;
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<CustomerVerificationDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    CustomerVerificationDTO customerVerificationDTO = GetCustomerVerificationDTO(dataRow);
                    list.Add(customerVerificationDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }

    }
}
