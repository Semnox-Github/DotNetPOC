using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parafait_POS
{
    public partial class frmWaiverWelcome : Form
    {
        //Begin: Added for logger function on 15-May-2016
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //End: Added for logger function on 15-May-2016

        public frmWaiverWelcome()
        {
            try
            {
                log.Debug("Start-frmWaiverWelcome() to display the welcome screen on Waiver");

                InitializeComponent();
                Semnox.Core.Utilities.Utilities utilities = new Semnox.Core.Utilities.Utilities();
                bool found = false;
                Image image = null;
                GenericUtils genericUtils = new GenericUtils();
                try
                {
                    byte[] bytes = genericUtils.GetFileFromServer(utilities, ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "IMAGE_DIRECTORY") + "\\WaiverWelcomeScreen.png");
                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        image = System.Drawing.Image.FromStream(ms, false, true);
                        if (image != null)
                            found = true;
                    }

                    if (!found)
                    {
                        image = Properties.Resources.WaiverWelcomeScreen;
                    }
                }
                catch
                {
                    image = Properties.Resources.WaiverWelcomeScreen;
                }

                pbWelcomeImage.Image = image;
                log.Debug("Ends-frmWaiverWelcome() to display the welcome screen on Waiver");
            }
            catch(Exception ex)
            {
                log.Error("Error while loading welcome screen into Waiver display. " + ex.Message);
            }
        }
    }
}
