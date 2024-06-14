/********************************************************************************************
 * Project Name - SystemOptions BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        14-Dec-2018      Raghuveera     Modified to get enrypted key and password values 
 *2.80.0      20-Aug-2019      Girish Kundar  Modified :  Added logger methods and Removed unused namespace's
 ********************************************************************************************/
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;

namespace Parafait_POS
{
    public partial class MasterCardManagement : Form
    {
       private Utilities Utilities = POSStatic.Utilities;
       private MessageUtils MessageUtils = POSStatic.MessageUtils;
       private TaskProcs TaskProcs = POSStatic.TaskProcs;
       private ParafaitEnv ParafaitEnv = POSStatic.ParafaitEnv;

        DeviceClass mifare;
        private string cardNumber = "";
        private const int MAXBLOCKS_MIFARE_S70 = 255;
        private const int MASTER_BLOCK = 4;
        private const int DATA_START_BLOCK = 8;

        private const int PLAYS_SAVED_COUNT_POS = 3;

        private const string WARNING = "WARNING";
        private const string ERROR = "ERROR";
        private const string MESSAGE = "MESSAGE";

        private const string DUPLICATE_IN_CARD = "Duplicate in Master card";
        private const string DUPLICATE_IN_SYSTEM = "Duplicate in System";
        private const string READY = "Ready to Upload";
        private const string COMPLETE = "Completed";
        private const string ERROR_UPLOADING = "Error Uploading";
        private const string INVALID_DATA = "Invalid Value";
        private const string NO_DATA_IN_SYSTEM = "Machine Id/ Site code does not exist";

        public int deviceAddress;
        private byte[] masterAuthKey;
        public byte[] basicAuthKey;

        public class MifareGamePlay
        {
            public int MachineId;
            public string MachineAddress;
            public int siteId;
            public int playsSaved;
            public string playCardNumber;
            public double startingBalance;
            public double endingBalance;
            public string status;
            public string machineName;
            public string childSiteName;
        }

        MifareGamePlay[] mifareGamePlay;
        string message = "";
        private int[] MIFARE_S70_AUTHBLOCKS = { 7, 11, 15, 19, 23, 27, 31, 35, 39, 43, 47, 51, 55, 59, 63, 67, 71,
                                                        75, 79, 83, 87, 91, 95, 99, 103, 107, 111, 115, 119, 123, 127,
                                                        143, 159, 175, 191, 207, 223, 239, 255};
        private byte[] blankData = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                     0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                     0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};

        //Begin: Modified Added for logger function on 08-Mar-2016
       private static readonly  Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //End: Modified Added for logger function on 08-Mar-2016

        public MasterCardManagement(string cardNumberParam, DeviceClass mifareReaderParam)
        { 
            //Begin: Added to Configure the logger root LogLevel using App.config on 08-March-2016           
            //log = ParafaitUtils.Logger.setLogFileAppenderName(log);
            Logger.setRootLogLevel(log);
            //End: Added to Configure the logger root LogLevel using App.config on 08-March-2016

            log.LogMethodEntry(cardNumberParam, mifareReaderParam);
            Utilities.setLanguage();
            InitializeComponent();
            dgvMasterCardDetails.Font = new Font("Arial", 8);

            mifare = mifareReaderParam;
            cardNumber = cardNumberParam;
            formAuthKeys();

            init_form();
            log.LogMethodExit();
        }

        public void formAuthKeys()
        {
            log.LogMethodEntry();
            int customerKey = Utilities.MifareCustomerKey;
                       
            basicAuthKey = new byte[6];
            for (int i = 0; i < 6; i++)
                basicAuthKey[i] = 0xff;

            string key = Encryption.GetParafaitKeys("MasterCard");//"MASTE";
            masterAuthKey = new byte[6];
            for (int i = 0; i < 5; i++)
                masterAuthKey[i] = Convert.ToByte(key[i]);
            masterAuthKey[5] = Convert.ToByte(customerKey);
            log.LogMethodExit();
        }
        private void CardSwiped(string CardNumber)
        {
            log.LogMethodEntry(CardNumber);
            cardNumber = CardNumber;
            init_form();
            log.LogMethodExit();
        }

