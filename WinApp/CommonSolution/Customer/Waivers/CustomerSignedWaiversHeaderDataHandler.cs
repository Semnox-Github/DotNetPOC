/********************************************************************************************
 * Project Name - CustomerSignedWaiverHeader Data Handler
 * Description  - Data handler of the CustomerSignedWaiverHeader class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.70.2       26-Sep-2019     Deeksha           Created for waiver phase 2
 *2.70.2       05-Dec-2019     Jinto Thomas      Removed siteid from update query
 *2.140.0      14-Sep-2021     Guru S A          Waiver mapping UI enhancements
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer.Waivers
{
    public class CustomerSignedWaiversHeaderDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"Select * from CustomerSignedWaiverHeader as csh ";

        /// <summary>
        /// Dictionary for searching Parameters for the CustomerSignedWaiver object.
        /// </summary>
        private static readonly Dictionary<CustomerSignedWaiverHeaderDTO.SearchByCSWHeaderParameters, string> DBSearchParameters = new Dictionary<CustomerSignedWaiverHeaderDTO.SearchByCSWHeaderParameters, string>
            {
                {CustomerSignedWaiverHeaderDTO.SearchByCSWHeaderParameters.POS_MACHINE_ID, "csh.PosMachineId"},
                {CustomerSignedWaiverHeaderDTO.SearchByCSWHeaderParameters.CUSTOMER_SIGNED_WAIVER_HEADER_ID, "csh.CustomerSignedWaiverHeaderId"},
                {CustomerSignedWaiverHeaderDTO.SearchByCSWHeaderParameters.SIGNED_BY,"csh.SignedBy"},
                {CustomerSignedWaiverHeaderDTO.SearchByCSWHeaderParameters.SITE_ID, "csh.site_id"},
                {CustomerSignedWaiverHeaderDTO.SearchByCSWHeaderParameters.IS_ACTIVE , "csh.IsActive"},
                {CustomerSignedWaiverHeaderDTO.SearchByCSWHeaderParameters.MASTER_ENTITY_ID, "csh.MasterEntityId"},
            };

        /// <summary>
        /// Default constructor of CustomerSignedWaiversHeaderDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public CustomerSignedWaiversHeaderDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodEntry();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating CustomerSignedWaiverHeaderDTO parameters Record.
        /// </summary>
        /// <param name="customerSignedWaiverHeaderDTO">CustomerSignedWaiverHeaderDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>  Returns the list of SQL parameter </returns>
        private List<SqlParameter> GetSQLParameters(CustomerSignedWaiverHeaderDTO customerSignedWaiverHeaderDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(customerSignedWaiverHeaderDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@customerSignedWaiverHeaderId", customerSignedWaiverHeaderDTO.CustomerSignedWaiverHeaderId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@signedBy", customerSignedWaiverHeaderDTO.SignedBy, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@signedDate", customerSignedWaiverHeaderDTO.SignedDate == DateTime.MinValue ? DBNull.Value : (object)customerSignedWaiverHeaderDTO.SignedDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@posMachineId", customerSignedWaiverHeaderDTO.PosMachineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@waiverCode", string.IsNullOrEmpty(customerSignedWaiverHeaderDTO.WaiverCode) ? DBNull.Value : (object)customerSignedWaiverHeaderDTO.WaiverCode));
            parameters.Add(dataAccessHandler.GetSQLParameter("@channel", string.IsNullOrEmpty(customerSignedWaiverHeaderDTO.Channel) ? DBNull.Value : (object)customerSignedWaiverHeaderDTO.Channel));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", customerSignedWaiverHeaderDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", customerSignedWaiverHeaderDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the CustomerSignedWaiverHeaderDTO record to the database
        /// </summary>
        /// <param name="customerSignedWaiverHeaderDTO">CustomerSignedWaiverHeaderDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>CustomerSignedWaiverHeaderDTO</returns>
        public CustomerSignedWaiverHeaderDTO InsertCustomerSignedWaiverHeader(CustomerSignedWaiverHeaderDTO customerSignedWaiverHeaderDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(customerSignedWaiverHeaderDTO, loginId, siteId);
            string query = @"INSERT INTO CustomerSignedWaiverHeader
                                        ( 
                                            SignedBy,
                                            SignedDate,
                                            Channel,
                                            PosMachineId,
                                            IsActive,
                                            WaiverCode,
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
                                            @signedBy,
                                            @signedDate,
                                            Upper(@channel),
                                            @posMachineId,
                                            @isActive,            
                                            @waiverCode,
                                            NEWID(),
                                            @site_id,
                                            @masterEntityId,
                                            @createdBy,
                                            GETDATE(),
                                            @lastUpdatedBy,
                                            GETDATE()
                                        ) SELECT * FROM CustomerSignedWaiverHeader WHERE CustomerSignedWaiverHeaderId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(customerSignedWaiverHeaderDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCustomerSignedWaiverHeaderDTO(customerSignedWaiverHeaderDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting customerSignedWaiverHeaderDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(customerSignedWaiverHeaderDTO);
            return customerSignedWaiverHeaderDTO;
        }

        

        /// <summary>
        /// Updates the customerSignedWaiver record
        /// </summary>
        /// <param name="customerSignedWaiverHeaderDTO">CustomerSignedWaiverHeaderDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>CustomerSignedWaiverHeaderDTO</returns>
        public CustomerSignedWaiverHeaderDTO UpdateCustomerSignedWaiverHeader(CustomerSignedWaiverHeaderDTO customerSignedWaiverHeaderDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(customerSignedWaiverHeaderDTO, loginId, siteId);
            string query = @"UPDATE CustomerSignedWaiverHeader 
                             SET SignedBy = @signedBy,
                                 SignedDate = @signedDate,
                                 Channel = Upper(@channel),
                                 PosMachineId = @posMachineId,
                                 IsActive = @isActive,
                                 WaiverCode = @waiverCode,
                                 LastUpdatedBy=@lastUpdatedBy,
                                 LastUpdateDate= GETDATE(),
                                 --site_id=@site_id,
                                 MasterEntityId=@masterEntityId
                             WHERE CustomerSignedWaiverHeaderId = @customerSignedWaiverHeaderId
                             SELECT * FROM CustomerSignedWaiverHeader WHERE CustomerSignedWaiverHeaderId = @customerSignedWaiverHeaderId ";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(customerSignedWaiverHeaderDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCustomerSignedWaiverHeaderDTO(customerSignedWaiverHeaderDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating customerSignedWaiverHeaderDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(customerSignedWaiverHeaderDTO);
            return customerSignedWaiverHeaderDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="customerSignedWaiverHeaderDTO">customerSignedWaiverHeaderDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshCustomerSignedWaiverHeaderDTO(CustomerSignedWaiverHeaderDTO customerSignedWaiverHeaderDTO, DataTable dt)
        {
            log.LogMethodEntry(customerSignedWaiverHeaderDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                customerSignedWaiverHeaderDTO.CustomerSignedWaiverHeaderId = Convert.ToInt32(dt.Rows[0]["CustomerSignedWaiverHeaderId"]);
                customerSignedWaiverHeaderDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                customerSignedWaiverHeaderDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                customerSignedWaiverHeaderDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                customerSignedWaiverHeaderDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                customerSignedWaiverHeaderDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                customerSignedWaiverHeaderDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to CustomerSignedWaiverHeaderDTO class type
        /// </summary>
        /// <param name="customerSignedWaiverHeaderDTODataRow">CustomerSignedWaiverHeader DataRow</param>
        /// <returns>Returns CustomerSignedWaiver</returns>
        private CustomerSignedWaiverHeaderDTO GetCustomerSignedWaiverHeaderDTO(DataRow customerSignedWaiverHeaderDTODataRow)
        {
            log.LogMethodEntry(customerSignedWaiverHeaderDTODataRow);
            CustomerSignedWaiverHeaderDTO customerSignedWaiverHeaderDataObject = new CustomerSignedWaiverHeaderDTO(Convert.ToInt32(customerSignedWaiverHeaderDTODataRow["CustomerSignedWaiverHeaderId"]),
                                            customerSignedWaiverHeaderDTODataRow["SignedBy"] == DBNull.Value ? -1 : Convert.ToInt32(customerSignedWaiverHeaderDTODataRow["SignedBy"]),
                                            customerSignedWaiverHeaderDTODataRow["SignedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(customerSignedWaiverHeaderDTODataRow["SignedDate"]),
                                            customerSignedWaiverHeaderDTODataRow["Channel"] == DBNull.Value ? string.Empty : customerSignedWaiverHeaderDTODataRow["Channel"].ToString().ToUpper(),
                                            customerSignedWaiverHeaderDTODataRow["PosMachineId"] == DBNull.Value ? -1 : Convert.ToInt32(customerSignedWaiverHeaderDTODataRow["PosMachineId"]),
                                            customerSignedWaiverHeaderDTODataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(customerSignedWaiverHeaderDTODataRow["IsActive"]),
                                            customerSignedWaiverHeaderDTODataRow["WaiverCode"] == DBNull.Value ? string.Empty : customerSignedWaiverHeaderDTODataRow["WaiverCode"].ToString(),
                                            customerSignedWaiverHeaderDTODataRow["Guid"] == DBNull.Value ? string.Empty : customerSignedWaiverHeaderDTODataRow["Guid"].ToString(),
                                            customerSignedWaiverHeaderDTODataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(customerSignedWaiverHeaderDTODataRow["site_id"]),
                                            customerSignedWaiverHeaderDTODataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(customerSignedWaiverHeaderDTODataRow["SynchStatus"]),
                                            customerSignedWaiverHeaderDTODataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(customerSignedWaiverHeaderDTODataRow["MasterEntityId"]),
                                            customerSignedWaiverHeaderDTODataRow["CreatedBy"] == DBNull.Value ? string.Empty : customerSignedWaiverHeaderDTODataRow["CreatedBy"].ToString(),
                                            customerSignedWaiverHeaderDTODataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(customerSignedWaiverHeaderDTODataRow["CreationDate"]),
                                            customerSignedWaiverHeaderDTODataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : customerSignedWaiverHeaderDTODataRow["LastUpdatedBy"].ToString(),
                                            customerSignedWaiverHeaderDTODataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(customerSignedWaiverHeaderDTODataRow["LastUpdateDate"])
                                            );
            log.LogMethodExit(customerSignedWaiverHeaderDataObject);
            return customerSignedWaiverHeaderDataObject;
        }

        /// <summary>
        /// Gets the customer Signed Waiver Detail of passed CustomerSignedWaiverHeaderId
        /// </summary>
        /// <param name="CustomerSignedWaiverHeaderId">integer type parameter</param>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        /// <returns>Returns CustomerSignedWaiverHeaderDTO</returns>
        public CustomerSignedWaiverHeaderDTO GetCustomerSignedWaiverHeaderDTO(int customerSignedWaiverHeaderId)
        {
            log.LogMethodEntry(customerSignedWaiverHeaderId);
            CustomerSignedWaiverHeaderDTO result = null;
            string selectCustomerSignedWaiverQuery = SELECT_QUERY + @" WHERE CustomerSignedWaiverHeaderId = @customerSignedWaiverHeaderId";
            SqlParameter parameter = new SqlParameter("@customerSignedWaiverHeaderId", customerSignedWaiverHeaderId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectCustomerSignedWaiverQuery, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetCustomerSignedWaiverHeaderDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the CustomerSignedWaiverHeaderDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        /// <returns>Returns the list of CustomerSignedWaiverHeaderDTO matching the search criteria</returns>
        public List<CustomerSignedWaiverHeaderDTO> GetAllCustomerSignedWaiverHeaderList(List<KeyValuePair<CustomerSignedWaiverHeaderDTO.SearchByCSWHeaderParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<CustomerSignedWaiverHeaderDTO> customerSignedWaiverHeaderDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<CustomerSignedWaiverHeaderDTO.SearchByCSWHeaderParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == CustomerSignedWaiverHeaderDTO.SearchByCSWHeaderParameters.CUSTOMER_SIGNED_WAIVER_HEADER_ID
                            || searchParameter.Key == CustomerSignedWaiverHeaderDTO.SearchByCSWHeaderParameters.POS_MACHINE_ID
                            || searchParameter.Key == CustomerSignedWaiverHeaderDTO.SearchByCSWHeaderParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CustomerSignedWaiverHeaderDTO.SearchByCSWHeaderParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'0')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        
                        else if (searchParameter.Key == CustomerSignedWaiverHeaderDTO.SearchByCSWHeaderParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
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
            if (dataTable.Rows.Count > 0)
            {
                customerSignedWaiverHeaderDTOList = new List<CustomerSignedWaiverHeaderDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    CustomerSignedWaiverHeaderDTO customerSignedWaiverHeaderDTO = GetCustomerSignedWaiverHeaderDTO(dataRow);
                    customerSignedWaiverHeaderDTOList.Add(customerSignedWaiverHeaderDTO);
                }
            }
            log.LogMethodExit(customerSignedWaiverHeaderDTOList);
            return customerSignedWaiverHeaderDTOList;
        }

        /// <summary>
        /// check whether code is already used in an active signed waiver
        /// </summary>
        /// <param name="waiverCodeValue"></param>
        /// <returns></returns>
        internal bool AlreadyUsedActiveWaiverCode(string waiverCodeValue)
        {
            log.LogMethodEntry();
            bool usedCode = false; 
            string selectQry = @"select top 1 1 as recordFound
                                  from CustomerSignedWaiverHeader csw,
                                       CustomerSignedWaiver cs
                                 Where csw.CustomerSignedWaiverHeaderId = cs.CustomerSignedWaiverHeaderId
                                  and csw.WaiverCode = @waiverCodeValue 
                                  and csw.IsActive = 1
                                  and (cs.ExpiryDate is null OR cs.ExpiryDate > getdate()-30)
                                  and cs.IsActive = 1
                                ";
            SqlParameter parameter = new SqlParameter("@waiverCodeValue", waiverCodeValue);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQry, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogVariableState("dataTable.Rows.Count", dataTable.Rows.Count);
            if (dataTable.Rows.Count > 0)
            {
                usedCode = true;
            } 
            log.LogMethodExit(usedCode);
            return usedCode;
        }
    }
}


