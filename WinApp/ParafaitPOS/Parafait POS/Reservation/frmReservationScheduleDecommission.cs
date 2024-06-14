using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;

namespace Parafait_POS.Reservation
{
    public partial class frmReservationScheduleDecommission : Form
    {
        //ParafaitUtils.Reservation.clsPackageList packageIdList;

        Utilities Utilities;
        // Utilities Utilities = POSStatic.Utilities;
        ParafaitEnv ParafaitEnv;
        MessageUtils MessageUtils;
        DataTable dtBookingSchedule;
        int bookingProductId = 0;
        public DateTime fromTime;
        public DateTime toTime;
        public string fixedSchedule = "";
        public int attractionScheduleId = -1;
        public double bookingProductPrice = 0.0;
        public int facilityId = -1;
        //Begin: Modified Added for logger function on 08-Mar-2016
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //End: Modified Added for logger function on 08-Mar-2016

        public frmReservationScheduleDecommission(int pbookingProductId, DateTime bookingDate, Utilities inUtilities)
        {
            //Begin: Added to Configure the logger root LogLevel using App.config on 08-March-2016           
            //log = ParafaitUtils.Logger.setLogFileAppenderName(log);
            Logger.setRootLogLevel(log);
            //End: Added to Configure the logger root LogLevel using App.config on 08-March-2016

            log.Debug("Starts-frmReservationSchedule(" + pbookingProductId + "," + bookingDate + ",inUtilities)");//Added for logger function on 08-Mar-2016
            bookingProductId = pbookingProductId;
            Utilities = inUtilities;

            ParafaitEnv = POSStatic.ParafaitEnv = Utilities.ParafaitEnv;
            MessageUtils = POSStatic.MessageUtils = Utilities.MessageUtils;
            ParafaitEnv.Initialize();
            InitializeComponent();
            dtpAttractionDate.Value = bookingDate;

            DataGridViewCellStyle Style = new DataGridViewCellStyle();
            Style.Font = new System.Drawing.Font(dgvSchedules.DefaultCellStyle.Font.Name, 10.0F, FontStyle.Bold);
            Style.Font = new System.Drawing.Font(dgvSchedules.ColumnHeadersDefaultCellStyle.Font.Name, 10.0F, FontStyle.Bold);
            dgvSchedules.DefaultCellStyle = Style;

            dgvSchedules.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSchedules.RowTemplate.Height = 30;
            dtpAttractionDate.MinDate = DateTime.Now;
            LoadSchedules();
            log.Debug("Ends-frmReservationSchedule(" + pbookingProductId + "," + bookingDate + ",inUtilities)");//Added for logger function on 08-Mar-2016
        }

