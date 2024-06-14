/********************************************************************************************
* Project Name - Parafait POS
* Description  - CheckInPauseLogUI 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
 *2.80     20-Aug-2019     Girish Kundar       Modified to add logger methods. 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;

namespace Parafait_POS
{
    public partial class CheckInPauseLogUI : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int checkInDetailId;
        private Utilities Utilities;
        public CheckInPauseLogUI(int checkInDetailId, Utilities Utilities,string detail)
        {
            log.LogMethodEntry(checkInDetailId, Utilities, detail);
            InitializeComponent();
            this.checkInDetailId = checkInDetailId;
            this.Utilities = Utilities;
            grpboxCheckInPauseLog.Text = "Check-In Pause Log - " + detail;
            lblTotalPauseTime.Text = lblTotalPauseTime.Text + ": ";

            lastUpdateDateDataGridViewTextBoxColumn.DefaultCellStyle = Utilities.gridViewDateTimeCellStyle();
            pauseEndTimeDataGridViewTextBoxColumn.DefaultCellStyle = Utilities.gridViewDateTimeCellStyle();
            pauseStartTimeDataGridViewTextBoxColumn.DefaultCellStyle = Utilities.gridViewDateTimeCellStyle();
            totalPauseTimeDataGridViewTextBoxColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            creationDateDataGridViewTextBoxColumn.DefaultCellStyle = Utilities.gridViewDateTimeCellStyle();
            Utilities.setLanguage(this);
            log.LogMethodExit();
        }

        private void CheckInPauseLogUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Utilities.setupDataGridProperties(ref dgvCheckInPauseLog);

            CheckInPauseLogListBL checkInPauseLogListBL = new CheckInPauseLogListBL(Utilities.ExecutionContext);
            List<KeyValuePair<CheckInPauseLogDTO.SearchByCheckInPauseLogParameters, string>> searchParams = new List<KeyValuePair<CheckInPauseLogDTO.SearchByCheckInPauseLogParameters, string>>();
            searchParams.Add(new KeyValuePair<CheckInPauseLogDTO.SearchByCheckInPauseLogParameters, string>(CheckInPauseLogDTO.SearchByCheckInPauseLogParameters.CHECK_IN_DETAIL_ID, checkInDetailId.ToString()));
            List<CheckInPauseLogDTO> checkInPauseLogDTOList = checkInPauseLogListBL.GetCheckInPauseLogDTOList(searchParams);
            if (checkInPauseLogDTOList != null && checkInPauseLogDTOList.Count > 0)
            {
                checkInPauseLogDTOList = checkInPauseLogDTOList.OrderByDescending(x => x.PauseStartTime).ToList();
                dgvCheckInPauseLog.DataSource = checkInPauseLogDTOList;
                double totalPauseTime = checkInPauseLogDTOList.Sum(x => x.TotalPauseTime);
                log.LogVariableState("Total Pause Time", totalPauseTime);
                lblTimeInMins.Text = totalPauseTime + " (mins)";
            }
            log.LogMethodExit();
        }
    }
}
