/********************************************************************************************
 * Project Name - KioskCore  
 * Description  - GBAST1.cs
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.80        3-Sep-2019       Deeksha            Added logger methods.
 *2.150.1     22-Feb-2023      Guru S A           Kiosk Cart Enhancements
 ********************************************************************************************/
using MSCPL_SDK;

namespace Semnox.Parafait.KioskCore.BillAcceptor
{
    public class GBAST1:BillAcceptor
    {
        NoteReader _NoteReader;
        public delegate void dataReceivedDelegate();
        public dataReceivedDelegate dataReceivedEvent = null;
        Status globalSatus;
        int globalDenomination;
        public GBAST1(NoteReader noteReader)
        {
             log.LogMethodEntry(noteReader);
            _NoteReader = noteReader;
            _NoteReader.NoteFeedEvent += new CoinOrNoteEventDelegate(_NoteReader_NoteFeedEvent);
            log.LogMethodExit();
        }

        public override bool ProcessReceivedData(byte[] billRec, ref string message)
        {
             log.LogMethodEntry(billRec, message);
            switch (globalSatus)
            {
                case Status.RESET_POWER_OFF_ON:
                    _NoteReader.MasterEnable = true;
                    break;

                case Status.MASTER_INHIBIT:
                    _NoteReader.MasterEnable = true;
                    break;

                case Status.ERROR:
                    message = _NoteReader.ErrorCode.ToString() + "," + _NoteReader.ErrorDescription + "," + globalSatus.ToString();
                    break;

                case Status.SENT_TO_CACHE_BOX:
                    {
                        bool ret = false;
                        if (ReceivedNoteDenomination > 0)
                        {
                            //ret = KioskStatic.updateAcceptance(ReceivedNoteDenomination, -1, acceptance);
                            ret = true;
                            message = KioskStatic.config.Notes[ReceivedNoteDenomination].Name + " accepted";
                            KioskStatic.logToFile(message);
                        }
                        log.LogMethodExit(ret);
                        return ret;
                    }
                case Status.BILL_HELD_IN_ESCROW:
                    {
                        ReceivedNoteDenomination = globalDenomination;
                        if (KioskStatic.config.Notes[ReceivedNoteDenomination] != null)
                        {
                            _NoteReader.SendToCacheBox();
                            message = KioskStatic.config.Notes[ReceivedNoteDenomination].Name + " inserted";
                            KioskStatic.logToFile(message);
                        }
                        else
                        {
                            message = "Bill [Denomination: " + ReceivedNoteDenomination.ToString() + "] rejected";
                            ReceivedNoteDenomination = 0;
                            KioskStatic.logToFile(message);
                        }
                        break;
                    }
                case Status.BARCODE:
                    _NoteReader.SendToCacheBox();
                    message = "BARCODE DETECTED";
                    break;
            }
            log.LogMethodExit(false);
            return false;
        }

        public override void disableBillAcceptor()
        {
            log.LogMethodEntry();
            _NoteReader.MasterEnable = false;
            log.LogMethodExit();
        }

        public override void requestStatus()
        {
        }

        public override void initialize()
        {
            log.LogMethodEntry();
            _NoteReader.MasterEnable = true;
            log.LogMethodExit();
        }

        void _NoteReader_NoteFeedEvent(string dt, int SerNumber, int Category, string CCTalkName, double CurrencyValue, Status stat)
        {
            log.LogMethodEntry(dt, SerNumber, Category, CCTalkName, CurrencyValue, stat);
            globalSatus = stat;
            globalDenomination = Category;
            dataReceivedEvent();            
            log.LogMethodExit();
        }
    }
}
