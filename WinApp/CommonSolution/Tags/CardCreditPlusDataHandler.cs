
/********************************************************************************************
 * Project Name - CardCore
 * Description  - Bussiness logic of the   CardcreditPlus DataHandler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 *1.00       17-May-2017    Rakshith         Created 
 *2.80.0      19-Mar-2020   Mathew NInan            Added new field ValidityStatus to track
 *                                                  status of entitlements
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Tags
{
    public class CardCreditPlusDataHandler
    {

        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DataAccessHandler dataAccessHandler;

        ////<summary>
        ////For search parameter Specified
        ////</summary>
        private static readonly Dictionary<CardCreditPlusDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<CardCreditPlusDTO.SearchByParameters, string>
        {
            {CardCreditPlusDTO.SearchByParameters.CARD_ID, "card_id"},
            {CardCreditPlusDTO.SearchByParameters.CREDITPLUSTYPE, "creditPlusType"} ,
            {CardCreditPlusDTO.SearchByParameters.EXPIREWITHMEMBERSHIP, "ExpireWithMembership"},
            {CardCreditPlusDTO.SearchByParameters.FORMEMBERSHIPONLY, "ForMembershipOnly"},
            {CardCreditPlusDTO.SearchByParameters.MEMBERSHIPS_ID, "MembershipId"},
            {CardCreditPlusDTO.SearchByParameters.MEMBERSHIP_REWARDS_ID, "MembershipRewardsId" },
            {CardCreditPlusDTO.SearchByParameters.SITE_ID, "site_id"},
            {CardCreditPlusDTO.SearchByParameters.TRANSACTION_ID, "TrxId"},
            {CardCreditPlusDTO.SearchByParameters.CARD_ID_LIST, "card_id"},
            {CardCreditPlusDTO.SearchByParameters.VALIDITYSTATUS, "validityStatus"}
        };


        /// <summary>
        /// Default constructor of CardcreditPlusDataHandler class
        /// </summary>
        public CardCreditPlusDataHandler()
        {
            log.Debug("Starts-CardCreditPlusDataHandler() default constructor.");
            dataAccessHandler = new DataAccessHandler();
            log.Debug("Ends-CardCreditPlusDataHandler() default constructor.");
        }

        /// <summary>
        /// Inserts the CardcreditPlusDTO record to the database
        /// </summary>
        /// <param name="CardcreditPlusDTO">CardcreditPlusDTO type object</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertCardCreditPlusDTO(CardCreditPlusDTO cardcreditPlusDTO, string userId, int siteId, SqlTransaction SQLTrx)
        {
            log.Debug("Starts-InsertCardcreditPlusDTO(cardcreditPlusDTO, string userId, int siteId) Method.");
            string insertCardCreditPlusQuery = @"insert into CardCreditPlus 
                                                         (
                                                            CreditPlus 
                                                           , CreditPlusType 
                                                           , Refundable 
                                                           , Remarks 
                                                           , Card_id 
                                                           , TrxId 
                                                           , LineId 
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
                                                           , LoyaltyRuleId 
                                                           , CreationDate 
                                                           , LastupdatedDate 
                                                           , LastUpdatedBy 
                                                           , Guid 
                                                           , site_id 
                                                         --  , SynchStatus 
                                                           , ExtendOnReload 
                                                           , PlayStartTime 
                                                           , TicketAllowed 
                                                           , MasterEntityId 
                                                           , ForMembershipOnly
                                                           , ExpireWithMembership
                                                           , MembershipRewardsId
                                                           , MembershipId
                                                           , ValidityStatus
                                                         )
                                                       values
                                                         ( 
                                                           @CreditPlus 
                                                           , @CreditPlusType 
                                                           , @Refundable 
                                                           , @Remarks 
                                                           , @Card_id 
                                                           , @TrxId 
                                                           , @LineId 
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
                                                           , @LoyaltyRuleId 
                                                           , getdate() 
                                                           , getdate() 
                                                           , @LastUpdatedBy 
                                                           , newid() 
                                                           , @site_id 
                                                          -- , @SynchStatus 
                                                           , @ExtendOnReload 
                                                           , @PlayStartTime 
                                                           , @TicketAllowed 
                                                           , @MasterEntityId 
                                                           , @ForMembershipOnly 
                                                           , @ExpireWithMembership 
                                                           , @MembershipRewardsId 
                                                           , @MembershipId 
                                                           , @ValidityStatus
                                                        )SELECT CAST(scope_identity() AS int)";

            List<SqlParameter> insertCardcreditPlusDTOParameters = new List<SqlParameter>();
          
            if (cardcreditPlusDTO.CreditPlus == -1)
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@CreditPlus", DBNull.Value));
            }
            else
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@CreditPlus", cardcreditPlusDTO.CreditPlus));
            }
            if (string.IsNullOrEmpty(cardcreditPlusDTO.CreditPlusType))
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@CreditPlusType", DBNull.Value));
            }
            else
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@CreditPlusType", cardcreditPlusDTO.CreditPlusType));
            }
            if (string.IsNullOrEmpty(cardcreditPlusDTO.Refundable))
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@Refundable", DBNull.Value));
            }
            else
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@Refundable", ((cardcreditPlusDTO.Refundable.ToUpper() == "YES" || cardcreditPlusDTO.Refundable.ToUpper() == "Y") ? "Y": "N")));
            }
            if (string.IsNullOrEmpty(cardcreditPlusDTO.Remarks))
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@Remarks", DBNull.Value));
            }
            else
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@Remarks", cardcreditPlusDTO.Remarks));
            }
            if (cardcreditPlusDTO.CardId == -1)
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@Card_id", DBNull.Value));
            }
            else
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@Card_id", cardcreditPlusDTO.CardId));
            }
            
            if (cardcreditPlusDTO.TrxId == -1)
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@TrxId", DBNull.Value));
            }
            else
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@TrxId", cardcreditPlusDTO.TrxId));
            }
            if (cardcreditPlusDTO.LineId == -1)
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@LineId", DBNull.Value));
            }
            else
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@LineId", cardcreditPlusDTO.LineId));
            } 
          
            if (cardcreditPlusDTO.CreditPlusBalance < 0)
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@CreditPlusBalance", DBNull.Value));
            }
            else
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@CreditPlusBalance", cardcreditPlusDTO.CreditPlusBalance));
            }
            if (cardcreditPlusDTO.PeriodFrom == DateTime.MinValue)
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@PeriodFrom", DBNull.Value));
            }
            else
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@PeriodFrom", cardcreditPlusDTO.PeriodFrom));
            }

            if (cardcreditPlusDTO.PeriodTo == DateTime.MinValue)
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@PeriodTo", DBNull.Value));
            }
            else
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@PeriodTo", cardcreditPlusDTO.PeriodTo));
            }
            if (cardcreditPlusDTO.TimeFrom < 0)
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@TimeFrom", DBNull.Value));
            }
            else
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@TimeFrom", cardcreditPlusDTO.TimeFrom));
            }
            if (cardcreditPlusDTO.TimeTo < 0)
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@TimeTo", DBNull.Value));
            }
            else
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@TimeTo", cardcreditPlusDTO.TimeTo));
            }
            if (cardcreditPlusDTO.NumberOfDays == -1)
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@NumberOfDays", DBNull.Value));
            }
            else
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@NumberOfDays", cardcreditPlusDTO.NumberOfDays));
            }
            if (string.IsNullOrEmpty(cardcreditPlusDTO.Monday))
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@Monday", DBNull.Value));
            }
            else
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@Monday", ((cardcreditPlusDTO.Monday.ToUpper() == "YES" || cardcreditPlusDTO.Monday.ToUpper() == "Y") ? "Y" : "N")));
            }
            if (string.IsNullOrEmpty(cardcreditPlusDTO.Tuesday))
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@Tuesday", DBNull.Value));
            }
            else
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@Tuesday", ((cardcreditPlusDTO.Tuesday.ToUpper() == "YES" || cardcreditPlusDTO.Tuesday.ToUpper() == "Y") ? "Y" : "N")));
            }
            if (string.IsNullOrEmpty(cardcreditPlusDTO.Wednesday))
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@Wednesday", DBNull.Value));
            }
            else
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@Wednesday", ((cardcreditPlusDTO.Wednesday.ToUpper() == "YES" || cardcreditPlusDTO.Wednesday.ToUpper() == "Y") ? "Y" : "N")));
            }
            if (string.IsNullOrEmpty(cardcreditPlusDTO.Thursday))
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@Thursday", DBNull.Value));
            }
            else
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@Thursday", ((cardcreditPlusDTO.Thursday.ToUpper() == "YES" || cardcreditPlusDTO.Thursday.ToUpper() == "Y") ? "Y" : "N")));
            }
            if (string.IsNullOrEmpty(cardcreditPlusDTO.Friday))
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@Friday", DBNull.Value));
            }
            else
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@Friday", ((cardcreditPlusDTO.Friday.ToUpper() == "YES" || cardcreditPlusDTO.Friday.ToUpper() == "Y") ? "Y" : "N")));
            }
            if (string.IsNullOrEmpty(cardcreditPlusDTO.Saturday))
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@Saturday", DBNull.Value));
            }
            else
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@Saturday", ((cardcreditPlusDTO.Saturday.ToUpper() == "YES" || cardcreditPlusDTO.Saturday.ToUpper() == "Y") ? "Y" : "N")));
            } 
            
            if (string.IsNullOrEmpty(cardcreditPlusDTO.Sunday))
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@Sunday", DBNull.Value));
            }
            else
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@Sunday", ((cardcreditPlusDTO.Sunday.ToUpper() == "YES" || cardcreditPlusDTO.Sunday.ToUpper() == "Y") ? "Y" : "N")));
            }
            if (cardcreditPlusDTO.MinimumSaleAmount < 0)
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@MinimumSaleAmount", DBNull.Value));
            }
            else
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@MinimumSaleAmount", cardcreditPlusDTO.MinimumSaleAmount));
            }
            if (cardcreditPlusDTO.LoyaltyRuleId == -1)
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@LoyaltyRuleId", DBNull.Value));
            }
            else
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@LoyaltyRuleId", cardcreditPlusDTO.LoyaltyRuleId));
            }
             if (string.IsNullOrEmpty(userId))
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@LastUpdatedBy", DBNull.Value));
            }
            else
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@LastUpdatedBy", userId));
            }
            if (siteId == -1)
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@site_id", DBNull.Value));
            }
            else
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@site_id", siteId));

            } 
           
            if (cardcreditPlusDTO.SynchStatus)
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@SynchStatus", cardcreditPlusDTO.SynchStatus));
            }
            else
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@SynchStatus", DBNull.Value));
            }
            if (string.IsNullOrEmpty(cardcreditPlusDTO.ExtendOnReload))
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@ExtendOnReload", DBNull.Value));
            }
            else
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@ExtendOnReload", ((cardcreditPlusDTO.ExtendOnReload.ToUpper() == "YES" || cardcreditPlusDTO.ExtendOnReload.ToUpper() == "Y") ? "Y" : "N")));
            }
            if (cardcreditPlusDTO.PlayStartTime == DateTime.MinValue)
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@PlayStartTime", DBNull.Value));
            }
            else
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@PlayStartTime", cardcreditPlusDTO.PlayStartTime));
            }

            insertCardcreditPlusDTOParameters.Add(new SqlParameter("@TicketAllowed", cardcreditPlusDTO.TicketAllowed));

            if (cardcreditPlusDTO.MasterEntityId == -1)
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@MasterEntityId", DBNull.Value));
            }
            else
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@MasterEntityId", cardcreditPlusDTO.MasterEntityId));
            }  
            if (!String.IsNullOrEmpty(cardcreditPlusDTO.ExpireWithMembership) && cardcreditPlusDTO.ExpireWithMembership == "Y")
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@ExpireWithMembership", cardcreditPlusDTO.ExpireWithMembership));
            }
            else
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@ExpireWithMembership", DBNull.Value));
            }
            if (cardcreditPlusDTO.ValidityStatus == CardCreditPlusDTO.TagValidityStatus.Valid)
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@ValidityStatus", "Y"));
            }
            else
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@ValidityStatus", "H"));
            }
            if (!String.IsNullOrEmpty(cardcreditPlusDTO.ForMembershipOnly) && cardcreditPlusDTO.ForMembershipOnly == "Y")
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@ForMembershipOnly", cardcreditPlusDTO.ForMembershipOnly));
            }
            else
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@ForMembershipOnly", DBNull.Value));
            }

            if (cardcreditPlusDTO.MembershipId == -1)
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@MembershipId", DBNull.Value));
            }
            else
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@MembershipId", cardcreditPlusDTO.MembershipId));
            }

            if (cardcreditPlusDTO.MembershipRewardsId == -1)
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@MembershipRewardsId", DBNull.Value));
            }
            else
            {
                insertCardcreditPlusDTOParameters.Add(new SqlParameter("@MembershipRewardsId", cardcreditPlusDTO.MembershipRewardsId));
            }


            int idOfRowInserted = dataAccessHandler.executeInsertQuery(insertCardCreditPlusQuery, insertCardcreditPlusDTOParameters.ToArray(), SQLTrx);
            //if(idOfRowInserted > -1)
            //{
            //    string creditPlusLogQry = @"INSERT INTO dbo.CardCreditPlusLog  (CardCreditPlusId, CreditPlus, CreditPlusType, CreditPlusBalance, PlayStartTime, CreatedBy, CreationDate, LastUpdatedBy, LastupdatedDate, site_id, SynchStatus, MasterEntityId )
            //                                (SELECT CardCreditPlusId ,CreditPlus ,CreditPlusType ,CreditPlusBalance ,PlayStartTime  ,@lastUpdatedBy ,GETDATE() ,@lastUpdatedBy  ,GETDATE()   ,site_id ,SynchStatus ,MasterEntityId
            //                                   from CardCreditPlus cp 
            //                                  where cp.CardCreditPlusId = @newCardCreditPlusId)";
            //    List<SqlParameter> cardcreditPlusLogParams = new List<SqlParameter>();
            //    cardcreditPlusLogParams.Add(new SqlParameter("@newCardCreditPlusId", idOfRowInserted));
            //    if(String.IsNullOrEmpty(userId))
            //       cardcreditPlusLogParams.Add(new SqlParameter("@lastUpdatedBy", DBNull.Value));
            //    else
            //         cardcreditPlusLogParams.Add(new SqlParameter("@lastUpdatedBy", userId));
            //    //int logEntry = dataAccessHandler.executeInsertQuery(creditPlusLogQry, cardcreditPlusLogParams.ToArray(), SQLTrx);
            //    dataAccessHandler.executeScalar(creditPlusLogQry, cardcreditPlusLogParams.ToArray(), SQLTrx);
            //    //log.LogVariableState("credit plus log entry ", logEntry);
            //}
            log.Debug("Ends-InsertCardcreditPlusDTO(cardcreditPlusDTO, string userId, int siteId) Method.");
            return idOfRowInserted;
        }



        /// <summary>
        /// Updates the CardcreditPlusDTO record to the database
        /// </summary>
        /// <param name="cardcreditPlusDTO">CardcreditPlusDTO cardcreditPlusDTO</param>
        /// <returns>returns int </returns>
        public int UpdateCardCreditPlusDTO(CardCreditPlusDTO cardcreditPlusDTO, string userId, int siteId, SqlTransaction SQLTrx)
        {
            log.Debug("Starts-UpdateCardcreditPlusDTO(cardcreditPlusDTO, string userId, int siteId) Method.");
            string updateCardcreditPlusDTOQuery = @"update   CardcreditPlus set
                                                             CreditPlus  =  @CreditPlus 
                                                            , CreditPlusType  =  @CreditPlusType 
                                                            , Refundable  =  @Refundable 
                                                            , Remarks  =  @Remarks 
                                                            , Card_id  =  @Card_id 
                                                            , TrxId  =  @TrxId 
                                                            , LineId  =  @LineId 
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
                                                            , LoyaltyRuleId  =  @LoyaltyRuleId  
                                                            , LastupdatedDate  =  GETDATE()  
                                                            , LastUpdatedBy  =  @LastUpdatedBy  
                                                            , site_id  =  @site_id  
                                                            --, SynchStatus  =  @SynchStatus  
                                                            , ExtendOnReload  =  @ExtendOnReload  
                                                            , PlayStartTime  =  @PlayStartTime  
                                                            , TicketAllowed  =  TicketAllowed  
                                                            , MasterEntityId  =  @MasterEntityId  
                                                            , ForMembershipOnly = @ForMembershipOnly
                                                            , ExpireWithMembership = @ExpireWithMembership
                                                            , MembershipRewardsId = @MembershipRewardsId
                                                            , MembershipId = @MembershipId
                                                            , ValidityStatus = @ValidityStatus
                                                        where CardCreditPlusId = @CardCreditPlusId  ";

            List<SqlParameter> updateCardcreditPlusDTOParameters = new List<SqlParameter>();

            updateCardcreditPlusDTOParameters.Add(new SqlParameter("@CardCreditPlusId", cardcreditPlusDTO.CardCreditPlusId));
            if (cardcreditPlusDTO.CreditPlus == -1)
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@CreditPlus", DBNull.Value));
            }
            else
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@CreditPlus", cardcreditPlusDTO.CreditPlus));
            }
            if (string.IsNullOrEmpty(cardcreditPlusDTO.CreditPlusType))
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@CreditPlusType", DBNull.Value));
            }
            else
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@CreditPlusType", cardcreditPlusDTO.CreditPlusType));
            }
            if (string.IsNullOrEmpty(cardcreditPlusDTO.Refundable))
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@Refundable", DBNull.Value));
            }
            else
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@Refundable", ((cardcreditPlusDTO.Refundable.ToUpper() == "YES" || cardcreditPlusDTO.Refundable.ToUpper() == "Y") ? "Y" : "N")));
            }
           
            if (string.IsNullOrEmpty(cardcreditPlusDTO.Remarks))
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@Remarks", DBNull.Value));
            }
            else
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@Remarks", cardcreditPlusDTO.Remarks));
            }
            if (cardcreditPlusDTO.CardId == -1)
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@Card_id", DBNull.Value));
            }
            else
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@Card_id", cardcreditPlusDTO.CardId));
            }
            if (cardcreditPlusDTO.TrxId == -1)
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@TrxId", DBNull.Value));
            }
            else
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@TrxId", cardcreditPlusDTO.TrxId));
            }
            if (cardcreditPlusDTO.LineId == -1)
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@LineId", DBNull.Value));
            }
            else
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@LineId", cardcreditPlusDTO.LineId));
            }
            
            if (cardcreditPlusDTO.CreditPlusBalance < 0)
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@CreditPlusBalance", DBNull.Value));
            }
            else
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@CreditPlusBalance", cardcreditPlusDTO.CreditPlusBalance));
            }
            if (cardcreditPlusDTO.PeriodFrom == DateTime.MinValue)
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@PeriodFrom", DBNull.Value));
            }
            else
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@PeriodFrom", cardcreditPlusDTO.PeriodFrom));
            }

            if (cardcreditPlusDTO.PeriodTo == DateTime.MinValue)
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@PeriodTo", DBNull.Value));
            }
            else
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@PeriodTo", cardcreditPlusDTO.PeriodTo));
            }
             
            if (cardcreditPlusDTO.TimeFrom < 0)
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@TimeFrom", DBNull.Value));
            }
            else
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@TimeFrom", cardcreditPlusDTO.TimeFrom));
            }
            if (cardcreditPlusDTO.TimeTo < 0)
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@TimeTo", DBNull.Value));
            }
            else
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@TimeTo", cardcreditPlusDTO.TimeTo));
            }
            if (cardcreditPlusDTO.NumberOfDays == -1)
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@NumberOfDays", DBNull.Value));
            }
            else
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@NumberOfDays", cardcreditPlusDTO.NumberOfDays));
            }
           
            if (string.IsNullOrEmpty(cardcreditPlusDTO.Monday))
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@Monday", DBNull.Value));
            }
            else
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@Monday", ((cardcreditPlusDTO.Monday.ToUpper() == "YES" || cardcreditPlusDTO.Monday.ToUpper() == "Y") ? "Y" : "N")));
            }
            if (string.IsNullOrEmpty(cardcreditPlusDTO.Tuesday))
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@Tuesday", DBNull.Value));
            }
            else
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@Tuesday", ((cardcreditPlusDTO.Tuesday.ToUpper() == "YES" || cardcreditPlusDTO.Tuesday.ToUpper() == "Y") ? "Y" : "N")));
            }
            if (string.IsNullOrEmpty(cardcreditPlusDTO.Wednesday))
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@Wednesday", DBNull.Value));
            }
            else
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@Wednesday", ((cardcreditPlusDTO.Wednesday.ToUpper() == "YES" || cardcreditPlusDTO.Wednesday.ToUpper() == "Y") ? "Y" : "N")));
            }
            if (string.IsNullOrEmpty(cardcreditPlusDTO.Thursday))
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@Thursday", DBNull.Value));
            }
            else
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@Thursday", ((cardcreditPlusDTO.Thursday.ToUpper() == "YES" || cardcreditPlusDTO.Thursday.ToUpper() == "Y") ? "Y" : "N")));
            }
            if (string.IsNullOrEmpty(cardcreditPlusDTO.Friday))
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@Friday", DBNull.Value));
            }
            else
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@Friday", ((cardcreditPlusDTO.Friday.ToUpper() == "YES" || cardcreditPlusDTO.Friday.ToUpper() == "Y") ? "Y" : "N")));
            }
            
            if (string.IsNullOrEmpty(cardcreditPlusDTO.Saturday))
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@Saturday", DBNull.Value));
            }
            else
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@Saturday", ((cardcreditPlusDTO.Saturday.ToUpper() == "YES" || cardcreditPlusDTO.Saturday.ToUpper() == "Y") ? "Y" : "N")));
            }
            if (string.IsNullOrEmpty(cardcreditPlusDTO.Sunday))
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@Sunday", DBNull.Value));
            }
            else
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@Sunday", ((cardcreditPlusDTO.Sunday.ToUpper() == "YES" || cardcreditPlusDTO.Sunday.ToUpper() == "Y") ? "Y" : "N")));
            }
            if (cardcreditPlusDTO.MinimumSaleAmount < 0)
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@MinimumSaleAmount", DBNull.Value));
            }
            else
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@MinimumSaleAmount", cardcreditPlusDTO.MinimumSaleAmount));
            }
           
            if (cardcreditPlusDTO.LoyaltyRuleId == -1)
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@LoyaltyRuleId", DBNull.Value));
            }
            else
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@LoyaltyRuleId", cardcreditPlusDTO.LoyaltyRuleId));
            }
            if (string.IsNullOrEmpty(userId))
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@LastUpdatedBy", DBNull.Value));
            }
            else
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@LastUpdatedBy", userId));
            }
            if (siteId == -1)
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@site_id", DBNull.Value));
            }
            else
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@site_id", siteId));

            }

            if (cardcreditPlusDTO.SynchStatus)
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@SynchStatus", cardcreditPlusDTO.SynchStatus));
            }
            else
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@SynchStatus", DBNull.Value));
            } 
            if (string.IsNullOrEmpty(cardcreditPlusDTO.ExtendOnReload))
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@ExtendOnReload", DBNull.Value));
            }
            else
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@ExtendOnReload", ((cardcreditPlusDTO.ExtendOnReload.ToUpper() == "YES" || cardcreditPlusDTO.ExtendOnReload.ToUpper() == "Y") ? "Y" : "N")));
            }
            if (cardcreditPlusDTO.PlayStartTime == DateTime.MinValue)
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@PlayStartTime", DBNull.Value));
            }
            else
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@PlayStartTime", cardcreditPlusDTO.PlayStartTime));
            }

            updateCardcreditPlusDTOParameters.Add(new SqlParameter("@TicketAllowed", cardcreditPlusDTO.TicketAllowed));

            if (cardcreditPlusDTO.MasterEntityId == -1)
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@MasterEntityId", DBNull.Value));
            }
            else
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@MasterEntityId", cardcreditPlusDTO.MasterEntityId));
            }
            
            if (!String.IsNullOrEmpty(cardcreditPlusDTO.ExpireWithMembership) && cardcreditPlusDTO.ExpireWithMembership == "Y")
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@ExpireWithMembership", cardcreditPlusDTO.ExpireWithMembership));
            }
            else
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@ExpireWithMembership", DBNull.Value));
            }

            if (cardcreditPlusDTO.ValidityStatus == CardCreditPlusDTO.TagValidityStatus.Valid)
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@ValidityStatus", "Y"));
            }
            else
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@ValidityStatus", "H"));
            }
            if (!String.IsNullOrEmpty(cardcreditPlusDTO.ForMembershipOnly) && cardcreditPlusDTO.ForMembershipOnly == "Y")
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@ForMembershipOnly", cardcreditPlusDTO.ForMembershipOnly));
            }
            else
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@ForMembershipOnly", DBNull.Value));
            }
            if (cardcreditPlusDTO.MembershipId == -1)
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@MembershipId", DBNull.Value));
            }
            else
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@MembershipId", cardcreditPlusDTO.MembershipId));
            }
            if (cardcreditPlusDTO.MembershipRewardsId == -1)
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@MembershipRewardsId", DBNull.Value));
            }
            else
            {
                updateCardcreditPlusDTOParameters.Add(new SqlParameter("@MembershipRewardsId", cardcreditPlusDTO.MembershipRewardsId));
            }
            int idOfRowUpdated = dataAccessHandler.executeUpdateQuery(updateCardcreditPlusDTOQuery, updateCardcreditPlusDTOParameters.ToArray(), SQLTrx);
            //if(idOfRowUpdated > -1)
            //{
            //    string creditPlusLogQry = @"INSERT INTO dbo.CardCreditPlusLog  (CardCreditPlusId, CreditPlus, CreditPlusType, CreditPlusBalance, PlayStartTime, CreatedBy, CreationDate, LastUpdatedBy, LastupdatedDate, site_id, SynchStatus, MasterEntityId )
            //                                (SELECT CardCreditPlusId ,CreditPlus ,CreditPlusType ,CreditPlusBalance ,PlayStartTime  ,@lastUpdatedBy ,GETDATE() ,@lastUpdatedBy  ,GETDATE()   ,site_id ,SynchStatus ,MasterEntityId
            //                                   from CardCreditPlus cp 
            //                                  where cp.CardCreditPlusId = @newCardCreditPlusId)";
            //    List<SqlParameter> cardcreditPlusLogParams = new List<SqlParameter>();
            //    cardcreditPlusLogParams.Add(new SqlParameter("@newCardCreditPlusId", idOfRowUpdated));
            //    cardcreditPlusLogParams.Add(new SqlParameter("@lastUpdatedBy", userId));
            //    //int logEntry = dataAccessHandler.executeInsertQuery(creditPlusLogQry, cardcreditPlusLogParams.ToArray(), SQLTrx);
            //    //log.LogVariableState("credit plus log entry ", logEntry);
            //    dataAccessHandler.executeScalar(creditPlusLogQry, cardcreditPlusLogParams.ToArray(), SQLTrx);
            //}
            log.Debug("Ends-UpdateCardcreditPlusDTO(cardcreditPlusDTO, string userId, int siteId) Method.");
            return idOfRowUpdated;
        }

        /// <summary>
        /// return GetCardCreditPlusDTO from datarow
        /// </summary>
        /// <param name="cardCreditPlusDataRow"></param>
        /// <returns></returns>
        private CardCreditPlusDTO GetCardCreditPlusDTO(DataRow cardCreditPlusDataRow)
        {
            log.Debug("Starts-GetCardcreditPlusDTO(cardCreditPlusDataRow) Method.");
            CardCreditPlusDTO cardcreditPlusDTO = new CardCreditPlusDTO
            (
                string.IsNullOrEmpty(cardCreditPlusDataRow["CardCreditPlusId"].ToString()) ? -1 : Convert.ToInt32(cardCreditPlusDataRow["CardCreditPlusId"]),
                string.IsNullOrEmpty(cardCreditPlusDataRow["CreditPlus"].ToString()) ? 0 : Convert.ToDouble(cardCreditPlusDataRow["CreditPlus"]),
                cardCreditPlusDataRow["CreditPlusType"].ToString(),
                cardCreditPlusDataRow["Refundable"].ToString(),
                cardCreditPlusDataRow["Remarks"].ToString(),
                string.IsNullOrEmpty(cardCreditPlusDataRow["Card_id"].ToString()) ? -1 : Convert.ToInt32(cardCreditPlusDataRow["Card_id"]),
                string.IsNullOrEmpty(cardCreditPlusDataRow["TrxId"].ToString()) ? -1 : Convert.ToInt32(cardCreditPlusDataRow["TrxId"]),
                string.IsNullOrEmpty(cardCreditPlusDataRow["LineId"].ToString()) ? -1 : Convert.ToInt32(cardCreditPlusDataRow["LineId"]),
                string.IsNullOrEmpty(cardCreditPlusDataRow["CreditPlusBalance"].ToString()) ? -1 : Convert.ToInt32(cardCreditPlusDataRow["CreditPlusBalance"]),
                cardCreditPlusDataRow["PeriodFrom"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(cardCreditPlusDataRow["PeriodFrom"]),
                cardCreditPlusDataRow["PeriodTo"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(cardCreditPlusDataRow["PeriodTo"]),
                string.IsNullOrEmpty(cardCreditPlusDataRow["TimeFrom"].ToString()) ? 0 : Convert.ToDouble(cardCreditPlusDataRow["TimeFrom"]),
                string.IsNullOrEmpty(cardCreditPlusDataRow["TimeTo"].ToString()) ? 0 : Convert.ToDouble(cardCreditPlusDataRow["TimeTo"]),
                string.IsNullOrEmpty(cardCreditPlusDataRow["NumberOfDays"].ToString()) ? -1 : Convert.ToInt32(cardCreditPlusDataRow["NumberOfDays"]),
                string.IsNullOrEmpty(cardCreditPlusDataRow["Monday"].ToString()) ? "" : cardCreditPlusDataRow["Monday"].ToString(),
                string.IsNullOrEmpty(cardCreditPlusDataRow["Tuesday"].ToString()) ? "" : cardCreditPlusDataRow["Tuesday"].ToString(),
                string.IsNullOrEmpty(cardCreditPlusDataRow["Wednesday"].ToString()) ? "" : cardCreditPlusDataRow["Wednesday"].ToString(),
                string.IsNullOrEmpty(cardCreditPlusDataRow["Thursday"].ToString()) ? "" : cardCreditPlusDataRow["Thursday"].ToString(),
                string.IsNullOrEmpty(cardCreditPlusDataRow["Friday"].ToString()) ? "" : cardCreditPlusDataRow["Friday"].ToString(),
                string.IsNullOrEmpty(cardCreditPlusDataRow["Saturday"].ToString()) ? "" : cardCreditPlusDataRow["Saturday"].ToString(),
                string.IsNullOrEmpty(cardCreditPlusDataRow["Sunday"].ToString()) ? "" : cardCreditPlusDataRow["Sunday"].ToString(),
                string.IsNullOrEmpty(cardCreditPlusDataRow["MinimumSaleAmount"].ToString()) ? 0 : Convert.ToDouble(cardCreditPlusDataRow["MinimumSaleAmount"]),
                string.IsNullOrEmpty(cardCreditPlusDataRow["LoyaltyRuleId"].ToString()) ? -1 : Convert.ToInt32(cardCreditPlusDataRow["LoyaltyRuleId"]),
                cardCreditPlusDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(cardCreditPlusDataRow["CreationDate"]),
                cardCreditPlusDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(cardCreditPlusDataRow["LastupdatedDate"]),
                cardCreditPlusDataRow["LastUpdatedBy"].ToString(),
                cardCreditPlusDataRow["Guid"].ToString(),
                string.IsNullOrEmpty(cardCreditPlusDataRow["site_id"].ToString()) ? -1 : Convert.ToInt32(cardCreditPlusDataRow["site_id"]),
                cardCreditPlusDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(cardCreditPlusDataRow["SynchStatus"]),
                cardCreditPlusDataRow["ExtendOnReload"].ToString(),
                cardCreditPlusDataRow["PlayStartTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(cardCreditPlusDataRow["PlayStartTime"]),
                cardCreditPlusDataRow["TicketAllowed"] == DBNull.Value ? false : Convert.ToBoolean(cardCreditPlusDataRow["TicketAllowed"]),
                cardCreditPlusDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(cardCreditPlusDataRow["MasterEntityId"]),
                cardCreditPlusDataRow["ForMembershipOnly"] == DBNull.Value ? "N" : cardCreditPlusDataRow["ForMembershipOnly"].ToString(),
                cardCreditPlusDataRow["ExpireWithMembership"] == DBNull.Value ? "N" : cardCreditPlusDataRow["ExpireWithMembership"].ToString(),
                cardCreditPlusDataRow["MembershipRewardsId"] == DBNull.Value ? -1 : Convert.ToInt32(cardCreditPlusDataRow["MembershipRewardsId"]),
                cardCreditPlusDataRow["MembershipId"] == DBNull.Value ? -1 : Convert.ToInt32(cardCreditPlusDataRow["MembershipId"]),
                cardCreditPlusDataRow["ValidityStatus"] == DBNull.Value ? CardCreditPlusDTO.TagValidityStatus.Valid : (cardCreditPlusDataRow["ValidityStatus"].ToString() == "Y" ? CardCreditPlusDTO.TagValidityStatus.Valid : CardCreditPlusDTO.TagValidityStatus.Hold)
               );

            log.Debug("Ends-GetCardcreditPlusDTO(cardCreditPlusDataRow) Method.");
            return cardcreditPlusDTO;
        }


        /// <summary>
        /// returns the GetCreditPlus of the CardNumber passed as parameter
        /// </summary>
        /// <param name="CardNumber">CardNumber</param>
        public double GetCreditPlusBalance(int cardId)
        {
            log.Debug("Starts-GetCreditPlusBalance(int cardId) Method.");
            double cardCreditPlus = 0;
            try
            {
                double CreditPlusCardBalance = 0;
                double CreditPlusCredits = 0;

                string getCreditPlusQuery = @"select CreditPlusCardBalance, CreditPlusCredits, CreditPlusBonus, creditPlusItemPurchase , CreditPlusTickets
                                                        from CardView
                                                        where card_id = @card_id";
                List<SqlParameter> getCreditPlusQueryParameters = new List<SqlParameter>();
                getCreditPlusQueryParameters.Add(new SqlParameter("@card_id", cardId));

                DataTable dt = dataAccessHandler.executeSelectQuery(getCreditPlusQuery, getCreditPlusQueryParameters.ToArray());
                if (dt.Rows.Count > 0)
                {
                    CreditPlusCardBalance = Convert.ToDouble(dt.Rows[0]["CreditPlusCardBalance"]);
                    CreditPlusCredits = Convert.ToDouble(dt.Rows[0]["CreditPlusCredits"]);
                }
                cardCreditPlus = (CreditPlusCardBalance + CreditPlusCredits);

            }
            catch
            {
                throw;
            }
            log.Debug("Ends-GetCreditPlusBalance(string cardNumber) Method.");
            return cardCreditPlus;
        }



        /// <summary>
        /// returns the GetCreditPlus of the CardNumber passed as parameter
        /// </summary>
        /// <param name="CardNumber">CardNumber</param>
        public CardCreditPlusBalanceDTO GetCreditPlusBalances(int cardId)
        {
            log.Debug("Starts-GetCreditPlusBalance(int cardId) Method.");
            CardCreditPlusBalanceDTO CardCreditPlusBalanceDTO  = null;
            try
            {

                string getCreditPlusQuery = @"select  CreditPlusTickets, 
                                                      CreditPlusCardBalance, CreditPlusCredits, CreditPlusBonus, 
                                                      CreditPlusRefundableBalance, CreditPlusItemPurchase ,
                                                      CreditPlusLoyaltyPoints
                                                from CardView
                                                where card_id = @card_id";
                List<SqlParameter> getCreditPlusQueryParameters = new List<SqlParameter>();
                getCreditPlusQueryParameters.Add(new SqlParameter("@card_id", cardId));

                DataTable dt = dataAccessHandler.executeSelectQuery(getCreditPlusQuery, getCreditPlusQueryParameters.ToArray());
                if (dt.Rows.Count > 0)
                {
                    CardCreditPlusBalanceDTO = new CardCreditPlusBalanceDTO();
                    CardCreditPlusBalanceDTO.CreditPlusTickets = Convert.ToInt32(dt.Rows[0]["CreditPlusTickets"]);
                    CardCreditPlusBalanceDTO.CreditPlusBalance = Convert.ToDouble(dt.Rows[0]["CreditPlusCardBalance"]) + Convert.ToDouble(dt.Rows[0]["CreditPlusCredits"]);
                    CardCreditPlusBalanceDTO.CreditPlusLoyaltyPoints = Convert.ToInt32(dt.Rows[0]["CreditPlusLoyaltyPoints"]);
                    CardCreditPlusBalanceDTO.CreditPlusRefundableBalance = Convert.ToDouble(dt.Rows[0]["CreditPlusRefundableBalance"]);
                }

            }
            catch
            {
                throw;
            }
            log.Debug("Ends-GetCreditPlusBalance(string cardNumber) Method.");
            return CardCreditPlusBalanceDTO;
        }

        /// <summary>
        /// GetCardCreditPlusDTOList 
        /// </summary>
        /// <param name="dtInCardCreditPlus"></param>
        /// <returns></returns>
        private List<CardCreditPlusDTO> GetCardCreditPlusDTOList(DataTable dtInCardCreditPlus)
        {
            log.Debug("Start-GetCardCreditPlusDTOList(dtInCardCreditPlus) Method.");

            List<CardCreditPlusDTO> cardCreditPlusDTOList = new List<CardCreditPlusDTO>();
            if (dtInCardCreditPlus.Rows.Count > 0)
            {
                foreach (DataRow cardCreditPlusRow in dtInCardCreditPlus.Rows)
                {
                    CardCreditPlusDTO cardCreditPlusDTO = GetCardCreditPlusDTO(cardCreditPlusRow);
                    cardCreditPlusDTOList.Add(cardCreditPlusDTO);
                }
            }
            log.Debug("Ends-GetCardCreditPlusDTOList(dtInCardCreditPlus) Method.");

            return cardCreditPlusDTOList;

        }

        /// <summary>
        /// Gets the CardCreditPlusDTO matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of CardCreditPlus matching the search criteria</returns>
        public List<CardCreditPlusDTO> GetCardCreditPlusList(List<KeyValuePair<CardCreditPlusDTO.SearchByParameters, string>> searchParameters)
        {
            log.Debug("Starts-GetCardCreditPlusList(searchParameters) Method.");
            try
            {
                string selectCardQuery = @"SELECT * 
                                                FROM CardCreditPlus ccp
                                                WHERE CreditPlusType = @creditPlusType 
                                                AND isnull(CreditPlusBalance, 0) > 0
                                                AND Card_id = @card_id
                                                AND isnull(ValidityStatus, 'Y') != 'H'
                                                AND (PeriodFrom is null or PeriodFrom <= GETDATE()) 
                                                AND (PeriodTo is null or PeriodTo >= GETDATE())
                                                 and (isnull(case DATEPART(WEEKDAY, getdate()) 
                                                                    when 1 then Sunday 
                                                                    when 2 then Monday 
                                                                    when 3 then Tuesday 
                                                                    when 4 then Wednesday 
                                                                    when 5 then Thursday 
                                                                    when 6 then Friday 
                                                                    when 7 then Saturday 
                                                                    else 'Y' end, 'Y') = 'Y'
                                                        OR EXISTS (select 1 from EntityOverrideDates ed WHERE ed.EntityGuid = ccp.Guid 
													                                                            AND ed.EntityName = 'CARDCREDITPLUS'
													                                                            AND ((getdate() between ed.OverrideDate and dateadd(dd, 1, ed.OverrideDate))
															                                                        OR ed.Day = DATEPART(WEEKDAY, GETDATE()))
													                                                            AND  ed.IncludeExcludeFlag = 1)
                                                           )
                                                        AND NOT EXISTS (select 1
                                                            from EntityOverrideDates ed WHERE ed.EntityGuid = ccp.Guid 
                                                            AND ed.EntityName = 'CARDCREDITPLUS'
                                                            AND isnull(IncludeExcludeFlag, 0) = 0
                                                            AND ((getdate() between ed.OverrideDate and dateadd(dd, 1, ed.OverrideDate))
                                                            or ed.Day = DATEPART(WEEKDAY, GETDATE()))
                                                            )
                                                ORDER BY ISNULL(PeriodTo, 999999)";

                List<SqlParameter> parameters = new List<SqlParameter>();

                foreach (KeyValuePair<CardCreditPlusDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        parameters.Add(new SqlParameter("@" + searchParameter.Key, searchParameter.Value));
                    }
                }

                DataTable dtCardCreditPlus = dataAccessHandler.executeSelectQuery(selectCardQuery, parameters.ToArray());
                log.Debug("Ends-GetCardCreditPlusList(searchParameters) Method.");
                return GetCardCreditPlusDTOList(dtCardCreditPlus);

            }
            catch (Exception ex)
            {
                log.Log("Error-GetCardCreditPlusList(searchParameters) ", ex);
                throw;
            }
        }


        /// <summary>
        /// Gets the CardCoreDTO matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of CardCreditPlus matching the search criteria</returns>
        public List<CardCreditPlusDTO> GetCardCreditPlusLoyalty(CardParams cardParams)
        {
            log.Debug("Starts-GetCardCreditPlusLoyalty(searchParameters) Method.");
            try
            {
                string selectCardQuery = @"SELECT [CardCreditPlusId]
		                                        ,[CreditPlus] 
                                                ,isnull(l.Attribute, 'Other') CreditPlusType
                                                ,case [Refundable] when 'N' then 'No' else 'Yes' end Refundable
		                                        ,[Remarks] 
		                                        ,Card_id, TrxId, LineId
                                                ,[CreditPlusBalance] 
		                                        ,[PeriodFrom] 
                                                ,[PeriodTo] 
		                                        ,[TimeFrom] 
                                                ,[TimeTo] 
		                                        ,[NumberOfDays] 
                                                ,case isnull([Monday], 'Y') when 'Y' then 'Yes' else 'No' end Monday
                                                ,case isnull([Tuesday], 'Y') when 'Y' then 'Yes' else 'No' end Tuesday
                                                ,case isnull([Wednesday], 'Y') when 'Y' then 'Yes' else 'No' end Wednesday
                                                ,case isnull([Thursday], 'Y') when 'Y' then 'Yes' else 'No' end Thursday
                                                ,case isnull([Friday], 'Y') when 'Y' then 'Yes' else 'No' end Friday
                                                ,case isnull([Saturday], 'Y') when 'Y' then 'Yes' else 'No' end Saturday
                                                ,case isnull([Sunday], 'Y') when 'Y' then 'Yes' else 'No' end Sunday
		                                        ,[MinimumSaleAmount] 
		                                        ,LoyaltyRuleId
		                                        ,cp.CreationDate
		                                        ,cp.LastupdatedDate
		                                        ,cp.LastUpdatedBy
		                                        ,cp.Guid
		                                        ,cp.site_id
		                                        ,cp.SynchStatus
                                                ,case [ExtendOnReload] when 'N' then 'No' else 'Yes' end ExtendOnReload
                                                ,[PlayStartTime] 
                                                ,TicketAllowed 
		                                        ,cp.MasterEntityId
                                                , cp.ForMembershipOnly
                                                , cp.ExpireWithMembership
                                                , cp.MembershipRewardsId
                                                , cp.MembershipId
                                                , cp.PauseAllowed
                                                , cp.ValidityStatus
                                                FROM [CardCreditPlus] cp left outer join LoyaltyAttributes l on (cp.CreditPlusType = l.CreditPlusType and l.site_id = cp.site_id)
                                            where card_id = @CardId
                                              AND ISNULL(cp.ValidityStatus, 'Y') != 'H'
                                            and (@all  = 'Y' or ((CreditPlusBalance != 0
                                                                or exists (select 1
                                                                            from cardCreditPlusConsumption cpc
                                                                            where cpc.CardCreditPlusId = cp.CardCreditPlusId
                                                                            and cpc.ConsumptionBalance > 0))
                                                                and (PeriodTo is null or PeriodTo + 1 >= getdate())))
                                            order by CreationDate desc";

                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(new SqlParameter("@CardId", cardParams.CardId));
                parameters.Add(new SqlParameter("@all", "Y"));

                DataTable dtCardCreditPlusLoyalty = dataAccessHandler.executeSelectQuery(selectCardQuery, parameters.ToArray());

                log.Debug("Ends-GetCardCreditPlusLoyalty(searchParameters) Method.");

                return GetCardCreditPlusDTOList(dtCardCreditPlusLoyalty);

            }
            catch (Exception ex)
            {
                log.Log("Error-GetCardCreditPlusLoyalty(searchParameters) ", ex);
                throw;
            }
        }

        /// <summary>
        /// Gets the CardCoreDTO matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of CardCreditPlus matching the search criteria</returns>
        public List<CardCreditPlusDTO> GetAllCardCreditPlus(List<KeyValuePair<CardCreditPlusDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(searchParameters);
            try
            {
                string selectCardQuery = @"SELECT [CardCreditPlusId]
		                                        ,[CreditPlus] 
                                                ,isnull(CreditPlusType, 'Other') CreditPlusType
                                                ,case [Refundable] when 'N' then 'No' else 'Yes' end Refundable
		                                        ,[Remarks] 
		                                        ,Card_id, TrxId, LineId
                                                ,[CreditPlusBalance] 
		                                        ,[PeriodFrom] 
                                                ,[PeriodTo] 
		                                        ,[TimeFrom] 
                                                ,[TimeTo] 
		                                        ,[NumberOfDays] 
                                                ,case isnull([Monday], 'Y') when 'Y' then 'Yes' else 'No' end Monday
                                                ,case isnull([Tuesday], 'Y') when 'Y' then 'Yes' else 'No' end Tuesday
                                                ,case isnull([Wednesday], 'Y') when 'Y' then 'Yes' else 'No' end Wednesday
                                                ,case isnull([Thursday], 'Y') when 'Y' then 'Yes' else 'No' end Thursday
                                                ,case isnull([Friday], 'Y') when 'Y' then 'Yes' else 'No' end Friday
                                                ,case isnull([Saturday], 'Y') when 'Y' then 'Yes' else 'No' end Saturday
                                                ,case isnull([Sunday], 'Y') when 'Y' then 'Yes' else 'No' end Sunday
		                                        ,[MinimumSaleAmount] 
		                                        ,LoyaltyRuleId
		                                        ,[CreationDate] 
		                                        ,cp.LastupdatedDate
		                                        ,cp.LastUpdatedBy
		                                        ,cp.Guid
		                                        ,cp.site_id
		                                        ,cp.SynchStatus
                                                ,case [ExtendOnReload] when 'N' then 'No' else 'Yes' end ExtendOnReload
                                                ,[PlayStartTime] 
                                                ,TicketAllowed 
		                                        ,cp.MasterEntityId
                                                ,cp.ForMembershipOnly
                                                ,cp.ExpireWithMembership
                                                ,cp.MembershipRewardsId
                                                ,cp.MembershipId
                                                ,cp.ValidityStatus
                                                FROM [CardCreditPlus] cp 
                                          ";
                StringBuilder query = new StringBuilder(" ");
                string joiner = "";
                int count = 0;
                if ((searchParameters != null) && (searchParameters.Count > 0))
                {
                    query.Append(" WHERE ");
                    foreach (KeyValuePair<CardCreditPlusDTO.SearchByParameters, string> searchParameter in searchParameters)
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (DBSearchParameters.ContainsKey(searchParameter.Key))
                        {
                            if (searchParameter.Key == CardCreditPlusDTO.SearchByParameters.CARD_ID ||
                                searchParameter.Key == CardCreditPlusDTO.SearchByParameters.MEMBERSHIPS_ID ||
                                searchParameter.Key == CardCreditPlusDTO.SearchByParameters.MEMBERSHIP_REWARDS_ID ||
                                searchParameter.Key == CardCreditPlusDTO.SearchByParameters.TRANSACTION_ID)
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value);
                            }
                            else if (searchParameter.Key == CardCreditPlusDTO.SearchByParameters.SITE_ID)
                            {
                                query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                            }
                            else if (searchParameter.Key == CardCreditPlusDTO.SearchByParameters.CREDITPLUSTYPE)
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value);
                            }
                            else if (searchParameter.Key == CardCreditPlusDTO.SearchByParameters.EXPIREWITHMEMBERSHIP || searchParameter.Key == CardCreditPlusDTO.SearchByParameters.FORMEMBERSHIPONLY)
                            {
                                query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'N' )= '" + searchParameter.Value +"' ");
                            }
                            else if (searchParameter.Key == CardCreditPlusDTO.SearchByParameters.VALIDITYSTATUS)
                            {
                                query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y' )= '" + searchParameter.Value + "' ");
                            }
                            else if(searchParameter.Key == CardCreditPlusDTO.SearchByParameters.CARD_ID_LIST)
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + " in (" + searchParameter.Value +") ");
                            }
                            else
                            {
                                query.Append(joiner + " Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "'%" + searchParameter.Value + "%'");
                            }
                            count++;
                        }
                        else
                        {
                            log.LogMethodExit(null, "throwing exception in GetAllCardCreditPlus");
                            log.LogVariableState("searchParameter.Key", searchParameter.Key);
                            throw new Exception("The query parameter does not exist " + searchParameter.Key);
                        }
                    }
                }

                selectCardQuery = selectCardQuery + query.ToString();
                DataTable dtAllCardCreditPlus = dataAccessHandler.executeSelectQuery(selectCardQuery,null, sqlTrx);

                log.LogMethodExit();

                return GetCardCreditPlusDTOList(dtAllCardCreditPlus);

            }
            catch (Exception ex)
            {
                log.Log("Error-GetCardCreditPlusLoyalty(searchParameters) ", ex);
                throw;
            }
        }
        /// <summary>
        /// Gets
        /// </summary>
        /// <param name="customerId">customerId</param>
        /// <param name="creditPlusType">creditPlusType</param>
        /// <param name="fromDate">fromDate</param>
        /// <param name="toDate">toDate</param>
        /// <param name="siteId">siteId</param>
        /// <param name="sqlTrx">sqlTrx</param>
        /// <returns> returns AvgBalance for the parameters</returns>
        public Dictionary<DateTime, double> GetCPAttributeCPBalance(int customerId, string creditPlusType, DateTime fromDate, DateTime toDate, int siteId, SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry();
            Dictionary<DateTime, double> balanceSheet = new Dictionary<DateTime, double>();
            string selectQuery = @"SELECT c.customer_id,  CPBalanceDate, sum(CPbalance) CPDailyBalance
                                     from (select cp.Card_id, cp.CardCreditPlusId,  cp.CreditPlusType, CAST(cpl.CreationDate AS DATE) CPBalanceDate,
                                                  cpl.CreditPlusBalance  CPbalance,
                                                  RANK() OVER (PARTITION BY cp.Card_id, cp.CardCreditPlusId,  cp.CreditPlusType,CAST(cpl.CreationDate AS DATE) ORDER BY cpl.CreationDate desc) AS RankValue 
                                             from cardCreditPLus cp ,
                                                  CardCreditPlusLog cpl  
                                            where cpl.CardCreditPlusId = cp.CardCreditPlusId
                                              and (cp.site_id = @siteId or @siteId = -1)
                                              and cpl.CreationDate >= @fromDate and  cpl.CreationDate < @toDate 
                                              and cp.CreditPlusType=@creditPlusType
                                              and ISNULL(cp.MembershipRewardsId,-1) = -1  
                                          ) a, cards c
                                     where a.RankValue =1 
                                       and a.Card_id = c.card_id
                                       and c.customer_id = @customerId
                                  group by c.customer_id, CPBalanceDate
                                  order by  CPBalanceDate ";

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@customerId", customerId));
            parameters.Add(new SqlParameter("@creditPlusType", creditPlusType));
            parameters.Add(new SqlParameter("@fromDate", fromDate));
            parameters.Add(new SqlParameter("@toDate", toDate));
            parameters.Add(new SqlParameter("@siteId", siteId));

            DataTable dtCPAttribute = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTrx);
            if(dtCPAttribute != null && dtCPAttribute.Rows.Count > 0)
            {
                foreach (DataRow cpAttributeRow in dtCPAttribute.Rows)
                {
                    balanceSheet.Add(Convert.ToDateTime(cpAttributeRow["CPBalanceDate"]), Convert.ToDouble(cpAttributeRow["CPDailyBalance"]));
                }
                
            }

            log.LogMethodExit(balanceSheet);
            return balanceSheet;

        }

    }
}
    
