/********************************************************************************************
 * Project Name - Customer
 * Description  - BL of CardType data handler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.80        27-May-2020   Girish Kundar               Created 
 *2.80        12-Jun-2020   Mushahid Faizan             Modified: BuildSQLParameters() and GetCardTypeDTO().
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer.Accounts
{
    public class CardTypeDataHandler
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM CardType AS cp ";
        private static readonly Dictionary<CardTypeDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<CardTypeDTO.SearchByParameters, string>
            {
                {CardTypeDTO.SearchByParameters.CARDTYPE_ID, "cp.user_id"},
                {CardTypeDTO.SearchByParameters.CARDTYPE, "cp.username"},
                {CardTypeDTO.SearchByParameters.DISCOUNT_ID, "cp.loginid"},
                {CardTypeDTO.SearchByParameters.SITE_ID, "cp.site_id"},
                {CardTypeDTO.SearchByParameters.MASTER_ENTITY_ID, "cp.MasterEntityId"}
               
            };


        /// <summary>
        /// Default constructor of CardTypeDataHandler class
        /// </summary>
        public CardTypeDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }
        private List<SqlParameter> BuildSQLParameters(CardTypeDTO cardTypeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(cardTypeDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@CardTypeId", cardTypeDTO.CardTypeId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CardType", cardTypeDTO.CardType));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Description", cardTypeDTO.Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MinimumUsageTrigger", cardTypeDTO.MinimumUsageTrigger));
            parameters.Add(dataAccessHandler.GetSQLParameter("@AutomaticApply", cardTypeDTO.AutomaticApply));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Discount_Id", cardTypeDTO.DiscountId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Redemption_Discount", cardTypeDTO.RedemptionDiscount, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SynchStatus", cardTypeDTO.SynchStatus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MinimumRechargeTrigger", cardTypeDTO.MinimumRechargeTrigger));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PriceListId", cardTypeDTO.PriceListId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TriggerDurationInDays", cardTypeDTO.TriggerDurationInDays));
            parameters.Add(dataAccessHandler.GetSQLParameter("@VIPStatusTrigger", cardTypeDTO.VipStatusTrigger));
            parameters.Add(dataAccessHandler.GetSQLParameter("@BaseCardTypeId", cardTypeDTO.BaseCardTypeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LoyaltyPointsTrigger", cardTypeDTO.LoyaltyPointsTrigger));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", cardTypeDTO.MasterEntityId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@NewMembershipId", cardTypeDTO.MembershipId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@cardTypeMigrated", cardTypeDTO.CardTypeMigrated));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MigrationMessage", cardTypeDTO.MigrationMessage));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ExistingTriggerSource", cardTypeDTO.ExistingTriggerSource));
            parameters.Add(dataAccessHandler.GetSQLParameter("@QualifyingDuration", cardTypeDTO.QualifyingDuration));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LoyaltyPointConvRatio", cardTypeDTO.LoyaltyPointConvRatio));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RedeemLoyaltyPoints", cardTypeDTO.RedeemLoyaltyPoints));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MigrationOrder", cardTypeDTO.MigrationOrder));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        private CardTypeDTO GetCardTypeDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            CardTypeDTO cardTypeDTO = new CardTypeDTO(
                 Convert.ToInt32(dataRow["CardTypeId"]),
                 dataRow["CardType"].ToString(),
                 dataRow["Description"].ToString(),
                 dataRow["MinimumUsageTrigger"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["MinimumUsageTrigger"]),
                 dataRow["AutomaticApply"] == DBNull.Value ? 'N' : Convert.ToChar(dataRow["AutomaticApply"].ToString()),
                 dataRow["Discount_Id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Discount_Id"]),
                 dataRow["Redemption_Discount"] == DBNull.Value ? (decimal?) null: Convert.ToDecimal(dataRow["Redemption_Discount"]),
                 dataRow["MinimumRechargeTrigger"] == DBNull.Value ? (int?) null : Convert.ToInt32(dataRow["MinimumRechargeTrigger"]),
                 dataRow["PriceListId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PriceListId"]),
                 dataRow["TriggerDurationInDays"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["TriggerDurationInDays"]),
                 dataRow["VIPStatusTrigger"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["VIPStatusTrigger"]),
                 dataRow["BaseCardTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["BaseCardTypeId"]),
                 dataRow["LoyaltyPointsTrigger"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["LoyaltyPointsTrigger"]),
                 dataRow["NewMembershipId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["NewMembershipId"]),
                 dataRow["cardTypeMigrated"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["cardTypeMigrated"].ToString()),
                 dataRow["MigrationMessage"] == DBNull.Value ? string.Empty : dataRow["MigrationMessage"].ToString(),
                 dataRow["ExistingTriggerSource"] == DBNull.Value ? -1 :Convert.ToInt32(dataRow["ExistingTriggerSource"].ToString()),
                 dataRow["QualifyingDuration"] == DBNull.Value ? -1 :Convert.ToInt32(dataRow["QualifyingDuration"].ToString()),
                 dataRow["LoyaltyPointConvRatio"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["LoyaltyPointConvRatio"].ToString()),
                 dataRow["RedeemLoyaltyPoints"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["RedeemLoyaltyPoints"]),
                 dataRow["MigrationOrder"] == DBNull.Value ? (int?) (null) : Convert.ToInt32(dataRow["MigrationOrder"]),
                 dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                 dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString(),
                 dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                 dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                 dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString(),
                 dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                 dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString(),
                 dataRow["LastupdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdateDate"])
               );
            log.LogMethodExit(cardTypeDTO);
            return cardTypeDTO;
        }
        /// <summary>
        /// Inserts the CardTypeDTO record to the database
        /// </summary>
        /// <param name="CardTypeDTO">CardTypeDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns CardTypeDTO</returns>
        public CardTypeDTO Insert(CardTypeDTO cardTypeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(cardTypeDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[CardType]
                                                       ([CardType]
                                                       ,[Description]
                                                       ,[MinimumUsageTrigger]
                                                       ,[AutomaticApply]
                                                       ,[Discount_Id]
                                                       ,[Redemption_Discount]
                                                       ,[Guid]
                                                       ,[site_id]
                                                       ,[SynchStatus]
                                                       ,[MinimumRechargeTrigger]
                                                       ,[PriceListId]
                                                       ,[TriggerDurationInDays]
                                                       ,[VIPStatusTrigger]
                                                       ,[BaseCardTypeId]
                                                       ,[LoyaltyPointsTrigger]
                                                       ,[MasterEntityId]
                                                       ,[NewMembershipId]
                                                       ,[cardTypeMigrated]
                                                       ,[MigrationMessage]
                                                       ,[ExistingTriggerSource]
                                                       ,[QualifyingDuration]
                                                       ,[LoyaltyPointConvRatio]
                                                       ,[RedeemLoyaltyPoints]
                                                       ,[MigrationOrder]
                                                       ,[CreatedBy]
                                                       ,[CreationDate]
                                                       ,[LastUpdatedBy]
                                                       ,[LastUpdateDate]
                                                        )
                                                 VALUES                                          
                                                        (
                                                        @CardType,
                                                        @Description,
                                                        @MinimumUsageTrigger,
                                                        @AutomaticApply,
                                                        @Discount_Id,                                         
                                                        @Redemption_Discount,
                                                        newId(),
                                                        @site_id,
                                                        @SynchStatus,
                                                        @MinimumRechargeTrigger,
                                                        @PriceListId,
                                                        @TriggerDurationInDays,
                                                        @VIPStatusTrigger,
                                                        @BaseCardTypeId,
                                                        @LoyaltyPointsTrigger,
                                                        @MasterEntityId,
                                                        @NewMembershipId,                                                  
                                                        @cardTypeMigrated,
                                                        @MigrationMessage,
                                                        @ExistingTriggerSource,
                                                        @QualifyingDuration,
                                                        @LoyaltyPointConvRatio,
                                                        @RedeemLoyaltyPoints,
                                                        @MigrationOrder,
                                                        @CreatedBy,
                                                        GetDate(),
                                                        @LastUpdatedBy,
                                                        GetDate()
                                                       )
                                                   SELECT  * from CardType where CardTypeId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, BuildSQLParameters(cardTypeDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCardTypeDTODTO(cardTypeDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting cardTypeDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(cardTypeDTO);
            return cardTypeDTO;
        }


        public CardTypeDTO Update(CardTypeDTO cardTypeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(cardTypeDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[CardType]
                                                      SET
                                                        [CardType]               = @CardType,
                                                        [Description]            = @Description,
                                                        [MinimumUsageTrigger]    = @MinimumUsageTrigger,
                                                        [AutomaticApply]         = @AutomaticApply,
                                                        [Discount_Id]            = @Discount_Id,            
                                                        [Redemption_Discount]    = @Redemption_Discount,
                                                        [MinimumRechargeTrigger] = @MinimumRechargeTrigger,
                                                        [PriceListId]            = @PriceListId,
                                                        [TriggerDurationInDays]  = @TriggerDurationInDays,
                                                        [VIPStatusTrigger]       = @VIPStatusTrigger,
                                                        [BaseCardTypeId]         = @BaseCardTypeId,
                                                        [LoyaltyPointsTrigger]   = @LoyaltyPointsTrigger,
                                                        [MasterEntityId]         = @MasterEntityId,
                                                        [NewMembershipId]        = @NewMembershipId,        
                                                        [cardTypeMigrated]       = @cardTypeMigrated,
                                                        [MigrationMessage]       = @MigrationMessage,
                                                        [ExistingTriggerSource]  = @ExistingTriggerSource,
                                                        [QualifyingDuration]     = @QualifyingDuration,
                                                        [LoyaltyPointConvRatio]  = @LoyaltyPointConvRatio,
                                                        [RedeemLoyaltyPoints]    = @RedeemLoyaltyPoints,
                                                        [MigrationOrder]         = @MigrationOrder,
                                                        [LastUpdatedBy]          = @LastUpdatedBy,
                                                        [LastUpdateDate]         = GetDate()
                                                          WHERE    CardTypeId = @CardTypeId             
                                                      SELECT  * from CardType where CardTypeId = @CardTypeId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, BuildSQLParameters(cardTypeDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshCardTypeDTODTO(cardTypeDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating cardTypeDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(cardTypeDTO);
            return cardTypeDTO;
        }

        private void RefreshCardTypeDTODTO(CardTypeDTO cardTypeDTO, DataTable dt)
        {
            log.LogMethodEntry(cardTypeDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                cardTypeDTO.CardTypeId = Convert.ToInt32(dt.Rows[0]["CardTypeId"]);
                cardTypeDTO.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                cardTypeDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                cardTypeDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                cardTypeDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                cardTypeDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }
        internal CardTypeDTO GetCardType(int id)
        {
            log.LogMethodEntry(id);
            CardTypeDTO cardTypeDTO = null;
            string query = SELECT_QUERY + "   where cp.CardTypeId = @cardTypeId";
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@cardTypeId", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, parameters, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                DataRow dataRow = dataTable.Rows[0];
                cardTypeDTO = GetCardTypeDTO(dataRow);
            }
            log.LogMethodExit(cardTypeDTO);
            return cardTypeDTO;

        }

        public List<CardTypeDTO> GetCardTypeDTOList(List<KeyValuePair<CardTypeDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string cardTypequery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<CardTypeDTO> CardTypeDTOList = new List<CardTypeDTO>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<CardTypeDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        string joinOperartor = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key.Equals(CardTypeDTO.SearchByParameters.CARDTYPE_ID)
                            || searchParameter.Key.Equals(CardTypeDTO.SearchByParameters.DISCOUNT_ID)
                            || searchParameter.Key.Equals(CardTypeDTO.SearchByParameters.MASTER_ENTITY_ID))
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + " =  " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(CardTypeDTO.SearchByParameters.SITE_ID))
                        {
                            query.Append(joinOperartor + "(" + DBSearchParameters[searchParameter.Key] + " =  " + dataAccessHandler.GetParameterName(searchParameter.Key) + " OR -1=" + dataAccessHandler.GetParameterName(searchParameter.Key) + ")");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                       
                        else if (searchParameter.Key == CardTypeDTO.SearchByParameters.CARDTYPE)
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + " =  " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), (searchParameter.Value)));
                        }
                        else
                        {
                            query.Append(joinOperartor + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",' ') like  " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        count++;
                    }
                    else
                    {
                        log.LogMethodExit(null, "throwing exception");
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }

                if (searchParameters.Count > 0)
                    cardTypequery = cardTypequery + query;
            }


            DataTable dataTable = dataAccessHandler.executeSelectQuery(cardTypequery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    CardTypeDTO cardTypeDTO = GetCardTypeDTO(dataRow);
                    CardTypeDTOList.Add(cardTypeDTO);
                }
            }
            log.LogMethodExit(CardTypeDTOList);
            return CardTypeDTOList;
        }

        internal DataTable GetMaxQualifyingDuration(int siteId =-1)
        {
            log.LogMethodEntry();
            int i = 0;
            string query = @"SELECT Bdate, Max(Qdate) MQdate 
                                           from (
                                                  select  CASE unitofQualificationWindow 
                                                             WHEN 'D' THEN DATEADD(day,QualificationWindow, Cast(getdate() as date))  
                                                             WHEN 'M' THEN DATEADD(Month,QualificationWindow, Cast(getdate() as date))  
									                         WHEN 'Y' THEN DATEADD(Year,QualificationWindow, Cast(getdate() as date))  
                                                              END QDate, Cast(getdate() as date) bDate
                                                    from MembershipRule mr, Membership m
                                                   where m.MembershipRuleID = mr.MembershipRuleID
                                                     and m.IsActive =1
                                                     and mr.IsActive =1
                                                     and (m.site_id = @siteId or @siteId = -1)
                                                ) a
                                         group by bdate";
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@siteId", siteId);
            DataTable dtMaxDuration = dataAccessHandler.executeSelectQuery(query, parameters, sqlTransaction);
            log.LogMethodExit(dtMaxDuration);
            return dtMaxDuration;
        }

    }
}
