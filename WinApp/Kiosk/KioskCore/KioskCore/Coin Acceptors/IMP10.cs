/********************************************************************************************
 * Project Name - KioskCore  
 * Description  - IMP10.cs
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
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.KioskCore.CoinAcceptor
{
    public class IMP10 : CoinAcceptor
    {
        public IMP10()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        public override bool ProcessReceivedData(byte[] imp10_rec, ref string message)
        {
            log.LogMethodEntry(imp10_rec, message);
            byte[] inp;
            if (Convert.ToString(imp10_rec[0], 16).ToUpper() == "0E" | Convert.ToString(imp10_rec[0], 16).ToUpper() == "E")
            {
                ReceivedCoinDenomination = Convert.ToInt32(imp10_rec[1]);
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
            else if (Convert.ToString(imp10_rec[0], 16).ToUpper() == "A1")
            {
                //message = "Insert Coins";
                message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 399);
            }
            else if (Convert.ToString(imp10_rec[0], 16).ToUpper() == "B1")
            {
                message = "Coin Acceptor Disabled";
                inp = new byte[] {
					0xf,
					0xaa
				};
                spCoinAcceptor.Write(inp, 0, 2);
            }
            else if (Convert.ToString(imp10_rec[1], 16).ToUpper() == "F1")
            {
                message = "No Acknowledgement Received";
            }
            log.LogMethodExit(false);
            return false;
        }

        public override void disableCoinAcceptor()
        {
            log.LogMethodEntry();
            byte[] inp = new byte[] {
				0xf,
				0xbb
			};
            spCoinAcceptor.Write(inp, 0, 2);
            log.LogMethodExit();
         }

        public override bool set_acceptance(bool isTokenMode = false)
        {
            log.LogMethodEntry(isTokenMode);
            if (spCoinAcceptor.IsOpen)
            {
                byte acceptance = 0;
                for (int i = 1; i < 5; i++)
                {
                    if (KioskStatic.config.Coins[i] == null)
                    {
                        Int32 b = 0x01;
                        b = b << i - 1;
                        acceptance += Convert.ToByte(b);
                    }
                }

                if (acceptance > 0)
                {
                    byte[] inp = new byte[] {
                0xe,
                acceptance};
                    spCoinAcceptor.Write(inp, 0, 2);

                    System.Threading.Thread.Sleep(100);
                    //enable coin acceptor
                    inp = new byte[] {
                    0xf,
                    0xaa
                };
                    spCoinAcceptor.Write(inp, 0, 2);
                    log.LogMethodExit(true);
                    return true;
                }
                else
                {
                    log.LogMethodExit(false);
                    return false;
                }
            }
            else
            {
                log.LogMethodExit(false);
                return false;
            }
        }

        public override void checkStatus()
        {
            log.LogMethodEntry();
            byte[] inp = new byte[] {
				0xf,
				0xcc
			};
            spCoinAcceptor.Write(inp, 0, 2);
            log.LogMethodExit();
        }
    }
}
