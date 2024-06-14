/********************************************************************************************
 * Class Name - Generic Utilities                                                                         
 * Description - frmScreenSaver UI 
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                   Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        09-Aug-2019            Deeksha        Added logger methods.
 ********************************************************************************************/
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;


namespace Semnox.Core.GenericUtilities
{
    public partial class frmScreenSaver : Form
    {
        Timer timer = new Timer();
        private DateTime startTime = DateTime.Now;
        int sequence = 0;
        Random _random = new Random();
        Semnox.Core.Utilities.Utilities Utilities = new Semnox.Core.Utilities.Utilities();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public frmScreenSaver()
        {
            log.LogMethodEntry();
            InitializeComponent();
            timer.Interval = 1;
            timer.Tick += timer_Tick;
            timer.Start();

            try
            {
                string folderPath = Utilities.getParafaitDefaults("IMAGE_DIRECTORY");

                Image image = null;
                GenericUtils genericUtils = new GenericUtils();
                try
                {
                    byte[] bytes = genericUtils.GetFileFromServer(Utilities, folderPath + "\\POS_ScreenSaverLogo.png");
                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        image = System.Drawing.Image.FromStream(ms, false, true);
                    }

                    Image logo = image;
                    if (logo != null)
                    {
                        pbLogo.BackgroundImage = logo;
                    }
                }
                catch(Exception ex)
                {
                    string[] files = System.IO.Directory.GetFiles(".\\Resources\\", "POSScreenSaverLogo.*");
                    if (files.Length > 0)
                    {
                        pbLogo.BackgroundImage = Image.FromFile(files[0]);
                    }
                    log.Info("Error while  executing GetFileFromServer()" + ex.Message);
                }
            }
            catch (Exception ex)
            {
                log.Info("Error while loading screen saver Logo : " + ex.Message);
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (sequence == 0)
            {
                double x = (DateTime.Now - startTime).TotalMilliseconds / 20;
                if (x > this.Width - 60)
                {
                    startTime = DateTime.Now;
                    sequence = _random.Next(1, 4);
                }
                else
                {
                    double y = 0;
                    int a = (this.Height - pbLogo.Height) / 2;

                    y = a - a * Math.Sin(2 * Math.PI * .1 * x * Math.PI / 180);
                    pbLogo.Location = new Point((int)x, (int)y);
                }
            }
            else if (sequence == 1)
            {
                int x = pbLogo.Left - 1;
                int y = pbLogo.Top + 1;

                if (y > this.Height - pbLogo.Height || x < 0)
                    sequence = _random.Next(0, 4);
                else
                    pbLogo.Location = new Point(x, y);
            }
            else if (sequence == 2)
            {
                int x = pbLogo.Left + 1;
                int y = pbLogo.Top - 1;

                if (x > this.Width || y < 0)
                    sequence = _random.Next(0, 4);
                else
                    pbLogo.Location = new Point(x, y);
            }
            else if (sequence == 3)
            {
                int x = pbLogo.Left - 1;
                int y = pbLogo.Top - 1;

                if (x < 0 || y < 0)
                    sequence = _random.Next(0, 4);
                else
                    pbLogo.Location = new Point(x, y);
            }
            else if (sequence == 4)
            {
                int x = pbLogo.Left + 1;
                int y = pbLogo.Top + 1;

                if (x > this.Width || y > this.Height - pbLogo.Height)
                    sequence = _random.Next(0, 4);
                else
                    pbLogo.Location = new Point(x, y);
            }
            log.LogMethodExit();
        }

        private void frmScreenSaver_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            timer.Stop();
            timer.Dispose();
            log.LogMethodExit();
        }
    }
}
