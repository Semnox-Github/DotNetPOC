/********************************************************************************************
 * Project Name - Customer.Accounts
 * Description  - Class for  of AccountColumnProvider      
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019   Girish kundar  Modified : Added Logger Methods and Removed Unused namespace's.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.Customer.Accounts
{
    class AccountColumnProvider : ColumnProvider
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        internal AccountColumnProvider()
        {
            log.LogMethodEntry("Begins  - Inside AccountColumnProvider() ");
            columnDictionary = new Dictionary<Enum, Column>() {
                    { AccountDTO.SearchByParameters.ACCOUNT_ID, new NumberColumn("Cards.card_id", "Card Id") },
                    { AccountDTO.SearchByParameters.TAG_NUMBER, new TextColumn("Cards.card_number", "Card Number")},
                    { AccountDTO.SearchByParameters.CUSTOMER_NAME, new TextColumn("(ISNULL(Profile.FirstName,'') + ' ' + ISNULL(Profile.LastName,''))", "Customer Name")},
                    { AccountDTO.SearchByParameters.ISSUE_DATE, new DateTimeColumn("Cards.issue_date", "Issue Date")},
                    { AccountDTO.SearchByParameters.FACE_VALUE, new NumberColumn("Cards.face_value", "Deposit")},
                    { AccountDTO.SearchByParameters.REFUND_FLAG, new TextColumn("Cards.refund_flag", "Refund Flag","'N'")},
                    { AccountDTO.SearchByParameters.REFUND_AMOUNT, new NumberColumn("Cards.refund_amount", "Refund Amount")},
                    { AccountDTO.SearchByParameters.REFUND_DATE, new DateTimeColumn("Cards.refund_date", "Refund Date")},
                    { AccountDTO.SearchByParameters.VALID_FLAG, new TextColumn("Cards.valid_flag", "Valid Flag","'N'")},
                    { AccountDTO.SearchByParameters.TICKET_COUNT, new NumberColumn("Cards.ticket_count", "Ticket Count")},
                    { AccountDTO.SearchByParameters.NOTES, new TextColumn("Cards.notes", "Notes")},
                    { AccountDTO.SearchByParameters.CUSTOMER_ID, new NumberColumn("Cards.customer_id", "Customer Id")},
                    { AccountDTO.SearchByParameters.LAST_UPDATE_TIME, new DateTimeColumn("Cards.last_update_time", "Last Update Time")},
                    { AccountDTO.SearchByParameters.CREDITS, new NumberColumn("Cards.credits", "Credits")},
                    { AccountDTO.SearchByParameters.COURTESY, new NumberColumn("Cards.courtesy", "Courtesy")},
                    { AccountDTO.SearchByParameters.BONUS, new NumberColumn("Cards.bonus", "Bonus")},
                    { AccountDTO.SearchByParameters.TIME, new NumberColumn("Cards.time", "Time")},
                    { AccountDTO.SearchByParameters.CREDITS_PLAYED, new NumberColumn("Cards.credits_played", "Credits Played")},
                    { AccountDTO.SearchByParameters.TICKET_ALLOWED, new TextColumn("Cards.ticket_allowed", "Ticket Allowed","'N'")},
                    { AccountDTO.SearchByParameters.REAL_TICKET_MODE, new TextColumn("Cards.real_ticket_mode", "Real Ticket Mode","'N'")},
                    { AccountDTO.SearchByParameters.VIP_CUSTOMER, new TextColumn("Cards.vip_customer", "Vip Customer","'N'")},
                    { AccountDTO.SearchByParameters.SITE_ID, new NumberColumn("Cards.site_id", "Site Id")},
                    { AccountDTO.SearchByParameters.START_TIME, new DateTimeColumn("Cards.start_time", "Start Time")},
                    { AccountDTO.SearchByParameters.LAST_PLAYED_TIME, new DateTimeColumn("Cards.last_played_time", "Last Played Time")},
                    { AccountDTO.SearchByParameters.TECHNICIAN_CARD, new TextColumn("Cards.technician_card", "Technician Card","'N'")},
                    { AccountDTO.SearchByParameters.TECH_GAMES, new NumberColumn("Cards.tech_games", "Tech Games")},
                    { AccountDTO.SearchByParameters.TIMER_RESET_CARD, new TextColumn("Cards.timer_reset_card", "Timer Reset Card","'N'")},
                    { AccountDTO.SearchByParameters.LOYALTY_POINTS, new NumberColumn("Cards.loyalty_points", "Loyalty Points")},
                    { AccountDTO.SearchByParameters.LAST_UPDATED_BY, new TextColumn("Cards.LastUpdatedBy", "Last Updated By")},
                    { AccountDTO.SearchByParameters.EXPIRY_DATE, new DateTimeColumn("Cards.ExpiryDate", "Expiry Date")},
                    { AccountDTO.SearchByParameters.ACCOUNT_IDENTIFIER, new TextColumn("Cards.CardIdentifier", "Card Identifier")},
                    { AccountDTO.SearchByParameters.PRIMARY_ACCOUNT, new TextColumn("Cards.PrimaryCard", "Primary Card","'N'")},
                    { AccountDTO.SearchByParameters.MEMBERSHIP_ID, new NumberColumn("Membership.MembershipID", "Membership Id")},
                    { AccountDTO.SearchByParameters.MEMBERSHIP_NAME, new TextColumn("Membership.MembershipName", "Membership Name")},
            };
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// customer search criteria
    /// </summary>
    public class AccountSearchCriteria : SearchCriteria
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public AccountSearchCriteria() : base(new AccountColumnProvider())
        {

        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="columnIdentifier"></param>
        /// <param name="operator"></param>
        /// <param name="parameters"></param>
        public AccountSearchCriteria(Enum columnIdentifier, Operator @operator, params object[] parameters) :
            base(new AccountColumnProvider(), columnIdentifier, @operator, parameters)
        {

        }
    }
}
