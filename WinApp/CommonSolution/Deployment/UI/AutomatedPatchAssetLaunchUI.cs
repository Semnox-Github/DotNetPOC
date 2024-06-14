using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.Deployment
{
    /// <summary>
    /// 
    /// </summary>
    public partial class AutomatedPatchAssetLaunchUI : Form
    {
        Utilities _Utilities;
        string connstring = "";
        /// <summary>
        /// 
        /// </summary>
        public AutomatedPatchAssetLaunchUI()
        {
            InitializeComponent();
            connstring = ConfigurationManager.ConnectionStrings["ParafaitConnectionString"].ConnectionString;
            _Utilities = new Utilities(connstring);
            _Utilities.ParafaitEnv.User_Id = 6;
            _Utilities.ParafaitEnv.LoginID = "Semnox Test";
            _Utilities.ParafaitEnv.Manager_Flag = "Y";
            _Utilities.ParafaitEnv.SiteId = 1010;
        }

        private void btnApplicationType_Click(object sender, EventArgs e)
        {
            try
            {
                AutoPatchApplicationTypeUI applicationTypeUI = new AutoPatchApplicationTypeUI(_Utilities);
                applicationTypeUI.ShowDialog();
                applicationTypeUI.Dispose();
            }
            catch (Exception e1)
            { MessageBox.Show(e1.ToString()); }
        }

        private void btnApplicationAsset_Click(object sender, EventArgs e)
        {
            try
            {
                AutoPatchAssetApplicationUI assetApplicationUI = new AutoPatchAssetApplicationUI(_Utilities);
                assetApplicationUI.ShowDialog();
                assetApplicationUI.Dispose();
            }
            catch (Exception e1)
            { MessageBox.Show(e1.ToString()); }
        }
    }
}
