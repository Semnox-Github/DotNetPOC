/********************************************************************************************
* Project Name - Parafait POS
* Description  - frmValidateCard 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
 * 2.70       01-Jul-2019     Lakshminarayana    Modified to add support for ULC cards 
 * 2.80       20-Aug-2019     Girish Kundar      Modified to add logger methods. 
 *2.130.4     22-Feb-2022     Mathew Ninan       Modified DateTime to ServerDateTime 
 *2.150.5     10-Oct-2023     Mathew Ninan       Added support for other entitlements and follow
 *                                               same logic as Game Server. Few UI changes.
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;//IP Address
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Game;
using Semnox.Parafait.ServerCore;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Communication;

namespace Parafait_POS.CardValidation
{
    public partial class frmValidateCard : Form
    {
        MessageUtils MessageUtils;
        Utilities Utilities;
        ParafaitEnv ParafaitEnv;
        int machineId = -1;//0;
        string cardNumber = string.Empty;
        string customerName = string.Empty;
        int panelCount = 0;
        string customerPhoto = string.Empty;
        string customerPhotoFullPath = string.Empty;
        int entitlementRemaining = 0;
        int customerID = -1;
        int TrxNumber = 0;
        int addControlCount = 0;//used for getting Controls count for refreshscreen part
        
        private readonly TagNumberParser tagNumberParser;

        //Begin: Added for logger function on 08-Mar-2016
       private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //End: Added for logger function on 08-Mar-2016
        List<string> lstCardsWOEntitlement = new List<string>();//card without Entitlements
        bool machineNotFound = false;//used for checking if machine not found

        public frmValidateCard()
        {
            //Begin: Added to Configure the logger root LogLevel using App.config on 08-March-2016           
            ////log = ParafaitUtils.Logger.setLogFileAppenderName(log);
            Semnox.Core.Utilities.Logger.setRootLogLevel(log);
            //End: Added to Configure the logger root LogLevel using App.config on 08-March-2016

            log.LogMethodEntry();
            InitializeComponent();
            Utilities = POSStatic.Utilities;
            ParafaitEnv = Utilities.ParafaitEnv;
            MessageUtils = Utilities.MessageUtils;
            tagNumberParser = new TagNumberParser(Utilities.ExecutionContext);
            Common.Devices.RegisterCardReaders(new EventHandler(CardScanCompleteEventHandle));
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the IP address of the system
        /// </summary>
        /// <returns></returns>
        private IPAddress GetIPAddress()
        {
            log.LogMethodEntry();
            IPAddress ipAddress = IPAddress.None;
            try
            {
                System.Net.IPAddress[] TempAd = System.Net.Dns.GetHostEntry(Dns.GetHostName()).AddressList;
                foreach (IPAddress ip in TempAd)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        ipAddress = ip;
                        break;
                    }
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit("Returns IPAddress");
            return ipAddress;
        }

        private void frmValidateCard_Load(object sender, EventArgs e)
        {
            try
            {
                log.LogMethodEntry();
                ParafaitEnv.Initialize();
                lblWelcomeTo.Text = MessageUtils.getMessage(415, ParafaitEnv.SiteName);
                //lblSiteName.Text = ParafaitEnv.SiteName;
                lblTransactionNumberLabel.Text = MessageUtils.getMessage("Transaction Number") + ":";
                lblAllMatchingLabel.Text = MessageUtils.getMessage("All Matching") + ":";
                btnExit.Text = MessageUtils.getMessage("Exit");
                btnCancel.Text = MessageUtils.getMessage("Cancel / Clear");
                btnOK.Text = MessageUtils.getMessage("OK");
                displayMessageLine("");
                string exeDir = System.IO.Path.GetDirectoryName(Environment.CommandLine.Replace("\"", ""));
                if (File.Exists(exeDir + "\\Resources\\ClientLogo.png"))
                {
                    pbLogo.Image = Image.FromFile(exeDir + "\\Resources\\ClientLogo.png");
                }
                else if (ParafaitEnv.CompanyLogo != null)
                {
                    pbLogo.Image = ParafaitEnv.CompanyLogo;
                }
                else
                {
                    pbLogo.Image = Properties.Resources.Semnox_Logo;
                }

                try
                {
                    log.Debug("Get the MachineIPAddress");
                    object objMachineIPAddress = GetIPAddress();
                    if (objMachineIPAddress != null)
                    {
                        object objIPWiseMachineId = Utilities.executeScalar("SELECT machine_id FROM machines WHERE IPAddress = @IPAddress", new SqlParameter("@IPAddress", objMachineIPAddress.ToString()));
                        if (objIPWiseMachineId != null)
                        {
                            machineId = Convert.ToInt32(objIPWiseMachineId);
                        }
                        else
                        {
                          machineNotFound = true;//Set as machine not found 
                        }
                    }
                    else
                    {
                        log.Debug("machineNotFound");
                        machineNotFound = true;//Set as machine not found 
                    }
                }
                catch
                {
                    log.Error("Exception : Machine Not Found");
                    machineNotFound = true;//Set as machine not found 
                }

                log.Debug("Check using MAC Address else using Machine Name");
                //Check using MAC Address else using Machine Name
                if (machineNotFound)
                {
                    string machineMacAddress = NetUtils.GetMacAddress(Environment.MachineName);
                    if (!string.IsNullOrEmpty(machineMacAddress) && machineMacAddress != null)
                    {
                        try
                        {
                            object objMACWiseMachineId = Utilities.executeScalar("SELECT machine_id FROM machines WHERE MACAddress = @MACAddress", new SqlParameter("@MACAddress", machineMacAddress.Trim().Replace('-', ':').ToUpper()));
                            if (objMACWiseMachineId != null)
                            {
                                machineId = Convert.ToInt32(objMACWiseMachineId);
                            }
                            else
                            {
                                object objMachineWiseId = Utilities.executeScalar("SELECT machine_id FROM machines WHERE machine_name = @machineName", new SqlParameter("@machineName", Environment.MachineName));
                                if (objMachineWiseId != null)
                                {
                                    machineId = Convert.ToInt32(objMachineWiseId);
                                }
                                else
                                {
                                    machineId = -1;//Set machine id -1
                                }
                            }
                        }
                        catch
                        {
                            machineId = -1;//Set machine id -1                                 
                        }
                    }
                    else
                    {
                        machineId = -1;//Set machine id -1
                    }
                }

                if (machineId == -1)
                {
                    //Shows the Register message and Close Application
                    lblTerminalName.Text = MessageUtils.getMessage("Invalid Terminal");
                    POSUtils.ParafaitMessageBox(MessageUtils.getMessage("Please register the Game Machine"));
                    Environment.Exit(1);
                }
                else
                {
                    lblTerminalName.Text = Environment.MachineName;
                }

                lblDate.Text = ServerDateTime.Now.ToString(ParafaitEnv.DATE_FORMAT);

                log.LogMethodExit();
            }
            catch(Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(ex.Message);
                Environment.Exit(1);
            }
        }

        private void frmValidateCard_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                clearControls();//Clear Flowlayoutpanel, transactionnumber ,matching flag in Display      
                Common.Devices.UnregisterCardReaders();
            }
            catch(Exception ex)
            {
                displayMessageLine(ex.Message);
                log.Fatal("Ends-frmValidateCard_FormClosing() due to exception"+ex.Message);//Added for logger function on 08-Mar-2016
            }
            log.LogMethodExit();
        }

        private void CardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (e is DeviceScannedEventArgs)
            {
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;
                TagNumber tagNumber;
                if (tagNumberParser.TryParse(checkScannedEvent.Message, out tagNumber) == false)
                {
                    string message = tagNumberParser.Validate(checkScannedEvent.Message);
                    displayMessageLine(message);
                    log.LogMethodExit(null, "Invalid Tag Number. " + message);
                    return;
                }

                string CardNumber = tagNumber.Value;
                log.LogVariableState("CardNumber :" , tagNumber.Value);
                try
                {
                    CardSwiped(CardNumber);
                }
                catch (Exception ex)
                {
                    displayMessageLine(ex.Message);
                    log.Fatal("Ends-CardScanCompleteEventHandle() due to exception " + ex.Message);//Added for logger function on 08-Mar-2016
                }
            }
            log.LogMethodExit();

        }

        private void CardSwiped(string CardNumber)
        {
            log.LogMethodEntry(CardNumber);
            displayMessageLine("");
            string cardValidity = string.Empty;
            int trxNo = -1;
            int trxLineId = -1;
            entitlementRemaining = 0;
            bool validateFlag = false;//used to set valid

            foreach (ValidateCardDataContol card in flowLayoutPanelSwipedCards.Controls.OfType<ValidateCardDataContol>())
            {
                if (card.cardNumber == CardNumber)
                {
                    displayMessageLine(MessageUtils.getMessage(59));
                    log.Info("Ends-CardSwiped(" + CardNumber + ") as Card Already Added");//Added for logger function on 08-Mar-2016
                    return;
                }
            }

            Card swipedCard = new Card(CardNumber, POSStatic.Utilities.ParafaitEnv.LoginID, POSStatic.Utilities);

            if (swipedCard.CardStatus == "NEW")
            {
                displayMessageLine(MessageUtils.getMessage(459));
                log.Info("Ends-CardSwiped(" + CardNumber + ") as Tapped card is a new card ");//Added for logger function on 08-Mar-2016
                log.LogMethodExit();
                return;
            }
            else if (swipedCard.technician_card.Equals('Y'))
            {
                displayMessageLine(MessageUtils.getMessage(197, CardNumber));
                log.Info("Ends-CardSwiped(" + CardNumber + ") as Tapped card is a technician card ");//Added for logger function on 08-Mar-2016
                log.LogMethodExit();
                return;
            }
            else if (swipedCard.siteId != -1 && swipedCard.siteId != ParafaitEnv.SiteId && ParafaitEnv.ALLOW_ROAMING_CARDS == "N")
            {
                displayMessageLine(MessageUtils.getMessage(133));
                log.Info("Ends-CardSwiped(" + CardNumber + ") as Tapped a Roaming cards");//Added for logger function on 08-Mar-2016
                log.LogMethodExit();
                return;
            }
            try
            {
                customerID = swipedCard.customer_id;
                cardNumber = swipedCard.CardNumber;
                if (customerID >= 0)
                {
                    CustomerDTO customerDTO = (new CustomerBL(Utilities.ExecutionContext, customerID)).CustomerDTO;
                    GetCustomerDetails(customerDTO, swipedCard.card_id);
                }
                else
                {
                    customerPhoto = "";
                    customerName = "";
                    customerPhotoFullPath = "";
                }
            }
            catch (Exception ex)
            {
                displayMessageLine(MessageUtils.getMessage("Unable to get customer details for card ", cardNumber));
                log.Fatal("Ends-CardSwiped(" + CardNumber + ") as unable to get customerDTO details due to exception " + ex.Message);
            }
            AccountDTO accountDTO = new AccountBL(Utilities.ExecutionContext, swipedCard.card_id, true).AccountDTO;
            DateTime CardEntitlementExpiryDate = DateTime.MinValue; //Utilities.executeScalar(@"SELECT Top 1 ExpiryDate FROM CardGames WHERE card_id = @cardId and ExpiryDate IS NOT NULL ORDER BY ExpiryDate DESC", new SqlParameter("@cardId", swipedCard.card_id));//used in both member and non member part
                                                                    // object objCardTypeId = swipedCard.CardTypeId;
            object objMembershipId = swipedCard.MembershipId;
            //if (objCardTypeId != DBNull.Value && objCardTypeId != null && Convert.ToInt32(objCardTypeId) != -1)
            if (objMembershipId != DBNull.Value && objMembershipId != null && Convert.ToInt32(objMembershipId) != -1 && (swipedCard.primaryCard != "Y"))
            {
                try
                {
                    log.Debug("For existing member");
                    //Member                    
                    object objCardIssueDate = swipedCard.issue_date;
                    if (objCardIssueDate != null && objCardIssueDate != DBNull.Value)
                    {
                        DateTime currentDate = DateTime.Parse(ServerDateTime.Now.ToString(ParafaitEnv.DATE_FORMAT));
                        string configCardValidity = Utilities.getParafaitDefaults("CARD_VALIDITY");
                        if (!string.IsNullOrEmpty(configCardValidity))
                        {
                            DateTime expiryDate = DateTime.Parse(objCardIssueDate.ToString()).AddMonths(Convert.ToInt32(configCardValidity));
                            CardEntitlementExpiryDate = expiryDate;
                            //DateTime memberCardValidity = DateTime.Parse(expiryDate.ToString(ParafaitEnv.DATE_FORMAT));
                            if (CardEntitlementExpiryDate < currentDate)
                            {
                                validateFlag = false;
                            }
                            else
                            {
                                validateFlag = true;
                            }
                        }
                        else
                        {
                            if (accountDTO != null && accountDTO.AccountCreditPlusDTOList != null)
                            {
                                List<AccountCreditPlusDTO> accountCreditPlusDTOs = accountDTO.AccountCreditPlusDTOList.Where(x => x.CreditPlusType == CreditPlusType.TIME
                                                                           && x.PeriodTo != null
                                                                           && x.PeriodTo > ServerDateTime.Now).OrderByDescending(x => x.PlayStartTime).OrderBy(x => x.PeriodTo).ToList();
                                if (accountCreditPlusDTOs != null && accountCreditPlusDTOs.Count > 0)
                                {
                                    foreach (AccountCreditPlusDTO accountCreditPlusDTO in accountCreditPlusDTOs)
                                    {
                                        if (accountCreditPlusDTO.PlayStartTime != null
                                            && 
                                            ((DateTime)accountCreditPlusDTO.PlayStartTime)
                                            .AddMinutes((Double)accountCreditPlusDTO.CreditPlus) < ServerDateTime.Now)
                                        {
                                            if (accountCreditPlusDTO.PlayStartTime != null)
                                                CardEntitlementExpiryDate = ((DateTime)accountCreditPlusDTO.PlayStartTime)
                                                .AddMinutes((Double)accountCreditPlusDTO.CreditPlus);
                                            else
                                                CardEntitlementExpiryDate = (DateTime)accountCreditPlusDTO.PeriodTo;
                                            entitlementRemaining = -1;
                                            continue;
                                        }
                                        if (accountCreditPlusDTO.PlayStartTime != null)
                                            CardEntitlementExpiryDate = ((DateTime)accountCreditPlusDTO.PlayStartTime)
                                            .AddMinutes((Double)accountCreditPlusDTO.CreditPlus);
                                        else
                                            CardEntitlementExpiryDate = (DateTime)accountCreditPlusDTO.PeriodTo;
                                        entitlementRemaining = -1;//Time entitlement exists, ignore game count
                                        if (accountCreditPlusDTO.TransactionId > -1)
                                            trxNo = accountCreditPlusDTO.TransactionId;
                                        if (accountCreditPlusDTO.TransactionLineId > -1)
                                            trxLineId = accountCreditPlusDTO.TransactionLineId;
                                        break;//break after getting one active time entitlement record.
                                    }
                                }
                                else
                                    entitlementRemaining = 0;
                            }
                            if (entitlementRemaining != -1
                                && accountDTO != null && accountDTO.AccountGameDTOList != null)
                            {
                                List<AccountGameDTO> accountGameDTOs = accountDTO.AccountGameDTOList.Where(x => x.ExpiryDate != null
                                                                           && x.ExpiryDate > ServerDateTime.Now)
                                                                           .OrderByDescending(x => x.ExpiryDate)
                                                                           .ToList();
                                if (accountGameDTOs != null && accountGameDTOs.Count > 0)
                                {
                                    CardEntitlementExpiryDate = (DateTime)accountGameDTOs[0].ExpiryDate;
                                    if (accountGameDTOs[0].TransactionId > -1)
                                        trxNo = accountGameDTOs[0].TransactionId;
                                    if (accountGameDTOs[0].TransactionLineId > -1)
                                        trxLineId = accountGameDTOs[0].TransactionLineId;
                                }
                            }
                            if (CardEntitlementExpiryDate != null
                                && CardEntitlementExpiryDate != DateTime.MinValue
                                && CardEntitlementExpiryDate < ServerDateTime.Now)
                            {
                                validateFlag = false;
                            }
                            else
                            {
                                validateFlag = true;
                            }
                        }
                    }
                    else
                    {
                        validateFlag = true;//changed to true so that if validity is not set, its valid.
                        displayMessageLine(MessageUtils.getMessage("Validity date is not set. Considered valid."));
                    }
                }
                catch (Exception ex)
                {
                    displayMessageLine(MessageUtils.getMessage("Unable to fetch Issue Date for card ", CardNumber));
                    log.Fatal("Ends-CardSwiped(" + CardNumber + ") as unable to fetch card IssueDate due to exception " + ex.Message);//Added for logger function on 08-Mar-2016
                }
            }
            else
            {
                try
                {
                    log.Debug("For Non Member");
                    //Non Member
                    if (accountDTO != null && accountDTO.AccountCreditPlusDTOList != null)
                    {
                        List<AccountCreditPlusDTO> accountCreditPlusDTOs = accountDTO.AccountCreditPlusDTOList.Where(x => x.CreditPlusType == CreditPlusType.TIME
                                                                   && x.PeriodTo != null
                                                                   && x.PeriodTo > ServerDateTime.Now)
                                                                   .OrderByDescending(x => x.PlayStartTime)
                                                                   .OrderBy(x => x.PeriodTo).ToList();
                        if (accountCreditPlusDTOs != null && accountCreditPlusDTOs.Count > 0)
                        {
                            foreach (AccountCreditPlusDTO accountCreditPlusDTO in accountCreditPlusDTOs)
                            {
                                if (accountCreditPlusDTO.PlayStartTime != null
                                    &&
                                    ((DateTime)accountCreditPlusDTO.PlayStartTime)
                                    .AddMinutes((Double)accountCreditPlusDTO.CreditPlus) < ServerDateTime.Now)
                                {
                                    if (accountCreditPlusDTO.PlayStartTime != null)
                                        CardEntitlementExpiryDate = ((DateTime)accountCreditPlusDTO.PlayStartTime)
                                        .AddMinutes((Double)accountCreditPlusDTO.CreditPlus);
                                    else
                                        CardEntitlementExpiryDate = (DateTime)accountCreditPlusDTO.PeriodTo;
                                    entitlementRemaining = -1;
                                    continue;
                                }
                                if (accountCreditPlusDTO.PlayStartTime != null)
                                    CardEntitlementExpiryDate = ((DateTime)accountCreditPlusDTO.PlayStartTime)
                                    .AddMinutes((Double)accountCreditPlusDTO.CreditPlus);
                                else
                                    CardEntitlementExpiryDate = (DateTime)accountCreditPlusDTO.PeriodTo;
                                entitlementRemaining = -1;//Time entitlement exists, ignore game count
                                if (accountCreditPlusDTO.TransactionId > -1)
                                    trxNo = accountCreditPlusDTO.TransactionId;
                                if (accountCreditPlusDTO.TransactionLineId > -1)
                                    trxLineId = accountCreditPlusDTO.TransactionLineId;
                                break;//break after getting one active time entitlement record.
                            }
                        }
                        else
                            entitlementRemaining = 0;
                    }
                    if (entitlementRemaining != -1
                        && accountDTO != null && accountDTO.AccountGameDTOList != null)
                    {
                        List<AccountGameDTO> accountGameDTOs = accountDTO.AccountGameDTOList.Where(x => x.ExpiryDate != null
                                                                   && x.ExpiryDate > ServerDateTime.Now).OrderByDescending(x => x.ExpiryDate).ToList();
                        if (accountGameDTOs != null && accountGameDTOs.Count > 0)
                        {
                            CardEntitlementExpiryDate = (DateTime)accountGameDTOs[0].ExpiryDate;
                            if (accountGameDTOs[0].TransactionId > -1)
                                trxNo = accountGameDTOs[0].TransactionId;
                            if (accountGameDTOs[0].TransactionLineId > -1)
                                trxLineId = accountGameDTOs[0].TransactionLineId;
                        }
                    }
                    if (CardEntitlementExpiryDate != null
                        && CardEntitlementExpiryDate < ServerDateTime.Now)
                    {
                        validateFlag = false;
                    }
                    else
                    {
                        validateFlag = true;
                    }
                }
                catch (Exception ex)
                {
                    displayMessageLine(MessageUtils.getMessage("Unable to fetch Expiry Date for card ", CardNumber));
                    log.Fatal("Ends-CardSwiped(" + CardNumber + ") as unable to fetch ExpiryDate from cardGames due to exception " + ex.Message);//Added for logger function on 08-Mar-2016
                }
            }
            try
            {
                //Transaction Number           
                //object objTrxId = Utilities.executeScalar(@"SELECT TOP 1 TrxId FROM trx_lines WHERE card_id = @cardId ORDER BY TrxId", new SqlParameter("@cardId", swipedCard.card_id));
                if (trxNo == -1)
                {
                    DataTable dtTrx = Utilities.executeDataTable(@"SELECT TOP 1 TrxId, lineId FROM trx_lines WHERE card_id = @cardId ORDER BY TrxId", new SqlParameter("@cardId", swipedCard.card_id));
                    if (dtTrx.Rows.Count > 0)
                    {

                        trxNo = Convert.ToInt32(dtTrx.Rows[0]["TrxId"]);
                        trxLineId = Convert.ToInt32(dtTrx.Rows[0]["lineId"]);
                    }
                    else
                    {
                        trxNo = 0;
                        displayMessageLine(MessageUtils.getMessage("Unable to find Transaction Number for the card ", CardNumber));
                        log.Info("CardSwiped(" + CardNumber + ") - unable to find Transaction Number for the card");//Added for logger function on 08-Mar-2016
                        validateFlag = false;
                        setMatchingFlag(validateFlag);
                        log.LogMethodExit();
                        return;
                    }
                }

                if (trxNo > 0)
                {
                    if (panelCount == 0)//for first tap
                    {
                        lblTransactionNumber.Text = trxNo.ToString();
                        TrxNumber = trxNo;
                        setMatchingFlag(true);
                    }
                    else
                    {
                        if (TrxNumber != trxNo)
                        {
                            setMatchingFlag(false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                displayMessageLine(MessageUtils.getMessage("Unable to get Transaction details for card ", cardNumber));
                log.Fatal("Ends-CardSwiped(" + CardNumber + ") as unable to get transaction details due to exception " + ex.Message);
            }

            try
            {
                if (customerID == -1)
                {
                    //Get Customer details from waiver if applicable else check if transaction has customer.
                    object objCustomerId = Utilities.executeScalar(@"SELECT cswh.signedBy
                                                                        FROM CustomerSignedWaiver csw, 
                                                                            CustomerSignedWaiverHeader cswh, 
	                                                                        waiversSigned ws
                                                                        WHERE ws.trxid = @trxid
                                                                        and ws.lineid = @lineId
                                                                        and ws.customerSignedWaiverId = csw.customerSignedWaiverId
                                                                        and csw.CustomerSignedWaiverHeaderId = cswh.CustomerSignedWaiverHeaderId
                                                                        and (csw.ExpiryDate is null or csw.expiryDate > getdate())"
                                                                     , new SqlParameter("@trxId", trxNo)
                                                                     , new SqlParameter("@lineId", trxLineId));
                    if (objCustomerId != null && objCustomerId != DBNull.Value)
                    {
                        customerID = Convert.ToInt32(objCustomerId);
                    }
                    else
                    {
                        objCustomerId = Utilities.executeScalar(@"SELECT customerId
                                                                    FROM trx_header
                                                                    WHERE trxid = @trxid"
                                                                    , new SqlParameter("@trxId", trxNo));
                        if (objCustomerId != null && objCustomerId != DBNull.Value)
                        {
                            customerID = Convert.ToInt32(objCustomerId);
                        }
                    }
                    if (customerID > -1)
                    {
                        CustomerDTO customerDTO = (new CustomerBL(Utilities.ExecutionContext, customerID, true)).CustomerDTO;
                        GetCustomerDetails(customerDTO, swipedCard.card_id);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                displayMessageLine("Could not fetch customer information. Please retry.");
                customerPhoto = "";
                customerName = "";
                customerPhotoFullPath = "";
            }

            try
            {
                DataTable dtMachine = Utilities.executeDataTable(@"select g.game_id, g.game_profile_id, (select card_id from cards where card_number = @cardNumber and valid_flag ='Y' and isnull(ExpiryDate, getdate()) >= getdate())
                                                                    from machines m, games g
                                                                    where m.game_id = g.game_id 
                                                                    and m.machine_id = @machineId",
                                                                    new SqlParameter("@machineId", machineId),
                                                                    new SqlParameter("@cardnumber", CardNumber));
                if (dtMachine.Rows.Count > 0)
                {
                    int gameId = Convert.ToInt32(dtMachine.Rows[0][0]);
                    int gameProfileId = Convert.ToInt32(dtMachine.Rows[0][1]);
                    int cardId = -1;// Convert.ToInt32(dtMachine.Rows[0][2]);
                    if (dtMachine.Rows[0][2] != DBNull.Value && dtMachine.Rows[0][2] != null)
                    {
                        cardId = Convert.ToInt32(dtMachine.Rows[0][2]);
                    }
                    else
                    {
                        cardId = -1;
                    }

                    try
                    {
                        GameServerEnvironment.GameServerPlayDTO gameServerPlayDTO = new GameServerEnvironment.GameServerPlayDTO(machineId);
                        GamePlayDTO gamePlayDTO = new GamePlayDTO(-1, machineId, cardId, 0, 0, 0, 0, ServerDateTime.Now, string.Empty, 0, string.Empty, 0, 0, 0, 0, -1, 0, -1, ServerDateTime.Now, string.Empty, string.Empty, string.Empty);
                        GamePlayBuildDTO gamePlayBuildDTO = new GamePlayBuildDTO("R", gamePlayDTO, gameServerPlayDTO, string.Empty, string.Empty, string.Empty, string.Empty, null, -1, false);//Only validate Gameplay
                        GameTransactionBL gameTransactionBL = new GameTransactionBL(Utilities.ExecutionContext, machineId);
                        try
                        {
                            gamePlayDTO = gameTransactionBL.PlayGame(cardId, gamePlayBuildDTO);
                            validateFlag = true;
                        }
                        catch (Exception ex)
                        {
                            displayMessageLine(ex.Message);
                            validateFlag = false;
                        }
                        if (entitlementRemaining != -1)
                        {
                            CardGames cg = new CardGames(Utilities);
                            bool ticketAllowed = false;
                            int cardGameId = -1;
                            int balanceGames = cg.getCardGames(cardId, gameId, gameProfileId, ref ticketAllowed, ref cardGameId);
                            if (balanceGames > 0)
                            {
                                entitlementRemaining = balanceGames;
                                //validateFlag = true;
                            }
                            else
                            {
                                entitlementRemaining = 0;
                                validateFlag = false;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        displayMessageLine(MessageUtils.getMessage("Unable to fetch Entitlement details for card ", CardNumber));
                        log.Fatal("Ends-CardSwiped(" + CardNumber + ") as unable to fetch EntitlementDetails due to exception " + ex.Message);//Added for logger function on 08-Mar-2016
                    }
                }
            }
            catch (Exception ex)
            {
                displayMessageLine(MessageUtils.getMessage("Unable to fetch Entitlement details for card ", CardNumber));
                log.Fatal("Ends-CardSwiped(" + CardNumber + ") as unable to fetch Entitlement due to exception " + ex.Message);//Added for logger function on 08-Mar-2016
            }            

            try
            {
                ValidateCardDataContol validUserControl = new ValidateCardDataContol(++panelCount, customerPhotoFullPath, cardNumber, customerName, CardEntitlementExpiryDate.ToString(ParafaitEnv.DATETIME_FORMAT), entitlementRemaining, validateFlag, trxNo);
                flowLayoutPanelSwipedCards.Controls.Add(validUserControl);
                flowLayoutPanelSwipedCards.Controls.SetChildIndex(validUserControl, 0);//show at first/on top new added control

                addControlCount = flowLayoutPanelSwipedCards.Controls.Count;//Set control count after adding each control           
            }
            catch (Exception ex)
            {
                displayMessageLine(MessageUtils.getMessage("Unable to load card details for card ", CardNumber));
                log.Fatal("Ends-CardSwiped(" + CardNumber + ") due to exception " + ex.Message);//Added for logger function on 08-Mar-2016
            }
            log.LogMethodExit("CardSwiped(" + CardNumber + ")");
        }

        private void displayMessageLine(string message)
        {
            log.LogMethodEntry(message );
            lblMessage.Text = message;
            log.LogMethodExit();
        }

        //DeductEntitlements 
        /// <summary>
        /// Deduct Tag entitlements by performing game play.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                btnOK.Enabled = false;
                displayMessageLine("");
                lstCardsWOEntitlement.Clear();//Clear Cards list withoutEntitlements
                foreach (ValidateCardDataContol card in flowLayoutPanelSwipedCards.Controls.OfType<ValidateCardDataContol>())
                {
                    DeductEntitlement(card.cardNumber);

                    if (card.pbValidationTag == "false")
                    {
                        lstCardsWOEntitlement.Add(card.cardNumber);
                    }
                }
                if (lstCardsWOEntitlement.Count > 1)
                {
                    displayMessageLine(MessageUtils.getMessage("No Entitlements available for cards ") + lstCardsWOEntitlement.Aggregate((x, y) => x + ", " + y));
                }
                else
                {
                    if (flowLayoutPanelSwipedCards.Controls.Count > 1)
                    {
                        displayMessageLine(MessageUtils.getMessage("Entitlement Deducted Successfully"));
                    }
                }
                clearControls();//Clear Flowlayoutpanel, transaction number, matching flag in Display
                lblDate.Text = ServerDateTime.Now.ToString(ParafaitEnv.DATE_FORMAT);//Refresh Date            
            }
            catch(Exception ex)
            {
                displayMessageLine(MessageUtils.getMessage("Unable to deduct entitlement"));
                log.Fatal("Ends-btnOK_Click() due to exception " + ex.Message);//Added for logger function on 08-Mar-2016
            }
            finally
            {
                btnOK.Enabled = true;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Fetch customer information and check-in photo as applicable
        /// </summary>
        /// <param name="customerDTO">Customer Object</param>
        /// <param name="cardId">Card Id</param>
        void GetCustomerDetails(CustomerDTO customerDTO, int cardId)
        {
            log.LogMethodEntry(customerDTO);
            customerPhoto = customerDTO.PhotoURL;
            customerName = customerDTO.FirstName + (customerDTO.LastName == "" ? "" : " " + customerDTO.LastName);
            customerPhotoFullPath = Utilities.getParafaitDefaults("IMAGE_DIRECTORY") + "\\" + customerPhoto;


            try
            {
                if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(Utilities.ExecutionContext, "SHOW_CHECKIN_PHOTO_IN_CARD_ENTITLEMENT_SCREEN"))
                {
                    object checkinPhotoPath = Utilities.executeScalar(@"SELECT TOP 1 PhotoFileName 
                                                                            FROM CheckIns
                                                                            WHERE CardId = @CardId
                                                                            AND ISNULL(RTRIM(LTRIM(PhotoFileName)), '') <> ''
                                                                            ORDER BY CheckInTime DESC", new SqlParameter[] { new SqlParameter("@CardId", cardId) });
                    if (checkinPhotoPath != null &&
                        checkinPhotoPath != DBNull.Value &&
                        string.IsNullOrWhiteSpace(Convert.ToString(checkinPhotoPath)) == false)
                    {
                        customerPhotoFullPath = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "CHECKIN_PHOTO_DIRECTORY") + "\\" + Convert.ToString(checkinPhotoPath);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while showing checkin photo", ex);
            }
        }

        void DeductEntitlement(string CardNumber)
        {
            log.LogMethodEntry(CardNumber );
            try
            {
                //int cardGameId = -1;
                DataTable dtMachine = Utilities.executeDataTable(@"select g.game_id, g.game_profile_id, (select card_id from cards where card_number = @cardNumber and valid_flag ='Y' and isnull(ExpiryDate, getdate()) >= getdate())
                                                                        from machines m, games g
                                                                        where m.game_id = g.game_id 
                                                                        and m.machine_id = @machineId",
                                                                           new SqlParameter("@machineId", machineId),
                                                                           new SqlParameter("@cardnumber", CardNumber));
                if (dtMachine.Rows.Count > 0)
                {
                    int gameId = Convert.ToInt32(dtMachine.Rows[0][0]);
                    int gameProfileId = Convert.ToInt32(dtMachine.Rows[0][1]);
                    int cardId = -1;// Convert.ToInt32(dtMachine.Rows[0][2]);
                    if (dtMachine.Rows[0][2] != DBNull.Value && dtMachine.Rows[0][2] != null)
                    {
                        cardId = Convert.ToInt32(dtMachine.Rows[0][2]);
                    }
                    else
                    {
                        cardId = -1;
                    }
                    try
                    {
                        GameServerEnvironment.GameServerPlayDTO gameServerPlayDTO = new GameServerEnvironment.GameServerPlayDTO(machineId);
                        GamePlayDTO gamePlayDTO = new GamePlayDTO(-1, machineId, cardId, 0, 0, 0, 0, ServerDateTime.Now, string.Empty, 0, string.Empty, 0, 0, 0, 0, -1, 0, -1, ServerDateTime.Now, string.Empty, string.Empty, string.Empty);
                        GamePlayBuildDTO gamePlayBuildDTO = new GamePlayBuildDTO("R", gamePlayDTO, gameServerPlayDTO, string.Empty, string.Empty, string.Empty, string.Empty, null, -1, true);//Perform Gameplay
                        GameTransactionBL gameTransactionBL = new GameTransactionBL(Utilities.ExecutionContext, machineId);
                        try
                        {
                            gamePlayDTO = gameTransactionBL.PlayGame(cardId, gamePlayBuildDTO);
                            log.Info("DeductEntitlement(" + CardNumber + ")-Entitlement Deducted Successfully");//Added for logger function on 08-Mar-2016
                            System.IO.Stream str = Properties.Resources.ValidBeep;
                            System.Media.SoundPlayer snd = new System.Media.SoundPlayer(str);
                            snd.Play();
                        }
                        catch (Exception ex)
                        {
                            displayMessageLine(MessageUtils.getMessage("Unable to deduct entitlement: "+ex.Message));
                            log.Fatal("Ends-DeductEntitlement(" + CardNumber + ") as unable to deduct the Entitlements due to exception " + ex.Message);//Added for logger function on 08-Mar-2016
                        }

                        //CardGames cg = new CardGames(Utilities);
                        //bool ticketAllowed = false;
                        //if (cg.getCardGames(cardId, gameId, gameProfileId, ref ticketAllowed, ref cardGameId) > 0)
                        //{
                        //    cg.deductGameCount(cardGameId, null);
                        //}

                        //ServerStatic serverStatic = new ServerStatic(Utilities);
                        //GamePlay.CreateGamePlayRecord(machineId, CardNumber, 0, 0, 0, 0, 0, 0, 0, 0, "N", 0, "Entry/Exit", cardGameId, 0, Utilities.getServerTime(),null, serverStatic);
                    }
                    catch (Exception ex)
                    {
                        displayMessageLine(MessageUtils.getMessage("Unable to deduct entitlement"));
                        log.Fatal("Ends-DeductEntitlement(" + CardNumber + ") as unable to deduct the Entitlements due to exception " + ex.Message);//Added for logger function on 08-Mar-2016
                    }
                }
            }
            catch (Exception ex)
            {
                displayMessageLine(MessageUtils.getMessage("Unable to deduct entitlement"));
                log.Fatal("Ends-DeductEntitlement(" + CardNumber + ") due to exception " + ex.Message);//Added for logger function on 08-Mar-2016
            }
            log.LogMethodExit();
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            displayMessageLine("");
            addControlCount = 0;//reset the add control count
            lstCardsWOEntitlement.Clear();//Clear Cards list withoutEntitlements
            clearControls();//Clear Flowlayoutpanel, transaction number, matching flag in Display
            lblDate.Text = ServerDateTime.Now.ToString(ParafaitEnv.DATE_FORMAT);//Refresh Date
            log.LogMethodExit();
        }

        //SEt Matching
        public void setMatchingFlag(bool validationFlag = false)
        {
            log.LogMethodEntry("validationFlag");
            try
            {
                string exeDir = System.IO.Path.GetDirectoryName(Environment.CommandLine.Replace("\"", ""));
                if (validationFlag)
                {
                    if (File.Exists(exeDir + "\\Resources\\Green.png"))
                    {
                        pbMatchFlag.Image = Image.FromFile(exeDir + "\\Resources\\Green.png");
                    }
                    else
                    {
                        pbMatchFlag.Image = Properties.Resources.Green;
                    }
                }
                else
                {
                    if (File.Exists(exeDir + "\\Resources\\Red.png"))
                    {
                        pbMatchFlag.Image = Image.FromFile(exeDir + "\\Resources\\Red.png");
                    }
                    else
                    {
                        pbMatchFlag.Image = Properties.Resources.Red;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Fatal("Ends-setMatchingFlag(validationFlag) due to exception " + ex.Message);
            }
            log.LogMethodExit();
        }

        //Clear The FlowLayoutPanel User Controls.TransactionNumber ,Matching Flag Icon
        public void clearControls()
        {
            log.LogMethodEntry();
            try
            {
                List<Control> listControls = new List<Control>();
                foreach (Control control in flowLayoutPanelSwipedCards.Controls)
                {
                    listControls.Add(control);
                }
                foreach (Control control in listControls)
                {
                    flowLayoutPanelSwipedCards.Controls.Remove(control);
                    control.Dispose();
                }
                listControls.Clear();
                this.flowLayoutPanelSwipedCards.PerformLayout();//Remove scrollbar 
                lblTransactionNumber.Text = "";//Clear TransactionNumber on screen
                panelCount = 0;
                TrxNumber = 0;//Clear Transaction No
                addControlCount = 0;//reset the add control count
                if (pbMatchFlag.Image != null)
                {
                    pbMatchFlag.Image.Dispose();
                    pbMatchFlag.Image = null;
                }
            }
            catch (Exception ex)
            {                
                displayMessageLine(ex.Message);
                log.Fatal("Ends-clearControls() due to exception " + ex.Message);//Added for logger function on 08-Mar-2016
            }
            log.LogMethodExit();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            DialogResult DR = POSUtils.ParafaitMessageBox(MessageUtils.getMessage(208), "Exit POS", MessageBoxButtons.YesNo);
            if (DR == DialogResult.No)
            {
                log.Info("Ends-btnExit_Click() - as in Exit POS dialog No was clicked");//Added for logger function on 08-Mar-2016
                return;
            }
            else
            {
                Environment.Exit(1);
            }
            log.LogMethodExit();
        }

        //used to Reload frmValidateCard Contents transaction Number, All Matching , Message 
        private void flowLayoutPanelSwipedCards_ControlRemoved(object sender, ControlEventArgs e)
        {
            log.LogMethodEntry();
            //displayMessageLine("");//clear messages
            refreshScreen();
            lstCardsWOEntitlement.Clear();//Clear Cards list withoutEntitlements
            log.LogMethodExit();
        }

        public void refreshScreen()
        {
            log.LogMethodEntry();
            try
            {
                if (flowLayoutPanelSwipedCards.Controls.Count == 1 || flowLayoutPanelSwipedCards.Controls.Count == 0)
                {
                    TrxNumber = 0;
                    addControlCount = 0;//reset the add control count
                    this.flowLayoutPanelSwipedCards.PerformLayout();//Remove scrollbar
                    panelCount = 0;
                    //displayMessageLine("");
                    lblTransactionNumber.Text = "";//Clear TransactionNumber on screen
                    if (pbMatchFlag.Image != null)
                    {
                        pbMatchFlag.Image.Dispose();
                        pbMatchFlag.Image = null;
                    }
                    log.LogMethodExit();
                    return;
                }
                else
                {
                    bool isMatched = true;
                    int closeControlCount = flowLayoutPanelSwipedCards.Controls.Count - 1;//Count of Controls after remove
                    //For Showing Transaction Number Part
                    foreach (ValidateCardDataContol card in flowLayoutPanelSwipedCards.Controls.OfType<ValidateCardDataContol>())
                    {
                        if (card.transactionNumber != null)
                        {
                            if (flowLayoutPanelSwipedCards.Controls.Count == 2)//reset transactionNumber with first card from the panel
                            {
                                TrxNumber = Convert.ToInt32(card.transactionNumber);
                                lblTransactionNumber.Text = card.transactionNumber;
                            }
                            else
                            {
                                if (addControlCount != closeControlCount) //initial load set last panel transaction Number as Current TransactionNumber
                                {
                                    if (Convert.ToInt32(card.transactionNumber) != TrxNumber)
                                    {
                                        TrxNumber = Convert.ToInt32(card.transactionNumber);
                                        lblTransactionNumber.Text = card.transactionNumber;
                                    } 
                                }
                            }
                        }
                    }
                    log.Debug("Showing all Matching Flag Part");
                    //For Showing all Matching Flag Part 
                    foreach (ValidateCardDataContol card in flowLayoutPanelSwipedCards.Controls.OfType<ValidateCardDataContol>())
                    {
                        if (Convert.ToInt32(card.transactionNumber) != TrxNumber)
                        {
                            isMatched = false;
                            break;
                        }
                    }
                    if (flowLayoutPanelSwipedCards.Controls.Count > 1)
                    {
                        setMatchingFlag(isMatched);//sets the All Matching Icon
                        addControlCount = flowLayoutPanelSwipedCards.Controls.Count;//reassign the add control count with close count
                    }
                }
            }
            catch (Exception ex)
            {
                displayMessageLine(MessageUtils.getMessage("Unable to refresh Screen"));
                log.Fatal("Ends-refreshScreen() due to exception " + ex.Message);//Added for logger function on 08-Mar-2016
            }
            log.LogMethodExit();
        }
    }
}