        private void init_form()
        {
            log.LogMethodEntry();
            txtCardNumber.Text = cardNumber;
            displayMessageLine("Card Read", MESSAGE);
            bool response = false;
            
            ButtonsState(Clear, false);

            //if (!mifare.s70Card)
            //{
            //    displayMessageLine(MessageUtils.getMessage(94), WARNING);
            //    ButtonsState(btnOK, false);
            //    return;
            //}

            response = false;
            try
            {
                mifare.Authenticate(MASTER_BLOCK, masterAuthKey);
                response = true;
            }
            catch
            {
                log.Fatal("init_form() due to exception in mifare.Authenticate");//Added for logger function on 08-Mar-2016
            }

            if (response)
            {
                btnOK.Text = "Update";
                btnOK.Click += new EventHandler(Update_Click);
                ButtonsState(btnOK, false);
                displayMessageLine(MessageUtils.getMessage(95), MESSAGE);
                log.Warn("init_form() - Current Card is a Master Card");//Added for logger function on 08-Mar-2016
            }
            else
            {
                response = false;
                try
                {
                    mifare.Authenticate(MASTER_BLOCK, basicAuthKey);
                    response = true;
                }
                catch
                {
                    log.Fatal("init_form() due to exception in mifare.Authenticate");//Added for logger function on 08-Mar-2016
                }

                if (response)
                {
                    btnOK.Text = "Issue Card";
                    btnOK.Click += new EventHandler(Issue_Click);
                    ButtonsState(btnOK, true);
                    displayMessageLine(MessageUtils.getMessage(96), MESSAGE);
                    log.Warn("init_form() - New Card, Please Issue");//Added for logger function on 08-Mar-2016
                }
                else
                {
                    ButtonsState(btnOK, false);
                    displayMessageLine(MessageUtils.getMessage(56), ERROR);
                    log.Error("init_form() - Invalid card, please check");//Added for logger function on 08-Mar-2016
                }
            }
            log.LogMethodExit();
        }

        private void Issue_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            bool returnSuccess = false;
            updateProgressBar(-1);

            SqlCommand cmd = Utilities.getCommand();
            SqlTransaction IssueTransaction = cmd.Connection.BeginTransaction();
            cmd.Transaction = IssueTransaction;

