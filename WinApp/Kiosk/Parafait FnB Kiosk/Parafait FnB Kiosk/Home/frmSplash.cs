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

namespace Parafait_FnB_Kiosk
{
    public partial class frmSplash : BaseForm
    {
        public frmSplash()
        {
            Common.logEnter();
            InitializeComponent();
        }

        private void frmSplash_Load(object sender, EventArgs e)
        {
            Common.logEnter();
            try
            {
                object screenId = Common.utils.executeScalar("select screenId from AppScreens where CodeObjectName = 'frmSplash' and AppScreenProfileId = (select AppScreenProfileId from AppScreenProfile where AppScreenProfileName = 'Parafait FnB Kiosk')");
                if (screenId == null)
                    throw new ApplicationException("Splash screen not defined in setup");

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
                                                            new SqlParameter("@UIPanelElementId", _screenModel.getPanelByIndex(2).getElementByIndex(1).UIPanelElementId));

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
      
                    Timer splashTimer = new Timer();
                    splashTimer.Interval = 10 * 1000;
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
                            splashTimer.Stop();
                    };

                    splashTimer.Start();
                }
                Common.logExit();
            }
            catch (Exception ex)
            {
                Common.logException(ex);
                Common.ShowMessage(ex.Message);
            }
        }
    }
}
