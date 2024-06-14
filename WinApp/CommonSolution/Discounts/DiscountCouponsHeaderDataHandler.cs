/********************************************************************************************
 * Project Name - DiscountCouponsHeader Data Handler
 * Description  - Data handler of the DiscountCouponsHeader class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        16-Jul-2017   Lakshminarayana     Created 
 *1.01        30-Oct-2017   Lakshminarayana     Modified   Option to choose generated coupons to sequential or random, Allow multiple coupons in one transaction 
 *2.60        05-Mar-2019   Akshay Gulaganji    Modified BuildSQLParameters() method (i.e.,) handling isActive
 *2.60        25-Mar-2019   Mushahid Faizan     Modified- Author Version, added log Method Entry & Exit, removed unused namespaces 
 *2.70.2        31-Jul-2019   Girish kundar       Modified- Added Missing Who columns to Insert/Update methods.
 *                                                        LogMethodEntry() and LogMethodExit()
 *2.120.2      12-May-2021  Mushahid Faizan     Added COUPON_EXPIRY_DATE search param for WMS UI.                                                        
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Discounts
{
    /// <summary>
    ///  DiscountCouponsHeader Data Handler - Handles insert, update and select of  DiscountCouponsHeader objects
    /// </summary>
    public class DiscountCouponsHeaderDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<DiscountCouponsHeaderDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<DiscountCouponsHeaderDTO.SearchByParameters, string>
            {
                {DiscountCouponsHeaderDTO.SearchByParameters.ID, "dch.Id"},
                {DiscountCouponsHeaderDTO.SearchByParameters.DISCOUNT_ID, "dch.DiscountId"},
                {DiscountCouponsHeaderDTO.SearchByParameters.IS_ACTIVE, "dch.IsActive"},
                {DiscountCouponsHeaderDTO.SearchByParameters.MASTER_ENTITY_ID,"dch.MasterEntityId"},
                {DiscountCouponsHeaderDTO.SearchByParameters.COUPON_EXPIRY_DATE,"dch.ExpiryDate"},
                {DiscountCouponsHeaderDTO.SearchByParameters.SITE_ID, "dch.site_id"}
            };
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction = null;
        private const string SELECT_QUERY = @"SELECT * FROM DiscountCouponsHeader AS dch ";
        /// <summary>
        /// Default constructor of DiscountCouponsHeaderDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public DiscountCouponsHeaderDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        ///  SQL parameter generator for Insert and Update Queries.
        /// </summary>
        /// <param name="parameters">parameters</param>
        /// <param name="parameterName">parameterName</param>
        /// <param name="value">value</param>
        /// <param name="negetiveValueNull">negetiveValueNull</param>
        private void ParameterHelper(List<SqlParameter> parameters, string parameterName, object value, bool negetiveValueNull = false)
        {
            log.LogMethodEntry(parameters, parameterName, value, negetiveValueNull);
            if (parameters != null && !string.IsNullOrEmpty(parameterName))
            {
                if (value is int)
                {
                    if (negetiveValueNull && ((int)value) < 0)
                    {
                        parameters.Add(new SqlParameter(parameterName, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(parameterName, value));
                    }
                }
                else if (value is string)
                {
                    if (string.IsNullOrEmpty(value as string))
                    {
                        parameters.Add(new SqlParameter(parameterName, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(parameterName, value));
                    }
                }
                else
                {
                    if (value == null)
                    {
                        parameters.Add(new SqlParameter(parameterName, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(parameterName, value));
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating DiscountCouponsHeader Record.
        /// </summary>
        /// <param name="discountCouponsHeaderDTO">DiscountCouponsHeaderDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> BuildSQLParameters(DiscountCouponsHeaderDTO discountCouponsHeaderDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(discountCouponsHeaderDTO,  loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            ParameterHelper(parameters, "@Id", discountCouponsHeaderDTO.Id, true);
            ParameterHelper(parameters, "@DiscountId", discountCouponsHeaderDTO.DiscountId, true);
            ParameterHelper(parameters, "@ExpiryDate", discountCouponsHeaderDTO.ExpiryDate);
            ParameterHelper(parameters, "@Count", discountCouponsHeaderDTO.Count);
            ParameterHelper(parameters, "@EffectiveDate", discountCouponsHeaderDTO.EffectiveDate);
            ParameterHelper(parameters, "@ExpiresInDays", discountCouponsHeaderDTO.ExpiresInDays);
            ParameterHelper(parameters, "@IsActive", discountCouponsHeaderDTO.IsActive ? "Y" : "N");
            ParameterHelper(parameters, "@Sequential", discountCouponsHeaderDTO.Sequential);
            ParameterHelper(parameters, "@PrintCoupon", discountCouponsHeaderDTO.PrintCoupon);
            ParameterHelper(parameters, "@LastUpdatedBy", loginId);
            ParameterHelper(parameters, "@CreatedBy", loginId);
            ParameterHelper(parameters, "@LastUpdatedDate", DateTime.Now);
            ParameterHelper(parameters, "@site_id", siteId, true);
            ParameterHelper(parameters, "@MasterEntityId", discountCouponsHeaderDTO.MasterEntityId, true);
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the DiscountCouponsHeader record to the database
        /// </summary>
        /// <param name="discountCouponsHeaderDTO">DiscountCouponsHeaderDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns DiscountCouponsHeaderDTO</returns>
        public DiscountCouponsHeaderDTO InsertDiscountCouponsHeader(DiscountCouponsHeaderDTO discountCouponsHeaderDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(discountCouponsHeaderDTO, loginId, siteId);
            string query = @"INSERT INTO DiscountCouponsHeader 
                                        ( 
                                            DiscountId,
                                            ExpiryDate,
                                            EffectiveDate,
                                            ExpiresInDays,
                                            Count,
                                            IsActive,
                                            PrintCoupon,
                                            Sequential,
                                            LastUpdatedBy,
                                            LastUpdatedDate,
                                            CreatedBy,
                                            CreationDate,
                                            site_id,
                                            MasterEntityId
                                        ) 
                                VALUES 
                                        (
                                            @DiscountId,
                                            @ExpiryDate,
                                            @EffectiveDate,
                                            @ExpiresInDays,
                                            @Count,
                                            @IsActive,
                                            @PrintCoupon,
                                            @Sequential,
                                            @LastUpdatedBy,
                                            getDate(),
                                            @CreatedBy,
                                            getDate(),
                                            @site_id,
                                            @MasterEntityId
                                        )  SELECT* from DiscountCouponsHeader where Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, BuildSQLParameters(discountCouponsHeaderDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshDiscountCouponsHeaderDTO(discountCouponsHeaderDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting discountCouponsHeaderDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(discountCouponsHeaderDTO);
            return discountCouponsHeaderDTO;
        }

        /// <summary>
        /// Updates the DiscountCouponsHeader record
        /// </summary>
        /// <param name="discountCouponsHeaderDTO">DiscountCouponsHeaderDTO type parameter</param>
        /// <param name="loginId">User updating the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns DiscountCouponsHeaderDTO</returns>
        public DiscountCouponsHeaderDTO UpdateDiscountCouponsHeader(DiscountCouponsHeaderDTO discountCouponsHeaderDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(discountCouponsHeaderDTO, loginId, siteId);
            string query = @"UPDATE DiscountCouponsHeader 
                             SET DiscountId=@DiscountId,
                                 ExpiryDate=@ExpiryDate,
                                 EffectiveDate=@EffectiveDate,
                                 ExpiresInDays=@ExpiresInDays,
                                 Count=@Count,
                                 IsActive=@IsActive,
                                 PrintCoupon=@PrintCoupon,
                                 Sequential=@Sequential,
                                 LastUpdatedBy=@LastUpdatedBy,
                                 LastUpdatedDate = getDate(),
                                 MasterEntityId=@MasterEntityId
                             WHERE Id = @Id
                             SELECT * from DiscountCouponsHeader where Id = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, BuildSQLParameters(discountCouponsHeaderDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshDiscountCouponsHeaderDTO(discountCouponsHeaderDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting discountCouponsHeaderDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(discountCouponsHeaderDTO);
            return discountCouponsHeaderDTO;
        }


        /// <summary>
        ///  updates the DiscountCouponsHeaderDTO with Id ,who columns values for further process.
        /// </summary>
        /// <param name="discountCouponsHeaderDTO">DiscountCouponsHeaderDTO object is passed</param>
        /// <param name="dt">dt an object of DataTable</param>
        private void RefreshDiscountCouponsHeaderDTO(DiscountCouponsHeaderDTO discountCouponsHeaderDTO, DataTable dt)
        {
            log.LogMethodEntry(discountCouponsHeaderDTO, dt);
            if (dt.Rows.Count > 0)
            {
                discountCouponsHeaderDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                discountCouponsHeaderDTO.LastUpdatedDate = dt.Rows[0]["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dt.Rows[0]["LastUpdatedDate"]);
                discountCouponsHeaderDTO.Guid = dt.Rows[0]["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dt.Rows[0]["Guid"]);
                discountCouponsHeaderDTO.LastUpdatedBy = dt.Rows[0]["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dt.Rows[0]["LastUpdatedBy"]);
                discountCouponsHeaderDTO.SiteId = dt.Rows[0]["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dt.Rows[0]["site_id"]);
                discountCouponsHeaderDTO.CreatedBy = dt.Rows[0]["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dt.Rows[0]["CreatedBy"]);
                discountCouponsHeaderDTO.CreationDate = dt.Rows[0]["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dt.Rows[0]["CreationDate"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Converts the Data row object to DiscountCouponsHeaderDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns DiscountCouponsHeaderDTO</returns>
        private DiscountCouponsHeaderDTO GetDiscountCouponsHeaderDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            DiscountCouponsHeaderDTO discountCouponsHeaderDTO = new DiscountCouponsHeaderDTO(Convert.ToInt32(dataRow["Id"]),
                                            dataRow["DiscountId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["DiscountId"]),
                                            dataRow["ExpiryDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["ExpiryDate"]),
                                            dataRow["EffectiveDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["EffectiveDate"]),
                                            dataRow["ExpiresInDays"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["ExpiresInDays"]),
                                            dataRow["Count"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["Count"]),
                                            dataRow["IsActive"] == DBNull.Value ? true : Convert.ToString(dataRow["IsActive"]) == "Y",
                                            dataRow["PrintCoupon"] == DBNull.Value ? "N" : Convert.ToString(dataRow["PrintCoupon"]),
                                            dataRow["Sequential"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["Sequential"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? "" : Convert.ToString(dataRow["LastUpdatedBy"]),
                                            dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty: dataRow["Guid"].ToString(),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"])
                                            );
            log.LogMethodExit(discountCouponsHeaderDTO);
            return discountCouponsHeaderDTO;
        }

        /// <summary>
        /// Gets the DiscountCouponsHeader data of passed DiscountCouponsHeader Id
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns DiscountCouponsHeaderDTO</returns>
        public DiscountCouponsHeaderDTO GetDiscountCouponsHeaderDTO(int id)
        {
            log.LogMethodEntry(id);
            DiscountCouponsHeaderDTO returnValue = null;
            string query = SELECT_QUERY + "    WHERE dch.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter },sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetDiscountCouponsHeaderDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Gets the DiscountCouponsHeaderDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of DiscountCouponsHeaderDTO matching the search criteria</returns>
        public List<DiscountCouponsHeaderDTO> GetDiscountCouponsHeaderDTOList(List<KeyValuePair<DiscountCouponsHeaderDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<DiscountCouponsHeaderDTO> list = null;
            int count = 0;
            string selectQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<DiscountCouponsHeaderDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == DiscountCouponsHeaderDTO.SearchByParameters.ID ||
                            searchParameter.Key == DiscountCouponsHeaderDTO.SearchByParameters.DISCOUNT_ID ||
                            searchParameter.Key == DiscountCouponsHeaderDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == DiscountCouponsHeaderDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == DiscountCouponsHeaderDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
                        }
                        else if (searchParameter.Key == DiscountCouponsHeaderDTO.SearchByParameters.COUPON_EXPIRY_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
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
                selectQuery = selectQuery + query;
            }

            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<DiscountCouponsHeaderDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    DiscountCouponsHeaderDTO discountCouponsHeaderDTO = GetDiscountCouponsHeaderDTO(dataRow);
                    list.Add(discountCouponsHeaderDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }
    }
}
