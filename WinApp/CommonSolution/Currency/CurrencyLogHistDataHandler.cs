/********************************************************************************************
 * Project Name -Currency
 * Description  -Data object of Currency
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        28-Sep-2016   Amaresh          Created 
 *2.70        01-Jul -2019  Girish Kundar   Modified : Moved to Product Module
 *                                                      Added CreatedBy column and Changed the Structure of Data Handler.
 *2.70.2        05-Dec-2019   Jinto Thomas            Removed siteid from update query
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Currency
{
    /// <summary>
    /// CurrencyUpdateLogDataHandler - Handles insert, update and select of currencyUpdateLog objects
    /// </summary>
    public class CurrencyLogHistDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string SELECT_QUERY = @"SELECT * FROM CurrencyLogHist AS clh";
        private readonly SqlTransaction sqlTransaction;
        private static readonly Dictionary<CurrencyLogHistDTO.SearchByCurrencyLogHistParameters, string> DBSearchParameters = new Dictionary<CurrencyLogHistDTO.SearchByCurrencyLogHistParameters, string>
            {
                {CurrencyLogHistDTO.SearchByCurrencyLogHistParameters.LOG_ID, "clh.LogId"},
                {CurrencyLogHistDTO.SearchByCurrencyLogHistParameters.CURRENCY_ID, "clh.Currency_Id"},
                {CurrencyLogHistDTO.SearchByCurrencyLogHistParameters.MASTER_ENTITY_ID, "clh.MasterEntityId"},
                {CurrencyLogHistDTO.SearchByCurrencyLogHistParameters.CURRENCY_CODE, "clh.CurrencyCode"},
                {CurrencyLogHistDTO.SearchByCurrencyLogHistParameters.IS_ACTIVE, "clh.IsActive"},
                {CurrencyLogHistDTO.SearchByCurrencyLogHistParameters.SITE_ID, "clh.site_id"},
            };
        DataAccessHandler dataAccessHandler;

        /// <summary>
        ///  Parameterized Constructor for CurrencyLogHistDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object is passed, default value is null</param>
        public CurrencyLogHistDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating CurrencyLogHist Record.
        /// </summary>
        /// <param name="currencyLogHistDTO">CurrencyLogHistDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(CurrencyLogHistDTO currencyLogHistDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(currencyLogHistDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@logId", currencyLogHistDTO.LogId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@currencyId", currencyLogHistDTO.CurrencyId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@currencyCode", string.IsNullOrEmpty(currencyLogHistDTO.CurrencySymbol) ? DBNull.Value : (Object)currencyLogHistDTO.CurrencyCode));
            parameters.Add(dataAccessHandler.GetSQLParameter("@currencySymbol", string.IsNullOrEmpty(currencyLogHistDTO.CurrencySymbol) ? DBNull.Value: (Object)currencyLogHistDTO.CurrencySymbol));
            parameters.Add(dataAccessHandler.GetSQLParameter("@description", string.IsNullOrEmpty(currencyLogHistDTO.Description) ? DBNull.Value : (Object)currencyLogHistDTO.Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@buyRate", currencyLogHistDTO.BuyRate == 0 ?  DBNull.Value : (object)currencyLogHistDTO.BuyRate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@sellRate", currencyLogHistDTO.SellRate == 0 ? DBNull.Value : (object)currencyLogHistDTO.SellRate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", currencyLogHistDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastModifiedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", currencyLogHistDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@effectiveStartDate", currencyLogHistDTO.EffectiveStartDate == DateTime.MinValue ? DBNull.Value :(object) currencyLogHistDTO.EffectiveStartDate));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the CurrencyLogHist record to the database
        /// </summary>
        /// <param name="currencyLogHistDTO">CurrencyLogHistDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns CurrencyLogHistDTO</returns>
        public CurrencyLogHistDTO InsertCurrencyLogHist(CurrencyLogHistDTO currencyLogHistDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(currencyLogHistDTO, loginId, siteId);
            string insertCurrencyLogHistQuery = @"insert into CurrencyLogHist 
                                                        (
                                                        Currency_Id,
                                                        CurrencyCode,
                                                        CurrencySymbol,
                                                        Description,
                                                        BuyRate,
                                                        SellRate,
                                                        CreatedDate,
                                                        LastModifiedDate,
                                                        LastModifiedBy,
                                                        EffectiveStartDate,
                                                        EffectiveEndDate,
                                                        IsActive,
                                                        Guid,
                                                        site_id,
                                                        MasterEntityId,
                                                        CreatedBy
                                                        ) 
                                                values 
                                                       ( 
                                                        @currencyId,
                                                        @currencyCode,
                                                        @currencySymbol,
                                                        @description,
                                                        @buyRate,
                                                        @sellRate,
                                                        Getdate(),
                                                        Getdate(),        
                                                        @lastModifiedBy,
                                                        @effectiveStartDate,
                                                        Getdate(),
                                                        @isActive,
                                                        NEWID(),
                                                        @siteId,
                                                        @masterEntityId,
                                                        @createdBy
                                                        )SELECT * from CurrencyLogHist where LogId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertCurrencyLogHistQuery, GetSQLParameters(currencyLogHistDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCurrencyLogHistDTO(currencyLogHistDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting CurrencyLogHistDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(currencyLogHistDTO);
            return currencyLogHistDTO;
        }
        /// <summary>
        ///  updates the CurrencyLogHistDTO with Id ,who columns values for further process.
        /// </summary>
        /// <param name="currencyDTO">CurrencyLogHistDTO object is passed</param>
        /// <param name="dt">dt an object of DataTable</param>
        private void RefreshCurrencyLogHistDTO(CurrencyLogHistDTO currencyLogHistDTO, DataTable dt)
        {
            log.LogMethodEntry(currencyLogHistDTO, dt);
            if (dt.Rows.Count > 0)
            {
                currencyLogHistDTO.LogId = Convert.ToInt32(dt.Rows[0]["LogId"]);
                currencyLogHistDTO.LastModifiedDate = dt.Rows[0]["LastModifiedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dt.Rows[0]["LastModifiedDate"]);
                currencyLogHistDTO.Guid = dt.Rows[0]["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dt.Rows[0]["Guid"]);
                currencyLogHistDTO.LastModifiedBy = dt.Rows[0]["LastModifiedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dt.Rows[0]["LastModifiedBy"]);
                currencyLogHistDTO.SiteId = dt.Rows[0]["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dt.Rows[0]["site_id"]);
                currencyLogHistDTO.CreatedBy = dt.Rows[0]["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dt.Rows[0]["CreatedBy"]);
                currencyLogHistDTO.CreatedDate = dt.Rows[0]["CreatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dt.Rows[0]["CreatedDate"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Updates the CurrencyLogHist record
        /// </summary>
        /// <param name="currencyLogHistDTO">CurrencyLogHistDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the CurrencyLogHistDTO</returns>
        public CurrencyLogHistDTO UpdateCurrencyLogHist(CurrencyLogHistDTO currencyLogHistDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(currencyLogHistDTO, loginId, siteId);
            string updateCurrencyLogHistQuery = @"update CurrencyLogHist 
                                                    set Currency_Id = @currencyId,
                                                    CurrencyCode = @currencyCode,
                                                    Description = @description,
                                                    BuyRate = @buyRate,
                                                    SellRate = @sellRate,
                                                    LastModifiedDate = GETDATE(),
                                                    LastModifiedBy = @lastModifiedBy,
                                                    EffectiveStartDate = @effectiveStartDate,
                                                    EffectiveEndDate = GETDATE(),
                                                    IsActive =@isActive,
                                                    --site_id = @siteId,
                                                    MasterEntityId = @masterEntityId,
                                                    CurrencySymbol = @currencySymbol
                                                    WHERE LogId = @logId
                                                    SELECT * from CurrencyLogHist where LogId = @logId ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateCurrencyLogHistQuery, GetSQLParameters(currencyLogHistDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCurrencyLogHistDTO(currencyLogHistDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating CurrencyLogHistDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(currencyLogHistDTO);
            return currencyLogHistDTO;
        }

        /// <summary>
        /// Converts the Data row object to CurrencyLogHistDTO class type
        /// </summary>
        /// <param name="currencyLogHistDataRow">Currency DataRow</param>
        /// <returns>Returns CurrencyLogHistDTO</returns>
        private CurrencyLogHistDTO GetCurrencyLogHistDTO(DataRow currencyLogHistDataRow)
        {
            log.LogMethodEntry(currencyLogHistDataRow);
            CurrencyLogHistDTO currencyLogHistDTO = new CurrencyLogHistDTO(
                                            currencyLogHistDataRow["LogId"] == DBNull.Value ? -1 : Convert.ToInt32(currencyLogHistDataRow["LogId"]),
                                            currencyLogHistDataRow["Currency_Id"] == DBNull.Value ? -1 : Convert.ToInt32(currencyLogHistDataRow["Currency_Id"]),
                                            currencyLogHistDataRow["CurrencyCode"] == DBNull.Value ? string.Empty : Convert.ToString(currencyLogHistDataRow["CurrencyCode"]).Trim(),
                                            currencyLogHistDataRow["CurrencySymbol"] == DBNull.Value ? string.Empty : Convert.ToString(currencyLogHistDataRow["CurrencySymbol"]),
                                            currencyLogHistDataRow["Description"] == DBNull.Value ? string.Empty : Convert.ToString(currencyLogHistDataRow["Description"]),
                                            currencyLogHistDataRow["BuyRate"] == DBNull.Value ? 0.0 : Convert.ToDouble(currencyLogHistDataRow["BuyRate"]),
                                            currencyLogHistDataRow["SellRate"] == DBNull.Value ? 0.0 : Convert.ToDouble(currencyLogHistDataRow["SellRate"]),
                                            currencyLogHistDataRow["CreatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(currencyLogHistDataRow["CreatedDate"]),
                                            currencyLogHistDataRow["LastModifiedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(currencyLogHistDataRow["LastModifiedDate"]),
                                            currencyLogHistDataRow["LastModifiedBy"] == DBNull.Value ? string.Empty : Convert.ToString(currencyLogHistDataRow["CreatedBy"]),
                                            currencyLogHistDataRow["EffectiveStartDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(currencyLogHistDataRow["EffectiveStartDate"]),
                                            currencyLogHistDataRow["EffectiveEndDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(currencyLogHistDataRow["EffectiveEndDate"]),
                                            currencyLogHistDataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(currencyLogHistDataRow["IsActive"]),
                                            currencyLogHistDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(currencyLogHistDataRow["Guid"]),
                                            currencyLogHistDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(currencyLogHistDataRow["site_id"]),
                                            currencyLogHistDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(currencyLogHistDataRow["SynchStatus"]),
                                            currencyLogHistDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(currencyLogHistDataRow["MasterEntityId"]),
                                            currencyLogHistDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(currencyLogHistDataRow["CreatedBy"])
                                            );
            log.LogMethodExit(currencyLogHistDTO);
            return currencyLogHistDTO;
        }

        /// <summary>
        /// Gets the currency data of passed Id
        /// </summary>
        /// <param name="logId">Int type parameter</param>
        /// <returns>Returns CurrencyLogHistDTO</returns>
        public CurrencyLogHistDTO GetCurrencyLogHist(int logId)
        {
            log.LogMethodEntry(logId);
            CurrencyLogHistDTO result = null;
            string selectCurrencyQuery = SELECT_QUERY+ "  WHERE clh.LogId = @logId";
            SqlParameter[] selectCurrencyParameters = new SqlParameter[1];
            selectCurrencyParameters[0] = new SqlParameter("@logId", logId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectCurrencyQuery, selectCurrencyParameters,sqlTransaction);

            if (dataTable.Rows.Count > 0)
            {
                result = GetCurrencyLogHistDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the CurrencyLogHistDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of CurrencyLogHistDTO matching the search criteria</returns>
        public List<CurrencyLogHistDTO> GetCurrencyLogHistList(List<KeyValuePair<CurrencyLogHistDTO.SearchByCurrencyLogHistParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectCurrencyLogHistQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<CurrencyLogHistDTO> currencyLogHistDTOList = null;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<CurrencyLogHistDTO.SearchByCurrencyLogHistParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";

                        if (searchParameter.Key == CurrencyLogHistDTO.SearchByCurrencyLogHistParameters.LOG_ID ||
                            searchParameter.Key == CurrencyLogHistDTO.SearchByCurrencyLogHistParameters.CURRENCY_ID ||
                            searchParameter.Key == CurrencyLogHistDTO.SearchByCurrencyLogHistParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CurrencyLogHistDTO.SearchByCurrencyLogHistParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CurrencyLogHistDTO.SearchByCurrencyLogHistParameters.CURRENCY_CODE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",' ') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
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
                    count++;
                }      
                
                    selectCurrencyLogHistQuery = selectCurrencyLogHistQuery + query;
            }
            DataTable CurrencyData = dataAccessHandler.executeSelectQuery(selectCurrencyLogHistQuery, parameters.ToArray(), sqlTransaction);

            if (CurrencyData.Rows.Count > 0)
            {
                currencyLogHistDTOList = new List<CurrencyLogHistDTO>();
                foreach (DataRow CurrencyDataRow in CurrencyData.Rows)
                {
                    CurrencyLogHistDTO currencyLogHistDTO = GetCurrencyLogHistDTO(CurrencyDataRow);
                    currencyLogHistDTOList.Add(currencyLogHistDTO);
                }
            }
            log.LogMethodExit(currencyLogHistDTOList);
            return currencyLogHistDTOList;
        }
    }
}
