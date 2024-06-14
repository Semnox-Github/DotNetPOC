using System;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.logging;

namespace Semnox.Parafait.KioskCore.BillAcceptor
{
    public class CommonBillAcceptor
    {
        private BillAcceptor billAcceptor;
        //KioskStatic.acceptance acceptance;
        private KioskStatic.configuration config = KioskStatic.config;
        private Semnox.Parafait.logger.Monitor _monitor;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        //public CommonBillAcceptor(logger.Monitor billAcceptorMonitor)
        //{
        //    log.LogMethodEntry(billAcceptorMonitor);
        //    if (KioskStatic.BillAcceptorModel.Equals(BillAcceptor.Models.NV9USB))
        //    {
        //        billAcceptor = KioskStatic.getBillAcceptor();
        //        ((NV9USB)billAcceptor).dataReceivedEvent = billAcceptorDataReceived;
        //    }
        //    else
        //        throw new ApplicationException("CommonBA: Bill Acceptor not defined");

        //   //acceptance = billAcceptor.acceptance;
        //    _monitor = billAcceptorMonitor;
        //    log.LogMethodExit();
        //}

        public CommonBillAcceptor(ExecutionContext executionContext, logger.Monitor billAcceptorMonitor, string serialPortNumber)
        {
            log.LogMethodEntry(executionContext, billAcceptorMonitor, serialPortNumber);
            this.executionContext = executionContext;
            if (KioskStatic.BillAcceptorModel.Equals(BillAcceptor.Models.NV9USB))
            {
                billAcceptor = KioskStatic.getBillAcceptor(serialPortNumber);
                ((NV9USB)billAcceptor).dataReceivedEvent = billAcceptorDataReceived;
            }
            else
                throw new ApplicationException("CommonBA: Bill Acceptor not defined");

            //acceptance = billAcceptor.acceptance;
            _monitor = billAcceptorMonitor;
            log.LogMethodExit();
        }

        public void Start()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Starting direct cash Bill acceptor");
            
           //acceptance.productPrice = 9999999;
            billAcceptor.initialize();
            billAcceptor.AmountRemainingToBeCollected = 9999999; 
            KioskStatic.logToFile("Direct cash Bill acceptor started");
            log.LogMethodExit();
        }

        public void Stop()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Stopping direct cash Bill acceptor");
            billAcceptor.disableBillAcceptor();
            KioskStatic.logToFile("Direct cash Bill acceptor disabled");
            log.LogMethodExit();
        }

        public delegate void InvokeHandle(int noteDenominationReceived);
        InvokeHandle receiveAction;

        private void handleBillRead(int noteDenominationReceived)
        {
            log.LogMethodEntry(noteDenominationReceived);
            if (receiveAction != null)
                receiveAction.Invoke(noteDenominationReceived);

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

        private void billAcceptorDataReceived()
        {
            log.LogMethodEntry();
            string billMessage = "";
            KioskStatic.billAcceptorDatareceived = true;
            
            bool noteRecd = billAcceptor.ProcessReceivedData(KioskStatic.billAcceptorRec, ref billMessage);

            if (billAcceptor.Working)
                _monitor.Post(Semnox.Parafait.logger.Monitor.MonitorLogStatus.INFO, "Bill Acceptor running");
            else
                _monitor.Post(Semnox.Parafait.logger.Monitor.MonitorLogStatus.ERROR, billMessage);

            if (noteRecd)
            {
                //log.LogVariableState("AC", acceptance);
                log.LogVariableState("billAcceptor.ReceivedNoteDenomination", billAcceptor.ReceivedNoteDenomination);
                int denomination = billAcceptor.ReceivedNoteDenomination;
                //KioskStatic.updateKioskActivityLog(billAcceptor.ReceivedNoteDenomination, -1, "", "BILL-IN", "DIRECTCASH: Bill Inserted", null);
                KioskStatic.ReceivedDenominationToActivityLog(executionContext, -1, string.Empty,
                                                                    denomination, KioskTransaction.GETBILLIN,
                                                                    KioskTransaction.GETDIRECTCASH_BILLINMSG, KioskStatic.NOTE); 
                billAcceptor.ReceivedNoteDenomination = 0;

                System.Threading.Thread.Sleep(300);
                billAcceptor.requestStatus();

                Application.OpenForms[0].BeginInvoke((MethodInvoker)delegate
                {
                    handleBillRead(denomination);
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

        public BillAcceptor GetBillAcceptor()
        {
            log.LogMethodEntry();
            log.LogMethodExit(billAcceptor);
            return billAcceptor;
        } 
    }
}
