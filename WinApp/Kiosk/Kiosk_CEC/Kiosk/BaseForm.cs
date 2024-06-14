/********************************************************************************************
* Project Name - Parafait_Kiosk - BaseForm
* Description  - BaseForm 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.90.1.0     21-Apr-2021      Guru S A           Wrist band printer flow changes
********************************************************************************************/
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.KioskCore.BillAcceptor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Parafait_Kiosk
{
    public partial class BaseForm : Form
    {
        internal static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public BaseForm()
        {
            log.LogMethodEntry();
            InitializeComponent();  
           
            kioskTimer.Interval = 1000;
            kioskTimer.Enabled = true;
            kioskTimer.Tick += KioskTimer_Tick;
            ResetKioskTimer();

            this.FormClosed += delegate
            {
                kioskTimer.Stop();
            };
            log.LogMethodExit();
        }

        private int kioskSecondsRemaining;
        private int kioskTimerTick = 30;

        public void SetKioskTimerTickValue(int tickCount = 30)
        {
            log.LogMethodEntry(tickCount);
            kioskTimerTick = tickCount;
            log.LogMethodExit();
        }
        public int GetKioskTimerTickValue()
        {
            log.LogMethodEntry();
            log.LogMethodExit(kioskTimerTick);
            return kioskTimerTick;
        }
        public int GetKioskTimerSecondsValue()
        {
            log.LogMethodEntry();
            log.LogMethodExit(kioskSecondsRemaining);
            return kioskSecondsRemaining;
        }

        public void setKioskTimerSecondsValue(int secRemaining)
        {
            log.LogMethodEntry(secRemaining);
            kioskSecondsRemaining = secRemaining;
            log.LogMethodExit(kioskSecondsRemaining);
        }

        public void ResetKioskTimer()
        {
            log.LogMethodEntry();
            try
            {
                kioskSecondsRemaining = kioskTimerTick;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(kioskSecondsRemaining);
        }
        public void StopKioskTimer()
        {
            log.LogMethodEntry();
            kioskTimer.Stop();
            log.LogMethodExit();
        }
        public void StartKioskTimer()
        {
            log.LogMethodEntry();
            kioskTimer.Start();
            log.LogMethodExit();
        }
        public bool GetKioskTimer()
        {
            log.LogMethodEntry();
            log.LogMethodExit(kioskTimer.Enabled);
            return kioskTimer.Enabled;
        }
        public void KioskTimerSwitch(bool boolSignal)
        {
            log.LogMethodEntry(boolSignal);
            kioskTimer.Enabled = boolSignal;
            log.LogMethodExit(kioskTimer.Enabled);
        }
        public void KioskTimerInterval(int timeDuration)
        {
            log.LogMethodEntry(timeDuration);
            kioskTimer.Interval = timeDuration;
            log.LogMethodExit(kioskTimer.Interval);
        }       
        public virtual void KioskTimer_Tick(object sender, EventArgs e)
        {
            //log.LogMethodEntry(sender, e);
            if (this == ActiveForm)
            {
                if (kioskSecondsRemaining <= 10)
                {
                    if (TimeOut.AbortTimeOut(this))
                    {
                        ResetKioskTimer();
                    }
                    else
                    {
                       this.Close();
                    }
                }
                kioskSecondsRemaining--;
            }
            else
            {
                ResetKioskTimer();
            }

            // log.LogMethodExit();
        }


        protected override CreateParams CreateParams
        {
            //this method is used to avoid the table layout flickering.
            get
            {
                CreateParams CP = base.CreateParams;
                CP.ExStyle = CP.ExStyle | 0x02000000;
                return CP;
            }
        }

        internal bool BillCollectorIsInWIPMode(BillAcceptor billAcceptor)
        {
            log.LogMethodEntry(billAcceptor);
            bool isInWIPMode = false;
            if (billAcceptor != null && billAcceptor.StillProcessing())
            {
                KioskStatic.logToFile("isInWIPMode: Still processing the bill");
                log.LogVariableState("Still processing the bill", billAcceptor);
                isInWIPMode = true;
            }
            KioskStatic.logToFile("isInWIPMode : " + isInWIPMode.ToString());
            log.LogMethodExit(isInWIPMode);
            return isInWIPMode;
        }
    }
}
