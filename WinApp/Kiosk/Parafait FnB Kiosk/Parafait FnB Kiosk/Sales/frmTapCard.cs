/********************************************************************************************
* Project Name - Parafait_Kiosk -frmTapCard.cs
* Description  - frmTapCard 
* 
**************
**Version Log
**************
*Version     Date               Modified By        Remarks          
*********************************************************************************************
 * 2.80        09-Sep-2019      Deeksha            Added logger methods.
********************************************************************************************/
using System;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Transaction;
using System.Windows.Forms;

namespace Parafait_FnB_Kiosk
{
    public partial class frmTapCard : BaseForm
    {
        Utilities Utilities = Common.utils;
        DeviceClass TopUpReaderDevice = null;
        ScreenModel.ElementParameter _Parameter;
        internal frmTapCard(ScreenModel.ElementParameter Parameter)
        {
            log.LogMethodEntry(Parameter);
            InitializeComponent();
            displayMessage(Common.utils.MessageUtils.getMessage(500));
            _Parameter = Parameter;
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Close();
            log.LogMethodExit();
        }

        private void frmTapCard_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry();
            if (TopUpReaderDevice != null)
            {
                TopUpReaderDevice.UnRegister();
                TopUpReaderDevice.Dispose();
            }
            log.LogMethodExit();
        }

        private void CardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (e is DeviceScannedEventArgs)
            {
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;
                try
                {
                    TagNumber tagNumber;
                    TagNumberParser tagNumberParser = new TagNumberParser(Common.utils.ExecutionContext);
                    if (tagNumberParser.TryParse(checkScannedEvent.Message, out tagNumber) == false)
                    {
                        string message = tagNumberParser.Validate(checkScannedEvent.Message);
                        Common.logToFile(message);
                        return;
                    }
                    string lclCardNumber = tagNumber.Value.ToString();
                    lclCardNumber = ReverseTopupCardNumber(lclCardNumber);
                    handleCardRead(lclCardNumber, sender as DeviceClass); 
                }
                catch (Exception ex)
                {
                    Common.logException(ex);
                    displayMessage(ex.Message);
                }
            }
            log.LogMethodExit();
        }

        public string ReverseTopupCardNumber(string cardNumber)
        {
            log.LogMethodEntry(cardNumber);
            bool REVERSE_KIOSK_TOPUP_CARD_NUMBER = Common.utils.getParafaitDefaults("REVERSE_KIOSK_TOPUP_CARD_NUMBER").Equals("Y");

            if (REVERSE_KIOSK_TOPUP_CARD_NUMBER == false)
                return cardNumber;
            else
            {
                try
                {
                    char[] arr = cardNumber.ToCharArray();

                    for (int i = 0; i < cardNumber.Length / 2; i += 2)
                    {
                        char x = arr[i];
                        char y = arr[i + 1];

                        arr[i] = arr[cardNumber.Length - i - 2];
                        arr[i + 1] = arr[cardNumber.Length - i - 1];

                        arr[cardNumber.Length - i - 2] = x;
                        arr[cardNumber.Length - i - 1] = y;
                    }
                    string ret = new string(arr);
                    log.LogMethodExit(ret);
                    return ret;
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while executing ReverseTopupCardNumber" + ex.Message);
                    log.LogMethodExit(cardNumber);
                    return cardNumber;
                }
            }
        }

        void handleCardRead(string inCardNumber, DeviceClass readerDevice)
        {
            log.LogMethodEntry(inCardNumber, readerDevice);
            Card Card = new Card(readerDevice, inCardNumber, "External POS", Utilities);

            string message = "";
            displayMessage(Utilities.MessageUtils.getMessage("Refreshing Card from HQ. Please Wait..."));
            Application.DoEvents();
            if (!Helper.refreshCardFromHQ(ref Card, ref message))
            {
                displayMessage(Utilities.MessageUtils.getMessage(441) + Environment.NewLine + message);
                log.LogMethodExit();
                return;
            }

            if (Card.CardStatus == "NEW")
            {
                displayMessage(Utilities.MessageUtils.getMessage(459));
            }
            else
            {
                //Modified on 14-Apr-2017, to restrict load points to Tech card in KIOSK
                if (Card.technician_card.Equals('Y'))
                {
                    displayMessage(Utilities.MessageUtils.getMessage(197, Card.CardNumber));

                }
                else
                {
                    //end
                    _Parameter.CardNumber = inCardNumber;
                    DialogResult = System.Windows.Forms.DialogResult.OK;
                    Close();
                }
            }
            log.LogMethodExit();
        }

        void displayMessage(string text)
        {
            log.LogMethodEntry(text);
            lblmsg.Text = text;
            log.LogMethodExit();
        }

        private void frmTapCard_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                // if (!registerTopUpKBWedge())
                //    Close();
                TopUpReaderDevice = DeviceContainer.RegisterUSBCardReader(Common.utils.ExecutionContext, this, CardScanCompleteEventHandle);
            }
            catch (Exception ex)
            {
                Common.logException(ex);
                Common.ShowMessage(ex.Message);
                Close();
            }
            log.LogMethodExit();
        }
    }
}
