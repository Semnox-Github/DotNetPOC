/********************************************************************************************
 * Project Name - Redemption Kiosk
 * Description  - Splash UI
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.3.0       05-Jun-2018      Archana/Guru S A     Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Redemption_Kiosk
{
    public partial class frmRedemptionKioskSplashScreen : frmRedemptionKioskBaseForm
    {
        public frmRedemptionKioskSplashScreen()
        {
            log.LogMethodEntry();
            InitializeComponent();
            log.LogMethodExit();
        }

        private void FrmSplash_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (Common.parafaitEnv.POSMachineId == -1)
                {
                    Common.ShowMessage(Common.utils.MessageUtils.getMessage(1608));
                    //"Please register this machine as Kiosk"
                    Application.Exit();
                }

                this.KeyPreview = true;
                this.KeyPress += frmSplash_KeyPress;
                object screenId = Common.utils.executeScalar("select screenId from AppScreens where CodeObjectName = 'frmRedemptionKioskSplashScreen' and AppScreenProfileId = (select AppScreenProfileId from AppScreenProfile where AppScreenProfileName = 'Redemption Kiosk')");
                if (screenId == null)
                {
                    throw new ApplicationException(Common.utils.MessageUtils.getMessage(1609)); 
                    //"Splash screen not defined in setup"
                }

                _screenModel = new ScreenModel(Convert.ToInt32(screenId));

                base.RenderPanelContent(_screenModel, panelHeader, 1);
                base.RenderPanelContent(_screenModel, panelSplashImage, 2);

                DataTable dtSplashImages = Common.utils.executeDataTable(@"select a1.Image 
                                                                             from AppUIPanelElementAttribute a1
                                                                            where a1.UIPanelElementId = @UIPanelElementId
                                                                              and (a1.LanguageId = @languageId or a1.LanguageId is null)
                                                                              and a1.ActiveFlag = 1
                                                                              order by a1.LanguageId desc",
                                                                           new SqlParameter("@languageId", Common.parafaitEnv.LanguageId),
                                                                           new SqlParameter("@UIPanelElementId", _screenModel.GetPanelByIndex(2).getElementByIndex(1).UIPanelElementId));

                if (dtSplashImages.Rows.Count > 0)
                {
                    List<Image> imageList = new List<Image>();
                    foreach (DataRow dr in dtSplashImages.Rows)
                    {
                        if (dr[0] != DBNull.Value)
                        {
                            var image = (byte[])(dr[0]);
                            using (var stream = new System.IO.MemoryStream(image))
                            {
                                imageList.Add(Image.FromStream(stream));
                            }
                        }
                    }

                    Timer splashTimer = new Timer
                    {
                        Interval = 10 * 1000
                    };
                    splashTimer.Tick += delegate
                    {
                        if (this == ActiveForm && imageList.Count > 0)
                        {
                            Random rnd = new Random();
                            int imageIndex = rnd.Next(dtSplashImages.Rows.Count);
                            panelSplashImage.BackgroundImage = imageList[imageIndex];
                        }
                    };

                    this.FormClosed += delegate
                    {
                        if (splashTimer != null)
                        {
                            splashTimer.Stop();
                        }
                    };

                    splashTimer.Start();
                } 
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Common.ShowMessage(ex.Message);
            }
            log.LogMethodExit();
        }

        void frmSplash_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if ((int)e.KeyChar == 3)
            {
                Cursor.Show();
            }
            //else if ((int)e.KeyChar == 19)
            //{
            //    Parafait_Kiosk.SetUp s = new Parafait_Kiosk.SetUp();
            //    s.ShowDialog();
            //}
            else if ((int)e.KeyChar == 5) // ctrl e
            {
                log.Info("Ctrl-E pressed");
                Application.Exit();
            }
            log.LogMethodExit();
        }
    }
}
