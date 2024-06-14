/********************************************************************************************
 * Project Name - Transaction
 * Description  - Data Handler object - InvoiceSequenceMappingDataHandler
 *
 **************
 ** Version Log
  **************
  * Version     Date         Modified By             Remarks
 *********************************************************************************************
 *2.80         28-May-2020   Girish Kundar          Modified : 3 tier standard and for Rest Api Changes. 
 *2.120.0      15-03-2021    Prajwal S              Modified : SiteId changes in Insert and Update.  
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    ///  InvoiceSequenceMapping Data Handler - Handles insert, update and select of  InvoiceSequenceMapping objects
    /// </summary>
    public class InvoiceSequenceMappingDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<InvoiceSequenceMappingDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<InvoiceSequenceMappingDTO.SearchByParameters, string>
            {
                {InvoiceSequenceMappingDTO.SearchByParameters.ID, "ism.Id"},
                {InvoiceSequenceMappingDTO.SearchByParameters.INVOICE_SETUP_ID, "ism.InvoiceSequenceSetupId"},
                {InvoiceSequenceMappingDTO.SearchByParameters.SEQUENCE_ID, "ism.SequenceId"},
                {InvoiceSequenceMappingDTO.SearchByParameters.ISACTIVE, "ism.IsActive"},
                {InvoiceSequenceMappingDTO.SearchByParameters.MASTER_ENTITY_ID,"ism.MasterEntityId"},
                {InvoiceSequenceMappingDTO.SearchByParameters.SITE_ID, "ism.site_id"},
                {InvoiceSequenceMappingDTO.SearchByParameters.EFFECTIVE_DATE, "ism.EffectiveDate"},
                {InvoiceSequenceMappingDTO.SearchByParameters.EFFECTIVE_DATE_LESSER_THAN, "ism.EffectiveDate"},
                {InvoiceSequenceMappingDTO.SearchByParameters.INVOICE_TYPE_ID, "InvoiceSequenceSetup.InvoiceTypeId"},
                {InvoiceSequenceMappingDTO.SearchByParameters.INVOICE_TYPE, "LookupValue"}

            };
        private DataAccessHandler dataAccessHandler;
        private Utilities utilities;
        private SqlTransaction sqlTransaction = null;
        private const string SELECT_QUERY = @"SELECT * FROM InvoiceSequenceMapping AS ism ";
        /// <summary>
        /// Default constructor of InvoiceSequenceMappingDataHandler class
        /// </summary>
        public InvoiceSequenceMappingDataHandler(SqlTransaction sqlTransaction = null) 
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            utilities = new Utilities();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        private List<SqlParameter> GetSQLParameters(InvoiceSequenceMappingDTO invoiceSequenceMappingDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(invoiceSequenceMappingDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", invoiceSequenceMappingDTO.Id));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SequenceId", invoiceSequenceMappingDTO.SequenceId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@InvoiceSequenceSetupId", invoiceSequenceMappingDTO.InvoiceSequenceSetupId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@EffectiveDate", invoiceSequenceMappingDTO.EffectiveDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", invoiceSequenceMappingDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", invoiceSequenceMappingDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedUser", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Returns InvoiceSequenceMappingDTO
        /// </summary>
        /// <param name="invoiceSequenceMappingDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public InvoiceSequenceMappingDTO InsertInvoiceSequenceMapping(InvoiceSequenceMappingDTO invoiceSequenceMappingDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(invoiceSequenceMappingDTO, loginId, siteId);
            string query = @"insert into InvoiceSequenceMapping 
                                                        (
                                                          SequenceId,
                                                          InvoiceSequenceSetupId,
                                                          EffectiveDate,
                                                          IsActive,
                                                          LastUpdatedUser,
                                                          LastupdatedDate,
                                                          site_id,
                                                          MasterEntityId,
                                                          Guid,
                                                          CreatedBy,
                                                          CreationDate
                                                        ) 
                                                values 
                                                        (
                                                          @SequenceId,
                                                          @InvoiceSequenceSetupId,
                                                          @EffectiveDate,
                                                          @IsActive,
                                                          @LastUpdatedUser,
                                                          Getdate(),
                                                          @site_id,
                                                          @MasterEntityId,
                                                          NewId(),
                                                          @CreatedBy,
                                                          GetDate()
                                                        ) SELECT * FROM InvoiceSequenceMapping WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(invoiceSequenceMappingDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshInvoiceSequenceMappingDTO(invoiceSequenceMappingDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting invoiceSequenceMappingDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(invoiceSequenceMappingDTO);
            return invoiceSequenceMappingDTO;
        }

        private void RefreshInvoiceSequenceMappingDTO(InvoiceSequenceMappingDTO invoiceSequenceMappingDTO, DataTable dt)
        {
            log.LogMethodEntry(invoiceSequenceMappingDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                invoiceSequenceMappingDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                invoiceSequenceMappingDTO.LastUpdatedDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
                invoiceSequenceMappingDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                invoiceSequenceMappingDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                invoiceSequenceMappingDTO.LastUpdatedBy = dataRow["LastUpdatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedUser"]);
                invoiceSequenceMappingDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                invoiceSequenceMappingDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// returns InvoiceSequenceMappingDTO
        /// </summary>
        /// <param name="invoiceSequenceMappingDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public InvoiceSequenceMappingDTO UpdateInvoiceSequenceMapping(InvoiceSequenceMappingDTO invoiceSequenceMappingDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(invoiceSequenceMappingDTO, loginId, siteId);
            string query = @"update InvoiceSequenceMapping 
                                                          set SequenceId = @SequenceId,
                                                          InvoiceSequenceSetupId =  @InvoiceSequenceSetupId,
                                                          EffectiveDate = @EffectiveDate,
                                                          IsActive = @IsActive,
                                                          LastUpdatedUser = @LastUpdatedUser, 
                                                          LastupdatedDate = Getdate(),
                                                          MasterEntityId =  @MasterEntityId,
                                                          where ID = @Id
                                                          SELECT * FROM InvoiceSequenceMapping WHERE Id = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(invoiceSequenceMappingDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshInvoiceSequenceMappingDTO(invoiceSequenceMappingDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating invoiceSequenceMappingDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(invoiceSequenceMappingDTO);
            return invoiceSequenceMappingDTO;
        }

        /// <summary>
        /// Converts the Data row object to InvoiceSequenceMappingDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns InvoiceSequenceMappingDTO</returns>
        private InvoiceSequenceMappingDTO GetInvoiceSequenceMappingDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            InvoiceSequenceMappingDTO invoiceSequenceMappingDTO = new InvoiceSequenceMappingDTO(Convert.ToInt32(dataRow["Id"]),
                                            dataRow["SequenceId"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["SequenceId"]),
                                            dataRow["InvoiceSequenceSetupId"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["InvoiceSequenceSetupId"]),
                                            dataRow["EffectiveDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["EffectiveDate"]),
                                            dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                                            dataRow["LastUpdatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedUser"]),
                                            dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString(),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"])
                                            );
            log.LogMethodExit(invoiceSequenceMappingDTO);
            return invoiceSequenceMappingDTO;
        }

        /// <summary>
        /// Gets the Event data of passed InvoiceSequenceMapping Id
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns InvoiceSequenceMappingDTO</returns>
        public InvoiceSequenceMappingDTO GetInvoiceSequenceMappingDTO(int id)
        {
            log.LogMethodEntry(id);
            InvoiceSequenceMappingDTO returnValue = null;
            string query = SELECT_QUERY + "  WHERE ism.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetInvoiceSequenceMappingDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Gets the InvoiceSequenceMappingDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of invoiceSequenceMappingDTO matching the search criteria</returns>
        public List<InvoiceSequenceMappingDTO> GetAllInvoiceSequenceMappingList(List<KeyValuePair<InvoiceSequenceMappingDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodExit(searchParameters);
            int count = 0;
            string selectProductQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if (searchParameters != null && (searchParameters.Count > 0))
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<InvoiceSequenceMappingDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        {
                            if (searchParameter.Key.Equals(InvoiceSequenceMappingDTO.SearchByParameters.ID)
                                || searchParameter.Key.Equals(InvoiceSequenceMappingDTO.SearchByParameters.INVOICE_SETUP_ID)
                                || searchParameter.Key.Equals(InvoiceSequenceMappingDTO.SearchByParameters.MASTER_ENTITY_ID)
                                || searchParameter.Key.Equals(InvoiceSequenceMappingDTO.SearchByParameters.SEQUENCE_ID))
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                            }
                            else if (searchParameter.Key == InvoiceSequenceMappingDTO.SearchByParameters.SITE_ID)
                            {
                                query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                            }
                            else if (searchParameter.Key == InvoiceSequenceMappingDTO.SearchByParameters.ISACTIVE)
                            {
                                query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                            }
                            else if (searchParameter.Key == InvoiceSequenceMappingDTO.SearchByParameters.EFFECTIVE_DATE)
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                            }
                            else if (searchParameter.Key == InvoiceSequenceMappingDTO.SearchByParameters.INVOICE_TYPE_ID)
                            {
                                query.Append(joiner + " InvoiceSequenceSetupId in(select InvoiceSequenceSetupId from InvoiceSequenceSetup where " +
                                   DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + ")");
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));

                            }
                            else if (searchParameter.Key == InvoiceSequenceMappingDTO.SearchByParameters.INVOICE_TYPE)
                            {
                                query.Append(joiner + " InvoiceSequenceSetupId in (select InvoiceSequenceSetupId from InvoiceSequenceSetup, LookupValues " +
                                        "where InvoiceSequenceSetup.InvoiceTypeId = LookupValues.LookupValueId " +
                                        "and LookupValues." + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) +")");
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));

                            }
                            else if (searchParameter.Key == InvoiceSequenceMappingDTO.SearchByParameters.EFFECTIVE_DATE_LESSER_THAN)
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + " <" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                            }
                            else
                            {
                                query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                            }
                        }
                        count++;
                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                }
                if (searchParameters.Count > 0)
                    selectProductQuery = selectProductQuery + query;
            }

            DataTable invoiceSequenceMappingData = dataAccessHandler.executeSelectQuery(selectProductQuery, parameters.ToArray(), sqlTransaction);
            if (invoiceSequenceMappingData.Rows.Count > 0)
            {
                List<InvoiceSequenceMappingDTO> invoiceSequenceMappingList = new List<InvoiceSequenceMappingDTO>();
                foreach (DataRow invoiceSequenceMappingDataRow in invoiceSequenceMappingData.Rows)
                {
                    InvoiceSequenceMappingDTO invoiceSequenceMappingDataObject = GetInvoiceSequenceMappingDTO(invoiceSequenceMappingDataRow);
                    invoiceSequenceMappingList.Add(invoiceSequenceMappingDataObject);
                }
                log.LogMethodExit(invoiceSequenceMappingList);
                return invoiceSequenceMappingList;
            }
            else
            {
                log.LogMethodExit(null);
                return null;
            }
        }
    }
}
