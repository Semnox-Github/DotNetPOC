/* Project Name - AttractionSchedules
* Description  - AttractionSchedules form
* 
**************
**Version Log
**************
*Version     Date           Modified By          Remarks          
********************************************************************************************* 
*2.70.0      17-Apr-2019    Guru S A             Modified for Booking phase 2 enhancement changes 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using Semnox.Parafait.Transaction;

namespace Parafait_POS
{
    public partial class AttractionSchedulesOld : Form
    {
        Utilities Utilities = POSStatic.Utilities;
        int productId, Quantity;
        int facilityMapId = -1;
        //public class clsReturnDataLine
        //{
        //    public int BookedUnits;
        //    public string PlayName;
        //    public string ScheduleName;

        //    public int AttractionPlayId;
        //    public int FacilityId;
        //    public string AttractionPlayName;
        //    public int AttractionScheduleId;
        //    public DateTime ScheduleTime;
        //    public decimal ScheduleFromTime;
        //    public decimal ScheduleToTime;
        //    public int AvailableUnits;
        //    public double Price;
        //    public DateTime ExpiryDate;
        //    public int PromotionId;
        //    public List<int> SelectedSeats;
        //    public List<string> SelectedSeatNames;
        //}

        //To be passed from Booking
        string excludeAttractionSchedule = "-1";
        bool bookingRequest = false;
        DateTime attractionDate;
        //End 08-Apr
        Card inCard;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        NumberPad numPad;
        bool _scheduleExist = false;
        public bool ScheduleExist
        {
            get
            {
                return _scheduleExist;
            }
        }

        public AttractionSchedulesOld(Card pInCard, int pProductId, int pQuantity)
        { 
            Logger.setRootLogLevel(log);  
            log.LogMethodEntry( pInCard , pProductId , pQuantity);
            productId = pProductId;
            Quantity = pQuantity;
            inCard = pInCard;
            //Products productsBL = new Products(ProductId);
            this.facilityMapId = -1;// productsBL.GetProductsDTO.FacilityMapId;
            Init();
            log.LogMethodExit();
        }

        //New constructor to take attraction date and exclusion schedule ids as input
        public AttractionSchedulesOld(Card pInCard, int pProductId, int pQuantity, DateTime pattractionDate, string pexcludeAttractionSchedule, int pfacilityMapId = -1)
        {
            log.LogMethodEntry(pInCard, pProductId, pQuantity, pattractionDate, pexcludeAttractionSchedule);
            productId = pProductId;
            Quantity = pQuantity;
            inCard = pInCard;
            attractionDate = pattractionDate;
            bookingRequest = true;
            excludeAttractionSchedule = pexcludeAttractionSchedule;
            this.facilityMapId = pfacilityMapId;
            //if(this.facilityMapId == -1)
            //{
               // Products productsBL = new Products(ProductId);
                //this.facilityMapId = productsBL.GetProductsDTO.FacilityMapId;
            //}
            Init();
            log.LogMethodExit();
        }

        void Init()
        {
            log.LogMethodEntry();
            Utilities.setLanguage();
            InitializeComponent();
            this.Width = Screen.PrimaryScreen.WorkingArea.Width;
            ((Parafait_POS.POS)Application.OpenForms["POS"]).LoadMasterScheduleBLList(); 
            GetAttractionFacilities();

            Utilities.setupDataGridProperties(ref dgvAttractionSchedules);
            POSStatic.CommonFuncs.setupDataGridProperties(dgvAttractionSchedules);
            dgvAttractionSchedules.BackgroundColor = this.BackColor;

            DataGridViewCellStyle Style = new DataGridViewCellStyle();
            Style.Font = new System.Drawing.Font(dgvAttractionSchedules.DefaultCellStyle.Font.Name, 10.0F, FontStyle.Bold);
            dgvAttractionSchedules.DefaultCellStyle = Style;

            dgvAttractionSchedules.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAttractionSchedules.RowTemplate.Height = 28;
            if (bookingRequest)
            {
                dtpAttractionDate.Enabled = false;
                btnNext.Enabled = false;
                btnPrev.Enabled = false;
                chkShowPast.Enabled = false;
                dtpAttractionDate.Value = attractionDate; 
            }
            else
            {
                if (this.facilityMapId > -1)
                {
                    cmbAttractionFacility.SelectedValue = this.facilityMapId;
                }
            }
           // cmbAttractionFacility.Enabled = false;
            this.cmbAttractionFacility.SelectedIndexChanged += new System.EventHandler(this.cmbAttractionFacility_SelectedIndexChanged);

            lblQuantity.Text = Quantity.ToString();

            //DataTable dt = getData();
            //if (dt.Rows.Count > 0)
            //    _scheduleExist = true;
            List<ScheduleDetailsDTO> scheduleDetailsTOList = GetScheduleData();

            if(scheduleDetailsTOList != null && scheduleDetailsTOList.Count > 0)
            {
                _scheduleExist = true;
            }

            log.LogMethodExit();
        }

        void GetAttractionFacilities()
        {
            log.LogMethodEntry();
            FacilityMapListBL facilityMapListBL = new FacilityMapListBL(Utilities.ExecutionContext);
            List<KeyValuePair<FacilityMapDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<FacilityMapDTO.SearchByParameters, string>>();
            searchParams.Add(new KeyValuePair<FacilityMapDTO.SearchByParameters, string>(FacilityMapDTO.SearchByParameters.SITE_ID, Utilities.ExecutionContext.GetSiteId().ToString()));
            searchParams.Add(new KeyValuePair<FacilityMapDTO.SearchByParameters, string>(FacilityMapDTO.SearchByParameters.IS_ACTIVE, "1"));
            searchParams.Add(new KeyValuePair<FacilityMapDTO.SearchByParameters, string>(FacilityMapDTO.SearchByParameters.HAVING_PRODUCT_TYPES_IN,ProductTypeValues.ATTRACTION));
            searchParams.Add(new KeyValuePair<FacilityMapDTO.SearchByParameters, string>(FacilityMapDTO.SearchByParameters.ALLOWED_PRODUCT_ID, productId.ToString()));
            List<FacilityMapDTO> facilityMapDTOList = facilityMapListBL.GetFacilityMapDTOList(searchParams);
            if(facilityMapDTOList == null || facilityMapDTOList.Count == 0)
            {
                facilityMapDTOList = new List<FacilityMapDTO>();
            }
            FacilityMapDTO facilityMapDTO = new FacilityMapDTO
            {
                FacilityMapName = "-All -"
            };
            facilityMapDTOList.Insert(0, facilityMapDTO);
            facilityMapDTOList = facilityMapDTOList.OrderBy(fac => fac.FacilityMapName).ToList();
            cmbAttractionFacility.DataSource = facilityMapDTOList;// Utilities.executeDataTable("select facilityId, FacilityName from CheckInFacility where active_Flag = 'Y' union all select -1, '- All -' order by 2");
            cmbAttractionFacility.DisplayMember = "FacilityMapName";
            cmbAttractionFacility.ValueMember = "facilityMapId";
            cmbAttractionFacility.SelectedIndex = 0; 


            log.LogMethodExit();
        }

        private void cmbAttractionFacility_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            RefreshGrid();
            log.LogMethodExit();
        }

        private void AttractionSchedules_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Timer delayed = new Timer();
            delayed.Interval = 50;
            delayed.Tick += new EventHandler(delayed_Tick);
            delayed.Start();
            POSStatic.Utilities.setLanguage(this);
            log.LogMethodExit();
        }

        void delayed_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ((sender) as Timer).Stop();

            this.dgvAttractionSchedules.SelectionChanged += new System.EventHandler(this.dgvAttractionSchedules_SelectionChanged);

            numPad = new NumberPad(POSStatic.Utilities.ParafaitEnv.NUMBER_FORMAT, 0);
            numPad.NewEntry = true;

            Panel NumberPadVarPanel = numPad.NumPadPanel();
            NumberPadVarPanel.Location = new System.Drawing.Point(0, 0);
            numPadPanel.Controls.Add(NumberPadVarPanel);
            numPad.setReceiveAction = EventnumPadOKReceived;

            RefreshGrid();

            this.dgvAttractionSchedules.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAttractionSchedules_CellValidated);

            this.KeyPreview = true;

            this.KeyPress += new KeyPressEventHandler(formKeyPress);
            log.LogMethodExit();
        }

        private void EventnumPadOKReceived()
        {
            log.LogMethodEntry();
            int qty = Math.Max(0, (int)numPad.ReturnNumber);
            if (dgvAttractionSchedules.SelectedRows.Count > 0)
            {
                DataGridViewRow dr = dgvAttractionSchedules.SelectedRows[0];
                if (dr.Cells["ExpiryDate"].ReadOnly == false)
                {
                    dr.Cells["DesiredUnits"].Value = qty;
                    int totalQty = GetAllocatedQuantity();
                    if (qty > Convert.ToInt32(dr.Cells["AvailableUnits"].Value))
                    {
                        POSUtils.ParafaitMessageBox(Utilities.MessageUtils.getMessage(2)); //New Message Box UI - 05-Mar-2016

                        int neededQty = Quantity - (totalQty - qty);
                        dr.Cells["DesiredUnits"].Value = Math.Min(neededQty, Convert.ToInt32(dr.Cells["AvailableUnits"].Value));
                    }

                    dgvAttractionSchedules.EndEdit();

                    lblAllocatedQty.Text = totalQty.ToString();
                    if (totalQty != Quantity)
                        lblAllocatedQty.ForeColor = Color.Red;
                    else
                    {
                        lblAllocatedQty.ForeColor = Color.Black;
                        //btnOK.PerformClick();
                        if (SeatSelectionIsDone())
                        {
                            ValidateAndProceed();
                        }
                    }

                    try
                    {
                        int nextEditableRow = -1;
                        for (int i = dr.Index + 1; i < dgvAttractionSchedules.Rows.Count; i++)
                        {
                            if (dgvAttractionSchedules["ExpiryDate", i].ReadOnly == false && Convert.ToInt32(dgvAttractionSchedules["AvailableUnits", i].Value) > 0)
                            {
                                nextEditableRow = i;
                                break;
                            }
                        }
                        if (nextEditableRow != -1)
                            dgvAttractionSchedules.Rows[nextEditableRow].Selected = true;
                    }
                    catch (Exception ex){ log.Error(ex); }
                }
            }
            log.LogMethodExit();
        }

        private bool SeatSelectionIsDone()
        {
            log.LogMethodEntry();
            bool seatsAreSelected = true;
            if (dgvAttractionSchedules.SelectedRows.Count > 0)
            {
                DataGridViewRow dr = dgvAttractionSchedules.SelectedRows[0];
                if (dr.Cells["ExpiryDate"].ReadOnly == false)
                {
                    int currentFacilityMapId = -1;
                    try
                    {
                        currentFacilityMapId = Convert.ToInt32(dr.Cells["FacilityMapId"].Value);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                    if (dr.Cells["Seats"].Tag == null && currentFacilityMapId > -1)
                    {
                        FacilityMapBL facilityMapBL = new FacilityMapBL(Utilities.ExecutionContext, currentFacilityMapId);
                        List<FacilityDTO> facilityDTOList = facilityMapBL.GetMappedFacilityDTOList();
                        if (facilityDTOList != null && facilityDTOList.Any())
                        {
                            for (int i = 0; i < facilityDTOList.Count; i++)
                            {
                                FacilitySeatsBL facilitySeatsBL = new FacilitySeatsBL(Utilities.ExecutionContext);
                                List<FacilitySeatLayoutDTO> facilitySeatLayoutDTOList = facilitySeatsBL.GetFacilitySeatLayout(facilityDTOList[i].FacilityId);
                                if (facilitySeatLayoutDTOList != null && facilitySeatLayoutDTOList.Count > 0)
                                {
                                    seatsAreSelected = false;
                                    break;
                                }
                            }
                           
                        }
                    }
                }
            }
            log.LogMethodExit(seatsAreSelected);
            return seatsAreSelected;
        }

        void formKeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
            else
            {
                if (dgvAttractionSchedules.CurrentRow != null)
                {
                    DataGridViewRow dr = dgvAttractionSchedules.CurrentRow;
                    if (dr.Cells["ExpiryDate"].IsInEditMode == false)
                        numPad.GetKey(e.KeyChar);
                }
            }
            log.LogMethodExit();
        }

        List<ScheduleDetailsDTO> GetScheduleData()
        {
            log.LogMethodEntry();
            //int facilityMapId = -1;
            if(cmbAttractionFacility.SelectedValue !=  null ) // && bookingRequest == false)
            {
                this.facilityMapId = Convert.ToInt32(cmbAttractionFacility.SelectedValue);
            }
            //if(bookingRequest)
            //{
            //    facilityMapId = this.facilityMapId;
            //}
            List<ScheduleDetailsDTO> scheduleDetailsDTOList = ((Parafait_POS.POS)Application.OpenForms["POS"]).GetEligibleSchedules(dtpAttractionDate.Value.Date, 0, 24, this.facilityMapId, productId);
            log.LogMethodExit(scheduleDetailsDTOList);
            return scheduleDetailsDTOList;
        }
        //DataTable getData()
        //{
        //    log.LogMethodEntry();
        //    //Modified the query to get the attraction schedules//
        //    string commandText = @"SELECT scheduleName [Schedule Name],
        //                                dateadd([mi], convert(int, ats.ScheduleTime)*60 + ats.ScheduleTime%1*100, @date) [Schedule Time],
        //                                PlayName [Play Name],
        //                                CASE
        //                                    WHEN isnull(RulePrice, 0) = 0 THEN CASE
        //                                                                            WHEN isnull(p.price, 0) = 0 THEN ap.price
        //                                                                            ELSE p.price
        //                                                                        END
        //                                    ELSE RulePrice
        //                                END Price,
        //                                CASE
        //                                    WHEN isnull(RuleUnits, 0) = 0 
        //                                    THEN (CASE WHEN ISNULL(p.AvailableUnits,0) = 0 then isnull(ats.Capacity,0)
								//						ELSE ISNULL(p.AvailableUnits,0)
								//						END)
        //                                    ELSE ISNULL(RuleUnits,0)
        //                                END [Total Units],
        //                                BookedUnits [Booked Units],
        //                                NULL [Available Units],
        //                                    NULL [Desired Units],
        //                                            ats.attractionScheduleId,
        //                                            ats.attractionPlayId,
								//					ats.ScheduleFromTime,
								//					ats.ScheduleToTime,
        //                                            convert(DateTime, NULL) [Expiry Date],
        //                                            p.CateGoryId,
        //                                            -1 promotionId,
        //                                            NULL Seats
        //                        FROM products p,

        //                            (SELECT product_Id,
        //                                    scheduleName,
        //                                    ScheduleTime,
        //                                    AttractionPlayId,
        //                                    p.AvailableUnits,
								//			cf.Capacity,
        //                                    p.AttractionMasterScheduleId,
        //                                    attractionScheduleId,
        //                                    p.Price,
        //                                    ats.FacilityId,
								//			ats.ScheduleTime ScheduleFromTime, ats.ScheduleToTime,
        //                                (SELECT top 1 Price
        //                                FROM AttractionScheduleRules,

        //                                (SELECT dateadd([mi], convert(int, ats.ScheduleTime)*60 + ats.ScheduleTime%1*100, @date) schDate) v
        //                                WHERE (v.schDate BETWEEN FromDate AND ToDate + 1
        //                                        OR DATEPART(weekday, v.schDate) = DAY + 1
        //                                        OR DATEPART(DAY, v.schDate) = DAY - 1000)
        //                                AND AttractionScheduleId = ats.AttractionScheduleId
        //                                AND ProductId = @product_Id) RulePrice,

        //                                (SELECT top 1 Units
        //                                FROM AttractionScheduleRules,

        //                                (SELECT dateadd([mi], convert(int, ats.ScheduleTime)*60 + ats.ScheduleTime%1*100, @date) schDate) v
        //                                WHERE (v.schDate BETWEEN FromDate AND ToDate + 1
        //                                        OR DATEPART(weekday, v.schDate) = DAY + 1
        //                                        OR DATEPART(DAY, v.schDate) = DAY - 1000)
        //                                AND AttractionScheduleId = ats.AttractionScheduleId
        //                                AND ProductId = @product_Id) RuleUnits
        //                            FROM attractionschedules ats,products p, CheckInFacility cf
        //                        where ats.AttractionMasterScheduleId=p.AttractionMasterScheduleId 
        //                            and ats.activeFlag='Y'
								//	and ats.FacilityId = cf.FacilityId
								//	and cf.active_flag = 'Y'
        //                            and p.product_id = @product_Id) ats
        //                        LEFT OUTER JOIN
        //                            (SELECT atb.attractionScheduleId,
        //                                    sum((CASE WHEN atb.expiryDate IS NULL THEN BookedUnits WHEN atb.expiryDate< getdate() THEN 0 ELSE BookedUnits END)) bookedUnits
        //                            FROM attractionschedules ats,
        //                                attractionBookings atb,
        //                                products p
        //                            WHERE p.AttractionMasterScheduleId = ats.AttractionMasterScheduleId
        //                                AND atb.attractionScheduleId = ats.attractionScheduleId
        //                                AND p.product_id = @product_Id
        //                                AND atb.ScheduleTime >= @date
        //                                AND atb.ScheduleTime < @date +1
        //                            GROUP BY atb.attractionScheduleId) bookings ON bookings.attractionScheduleId = ats.attractionScheduleId,
        //                               attractionPlays ap
        //                        WHERE p.AttractionMasterScheduleId = ats.AttractionMasterScheduleId
        //                            AND p.product_Id = @product_Id
        //                            AND (ats.FacilityId = @facilityId
        //                                OR @facilityId = -1)
        //                            AND ap.AttractionPlayId = ats.AttractionPlayId
        //                            AND (ap.ExpiryDate >= @date
        //                                OR ap.ExpiryDate IS NULL)
        //                        AND ats.attractionScheduleId NOT IN (" + excludeAttractionSchedule + ") " +
        //                        "ORDER BY ats.scheduleTime";

        //    return Utilities.executeDataTable(commandText,
        //                                        new SqlParameter("@product_Id", ProductId),
        //                                        new SqlParameter("@date", dtpAttractionDate.Value.Date),
        //                                        new SqlParameter("@facilityId", cmbAttractionFacility.SelectedValue));
        //}

        void RefreshGrid()
        {
            log.LogMethodEntry();
            //DataTable dt = getData();
            List<ScheduleDetailsDTO> scheduleDetailsDTOList = GetScheduleData();
            lblAllocatedQty.Text = "0";
            lblAllocatedQty.ForeColor = Color.Red;

            //if (dt.Rows.Count == 0)
            //{
            //    dgvAttractionSchedules.DataSource = dt;
            //    log.Debug("Ends-refreshGrid() as dt.Rows.Count == 0");
            //    return;
            //}

            if(scheduleDetailsDTOList == null || scheduleDetailsDTOList.Count == 0)
            {
                dgvAttractionSchedules.DataSource = scheduleDetailsDTOList;
                log.LogMethodExit("scheduleSummaryDTOList == null || scheduleSummaryDTOList.Count == 0");
                return;
            }

            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    double Price = (dt.Rows[i]["Price"] == DBNull.Value ? 0 : Convert.ToDouble(dt.Rows[i]["Price"]));
            //    int PromotionId = Promotions.getProductPromotionPrice(inCard, ProductId, dt.Rows[0]["CategoryId"], "N", 0, ref Price, Utilities);

            //    dt.Rows[i]["Price"] = Price;
            //    dt.Rows[i]["promotionId"] = PromotionId;

            //    int totalUnits = 0;
            //    if (dt.Rows[i]["Total Units"] != DBNull.Value)
            //        totalUnits = Convert.ToInt32(dt.Rows[i]["Total Units"]);

            //    int bookedUnits = 0;
            //    if (dt.Rows[i]["Booked Units"] != DBNull.Value)
            //        bookedUnits = Convert.ToInt32(dt.Rows[i]["Booked Units"]);
            //    dt.Rows[i]["Available Units"] = totalUnits - bookedUnits;
            //}

            //if (chkShowPast.Checked == false)
            //{
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        if (Convert.ToDateTime(dt.Rows[i]["Schedule Time"]) < (bookingRequest ? attractionDate : DateTime.Now.AddMinutes(POSStatic.ATTRACTION_BOOKING_GRACE_PERIOD)))
            //        {
            //            dt.Rows.RemoveAt(i);
            //            i = -1;
            //        }
            //    }
            //}
            if (chkShowPast.Checked == false)
            {
                for (int i = 0; i < scheduleDetailsDTOList.Count; i++)
                {
                    if (scheduleDetailsDTOList[i].ScheduleFromDate < (bookingRequest ? attractionDate : Utilities.getServerTime().AddMinutes(POSStatic.ATTRACTION_BOOKING_GRACE_PERIOD)))
                    {
                        scheduleDetailsDTOList.RemoveAt(i);
                        i = -1;
                    }
                }
            }
            for (int i = 0; i < scheduleDetailsDTOList.Count; i++)
            {
                double Price = (scheduleDetailsDTOList[i].Price ==null ? 0 : (double)scheduleDetailsDTOList[i].Price);
                int PromotionId = Promotions.getProductPromotionPrice(inCard, productId, scheduleDetailsDTOList[i].CategoryId, "N", 0, ref Price, Utilities, scheduleDetailsDTOList[i].ScheduleFromDate);

                scheduleDetailsDTOList[i].Price = Price;
                scheduleDetailsDTOList[i].PromotionId = PromotionId;

                int? totalUnits = 0;
                totalUnits = scheduleDetailsDTOList[i].TotalUnits;

                int bookedUnits = 0;
                //bookedUnits = (scheduleDetailsDTOList[i].BookedUnits == null ? 0: (int) scheduleDetailsDTOList[i].BookedUnits);
                FacilityMapBL facilityMapBL = new FacilityMapBL(Utilities.ExecutionContext, scheduleDetailsDTOList[i].FacilityMapDTO);
                bookedUnits = facilityMapBL.GetTotalBookedUnitsForAttraction(scheduleDetailsDTOList[i].ScheduleFromDate, scheduleDetailsDTOList[i].ScheduleToDate);
                if (bookedUnits > 0)
                {
                    scheduleDetailsDTOList[i].BookedUnits = bookedUnits;
                }
                if (totalUnits < 0)
                {
                    scheduleDetailsDTOList[i].AvailableUnits = 0;
                }
                else
                {
                    scheduleDetailsDTOList[i].AvailableUnits = totalUnits - bookedUnits;
                }
            }
           
            if(scheduleDetailsDTOList != null && scheduleDetailsDTOList.Count > 0)
            {
                scheduleDetailsDTOList = scheduleDetailsDTOList.OrderBy(sch => sch.ScheduleFromDate).ToList();
            }
            dgvAttractionSchedules.DataSource = scheduleDetailsDTOList;
            dgvAttractionSchedules.Columns["CategoryId"].Visible = false;
            dgvAttractionSchedules.Columns["promotionId"].Visible = false;
            dgvAttractionSchedules.Columns["Seats"].Visible = false;
            dgvAttractionSchedules.Columns["ScheduleToDate"].Visible = false;
            dgvAttractionSchedules.Columns["ScheduleFromTime"].Visible = false;
            dgvAttractionSchedules.Columns["ScheduleToTime"].Visible = false;
            dgvAttractionSchedules.Columns["FacilityMapId"].Visible = false;
            dgvAttractionSchedules.Columns["FacilityMapName"].Visible = true;
            dgvAttractionSchedules.Columns["MasterScheduleId"].Visible = false;
            dgvAttractionSchedules.Columns["MasterScheduleName"].Visible = false; 
            dgvAttractionSchedules.Columns["FixedSchedule"].Visible = false;
            dgvAttractionSchedules.Columns["facilityCapacity"].Visible = false;
            dgvAttractionSchedules.Columns["ruleUnits"].Visible = false;
            //dgvAttractionSchedules.Columns["productLevelUnits"].Visible = false;
            dgvAttractionSchedules.Columns["productId"].Visible = false;
            dgvAttractionSchedules.Columns["productName"].Visible = false; 
            //dgvAttractionSchedules.Columns["siteId"].Visible = false;
            dgvAttractionSchedules.Columns["FacilityMapDTO"].Visible = false;
            dgvAttractionSchedules.Columns["AttractionPlayPrice"].Visible = false;
            //dgvAttractionSchedules.Columns["AttractionPlayExpiryDate"].Visible = false;   
            dgvAttractionSchedules.Columns["DesiredUnits"].ReadOnly = true;

            int editRowIndex = -1;
            DataGridViewCellStyle Expirestyle = new DataGridViewCellStyle();
            Expirestyle.BackColor = Color.LightGray;
            DataGridViewCellStyle style = new DataGridViewCellStyle();
            style.BackColor = Color.LightCyan;
            DataGridViewCellStyle editStyle = new DataGridViewCellStyle();
            editStyle.SelectionBackColor = editStyle.BackColor = Color.LightGoldenrodYellow;
            DataGridViewCellStyle editStyleDate = new DataGridViewCellStyle();
            editStyleDate.SelectionBackColor = editStyleDate.BackColor = Color.White;

            dgvAttractionSchedules.RowTemplate.DefaultCellStyle = style;

            for (int i = 0; i < dgvAttractionSchedules.Rows.Count; i++)
            {
                try
                {
                    int avlUnits = Convert.ToInt32(dgvAttractionSchedules["AvailableUnits", i].Value);
                    if (Convert.ToDateTime(dgvAttractionSchedules["scheduleFromDate", i].Value) < DateTime.Now.AddMinutes(POSStatic.ATTRACTION_BOOKING_GRACE_PERIOD) || avlUnits <= 0)
                    {
                        dgvAttractionSchedules.Rows[i].DefaultCellStyle = Expirestyle;
                        dgvAttractionSchedules["ExpiryDate", i].ReadOnly = true;
                    }
                    else
                    {
                        if (editRowIndex == -1) editRowIndex = i;
                        dgvAttractionSchedules["DesiredUnits", i].Style = editStyle;
                        dgvAttractionSchedules["ExpiryDate", i].Style = editStyleDate;
                    }
                }
                catch (Exception ex)
                {
                    log.Fatal("Ends-refreshGrid() due to exception " + ex.Message);
                }
            }

            dgvAttractionSchedules.Columns["Price"].DefaultCellStyle = Utilities.gridViewAmountCellStyle();
            dgvAttractionSchedules.Columns["scheduleFromDate"].DefaultCellStyle = Utilities.gridViewDateTimeCellStyle();
            dgvAttractionSchedules.Columns["scheduleToDate"].DefaultCellStyle = Utilities.gridViewDateTimeCellStyle();
            dgvAttractionSchedules.Columns["ExpiryDate"].HeaderText = "Booking Expiry";
            dgvAttractionSchedules.Columns["ExpiryDate"].DefaultCellStyle.Format = Utilities.gridViewDateTimeCellStyle().Format;
            dgvAttractionSchedules.EditMode = DataGridViewEditMode.EditOnEnter;

            for (int i = 0; i < 8; i++)
            {
                dgvAttractionSchedules.Columns[i].ReadOnly = true;
            }

            for (int i = 0; i < dgvAttractionSchedules.Columns.Count; i++)
            {
                dgvAttractionSchedules.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            dgvAttractionSchedules.Columns["ExpiryDate"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvAttractionSchedules.Columns["ExpiryDate"].Width = 180;
            dgvAttractionSchedules.Columns["ScheduleId"].Visible = false;
            dgvAttractionSchedules.Columns["attractionPlayId"].Visible = false;

            dgvAttractionSchedules.Columns["AvailableUnits"].DefaultCellStyle.Alignment =
            dgvAttractionSchedules.Columns["DesiredUnits"].DefaultCellStyle.Alignment =
            dgvAttractionSchedules.Columns["BookedUnits"].DefaultCellStyle.Alignment =
            dgvAttractionSchedules.Columns["TotalUnits"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            this.ActiveControl = dgvAttractionSchedules;
            dgvAttractionSchedules.TabIndex = 0;
            if (editRowIndex != -1 && editRowIndex < dgvAttractionSchedules.Rows.Count)
            {
                dgvAttractionSchedules.CurrentCell = dgvAttractionSchedules["DesiredUnits", editRowIndex];
            }
            dgvAttractionSchedules_SelectionChanged(null, null);
            dgvAttractionSchedules.EndEdit();
            dgvAttractionSchedules.RowsDefaultCellStyle.SelectionBackColor = Color.YellowGreen;

            dgvAttractionSchedules.Columns["PickSeats"].DisplayIndex = dgvAttractionSchedules.Columns["ExpiryDate"].DisplayIndex - 1;
            PickSeats.Image = Properties.Resources.GreenSeat;

            Utilities.setLanguage(dgvAttractionSchedules);
            dgvAttractionSchedules.Refresh();
            log.LogMethodExit();
        }

        private void dtpAttractionDate_ValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (!bookingRequest) //In case of booking, date picker cannot be changed so no need to refresh grid in this event
                RefreshGrid();
            log.LogMethodExit();
        }

        private void dgvAttractionSchedules_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            log.LogVariableState("e.RowIndex", e.RowIndex);
            if (e.RowIndex < 0)
            {
                log.LogMethodExit("e.RowIndex < 0");
                return;
            }
            if (dgvAttractionSchedules.CurrentCell != null && dgvAttractionSchedules.Columns[e.ColumnIndex].Name == "DesiredUnits")
            {
                if (dgvAttractionSchedules["DesiredUnits", e.RowIndex].Value == null || dgvAttractionSchedules["DesiredUnits", e.RowIndex].Value == DBNull.Value)
                {
                    log.LogMethodExit("dgvAttractionSchedules['DesiredUnits', e.RowIndex].Value == null");
                    return;
                }

                int qty = 0;
                try
                {
                    if (dgvAttractionSchedules["DesiredUnits", e.RowIndex].Value != DBNull.Value)
                        qty = Convert.ToInt32(dgvAttractionSchedules["DesiredUnits", e.RowIndex].Value);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    POSUtils.ParafaitMessageBox(Utilities.MessageUtils.getMessage(1)); //New Message Box UI - 05-Mar-2016
                    dgvAttractionSchedules[e.ColumnIndex, e.RowIndex].Value = 0;
                    log.LogMethodExit("Due to exception Invalid value for Desired Units ");
                    return;
                }

                int totalQty = GetAllocatedQuantity();
                lblAllocatedQty.Text = totalQty.ToString();
                if (totalQty != Quantity)
                    lblAllocatedQty.ForeColor = Color.Red;
                else
                    lblAllocatedQty.ForeColor = Color.Black;

                if (totalQty > Quantity || qty > Convert.ToInt32(dgvAttractionSchedules["AvailableUnits", e.RowIndex].Value))
                {
                    POSUtils.ParafaitMessageBox(Utilities.MessageUtils.getMessage(2)); //New Message Box UI - 05-Mar-2016

                    int neededQty = Quantity - (totalQty - qty);
                    dgvAttractionSchedules[e.ColumnIndex, e.RowIndex].Value = Math.Min(neededQty, Convert.ToInt32(dgvAttractionSchedules["AvailableUnits", e.RowIndex].Value));
                    log.Info("Desired units cannot be more than Available units ");
                }
            }
            log.LogMethodExit();
        }

        private void dgvAttractionSchedules_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.ParafaitMessageBox(Utilities.MessageUtils.getMessage(3, e.RowIndex + 1, e.Exception.Message));  //New Message Box UI - 05-Mar-2016
            log.Info("Error in grid data at row");
            log.Error(e.Exception);
            e.Cancel = true;
            log.LogMethodExit();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            dtpAttractionDate.Value = dtpAttractionDate.Value.AddDays(-1);
            log.LogMethodExit();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            dtpAttractionDate.Value = dtpAttractionDate.Value.AddDays(1);
            log.LogMethodExit();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ValidateAndProceed();
            log.LogMethodExit();
        }

        private void ValidateAndProceed()
        {
            log.LogMethodEntry();
            if (this.Validate() && this.ValidateChildren())
            {
                int allocQty = GetAllocatedQuantity();
                if (allocQty != Quantity)
                {
                    POSUtils.ParafaitMessageBox(Utilities.MessageUtils.getMessage(1));
                    log.LogMethodExit("allocQty != Quantity");
                    return;
                }

                DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            log.LogMethodExit();
        }
        void ShowSeats()
        {
            log.LogMethodEntry(); 
            using (AttractionSeatBooking asb = new AttractionSeatBooking(Convert.ToInt32(dgvAttractionSchedules.CurrentRow.Cells["ScheduleId"].Value),
                                                                         Convert.ToInt32(dgvAttractionSchedules.CurrentRow.Cells["FacilityMapId"].Value),
                                                                         Convert.ToDateTime(dgvAttractionSchedules.CurrentRow.Cells["ScheduleFromDate"].Value),
                                                                          Convert.ToDateTime(dgvAttractionSchedules.CurrentRow.Cells["ScheduleToDate"].Value),
                                                                         (dgvAttractionSchedules.CurrentRow.Cells["DesiredUnits"].Value == DBNull.Value ? 0 : Convert.ToInt32(dgvAttractionSchedules.CurrentRow.Cells["DesiredUnits"].Value)),
                                                                         dgvAttractionSchedules.CurrentRow.Cells["Seats"].Tag))
            {
                if (asb.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    dgvAttractionSchedules.CurrentRow.Cells["Seats"].Tag = asb.GetSelectedSeatIds;
                    dgvAttractionSchedules.CurrentRow.Cells["PickSeats"].Tag = asb.GetSelectedSeatNames;
                }
            }
            log.LogMethodExit();
        }

        private void dgvAttractionSchedules_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (dgvAttractionSchedules.CurrentCell != null && dgvAttractionSchedules.Columns[e.ColumnIndex].Name == "PickSeats")
                {
                    ShowSeats();
                }
            }
            catch (Exception ex){ log.Error(ex); };
            log.LogMethodExit();
        }

        private void dgvAttractionSchedules_SelectionChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e); 

            if (dgvAttractionSchedules.CurrentCell != null && dgvAttractionSchedules.SelectedRows.Count > 0)
            {
                int i = dgvAttractionSchedules.SelectedRows[0].Index;
                if (dgvAttractionSchedules.Rows[i].DefaultCellStyle.BackColor != Color.LightGray)
                {
                    dgvAttractionSchedules.CurrentCell = dgvAttractionSchedules["DesiredUnits", i];

                    int cellQty = 0;
                    if (dgvAttractionSchedules["DesiredUnits", i].Value != DBNull.Value)
                        cellQty = Convert.ToInt32(dgvAttractionSchedules["DesiredUnits", i].Value);

                    int avlQty = Convert.ToInt32(dgvAttractionSchedules["AvailableUnits", i].Value);
                    if (avlQty > 0)
                    {
                        int possibleQty = Math.Max(0, Math.Min(Quantity - (GetAllocatedQuantity() - cellQty), avlQty));
                        if (numPad != null)
                        {
                            numPad.NewEntry = true;
                            numPad.handleaction(possibleQty.ToString());
                            numPad.NewEntry = true;
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        int GetAllocatedQuantity()
        {
            log.LogMethodEntry();
            int qty = 0;
            foreach (DataGridViewRow dr in dgvAttractionSchedules.Rows)
            {
                if (dr.Cells["DesiredUnits"].Value != DBNull.Value && dr.Cells["DesiredUnits"].Value != null)
                {
                    qty += Convert.ToInt32(dr.Cells["DesiredUnits"].Value);
                }
            }
            log.LogMethodExit(qty);
            return qty;
        }

        private void chkShowPast_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            RefreshGrid();
            log.LogMethodExit();
        }

        public bool ShowSchedules(List<AttractionBookingDTO> returnData)
        {
            log.LogMethodEntry(returnData);
            DialogResult dr = this.ShowDialog();
            if (dr == DialogResult.Cancel)
            {
                log.LogMethodExit("dr == DialogResult.Cancel");
                return false;
            }
            else
            {
                foreach (DataGridViewRow dRow in dgvAttractionSchedules.Rows)
                {
                    if (dRow.Cells["DesiredUnits"].Value != null && dRow.Cells["DesiredUnits"].Value != DBNull.Value)
                    {
                        int qty = Convert.ToInt32(dRow.Cells["DesiredUnits"].Value);
                        if (qty <= 0)
                        { continue; }
                        if (qty > 999)
                            qty = 999;

                        //clsReturnDataLine returnDataLine = new clsReturnDataLine();
                        AttractionBookingDTO selectedattractionBookingDTO = new AttractionBookingDTO();
                        selectedattractionBookingDTO.AttractionProductId = productId;
                        selectedattractionBookingDTO.FacilityMapId = Convert.ToInt32(dRow.Cells["FacilityMapId"].Value); 
                        selectedattractionBookingDTO.AttractionPlayId = Convert.ToInt32(dRow.Cells["AttractionPlayId"].Value);
                        selectedattractionBookingDTO.AttractionPlayName = dRow.Cells["AttractionPlayName"].Value.ToString(); 
                        selectedattractionBookingDTO.AttractionScheduleId = Convert.ToInt32(dRow.Cells["ScheduleId"].Value);
                        selectedattractionBookingDTO.AttractionScheduleName = dRow.Cells["ScheduleName"].Value.ToString();
                        selectedattractionBookingDTO.ScheduleFromDate = Convert.ToDateTime(dRow.Cells["ScheduleFromDate"].Value);
                        selectedattractionBookingDTO.ScheduleToDate = Convert.ToDateTime(dRow.Cells["ScheduleToDate"].Value);
                        selectedattractionBookingDTO.BookedUnits = qty;
                        if (dRow.Cells["TotalUnits"].Value != DBNull.Value)
                            selectedattractionBookingDTO.AvailableUnits = Convert.ToInt32(dRow.Cells["TotalUnits"].Value);
                        selectedattractionBookingDTO.Price = Convert.ToDouble(dRow.Cells["Price"].Value == DBNull.Value ? 0 : dRow.Cells["Price"].Value); 
                        if (dRow.Cells["ExpiryDate"].Value != DBNull.Value)
                            selectedattractionBookingDTO.ExpiryDate = Convert.ToDateTime(dRow.Cells["ExpiryDate"].Value);
                        selectedattractionBookingDTO.PromotionId = Convert.ToInt32(dRow.Cells["PromotionId"].Value);
                        if (dRow.Cells["Seats"].Tag != null)
                        {
                            //returnDataLine.SelectedSeats = (dRow.Cells["Seats"].Tag == null ? null : dRow.Cells["Seats"].Tag as List<int>);//GG
                            //returnDataLine.SelectedSeatNames = (dRow.Cells["PickSeats"].Tag == null ? null : dRow.Cells["PickSeats"].Tag as List<string>);//GG
                            List<int> selectedSeats = dRow.Cells["Seats"].Tag as List<int>;
                            List<string> selectedSeatNames = (dRow.Cells["PickSeats"].Tag == null ? null : dRow.Cells["PickSeats"].Tag as List<string>);
                            for (int i = 0; i < selectedSeats.Count; i++)
                            {
                                AttractionBookingSeatsDTO attractionBookingSeatsDTO = new AttractionBookingSeatsDTO();
                                attractionBookingSeatsDTO.SeatId = selectedSeats[i];
                                if (selectedSeatNames != null)
                                {
                                    attractionBookingSeatsDTO.SeatName = selectedSeatNames[i];
                                }
                                selectedattractionBookingDTO.AttractionBookingSeatsDTOList.Add(attractionBookingSeatsDTO);
                            }
                        } 
                        selectedattractionBookingDTO.ScheduleFromTime = Decimal.Round(Convert.ToDecimal(dRow.Cells["ScheduleFromTime"].Value), 2, MidpointRounding.AwayFromZero);
                        selectedattractionBookingDTO.ScheduleToTime = Decimal.Round(Convert.ToDecimal(dRow.Cells["ScheduleToTime"].Value), 2, MidpointRounding.AwayFromZero);
                        log.LogVariableState("returnDataLine", selectedattractionBookingDTO);
                        returnData.Add(selectedattractionBookingDTO);
                    }
                }
                log.LogMethodExit(true);
                return true;
            }
        }
    }
}
