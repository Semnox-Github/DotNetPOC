/********************************************************************************************
* Project Name - Parafait POS
* Description  - frmCancelTrxCard 
* 
**************
**Version Log
**************
*Version     Date             Modified By          Remarks          
*********************************************************************************************
 *2.70        1-Jul-2019      Lakshminarayana     Modified to add support for ULC cards 
 *2.80.0        20-Aug-2019    Girish Kundar       Modified :  Added logger methods and Removed unused namespace's
********************************************************************************************/
using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;
using Logger = Semnox.Core.Utilities.Logger;

namespace Parafait_POS
{
    public partial class frmCancelTrxCard : Form
    {
        Transaction _Trx;
        //Begin: Modified Added for logger function on 08-Mar-2016
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //End: Modified Added for logger function on 08-Mar-2016
        private readonly TagNumberParser tagNumberParser;

        public frmCancelTrxCard(Transaction pTrx)
        { 
            //Begin: Added to Configure the logger root LogLevel using App.config on 08-March-2016           
            //log = ParafaitUtils.Logger.setLogFileAppenderName(log);
            Logger.setRootLogLevel(log);
            //End: Added to Configure the logger root LogLevel using App.config on 08-March-2016

            log.LogMethodEntry(pTrx);
            InitializeComponent();
            _Trx = pTrx;
            tagNumberParser = new TagNumberParser(POSStatic.Utilities.ExecutionContext);
            log.LogMethodExit();
        }

        private void frmCancelTrxCard_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            displayMessageLine("Tap Card to cancel from Order");
            log.Info("frmCancelTrxCard_Load() - Tap Card to cancel from Order");//Added for logger function on 08-Mar-2016
            this.ActiveControl = dgvCards;

            Common.Devices.RegisterCardReaders(new EventHandler(CardScanCompleteEventHandle));
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
                    log.LogMethodExit(null, "Invalid Tag Number.");
                    return;
                }

                string CardNumber = tagNumber.Value;
                log.LogVariableState("CardNumber" , CardNumber);
                try
                {
                    CardSwiped(CardNumber, sender as DeviceClass);
                }
                catch(Exception ex)
                {
                    displayMessageLine(ex.Message);
                    log.Fatal("Ends-CardScanCompleteEventHandle() due to exception " + ex.Message);//Added for logger function on 08-Mar-2016
                }
            }
            log.LogMethodExit();
        }

        private void displayMessageLine(string message)
        {
            log.LogMethodEntry(message);
            txtMessage.Text = message;
            log.LogMethodExit();
        }

        private void CardSwiped(string CardNumber, DeviceClass readerDevice)
        {
            log.LogMethodEntry(CardNumber , readerDevice);
            displayMessageLine("");

            // make sure card is valid, part of this trx and not used anywhere before and after. 
            object cardId = POSStatic.Utilities.executeScalar(@"select top 1 c.card_id 
                                                                from trx_header th, trx_lines tl, cards c 
                                                                where tl.card_id = c.card_Id 
                                                                and c.valid_flag = 'Y'
                                                                and c.card_number = @cardNumber
                                                                and tl.trxid = th.trxid
                                                                and th.trxid = @trxId
                                                                and th.status = 'OPEN'
                                                                and not exists (select 1 from trxPayments tp where cardId = c.card_id)
                                                                and not exists (select 1 from gameplay gp where gp.card_Id = c.card_id)
                                                                and not exists (select 1 from trx_lines l where l.card_Id = c.card_id and l.trxId != th.trxId)",
                                                                new SqlParameter("@cardNumber", CardNumber),
                                                                new SqlParameter("@trxId", _Trx.Trx_id));
            if (cardId == null)
            {
                displayMessageLine("Invalid card or card does not to belong to Order, or Card has been used.");
                log.Info("Ends-CardSwiped(" + CardNumber + ", readerDevice) as Invalid card or card does not to belong to Order, or Card has been used)");//Added for logger function on 08-Mar-2016
                return;
            }

            if (POSUtils.ParafaitMessageBox("Do you want to cancel this card from Order?", "Confirm") == System.Windows.Forms.DialogResult.Yes)
            {
                Card card;
                if (POSStatic.ParafaitEnv.MIFARE_CARD)
                    card = new MifareCard(readerDevice, CardNumber, "", POSStatic.Utilities);
                else
                    card = new Card(readerDevice, CardNumber, "", POSStatic.Utilities);
                string message = "";

                if (POSStatic.ParafaitEnv.MIFARE_CARD)
                {
                    bool ret = card.refund_MCard(ref message);
                    displayMessageLine(message);
                    if (!ret)
                    {
                        log.Info("Ends-CardSwiped(" + CardNumber + ", readerDevice) as no refund_MCard !ret error: " + message);//Added for logger function on 08-Mar-2016
                        return;
                    }
                }

                SqlConnection cnn = null;
                SqlTransaction SQLTrx = POSStatic.Utilities.createConnection().BeginTransaction();
                cnn = SQLTrx.Connection;
                try
                {
                    int i = 0;
                    foreach (Transaction.TransactionLine tl in _Trx.TrxLines)
                    {
                        if (CardNumber.Equals(tl.CardNumber))
                        {
                            _Trx.cancelLine(i, SQLTrx);
                        }
                        i++;
                    }
                    card.invalidateCard(SQLTrx);
                    SQLTrx.Commit();
                }
                catch (Exception ex)
                {
                    SQLTrx.Rollback();
                    displayMessageLine(ex.Message);
                    log.Fatal("Ends-CardSwiped(" + CardNumber + ", readerDevice) due to exception " + ex.Message);//Added for logger function on 08-Mar-2016
                    return;
                }

                dgvCards.Rows.Add(cardId, CardNumber);

                displayMessageLine("Card cancelled successfully");
                log.Info("CardSwiped(" + CardNumber + ", readerDevice) - Card cancelled successfully");//Added for logger function on 08-Mar-2016
            }
            log.LogMethodExit();
        }

        private void frmCancelTrxCard_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            Common.Devices.UnregisterCardReaders();
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Close();
            log.LogMethodExit();
        }
    }
}
