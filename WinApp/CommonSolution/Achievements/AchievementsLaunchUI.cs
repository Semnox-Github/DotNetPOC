/********************************************************************************************
 * Project Name - Achievements
 * Description  - UI -AchievementLaunch
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.2        13-Aug-2019   Deeksha                 Added logger methods.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Configuration;
using System.Windows.Forms;

namespace Semnox.Parafait.Achievements
{
    /// <summary>
    /// AchievementsLaunchUI
    /// </summary>
    public partial class AchievementsLaunchUI : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Utilities _Utilities;
        private string connstring = "";
        /// <summary>
        /// 
        /// </summary>
        /// 
        public AchievementsLaunchUI()
        {
            log.LogMethodEntry();
            InitializeComponent();
            connstring = ConfigurationManager.ConnectionStrings["ParafaitConnectionString"].ConnectionString;
            _Utilities = new Utilities(connstring);
            _Utilities.ParafaitEnv.User_Id = 3;
            _Utilities.ParafaitEnv.LoginID = "Semnox";
            _Utilities.ParafaitEnv.SiteId = -1;
            ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();


            if (_Utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(_Utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            machineUserContext.SetUserId(_Utilities.ParafaitEnv.LoginID);
            _Utilities.setLanguage(this);
            log.LogMethodExit();
        }

        private void btnAchievementProjects_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                AchievementProjectsUI achievementProjectsUI = new AchievementProjectsUI(_Utilities);
                achievementProjectsUI.Show();
            }
            catch (Exception e1)
            {
                log.Error("error while Excecuting btnAchievementProjects_Click()" + e1.Message);
                MessageBox.Show(e1.ToString());
            }


            log.LogMethodExit();
        }

        private void btnAchievementClass_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                AchievementClassUI achievementClassUI = new AchievementClassUI(_Utilities,-1);
                achievementClassUI.Show();
            }
            catch (Exception e1)
            {
                log.Error("error while Excecuting btnAchievementClass_Click()" + e1.Message);
                MessageBox.Show(e1.ToString());
            }
            log.LogMethodExit();
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                AchievementDetailsPosUI achievementDetailsPos = new AchievementDetailsPosUI(_Utilities,"");
                achievementDetailsPos.Show();
            }
            catch (Exception e1)
            {
                log.Error("error while Excecuting btn1_Click()" + e1.Message);
                MessageBox.Show(e1.ToString());
            }
            log.LogMethodExit();
        }
    }
}
