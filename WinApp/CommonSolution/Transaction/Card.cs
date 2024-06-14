/************************************************************************************************************************
 * Project Name - Card
 * Description  - Business Logic to create and save card
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *************************************************************************************************************************
 *1.00        17-Sep-2008      Iqbal Mohammad Created 
 *2.60.1      25-Apr-2019      Indhu K        Enhanced logic to load registration entitlement
 *                                            only once for a customer
 *2.60.2      25-May-2019      Mathew Ninan   Bug fix - Issued cards were not loading registration 
 *                                            entitlement for a customer
 *2.70        1-Jul-2019       Lakshminarayana     Modified to add support for ULC cards 
 *2.70.0      16-Jul-2019      Mathew Ninan   Bug Fix - Change status of card property to Issued 
 *                                            once created as part of CreateCard method
 *2.70.3      07-Jan-2020      Nitin Pai      Fun City Changes - Force a DB Sync entry is the card site 
 *                                            and context site are different
 *2.70.3      14-Feb-2020      Lakshminarayana      Modified: Creating unregistered customer during check-in process
 *2.80.0      19-Mar-2020      Mathew NInan           Use new field ValidityStatus to track
 *                                                 status of entitlements
 *2.90.0       23-Jun-2020     Raghuveera      Variable refund changes in updating the credits_played
 *2.140.0      12-Dec-2021     Guru S A        Booking execute process performance fixes
 *2.130.4     22-Feb-2022   Mathew Ninan    Modified DateTime to ServerDateTime 
 *************************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Device;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Tags;
using Semnox.Parafait.Device.Lockers;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.DBSynch;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// Card class
    /// </summary>
    public class Card
    {
        DataAccessHandler dataAccessHandler;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string connstring;
        /// <summary>
        /// CardNumber
        /// </summary>
        public string CardNumber;
        /// <summary>
        /// CardStatus
        /// </summary>
        public string CardStatus;
        /// <summary>
        /// card_id
        /// </summary>
        public int card_id = -1;
        /// <summary>
        /// issue_date
        /// </summary>
        public DateTime issue_date;
        /// <summary>
        /// ExpiryDate
        /// </summary>
        public DateTime ExpiryDate;
        /// <summary>
        /// face_value
        /// </summary>
        public float face_value;
        /// <summary>
        /// refund_flag
        /// </summary>
        public char refund_flag;
        /// <summary>
        /// refund_amount
        /// </summary>
        public float refund_amount;
        /// <summary>
        /// refund_date
        /// </summary>
        public DateTime refund_date;
        /// <summary>
        /// valid_flag
        /// </summary>
        public char valid_flag;
        /// <summary>
        /// ticket_count
        /// </summary>
        public int ticket_count;
        /// <summary>
        /// addTicketCount
        /// </summary>
        public int addTicketCount;
        /// <summary>
        /// notes
        /// </summary>
        public string notes = "";
        /// <summary>
        /// last_update_time
        /// </summary>
        public DateTime last_update_time;
        /// <summary>
        /// CardNumber
        /// </summary>
        public double credits;
        /// <summary>
        /// courtesy
        /// </summary>
        public double courtesy;
        /// <summary>
        /// bonus
        /// </summary>
        public double bonus;
        /// <summary>
        /// time
        /// </summary>
        public double time;
        /// <summary>
        /// addCredits
        /// </summary>
        public double addCredits;
        /// <summary>
        /// addCourtesy
        /// </summary>
        public double addCourtesy;
        /// <summary>
        /// addBonus
        /// </summary>
        public double addBonus;
        /// <summary>
        /// addTime
        /// </summary>
        public double addTime;
        /// <summary>
        /// customer_id
        /// </summary>
        public int customer_id = -1;
        /// <summary>
        /// credits_played
        /// </summary>
        public double credits_played;
        /// <summary>
        /// loyalty_points
        /// </summary>
        public double loyalty_points;
        /// <summary>
        /// virtualPoints
        /// </summary>
        public double virtualPoints;
        /// <summary>
        /// ticket_allowed
        /// </summary>
        public char ticket_allowed = 'Y';
        /// <summary>
        /// real_ticket_mode
        /// </summary>
        public char real_ticket_mode;
        /// <summary>
        /// vip_customer
        /// </summary>
        public char vip_customer = 'N';
        /// <summary>
        /// customerDTO
        /// </summary>
        public CustomerDTO customerDTO;
        /// <summary>
        /// start_time
        /// </summary>
        public DateTime start_time = DateTime.MinValue;
        /// <summary>
        /// last_played_time
        /// </summary>
        public DateTime last_played_time = DateTime.MinValue;
        /// <summary>
        /// technician_card
        /// </summary>
        public char technician_card = 'N';
        /// <summary>
        /// tech_games
        /// </summary>
        public int tech_games;
        /// <summary>
        /// CardGames
        /// </summary>
        public int CardGames;
        /// <summary>
        /// CreditPlusCardBalance
        /// </summary>
        public double CreditPlusCardBalance;
        /// <summary>
        /// addCreditPlusCardBalance
        /// </summary>
        public double addCreditPlusCardBalance;
        /// <summary>
        /// CreditPlusCredits
        /// </summary> 
        public double CreditPlusCredits;
        /// <summary>
        /// CreditPlusBonus
        /// </summary>
        public double CreditPlusBonus;
        /// <summary>
        /// TotalCreditPlusLoyaltyPoints
        /// </summary>
        public double TotalCreditPlusLoyaltyPoints;
        /// <summary>
        /// RedeemableCreditPlusLoyaltyPoints
        /// </summary>
        public double RedeemableCreditPlusLoyaltyPoints;
        /// <summary>
        /// CreditPlusVirtualPoints
        /// </summary>
        public double CreditPlusVirtualPoints;
        /// <summary>
        /// CreditPlusTime
        /// </summary>
        public double CreditPlusTime;
        /// <summary>
        /// CreditPlusTickets
        /// </summary>
        public int CreditPlusTickets;
        /// <summary>
        /// loginId
        /// </summary>
        public string loginId;
        /// <summary>
        /// siteId
        /// </summary>
        public int siteId = -1;
        /// <summary>
        /// MembershipId
        /// </summary>
        public int MembershipId = -1;
        /// <summary>
        /// MembershipName
        /// </summary>
        public string MembershipName = "Normal";
        /// <summary>
        /// TotalRechargeAmount
        /// </summary>
        public double TotalRechargeAmount;
        /// <summary>
        /// creditPlusItemPurchase
        /// </summary>
        public double creditPlusItemPurchase;
        /// <summary>
        /// isMifare
        /// </summary>
        public bool isMifare = false;
        /// <summary>
        /// Utilities
        /// </summary>
        public Utilities Utilities;
        /// <summary>
        /// site_id
        /// </summary>
        public object site_id = DBNull.Value;
        /// <summary>
        /// RefreshFromHQTime
        /// </summary>
        public DateTime RefreshFromHQTime;
        /// <summary>
        /// primaryCard
        /// </summary>
        public string primaryCard;
        /// <summary>
        /// ReaderDevice
        /// </summary>
        public DeviceClass ReaderDevice;

        private String guid = "";

        /// <summary>
        /// CardGuid
        /// </summary>
        public string CardGuid { get { return guid; } }
        internal string SetCardGuid { set { guid = value; } }
        /// <summary>
        /// Entitlements
        /// </summary>
        public class Entitlement
        {
            /// <summary>  field </summary>
            public byte EntType;
            /// <summary>  field </summary>
            public byte IdType;
            /// <summary>  field </summary>
            public byte UserIdentifier;
            /// <summary>  field </summary>
            public UInt16 EntCount;
            /// <summary>  field </summary>
            public DateTime ExpiryTime;
        }
        /// <summary>
        /// Entitlements
        /// </summary>
        public List<Entitlement> Entitlements = new List<Entitlement>();

        /// <summary>
        /// Card
        /// </summary> 
        public Card(Utilities ParafaitUtilities)
        {
            log.LogMethodEntry(ParafaitUtilities);

            Utilities = ParafaitUtilities;
            Init();

            log.LogMethodExit(null);
        }

        /// <summary>
        /// Card
        /// </summary> 
        public Card(DeviceClass readerDevice, Utilities ParafaitUtilities)
            : this(ParafaitUtilities)
        {
            log.LogMethodEntry(readerDevice, ParafaitUtilities);

            ReaderDevice = readerDevice;

            log.LogMethodExit(null);
        }

        /// <summary>
        /// Card
        /// </summary> 
        public Card(string cardNumber, string ploginId, Utilities ParafaitUtilities)
            : this(ParafaitUtilities)
        {
            log.LogMethodEntry(cardNumber, ploginId, ParafaitUtilities);

            CardNumber = cardNumber;
            loginId = ploginId;
            getCardDetails(CardNumber);

            log.LogMethodExit(null);
        }
        /// <summary>
        /// Card
        /// </summary> 
        public Card(string cardNumber, string ploginId, Utilities ParafaitUtilities, SqlTransaction sqlTrx = null)
            : this(ParafaitUtilities)
        {
            log.LogMethodEntry(cardNumber, ploginId, ParafaitUtilities, sqlTrx);

            CardNumber = cardNumber;
            loginId = ploginId;
            getCardDetails(CardNumber, -1, sqlTrx);

            log.LogMethodExit(null);
        }
        /// <summary>
        /// Card
        /// </summary> 
        public Card(DeviceClass readerDevice, string cardNumber, string ploginId, Utilities ParafaitUtilities)
            : this(readerDevice, ParafaitUtilities)
        {
            log.LogMethodEntry(readerDevice, cardNumber, ploginId, ParafaitUtilities);

            CardNumber = cardNumber;
            loginId = ploginId;
            getCardDetails(CardNumber);

            log.LogMethodExit(null);
        }
        /// <summary>
        /// Card
        /// </summary> 
        public Card(int CardId, string ploginId, Utilities ParafaitUtilities, SqlTransaction SQLTrx = null)
            : this(ParafaitUtilities)
        {
            log.LogMethodEntry(CardId, ploginId, ParafaitUtilities);

            loginId = ploginId;
            getCardDetails(CardId, SQLTrx);

            log.LogMethodExit(null);
        }
        /// <summary>
        /// Card
        /// </summary> 
        public Card(DeviceClass readerDevice, int CardId, string ploginId, Utilities ParafaitUtilities)
            : this(readerDevice, ParafaitUtilities)
        {
            log.LogMethodEntry(readerDevice, CardId, ploginId, ParafaitUtilities);

            loginId = ploginId;
            getCardDetails(CardId);

            log.LogMethodExit(null);
        }

        void Init()
        {
            log.LogMethodEntry();

            real_ticket_mode = Utilities.ParafaitEnv.REAL_TICKET_MODE;

            log.LogMethodExit(null);
        }
        /// <summary>
        /// getCardDetails
        /// </summary>
        /// <param name="CardId"></param>
        /// <param name="SQLTrx"></param>
        public virtual void getCardDetails(int CardId, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(CardId, SQLTrx);

            getCardDetails("", CardId, SQLTrx);

            log.LogMethodExit(null);
        }
        /// <summary>
        /// getCardDetails
        /// </summary>
        /// <param name="cardNumber"></param>
        public virtual void getCardDetails(string cardNumber)
        {
            log.LogMethodEntry(cardNumber);

            getCardDetails(cardNumber, -1);

            log.LogMethodExit(null);
        }
        /// <summary>
        /// getCardDetails
        /// </summary>
        public virtual void getCardDetails(string cardNumber, int cardId, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(cardNumber, cardId, SQLTrx);

            DataTable DT;
            if (cardId == -1)
            {
                int bonusDays = 0;
                if (Utilities.ParafaitEnv.REACTIVATE_EXPIRED_CARD)
                {
                    bonusDays = ParafaitDefaultContainerList.GetParafaitDefault<int>(Utilities.ExecutionContext, "CARD_EXPIRY_GRACE_PERIOD", 0);
                }
                string CommandText = @"select c.*, getdate() as sysdate 
                                                        --from CardView c left outer join cardType ct on ct.CardTypeId = c.CardTypeId 
                                                          from CardView c left outer join Membership m on m.membershipId = c.membershipId
                                                        where card_number = @card_number 
                                                          and (valid_flag = 'Y' and (ExpiryDate is null or ExpiryDate > getdate())
                                                                or (@reactivateExpired = 1 
                                                                    and Refund_flag = 'N'
                                                                    and case when  @ExpireAfterMonths = -1  then DATEADD(day,@bonusdays*-1,ExpiryDate)  else ExpiryDate  end < getdate()
                                                                    and card_id = (select max(card_id) 
                                                                                     from cards ce 
                                                                                    where ce.card_number = c.card_number)))
                                                        order by issue_date desc";
                DT = Utilities.executeDataTable(CommandText, SQLTrx,
                                                new SqlParameter("@bonusdays", bonusDays),
                                                new SqlParameter("@ExpireAfterMonths", (Utilities.ParafaitEnv.CARD_EXPIRY_RULE == "ISSUEDATE" ? Utilities.ParafaitEnv.CARD_VALIDITY : -1)),
                                                new SqlParameter("@card_number", cardNumber),
                                                new SqlParameter("@reactivateExpired", Utilities.ParafaitEnv.REACTIVATE_EXPIRED_CARD));

                log.LogVariableState("@bonusdays", bonusDays);
                log.LogVariableState("@ExpireAfterMonths", (Utilities.ParafaitEnv.CARD_EXPIRY_RULE == "ISSUEDATE" ? Utilities.ParafaitEnv.CARD_VALIDITY : -1));
                log.LogVariableState("@card_number", cardNumber);
                log.LogVariableState("@reactivateExpired", Utilities.ParafaitEnv.REACTIVATE_EXPIRED_CARD);

            }
            else
            {
                string CommandText = @"select c.*, getdate() as sysdate 
                                    --from CardView c left outer join cardType ct on ct.CardTypeId = c.CardTypeId 
                                      from CardView c left outer join Membership m on m.membershipId = c.membershipId
                                    where card_id = @card_id
                                    order by issue_date desc";
                DT = Utilities.executeDataTable(CommandText, SQLTrx, new SqlParameter("@card_id", cardId));

                log.LogVariableState("@card_id", cardId);
            }

            if (DT.Rows.Count == 0)
            {
                CardStatus = "NEW";
                issue_date = ServerDateTime.Now;
            }
            else
            {
                if (DT.Rows[0]["ExpiryDate"] != DBNull.Value)
                    ExpiryDate = (DateTime)DT.Rows[0]["ExpiryDate"];
                else
                    ExpiryDate = DateTime.MinValue;

                if (ExpiryDate.Equals(DateTime.MinValue) || ExpiryDate > Convert.ToDateTime(DT.Rows[0]["sysdate"]))
                    CardStatus = "ISSUED";
                else
                    CardStatus = "EXPIRED";

                CardNumber = DT.Rows[0]["card_number"].ToString();
                card_id = Convert.ToInt32(DT.Rows[0]["card_id"]);

                if (DT.Rows[0]["site_id"] != DBNull.Value)
                    siteId = Convert.ToInt32(DT.Rows[0]["site_id"]);
                else
                    siteId = -1;

                issue_date = (DateTime)DT.Rows[0]["issue_date"];

                if (DT.Rows[0]["face_value"] != DBNull.Value)
                    face_value = (float)Convert.ToDouble(DT.Rows[0]["face_value"]);

                refund_flag = DT.Rows[0]["refund_flag"].ToString()[0];
                if (DT.Rows[0]["refund_amount"] == DBNull.Value)
                    refund_amount = -1;
                else
                    refund_amount = (float)Convert.ToDouble(DT.Rows[0]["refund_amount"]);
                if (DT.Rows[0]["refund_date"] == DBNull.Value)
                    refund_date = DateTime.MinValue;
                else
                    refund_date = (DateTime)DT.Rows[0]["refund_date"];

                valid_flag = (char)DT.Rows[0]["valid_flag"].ToString()[0];
                real_ticket_mode = (char)DT.Rows[0]["real_ticket_mode"].ToString()[0];
                vip_customer = (char)DT.Rows[0]["vip_customer"].ToString()[0];
                ticket_allowed = (char)DT.Rows[0]["ticket_allowed"].ToString()[0];

                technician_card = (DT.Rows[0]["technician_card"] == DBNull.Value ? 'N' : DT.Rows[0]["technician_card"].ToString()[0]);

                if (DT.Rows[0]["ticket_count"] != DBNull.Value)
                    ticket_count = Convert.ToInt32(DT.Rows[0]["ticket_count"]);

                notes = DT.Rows[0]["notes"].ToString();

                if (DT.Rows[0]["last_update_time"] != DBNull.Value)
                    last_update_time = (DateTime)DT.Rows[0]["last_update_time"];
                else
                    last_update_time = DateTime.MinValue;

                if (DT.Rows[0]["last_played_time"] != DBNull.Value)
                    last_played_time = (DateTime)DT.Rows[0]["last_played_time"];
                else
                    last_played_time = DateTime.MinValue;

                if (DT.Rows[0]["start_time"] != DBNull.Value)
                    start_time = (DateTime)DT.Rows[0]["start_time"];
                else
                    start_time = DateTime.MinValue;

                if (DT.Rows[0]["RefreshFromHQTime"] != DBNull.Value)
                    RefreshFromHQTime = (DateTime)DT.Rows[0]["RefreshFromHQTime"];
                else
                    RefreshFromHQTime = DateTime.MinValue;

                if (DT.Rows[0]["credits"] != DBNull.Value)
                    credits = Convert.ToDouble(DT.Rows[0]["credits"]);

                if (DT.Rows[0]["courtesy"] != DBNull.Value)
                    courtesy = Convert.ToDouble(DT.Rows[0]["courtesy"]);

                if (DT.Rows[0]["bonus"] != DBNull.Value)
                    bonus = Convert.ToDouble(DT.Rows[0]["bonus"]);

                if (DT.Rows[0]["time"] != DBNull.Value)
                {
                    time = Convert.ToDouble(DT.Rows[0]["time"]);
                    if (time > 0)
                    {
                        if (start_time != DateTime.MinValue)
                        {
                            TimeSpan ts = start_time.AddMinutes(time) - Convert.ToDateTime(DT.Rows[0]["sysdate"]);

                            double balanceTime = ts.TotalSeconds;
                            if (balanceTime <= 0)
                                time = 0;
                            else
                                time = Math.Floor(balanceTime / 60) + (balanceTime % 60) / 100;
                        }
                    }
                }

                if (DT.Rows[0]["guid"] == DBNull.Value)
                    guid = "";
                else
                    guid = Convert.ToString(DT.Rows[0]["guid"]);

                string creditPluscmd = @"select isnull(sum(TimeBalance), 0) from 
                                                (select case when PlayStartTime is null 
                                                        then CreditPlusBalance 
	                                                    else case when TimeTo is null 
		                                                    then datediff(MI, getdate(), dateadd(MI, CreditPlusBalance, PlayStartTime))
		                                                    else datediff(MI, getdate(), 
			                                                (select min(endTime) 
				                                                from (select DATEADD(MI, (timeto - cast(timeto as integer))*100, DateAdd(HH, case timeto when 0 then 24 else timeto end, dateadd(D, 0, datediff(D, 0, getdate())))) endTime 
						                                                union all 
					                                                  select dateadd(MI, CreditPlusBalance, PlayStartTime)) as v)) 
		                                                    end
	                                                    end TimeBalance
                                                from CardCreditPlus cp
                                                where cp.card_id = @card_id
								                AND isnull(validityStatus, 'Y') != 'H' 
                                                and CreditPlusType in ('M')
                                                and (case when PlayStartTime is null then getdate() else dateadd(MI, CreditPlusBalance, PlayStartTime) end) >= getdate()
                                                and (cp.PeriodFrom is null or cp.PeriodFrom <= getdate()) 
                                                and (cp.PeriodTo is null or cp.PeriodTo > getdate())) v";
                CreditPlusTime = Math.Max(0, Convert.ToInt32(Utilities.executeScalar(creditPluscmd, SQLTrx, new SqlParameter("@card_id", card_id)))); //Changed time to CreditPlusTime

                log.LogVariableState("@card_id", card_id);

                if (DT.Rows[0]["customer_id"] == DBNull.Value)
                    customer_id = -1;
                else
                {
                    customer_id = Convert.ToInt32(DT.Rows[0]["customer_id"]);
                    customerDTO = (new CustomerBL(Utilities.ExecutionContext, customer_id, true, true, SQLTrx)).CustomerDTO;
                }

                if (DT.Rows[0]["credits_played"] != DBNull.Value)
                    credits_played = Convert.ToDouble(DT.Rows[0]["credits_played"]);

                if (DT.Rows[0]["loyalty_points"] != DBNull.Value)
                    loyalty_points = Convert.ToDouble(DT.Rows[0]["loyalty_points"]);

                if (DT.Rows[0]["tech_games"] != DBNull.Value)
                    tech_games = Convert.ToInt32(DT.Rows[0]["tech_games"]);

                if (DT.Rows[0]["MembershipId"] != DBNull.Value)
                {
                    MembershipId = Convert.ToInt32(DT.Rows[0]["MembershipId"]);
                    MembershipName = DT.Rows[0]["MembershipName"].ToString();
                }
                else
                {
                    MembershipId = -1;
                    MembershipName = Utilities.MessageUtils.getMessage("Normal");
                }

                if (DT.Rows[0]["PrimaryCard"] != DBNull.Value)
                    primaryCard = DT.Rows[0]["PrimaryCard"].ToString();

                TotalCreditPlusLoyaltyPoints = Convert.ToDouble(DT.Rows[0]["CreditPlusLoyaltyPoints"]);
                RedeemableCreditPlusLoyaltyPoints = Convert.ToDouble(DT.Rows[0]["RedeemableCreditPlusLoyaltyPoints"]);
                CreditPlusVirtualPoints = Convert.ToDouble(DT.Rows[0]["CreditPlusVirtualPoints"]);
                CreditPlusTickets = Convert.ToInt32(DT.Rows[0]["CreditPlusTickets"]);
                CreditPlusCardBalance = Convert.ToDouble(DT.Rows[0]["CreditPlusCardBalance"] == DBNull.Value ? 0 : DT.Rows[0]["CreditPlusCardBalance"]);
                CreditPlusCredits = Convert.ToDouble(DT.Rows[0]["CreditPlusCredits"]);
                CreditPlusBonus = Convert.ToDouble(DT.Rows[0]["CreditPlusBonus"]);
                creditPlusItemPurchase = Convert.ToDouble(DT.Rows[0]["creditPlusItemPurchase"]);
            }

            log.LogMethodExit(null);
        }
        /// <summary>
        /// createCard
        /// </summary>
        /// <param name="SQLTrx"></param>
        public virtual void createCard(SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(SQLTrx);

            SqlCommand cardCmd = Utilities.getCommand(SQLTrx);

            //cardCmd.CommandText = "select top 1 1 from cards where card_number = @cardNumber and valid_flag ='Y'";
            //cardCmd.Parameters.AddWithValue("@cardNumber", CardNumber);
            //if (cardCmd.ExecuteScalar() != null)
            //{
            //    log.LogMethodExit(null, "Throwing ApplicationException - Card " + CardNumber + " already exists.");
            //    throw new ApplicationException("Card " + CardNumber + " already exists.");
            //}

            cardCmd.CommandText = @"insert into cards (card_number, issue_date, face_value, refund_flag, refund_date, refund_amount, valid_flag, ticket_count, notes,
                                                       LastUpdatedBy, last_update_time, credits, courtesy, bonus, time, customer_id, ticket_allowed, credits_played, loyalty_points,
                                                       start_time, last_played_time, vip_customer, technician_card, real_ticket_mode, ExpiryDate, PrimaryCard, Site_id)  
                                        SELECT * FROM (SELECT  @card_number as card_number, getdate() as issue_date, @face_value as face_value, @refund_flag as refund_flag, 
                                                        @refund_date as refund_date, @refund_amount as refund_amount, @valid_flag as valid_flag, @ticket_count as ticket_count,
                                                        @notes as notes, @LastUpdatedBy as LastUpdatedBy, getdate() as last_update_time, @credits as credits, @courtesy as courtesy,
                                                    @bonus as bonus, @time as time, @customer_id as customer_id, @ticket_allowed as ticket_allowed, @credits_played as credits_played,          @loyalty_points as loyalty_points, @start_time as start_time, @last_played_time as last_played_time, 
                                                        @vip_customer as vip_customer, @technician_card as technician_card, @real_ticket_mode as real_ticket_mode,
                                                        @ExpireAfterMonths as ExpireAfterMonths, @PrimaryCard as PrimaryCard, @Site_id as Site_id) AS a 
                                        WHERE NOT EXISTS (SELECT 1 FROM cards WHERE card_number = a.card_number and valid_flag='Y');
                                     select ISNULL(@@identity,-1);";

            cardCmd.Parameters.Clear();
            cardCmd.Parameters.AddWithValue("@card_number", CardNumber);
            cardCmd.Parameters.AddWithValue("@face_value", face_value);
            cardCmd.Parameters.AddWithValue("@refund_flag", "N");
            cardCmd.Parameters.AddWithValue("@refund_date", DBNull.Value);
            cardCmd.Parameters.AddWithValue("@refund_amount", DBNull.Value);
            cardCmd.Parameters.AddWithValue("@valid_flag", "Y");
            cardCmd.Parameters.AddWithValue("@ticket_count", addTicketCount);
            cardCmd.Parameters.AddWithValue("@notes", notes);
            cardCmd.Parameters.AddWithValue("@LastUpdatedBy", loginId);
            cardCmd.Parameters.AddWithValue("@credits", addCredits);
            cardCmd.Parameters.AddWithValue("@courtesy", addCourtesy);
            cardCmd.Parameters.AddWithValue("@bonus", addBonus);
            cardCmd.Parameters.AddWithValue("@time", addTime);
            cardCmd.Parameters.AddWithValue("@ticket_allowed", ticket_allowed);
            cardCmd.Parameters.AddWithValue("@real_ticket_mode", real_ticket_mode);
            cardCmd.Parameters.AddWithValue("@technician_card", technician_card);

            log.LogVariableState("@card_number", CardNumber);
            log.LogVariableState("@face_value", face_value);
            log.LogVariableState("@refund_flag", "N");
            log.LogVariableState("@refund_date", DBNull.Value);
            log.LogVariableState("@refund_amount", DBNull.Value);
            log.LogVariableState("@valid_flag", "Y");
            log.LogVariableState("@ticket_count", addTicketCount);
            log.LogVariableState("@notes", notes);
            log.LogVariableState("@LastUpdatedBy", loginId);
            log.LogVariableState("@credits", addCredits);
            log.LogVariableState("@courtesy", addCourtesy);
            log.LogVariableState("@bonus", addBonus);
            log.LogVariableState("@time", addTime);
            log.LogVariableState("@ticket_allowed", ticket_allowed);
            log.LogVariableState("@real_ticket_mode", real_ticket_mode);
            log.LogVariableState("@technician_card", technician_card);

            int savCustomerId = customer_id;
            if (customerDTO != null && customerDTO.CustomerType == CustomerType.UNREGISTERED)
            {
                log.Info("Converting customer with id : " + customerDTO.Id + " from unregistered to registered");
                customerDTO.CustomerType = CustomerType.REGISTERED;
            }
            if (customerDTO != null && customerDTO.IsChangedRecursive)
            {
                CustomerBL customerBL = new CustomerBL(Utilities.ExecutionContext, customerDTO);
                customerBL.Save(SQLTrx);
            }
            if (customerDTO != null)
            {
                customer_id = customerDTO.Id;
            }

            TransactionUtils transactionUtils = new TransactionUtils(Utilities);
            transactionUtils.VIPUpgrade(this, SQLTrx);
            cardCmd.Parameters.AddWithValue("@vip_customer", vip_customer);

            log.LogVariableState("@vip_customer", vip_customer);

            if (customer_id == -1)
            {
                cardCmd.Parameters.AddWithValue("@customer_id", DBNull.Value);
                log.LogVariableState("@customer_id", DBNull.Value);
            }
            else
            {
                cardCmd.Parameters.AddWithValue("@customer_id", customer_id);
                log.LogVariableState("@customer_id", customer_id);
            }

            cardCmd.Parameters.AddWithValue("@credits_played", credits_played);
            cardCmd.Parameters.AddWithValue("@loyalty_points", loyalty_points);

            log.LogVariableState("@credits_played", credits_played);
            log.LogVariableState("@loyalty_points", loyalty_points);

            if (start_time != DateTime.MinValue)
            {
                cardCmd.Parameters.AddWithValue("@start_time", start_time);
                log.LogVariableState("@start_time", start_time);
            }
            else
            {
                cardCmd.Parameters.AddWithValue("@start_time", DBNull.Value);
                log.LogVariableState("@start_time", DBNull.Value);
            }

            if (last_played_time != DateTime.MinValue)
            {
                cardCmd.Parameters.AddWithValue("@last_played_time", last_played_time);
                log.LogVariableState("@last_played_time", last_played_time);
            }
            else
            {
                cardCmd.Parameters.AddWithValue("@last_played_time", DBNull.Value);
                log.LogVariableState("@last_played_time", DBNull.Value);
            }

            cardCmd.Parameters.Add("@ExpireAfterMonths", SqlDbType.DateTime);
            log.LogVariableState("@ExpireAfterMonths", SqlDbType.DateTime);

            //Begin: Added to check if Card Expiry was previously based on the product setup on Dec-01-2015//
            if (ExpiryDate != DateTime.MinValue)
            {
                cardCmd.Parameters["@ExpireAfterMonths"].Value = ExpiryDate;
            }
            else
            {
                if (Utilities.ParafaitEnv.CARD_EXPIRY_RULE == "ISSUEDATE")
                {
                    cardCmd.Parameters["@ExpireAfterMonths"].Value = Utilities.getServerTime().AddMonths(Utilities.ParafaitEnv.CARD_VALIDITY);
                }
                else
                    cardCmd.Parameters["@ExpireAfterMonths"].Value = DBNull.Value;
            }

            if (!String.IsNullOrEmpty(primaryCard) && primaryCard == "Y")
            {
                cardCmd.Parameters.AddWithValue("@PrimaryCard", primaryCard);
                log.LogVariableState("@PrimaryCard", primaryCard);
            }
            else
            {
                cardCmd.Parameters.AddWithValue("@PrimaryCard", DBNull.Value);
                log.LogVariableState("@PrimaryCard", DBNull.Value);
            }
            //End//
            //Begin Modification - Added site id for HQ synch - 01-Oct-2015
            if (Utilities.ParafaitEnv.IsCorporate == false || Utilities.ParafaitEnv.SiteId <= 0)
                site_id = DBNull.Value;
            else
                site_id = Utilities.ParafaitEnv.SiteId;
            cardCmd.Parameters.AddWithValue("@Site_id", site_id);
            //End Modification - Added site id for HQ synch - 01-Oct-2015

            card_id = Convert.ToInt32(cardCmd.ExecuteScalar());
            if (card_id == -1)
            {
                log.LogMethodExit(null, "Throwing ApplicationException - Card " + CardNumber + " already exists.");
                throw new ApplicationException("Card " + CardNumber + " already exists.");
            }
            CardStatus = "ISSUED";//Mark card status issued
            if (savCustomerId == -1 && customer_id > 0)
                createRegistrationEntitlements(SQLTrx);


            log.LogMethodExit(null);
        }
        /// <summary>
        /// updateCardTime
        /// </summary>
        /// <param name="sqlTrx"></param>
        public virtual void updateCardTime(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            Utilities.executeScalar(@"update cards 
                                         set last_update_time = getdate(),
                                             LastUpdatedBy = @LastUpdatedBy
                                       where card_id = @cardId", sqlTrx,
                                       new SqlParameter("@cardId", card_id),
                                       new SqlParameter("@LastUpdatedBy", Utilities.ParafaitEnv.LoginID));
            log.LogVariableState("cardId", card_id);
            log.LogMethodExit();
        }
        /// <summary>
        /// rechargeCard
        /// </summary>
        /// <param name="SQLTrx"></param>
        public virtual void rechargeCard(SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(SQLTrx);

            SqlCommand cardCmd = Utilities.getCommand(SQLTrx);
            SqlCommand trxCmd = Utilities.getCommand(SQLTrx);

            cardCmd.CommandText = "update cards set " +
                                    "last_update_time = getdate(), " +
                                    "credits = isnull(credits, 0) + @credits, " +
                                    "courtesy = isnull(courtesy, 0) +  @courtesy, " +
                                    "bonus = isnull(bonus, 0) + @bonus, " +
                                    "ticket_count = isnull(ticket_count, 0) + @ticket_count, " +
                                    // load fresh time slot if existing time slot has expired
                                    "time = case when start_time is null then time + @time " +
                                                "when datediff(\"ss\", getdate(), dateadd(\"ss\", time*60, start_time)) <= 0 then @time " +
                                                "else floor(datediff(\"ss\", getdate(), dateadd(\"ss\", time*60, start_time)) / 60) + (datediff(\"ss\", getdate(), dateadd(\"ss\", time*60, start_time)) % 60) / 100.0 + @time end, " +
                                    // make start_time null if current slot has expired
                                    "start_time = null, " +
                                    "vip_customer = @vip_customer, " +
                                    "notes = @notes, " +
                                    // "CardTypeId = @CardTypeId, " +
                                    "LastUpdatedBy = @LastUpdatedBy, " +
                                    "technician_card = @techCard, " +
                                    "valid_flag = 'Y', " +
                                    // "RefreshFromHQTime = getdate(), " + // 2.130.1
                                    "ExpiryDate = case when @ExpireAfterMonths = -1 then null else ExpiryDate end " +
                                    "where card_id = @card_id";

            cardCmd.Parameters.AddWithValue("@card_id", card_id);
            cardCmd.Parameters.AddWithValue("@credits", addCredits);
            cardCmd.Parameters.AddWithValue("@courtesy", addCourtesy);
            cardCmd.Parameters.AddWithValue("@bonus", addBonus);
            cardCmd.Parameters.AddWithValue("@time", addTime);
            cardCmd.Parameters.AddWithValue("@ticket_count", addTicketCount);
            cardCmd.Parameters.AddWithValue("@notes", notes);
            cardCmd.Parameters.AddWithValue("@LastUpdatedBy", loginId);
            cardCmd.Parameters.AddWithValue("@techCard", technician_card);

            log.LogVariableState("@card_id", card_id);
            log.LogVariableState("@credits", addCredits);
            log.LogVariableState("@courtesy", addCourtesy);
            log.LogVariableState("@bonus", addBonus);
            log.LogVariableState("@time", addTime);
            log.LogVariableState("@ticket_count", addTicketCount);
            log.LogVariableState("@notes", notes);
            log.LogVariableState("@LastUpdatedBy", loginId);
            log.LogVariableState("@techCard", technician_card);


            //the following section of code is added on-19-08-2015
            cardCmd.Parameters.Add("@ExpireAfterMonths", SqlDbType.Int);
            if (Utilities.ParafaitEnv.CARD_EXPIRY_RULE == "ISSUEDATE")
                cardCmd.Parameters["@ExpireAfterMonths"].Value = Utilities.ParafaitEnv.CARD_VALIDITY;
            else
            {
                if (Utilities.ParafaitEnv.REACTIVATE_EXPIRED_CARD)
                    cardCmd.Parameters["@ExpireAfterMonths"].Value = -1;
                else
                    cardCmd.Parameters["@ExpireAfterMonths"].Value = 0;//default value so that expirydate is not updated
            }
            //-changes on 19-08-2015 end - Modified to allow product level card validity to take priority
            //Reactive expire card configuration should not be enabled if product level card validity is set

            getTotalRechargeAmount(trxCmd);

            //int currentCardTypeId = CardTypeId;
            TransactionUtils TransactionUtils = new TransactionUtils(Utilities);
            TransactionUtils.VIPUpgrade(this, SQLTrx);
            cardCmd.Parameters.AddWithValue("@vip_customer", vip_customer);

            cardCmd.ExecuteNonQuery();

            if (this.siteId != this.Utilities.ExecutionContext.GetSiteId())
            {
                DBSynchLogDTO dbSynchLogDTO = new DBSynchLogDTO("I", this.guid, "Cards", Utilities.getServerTime(), Utilities.ExecutionContext.GetSiteId());
                DBSynchLogBL dbSynchLogBL = new DBSynchLogBL(Utilities.ExecutionContext, dbSynchLogDTO);
                dbSynchLogBL.Save(SQLTrx);
            }

            log.LogMethodExit(null);
        }

        /// <summary>
        /// getTotalRechargeAmount
        /// </summary>
        public virtual void getTotalRechargeAmount()
        {
            log.LogMethodEntry();

            using (SqlConnection cnn = Utilities.createConnection())
            {
                using (SqlCommand cmd = new SqlCommand("", cnn))
                {
                    TotalRechargeAmount = 0;
                    getTotalRechargeAmount(cmd);
                }
            }

            log.LogMethodExit(null);
        }

        void getTotalRechargeAmount(SqlCommand cmd)
        {
            log.LogMethodEntry(cmd);

            if (card_id < 0)
            {
                log.LogMethodExit(null);
                return;
            }

            //Changed the query on 29/10/2015 to the below query-to exclude check-in & check out recharges from total recharges
            //cmd.CommandText = "select isnull(sum(amount), 0) from trx_lines where card_id = @card_id";
            cmd.CommandText = @" Select isnull(sum(amount), 0) from trx_lines tl,trx_header th                                                               
                                  where th.trxid = tl.TrxId and not exists(select 1 from OrderHeader oh ,TransactionOrderType tot 
                                                                           where th.OrderId = oh.OrderId and tot.Name='Item Refund' and tot.Id = oh.TransactionOrderTypeId) 
                                       and tl.card_id = @card_id 
                                       and not exists(select  product_id from Products p,product_type pt 
                                                          where p.product_id=tl.product_id and p.product_type_id=pt.product_type_id 
                                                          and product_type in ('CHECK-IN','CHECK-OUT', 'EXTERNAL POS') 
	                              )";
            cmd.Parameters.AddWithValue("@card_id", card_id);
            TotalRechargeAmount += Convert.ToDouble(cmd.ExecuteScalar());

            log.LogVariableState("@card_id", card_id);
            log.LogMethodExit(null);
        }

        /// <summary>
        /// invalidateCard
        /// </summary>
        /// <param name="SQLTrx"></param>
        public virtual void invalidateCard(SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(SQLTrx);

            SqlCommand cardCmd = Utilities.getCommand(SQLTrx);

            cardCmd.CommandText = "update cards set " +
                                    "valid_flag = 'N', " +
                                    "last_update_time = getdate(), " +
                                    "LastUpdatedBy = @LastUpdatedBy " +
                                    "where card_id = @card_id";

            cardCmd.Parameters.AddWithValue("@card_id", card_id);
            cardCmd.Parameters.AddWithValue("@LastUpdatedBy", loginId);
            cardCmd.ExecuteNonQuery();

            log.LogVariableState("@card_id", card_id);
            log.LogVariableState("@LastUpdatedBy", loginId);

            face_value = 0;
            credits_played = 0;
            credits = courtesy = bonus = time = CreditPlusCredits = CreditPlusCardBalance = CreditPlusTime = creditPlusItemPurchase = CardGames = 0;

            log.LogMethodExit(null);
        }
        /// <summary>
        /// ExpireCard
        /// </summary>
        /// <param name="SQLTrx"></param>
        public virtual void ExpireCard(SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(SQLTrx);

            SqlCommand cardCmd = Utilities.getCommand(SQLTrx);
            //CardActivityLogBL cardActivityLogBL = new CardActivityLogBL(Utilities.ExecutionContext);
            //cardActivityLogBL.FetchCPBalanceAndSave("CPDEDUCTION", "REFUND CARD CP", -1, card_id, "", loginId, Utilities.ParafaitEnv.POSMachineId, "", SQLTrx);

            cardCmd.CommandText = "update cards set credits = 0, courtesy = 0, bonus = 0, time = 0, " +
                                    "last_update_time = getdate(), " +
                                    "LastUpdatedBy = @LastUpdatedBy " +
                                    "where card_id = @card_id; " +
                                    "update cardGames set expiryDate = getdate() where card_Id = @card_id; " +
                                    "update cardCreditPlus set CreditPlusBalance = 0 where card_Id = @card_id; ";
            //    "INSERT INTO dbo.CardCreditPlusLog (CardCreditPlusId, CreditPlus, CreditPlusType, CreditPlusBalance, PlayStartTime, CreatedBy, CreationDate, LastUpdatedBy, LastupdatedDate, site_id, SynchStatus, MasterEntityId) "+
            //    " (SELECT CardCreditPlusId, CreditPlus, CreditPlusType, CreditPlusBalance, PlayStartTime, @LastUpdatedBy, GETDATE(), @LastUpdatedBy, GETDATE(), site_id, SynchStatus, MasterEntityId from CardCreditPlus where Card_id = @card_id ); ";

            cardCmd.Parameters.AddWithValue("@card_id", card_id);
            cardCmd.Parameters.AddWithValue("@LastUpdatedBy", loginId);
            cardCmd.ExecuteNonQuery();

            log.LogVariableState("@card_id", card_id);
            log.LogVariableState("@LastUpdatedBy", loginId);

            credits = courtesy = bonus = time = CreditPlusCredits = CreditPlusTime = CreditPlusCardBalance = creditPlusItemPurchase = CardGames = 0;

            log.LogVariableState("credits", credits);

            log.LogMethodExit(null);
        }
        /// <summary>
        /// updateCustomer
        /// </summary>
        /// <param name="inSQLTrx"></param>
        public virtual void updateCustomer(SqlTransaction inSQLTrx = null)
        {
            log.LogMethodEntry(inSQLTrx);

            if (customerDTO == null || customerDTO.Id == -1)
            {
                log.LogMethodExit(null);
                return;
            }

            SqlConnection cnn = null;
            SqlTransaction SQLTrx;

            if (inSQLTrx == null)
            {
                cnn = Utilities.createConnection();
                SQLTrx = cnn.BeginTransaction();
            }
            else
                SQLTrx = inSQLTrx;

            if (customerDTO.CustomerType == CustomerType.UNREGISTERED)
            {
                customerDTO.CustomerType = CustomerType.REGISTERED;
                CustomerBL customerBL = new CustomerBL(Utilities.ExecutionContext, customerDTO);
                customerBL.Save(SQLTrx);
            }

            int savCustomerId = customer_id;

            customer_id = customerDTO.Id;



            SqlCommand cardCmd = Utilities.getCommand(SQLTrx);


            cardCmd.CommandText = "update cards set " +
                                    "last_update_time = getdate(), " +
                                    "customer_id = @customer_id, " +
                                    "LastUpdatedBy = @LastUpdatedBy " +
                                    "where card_id = @card_id";

            cardCmd.Parameters.AddWithValue("@card_id", card_id);
            log.LogVariableState("@card_id", card_id);

            cardCmd.Parameters.AddWithValue("@customer_id", customer_id);
            log.LogVariableState("@customer_id", customer_id);

            cardCmd.Parameters.AddWithValue("@LastUpdatedBy", loginId);
            log.LogVariableState("@LastUpdatedBy", loginId);

            try
            {
                cardCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                log.Error("Unable to execute Query for Card Command", ex);
                if (inSQLTrx == null)
                {
                    SQLTrx.Rollback();
                    cnn.Close();
                }
                log.LogMethodExit(null, "Throwing Exception " + ex);
                throw ex;
            }

            if (savCustomerId == -1 && technician_card == 'N') // first time registration
                createRegistrationEntitlements(SQLTrx);

            cardCmd.CommandText = "select isnull(sum(amount), 0) from trx_lines where card_id = @card_id";

            TotalRechargeAmount = Convert.ToDouble(cardCmd.ExecuteScalar());

            //int currentCardTypeId = CardTypeId;
            TransactionUtils TransactionUtils = new TransactionUtils(Utilities);
            TransactionUtils.VIPUpgrade(this, SQLTrx);

            cardCmd.CommandText = "update cards set " +
                                    "last_update_time = getdate(), " +
                                    "vip_customer = @vip_customer, " +
                                    "LastUpdatedBy = @LastUpdatedBy " +
                                    "where card_id = @card_id";

            cardCmd.Parameters.AddWithValue("@vip_customer", vip_customer);
            log.LogVariableState("@vip_customer", vip_customer);

            try
            {
                cardCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                log.Error("Unable to execute Query for Card Command", ex);
                if (inSQLTrx == null)
                {
                    SQLTrx.Rollback();
                    cnn.Close();
                }
                log.LogMethodExit(null, "Throwing Exception " + ex);
                throw ex;
            }

            if (inSQLTrx == null)
            {
                SQLTrx.Commit();
                cnn.Close();
            }
            log.LogMethodExit(null);
        }
        /// <summary>
        /// refund_MCard
        /// </summary> 
        /// <returns></returns>
        public virtual bool refund_MCard(ref string message)
        {
            log.LogMethodEntry(message);

            log.LogVariableState("Message ", message);
            log.LogMethodExit(true);
            return true;
        }
        /// <summary>
        /// setChildSiteCode
        /// </summary> 
        /// <returns></returns>
        public virtual bool setChildSiteCode(ref byte[] purseDataBuffer, int siteCode, ref string message)
        {
            log.LogMethodEntry(purseDataBuffer, siteCode, message);

            log.LogVariableState("Message ", message);
            log.LogMethodExit(false);
            return false;
        }
        /// <summary>
        /// getChildSiteCode
        /// </summary> 
        /// <returns></returns>
        public virtual int getChildSiteCode(ref byte[] purseDataBuffer, ref string message)
        {
            log.LogMethodEntry(purseDataBuffer, message);

            log.LogVariableState("Message ", message);
            log.LogMethodExit(-1);
            return -1;
        }

        /// <summary>
        /// checkCardExists
        /// </summary> 
        /// <returns></returns>
        public virtual bool checkCardExists()
        {
            log.LogMethodEntry();
            log.LogMethodExit(false);
            return false;
        }
        /// <summary>
        /// getPlayDetails
        /// </summary> 
        /// <returns></returns>
        public virtual string getPlayDetails(int gamePlayNumber, ref int siteCode, ref int machineId, ref double startingBalance, ref double endingBalance)
        {
            log.LogMethodEntry(gamePlayNumber, siteCode, machineId, startingBalance, endingBalance);

            log.LogVariableState("Site code ", siteCode);
            log.LogVariableState("Machine ID ", machineId);
            log.LogVariableState("Starting Balance ", startingBalance);
            log.LogVariableState("Ending Balance ", endingBalance);
            log.LogMethodExit("");
            return "";
        }
        /// <summary>
        /// updateMifareCard
        /// </summary>
        /// <returns></returns>
        public virtual int getPlayCount()
        {
            log.LogMethodEntry();
            log.LogMethodExit(0);
            return 0;
        }
        /// <summary>
        /// updateMifareCard
        /// </summary> 
        public virtual bool updateMifareCard(bool AbsoluteOrIncremental, ref string message, params object[] values)
        {
            log.LogMethodEntry(AbsoluteOrIncremental, message, values);

            log.LogVariableState("Message ", message);
            log.LogMethodExit(true);
            return true;
        }
        /// <summary>
        /// AddCreditsToCard
        /// </summary> 
        public virtual bool AddCreditsToCard(double addCredits, SqlTransaction SQLTrx, ref string message, double usedCredits = 0, double addMiFareCreditPlusCardBalance = 0)
        {
            log.LogMethodEntry(addCredits, SQLTrx, message, usedCredits, addMiFareCreditPlusCardBalance);

            SqlCommand cmd = Utilities.getCommand(SQLTrx);
            cmd.CommandText = @"update cards set credits = isnull(credits, 0) + @credits,
                                    last_update_time = getdate(),
                                    LastUpdatedBy = @LastUpdatedBy,
                                    credits_played = isnull(credits_played, 0) + @usedCredits 
                                    where card_id = @card_id";
            cmd.Parameters.AddWithValue("@card_id", card_id);
            cmd.Parameters.AddWithValue("@credits", addCredits);
            if (usedCredits > 0)
            {
                cmd.Parameters.AddWithValue("@usedCredits", usedCredits);
            }
            else
            {
                cmd.Parameters.AddWithValue("@usedCredits", 0);
            }
            cmd.Parameters.AddWithValue("@LastUpdatedBy", loginId);

            log.LogVariableState("@card_id", card_id);
            log.LogVariableState("@credits", addCredits);
            log.LogVariableState("@usedCredits", usedCredits);
            log.LogVariableState("@LastUpdatedBy", loginId);

            try
            {
                cmd.ExecuteNonQuery();

                log.LogVariableState("Message ", message);
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Unable to execute SQL command Query", ex);

                log.LogVariableState("Message ", message);
                log.LogMethodExit(false);
                message = ex.Message;
                return false;
            }
        }

        /*
        Data on the card
        Sector 2 and Sector 3 is being used to store the entitlement. Each entitlement is of 8 bytes. 
        So totally 6 entitlements can be stored in a sector and in our case as there are 2 sectors in use, total of 12 entitlements can be stored.
 
        The format will be
        Byte 0 - Type - Top nibble will indicate the type of entitlement. In case of game entitlement we will store the top nibble as 1. 
                        The bottom nibble will indicate what is the id. It will be 0 if game id, 1 if game profile id and 2 if generic.
        Byte 1 - Id. This would be game id or game profile id or will be 255 to indicate generic (255 generic is redundant as byte 0 bottom nibble would have indicated already, but we'll keep it just as precaution)
        Byte 2 and 3 - Count - Number of games in this case
        Byte 4 - 7 - Future use to store the time
 
        So, in case it's an entitlement of 20 games for the game id 37, the data would be
        0x10, 0x25, 0x14, 0x00, 0x00, 0x00, 0x00, 0x00 
        */
        /// <summary>
        /// AddEntitlements
        /// </summary>
        /// <param name="ProductId"></param>
        /// <returns></returns>
        public virtual bool AddEntitlements(int ProductId)
        {
            log.LogMethodEntry(ProductId);

            DataTable dt = Utilities.executeDataTable(@"select pg.game_profile_id, pg.game_Id, isnull(pg.quantity, 0) quantity, isnull(g.UserIdentifier, gp.UserIdentifier) UserIdentifier
                                                       from ProductGames pg 
                                                            left outer join games g on g.game_id = pg.game_id
                                                            left outer join game_profile gp on gp.game_profile_id = pg.game_profile_id
                                                      where product_id = @product_id
                                                      and quantity != 0",
                                                     new SqlParameter("@product_id", ProductId));

            log.LogVariableState("@product_id", ProductId);

            foreach (DataRow dr in dt.Rows)
            {
                Entitlement ent = new Entitlement();

                ent.EntType = 1;

                if (dr["game_Id"] != DBNull.Value)
                    ent.IdType = 0;
                else if (dr["game_profile_Id"] != DBNull.Value)
                    ent.IdType = 1;
                else
                    ent.IdType = 2;

                if (dr["UserIdentifier"] != DBNull.Value)
                {
                    int uid = Convert.ToInt32(dr["UserIdentifier"]);
                    if (uid < 0)
                        ent.UserIdentifier = 0xff;
                    else
                        ent.UserIdentifier = Convert.ToByte(uid);
                }
                else
                    ent.UserIdentifier = 0xff;

                int qty = Convert.ToInt32(dr["quantity"]);
                if (qty < 0)
                    ent.EntCount = 0xffff;
                else
                    ent.EntCount = Convert.ToUInt16(qty);

                Entitlements.Add(ent);
            }

            log.LogMethodExit(true);
            return true;
        }
        /// <summary>
        /// createRegistrationEntitlements
        /// </summary>
        /// <param name="SQLTrx"></param>
        public void createRegistrationEntitlements(SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(SQLTrx);

            if (ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "REGISTRATION_BONUS_ON_VERIFICATION").Equals("N"))
            {
                int productId = ParafaitDefaultContainerList.GetParafaitDefault<int>(Utilities.ExecutionContext, "LOAD_PRODUCT_ON_REGISTRATION", -1);
                if (productId > -1)
                {
                    AccountListBL accountListBL = new AccountListBL(Utilities.ExecutionContext);
                    List<KeyValuePair<AccountDTO.SearchByParameters, string>> accountSearchParameters = new List<KeyValuePair<AccountDTO.SearchByParameters, string>>();
                    accountSearchParameters.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.CUSTOMER_ID, customerDTO.Id.ToString()));
                    List<AccountDTO> accountDTOList = accountListBL.GetAccountDTOList(accountSearchParameters, false, true, SQLTrx);

                    if (accountDTOList == null
                       || (accountDTOList != null && accountDTOList.Count == 1 && accountDTOList[0].TagNumber == CardNumber)
                       || (accountDTOList != null && accountDTOList.Count == 0)
                      )
                    {
                        //string strProdId = Utilities.getParafaitDefaults("LOAD_PRODUCT_ON_REGISTRATION");
                        //int productId = -1;
                        //if (int.TryParse(strProdId, out productId) == true && productId != -1)
                        //{
                        string message = "";
                        Transaction trx = new Transaction(Utilities);
                        Card card = new Card(-1, this.loginId, Utilities, SQLTrx);
                        card.getCardDetails(this.card_id, SQLTrx);
                        trx.createTransactionLine(card, productId, 1, ref message);
                        //trx.StaticDataExchange.PaymentCashAmount = trx.Net_Transaction_Amount;
                        PaymentModeList paymentModeListBL = new PaymentModeList(Utilities.ExecutionContext);
                        List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.SITE_ID, (Utilities.ParafaitEnv.IsCorporate ? Utilities.ParafaitEnv.SiteId : -1).ToString()));
                        searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISCASH, "Y"));
                        List<PaymentModeDTO> paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchParameters, SQLTrx);
                        if (paymentModeDTOList != null)
                        {
                            TransactionPaymentsDTO trxPaymentDTO = new TransactionPaymentsDTO(-1, -1, paymentModeDTOList[0].PaymentModeId, trx.Net_Transaction_Amount,
                                                                                              "", "", "", "", "", -1, "", -1, 0, -1, "", "", false, -1, -1, "", Utilities.getServerTime(),
                                                                                              this.loginId, -1, null, 0, -1, Utilities.ParafaitEnv.POSMachine, -1, "", null);
                            trxPaymentDTO.paymentModeDTO = paymentModeDTOList[0];
                            trx.TransactionPaymentsDTOList.Add(trxPaymentDTO);
                        }
                        trx.SaveTransacation(SQLTrx, ref message);
                        //}
                    }
                    else
                    {
                        Semnox.Core.Utilities.EventLog eventLog = new Semnox.Core.Utilities.EventLog(Utilities);
                        eventLog.logEvent("Customer", 'D', "Registration Bonus", "Customer Id: " + customerDTO.Id + " is given bonus already. So no bonus is given to Card Id: " + this.card_id, "RegistrationBonus", 0, "", customerDTO.Id.ToString(), SQLTrx);
                        log.Debug("Registration bonus is not given as the customer " + customerDTO.Id.ToString() + " is already registered against a card");
                    }
                }
            }
            log.LogMethodExit(null);
        }
        /// <summary>
        /// GetGameCount
        /// </summary>
        /// <param name="SQLTrx"></param>
        public virtual void GetGameCount(SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(SQLTrx);

            if (card_id < 0)
            {
                log.LogMethodExit(null);
                return;
            }
            CardGames = Convert.ToInt32(Utilities.executeScalar(@"select isnull(sum(BalanceGames), 0) play_count
                                                                from CardGames cg
                                                                where card_id = @card_id 
								                                  AND isnull(validityStatus, 'Y') != 'H' 
                                                                  and (expiryDate is null or expiryDate >= getdate()) 
                                                                  and BalanceGames > 0",
                                                        new SqlParameter("@card_id", card_id)));
            log.LogVariableState("@card_id", card_id);

            log.LogMethodExit(null);
        }
        /// <summary>
        /// GetCardEntitlements
        /// </summary>
        /// <param name="CardId"></param>
        /// <returns></returns>
        public virtual bool GetCardEntitlements(int CardId)
        {
            log.LogMethodEntry(CardId);

            DataTable dt = Utilities.executeDataTable(@"select cg.game_profile_id, cg.game_Id, isnull(cg.BalanceGames, 0) BalanceGames, isnull(g.UserIdentifier, gp.UserIdentifier) UserIdentifier
                                                       from CardGames cg 
                                                            left outer join games g on g.game_id = cg.game_id
                                                            left outer join game_profile gp on gp.game_profile_id = cg.game_profile_id
                                                      where card_id = @card_id
                                                        and isnull(ValidityStatus, 'Y') != 'H'
                                                        and (expiryDate is null or expiryDate >= getdate()) 
                                                        and BalanceGames != 0",
                                                     new SqlParameter("@card_id", CardId));

            log.LogVariableState("@card_id", CardId);

            foreach (DataRow dr in dt.Rows)
            {
                Entitlement ent = new Entitlement();

                ent.EntType = 1;

                if (dr["game_Id"] != DBNull.Value)
                    ent.IdType = 0;
                else if (dr["game_profile_Id"] != DBNull.Value)
                    ent.IdType = 1;
                else
                    ent.IdType = 2;

                if (dr["UserIdentifier"] != DBNull.Value)
                    ent.UserIdentifier = Convert.ToByte(dr["UserIdentifier"].ToString(), 16);
                else
                    ent.UserIdentifier = 0xff;

                int qty = Convert.ToInt32(dr["BalanceGames"]);
                if (qty < 0)
                    ent.EntCount = 0xffff;
                else
                    ent.EntCount = Convert.ToUInt16(qty);

                Entitlements.Add(ent);
            }

            log.LogMethodExit(true);
            return true;
        }
        /// <summary>
        /// getOtherDeposits
        /// </summary>
        /// <param name="lockerAllocationDTO"></param>
        /// <returns></returns>
        public double getOtherDeposits(LockerAllocationDTO lockerAllocationDTO)
        {
            log.LogMethodEntry(lockerAllocationDTO);
            LockerAllocationList lockerAllocationList = new LockerAllocationList();
            List<KeyValuePair<LockerAllocationDTO.SearchByLockerAllocationParameters, string>> lockerAllocationSearchParams = new List<KeyValuePair<LockerAllocationDTO.SearchByLockerAllocationParameters, string>>();
            lockerAllocationSearchParams.Add(new KeyValuePair<LockerAllocationDTO.SearchByLockerAllocationParameters, string>(LockerAllocationDTO.SearchByLockerAllocationParameters.REFUNDED, "0"));
            lockerAllocationSearchParams.Add(new KeyValuePair<LockerAllocationDTO.SearchByLockerAllocationParameters, string>(LockerAllocationDTO.SearchByLockerAllocationParameters.CARD_ID, card_id.ToString()));
            List<LockerAllocationDTO> lockerAllocationDTOList = lockerAllocationList.GetAllLockerAllocations(lockerAllocationSearchParams);
            if (lockerAllocationDTOList == null || (lockerAllocationDTOList != null && lockerAllocationDTOList.Count == 0))
            {
                log.LogMethodExit(0);
                return 0;
            }
            lockerAllocationDTO = lockerAllocationDTOList[0];
            object o = Utilities.executeScalar(@"select top 1 l2.amount
                                        from trx_lines l1, LockerAllocation la, trx_lines l2, products p, product_type pt
                                        where l1.TrxId = la.TrxId
                                        and l1.LineId = la.TrxLineId
                                        and la.id = @id
                                        and l2.TrxId = l1.TrxId
                                        and l2.product_id = p.product_id
                                        and p.product_type_id = pt.product_type_id
                                        and pt.product_type = 'LOCKERDEPOSIT'
                                        and l2.CancelledTime is null 
										and l2.LineId = la.TrxLineId-1
                                        and not exists(select 1 from 
                                                       trx_header th Left join trx_Lines tl on th.TrxId = tl.TrxId
										               where th.OriginalTrxID = l2.TrxId and tl.OriginalLineID = l2.LineId )
                                        order by la.IssuedTime desc",
                                        new SqlParameter("@id", lockerAllocationDTO.Id));

            log.LogVariableState("@id", lockerAllocationDTO.Id);

            if (o != null)
            {
                log.LogMethodExit(Convert.ToDouble(o));
                return Convert.ToDouble(o);
            }
            else
            {
                log.LogMethodExit(0);
                return 0;
            }
        }
        /// <summary>
        /// ReverseEntitlements
        /// </summary>
        /// <param name="EntList"></param>
        public virtual void ReverseEntitlements(List<Card.Entitlement> EntList)
        {
            log.LogMethodEntry(EntList);
            log.LogMethodExit(null);
        }
        private int GetPosCardId(string cardNumber, string loginId)
        {
            log.Debug("Begins-GetPosCardId(string cardNumber,string loginId).");
            dataAccessHandler = new DataAccessHandler();
            connstring = dataAccessHandler.ConnectionString;
            using (Utilities = new Utilities(connstring))
            {
                Utilities.ParafaitEnv.Initialize();

                Card currentCard = new Card(cardNumber, loginId, Utilities);
                log.Debug("Ends-GetPosCardId(string cardNumber,string loginId).");
                return currentCard.card_id;
            }
        }
        /// <summary>
        /// returns the details of the CardNumber passed as parameter
        /// </summary> 
        public List<CoreKeyValueStruct> GetCardDetails(string cardNumber, string loginId)
        {
            log.Debug("Starts-GetCardDetails(string cardNumber, string loginId) Method.");
            List<CoreKeyValueStruct> cardDetails = new List<CoreKeyValueStruct>();
            try
            {
                dataAccessHandler = new DataAccessHandler();
                using (Utilities parafaitUtility = new Utilities(dataAccessHandler.ConnectionString))
                {
                    ParafaitEnv parafaitEnv = new ParafaitEnv(parafaitUtility.DBUtilities);
                    parafaitEnv.Initialize();
                    //18-Feb-2016 Validate card number length based on site configuration
                    TagNumberLengthList tagNumberLengthList = new TagNumberLengthList(parafaitUtility.ExecutionContext);
                    if (tagNumberLengthList.Contains(cardNumber.Length) == false)
                        throw new Exception("Invalid Card Number length. Should be " + "(" + tagNumberLengthList + ")");

                    //New method call by passing Device object
                    Card currentCard = new Card(new Semnox.Core.Utilities.DeviceClass(), cardNumber, "", parafaitUtility);
                    parafaitUtility.ParafaitEnv = parafaitEnv;
                    // In case of on-demand roaming, get card from HQ if not present in Local DB
                    if (parafaitEnv.ALLOW_ROAMING_CARDS == "Y" && parafaitEnv.ENABLE_ON_DEMAND_ROAMING == "Y")
                    {
                        string message = "";
                        // string conString = ConfigurationManager.ConnectionStrings["ParafaitConnectionString"].ConnectionString;
                        RemotingClient CardRoamingRemotingClient = new RemotingClient(dataAccessHandler.ConnectionString);
                        if (!new CardUtils(parafaitUtility).getCardFromHQ(CardRoamingRemotingClient, ref currentCard, ref message))
                            throw new Exception("On-Demand Roaming - Unable to get Card Details. Message: " + message);
                    }

                    CreditPlus creditpls = new CreditPlus(parafaitUtility);
                    string cardIssuedDate = currentCard.issue_date.ToString(parafaitEnv.DATETIME_FORMAT);
                    string cardFaceValue = currentCard.face_value.ToString(parafaitEnv.AMOUNT_FORMAT);
                    string cardCredits = currentCard.credits.ToString(parafaitEnv.AMOUNT_FORMAT);
                    string cardCourtesy = currentCard.courtesy.ToString(parafaitEnv.AMOUNT_FORMAT);
                    string cardBonus = currentCard.bonus.ToString(parafaitEnv.AMOUNT_FORMAT);
                    string cardTime = (currentCard.CreditPlusTime + currentCard.time).ToString(parafaitEnv.AMOUNT_FORMAT);
                    string isTechCard = currentCard.technician_card.ToString();

                    int cardID = currentCard.card_id;
                    if (isTechCard.StartsWith("\0"))
                    {
                        String selectUserIdQuery = @"select user_id 
                                          from users
                                         where active_flag = 'Y'
                                           and card_number = @cardNumber";

                        List<SqlParameter> querryParameters = new List<SqlParameter>();
                        querryParameters.Add(new SqlParameter("@cardNumber", cardNumber));
                        DataTable dataTable = dataAccessHandler.executeSelectQuery(selectUserIdQuery, querryParameters.ToArray());
                        if (dataTable.Rows.Count == 0)
                            isTechCard = "N";
                        else
                            isTechCard = "Y";
                    }
                    string cardGames = "";
                    double cardCreditPlusCredits = 0;
                    double cardCreditPlusBonus = 0;
                    double cardCreditPlusProduct = 0;
                    double cardCreditPlusBalance = 0;
                    string cardCreditPlus = creditpls.getBalanceCreditPlus(currentCard.card_id, ref cardCreditPlusBalance, ref cardCreditPlusCredits, ref cardCreditPlusBonus, ref cardCreditPlusProduct).ToString(parafaitEnv.AMOUNT_FORMAT);
                    string cardTicketCount = (currentCard.ticket_count + creditpls.getCreditPlusTickets(currentCard.card_id)).ToString(parafaitEnv.NUMBER_FORMAT);
                    string cardLoyaltyPoints = (currentCard.loyalty_points + creditpls.getCreditPlusLoyaltyPoints(currentCard.card_id)).ToString(parafaitEnv.NUMBER_FORMAT);
                    string cardRedeemableLoyaltyPoints = (currentCard.loyalty_points + creditpls.GetRedeemableCreditPlusLoyaltyPoints(currentCard.card_id)).ToString(parafaitEnv.NUMBER_FORMAT);
                    string cardCreditsPlayed = currentCard.credits_played.ToString(parafaitEnv.NUMBER_FORMAT);
                    currentCard.GetGameCount();
                    cardGames = currentCard.CardGames.ToString();
                    //VA changes
                    string cardCreditVirtualPoint = creditpls.GetCreditPlusVirtualPoints(currentCard.card_id).ToString(parafaitEnv.NUMBER_FORMAT);
                    //VA changes

                    string cardStatus;
                    if (currentCard.card_id == -1)
                        cardStatus = "New Card";
                    else
                    {
                        if (currentCard.vip_customer == 'Y')
                            cardStatus = "MIP";
                        else
                            cardStatus = "Normal";
                    }
                    string customerName = "";
                    string customerDOB = "";
                    if (currentCard.customer_id != -1)
                    {
                        customerName = currentCard.customerDTO.FirstName + " " + currentCard.customerDTO.MiddleName + " " + currentCard.customerDTO.LastName;
                        if (currentCard.customerDTO.DateOfBirth != null)
                            customerDOB = currentCard.customerDTO.DateOfBirth.Value.ToString("M");
                    }
                    cardDetails.Add(new CoreKeyValueStruct("CardStatus", cardStatus));
                    cardDetails.Add(new CoreKeyValueStruct("CardCustomerName", customerName));
                    cardDetails.Add(new CoreKeyValueStruct("CardCustomerId", currentCard.customer_id.ToString()));
                    cardDetails.Add(new CoreKeyValueStruct("CardCustomerDOB", customerDOB));
                    cardDetails.Add(new CoreKeyValueStruct("CardIssuedDate", cardIssuedDate));
                    cardDetails.Add(new CoreKeyValueStruct("CardFaceValue", cardFaceValue));
                    cardDetails.Add(new CoreKeyValueStruct("CardCredits", cardCredits));
                    cardDetails.Add(new CoreKeyValueStruct("CardCourtesy", cardCourtesy));
                    cardDetails.Add(new CoreKeyValueStruct("CardBonus", cardBonus));
                    cardDetails.Add(new CoreKeyValueStruct("CardTime", cardTime));
                    cardDetails.Add(new CoreKeyValueStruct("CardGames", cardGames));
                    cardDetails.Add(new CoreKeyValueStruct("CardCreditPlus", cardCreditPlus));
                    cardDetails.Add(new CoreKeyValueStruct("CardTicketCount", cardTicketCount));
                    cardDetails.Add(new CoreKeyValueStruct("CardLoyaltyPoints", cardLoyaltyPoints));
                    cardDetails.Add(new CoreKeyValueStruct("CardRedeemableLoyaltyPoints", cardRedeemableLoyaltyPoints));
                    cardDetails.Add(new CoreKeyValueStruct("CardCreditsPlayed", cardCreditsPlayed));
                    cardDetails.Add(new CoreKeyValueStruct("IsTechCard", isTechCard));
                    cardDetails.Add(new CoreKeyValueStruct("Status", "VALID"));
                    cardDetails.Add(new CoreKeyValueStruct("CardID", cardID.ToString()));
                    cardDetails.Add(new CoreKeyValueStruct("SiteId", currentCard.siteId.ToString()));

                    log.Debug("Ends-GetCardDetails(string cardNumber, string loginId) Method.");
                }
            }
            catch (Exception ex)
            {
                log.Debug("Ends-GetCardDetails(string cardNumber, string loginId) with an Exception.");
                cardDetails.Clear();
                cardDetails.Add(new CoreKeyValueStruct("Status", "FAILURE"));
                cardDetails.Add(new CoreKeyValueStruct("ErrorMsg", ex.Message));
            }
            return cardDetails;
        }
        /// <summary>
        /// GetGamePlays(string cardNumber, string loginId) method
        /// </summary>
        /// <param name="cardNumber">cardNumber</param>
        /// <param name="loginId">loginId</param>
        /// <returns> returns List of GamePlayStruct  </returns>
        public List<GamePlayStruct> GetGamePlays(string cardNumber, string loginId)
        {
            log.Debug("Begins-GetGamePlays(string cardNumber, string loginId).");
            List<GamePlayStruct> gamePlayList = new List<GamePlayStruct>();

            try
            {
                int cardId = GetPosCardId(cardNumber, loginId);

                string gamePlayQuery = @"Select TOP 10 gp.gameplay_id, gp.play_date Date, m.machine_name Game, isnull(gp.credits, 0) Credits,
                                           isnull(gp.courtesy, 0) Courtesy, isnull(gp.bonus, 0) Bonus, gp.time Time,
                                           isnull(gp.ticket_count, 0) Tickets, gp.ticket_mode Mode from gameplay gp, 
                                           machines m where gp.card_id = @card_id and gp.machine_id = m.machine_id 
                                           order by gp.play_date desc";


                SqlParameter[] gamePlayParameters = new SqlParameter[1];
                gamePlayParameters[0] = new SqlParameter("@card_id", cardId);

                DataTable gamePlayDataTable = dataAccessHandler.executeSelectQuery(gamePlayQuery, gamePlayParameters);



                if (gamePlayDataTable.Rows.Count == 0)
                {
                    gamePlayList.Add(new GamePlayStruct(-1, DateTime.MinValue.ToString(), "There are no game plays", "0", "0", "0", "0"));
                }

                for (int i = 0; i < gamePlayDataTable.Rows.Count; i++)
                {
                    int gamePlayId = Convert.ToInt32(gamePlayDataTable.Rows[i]["gameplay_id"].ToString());
                    string gamePlayDate = Convert.ToDateTime(Convert.ToDateTime(gamePlayDataTable.Rows[i]["Date"])).ToString(Utilities.ParafaitEnv.DATETIME_FORMAT);
                    string gamePlayCredits = Convert.ToDouble(gamePlayDataTable.Rows[i]["Credits"]).ToString(Utilities.ParafaitEnv.AMOUNT_FORMAT);
                    string gamePlayBonus = Convert.ToDouble(gamePlayDataTable.Rows[i]["Bonus"]).ToString(Utilities.ParafaitEnv.AMOUNT_FORMAT);
                    string gamePlayCourtesy = Convert.ToDouble(gamePlayDataTable.Rows[i]["Courtesy"]).ToString(Utilities.ParafaitEnv.AMOUNT_FORMAT);
                    string gamePlayTickets = gamePlayDataTable.Rows[i]["Tickets"].ToString();

                    gamePlayList.Add(new GamePlayStruct(gamePlayId, gamePlayDate, gamePlayDataTable.Rows[i]["Game"].ToString(), gamePlayCredits, gamePlayBonus, gamePlayCourtesy, gamePlayTickets));
                }
            }
            catch (Exception ex)
            {
                gamePlayList.Add(new GamePlayStruct(-1, DateTime.MinValue.ToString(), ex.Message, "0", "0", "0", "0"));
            }
            log.Debug("Ends-GetGamePlays(string cardNumber, string loginId).");
            return gamePlayList;
        }

        /// <summary>
        /// GetPurchases
        /// </summary> 
        /// <returns></returns>
        public List<PurchasesStruct> GetPurchases(string cardNumber, string loginId)
        {
            List<PurchasesStruct> purchaseList = new List<PurchasesStruct>();
            try
            {
                log.Debug("Begins-GetPurchases(string cardNumber, string loginId).");
                int cardId = GetPosCardId(cardNumber, loginId);

                string purchaseQuery = @"Select TOP 10 card_id, Date, Product, isnull(Amount, 0) Amount, isnull(Credits, 0) Credits,
                                            isnull(Courtesy, 0) Courtesy, isnull(Bonus, 0) Bonus, isnull(tickets, 0) Tickets from CardActivityView 
                                            where card_id = @card_id order by Date DESC";

                SqlParameter[] purchaseParameters = new SqlParameter[1];
                purchaseParameters[0] = new SqlParameter("@card_id", cardId);

                DataTable purchaseDataTable = dataAccessHandler.executeSelectQuery(purchaseQuery, purchaseParameters);

                if (purchaseDataTable.Rows.Count == 0)
                {
                    purchaseList.Add(new PurchasesStruct(-1, DateTime.MinValue.ToString(), "There are no purchases", "0", "0", "0", "0", "0"));
                }

                for (int i = 0; i < purchaseDataTable.Rows.Count; i++)
                {
                    int purchaseId = Convert.ToInt32(purchaseDataTable.Rows[i]["card_id"]);
                    string productName = purchaseDataTable.Rows[i]["Product"].ToString();
                    string purchaseDate = Convert.ToDateTime(purchaseDataTable.Rows[i]["Date"]).ToString(Utilities.ParafaitEnv.AMOUNT_FORMAT);
                    string purchaseAmt = Convert.ToDouble(purchaseDataTable.Rows[i]["Amount"].ToString()).ToString(Utilities.ParafaitEnv.AMOUNT_FORMAT);
                    string purchaseCredits = Convert.ToDouble(purchaseDataTable.Rows[i]["Credits"].ToString()).ToString(Utilities.ParafaitEnv.AMOUNT_FORMAT);
                    string purchaseCourtesy = Convert.ToDouble(purchaseDataTable.Rows[i]["Courtesy"].ToString()).ToString(Utilities.ParafaitEnv.AMOUNT_FORMAT);
                    string purchaseBonus = Convert.ToDouble(purchaseDataTable.Rows[i]["Bonus"].ToString()).ToString(Utilities.ParafaitEnv.AMOUNT_FORMAT);
                    string purchaseTickets = purchaseDataTable.Rows[i]["Tickets"].ToString();
                    purchaseList.Add(new PurchasesStruct(purchaseId, purchaseDate, productName, purchaseAmt, purchaseCredits, purchaseBonus, purchaseCourtesy, purchaseTickets));
                }
            }
            catch (Exception ex)
            {
                purchaseList.Add(new PurchasesStruct(-1, DateTime.MinValue.ToString(), ex.Message, "0", "0", "0", "0", "0"));
            }
            log.Debug("Ends-GetPurchases(string cardNumber, string loginId).");
            return purchaseList;
        }
        /// <summary>
        /// GetRedemptionDiscountForMembership
        /// </summary>
        /// <returns></returns>
        public double GetRedemptionDiscountForMembership()
        {
            log.LogMethodEntry();
            double retValue = 1;
            if (customer_id != -1)
            {
                CustomerBL cardCustomer;
                if (customerDTO == null)
                {
                    cardCustomer = new CustomerBL(Utilities.ExecutionContext, customer_id);
                }
                else
                {
                    cardCustomer = new CustomerBL(Utilities.ExecutionContext, customerDTO);
                }
                retValue = cardCustomer.RedemptionDiscountForMembership();
            }
            log.LogMethodExit(retValue);
            return retValue;
        }
        /// <summary>
        /// SaveCustomer
        /// </summary>
        /// <param name="sqlTrx"></param>
        public void SaveCustomer(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry();
            if (this.customerDTO != null && (this.customerDTO.Id == -1 || this.customerDTO.IsChangedRecursive))
            {
                CustomerBL customerBL = new CustomerBL(Utilities.ExecutionContext, customerDTO);
                customerBL.Save(sqlTrx);
            }
            log.LogMethodExit();
        }
    }
}
