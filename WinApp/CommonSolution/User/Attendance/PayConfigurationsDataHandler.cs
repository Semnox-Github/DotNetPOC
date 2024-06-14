/********************************************************************************************
 * Project Name - PayConfigurationsDataHandler
 * Description  - data handler file for  Pay Configurations
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.90.0      03-Jul-2020   Akshay Gulaganji        Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// Pay Configurations Data Handler - Handles insert, update and select of Pay Configurations objects
    /// </summary>
    public class PayConfigurationsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM PayConfigurations as pc ";

        /// <summary>
        /// Dictionary for searching Parameters for the PayConfigurationDetails object
        /// </summary>
        private static readonly Dictionary<PayConfigurationsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<PayConfigurationsDTO.SearchByParameters, string>
        {
            { PayConfigurationsDTO.SearchByParameters.PAY_CONFIGURATION_ID,"pc.PayConfigurationId"},
            { PayConfigurationsDTO.SearchByParameters.PAY_CONFIGURATION_NAME,"pc.PayConfigurationName"},
            { PayConfigurationsDTO.SearchByParameters.PAY_TYPE_ID,"pc.PayTypeId"},
            { PayConfigurationsDTO.SearchByParameters.IS_ACTIVE,"pc.IsActive"},
            { PayConfigurationsDTO.SearchByParameters.SITE_ID,"pc.site_id"},
            { PayConfigurationsDTO.SearchByParameters.MASTER_ENTITY_ID,"pc.MasterEntityId"}
        };

        /// <summary>
        /// Parameterized Constructor for PayConfigurationsDataHandler
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction Object</param>
        public PayConfigurationsDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating PayConfigurations Record
        /// </summary>
        /// <param name="payConfigurationsDTO">PayConfigurationsDTO object is passed as Parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(PayConfigurationsDTO payConfigurationsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(payConfigurationsDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@payConfigurationId", payConfigurationsDTO.PayConfigurationId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@payConfigurationName", payConfigurationsDTO.PayConfigurationName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@payTypeId", PayConfigurationsDTO.PayTypeEnumToInteger(payConfigurationsDTO.PayTypeId), true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", payConfigurationsDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", payConfigurationsDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Converts the Data row object to PayConfigurationsDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the object of PayConfigurationsDTO</returns>
        private PayConfigurationsDTO GetPayConfigurationsDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            PayConfigurationsDTO payConfigurationsDTO = new PayConfigurationsDTO(
                                                dataRow["PayConfigurationId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PayConfigurationId"]),
                                                dataRow["PayConfigurationName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["PayConfigurationName"]),
                                                dataRow["PayTypeId"] == DBNull.Value ? PayConfigurationsDTO.PayTypeEnum.NONE : PayConfigurationsDTO.PayTypeEnumFromString(dataRow["PayTypeId"].ToString()),
                                                dataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["IsActive"].ToString()),
                                                dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                                dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"])
                                                );
            log.LogMethodExit(payConfigurationsDTO);
            return payConfigurationsDTO;
        }

        /// <summary>
        /// Gets the PayConfigurationsDTO data of passed PayConfigurations Id
        /// </summary>
        /// <param name="payConfigurationsId">PayConfigurationsId is passed as Parameter</param>
        /// <returns>Returns payConfigurationDetailsDTO</returns>
        public PayConfigurationsDTO GetPayConfigurationsDTO(int payConfigurationId)
        {
            log.LogMethodEntry(payConfigurationId);
            PayConfigurationsDTO payConfigurationsDTO = null;
            string query = SELECT_QUERY + @" WHERE pc.PayConfigurationDetailId = @payConfigurationId";
            SqlParameter parameter = new SqlParameter("@payConfigurationId", payConfigurationId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                payConfigurationsDTO = GetPayConfigurationsDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(payConfigurationsDTO);
            return payConfigurationsDTO;
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="payConfigurationsDTO">payConfigurationsDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        private void RefreshPayConfigurationsDTO(PayConfigurationsDTO payConfigurationsDTO, DataTable dt)
        {
            log.LogMethodEntry(payConfigurationsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                payConfigurationsDTO.PayConfigurationId = Convert.ToInt32(dt.Rows[0]["PayConfigurationId"]);
                payConfigurationsDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                payConfigurationsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                payConfigurationsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                payConfigurationsDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                payConfigurationsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                payConfigurationsDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///  Inserts the record to the PayConfigurations Table. 
        /// </summary>
        /// <param name="payConfigurationsDTO">PayConfigurationsDTO object is passed as Parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated PayConfigurationsDTO</returns>
        public PayConfigurationsDTO Insert(PayConfigurationsDTO payConfigurationsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(payConfigurationsDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[PayConfigurations]
                            (
                            PayConfigurationName,
                            PayTypeId,
                            IsActive,
                            Guid,
                            site_id,
                            MasterEntityId,
                            LastUpdatedDate,
                            LastUpdatedBy,
                            CreatedBy,
                            CreationDate
                            )
                            VALUES
                            (
                            @payConfigurationName,
                            @payTypeId,
                            @isActive,
                            NEWID(),
                            @siteId,
                            @masterEntityId,
                            GETDATE(),
                            @lastUpdatedBy,
                            @createdBy,
                            GETDATE()                      
                            )
                            SELECT pc.* FROM PayConfigurations pc WHERE pc.PayConfigurationId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(payConfigurationsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPayConfigurationsDTO(payConfigurationsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting PayConfigurationsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(payConfigurationsDTO);
            return payConfigurationsDTO;
        }

        /// <summary>
        /// Update the record in the PayConfigurations Table. 
        /// </summary>
        /// <param name="payConfigurationsDTO">PayConfigurationsDTO object is passed as Parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated PayConfigurationsDTO</returns>
        public PayConfigurationsDTO Update(PayConfigurationsDTO payConfigurationsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(payConfigurationsDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[PayConfigurations]
                             SET
                             PayConfigurationName = @payConfigurationName,
                             PayTypeId = @payTypeId,
                             IsActive = @isActive,
                             LastUpdatedDate = GETDATE(),
                             LastUpdatedBy = @lastUpdatedBy        
                             WHERE PayConfigurationId = @payConfigurationId
                            SELECT pc.* FROM PayConfigurations pc WHERE pc.PayConfigurationId = @payConfigurationId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(payConfigurationsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPayConfigurationsDTO(payConfigurationsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating PayConfigurationsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(payConfigurationsDTO);
            return payConfigurationsDTO;
        }

        /// <summary>
        /// Returns the List of PayConfigurationsDTO based on the search parameters
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of PayConfigurationsDTO</returns>
        public List<PayConfigurationsDTO> GetPayConfigurationsDTOList(List<KeyValuePair<PayConfigurationsDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<PayConfigurationsDTO> payConfigurationsDTOList = new List<PayConfigurationsDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<PayConfigurationsDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == PayConfigurationsDTO.SearchByParameters.PAY_CONFIGURATION_ID ||
                            searchParameter.Key == PayConfigurationsDTO.SearchByParameters.PAY_TYPE_ID ||
                            searchParameter.Key == PayConfigurationsDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == PayConfigurationsDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1')= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == PayConfigurationsDTO.SearchByParameters.SITE_ID)
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
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    PayConfigurationsDTO payConfigurationsDTO = GetPayConfigurationsDTO(dataRow);
                    payConfigurationsDTOList.Add(payConfigurationsDTO);
                }
            }
            log.LogMethodExit(payConfigurationsDTOList);
            return payConfigurationsDTOList;
        }
    }
}
