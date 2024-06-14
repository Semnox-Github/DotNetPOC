using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KioskCore;
using System.Windows.Forms;

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
            if (KioskStatic.BillAcceptorModel.Equals(BillAcceptor.Models.NV9USB))
            {
                billAcceptor = KioskStatic.getBillAcceptor();
                ((NV9USB)billAcceptor).dataReceivedEvent = billAcceptorDataReceived;
            }
            else
                throw new ApplicationException("CommonBA: Bill Acceptor not defined");

            acceptance = billAcceptor.acceptance;
        }

        public void Start()
        {
            if (NV9Thread != null)
            {
                KioskStatic.logToFile("NV9Thread active. Disabling Direct Cash acceptor");
                billAcceptor.disableBillAcceptor();
                KioskStatic.logToFile("Direct cash Bill acceptor disabled");
            }

            NV9Thread = new Thread(billAcceptor.initialize);
            NV9Thread.Start();
            acceptance.productPrice = 9999999;

            KioskStatic.logToFile("Direct cash Bill acceptor started");
        }

        public void Stop()
        {
            billAcceptor.disableBillAcceptor();
            NV9Thread = null;
            KioskStatic.logToFile("Direct cash Bill acceptor disabled");
        }

        public delegate void InvokeHandle(KioskStatic.acceptance pAcceptance);
        InvokeHandle receiveAction;

        void handleBillRead(KioskStatic.acceptance pAcceptance)
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

        void billAcceptorDataReceived()
        {
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
        }

        public KioskStatic.acceptance GetAcceptance()
        {
            return acceptance;
        }
    }
}
