/********************************************************************************************
 * Project Name - CustomerSignedWaiver Data Handler
 * Description  - Data handler of the CustomerSignedWaiver class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.70.2      26-Sep-2019     Deeksha           Created for waiver phase 2
 *2.70.2       05-Dec-2019   Jinto Thomas            Removed siteid from update query
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Customer.Waivers
{
    public class CustomerSignedWaiverDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"Select cs.*, cswh.SignedBy, cswh.SignedDate,cswh.WaiverCode, 
                                                     cSignedFor.CustName as signedForName,  cSignedBy.CustName as signedByName,
													 wss.WaiverSetId, wss.description as WaiverSetDescription, wss.waivername, wss.WaiverFileName
                                                from CustomerSignedWaiver as cs left outer join (select ws.waiverSetId, ws.Description , wsd.WaiverSetDetailid,
                                                                                                        wsd.Name as waiverName, wsd.WaiverFileName
                                                                                                   from waiverSet ws, waiversetdetails wsd
                                                                                                  where ws.waiverSetId = wsd.waiverSetId
                                                                                                    ) wss on wss.WaiverSetDetailid = cs.WaiverSetDetailId, 
                                                     CustomerSignedWaiverHeader cswh ,
                                                     (SELECT customers.customer_id, Profile.FirstName + ' '+ ISNULL(Profile.LastName,'')  as CustName
                                                        FROM Customers
                                                             INNER JOIN Profile ON Profile.Id = Customers.ProfileId) cSignedFor, 
                                                     (SELECT customers.customer_id, Profile.FirstName + ' '+ ISNULL(Profile.LastName,'') as CustName
                                                        FROM Customers
                                                             INNER JOIN Profile ON Profile.Id = Customers.ProfileId) cSignedBy                                                     
                                               Where cs.CustomerSignedWaiverHeaderId = cswh.CustomerSignedWaiverHeaderId
                                                 and cs.SignedFor = cSignedFor.customer_id
                                                 and cswh.SignedBy = cSignedBy.Customer_id ";

        /// <summary>
        /// Dictionary for searching Parameters for the CustomerSignedWaiver object.
        /// </summary>
        private static readonly Dictionary<CustomerSignedWaiverDTO.SearchByCSWParameters, string> DBSearchParameters = new Dictionary<CustomerSignedWaiverDTO.SearchByCSWParameters, string>
            {
                {CustomerSignedWaiverDTO.SearchByCSWParameters.CUSTOMER_SIGNED_WAIVER_ID, "cs.CustomerSignedWaiverId"},
                {CustomerSignedWaiverDTO.SearchByCSWParameters.CUSTOMER_SIGNED_WAIVER_ID_LIST, "cs.CustomerSignedWaiverId"},
                {CustomerSignedWaiverDTO.SearchByCSWParameters.CUSTOMER_SIGNED_WAIVER_HEADER_ID, "cs.CustomerSignedWaiverHeaderId"},
                {CustomerSignedWaiverDTO.SearchByCSWParameters.SIGNED_WAIVER_FILE_NAME,"cs.SignedWaiverFileName"},
                {CustomerSignedWaiverDTO.SearchByCSWParameters.WAIVER_SET_DETAIL_ID,"cs.WaiverSetDetailId"},
                {CustomerSignedWaiverDTO.SearchByCSWParameters.EXPIRY_DATE,"cs.ExpiryDate"},
                {CustomerSignedWaiverDTO.SearchByCSWParameters.SITE_ID, "cs.site_id"},
                {CustomerSignedWaiverDTO.SearchByCSWParameters.IS_ACTIVE , "cs.IsActive"},
                {CustomerSignedWaiverDTO.SearchByCSWParameters.MASTER_ENTITY_ID, "cs.MasterEntityId"},
                {CustomerSignedWaiverDTO.SearchByCSWParameters.CUSTOMER_SIGNED_WAIVER_HEADER_ID_LIST , "cs.CustomerSignedWaiverHeaderId"},
                {CustomerSignedWaiverDTO.SearchByCSWParameters.SIGNED_FOR,"cs.SignedFor"},
                {CustomerSignedWaiverDTO.SearchByCSWParameters.SIGNED_BY,"cswh.SignedBy"},
                 {CustomerSignedWaiverDTO.SearchByCSWParameters.IGNORE_EXPIRED,"cs.ExpiryDate"},
                 {CustomerSignedWaiverDTO.SearchByCSWParameters.WAIVER_CODE,"cswh.WaiverCode"},
                 {CustomerSignedWaiverDTO.SearchByCSWParameters.WAIVER_SET_ID,"wss.waiverSetId"},
                  {CustomerSignedWaiverDTO.SearchByCSWParameters.FACILITY_ID,"fac.FacilityId"},
                {CustomerSignedWaiverDTO.SearchByCSWParameters.PRODUCT_ID_LIST,"p.product_id"},
                {CustomerSignedWaiverDTO.SearchByCSWParameters.LAST_UPDATE_FROM_DATE,"cs.LastUpdateDate"},
                {CustomerSignedWaiverDTO.SearchByCSWParameters.LAST_UPDATE_TO_DATE,"cs.LastUpdateDate"},
                {CustomerSignedWaiverDTO.SearchByCSWParameters.SIGNED_FOR_IN,"cs.SignedFor"}
            };

        /// <summary>
        /// Default constructor of CustomerSignedWaiverDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public CustomerSignedWaiverDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodEntry();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating CustomerSignedWaiverDTO parameters Record.
        /// </summary>
        /// <param name="customerSignedWaiverDTO">CustomerSignedWaiverDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>  Returns the list of SQL parameter </returns>
        private List<SqlParameter> GetSQLParameters(CustomerSignedWaiverDTO customerSignedWaiverDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(customerSignedWaiverDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@customerSignedWaiverId", customerSignedWaiverDTO.CustomerSignedWaiverId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@customerSignedWaiverHeaderId", customerSignedWaiverDTO.CustomerSignedWaiverHeaderId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@waiverSetDetailId", customerSignedWaiverDTO.WaiverSetDetailId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@signedFor", customerSignedWaiverDTO.SignedFor, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@deactivatedBy", customerSignedWaiverDTO.DeactivatedBy == null ? DBNull.Value : (object)customerSignedWaiverDTO.DeactivatedBy, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@signedWaiverFileName", string.IsNullOrEmpty(customerSignedWaiverDTO.SignedWaiverFileName) ? DBNull.Value : (object)customerSignedWaiverDTO.SignedWaiverFileName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@deactivationApprovedBy", customerSignedWaiverDTO.DeactivationApprovedBy == null ? DBNull.Value : (object)customerSignedWaiverDTO.DeactivationApprovedBy, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@expiryDate", customerSignedWaiverDTO.ExpiryDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@deactivationDate", customerSignedWaiverDTO.DeactivationDate == null ? DBNull.Value : (object)customerSignedWaiverDTO.DeactivationDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", customerSignedWaiverDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", customerSignedWaiverDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the CustomerSignedWaiverDTO record to the database
        /// </summary>
        /// <param name="customerSignedWaiverDTO">customerSignedWaiverDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>customerSignedWaiverDTO</returns>
        public CustomerSignedWaiverDTO InsertCustomerSignedWaiver(CustomerSignedWaiverDTO customerSignedWaiverDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(customerSignedWaiverDTO, loginId, siteId);
            string query = @"INSERT INTO CustomerSignedWaiver
                                        ( 
                                            CustomerSignedWaiverHeaderId,
                                            WaiverSetDetailId,
                                            SignedWaiverFileName,
                                            SignedFor,
                                            ExpiryDate,
                                            IsActive,
                                            DeactivatedBy,
                                            DeactivationDate,
                                            DeactivationApprovedBy,
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
                                            @customerSignedWaiverHeaderId,
                                            @waiverSetDetailId,
                                            @signedWaiverFileName,
                                            @signedFor,
                                            @expiryDate,
                                            @isActive,            
                                            @deactivatedBy,
                                            @deactivationDate,
                                            @deactivationApprovedBy,
                                            NEWID(),
                                            @site_id,
                                            @masterEntityId,
                                            @createdBy,
                                            GETDATE(),
                                            @lastUpdatedBy,
                                            GETDATE()
                                        ) SELECT * FROM CustomerSignedWaiver WHERE CustomerSignedWaiverId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(customerSignedWaiverDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCustomerSignedWaiverDTO(customerSignedWaiverDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting customerSignedWaiverDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(customerSignedWaiverDTO);
            return customerSignedWaiverDTO;
        }


        /// <summary>
        /// Updates the customerSignedWaiver record
        /// </summary>
        /// <param name="customerSignedWaiverDTO">CustomerSignedWaiverDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>CustomerSignedWaiverDTO</returns>
        public CustomerSignedWaiverDTO UpdateCustomerSignedWaiver(CustomerSignedWaiverDTO customerSignedWaiverDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(customerSignedWaiverDTO, loginId, siteId);
            string query = @"UPDATE CustomerSignedWaiver 
                             SET CustomerSignedWaiverHeaderId = @customerSignedWaiverHeaderId,
                                 WaiverSetDetailId = @waiverSetDetailId,
                                 SignedWaiverFileName = @signedWaiverFileName,
                                 SignedFor = @signedFor,
                                 ExpiryDate = @expiryDate,
                                 IsActive = @isActive,
                                 DeactivatedBy = @deactivatedBy,
                                 DeactivationDate = @deactivationDate,
                                 DeactivationApprovedBy = @deactivationApprovedBy,
                                 LastUpdatedBy=@lastUpdatedBy,
                                 LastUpdateDate= GETDATE(),
                                 --site_id=@site_id,
                                 MasterEntityId=@masterEntityId
                             WHERE CustomerSignedWaiverId = @customerSignedWaiverId
                             SELECT * FROM CustomerSignedWaiver WHERE CustomerSignedWaiverId = @customerSignedWaiverId ";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(customerSignedWaiverDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCustomerSignedWaiverDTO(customerSignedWaiverDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating customerSignedWaiverDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(customerSignedWaiverDTO);
            return customerSignedWaiverDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="customerSignedWaiverDTO">customerSignedWaiverDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshCustomerSignedWaiverDTO(CustomerSignedWaiverDTO customerSignedWaiverDTO, DataTable dt)
        {
            log.LogMethodEntry(customerSignedWaiverDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                customerSignedWaiverDTO.CustomerSignedWaiverId = Convert.ToInt32(dt.Rows[0]["CustomerSignedWaiverId"]);
                customerSignedWaiverDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                customerSignedWaiverDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                customerSignedWaiverDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                customerSignedWaiverDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                customerSignedWaiverDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                customerSignedWaiverDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Converts the Data row object to CustomerSignedWaiverDTO class type
        /// </summary>
        /// <param name="customerSignedWaiverDTODataRow">CustomerSignedWaiver DataRow</param>
        /// <returns>Returns CustomerSignedWaiver</returns>
        private CustomerSignedWaiverDTO GetCustomerSignedWaiverDTO(DataRow customerSignedWaiverDTODataRow)
        {
            log.LogMethodEntry(customerSignedWaiverDTODataRow);
            CustomerSignedWaiverDTO customerSignedWaiverDataObject = new CustomerSignedWaiverDTO(Convert.ToInt32(customerSignedWaiverDTODataRow["customerSignedWaiverId"]),
                                            customerSignedWaiverDTODataRow["CustomerSignedWaiverHeaderId"] == DBNull.Value ? -1 : Convert.ToInt32(customerSignedWaiverDTODataRow["CustomerSignedWaiverHeaderId"]),
                                            customerSignedWaiverDTODataRow["WaiverSetDetailId"] == DBNull.Value ? -1 : Convert.ToInt32(customerSignedWaiverDTODataRow["WaiverSetDetailId"]),
                                            customerSignedWaiverDTODataRow["SignedWaiverFileName"] == DBNull.Value ? string.Empty : customerSignedWaiverDTODataRow["SignedWaiverFileName"].ToString(),
                                            customerSignedWaiverDTODataRow["SignedFor"] == DBNull.Value ? -1 : Convert.ToInt32(customerSignedWaiverDTODataRow["SignedFor"]),
                                            customerSignedWaiverDTODataRow["signedForName"] == DBNull.Value ? string.Empty : customerSignedWaiverDTODataRow["signedForName"].ToString(),
                                            customerSignedWaiverDTODataRow["ExpiryDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(customerSignedWaiverDTODataRow["ExpiryDate"]),
                                            customerSignedWaiverDTODataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(customerSignedWaiverDTODataRow["IsActive"]),
                                            customerSignedWaiverDTODataRow["DeactivatedBy"] == DBNull.Value ? (int?)null : Convert.ToInt32(customerSignedWaiverDTODataRow["DeactivatedBy"]),
                                            customerSignedWaiverDTODataRow["DeactivationDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(customerSignedWaiverDTODataRow["DeactivationDate"]),
                                            customerSignedWaiverDTODataRow["DeactivationApprovedBy"] == DBNull.Value ? (int?)null : Convert.ToInt32(customerSignedWaiverDTODataRow["DeactivationApprovedBy"]),
                                            customerSignedWaiverDTODataRow["Guid"] == DBNull.Value ? string.Empty : customerSignedWaiverDTODataRow["Guid"].ToString(),
                                            customerSignedWaiverDTODataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(customerSignedWaiverDTODataRow["site_id"]),
                                            customerSignedWaiverDTODataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(customerSignedWaiverDTODataRow["SynchStatus"]),
                                            customerSignedWaiverDTODataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(customerSignedWaiverDTODataRow["MasterEntityId"]),
                                            customerSignedWaiverDTODataRow["CreatedBy"] == DBNull.Value ? string.Empty : customerSignedWaiverDTODataRow["CreatedBy"].ToString(),
                                            customerSignedWaiverDTODataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(customerSignedWaiverDTODataRow["CreationDate"]),
                                            customerSignedWaiverDTODataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : customerSignedWaiverDTODataRow["LastUpdatedBy"].ToString(),
                                            customerSignedWaiverDTODataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(customerSignedWaiverDTODataRow["LastUpdateDate"]),
                                            customerSignedWaiverDTODataRow["SignedBy"] == DBNull.Value ? -1 : Convert.ToInt32(customerSignedWaiverDTODataRow["SignedBy"]),
                                            customerSignedWaiverDTODataRow["signedByName"] == DBNull.Value ? string.Empty : customerSignedWaiverDTODataRow["signedByName"].ToString(),
                                            customerSignedWaiverDTODataRow["SignedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(customerSignedWaiverDTODataRow["SignedDate"]),
                                            customerSignedWaiverDTODataRow["WaiverCode"] == DBNull.Value ? string.Empty : customerSignedWaiverDTODataRow["WaiverCode"].ToString(),
                                            customerSignedWaiverDTODataRow["WaiverSetId"] == DBNull.Value ? -1 : Convert.ToInt32(customerSignedWaiverDTODataRow["WaiverSetId"]),
                                            customerSignedWaiverDTODataRow["WaiverSetDescription"] == DBNull.Value ? string.Empty : customerSignedWaiverDTODataRow["WaiverSetDescription"].ToString(),
                                            customerSignedWaiverDTODataRow["waivername"] == DBNull.Value ? string.Empty : customerSignedWaiverDTODataRow["waivername"].ToString(),
                                            customerSignedWaiverDTODataRow["WaiverFileName"] == DBNull.Value ? string.Empty : customerSignedWaiverDTODataRow["WaiverFileName"].ToString()
                                            );
            log.LogMethodExit(customerSignedWaiverDataObject);
            return customerSignedWaiverDataObject;
        }

        /// <summary>
        /// Gets the customer Signed Waiver Detail of passed CustomerSignedWaiverId
        /// </summary>
        /// <param name="CustomerSignedWaiverId">integer type parameter</param>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        /// <returns>Returns CustomerSignedWaiverDTO</returns>
        public CustomerSignedWaiverDTO GetCustomerSignedWaiverDTO(int customerSignedWaiverId)
        {
            log.LogMethodEntry(customerSignedWaiverId);
            CustomerSignedWaiverDTO result = null;
            string selectCustomerSignedWaiverQuery = SELECT_QUERY + @" and cs.CustomerSignedWaiverId = @customerSignedWaiverId";
            SqlParameter parameter = new SqlParameter("@customerSignedWaiverId", customerSignedWaiverId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectCustomerSignedWaiverQuery, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetCustomerSignedWaiverDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the CustomerSignedWaiverDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        /// <returns>Returns the list of CustomerSignedWaiverDTO matching the search criteria</returns>
        public List<CustomerSignedWaiverDTO> GetCustomerSignedWaiverDTOList(List<KeyValuePair<CustomerSignedWaiverDTO.SearchByCSWParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<CustomerSignedWaiverDTO> customerSignedWaiverDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" ");
                foreach (KeyValuePair<CustomerSignedWaiverDTO.SearchByCSWParameters, string> searchParameter in searchParameters)
                {
                    joiner = " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == CustomerSignedWaiverDTO.SearchByCSWParameters.CUSTOMER_SIGNED_WAIVER_HEADER_ID
                            || searchParameter.Key == CustomerSignedWaiverDTO.SearchByCSWParameters.CUSTOMER_SIGNED_WAIVER_ID
                            || searchParameter.Key == CustomerSignedWaiverDTO.SearchByCSWParameters.WAIVER_SET_DETAIL_ID
                            || searchParameter.Key == CustomerSignedWaiverDTO.SearchByCSWParameters.WAIVER_SET_ID
                            || searchParameter.Key == CustomerSignedWaiverDTO.SearchByCSWParameters.MASTER_ENTITY_ID
                            || searchParameter.Key == CustomerSignedWaiverDTO.SearchByCSWParameters.SIGNED_BY
                            || searchParameter.Key == CustomerSignedWaiverDTO.SearchByCSWParameters.SIGNED_FOR)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CustomerSignedWaiverDTO.SearchByCSWParameters.SIGNED_WAIVER_FILE_NAME
                            || searchParameter.Key == CustomerSignedWaiverDTO.SearchByCSWParameters.WAIVER_CODE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == CustomerSignedWaiverDTO.SearchByCSWParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'0')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else if (searchParameter.Key == CustomerSignedWaiverDTO.SearchByCSWParameters.CUSTOMER_SIGNED_WAIVER_HEADER_ID_LIST ||
                              searchParameter.Key == CustomerSignedWaiverDTO.SearchByCSWParameters.CUSTOMER_SIGNED_WAIVER_ID_LIST ||
                             searchParameter.Key == CustomerSignedWaiverDTO.SearchByCSWParameters.SIGNED_FOR_IN)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == CustomerSignedWaiverDTO.SearchByCSWParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CustomerSignedWaiverDTO.SearchByCSWParameters.EXPIRY_DATE ||
                            searchParameter.Key == CustomerSignedWaiverDTO.SearchByCSWParameters.LAST_UPDATE_TO_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) <= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == CustomerSignedWaiverDTO.SearchByCSWParameters.IGNORE_EXPIRED ||
                            searchParameter.Key == CustomerSignedWaiverDTO.SearchByCSWParameters.LAST_UPDATE_FROM_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == CustomerSignedWaiverDTO.SearchByCSWParameters.FACILITY_ID)
                        {
                            query.Append(joiner + @" (exists (
                                                            select 1 
		                                                      from CheckInFacility fac,
                                                                   FacilityWaiver fw, 
			                                                       WaiverSetDetails wsd 
		                                                     where fac.FacilityId = " + dataAccessHandler.GetParameterName(searchParameter.Key) + @"
		                                                       and wsd.WaiverSetDetailId = cs.WaiverSetDetailId
		                                                       and wsd.IsActive = 1
		                                                       and fw.WaiverSetId = wsd.WaiverSetId
		                                                       and fw.IsActive = 1
		                                                       and ISNULL(fw.EffectiveFrom, getdate()) <= getdate()
		                                                       and ISNULL(fw.EffectiveTo,GETDATE()) >= getdate()
		                                                       and fw.FacilityId = fac.FacilityId
		                                                       and fac.active_flag = 'Y'
		                                                       )
                                                               or exists (
                                                               select 1 
                                                               from CheckInFacility fac, 
		                                                            FacilityMapDetails fmd,
		                                                            ProductsAllowedInFacility pac,
		                                                            Products p,
		                                                            WaiverSetDetails wsd 
	                                                            where fac.FacilityId = " + dataAccessHandler.GetParameterName(searchParameter.Key) + @"
	                                                            and wsd.WaiverSetDetailId = cs.WaiverSetDetailId
	                                                            and p.WaiverSetId = wsd.WaiverSetId
	                                                            and p.product_id = pac.ProductsId
	                                                            and pac.FacilityMapId = fmd.FacilityMapId
	                                                            and fmd.FacilityId = fac.FacilityId
                                                                )
                                                            )");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CustomerSignedWaiverDTO.SearchByCSWParameters.PRODUCT_ID_LIST)
                        {
                            query.Append(joiner + @" exists (Select 1 
												              from products p, waiverSet wsp
															  where p.product_id IN (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + @")
															    and p.active_flag = 'Y'
																and p.WaiverSetId = wsp.WaiverSetId
																and wsp.IsActive = 1
																and wsp.WaiverSetId =  wss.WaiverSetId
															union all
															 Select 1 
												              from products p, CheckInFacility facP, FacilityWaiver fwp, FacilityMapDetails fmdp, ProductsAllowedInFacility paifp
															  where p.product_id IN (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + @")
															    and p.active_flag = 'Y'
																and p.product_id = paifp.ProductsId
																and paifp.IsActive = 1
																and paifp.FacilityMapId = fmdp.FacilityMapId
																and fmdp.IsActive = 1
																and fmdp.FacilityId = fwp.FacilityId
																and ISNULL(fwp.EffectiveFrom ,getdate()) <= getdate()
																and ISNULL(fwp.EffectiveTo, getdate()) >= getdate()
																and fwp.IsActive = 1
																and fwp.FacilityId = facP.FacilityId
																and facP.active_flag ='Y'
																and fwp.WaiverSetId = wss.WaiverSetId) ");                            
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
                customerSignedWaiverDTOList = new List<CustomerSignedWaiverDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    CustomerSignedWaiverDTO customerSignedWaiverDTO = GetCustomerSignedWaiverDTO(dataRow);
                    customerSignedWaiverDTOList.Add(customerSignedWaiverDTO);
                }
            }
            log.LogMethodExit(customerSignedWaiverDTOList);
            return customerSignedWaiverDTOList;
        }
        /// <summary>
        /// GetCustomerSignedWaiverDTOList
        /// </summary>
        /// <param name="customerIdList"></param>
        /// <param name="activeRecordsOnly"></param>
        /// <returns></returns>
        internal List<CustomerSignedWaiverDTO> GetCustomerSignedWaiverDTOList(List<int> customerIdList, bool activeRecordsOnly = true)
        {

            log.LogMethodEntry(customerIdList, activeRecordsOnly);
            List<CustomerSignedWaiverDTO> customerSignedWaiverDTOList = new List<CustomerSignedWaiverDTO>();
            string query = SELECT_QUERY + @"  and EXISTS  (select 1 from @CustomerIdList List where (cs.signedFor = List.Id OR cswh.SignedBy = List.Id)) ";
            if (activeRecordsOnly)
            {
                query = query + "AND cs.IsActive = 1 AND ISNULL(cs.ExpiryDate, getdate())>= getdate() ";
            }
            DataTable dataTable = dataAccessHandler.BatchSelect(query, "@CustomerIdList", customerIdList, null, sqlTransaction); 
            if (dataTable.Rows.Count > 0)
            { 
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    CustomerSignedWaiverDTO customerSignedWaiverDTO = GetCustomerSignedWaiverDTO(dataRow);
                    customerSignedWaiverDTOList.Add(customerSignedWaiverDTO);
                }
            }
            log.LogMethodExit(customerSignedWaiverDTOList);
            return customerSignedWaiverDTOList;
        }
    }
}


