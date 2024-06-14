/********************************************************************************************
 * Project Name - ScheduleLaunchUI
 * Description  - Schedule launch UI 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks           
 *2.70.0      12-Mar-2019   Guru S A       Modified for schedule class renaming as par of booking phase2
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
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

namespace Semnox.Parafait.Schedule
{
    public partial class ScheduleLaunchUI : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Semnox.Core.Utilities.Utilities _Utilities;
        string connstring = "";
        public ScheduleLaunchUI()
        {
            log.LogMethodEntry();
            InitializeComponent();
            connstring = ConfigurationManager.ConnectionStrings["ParafaitConnectionString"].ConnectionString;
            _Utilities = new Semnox.Core.Utilities.Utilities(connstring);
            _Utilities.ParafaitEnv.User_Id = 6;
            _Utilities.ParafaitEnv.LoginID = "Semnox Test";
            _Utilities.ParafaitEnv.SiteId = 1010;
            log.LogMethodExit();
        }

        private void btnSchedule_Click(object sender, EventArgs e)
        {
            try
            {
                ScheduleCalendarUI scheduleUI = new ScheduleCalendarUI(_Utilities);
                scheduleUI.Show();
            }
            catch (Exception e1)
            { MessageBox.Show(e1.ToString()); }
        }

        private void btnScheduleExclusions_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                List<ScheduleCalendarDTO> scheduleCalendarDTOList;
                List<KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>> scheduleCalendarSearchParams = new List<KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>>();
                scheduleCalendarSearchParams.Add(new KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>(ScheduleCalendarDTO.SearchByScheduleCalendarParameters.IS_ACTIVE, "1"));
                ScheduleCalendarDataHandler scheduleCalendarDataHandler = new ScheduleCalendarDataHandler(_Utilities.ExecutionContext, null);
                scheduleCalendarDTOList = scheduleCalendarDataHandler.GetScheduleCalendarDTOList(scheduleCalendarSearchParams);
                if (scheduleCalendarDTOList != null)
                {
                    using (ScheduleExclusionUI scheduleExclusionUI = new ScheduleExclusionUI(scheduleCalendarDTOList[0], _Utilities))
                    {
                        scheduleExclusionUI.Show();
                    }
                }
                else { MessageBox.Show("No schedule."); }
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.ToString());
                log.Error(e1);
            }
            log.LogMethodExit();
        }

        private void btnScheduleUI_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                using (ScheduleUI scheduleUI = new ScheduleUI(null, _Utilities))
                {
                    scheduleUI.Show();
                }
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.ToString());
                log.Error(e1);
            }
            log.LogMethodExit();
        }
    }
}
