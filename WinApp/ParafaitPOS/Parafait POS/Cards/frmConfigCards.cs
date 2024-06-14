/********************************************************************************************
 * Project Name - frmConfigCards
 * Description  - frmConfigCards UI
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        14-Dec-2018      Raghuveera     Modified to get enrypted key and password values
 *2.70        1-Jul-2019      Lakshminarayana     Modified to add support for ULC cards 
 *2.80.0      20-Aug-2019     Girish Kundar       Modified :  Added logger methods and Removed unused namespace's
 ********************************************************************************************/
using System;
using System.Text;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;
using Logger = Semnox.Core.Utilities.Logger;

namespace Parafait_POS.Cards
{
    public partial class frmConfigCards : Form
    {
        private readonly TagNumberParser tagNumberParser;
        Card Card = null;
        //Begin: Modified Added for logger function on 08-Mar-2016
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //End: Modified Added for logger function on 08-Mar-2016
        private readonly UlcKeyStore ulcKeyStore;
        public frmConfigCards()
        {
            //Begin: Added to Configure the logger root LogLevel using App.config on 08-March-2016           
            //log = ParafaitUtils.Logger.setLogFileAppenderName(log);
            Logger.setRootLogLevel(log);
            //End: Added to Configure the logger root LogLevel using App.config on 08-March-2016

            log.LogMethodEntry();
            InitializeComponent();
            lblMessage.Text = "";
            log.LogMethodExit();
            tagNumberParser = new TagNumberParser(POSStatic.Utilities.ExecutionContext);
            ulcKeyStore = new UlcKeyStore();
        }

        private void btnSubmit_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            btnSubmit.BackgroundImage = Properties.Resources.pressed2;
            log.LogMethodExit();
        }

