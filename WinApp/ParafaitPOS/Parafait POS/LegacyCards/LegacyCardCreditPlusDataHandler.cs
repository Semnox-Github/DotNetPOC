/*/********************************************************************************************
 * Project Name - POS
 * Description  - Data Handler File for LegacyCardCreditPlus
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks 
 *********************************************************************************************
 *2.130.4     03-Sep-2020    Dakshakh                Created 
 ********************************************************************************************/
using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Customer.Accounts;

namespace Parafait_POS
{
    /// <summary>
    /// LegacyCardCreditPlus Data Handler - Handles insert, update and selection of LegacyCardCreditPlus objects
    /// </summary>
    public class LegacyCardCreditPlusDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM LegacyCardCreditPlus as lccp ";

        /// <summary>
        /// Dictionary for searching Parameters for the LegacyCardCreditPlus object.
        /// </summary>
        private static readonly Dictionary<LegacyCardCreditPlusDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<LegacyCardCreditPlusDTO.SearchByParameters, string>
        {
            { LegacyCardCreditPlusDTO.SearchByParameters.LEGACY_CARD_CREDIT_PLUS_ID,"lccpcp.LegacyCardCreditPlusId"},
            { LegacyCardCreditPlusDTO.SearchByParameters.CREDIT_PLUS_TYPE,"lccp.LegacyCreditPlus"},
            { LegacyCardCreditPlusDTO.SearchByParameters.LEGACY_CARD_ID,"lccp.LegacyCard_id"},
            { LegacyCardCreditPlusDTO.SearchByParameters.SITE_ID,"lccp.site_id"},
            { LegacyCardCreditPlusDTO.SearchByParameters.MASTER_ENTITY_ID,"lccp.MasterEntityId"},
            { LegacyCardCreditPlusDTO.SearchByParameters.CARD_ID_LIST,"lccp.LegacyCard_id"},
        };

