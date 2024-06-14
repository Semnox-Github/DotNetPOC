/********************************************************************************************
 * Project Name - POS- clsRedemption
 * Description  - POS redemption class 
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.4.0       09-Sep-2018      Guru S A           Modified for redemption reversal changes 
 *2.5.0       12-Feb-2019      Archana            Redemption gift search changes
 *2.6.0       11-Apr-2019      Archana            Include/Exclude for redeemable products
 *2.70.3      21-Aug-2019      Archana            Reprint ticket receipt changes
 *2.70.3      28-Aug-2019      Dakshakh           Redemption currency rule enhancement
 *2.70.3      10-Oct-2019      Girish Kundar      Modified : Ticket station enhancement
 *2.70.3      30-Jan-2020      Archana            Modified : Per day limit check is added for the manual ticket add process
 *2.70.3      30-Apr-2020      Archana            Display card number in Redeem screen when the non registerd card is tapped
 *2.80.1      05-Jan-2021      Deeksha            Issue Fix : CEC HQ Negative tickets
 *2.130.4     22-Feb-2022      Mathew Ninan    Modified DateTime to ServerDateTime 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Printer;
using Semnox.Parafait.Redemption;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.User;

namespace Parafait_POS
{
    public class clsRedemption
    {
        public int _RedemptionId = -1;
        string createdFromSuspendId = "";
        string _ScreenNumber;
        private Tuple<int, int, int> addManualTicketMangerApprovalDetails = null;

        public Tuple<int, int, int> AddManualTicketMangerApprovalDetails
        {
            get { return addManualTicketMangerApprovalDetails; }
            set { addManualTicketMangerApprovalDetails = value; }
        }

        public delegate bool AuthenticateManager(ref int managerId);
        public AuthenticateManager authenticateManager;

        public delegate void LaunchFlagTicketReceiptUI(string barCode = null);
        public LaunchFlagTicketReceiptUI launchFlagTicketReceiptUI;

        public class clsCards
        {
            public string cardNumber;
            public string customerName;
            public int cardId;
            public int Tickets;
            public int Redeemed;
            public int customerId;
            public double RedemptionDiscount;
        }

        public class clsScanTickets
        {
            public string barCode;
            public int Tickets;
        }

        public class clsRedemptionCurrency
        {
            [DisplayName("Currency Id")]
            public int currencyId { get; set; }
            [DisplayName("Currency Name")]
            public string currencyName { get; set; }
            [DisplayName("Bar Code")]
            public string barCode { get; set; }
            [DisplayName("Value In Tickets")]
            public int ValueInTickets { get; set; }
            [DisplayName("Quantity")]
            public int quantity { get; set; }
            [Browsable(false)]
            public object productId { get; set; }

            [DisplayName("RedemptionCurrency Rule Id")]
            public int? redemptionCurrencyRuleId { get; set; }

            [DisplayName("RedemptionRule Ticket Delta")]
            public int? redemptionRuleTicketDelta { get; set; }

            [DisplayName("SourceCurrency Rule Id")]
            public int? sourceCurrencyRuleId { get; set; }

            bool checkApplicableCurrenyRule { get; set; }

            [DisplayName("RedemptionCurrency Rule Name")]
            public string redemptionCurrencyRuleName { get; set; }
        }

        public class clsProducts
        {
            public int productId;
            public string code;
            public string barCode;
            public string productName;
            public int priceInTickets;
            public int Quantity;
            public int originalPriceInTickets;
        }

        public List<clsCards> cardList = new List<clsCards>();
        public List<clsProducts> productList = new List<clsProducts>();
        public List<clsScanTickets> scanTicketList = new List<clsScanTickets>();
        public List<clsRedemptionCurrency> currencyList = new List<clsRedemptionCurrency>();
        public class TicketSourceInfo
        {
            public string ticketSource;
            public int ticketValue;
            public string receiptNo;
            public int currencyId;
            public double currencyValueInTickets;
            public double currencyQuantity;
            public int cardId;
            public int balanceTickets;
            public int? redemptionCurrencyRuleId;
            public int? redemptionRuleTicketDelta;
            public int sourceCurrencyRuleId;
            public string redemptionCurrencyRuleName;

        }

        public List<TicketSourceInfo> ticketSourceInfoObj = new List<TicketSourceInfo>();
        int ManualTickets = 0;
        Utilities _utilities;
        MessageUtils MessageUtils;
        ParafaitEnv ParafaitEnv;
        //Begin: Modified Added for logger function on 08-Mar-2016
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //End: Modified Added for logger function on 08-Mar-2016

        /// <summary>
        /// cls redemption
        /// </summary>
        /// <param name="inUtilities">inUtilities</param>
        /// <param name="ScreenNumber">ScreenNumber</param>
        public clsRedemption(Utilities inUtilities, string ScreenNumber = null)
        {
            //log = Logger.setLogFileAppenderName(log);
            Semnox.Core.Utilities.Logger.setRootLogLevel(log);
            log.LogMethodEntry(inUtilities, ScreenNumber);
            _utilities = inUtilities;
            MessageUtils = _utilities.MessageUtils;
            ParafaitEnv = _utilities.ParafaitEnv;
            _ScreenNumber = ScreenNumber;
            log.LogMethodExit();
        }

        /// <summary>
        /// Add manual tickets
        /// </summary>
        /// <param name="tickets">tickets</param>
        /// <param name="message">message</param>
        /// <returns></returns>
        public bool addManualTickets(int tickets, ref string message)
        {
            log.LogMethodEntry(tickets, message);
            if (ManualTickets + tickets > POSStatic.MAX_MANUAL_TICKETS_PER_REDEMPTION)
            {
                message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 2495, POSStatic.MAX_MANUAL_TICKETS_PER_REDEMPTION.ToString());

                log.Debug("Manual ticket: " + (ManualTickets + tickets) + "," + message);
                log.LogMethodExit(false);
                return false;
            }
            RedemptionBL redemptionBL = new RedemptionBL(_utilities.ExecutionContext);
            try
            {
                redemptionBL.PerDayLimitCheckForManualTickets(ManualTickets + tickets);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                message = ex.Message;
                log.LogMethodExit(false);
                return false;
            }
            ManualTickets += tickets;
            message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 2682, tickets.ToString());// tickets.ToString() + " Manual tickets added";
            log.Info("addManualTickets(" + tickets + "," + message + ") - Manual tickets added ");
            log.Debug("Ends-addManualTickets(" + tickets + "," + message + ")");
            log.LogMethodExit(true);
            return true;
        }

        /// <summary>
        /// add scan tickets
        /// </summary>
        /// <param name="barCode">barCode</param>
        /// <param name="message">message</param>
        /// <returns></returns>
        public bool addScanTickets(string barCode, ref string message)
        {
            log.LogMethodEntry(barCode, message);
            if (scanTicketList.Find(delegate (clsScanTickets scanTicket) { return (scanTicket.barCode == barCode); }) != null)
            {
                message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 112);
                log.Info("Ends-addScanTickets(" + barCode + "," + message + ") as Receipt already used");
                log.LogMethodExit(false);
                return false;
            }

            //SqlCommand cmd = _utilities.getCommand();
            //cmd.CommandText = "select top 1 1 from ManualTicketReceipts where ManualTicketReceiptNo = @receiptNo and BalanceTickets = 0";
            //cmd.Parameters.AddWithValue("@receiptNo", barCode);
            //if (cmd.ExecuteScalar() != null)
            TicketReceipt ticketReceipt = new TicketReceipt(_utilities.ExecutionContext,barCode);
            if (ticketReceipt.IsUsedTicketReceipt(null))
            {
                message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 112);
                log.Info("Ends-addScanTickets(" + barCode + "," + message + ") as Receipt already used");
                log.LogMethodExit(false);
                return false;
            }
            if (ticketReceipt.IsFlaggedTicketReceipt())
            {
                ApplicationRemarksList applicationRemarksList = new ApplicationRemarksList(_utilities.ExecutionContext);
                List<KeyValuePair<ApplicationRemarksDTO.SearchByApplicationRemarksParameters, string>> applicationRemarksSearchParams = new List<KeyValuePair<ApplicationRemarksDTO.SearchByApplicationRemarksParameters, string>>();
                applicationRemarksSearchParams.Add(new KeyValuePair<ApplicationRemarksDTO.SearchByApplicationRemarksParameters, string>(ApplicationRemarksDTO.SearchByApplicationRemarksParameters.ACTIVE_FLAG, "1"));
                applicationRemarksSearchParams.Add(new KeyValuePair<ApplicationRemarksDTO.SearchByApplicationRemarksParameters, string>(ApplicationRemarksDTO.SearchByApplicationRemarksParameters.SITE_ID, ((_utilities.ParafaitEnv.IsCorporate && _utilities.ParafaitEnv.IsMasterSite) ? _utilities.ParafaitEnv.SiteId : -1).ToString()));
                applicationRemarksSearchParams.Add(new KeyValuePair<ApplicationRemarksDTO.SearchByApplicationRemarksParameters, string>(ApplicationRemarksDTO.SearchByApplicationRemarksParameters.SOURCE_NAME, "ManualTicketReceipts"));
                applicationRemarksSearchParams.Add(new KeyValuePair<ApplicationRemarksDTO.SearchByApplicationRemarksParameters, string>(ApplicationRemarksDTO.SearchByApplicationRemarksParameters.SOURCE_GUID, ticketReceipt.TicketReceiptDTO.Guid));
                List<ApplicationRemarksDTO> applicationRemarksListOnDisplay = applicationRemarksList.GetAllApplicationRemarks(applicationRemarksSearchParams);
                if (MessageBox.Show(MessageContainerList.GetMessage(_utilities.ExecutionContext, 1395, ": " + ((applicationRemarksListOnDisplay != null && applicationRemarksListOnDisplay.Count > 0) ? applicationRemarksListOnDisplay[0].Remarks : " " + MessageContainerList.GetMessage(_utilities.ExecutionContext, "unknown") + ".") + "\n"),
                                   MessageContainerList.GetMessage(_utilities.ExecutionContext, "Ticket receipt flagged.") + MessageContainerList.GetMessage(_utilities.ExecutionContext, 2693, _ScreenNumber),
                                   MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (launchFlagTicketReceiptUI != null)
                    {
                        launchFlagTicketReceiptUI(barCode);
                    }
                    else
                    {
                        message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 2690);// 'Unable to launch Flag Ticket Receipt UI'
                        log.LogMethodExit(false);
                        return false;
                    }
                    //TicketReceiptUI ticketReceiptUI = new TicketReceiptUI(_utilities, barCode);
                    //ticketReceiptUI.DoManagerAuthenticationCheck += new TicketReceiptUI.DoManagerAuthenticationCheckDelegate(AuthenticateManager);
                    //ticketReceiptUI.FormBorderStyle = FormBorderStyle.None;
                    //ticketReceiptUI.Location = this.Location;
                    ////ticketReceiptUI.WindowState = FormWindowState.Maximized;
                    //ticketReceiptUI.Width = this.Width;
                    //ticketReceiptUI.Height = this.Height;
                    //ticketReceiptUI.AutoScroll = true;  
                    //ticketReceiptUI.Owner = this;
                    //ticketReceiptUI.SetLastActivityTime += new TicketReceiptUI.SetLastActivityTimeDelegate(this.SetLastActivityTime);
                    //ticketReceiptUI.Show();

                }
                message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 2664);//Suspected Receipt
                log.Info(barCode + "," + message + ") as ticket receipt is flagged for the reason :" + ((applicationRemarksListOnDisplay != null && applicationRemarksListOnDisplay.Count > 0) ? applicationRemarksListOnDisplay[0].Remarks : " unknown."));
                log.LogMethodExit(false);
                return false;
            }
            TicketStationBL ticketStationBL = null;
            bool valid = false;
            string stationId = string.Empty;
            try
            {
                TicketStationFactory ticketStationFactory = new TicketStationFactory();
                ticketStationBL = ticketStationFactory.GetTicketStationObject(barCode);
                if (ticketStationBL == null)
                {
                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(_utilities.ExecutionContext, 2321),
                                               MessageContainerList.GetMessage(_utilities.ExecutionContext, "Ticket Station")
                                               + MessageContainerList.GetMessage(_utilities.ExecutionContext, 2693, _ScreenNumber));
                }
                else
                {
                    if (ticketStationBL.BelongsToThisStation(barCode) && ticketStationBL.ValidCheckBit(barCode))
                    {
                        valid = true;
                    }
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
            if (!valid)
            {
                message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 115, POSStatic.TicketTermVariant);
                log.Info("Ends-addScanTickets(" + barCode + "," + message + ") as Scanned Ticket Receipt is invalid");
                log.LogMethodExit(false);
                return false;
            }

            clsScanTickets item = new clsScanTickets();
            item.barCode = barCode;
            try
            {
                item.Tickets = ticketStationBL.GetTicketValue(barCode);
            }
            catch
            {
                message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 115, POSStatic.TicketTermVariant);
                log.Fatal("Ends-addScanTickets(" + barCode + "," + message + ") due to exception  Scanned Ticket Receipt is invalid");
                log.LogMethodExit(false);
                return false;
            }
            scanTicketList.Add(item);
            message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 114, item.Tickets.ToString(), POSStatic.TicketTermVariant);
            log.Debug("Ends-addScanTickets(" + barCode + "," + message + ") ");
            log.LogMethodExit(true);
            return true;
        }

        /// <summary>
        /// add Redemption Currency
        /// </summary>
        /// <param name="barCode">barCode</param>
        /// <param name="currencyName">currencyName</param>
        /// <param name="message">message</param>
        /// <returns></returns>
        public bool addRedemptionCurrency(string barCode, string currencyName, ref string message)
        {
            log.LogMethodEntry(barCode, currencyName, message);
            RedemptionCurrencyList redemptionCurrencyList = new RedemptionCurrencyList(_utilities.ExecutionContext);
            List<KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>> searchParam = new List<KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>>();
            if (!String.IsNullOrEmpty(barCode))
            {
                searchParam.Add(new KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>(RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.BARCODE, barCode));
            }

            if (!String.IsNullOrEmpty(currencyName))
            {
                searchParam.Add(new KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>(RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.CURRENCY_NAME, currencyName));
            }
            searchParam.Add(new KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>(RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.ISACTIVE, "1"));
            List<RedemptionCurrencyDTO> redemptionCurrencyDTOList = redemptionCurrencyList.GetAllRedemptionCurrency(searchParam);
            if (redemptionCurrencyDTOList != null && redemptionCurrencyDTOList.Count > 0)
            {
                if (CheckRedemptionCurrencyAccess(redemptionCurrencyDTOList[0].CurrencyName, redemptionCurrencyDTOList[0].Guid) == true)
                {
                    if (redemptionCurrencyDTOList[0].ManagerApproval && ((currencyList == null || currencyList.Count == 0) || (currencyList != null && currencyList.Count > 0 && !currencyList.Exists(c => c.currencyName == redemptionCurrencyDTOList[0].CurrencyName))))
                    { //already approved then skipp 
                        int mgrId = -1;
                        if (!authenticateManager(ref mgrId))
                        {
                            message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 268);
                            log.LogMethodExit(message + " return false");
                            throw new Exception(message);
                        }
                    }
                    clsRedemptionCurrency item = new clsRedemptionCurrency();
                    item.barCode = barCode;
                    item.currencyId = redemptionCurrencyDTOList[0].CurrencyId;
                    item.currencyName = redemptionCurrencyDTOList[0].CurrencyName;
                    if (redemptionCurrencyDTOList[0].ProductId == -1)
                        item.productId = DBNull.Value;
                    else
                        item.productId = redemptionCurrencyDTOList[0].ProductId;
                    item.quantity = 1;
                    item.ValueInTickets = (int)redemptionCurrencyDTOList[0].ValueInTickets;
                    item.redemptionCurrencyRuleId = -1;
                    item.sourceCurrencyRuleId = -1;
                    item.redemptionRuleTicketDelta = 0;
                    currencyList.Add(item);
                    message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 1393);

                    log.LogMethodExit(message + " return true");
                    return true;
                }
                else
                {
                    message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 1550, redemptionCurrencyDTOList[0].CurrencyName);
                    log.LogMethodExit(message + " return false");
                    throw new Exception(message);
                }
            }
            else
            {
                message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 1389);
                log.LogMethodExit(message + " return false");
                return false;
            }
        }

        /// <summary>
        /// add Redemption Currency
        /// </summary>
        /// <param name="currencyId">currencyId</param>
        /// <param name="message">message</param>
        /// <returns></returns>
        public bool addRedemptionCurrency(int currencyId, ref string message)
        {
            log.LogMethodEntry(currencyId, message);
            clsRedemptionCurrency rcItem = currencyList.Find(delegate (clsRedemptionCurrency scanCurrency) { return (scanCurrency.currencyId == currencyId); });
            if (rcItem != null)
            {
                rcItem.quantity++;
                message = rcItem.currencyName + " added";
                log.Info("Ends-addRedemptionCurrency(" + currencyId + "," + message + ") - " + rcItem.currencyName + " added");
                log.LogMethodExit(true);
                return true;
            }

            DataTable dt = _utilities.executeDataTable("select top 1 BarCode, CurrencyName, isnull(ValueInTickets, 0) ValueInTickets, CurrencyId, ProductId from RedemptionCurrency where currencyId = @currencyId",
                                             new SqlParameter("@currencyId", currencyId));
            if (dt.Rows.Count == 0)
            {
                message = "Invalid redemption currency";
                log.Info("Ends-addRedemptionCurrency(" + currencyId + "," + message + ") - Invalid redemption currency");
                log.LogMethodExit(false);
                return false;
            }

            RedemptionCurrencyList redemptionCurrencyList = new RedemptionCurrencyList(_utilities.ExecutionContext);
            List<KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>> searchParam = new List<KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>>();
            searchParam.Add(new KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>(RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.CURRENCY_ID, currencyId.ToString()));
            searchParam.Add(new KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>(RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.ISACTIVE, "1"));
            List<RedemptionCurrencyDTO> redemptionCurrencyDTOList = redemptionCurrencyList.GetAllRedemptionCurrency(searchParam);
            clsRedemptionCurrency item = new clsRedemptionCurrency();
            item.barCode = dt.Rows[0]["BarCode"].ToString();
            item.currencyId = currencyId;
            item.currencyName = dt.Rows[0]["CurrencyName"].ToString();
            item.productId = dt.Rows[0]["ProductId"];
            item.quantity = 1;
            item.ValueInTickets = Convert.ToInt32(dt.Rows[0]["ValueInTickets"]);
            item.redemptionCurrencyRuleId = -1;
            item.redemptionRuleTicketDelta = 0;
            item.sourceCurrencyRuleId = -1;
            currencyList.Add(item);
            message = item.currencyName + " added";

            log.Debug("Ends-addRedemptionCurrency(" + currencyId + "," + message + ") ");
            log.LogMethodExit(true);
            return true;
        }


        private DataTable GetCardDetails(string SwipedCardNumber)
        {
            log.LogMethodEntry(SwipedCardNumber);
            string cmdText = "";
            cmdText = @"SELECT card_id, card_number, customer_name, vip_customer, ticket_count, c.site_id, isnull(m.RedemptionDiscount, 0) redemption_discount,
                                       m.membershipName, technician_card, customer_id
                                  FROM (select card_id, card_number, (ISNULL(pf.FirstName,'') + ' ' + ISNULL(pf.lastName,'')) as customer_name, vip_customer, 
                                               isnull(ticket_count, 0) ticket_count, c.site_id,  technician_card, c.customer_id, cu.MembershipId
                                          from cards c  
                                           left outer join customers cu on cu.customer_id = c.customer_id                                 
                                           left outer join Profile pf on pf.Id = cu.ProfileId 
                                         where card_number = @cardno 
                                           and valid_flag = 'Y'
                                       ) c left outer join Membership m on m.MembershipId= c.MembershipId 
                                  WHERE card_number = @cardno ";
            DataTable DT = new DataTable();
            SqlParameter[] sqlParams = new SqlParameter[1];
            sqlParams[0] = new SqlParameter("@cardno", SwipedCardNumber);

            DT = _utilities.executeDataTable(cmdText, sqlParams);
            if (DT.Rows.Count > 0)
            {
                CreditPlus creditPlus = new CreditPlus(_utilities);
                DT.Rows[0]["ticket_count"] = Convert.ToInt32(DT.Rows[0]["ticket_count"]) + creditPlus.getCreditPlusTickets(Convert.ToInt32(DT.Rows[0]["card_id"]));
            }

            log.LogMethodExit(DT);
            return DT;
        }

        /// <summary>
        /// GetScanedReciptStatus
        /// </summary>
        /// <param name="scanedBarcode">scanedBarcode</param>
        /// <param name="sqlTrx">sqlTrx</param>
        /// <returns></returns>
        public bool GetScanedReciptStatus(string scanedBarcode, SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(scanedBarcode, sqlTrx);

            log.Debug("Starts GetScanedReceiptStatus(" + scanedBarcode + ") ");
            if (_utilities.executeScalar("select top 1 1 from ManualTicketReceipts where ManualTicketReceiptNo = @receiptNo and BalanceTickets = 0", sqlTrx,
                                                  new SqlParameter("@receiptNo", scanedBarcode)) != null)
            {
                log.Info("Ends-GetScanedReciptStatus (" + scanedBarcode + ") Receipt already used");
                log.LogMethodExit(false);
                return false;
            }
            else
                log.LogMethodExit(true);
            return true;
        }

        /// <summary>
        /// add card
        /// </summary>
        /// <param name="CardNumber">CardNumber</param>
        /// <param name="message">message</param>
        /// <returns></returns>
        public bool addCard(string CardNumber, ref string message)
        {
            log.LogMethodEntry(CardNumber, message);
            if (cardList.Find(delegate (clsCards item) { return (item.cardNumber == CardNumber); }) != null)
            {
                message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 59);
                log.Info("Ends-addCard(" + CardNumber + "," + message + ") as Card already added");
                log.LogMethodExit(true);
                return true;
            }

            DataTable DT = GetCardDetails(CardNumber);
            if (DT.Rows.Count > 0)
            {
                if (cardList.Count > 0)
                {
                    if (POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(_utilities.ExecutionContext, 1434, CardNumber),
                                                    MessageContainerList.GetMessage(_utilities.ExecutionContext, "Card Consolidation")
                                                    + MessageContainerList.GetMessage(_utilities.ExecutionContext, 2693, _ScreenNumber)
                                                    , MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 1435, CardNumber);
                        log.Info("Ends-addCard(User declined to consolidate " + CardNumber + " with current redemption");
                        log.LogMethodExit(true);
                        return true;
                    }
                }

                if (DT.Rows[0]["site_id"] != DBNull.Value && Convert.ToInt32(DT.Rows[0]["site_id"]) != ParafaitEnv.SiteId && ParafaitEnv.ALLOW_ROAMING_CARDS == "N")
                {
                    message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 110);
                    log.Info("Ends-addCard(" + CardNumber + "," + message + ") as site_id is null or Roaming card are not allowed");
                    log.LogMethodExit(false);
                    return false;
                }

                if (DT.Rows[0]["technician_card"].ToString() == "Y")
                {
                    message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 197, CardNumber);
                    log.Info("Ends-addCard(" + CardNumber + "," + message + ") as Technician Card (" + CardNumber + ") not allowed for Transaction");
                    log.LogMethodExit(false);
                    return false;
                }
                string custName = DT.Rows[0]["customer_name"].ToString();
                if (POSStatic.REGISTRATION_MANDATORY_FOR_REDEMPTION && string.IsNullOrEmpty((custName.Trim())) == true)
                {
                    message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 515);
                    log.Info("Ends-addCard(" + CardNumber + "," + message + ") as Redemption allowed for registered customers only");
                    log.LogMethodExit(false);
                    return false;
                }

                clsCards item = new clsCards();
                item.cardNumber = CardNumber;
                item.customerId = ((DT.Rows[0]["customer_id"] != DBNull.Value && DT.Rows[0]["customer_id"].ToString() != "") ? Convert.ToInt32(DT.Rows[0]["customer_id"]) : -1);
                item.customerName = DT.Rows[0]["customer_name"].ToString();
                item.cardId = Convert.ToInt32(DT.Rows[0]["card_id"]);
                item.Tickets = Convert.ToInt32(DT.Rows[0]["ticket_count"]);
                if (string.IsNullOrEmpty(item.customerName) == false)
                {
                    item.customerName = item.customerName.Trim();
                }
                item.RedemptionDiscount = Convert.ToDouble(DT.Rows[0]["redemption_discount"]);
                if (cardList.Exists(c => c.cardId == item.cardId))
                {
                    message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 59);
                    log.Info("Ends-addCard(" + CardNumber + "," + message + ") as Card already added");
                    log.LogMethodExit(true);
                    return true;
                }
                cardList.Add(item);

                message = MessageContainerList.GetMessage(_utilities.ExecutionContext, "Card") + ": " + CardNumber;
                log.Info("Ends-addCard(" + CardNumber + "," + message + ")");
                log.LogMethodExit(true);
                return true;
            }
            else
            {
                message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 110, CardNumber);
                log.Info("Ends-addCard(" + CardNumber + "," + message + ") as Card " + CardNumber + " not found. Please issue the card before proceeding");
                log.LogMethodExit(false);
                return false;
            }
        }

        /// <summary>
        /// refresh card list
        /// </summary>
        void refreshCardList()
        {
            log.LogMethodEntry();
            foreach (clsCards item in cardList)
            {
                DataTable DT = GetCardDetails(item.cardNumber);
                if (DT.Rows.Count > 0)
                {
                    item.Tickets = Convert.ToInt32(DT.Rows[0]["ticket_count"]);
                    if (string.IsNullOrEmpty(item.customerName) == false)
                    {
                        item.customerName = item.customerName.Trim();
                    }
                    item.customerId = ((DT.Rows[0]["customer_id"] != DBNull.Value && DT.Rows[0]["customer_id"].ToString() != "") ? Convert.ToInt32(DT.Rows[0]["customer_id"]) : -1);
                    item.RedemptionDiscount = Convert.ToDouble(DT.Rows[0]["redemption_discount"]);
                }
                else
                    item.Tickets = 0;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// remove currency
        /// </summary>
        /// <param name="currencyId">currencyId</param>
        public void removeCurrency(int currencyId)
        {
            log.LogMethodEntry(currencyId);
            clsRedemptionCurrency foundItem = currencyList.Find(delegate (clsRedemptionCurrency item) { return (item.currencyId == currencyId); });
            if (foundItem != null)
            {
                currencyList.Remove(foundItem);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// reduce currency
        /// </summary>
        /// <param name="currencyId">currencyId</param>
        public void reduceCurrency(int currencyId)
        {
            log.LogMethodEntry(currencyId);
            clsRedemptionCurrency foundItem = currencyList.Find(delegate (clsRedemptionCurrency item) { return (item.currencyId == currencyId); });
            if (foundItem != null)
            {
                if (foundItem.quantity > 1)
                {
                    foundItem.quantity--;
                }
                else if (foundItem.quantity == 1)
                {
                    currencyList.Remove(foundItem);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Increase Currency
        /// </summary>
        /// <param name="currencyId">currencyId</param>
        public void IncreaseCurrency(int currencyId)
        {
            log.LogMethodEntry(currencyId);
            clsRedemptionCurrency foundItem = currencyList.Find(delegate (clsRedemptionCurrency item) { return (item.currencyId == currencyId); });
            if (foundItem != null)
            {
                foundItem.quantity++;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Update currency quantity
        /// </summary>
        /// <param name="barCode">barCode</param>
        /// <param name="newQuantity">newQuantity</param>
        /// <param name="message">message</param>
        public void updateCurrencyQuantity(string barCode, int newQuantity, ref string message)
        {
            log.LogMethodEntry(barCode, message);
            clsRedemptionCurrency foundItem = currencyList.Find(delegate (clsRedemptionCurrency item) { return (item.barCode == barCode); });
            if (foundItem != null)
            {
                foundItem.quantity = newQuantity;
                message = foundItem.currencyName + ": Quantity updated to " + newQuantity.ToString();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// remove card
        /// </summary>
        /// <param name="cardNumber">cardNumber</param>
        public void removeCard(string cardNumber)
        {
            log.LogMethodEntry(cardNumber);
            clsCards foundItem = cardList.Find(delegate (clsCards item) { return (item.cardNumber == cardNumber); });
            if (foundItem != null)
            {
                cardList.Remove(foundItem);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// remove voucher
        /// </summary>
        /// <param name="voucherBarCode">voucherBarCode</param>
        public void removeVoucher(string voucherBarCode)
        {
            log.LogMethodEntry(voucherBarCode);
            clsScanTickets foundItem = scanTicketList.Find(delegate (clsScanTickets item) { return (item.barCode == voucherBarCode); });
            if (foundItem != null)
            {
                scanTicketList.Remove(foundItem);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// get discount
        /// </summary>
        /// <returns></returns>
        public double getDiscount()
        {
            log.LogMethodEntry();
            double RedemptionDiscount = 0;
            foreach (clsCards item in cardList)
            {
                double disc = item.RedemptionDiscount / 100.0;
                if (RedemptionDiscount < disc)  // get the highest discount
                {
                    RedemptionDiscount = disc;
                }
            }

            log.LogMethodExit(RedemptionDiscount);
            return RedemptionDiscount;
        }

        private DataTable GetGiftDetails(int ProductId)
        {
            log.LogMethodEntry(ProductId);
            DataTable dt = GetGiftDetails(ProductId.ToString(), 'I');
            log.LogMethodExit(dt);
            return dt;
        }

        /// <summary>
        /// Get gift details
        /// </summary>
        /// <param name="scannedBarcodeOrCode">scannedBarcodeOrCode</param>
        /// <param name="codeType">codeType</param>
        /// <returns></returns>
        public DataTable GetGiftDetails(String scannedBarcodeOrCode, char codeType)
        {
            log.LogMethodEntry(scannedBarcodeOrCode, codeType);
            DataTable DT = new DataTable();
            //SqlCommand cmd = _utilities.getCommand();
            string cmdText = "";
            //24-Mar-2016
            cmdText = @"select Code, P.Description, round(PriceInTickets * @disc, 0) PriceInTickets,'N' as selectGiftMain, 
                                    isnull(Quantity, 0) Quantity, P.productId, b.barCode, PriceInTickets as OriginalPriceInTickets
                                 from Product P join products ps on p.ManualProductId = ps.product_id
                                            left outer join Inventory I 
                                            on P.productId = I.ProductId 
                                            and I.LocationId = (select isnull(pos.InventoryLocationId, p.outboundLocationId) 
                                                        from (select 1 a) v left outer join POSMachines pos 
                                                        on POSMachineId = @posName) 
										left outer join (select * 
														 from (
																select *, row_number() over(partition by productid order by productid) as num 
																from productbarcode 
																where BarCode = @code and isactive = 'Y')v 
														 where num = 1) b on p.productid = b.productid 
                                    where ((b.BarCode = @code and @Type = 'B') 
                                            or (Code = @code and @Type = 'C') 
                                            or (p.productId = @productId and @Type = 'I')
                                            or ((p.Code like @code + '%' or p.Description like @code + '%' or p.ProductName like @code + '%') and @Type = 'D'))
                                    AND (Quantity > 0 or @ignoreQtyCheck = 'Y')
                                    and not exists (select 1 from redemptionCurrency rc where rc.productId = p.productId) 
                                    AND P.IsActive = 'Y' 
                                    AND P.IsRedeemable = 'Y' 
									and not exists (select 1 
													  from ProductsDisplayGroup pd , 
														   ProductDisplayGroupFormat pdgf,
														   POSProductExclusions ppe 
												  	  where ps.product_id = pd.ProductId 
													   and pd.DisplayGroupId = pdgf.Id 
												  	   and ppe.ProductDisplayGroupFormatId = pdgf.Id
													   and ppe.POSMachineId = @posName ) 
									and not exists (select 1 
														from ProductsDisplayGroup pd , 
														     ProductDisplayGroupFormat pdgf,
														     UserRoleDisplayGroupExclusions urdge , 
														     users u
														where  ps.product_id = pd.ProductId 
													      and pd.DisplayGroupId = pdgf.Id 
													      and urdge.ProductDisplayGroupId = pd.Id
                                                          and urdge.role_id = u.role_id
                                                          and u.loginId = @loginId)";
            //24-Mar-2016
            //cmd.Parameters.AddWithValue("@Type", codeType);
            //cmd.Parameters.AddWithValue("@code", scannedBarcodeOrCode);
            //cmd.Parameters.AddWithValue("@productId", codeType.Equals('I') ? Convert.ToInt32(scannedBarcodeOrCode) : -1);
            //cmd.Parameters.AddWithValue("@disc", 1.0 - getDiscount());
            //cmd.Parameters.AddWithValue("@posName", ParafaitEnv.POSMachineId);
            //cmd.Parameters.AddWithValue("@ignoreQtyCheck", ParafaitEnv.ALLOW_TRANSACTION_ON_ZERO_STOCK);
            //SqlDataAdapter da = new SqlDataAdapter(cmd);
            //da.Fill(DT);
            //da.Dispose();
            SqlParameter[] sqlParams = new SqlParameter[7];
            sqlParams[0] = new SqlParameter("@Type", codeType);
            sqlParams[1] = new SqlParameter("@code", scannedBarcodeOrCode);
            sqlParams[2] = new SqlParameter("@productId", codeType.Equals('I') ? Convert.ToInt32(scannedBarcodeOrCode) : -1);
            sqlParams[3] = new SqlParameter("@disc", 1.0 - getDiscount());
            sqlParams[4] = new SqlParameter("@posName", ParafaitEnv.POSMachineId);
            sqlParams[5] = new SqlParameter("@ignoreQtyCheck", ParafaitEnv.ALLOW_TRANSACTION_ON_ZERO_STOCK);
            sqlParams[6] = new SqlParameter("@loginId", ParafaitEnv.LoginID);
            DT = _utilities.executeDataTable(cmdText, sqlParams);
            log.Debug("Ends-GetGiftDetails(" + scannedBarcodeOrCode + ",codeType)");
            log.LogMethodExit(DT);
            return DT;
        }

        /// <summary>
        /// add Gift
        /// </summary>
        /// <param name="giftCode">giftCode</param>
        /// <param name="CodeType">CodeType</param>
        /// <param name="message">message</param>
        /// <returns></returns>
        public bool addGift(string giftCode, char CodeType, ref string message)
        {
            log.LogMethodEntry(giftCode, CodeType, message);
            DataTable DT = GetGiftDetails(giftCode, CodeType);
            if (DT.Rows.Count > 0)
            {
                clsProducts checkProd = productList.Find(delegate (clsProducts item) { return (item.productId == Convert.ToInt32(DT.Rows[0]["productId"])); });
                if (checkProd != null)
                {
                    if (!checkGiftAvailability(checkProd, true))
                    {
                        message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 120);
                        log.Info("Ends-addGift(" + giftCode + ",codeType,message) as selected more gifts than available stock. Please crosscheck available quantity before proceeding");
                        log.LogMethodExit(false);
                        return false;
                    }
                    else
                    {
                        checkProd.Quantity++;
                        message = MessageContainerList.GetMessage(_utilities.ExecutionContext, "Gift added");
                        log.Info("Ends-addGift(" + giftCode + ",codeType,message) - Gift added");
                    }
                }
                else
                {
                    message = DT.Rows[0]["Description"].ToString();
                    clsProducts item = new clsProducts();
                    item.priceInTickets = Convert.ToInt32(DT.Rows[0]["PriceInTickets"]);
                    item.originalPriceInTickets = Convert.ToInt32(DT.Rows[0]["OriginalPriceInTickets"]);
                    item.productId = Convert.ToInt32(DT.Rows[0]["productId"]);
                    item.productName = DT.Rows[0]["Description"].ToString();
                    item.code = DT.Rows[0]["Code"].ToString();
                    item.barCode = DT.Rows[0]["BarCode"].ToString();
                    item.Quantity = 0;

                    if (!checkGiftAvailability(item, true))
                    {
                        message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 120);
                        log.Info("Ends-addGift(" + giftCode + ",codeType,message) as selected more gifts than available stock. Please crosscheck available quantity before proceeding");
                        log.LogMethodExit(false);
                        return false;
                    }
                    else
                    {
                        item.Quantity = 1;
                        productList.Add(item);
                    }
                }
                log.Debug("Ends-addGift(" + giftCode + ",codeType,message)");
                log.LogMethodExit(true);
                return true;
            }
            else
            {
                message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 111);
                log.Debug("Ends-addGift(" + giftCode + ",codeType,message) as Product not found");
                log.LogMethodExit(false);
                return false;
            }
        }

        /// <summary>
        /// remove gift
        /// </summary>
        /// <param name="productId">productId</param>
        public void removeGift(object productId)
        {
            log.LogMethodEntry(productId);
            if (productId == null || productId == DBNull.Value)
            {
                log.Info("Ends-removeGift(" + productId + ") as productId == null || productId == DBNull.Value");
                return;
            }

            clsProducts checkProd = productList.Find(delegate (clsProducts item) { return (item.productId == Convert.ToInt32(productId)); });
            productList.Remove(checkProd);
            log.Debug("Ends-removeGift(" + productId + ")");
            log.LogMethodExit();
        }

        /// <summary>
        /// update quantity
        /// </summary>
        /// <param name="productId">productId</param>
        /// <param name="newQuantity">newQuantity</param>
        /// <param name="message">message</param>
        public void updateQuantity(object productId, int newQuantity, ref string message)
        {
            log.LogMethodEntry(productId, newQuantity, message);
            if (productId == null || productId == DBNull.Value)
            {
                log.Info("Ends-updateQuantity(" + productId + ") as productId == null || productId == DBNull.Value");
                return;
            }
            clsProducts checkProd = productList.Find(delegate (clsProducts item) { return (item.productId == Convert.ToInt32(productId)); });
            if (newQuantity <= checkProd.Quantity)
                checkProd.Quantity = newQuantity;
            else
            {
                int savQty = checkProd.Quantity;
                while (checkProd.Quantity < newQuantity)
                    if (!addGift(checkProd.code, 'C', ref message))
                        break;
            }
            log.Debug("Ends-updateQuantity(" + productId + "," + newQuantity + ",message)");
            log.LogMethodExit();
        }

        /// <summary>
        /// update Quantity
        /// </summary>
        /// <param name="barCode">barCode</param>
        /// <param name="newQuantity">newQuantity</param>
        /// <param name="message">message</param>
        public void updateQuantity(string barCode, int newQuantity, ref string message)
        {
            log.LogMethodEntry(barCode, newQuantity, message);
            clsProducts checkProd = productList.Find(delegate (clsProducts item) { return (((item.barCode == null || item.barCode.ToString() == "") ? item.code : item.barCode) == barCode); });
            if (checkProd == null)
            {
                message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 111); //"Product not found";
                log.Info("Ends-updateQuantity(" + barCode + "," + newQuantity + ",message) as Product not found");
                return;
            }

            if (newQuantity <= checkProd.Quantity)
                checkProd.Quantity = newQuantity;
            else
            {
                int savQty = checkProd.Quantity;
                while (checkProd.Quantity < newQuantity)
                    if (!addGift(checkProd.code, 'C', ref message))
                        break;
            }

            message = checkProd.productName + ": Quantity updated to " + checkProd.Quantity.ToString();
            log.Info("updateQuantity(" + barCode + "," + newQuantity + ",message) -" + message);
            log.Debug("Ends-updateQuantity(" + barCode + "," + newQuantity + ",message)");
            log.LogMethodExit();
        }

        /// <summary>
        /// get Total Tickets
        /// </summary>
        /// <returns></returns>
        public int getTotalTickets()
        {
            log.LogMethodEntry();
            int etickets = getETickets();
            int physicalTickets = getPhysicalTickets() + getCurrencyTickets();
            log.Debug("Ends-getTotalTickets()");
            int totalTickets = ManualTickets + etickets + physicalTickets + getGraceTickets(etickets + physicalTickets + ManualTickets);
            log.LogMethodExit(totalTickets);
            return totalTickets;
        }

        /// <summary>
        /// get Currency Tickets
        /// </summary>
        /// <returns></returns>
        public int getCurrencyTickets()
        {
            log.LogMethodEntry();
            int tickets = 0;
            foreach (clsRedemptionCurrency item in currencyList)
            {
                if (item.redemptionCurrencyRuleId > -1)
                {
                    tickets += Convert.ToInt32(item.redemptionRuleTicketDelta) * item.quantity;
                }
                tickets += item.ValueInTickets * item.quantity;
            }
            log.LogMethodExit(tickets);
            return tickets;
        }

        /// <summary>
        /// get ETickets
        /// </summary>
        /// <returns></returns>
        public int getETickets()
        {
            log.LogMethodEntry();
            int tickets = 0;
            foreach (clsCards item in cardList)
                tickets += item.Tickets;

            log.LogMethodExit(tickets);
            return tickets;
        }

        /// <summary>
        /// get Physical Tickets
        /// </summary>
        /// <returns></returns>
        public int getPhysicalTickets()
        {
            log.LogMethodEntry();
            int tickets = 0;

            foreach (clsScanTickets item in scanTicketList)
                tickets += item.Tickets;

            log.LogMethodExit(tickets);
            return tickets;
        }

        /// <summary>
        /// get Manual Tickets
        /// </summary>
        /// <returns></returns>
        public int getManualTickets()
        {
            log.LogMethodEntry();
            log.LogMethodExit(ManualTickets);
            return ManualTickets;
        }

        /// <summary>
        /// get Total Redeemed
        /// </summary>
        /// <returns></returns>
        public int getTotalRedeemed()
        {
            log.LogMethodEntry();
            int redeemed = 0;
            foreach (clsProducts item in productList)
                redeemed += item.Quantity * item.priceInTickets;

            log.LogMethodExit(redeemed);
            return redeemed;
        }

        /// <summary>
        /// redeem Gifts
        /// </summary>
        /// <param name="message">message</param>
        /// <returns></returns>
        public bool redeemGifts(ref string message)
        {
            log.LogMethodEntry(message);
            if (ParafaitEnv.ALLOW_REDEMPTION_WITHOUT_CARD == "N")
            {
                if (cardList.Count == 0)
                {
                    message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 475);
                    log.Info("Ends-redeemGifts() - Please scan a card for redeeming gifts");
                    log.LogMethodExit(false);
                    return false;
                }
            }

            if (productList.Count == 0)
            {
                message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 119);
                log.Info("Ends-redeemGifts() - Please select gift(s) before saving");
                log.LogMethodExit(false);
                return false;
            }

            if (string.IsNullOrEmpty(createdFromSuspendId) == false)
            {
                if (null == _utilities.executeScalar("select 1 from SuspendedRedemption where data = @id",
                                         new SqlParameter("@id", createdFromSuspendId)))
                {
                    message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 1392); //"Suspended redemption is already processed";
                    log.Error("Ends-redeemGifts() - Suspended redemption is already processed");
                    log.LogMethodExit(false);
                    return false;
                }
            }

            foreach (clsProducts item in productList)
            {
                if (item.Quantity == 0 || item.Quantity.ToString().Trim() == "")
                {
                    message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 146);
                    log.Info("Ends- redeemGifts() gift quantity can not be zero");
                    log.LogMethodExit(false);
                    return false;
                }
                if (!checkGiftAvailability(item, false))
                {
                    message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 120);
                    log.Info("Starts-redeemGifts() as have selected more gifts than available stock. Please crosscheck available quantity before proceeding");
                    log.LogMethodExit(false);
                    return false;
                }
            }

            if (getTotalRedeemed() > getTotalTickets())
            {
                message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 121, POSStatic.TicketTermVariant, POSStatic.TicketTermVariant);
                log.Debug("Starts-redeemGifts() as Not enough " + POSStatic.TicketTermVariant + " for selected gifts. Please remove gifts or add " + POSStatic.TicketTermVariant + " to proceed.");
                log.LogMethodExit(false);
                return false;
            }

            int mgrApprovalLimitForRedemption = 0;
            try
            {
                mgrApprovalLimitForRedemption = Convert.ToInt32(_utilities.getParafaitDefaults("REDEMPTION_LIMIT_FOR_MANAGER_APPROVAL"));
            }
            catch { mgrApprovalLimitForRedemption = 0; }
            if ((getTotalRedeemed() > mgrApprovalLimitForRedemption && mgrApprovalLimitForRedemption != 0 && _utilities.ParafaitEnv.ManagerId == -1))
            {

                if (!authenticateManager(ref _utilities.ParafaitEnv.ManagerId))
                {
                    message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 268);
                    log.Debug("Starts-redeemGifts() " + message);
                    log.LogMethodExit(false);
                    return false;
                }
            }

            int redemptionTransactionTicketLimit = 0;
            try
            {
                redemptionTransactionTicketLimit = Convert.ToInt32(_utilities.getParafaitDefaults("REDEMPTION_TRANSACTION_TICKET_LIMIT"));
            }
            catch
            {
                redemptionTransactionTicketLimit = 0;
            }

            if (redemptionTransactionTicketLimit > 0 && getTotalRedeemed() > redemptionTransactionTicketLimit)
            {
                message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 1438, getTotalRedeemed(), redemptionTransactionTicketLimit);
                log.Debug("Starts-redeemGifts() " + message);
                log.LogMethodExit(false);
                return false;

            }

            bool no_db_error = UpdateDatabase(ref message);
            if (no_db_error)
            {
                message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 122);
                log.Info("redeemGifts() - Save Successfull");
                productList.Clear();
                cardList.Clear();
                scanTicketList.Clear();
                currencyList.Clear();

                if (POSStatic.AUTO_PRINT_REDEMPTION_RECEIPT)
                    PrintRedemptionReceipt.Print(_RedemptionId, _ScreenNumber);
            }
            _utilities.ParafaitEnv.ManagerId = -1;
            log.Debug("Ends-redeemGifts()");
            log.LogMethodExit(no_db_error);
            return no_db_error;
        }

        private bool checkGiftAvailability(clsProducts product, bool BeforeAdding)
        {
            log.LogMethodEntry(product, BeforeAdding);
            if (ParafaitEnv.ALLOW_TRANSACTION_ON_ZERO_STOCK.Equals("Y"))
            {
                log.Debug("Ends-checkGiftAvailability(product,BeforeAdding) as ALLOW_TRANSACTION_ON_ZERO_STOCK is Yes");
                log.LogMethodExit(true);
                return true;
            }
            int numberOfGiftsAvailable = 0;

            DataTable dt = GetGiftDetails(product.productId);
            numberOfGiftsAvailable = Convert.ToInt32(dt.Rows[0]["quantity"]);

            if (BeforeAdding)
            {
                if (numberOfGiftsAvailable > product.Quantity)
                {
                    log.Debug("Ends-checkGiftAvailability(product,BeforeAdding) as numberOfGiftsAvailable > product.Quantity");
                    log.LogMethodExit(true);
                    return true;
                }
                else
                {
                    log.Debug("Ends-checkGiftAvailability(product,BeforeAdding)");
                    log.LogMethodExit(false);
                    return false;
                }
            }
            else
            {
                if (numberOfGiftsAvailable >= product.Quantity)
                {
                    log.Debug("Ends-checkGiftAvailability(product,BeforeAdding)");
                    log.LogMethodExit(true);
                    return true;
                }
                else
                {
                    log.Debug("Ends-checkGiftAvailability(product,BeforeAdding)");
                    log.LogMethodExit(false);
                    return false;
                }
            }
        }

        /// <summary>
        /// get Grace Tickets
        /// </summary>
        /// <param name="Tickets">Tickets</param>
        /// <returns></returns>
        int getGraceTickets(int Tickets)
        {
            log.LogMethodEntry(Tickets);
            if (Tickets > 0)
            {
                if (POSStatic.REDEMPTION_GRACE_TICKETS > 0)
                    return POSStatic.REDEMPTION_GRACE_TICKETS;
                else if (POSStatic.REDEMPTION_GRACE_TICKETS_PERCENTAGE > 0)
                    return Convert.ToInt32(POSStatic.REDEMPTION_GRACE_TICKETS_PERCENTAGE * Tickets / 100);
            }

            log.Debug("Ends-getGraceTickets(" + Tickets + ")");
            log.LogMethodExit(0);
            return 0;
        }

        /// <summary>
        /// Returns order Number
        /// </summary>
        /// <param name="POSMachineId">POSMachineId</param>
        /// <param name="SQLTrx">SQLTrx</param>
        /// <returns></returns>
        public string GetNextRedemptionOrderNo(int POSMachineId, SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(POSMachineId, SQLTrx);

            SqlCommand cmd = _utilities.getCommand(SQLTrx);

            cmd.CommandText = @"declare @value varchar(20)
                                exec GetNextSeqValue N'RedemptionOrder', @value out, " + POSMachineId.ToString() + @"
                                select @value";
            try
            {
                object o = cmd.ExecuteScalar();
                if (o != null)
                {
                    log.LogMethodExit(o.ToString());
                    return (o.ToString());
                }
                else
                {
                    log.LogMethodExit("-1");
                    return "-1";
                }
            }
            catch (Exception ex)
            {
                log.Error("Unable to execute query on RedemptionOrder! ", ex);
                log.LogMethodExit("-1");
                return "-1";
            }
        }

        private bool UpdateDatabase(ref string message)
        {
            log.LogMethodEntry(message);
            int customer_tickets = 0;
            int etickets_to_be_allocated = 0;
            int physical_tickets_redeemed = 0;
            int GraceTickets = 0;

            int manualTicketRedeemed = 0, currencyTicketRedeemed = 0, receiptTicketRedeemed = 0;

            SqlCommand cmd = _utilities.getCommand(_utilities.createConnection().BeginTransaction());
            SqlTransaction cmd_trx = cmd.Transaction;

            try
            {
                int redeemed_points = getTotalRedeemed();
                int tickets_receipt = getPhysicalTickets();
                int tickets_currency = getCurrencyTickets();
                int tickets_total_physical = tickets_receipt + tickets_currency + ManualTickets;
                refreshCardList(); // refresh latest tickets
                int tickets_card = getETickets();

                if (redeemed_points > (tickets_card + tickets_total_physical + getGraceTickets(tickets_card + tickets_total_physical)))
                {
                    message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 121);
                    log.Info("Ends-UpdateDatabase(message) as redeemed_points > tickets_card + tickets_physical");
                    log.LogMethodExit(false);
                    return false;
                }

                RedemptionBL redemptionBL = new RedemptionBL(_utilities.ExecutionContext);
                try
                {
                    redemptionBL.PerDayLimitCheckForManualTickets(ManualTickets);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    message = ex.Message;
                    log.LogMethodExit(false);
                    return false;
                }

                foreach (clsScanTickets item in scanTicketList)
                {
                    if (_utilities.executeScalar("select top 1 1 from ManualTicketReceipts where ManualTicketReceiptNo = @receiptNo and BalanceTickets = 0",
                                                  new SqlParameter("@receiptNo", item.barCode)) != null)
                    {
                        message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 112);
                        log.Info("Ends-UpdateDatabase(message) as Receipt already used");
                        log.LogMethodExit(false);
                        return false;
                    }
                }

                ticketSourceInfoObj.Clear();
               

                if (getManualTickets() > 0)
                {
                    TicketSourceInfo ticketSourceObj = new TicketSourceInfo();
                    ticketSourceObj.ticketSource = "Manual";
                    ticketSourceObj.ticketValue = getManualTickets();
                    ticketSourceObj.balanceTickets = ticketSourceObj.ticketValue;
                    ticketSourceObj.sourceCurrencyRuleId = -1;
                    ticketSourceInfoObj.Add(ticketSourceObj);
                }
                if (currencyList != null && currencyList.Count > 0)
                {
                    foreach (clsRedemptionCurrency currencyEntry in currencyList)
                    {
                        TicketSourceInfo ticketSourceObj = new TicketSourceInfo();

                        if (currencyEntry.currencyId != -1)
                        {
                            ticketSourceObj.ticketSource = "Currency";
                            ticketSourceObj.ticketValue = currencyEntry.ValueInTickets * currencyEntry.quantity;
                            ticketSourceObj.balanceTickets = ticketSourceObj.ticketValue;
                            ticketSourceObj.currencyId = currencyEntry.currencyId;
                            ticketSourceObj.currencyValueInTickets = currencyEntry.ValueInTickets;
                            ticketSourceObj.currencyQuantity = currencyEntry.quantity;
                            ticketSourceObj.sourceCurrencyRuleId = (int)(currencyEntry.sourceCurrencyRuleId);
                            //ticketSourceInfoObj.Add(ticketSourceObj);
                        }
                        else
                        {
                            ticketSourceObj.ticketSource = "RedemptionCurrencyRule";
                            ticketSourceObj.ticketValue = (int)currencyEntry.redemptionRuleTicketDelta * currencyEntry.quantity;
                            ticketSourceObj.balanceTickets = ticketSourceObj.ticketValue;
                            ticketSourceObj.currencyId = currencyEntry.currencyId;
                            ticketSourceObj.currencyValueInTickets = currencyEntry.ValueInTickets;
                            ticketSourceObj.currencyQuantity = currencyEntry.quantity;
                            ticketSourceObj.redemptionCurrencyRuleId = currencyEntry.redemptionCurrencyRuleId;
                            ticketSourceObj.redemptionRuleTicketDelta = currencyEntry.redemptionRuleTicketDelta;
                            ticketSourceObj.sourceCurrencyRuleId = (int)(currencyEntry.sourceCurrencyRuleId);
                            ticketSourceObj.redemptionCurrencyRuleName = currencyEntry.redemptionCurrencyRuleName;
                        }
                        ticketSourceInfoObj.Add(ticketSourceObj);
                    }
                }
                foreach (clsScanTickets item in scanTicketList)
                {
                    TicketSourceInfo ticketSourceObj = new TicketSourceInfo();
                    ticketSourceObj.ticketSource = "Receipt";
                    ticketSourceObj.ticketValue = item.Tickets;
                    ticketSourceObj.balanceTickets = ticketSourceObj.ticketValue;
                    ticketSourceObj.receiptNo = item.barCode;
                    ticketSourceObj.sourceCurrencyRuleId = -1;
                    ticketSourceInfoObj.Add(ticketSourceObj);
                }
                if (cardList != null && cardList.Count > 0)
                {
                    foreach (clsCards cardEntry in cardList)
                    {
                        TicketSourceInfo ticketSourceObj = new TicketSourceInfo();
                        ticketSourceObj.ticketSource = "Cards";
                        ticketSourceObj.ticketValue = cardEntry.Tickets;
                        ticketSourceObj.balanceTickets = ticketSourceObj.ticketValue;
                        ticketSourceObj.cardId = cardEntry.cardId;
                        ticketSourceObj.sourceCurrencyRuleId = -1;
                        ticketSourceInfoObj.Add(ticketSourceObj);
                    }
                }


                int balancePhysicalTickets;
                if (redeemed_points <= tickets_total_physical)
                {
                    etickets_to_be_allocated = 0;
                    physical_tickets_redeemed = redeemed_points;
                    if (ManualTickets >= physical_tickets_redeemed)
                    {
                        manualTicketRedeemed = physical_tickets_redeemed;
                    }
                    else if (ManualTickets + tickets_currency >= physical_tickets_redeemed)
                    {
                        manualTicketRedeemed = ManualTickets;
                        currencyTicketRedeemed = physical_tickets_redeemed - manualTicketRedeemed;
                    }
                    else
                    {
                        manualTicketRedeemed = ManualTickets;
                        currencyTicketRedeemed = tickets_currency;
                        receiptTicketRedeemed = physical_tickets_redeemed - manualTicketRedeemed - currencyTicketRedeemed;
                    }

                    balancePhysicalTickets = tickets_total_physical - physical_tickets_redeemed;
                }
                else if (redeemed_points <= tickets_total_physical + tickets_card)
                {
                    etickets_to_be_allocated = redeemed_points - tickets_total_physical;
                    physical_tickets_redeemed = tickets_total_physical;

                    if (ManualTickets >= physical_tickets_redeemed)
                    {
                        manualTicketRedeemed = physical_tickets_redeemed;
                    }
                    else if (ManualTickets + tickets_currency >= physical_tickets_redeemed)
                    {
                        manualTicketRedeemed = ManualTickets;
                        currencyTicketRedeemed = physical_tickets_redeemed - manualTicketRedeemed;
                    }
                    else
                    {
                        manualTicketRedeemed = ManualTickets;
                        currencyTicketRedeemed = tickets_currency;
                        receiptTicketRedeemed = physical_tickets_redeemed - manualTicketRedeemed - currencyTicketRedeemed;
                    }

                    balancePhysicalTickets = 0;
                }
                else // redemption using grace tickets
                {
                    etickets_to_be_allocated = tickets_card;
                    physical_tickets_redeemed = tickets_total_physical;

                    if (ManualTickets >= physical_tickets_redeemed)
                    {
                        manualTicketRedeemed = physical_tickets_redeemed;
                    }
                    else if (ManualTickets + tickets_currency >= physical_tickets_redeemed)
                    {
                        manualTicketRedeemed = ManualTickets;
                        currencyTicketRedeemed = physical_tickets_redeemed - manualTicketRedeemed;
                    }
                    else
                    {
                        manualTicketRedeemed = ManualTickets;
                        currencyTicketRedeemed = tickets_currency;
                        receiptTicketRedeemed = physical_tickets_redeemed - manualTicketRedeemed - currencyTicketRedeemed;
                    }

                    GraceTickets = redeemed_points - tickets_total_physical - tickets_card;
                    if (GraceTickets > 0)
                    {
                        TicketSourceInfo ticketSourceObj = new TicketSourceInfo();
                        ticketSourceObj.ticketSource = "Grace";
                        ticketSourceObj.ticketValue = GraceTickets;
                        ticketSourceObj.balanceTickets = ticketSourceObj.ticketValue;
                        ticketSourceObj.sourceCurrencyRuleId = -1;
                        ticketSourceInfoObj.Add(ticketSourceObj);
                    }
                    balancePhysicalTickets = 0;
                }

                cmd.CommandText = "Insert into Redemption (card_id, primary_card_number, ReceiptTickets, manual_tickets, eTickets, GraceTickets, CurrencyTickets, redeemed_date, LastUpdatedBy, Source, RedemptionOrderNo, LastUpdateDate, OrderCompletedDate, OrderDeliveredDate, RedemptionStatus, CreatedBy, CreationDate, customerId, posmachineId) " +
                                  " Values(@card_id, @primary_card_number, @receiptTickets, @manual_tickets, @eTickets, @GraceTickets, @currencyTickets, getdate(), @LastUpdatedBy, @Source, @RedemptionOrderNo, getdate(), getdate(), getdate(), @RedemptionStatus, @CreatedBy, getdate(), @CustomerId, @PosMachineId); SELECT @@IDENTITY";

                if (cardList.Count > 0)
                {
                    cmd.Parameters.AddWithValue("@card_id", cardList[0].cardId);
                    cmd.Parameters.AddWithValue("@primary_card_number", cardList[0].cardNumber);
                    if (cardList[0].customerId > -1)
                    { cmd.Parameters.AddWithValue("@CustomerId", cardList[0].customerId); }
                    else
                    { cmd.Parameters.AddWithValue("@CustomerId", DBNull.Value); }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@card_id", DBNull.Value);
                    cmd.Parameters.AddWithValue("@primary_card_number", "");
                    cmd.Parameters.AddWithValue("@CustomerId", DBNull.Value);
                }

                cmd.Parameters.AddWithValue("@ReceiptTickets", receiptTicketRedeemed);
                cmd.Parameters.AddWithValue("@currencyTickets", currencyTicketRedeemed);
                cmd.Parameters.AddWithValue("@manual_tickets", manualTicketRedeemed);
                cmd.Parameters.AddWithValue("@eTickets", etickets_to_be_allocated);
                cmd.Parameters.AddWithValue("@Source", "POS Redemption");
                cmd.Parameters.AddWithValue("@GraceTickets", GraceTickets);
                cmd.Parameters.AddWithValue("@LastUpdatedBy", ParafaitEnv.LoginID);
                cmd.Parameters.AddWithValue("@RedemptionOrderNo", GetNextRedemptionOrderNo(ParafaitEnv.POSMachineId, null));
                cmd.Parameters.AddWithValue("@RedemptionStatus", "DELIVERED");
                cmd.Parameters.AddWithValue("@CreatedBy", ParafaitEnv.LoginID);
                cmd.Parameters.AddWithValue("@PosMachineId", ParafaitEnv.POSMachineId);
                int redemptionId = Convert.ToInt32((Decimal)cmd.ExecuteScalar());

                InsertManualTicketReceipts(redemptionId, cmd_trx);

                //Insert into redemption gifts table
                cmd.CommandText = "Insert into Redemption_gifts (redemption_id, gift_code, productid, locationId, Tickets, GraceTickets, OriginalPriceInTickets, LastUpdatedBy, LastUpdateDate, CreationDate, CreatedBy )  " +
                                        @" select @redemption, Code, productid, isnull(pos.InventoryLocationId, outboundLocationId), @Tickets, @GraceTickets , @OriginalPriceInTickets, @LastUpdatedBy, getdate(), getdate(), @CreatedBy
                                            from product left outer join posMachines pos on POSMachineId = @POSMachine
                                            where ProductId = @productId; SELECT @@IDENTITY";
                int balGraceTickets = GraceTickets;
                foreach (clsProducts item in productList)
                {
                    int price = 0;
                    int grace = 0;
                    for (int i = 0; i < item.Quantity; i++)
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@productId", item.productId);
                        cmd.Parameters.AddWithValue("@redemption", redemptionId);
                        cmd.Parameters.AddWithValue("@POSMachine", ParafaitEnv.POSMachineId);
                        cmd.Parameters.AddWithValue("@OriginalPriceInTickets", item.originalPriceInTickets);
                        cmd.Parameters.AddWithValue("@LastUpdatedBy", ParafaitEnv.LoginID);
                        cmd.Parameters.AddWithValue("@CreatedBy", ParafaitEnv.LoginID);
                        if (GraceTickets > 0)
                        {
                            price = item.priceInTickets;
                            grace = 0;
                            if (item.Equals(productList[productList.Count - 1]) && i == item.Quantity - 1) // last line
                            {
                                price = price - balGraceTickets;
                                grace = balGraceTickets;
                            }
                            else
                            {
                                grace = price * GraceTickets / redeemed_points;
                                price = price - grace;
                                balGraceTickets -= grace;
                            }
                            cmd.Parameters.AddWithValue("@Tickets", price);
                            cmd.Parameters.AddWithValue("@GraceTickets", grace);
                        }
                        else
                        {
                            price = item.priceInTickets;
                            grace = 0;
                            cmd.Parameters.AddWithValue("@Tickets", price);
                            cmd.Parameters.AddWithValue("@GraceTickets", grace);
                        }

                        try
                        {
                            int redemptionGiftId = Convert.ToInt32((Decimal)cmd.ExecuteScalar());
                            SqlCommand cmdstock = _utilities.getCommand(cmd_trx);
                            Inventory.updateStock(item.code, cmdstock, 1, ParafaitEnv.POSMachineId, ParafaitEnv.LoginID,redemptionId, redemptionGiftId, 0, 0, "", ParafaitEnv.ExecutionContext.SiteId,-1,-1 ,"Redemption");
                            ticketSourceInfoObj = UpdateGiftTicketSourceInfo(redemptionId, redemptionGiftId, price, grace, ticketSourceInfoObj, cmd_trx);
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            message = ex.Message;
                            cmd_trx.Rollback();
                            return false;
                        }
                    }
                }

                cmd.CommandText = "Insert into Redemption_cards (redemption_id, card_number, card_id, ticket_count, LastUpdateDate, LastUpdatedBy, CreationDate, CreatedBy ) " +
                                        " Values (@redemption_id, @card_no, @card_id, @ticket_count, getdate(), @LastUpdatedBy, getdate(), @CreatedBy)";
                int ticketsUsed = 0;
                CreditPlus creditPlus = new CreditPlus(_utilities);
                for (int i = 0; i < cardList.Count; i++)
                {
                    if (etickets_to_be_allocated > 0)
                    {
                        customer_tickets = 0;
                        customer_tickets = cardList[i].Tickets;
                        if (customer_tickets <= 0)
                            continue;
                        if (customer_tickets - etickets_to_be_allocated >= 0)
                        {
                            customer_tickets -= etickets_to_be_allocated;
                            ticketsUsed = etickets_to_be_allocated;
                            etickets_to_be_allocated = 0;
                        }
                        else
                        {
                            etickets_to_be_allocated -= customer_tickets;
                            ticketsUsed = customer_tickets;
                            customer_tickets = 0;
                        }

                        try
                        {
                            creditPlus.deductCreditPlusTicketsLoyaltyPoints(cardList[i].cardNumber, ticketsUsed, 0, cmd_trx);

                            //Insert into redemption cards table
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@card_no", cardList[i].cardNumber);
                            cmd.Parameters.AddWithValue("@redemption_id", redemptionId);
                            cmd.Parameters.AddWithValue("@card_id", cardList[i].cardId);
                            cmd.Parameters.AddWithValue("@ticket_count", ticketsUsed);
                            cmd.Parameters.AddWithValue("@LastUpdatedBy", ParafaitEnv.LoginID);
                            cmd.Parameters.AddWithValue("@CreatedBy", ParafaitEnv.LoginID);
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception Ex)
                        {
                            message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 123, Ex.Message);
                            log.Fatal("Ends-UpdateDatabase(message) due to exception in creating redemption cards information error: " + Ex.Message);
                            cmd_trx.Rollback();
                            return false;
                        }
                    }
                }

                cmd.CommandText = "Insert into Redemption_cards (redemption_id, CurrencyId, CurrencyQuantity, ticket_count, card_number, LastUpdateDate, LastUpdatedBy, CreationDate, CreatedBy) " +
                                       " Values (@redemption_id, @CurrencyId, @CurQuantity, @ticket_count, '', getdate(), @LastUpdatedBy, getdate(), @CreatedBy)";
                foreach (clsRedemptionCurrency item in currencyList)
                {
                    customer_tickets = 0;
                    customer_tickets = item.ValueInTickets * item.quantity;
                    if (customer_tickets <= 0)
                        continue;

                    try
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@redemption_id", redemptionId);
                        cmd.Parameters.AddWithValue("@CurrencyId", item.currencyId);
                        cmd.Parameters.AddWithValue("@CurQuantity", item.quantity);
                        cmd.Parameters.AddWithValue("@ticket_count", customer_tickets);
                        cmd.Parameters.AddWithValue("@LastUpdatedBy", ParafaitEnv.LoginID);
                        cmd.Parameters.AddWithValue("@CreatedBy", ParafaitEnv.LoginID);
                        cmd.ExecuteNonQuery();
                        if (item.productId != DBNull.Value && item.currencyId > -1)
                        {
                            UpdateRedemptionCurrencyInventory(item, cmd_trx);
                        }
                    }
                    catch (Exception Ex)
                    {
                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(_utilities.ExecutionContext, 124, Ex.Message),
                                                    MessageContainerList.GetMessage(_utilities.ExecutionContext, "Save Information")
                                                    + MessageContainerList.GetMessage(_utilities.ExecutionContext, 2693, _ScreenNumber));
                        log.Fatal("Ends-UpdateDatabase(message) due to exception " + Ex.Message);
                        cmd_trx.Rollback();
                        return false;
                    }
                }

                InsertManualTicketReceipts(redemptionId, cmd_trx);

                if (string.IsNullOrEmpty(createdFromSuspendId) == false)
                {
                    /*_utilities.executeScalar("delete from eventLog where data = @id",
                                             cmd_trx,
                                             new SqlParameter("@id", createdFromSuspendId));*/
                    _utilities.executeScalar("delete from SuspendedRedemption where data = @id",
                                              cmd_trx,
                                              new SqlParameter("@id", createdFromSuspendId));
                }

                //cmd_trx.Commit();
                //_RedemptionId = redemptionId;

                //message = MessageContainerList.GetMessage(_utilities.ExecutionContext,122);
                bool okayToCommit = true;
                if (balancePhysicalTickets > 0)
                {
                    bool printTickets = false;
                    if (cardList.Count > 0)
                    {
                        if (_utilities.getParafaitDefaults("AUTO_LOAD_BALANCE_TICKETS_TO_CARD").Equals("Y")
                            || ((tickets_receipt - receiptTicketRedeemed) > 0)
                            || POSUtils.ParafaitMessageBox("Do you want to load " + balancePhysicalTickets.ToString() + " balance physical " + POSStatic.TicketTermVariant + " to card?",
                                                             MessageContainerList.GetMessage(_utilities.ExecutionContext, "Load Balance") + MessageContainerList.GetMessage(_utilities.ExecutionContext, 2693, _ScreenNumber),
                                                             MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.Yes)
                        {
                            TaskProcs tp = new TaskProcs(_utilities);
                            //SqlCommand sqlCmd = _utilities.getCommand();

                            //sqlCmd.CommandText = "select requires_manager_approval from task_type where task_type = @task_type";
                            //sqlCmd.Parameters.AddWithValue("@task_type", "LOADTICKETS");
                            string cmdText = "select requires_manager_approval from task_type where task_type = @task_type";
                            SqlParameter sqlParamTaskType = new SqlParameter("@task_type", "LOADTICKETS");
                            //if (sqlCmd.ExecuteScalar().ToString() == "Y" && statdata.ManagerId == -1)
                            //{
                            int mgrApprovalLimit = 0;
                            try
                            {
                                mgrApprovalLimit = Convert.ToInt32(_utilities.getParafaitDefaults("LOAD_TICKET_LIMIT_FOR_MANAGER_APPROVAL"));
                            }
                            catch { mgrApprovalLimit = 0; }
                            //  if (balancePhysicalTickets > mgrApprovalLimit )
                            if ((balancePhysicalTickets > mgrApprovalLimit && mgrApprovalLimit != 0 && _utilities.ParafaitEnv.ManagerId == -1) || (mgrApprovalLimit == 0 && (_utilities.executeScalar(cmdText, sqlParamTaskType).ToString() == "Y" && _utilities.ParafaitEnv.ManagerId == -1)))
                            {
                                if (!authenticateManager(ref _utilities.ParafaitEnv.ManagerId))
                                {
                                    message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 268);
                                    okayToCommit = false;
                                    throw new Exception(message);
                                }
                            }

                            CreateRedemptionTicketAllocation(redemptionId, balancePhysicalTickets, cardList, ticketSourceInfoObj, cmd_trx);
                            //if (tp.loadTickets(new POSCore.Card((int)cardList[0].cardId, "", _utilities), balancePhysicalTickets, "Redemption balance tickets", manualReceipts, ref message, cmd_trx))
                            int originalMangerId = _utilities.ParafaitEnv.ManagerId;
                            if (addManualTicketMangerApprovalDetails != null)
                            {
                                _utilities.ParafaitEnv.ManagerId = addManualTicketMangerApprovalDetails.Item3;
                            }
                            if (tp.loadTickets(new Card((int)cardList[0].cardId, "", _utilities), balancePhysicalTickets, "Redemption balance tickets", redemptionId, ref message, cmd_trx))
                            {
                                message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 36, POSStatic.TicketTermVariant);

                            }
                            else
                            {
                                okayToCommit = false;
                                POSUtils.ParafaitMessageBox(message,
                                                           MessageContainerList.GetMessage(_utilities.ExecutionContext, "Load Balance")
                                                           + MessageContainerList.GetMessage(_utilities.ExecutionContext, 2693, _ScreenNumber), MessageBoxButtons.OK);
                            }
                            _utilities.ParafaitEnv.ManagerId = originalMangerId;

                        }
                        else
                            printTickets = true;
                    }
                    else
                        printTickets = true;

                    if (printTickets && okayToCommit)
                    {
                        if (_utilities.getParafaitDefaults("AUTO_PRINT_BALANCE_TICKETS").Equals("Y")
                            || ((tickets_receipt - receiptTicketRedeemed) > 0)
                            || POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(_utilities.ExecutionContext, 125, balancePhysicalTickets, POSStatic.TicketTermVariant),
                                                          MessageContainerList.GetMessage(_utilities.ExecutionContext, "Print Balance?") + MessageContainerList.GetMessage(_utilities.ExecutionContext, 2693, _ScreenNumber),
                                                          MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.Yes)
                        {
                            try
                            {
                                CreateRedemptionTicketAllocation(redemptionId, balancePhysicalTickets, cardList, ticketSourceInfoObj, cmd_trx);
                                if (printRealTicketReceipt(redemptionId, balancePhysicalTickets, ref message, cmd_trx) == -1)
                                    okayToCommit = false;
                            }
                            catch (Exception ex)
                            {
                                okayToCommit = false;
                                message = ex.Message;
                                log.Fatal("Ends-UpdateDatabase(message) due to exception " + ex.Message);
                            }
                        }
                    }
                }
                if (okayToCommit)
                {
                    cmd_trx.Commit();
                    _RedemptionId = redemptionId;
                    message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 122);
                }
                else
                {
                    cmd_trx.Rollback();
                    log.Fatal("Ends-UpdateDatabase(message) due to exception " + message);
                    return false;
                }
                addManualTicketMangerApprovalDetails = null;
                log.Debug("Ends-UpdateDatabase(message) ");
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception Ex)
            {
                cmd_trx.Rollback();
                log.Fatal("Ends-UpdateDatabase(message) due to exception " + Ex.Message);
                message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 126, Ex.Message);
                log.LogMethodExit(false);
                return false;
            }
            finally
            {
                _utilities.ParafaitEnv.ManagerId = -1;
            }
        }

        /// <summary>
        /// Update Redemption Currency Inventory
        /// </summary>
        /// <param name="currencyItem">currencyItem</param>
        /// <param name="sqlTrx">sqlTrx</param>
        public void UpdateRedemptionCurrencyInventory(clsRedemptionCurrency currencyItem, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(currencyItem, sqlTrx);
            SqlCommand cmd = _utilities.getCommand(sqlTrx);
            cmd.CommandText = "Update Inventory " +
                                            "Set Quantity = (Quantity + @qty), Lastupdated_userid = @lmid, timestamp = getdate() " +
                                            "where exists (select 1 from Product P left outer join posMachines pos on POSMachineId = @POSMachine " +
                                                "where P.ProductId = @prod_id " +
                                                "and P.ProductId = Inventory.ProductId " +
                                                "and isnull(pos.InventoryLocationId, P.DefaultLocationId) = Inventory.LocationId)";

            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@prod_id", currencyItem.productId);
            cmd.Parameters.AddWithValue("@qty", currencyItem.quantity);
            cmd.Parameters.AddWithValue("@lmid", ParafaitEnv.LoginID);
            cmd.Parameters.AddWithValue("@POSMachine", ParafaitEnv.POSMachineId);

            if (cmd.ExecuteNonQuery() == 0)
            {
                cmd.CommandText = "insert into Inventory (ProductId, LocationId, Quantity, Timestamp, LastUpdated_UserId) " +
                                    "select ProductId, isnull(pos.InventoryLocationId, P.DefaultLocationId), @qty, getdate(), @lmid " +
                                    "from Product P left outer join posMachines pos on POSMachineId = @POSMachine " +
                                   "where P.ProductId = @prod_id ";
                cmd.ExecuteNonQuery();
            }

            if (currencyItem.productId != DBNull.Value)
            {
                object locId = _utilities.executeScalar(@"select isnull(pos.InventoryLocationId, P.DefaultLocationId)
                                                                        from Product P 
                                                                            left outer join posMachines pos 
                                                                            on POSMachineId = @POSMachine
                                                                        where P.ProductId = @ProductId",
                                                           new SqlParameter("@ProductId", currencyItem.productId),
                                                           new SqlParameter("@POSMachine", ParafaitEnv.POSMachineId));

                Inventory.AdjustInventory(Inventory.AdjustmentTypes.TradeIn,
                                                    _utilities,
                                                    Convert.ToInt32(locId),
                                                    Convert.ToInt32(currencyItem.productId),
                                                    currencyItem.quantity,
                                                    ParafaitEnv.LoginID,
                                                    "Redemption Trade-In", null, sqlTrx);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Insert Manual Ticket Receipts
        /// </summary>
        /// <param name="redemptionId">redemptionId</param>
        /// <param name="SQLTrx">SQLTrx</param>
        /// <param name="clearList">clearList</param>
        public void InsertManualTicketReceipts(int redemptionId, SqlTransaction SQLTrx, bool clearList = true)
        {
            log.LogMethodEntry(redemptionId, SQLTrx, clearList);
            if (scanTicketList.Count > 0)
            {
                SqlCommand cmd = _utilities.getCommand(SQLTrx);
                string commandTextForInsertUpdate = @"insert into ManualTicketReceipts (redemption_id, ManualTicketReceiptNo, Tickets, BalanceTickets, LastUpdatedBy, LastUpdatedDate, issueDate) values (@redemptiondId, @receipt, @tickets, 0, @user, getdate(), getdate())";
                //cmd.CommandText = @"insert into ManualTicketReceipts (redemption_id, ManualTicketReceiptNo, Tickets, BalanceTickets, LastUpdatedBy, LastUpdatedDate) values (@redemptiondId, @receipt, @tickets, 0, @user, getdate());
                //                update ManualTicketReceipts set BalanceTickets = 0, LastUpdatedBy = @user, LastUpdatedDate = getdate() where ManualTicketReceiptNo = @receipt and redemption_id is null and BalanceTickets > 0";
                string commandTextForUpdate = @"update ManualTicketReceipts set redemption_id = @redemptiondId, BalanceTickets = 0, LastUpdatedBy = @user, LastUpdatedDate = getdate() where ManualTicketReceiptNo = @receipt";

                cmd.Parameters.AddWithValue("@receipt", "");
                cmd.Parameters.AddWithValue("@tickets", 0);
                cmd.Parameters.AddWithValue("@user", _utilities.ParafaitEnv.LoginID);
                cmd.Parameters.AddWithValue("@redemptiondId", redemptionId);

                foreach (clsScanTickets item in scanTicketList)
                {
                    if (_utilities.executeScalar("select top 1 1 from ManualTicketReceipts where ManualTicketReceiptNo = @receiptNo", SQLTrx,
                                                  new SqlParameter("@receiptNo", item.barCode)) != null)
                    {
                        cmd.CommandText = commandTextForUpdate;
                    }
                    else
                    {
                        cmd.CommandText = commandTextForInsertUpdate;
                    }
                    cmd.Parameters["@receipt"].Value = item.barCode;
                    cmd.Parameters["@tickets"].Value = item.Tickets;
                    cmd.ExecuteNonQuery();
                }
                if (clearList)
                    scanTicketList.Clear();
            }
            log.Debug("Ends-InsertManualTicketReceipts(" + redemptionId + ",sqlTrx) ");
            log.LogMethodExit();
        }

        /// <summary>
        /// insert Physical Receipt To DB
        /// </summary>
        /// <param name="sourceRedemptionId">sourceRedemptionId</param>
        /// <param name="receipt">receipt</param>
        /// <param name="tickets">tickets</param>
        /// <param name="sqlTrx">sqlTrx</param>
        /// <param name="issueDate">issueDate</param>
        /// <returns></returns>
        internal int insertPhysicalReceiptToDB(int sourceRedemptionId, string receipt, int tickets, SqlTransaction sqlTrx, DateTime? issueDate = null)
        {
            log.LogMethodEntry(sourceRedemptionId, receipt, tickets, sqlTrx, issueDate);
            int returnValue = -1;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@sourceRedemptionId", sourceRedemptionId));
                sqlParamList.Add(new SqlParameter("@receipt", receipt));
                sqlParamList.Add(new SqlParameter("@tickets", tickets));
                if (issueDate == null)
                    sqlParamList.Add(new SqlParameter("@issueDate", ServerDateTime.Now));
                else
                    sqlParamList.Add(new SqlParameter("@issueDate", issueDate));
                sqlParamList.Add(new SqlParameter("@user", _utilities.ParafaitEnv.LoginID));

                returnValue = Convert.ToInt32(_utilities.executeScalar(@"insert into ManualTicketReceipts (redemption_id, ManualTicketReceiptNo, Tickets, BalanceTickets , LastUpdatedBy, LastUpdatedDate, SourceRedemptionId, issueDate) values (null, @receipt, @tickets, @tickets, @user, getdate(),@sourceRedemptionId ,@issueDate); SELECT @@IDENTITY",
                                             sqlTrx, sqlParamList.ToArray()));
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(-1);
                return -1;
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// insert Suspended Redemption To DB
        /// </summary>
        /// <param name="Source">Source</param>
        /// <param name="Type">Type</param>
        /// <param name="Data">Data</param>
        /// <param name="Description">Description</param>
        /// <param name="Category">Category</param>
        /// <param name="Severity">Severity</param>
        /// <param name="Name">Name</param>
        /// <param name="Value">Value</param>
        /// <param name="Username">Username</param>
        /// <param name="POSMachine">POSMachine</param>
        /// <param name="SQLTrx">SQLTrx</param>
        internal void insertSuspendedRedemptionToDB(string Source, char Type, string Data, string Description, string Category, int Severity, string Name, string Value, string Username, string POSMachine, SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(Source, Type, Data, Description, Category, Severity, Name, Value, Username, POSMachine, SQLTrx);
            if (Description == null)
                Description = "";

            if (Data.Length > 500)
                Data = Data.Substring(0, 500);

            if (Description.Length > 2000)
                Description = Description.Substring(0, 2000);

            string CommandText = @"insert into SuspendedRedemption 
                                        (Source, Type, UserName, Computer, Data, Description, Timestamp, Category, Severity, Name, Value)
                                    values
                                        (@Source, @Type, @UserName, @Computer, @Data, @Description, getdate(), @Category, @Severity, @Name, @Value)";
            _utilities.executeNonQuery(CommandText,
                                        new SqlParameter("@Source", Source),
                                        new SqlParameter("@Type", Type),
                                        new SqlParameter("@Username", Username),
                                        new SqlParameter("@Computer", POSMachine),
                                        new SqlParameter("@Data", Data),
                                        new SqlParameter("@Description", Description),
                                        new SqlParameter("@Category", Category),
                                        new SqlParameter("@Severity", Severity),
                                        new SqlParameter("@Name", Name),
                                        new SqlParameter("@Value", Value));
            log.LogMethodExit();
        }

        /// <summary>
        /// print Real Ticket Receipt
        /// </summary>
        /// <param name="sourceRedemptionId">sourceRedemptionId</param>
        /// <param name="Tickets">Tickets</param>
        /// <param name="message">message</param>
        /// <param name="sqlTrx">sqlTrx</param>
        /// <param name="issueDate">issueDate</param>
        /// <returns></returns>
        public int printRealTicketReceipt(int sourceRedemptionId, int Tickets, ref string message, SqlTransaction sqlTrx, DateTime? issueDate = null)
        {
            log.LogMethodEntry(sourceRedemptionId, Tickets, message, sqlTrx, issueDate);
            try
            {
                if (Tickets > 0)
                {
                    string BarCodeText = string.Empty;

                    TicketStationFactory ticketStationFactory = new TicketStationFactory();
                    POSCounterTicketStationBL posCounterTicketStationBL = ticketStationFactory.GetPosCounterTicketStationObject();
                    if (posCounterTicketStationBL == null)
                    {
                        message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 2322);
                        throw new Exception(message);
                    }
                    else
                    {
                        BarCodeText = posCounterTicketStationBL.GenerateBarCode(Tickets);
                    }
                    //Image BarcodeImage = GenCode128.Code128Rendering.MakeBarcodeImage(BarCodeText, 1, true);                   
                    if (setupPrint(BarCodeText, Tickets, sqlTrx))
                    {
                        int newReceiptId = insertPhysicalReceiptToDB(sourceRedemptionId, BarCodeText, Tickets, sqlTrx, issueDate);
                        message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 143);
                        log.Info("Ends-printRealTicketReceipt(" + Tickets + ",message) as Ticket Receipt Printed");
                        return newReceiptId;
                    }
                    else
                    {
                        message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 144);
                        log.Info("Ends-printRealTicketReceipt(" + Tickets + ",message) as Receipt Print cancelled");
                        return -1;
                    }
                }
                else
                {
                    message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 145);
                    log.Info("Ends-printRealTicketReceipt(" + Tickets + ",message) as Nothing to Print");
                    return -1;
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                log.Fatal("Ends-printRealTicketReceipt(" + Tickets + ",message) due to exception " + ex.Message);
                log.LogMethodExit(-1);
                return -1;
            }
        }

        private bool setupPrint(string BarCodeText, int Tickets, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(BarCodeText, Tickets, sqlTrx);
            PrintDialog MyPrintDialog = new PrintDialog();
            MyPrintDialog.AllowCurrentPage = false;
            MyPrintDialog.AllowPrintToFile = false;
            MyPrintDialog.AllowSelection = false;
            MyPrintDialog.AllowSomePages = false;
            MyPrintDialog.PrintToFile = false;
            MyPrintDialog.ShowHelp = false;
            MyPrintDialog.ShowNetwork = false;
            MyPrintDialog.PrinterSettings.DefaultPageSettings.Landscape = false;

            if (ParafaitEnv.ShowPrintDialog == "Y")
            {
                if (MyPrintDialog.ShowDialog() != DialogResult.OK)
                {
                    log.Info("Ends-setupPrint(" + BarCodeText + "BarcodeImage," + Tickets + ") as print dialog was cancelled");
                    log.LogMethodExit(false);
                    return false;
                }
            }

            PrinterBL printerBL = new PrinterBL(POSStatic.Utilities.ExecutionContext);
            PrintDocument printDocument = new System.Drawing.Printing.PrintDocument();

            printDocument.DefaultPageSettings =
            MyPrintDialog.PrinterSettings.DefaultPageSettings;
            printDocument.DefaultPageSettings.Margins =
                             new Margins(20, 20, 20, 20);
            printDocument.PrinterSettings =
                                MyPrintDialog.PrinterSettings;
            int ticketReceiptTemplate = -1;
            try
            {
                ticketReceiptTemplate = Convert.ToInt32(_utilities.getParafaitDefaults("TICKET_VOUCHER_TEMPLATE"));
            }
            catch { ticketReceiptTemplate = -1; }

            if (ticketReceiptTemplate == -1)
            {

                printDocument.PrintPage += (sender, e) =>
                {
                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Center;

                    using (Graphics g = e.Graphics)
                    {
                        using (Font fnt = new Font("Arial", 10))
                        {
                            int weight = 1;
                            if (fnt.Size >= 16)
                                weight = 3;
                            else if (fnt.Size >= 12)
                                weight = 2;
                            //Image BarcodeImage = POSPrint.MakeBarcodeLibImage(weight, 24, BarcodeLib.TYPE.CODE128.ToString(), BarCodeText);
                            Image BarcodeImage = printerBL.MakeBarcodeLibImage(weight, 24, BarcodeLib.TYPE.CODE128.ToString(), BarCodeText);
                            int yLocation = 20;
                            if (ParafaitEnv.CompanyLogo != null)
                            {
                                int imgWidth = Math.Min(ParafaitEnv.CompanyLogo.Width, (int)printDocument.DefaultPageSettings.PrintableArea.Width);
                                int imgHeight = 180 * ParafaitEnv.CompanyLogo.Height / imgWidth;
                                g.DrawImage(ParafaitEnv.CompanyLogo, (printDocument.DefaultPageSettings.PrintableArea.Width - imgWidth) / 2, yLocation, imgWidth, imgHeight);
                                yLocation += imgHeight;
                            }

                            g.DrawString(ParafaitEnv.SiteName, fnt, Brushes.Black, new Rectangle(0, yLocation, (int)printDocument.DefaultPageSettings.PrintableArea.Width, 20), sf);
                            yLocation += 20;
                            g.DrawString("* " + POSStatic.TicketTermVariant + " " + MessageContainerList.GetMessage(_utilities.ExecutionContext, "Receipt") + " *", fnt, Brushes.Black, new Rectangle(0, yLocation, (int)printDocument.DefaultPageSettings.PrintableArea.Width, 20), sf);
                            yLocation += 30;
                            g.DrawString(ServerDateTime.Now.ToString("dd-MMM-yyyy h:mm:ss tt"), fnt, Brushes.Black, new Rectangle(0, yLocation, (int)printDocument.DefaultPageSettings.PrintableArea.Width, 20), sf);
                            yLocation += 20;
                            g.DrawString(ParafaitEnv.POSMachine + " / " + ParafaitEnv.LoginID, fnt, Brushes.Black, new Rectangle(0, yLocation, (int)printDocument.DefaultPageSettings.PrintableArea.Width, 20), sf);
                            yLocation += 20;
                            if (string.IsNullOrEmpty(_ScreenNumber) == false)
                            {
                                e.Graphics.DrawString(MessageContainerList.GetMessage(_utilities.ExecutionContext, "Screen") + ": " + _ScreenNumber, fnt, Brushes.Black, new Rectangle(0, yLocation, (int)printDocument.DefaultPageSettings.PrintableArea.Width, 20), sf);
                                yLocation += 20;
                            }
                            g.DrawString(Tickets.ToString() + " " + POSStatic.TicketTermVariant, fnt, Brushes.Black, 58, yLocation);
                            yLocation += 20;
                            g.DrawImage(BarcodeImage, 20, yLocation, BarcodeImage.Width, BarcodeImage.Height * 2);
                            yLocation += 65;
                            g.DrawString(BarCodeText, fnt, Brushes.Black, 28, yLocation);
                        }
                    }
                };
            }
            else
            {
                printDocument.PrintPage += (object sender, PrintPageEventArgs e) =>
                {
                    MyPrintDocumentPrintPage(ticketReceiptTemplate, BarCodeText, Tickets, e, sqlTrx);
                };
            }


            printDocument.Print();

            log.Debug("Ends-setupPrint(" + BarCodeText + "BarcodeImage," + Tickets + ") ");
            log.LogMethodExit(true);
            return true;
        }

        /// <summary>
        /// My Print Document Print Page
        /// </summary>
        /// <param name="ticketReceiptTemplate">ticketReceiptTemplate</param>
        /// <param name="barCodeText">barCodeText</param>
        /// <param name="tickets">tickets</param>
        /// <param name="e">e</param>
        /// <param name="sqlTrx">sqlTrx</param>
        void MyPrintDocumentPrintPage(int ticketReceiptTemplate, string barCodeText, int tickets, PrintPageEventArgs e, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(ticketReceiptTemplate, barCodeText, tickets, e, sqlTrx);
            clsTicketTemplate ticketTemplate = new clsTicketTemplate(ticketReceiptTemplate, _utilities);
            PrinterBL printerBL = new PrinterBL(_utilities.ExecutionContext);
            //POSPrint.clsTicket ticket = new POSPrint.clsTicket();
            Semnox.Parafait.Printer.clsTicket ticket = new Semnox.Parafait.Printer.clsTicket();
            ticket.TicketBorderProperty = new Rectangle(new Point(0, 0), ticketTemplate.getTicketSize());
            ticket.MarginProperty = ticketTemplate.Header.Margins;
            ticket.BorderWidthProperty = ticketTemplate.Header.BorderWidth;
            ticket.PaperSizeProperty = new PaperSize("custom", (int)(ticketTemplate.Header._Width * 100), (int)(ticketTemplate.Header._Height * 100));
            DataTable dtRedemption = PrintRedemptionReceipt.GetRedemptionOrderDetails(_RedemptionId, sqlTrx);
            string dateFormat = ParafaitEnv.DATE_FORMAT;
            string dateTimeFormat = ParafaitEnv.DATETIME_FORMAT;
            string numberFormat = ParafaitEnv.AMOUNT_FORMAT;
            string redemptionDate = "";
            string redemptionId = "";
            string redemptionOrder = "";
            string redemptionCardNumber = "";
            string originalRedemptionId = "";
            string originalRedemptionOrder = "";
            string redemptionRemarks = "";
            string manualTicketsUsed = "";
            string eTicketsUsed = "";
            string GraceTicketsUsed = "";
            string receiptTicketsUsed = "";
            string currencyTicketsUsed = "";
            string redemptionCustomerName = "";
            string vipCustomer = "";
            string CardTickets = "";
            if (dtRedemption.Rows.Count > 0)
            {
                redemptionDate = Convert.ToDateTime(dtRedemption.Rows[0]["redeemed_date"]).ToString(dateTimeFormat);
                redemptionId = dtRedemption.Rows[0]["redemption_id"].ToString();
                redemptionOrder = dtRedemption.Rows[0]["redemptionOrder"].ToString();
                redemptionCardNumber = dtRedemption.Rows[0]["primary_card_number"].ToString();
                originalRedemptionId = dtRedemption.Rows[0]["OrigRedemptionId"].ToString();
                originalRedemptionOrder = dtRedemption.Rows[0]["OrignalRedemptionOrder"].ToString();
                redemptionRemarks = dtRedemption.Rows[0]["remarks"].ToString();
                manualTicketsUsed = dtRedemption.Rows[0]["manual_tickets"].ToString();
                eTicketsUsed = dtRedemption.Rows[0]["eTickets"].ToString();
                GraceTicketsUsed = dtRedemption.Rows[0]["GraceTickets"].ToString();
                receiptTicketsUsed = dtRedemption.Rows[0]["receiptTickets"].ToString();
                currencyTicketsUsed = dtRedemption.Rows[0]["CurrencyTickets"].ToString();
                redemptionCustomerName = dtRedemption.Rows[0]["customer_name"].ToString();
                vipCustomer = dtRedemption.Rows[0]["vip_customer"].ToString();
                CardTickets = dtRedemption.Rows[0]["ticketsOnCard"].ToString();
            }
            foreach (clsTicketTemplate.clsTicketElement element in ticketTemplate.TicketElements)
            {
                Semnox.Parafait.Printer.clsTicket.PrintObject printObject = new Semnox.Parafait.Printer.clsTicket.PrintObject();
                printObject.FontProperty = element.Font;
                printObject.LocationProperty = element.Location;
                ticket.PrintObjectList.Add(printObject);
                printObject.AlignmentProperty = element.Alignment;
                printObject.WidthProperty = element.Width;
                printObject.RotateProperty = element.Rotate;
                printObject.ColorProperty = element.Color;
                string barCodeEncodeFormat = (element.formatId != -1) ? POSPrint.GetFormat(element.formatId, "BARCODE_ENCODE_TYPE") : BarcodeLib.TYPE.CODE128.ToString();
                printObject.BarCodeHeightProperty = element.BarCodeHeight;
                printObject.BarCodeEncodeTypeProperty = barCodeEncodeFormat;


                string line = element.Value.Replace("@SiteName", ((string.IsNullOrEmpty(_utilities.ParafaitEnv.POS_LEGAL_ENTITY) == false) ? _utilities.ParafaitEnv.POS_LEGAL_ENTITY : ParafaitEnv.SiteName)).Replace
                                        ("@Date", redemptionDate).Replace
                                        ("@SystemDate", ServerDateTime.Now.ToString(dateTimeFormat)).Replace
                                        ("@TrxNo", redemptionOrder).Replace
                                        ("@Cashier", ParafaitEnv.Username).Replace
                                        ("@Token", "").Replace
                                        ("@POS", ParafaitEnv.POSMachine).Replace
                                        ("@TaxNo", "").Replace
                                        ("@PrimaryCardNumber", redemptionCardNumber).Replace
                                        ("@CardNumber", redemptionCardNumber).Replace
                                        ("@CustomerName", redemptionCustomerName).Replace
                                        ("@Phone", "").Replace
                                        ("@Remarks", redemptionRemarks).Replace
                                        ("@CardBalance", "").Replace
                                        ("@CreditBalance", "").Replace
                                        ("@BonusBalance", "").Replace
                                        ("@SiteAddress", ParafaitEnv.SiteAddress).Replace
                                        ("@CardTickets", CardTickets).Replace
                                        ("@ScreenNumber", _ScreenNumber);

                line = line.Replace("@Product", "").Replace
                                    ("@Price", "").Replace
                                    ("@Quantity", "").Replace
                                    ("@Amount", "").Replace
                                    ("@LineRemarks", "").Replace
                                    ("@TaxName", "").Replace
                                    ("@Tax", "").Replace
                                    ("@Time", "").Replace
                                    ("@FromTime", "").Replace
                                    ("@ToTime", "").Replace
                                    ("@Seat", "").Replace
                                    ("@Tickets", tickets.ToString()).Replace
                                    ("@TicketBarCodeNo", barCodeText);
                if (line.Contains("@TicketBarCode"))
                {
                    line = line.Replace("@TicketBarCode", "");
                    // if (barcodeImage != null)
                    // {
                    // printObject.BarCodeProperty = barcodeImage;
                    int weight = 1;
                    if (printObject.FontProperty.Size >= 16)
                        weight = 3;
                    else if (printObject.FontProperty.Size >= 12)
                        weight = 2;
                    printObject.BarCodeProperty = printerBL.MakeBarcodeLibImage(weight, printObject.BarCodeHeightProperty, printObject.BarCodeEncodeTypeProperty, barCodeText);
                    // }
                }

                line = line.Replace("@Total", "").Replace
                                    ("@TaxTotal", "");

                line = line.Replace("@CouponNumber", "").Replace
                                    ("@DiscountName", "").Replace
                                    ("@DiscountPercentage", "").Replace
                                    ("@DiscountAmount", "").Replace
                                    ("@CouponEffectiveDate", "").Replace
                                    ("@CouponExpiryDate", "");



                line = line.Replace("@BarCodeCouponNumber", "").Replace
                                    ("@BarCodeCardNumber", "");
                line = line.Replace("@QRCodeCouponNumber", "").Replace
                                   ("@QRCodeCardNumber", "");

                if (redemptionId != "-1")
                    line = line.Replace("@TrxId", redemptionId);
                else
                    line = line.Replace("@TrxId", "");

                if (line.Contains("@SiteLogo"))
                {
                    line = line.Replace("@SiteLogo", "");
                    printObject.ImageProperty = ParafaitEnv.CompanyLogo;
                }

                if (line.Contains("@CustomerPhoto"))
                {
                    line = line.Replace("@CustomerPhoto", "");
                }

                printObject.TextProperty = line;
            }

            if (!string.IsNullOrEmpty(redemptionCardNumber))
                ticket.CardNumber = redemptionCardNumber;
            ticket.BackgroundImage = ticketTemplate.Header.BackgroundImage;
            if (!string.IsNullOrEmpty(redemptionId))
                ticket.TrxId = Convert.ToInt32(redemptionId);
            //ticket.TrxLineId = trxLine.DBLineId; 
            //POSPrint.printTicketElements(ticket, e.Graphics);
            printerBL.PrintTicketElements(ticket, e.Graphics);

            if (_utilities.ParafaitEnv.PRINT_TICKET_BORDER == "Y")
            {
                using (Pen pen = new Pen(Color.Black, ticket.BorderWidthProperty))
                    e.Graphics.DrawRectangle(pen, ticket.TicketBorderProperty);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// supend
        /// </summary>
        /// <param name="message">message</param>
        /// <returns></returns>
        public bool suspend(ref string message)
        {
            log.LogMethodEntry(message);
            if (string.IsNullOrEmpty(createdFromSuspendId) == false)
            {
                if (null == _utilities.executeScalar("select 1 from SuspendedRedemption where data = @id",
                                         new SqlParameter("@id", createdFromSuspendId)))
                {
                    message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 1392);// "Suspended redemption is already processed";
                    log.Info("Ends-suspend(message) as Suspended redemption is already processed");
                    log.LogMethodExit(false);
                    return false;
                }
            }

            if (productList.Count > 0 || getTotalTickets() > 0)
            {
                string idData = "RDSPND" + _utilities.ParafaitEnv.LoginID + ServerDateTime.Now.ToString("ssfffff");
                string name = "", value = "";
                foreach (clsRedemption.clsProducts item in productList)
                {
                    name += "<productId,Qty>;";
                    value += "<" + item.productId.ToString() + "," + item.Quantity.ToString() + ">;";
                }
                foreach (clsRedemption.clsScanTickets item in scanTicketList)
                {
                    name += "<ticketReceipt>;";
                    value += "<" + item.barCode + ">;";
                }
                foreach (clsRedemption.clsCards item in cardList)
                {
                    name += "<cardNumber>;";
                    value += "<" + item.cardNumber + ">;";
                }
                foreach (clsRedemption.clsRedemptionCurrency item in currencyList)
                {
                    name += "<currencyId,Qty>;";
                    value += "<" + item.currencyId + "," + item.quantity.ToString() + ">;";
                }

                name += "<ManualTickets>;";
                value += "<" + ManualTickets.ToString() + ">;";

                name = name.TrimEnd(';');
                value = value.TrimEnd(';');

                //_utilities.EventLog.logEvent("REDEMPTION", 'D', idData, "On-hold redemption", "REDEMPTION-SUSPEND", 0, name, value, _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);
                insertSuspendedRedemptionToDB("REDEMPTION", 'D', idData, "On-hold redemption", "REDEMPTION-SUSPEND", 0, name, value, _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);
                message = MessageContainerList.GetMessage(_utilities.ExecutionContext, "Suspend success");

                if (string.IsNullOrEmpty(createdFromSuspendId) == false)
                {
                    /*_utilities.executeScalar("delete from eventLog where data = @id",
                                             new SqlParameter("@id", createdFromSuspendId));*/
                    _utilities.executeScalar("delete from SuspendedRedemption where data = @id",
                                             new SqlParameter("@id", createdFromSuspendId));
                }

                printSuspended(idData);
                log.Debug("Ends-suspend(message) ");
                log.LogMethodExit(true);
                return true;
            }
            else
            {
                message = MessageContainerList.GetMessage(_utilities.ExecutionContext, "Empty redemption");
                log.Debug("Ends-suspend(message) as Empty redemption");
                log.LogMethodExit(false);
                return false;
            }
        }

        /// <summary>
        /// retrieve Suspended
        /// </summary>
        /// <param name="idData">idData</param>
        /// <param name="message">message</param>
        /// <returns></returns>
        public bool retrieveSuspended(string idData, ref string message)
        {
            log.LogMethodEntry(idData, message);
            /* DataTable dt = _utilities.executeDataTable(@"select name, value from EventLog where data = @id",
                                                       new System.Data.SqlClient.SqlParameter("@id", idData));*/
            DataTable dt = _utilities.executeDataTable(@"select name, value from SuspendedRedemption where data = @id",
                                                       new System.Data.SqlClient.SqlParameter("@id", idData));

            if (dt.Rows.Count == 0)
            {
                message = "Unable to retrieve suspended redemption";
                log.Info("Ends-retrieveSuspended(" + idData + ",message) as Unable to retrieve suspended redemption");
                return false;
            }

            try
            {
                string[] names = dt.Rows[0]["name"].ToString().Split(';');
                string[] values = dt.Rows[0]["value"].ToString().Split(';');

                for (int i = 0; i < names.Length; i++)
                {
                    string n = names[i].Trim(' ', '<', '>');
                    if (n.StartsWith("productid", StringComparison.CurrentCultureIgnoreCase))
                    {
                        string[] v = values[i].Trim(' ', '<', '>').Split(',');

                        DataTable dtp = GetGiftDetails(Convert.ToInt32(v[0]));
                        if (dtp.Rows.Count > 0)
                        {
                            int qty = Convert.ToInt32(v[1]);
                            while (qty-- > 0)
                            {
                                if (!addGift(dtp.Rows[0]["Code"].ToString(), 'C', ref message))
                                    break;
                            }
                        }
                        else
                            break;
                    }
                    if (n.StartsWith("currencyId", StringComparison.CurrentCultureIgnoreCase))
                    {
                        string[] v = values[i].Trim(' ', '<', '>').Split(',');

                        int qty = Convert.ToInt32(v[1]);
                        while (qty-- > 0)
                        {
                            if (!addRedemptionCurrency(Convert.ToInt32(v[0]), ref message))
                                break;
                        }
                    }
                    else if (n.StartsWith("ticketReceipt", StringComparison.CurrentCultureIgnoreCase))
                    {
                        string v = values[i].Trim(' ', '<', '>');

                        if (!addScanTickets(v, ref message))
                            return false;
                    }
                    else if (n.StartsWith("cardNumber", StringComparison.CurrentCultureIgnoreCase))
                    {
                        string v = values[i].Trim(' ', '<', '>');

                        if (!addCard(v, ref message))
                            return false;
                    }
                    else if (n.StartsWith("ManualTickets", StringComparison.CurrentCultureIgnoreCase))
                    {
                        string v = values[i].Trim(' ', '<', '>');

                        string mes = "";
                        addManualTickets(Convert.ToInt32(v), ref mes);
                    }
                }

                createdFromSuspendId = idData;

                message = MessageContainerList.GetMessage(_utilities.ExecutionContext, "Redemption retrieved");
                log.Info("retrieveSuspended(" + idData + ",message) - Redemption retrieved");
                log.Debug("Ends-retrieveSuspended(" + idData + ",message)");
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                log.Fatal("Ends-retrieveSuspended(" + idData + ",message) due to exception " + ex.Message);
                log.LogMethodExit(false);
                return false;
            }
        }

        private bool printSuspended(string BarCodeText)
        {
            log.LogMethodEntry(BarCodeText);
            PrintDialog MyPrintDialog = new PrintDialog();
            MyPrintDialog.AllowCurrentPage = false;
            MyPrintDialog.AllowPrintToFile = false;
            MyPrintDialog.AllowSelection = false;
            MyPrintDialog.AllowSomePages = false;
            MyPrintDialog.PrintToFile = false;
            MyPrintDialog.ShowHelp = false;
            MyPrintDialog.ShowNetwork = false;
            MyPrintDialog.PrinterSettings.DefaultPageSettings.Landscape = false;

            if (ParafaitEnv.ShowPrintDialog == "Y")
            {
                if (MyPrintDialog.ShowDialog() != DialogResult.OK)
                {
                    log.Debug("Ends-printSuspended(" + BarCodeText + ") as Print dialog was cancelled");
                    log.LogMethodExit(false);
                    return false;
                }
            }

            PrinterBL printerBL = new PrinterBL(POSStatic.Utilities.ExecutionContext);
            PrintDocument printDocument = new System.Drawing.Printing.PrintDocument();

            printDocument.DefaultPageSettings =
            MyPrintDialog.PrinterSettings.DefaultPageSettings;
            printDocument.DefaultPageSettings.Margins =
                             new Margins(20, 20, 20, 20);
            printDocument.PrinterSettings =
                                MyPrintDialog.PrinterSettings;

            printDocument.PrintPage += (sender, e) =>
            {
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;

                using (Graphics g = e.Graphics)
                {
                    using (Font fnt = new Font("Arial", 10))
                    {
                        //Image BarcodeImage = GenCode128.Code128Rendering.MakeBarcodeImage(BarCodeText, 1, true);
                        Image BarcodeImage = printerBL.MakeBarcodeLibImage(1, 24, BarcodeLib.TYPE.CODE128.ToString(), BarCodeText);
                        int yLocation = 20;
                        if (ParafaitEnv.CompanyLogo != null)
                        {
                            int imgWidth = Math.Min(ParafaitEnv.CompanyLogo.Width, (int)printDocument.DefaultPageSettings.PrintableArea.Width);
                            int imgHeight = 180 * ParafaitEnv.CompanyLogo.Height / imgWidth;
                            g.DrawImage(ParafaitEnv.CompanyLogo, (printDocument.DefaultPageSettings.PrintableArea.Width - imgWidth) / 2, yLocation, imgWidth, imgHeight);
                            yLocation += imgHeight;
                        }

                        g.DrawString(ParafaitEnv.SiteName, fnt, Brushes.Black, new Rectangle(0, yLocation, (int)printDocument.DefaultPageSettings.PrintableArea.Width, 20), sf);
                        yLocation += 20;
                        g.DrawString("* REDEMPTION SUSPENDED *", fnt, Brushes.Black, new Rectangle(0, yLocation, (int)printDocument.DefaultPageSettings.PrintableArea.Width, 20), sf);
                        yLocation += 30;
                        g.DrawString(ServerDateTime.Now.ToString("dd-MMM-yyyy h:mm:ss tt"), fnt, Brushes.Black, new Rectangle(0, yLocation, (int)printDocument.DefaultPageSettings.PrintableArea.Width, 20), sf);
                        yLocation += 20;
                        g.DrawString(ParafaitEnv.POSMachine + " / " + ParafaitEnv.LoginID, fnt, Brushes.Black, new Rectangle(0, yLocation, (int)printDocument.DefaultPageSettings.PrintableArea.Width, 20), sf);
                        yLocation += 20;
                        g.DrawString(BarCodeText, fnt, Brushes.Black, 58, yLocation);
                        yLocation += 20;
                        g.DrawImage(BarcodeImage, 20, yLocation, BarcodeImage.Width, BarcodeImage.Height * 2);
                    }
                }
            };

            printDocument.Print();
            log.Debug("Ends-printSuspended(" + BarCodeText + ")");
            log.LogMethodExit(true);
            return true;
        }

        /// <summary>
        /// Redeem Ticket Receipt
        /// </summary>
        /// <param name="sqlTrx">sqlTrx</param>
        public void RedeemTicketReceipt(SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(sqlTrx);
            string message = "";

            if (productList.Count > 0)
            {
                throw new Exception(MessageContainerList.GetMessage(_utilities.ExecutionContext, 1377));
            }

            if (cardList.Count > 0)
            {
                refreshCardList();
            }

            if (string.IsNullOrEmpty(createdFromSuspendId) == false)
            {
                if (null == _utilities.executeScalar("select 1 from SuspendedRedemption where data = @id",
                                         new SqlParameter("@id", createdFromSuspendId)))
                {
                    log.Error("Ends-redeemGifts() - Suspended redemption is already processed");
                    throw new Exception(MessageContainerList.GetMessage(_utilities.ExecutionContext, 1392));
                }
            }

            foreach (clsRedemption.clsScanTickets item in scanTicketList)
            {
                if (!GetScanedReciptStatus(item.barCode, sqlTrx))
                {
                    throw new Exception(MessageContainerList.GetMessage(_utilities.ExecutionContext, 1391));
                }
            }

            int totalTicketsToPring = getPhysicalTickets() + getCurrencyTickets() + getManualTickets();
            if (totalTicketsToPring == 0)
            {
                return;
            }
            RedemptionBL redemptionBL = new RedemptionBL(_utilities.ExecutionContext);
            redemptionBL.PerDayLimitCheckForManualTickets(ManualTickets);

            if (totalTicketsToPring > ParafaitEnv.LOAD_TICKETS_LIMIT)
            {
                //Sorry, cannot proceed with &1 &2. Load ticket limit is &3 
                throw new Exception(MessageContainerList.GetMessage(_utilities.ExecutionContext, 2830, totalTicketsToPring, POSStatic.TicketTermVariant,
                                                                 ParafaitEnv.LOAD_TICKETS_LIMIT.ToString()));
            }

            try
            {
                LoadTicketLimitCheck(totalTicketsToPring, _utilities);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            SqlCommand sqlCmd;
            SqlTransaction cmd_trx;
            if (sqlTrx == null)
            {
                sqlCmd = _utilities.getCommand(_utilities.createConnection().BeginTransaction());
                cmd_trx = sqlCmd.Transaction;
            }
            else
            {
                sqlCmd = _utilities.getCommand(sqlTrx);
                cmd_trx = sqlCmd.Transaction;
            }
            try
            {
                sqlCmd.CommandText = "Insert into Redemption (card_id, primary_card_number, ReceiptTickets, manual_tickets, eTickets, GraceTickets, CurrencyTickets, redeemed_date, LastUpdatedBy, Source, RedemptionOrderNo, LastUpdateDate, OrderCompletedDate, OrderDeliveredDate, RedemptionStatus, createdBy, creationDate, customerId, posmachineId) " +
                                            " Values(@card_id, @primary_card_number, @receiptTickets, @manual_tickets, @eTickets, @GraceTickets, @currencyTickets, getdate(), @LastUpdatedBy, @Source, @RedemptionOrderNo, getdate(), getdate(), getdate(), @RedemptionStatus, @CreatedBy, getdate(), @CustomerId, @PosMachineID ); SELECT @@IDENTITY";

                if (cardList.Count > 0)
                {
                    sqlCmd.Parameters.AddWithValue("@card_id", cardList[0].cardId);
                    sqlCmd.Parameters.AddWithValue("@primary_card_number", cardList[0].cardNumber);
                    if (cardList[0].customerId > -1)
                    { sqlCmd.Parameters.AddWithValue("@CustomerId", cardList[0].customerId); }
                    else
                    { sqlCmd.Parameters.AddWithValue("@CustomerId", DBNull.Value); }
                }
                else
                {
                    sqlCmd.Parameters.AddWithValue("@card_id", DBNull.Value);
                    sqlCmd.Parameters.AddWithValue("@primary_card_number", "");
                    sqlCmd.Parameters.AddWithValue("@CustomerId", DBNull.Value);
                }

                sqlCmd.Parameters.AddWithValue("@ReceiptTickets", getPhysicalTickets());
                sqlCmd.Parameters.AddWithValue("@currencyTickets", getCurrencyTickets());
                sqlCmd.Parameters.AddWithValue("@manual_tickets", getManualTickets());
                sqlCmd.Parameters.AddWithValue("@eTickets", 0);
                sqlCmd.Parameters.AddWithValue("@Source", "POS Redemption");
                sqlCmd.Parameters.AddWithValue("@GraceTickets", 0);
                sqlCmd.Parameters.AddWithValue("@LastUpdatedBy", ParafaitEnv.LoginID);
                sqlCmd.Parameters.AddWithValue("@CreatedBy", ParafaitEnv.LoginID);
                sqlCmd.Parameters.AddWithValue("@RedemptionOrderNo", GetNextRedemptionOrderNo(ParafaitEnv.POSMachineId, null));
                sqlCmd.Parameters.AddWithValue("@RedemptionStatus", "DELIVERED");
                sqlCmd.Parameters.AddWithValue("@PosMachineID", ParafaitEnv.POSMachineId);
                int redemptionId = Convert.ToInt32((Decimal)sqlCmd.ExecuteScalar());

                sqlCmd.CommandText = "Insert into Redemption_cards (redemption_id, card_number, card_id, ticket_count, LastUpdateDate, LastUpdatedBy, CreationDate, CreatedBy) " +
                                            " Values (@redemption_id, @card_no, @card_id, @ticket_count, getdate(), @LastUpdatedBy, getdate(), @CreatedBy)";
                // CreditPlus creditPlus = new CreditPlus(_utilities);
                for (int i = 0; i < cardList.Count; i++)
                {
                    try
                    {
                        //Insert into redemption cards table
                        sqlCmd.Parameters.Clear();
                        sqlCmd.Parameters.AddWithValue("@card_no", cardList[i].cardNumber);
                        sqlCmd.Parameters.AddWithValue("@redemption_id", redemptionId);
                        sqlCmd.Parameters.AddWithValue("@card_id", cardList[i].cardId);
                        sqlCmd.Parameters.AddWithValue("@ticket_count", 0);
                        sqlCmd.Parameters.AddWithValue("@LastUpdatedBy", ParafaitEnv.LoginID);
                        sqlCmd.Parameters.AddWithValue("@CreatedBy", ParafaitEnv.LoginID);
                        sqlCmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 123, ex.Message);
                        log.Fatal("Ends-RedeemTicketReceipt() due to exception in creating redemption cards information error: " + ex.Message);
                        throw new Exception(message);
                    }
                }

                sqlCmd.CommandText = "Insert into Redemption_cards (redemption_id, CurrencyId, CurrencyQuantity, ticket_count, card_number, LastUpdateDate, LastUpdatedBy, CreationDate, CreatedBy) " +
                                            " Values (@redemption_id, @CurrencyId, @CurQuantity, @ticket_count, '',  getdate(), @LastUpdatedBy, getdate(), @CreatedBy)";
                int customer_tickets = 0;
                foreach (clsRedemptionCurrency item in currencyList)
                {
                    customer_tickets = 0;
                    customer_tickets = item.ValueInTickets * item.quantity;
                    if (customer_tickets <= 0)
                        continue;

                    try
                    {
                        //Insert into redemption cards table
                        sqlCmd.Parameters.Clear();
                        sqlCmd.Parameters.AddWithValue("@CurrencyId", item.currencyId);
                        sqlCmd.Parameters.AddWithValue("@CurQuantity", item.quantity);
                        sqlCmd.Parameters.AddWithValue("@redemption_id", redemptionId);
                        sqlCmd.Parameters.AddWithValue("@ticket_count", customer_tickets);
                        sqlCmd.Parameters.AddWithValue("@LastUpdatedBy", ParafaitEnv.LoginID);
                        sqlCmd.Parameters.AddWithValue("@CreatedBy", ParafaitEnv.LoginID);
                        sqlCmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(_utilities.ExecutionContext, 124, ex.Message),
                                                  MessageContainerList.GetMessage(_utilities.ExecutionContext, "Save Information") + MessageContainerList.GetMessage(_utilities.ExecutionContext, 2693, _ScreenNumber));
                        log.Error(ex.Message);
                        throw new Exception(MessageContainerList.GetMessage(_utilities.ExecutionContext, 124, ex.Message));
                    }

                    if (item.productId != DBNull.Value && item.currencyId > -1)
                    {
                        UpdateRedemptionCurrencyInventory(item, cmd_trx);
                    }
                }
                InsertManualTicketReceipts(redemptionId, cmd_trx, false);
                SetTicketAllocationDetails(redemptionId, cmd_trx);

                _RedemptionId = redemptionId;
                int newTicketReceiptId = printTotalManualTickets(redemptionId, ref message, cmd_trx);
                if (newTicketReceiptId == -1)
                {
                    POSUtils.ParafaitMessageBox(message,
                                                 MessageContainerList.GetMessage(_utilities.ExecutionContext, "Save Information") + MessageContainerList.GetMessage(_utilities.ExecutionContext, 2693, _ScreenNumber));
                    throw new Exception(message);
                }
                if (sqlTrx == null)
                    cmd_trx.Commit();
                try
                {
                    if (POSStatic.AUTO_PRINT_REDEMPTION_RECEIPT)
                        PrintRedemptionReceipt.Print(redemptionId, _ScreenNumber);
                }
                catch (Exception ex)
                {
                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(_utilities.ExecutionContext, 1819, ex.Message),
                                                    MessageContainerList.GetMessage(_utilities.ExecutionContext, "Receipt Print Error") + MessageContainerList.GetMessage(_utilities.ExecutionContext, 2693, _ScreenNumber));
                    log.Error("Receipt Print Error", ex);
                }
            }
            catch (Exception ex)
            {
                log.Fatal("Ends-RedeemTicketReceipt() due to exception " + ex.Message);
                if (sqlTrx == null)
                    cmd_trx.Rollback();
                _RedemptionId = -1;
                throw new Exception(ex.Message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// print Total Manual Tickets
        /// </summary>
        /// <param name="sourceRedemptionId">sourceRedemptionId</param>
        /// <param name="message">message</param>
        /// <param name="sqlCmd">sqlCmd</param>
        /// <returns></returns>
        public int printTotalManualTickets(int sourceRedemptionId, ref string message, SqlTransaction sqlCmd)
        {
            log.LogMethodEntry(sourceRedemptionId, message, sqlCmd);
            int newTicketReceiptId = -1;
            try
            {
                int total = getCurrencyTickets() + getManualTickets() + getPhysicalTickets();
                log.LogVariableState("total", total);
                if (total > 0)
                {
                    newTicketReceiptId = printRealTicketReceipt(sourceRedemptionId, total, ref message, sqlCmd);
                    log.LogVariableState("newTicketReceiptId", newTicketReceiptId);
                    if (newTicketReceiptId > -1)
                    {
                        if (getPhysicalTickets() > 0)
                        {
                            InsertManualTicketReceipts(_RedemptionId, sqlCmd);
                        }

                        if (getCurrencyTickets() > 0)
                        {
                            foreach (clsRedemption.clsRedemptionCurrency cur in currencyList)
                            {
                                if (cur.productId != DBNull.Value && cur.currencyId > -1)
                                    UpdateRedemptionCurrencyInventory(cur, sqlCmd);
                            }
                            currencyList.Clear();
                        }

                        ManualTickets = 0;
                        log.Info("Ends-printTotalManualTickets(message)");
                        log.LogMethodExit(newTicketReceiptId);
                        return newTicketReceiptId;
                    }
                    else
                    {
                        log.Info("Ends-printTotalManualTickets(message) as unable to printRealTicketReceipt");
                        log.LogMethodExit(-1);
                        return -1;
                    }
                }
                else
                {
                    log.Debug("Ends-printTotalManualTickets(message)");
                    log.LogMethodExit(-10);
                    return -10;
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                log.Fatal("Ends-printTotalManualTickets(message) due to exception " + ex.Message);
                log.LogMethodExit(-1);
                return -1;
            }
        }

        /// <summary>
        /// print Load Ticket Receipt
        /// </summary>
        /// <param name="ticketsLoaded">ticketsLoaded</param>
        /// <param name="cardNumber">cardNumber</param>
        /// <param name="ScreenNumber">ScreenNumber</param>
        /// <returns></returns>
        public bool printLoadTicketReceipt(int ticketsLoaded, string cardNumber, string ScreenNumber = null)
        {
            log.LogMethodEntry(ticketsLoaded, cardNumber, ScreenNumber);
            _ScreenNumber = ScreenNumber;

            PrintDocument MyPrintDocument = new PrintDocument();
            PrintDialog MyPrintDialog = new PrintDialog();
            MyPrintDialog.AllowCurrentPage = false;
            MyPrintDialog.AllowPrintToFile = false;
            MyPrintDialog.AllowSelection = false;
            MyPrintDialog.AllowSomePages = false;
            MyPrintDialog.PrintToFile = false;
            MyPrintDialog.ShowHelp = false;
            MyPrintDialog.ShowNetwork = false;
            MyPrintDialog.PrinterSettings.DefaultPageSettings.Landscape = false;
            MyPrintDialog.UseEXDialog = true;

            if (MyPrintDialog.ShowDialog() != DialogResult.OK)
                return false;

            MyPrintDocument.DocumentName = MessageContainerList.GetMessage(_utilities.ExecutionContext, 2665);//Load Tickets Receipt
            MyPrintDocument.PrinterSettings = MyPrintDialog.PrinterSettings;
            MyPrintDocument.DefaultPageSettings = MyPrintDialog.PrinterSettings.DefaultPageSettings;
            MyPrintDocument.DefaultPageSettings.Margins = new Margins(10, 10, 20, 20);

            int col1x = 0;
            //int col2x = 60;
            //int col4x = 220;
            int yLocation = 20;
            int yIncrement = 20;
            Font defaultFont = new System.Drawing.Font("courier narrow", 10f);
            MyPrintDocument.PrintPage += (sender, e) =>
            {

                if (ParafaitEnv.CompanyLogo != null)
                {
                    int imgWidth = Math.Min(ParafaitEnv.CompanyLogo.Width, (int)e.PageSettings.PrintableArea.Width);
                    int imgHeight = 180 * ParafaitEnv.CompanyLogo.Height / imgWidth;
                    e.Graphics.DrawImage(ParafaitEnv.CompanyLogo, (e.PageSettings.PrintableArea.Width - imgWidth) / 2, yLocation, imgWidth, imgHeight);
                    yLocation += imgHeight;
                }

                e.Graphics.DrawString(MessageContainerList.GetMessage(_utilities.ExecutionContext, "Load Tickets Receipt"), new Font(defaultFont.FontFamily, 9.0F, FontStyle.Bold), Brushes.Black, 10, yLocation);
                yLocation += 30;
                e.Graphics.DrawString(MessageContainerList.GetMessage(_utilities.ExecutionContext, "Site") + ": " + ParafaitEnv.SiteName, new Font(defaultFont.FontFamily, 8.0F, FontStyle.Bold), Brushes.Black, 10, yLocation);
                yLocation += 20;
                e.Graphics.DrawString(MessageContainerList.GetMessage(_utilities.ExecutionContext, "POS Name") + ": " + ParafaitEnv.POSMachine, defaultFont, Brushes.Black, col1x, yLocation);
                yLocation += 20;
                e.Graphics.DrawString(MessageContainerList.GetMessage(_utilities.ExecutionContext, "Date") + ": " + ServerDateTime.Now.ToString(ParafaitEnv.DATETIME_FORMAT), defaultFont, Brushes.Black, col1x, yLocation);
                yLocation += 20;
                e.Graphics.DrawString(MessageContainerList.GetMessage(_utilities.ExecutionContext, "Cashier") + ": " + ParafaitEnv.Username, defaultFont, Brushes.Black, col1x, yLocation);
                yLocation += yIncrement;
                e.Graphics.DrawString(MessageContainerList.GetMessage(_utilities.ExecutionContext, "Card") + ": " + cardNumber.ToString(), defaultFont, Brushes.Black, col1x, yLocation);
                yLocation += yIncrement;


                yLocation += yIncrement;
                e.Graphics.DrawString(MessageContainerList.GetMessage(_utilities.ExecutionContext, "Tickets Loaded: " + ticketsLoaded.ToString()), defaultFont, Brushes.Black, col1x, yLocation);
                yLocation += yIncrement;
                e.Graphics.DrawString(MessageContainerList.GetMessage(_utilities.ExecutionContext, "Total Tickets: " + (cardList[0].Tickets + ticketsLoaded).ToString()), defaultFont, Brushes.Black, col1x, yLocation);

                yLocation += yIncrement;

            };
            try
            {
                MyPrintDocument.Print();
                log.Debug("Ends-printLoadTicketReceipt()");
                return true;
            }
            catch (Exception ex)
            {
                POSUtils.ParafaitMessageBox(ex.Message,
                                             MessageContainerList.GetMessage(_utilities.ExecutionContext, "Print Error") + MessageContainerList.GetMessage(_utilities.ExecutionContext, 2693, _ScreenNumber));
                log.Fatal("Ends-printLoadTicketReceipt() due to exception " + ex.Message);
                log.LogMethodExit(false);
                return false;
            }
        }

        /// <summary>
        /// Load Tickets To Card
        /// </summary>
        /// <param name="ticketsToLoad">ticketsToLoad</param>
        public void LoadTicketsToCard(int ticketsToLoad)
        {
            log.LogMethodEntry(ticketsToLoad);
            try
            {
                LoadTicketLimitCheck(ticketsToLoad, _utilities);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            if (string.IsNullOrEmpty(createdFromSuspendId) == false)
            {
                if (null == _utilities.executeScalar("select 1 from SuspendedRedemption where data = @id",
                                         new SqlParameter("@id", createdFromSuspendId)))
                {
                    log.Error("Ends-redeemGifts() - Suspended redemption is already processed");
                    throw new Exception(MessageContainerList.GetMessage(_utilities.ExecutionContext, 1392));
                }
            }
            refreshCardList();
            //List<Tuple<string, int, DateTime?>> manualTicketRecepts = new List<Tuple<string, int, DateTime?>>();
            foreach (clsRedemption.clsScanTickets item in scanTicketList)
            {
                if (!GetScanedReciptStatus(item.barCode))
                {
                    throw new Exception(MessageContainerList.GetMessage(_utilities.ExecutionContext, 1391));
                }
                // manualTicketRecepts.Add(new Tuple<string, int, DateTime?>(item.barCode, item.Tickets, null));
            }
            if (getPhysicalTickets() + getCurrencyTickets() + getManualTickets() != ticketsToLoad)
            {
                log.Error("Ends-redeemGifts() - Tickets passed " + ticketsToLoad.ToString() + " is not mathcing with tickets available on order");
                throw new Exception(MessageContainerList.GetMessage(_utilities.ExecutionContext, 1390) + ": " + ticketsToLoad.ToString());
            }


            string message = "";
            SqlCommand sqlCmd = _utilities.getCommand(_utilities.createConnection().BeginTransaction());
            SqlTransaction cmd_trx = sqlCmd.Transaction;
            try
            {
                sqlCmd.CommandText = "Insert into Redemption (card_id, primary_card_number, ReceiptTickets, manual_tickets, eTickets, GraceTickets, CurrencyTickets, redeemed_date, LastUpdatedBy, Source, RedemptionOrderNo, LastUpdateDate, OrderCompletedDate, OrderDeliveredDate, RedemptionStatus, createdby, creationDate, customerId, PosMachineId) " +
                                     " Values(@card_id, @primary_card_number, @receiptTickets, @manual_tickets, @eTickets, @GraceTickets, @currencyTickets, getdate(),  @LastUpdatedBy, @Source, @RedemptionOrderNo, getdate(), getdate(), getdate(), @RedemptionStatus, @CreatedBy, getdate(), @CustomerId, @PosMachineId); SELECT @@IDENTITY";


                if (cardList.Count > 0)
                {
                    sqlCmd.Parameters.AddWithValue("@card_id", cardList[0].cardId);
                    sqlCmd.Parameters.AddWithValue("@primary_card_number", cardList[0].cardNumber);
                    if (cardList[0].customerId > -1)
                    { sqlCmd.Parameters.AddWithValue("@CustomerId", cardList[0].customerId); }
                    else
                    { sqlCmd.Parameters.AddWithValue("@CustomerId", DBNull.Value); }
                }
                else
                {
                    sqlCmd.Parameters.AddWithValue("@card_id", DBNull.Value);
                    sqlCmd.Parameters.AddWithValue("@primary_card_number", "");
                    sqlCmd.Parameters.AddWithValue("@CustomerId", DBNull.Value);
                }

                sqlCmd.Parameters.AddWithValue("@ReceiptTickets", getPhysicalTickets());
                sqlCmd.Parameters.AddWithValue("@currencyTickets", getCurrencyTickets());
                sqlCmd.Parameters.AddWithValue("@manual_tickets", getManualTickets());
                sqlCmd.Parameters.AddWithValue("@eTickets", 0);
                sqlCmd.Parameters.AddWithValue("@GraceTickets", 0);
                sqlCmd.Parameters.AddWithValue("@Source", "POS Redemption");
                sqlCmd.Parameters.AddWithValue("@LastUpdatedBy", ParafaitEnv.LoginID);
                sqlCmd.Parameters.AddWithValue("@RedemptionOrderNo", GetNextRedemptionOrderNo(ParafaitEnv.POSMachineId, null));
                sqlCmd.Parameters.AddWithValue("@RedemptionStatus", "DELIVERED");
                sqlCmd.Parameters.AddWithValue("@CreatedBy", ParafaitEnv.LoginID);
                sqlCmd.Parameters.AddWithValue("@PosMachineId", ParafaitEnv.POSMachineId);
                int redemptionId = Convert.ToInt32((Decimal)sqlCmd.ExecuteScalar());

                sqlCmd.CommandText = "Insert into Redemption_cards (redemption_id, card_number, card_id, ticket_count, LastUpdateDate, LastUpdatedBy, CreationDate, CreatedBy) " +
                                            " Values (@redemption_id, @card_no, @card_id, @ticket_count, getdate(), @LastUpdatedBy, getdate(), @CreatedBy)";
                CreditPlus creditPlus = new CreditPlus(_utilities);
                for (int i = 0; i < cardList.Count; i++)
                {
                    try
                    {
                        //Insert into redemption cards table
                        sqlCmd.Parameters.Clear();
                        sqlCmd.Parameters.AddWithValue("@card_no", cardList[i].cardNumber);
                        sqlCmd.Parameters.AddWithValue("@redemption_id", redemptionId);
                        sqlCmd.Parameters.AddWithValue("@card_id", cardList[i].cardId);
                        sqlCmd.Parameters.AddWithValue("@ticket_count", 0);
                        sqlCmd.Parameters.AddWithValue("@LastUpdatedBy", ParafaitEnv.LoginID);
                        sqlCmd.Parameters.AddWithValue("@CreatedBy", ParafaitEnv.LoginID);
                        sqlCmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 123, ex.Message);
                        log.Fatal("Ends-LoadTicketsToCard() due to exception in creating redemption cards information error: " + ex.Message);
                        //cmd_trx.Rollback();
                        throw new Exception(message);
                    }
                }

                sqlCmd.CommandText = "Insert into Redemption_cards (redemption_id, CurrencyId, CurrencyQuantity, ticket_count, card_number, LastUpdateDate, LastUpdatedBy, CreationDate, CreatedBy) " +
                                           " Values (@redemption_id, @CurrencyId, @CurQuantity, @ticket_count, '', getdate(), @LastUpdatedBy, getdate(), @CreatedBy)";
                int customer_tickets = 0;
                foreach (clsRedemptionCurrency item in currencyList)
                {
                    customer_tickets = 0;
                    customer_tickets = item.ValueInTickets * item.quantity;
                    if (customer_tickets <= 0)
                        continue;

                    try
                    {
                        //Insert into redemption cards table
                        sqlCmd.Parameters.Clear();
                        sqlCmd.Parameters.AddWithValue("@CurrencyId", item.currencyId);
                        sqlCmd.Parameters.AddWithValue("@CurQuantity", item.quantity);
                        sqlCmd.Parameters.AddWithValue("@redemption_id", redemptionId);
                        sqlCmd.Parameters.AddWithValue("@ticket_count", customer_tickets);
                        sqlCmd.Parameters.AddWithValue("@LastUpdatedBy", ParafaitEnv.LoginID);
                        sqlCmd.Parameters.AddWithValue("@CreatedBy", ParafaitEnv.LoginID);
                        sqlCmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(_utilities.ExecutionContext, 124, ex.Message),
                                                   MessageContainerList.GetMessage(_utilities.ExecutionContext, "Save Information") + MessageContainerList.GetMessage(_utilities.ExecutionContext, 2693, _ScreenNumber));
                        log.Fatal("Ends-LoadTicketsToCard() due to exception " + ex.Message);
                        //cmd_trx.Rollback();
                        throw new Exception(MessageContainerList.GetMessage(_utilities.ExecutionContext, 124, ex.Message));
                    }

                    if (item.productId != DBNull.Value && item.currencyId != -1)
                    {
                        UpdateRedemptionCurrencyInventory(item, cmd_trx);
                    }
                }
                InsertManualTicketReceipts(redemptionId, cmd_trx, false);

                SetTicketAllocationDetails(redemptionId, cmd_trx);

                if (string.IsNullOrEmpty(createdFromSuspendId) == false)
                {
                    _utilities.executeScalar("delete from SuspendedRedemption where data = @id",
                                              cmd_trx,
                                              new SqlParameter("@id", createdFromSuspendId));
                }
                TaskProcs tp = new TaskProcs(_utilities);
                int originalMangerId = _utilities.ParafaitEnv.ManagerId;
                if (addManualTicketMangerApprovalDetails != null)
                {
                    _utilities.ParafaitEnv.ManagerId = addManualTicketMangerApprovalDetails.Item3;
                }
                // if (!tp.loadTickets(new POSCore.Card((int)cardList[0].cardId, "", _utilities), ticketsToLoad, "Load Tickets under Redeem", manualTicketRecepts, ref message, cmd_trx))
                if (!tp.loadTickets(new Card((int)cardList[0].cardId, "", _utilities), ticketsToLoad, "Load Tickets under Redeem", redemptionId, ref message, cmd_trx))
                {
                    log.Error("LOADTICKETS- unable to loadTickets as error " + message);
                    _utilities.ParafaitEnv.ManagerId = originalMangerId;
                    throw new Exception(message);
                }
                _utilities.ParafaitEnv.ManagerId = originalMangerId;
                cmd_trx.Commit();
                addManualTicketMangerApprovalDetails = null;
                _RedemptionId = redemptionId;
            }
            catch (Exception ex)
            {
                log.Fatal("Ends-LoadTicketsToCard() due to exception " + ex.Message);
                cmd_trx.Rollback();
                _RedemptionId = -1;
                throw new Exception(ex.Message);
                //return;
            }
            int RedemptionReceiptTemplate = -1;
            try
            {
                RedemptionReceiptTemplate = Convert.ToInt32(_utilities.getParafaitDefaults("REDEMPTION_RECEIPT_TEMPLATE"));
            }
            catch { RedemptionReceiptTemplate = -1; }

            if (_utilities.getParafaitDefaults("AUTO_PRINT_LOAD_TICKETS") == "Y")
            {
                if (RedemptionReceiptTemplate == -1)
                    printLoadTicketReceipt(ticketsToLoad, cardList[0].cardNumber, null);
                else
                    PrintRedemptionReceipt.Print(_RedemptionId, _ScreenNumber = null);
            }
            else if (_utilities.getParafaitDefaults("AUTO_PRINT_LOAD_TICKETS") == "A")
            {
                if (POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(_utilities.ExecutionContext, 484),
                                                 MessageContainerList.GetMessage(_utilities.ExecutionContext, "Print Receipt")
                                                 + MessageContainerList.GetMessage(_utilities.ExecutionContext, 2693, _ScreenNumber), MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (RedemptionReceiptTemplate == -1)
                        printLoadTicketReceipt(ticketsToLoad, cardList[0].cardNumber, null);
                    else
                        PrintRedemptionReceipt.Print(_RedemptionId, _ScreenNumber = null);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Load Ticket Limit Check
        /// </summary>
        /// <param name="ticketsToLoad">ticketsToLoad</param>
        /// <param name="_utilities">_utilities</param>
        public void LoadTicketLimitCheck(int ticketsToLoad, Utilities _utilities)
        {
            log.LogMethodEntry(ticketsToLoad, _utilities);
            //SqlCommand sqlCmd = _utilities.getCommand();
            //sqlCmd.CommandText = "select requires_manager_approval from task_type where task_type = @task_type";
            //sqlCmd.Parameters.AddWithValue("@task_type", "LOADTICKETS");
            string mgrApprovalRequired = _utilities.executeScalar("select requires_manager_approval from task_type where task_type = @task_type", new SqlParameter("@task_type", "LOADTICKETS")).ToString();
            int mgrApprovalLimit = 0;
            try
            {
                mgrApprovalLimit = Convert.ToInt32(_utilities.getParafaitDefaults("LOAD_TICKET_LIMIT_FOR_MANAGER_APPROVAL"));
            }
            catch { mgrApprovalLimit = 0; }
            if ((ticketsToLoad > mgrApprovalLimit && mgrApprovalLimit != 0 && _utilities.ParafaitEnv.ManagerId == -1) || (mgrApprovalLimit == 0 && (mgrApprovalRequired == "Y" && _utilities.ParafaitEnv.ManagerId == -1)))
            {
                if (!authenticateManager(ref _utilities.ParafaitEnv.ManagerId))
                {
                    throw new Exception(MessageContainerList.GetMessage(_utilities.ExecutionContext, 268));
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Set Ticket Allocation Details
        /// </summary>
        /// <param name="redemptionId">redemptionId</param>
        /// <param name="sqlTrx">sqlTrx</param>
        public void SetTicketAllocationDetails(int redemptionId, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(redemptionId, sqlTrx);
            SqlCommand sqlCmd = _utilities.getCommand(sqlTrx);
            string message = "";
            if (getManualTickets() > 0)
            {  //load manual ticket info
                try
                {
                    sqlCmd.CommandText = @"INSERT INTO [dbo].[RedemptionTicketAllocation]
                                               ([RedemptionId],[RedemptionGiftId],[ManualTickets],[GraceTickets],[CardId],[ETickets],[CurrencyId],[CurrencyQuantity],[CurrencyTickets],[ManualTicketReceiptId]
                                                ,[ReceiptTickets],[TurnInTickets] , [TrxId], [TrxLineId],[CreatedBy] ,[CreationDate],[LastUpdatedBy] ,[LastUpdatedDate] )
                                               VALUES
                                              (@redemptionId, null, @manualTickets,  null,null, null, null, null,null,null, null, null, null, null, @userId, GETDATE(), @userId, getDATE() )";

                    sqlCmd.Parameters.Clear();
                    sqlCmd.Parameters.AddWithValue("@redemptionId", redemptionId);
                    sqlCmd.Parameters.AddWithValue("@manualTickets", getManualTickets());
                    sqlCmd.Parameters.AddWithValue("@userId", ParafaitEnv.LoginID);
                    sqlCmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 1502);
                    log.Error(message + ": " + ex.Message);
                    throw new Exception(message);
                }
            }
            if (currencyList != null && currencyList.Count > 0)
            {  //load currencyList info
                try
                {
                    sqlCmd.CommandText = @"INSERT INTO [dbo].[RedemptionTicketAllocation]
                                               ([RedemptionId],[RedemptionGiftId],[ManualTickets],[GraceTickets],[CardId],[ETickets],[CurrencyId],[CurrencyQuantity],[CurrencyTickets],[ManualTicketReceiptId]
                                                ,[ReceiptTickets] , [TurnInTickets],[TrxId], [TrxLineId], [CreatedBy] ,[CreationDate],[LastUpdatedBy] ,[LastUpdatedDate], [RedemptionCurrencyRuleId], [RedemptionCurrencyRuleTicket], [SourceCurrencyRuleId] )
                                               VALUES
                                              (@redemptionId, null, null,  null,null, null, @currencyId, @currencyQuantity,@currencyTickets,null, null, null, null, null, @userId, GETDATE(), @userId, getDATE(), @redemptionCurrencyRuleId, @redemptionRuleTicketDelta, @sourceCurrencyRuleId)";

                    foreach (clsRedemptionCurrency redemptionCurrencyEntry in currencyList)
                    {
                        sqlCmd.Parameters.Clear();
                        sqlCmd.Parameters.AddWithValue("@redemptionId", redemptionId);
                        sqlCmd.Parameters.AddWithValue("@currencyId", (redemptionCurrencyEntry.currencyId) == -1 ? DBNull.Value : (object)(redemptionCurrencyEntry.currencyId));
                        sqlCmd.Parameters.AddWithValue("@currencyQuantity", redemptionCurrencyEntry.quantity);
                        sqlCmd.Parameters.AddWithValue("@currencyTickets", (redemptionCurrencyEntry.quantity * redemptionCurrencyEntry.ValueInTickets) == 0 ? DBNull.Value : (object)(redemptionCurrencyEntry.quantity * redemptionCurrencyEntry.ValueInTickets));
                        sqlCmd.Parameters.AddWithValue("@userId", ParafaitEnv.LoginID);
                        sqlCmd.Parameters.AddWithValue("@redemptionCurrencyRuleId", (redemptionCurrencyEntry.redemptionCurrencyRuleId) == -1 ? DBNull.Value : (object)(redemptionCurrencyEntry.redemptionCurrencyRuleId));
                        sqlCmd.Parameters.AddWithValue("@redemptionRuleTicketDelta", (redemptionCurrencyEntry.quantity * redemptionCurrencyEntry.redemptionRuleTicketDelta) == null ? DBNull.Value : (object)(redemptionCurrencyEntry.quantity * redemptionCurrencyEntry.redemptionRuleTicketDelta));
                        sqlCmd.Parameters.AddWithValue("@sourceCurrencyRuleId", (redemptionCurrencyEntry.sourceCurrencyRuleId) < 0 ? DBNull.Value : (object)(redemptionCurrencyEntry.sourceCurrencyRuleId));
                        sqlCmd.ExecuteNonQuery();

                        if (redemptionCurrencyEntry.productId != DBNull.Value && redemptionCurrencyEntry.currencyId != -1)
                        {
                            UpdateRedemptionCurrencyInventory(redemptionCurrencyEntry, sqlTrx);
                        }
                    }
                }
                catch (Exception ex)
                {
                    message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 1503);
                    log.Error(message + ": " + ex.Message);
                    throw new Exception(message);
                }
            }
            if (scanTicketList != null && scanTicketList.Count > 0)
            {  //load scanTicketList info
                try
                {

                    sqlCmd.CommandText = @"INSERT INTO [dbo].[RedemptionTicketAllocation]
                                               ([RedemptionId],[RedemptionGiftId],[ManualTickets],[GraceTickets],[CardId],[ETickets],[CurrencyId],[CurrencyQuantity],[CurrencyTickets],[ManualTicketReceiptId]
                                                ,[ReceiptTickets] , [TurnInTickets],[TrxId], [TrxLineId],[CreatedBy] ,[CreationDate],[LastUpdatedBy] ,[LastUpdatedDate] )
                                               VALUES
                                              (@redemptionId, null, null,  null,null, null, null, null,null,@ticketReceiptId,@receiptTickets, null, null, null, @userId, GETDATE(), @userId, getDATE() )";

                    foreach (clsRedemption.clsScanTickets scanTicketReceiptEntry in scanTicketList)
                    {
                        int ticketReceiptNumber = GetTicketReceiptNumber(scanTicketReceiptEntry.barCode, sqlCmd.Transaction);
                        sqlCmd.Parameters.Clear();
                        sqlCmd.Parameters.AddWithValue("@redemptionId", redemptionId);
                        //sqlCmd.Parameters.AddWithValue("@ticketReceiptNo", scanTicketReceiptEntry.barCode);
                        sqlCmd.Parameters.AddWithValue("@ticketReceiptId", ticketReceiptNumber);
                        sqlCmd.Parameters.AddWithValue("@receiptTickets", scanTicketReceiptEntry.Tickets);
                        sqlCmd.Parameters.AddWithValue("@userId", ParafaitEnv.LoginID);
                        sqlCmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 1504);
                    log.Error(message + ": " + ex.Message);
                    throw new Exception(message);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Set Legacy Ticket Allocation Details
        /// </summary>
        /// <param name="redemptionId">redemptionId</param>
        /// <param name="tickets">tickets</param>
        /// <param name="originalBarcode">originalBarcode</param>
        /// <param name="sqlTrx">sqlTrx</param>
        public void SetLegacyTicketAllocationDetails(int redemptionId, int tickets, string originalBarcode, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(redemptionId, tickets, originalBarcode, sqlTrx);
            SqlCommand sqlCmd = _utilities.getCommand(sqlTrx);
            string message = "";
            if (tickets > 0)
            {  //load scanTicketList info
                try
                {
                    int ticketReceiptId = GetTicketReceiptNumber(originalBarcode, sqlTrx);
                    sqlCmd.CommandText = @"INSERT INTO [dbo].[RedemptionTicketAllocation]
                                               ([RedemptionId],[RedemptionGiftId],[ManualTickets],[GraceTickets],[CardId],[ETickets],[CurrencyId],[CurrencyQuantity],[CurrencyTickets],[ManualTicketReceiptId]
                                               ,[ManualTicketReceiptNo], [ReceiptTickets] , [TurnInTickets],[TrxId], [TrxLineId],[CreatedBy] ,[CreationDate],[LastUpdatedBy] ,[LastUpdatedDate] )
                                               VALUES
                                              (@redemptionId, null, null,  null,null, null, null, null,null,@ticketReceiptId, @ticketReceiptNo, @receiptTickets, null, null, null, @userId, GETDATE(), @userId, getDATE() )";

                    sqlCmd.Parameters.Clear();
                    sqlCmd.Parameters.AddWithValue("@redemptionId", redemptionId);
                    sqlCmd.Parameters.AddWithValue("@ticketReceiptNo", originalBarcode);
                    sqlCmd.Parameters.AddWithValue("@ticketReceiptId", DBNull.Value);
                    sqlCmd.Parameters.AddWithValue("@receiptTickets", tickets);
                    sqlCmd.Parameters.AddWithValue("@userId", ParafaitEnv.LoginID);
                    sqlCmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 1504);
                    log.Error(message + ": " + ex.Message);
                    throw new Exception(message);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Update Gift Ticket Source Info
        /// </summary>
        /// <param name="redemptionId">redemptionId</param>
        /// <param name="redemptionGiftId">redemptionGiftId</param>
        /// <param name="tickets">tickets</param>
        /// <param name="graceTicket">graceTicket</param>
        /// <param name="ticketSourceInfoObj">ticketSourceInfoObj</param>
        /// <param name="sqlTrx">sqlTrx</param>
        /// <returns></returns>
        internal List<TicketSourceInfo> UpdateGiftTicketSourceInfo(int redemptionId, int redemptionGiftId, int tickets, int graceTicket, List<TicketSourceInfo> ticketSourceInfoObj, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(redemptionId, redemptionGiftId, tickets, graceTicket, ticketSourceInfoObj, sqlTrx);
            int locTickets = tickets;
            SqlCommand cmd = _utilities.getCommand(sqlTrx);
            foreach (TicketSourceInfo ticketSourceobj in ticketSourceInfoObj)
            {
                if (ticketSourceobj.ticketSource != "Grace" && ticketSourceobj.balanceTickets != 0)
                {
                    if (ticketSourceobj.balanceTickets >= locTickets && locTickets > 0)
                    {
                        InsertTicketAllocationLine(redemptionId, redemptionGiftId, ticketSourceobj, locTickets, sqlTrx);
                        ticketSourceobj.balanceTickets = ticketSourceobj.balanceTickets - locTickets;
                        locTickets = 0;
                        break;
                    }
                    else
                    {
                        InsertTicketAllocationLine(redemptionId, redemptionGiftId, ticketSourceobj, ticketSourceobj.balanceTickets, sqlTrx);
                        locTickets = locTickets - ticketSourceobj.balanceTickets;
                        ticketSourceobj.balanceTickets = 0;
                    }
                }
            }

            if (graceTicket > 0)
            {
                int locGrace = graceTicket;

                foreach (TicketSourceInfo ticketSourceobj in ticketSourceInfoObj)
                {
                    if (ticketSourceobj.ticketSource == "Grace" && ticketSourceobj.balanceTickets != 0)
                    {
                        if (ticketSourceobj.balanceTickets >= locGrace && locGrace > 0)
                        {
                            InsertTicketAllocationLine(redemptionId, redemptionGiftId, ticketSourceobj, locGrace, sqlTrx);
                            ticketSourceobj.balanceTickets = ticketSourceobj.balanceTickets - locGrace;
                            locGrace = 0;
                            break;
                        }
                        else
                        {
                            InsertTicketAllocationLine(redemptionId, redemptionGiftId, ticketSourceobj, ticketSourceobj.balanceTickets, sqlTrx);
                            locGrace = locGrace - ticketSourceobj.balanceTickets;
                            ticketSourceobj.balanceTickets = 0;
                        }
                    }
                }

            }
            log.LogMethodExit(ticketSourceInfoObj);
            return ticketSourceInfoObj;
        }

        /// <summary>
        /// Insert Ticket Allocation Line
        /// </summary>
        /// <param name="redemptionId">redemptionId</param>
        /// <param name="redemptionGiftId">redemptionGiftId</param>
        /// <param name="ticketSourceobj">ticketSourceobj</param>
        /// <param name="tickets">tickets</param>
        /// <param name="sqlTrx">sqlTrx</param>
        void InsertTicketAllocationLine(int redemptionId, int redemptionGiftId, TicketSourceInfo ticketSourceobj, int tickets, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(redemptionId, redemptionGiftId, ticketSourceobj, tickets, sqlTrx);
            SqlCommand sqlCmd = _utilities.getCommand(sqlTrx);
            try
            {
                sqlCmd.CommandText = @"INSERT INTO [dbo].[RedemptionTicketAllocation]
                                               ([RedemptionId],[RedemptionGiftId],[ManualTickets],[GraceTickets],[CardId],[ETickets],[CurrencyId],[CurrencyQuantity],[CurrencyTickets],[ManualTicketReceiptId]
                                                ,[ReceiptTickets], [TurnInTickets] ,[TrxId], [TrxLineId], [CreatedBy] ,[CreationDate],[LastUpdatedBy] ,[LastUpdatedDate], [RedemptionCurrencyRuleId], [RedemptionCurrencyRuleTicket], [SourceCurrencyRuleId])
                                               VALUES
                                              (@redemptionId, @redemptionGiftId, @manualTickets, @graceTickets, @cardId, @eTickets, @currencyId, @currencyQuantity, @currencyTickets, @manualTicketReceiptId, @receiptTickets, null, null, null, @userId, GETDATE(), @userId, getDATE(), @redemptionCurrencyRuleId, @redemptionRuleTicketDelta, @sourceCurrencyRuleId )";

                sqlCmd.Parameters.Clear();
                sqlCmd.Parameters.AddWithValue("@redemptionId", redemptionId);
                if (redemptionGiftId == -1)
                    sqlCmd.Parameters.AddWithValue("@redemptionGiftId", DBNull.Value);
                else
                    sqlCmd.Parameters.AddWithValue("@redemptionGiftId", redemptionGiftId);

                if (ticketSourceobj.ticketSource == "Manual")
                {
                    sqlCmd.Parameters.AddWithValue("@manualTickets", tickets);
                }
                else
                {
                    sqlCmd.Parameters.AddWithValue("@manualTickets", DBNull.Value);
                }
                if (ticketSourceobj.ticketSource == "Grace")
                {
                    sqlCmd.Parameters.AddWithValue("@graceTickets", tickets);
                }
                else
                {
                    sqlCmd.Parameters.AddWithValue("@graceTickets", DBNull.Value);
                }
                if (ticketSourceobj.ticketSource == "Cards")
                {
                    sqlCmd.Parameters.AddWithValue("@eTickets", tickets);
                    sqlCmd.Parameters.AddWithValue("@cardId", ticketSourceobj.cardId);
                }
                else
                {
                    sqlCmd.Parameters.AddWithValue("@eTickets", DBNull.Value);
                    sqlCmd.Parameters.AddWithValue("@cardId", DBNull.Value);
                }
                if (ticketSourceobj.ticketSource == "Currency")
                {
                    sqlCmd.Parameters.AddWithValue("@currencyTickets", tickets);
                    sqlCmd.Parameters.AddWithValue("@currencyId", ticketSourceobj.currencyId);
                    sqlCmd.Parameters.AddWithValue("@currencyQuantity", Math.Round((decimal)(tickets / ticketSourceobj.currencyValueInTickets), 2));
                    sqlCmd.Parameters.AddWithValue("@sourceCurrencyRuleId", (ticketSourceobj.sourceCurrencyRuleId) < 0 ? DBNull.Value : (object)(ticketSourceobj.sourceCurrencyRuleId));
                }
                else
                {
                    sqlCmd.Parameters.AddWithValue("@currencyTickets", DBNull.Value);
                    sqlCmd.Parameters.AddWithValue("@currencyId", DBNull.Value);
                    sqlCmd.Parameters.AddWithValue("@sourceCurrencyRuleId", DBNull.Value);
                }
                if (ticketSourceobj.ticketSource == "RedemptionCurrencyRule")
                {
                    sqlCmd.Parameters.AddWithValue("@redemptionCurrencyRuleId", (ticketSourceobj.redemptionCurrencyRuleId) == -1 ? DBNull.Value : (object)ticketSourceobj.redemptionCurrencyRuleId);
                    sqlCmd.Parameters.AddWithValue("@redemptionRuleTicketDelta", (Convert.ToDouble(ticketSourceobj.redemptionRuleTicketDelta * (Math.Round((decimal)(tickets / (Convert.ToDouble(ticketSourceobj.redemptionRuleTicketDelta))), 2)))));
                    sqlCmd.Parameters.AddWithValue("@currencyQuantity", Math.Round((decimal)(tickets / (Convert.ToDouble(ticketSourceobj.redemptionRuleTicketDelta))), 2));
                }
                else
                {
                    sqlCmd.Parameters.AddWithValue("@redemptionCurrencyRuleId", DBNull.Value);
                    sqlCmd.Parameters.AddWithValue("@redemptionRuleTicketDelta", DBNull.Value);
                }
                if (ticketSourceobj.ticketSource != "RedemptionCurrencyRule" && ticketSourceobj.ticketSource != "Currency")
                {
                    sqlCmd.Parameters.AddWithValue("@currencyQuantity", DBNull.Value);
                }
                if (ticketSourceobj.ticketSource == "Receipt")
                {
                    sqlCmd.Parameters.AddWithValue("@receiptTickets", tickets);
                    int manualTicketReceiptId = GetTicketReceiptNumber(ticketSourceobj.receiptNo, sqlCmd.Transaction);
                    sqlCmd.Parameters.AddWithValue("@manualTicketReceiptId", manualTicketReceiptId);
                }
                else
                {
                    sqlCmd.Parameters.AddWithValue("@receiptTickets", DBNull.Value);
                    sqlCmd.Parameters.AddWithValue("@manualTicketReceiptId", DBNull.Value);
                }
                sqlCmd.Parameters.AddWithValue("@userId", ParafaitEnv.LoginID);
                sqlCmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(MessageContainerList.GetMessage(_utilities.ExecutionContext, 1505));
            }
            log.LogMethodExit();
        }

        //void InsertTicketAllocationLine(int redemptionId, TicketSourceInfo ticketSourceobj, int tickets, SqlTransaction sqlTrx)
        //{
        //    log.LogMethodEntry(redemptionId, ticketSourceobj, tickets, sqlTrx);
        //    SqlCommand sqlCmd = _utilities.getCommand(sqlTrx);
        //    try
        //    {
        //        sqlCmd.CommandText = @"INSERT INTO [dbo].[RedemptionTicketAllocation]
        //                                       ([RedemptionId],[RedemptionGiftId],[ManualTickets],[GraceTickets],[CardId],[ETickets],[CurrencyId],[CurrencyQuantity],[CurrencyTickets],[ManualTicketReceiptId]
        //                                        ,[ReceiptTickets], [TurnInTickets] ,[TrxId], [TrxLineId], [CreatedBy] ,[CreationDate],[LastUpdatedBy] ,[LastUpdatedDate] )
        //                                       VALUES
        //                                      (@redemptionId, null, @manualTickets, @graceTickets, @cardId, @eTickets, @currencyId, @currencyQuantity, @currencyTickets, @manualTicketReceiptId, @receiptTickets, @turnInTickets, null, null, @userId, GETDATE(), @userId, getDATE() )";

        //        sqlCmd.Parameters.Clear();
        //        sqlCmd.Parameters.AddWithValue("@redemptionId", redemptionId);
        //        //  sqlCmd.Parameters.AddWithValue("@redemptionGiftId", redemptionGiftId);
        //        if (ticketSourceobj.ticketSource == "Manual")
        //        {
        //            sqlCmd.Parameters.AddWithValue("@manualTickets", tickets);
        //        }
        //        else
        //        {
        //            sqlCmd.Parameters.AddWithValue("@manualTickets", DBNull.Value);
        //        }
        //        if (ticketSourceobj.ticketSource == "Grace")
        //        {
        //            sqlCmd.Parameters.AddWithValue("@graceTickets", tickets);
        //        }
        //        else
        //        {
        //            sqlCmd.Parameters.AddWithValue("@graceTickets", DBNull.Value);
        //        }
        //        if (ticketSourceobj.ticketSource == "Cards")
        //        {
        //            sqlCmd.Parameters.AddWithValue("@eTickets", tickets);
        //            sqlCmd.Parameters.AddWithValue("@cardId", ticketSourceobj.cardId);
        //        }
        //        else
        //        {
        //            sqlCmd.Parameters.AddWithValue("@eTickets", DBNull.Value);
        //            sqlCmd.Parameters.AddWithValue("@cardId", DBNull.Value);
        //        }
        //        if (ticketSourceobj.ticketSource == "Currency")
        //        {
        //            sqlCmd.Parameters.AddWithValue("@currencyTickets", tickets);
        //            sqlCmd.Parameters.AddWithValue("@currencyId", ticketSourceobj.currencyId);
        //            sqlCmd.Parameters.AddWithValue("@currencyQuantity", ticketSourceobj.currencyQuantity);
        //        }
        //        else
        //        {
        //            sqlCmd.Parameters.AddWithValue("@currencyTickets", DBNull.Value);
        //            sqlCmd.Parameters.AddWithValue("@currencyId", DBNull.Value);
        //            sqlCmd.Parameters.AddWithValue("@currencyQuantity", DBNull.Value);
        //        }
        //        if (ticketSourceobj.ticketSource == "Receipt")
        //        {
        //            sqlCmd.Parameters.AddWithValue("@receiptTickets", tickets);
        //            int manualTicketReceiptId = GetTicketReceiptNumber(ticketSourceobj.receiptNo, sqlCmd.Transaction);
        //            sqlCmd.Parameters.AddWithValue("@manualTicketReceiptId", manualTicketReceiptId);
        //        }
        //        else
        //        {
        //            sqlCmd.Parameters.AddWithValue("@receiptTickets", DBNull.Value);
        //            sqlCmd.Parameters.AddWithValue("@manualTicketReceiptId", DBNull.Value);
        //        }
        //        if (ticketSourceobj.ticketSource == "TurnIn")
        //        {
        //            sqlCmd.Parameters.AddWithValue("@turnInTickets", tickets);
        //        }
        //        else
        //        {
        //            sqlCmd.Parameters.AddWithValue("@turnInTickets", DBNull.Value);
        //        }
        //        sqlCmd.Parameters.AddWithValue("@userId", ParafaitEnv.LoginID);
        //        sqlCmd.ExecuteNonQuery();

        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex);
        //        throw new Exception(MessageContainerList.GetMessage(_utilities.ExecutionContext,1505));
        //    }
        //    log.LogMethodExit();
        //}

        /// <summary>
        /// Create Redemption Ticket Allocation
        /// </summary>
        /// <param name="redemptionId">redemptionId</param>
        /// <param name="balancePhysicalTickets">balancePhysicalTickets</param>
        /// <param name="cardList">cardList</param>
        /// <param name="ticketSourceInfoObj">ticketSourceInfoObj</param>
        /// <param name="sqlTrx">sqlTrx</param>
        public void CreateRedemptionTicketAllocation(int redemptionId, int balancePhysicalTickets, List<clsCards> cardList, List<TicketSourceInfo> ticketSourceInfoObj, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(redemptionId, balancePhysicalTickets, cardList, ticketSourceInfoObj, sqlTrx);
            SqlCommand sqlCmd = _utilities.getCommand(sqlTrx);
            try
            {
                foreach (TicketSourceInfo item in ticketSourceInfoObj)
                {
                    if (item.balanceTickets > 0 && item.ticketSource != "Cards")
                    {
                        InsertTicketAllocationLine(redemptionId, -1, item, item.balanceTickets, sqlTrx);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit();
                throw new Exception(MessageContainerList.GetMessage(_utilities.ExecutionContext, 1506));
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Transfer Legacy Ticket Receipt
        /// </summary>
        /// <param name="originalBarcode">originalBarcode</param>
        /// <param name="tickets">tickets</param>
        /// <param name="issueDate">issueDate</param>
        /// <param name="card">card</param>
        /// <param name="sqlTrx">sqlTrx</param>
        /// <returns></returns>
        public int TransferLegacyTicketReceipt(string originalBarcode, int tickets, DateTime issueDate, clsCards card, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(originalBarcode, tickets, issueDate, card, sqlTrx);
            string message = "";
            int newTicketReceiptId = -1;
            if (tickets > 0)
            {
                if (tickets > ParafaitEnv.LOAD_TICKETS_LIMIT)
                {
                    //Sorry, cannot proceed with &1 &2. Load ticket limit is &3 
                    throw new Exception(MessageContainerList.GetMessage(_utilities.ExecutionContext, 2830, tickets, POSStatic.TicketTermVariant,
                                                                    ParafaitEnv.LOAD_TICKETS_LIMIT.ToString()));
                    // throw new Exception(MessageContainer.GetMessage(_utilities.ExecutionContext,MessageContainer.GetMessage(_utilities.ExecutionContext,35, ParafaitEnv.LOAD_TICKETS_LIMIT.ToString(), POSStatic.TicketTermVariant)));
                }

                SqlCommand sqlCmd = _utilities.getCommand(sqlTrx);
                SqlTransaction cmd_trx = sqlCmd.Transaction;
                try
                {
                    sqlCmd.CommandText = "Insert into Redemption (card_id, primary_card_number, ReceiptTickets, manual_tickets, eTickets, GraceTickets, CurrencyTickets, redeemed_date, LastUpdatedBy, Source, RedemptionOrderNo, LastUpdateDate, OrderCompletedDate, OrderDeliveredDate, RedemptionStatus, customerId, posmachineId) " +
                                                " Values(@card_id, @primary_card_number, @receiptTickets, @manual_tickets, @eTickets, @GraceTickets, @currencyTickets, getdate(), @LastUpdatedBy, @Source, @RedemptionOrderNo, getdate(), getdate(), getdate(), @RedemptionStatus, @CustomerId, @PosMachineId); SELECT @@IDENTITY";

                    if (card != null)
                    {
                        sqlCmd.Parameters.AddWithValue("@card_id", card.cardId);
                        sqlCmd.Parameters.AddWithValue("@primary_card_number", card.cardNumber);
                        if (cardList[0].customerId > -1)
                        { sqlCmd.Parameters.AddWithValue("@CustomerId", cardList[0].customerId); }
                        else
                        { sqlCmd.Parameters.AddWithValue("@CustomerId", DBNull.Value); }
                    }
                    else
                    {
                        sqlCmd.Parameters.AddWithValue("@card_id", DBNull.Value);
                        sqlCmd.Parameters.AddWithValue("@primary_card_number", "");
                        sqlCmd.Parameters.AddWithValue("@CustomerId", DBNull.Value);
                    }

                    sqlCmd.Parameters.AddWithValue("@ReceiptTickets", tickets);
                    sqlCmd.Parameters.AddWithValue("@currencyTickets", 0);
                    sqlCmd.Parameters.AddWithValue("@manual_tickets", 0);
                    sqlCmd.Parameters.AddWithValue("@eTickets", 0);
                    sqlCmd.Parameters.AddWithValue("@Source", "POS Redemption");
                    sqlCmd.Parameters.AddWithValue("@GraceTickets", 0);
                    sqlCmd.Parameters.AddWithValue("@LastUpdatedBy", ParafaitEnv.LoginID);
                    sqlCmd.Parameters.AddWithValue("@CreatedBy", ParafaitEnv.LoginID);
                    sqlCmd.Parameters.AddWithValue("@RedemptionOrderNo", GetNextRedemptionOrderNo(ParafaitEnv.POSMachineId, null));
                    sqlCmd.Parameters.AddWithValue("@RedemptionStatus", "DELIVERED");
                    sqlCmd.Parameters.AddWithValue("@PosMachineId", ParafaitEnv.POSMachineId);

                    int redemptionId = Convert.ToInt32((Decimal)sqlCmd.ExecuteScalar());

                    if (card != null)
                    {
                        try
                        {
                            sqlCmd.CommandText = "Insert into Redemption_cards (redemption_id, card_number, card_id, ticket_count, LastUpdateDate, LastUpdatedBy, CreationDate, CreatedBy) " +
                                                    " Values (@redemption_id, @card_no, @card_id, @ticket_count, getdate(), @LastUpdatedBy, getdate(), @CreatedBy)";

                            //Insert into redemption cards table
                            sqlCmd.Parameters.Clear();
                            sqlCmd.Parameters.AddWithValue("@card_no", card.cardNumber);
                            sqlCmd.Parameters.AddWithValue("@redemption_id", redemptionId);
                            sqlCmd.Parameters.AddWithValue("@card_id", card.cardId);
                            sqlCmd.Parameters.AddWithValue("@ticket_count", 0);
                            sqlCmd.Parameters.AddWithValue("@LastUpdatedBy", ParafaitEnv.LoginID);
                            sqlCmd.Parameters.AddWithValue("@CreatedBy", ParafaitEnv.LoginID);
                            sqlCmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 123, ex.Message);
                            log.Error(ex);
                            throw new Exception(message);
                        }
                    }

                    SetLegacyTicketAllocationDetails(redemptionId, tickets, originalBarcode, cmd_trx);
                    _RedemptionId = redemptionId;
                    newTicketReceiptId = printRealTicketReceipt(redemptionId, tickets, ref message, cmd_trx, issueDate);
                    if (newTicketReceiptId == -1)
                    {
                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(_utilities.ExecutionContext, 124),
                                                   MessageContainerList.GetMessage(_utilities.ExecutionContext, "Save Information") + MessageContainerList.GetMessage(_utilities.ExecutionContext, 2693, _ScreenNumber));
                        throw new Exception(MessageContainerList.GetMessage(_utilities.ExecutionContext, 124));
                    }
                    if (sqlTrx == null)
                        cmd_trx.Commit();

                    log.LogMethodExit();
                    return newTicketReceiptId;
                }
                catch (Exception ex)
                {
                    log.Fatal("Ends-RedeemTicketReceipt() due to exception " + ex.Message);
                    if (sqlTrx == null)
                        cmd_trx.Rollback();
                    _RedemptionId = -1;
                    throw new Exception(ex.Message);
                }
            }
            log.LogMethodExit(newTicketReceiptId);
            return newTicketReceiptId;
        }

        /// <summary>
        /// Create Load Ticket Redemption Order
        /// </summary>
        /// <param name="card">card</param>
        /// <param name="ticketSourceobjList">ticketSourceobjList</param>
        /// <param name="sqltrx">sqltrx</param>
        /// <returns></returns>
        public int CreateLoadTicketRedemptionOrder(Card card, List<TicketSourceInfo> ticketSourceobjList, SqlTransaction sqltrx)
        {
            log.LogMethodEntry(card, ticketSourceobjList, sqltrx);
            string message = "";
            int redemptionId = -1;


            int tickets = ticketSourceobjList.Sum(t => t.ticketValue);

            SqlCommand sqlCmd = _utilities.getCommand(sqltrx);
            SqlTransaction cmd_trx = sqlCmd.Transaction;
            try
            {
                sqlCmd.CommandText = "Insert into Redemption (card_id, primary_card_number, ReceiptTickets, manual_tickets, eTickets, GraceTickets, CurrencyTickets, redeemed_date, LastUpdatedBy, Source, RedemptionOrderNo, LastUpdateDate, OrderCompletedDate, OrderDeliveredDate, RedemptionStatus, customerId, PosMachineId) " +
                                            " Values(@card_id, @primary_card_number, @receiptTickets, @manual_tickets, @eTickets, @GraceTickets, @currencyTickets, getdate(), @LastUpdatedBy, @Source, @RedemptionOrderNo, getdate(), getdate(), getdate(), @RedemptionStatus, @CustomerId, @PosMachineId); SELECT @@IDENTITY";

                if (card != null)
                {
                    sqlCmd.Parameters.AddWithValue("@card_id", card.card_id);
                    sqlCmd.Parameters.AddWithValue("@primary_card_number", card.CardNumber);
                    if (card.customer_id > -1)
                    { sqlCmd.Parameters.AddWithValue("@CustomerId", card.customer_id); }
                    else
                    { sqlCmd.Parameters.AddWithValue("@CustomerId", DBNull.Value); }
                }
                else
                {
                    sqlCmd.Parameters.AddWithValue("@card_id", DBNull.Value);
                    sqlCmd.Parameters.AddWithValue("@primary_card_number", "");
                    sqlCmd.Parameters.AddWithValue("@CustomerId", DBNull.Value);
                }

                sqlCmd.Parameters.AddWithValue("@ReceiptTickets", 0);
                sqlCmd.Parameters.AddWithValue("@currencyTickets", 0);
                sqlCmd.Parameters.AddWithValue("@manual_tickets", tickets);
                sqlCmd.Parameters.AddWithValue("@eTickets", 0);
                sqlCmd.Parameters.AddWithValue("@Source", "POS Redemption");
                sqlCmd.Parameters.AddWithValue("@GraceTickets", 0);
                sqlCmd.Parameters.AddWithValue("@LastUpdatedBy", ParafaitEnv.LoginID);
                sqlCmd.Parameters.AddWithValue("@CreatedBy", ParafaitEnv.LoginID);
                sqlCmd.Parameters.AddWithValue("@RedemptionOrderNo", GetNextRedemptionOrderNo(ParafaitEnv.POSMachineId, null));
                sqlCmd.Parameters.AddWithValue("@RedemptionStatus", "DELIVERED");
                sqlCmd.Parameters.AddWithValue("@PosMachineId", ParafaitEnv.POSMachineId);
                redemptionId = Convert.ToInt32((Decimal)sqlCmd.ExecuteScalar());

                if (card != null)
                {
                    try
                    {
                        sqlCmd.CommandText = "Insert into Redemption_cards (redemption_id, card_number, card_id, ticket_count, LastUpdateDate, LastUpdatedBy, CreationDate, CreatedBy) " +
                                                " Values (@redemption_id, @card_no, @card_id, @ticket_count, getdate(), @LastUpdatedBy, getdate(), @CreatedBy)";

                        //Insert into redemption cards table
                        sqlCmd.Parameters.Clear();
                        sqlCmd.Parameters.AddWithValue("@card_no", card.CardNumber);
                        sqlCmd.Parameters.AddWithValue("@redemption_id", redemptionId);
                        sqlCmd.Parameters.AddWithValue("@card_id", card.card_id);
                        sqlCmd.Parameters.AddWithValue("@ticket_count", 0);
                        sqlCmd.Parameters.AddWithValue("@LastUpdatedBy", ParafaitEnv.LoginID);
                        sqlCmd.Parameters.AddWithValue("@CreatedBy", ParafaitEnv.LoginID);
                        sqlCmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 123, ex.Message);
                        log.Error(ex);
                        throw new Exception(message);
                    }
                }

                foreach (TicketSourceInfo ticketSourceobj in ticketSourceobjList)
                {
                    InsertTicketAllocationLine(redemptionId, -1, ticketSourceobj, ticketSourceobj.ticketValue, cmd_trx);
                }


                log.LogMethodExit(redemptionId);
                return redemptionId;
            }
            catch (Exception ex)
            {
                log.Fatal("Ends-RedeemTicketReceipt() due to exception " + ex.Message);
                redemptionId = -1;
                log.LogMethodExit(redemptionId);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Get Ticket Receipt Number
        /// </summary>
        /// <param name="receiptNumber">receiptNumber</param>
        /// <param name="sqlTrx">sqlTrx</param>
        /// <returns></returns>
        int GetTicketReceiptNumber(string receiptNumber, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(receiptNumber, sqlTrx);
            int ticketReceiptId = -1;
            object ticketId = _utilities.executeScalar(@"select top 1 id from manualticketreceipts where manualticketreceiptNo = @receiptNumber
                                                           order by lastupdatedBy desc ", sqlTrx, new SqlParameter("@receiptNumber", receiptNumber));
            if (ticketId != null && ticketId != DBNull.Value)
                ticketReceiptId = Convert.ToInt32(ticketId);
            log.LogMethodExit(ticketReceiptId);
            return ticketReceiptId;
        }

        /// <summary>
        /// Check Redemption Currency Access
        /// </summary>
        /// <param name="currencyName">currencyName</param>
        /// <param name="currencyGUID">currencyGUID</param>
        /// <returns></returns>
        bool CheckRedemptionCurrencyAccess(string currencyName, string currencyGUID)
        {
            log.LogMethodEntry(currencyName, currencyGUID);
            bool retVal = false;
            string rcAccessEnabled = "N";
            try { rcAccessEnabled = _utilities.getParafaitDefaults("ENABLE_REDEMPTION_CURRENCY_ACCESS_CONTROL"); } catch { rcAccessEnabled = "N"; }
            if (rcAccessEnabled == "Y")
            {
                try
                {
                    Users currentUser = new Users(_utilities.ExecutionContext, ParafaitEnv.LoginID);
                    ManagementFormAccessListBL managementFormAccessListBL = new ManagementFormAccessListBL(_utilities.ExecutionContext);
                    if(currentUser.UserDTO.RoleId != -1)
                    {
                        retVal = managementFormAccessListBL.HasMgmtFormAccess("Data Access", "Redemption Currency", currencyName, currentUser.UserDTO.RoleId, currencyGUID);
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while checking management form access", ex);
                    retVal = false;
                }
            }
            else
            {
                retVal = true;
            }
            log.LogMethodExit(retVal);
            return retVal;
        }

        /// <summary>
        /// Populate RC ShortCut Keys
        /// </summary>
        /// <returns></returns>
        public List<Tuple<Byte[], string>> PopulateRCShortCutKeys()
        {
            log.LogMethodEntry();
            List<Tuple<Byte[], string>> keyList = new List<Tuple<Byte[], string>>();
            RedemptionCurrencyList redemptionCurrencyList = new RedemptionCurrencyList(_utilities.ExecutionContext);
            List<KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>> searchParameters = new List<KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>>();
            searchParameters.Add(new KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>(RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.ISACTIVE, "1"));
            searchParameters.Add(new KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>(RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.SITE_ID, ((_utilities.ParafaitEnv.IsCorporate && _utilities.ParafaitEnv.IsMasterSite) ? _utilities.ParafaitEnv.SiteId : -1).ToString()));
            List<RedemptionCurrencyDTO> redemptionCurrencyDTOList = redemptionCurrencyList.GetAllRedemptionCurrency(searchParameters);
            if (redemptionCurrencyDTOList != null && redemptionCurrencyDTOList.Count > 0)
            {
                foreach (RedemptionCurrencyDTO redemptionCurrencyDTO in redemptionCurrencyDTOList)
                {
                    if (!String.IsNullOrEmpty(redemptionCurrencyDTO.ShortCutKeys))
                    {
                        keyList.Add(new Tuple<Byte[], string>(Encoding.Unicode.GetBytes(redemptionCurrencyDTO.ShortCutKeys.ToUpper()), redemptionCurrencyDTO.CurrencyName));
                    }
                }
            }
            log.LogMethodExit(keyList);
            return keyList;
        }

        /// <summary>
        /// Apply Currency Rule
        /// </summary>
        /// <param name="redemptionCurrencyRuleBLList">redemptionCurrencyRuleBLList</param>
        public void ApplyCurrencyRule(List<RedemptionCurrencyRuleBL> redemptionCurrencyRuleBLList)
        {
            log.LogMethodEntry(redemptionCurrencyRuleBLList);
            if (redemptionCurrencyRuleBLList != null && redemptionCurrencyRuleBLList.Any())
            {
                List<int> userAddedCurrencyIdList = new List<int>();
                List<Tuple<int, int, List<int>>> appliedRuleIdAndCurrencyIdList = new List<Tuple<int, int, List<int>>>();
                List<clsRedemptionCurrency> appliedCurrencyDTOList = new List<clsRedemptionCurrency>();
                List<KeyValuePair<int, List<RedemptionCurrencyDTO>>> appliedRuleList = new List<KeyValuePair<int, List<RedemptionCurrencyDTO>>>();

                for (int i = currencyList.Count - 1; i >= 0; i--)
                {
                    if (currencyList[i].redemptionCurrencyRuleId > -1)
                    {
                        currencyList.RemoveAt(i);
                    }
                    else
                    {
                        currencyList[i].sourceCurrencyRuleId = -1;
                    }
                }

                List<clsRedemptionCurrency> singleQtyEntries = new List<clsRedemptionCurrency>();
                foreach (clsRedemptionCurrency currencyEntry in currencyList)
                {
                    for (int j = 0; j < currencyEntry.quantity; j++)
                    {
                        userAddedCurrencyIdList.Add(currencyEntry.currencyId);
                        if (j > 0)
                        {
                            clsRedemptionCurrency newEntry = CreateEntryWithQuantityAsOne(currencyEntry);
                            singleQtyEntries.Add(newEntry);
                        }
                    }
                    if (currencyEntry.quantity > 1)
                    {
                        currencyEntry.quantity = 1;
                    }
                }
                if (singleQtyEntries != null && singleQtyEntries.Any())
                {
                    currencyList.AddRange(singleQtyEntries);
                }
                if (userAddedCurrencyIdList != null && userAddedCurrencyIdList.Any())
                {
                    //redemptionCurrencyRuleBLList is a sorted list based on priority
                    foreach (RedemptionCurrencyRuleBL redemptionCurrencyRuleBL in redemptionCurrencyRuleBLList)
                    {

                        List<KeyValuePair<int, List<int>>> ruleApplicableCurrencyIdList = redemptionCurrencyRuleBL.IsRuleApplicable(userAddedCurrencyIdList);
                        if (ruleApplicableCurrencyIdList != null && ruleApplicableCurrencyIdList.Any())
                        {
                            appliedRuleIdAndCurrencyIdList.Add(new Tuple<int, int, List<int>>(redemptionCurrencyRuleBL.GetRedemptionCurrencyRuleDTO.RedemptionCurrencyRuleId, ruleApplicableCurrencyIdList[0].Key, ruleApplicableCurrencyIdList[0].Value));
                        }

                    }
                    if (appliedRuleIdAndCurrencyIdList != null && appliedRuleIdAndCurrencyIdList.Any())
                    {
                        for (int i = 0; i < appliedRuleIdAndCurrencyIdList.Count; i++)
                        {
                            //Loop through the currency list applied with rule id
                            for (int j = 0; j < appliedRuleIdAndCurrencyIdList[i].Item3.Count; j++)
                            {
                                foreach (clsRedemptionCurrency currencyEntry in currencyList)
                                {
                                    if (appliedRuleIdAndCurrencyIdList[i].Item3[j] == currencyEntry.currencyId && currencyEntry.sourceCurrencyRuleId == -1)
                                    {
                                        currencyEntry.sourceCurrencyRuleId = appliedRuleIdAndCurrencyIdList[i].Item1;
                                        break;
                                    }
                                }
                            }
                        }

                        for (int i = 0; i < appliedRuleIdAndCurrencyIdList.Count; i++)
                        {
                            clsRedemptionCurrency newRuleEntry = CreateCurrencyRuleEntry(appliedRuleIdAndCurrencyIdList[i].Item1, appliedRuleIdAndCurrencyIdList[i].Item2, redemptionCurrencyRuleBLList);
                            currencyList.Add(newRuleEntry);
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        private clsRedemptionCurrency CreateEntryWithQuantityAsOne(clsRedemptionCurrency currencyEntry)
        {
            log.LogMethodEntry(currencyEntry);
            clsRedemptionCurrency newEntry = new clsRedemptionCurrency();
            newEntry.barCode = currencyEntry.barCode;
            newEntry.currencyId = currencyEntry.currencyId;
            newEntry.currencyName = currencyEntry.currencyName;
            newEntry.productId = currencyEntry.productId;
            newEntry.quantity = 1;
            newEntry.redemptionCurrencyRuleId = currencyEntry.redemptionCurrencyRuleId;
            newEntry.redemptionCurrencyRuleName = currencyEntry.redemptionCurrencyRuleName;
            newEntry.redemptionRuleTicketDelta = currencyEntry.redemptionRuleTicketDelta;
            newEntry.sourceCurrencyRuleId = -1;
            newEntry.ValueInTickets = currencyEntry.ValueInTickets;
            log.LogMethodExit(newEntry);
            return newEntry;
        }
        private clsRedemptionCurrency CreateCurrencyRuleEntry(int currencyRuleId, int ruleQty, List<RedemptionCurrencyRuleBL> redemptionCurrencyRuleBLList)
        {
            log.LogMethodEntry(currencyRuleId, ruleQty);
            clsRedemptionCurrency newEntry = new clsRedemptionCurrency();
            newEntry.barCode = null;
            newEntry.currencyId = -1;
            newEntry.currencyName = string.Empty;
            newEntry.productId = -1;
            newEntry.quantity = ruleQty;
            newEntry.redemptionCurrencyRuleId = currencyRuleId;
            RedemptionCurrencyRuleBL redemptionCurrencyRuleBL = GetCurrencyRuleBL(redemptionCurrencyRuleBLList, currencyRuleId);
            newEntry.redemptionCurrencyRuleName = redemptionCurrencyRuleBL.GetRedemptionCurrencyRuleDTO.RedemptionCurrencyRuleName;
            newEntry.redemptionRuleTicketDelta = redemptionCurrencyRuleBL.GetRuleDeltaTicket();
            newEntry.sourceCurrencyRuleId = -1;
            newEntry.ValueInTickets = 0;
            log.LogMethodExit(newEntry);
            return newEntry;
        }

        private RedemptionCurrencyRuleBL GetCurrencyRuleBL(List<RedemptionCurrencyRuleBL> redemptionCurrencyRuleBLList, int currencyRuleId)
        {
            log.LogMethodEntry(currencyRuleId);
            RedemptionCurrencyRuleBL redemptionCurrencyRuleBL = null;
            foreach (RedemptionCurrencyRuleBL item in redemptionCurrencyRuleBLList)
            {
                if (item.GetRedemptionCurrencyRuleDTO.RedemptionCurrencyRuleId == currencyRuleId)
                {
                    redemptionCurrencyRuleBL = item;
                    break;
                }
            }
            log.LogMethodExit(redemptionCurrencyRuleBL);
            return redemptionCurrencyRuleBL;
        }
    }
}