            try
            {
                displayMessageLine("Starting...", MESSAGE);
                log.Info("Issue_Click() - Starting...");//Added for logger function on 08-Mar-2016
                updateProgressBar(1);

                makeCardTechnician(cmd, cardNumber);
                
                for (int i = 0; i < MIFARE_S70_AUTHBLOCKS.Length; i++)
                {
                    returnSuccess = mifare.change_authentication_key(MIFARE_S70_AUTHBLOCKS[i], basicAuthKey, masterAuthKey, ref message);
                    if (!returnSuccess)
                    {
                        displayMessageLine(message, ERROR);
                        log.Error("Issue_Click() as mifare.change_authentication_key has error "+message);//Added for logger function on 08-Mar-2016
                        break;
                    }
                    updateProgressBar(3);
                }
                displayMessageLine(MessageUtils.getMessage(98), MESSAGE);
                log.Info("Issue_Click() - Please wait, Issuing a master card in progress...");//Added for logger function on 08-Mar-2016
                this.Refresh();

                if (returnSuccess)
                {
                    for (int i = MASTER_BLOCK; i < MAXBLOCKS_MIFARE_S70; i++)
                    {
                        if (!MIFARE_S70_AUTHBLOCKS.Contains(i))
                        {
                            returnSuccess = mifare.write_data(i, 3, masterAuthKey, blankData, ref message);
                            if (!returnSuccess)
                            {
                                displayMessageLine(message, ERROR);
                                log.Error("Ends-Issue_Click() as mifare.write_data has error " + message);//Added for logger function on 08-Mar-2016
                                break;
                            }
                            i += 2;
                        }
                        updateProgressBar(3);
                    }
                }

                if (returnSuccess)
                {
                    IssueTransaction.Commit();
                    displayMessageLine(MessageUtils.getMessage(99), MESSAGE);
                    log.Info("Issue_Click() - Successfully Issued Master Card");//Added for logger function on 08-Mar-2016
                    ButtonsState(btnOK, false);
                }
                else
                {
                    IssueTransaction.Rollback();
                    displayMessageLine(MessageUtils.getMessage(240, message), ERROR);
                    log.Error("Issue_Click() - Issuing card error " + message);//Added for logger function on 08-Mar-2016
                }
                updateProgressBar(MAX_PROGRESS_VAL);
            }
            catch (Exception ex)
            {
                IssueTransaction.Rollback();
                displayMessageLine(ex.Message, ERROR);
                updateProgressBar(MAX_PROGRESS_VAL); 
                log.Fatal("Ends-Issue_Click() due to exception " + ex.Message);//Added for logger function on 08-Mar-2016
            }
            log.LogMethodExit();
        }

        private void makeCardTechnician(SqlCommand SQLCmd, string cardNumber)
        {
            log.LogMethodEntry(SQLCmd, cardNumber);
            if (cardNumber != String.Empty)
            {
                SQLCmd.CommandText = @"Select * from cards where card_number = @card_number and technician_card = 'Y'";
                SQLCmd.Parameters.AddWithValue("@card_number", cardNumber);
                DataTable dt = new DataTable();
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter(SQLCmd);
                    da.Fill(dt);
                }
                catch (Exception ex)
                {
                    log.Fatal("Ends-makeCardTechnician(SQLCmd," + cardNumber + ") due to exception " + ex.Message);//Added for logger function on 08-Mar-2016
                    throw new ApplicationException(ex.Message);
                }
                if (dt.Rows.Count > 0)
                {
                    SQLCmd.CommandText = @"Update cards set valid_flag = @valid_flag, last_update_time = getdate(), 
                                            LastUpdatedBy = @LastUpdatedBy where card_number = @card_number and technician_card = 'Y'";
                }
                else if (dt.Rows.Count == 0)
                {
                    SQLCmd.CommandText = @"Insert into cards (card_number, technician_card, valid_flag, notes, issue_date, LastUpdatedBy, last_update_time) 
                                             values (@card_number, @technician_card, @valid_flag, @notes, getdate(), @LastUpdatedBy, getdate())";
                }
                SQLCmd.Parameters.AddWithValue("@technician_card", 'Y');
                SQLCmd.Parameters.AddWithValue("@valid_flag", "Y");
                SQLCmd.Parameters.AddWithValue("@LastUpdatedBy", ParafaitEnv.LoginID);
                SQLCmd.Parameters.AddWithValue("@notes", "Mifare Master Card");
                try
                {
                    SQLCmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    log.Fatal("Ends-makeCardTechnician(SQLCmd," + cardNumber + ") due to exception " + ex.Message);//Added for logger function on 08-Mar-2016
                    throw new ApplicationException(ex.Message);
                }
            }
            log.LogMethodExit();
        }
        private void inValidateMasterCard(SqlCommand SQLCmd, string cardNumber)
        {
            log.LogMethodEntry(cardNumber );
            if (cardNumber != String.Empty)
            {
                SQLCmd.CommandText = @"Update cards set valid_flag = @valid_flag, last_update_time = getdate(), LastUpdatedBy = @LastUpdatedBy
                                    where card_number = @card_number";

                SQLCmd.Parameters.AddWithValue("@card_number", cardNumber);

                SQLCmd.Parameters.AddWithValue("@valid_flag", "N");
                SQLCmd.Parameters.AddWithValue("@LastUpdatedBy", ParafaitEnv.LoginID);
                try
                {
                    SQLCmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    log.Fatal("Ends-inValidateMasterCard(SQLCmd, " + cardNumber + ") due to exception " + ex.Message);//Added for logger function on 08-Mar-2016
                    throw new ApplicationException(ex.Message);
                }
            }
            log.LogMethodExit();
        }

        private void btnGetDetails_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            dgvMasterCardDetails.Rows.Clear();
            mifareGamePlay = getPlayDetails(txtCardNumber.Text);
            displayCardDetails();
            ButtonsState(btnOK, true);
            log.LogMethodExit();
        }
        private MifareGamePlay[] getPlayDetails(string cardNumber)
        {
            log.LogMethodEntry(cardNumber );
            int i = 0;
            int blockNumber;
            bool returnSuccess = false;
            bool readFailed = false;
            byte[] dataBuffer = new byte[48];
            int totalNumberOfGames = 0;
            int gamesCopied = 0;
            updateProgressBar(-1);

            try
            {
                returnSuccess = mifare.read_data(MASTER_BLOCK, 1, masterAuthKey, ref dataBuffer, ref message);
                log.LogVariableState("returnSuccess" , returnSuccess);
                if (!returnSuccess)
                {
                    displayMessageLine(MessageUtils.getMessage(100), MESSAGE);
                    log.Warn("getPlayDetails(" + cardNumber + ") - Reading data from the Master block failed");//Added for logger function on 08-Mar-2016
                    return null;
                }
                totalNumberOfGames = dataBuffer[PLAYS_SAVED_COUNT_POS];
                if (totalNumberOfGames <= 0)
                {
                    displayMessageLine(MessageUtils.getMessage(101), MESSAGE);
                    log.Warn("getPlayDetails(" + cardNumber + ") - There are no records stored on the card");//Added for logger function on 08-Mar-2016
                    return null;
                }
                MifareGamePlay[] gameDetails = new MifareGamePlay[totalNumberOfGames];
                displayMessageLine(MessageUtils.getMessage(102), MESSAGE);
                log.Info("getPlayDetails(" + cardNumber + ") - Reading data from Card; please do not remove the card ");//Added for logger function on 08-Mar-2016
                updateProgressBar(1);
                for (blockNumber = DATA_START_BLOCK; ((blockNumber < MAXBLOCKS_MIFARE_S70) && (gamesCopied < totalNumberOfGames)); blockNumber++)
                {
                    if (!MIFARE_S70_AUTHBLOCKS.Contains(blockNumber))
                    {
                        returnSuccess = mifare.read_data(blockNumber, 3, masterAuthKey, ref dataBuffer, ref message);
                        if (returnSuccess)
                        {
                            MifareGamePlay thisGameDetails = new MifareGamePlay();
                            try
                            {
                                thisGameDetails.siteId = Convert.ToInt32(getValue(dataBuffer, 6, 2));
                            }
                            catch
                            {
                                thisGameDetails.status = INVALID_DATA;
                            }
                            try
                            {
                                thisGameDetails.MachineId = Convert.ToInt32(getValue(dataBuffer, 8, 2));
                            }
                            catch
                            {
                                thisGameDetails.status = INVALID_DATA;
                            }
                            try
                            {
                                thisGameDetails.playCardNumber = String.Format("{0:X2}{1:X2}{2:X2}{3:X2}", dataBuffer[11], dataBuffer[12], dataBuffer[13], dataBuffer[14]);
                            }
                            catch
                            {
                                thisGameDetails.status = INVALID_DATA;
                            }
                            try
                            {
                                thisGameDetails.playsSaved = Convert.ToInt32(getValue(dataBuffer, 16, 6));
                            }
                            catch
                            {
                                thisGameDetails.status = INVALID_DATA;
                            }
                            try
                            {
                                thisGameDetails.startingBalance = Convert.ToDouble(getValue(dataBuffer, 23, 6)) / 100;
                            }
                            catch
                            {
                                thisGameDetails.status = INVALID_DATA;
                            }
                            try
                            {
                                thisGameDetails.endingBalance = Convert.ToDouble(getValue(dataBuffer, 30, 6)) / 100;
                            }
                            catch
                            {
                                thisGameDetails.status = INVALID_DATA;
                            }
                            if(thisGameDetails.status != INVALID_DATA)
                                thisGameDetails.status = READY;
                            gameDetails[i++] = thisGameDetails;
                            blockNumber += 2;
                            gamesCopied++;
                        }
                        else
                        {
                            displayMessageLine(MessageUtils.getMessage(103, blockNumber), WARNING);
                            log.Warn("getPlayDetails(" + cardNumber + ") - Read operation complete, read operation stopped at " + blockNumber.ToString());//Added for logger function on 08-Mar-2016
                            readFailed = true;
                            break;
                        }
                        updateProgressBar(3);
                    }
                }
                if (readFailed)
                {
                    displayMessageLine(MessageUtils.getMessage(104), WARNING);
                    log.Warn("getPlayDetails(" + cardNumber + ") - Read operation failed ");//Added for logger function on 08-Mar-2016
                    return null;
                }
                displayMessageLine(MessageUtils.getMessage(105, i), MESSAGE);
                log.Info("getPlayDetails(" + cardNumber + ") - Read operation complete," + i.ToString() + " records found.");//Added for logger function on 08-Mar-2016
                this.Refresh();
                if (blockNumber == MAXBLOCKS_MIFARE_S70 || gamesCopied == totalNumberOfGames)
                {
                    for (int j = 0; j < totalNumberOfGames; j++)
                    {
                        if (gameDetails[j].status == DUPLICATE_IN_CARD)
                            continue;
                        for (int k = j + 1; k < totalNumberOfGames; k++)
                        {
                            if (gameDetails[k].status != DUPLICATE_IN_CARD)
                            {
                                if (gameDetails[j].MachineId == gameDetails[k].MachineId &&
                                    gameDetails[j].siteId == gameDetails[k].siteId &&
                                    gameDetails[j].playsSaved == gameDetails[k].playsSaved &&
                                    gameDetails[j].startingBalance == gameDetails[k].startingBalance &&
                                    gameDetails[j].endingBalance == gameDetails[k].endingBalance)
                                {
                                    gameDetails[k].status = DUPLICATE_IN_CARD;
                                }
                            }
                        }
                    }
                }

                SqlCommand cmd = Utilities.getCommand();
                for (int index = 0; index < totalNumberOfGames; index++)
                {
                    cmd.CommandText = @"Select * from gameplay where card_id = (select card_id from cards where card_number = @cardNumber) and
                                        machine_id = (select machine_id from machines where machine_address = @machineAddress) and notes = @uniqueId";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@cardNumber", "FFFFFFFFFF");
                    gameDetails[index].MachineAddress = gameDetails[index].siteId.ToString().PadLeft(2, '0') + gameDetails[index].MachineId.ToString().PadLeft(2, '0');
                    cmd.Parameters.AddWithValue("@machineAddress", gameDetails[index].MachineAddress);
                    // uniqueIdentifier = CARDNUMBER-SITEID-PLAYSSAVED-STARTBALANCE-ENDBALANCE
                    string uniqueIdentifier = gameDetails[index].playCardNumber + "-" + gameDetails[index].siteId + "-" + gameDetails[index].playsSaved + "-" + gameDetails[index].startingBalance + "-" + gameDetails[index].endingBalance;
                    cmd.Parameters.AddWithValue("@uniqueId", uniqueIdentifier);
                    DataTable dt = new DataTable();
                    try
                    {
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        if (dt.Rows.Count >= 1)
                        {
                            gameDetails[index].status = DUPLICATE_IN_SYSTEM;
                        }
                    }
                    catch
                    {
                        log.Fatal("Ends-getPlayDetails(" + cardNumber + ") due to exception in uniqueId ");//Added for logger function on 08-Mar-2016
                    }
                    cmd.CommandText = @"Select machine_name, Description + '-' + Lookupvalue as ChildSiteCode 
                                            from machines, LookupView where machine_address = @machineAddress and 
                                            lookupvalue = @childSiteId";
                    cmd.Parameters.AddWithValue("@childSiteId", gameDetails[index].siteId.ToString().PadLeft(2, '0'));
                    try
                    {
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                        dt.Clear();
                        dataAdapter.Fill(dt);
                        if (dt.Rows.Count != 0)
                        {
                            gameDetails[index].machineName = dt.Rows[0]["machine_name"].ToString();
                            gameDetails[index].childSiteName = dt.Rows[0]["ChildSiteCode"].ToString();
                        }
                        else
                            gameDetails[index].status = NO_DATA_IN_SYSTEM;
                    }
                    catch
                    {
                        log.Fatal("Ends-getPlayDetails(" + cardNumber + ") due to exception in childSiteId ");//Added for logger function on 08-Mar-2016
                    }
                }

                updateProgressBar(MAX_PROGRESS_VAL);
                log.LogMethodExit(gameDetails);
                return gameDetails;
            }
            catch(Exception ex)
            {
                displayMessageLine(ex.Message, ERROR);
                log.Fatal("Ends-getPlayDetails(" + cardNumber + ") due to exception " + ex.Message);//Added for logger function on 08-Mar-2016
                return null;
            }
        }
        private string getValue(byte[] dataArray, int offset, int size)
        {
            log.LogMethodEntry();
            string value = "";
            for (int i = size - 1; i >= 0; i--)
            {
                value = String.Concat(value, (dataArray[offset] - 48).ToString());
                offset++;
            }
            return value;
        }
        private void displayCardDetails()
        {
            log.Debug("Starts-displayCardDetails()");//Added for logger function on 08-Mar-2016
            dgvMasterCardDetails.Rows.Clear();
            try
            {
                if(mifareGamePlay == null)
                {
                    log.Info("Ends-displayCardDetails() as mifareGamePlay == null");//Added for logger function on 08-Mar-2016
                    return;
                }
                for (int i = 0; i < mifareGamePlay.Length; i++)
                {
                    dgvMasterCardDetails.Rows.Add();
                    dgvMasterCardDetails.Rows[i].Cells["dcSiteId"].Value = mifareGamePlay[i].siteId;
                    dgvMasterCardDetails.Rows[i].Cells["dcMachineAddress"].Value = mifareGamePlay[i].MachineAddress;
                    dgvMasterCardDetails.Rows[i].Cells["dcCardNumber"].Value = mifareGamePlay[i].playCardNumber;
                    dgvMasterCardDetails.Rows[i].Cells["dcPlaysSaved"].Value = mifareGamePlay[i].playsSaved;
                    dgvMasterCardDetails.Rows[i].Cells["dcStatus"].Value = mifareGamePlay[i].status;
                    dgvMasterCardDetails.Rows[i].Cells["dcUpload"].Value = false;
                    
                    if (mifareGamePlay[i].machineName != null)
                        dgvMasterCardDetails.Rows[i].Cells["dcMachineAddress"].Value = mifareGamePlay[i].machineName;

                    if(mifareGamePlay[i].childSiteName != null)
                        dgvMasterCardDetails.Rows[i].Cells["dcSiteId"].Value = mifareGamePlay[i].childSiteName;
                    
                    if (mifareGamePlay[i].status == READY)
                    {
                        dgvMasterCardDetails.Rows[i].Cells["dcUpload"].Value = true;
                        dgvMasterCardDetails.Rows[i].Cells["dcMachineAddress"].Value = mifareGamePlay[i].machineName;
                        dgvMasterCardDetails.Rows[i].Cells["dcSiteId"].Value = mifareGamePlay[i].childSiteName;
                    }
                    if (mifareGamePlay[i].status == COMPLETE)
                    {
                        dgvMasterCardDetails.Rows[i].Cells["dcUpload"].Value = true;
                        dgvMasterCardDetails.Rows[i].ReadOnly = true;
                        dgvMasterCardDetails.Rows[i].Cells["dcSiteId"].Style.ForeColor = Color.Green;
                        dgvMasterCardDetails.Rows[i].Cells["dcMachineAddress"].Style.ForeColor = Color.Green;
                        dgvMasterCardDetails.Rows[i].Cells["dcCardNumber"].Style.ForeColor = Color.Green;
                        dgvMasterCardDetails.Rows[i].Cells["dcPlaysSaved"].Style.ForeColor = Color.Green;
                        dgvMasterCardDetails.Rows[i].Cells["dcStatus"].Style.ForeColor = Color.Green;
                        dgvMasterCardDetails.Rows[i].Cells["dcUpload"].Style.ForeColor = Color.Green;
                    }
                }
            }
            catch
            {
                log.Info("Ends-displayCardDetails() due to exception in dgvMasterCardDetails");//Added for logger function on 08-Mar-2016
            }
            log.Debug("Ends-displayCardDetails()");//Added for logger function on 08-Mar-2016
        }

        private void Update_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-Update_Click()");//Added for logger function on 08-Mar-2016
            int response;
            updateProgressBar(-1);
            try
            {
                if (mifareGamePlay == null)
                {
                    displayMessageLine(MessageUtils.getMessage(106), WARNING);
                    log.Info("Ends-Update_Click() as There are no data to be updated");//Added for logger function on 08-Mar-2016
                    return;
                }
                int uploadedDataCount = 0;
                for (int i = 0; i < mifareGamePlay.Length; i++)
                {
                    if ((bool)dgvMasterCardDetails.Rows[i].Cells["dcUpload"].Value == true && mifareGamePlay[i].status != COMPLETE)
                    {
                        response = createGamePlay(mifareGamePlay[i]);
                        if (response > 0)
                        {
                            mifareGamePlay[i].status = COMPLETE;
                            uploadedDataCount++;
                        }
                        else
                            mifareGamePlay[i].status = ERROR_UPLOADING;
                        updateProgressBar(3);
                    }
                }
                displayMessageLine(uploadedDataCount + " gameplay records added to the system.", MESSAGE);
                log.Info("Update_Click()- " + uploadedDataCount + " gameplay records added to the system");//Added for logger function on 08-Mar-2016
                updateProgressBar(MAX_PROGRESS_VAL);
                ButtonsState(Clear, true);
                displayCardDetails();
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message, ERROR);
                log.Debug("Ends-Update_Click() due to exception " + ex.Message);//Added for logger function on 08-Mar-2016
            }
            log.Debug("Ends-Update_Click()");//Added for logger function on 08-Mar-2016
        }
        private int createGamePlay(MifareGamePlay gamePlay)
        {
            log.Debug("Starts-createGamePlay(" + gamePlay + ")");//Added for logger function on 08-Mar-2016
            SqlCommand insertCMD = Utilities.getCommand();
            int response = 0;
            try
            {
                insertCMD.CommandText = @"Insert into gameplay (machine_id, card_id, credits, courtesy, bonus, time, notes, play_date)
                                      (select top 1 machine_id, card_id, @startingBalance - @endingBalance, 0, 0, 0, @uniqueId, @playDate 
                                        from cards c, machines m where card_number = @cardNumber
                                        and valid_flag = 'Y' 
                                        and m.machine_address = @machineAddress
                                        ) ";
                insertCMD.Parameters.AddWithValue("@machineAddress", gamePlay.MachineAddress);
                insertCMD.Parameters.AddWithValue("@cardNumber", "FFFFFFFFFF");
                insertCMD.Parameters.AddWithValue("@startingBalance", gamePlay.startingBalance);
                insertCMD.Parameters.AddWithValue("@endingBalance", gamePlay.endingBalance);
                string uniqueIdentifier = gamePlay.playCardNumber + "-" + gamePlay.siteId.ToString() + "-" + gamePlay.playsSaved + "-" + gamePlay.startingBalance + "-" + gamePlay.endingBalance;
                insertCMD.Parameters.AddWithValue("@playDate", dateTimeGamePlay.Value);
                insertCMD.Parameters.AddWithValue("@uniqueId", uniqueIdentifier);
                response = insertCMD.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message, ERROR);
                log.Fatal("Ends-createGamePlay(" + gamePlay + ") due to exception " + ex.Message);//Added for logger function on 08-Mar-2016
            }

            log.Debug("Ends-createGamePlay(" + gamePlay + ")");//Added for logger function on 08-Mar-2016
            return response;
        }
        
        private void Clear_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-Clear_Click()");//Added for logger function on 08-Mar-2016
            bool returnSuccess = false;
            progressBar.Value = 0;

            dgvMasterCardDetails.Rows.Clear();
            mifareGamePlay = null;

            byte[] localAuthKey = new byte[6];

            localAuthKey = masterAuthKey;

            SqlCommand cmd = Utilities.getCommand();
            SqlTransaction ClearTransaction = cmd.Connection.BeginTransaction();
            cmd.Transaction = ClearTransaction;
            try
            {
                displayMessageLine("Starting...", MESSAGE);
                log.Info("Clear_Click() - Starting...");//Added for logger function on 08-Mar-2016
                returnSuccess = true;
                if (secret == "SEMNOX")
                {
                    updateProgressBar(1);

                    inValidateMasterCard(cmd,cardNumber);

                    for (int i = 0; i < MIFARE_S70_AUTHBLOCKS.Length; i++)
                    {
                        returnSuccess = mifare.change_authentication_key(MIFARE_S70_AUTHBLOCKS[i], masterAuthKey, basicAuthKey, ref message);
                        if (!returnSuccess)
                        {
                            message = "Changing key failed";
                            break;
                        }
                        updateProgressBar(3);
                    }
                    localAuthKey = basicAuthKey;
                    updateProgressBar(MAX_PROGRESS_VAL);
                }
                displayMessageLine(MessageUtils.getMessage(107), MESSAGE);
                log.Info("Clear_Click() - Please wait, Clearing  master card data...");//Added for logger function on 08-Mar-2016
                updateProgressBar(-1);
                
                if (returnSuccess)
                {
                    for (int i = MASTER_BLOCK; i < MAXBLOCKS_MIFARE_S70; i++)
                    {
                        if (!MIFARE_S70_AUTHBLOCKS.Contains(i))
                        {
                            returnSuccess = mifare.write_data(i, 3, localAuthKey, blankData, ref message);
                            if (!returnSuccess)
                            {
                                message = "Clearing the data failed";
                                break;
                            }
                            i += 2;
                        }
                        updateProgressBar(3);
                    }
                }
                updateProgressBar(MAX_PROGRESS_VAL);
                if (returnSuccess)
                {
                    ClearTransaction.Commit();
                    displayMessageLine(MessageUtils.getMessage(108), MESSAGE);
                    log.Info("Clear_Click() - Successfully cleared Master Card");//Added for logger function on 08-Mar-2016
                }
                else
                {
                    ClearTransaction.Rollback();
                    displayMessageLine(MessageUtils.getMessage(109) + ":" + message, ERROR);
                    log.Error("Clear_Click() - Clearing the master card failed ");//Added for logger function on 08-Mar-2016 
                }
                
            }
            catch (Exception ex)
            {
                ClearTransaction.Rollback();
                displayMessageLine(ex.Message, ERROR);
                log.Fatal("Ends-Clear_Click() due to exception " + ex.Message);//Added for logger function on 08-Mar-2016
            }
            log.Debug("Ends-Clear_Click()");//Added for logger function on 08-Mar-2016
        }
        
        string secret = "";
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if ((keyData & Keys.Control) < 0)
                return true;

            if ((keyData & Keys.KeyCode) == Keys.C)
                secret = "C";
            else
                if (((keyData & Keys.KeyCode) == Keys.L) && secret == "C")
                    secret += "L";
            else 
                if (((keyData & Keys.KeyCode) == Keys.R) && secret == "CL")
                {
                    secret += "R";
                    ButtonsState(Clear, true);
                    secret = "";
                }
            else
                if ((keyData & Keys.KeyCode) == Keys.S)
                    secret = "S";
                else
                    if (((keyData & Keys.KeyCode) == Keys.E) && secret == "S")
                        secret += "E";
                    else
                        if (((keyData & Keys.KeyCode) == Keys.M) && secret == "SE")
                            secret += "M";
                    else
                        if (((keyData & Keys.KeyCode) == Keys.N) && secret == "SEM")
                            secret += "N";
                    else
                        if (((keyData & Keys.KeyCode) == Keys.O) && secret == "SEMN")
                                secret += "O";
                        else
                            if (((keyData & Keys.KeyCode) == Keys.X) && secret == "SEMNO")
                            {
                                secret += "X";
                                ButtonsState(Clear, true);
                                return true;
                            }
                            else
                                secret = "";
            return true;
        }

        private void MasterCardManagement_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnCancel_Click()");//Added for logger function on 08-Mar-2016
            this.Close();
            log.Debug("Ends-btnCancel_Click()");//Added for logger function on 08-Mar-2016
        }

        private void ButtonsState(object sender, bool Enabled)
        {
            log.Debug("Starts-ButtonsState()");//Added for logger function on 08-Mar-2016
            Button paramButton = (Button)sender;
            paramButton.Enabled = Enabled;
            log.Debug("Ends-ButtonsState()");//Added for logger function on 08-Mar-2016
        }
        int MAX_PROGRESS_VAL = 100;
        private void updateProgressBar(int value)
        {
            if (value == -1)
                progressBar.Value = 0;
            else if (value == MAX_PROGRESS_VAL)
                progressBar.Value = value;
            else if (progressBar.Value < 90)
                progressBar.Value += value;
            this.Refresh();
        }
        private void displayMessageLine(string message, string msgType)
        {
            log.Debug("Starts-displayMessageLine(" + message + "," + msgType + ")");//Added for logger function on 08-Mar-2016
            switch (msgType)
            {
                case "WARNING": txtLog.BackColor = Color.Yellow; txtLog.ForeColor = Color.Black; break;
                case "ERROR": txtLog.BackColor = Color.Red; txtLog.ForeColor = Color.White; break;
                case "MESSAGE": txtLog.BackColor = this.BackColor; txtLog.ForeColor = Color.Black; break;
                default: txtLog.ForeColor = Color.Black; break;
            }
            txtLog.Text = message;
            log.Debug("Ends-displayMessageLine(" + message + "," + msgType + ")");//Added for logger function on 08-Mar-2016
        }

        private void MasterCardManagement_Load(object sender, EventArgs e)
        {
            log.Debug("Starts-MasterCardManagement_Load()");//Added for logger function on 08-Mar-2016
            Utilities.setupDataGridProperties(ref dgvMasterCardDetails);
            dgvMasterCardDetails.BackgroundColor = this.BackColor;
            dgvMasterCardDetails.BorderStyle = BorderStyle.FixedSingle;
            log.Debug("Ends-MasterCardManagement_Load()");//Added for logger function on 08-Mar-2016
        }
    }
}
