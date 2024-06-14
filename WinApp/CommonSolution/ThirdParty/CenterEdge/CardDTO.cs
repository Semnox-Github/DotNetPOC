/********************************************************************************************
 * Project Name - CenterEdge  
 * Description  - CardDTO class represnts the card details 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Sep-2020       Girish Kundar             Created : CenterEdge  REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.ThirdParty.CenterEdge
{
    public class CardDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// accountId field
        /// </summary>
        protected int accountId;
        /// <summary>
        /// tagNumber field
        /// </summary>
        protected string accountNumber;
        /// <summary>
        /// customerName field
        /// </summary>
        protected string customerName;
        /// <summary>
        /// issueDate field
        /// </summary>
        protected DateTime? issueDate;
        /// <summary>
        /// faceValue field
        /// </summary>
        protected decimal? faceValue;
        /// <summary>
        /// refundFlag field
        /// </summary>
        protected bool refundFlag;
        /// <summary>
        /// refundAmount field
        /// </summary>
        protected decimal? refundAmount;
        /// <summary>
        /// refundDate field
        /// </summary>
        protected DateTime? refundDate;
        /// <summary>
        /// validFlag field
        /// </summary>
        protected bool validFlag;
        /// <summary>
        /// ticketCount field
        /// </summary>
        protected int? ticketCount;
        /// <summary>
        /// notes field
        /// </summary>
        protected string notes;
        /// <summary>
        /// credits field
        /// </summary>
        protected decimal? credits;
        /// <summary>
        /// courtesy field
        /// </summary>
        protected decimal? courtesy;
        /// <summary>
        /// bonus field
        /// </summary>
        protected decimal? bonus;
        /// <summary>
        /// time field
        /// </summary>
        protected decimal? time;
        /// <summary>
        /// customerId field
        /// </summary>
        protected int customerId;
        /// <summary>
        /// creditsPlayed field
        /// </summary>
        protected decimal? creditsPlayed;
        /// <summary>
        /// ticketAllowed field
        /// </summary>
        protected bool ticketAllowed;
        /// <summary>
        /// realTicketMode field
        /// </summary>
        protected bool realTicketMode;
        /// <summary>
        /// vipCustomer field
        /// </summary>
        protected bool vipCustomer;
        /// <summary>
        /// startTime field
        /// </summary>
        protected DateTime? startTime;
        /// <summary>
        /// lastPlayedTime field
        /// </summary>
        protected DateTime? lastPlayedTime;
        /// <summary>
        /// technicianCard field
        /// </summary>
        protected string technicianCard;
        /// <summary>
        /// techGames field
        /// </summary>
        protected int? techGames;
        /// <summary>
        /// timerResetCard field
        /// </summary>
        protected bool timerResetCard;
        /// <summary>
        /// loyaltyPoints field
        /// </summary>
        protected decimal? loyaltyPoints;
        /// <summary>
        /// uploadSiteId field
        /// </summary>
        protected int uploadSiteId;
        /// <summary>
        /// uploadTime field
        /// </summary>
        protected DateTime? uploadTime;
        /// <summary>
        /// expiryDate field
        /// </summary>
        protected DateTime? expiryDate;
        /// <summary>
        /// downloadBatchId field
        /// </summary>
        protected int downloadBatchId;
        /// <summary>
        /// refreshFromHQTime field
        /// </summary>
        protected DateTime? refreshFromHQTime;
        /// <summary>
        /// accountIdentifier field
        /// </summary>
        protected string accountIdentifier;
        /// <summary>
        /// primaryAccount field
        /// </summary>
        protected bool primaryAccount;
        /// <summary>
        /// lastUpdatedBy field
        /// </summary>
        protected string lastUpdatedBy;
        /// <summary>
        /// lastUpdateDate field
        /// </summary>
        protected DateTime lastUpdateDate;
        /// <summary>
        /// siteId field
        /// </summary>
        protected int siteId;
        /// <summary>
        /// masterEntityId field
        /// </summary>
        protected int masterEntityId;
        /// <summary>
        /// synchStatus field
        /// </summary>
        protected bool synchStatus;
        /// <summary>
        /// guid field
        /// </summary>
        protected string guid;
        /// <summary>
        /// membershipId field
        /// </summary>
        protected int membershipId;
        /// <summary>
        /// membershipName field
        /// </summary>
        protected string membershipName;

        /// <summary>
        /// Default constructor
        /// </summary>
        public CardDTO()
        {
            log.LogMethodEntry();
            accountId = -1;
            customerId = -1;
            masterEntityId = -1;
            membershipId = -1;
            refundFlag = false;
            validFlag = true;
            ticketAllowed = true;
            realTicketMode = false;
            vipCustomer = false;
            timerResetCard = false;
            primaryAccount = false;
            siteId = -1;
            log.LogMethodExit();
        }
        public int cardId
        {
            get
            {
                return accountId;
            }

            set
            {
             
                accountId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the tagNumber field
        /// </summary>
        [DisplayName("Card Number")]
        public string cardNumber
        {
            get
            {
                return accountNumber;
            }

            set
            {

                accountNumber = value;
            }
        }

        /// <summary>
        /// Get/Set method of the customerName field
        /// </summary>
        [DisplayName("Customer")]
        public string CustomerName
        {
            get
            {
                return customerName;
            }

            set
            {
             
                customerName = value;
            }
        }

        /// <summary>
        /// Get/Set method of the issueDate field
        /// </summary>
        [DisplayName("Issue Date")]
        public DateTime? IssueDate
        {
            get
            {
                return issueDate;
            }

            set
            {
             
                issueDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the faceValue field
        /// </summary>
        [DisplayName("Deposit")]
        public decimal? FaceValue
        {
            get
            {
                return faceValue;
            }

            set
            {
             
                faceValue = value;
            }
        }

        /// <summary>
        /// Get/Set method of the credits field
        /// </summary>
        [DisplayName("Credits")]
        public decimal? Credits
        {
            get
            {
                return credits;
            }

            set
            {
             
                credits = value;
            }
        }

        /// <summary>
        /// Get/Set method of the courtesy field
        /// </summary>
        [DisplayName("Courtesy")]
        public decimal? Courtesy
        {
            get
            {
                return courtesy;
            }

            set
            {
             
                courtesy = value;
            }
        }

        /// <summary>
        /// Get/Set method of the bonus field
        /// </summary>
        [DisplayName("Bonus")]
        public decimal? Bonus
        {
            get
            {
                return bonus;
            }

            set
            {
             
                bonus = value;
            }
        }

        /// <summary>
        /// Get/Set method of the time field
        /// </summary>
        [DisplayName("Time")]
        public decimal? Time
        {
            get
            {
                return time;
            }

            set
            {
             
                time = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ticketCount field
        /// </summary>
        [DisplayName("Ticket Count")]
        public int? TicketCount
        {
            get
            {
                return ticketCount;
            }

            set
            {
             
                ticketCount = value;
            }
        }

        /// <summary>
        /// Get/Set method of the loyaltyPoints field
        /// </summary>
        [DisplayName("Loyalty Points")]
        public decimal? LoyaltyPoints
        {
            get
            {
                return loyaltyPoints;
            }

            set
            {
             
                loyaltyPoints = value;
            }
        }

        /// <summary>
        /// Get/Set method of the creditsPlayed field
        /// </summary>
        [DisplayName("Credits Played")]
        public decimal? CreditsPlayed
        {
            get
            {
                return creditsPlayed;
            }

            set
            {
             
                creditsPlayed = value;
            }
        }

        /// <summary>
        /// Get/Set method of the realTicketMode field
        /// </summary>
        [DisplayName("Real Ticket Mode")]
        public bool RealTicketMode
        {
            get
            {
                return realTicketMode;
            }

            set
            {
             
                realTicketMode = value;
            }
        }

        /// <summary>
        /// Get/Set method of the vipCustomer field
        /// </summary>
        [DisplayName("Vip Customer")]
        public bool VipCustomer
        {
            get
            {
                return vipCustomer;
            }

            set
            {
             
                vipCustomer = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ticketAllowed field
        /// </summary>
        [DisplayName("Ticket Allowed")]
        public bool TicketAllowed
        {
            get
            {
                return ticketAllowed;
            }

            set
            {
             
                ticketAllowed = value;
            }
        }

        /// <summary>
        /// Get/Set method of the technicianCard field
        /// </summary>
        [DisplayName("Tech Card?")]
        public string TechnicianCard
        {
            get
            {
                return technicianCard;
            }

            set
            {
             
                technicianCard = value;
            }
        }

        /// <summary>
        /// Get/Set method of the timerResetCard field
        /// </summary>
        [DisplayName("Timer Reset Card")]
        public bool TimerResetCard
        {
            get
            {
                return timerResetCard;
            }
            set
            {
             
                timerResetCard = value;
            }
        }

        /// <summary>
        /// Get/Set method of the techGames field
        /// </summary>
        [DisplayName("Tech Games")]
        public int? TechGames
        {
            get
            {
                return techGames;
            }

            set
            {
             
                techGames = value;
            }
        }

        /// <summary>
        /// Get/Set method of the validFlag field
        /// </summary>
        [DisplayName("Valid Flag")]
        public bool ValidFlag
        {
            get
            {
                return validFlag;
            }

            set
            {
             
                validFlag = value;
            }
        }

        /// <summary>
        /// Get/Set method of the refundFlag field
        /// </summary>
        [DisplayName("Refund Flag")]
        public bool RefundFlag
        {
            get
            {
                return refundFlag;
            }

            set
            {
             
                refundFlag = value;
            }
        }

        /// <summary>
        /// Get/Set method of the refundAmount field
        /// </summary>
        [DisplayName("Refund Amount")]
        public decimal? RefundAmount
        {
            get
            {
                return refundAmount;
            }

            set
            {
             
                refundAmount = value;
            }
        }

        /// <summary>
        /// Get/Set method of the refundDate field
        /// </summary>
        [DisplayName("Refund Date")]
        public DateTime? RefundDate
        {
            get
            {
                return refundDate;
            }

            set
            {
             
                refundDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the expiryDate field
        /// </summary>
        [DisplayName("Expiry Date")]
        public DateTime? ExpiryDate
        {
            get
            {
                return expiryDate;
            }

            set
            {
             
                expiryDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the startTime field
        /// </summary>
        [DisplayName("Start Time")]
        public DateTime? StartTime
        {
            get
            {
                return startTime;
            }

            set
            {
             
                startTime = value;
            }
        }

        /// <summary>
        /// Get/Set method of the lastPlayedTime field
        /// </summary>
        [DisplayName("Last Played Time")]
        public DateTime? LastPlayedTime
        {
            get
            {
                return lastPlayedTime;
            }

            set
            {
             
                lastPlayedTime = value;
            }
        }

        /// <summary>
        /// Get/Set method of the notes field
        /// </summary>
        [DisplayName("Notes")]
        public string Notes
        {
            get
            {
                return notes;
            }

            set
            {
             
                notes = value;
            }
        }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("Last Update Time")]
        public DateTime LastUpdateDate
        {
            get
            {
                return lastUpdateDate;
            }

            set
            {
             
                lastUpdateDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Last Updated By")]
        public string LastUpdatedBy
        {
            get
            {
                return lastUpdatedBy;
            }
            set
            {
             
                lastUpdatedBy = value;
            }
        }

        /// <summary>
        /// Get/Set method of the primaryCard field
        /// </summary>
        [DisplayName("Primary Card")]
        public bool PrimaryAccount
        {
            get
            {
                return primaryAccount;
            }

            set
            {
             
                primaryAccount = value;
            }
        }

        /// <summary>
        /// Get/Set method of the tagIdentifier field
        /// </summary>
        [DisplayName("Card Identifier")]
        public string AccountIdentifier
        {
            get
            {
                return accountIdentifier;
            }

            set
            {
             
                accountIdentifier = value;
            }
        }

        /// <summary>
        /// Get/Set method of the membershipName field
        /// </summary>
        [DisplayName("Membership")]
        public string MembershipName
        {
            get
            {
                return membershipName;
            }

            set
            {
             
                membershipName = value;
            }
        }

        /// <summary>
        /// Get/Set method of the membershipId field
        /// </summary>
        public int MembershipId
        {
            get
            {
                return membershipId;
            }

            set
            {
             
                membershipId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the customerId field
        /// </summary>
        public int CustomerId
        {
            get
            {
                return customerId;
            }

            set
            {
             
                customerId = value;
            }
        }

         

 
    }
}
