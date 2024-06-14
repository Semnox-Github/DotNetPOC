/********************************************************************************************
 * Project Name - Transaction
 * Description  - Data Handler object - InvoiceSequenceSetupDataHandler
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
    ///  InvoiceSequenceSetup Data Handler - Handles insert, update and select of  InvoiceSequenceSetup objects
    /// </summary>
    public class InvoiceSequenceSetupDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<InvoiceSequenceSetupDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<InvoiceSequenceSetupDTO.SearchByParameters, string>
            {
                {InvoiceSequenceSetupDTO.SearchByParameters.INVOICE_SEQUENCE_SETUP_ID, "iset.InvoiceSequenceSetupId"},
                {InvoiceSequenceSetupDTO.SearchByParameters.INVOICE_TYPE_ID, "iset.InvoiceTypeId"},
                {InvoiceSequenceSetupDTO.SearchByParameters.ISACTIVE, "iset.IsActive"},
                {InvoiceSequenceSetupDTO.SearchByParameters.MASTER_ENTITY_ID,"iset.MasterEntityId"},
                {InvoiceSequenceSetupDTO.SearchByParameters.SITE_ID, "iset.site_id"},
                {InvoiceSequenceSetupDTO.SearchByParameters.PREFIX, "iset.Prefix"},
                {InvoiceSequenceSetupDTO.SearchByParameters.RESOLUTION_NUMBER, "iset.ResolutionNo"},
                {InvoiceSequenceSetupDTO.SearchByParameters.SERIES_END_NUMBER, "iset.SeriesEndNumber"},
                {InvoiceSequenceSetupDTO.SearchByParameters.SERIES_START_NUMBER, "iset.SeriesStartNumber"},
                {InvoiceSequenceSetupDTO.SearchByParameters.EXPIRY_DATE,"iset.ExpiryDate" },
                {InvoiceSequenceSetupDTO.SearchByParameters.APPROVE_DATE,"iset.ApprovedDate" }
            };
        private DataAccessHandler dataAccessHandler;
        private Utilities utilities;
        private SqlTransaction sqlTransaction = null;
        private const string SELECT_QUERY = @"SELECT * FROM InvoiceSequenceSetup AS iset ";

        /// <summary>
        /// Default constructor of InvoiceSequenceSetupDataHandler class
        /// </summary>
        public InvoiceSequenceSetupDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            utilities = new Utilities();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        private List<SqlParameter> GetSQLParameters(InvoiceSequenceSetupDTO invoiceSequenceSetupDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(invoiceSequenceSetupDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@InvoiceSequenceSetupId", invoiceSequenceSetupDTO.InvoiceSequenceSetupId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@InvoiceTypeId", invoiceSequenceSetupDTO.InvoiceTypeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Prefix", invoiceSequenceSetupDTO.Prefix));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Currval", invoiceSequenceSetupDTO.CurrentValue));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SeriesStartNumber", invoiceSequenceSetupDTO.SeriesStartNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SeriesEndNumber", invoiceSequenceSetupDTO.SeriesEndNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ApprovedDate", invoiceSequenceSetupDTO.ApprovedDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ExpiryDate", invoiceSequenceSetupDTO.ExpiryDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ResolutionNo", invoiceSequenceSetupDTO.ResolutionNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ResolutionDate", invoiceSequenceSetupDTO.ResolutionDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", invoiceSequenceSetupDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@InvoiceGroupId", invoiceSequenceSetupDTO.InvoiceGroupId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", invoiceSequenceSetupDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedUser", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts InvoiceSequenceSetupDTOs
        /// </summary>
        /// <param name="invoiceSequenceSetupDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public InvoiceSequenceSetupDTO InsertInvoiceSequenceSetup(InvoiceSequenceSetupDTO invoiceSequenceSetupDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(invoiceSequenceSetupDTO, loginId, siteId);
            string query = @"insert into InvoiceSequenceSetup 
                                                        (
                                                          InvoiceTypeId,
                                                          Prefix,
                                                          Currval,
                                                          SeriesStartNumber,
                                                          SeriesEndNumber,
                                                          ApprovedDate,
                                                          ExpiryDate,
                                                          ResolutionNo,
                                                          ResolutionDate,
                                                          InvoiceGroupId,
                                                          IsActive,
                                                          LastupdatedDate,
                                                          LastUpdatedUser,
                                                          site_id,
                                                          MasterEntityId,
                                                          Guid,
                                                          CreatedBy,
                                                          CreationDate
                                                        ) 
                                                values 
                                                        (
                                                          @InvoiceTypeId,
                                                          @Prefix,
                                                          @Currval,
                                                          @SeriesStartNumber,
                                                          @SeriesEndNumber,
                                                          @ApprovedDate,
                                                          @ExpiryDate,
                                                          @ResolutionNo,
                                                          @ResolutionDate,
                                                          @InvoiceGroupId,
                                                          @IsActive,
                                                          Getdate(),
                                                          @LastUpdatedUser,
                                                          @site_id,
                                                          @MasterEntityId,
                                                          NewId(),  
                                                          @CreatedBy,
                                                          GetDate()
                                                        )
                                        SELECT * FROM InvoiceSequenceSetup WHERE InvoiceSequenceSetupId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(invoiceSequenceSetupDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshInvoiceSequenceSetupDTO(invoiceSequenceSetupDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting invoiceSequenceSetupDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(invoiceSequenceSetupDTO);
            return invoiceSequenceSetupDTO;
        }
        private void RefreshInvoiceSequenceSetupDTO(InvoiceSequenceSetupDTO invoiceSequenceSetupDTO, DataTable dt)
        {
            log.LogMethodEntry(invoiceSequenceSetupDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                invoiceSequenceSetupDTO.InvoiceSequenceSetupId = Convert.ToInt32(dt.Rows[0]["InvoiceSequenceSetupId"]);
                invoiceSequenceSetupDTO.LastUpdatedDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
                invoiceSequenceSetupDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                invoiceSequenceSetupDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                invoiceSequenceSetupDTO.LastUpdatedBy = dataRow["LastUpdatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedUser"]);
                invoiceSequenceSetupDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                invoiceSequenceSetupDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }
     
        /// <summary>
        /// Updates the InvoiceSequenceSetup record
        /// </summary>
        /// <param name="invoiceSequenceSetupDTO">InvoiceSequenceSetupDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public InvoiceSequenceSetupDTO UpdateInvoiceSequenceSetup(InvoiceSequenceSetupDTO invoiceSequenceSetupDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(invoiceSequenceSetupDTO, loginId, siteId);
            string query = @"update InvoiceSequenceSetup 
                                                          set InvoiceTypeId = @InvoiceTypeId,
                                                          Prefix = @Prefix,
                                                          Currval = @Currval,
                                                          SeriesStartNumber = @SeriesStartNumber,
                                                          SeriesEndNumber = @SeriesEndNumber,
                                                          ApprovedDate =@ApprovedDate,
                                                          ExpiryDate =@ExpiryDate,
                                                          ResolutionNo = @ResolutionNo,
                                                          ResolutionDate = @ResolutionDate,
                                                          InvoiceGroupId = @InvoiceGroupId,
                                                          IsActive = @IsActive,
                                                          LastUpdatedUser = @LastUpdatedUser, 
                                                          LastupdatedDate = Getdate(),
                                                          MasterEntityId =  @MasterEntityId,
                                                          where InvoiceSequenceSetupId = @InvoiceSequenceSetupId
                                       SELECT* FROM InvoiceSequenceSetup WHERE InvoiceSequenceSetupId = @InvoiceSequenceSetupId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(invoiceSequenceSetupDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshInvoiceSequenceSetupDTO(invoiceSequenceSetupDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating invoiceSequenceSetupDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(invoiceSequenceSetupDTO);
            return invoiceSequenceSetupDTO;
        }

        /// <summary>
        /// Converts the Data row object to InvoiceSequenceSetupDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns InvoiceSequenceSetupDTO</returns>
        private InvoiceSequenceSetupDTO GetInvoiceSequenceSetupDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            InvoiceSequenceSetupDTO invoiceSequenceSetupDTO = new InvoiceSequenceSetupDTO(Convert.ToInt32(dataRow["InvoiceSequenceSetupId"]),
                                            dataRow["InvoiceTypeId"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["InvoiceTypeId"]),
                                            dataRow["Prefix"].ToString(),
                                            dataRow["Currval"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["Currval"]),
                                            dataRow["SeriesStartNumber"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["SeriesStartNumber"]),
                                            dataRow["SeriesEndNumber"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["SeriesEndNumber"]),
                                            dataRow["ApprovedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["ApprovedDate"]),
                                            dataRow["ExpiryDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["ExpiryDate"]),
                                            dataRow["ResolutionNo"].ToString(),
                                            dataRow["ResolutionDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["ResolutionDate"]),
                                            dataRow["InvoiceGroupId"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["InvoiceGroupId"]),
                                            dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                                            dataRow["LastUpdatedUser"] == DBNull.Value ? "" : Convert.ToString(dataRow["LastUpdatedUser"]),
                                            dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["Guid"] == DBNull.Value ? "" : dataRow["Guid"].ToString(),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"])
                                            );
            log.LogMethodExit(invoiceSequenceSetupDTO); ;
            return invoiceSequenceSetupDTO;
        }

        /// <summary>
        /// Gets the Event data of passed InvoiceSequenceSetup Id
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns InvoiceSequenceSetupDTO</returns>
        public InvoiceSequenceSetupDTO GetInvoiceSequenceSetupDTO(int id)
        {
            log.LogMethodEntry(id);
            InvoiceSequenceSetupDTO returnValue = null;
            string query = SELECT_QUERY + " WHERE iset.InvoiceSequenceSetupId = @InvoiceSequenceSetupId";
            SqlParameter parameter = new SqlParameter("@InvoiceSequenceSetupId", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetInvoiceSequenceSetupDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Checks whether event is in use.
        /// <param name="id">Invoice Sequence Setup Id</param>
        /// </summary>
        /// <returns>Returns refrenceCount</returns>
        private int GetEventReferenceCount(int id)
        {
            log.LogMethodEntry(id);
            int refrenceCount = 0;
            string query = @"SELECT Count(*) as ReferenceCount
                             FROM InvoiceSequenceMapping
                             WHERE InvoiceSequenceSetupId = @InvoiceSequenceSetupId AND IsActive = 1";
            SqlParameter parameter = new SqlParameter("@InvoiceSequenceSetupId", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                refrenceCount = Convert.ToInt32(dataTable.Rows[0]["ReferenceCount"]);
            }
            log.LogMethodExit(refrenceCount);
            return refrenceCount;
        }

        /// <summary>
        /// Gets the InvoiceSequenceSetupDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of invoiceSequenceSetupDTO matching the search criteria</returns>
        public List<InvoiceSequenceSetupDTO> GetAllInvoiceSequenceSetupList(List<KeyValuePair<InvoiceSequenceSetupDTO.SearchByParameters, string>> searchParameters ,SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry( searchParameters, sqlTransaction);
            int count = 0;
            string selectProductQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if (searchParameters != null && (searchParameters.Count > 0))
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<InvoiceSequenceSetupDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        {
                            if (searchParameter.Key.Equals(InvoiceSequenceSetupDTO.SearchByParameters.INVOICE_SEQUENCE_SETUP_ID)
                                || searchParameter.Key.Equals(InvoiceSequenceSetupDTO.SearchByParameters.INVOICE_TYPE_ID)
                                || searchParameter.Key.Equals(InvoiceSequenceSetupDTO.SearchByParameters.MASTER_ENTITY_ID))
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                            }
                           
                            else if (searchParameter.Key == InvoiceSequenceSetupDTO.SearchByParameters.SITE_ID)
                            {
                                query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                            }
                            else if (searchParameter.Key == InvoiceSequenceSetupDTO.SearchByParameters.ISACTIVE)
                            {
                                query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                            }
                            else if (searchParameter.Key == InvoiceSequenceSetupDTO.SearchByParameters.EXPIRY_DATE)
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + ">=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                            }
                            else if (searchParameter.Key == InvoiceSequenceSetupDTO.SearchByParameters.APPROVE_DATE)
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                            }
                            else if (searchParameter.Key == InvoiceSequenceSetupDTO.SearchByParameters.PREFIX)
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                            }
                            else if (searchParameter.Key == InvoiceSequenceSetupDTO.SearchByParameters.SERIES_START_NUMBER)
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + "<=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                            }
                            else if (searchParameter.Key == InvoiceSequenceSetupDTO.SearchByParameters.SERIES_END_NUMBER)
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + ">=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                            }
                            else if (searchParameter.Key == InvoiceSequenceSetupDTO.SearchByParameters.RESOLUTION_NUMBER)
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

            DataTable invoiceSequenceSetupData = dataAccessHandler.executeSelectQuery(selectProductQuery, parameters.ToArray(),sqlTransaction);
            if (invoiceSequenceSetupData.Rows.Count > 0)
            {
                List<InvoiceSequenceSetupDTO> invoiceSequenceSetupList = new List<InvoiceSequenceSetupDTO>();
                foreach (DataRow invoiceSequenceSetupDataRow in invoiceSequenceSetupData.Rows)
                {
                    InvoiceSequenceSetupDTO invoiceSequenceSetupDataObject = GetInvoiceSequenceSetupDTO(invoiceSequenceSetupDataRow);
                    invoiceSequenceSetupList.Add(invoiceSequenceSetupDataObject);
                }
                log.LogMethodExit(invoiceSequenceSetupList);
                return invoiceSequenceSetupList;
            }
            else
            {
                log.LogMethodExit(null);
                return null;
            }
        }

        /// <summary>
        /// Validates if the entered sequence start and end number is valid
        /// </summary>
        /// <returns></returns>
        public bool ValidateInvoiceSequenceSetup(int? seriesStartNumber , int? seriesEndNumber , string prefix,int Id)
        {
            log.LogMethodEntry(seriesStartNumber, seriesEndNumber, prefix,Id);
             string query = @"select* from InvoiceSequenceSetup
                            where((@seriesStartNumber >= SeriesStartNumber and @seriesStartNumber <= SeriesEndNumber)
                                or( @seriesEndNumber >= SeriesStartNumber and @seriesEndNumber <= SeriesEndNumber)
                                or (@seriesStartNumber <= SeriesStartNumber and @seriesEndNumber >= SeriesEndNumber)
                                or @seriesStartNumber is null or @seriesEndNumber is null)
                                and prefix = @prefix and InvoiceSequenceSetupId != @id and isActive = 1";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@seriesStartNumber", seriesStartNumber == null ? (object)DBNull.Value: seriesStartNumber));
            parameters.Add(new SqlParameter("@seriesEndNumber", seriesEndNumber == null ? (object)DBNull.Value : seriesEndNumber));
            parameters.Add(new SqlParameter("@prefix", prefix));
            parameters.Add(new SqlParameter("@id", Id));
            DataTable dt = dataAccessHandler.executeSelectQuery(query, parameters.ToArray(), null);
            if(dt.Rows.Count > 0)
            {
                log.LogMethodExit(false);
                return false;
            }
            log.LogMethodExit(true);
            return true;
        }
    }

    /// <summary>
    /// Represents foreign key error that occur during application execution. 
    /// </summary>
    public class ForeignKeyException : Exception
    {
        /// <summary>
        /// Default constructor of ForeignKeyException.
        /// </summary>
        public ForeignKeyException()
        {
        }

        /// <summary>
        /// Initializes a new instance of ForeignKeyException class with a specified error message.
        /// </summary>
        /// <param name="message">message</param>
        public ForeignKeyException(string message)
        : base(message)
        {
        }
        /// <summary>
        /// Initializes a new instance of ForeignKeyException class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="inner">inner exception</param>
        public ForeignKeyException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
