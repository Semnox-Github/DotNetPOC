/********************************************************************************************
 * Project Name - Achievements
 * Description  - Data Handler -LoyaltyAttributeDataHandler
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By             Remarks          
 *********************************************************************************************
 *2.70.3     10-Jun-2019      Girish Kundar           Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Promotions
{
    /// <summary>
    /// LoyaltyAttributeDataHandler Data Handler - Handles insert, update and select of  LoyaltyAttribute objects
    /// </summary>
    public class LoyaltyAttributeDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM LoyaltyAttributes AS la ";

        /// <summary>
        /// Dictionary for searching Parameters for the LoyaltyAttribute object.
        /// </summary>
        private static readonly Dictionary<LoyaltyAttributesDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<LoyaltyAttributesDTO.SearchByParameters, string>
        {
            {LoyaltyAttributesDTO.SearchByParameters.LOYALTY_ATTRIBUTE_ID,"la.LoyaltyAttributeId"},
            {LoyaltyAttributesDTO.SearchByParameters.SITE_ID,"la.site_id"},
            {LoyaltyAttributesDTO.SearchByParameters.PURCHASE_APPLICABLE,"la.PurchaseApplicable"},
            {LoyaltyAttributesDTO.SearchByParameters.CONSUMPTION_APPLICABLE,"la.ConsumptionApplicable"},
            {LoyaltyAttributesDTO.SearchByParameters.MASTER_ENTITY_ID,"la.MasterEntityId"},
            {LoyaltyAttributesDTO.SearchByParameters.IS_ACTIVE,"la.IsActive"}
        };
        /// <summary>
        /// Parameterized Constructor for LoyaltyAttributeDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        public LoyaltyAttributeDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating LoyaltyAttribute Record.
        /// </summary>
        /// <param name="loyaltyAttributesDTO">LoyaltyAttributesDTO object passed as a parameter</param>
        /// <param name="loginId"> login id of the user</param>
        /// <param name="siteId"> site id of the user</param>
        /// <returns>SQL Parameters</returns>
        private List<SqlParameter> GetSQLParameters(LoyaltyAttributesDTO loyaltyAttributesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(loyaltyAttributesDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@LoyaltyAttributeId", loyaltyAttributesDTO.LoyaltyAttributeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Attribute", loyaltyAttributesDTO.Attribute));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ConsumptionApplicable", loyaltyAttributesDTO.ConsumptionApplicable));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreditPlusType", loyaltyAttributesDTO.CreditPlusType));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DBColumnName", loyaltyAttributesDTO.DBColumnName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PurchaseApplicable", loyaltyAttributesDTO.PurchaseApplicable));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", loyaltyAttributesDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        ///  Converts the Data row object to LoyaltyAttributesDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow Object of DataRow</param>
        /// <returns>LoyaltyAttributesDTO</returns>
        private LoyaltyAttributesDTO GetLoyaltyAttributesDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            LoyaltyAttributesDTO loyaltyAttributesDTO = new LoyaltyAttributesDTO(dataRow["LoyaltyAttributeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LoyaltyAttributeId"]),
                                          dataRow["Attribute"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Attribute"]),
                                          dataRow["PurchaseApplicable"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["PurchaseApplicable"]),
                                          dataRow["ConsumptionApplicable"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ConsumptionApplicable"]),
                                          dataRow["DBColumnName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["DBColumnName"]),
                                          dataRow["CreditPlusType"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreditPlusType"]),
                                          dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                          dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                          dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                          dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                          dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                          dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                          dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                          dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"])
                                          );
            log.LogMethodExit(loyaltyAttributesDTO);
            return loyaltyAttributesDTO;
        }

        /// <summary>
        /// Gets the LoyaltyAttributesDTO data of passed loyaltyAttributeId 
        /// </summary>
        /// <param name="loyaltyAttributeId">loyalty Attribute Id, integer type parameter</param>
        /// <returns>Returns LoyaltyAttributesDTO</returns>
        public LoyaltyAttributesDTO GetLoyaltyAttributes(int loyaltyAttributeId)
        {
            log.LogMethodEntry(loyaltyAttributeId);
            LoyaltyAttributesDTO result = null;
            string query = SELECT_QUERY + @" WHERE la.LoyaltyAttributeId = @Id";
            SqlParameter parameter = new SqlParameter("@Id", loyaltyAttributeId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetLoyaltyAttributesDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        ///  Inserts the record to the LoyaltyAttribute Table.
        /// </summary>
        /// <param name="loyaltyAttributesDTO">LoyaltyAttributesDTO object passed as a parameter</param>
        /// <param name="loginId"> login id of the user</param>
        /// <param name="siteId"> site id of the user</param>
        /// <returns>LoyaltyAttributesDTO</returns>
        public LoyaltyAttributesDTO Insert(LoyaltyAttributesDTO loyaltyAttributesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(loyaltyAttributesDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[LoyaltyAttributes]
                               ([Attribute],
                                [PurchaseApplicable],
                                [ConsumptionApplicable],
                                [DBColumnName],
                                [LastUpdatedDate],
                                [LastUpdatedBy],
                                [Guid],
                                [site_id],
                                [CreditPlusType],
                                [MasterEntityId],
                                [CreatedBy],
                                [CreationDate])
                         VALUES
                               (@Attribute,
                                @PurchaseApplicable,
                                @ConsumptionApplicable,
                                @DBColumnName,
                                GETDATE(),
                                @LastUpdatedBy,
                                NEWID(),
                                @site_id,
                                @CreditPlusType,
                                @MasterEntityId,
                                @CreatedBy,
                                GETDATE())
                                SELECT * FROM LoyaltyAttributes WHERE LoyaltyAttributeId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(loyaltyAttributesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshLoyaltyAttributesDTO(loyaltyAttributesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting LoyaltyAttributesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(loyaltyAttributesDTO);
            return loyaltyAttributesDTO;
        }
        /// <summary>
        ///  Updates the record to the LoyaltyAttributes Table.
        /// </summary>
        /// <param name="loyaltyAttributesDTO">LoyaltyAttributesDTO object passed as a parameter</param>
        /// <param name="loginId"> login id of the user</param>
        /// <param name="siteId"> site id of the user</param>
        /// <returns>returns the object of LoyaltyAttributesDTO</returns>
        public LoyaltyAttributesDTO Update(LoyaltyAttributesDTO loyaltyAttributesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(loyaltyAttributesDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[LoyaltyAttributes]
                               SET
                                [Attribute]             = @Attribute,
                                [PurchaseApplicable]    = @PurchaseApplicable,
                                [ConsumptionApplicable] = @ConsumptionApplicable,
                                [DBColumnName]          = @DBColumnName,
                                [LastUpdatedDate]       = GETDATE(),
                                [LastUpdatedBy]         = @LastUpdatedBy,
                                [CreditPlusType]        = @CreditPlusType,
                                [MasterEntityId]        = @MasterEntityId
                                 WHERE LoyaltyAttributeId = @LoyaltyAttributeId
                                SELECT * FROM LoyaltyAttributes WHERE LoyaltyAttributeId = @LoyaltyAttributeId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(loyaltyAttributesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshLoyaltyAttributesDTO(loyaltyAttributesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating LoyaltyAttributesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(loyaltyAttributesDTO);
            return loyaltyAttributesDTO;
        }
        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured
        /// </summary>
        /// <param name="loyaltyAttributesDTO">LoyaltyAttributesDTO object passed as a parameter</param>
        /// <param name="dt"> dt is an object of DataTable</param>
        /// <param name="loginId"> login id of the user</param>
        /// <param name="siteId"> site id of the user</param>
        private void RefreshLoyaltyAttributesDTO(LoyaltyAttributesDTO loyaltyAttributesDTO, DataTable dt)
        {
            log.LogMethodEntry(loyaltyAttributesDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                loyaltyAttributesDTO.LoyaltyAttributeId = Convert.ToInt32(dt.Rows[0]["LoyaltyAttributeId"]);
                loyaltyAttributesDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                loyaltyAttributesDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                loyaltyAttributesDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                loyaltyAttributesDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                loyaltyAttributesDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                loyaltyAttributesDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }
      
        /// <summary>
        /// Returns the List of LoyaltyAttributesDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>returns the list of LoyaltyAttributesDTO</returns>
        public List<LoyaltyAttributesDTO> GetLoyaltyAttributesDTOList(List<KeyValuePair<LoyaltyAttributesDTO.SearchByParameters, string>> searchParameters,
                                                                      SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<LoyaltyAttributesDTO> loyaltyAttributesDTOList = new List<LoyaltyAttributesDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<LoyaltyAttributesDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == LoyaltyAttributesDTO.SearchByParameters.LOYALTY_ATTRIBUTE_ID ||
                             searchParameter.Key == LoyaltyAttributesDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == LoyaltyAttributesDTO.SearchByParameters.PURCHASE_APPLICABLE ||
                                 searchParameter.Key == LoyaltyAttributesDTO.SearchByParameters.CONSUMPTION_APPLICABLE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), (searchParameter.Value == "1" || searchParameter.Value == "Y" ? 'Y' : 'N')));
                        }
                        else if (searchParameter.Key == LoyaltyAttributesDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == LoyaltyAttributesDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
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
                    LoyaltyAttributesDTO loyaltyAttributesDTO = GetLoyaltyAttributesDTO(dataRow);
                    loyaltyAttributesDTOList.Add(loyaltyAttributesDTO);
                }
            }
            log.LogMethodExit(loyaltyAttributesDTOList);
            return loyaltyAttributesDTOList;
        }
        internal DateTime? GetLoyaltyAttributeLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"SELECT MAX(LastUpdatedDate) LastUpdatedDate 
                            FROM (
                            select max(LastUpdatedDate) LastUpdatedDate from LoyaltyAttributes WHERE (site_id = @siteId or @siteId = -1)
                            )a";
            SqlParameter parameter = new SqlParameter("@siteId", siteId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, sqlTransaction);
            DateTime? result = null;
            if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["LastUpdatedDate"] != DBNull.Value)
            {
                result = Convert.ToDateTime(dataTable.Rows[0]["LastUpdatedDate"]);
            }
            log.LogMethodExit(result);
            return result;
        }

    }
}
