using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.ServerCore
{
    public class MachinePromotion
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        int gameId;
        int gameProfileId;
        char type;
        int promotionDetailId;

        public MachinePromotion(int gameId, int gameProfileId, char type, int promotionDetailId)
        {
            log.LogMethodEntry(gameId, gameProfileId, type, promotionDetailId);
            this.gameId = gameId;
            this.gameProfileId = gameProfileId;
            this.type = type;
            this.promotionDetailId = promotionDetailId;
            log.LogMethodExit();
        }
        public int GameId
        {
            get { return gameId; }
            set { gameId = value; }
        }

        public int GameProfileId
        {
            get { return gameProfileId; }
            set { gameProfileId = value; }
        }
        public char Type
        {
            get { return type; }
            set { type = value; }
        }
        public int PromotionDetailId
        {
            get { return promotionDetailId; }
            set { promotionDetailId = value; }
        }
    }

    public static class Promotions
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static void getPromotionDetails(int inMembershipId, int game_id, int game_profile_id, 
                                                decimal credits, decimal vip_credits, ref decimal promotion_credits, 
                                                ref decimal promotion_vip_credits, ref string bonus_allowed, ref string courtesy_allowed, 
                                                ref string time_allowed, ref string ticket_allowed, ref int ThemeNumber,
                                                ref int promoVisualizationThemeNumber,
                                                ref int PromotionId,
                                                ref int PromotionDetailId,
                                                Utilities Utilities)
        {
            log.LogMethodEntry(inMembershipId, game_id, game_profile_id, credits, vip_credits, promotion_credits,
              promotion_vip_credits, bonus_allowed, courtesy_allowed, time_allowed, ticket_allowed, ThemeNumber, 
              promoVisualizationThemeNumber, PromotionId, PromotionDetailId, Utilities);
            DataTable dt = Utilities.executeDataTable(
                            @"select v.*, rt.ThemeNumber, rtv.ThemeNumber VisualizationThemeNumber, case when @membershipId < 0 then r.MembershipId else case when r.MembershipId = @membershipId then -2 else isnull(r.MembershipId, -1) end end sort1, 1 sort2 
                                from PromotionView v
                                left outer join PromotionRule r
                                    on r.promotion_id = v.promotion_id
                                left outer join Theme rt
                                    on rt.Id = v.ThemeId
                                left outer join Theme rtv
                                    on rtv.Id = v.VisualizationThemeId
                              where v.game_id = @game_id
                              union all
                              select v.*, rt.ThemeNumber, rtv.ThemeNumber VisualizationThemeNumber, case when @membershipId < 0 then r.MembershipId else case when r.MembershipId = @membershipId then -2 else isnull(r.MembershipId, -1) end end, 2 
                                from PromotionView v
                                left outer join PromotionRule r
                                    on r.promotion_id = v.promotion_id
                                left outer join Theme rt
                                    on rt.Id = v.ThemeId
                                left outer join Theme rtv
                                    on rtv.Id = v.VisualizationThemeId
                              where v.game_profile_id = @game_profile_id
                              union all
                              select v.*, rt.ThemeNumber, rtv.ThemeNumber VisualizationThemeNumber, case when @membershipId < 0 then r.MembershipId else case when r.MembershipId = @membershipId then -2 else isnull(r.MembershipId, -1) end end, 3 
                                from PromotionView v
                                left outer join PromotionRule r
                                    on r.promotion_id = v.promotion_id
                                left outer join Theme rt
                                    on rt.Id = v.ThemeId
                                left outer join Theme rtv
                                    on rtv.Id = v.VisualizationThemeId
                              where v.game_id is null 
                                and v.game_profile_id is null
                                and v.PromotionType = 'G'
                                order by sort1, sort2",
                    new SqlParameter("@game_id", game_id),
                    new SqlParameter("@game_profile_id", game_profile_id),
                    new SqlParameter("@membershipId", inMembershipId));

            log.LogVariableState("@game_id", game_id);
            log.LogVariableState("@game_profile_id", game_profile_id);
            log.LogVariableState("@membershipId", inMembershipId);
            promotion_credits = credits;
            promotion_vip_credits = vip_credits;
            bonus_allowed = "Y";
            courtesy_allowed = "Y";
            time_allowed = "Y";
            ticket_allowed = "Y";

            if (dt.Rows.Count > 0)
            {
                if (inMembershipId != -2) // price for a specific gameplay
                {
                    if (Utilities.executeScalar(@"select top 1 1 
                                                  where not exists (select 1 
                                                                    from PromotionRule 
                                                                    where promotion_id = @promoId) 
                                                        or (exists (select 1 
                                                                      from PromotionRule pr
                                                                     where promotion_id = @promoId
                                                                      and MembershipID = @membershipId))",
                                                new SqlParameter("@promoId", dt.Rows[0]["promotion_id"]),
                                                new SqlParameter("@membershipId", inMembershipId)) == null)
                    {
                        log.LogVariableState("@promoId", dt.Rows[0]["promotion_id"]);
                        log.LogVariableState("@membershipId", inMembershipId);
                        log.LogVariableState("Promotion credits", promotion_credits);
                        log.LogVariableState("promotion_vip_credits", promotion_vip_credits);
                        log.LogVariableState("bonus_allowed", bonus_allowed);
                        log.LogVariableState("courtesy_allowed", courtesy_allowed);
                        log.LogVariableState("time_allowed", time_allowed);
                        log.LogVariableState("ticket_allowed", ticket_allowed);
                        log.LogVariableState("ThemeNumber", ThemeNumber);
                        log.LogVariableState("promoVisualizationThemeNumber", promoVisualizationThemeNumber);
                        log.LogVariableState("PromotionId", PromotionId);
                        log.LogMethodExit(null);
                        return;
                    }
                }
                else // exclusions exist, don't change price on readers
                {
                    log.LogVariableState("@promoId", dt.Rows[0]["promotion_id"]);
                    if (Utilities.executeScalar(@"select top 1 1 
                                                    from PromotionRule 
                                                    where promotion_id = @promoId",
                                                new SqlParameter("@promoId", dt.Rows[0]["promotion_id"])) != null)
                    {
                        log.LogVariableState("Promotion credits", promotion_credits);
                        log.LogVariableState("promotion_vip_credits", promotion_vip_credits);
                        log.LogVariableState("bonus_allowed", bonus_allowed);
                        log.LogVariableState("courtesy_allowed", courtesy_allowed);
                        log.LogVariableState("time_allowed", time_allowed);
                        log.LogVariableState("ticket_allowed", ticket_allowed);
                        log.LogVariableState("ThemeNumber", ThemeNumber);
                        log.LogVariableState("promoVisualizationThemeNumber", promoVisualizationThemeNumber);
                        log.LogVariableState("PromotionId", PromotionId);
                        log.LogMethodExit(null);
                        return;
                    }
                }

                if (dt.Rows[0]["absolute_credits"] != DBNull.Value)
                    promotion_credits = Convert.ToDecimal(dt.Rows[0]["absolute_credits"]);
                else
                    if (dt.Rows[0]["discount_on_credits"] != DBNull.Value)
                        promotion_credits = credits - credits * Convert.ToDecimal(dt.Rows[0]["discount_on_credits"]) / 100;
                    else
                        promotion_credits = credits;

                if (dt.Rows[0]["absolute_vip_credits"] != DBNull.Value)
                    promotion_vip_credits = Convert.ToDecimal(dt.Rows[0]["absolute_vip_credits"]);
                else
                    if (dt.Rows[0]["discount_on_vip_credits"] != DBNull.Value)
                        promotion_vip_credits = vip_credits - vip_credits * Convert.ToDecimal(dt.Rows[0]["discount_on_vip_credits"]) / 100;
                    else
                        promotion_vip_credits = vip_credits;

                bonus_allowed = dt.Rows[0]["bonus_allowed"].ToString();
                courtesy_allowed = dt.Rows[0]["courtesy_allowed"].ToString();
                time_allowed = dt.Rows[0]["time_allowed"].ToString();
                ticket_allowed = dt.Rows[0]["ticket_allowed"].ToString();

                if (dt.Rows[0]["ThemeNumber"] != DBNull.Value)
                    ThemeNumber = Convert.ToInt32(dt.Rows[0]["ThemeNumber"]);

                if (dt.Rows[0]["VisualizationThemeNumber"] != DBNull.Value)
                    promoVisualizationThemeNumber = Convert.ToInt32(dt.Rows[0]["VisualizationThemeNumber"]);

                PromotionId = Convert.ToInt32(dt.Rows[0]["promotion_id"]);
                PromotionDetailId = Convert.ToInt32(dt.Rows[0]["promotion_detail_id"]);
            }
            log.LogVariableState("Promotion credits", promotion_credits);
            log.LogVariableState("promotion_vip_credits", promotion_vip_credits);
            log.LogVariableState("bonus_allowed", bonus_allowed);
            log.LogVariableState("courtesy_allowed", courtesy_allowed);
            log.LogVariableState("time_allowed", time_allowed);
            log.LogVariableState("ticket_allowed", ticket_allowed);
            log.LogVariableState("ThemeNumber", ThemeNumber);
            log.LogVariableState("promoVisualizationThemeNumber", promoVisualizationThemeNumber);
            log.LogVariableState("PromotionId", PromotionId);
            log.LogVariableState("PromotionDetailId", PromotionDetailId);
            log.LogMethodExit(null);
        }

        public static List<MachinePromotion> getPromotionDetails(int inMembershipId, string machineIdList,
                                                Utilities Utilities)
        {
            log.LogMethodEntry(inMembershipId, machineIdList, Utilities);
            List<MachinePromotion> machinePromotionsList = new List<MachinePromotion>();
            int gameId = -1;
            int gameProfileId = -1;
            char type = 'G';
            int promotionDetailId = -1;
            DataTable dt = Utilities.executeDataTable(
                            @"select v.*, rt.ThemeNumber, rtv.ThemeNumber VisualizationThemeNumber, case when @membershipId < 0 then r.MembershipId else case when r.MembershipId = @membershipId then -2 else isnull(r.MembershipId, -1) end end sort1, 1 sort2 
                                from PromotionView v
                                left outer join PromotionRule r
                                    on r.promotion_id = v.promotion_id
                                left outer join Theme rt
                                    on rt.Id = v.ThemeId
                                left outer join Theme rtv
                                    on rtv.Id = v.VisualizationThemeId
                              where v.game_id in (select game_id from machines where machine_id in " + machineIdList + @")
                              union all
                              select v.*, rt.ThemeNumber, rtv.ThemeNumber VisualizationThemeNumber, case when @membershipId < 0 then r.MembershipId else case when r.MembershipId = @membershipId then -2 else isnull(r.MembershipId, -1) end end, 2 
                                from PromotionView v
                                left outer join PromotionRule r
                                    on r.promotion_id = v.promotion_id
                                left outer join Theme rt
                                    on rt.Id = v.ThemeId
                                left outer join Theme rtv
                                    on rtv.Id = v.VisualizationThemeId
                              where v.game_profile_id in (select g.game_profile_id from games g, machines m
                                                           where g.game_id = m.game_id 
                                                             and m.machine_id in " + machineIdList + @")
                              union all
                              select v.*, rt.ThemeNumber, rtv.ThemeNumber VisualizationThemeNumber, case when @membershipId < 0 then r.MembershipId else case when r.MembershipId = @membershipId then -2 else isnull(r.MembershipId, -1) end end, 3 
                                from PromotionView v
                                left outer join PromotionRule r
                                    on r.promotion_id = v.promotion_id
                                left outer join Theme rt
                                    on rt.Id = v.ThemeId
                                left outer join Theme rtv
                                    on rtv.Id = v.VisualizationThemeId
                              where v.game_id is null 
                                and v.game_profile_id is null
                                and v.PromotionType = 'G'
                                order by sort1, sort2",
                    new SqlParameter("@membershipId", inMembershipId));

            log.LogVariableState("@membershipId", inMembershipId);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["game_id"] != DBNull.Value)
                {
                    gameId = Convert.ToInt32(dt.Rows[0]["game_id"]);
                    type = 'G';
                }
                if (dt.Rows[0]["game_profile_id"] != DBNull.Value)
                {
                    gameProfileId = Convert.ToInt32(dt.Rows[0]["game_profile_id"]);
                    type = 'P';
                }
                promotionDetailId = Convert.ToInt32(dt.Rows[0]["promotion_detail_id"]);
                MachinePromotion machinePromotion = new MachinePromotion(gameId, gameProfileId, type, promotionDetailId);
                machinePromotionsList.Add(machinePromotion);
                log.LogVariableState("Machine Promotion: ", machinePromotion);
            }
            log.LogMethodExit(machinePromotionsList);
            return machinePromotionsList;
        }
    }
}
