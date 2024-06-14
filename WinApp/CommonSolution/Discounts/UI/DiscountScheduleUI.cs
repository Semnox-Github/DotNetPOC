/********************************************************************************************
 * Project Name - DiscountScheduleUI
 * Description  - UI for discounts schedule
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70        23-Apr-2019   Guru S A       updates due to renamed classes for maint schedule module
 *2.70.2      05-Aug-2019   Girish Kundar  Added LogMethodEntry() and LogMethodExit() methods. 
 *2.80        28-Jun-2020   Deeksha        Modified to Make Product module read only in Windows Management Studio.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Discounts
{
    /// <summary>
    /// DiscountScheduleUI Class.
    /// </summary>
    public partial class DiscountScheduleUI : Form
    {
       private Utilities utilities;
       private  static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       private ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
       private DiscountsDTO discountsDTO;
       private ScheduleCalendarDTO scheduleDTO;
       private ManagementStudioSwitch managementStudioSwitch;
       
        /// <summary>
        /// Constructor of DiscountScheduleUI class.
        /// </summary>
        /// <param name="utilities">Parafait Utilities</param>
        /// <param name="discountsDTO">Discounts DTO</param>
        public DiscountScheduleUI(Utilities utilities, DiscountsDTO discountsDTO)
        {
            log.LogMethodEntry(utilities, discountsDTO);
            InitializeComponent();
            this.utilities = utilities;
            this.discountsDTO = discountsDTO;
            if (discountsDTO.ScheduleId != -1)
            {
                ScheduleCalendarListBL scheduleList = new ScheduleCalendarListBL(machineUserContext);
                List<KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>> searchParameters = new List<KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>>();
                searchParameters.Add(new KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>(ScheduleCalendarDTO.SearchByScheduleCalendarParameters.SCHEDULE_ID, discountsDTO.ScheduleId.ToString()));
                List<ScheduleCalendarDTO> scheduleDTOList = scheduleList.GetAllSchedule(searchParameters);
                if (scheduleDTOList != null && scheduleDTOList.Count > 0)
                {
                    scheduleDTO = scheduleDTOList[0];
                }
            }
            if (scheduleDTO == null)
            {
                scheduleDTO = new ScheduleCalendarDTO
                {
                    ScheduleId = -1,
                    ScheduleTime = DateTime.Now,
                    ScheduleEndDate = DateTime.Now,
                    RecurFlag = "N",
                    IsActive = true,
                    RecurEndDate = DateTime.MinValue
                };
            }
            utilities.setLanguage(this);
            ThemeUtils.SetupVisuals(this);
            managementStudioSwitch = new ManagementStudioSwitch(machineUserContext);
            UpdateUIElements();
            log.LogMethodExit();
        }

        private void DiscountScheduleUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender ,e);
            if (scheduleDTO != null)
            {
                txtScheduleName.MaxLength = 100;
                rdbDaily.Enabled = false;
                rdbMonthly.Enabled = false;
                rdbWeekly.Enabled = false;
                rdbDay.Enabled = false;
                rdbWeekDay.Enabled = false;
                dtpRecurEndDate.Enabled = false;

                chbActive.Checked = false;
                chbRecurFlag.Checked = false;
                rdbDaily.Checked = false;
                rdbMonthly.Checked = false;
                rdbWeekly.Checked = false;
                rdbDay.Checked = false;
                rdbWeekDay.Checked = false;

                txtScheduleName.Text = scheduleDTO.ScheduleName;
                dtpScheduleDate.Value = new DateTime(scheduleDTO.ScheduleTime.Year,
                                                     scheduleDTO.ScheduleTime.Month,
                                                     scheduleDTO.ScheduleTime.Day);
                dtpScheduleTime.Value = scheduleDTO.ScheduleTime;
                dtpScheduleEndTime.MinDate = scheduleDTO.ScheduleTime.Date;
                if (scheduleDTO.ScheduleEndDate == DateTime.MinValue)
                {
                    dtpScheduleEndDate.Value = dtpScheduleEndDate.MinDate;
                    dtpScheduleEndTime.Value = dtpScheduleEndDate.MinDate;
                }
                else
                {
                    dtpScheduleEndDate.Value = new DateTime(scheduleDTO.ScheduleEndDate.Year,
                                                            scheduleDTO.ScheduleEndDate.Month,
                                                            scheduleDTO.ScheduleEndDate.Day);
                    dtpScheduleEndTime.Value = scheduleDTO.ScheduleEndDate;
                }
                if (scheduleDTO.RecurEndDate == DateTime.MinValue)
                {
                    dtpRecurEndDate.Value = dtpRecurEndDate.MinDate;
                }
                else
                {
                    dtpRecurEndDate.Value = new DateTime(scheduleDTO.RecurEndDate.Year,
                                                     scheduleDTO.RecurEndDate.Month,
                                                     scheduleDTO.RecurEndDate.Day);
                }
                if (scheduleDTO.IsActive)
                {
                    chbActive.Checked = true;
                }

                if (!string.IsNullOrEmpty(scheduleDTO.RecurFlag) && string.Equals(scheduleDTO.RecurFlag, "Y"))
                {
                    chbRecurFlag.Checked = true;
                    rdbDaily.Enabled = true;
                    rdbMonthly.Enabled = true;
                    rdbWeekly.Enabled = true;
                    dtpRecurEndDate.Enabled = true;
                }
                log.LogVariableState("scheduleDTO", scheduleDTO);
                switch (scheduleDTO.RecurFrequency)
                {
                    case "D":
                        { 
                            rdbDaily.Checked = true;
                            dtpScheduleEndDate.Enabled = false;
                            dtpScheduleEndDate.Value = new DateTime(scheduleDTO.ScheduleTime.Year,
                                                                    scheduleDTO.ScheduleTime.Month,
                                                                    scheduleDTO.ScheduleTime.Day);
                            break;
                        }
                    case "W":
                        {
                            rdbWeekly.Checked = true;
                            dtpScheduleEndDate.Enabled = true;
                            dtpScheduleEndDate.MaxDate = dtpScheduleDate.Value.AddDays(6);
                            break;
                        }
                    case "M":
                        {
                            rdbMonthly.Checked = true;
                            rdbDay.Enabled = true;
                            rdbWeekDay.Enabled = true;
                            dtpScheduleEndDate.Enabled = false;
                            dtpScheduleEndDate.Value = new DateTime(scheduleDTO.ScheduleTime.Year,
                                                                    scheduleDTO.ScheduleTime.Month,
                                                                    scheduleDTO.ScheduleTime.Day);
                            break;
                        }
                }

                switch (scheduleDTO.RecurType)
                {
                    case "D":
                        {
                            rdbDay.Checked = true;
                            break;
                        }
                    case "W":
                        {
                            rdbWeekDay.Checked = true;
                            break;
                        }
                }

            }
            log.LogMethodExit();
        }

        private void chbRecurFlag_CheckStateChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (chbRecurFlag.Checked)
            {
                rdbDaily.Enabled = true;
                rdbMonthly.Enabled = true;
                rdbWeekly.Enabled = true;
                rdbDay.Enabled = true;
                rdbWeekDay.Enabled = true;
                dtpRecurEndDate.Enabled = true;
                if (dtpRecurEndDate.Value == dtpRecurEndDate.MinDate)
                {
                    dtpRecurEndDate.Value = DateTime.Today;
                }
                rdbDaily.Checked = true;
                dtpScheduleEndDate.Enabled = false;
                dtpScheduleEndDate.Value = dtpScheduleDate.Value;
            }
            else
            {
                rdbDaily.Enabled = false;
                rdbMonthly.Enabled = false;
                rdbWeekly.Enabled = false;
                rdbDay.Enabled = false;
                rdbWeekDay.Enabled = false;
                dtpRecurEndDate.Enabled = false;
                dtpScheduleEndDate.Enabled = true;
                dtpScheduleEndDate.MaxDate = dtpScheduleDate.MaxDate;

                rdbDaily.Checked = false;
                rdbMonthly.Checked = false;
                rdbWeekly.Checked = false;
                rdbDay.Checked = false;
                rdbWeekDay.Checked = false;
            }
            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                UpdateScheduleDTO();
                discountsDTO.ScheduleCalendarDTO = scheduleDTO;
                using (UnitOfWork unitOfWork = new UnitOfWork())
                {
                    unitOfWork.Begin();
                    DiscountsBL discountsBL = new DiscountsBL(machineUserContext, discountsDTO, unitOfWork);
                    discountsBL.Save();
                    unitOfWork.Commit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.DialogResult = DialogResult.OK;
            log.LogMethodExit();
        }

        private void btnInclExclDays_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ScheduleExclusionUI scheduleExclusionUI = new ScheduleExclusionUI(scheduleDTO, utilities);
            scheduleExclusionUI.ShowDialog();
            log.LogMethodExit();
        }

        private void UpdateScheduleDTO()
        {
            log.LogMethodEntry(); 
            if (scheduleDTO.ScheduleName != txtScheduleName.Text)
            {
                scheduleDTO.ScheduleName = txtScheduleName.Text;
            }
            if (scheduleDTO.ScheduleTime.Day != dtpScheduleDate.Value.Day ||
                scheduleDTO.ScheduleTime.Month != dtpScheduleDate.Value.Month ||
                scheduleDTO.ScheduleTime.Year != dtpScheduleDate.Value.Year)
            {
                scheduleDTO.ScheduleTime = dtpScheduleDate.Value;
            }

            if (scheduleDTO.ScheduleTime.Hour != dtpScheduleTime.Value.Hour ||
               scheduleDTO.ScheduleTime.Minute != dtpScheduleTime.Value.Minute ||
               scheduleDTO.ScheduleTime.Second != dtpScheduleTime.Value.Second)
            {
                DateTime dateTime = new DateTime(scheduleDTO.ScheduleTime.Year, scheduleDTO.ScheduleTime.Month, scheduleDTO.ScheduleTime.Day, dtpScheduleTime.Value.Hour, dtpScheduleTime.Value.Minute, dtpScheduleTime.Value.Second);
                scheduleDTO.ScheduleTime = dateTime;
            }

            if (scheduleDTO.ScheduleEndDate.Day != dtpScheduleEndDate.Value.Day ||
                scheduleDTO.ScheduleEndDate.Month != dtpScheduleEndDate.Value.Month ||
                scheduleDTO.ScheduleEndDate.Year != dtpScheduleEndDate.Value.Year)
            {
                scheduleDTO.ScheduleEndDate = dtpScheduleEndDate.Value;
            }

            if (scheduleDTO.ScheduleEndDate.Hour != dtpScheduleEndTime.Value.Hour ||
               scheduleDTO.ScheduleEndDate.Minute != dtpScheduleEndTime.Value.Minute ||
               scheduleDTO.ScheduleEndDate.Second != dtpScheduleEndTime.Value.Second)
            {
                DateTime dateTime = new DateTime(scheduleDTO.ScheduleEndDate.Year, scheduleDTO.ScheduleEndDate.Month, scheduleDTO.ScheduleEndDate.Day, dtpScheduleEndTime.Value.Hour, dtpScheduleEndTime.Value.Minute, dtpScheduleEndTime.Value.Second);
                scheduleDTO.ScheduleEndDate = dateTime;
            }

            if (scheduleDTO.RecurFlag == "Y")
            {
                if (!chbRecurFlag.Checked)
                {
                    scheduleDTO.RecurFlag = "N";
                }
            }
            else
            {
                if (chbRecurFlag.Checked)
                {
                    scheduleDTO.RecurFlag = "Y";
                }
            }
            if (scheduleDTO.RecurFlag == "Y")
            {
                if (scheduleDTO.RecurEndDate.Day != dtpRecurEndDate.Value.Day ||
                   scheduleDTO.RecurEndDate.Month != dtpRecurEndDate.Value.Month ||
                   scheduleDTO.RecurEndDate.Year != dtpRecurEndDate.Value.Year)
                {
                    scheduleDTO.RecurEndDate = dtpRecurEndDate.Value;
                }
                switch (scheduleDTO.RecurFrequency)
                {
                    case "D":
                        {
                            if (rdbWeekly.Checked)
                            {
                                scheduleDTO.RecurFrequency = "W";
                            }
                            else if (rdbMonthly.Checked)
                            {
                                scheduleDTO.RecurFrequency = "M";
                            }
                            break;
                        }
                    case "W":
                        {
                            if (rdbDaily.Checked)
                            {
                                scheduleDTO.RecurFrequency = "D";
                            }
                            else if (rdbMonthly.Checked)
                            {
                                scheduleDTO.RecurFrequency = "M";
                            }
                            break;
                        }
                    case "M":
                        {
                            if (rdbDaily.Checked)
                            {
                                scheduleDTO.RecurFrequency = "D";
                            }
                            else if (rdbWeekly.Checked)
                            {
                                scheduleDTO.RecurFrequency = "W";
                            }
                            break;
                        }
                    default:
                        {
                            if (rdbDaily.Checked)
                            {
                                scheduleDTO.RecurFrequency = "D";
                            }
                            else if (rdbWeekly.Checked)
                            {
                                scheduleDTO.RecurFrequency = "W";
                            }
                            else if (rdbMonthly.Checked)
                            {
                                scheduleDTO.RecurFrequency = "M";
                            }
                            break;
                        }
                }
                if (string.Equals(scheduleDTO.RecurFrequency, "M"))
                {
                    switch (scheduleDTO.RecurType)
                    {
                        case "D":
                            {
                                if (rdbWeekDay.Checked)
                                {
                                    scheduleDTO.RecurType = "W";
                                }
                                break;
                            }
                        case "W":
                            {
                                if (rdbDay.Checked)
                                {
                                    scheduleDTO.RecurType = "D";
                                }
                                break;
                            }
                        default:
                            {
                                if (rdbWeekDay.Checked)
                                {
                                    scheduleDTO.RecurType = "W";
                                }
                                if (rdbDay.Checked)
                                {
                                    scheduleDTO.RecurType = "D";
                                }
                                break;
                            }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(scheduleDTO.RecurType))
                    {
                        scheduleDTO.RecurType = "";
                    }
                }
            }
            else
            {
                if (scheduleDTO.RecurEndDate != DateTime.MinValue)
                {
                    scheduleDTO.RecurEndDate = DateTime.MinValue;
                }
                if (!string.IsNullOrEmpty(scheduleDTO.RecurFrequency))
                {
                    scheduleDTO.RecurFrequency = "";
                }
                if (!string.IsNullOrEmpty(scheduleDTO.RecurType))
                {
                    scheduleDTO.RecurType = "";
                }
            }
            if (scheduleDTO.IsActive)
            {
                if (!chbActive.Checked)
                {
                    scheduleDTO.IsActive = false;
                }
            }
            else
            {
                if (chbActive.Checked)
                {
                    scheduleDTO.IsActive = true;
                }
            }
            log.LogVariableState("ScheduleDTO" , scheduleDTO);
            log.LogMethodExit();
        }

        private string ValidateScheduleDTO(ScheduleCalendarDTO scheduleDTO)
        {
            log.LogMethodEntry(scheduleDTO);
            string message = string.Empty;
            if (string.IsNullOrEmpty(scheduleDTO.ScheduleName) || string.IsNullOrWhiteSpace(scheduleDTO.ScheduleName))
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", lblScheduleName.Text);
            }
            if (scheduleDTO.ScheduleTime.Date > scheduleDTO.ScheduleEndDate.Date)
            {
                message = utilities.MessageUtils.getMessage(571);
            }
            if (string.Equals(scheduleDTO.RecurFlag, "Y"))
            {
                if (scheduleDTO.ScheduleTime >= scheduleDTO.RecurEndDate)
                {
                    message = utilities.MessageUtils.getMessage(606);
                }
                decimal scheduleTime = new decimal(scheduleDTO.ScheduleTime.Hour + (((double)scheduleDTO.ScheduleTime.Minute) / 100));
                decimal scheduleEndTime = new decimal(scheduleDTO.ScheduleEndDate.Hour + (((double)scheduleDTO.ScheduleEndDate.Minute) / 100));
                if (scheduleTime >= scheduleEndTime)
                {
                    message = utilities.MessageUtils.getMessage(571);
                }
            }
            else
            {
                if (scheduleDTO.ScheduleTime.Date == scheduleDTO.ScheduleEndDate.Date)
                {
                    if (scheduleDTO.ScheduleTime > scheduleDTO.ScheduleEndDate)
                    {
                        message = utilities.MessageUtils.getMessage(571);
                    }
                }
            }
            log.LogMethodExit(message);
            return message;
        }

        private void rdbDaily_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            UpdateRecurFrequecyAndRecureType();
            log.LogMethodExit();
        }

        private void rdbWeekly_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            UpdateRecurFrequecyAndRecureType();
            log.LogMethodExit();
        }

        private void rdbMonthly_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            UpdateRecurFrequecyAndRecureType();
            log.LogMethodExit();
        }

        private void UpdateRecurFrequecyAndRecureType()
        {
            log.LogMethodEntry();
            if (rdbDaily.Checked || rdbWeekly.Checked)
            {
                rdbDay.Enabled = false;
                rdbDay.Checked = false;
                rdbWeekDay.Enabled = false;
                rdbWeekDay.Checked = false;
            }
            else if (rdbMonthly.Checked)
            {
                rdbDay.Enabled = true;
                rdbDay.Checked = true;
                rdbWeekDay.Enabled = true;
                rdbWeekDay.Checked = false;
            }
            dtpScheduleEndDate.MinDate = dtpScheduleDate.Value.Date;
            if (rdbDaily.Checked || rdbMonthly.Checked)
            {
                dtpScheduleEndDate.Enabled = false;
                dtpScheduleEndDate.Value = dtpScheduleDate.Value;
                dtpScheduleEndDate.MaxDate = dtpScheduleDate.MaxDate;
            }
            else if (rdbWeekly.Checked)
            {
                dtpScheduleEndDate.Enabled = true;
                dtpScheduleEndDate.Value = new DateTime(scheduleDTO.ScheduleEndDate.Year,
                                                        scheduleDTO.ScheduleEndDate.Month,
                                                        scheduleDTO.ScheduleEndDate.Day);
                dtpScheduleEndDate.MaxDate = dtpScheduleDate.Value.AddDays(6);
            }
            else
            {
                dtpScheduleEndDate.Enabled = true;
                dtpScheduleEndDate.Value = new DateTime(scheduleDTO.ScheduleEndDate.Year,
                                                        scheduleDTO.ScheduleEndDate.Month,
                                                        scheduleDTO.ScheduleEndDate.Day);
                dtpScheduleEndDate.MaxDate = dtpScheduleDate.MaxDate;
            }
            log.LogMethodExit();
        }

        private void dtpScheduleDate_ValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            dtpScheduleEndDate.MinDate = dtpScheduleDate.Value.Date;
            if (rdbDaily.Checked || rdbMonthly.Checked)
            {
                dtpScheduleEndDate.Enabled = false;
                dtpScheduleEndDate.Value = dtpScheduleDate.Value;
                dtpScheduleEndDate.MaxDate = dtpScheduleDate.MaxDate;
            }
            else if (rdbWeekly.Checked)
            {
                dtpScheduleEndDate.Enabled = true;
                dtpScheduleEndDate.MaxDate = dtpScheduleDate.Value.AddDays(6);
            }
            else
            {
                dtpScheduleEndDate.Enabled = true;
                dtpScheduleEndDate.MaxDate = dtpScheduleDate.MaxDate;
            }
            log.LogMethodExit();
        }

        private void UpdateUIElements()
        {
            log.LogMethodEntry();
            if (managementStudioSwitch.EnablProductModule)
            {
                btnSave.Enabled = true;
            }
            else
            {
                btnSave.Enabled = false;
            }
            log.LogMethodExit();
        }
    }
}
