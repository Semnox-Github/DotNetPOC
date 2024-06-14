/********************************************************************************************
 * Project Name - Redemption Kiosk
 * Description  - Kiosk Base form 
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.4.0       3-Sep-2018       Archana            Created 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Semnox.Parafait.Customer
{
    public partial class FrmKioskBaseForm : Form
    {
        static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int kioskTimerTick = 30;
        protected int kioskSecondsRemaining;
        public FrmKioskBaseForm()
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
        public void ResetKioskTimer()
        {
            log.LogMethodEntry();
            kioskSecondsRemaining = kioskTimerTick;
            log.LogMethodExit(kioskSecondsRemaining);
        }
        public void StopKioskTimer()
        {
            log.LogMethodEntry();
            kioskTimer.Stop();
            log.LogMethodExit();
        }

        public int GetKioskTimerSecondsValue()
        {
            log.LogMethodEntry();
            log.LogMethodExit(kioskSecondsRemaining);
            return kioskSecondsRemaining;
        }

        public void SetKioskTimerSecondsValue(int secRemaining)
        {
            log.LogMethodEntry(secRemaining);
            kioskSecondsRemaining = secRemaining;
            log.LogMethodExit();
        }
        public void StartKioskTimer()
        {
            log.LogMethodEntry();
            kioskTimer.Start();
            log.LogMethodExit();
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

        protected virtual void KioskTimer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (kioskSecondsRemaining == 10)
            {
                this.Close();
            }
            else
            {
                kioskSecondsRemaining--;
            }
            log.LogMethodExit();
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
    }
}
