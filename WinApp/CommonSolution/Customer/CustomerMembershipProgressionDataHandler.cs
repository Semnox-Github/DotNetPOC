/********************************************************************************************
 * Project Name - Customer Membership Progression Data Handler
 * Description  - Data handler of the Customer Membership Progression class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2       19-Jul-2019    Girish Kundar   Modified :Structure of data Handler - insert /Update methods
 *                                                    Fix for SQL Injection Issue  
 *2.70.2        05-Dec-2019   Jinto Thomas            Removed siteid from update query                                                    
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;


namespace Semnox.Parafait.Customer
{
    /// <summary>
    ///  CustomerMembershipProgression Data Handler - Handles insert, update and select of  MembershipProgression 
    /// </summary>
    public class CustomerMembershipProgressionDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private static readonly Dictionary<CustomerMembershipProgressionDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<CustomerMembershipProgressionDTO.SearchByParameters, string>
            {
                {CustomerMembershipProgressionDTO.SearchByParameters.ID, "msp.Id"},
                {CustomerMembershipProgressionDTO.SearchByParameters.CARD_TYPE_ID, "msp.CardTypeId"},
                {CustomerMembershipProgressionDTO.SearchByParameters.Card_ID,"msp.CardId"},
                {CustomerMembershipProgressionDTO.SearchByParameters.CUSTOMER_ID, "msp.CustomerId"},
                {CustomerMembershipProgressionDTO.SearchByParameters.MASTERENTITY_ID, "msp.MasterEntityId"},
                {CustomerMembershipProgressionDTO.SearchByParameters.MEMBERSHIP_ID, "msp.MembershipId"},
                {CustomerMembershipProgressionDTO.SearchByParameters.SITE_ID, "msp.site_id"}
            };
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * from MembershipProgression AS msp";
        /// <summary>
        /// Default constructor of CustomerMembershipProgressionDataHandler class
        /// </summary>
        public CustomerMembershipProgressionDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating MembershipProgression Record.
        /// </summary>
        /// <param name="customerMembershipProgressionDTO">CustomerMembershipProgressionDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(CustomerMembershipProgressionDTO customerMembershipProgressionDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(customerMembershipProgressionDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@MembershipProgressionId", customerMembershipProgressionDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CardId", customerMembershipProgressionDTO.CardId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CardTypeId", customerMembershipProgressionDTO.CardTypeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@EffectiveDate", customerMembershipProgressionDTO.EffectiveDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Site_Id", customerMembershipProgressionDTO.Site_Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Guid", customerMembershipProgressionDTO.Guid));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SynchStatus", customerMembershipProgressionDTO.SynchStatus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", customerMembershipProgressionDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MembershipId", customerMembershipProgressionDTO.MembershipId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CustomerId", customerMembershipProgressionDTO.CustomerId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@EffectiveFromDate", customerMembershipProgressionDTO.EffectiveFromDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@EffectiveToDate", customerMembershipProgressionDTO.EffectiveToDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastRetentionDate", customerMembershipProgressionDTO.LastRetentionDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", customerMembershipProgressionDTO.CreatedBy));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreationDate", customerMembershipProgressionDTO.CreationDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", customerMembershipProgressionDTO.LastUpdatedBy));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdateDate", customerMembershipProgressionDTO.LastUpdateDate));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the MembershipProgression record to the database
        /// </summary>
        /// <param name="customerMembershipProgressionDTO">CustomerMembershipProgressionDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns CustomerMembershipProgressionDTO</returns>
        public CustomerMembershipProgressionDTO InsertMembershipProgression(CustomerMembershipProgressionDTO customerMembershipProgressionDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(customerMembershipProgressionDTO, loginId, siteId);
            string query = @"INSERT INTO MembershipProgression 
                                        ( 
                                              CardId
                                            , CardTypeId
                                            , EffectiveDate
                                            , site_id
                                            , Guid
                                            , MasterEntityId
                                            , MembershipId
                                            , CustomerId
                                            , EffectiveFromDate
                                            , EffectiveToDate
                                            , LastRetentionDate
                                            , CreatedBy
                                            , CreationDate
                                            , LastUpdatedBy
                                            , LastUpdateDate
                                        ) 
                                VALUES 
                                        (
                                             @CardId
                                            ,@CardTypeId
                                            ,@EffectiveDate
                                            ,@site_id
                                            ,NEWID()
                                            ,@MasterEntityId
                                            ,@MembershipId
                                            ,@CustomerId
                                            ,@EffectiveFromDate
                                            ,@EffectiveToDate
                                            ,@LastRetentionDate
                                            ,@CreatedBy
                                            ,GETDATE()
                                            ,@LastUpdatedBy
                                            ,GETDATE()
                                        ) SELECT * FROM MembershipProgression WHERE Id  = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(customerMembershipProgressionDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCustomerMembershipProgressionDTO(customerMembershipProgressionDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting customerMembershipProgressionDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(customerMembershipProgressionDTO);
            return customerMembershipProgressionDTO;
        }

        /// <summary>
        /// Updates the MembershipProgression record
        /// </summary>
        /// <param name="customerMembershipProgressionDTO">CustomerMembershipProgressionDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns CustomerMembershipProgressionDTO</returns>
        public CustomerMembershipProgressionDTO UpdateMembershipProgression(CustomerMembershipProgressionDTO customerMembershipProgressionDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(customerMembershipProgressionDTO, loginId, siteId);
            string query = @"UPDATE MembershipProgression 
                                SET CardId = @CardId
                                  , CardTypeId = @CardTypeId
                                  , EffectiveDate = @EffectiveDate
                                 -- , site_id = @Site_Id 
                                  , MasterEntityId = @MasterEntityId
                                  , MembershipId = @MembershipId
                                  , CustomerId = @CustomerId
                                  , EffectiveFromDate = @EffectiveFromDate
                                  , EffectiveToDate = @EffectiveToDate
                                  , LastRetentionDate = @LastRetentionDate 
                                  , LastUpdatedBy = @LastUpdatedBy
                                  , LastUpdateDate = GETDATE()
                             WHERE Id = @MembershipProgressionId 
                             SELECT * FROM MembershipProgression WHERE Id  =  @MembershipProgressionId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(customerMembershipProgressionDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCustomerMembershipProgressionDTO(customerMembershipProgressionDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating customerMembershipProgressionDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(customerMembershipProgressionDTO);
            return customerMembershipProgressionDTO;
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="customerMembershipProgressionDTO">CustomerMembershipProgressionDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshCustomerMembershipProgressionDTO(CustomerMembershipProgressionDTO customerMembershipProgressionDTO, DataTable dt)
        {
            log.LogMethodEntry(customerMembershipProgressionDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                customerMembershipProgressionDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                customerMembershipProgressionDTO.LastUpdateDate = dataRow["LastupdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdateDate"]);
                customerMembershipProgressionDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                customerMembershipProgressionDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                customerMembershipProgressionDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                customerMembershipProgressionDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                customerMembershipProgressionDTO.Site_Id = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }



        /// <summary>
        /// Converts the Data row object to CustomerMembershipProgressionDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns CustomerMembershipProgressionDTO</returns>
        private CustomerMembershipProgressionDTO GetCustomerMembershipProgressionDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            CustomerMembershipProgressionDTO customerMembershipProgressionDTO = new CustomerMembershipProgressionDTO(Convert.ToInt32(dataRow["Id"]),
                                            dataRow["CardId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CardId"]),
                                            dataRow["CardTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CardTypeId"]),
                                            dataRow["EffectiveDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["EffectiveDate"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["MembershipId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MembershipId"]),
                                            dataRow["CustomerId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CustomerId"]),
                                            dataRow["EffectiveFromDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["EffectiveFromDate"]),
                                            dataRow["EffectiveToDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["EffectiveToDate"]),
                                            dataRow["LastRetentionDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["LastRetentionDate"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                            dataRow["CreationDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                            dataRow["LastUpdateDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["LastUpdateDate"])
                                            );
            log.LogMethodExit(customerMembershipProgressionDTO);
            return customerMembershipProgressionDTO;
        }

        /// <summary>
        /// Gets the MembershipProgression data of passed MembershipProgression Id
        /// </summary>
        /// <param name="membershipProgressionId">integer type parameter</param>
        /// <returns>Returns CustomerMembershipProgressionDTO</returns>
        public CustomerMembershipProgressionDTO GetCustomerMembershipProgressionDTO(int membershipProgressionId)
        {
            log.LogMethodEntry(membershipProgressionId);
            CustomerMembershipProgressionDTO returnValue = null;
            string query = SELECT_QUERY + "  WHERE msp.Id = @MembershipProgressionId";
            SqlParameter parameter = new SqlParameter("@MembershipProgressionId", membershipProgressionId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetCustomerMembershipProgressionDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }


        /// <summary>
        /// Gets the CustomerMembershipProgressionDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of CustomerMembershipProgressionDTO matching the search criteria</returns>
        public List<CustomerMembershipProgressionDTO> GetCustomerMembershipProgressionDTOList(List<KeyValuePair<CustomerMembershipProgressionDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<CustomerMembershipProgressionDTO> list = null;
            int count = 0;
            string selectQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<CustomerMembershipProgressionDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if ((searchParameter.Key == CustomerMembershipProgressionDTO.SearchByParameters.ID) ||
                            (searchParameter.Key == CustomerMembershipProgressionDTO.SearchByParameters.Card_ID) ||
                            (searchParameter.Key == CustomerMembershipProgressionDTO.SearchByParameters.CARD_TYPE_ID) ||
                            (searchParameter.Key == CustomerMembershipProgressionDTO.SearchByParameters.CUSTOMER_ID) ||
                            (searchParameter.Key == CustomerMembershipProgressionDTO.SearchByParameters.MASTERENTITY_ID) ||
                            (searchParameter.Key == CustomerMembershipProgressionDTO.SearchByParameters.MEMBERSHIP_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CustomerMembershipProgressionDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
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
                list = new List<CustomerMembershipProgressionDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    CustomerMembershipProgressionDTO customerMembershipProgressionDTO = GetCustomerMembershipProgressionDTO(dataRow);
                    list.Add(customerMembershipProgressionDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// GetCustomerMembershipProgressionByCustomerIds
        /// </summary>
        /// <param name="custIdList"></param>
        /// <returns></returns>
        public List<CustomerMembershipProgressionDTO> GetCustomerMembershipProgressionByCustomerIds(List<int> custIdList)
        {
            log.LogMethodEntry(custIdList);
            List<CustomerMembershipProgressionDTO> list = new List<CustomerMembershipProgressionDTO>();
            string query = SELECT_QUERY + @" , @custIdList List WHERE msp.CustomerId = List.Id ";
            DataTable table = dataAccessHandler.BatchSelect(query, "@custIdList", custIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetCustomerMembershipProgressionDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }
    }
}