        /// <summary>
        /// Parameterized Constructor for LegacyCardDataHandler.
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public LegacyCardCreditPlusDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating LegacyCardCreditPlus Record.
        /// </summary>
        /// <param name="LegacyCardCreditPlusDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        private List<SqlParameter> GetSQLParameters(LegacyCardCreditPlusDTO LegacyCardCreditPlusDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(LegacyCardCreditPlusDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@legacyCardCreditPlusId", LegacyCardCreditPlusDTO.LegacyCardCreditPlusId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@legacyCreditPlus", LegacyCardCreditPlusDTO.LegacyCreditPlus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@revisedLegacyCreditPlus", LegacyCardCreditPlusDTO.RevisedLegacyCreditPlus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@creditPlusType", CreditPlusTypeConverter.ToString(LegacyCardCreditPlusDTO.CreditPlusType)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@refundable", LegacyCardCreditPlusDTO.Refundable ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@remarks", LegacyCardCreditPlusDTO.Remarks));
            parameters.Add(dataAccessHandler.GetSQLParameter("@legacyCard_id", LegacyCardCreditPlusDTO.LegacyCard_id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@creditPlusBalance", LegacyCardCreditPlusDTO.CreditPlusBalance));
            parameters.Add(dataAccessHandler.GetSQLParameter("@periodFrom", LegacyCardCreditPlusDTO.PeriodFrom));
            parameters.Add(dataAccessHandler.GetSQLParameter("@periodTo", LegacyCardCreditPlusDTO.PeriodTo));
            parameters.Add(dataAccessHandler.GetSQLParameter("@timeFrom", LegacyCardCreditPlusDTO.TimeFrom));
            parameters.Add(dataAccessHandler.GetSQLParameter("@timeTo", LegacyCardCreditPlusDTO.TimeTo));
            parameters.Add(dataAccessHandler.GetSQLParameter("@numberOfDays", LegacyCardCreditPlusDTO.NumberOfDays));
            parameters.Add(dataAccessHandler.GetSQLParameter("@monday", LegacyCardCreditPlusDTO.Monday ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@tuesday", LegacyCardCreditPlusDTO.Tuesday ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@wednesday", LegacyCardCreditPlusDTO.Wednesday ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@thursday", LegacyCardCreditPlusDTO.Thursday ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@friday", LegacyCardCreditPlusDTO.Friday ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@saturday", LegacyCardCreditPlusDTO.Saturday ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@sunday", LegacyCardCreditPlusDTO.Sunday ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@minimumSaleAmount", LegacyCardCreditPlusDTO.MinimumSaleAmount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ticketAllowed", LegacyCardCreditPlusDTO.TicketAllowed));
            parameters.Add(dataAccessHandler.GetSQLParameter("@expireWithMembership", LegacyCardCreditPlusDTO.ExpireWithMembership ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@pauseAllowed", LegacyCardCreditPlusDTO.PauseAllowed));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", LegacyCardCreditPlusDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Guid", LegacyCardCreditPlusDTO.Guid));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", LegacyCardCreditPlusDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        private LegacyCardCreditPlusDTO GetLegacyCardCreditPlusDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            LegacyCardCreditPlusDTO LegacyCardCreditPlusDTO = new LegacyCardCreditPlusDTO(
                                                dataRow["LegacyCardCreditPlusId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LegacyCardCreditPlusId"]),
                                                dataRow["LegacyCreditPlus"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["LegacyCreditPlus"]),
                                                dataRow["RevisedLegacyCreditPlus"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["RevisedLegacyCreditPlus"]),
                                                CreditPlusTypeConverter.FromString(dataRow["CreditPlusType"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreditPlusType"])),
                                                dataRow["Refundable"] == DBNull.Value ? false : Convert.ToString(dataRow["Refundable"]) == "Y",
                                                dataRow["Remarks"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Remarks"]),
                                                dataRow["LegacyCard_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LegacyCard_id"]),
                                                dataRow["CreditPlusBalance"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["CreditPlusBalance"]),
                                                dataRow["PeriodFrom"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["PeriodFrom"]),
                                                dataRow["PeriodTo"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["PeriodTo"]),
                                                dataRow["TimeFrom"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["TimeFrom"]),
                                                dataRow["TimeTo"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["TimeTo"]),
                                                dataRow["NumberOfDays"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["NumberOfDays"]),
                                                dataRow["Monday"] == DBNull.Value ? true : Convert.ToString(dataRow["Monday"]) == "Y",
                                                dataRow["Tuesday"] == DBNull.Value ? true : Convert.ToString(dataRow["Tuesday"]) == "Y",
                                                dataRow["Wednesday"] == DBNull.Value ? true : Convert.ToString(dataRow["Wednesday"]) == "Y",
                                                dataRow["Thursday"] == DBNull.Value ? true : Convert.ToString(dataRow["Thursday"]) == "Y",
                                                dataRow["Friday"] == DBNull.Value ? true : Convert.ToString(dataRow["Friday"]) == "Y",
                                                dataRow["Saturday"] == DBNull.Value ? true : Convert.ToString(dataRow["Saturday"]) == "Y",
                                                dataRow["Sunday"] == DBNull.Value ? true : Convert.ToString(dataRow["Sunday"]) == "Y",
                                                dataRow["MinimumSaleAmount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["MinimumSaleAmount"]),
                                                dataRow["TicketAllowed"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["TicketAllowed"]),
                                                dataRow["ExpireWithMembership"] == DBNull.Value ? false : Convert.ToString(dataRow["ExpireWithMembership"]) == "Y",
                                                dataRow["PauseAllowed"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["PauseAllowed"]),
                                                dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                dataRow["IsActive"] == DBNull.Value ? true : (Convert.ToBoolean(dataRow["IsActive"])),
                                                dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]),
                                                dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]));
            return LegacyCardCreditPlusDTO;
        }

        /// <summary>
        /// Gets the LegacyCard data of passed LegacyCard ID
        /// </summary>
        /// <param name="LegacyCardCreditPlusId"></param>
        /// <returns>Returns LegacyCardCreditPlusDTO</returns>
        public LegacyCardCreditPlusDTO GetLegacyCardCreditPlusDTO(int legacyCardCreditPlusId)
        {
            log.LogMethodEntry(legacyCardCreditPlusId);
            LegacyCardCreditPlusDTO result = null;
            string query = SELECT_QUERY + @" WHERE lccp.LegacyCardCreditPlusId = @LegacyCardCreditPlusId";
            SqlParameter parameter = new SqlParameter("@LegacyCardCreditPlusId", legacyCardCreditPlusId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetLegacyCardCreditPlusDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        private void RefreshLegacyCardCreditPlusDTO(LegacyCardCreditPlusDTO legacyCardCreditPlusDTO, DataTable dt)
        {
            log.LogMethodEntry(legacyCardCreditPlusDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                legacyCardCreditPlusDTO.LegacyCardCreditPlusId = Convert.ToInt32(dt.Rows[0]["LegacyCardCreditPlusId"]);
                legacyCardCreditPlusDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                legacyCardCreditPlusDTO.LastUpdateDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
                legacyCardCreditPlusDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString();
                legacyCardCreditPlusDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString();
                legacyCardCreditPlusDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString();
                legacyCardCreditPlusDTO.Site_id = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// InsertAccountCreditPlus
        /// </summary>
        /// <param name="accountCreditPlusDTO"></param>
        /// <param name="userId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public LegacyCardCreditPlusDTO InsertLegacyCardCreditPlus(LegacyCardCreditPlusDTO legacyCardCreditPlusDTO, string userId, int siteId)
        {
            log.LogMethodEntry(legacyCardCreditPlusDTO, userId, siteId);
            string query = @"insert into legacyCardCreditPlus 
                                                         (
                                                            LegacyCreditPlus 
                                                           , RevisedLegacyCreditPlus
                                                           , CreditPlusType
                                                           , Refundable 
                                                           , Remarks 
                                                           , LegacyCard_id 
                                                           , CreditPlusBalance 
                                                           , PeriodFrom 
                                                           , PeriodTo 
                                                           , TimeFrom 
                                                           , TimeTo 
                                                           , NumberOfDays 
                                                           , Monday 
                                                           , Tuesday 
                                                           , Wednesday 
                                                           , Thursday 
                                                           , Friday 
                                                           , Saturday 
                                                           , Sunday 
                                                           , MinimumSaleAmount 
                                                           , TicketAllowed
                                                           , ExpireWithMembership
                                                           , PauseAllowed
                                                           , CreatedBy
                                                           , CreationDate 
                                                           , LastUpdatedBy 
                                                           , site_id 
                                                           , LastupdatedDate 
                                                           , TicketAllowed 
                                                           , MasterEntityId 
                                                           , IsActive
                                                         )
                                                       values
                                                         ( 
                                                             @LegacyCreditPlus 
                                                           , @RevisedLegacyCreditPlus
                                                           , @CreditPlusType 
                                                           , @Refundable 
                                                           , @Remarks 
                                                           , @LegacyCard_id 
                                                           , @CreditPlusBalance 
                                                           , @PeriodFrom 
                                                           , @PeriodTo 
                                                           , @TimeFrom 
                                                           , @TimeTo 
                                                           , @NumberOfDays 
                                                           , @Monday 
                                                           , @Tuesday 
                                                           , @Wednesday 
                                                           , @Thursday 
                                                           , @Friday 
                                                           , @Saturday 
                                                           , @Sunday 
                                                           , @MinimumSaleAmount 
                                                           , @TicketAllowed 
                                                           , @CreatedBy
                                                           , getdate() 
                                                           , @LastUpdatedBy 
                                                           , @site_id 
                                                           , getdate() 
                                                           , @MasterEntityId 
                                                           , @IsActive
                                                        )
                                                        SELECT * FROM legacyCardCreditPlus WHERE LegacyCardCreditPlusId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(legacyCardCreditPlusDTO, userId, siteId).ToArray(), sqlTransaction);
                RefreshLegacyCardCreditPlusDTO(legacyCardCreditPlusDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Inserting legacyCardCreditPlusDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(legacyCardCreditPlusDTO);
            return legacyCardCreditPlusDTO;
        }

        /// <summary>
        /// Update the record in the LegacyCardCreditPlus Table. 
        /// </summary>
        /// <param name="LegacyCardCreditPlusDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns>Returns updated LegacyCardCreditPlusDTO</returns>
        public LegacyCardCreditPlusDTO Update(LegacyCardCreditPlusDTO LegacyCardCreditPlusDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(LegacyCardCreditPlusDTO, loginId, siteId);
            string query = @"update   legacyCardCreditPlus set
                                        LegacyCreditPlus  =  @LegacyCreditPlus
                                    , RevisedLegacyCreditPlus = @RevisedLegacyCreditPlus
                                    , CreditPlusType  =  @CreditPlusType 
                                    , Refundable  =  @Refundable 
                                    , Remarks  =  @Remarks 
                                    , LegacyCard_id  =  @LegacyCard_id 
                                    , CreditPlusBalance  =  @CreditPlusBalance  
                                    , PeriodFrom  =  @PeriodFrom 
                                    , PeriodTo  =  @PeriodTo 
                                    , TimeFrom  =  @TimeFrom  
                                    , TimeTo  =  @TimeTo  
                                    , NumberOfDays  =  @NumberOfDays  
                                    , Monday  =  @Monday  
                                    , Tuesday  =  @Tuesday  
                                    , Wednesday  =  @Wednesday  
                                    , Thursday  =  @Thursday  
                                    , Friday  =  @Friday  
                                    , Saturday  =  @Saturday  
                                    , Sunday  =  @Sunday  
                                    , MinimumSaleAmount  =  @MinimumSaleAmount  
                                    , TicketAllowed  =  @TicketAllowed  
                                    , LastupdatedDate  =  GETDATE()  
                                    , LastUpdatedBy  =  @LastUpdatedBy  
                                    , MasterEntityId  =  @MasterEntityId  
                                    , PauseAllowed = @PauseAllowed
                                    ,IsActive = @IsActive
                                where LegacyCardCreditPlusId = @LegacyCardCreditPlusId
                                SELECT * FROM LegacyCardCreditPlus WHERE LegacyCardCreditPlusId = @LegacyCardCreditPlusId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(LegacyCardCreditPlusDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshLegacyCardCreditPlusDTO(LegacyCardCreditPlusDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating LegacyCardCreditPlusDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(LegacyCardCreditPlusDTO);
            return LegacyCardCreditPlusDTO;
        }

        /// <summary>
        /// Returns the List of LegacyCard based on the search parameters.
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<LegacyCardCreditPlusDTO> GetLegacyCardCreditPlusDTOList(List<KeyValuePair<LegacyCardCreditPlusDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            List<LegacyCardCreditPlusDTO> list = null;
            string selectQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<LegacyCardCreditPlusDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == LegacyCardCreditPlusDTO.SearchByParameters.LEGACY_CARD_ID ||
                            searchParameter.Key == LegacyCardCreditPlusDTO.SearchByParameters.LEGACY_CARD_CREDIT_PLUS_ID ||
                            searchParameter.Key == LegacyCardCreditPlusDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == LegacyCardCreditPlusDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == LegacyCardCreditPlusDTO.SearchByParameters.CREDITPLUS_TYPE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == LegacyCardCreditPlusDTO.SearchByParameters.ISACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == LegacyCardCreditPlusDTO.SearchByParameters.CARD_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like  " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                    }
                    else
                    {
                        log.LogMethodExit(null, "throwing exception in GetLegacyCardCreditPlusDTO");
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                    counter++;
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<LegacyCardCreditPlusDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    LegacyCardCreditPlusDTO legacyCardCreditPlusDTO = GetLegacyCardCreditPlusDTO(dataRow);
                    list.Add(legacyCardCreditPlusDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }
    }
}
