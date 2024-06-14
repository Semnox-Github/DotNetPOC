/********************************************************************************************
 * Class Name - Maintenance                                                                         
 * Description - Asset Launch UI 
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                   Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        12-Aug-2019            Deeksha        Added logger methods.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Configuration;
using System.Windows.Forms;

namespace Semnox.Parafait.Maintenance
{
    public partial class AssetLaunchUI : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Utilities _Utilities;
        private string connstring = "";
        public AssetLaunchUI()
        {
            log.LogMethodEntry();
            InitializeComponent();
            connstring = ConfigurationManager.ConnectionStrings["ParafaitConnectionString"].ConnectionString;
            _Utilities = new Utilities(connstring);
            _Utilities.ParafaitEnv.User_Id = 6;
            _Utilities.ParafaitEnv.LoginID = "Semnox Test";
            _Utilities.ParafaitEnv.SiteId = 1010;
            log.LogMethodExit();
        }

        private void assetGroupLaunchBtn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                AssetGroupUI assetGroupUI = new AssetGroupUI(_Utilities);              
                assetGroupUI.Show();
            }
            catch (Exception e1)
            {
                log.Error("Error occured wile executing assetGroupLaunchBtn_Click()" + e1.Message);
                MessageBox.Show(e1.ToString());
            }
            log.LogMethodExit();
        }

        private void assetTypeLaunchBtn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                AssetTypeUI assetTypeUI = new AssetTypeUI(_Utilities);
                assetTypeUI.Show();
            }
            catch (Exception e1)
            {
                log.Error("Error occured wile executing assetTypeLaunchBtn_Click()" + e1.Message);
                MessageBox.Show(e1.ToString());
            }
            log.LogMethodExit();
        }

        private void assetLaunchBtn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                GenericAssetUI assetUI = new GenericAssetUI(_Utilities);
                assetUI.Show();
            }
            catch (Exception e1)
            {
                log.Error("Error occured wile executing assetLaunchBtn_Click()" + e1.Message);
                MessageBox.Show(e1.ToString());
            }
            log.LogMethodExit();
        }

        private void btnGroup_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                AssetGroupAssetMapUI assetGrpMapUI = new AssetGroupAssetMapUI(_Utilities);
                assetGrpMapUI.Show();
            }
            catch (Exception e1)
            {
                log.Error("Error occured wile executing btnGroup_Click()" + e1.Message);
                MessageBox.Show(e1.ToString());
            }
            log.LogMethodExit();
        }
    }
}
