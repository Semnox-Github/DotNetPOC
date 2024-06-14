/********************************************************************************************
 * Project Name - Redemption Kiosk
 * Description  - TapCard UI
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.3.0       05-Jun-2018      Archana/Guru S A     Created
 * 2.70        1-Jul-2019      Lakshminarayana     Modified to add support for ULC cards 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Transaction;

namespace Redemption_Kiosk
{
    public partial class frmRedemptionKioskTapCard : frmRedemptionKioskBaseForm
    {
        internal string cardNumber;
        DeviceClass cardReaderDevice = null;
        private readonly TagNumberParser tagNumberParser;

        /// <summary>
        /// Get method of the cardNumber field
        /// </summary>
        [DisplayName("Card Number")]
        [Browsable(false)]
        public string CardNumber { get { return cardNumber; } }
        public frmRedemptionKioskTapCard()
        {
            log.LogMethodEntry();
            InitializeComponent();
            DisplayMessage(Common.utils.MessageUtils.getMessage(500));
            tagNumberParser = new TagNumberParser(Common.utils.ExecutionContext);

            log.LogMethodExit();

        }
        void DisplayMessage(string text)
        {
            log.LogMethodEntry(text);
            lblmsg.Text = text;
            log.LogMethodExit();
        }

        private void TapCardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e is DeviceScannedEventArgs)
            {
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;
                TagNumber tagNumber;
                if (tagNumberParser.TryParse(checkScannedEvent.Message, out tagNumber) == false)
                {
                    string message = tagNumberParser.Validate(checkScannedEvent.Message);
                    DisplayMessage(message);
                    log.LogMethodExit(null, "Invalid Tag Number. " + message);
                    return;
                }

                string lclCardNumber = tagNumber.Value;
                lclCardNumber = RedemptionKioskHelper.ReverseTopupCardNumber(lclCardNumber);
                try
                {
                    HandleCardRead(lclCardNumber, sender as DeviceClass);
                    DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    DisplayMessage(ex.Message);
                }
            }
            log.LogMethodExit();
        }


        void HandleCardRead(string inCardNumber, DeviceClass readerDevice)
        {
            log.LogMethodEntry(inCardNumber, readerDevice);
            Card card = new Card(readerDevice, inCardNumber, Common.utils.ExecutionContext.GetUserId(), Common.utils);

            string message = "";
            DisplayMessage(Common.utils.MessageUtils.getMessage(1607)); //("Refreshing Card from HQ. Please Wait..."
            Application.DoEvents();
            try
            {
                card = RedemptionKioskHelper.RefreshCardFromHQ(card);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(Common.utils.MessageUtils.getMessage(441) + Environment.NewLine + message);
            }
            if (card.CardStatus == "NEW")
            {
                throw new Exception(Common.utils.MessageUtils.getMessage(459));
            }
            else
            {
                if (card.technician_card.Equals('Y'))
                {
                    throw new Exception(Common.utils.MessageUtils.getMessage(197, card.CardNumber));
                }
                else
                {
                    cardNumber = card.CardNumber;
                }
            }
            log.LogMethodExit();
        }

        private void FrmTapCard_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);            
            Common.utils.setLanguage(this);
            cardReaderDevice = RedemptionKioskHelper.RegisterCardReader(this, TapCardScanCompleteEventHandle);           

            if (cardReaderDevice == null)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.Close();
                return;
            }

            log.LogMethodExit(); 
        }

        private void FrmTapCard_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry(sender, e); 
            if (cardReaderDevice != null)
            {
                cardReaderDevice.UnRegister();
                cardReaderDevice.Dispose();
                log.Debug(this.Name + ": TopUp Reader unregistered");
            }
            log.LogMethodExit();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            log.LogMethodExit();
        }
    }
}
