/********************************************************************************************
 * Project Name - KioskCore  
 * Description  - Common Coin Acceptor.cs
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.80        3-Sep-2019       Deeksha            Added logger methods.
 *2.150.1     22-Feb-2023      Guru S A           Kiosk Cart Enhancements
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Windows.Forms;

namespace Semnox.Parafait.KioskCore.CoinAcceptor
{
    public class CommonCoinAcceptor
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        CoinAcceptor coinAcceptor;
        //KioskStatic.acceptance acceptance;
        //KioskStatic.configuration config = KioskStatic.config;
        private ExecutionContext executionContext;
        public CommonCoinAcceptor(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            if (KioskStatic.CoinAcceptorModel.Equals(CoinAcceptor.Models.UCA2))
            {
                try
                {
                    coinAcceptor = KioskStatic.getCoinAcceptor(KioskStatic.config.coinAcceptorport);
                    //coinAcceptor.spCoinAcceptor = KioskStatic.spCoinAcceptor;
                    if (coinAcceptor.set_acceptance())
                        KioskStatic.caReceiveAction = coinAcceptorDataReceived;
                    else
                        KioskStatic.caReceiveAction = null;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    KioskStatic.logToFile("Error while Initializing Coin Acceptor: " + ex.Message);
                }
            }
            else
                throw new ApplicationException("UCA2 Coin Acceptor not defined");

            //if (Acceptance == null)
            //    acceptance = coinAcceptor.acceptance;
            //else
            //    acceptance = coinAcceptor.acceptance = Acceptance;
            log.LogMethodExit();
        }

        public void Start()
        {
            log.LogMethodEntry();
            if (coinAcceptor != null)
            {
                //coinAcceptor.spCoinAcceptor = KioskStatic.spCoinAcceptor;
                if (coinAcceptor.set_acceptance())
                {
                    KioskStatic.caReceiveAction = coinAcceptorDataReceived;
                    //acceptance.productPrice = 9999999;
                    coinAcceptor.AmountRemainingToBeCollected = 9999999;
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

        public delegate void InvokeHandle(int coindenomination);
        InvokeHandle receiveAction;

        void handleCoinRead(int coindenomination)
        {
            log.LogMethodEntry(coindenomination);
            if (receiveAction != null)
                receiveAction.Invoke(coindenomination);
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
                //KioskStatic.updateKioskActivityLog(coinAcceptor.ReceivedCoinDenomination, -1, "", "COIN-IN", "DIRECTCASH: Coin Inserted", acceptance);
                KioskStatic.ReceivedDenominationToActivityLog(executionContext,-1, null, coinAcceptor.ReceivedCoinDenomination, KioskTransaction.GETCOININ,
                     KioskTransaction.GETDIRECTCASH_COININMSG, KioskStatic.COIN);
                int denomination = coinAcceptor.ReceivedCoinDenomination;
                coinAcceptor.ReceivedCoinDenomination = 0;

                Application.OpenForms[0].BeginInvoke((MethodInvoker)delegate
                {
                    handleCoinRead(denomination);
                });
            }
            log.LogMethodExit();
        }

        //public KioskStatic.acceptance GetAcceptance()
        //{
        //    log.LogMethodEntry();
        //    log.LogMethodExit(acceptance);
        //    return acceptance;
        //}
    }
}