        private void btnSubmit_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            btnSubmit.BackgroundImage = Properties.Resources.normal2;
            log.LogMethodExit();
        }

        byte[] getKey()
        {
            log.LogMethodEntry();
            byte[] customerAuthKey = new byte[6];

            string key = Encryption.GetParafaitKeys("NonMifareAuthorization");
            for (int i = 0; i < 5; i++)
                customerAuthKey[i] = Convert.ToByte(key[i]);
            customerAuthKey[5] = Convert.ToByte(POSStatic.Utilities.MifareCustomerKey);

            log.LogMethodExit("customerAuthKey");
            return customerAuthKey;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            lblMessage.Text = "";
            try
            {
                if (rbInvalidateFreePlayCards.Checked)
                {
                    Random rnd = new Random();
                    int value = rnd.Next(1000, 9999);
                    POSStatic.Utilities.executeNonQuery(@"Update parafait_defaults 
                                                        set default_value = @value 
                                                        where default_value_name ='FREE_PLAY_CARD_MAGIC_COUNTER'",
                                                        new System.Data.SqlClient.SqlParameter("@value", value.ToString()));
                    POSStatic.Utilities.EventLog.logEvent("POS", 'D', "FREE_PLAY_CARD_MAGIC_COUNTER", "Invalidate Free Play Cards", "CONFIG-CARDS", 0, "FreePlay Magic Counter", value.ToString(), null);
                    POSUtils.ParafaitMessageBox("Free play cards have been invalidated. Please restart game readers.");
                    log.Info("Ends-btnSubmit_Click() as Free play cards have been invalidated. Please restart game readers");//Added for logger function on 08-Mar-2016
                    return;
                }
                else if (rbExitFreePlayMode.Checked)
                {
                    POSStatic.Utilities.executeNonQuery(@"Update parafait_defaults 
                                                        set default_value = 'N' 
                                                        where default_value_name ='FREE_PLAY_MODE'
                                                        and default_value = 'Y'");
                    POSStatic.Utilities.EventLog.logEvent("POS", 'D', "Exit FREE_PLAY_MODE", "Exit free play mode", "CONFIG-CARDS", 0);
                    POSUtils.ParafaitMessageBox("Free play mode exited");
                    log.Info("Ends-btnSubmit_Click() as Free play mode exited");
                    return;
                }
                else if (rbEnterFreePlayMode.Checked)
                {
                    POSStatic.Utilities.executeNonQuery(@"Update parafait_defaults 
                                                        set default_value = 'Y' 
                                                        where default_value_name ='FREE_PLAY_MODE'
                                                        and default_value = 'N'");
                    POSStatic.Utilities.EventLog.logEvent("POS", 'D', "Enter FREE_PLAY_MODE", "Enter free play mode", "CONFIG-CARDS", 0);
                    POSUtils.ParafaitMessageBox("Free play mode set in configuration");
                    log.Info("Ends-btnSubmit_Click() as Free play mode entered");
                    return;
                }

                if (Card == null)
                {
                    POSUtils.ParafaitMessageBox(POSStatic.MessageUtils.getMessage(257));
                    log.Info("Ends-btnSubmit_Click() as need to Tap a card");//Added for logger function on 08-Mar-2016
                    return;
                }

                byte[] dataBuffer = new byte[16];
                string message = "";
                CardType cardType = Card.ReaderDevice.CardType;
                byte[] key;
                if (cardType == CardType.MIFARE_ULTRA_LIGHT_C)
                {
                    key = ulcKeyStore.LatestCustomerUlcKey.Value;
                }
                else
                {
                    key = getKey();
                }
                bool response = Card.ReaderDevice.read_data(4, 1, key, ref dataBuffer, ref message);
                if (!response)
                {
                    POSUtils.ParafaitMessageBox("Unable to read card");
                    log.Info("Ends-btnSubmit_Click() as Unable to read card");//Added for logger function on 08-Mar-2016
                    return;
                }

                byte[] tempArr = new byte[0];
                string CMD = "";
                if (rbFreePlayCard.Checked)
                {
                    tempArr = Encoding.ASCII.GetBytes("_FREEPLAY_");
                    byte[] counter = BitConverter.GetBytes(Convert.ToUInt16(POSStatic.Utilities.getParafaitDefaults("FREE_PLAY_CARD_MAGIC_COUNTER")));
                    byte[] newArr = new byte[tempArr.Length + counter.Length];
                    Array.Copy(tempArr, newArr, tempArr.Length);
                    Array.Copy(counter, 0, newArr, tempArr.Length, counter.Length);
                    tempArr = newArr;
                    CMD = "FREEPLAY";
                }
                else if (rbExitFreePlay.Checked)
                {
                    tempArr = Encoding.ASCII.GetBytes("_ENDFREEP_");
                    CMD = "ENDFREEP";
                }
                else if (rbChangeSSID.Checked)
                {
                    tempArr = Encoding.ASCII.GetBytes("_SSIDCHNG_");
                    byte[] ssid = BitConverter.GetBytes(Convert.ToUInt16(nudSSID.Value));
                    byte[] newArr = new byte[tempArr.Length + ssid.Length];
                    Array.Copy(tempArr, newArr, tempArr.Length);
                    Array.Copy(ssid, 0, newArr, tempArr.Length, ssid.Length);
                    tempArr = newArr;
                    CMD = "SSIDCHNG_" + nudSSID.Value.ToString();
                }

                byte[] Arr = new byte[16];
                Array.Copy(tempArr, 0, Arr, 1, tempArr.Length);
                Arr[0] = 0x4c;
                Arr[tempArr.Length + 1] = (byte)'_';
                Arr[tempArr.Length + 2] = 0x4c;

                tempArr = EncryptionAES.Encrypt(Arr, getKey(Card.CardNumber));
                
                cardType = Card.ReaderDevice.CardType;
                if (cardType == CardType.MIFARE_ULTRA_LIGHT_C)
                {
                    key = ulcKeyStore.LatestCustomerUlcKey.Value;
                }
                else
                {
                    key = getKey();
                }
                response = Card.ReaderDevice.write_data(4, 1, key, tempArr, ref message);
                if (!response)
                {
                    POSUtils.ParafaitMessageBox("Writing data to card failed: " + message);
                    log.Info("Ends-btnSubmit_Click() as Writing data to card failed: " + message);//Added for logger function on 08-Mar-2016
                    return;
                }
                Card.ReaderDevice.beep();

                if (Card.CardStatus == "NEW")
                {
                    Card.createCard(null);
                }

                POSStatic.Utilities.EventLog.logEvent("POS", 'D', CMD, "Config Cards setup", "CONFIG-CARDS", 0, "Card Number", Card.CardNumber, null);

                lblMessage.Text = "Master card created successfully";
                POSUtils.ParafaitMessageBox(lblMessage.Text);
                log.Info("btnSubmit_Click() - Master card created successfully ");//Added for logger function on 08-Mar-2016
            }
            catch (Exception ex)
            {
                POSUtils.ParafaitMessageBox(ex.Message);
                log.Fatal("Ends-btnSubmit_Click() due to exception " + ex.Message);//Added for logger function on 08-Mar-2016
            }
            log.LogMethodExit();
        }

        byte[] getKey(string _CardNumber)
        {
            log.LogMethodEntry(_CardNumber);
            string encryptionKey = Encryption.GetParafaitKeys("MifareCard");// "46A97988SEMNOX!1CCCC9D1C581D86EE";

            byte[] key = Encoding.UTF8.GetBytes(encryptionKey);
            key[16] = Convert.ToByte(Convert.ToInt32(_CardNumber[0].ToString() + _CardNumber[1].ToString(), 16));
            key[17] = Convert.ToByte(Convert.ToInt32(_CardNumber[2].ToString() + _CardNumber[3].ToString(), 16));
            key[18] = Convert.ToByte(Convert.ToInt32(_CardNumber[4].ToString() + _CardNumber[5].ToString(), 16));
            key[19] = Convert.ToByte(Convert.ToInt32(_CardNumber[6].ToString() + _CardNumber[7].ToString(), 16));
            log.LogMethodExit("encryptionKey");
            return key;
        }

        private void CardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (e is DeviceScannedEventArgs)
            {
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;

                TagNumber tagNumber;
                try
                {
                    tagNumber = tagNumberParser.Parse(checkScannedEvent.Message);
                }
        
                catch (Exception ex)
                {
                    log.Error("Error occured while parsing the scanned tag number", ex);
                    POSUtils.ParafaitMessageBox(ex.Message);
                    log.LogMethodExit(null, "Invalid Tag Number.");
                    return;
                }
                log.Debug("TagNumber :" + tagNumber);
                string CardNumber = tagNumber.Value;
                CardSwiped(CardNumber, sender as DeviceClass);
            }
            log.LogMethodExit();
        }

        private void CardSwiped(string CardNumber, DeviceClass readerDevice)
        {
            log.LogMethodEntry(CardNumber, readerDevice);
            try
            {
                Card = new Card(readerDevice, CardNumber, POSStatic.ParafaitEnv.LoginID, POSStatic.Utilities);
                txtCardNumber.Text = CardNumber;
                lblMessage.Text = "";

                try
                {
                    byte[] dataBuffer = new byte[16];
                    string message = "";
                    CardType cardType = Card.ReaderDevice.CardType;
                    byte[] key;
                    if (cardType == CardType.MIFARE_ULTRA_LIGHT_C)
                    {
                        key = ulcKeyStore.LatestCustomerUlcKey.Value;
                    }
                    else
                    {
                        key = getKey();
                    }
                    log.Debug("Reading the card details ");
                    bool response = Card.ReaderDevice.read_data(4, 1, key, ref dataBuffer, ref message);
                    if (response)
                    {
                        byte[] decr = EncryptionAES.Decrypt(dataBuffer, getKey(Card.CardNumber));
                        int token = 0;
                        string cmd = "", data = "";
                        int i = 0;
                        while (i < decr.Length)
                        {
                            while (decr[i++] != '_')
                                continue;

                            byte[] tempb = new byte[20];
                            int ind = 0;
                            for (int j = i; j < decr.Length; j++)
                            {
                                if (decr[j] == '_')
                                {
                                    i = j;
                                    break;
                                }
                                tempb[ind++] = decr[j];
                            }

                            if (token == 0)
                            {
                                cmd = Encoding.UTF8.GetString(tempb).TrimEnd('\0');
                                token++;
                            }
                            else if (token == 1)
                            {
                                data = BitConverter.ToUInt16(new byte[] { tempb[0], tempb[1] }, 0).ToString();
                                break;
                            }
                        }

                        switch (cmd)
                        {
                            case "FREEPLAY": lblMessage.Text = "Free Play " + data; break;
                            case "ENDFREEP": lblMessage.Text = "Exit Free Play"; break;
                            case "SSIDCHNG": lblMessage.Text = "Change SSID " + data; break;
                            default: lblMessage.Text = ""; break;
                        }
                    }
                }
                catch //(Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                }
            }
            catch (Exception ex)
            {
                POSUtils.ParafaitMessageBox(ex.Message); 
                log.Fatal("Ends-CardSwiped(" + CardNumber + ",readerDevice) due to exception " + ex.Message);//Added for logger function on 08-Mar-2016
            }
            log.LogMethodExit();
        }

        private void btnCancel_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            btnCancel.BackgroundImage = Properties.Resources.pressed2;
            log.LogMethodExit();
        }

        private void btnCancel_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            btnCancel.BackgroundImage = Properties.Resources.normal2;
            log.LogMethodExit();
        }

        private void frmConfigCards_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            (Application.OpenForms["POS"] as Parafait_POS.POS).lastTrxActivityTime = DateTime.Now;
            lblSSID.Visible = nudSSID.Visible = false;
            Common.Devices.RegisterCardReaders(new EventHandler(CardScanCompleteEventHandle));
            log.LogMethodExit();
        }

        private void frmConfigCards_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            Common.Devices.UnregisterCardReaders();
            log.LogMethodExit();
        }

        private void rbInvalidateFreePlayCards_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (rbInvalidateFreePlayCards.Checked)
                grpCardNumber.Visible = false;
            else
                grpCardNumber.Visible = true;

            log.LogMethodExit();
        }

        private void rbChangeSSID_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (rbChangeSSID.Checked)
                lblSSID.Visible = nudSSID.Visible = true;
            else
                lblSSID.Visible = nudSSID.Visible = false;

            log.LogMethodExit();
        }

        private void rbExitFreePlayMode_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (rbExitFreePlayMode.Checked)
                grpCardNumber.Visible = false;
            else
                grpCardNumber.Visible = true;

            log.LogMethodExit();
        }

        private void rbEnterFreePlayMode_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (rbEnterFreePlayMode.Checked)
                grpCardNumber.Visible = false;
            else
                grpCardNumber.Visible = true;

            log.LogMethodExit();
        }
    }
}
