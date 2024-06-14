/********************************************************************************************
* Project Name - Loyalty
* Description  - Punchh Loyalty Class 
* 
**************
**Version Log
**************
*Version     Date             Modified By         Remarks          
*********************************************************************************************
*2.120.0     12-Dec-2020      Girish Kundar       Created
*********************************************************************************************/
using System;
using System.Collections.Generic;
namespace Semnox.Parafait.ThirdParty.ThirdPartyLoyalty
{
    /// <summary>
    /// PunchhMemberDetailDTO
    /// </summary>
    public class PunchhMemberDetailDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string address_line1 { get; set; }
        public string anniversary { get; set; }
        public string avatar_remote_url { get; set; }
        public string birthday { get; set; }
        public string city { get; set; }
        public string created_at { get; set; }
        public string email { get; set; }
        public bool email_verified { get; set; }
        public string fb_uid { get; set; }
        public string first_name { get; set; }
        public bool age_verified { get; set; }
        public bool privacy_policy { get; set; }
        public string gender { get; set; }
        public int id { get; set; }
        public string last_name { get; set; }
        public string state { get; set; }
        public string updated_at { get; set; }
        public string zip_code { get; set; }
        public Balance balance { get; set; }
        public string user_digest { get; set; }
        public string selected_card_number { get; set; }
        public int? selected_reward_id { get; set; }
        public decimal? selected_discount_amount { get; set; }
        public int? selected_redeemable_id { get; set; }
        public List<Reward> rewards { get; set; }
        public string discount_type { get; set; }
        public string phone { get; set; }

        public PunchhMemberDetailDTO()
        {
            log.LogMethodEntry();
            this.address_line1 = string.Empty;
            this.anniversary = null;
            this.avatar_remote_url = string.Empty;
            this.birthday = string.Empty;
            this.city = string.Empty;
            this.created_at = string.Empty;
            this.email = string.Empty;
            this.fb_uid = string.Empty;
            this.age_verified = true;
            this.privacy_policy = true;
            this.gender = string.Empty;
            this.id = -1;
            this.last_name = string.Empty;
            this.state = string.Empty;
            this.updated_at = string.Empty;
            this.balance = new Balance();
            this.user_digest = string.Empty;
            this.selected_card_number = string.Empty;
            this.selected_reward_id = null;
            this.selected_discount_amount = null;
            this.rewards = new List<Reward>();
            this.discount_type = string.Empty;
            this.phone = string.Empty;
            this.selected_redeemable_id = null;
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Balance
    /// </summary>
    public class Balance
    {
        public string banked_rewards { get; set; }
        public string membership_level { get; set; }
        public int? membership_level_id { get; set; }
        public double net_balance { get; set; }
        public double net_debits { get; set; }
        public double pending_points { get; set; }
        public double points_balance { get; set; }
        public string signup_anniversary_day { get; set; }
        public double total_credits { get; set; }
        public string total_debits { get; set; }
        public double total_point_credits { get; set; }
        public int total_redeemable_visits { get; set; }
        public string expired_membership_level { get; set; }
        public int total_visits { get; set; }
        public int initial_visits { get; set; }
        public int unredeemed_cards { get; set; }
        public int? earliest_expiring_points { get; set; }
        public string earliest_expiry_date { get; set; }

        public Balance()
        {
            this.banked_rewards = string.Empty;
            this.membership_level = string.Empty;
            this.membership_level_id = null;
            this.net_balance = 0;
            this.net_debits = 0;
            this.pending_points = 0;
            this.signup_anniversary_day = string.Empty;
            this.total_credits = 0;
            this.total_debits = string.Empty;
            this.total_point_credits = 0;
            this.total_redeemable_visits = 0;
            this.expired_membership_level = string.Empty;
            this.total_visits = 0;
            this.initial_visits = 0;
            this.unredeemed_cards = 0;
            this.earliest_expiring_points = null;
            this.earliest_expiry_date = string.Empty;
        }
    }



    /// <summary>
    /// Reward
    /// </summary>
    public class Reward
    {
        public long id { get; set; }
        public string created_at { get; set; }
        public string end_date_tz { get; set; }
        public string start_date_tz { get; set; }
        public string updated_at { get; set; }
        public string image { get; set; }
        public string status { get; set; }
        public int points { get; set; }
        public double discount_amount { get; set; }
        public string description { get; set; }
        public string name { get; set; }
        public string redeemable_properties { get; set; }
        public string type { get; set; }

        public Reward()
        {
            this.id = -1;
            this.created_at = string.Empty;
            this.end_date_tz = string.Empty;
            this.start_date_tz = string.Empty;
            this.updated_at = string.Empty;
            this.image = string.Empty;
            this.status = string.Empty;
            this.points = 0;
            this.discount_amount = 0;
            this.description = string.Empty;
            this.name = string.Empty;
            this.redeemable_properties = string.Empty;
            this.type = string.Empty;
        }
    }


}

