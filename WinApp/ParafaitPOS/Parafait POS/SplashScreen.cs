using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace Parafait_POS
{
    public partial class SplashScreen : Form
    {
        public SplashScreen()
        {
            try
            {
                InitializeComponent();
                this.BackgroundImage = Image.FromFile(Environment.CurrentDirectory + "\\Resources\\Parafait-Start-Splash.png");
                this.ClientSize = this.BackgroundImage.Size;
            }
            catch { }
        }

        public void CloseSplash()
        {
            this.Close();
        }
    }
}
