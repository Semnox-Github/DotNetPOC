/********************************************************************************************
 * Project Name - POS
 * Description  - Data handler object of POSPaymentModeInclusion
 * 
 **************
 **Version Log
 **************
 *Version        Date          Modified By         Remarks          
 *********************************************************************************************
 *2.90.0        13-Jun-2020    Girish kundar       Created
 *********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Device.PaymentGateway;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.POS
{
    public class POSPaymentModeInclusionDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM POSPaymentModeInclusions AS posPMInc ";

        /// <summary>
        /// Dictionary for searching Parameters for the LoyaltyRule object.
        /// </summary>
        private static readonly Dictionary<POSPaymentModeInclusionDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<POSPaymentModeInclusionDTO.SearchByParameters, string>
            {
                {POSPaymentModeInclusionDTO.SearchByParameters.POS_PAYMENTMODE_INCLUSION_ID, "posPMInc.POSPaymentModeInclusionId"},
                {POSPaymentModeInclusionDTO.SearchByParameters.IS_ACTIVE, "posPMInc.IsActive"},
                {POSPaymentModeInclusionDTO.SearchByParameters.POS_MACHINE_ID, "posPMInc.POSMachineId"},
                {POSPaymentModeInclusionDTO.SearchByParameters.POS_MACHINE_ID_LIST, "posPMInc.POSMachineId"},
                {POSPaymentModeInclusionDTO.SearchByParameters.PAYMENT_MODE_ID, "posPMInc.PaymentModeId"},
                {POSPaymentModeInclusionDTO.SearchByParameters.MASTER_ENTITY_ID, "posPMInc.MasterEntityId"},
                {POSPaymentModeInclusionDTO.SearchByParameters.SITE_ID, "posPMInc.site_id"},
            };


        /// <summary>
        /// Parameterized Constructor for POSPaymentModeInclusionDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction Object</param>
        public POSPaymentModeInclusionDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }


        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating LoyaltyRule Record.
        /// </summary>
        /// <param name="POSPaymentModeInclusionDTO">POSPaymentModeInclusionDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site_id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(POSPaymentModeInclusionDTO POSPaymentModeInclusionDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(POSPaymentModeInclusionDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@POSPaymentModeInclusionId", POSPaymentModeInclusionDTO.POSPaymentModeInclusionId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@POSMachineId", POSPaymentModeInclusionDTO.POSMachineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PaymentModeId", POSPaymentModeInclusionDTO.PaymentModeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@FriendlyName", POSPaymentModeInclusionDTO.FriendlyName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", POSPaymentModeInclusionDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", POSPaymentModeInclusionDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastupdatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Converts the Data row object to POSPaymentModeInclusionDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the object of POSPaymentModeInclusionDTO</returns>
        private POSPaymentModeInclusionDTO GetPOSPaymentModeInclusionDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            POSPaymentModeInclusionDTO posPaymentModeInclusionDTO = new POSPaymentModeInclusionDTO(
                dataRow["POSPaymentModeInclusionId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["POSPaymentModeInclusionId"]),
                dataRow["POSMachineId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["POSMachineId"]),
                dataRow["PaymentModeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PaymentModeId"]),
                dataRow["FriendlyName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["FriendlyName"]),
                dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]));
            log.LogMethodExit(posPaymentModeInclusionDTO);
            return posPaymentModeInclusionDTO;
        }

        /// <summary>
        /// Gets the POSPaymentModeInclusionDTO data of passed POSPaymentModeInclusion ID
        /// </summary>
        /// <param name="POSPaymentModeInclusionDTO">POSPaymentModeInclusionDTO is passed as parameter</param>
        /// <returns>Returns POSPaymentModeInclusionDTO</returns>
        internal POSPaymentModeInclusionDTO GetPOSPaymentModeInclusionDTO(int posPaymentModeInclusionId)
        {
            log.LogMethodEntry(posPaymentModeInclusionId);
            POSPaymentModeInclusionDTO result = null;
            string query = SELECT_QUERY + @" WHERE posPMInc.POSPaymentModeInclusionId = @Id";
            SqlParameter parameter = new SqlParameter("@Id", posPaymentModeInclusionId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetPOSPaymentModeInclusionDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        ///  Deletes the POSPaymentModeInclusionDTO record
        /// </summary>
        /// <param name="POSPaymentModeInclusionDTO">POSPaymentModeInclusionDTO is passed as parameter</param>
        internal void Delete(int id)
        {
            log.LogMethodEntry(id);
            string query = @"DELETE  
                             FROM POSPaymentModeInclusions
                             WHERE POSPaymentModeInclusionId = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit();
        }

        private void RefreshPOSPaymentModeInclusionDTO(POSPaymentModeInclusionDTO posPaymentModeInclusionDTO, DataTable dt)
        {
            log.LogMethodEntry(posPaymentModeInclusionDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                posPaymentModeInclusionDTO.POSPaymentModeInclusionId = Convert.ToInt32(dt.Rows[0]["POSPaymentModeInclusionId"]);
                posPaymentModeInclusionDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                posPaymentModeInclusionDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                posPaymentModeInclusionDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                posPaymentModeInclusionDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                posPaymentModeInclusionDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                posPaymentModeInclusionDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        internal POSPaymentModeInclusionDTO Insert(POSPaymentModeInclusionDTO posPaymentModeInclusionDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(posPaymentModeInclusionDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[POSPaymentModeInclusions]
                               ([POSMachineId]
                               ,[PaymentModeId]
                               ,[FriendlyName]
                               ,[Guid]
                               ,[site_id]
                               ,[MasterEntityId]
                               ,[IsActive]
                               ,[CreatedBy]
                               ,[CreationDate]
                               ,[LastUpdatedBy]
                               ,[LastUpdateDate])
                         VALUES
                               (
                                @POSMachineId,
                                @PaymentModeId,
                                @FriendlyName,
                                NEWID(), 
                                @SiteId,
                                @MasterEntityId,
                                @IsActive,
                                @CreatedBy,
                                GETDATE(),
                                @LastUpdatedBy,
                                GETDATE() )
            SELECT * FROM POSPaymentModeInclusions WHERE POSPaymentModeInclusionId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(posPaymentModeInclusionDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPOSPaymentModeInclusionDTO(posPaymentModeInclusionDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting POSPaymentModeInclusionDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(posPaymentModeInclusionDTO);
            return posPaymentModeInclusionDTO;
        }

        internal List<POSPaymentModeInclusionDTO> GetPOSPaymentModeInclusionDTOList(List<int> pOSMachineIdList, bool activeRecords)
        {
            log.LogMethodEntry(pOSMachineIdList);
            List<POSPaymentModeInclusionDTO> pOSPaymentModeInclusionDTOList = new List<POSPaymentModeInclusionDTO>();
            string query = @"SELECT *
                            FROM POSPaymentModeInclusions, @POSMachineIdList List
                            WHERE POSMachineId = List.Id ";
            if (activeRecords)
            {
                query += " AND  isnull(isActive,'1') = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@POSMachineIdList", pOSMachineIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                pOSPaymentModeInclusionDTOList = table.Rows.Cast<DataRow>().Select(x => GetPOSPaymentModeInclusionDTO(x)).ToList();
            }
            log.LogMethodExit(pOSPaymentModeInclusionDTOList);
            return pOSPaymentModeInclusionDTOList;
        }

        internal POSPaymentModeInclusionDTO Update(POSPaymentModeInclusionDTO posPaymentModeInclusionDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(posPaymentModeInclusionDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[POSPaymentModeInclusions]
                              SET
                              [POSMachineId]   = @POSMachineId,
                              [PaymentModeId]  = @PaymentModeId,
                              [FriendlyName]   = @FriendlyName,
                              [MasterEntityId] = @MasterEntityId,
                              [IsActive]       = @IsActive,
                              [LastUpdatedBy]  = @LastUpdatedBy,
                              [LastUpdateDate] = GETDATE()
                         WHERE POSPaymentModeInclusionId = @POSPaymentModeInclusionId
              SELECT * FROM POSPaymentModeInclusions WHERE POSPaymentModeInclusionId = @POSPaymentModeInclusionId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(posPaymentModeInclusionDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPOSPaymentModeInclusionDTO(posPaymentModeInclusionDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating  POSPaymentModeInclusionDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(posPaymentModeInclusionDTO);
            return posPaymentModeInclusionDTO;
        }

        public List<POSPaymentModeInclusionDTO> GetPOSPaymentModeInclusionDTOList(List<KeyValuePair<POSPaymentModeInclusionDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<POSPaymentModeInclusionDTO> posPaymentModeInclusionDTOList = new List<POSPaymentModeInclusionDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = ""; ;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<POSPaymentModeInclusionDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == POSPaymentModeInclusionDTO.SearchByParameters.POS_PAYMENTMODE_INCLUSION_ID ||
                            searchParameter.Key == POSPaymentModeInclusionDTO.SearchByParameters.POS_MACHINE_ID ||
                            searchParameter.Key == POSPaymentModeInclusionDTO.SearchByParameters.PAYMENT_MODE_ID ||
                            searchParameter.Key == POSPaymentModeInclusionDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == POSPaymentModeInclusionDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == POSPaymentModeInclusionDTO.SearchByParameters.POS_MACHINE_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == POSPaymentModeInclusionDTO.SearchByParameters.IS_ACTIVE)
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
                    POSPaymentModeInclusionDTO POSPaymentModeInclusionDTO = GetPOSPaymentModeInclusionDTO(dataRow);
                    posPaymentModeInclusionDTOList.Add(POSPaymentModeInclusionDTO);
                }
            }
            log.LogMethodExit(posPaymentModeInclusionDTOList);
            return posPaymentModeInclusionDTOList;
        }

    }
}
