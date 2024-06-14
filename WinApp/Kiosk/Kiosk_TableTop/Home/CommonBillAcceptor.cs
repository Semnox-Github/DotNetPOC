using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Semnox.Parafait.KioskCore.BillAcceptor;
using Semnox.Parafait.KioskCore;

namespace Parafait_Kiosk.Home
{
    public class CommonBillAcceptor
    {
        BillAcceptor billAcceptor;
        KioskStatic.acceptance acceptance;
        KioskStatic.configuration config = KioskStatic.config;
        Thread NV9Thread;

        public CommonBillAcceptor()
        {
            log.LogMethodEntry();
            if (KioskStatic.BillAcceptorModel.Equals(BillAcceptor.Models.NV9USB))
            {
                billAcceptor = KioskStatic.getBillAcceptor();
                ((NV9USB)billAcceptor).dataReceivedEvent = billAcceptorDataReceived;
            }
            else
                throw new ApplicationException("CommonBA: Bill Acceptor not defined");

            acceptance = billAcceptor.acceptance;
            log.LogMethodExit();
        }

        public void Start()
        {
            log.LogMethodEntry();
            if (NV9Thread == null || NV9Thread.IsAlive == false)
            {
                if (NV9Thread != null)
                {
                    try
                    {
                        NV9Thread.Abort();
                    }
                    catch { }
                }
                NV9Thread = new Thread(billAcceptor.initialize);
                NV9Thread.Start();
                acceptance.productPrice = 9999999;

                KioskStatic.logToFile("Direct cash Bill acceptor started");
            }
            else
                KioskStatic.logToFile("Direct cash Bill acceptor thread already running");
            log.LogMethodExit();
        }

        public void Stop()
        {
            log.LogMethodEntry();
            billAcceptor.disableBillAcceptor();
            KioskStatic.logToFile("Direct cash Bill acceptor disabled");
            log.LogMethodExit();
        }

        public delegate void InvokeHandle(KioskStatic.acceptance pAcceptance);
        InvokeHandle receiveAction;

        void handleBillRead(KioskStatic.acceptance pAcceptance)
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

        void billAcceptorDataReceived()
        {
            log.LogMethodEntry();
            string billMessage = "";
            KioskStatic.billAcceptorDatareceived = true;
            bool noteRecd = billAcceptor.ProcessReceivedData(KioskStatic.billAcceptorRec, ref billMessage);
            if (noteRecd)
            {
                KioskStatic.updateKioskActivityLog(billAcceptor.ReceivedNoteDenomination, -1, "", "BILL-IN", "DIRECTCASH: Bill Inserted", acceptance);
                billAcceptor.ReceivedNoteDenomination = 0;

                System.Threading.Thread.Sleep(300);
                billAcceptor.requestStatus();

                Application.OpenForms[0].BeginInvoke((MethodInvoker)delegate
                {
                    handleBillRead(acceptance);
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
