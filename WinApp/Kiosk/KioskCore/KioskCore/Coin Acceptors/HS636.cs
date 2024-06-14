/********************************************************************************************
 * Project Name - KioskCore  
 * Description  - HS636.cs
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.80        3-Sep-2019       Deeksha            Added logger methods.
 *2.150.1     22-Feb-2023      Guru S A           Kiosk Cart Enhancements
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.KioskCore.CoinAcceptor
{
    public class HS636 : CoinAcceptor
    {
        public HS636()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        public override bool ProcessReceivedData(byte[] hs636_rec, ref string message)
        {
            log.LogMethodEntry(hs636_rec, message);
            try
            {
                // 'H', 'S', 0x00, 0x00, 0x02, 0x0d, 0x0a
                if (hs636_rec[0].Equals(0x48) && hs636_rec[1].Equals(0x53))
                {
                    ReceivedCoinDenomination = Convert.ToInt32(hs636_rec[4]);
                    bool ret = true;// KioskStatic.updateAcceptance(-1, ReceivedCoinDenomination, acceptance);
                    if (ret)
                    {
                        message = KioskStatic.config.Coins[ReceivedCoinDenomination].Name + " accepted";
                        KioskStatic.logToFile(message);
                    }
                    else
                    {
                        ReceivedCoinDenomination = 0;
                        message = "Denomination " + ReceivedCoinDenomination.ToString() + " rejected";
                        KioskStatic.logToFile(message);
                    }
                    log.LogMethodExit(ret);
                    return ret;
                }
                log.LogMethodExit(false);
                return false;
            }
            catch (Exception ex)
            {
                message = ex.Message + ex.StackTrace;
                log.LogMethodExit(false);
                return false;
            }
        }

        public override void disableCoinAcceptor()
        {
            log.LogMethodEntry();
            spCoinAcceptor.Write("D");
            log.LogMethodExit();
        }

        public override bool set_acceptance(bool isTokenMode = false)
        {
            log.LogMethodEntry(isTokenMode);
            spCoinAcceptor.Write("E");
            log.LogMethodExit(true);
            return true;
        }

        public override void checkStatus()
        {
        }
    }
}