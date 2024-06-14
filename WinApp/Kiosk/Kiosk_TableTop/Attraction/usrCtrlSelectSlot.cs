/********************************************************************************************
 * Project Name - Parafait_Kiosk
 * Description  - user control for select slot object in frmSelectSlot UI
 * 
 **************
 **Version Log
 **************
 *Version      Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.155.0.0    16-Jun-2023   Sathyavathi             Created for Attraction Sale in Kiosk
 *2.152.0.0    12-Dec-2023   Suraj Pai               Modified for Attraction Sale in TableTop Kiosk
 ********************************************************************************************/
using System;
using System.Drawing;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Product;
using System.Data.SqlClient;
using Semnox.Parafait.Languages;

namespace Parafait_Kiosk
{
    public partial class usrCtrlSlot : UserControl
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        internal delegate void SelctedSlotDelegate(ScheduleDetailsDTO selectedScheduleDetailsDTO);
        internal delegate void UnSelctSlotDelegate(ScheduleDetailsDTO selectedScheduleDetailsDTO);
        internal SelctedSlotDelegate selectedSlotMethod;
        internal UnSelctSlotDelegate unSelctSlotMethod;
        private ScheduleDetailsDTO scheduleDetails;
        private DateTime slotDate;
        private Semnox.Parafait.Transaction.AttractionBookingDTO selectedAttractionBookingdDTO;
        private string DATEFORMAT;

        public Image SetBackgroungImage { set { this.usrControlPanel.BackgroundImage = value; } }
        public bool IsSelected { get { return this.pbxSelectd.Visible; } set { this.pbxSelectd.Visible = value; } }

        public usrCtrlSlot(ExecutionContext executionContext, ScheduleDetailsDTO scheduleDetailsDTO, Semnox.Parafait.Transaction.AttractionBookingDTO selectedAttractionBookingdDTO)
        {
            log.LogMethodEntry(executionContext, scheduleDetailsDTO, selectedAttractionBookingdDTO);
            InitializeComponent();
            this.executionContext = executionContext;
            this.scheduleDetails = scheduleDetailsDTO;
            this.selectedAttractionBookingdDTO = selectedAttractionBookingdDTO;
            this.slotDate = KioskHelper.GetSlotDate(executionContext, this.scheduleDetails.ScheduleFromDate);

            DATEFORMAT = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATE_FORMAT");
            SetDisplayElements();
            SetCustomizedFontColors();
            log.LogMethodExit();
        }

        public void usrControl_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (IsSelected == false)
                {
                    if (selectedSlotMethod != null)
                    {
                        ScheduleDetailsDTO retValue = scheduleDetails;
                        selectedSlotMethod(retValue);
                        this.usrControlPanel.BackgroundImage = ThemeManager.CurrentThemeImages.SlotSelectedBackgroundImage;
                        IsSelected = true;
                    }
                }
                else
                {
                    if (unSelctSlotMethod != null)
                    {
                        ScheduleDetailsDTO retValue = scheduleDetails;
                        unSelctSlotMethod(retValue);
                        IsSelected = false;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in usrControl_Click() of usrControlFundsDonationsProduct : " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetDisplayElements()
        {
            log.LogMethodEntry();
            this.usrControlPanel.BackgroundImage = ThemeManager.CurrentThemeImages.SlotBackgroundImage;
            this.pbxSelectd.BackgroundImage = ThemeManager.CurrentThemeImages.CheckboxSelected;
            string slotDate = string.Empty;
            if (scheduleDetails.ScheduleFromTime != -1)
            {
                //convert fromTime from decimal to DateTime
                int fromHours = Decimal.ToInt32(scheduleDetails.ScheduleFromTime);
                int fromMinutes = (int)((scheduleDetails.ScheduleFromTime - fromHours) * 100);
                DateTime fromTime = this.scheduleDetails.ScheduleFromDate.Date.AddMinutes(fromHours * 60 + fromMinutes);

                bool showScheduleToTimeForAttraction = ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "SHOW_SCHEDULE_TO_TIME_FOR_ATTRACTION", true);
                if (showScheduleToTimeForAttraction)
                {
                    //convert toTime from decimal to DateTime
                    int toHours = Decimal.ToInt32(scheduleDetails.ScheduleToTime);
                    int toMinutes = (int)((scheduleDetails.ScheduleToTime - toHours) * 100);
                    DateTime toTime = this.scheduleDetails.ScheduleToDate.Date.AddMinutes(toHours * 60 + toMinutes);
                    lblScheduleTime.Text = fromTime.ToString("hh:mm tt").TrimStart('0') + " - " + toTime.ToString("hh:mm tt").TrimStart('0');
                }
                else
                {
                    lblScheduleTime.Text = fromTime.ToString("hh:mm tt").TrimStart('0');
                }

                slotDate = (this.slotDate.Date != this.scheduleDetails.ScheduleFromDate.Date
                                   ? "*[" + this.scheduleDetails.ScheduleFromDate.Date.ToString(DATEFORMAT) + "] - "
                                   : "");
            }
            bool hideAvailableSlots = ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "HIDE_AVAILABLE_SLOTS_FOR_ATTRACTION", false);
            lblSlotInfo.Text = slotDate + scheduleDetails.ScheduleName
                + (hideAvailableSlots ? "" :
                " | " + scheduleDetails.AvailableUnits.ToString() + " " + MessageContainerList.GetMessage(executionContext, "available")
                );

            if (selectedAttractionBookingdDTO != null && scheduleDetails.ScheduleId == selectedAttractionBookingdDTO.AttractionScheduleId
                && scheduleDetails.FacilityMapId == selectedAttractionBookingdDTO.FacilityMapId
                && scheduleDetails.ScheduleFromDate == selectedAttractionBookingdDTO.ScheduleFromDate)
            {
                this.usrControlPanel.BackgroundImage = ThemeManager.CurrentThemeImages.SlotSelectedBackgroundImage;
                IsSelected = true;
            }
            lblScheduleTime.ForeColor = KioskStatic.CurrentTheme.SelectSlotBtnDatesTextForeColor;
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            try
            {
                this.lblSlotInfo.ForeColor = KioskStatic.CurrentTheme.UsrCtrlSlotLblSlotInfo;
                this.lblScheduleTime.ForeColor = KioskStatic.CurrentTheme.UsrCtrlSlotLblScheduleTime;
            }
            catch (Exception ex)
            {
                log.Error("Error while Setting Customized Fore Colors for usrCtrlSlot", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements of usrCtrlSlot: " + ex.Message);
            }
            log.LogMethodExit();
        }
    }
}
