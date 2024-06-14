using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KioskCore;
using System.Windows.Forms;

namespace Parafait_Kiosk.Home
{
    public class CommonCoinAcceptor
    {
        CoinAcceptor coinAcceptor;
        KioskStatic.acceptance acceptance;
        KioskStatic.configuration config = KioskStatic.config;

        public CommonCoinAcceptor(KioskStatic.acceptance Acceptance)
        {
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
        }

        public void Start()
        {
            if (coinAcceptor != null)
            {
                coinAcceptor.spCoinAcceptor = KioskStatic.spCoinAcceptor;
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
        }

        public void Stop()
        {
            if (coinAcceptor != null)
                coinAcceptor.disableCoinAcceptor();

            KioskStatic.caReceiveAction = null;

            KioskStatic.logToFile("Direct cash Coin acceptor disabled");
        }

        public delegate void InvokeHandle(KioskStatic.acceptance pAcceptance);
        InvokeHandle receiveAction;

        void handleCoinRead(KioskStatic.acceptance pAcceptance)
        {
            if (receiveAction != null)
                receiveAction.Invoke(pAcceptance);
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
        }

        public KioskStatic.acceptance GetAcceptance()
        {
            return acceptance;
        }
    }
}
