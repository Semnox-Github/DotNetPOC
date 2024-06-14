using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Semnox.Parafait.KioskCore.CoinAcceptor;
using Semnox.Parafait.KioskCore;

namespace Parafait_Kiosk.Home
{
    public class CommonCoinAcceptor
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        CoinAcceptor coinAcceptor;
        KioskStatic.acceptance acceptance;
        KioskStatic.configuration config = KioskStatic.config;

        public CommonCoinAcceptor(KioskStatic.acceptance Acceptance)
        {
            log.LogMethodEntry(Acceptance);
            if (KioskStatic.CoinAcceptorModel.Equals(CoinAcceptor.Models.UCA2))
            {
                coinAcceptor = KioskStatic.getCoinAcceptor();
                coinAcceptor.spCoinAcceptor = KioskStatic.spCoinAcceptor;
                if (coinAcceptor.set_acceptance())
                    KioskStatic.caReceiveAction = coinAcceptorDataReceived;
                else
                    KioskStatic.caReceiveAction = null;
            }
            else
                throw new ApplicationException("UCA2 Coin Acceptor not defined");

            if (Acceptance == null)
                acceptance = coinAcceptor.acceptance;
            else
                acceptance = coinAcceptor.acceptance = Acceptance;
            log.LogVariableState("acceptance", acceptance);
            log.LogMethodExit();
        }

        //public CommonCoinAcceptor(KioskStatic.acceptance Acceptance, string serialPort)
        //{
        //    if (KioskStatic.CoinAcceptorModel.Equals(CoinAcceptor.Models.UCA2))
        //    {
        //        coinAcceptor = KioskStatic.getCoinAcceptor(serialPort);
        //        if (coinAcceptor.set_acceptance())
        //            KioskStatic.caReceiveAction = coinAcceptorDataReceived;
        //        else
        //            KioskStatic.caReceiveAction = null;
        //    }
        //    else
        //        throw new ApplicationException("UCA2 Coin Acceptor not defined");

        //    if (Acceptance == null)
        //        acceptance = coinAcceptor.acceptance;
        //    else
        //        acceptance = coinAcceptor.acceptance = Acceptance;
        //}

        public void Start()
        {
            log.LogMethodEntry();
            if (coinAcceptor != null)
            {
               // coinAcceptor.spCoinAcceptor = KioskStatic.spCoinAcceptor;
                if (coinAcceptor.set_acceptance())
                {
                    KioskStatic.caReceiveAction = coinAcceptorDataReceived;
                    acceptance.productPrice = 9999999;
                    KioskStatic.logToFile("Direct cash Coin acceptor started");
                }
                else
                {
                    KioskStatic.caReceiveAction = null;
                    KioskStatic.logToFile("Direct cash Coin acceptor not found");
                }
            }
            else
                KioskStatic.logToFile("Direct cash Coin acceptor not found");
            log.LogMethodExit();
        }

        public void Stop()
        {
            log.LogMethodEntry();
            if (coinAcceptor != null)
                coinAcceptor.disableCoinAcceptor();

            KioskStatic.caReceiveAction = null;

            KioskStatic.logToFile("Direct cash Coin acceptor disabled");
            log.LogMethodExit();
        }

        public delegate void InvokeHandle(KioskStatic.acceptance pAcceptance);
        InvokeHandle receiveAction;

        void handleCoinRead(KioskStatic.acceptance pAcceptance)
        {
            log.LogMethodEntry(pAcceptance);
            if (receiveAction != null)
                receiveAction.Invoke(pAcceptance);
            log.LogMethodExit();
        }

        public InvokeHandle setReceiveAction
        {
            get
            {
                return receiveAction;
            }
            set
            {
                receiveAction = value;
            }
        }

        void coinAcceptorDataReceived()
        {
            log.LogMethodEntry();
            string coinMessage = "";
            KioskStatic.coinAcceptorDatareceived = true;
            bool coinRecd = coinAcceptor.ProcessReceivedData(KioskStatic.coinAcceptor_rec, ref coinMessage);
            if (coinRecd)
            {
                KioskStatic.updateKioskActivityLog(coinAcceptor.ReceivedCoinDenomination, -1, "", "COIN-IN", "DIRECTCASH: Coin Inserted", acceptance);
                coinAcceptor.ReceivedCoinDenomination = 0;

                Application.OpenForms[0].BeginInvoke((MethodInvoker)delegate
                {
                    handleCoinRead(acceptance);
                });
            }
            log.LogMethodExit();
        }

        public KioskStatic.acceptance GetAcceptance()
        {
            log.LogMethodEntry();
            log.LogMethodExit(acceptance);
            return acceptance;
        }
    }
}
