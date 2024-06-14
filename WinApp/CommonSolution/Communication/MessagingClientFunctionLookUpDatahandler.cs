/********************************************************************************************
 * Project Name - Communication
 * Description  - Data Handler object -MessagingClientFunctionLookUpDatahandler
 *
 **************
 ** Version Log
  **************
  * Version     Date             Modified By             Remarks
 *********************************************************************************************
 *2.80          25-Jun-2020      Jinto Thomas            Created
 *2.100.0       22-Aug-2020      Vikas Dwivedi           Modified as per 3-Tier Standard CheckList
 *2.110.0       10-Dec-2020      Fiona                   For Subscription changes                  
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Semnox.Parafait.Communication
{
    /// <summary>
    /// MessagingClientFunctionLookUpDatahandler object for Insert, Update and Search for MessagingClientFunctionLookUp Object
    /// </summary>
    public class MessagingClientFunctionLookUpDatahandler
    {
        private static readonly Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM MessagingClientFunctionLookUp AS mcfl ";

        /// <summary>
        /// Dictionary for searching Parameters for the MessagingClientFunctionLookUp object.
        /// </summary>

        private static readonly Dictionary<MessagingClientFunctionLookUpDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<MessagingClientFunctionLookUpDTO.SearchByParameters, string>
        {
            {MessagingClientFunctionLookUpDTO.SearchByParameters.ID,"mcfl.Id"},
            {MessagingClientFunctionLookUpDTO.SearchByParameters.CLIENT_ID,"mcfl.MessagingClientId"},
            //{MessagingClientFunctionLookUpDTO.SearchByParameters.LOOKUP_ID,"mcfl.LookUpId"},
            //{MessagingClientFunctionLookUpDTO.SearchByParameters.LOOKUP_VALUE_ID,"mcfl.LookUpValueId"},
            {MessagingClientFunctionLookUpDTO.SearchByParameters.MESSAGE_TYPE,"mcfl.MessageType" },
            {MessagingClientFunctionLookUpDTO.SearchByParameters.SITE_ID,"mcfl.site_id"},
            {MessagingClientFunctionLookUpDTO.SearchByParameters.IS_ACTIVE,"mcfl.IsActive"},
            {MessagingClientFunctionLookUpDTO.SearchByParameters.MASTER_ENTITY_ID,"mcfl.MasterEntityId"},
            {MessagingClientFunctionLookUpDTO.SearchByParameters.PARAFAIT_FUNCTION_EVENT_ID,"mcfl.ParafaitFunctionEventId"},
            {MessagingClientFunctionLookUpDTO.SearchByParameters.PARAFAIT_FUNCTION_EVENT_NAME,"pfe.ParafaitFunctionEventName"},
            {MessagingClientFunctionLookUpDTO.SearchByParameters.PARAFAIT_FUNCTION_NAME,"pf.ParafaitFunctionName"}
        };

        /// <summary>
        /// Parameterized Constructor for MessagingClientFunctionLookUpDatahandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        public MessagingClientFunctionLookUpDatahandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating MessagingClientFunctionLookUp Record.
        /// </summary>
        /// <param name="messagingClientFunctionLookUpDTO">MessagingClientFunctionLookUpDTO object passed as parameter</param>
        /// <param name="loginId">login Id of the user</param>
        /// <param name="siteId"> site Id </param>
        /// <returns>SQL parameters</returns>

        private List<SqlParameter> GetSQLParameters(MessagingClientFunctionLookUpDTO messagingClientFunctionLookUpDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(messagingClientFunctionLookUpDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", messagingClientFunctionLookUpDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MessagingClientId", messagingClientFunctionLookUpDTO.MessageClientId, true));
            //parameters.Add(dataAccessHandler.GetSQLParameter("@LookUpId", messagingClientFunctionLookUpDTO.LookUpId, true));
            //parameters.Add(dataAccessHandler.GetSQLParameter("@LookUpValueId", messagingClientFunctionLookUpDTO.LookUpValueId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ParafaitFunctionEventId", messagingClientFunctionLookUpDTO.ParafaitFunctionEventId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MessageTemplateId", messagingClientFunctionLookUpDTO.MessageTemplateId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ReceiptPrintTemplateId", messagingClientFunctionLookUpDTO.ReceiptPrintTemplateId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MessageType", messagingClientFunctionLookUpDTO.MessageType));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CCList", messagingClientFunctionLookUpDTO.CCList));
            parameters.Add(dataAccessHandler.GetSQLParameter("@BCCList", messagingClientFunctionLookUpDTO.BCCList));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SynchStatus", messagingClientFunctionLookUpDTO.SynchStatus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", messagingClientFunctionLookUpDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", messagingClientFunctionLookUpDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        ///  Converts the Data row object to MessagingClientFunctionLookUpDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>MessagingClientFunctionLookUpDTO</returns>
        private MessagingClientFunctionLookUpDTO GetMessagingClientFunctionLookUpDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            MessagingClientFunctionLookUpDTO messagingClientFunctionLookUpDTO = new MessagingClientFunctionLookUpDTO(dataRow["Id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Id"]),
                                                         dataRow["MessagingClientId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MessagingClientId"]),
                                                         //dataRow["LookUpId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LookUpId"]),
                                                         //dataRow["LookUpValueId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LookUpValueId"]),
                                                         dataRow["MessageType"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["MessageType"]),
                                                         dataRow["ParafaitFunctionEventId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ParafaitFunctionEventId"]),
                                                         dataRow["MessageTemplateId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MessageTemplateId"]),
                                                         dataRow["ReceiptPrintTemplateId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ReceiptPrintTemplateId"]),
                                                         dataRow["CCList"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CCList"]),
                                                         dataRow["BCCList"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["BCCList"]),
                                                         dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                         dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                         dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                         dataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["IsActive"]),
                                                         dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                         dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                         dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                         dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                         dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"])
                                                        );
            log.LogMethodExit(messagingClientFunctionLookUpDTO);
            return messagingClientFunctionLookUpDTO;
        }

        /// <summary>
        /// Gets the MessagingClientFunctionLookUpDTO data of passed Id 
        /// </summary>
        /// <param name="id">id -MessagingClientFunctionLookUpId </param>
        /// <returns>Returns MessagingClientFunctionLookUpDTO</returns>
        public MessagingClientFunctionLookUpDTO GetMessagingClientFunctionLookUp(int id)
        {
            log.LogMethodEntry(id);
            MessagingClientFunctionLookUpDTO result = null;
            string query = SELECT_QUERY + @" WHERE mcfl.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetMessagingClientFunctionLookUpDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        ///  Inserts the record to the MessagingClientFunctionLookUp Table.
        /// </summary>
        /// <param name="messagingClientFunctionLookUpDTO">MessagingClientFunctionLookUpDTO object passed as parameter</param>
        /// <param name="loginId">login Id of the user</param>
        /// <param name="siteId"> site Id </param>
        /// <returns>MessagingClientDTO</returns>
        public MessagingClientFunctionLookUpDTO Insert(MessagingClientFunctionLookUpDTO messagingClientFunctionLookUpDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(messagingClientFunctionLookUpDTO, loginId, siteId);
            string query = @"INSERT INTO dbo.MessagingClientFunctionLookUp
                           (MessagingClientId,
                            --LookUpId,
                            --LookUpValueId,
                            MessageType,
                            ParafaitFunctionEventId,
                            MessageTemplateId,
                            ReceiptPrintTemplateId,
                            CCList,
                            BCCList,
                            SynchStatus,
                            Guid,
                            site_id,
                            IsActive,
                            MasterEntityId,
                            CreatedBy,
                            CreationDate,
                            LastUpdatedBy,
                            LastUpdateDate)
                     VALUES
                           (@MessagingClientId,
                           -- @LookUpId,
                           -- @LookUpValueId,
                            @MessageType,
                            @ParafaitFunctionEventId,
                            @MessageTemplateId,
                            @ReceiptPrintTemplateId,
                            @CCList,
                            @BCCList,
                            @SynchStatus,
                            NEWID(),
                            @SiteId,
                            @IsActive,
                            @MasterEntityId,
                            @CreatedBy,
                            GETDATE(),
                            @LastUpdatedBy,
                            GETDATE())
                            SELECT * FROM MessagingClientFunctionLookUp WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(messagingClientFunctionLookUpDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshMessagingClientDTO(messagingClientFunctionLookUpDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting messagingClientFunctionLookUpDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(messagingClientFunctionLookUpDTO);
            return messagingClientFunctionLookUpDTO;
        }

        /// <summary>
        ///  Updates the record to the MessagingClientFunctionLookUp Table.
        /// </summary>
        /// <param name="messagingClientFunctionLookUpDTO">MessagingClientFunctionLookUpDTO object passed as parameter</param>
        /// <param name="loginId">login Id of the user</param>
        /// <param name="siteId"> site Id </param>
        /// <returns>MessagingRequestDTO</returns>
        public MessagingClientFunctionLookUpDTO Update(MessagingClientFunctionLookUpDTO messagingClientFunctionLookUpDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(messagingClientFunctionLookUpDTO, loginId, siteId);
            string query = @"UPDATE dbo.MessagingClientFunctionLookup
                           SET
                            MessagingClientId = @MessagingClientId,
                            --LookUpId = @LookUpId,
                            --LookUpValueId = @LookUpValueId,
                            MessageType = @MessageType,
                            ParafaitFunctionEventId = @ParafaitFunctionEventId,
                            MessageTemplateId = @MessageTemplateId,
                            ReceiptPrintTemplateId = @ReceiptPrintTemplateId,
                            CCList = @CCList,
                            BCCList = @BCCList,
                            MasterEntityId = @MasterEntityId,
                            LastUpdatedBy = @LastUpdatedBy,
                            IsActive = @IsActive,
                            LastUpdateDate = GETDATE()
                            WHERE Id = @Id;
                            SELECT * FROM MessagingClientFunctionLookUp WHERE Id = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(messagingClientFunctionLookUpDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshMessagingClientDTO(messagingClientFunctionLookUpDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating messagingClientFunctionLookUpDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(messagingClientFunctionLookUpDTO);
            return messagingClientFunctionLookUpDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured
        /// </summary>
        /// <param name="messagingClientFunctionLookUpDTO">MessagingClientFunctionLookUpDTO object passed as parameter</param>
        /// <param name="dt">dt is an object of DataTable </param>
        /// <param name="loginId">login Id of the user</param>
        /// <param name="siteId"> site Id </param>
        private void RefreshMessagingClientDTO(MessagingClientFunctionLookUpDTO messagingClientFunctionLookUpDTO, DataTable dt)
        {
            log.LogMethodEntry(messagingClientFunctionLookUpDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                messagingClientFunctionLookUpDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                messagingClientFunctionLookUpDTO.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                messagingClientFunctionLookUpDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                messagingClientFunctionLookUpDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                messagingClientFunctionLookUpDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                messagingClientFunctionLookUpDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                messagingClientFunctionLookUpDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the List of MessagingClientFunctionLookUpDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">Search Parameters</param>
        /// <returns>Returns the List of MessagingClientFunctionLookUpDTO</returns>
        public List<MessagingClientFunctionLookUpDTO> GetMessagingClientFunctionLookUpDTOList(List<KeyValuePair<MessagingClientFunctionLookUpDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<MessagingClientFunctionLookUpDTO> messagingClientFunctionLookUpDTOList = new List<MessagingClientFunctionLookUpDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<MessagingClientFunctionLookUpDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? "" : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == MessagingClientFunctionLookUpDTO.SearchByParameters.ID
                            || searchParameter.Key == MessagingClientFunctionLookUpDTO.SearchByParameters.CLIENT_ID
                            //|| searchParameter.Key == MessagingClientFunctionLookUpDTO.SearchByParameters.LOOKUP_ID
                            //|| searchParameter.Key == MessagingClientFunctionLookUpDTO.SearchByParameters.LOOKUP_VALUE_ID
                            || searchParameter.Key == MessagingClientFunctionLookUpDTO.SearchByParameters.MASTER_ENTITY_ID
                            || searchParameter.Key == MessagingClientFunctionLookUpDTO.SearchByParameters.PARAFAIT_FUNCTION_EVENT_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == MessagingClientFunctionLookUpDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == MessagingClientFunctionLookUpDTO.SearchByParameters.IS_ACTIVE) // bit
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == MessagingClientFunctionLookUpDTO.SearchByParameters.MESSAGE_TYPE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == MessagingClientFunctionLookUpDTO.SearchByParameters.PARAFAIT_FUNCTION_EVENT_NAME)
                        {
                            query.Append(joiner + @" EXISTS (SELECT 1 
                                                               FROM ParafaitFunctionEvents pfe 
                                                              where pfe.ParafaitFunctionEventId = mcfl.ParafaitFunctionEventId
                                                                AND " + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key)
                                                                + " ) ");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == MessagingClientFunctionLookUpDTO.SearchByParameters.PARAFAIT_FUNCTION_NAME)
                        {
                            query.Append(joiner + @" EXISTS (SELECT 1 
                                                               FROM ParafaitFunctionEvents pfe, ParafaitFunctions pf
                                                              where pf.ParafaitFunctionId = pfe.ParafaitFunctionId
                                                                AND pfe.ParafaitFunctionEventId = mcfl.ParafaitFunctionEventId
                                                                AND " + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key)
                                                                + " ) ");
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
                    MessagingClientFunctionLookUpDTO messagingClientFunctionLookUpDTO = GetMessagingClientFunctionLookUpDTO(dataRow);
                    messagingClientFunctionLookUpDTOList.Add(messagingClientFunctionLookUpDTO);
                }
            }
            log.LogMethodExit(messagingClientFunctionLookUpDTOList);
            return messagingClientFunctionLookUpDTOList;
        }
    }
}
