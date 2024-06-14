using Semnox.Core.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Semnox.Parafait.KioskCore
{
    public partial class frmPreLoader : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Timer preloaderTimer;
        private ConcurrentQueue<KeyValuePair<int, string>> statusProgressMsgQueue;
        private static string CLOSEFORM = "CLOSEFORM";
        public frmPreLoader(Utilities utilities, Image backGround, Image preLoaderGIF, ConcurrentQueue<KeyValuePair<int, string>> statusProgressMsgQueue)
        {
            log.LogMethodEntry(utilities, backGround, preLoaderGIF);
            InitializeComponent();

            btnWait.BackgroundImage = backGround;// Properties.Resources.Back_button_box;
            btnWait.Image = preLoaderGIF;// Properties.Resources.PreLoader;
            btnWait.Text = utilities.MessageUtils.getMessage(1008); //Processing..Please wait...
            this.TopLevel = this.TopMost = true;
            this.statusProgressMsgQueue = statusProgressMsgQueue;
            preloaderTimer = new Timer();
            preloaderTimer.Interval = 50;
            preloaderTimer.Tick += new EventHandler(PreloaderTimerTick);
            preloaderTimer.Start();
            log.LogMethodExit();
        }

        private void PreloaderTimerTick(object sender, EventArgs e)
        {
            bool dontRestart = false;
            try
            {
                preloaderTimer.Stop(); 
                if (statusProgressMsgQueue != null)
                {
                    KeyValuePair<int, string> msgItem = new KeyValuePair<int, string>();
                    while (statusProgressMsgQueue.IsEmpty == false)
                    {
                        statusProgressMsgQueue.TryDequeue(out msgItem);
                        if (msgItem.Value == CLOSEFORM)
                        {
                            dontRestart = true;
                            this.Close();
                            log.LogMethodExit("CLose form");
                            break;
                        } 
                    }
                }
            }
            catch
            {

            }
            finally
            {
                if (dontRestart == false)
                {
                    preloaderTimer.Start();
                }
            }
        }
    }
}
