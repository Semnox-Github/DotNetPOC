using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Parafait.logging;
namespace Semnox.Parafait.Device.PaymentGateway.Menories
{
    public partial class frmUx300Status : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public bool IsExitTriggered = false;
        public string MessageToDisplay = "Processing... Please wait...";
        public string HeaderTextToDisplay = "Moneris Payment Gateway";
        public frmUx300Status(string message = "")
        {
            log.LogMethodEntry();

            InitializeComponent();
            try
            {
                if (System.IO.File.Exists(Application.StartupPath + "\\Media\\Images\\PaymentScreen.png"))
                {
                    this.BackgroundImage = Image.FromFile(Application.StartupPath + "\\Media\\Images\\PaymentScreen.png");
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while setting up the background image", ex);
            }
            lblAmount.Text = message;
            this.Size = this.BackgroundImage.Size;

            log.LogMethodExit(null);
        }

        private void frmUx300Status_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            lblStatus.Text = "Processing... Please wait...";
            MessageDisplayTimer.Start();

            log.LogMethodExit(null);
        }

        private void MessageDisplayTimer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            lblHeading.Text = HeaderTextToDisplay;
            lblHeading.Refresh();
            lblStatus.Text = MessageToDisplay;
            lblStatus.Refresh();
            if (IsExitTriggered)
            {
                MessageDisplayTimer.Stop();
                Thread.Sleep(3000);
                this.Close();
            }

            log.LogMethodExit(null);
        }
    }
}
