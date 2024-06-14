/********************************************************************************************
 * Project Name -Currency
 * Description  -Data Handler of Currency
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        21-Sep-2016   Amaresh          Created 
 *2.70        01-Jul -2019   Girish Kundar   Modified : Moved to Product Module
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
    /// CurrencyDataHandler - Handles insert, update and select of currency objects
    /// </summary>
    public class CurrencyDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string SELECT_QUERY = @"SELECT * FROM Currency AS c ";
        private readonly SqlTransaction sqlTransaction;
        private static readonly Dictionary<CurrencyDTO.SearchByCurrencyParameters, string> DBSearchParameters = new Dictionary<CurrencyDTO.SearchByCurrencyParameters, string>
        {
                {CurrencyDTO.SearchByCurrencyParameters.CURRENCY_ID, "c.CurrencyId"},
                {CurrencyDTO.SearchByCurrencyParameters.CURRENCY_CODE, "c.CurrencyCode"},
                {CurrencyDTO.SearchByCurrencyParameters.MASTER_ENTITY_ID, "c.MasterEntityId"},
                {CurrencyDTO.SearchByCurrencyParameters.IS_ACTIVE, "c.IsActive"},
                {CurrencyDTO.SearchByCurrencyParameters.SITE_ID, "c.site_id"},
            };

        DataAccessHandler dataAccessHandler;
        /// <summary>
        ///  Parameterized Constructor for CurrencyDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object is passed, default value is null</param>
        public CurrencyDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating Currency Record.
        /// </summary>
        /// <param name="currencyDTO">CurrencyDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(CurrencyDTO currencyDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(currencyDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@currencyId", currencyDTO.CurrencyId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@currencyCode", currencyDTO.CurrencyCode));
            parameters.Add(dataAccessHandler.GetSQLParameter("@currencySymbol", string.IsNullOrEmpty(currencyDTO.CurrencySymbol) ? DBNull.Value.ToString() : currencyDTO.CurrencySymbol));
            parameters.Add(dataAccessHandler.GetSQLParameter("@description", string.IsNullOrEmpty(currencyDTO.Description) ? DBNull.Value.ToString() : currencyDTO.Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@buyRate", currencyDTO.BuyRate == 0 ? DBNull.Value : (object)currencyDTO.BuyRate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@sellRate", currencyDTO.SellRate == 0 ? DBNull.Value : (object)currencyDTO.SellRate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", currencyDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastModifiedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", currencyDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        ///  updates the CurrencyDTO with Id ,who columns values for further process.
        /// </summary>
        /// <param name="currencyDTO">currencyDTO object is passed</param>
        /// <param name="dt">dt an object of DataTable</param>
        private void RefreshCurrencyDTO(CurrencyDTO currencyDTO, DataTable dt)
        {
            log.LogMethodEntry(currencyDTO, dt);
            if (dt.Rows.Count > 0)
            {
                currencyDTO.CurrencyId = Convert.ToInt32(dt.Rows[0]["CurrencyId"]);
                currencyDTO.LastModifiedDate = dt.Rows[0]["LastModifiedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dt.Rows[0]["LastModifiedDate"]);
                currencyDTO.Guid = dt.Rows[0]["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dt.Rows[0]["Guid"]);
                currencyDTO.LastModifiedBy = dt.Rows[0]["LastModifiedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dt.Rows[0]["LastModifiedBy"]);
                currencyDTO.SiteId = dt.Rows[0]["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dt.Rows[0]["site_id"]);
                currencyDTO.CreatedBy = dt.Rows[0]["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dt.Rows[0]["CreatedBy"]);
                currencyDTO.CreatedDate = dt.Rows[0]["CreatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dt.Rows[0]["CreatedDate"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Inserts the currency record to the database
        /// </summary>
        /// <param name="currencyDTO">CurrencyDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns CurrencyDTO</returns>
        public CurrencyDTO InsertCurrency(CurrencyDTO currencyDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(currencyDTO, loginId, siteId);
            string insertCurrencyQuery = @"insert into Currency 
                                                        (
                                                        CurrencyCode,
                                                        CurrencySymbol,
                                                        Description,
                                                        BuyRate,
                                                        SellRate,
                                                        EffectiveDate,
                                                        CreatedDate,
                                                        LastModifiedDate,
                                                        LastModifiedBy,
                                                        IsActive,
                                                        Guid,
                                                        site_id,
                                                        MasterEntityId,
                                                        CreatedBy
                                                        ) 
                                                values 
                                                       ( 
                                                        @currencyCode,
                                                        @currencySymbol,
                                                        @description,
                                                        @buyRate,
                                                        @sellRate,
                                                        Getdate(),
                                                        Getdate(),
                                                        Getdate(),        
                                                        @lastModifiedBy,
                                                        @isActive,
                                                        NEWID(),
                                                        @siteId,
                                                        @masterEntityId,
                                                        @createdBy
                                                        )SELECT * from Currency where CurrencyId = scope_identity()";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertCurrencyQuery, GetSQLParameters(currencyDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCurrencyDTO(currencyDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting CurrencyDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(currencyDTO);
            return currencyDTO;
        }

        /// <summary>
        /// Updates the Currency record
        /// </summary>
        /// <param name="currencyDTO">CurrencyDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns CurrencyDTO</returns>
        public CurrencyDTO UpdateCurrency(CurrencyDTO currencyDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(currencyDTO, loginId, siteId);
            string updateCurrencyQuery = @"update Currency 
                                             set CurrencyCode=@currencyCode,
                                             CurrencySymbol = @currencySymbol,
                                             Description=@description,
                                             BuyRate=@buyRate,
                                             SellRate=@sellRate,
                                             LastModifiedDate=Getdate(),
                                             LastModifiedBy= @lastModifiedBy,
                                             EffectiveDate=Getdate(),
                                             IsActive =@isActive,
                                             --site_id=@siteId,
                                             MasterEntityId = @masterEntityId
                                             WHERE CurrencyId = @currencyId
                                             select * from Currency where CurrencyId = @currencyId ";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateCurrencyQuery, GetSQLParameters(currencyDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCurrencyDTO(currencyDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating CurrencyDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(currencyDTO);
            return currencyDTO;
        }

        /// <summary>
        /// Converts the Data row object to CurrencyDTO class type
        /// </summary>
        /// <param name="currencyDataRow">Currency DataRow</param>
        /// <returns>Returns CurrencyDTO</returns>
        private CurrencyDTO GetCurrencyDTO(DataRow currencyDataRow)
        {
            log.LogMethodEntry(currencyDataRow);
            CurrencyDTO currencyDTO = new CurrencyDTO(
                                            currencyDataRow["CurrencyId"] == DBNull.Value ? -1 : Convert.ToInt32(currencyDataRow["CurrencyId"]),
                                            currencyDataRow["CurrencyCode"] == DBNull.Value ? string.Empty : currencyDataRow["CurrencyCode"].ToString(),
                                            currencyDataRow["CurrencySymbol"] == DBNull.Value ? string.Empty : currencyDataRow["CurrencySymbol"].ToString(),
                                            currencyDataRow["Description"] == DBNull.Value ? string.Empty : currencyDataRow["Description"].ToString(),
                                            currencyDataRow["BuyRate"] == DBNull.Value ? 0.0 : Convert.ToDouble(currencyDataRow["BuyRate"]),
                                            currencyDataRow["SellRate"] == DBNull.Value ? 0.0 : Convert.ToDouble(currencyDataRow["SellRate"]),
                                            currencyDataRow["EffectiveDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(currencyDataRow["EffectiveDate"]),
                                            currencyDataRow["CreatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(currencyDataRow["CreatedDate"]),
                                            currencyDataRow["LastModifiedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(currencyDataRow["LastModifiedDate"]),
                                            currencyDataRow["LastModifiedBy"] == DBNull.Value ? string.Empty : currencyDataRow["LastModifiedBy"].ToString(),
                                            currencyDataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(currencyDataRow["IsActive"]),
                                            currencyDataRow["Guid"] == DBNull.Value ? string.Empty : currencyDataRow["Guid"].ToString(),
                                            currencyDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(currencyDataRow["site_id"]),
                                            currencyDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(currencyDataRow["SynchStatus"]),
                                            currencyDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(currencyDataRow["MasterEntityId"]),
                                            currencyDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(currencyDataRow["CreatedBy"])
                                            );
            log.LogMethodExit();
            return currencyDTO;
        }

        /// <summary>
        /// Gets the currency data of passed Id
        /// </summary>
        /// <param name="currencyId">Int type parameter</param>
        /// <returns>Returns CurrencyDTO</returns>
        public CurrencyDTO GetCurrency(int currencyId)
        {
            log.LogMethodEntry(currencyId);
            string selectCurrencyQuery = SELECT_QUERY + "WHERE c.CurrencyId = @currencyId";
            CurrencyDTO CurrencyDataObject = null;
            SqlParameter[] selectCurrencyParameters = new SqlParameter[1];
            selectCurrencyParameters[0] = new SqlParameter("@currencyId", currencyId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectCurrencyQuery, selectCurrencyParameters, sqlTransaction);

            if (dataTable.Rows.Count > 0)
            {
                CurrencyDataObject = GetCurrencyDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(CurrencyDataObject);
            return CurrencyDataObject;
        }

        /// <summary>
        /// Gets the CurrencyDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of CurrencyDTO matching the search criteria</returns>
        public List<CurrencyDTO> GetCurrencyList(List<KeyValuePair<CurrencyDTO.SearchByCurrencyParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<CurrencyDTO> currencyDTOList = null; 
            string selectCurrencyQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<CurrencyDTO.SearchByCurrencyParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : "  and ";

                        if (searchParameter.Key == CurrencyDTO.SearchByCurrencyParameters.CURRENCY_ID
                            || searchParameter.Key == CurrencyDTO.SearchByCurrencyParameters.MASTER_ENTITY_ID)

                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == CurrencyDTO.SearchByCurrencyParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0) =  " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == CurrencyDTO.SearchByCurrencyParameters.CURRENCY_CODE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == CurrencyDTO.SearchByCurrencyParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
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
                selectCurrencyQuery = selectCurrencyQuery + query;
            }
            DataTable CurrencyData = dataAccessHandler.executeSelectQuery(selectCurrencyQuery, parameters.ToArray(), sqlTransaction);

            if (CurrencyData.Rows.Count > 0)
            {
                currencyDTOList = new List<CurrencyDTO>();
                foreach (DataRow CurrencyDataRow in CurrencyData.Rows)
                {
                    CurrencyDTO currencyDTO = GetCurrencyDTO(CurrencyDataRow);
                    currencyDTOList.Add(currencyDTO);
                }
            }
            log.LogMethodExit(currencyDTOList);
            return currencyDTOList;
        }
    }
}