        void LoadSchedules()
        {
            log.Debug("Starts-LoadSchedules()");//Added for logger function on 08-Mar-2016
            dtBookingSchedule = Utilities.executeDataTable(@"select p.product_id,product_name,p.MinimumTime,cf.FacilityName [Facility Name],ats.FacilityId, scheduleName [Schedule Name] ,ams.MasterScheduleName,
                                                          dateadd([mi], convert(int, ats.ScheduleTime)*60 + ats.ScheduleTime%1*100,@date) [From Time],
                                                            CASE WHEN ats.ScheduleTime is not null and ats.ScheduleToTime is not null and  ats.ScheduleTime > ats.ScheduleToTime THEN 
	                                                                 DATEADD(DAY,1, dateadd([mi], convert(int, ats.ScheduleToTime)*60 + ats.ScheduleToTime%1*100,@date))
                                                              ELSE
                                                                  dateadd([mi], convert(int, ats.ScheduleToTime)*60 + ats.ScheduleToTime%1*100,@date) end [To Time],
                                                                        ats.Fixed,
                                                                            case when isnull(RulePrice, 0) = 0 then
									                                        case when isnull(p.price, 0) = 0 then ap.price else p.price end else RulePrice end Price, case when isnull(RuleUnits, 0) = 0 then 
                                                                            case when isnull(p.AvailableUnits, 0) = 0 then cf.Capacity else p.AvailableUnits end else RuleUnits end [Total Units],
                                                                             ats.attractionScheduleId
                                                                            from products p ,
                                                                            (select ats.ScheduleName, ScheduleTime,ScheduleToTime,Fixed, AttractionPlayId, AvailableUnits, 
                                                                                    AttractionMasterScheduleId,attractionScheduleId, Price,ats.ActiveFlag, ats.FacilityId,
                                                                                (select top 1 Price 
                                                                                from AttractionScheduleRules, (select dateadd([mi], convert(int, ats.ScheduleTime)*60 + ats.ScheduleTime%1*100, @date) schDate) v
                                                                               where (v.schDate between FromDate and ToDate + 1
                                                                                    or DATEPART(weekday, v.schDate) = Day + 1
                                                                                    or DATEPART(DAY, v.schDate) = Day - 1000)
                                                                                    and AttractionScheduleId = ats.AttractionScheduleId) RulePrice,
                                                                                (select top 1 Units 
                                                                                from AttractionScheduleRules, (select dateadd([mi], convert(int, ats.ScheduleTime)*60 + ats.ScheduleTime%1*100, @date) schDate) v
                                                                               where (v.schDate between FromDate and ToDate + 1
                                                                                    or DATEPART(weekday, v.schDate) = Day + 1
                                                                                    or DATEPART(DAY, v.schDate) = Day - 1000)
                                                                                    and AttractionScheduleId = ats.AttractionScheduleId and ProductId = @product_Id ) RuleUnits
                                                                                from attractionschedules ats
										                                        ) ats,
                                                                        attractionPlays ap,CheckInFacility cf, AttractionMasterSchedule ams
                                                                        where ams.AttractionMasterScheduleId = p.AttractionMasterScheduleId 
								                                        and
								                                        ats.AttractionMasterScheduleId = ams.AttractionMasterScheduleId and
								                                         p.product_Id = @product_Id
								                                        and cf.FacilityId = ats.FacilityId
                                                                            and ap.AttractionPlayId = ats.AttractionPlayId
                                                                            and (ap.ExpiryDate >= @date or ap.ExpiryDate is null)
                                                                            and ats.ActiveFlag = 'Y'
                                                                       order by ats.scheduleTime",
                                                                       new SqlParameter("@product_Id", bookingProductId),
                                                                       new SqlParameter("@date", dtpAttractionDate.Value.Date),
                                                                       new SqlParameter("@dateAdd", dtpAttractionDate.Value.Date.AddDays(1)));

            dgvSchedules.DataSource = dtBookingSchedule;
            Utilities.setupDataGridProperties(ref dgvSchedules);
            dgvSchedules.Columns["product_id"].Visible =
            dgvSchedules.Columns["product_name"].Visible =
            dgvSchedules.Columns["Price"].Visible =
            dgvSchedules.Columns["MinimumTime"].Visible =
            dgvSchedules.Columns["MasterScheduleName"].Visible =
            dgvSchedules.Columns["AttractionScheduleId"].Visible =
            dgvSchedules.Columns["Total Units"].Visible =
            dgvSchedules.Columns["FacilityId"].Visible = false;

            // dgvSchedules.BorderStyle = BorderStyle.FixedSingle;
            DataGridViewCellStyle Expirestyle = new DataGridViewCellStyle();
            Expirestyle.BackColor = Color.LightGray;
            DataGridViewCellStyle style = new DataGridViewCellStyle();
            style.BackColor = Color.LightCyan;
            dgvSchedules.RowTemplate.DefaultCellStyle = style;
            Utilities.setupDataGridProperties(ref dgvSchedules);
            POSStatic.CommonFuncs.setupDataGridProperties(dgvSchedules);
            dgvSchedules.BackgroundColor = this.BackColor;
            DataGridViewCellStyle Style = new DataGridViewCellStyle();
            Style.Font = new System.Drawing.Font(dgvSchedules.DefaultCellStyle.Font.Name, 10.0F, FontStyle.Bold);
            dgvSchedules.DefaultCellStyle = Style;
            log.Debug("Ends-LoadSchedules()");//Added for logger function on 08-Mar-2016
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnNext_Click()");//Added for logger function on 08-Mar-2016
            dtpAttractionDate.Value = dtpAttractionDate.Value.AddDays(1);
            log.Debug("Ends-btnNext_Click()");//Added for logger function on 08-Mar-2016
        }

        private void dtpAttractionDate_ValueChanged(object sender, EventArgs e)
        {
            log.Debug("Starts-dtpAttractionDate_ValueChanged()");//Added for logger function on 08-Mar-2016
            LoadSchedules();
            log.Debug("Ends-dtpAttractionDate_ValueChanged()");//Added for logger function on 08-Mar-2016
        }

        private void dgvSchedules_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.Debug("Starts-dgvSchedules_CellClick()");//Added for logger function on 08-Mar-2016
            if (e.RowIndex < 0)
            {
                log.Info("Ends-dgvSchedules_CellClick() as e.RowIndex < 0");//Added for logger function on 08-Mar-2016
                return;
            }

            if (dgvSchedules.Rows[e.RowIndex].Cells["product_id"].Value != DBNull.Value)
            {
                fromTime = Convert.ToDateTime(dgvSchedules["From Time", e.RowIndex].Value);
                toTime = dgvSchedules["To Time", e.RowIndex].Value == DBNull.Value ? DateTime.MaxValue : Convert.ToDateTime(dgvSchedules["To Time", e.RowIndex].Value);
                fixedSchedule = (dgvSchedules["Fixed", e.RowIndex].Value.ToString());
                attractionScheduleId = Convert.ToInt32(dgvSchedules["AttractionScheduleId", e.RowIndex].Value.ToString());
                bookingProductPrice = dgvSchedules["Price", e.RowIndex].Value == DBNull.Value ? 0.0 : Convert.ToDouble(dgvSchedules["Price", e.RowIndex].Value.ToString());
                facilityId = Convert.ToInt32(dgvSchedules["FacilityId", e.RowIndex].Value.ToString());//Added to send facility Id as parameter to get check availablity on Dec-07-2015//
            }
            log.Debug("Ends-dgvSchedules_CellClick()");//Added for logger function on 08-Mar-2016
        }

        private void btnScheduleOk_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnScheduleOk_Click()");//Added for logger function on 08-Mar-2016
            DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
            log.Debug("Ends-btnScheduleOk_Click()");//Added for logger function on 08-Mar-2016
        }
    }
}
