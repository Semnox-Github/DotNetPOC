/********************************************************************************************
 * Project Name - ServerCore
 * Description  - Class for handling Game Machine Promotion     
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.150.2      03-Mar-2022   Abhishek       Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Promotions;
using System.Threading.Tasks;

namespace Semnox.Parafait.ServerCore
{
    public class GameMachinePromotion
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        int gameId;
        int gameProfileId;
        char type;
        int promotionDetailId;

        public GameMachinePromotion(int gameId, int gameProfileId, char type, int promotionDetailId)
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

    public static class GamePromotionBL
    {
        
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        public static PromotionViewDTO getPromotionDetails(int inMembershipId, int game_id, int game_profile_id,
                                                decimal credits, decimal vip_credits, ref decimal promotion_credits,
                                                ref decimal promotion_vip_credits, ref string bonus_allowed, ref string courtesy_allowed,
                                                ref string time_allowed, ref string ticket_allowed, ref int ThemeNumber,
                                                ref int promoVisualizationThemeNumber,
                                                ref int PromotionId,
                                                ref int PromotionDetailId,
                                                ExecutionContext executionContext)
        {
            PromotionViewDTO promotionViewDTO = new PromotionViewDTO();
            promotion_credits = credits;
            promotion_vip_credits = vip_credits;
            bonus_allowed = "Y";
            courtesy_allowed = "Y";
            time_allowed = "Y";
            ticket_allowed = "Y";            
            promotionViewDTO = getPromotionDetails(inMembershipId, game_id, game_profile_id, executionContext);
            if (promotionViewDTO != null)
            {
                if (promotionViewDTO.AbsoluteCredits != null)
                    promotion_credits = Convert.ToDecimal(promotionViewDTO.AbsoluteCredits);
                else if (promotionViewDTO.DiscountOnCredits != null)
                    promotion_credits = credits - credits * Convert.ToDecimal(promotionViewDTO.DiscountOnCredits) / 100;
                else
                    promotion_credits = credits;

                if (promotionViewDTO.AbsoluteVipCredits != null)
                    promotion_vip_credits = Convert.ToDecimal(promotionViewDTO.AbsoluteVipCredits);
                else if (promotionViewDTO.DiscountOnVipCredits != null)
                    promotion_vip_credits = vip_credits - vip_credits * Convert.ToDecimal(promotionViewDTO.DiscountOnVipCredits) / 100;
                else
                    promotion_vip_credits = vip_credits;

                bonus_allowed = promotionViewDTO.BonusAllowed;
                courtesy_allowed = promotionViewDTO.CourtesyAllowed;
                time_allowed = promotionViewDTO.TimeAllowed;
                ticket_allowed = promotionViewDTO.TicketAllowed;

                if (promotionViewDTO.ThemeNumber > -1)
                    ThemeNumber = promotionViewDTO.ThemeNumber;

                if (promotionViewDTO.VisualizationThemeNumber > -1)
                    promoVisualizationThemeNumber = promotionViewDTO.VisualizationThemeNumber;

                PromotionId = promotionViewDTO.PromotionId;
                PromotionDetailId = promotionViewDTO.PromotionDetailId;
            }
            return promotionViewDTO;
        }

        public static PromotionViewDTO getPromotionDetails(int inMembershipId, int game_id, int game_profile_id, ExecutionContext executionContext, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(inMembershipId, game_id, game_profile_id, sqlTransaction);
            PromotionListBL promotionListBL = new PromotionListBL(executionContext);
            PromotionViewDTO promotionViewDTO = promotionListBL.GetGamePromotionDetailViewDTO(inMembershipId, game_id, game_profile_id, sqlTransaction);

            log.LogVariableState("@game_id", game_id);
            log.LogVariableState("@game_profile_id", game_profile_id);
            log.LogVariableState("@membershipId", inMembershipId);

            if (promotionViewDTO != null)
            {
                if (inMembershipId != -2) // price for a specific gameplay
                {
                    PromotionRuleListBL promotionRuleListBL = new PromotionRuleListBL(executionContext);
                    List<PromotionRuleDTO> promotionRuleDTOList = null;
                    List<PromotionRuleDTO> promotionRuleMembershipDTOList = null;
                    List<KeyValuePair<PromotionRuleDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<PromotionRuleDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<PromotionRuleDTO.SearchByParameters, string>(PromotionRuleDTO.SearchByParameters.PROMOTION_ID, promotionViewDTO.PromotionId.ToString()));
                    promotionRuleDTOList = promotionRuleListBL.GetPromotionRuleDTOList(searchParameters);
                    promotionRuleListBL = new PromotionRuleListBL(executionContext);
                    searchParameters.Add(new KeyValuePair<PromotionRuleDTO.SearchByParameters, string>(PromotionRuleDTO.SearchByParameters.MEMBERSHIP_ID, inMembershipId.ToString()));
                    promotionRuleMembershipDTOList = promotionRuleListBL.GetPromotionRuleDTOList(searchParameters);
                    bool promoRule = false;
                    bool promoRuleMembership = false;

                    if (promotionRuleDTOList != null && promotionRuleDTOList.Count > 0)
                    {
                        log.LogVariableState("Promotion rule exists hence return and ignore PromotionViewDTO", promoRule);
                        promoRule = true;
                    }
                    if (promotionRuleMembershipDTOList == null && promotionRuleMembershipDTOList.Count > 0)
                    {
                        log.LogVariableState("Promotion rule and Membership does not exists hence return and ignore PromotionViewDTO", promoRuleMembership);
                        promoRuleMembership = true;
                    }

                    if(promoRule || promoRuleMembership)
                    {
                        log.LogVariableState("@promoId", promotionViewDTO.PromotionId);
                        log.LogVariableState("@membershipId", inMembershipId);
                        log.LogMethodExit("null");
                        return null;
                    }
                }
                else // exclusions exist, don't change price on readers
                {
                    PromotionRuleListBL promotionRuleListBL = new PromotionRuleListBL(executionContext);
                    List<KeyValuePair<PromotionRuleDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<PromotionRuleDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<PromotionRuleDTO.SearchByParameters, string>(PromotionRuleDTO.SearchByParameters.PROMOTION_ID, promotionViewDTO.PromotionId.ToString()));
                    List<PromotionRuleDTO> promotionRuleDTOList =  promotionRuleListBL.GetPromotionRuleDTOList(searchParameters);
                    if (promotionRuleDTOList != null && promotionRuleDTOList.Any())
                    {
                        log.LogMethodExit("null");
                        return null;
                    }
                }
            }
            log.LogMethodExit(promotionViewDTO);
            return promotionViewDTO;
        }

        public static List<GameMachinePromotion> getPromotionDetails(int inMembershipId, string machineIdList, ExecutionContext executionContext, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(inMembershipId, machineIdList, sqlTransaction);
            List<GameMachinePromotion> machinePromotionsList = new List<GameMachinePromotion>();
            int gameId = -1;
            int gameProfileId = -1;
            char type = 'G';
            int promotionDetailId = -1;
            PromotionListBL promotionListBL = new PromotionListBL(executionContext);
            List<PromotionViewDTO> promotionViewDTOList = promotionListBL.GetGamePromotionDetailViewDTOList(inMembershipId, machineIdList, sqlTransaction);
            PromotionViewDTO promotionViewDTO = promotionViewDTOList.FirstOrDefault();
            log.LogVariableState("@membershipId", inMembershipId);
            if (promotionViewDTOList.Count > 0)
            {
                if (promotionViewDTO.GameId > -1)
                {
                    gameId = promotionViewDTO.GameId;
                    type = 'G';
                }
                if (promotionViewDTO.GameProfileId > -1)
                {
                    gameProfileId = promotionViewDTO.GameProfileId;
                    type = 'P';
                }
                promotionDetailId = promotionViewDTO.PromotionDetailId;
                GameMachinePromotion gameMachinePromotion = new GameMachinePromotion(gameId, gameProfileId, type, promotionDetailId);
                machinePromotionsList.Add(gameMachinePromotion);
                log.LogVariableState("Machine Promotion: ", gameMachinePromotion);
            }
            log.LogMethodExit(machinePromotionsList);
            return machinePromotionsList;
        }
    }
}
