/********************************************************************************************
 * Project Name - Digital Signage
 * Description  - URL Video
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        12-Aug-2019   Deeksha             Added logger methods.
 ********************************************************************************************/
using System;
using System.Windows.Forms;

namespace Semnox.Parafait.DigitalSignage
{
    public partial class frmURLVideo : Form
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        string shockwaveurl = string.Empty;
        /// <summary>
        /// Show URL Video previews
        /// </summary>
        /// <param name="videourl">video URL</param>
        public frmURLVideo(string videourl)
        {
            log.LogMethodEntry(videourl);
            InitializeComponent();
            shockwaveurl = videourl;
            log.LogMethodExit();
        }

        private void frmURLVideo_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            loadWebBrowser();
            log.LogMethodExit();
        }
        private void loadWebBrowser()
        {
            log.LogMethodEntry();
            Uri urivideo = new Uri(shockwaveurl);
           
            try
            {
                VideoWebBrowser.Url = urivideo;
            }
            catch (Exception ex)
            {
                log.Error("Error while executing loadWebBrowser()" + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }
    }
}
