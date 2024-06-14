/********************************************************************************************
 * Project Name - CardGames
 * Description  - Logic to get and deduct card games via Game Play and other applications
 * 
 **************
 **Version Log
 **************
 *Version     Date              Modified By        Remarks          
 *********************************************************************************************
 *2.80.0      19-Mar-2020   Mathew NInan           Use new field ValidityStatus to track
 *                                                 status of entitlements
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace Semnox.Core.GenericUtilities
{
    public class CardGames
    {
       private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Semnox.Core.Utilities.Utilities Utilities;
        public CardGames(Semnox.Core.Utilities.Utilities ParafaitUtilities)
        {
            log.LogMethodEntry(ParafaitUtilities);
            Utilities = ParafaitUtilities;
            log.LogMethodExit(null);
        }

        private DataTable getCardGameList(int CardId, int GameId, int GameProfileId, SqlTransaction trx = null)
        {
            log.LogMethodEntry(CardId, GameId, GameProfileId);

            DataTable returnValue = Utilities.executeDataTable(
                      @"select * from
	                     (select top 100000 card_game_id, balance, TicketAllowed, Frequency, FromDate
                          from (select card_game_id, game_id, game_profile_id, ExpiryDate, Frequency, FromDate,
                                case Frequency 
                                    when 'N' then BalanceGames 
                                    when 'D' then case when DATEPART(YEAR, LastPlayedTime)*1000 + DATEPART(DAYOFYEAR, LastPlayedTime) = DATEPART(YEAR, GETDATE())*1000 + DATEPART(DAYOFYEAR, GETDATE()) then BalanceGames else quantity end 
                                    when 'W' then case when DATEPART(YEAR, LastPlayedTime)*1000 + DATEPART(WEEK, LastPlayedTime) = DATEPART(YEAR, GETDATE())*1000 + DATEPART(WEEK, GETDATE()) then BalanceGames else quantity end 
                                    when 'M' then case when DATEPART(YEAR, LastPlayedTime)*1000 + DATEPART(MONTH, LastPlayedTime) = DATEPART(YEAR, GETDATE())*1000 + DATEPART(MONTH, GETDATE()) then BalanceGames else quantity end 
                                    when 'Y' then case when DATEPART(YEAR, LastPlayedTime) = DATEPART(YEAR, GETDATE()) then BalanceGames else quantity end 
                                    when 'B' then case when FromDate is null then 0
                                                        else
                                                        case when DATEPART(MONTH, FromDate)*100 + DATEPART(DAY, FromDate) = DATEPART(MONTH, GETDATE())*100 + DATEPART(DAY, GETDATE()) 
                                                             then case when DATEPART(YEAR, LastPlayedTime)*10000 + DATEPART(MONTH, LastPlayedTime)*100 + DATEPART(DAY, LastPlayedTime) = DATEPART(YEAR, GETDATE())*10000 + DATEPART(MONTH, GETDATE())*100 + DATEPART(DAY, GETDATE()) then BalanceGames else quantity end
                                                             else 0 
                                                        end
                                                    end
                                    when 'A' then case when FromDate is null then 0
                                                        else
                                                        case when DATEPART(MONTH, FromDate)*100 + DATEPART(DAY, FromDate) = DATEPART(MONTH, GETDATE())*100 + DATEPART(DAY, GETDATE()) 
                                                             then case when DATEPART(YEAR, LastPlayedTime)*10000 + DATEPART(MONTH, LastPlayedTime)*100 + DATEPART(DAY, LastPlayedTime) = DATEPART(YEAR, GETDATE())*10000 + DATEPART(MONTH, GETDATE())*100 + DATEPART(DAY, GETDATE()) then BalanceGames else quantity end
                                                             else 0 
                                                        end
                                                    end
                                    else 0 end balance, isnull(TicketAllowed, 1) TicketAllowed
                                    from CardGames 
                                   WHERE card_id = @card_id
								    AND isnull(validityStatus, 'Y') != 'H' 
                                    and (FromDate is null or getdate() > FromDate)
                                    and (game_id = @game_id 
                                        or game_id in (select g2.game_id 
                                                        from games g1, games g2, lookupView l
                                                        where l.LookupName = 'QUEUE_COMPATIBILITY'
                                                        and g1.game_id = @game_id 
                                                        and g1.game_name = l.LookupValue
                                                        and g2.game_name = l.Description)
                                        or game_profile_id = @GameProfileId 
                                        or (game_id is null and game_profile_id is null)
                                        or (@game_id = -1 and @GameProfileId = -1)) 
                                   AND (isnull(case DATEPART(WEEKDAY, getdate()) 
                                                        when 1 then Sunday 
                                                        when 2 then Monday 
                                                        when 3 then Tuesday 
                                                        when 4 then Wednesday 
                                                        when 5 then Thursday 
                                                        when 6 then Friday 
                                                        when 7 then Saturday 
                                                        else 'Y' end, 'Y') = 'Y'
                                          OR EXISTS (select 1 from EntityOverrideDates ed WHERE ed.EntityGuid = CardGames.Guid 
													   AND ed.EntityName = 'CARDGAMES'
													   AND ((getdate() between ed.OverrideDate and dateadd(dd, 1, ed.OverrideDate))
															OR ed.Day = DATEPART(WEEKDAY, GETDATE()))
													   AND  ed.IncludeExcludeFlag = 1)
                                    )
                                    AND NOT EXISTS (select 1
											from EntityOverrideDates ed WHERE ed.EntityGuid = CardGames.Guid 
											AND ed.EntityName = 'CARDGAMES'
											AND isnull(IncludeExcludeFlag, 0) = 0
											AND ((getdate() between ed.OverrideDate and dateadd(dd, 1, ed.OverrideDate))
												or ed.Day = DATEPART(WEEKDAY, GETDATE()))
					                          )
                                    and not exists (select 1
                                                        from cardGames cg1
                                                        where cg1.card_id = @card_id
                                                        and (cg1.game_id = @game_id or cg1.game_profile_id = @GameProfileId) 
                                                        and (cg1.ExpiryDate is null or cg1.ExpiryDate > getdate())
                                                        and (cg1.FromDate is null or getdate() > cg1.FromDate)
                                                        and isnull(cg1.TrxId, -1) = isnull(CardGames.TrxId, -1)
                                                        and isnull(cg1.TrxLineId, -1) = isnull(CardGames.TrxLineId, -1)
                                                        and BalanceGames < 0) 
                                    and 
                                        (not exists (select 1
                                                        from cardGameExtended cge
                                                        where cge.cardGameId = CardGames.card_game_id)
                                        or 
                                            (exists (select 1
                                                        from cardGameExtended cge
                                                        where cge.cardGameId = CardGames.card_game_id
                                                        and Exclude = 1)
												and not exists (select 1
                                                        from cardGameExtended cge
                                                        where cge.cardGameId = CardGames.card_game_id
                                                        and (cge.gameId = @game_id or cge.gameProfileId = @GameProfileId) 
                                                        and Exclude = 1)
											)
                                         or 
                                             (exists (select 1
                                                        from cardGameExtended cge
                                                        where cge.cardGameId = CardGames.card_game_id
                                                        and Exclude = 0)
                                                and exists (select 1
                                                        from cardGameExtended cge
                                                        where cge.cardGameId = CardGames.card_game_id
                                                        and (cge.gameId = @game_id or cge.gameProfileId = @GameProfileId or (cge.gameId is null and cge.gameProfileId is null)) 
                                                        and Exclude = 0)
                                             )
                                        )
                                    and (ExpiryDate is null or ExpiryDate > getdate()) ) base
                                where balance > 0                               
                                order by game_id desc, game_profile_id desc, case when ExpiryDate is null then 1 else 0 end, ExpiryDate, card_game_id) inView
							where not exists (select 1
													from cardGameExtended cge
													where cge.cardGameId = inView.card_game_id
													and Exclude = 0
													and (cge.gameId = @game_id or cge.gameProfileId = @GameProfileId or (cge.gameId is null and cge.gameProfileId is null))
													and isnull(cge.PlayLimitPerGame, 0) > 0)
									or exists (select 1
											from cardGameExtended cge
											where cge.cardGameId = inView.card_game_id
											and (cge.gameId = @game_id or cge.gameProfileId = @GameProfileId or (cge.gameId is null and cge.gameProfileId is null)) 
											and Exclude = 0
											and isnull(cge.PlayLimitPerGame, 0) 
												> (select count(1) 
													from gameplay g, machines m 
													where m.machine_id = g.machine_id 
													and g.card_id = @card_id 
													and m.game_id = @game_id
													and g.CardGameId = inView.card_game_id
													and case inView.Frequency
                                    when 'N' then 1
                                    when 'D' then case when DATEPART(YEAR, g.play_date)*1000 + DATEPART(DAYOFYEAR, g.play_date) = DATEPART(YEAR, GETDATE())*1000 + DATEPART(DAYOFYEAR, GETDATE()) then 1 else 0 end 
                                    when 'W' then case when DATEPART(YEAR, g.play_date)*1000 + DATEPART(WEEK, g.play_date) = DATEPART(YEAR, GETDATE())*1000 + DATEPART(WEEK, GETDATE()) then 1 else 0 end 
                                    when 'M' then case when DATEPART(YEAR, g.play_date)*1000 + DATEPART(MONTH, g.play_date) = DATEPART(YEAR, GETDATE())*1000 + DATEPART(MONTH, GETDATE()) then 1 else 0 end 
                                    when 'Y' then case when DATEPART(YEAR, g.play_date) = DATEPART(YEAR, GETDATE()) then 1 else 0 end 
                                    when 'B' then case when FromDate is null then 0
                                                        else
                                                        case when DATEPART(MONTH, FromDate)*100 + DATEPART(DAY, FromDate) = DATEPART(MONTH, GETDATE())*100 + DATEPART(DAY, GETDATE()) 
                                                             then case when DATEPART(YEAR, g.play_date)*10000 + DATEPART(MONTH, g.play_date)*100 + DATEPART(DAY, g.play_date) = DATEPART(YEAR, GETDATE())*10000 + DATEPART(MONTH, GETDATE())*100 + DATEPART(DAY, GETDATE()) then 1 else 0 end
                                                             else 0 
                                                        end
                                                    end
                                    when 'A' then case when FromDate is null then 0
                                                        else
                                                        case when DATEPART(MONTH, FromDate)*100 + DATEPART(DAY, FromDate) = DATEPART(MONTH, GETDATE())*100 + DATEPART(DAY, GETDATE()) 
                                                             then case when DATEPART(YEAR, g.play_date)*10000 + DATEPART(MONTH, g.play_date)*100 + DATEPART(DAY, g.play_date) = DATEPART(YEAR, GETDATE())*10000 + DATEPART(MONTH, GETDATE())*100 + DATEPART(DAY, GETDATE()) then 1 else 0 end
                                                             else 0 
                                                        end
                                                    end
                                    else 0 end = 1))", trx,
                                         new SqlParameter("@card_id", CardId),
                                         new SqlParameter("@game_id", GameId),
                                         new SqlParameter("@GameProfileId", GameProfileId));
            log.LogVariableState("@card_id", CardId);
            log.LogVariableState("@game_id", GameId);
            log.LogVariableState("@GameProfileId", GameProfileId);
            log.LogMethodExit(returnValue);

            return returnValue;
        }

        public int getCardGames(int CardId, int GameId, int GameProfileId, ref bool TicketAllowed, ref int CardGameId, SqlTransaction trx = null)
        {
            log.LogMethodEntry(CardId, GameId, GameProfileId, TicketAllowed, CardGameId);
            DataTable dt = getCardGameList(CardId, GameId, GameProfileId, trx);
            int TotalGames = 0;
            foreach (DataRow dr in dt.Rows)
            {
                TotalGames += Convert.ToInt32(dr["balance"]);
            }

            if (dt.Rows.Count > 0)
            {
                CardGameId = Convert.ToInt32(dt.Rows[0]["card_game_id"]);
                TicketAllowed = Convert.ToBoolean(dt.Rows[0]["TicketAllowed"]);
            }
            log.LogVariableState("Ticket allowed", TicketAllowed);
            log.LogVariableState("CardGameId", CardGameId);
            log.LogMethodExit(TotalGames);
            return TotalGames;
        }

        public int getCardGames(int CardId, int GameId, int GameProfileId, ref bool TicketAllowed, ref List<int> CardGameIdList, SqlTransaction trx = null)
        {
            log.LogMethodEntry(CardId, GameId, GameProfileId, TicketAllowed, CardGameIdList);
            DataTable dt = getCardGameList(CardId, GameId, GameProfileId, trx);
            int TotalGames = 0;
            foreach (DataRow dr in dt.Rows)
            {
                int balance = Convert.ToInt32(dr["balance"]);
                TotalGames += balance;
                while (balance-- > 0)
                    CardGameIdList.Add(Convert.ToInt32(dr["card_game_id"]));
            }

            if (dt.Rows.Count > 0)
            {
                TicketAllowed = Convert.ToBoolean(dt.Rows[0]["TicketAllowed"]);
            }
            log.LogVariableState("Ticket allowed", TicketAllowed);
            log.LogVariableState("CardGameIdList", CardGameIdList);
            log.LogMethodExit(TotalGames);
            return TotalGames;
        }

        public int getCardGames(int CardId, int GameId, int GameProfileId, ref string EntitlementType, ref int BalanceTime)
        {
            log.LogMethodEntry(CardId, GameId, GameProfileId, EntitlementType, BalanceTime);
            DataTable dt = Utilities.executeDataTable(@"select top 1 EntitlementType, isnull((case Frequency 
                                when 'N' then BalanceGames 
                                when 'D' then case when DATEPART(YEAR, LastPlayedTime)*1000 + DATEPART(DAYOFYEAR, LastPlayedTime) = DATEPART(YEAR, GETDATE())*1000 + DATEPART(DAYOFYEAR, GETDATE()) then BalanceGames else quantity end 
                                when 'W' then case when DATEPART(YEAR, LastPlayedTime)*1000 + DATEPART(WEEK, LastPlayedTime) = DATEPART(YEAR, GETDATE())*1000 + DATEPART(WEEK, GETDATE()) then BalanceGames else quantity end 
                                when 'M' then case when DATEPART(YEAR, LastPlayedTime)*1000 + DATEPART(MONTH, LastPlayedTime) = DATEPART(YEAR, GETDATE())*1000 + DATEPART(MONTH, GETDATE()) then BalanceGames else quantity end 
                                when 'Y' then case when DATEPART(YEAR, LastPlayedTime) = DATEPART(YEAR, GETDATE()) then BalanceGames else quantity end 
                                else 0 end), 0) Balance, isnull((case when isnumeric(OptionalAttribute) = 1 then convert(int, OptionalAttribute) else 0 end), 0) BalanceTime 
                                from CardGames 
                                where card_id = @card_id and BalanceGames > 0
								AND isnull(validityStatus, 'Y') != 'H' 
                                and (FromDate is null or getdate() > FromDate)
                                and (game_id = @game_id 
                                    or game_id in (select g2.game_id 
                                                    from games g1, games g2, lookupView l
                                                    where l.LookupName = 'QUEUE_COMPATIBILITY'
                                                    and g1.game_id = @game_id 
                                                    and g1.game_name = l.LookupValue
                                                    and g2.game_name = l.Description)
                                    or game_profile_id = @GameProfileId 
                                    or (game_id is null and game_profile_id is null))
                                 AND (isnull(case DATEPART(WEEKDAY, getdate()) 
                                                        when 1 then Sunday 
                                                        when 2 then Monday 
                                                        when 3 then Tuesday 
                                                        when 4 then Wednesday 
                                                        when 5 then Thursday 
                                                        when 6 then Friday 
                                                        when 7 then Saturday 
                                                        else 'Y' end, 'Y') = 'Y'
                                          OR EXISTS (select 1 from EntityOverrideDates ed WHERE ed.EntityGuid = CardGames.Guid 
													   AND ed.EntityName = 'CARDGAMES'
													   AND ((getdate() between ed.OverrideDate and dateadd(dd, 1, ed.OverrideDate))
															OR ed.Day = DATEPART(WEEKDAY, GETDATE()))
													   AND  ed.IncludeExcludeFlag = 1)
                                    )
                                    AND NOT EXISTS (select 1
											from EntityOverrideDates ed WHERE ed.EntityGuid = CardGames.Guid 
											AND ed.EntityName = 'CARDGAMES'
											AND isnull(IncludeExcludeFlag, 0) = 0
											AND ((getdate() between ed.OverrideDate and dateadd(dd, 1, ed.OverrideDate))
												or ed.Day = DATEPART(WEEKDAY, GETDATE()))
					                          )
                                and not exists (select 1
                                                    from cardGames cg1
                                                    where cg1.card_id = @card_id
                                                    and (game_id = @game_id or game_profile_id = @GameProfileId) 
                                                    and (ExpiryDate is null or ExpiryDate > getdate())
                                                    and (FromDate is null or getdate() > FromDate)
                                                    and isnull(cg1.TrxId, -1) = isnull(CardGames.TrxId, -1)
                                                    and isnull(cg1.TrxLineId, -1) = isnull(CardGames.TrxLineId, -1)
                                                    and BalanceGames < 0) 
                                and (ExpiryDate is null or ExpiryDate > getdate()) 
                                order by isnull(ExpiryDate, getdate() + 999)",
                            new SqlParameter("@card_id", CardId),
                            new SqlParameter("@game_id", GameId),
                            new SqlParameter("@GameProfileId", GameProfileId));
            log.LogVariableState("@card_id", CardId);
            log.LogVariableState("@game_id", GameId);
            log.LogVariableState("@GameProfileId", GameProfileId);

            if (dt.Rows.Count > 0)
            {
                EntitlementType = dt.Rows[0]["EntitlementType"].ToString();
                BalanceTime = Convert.ToInt32(dt.Rows[0]["BalanceTime"]);
                log.LogVariableState("EntitlementType", EntitlementType);
                log.LogVariableState("BalanceTime", BalanceTime);
                int returnvalue1 = (Convert.ToInt32(dt.Rows[0]["Balance"]));
                log.LogMethodExit(returnvalue1);
                return (returnvalue1);
            }
            else
            {
                log.LogVariableState("EntitlementType", EntitlementType);
                log.LogVariableState("BalanceTime", BalanceTime);
                log.LogMethodExit(0);
                return 0;
            }
        }

        public int deductGameCount(string CardNumber, int MachineId, SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(CardNumber, MachineId, SQLTrx);
            SqlCommand cmd = Utilities.getCommand(SQLTrx);

            DataTable dt = Utilities.executeDataTable(@"select (select card_id from cards where card_number = @card_number and valid_flag ='Y'),
                                                        g.game_id, g.game_profile_id 
                                                        from games g, machines m 
                                                        where m.machine_id = @machine_id 
                                                        and m.game_id = g.game_id",
                                                        SQLTrx,
                                                        new SqlParameter("@machine_id", MachineId));
            log.LogVariableState("@machine_id", MachineId);
            DataTable dtCG = getCardGameList((int)dt.Rows[0][0], (int)dt.Rows[0][1], (int)dt.Rows[0][2]);

            if (dtCG.Rows.Count == 0)
            {
                log.LogMethodExit(-1);
                return -1;
            }
              
            else
            {
                int returnvalue= (deductGameCount((int)dtCG.Rows[0]["card_game_id"], SQLTrx));
                log.LogMethodExit(returnvalue);
                return (returnvalue);
            }
                
        }

        public int deductGameCount(int CardGameId, SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(CardGameId, SQLTrx);
            Utilities.executeNonQuery(@"update CardGames set BalanceGames = case when EntitlementType is null then 
                                                                 (case Frequency 
                                                                       when 'N' then BalanceGames 
                                                                       when 'D' then case when DATEPART(YEAR, LastPlayedTime)*1000 + DATEPART(DAYOFYEAR, LastPlayedTime) = DATEPART(YEAR, GETDATE())*1000 + DATEPART(DAYOFYEAR, GETDATE()) then BalanceGames else quantity end 
                                                                       when 'W' then case when DATEPART(YEAR, LastPlayedTime)*1000 + DATEPART(WEEK, LastPlayedTime) = DATEPART(YEAR, GETDATE())*1000 + DATEPART(WEEK, GETDATE()) then BalanceGames else quantity end 
                                                                       when 'M' then case when DATEPART(YEAR, LastPlayedTime)*1000 + DATEPART(MONTH, LastPlayedTime) = DATEPART(YEAR, GETDATE())*1000 + DATEPART(MONTH, GETDATE()) then BalanceGames else quantity end 
                                                                       when 'Y' then case when DATEPART(YEAR, LastPlayedTime) = DATEPART(YEAR, GETDATE()) then BalanceGames else quantity end 
                                                                       when 'A' then case when DATEPART(YEAR, LastPlayedTime)*10000 + DATEPART(MONTH, LastPlayedTime)*100 + DATEPART(DAY, LastPlayedTime) = DATEPART(YEAR, GETDATE())*10000 + DATEPART(MONTH, GETDATE())*100 + DATEPART(DAY, GETDATE()) then BalanceGames else quantity end
                                                                       when 'B' then case when DATEPART(YEAR, LastPlayedTime)*10000 + DATEPART(MONTH, LastPlayedTime)*100 + DATEPART(DAY, LastPlayedTime) = DATEPART(YEAR, GETDATE())*10000 + DATEPART(MONTH, GETDATE())*100 + DATEPART(DAY, GETDATE()) then BalanceGames else quantity end
                                                                      else 0 end) - 1 
                                                                  else 0 end, 
                                                         LastPlayedTime = getdate(), last_update_date = getdate()
                                        where card_game_id = @cgId",
                                        SQLTrx,
                                        new SqlParameter("@cgId", CardGameId));
            log.LogVariableState("@cgId", CardGameId);
            log.LogMethodExit(CardGameId);

            return CardGameId;
        }
    }
}
