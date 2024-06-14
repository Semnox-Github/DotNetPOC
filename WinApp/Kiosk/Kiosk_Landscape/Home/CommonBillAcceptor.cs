/********************************************************************************************
* Project Name - Parafait_Kiosk 
* Description  - CommonBillAcceptor.cs
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
 * 2.80         4-Sep-2019       Deeksha        Added logger methods.
********************************************************************************************/
using System;
using System.Threading;
using System.Windows.Forms;
using Semnox.Parafait.KioskCore.BillAcceptor;
using Semnox.Parafait.KioskCore;

namespace Parafait_Kiosk.Home
{
    public class CommonBillAcceptor
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
                throw new ApplicationException("Bill Acceptor not defined");

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
                    catch (Exception ex)
                    {
                        log.Error("Error occurred while executing Start()" + ex.Message);
                    }
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
