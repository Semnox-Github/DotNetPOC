/********************************************************************************************
 * Project Name - Attraction
 * Description  - AttractionSchedule form
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 ********************************************************************************************* 
 *2.70.2      11-Nov-2019      Nitin Pai      Club speed enhancement
 *2.70.2      04-Feb-2020      Nitin Pai      Reschedule Slot related changes
 *2.80.0      22-Apr-2020      Guru S A        Ability to reschedule attraction schedule selection as per reservation change  
 *2.80.0      15-Jun-2020      Nitin Pai       Fix: Reschedule does not consider the isBooking flag
 *2.110.0     01-Jan-2021      Nitin Pai       Used WCF Component for Schedules. The new ScheduleViewDTP. Performance fixes.
 *2.120.0     29-mar-2021      Sathyavathi     Bug fix - Attraction within Reservations
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Booking;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Transaction.TransactionFunctions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
 
namespace Parafait_POS.Attraction
{
    public partial class AttractionSchedule : Form
    {
        private readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private List<MasterScheduleBL> masterScheduleBLList;
        private AttractionBookingSchedulesBL attractionBookingScheduleBL;
        private List<ProductsDTO> productsList;
        private List<FacilityMapDTO> facilityMapDTOList;
        private Dictionary<int, int> quantityMap;
        private List<AttractionBooking> existingAttractionBooking;
        private List<AttractionBookingDTO> selectedAttractionBookings;
        private DateTime selectedDate;
        private DateTime selectedToDateTime = DateTime.MinValue;
        private int facilityMapId;
        private bool isBooking;
        private bool isAttractionsWithinReservationPeriodOnly = false;
        private bool isEvent;
        private CustomerDTO trxCustomerDTO;
        private ExecutionContext executionContext;
        private Utilities Utilities;
        private ProductsDTO selectedProduct;
        private usrCtrlAttractionScheduleDetails selectedScheduleUsrCtrl;
        private Dictionary<int, string> facilityMapNames;
        private Dictionary<int, List<int>> relatedFacilityMaps;
        private int bookingId;
        private int selectedFacilityMap;
        private bool isRescheduleSlot;

        private AttractionBookingDTO.RescheduleActionEnum rescheduleAction = AttractionBookingDTO.RescheduleActionEnum.NONE;
        private DayAttractionScheduleDTO sourceDASDTO;
        private DayAttractionScheduleDTO targetDASDTO;
        private ScheduleDetailsDTO sourceScheduleDetailsDTO;
        private ScheduleDetailsDTO targetScheduleDetailsDTO;
        private List<DayAttractionScheduleDTO> dayAttractionSchedulesList;
        private bool rescheduleInProgress;
        private readonly object reschduleLock = new object();
        private static Dictionary<string, System.Drawing.Color> statusCellColorMap = new Dictionary<string, Color>();

        private int pageNumber = 0;
        private int slotsPerPage = 18;
        private System.Windows.Controls.ScrollViewer scrollViewer = null;

        public AttractionSchedule(Utilities Utilities, ExecutionContext executionContect)
        {
            log.LogMethodEntry();
            this.Utilities = Utilities;
            this.executionContext = executionContect;
            isRescheduleSlot = false;
            facilityMapNames = new Dictionary<int, string>();
            relatedFacilityMaps = new Dictionary<int, List<int>>();
            log.LogMethodExit();
        }
        public AttractionSchedule(Utilities Utilities, ExecutionContext executionContext, List<MasterScheduleBL> masterScheduleBLList, List<ProductsDTO> productsList,
            Dictionary<int, int> quantityMap, List<AttractionBooking> existingAttractionSchedules, DateTime selectedDate, int facilityMapId, bool isBooking, CustomerDTO customerDTO,
            bool isEvent = false, DateTime? selectedToDateTime = null)
            : this(Utilities, executionContext)
        {
            log.LogMethodEntry();
            this.masterScheduleBLList = masterScheduleBLList;
            attractionBookingScheduleBL = new AttractionBookingSchedulesBL(executionContext, selectedDate, this.masterScheduleBLList);
            //do not sort, it is already sorted when it comes in
            this.productsList = productsList;
            this.quantityMap = quantityMap;
            this.selectedAttractionBookings = new List<AttractionBookingDTO>();
            this.existingAttractionBooking = existingAttractionSchedules;

            selectedFacilityMap = this.facilityMapId = facilityMapId;
            this.isBooking = isBooking;
            if (this.isBooking)
            {
                isAttractionsWithinReservationPeriodOnly = ParafaitDefaultContainerList.GetParafaitDefault<bool>(this.executionContext, "ATTRACTIONS_WITHIN_RESERVATION_PERIOD_ONLY", false);
            }
            if (this.isBooking && isAttractionsWithinReservationPeriodOnly)
            {
                this.selectedDate = selectedDate;
            }
            else
            {
                //this.selectedDate = selectedDate.Date;
                this.selectedDate = selectedDate;
                selectedToDateTime = null;
            }

            this.selectedToDateTime = (selectedToDateTime == null ? DateTime.MinValue : (DateTime)selectedToDateTime);

            this.isEvent = isEvent;
            this.bookingId = -1;
            this.trxCustomerDTO = customerDTO;

            if (productsList.Any())
                selectedProduct = productsList[0];

            InitlializeScreen();
            log.LogMethodExit();
        }

        private void InitlializeScreen()
        {
            log.LogMethodEntry();
            InitializeComponent();
            Utilities.setLanguage(this);
            this.Width = Screen.PrimaryScreen.WorkingArea.Width;
            this.Height = Screen.PrimaryScreen.WorkingArea.Height;
            log.LogMethodExit();
        }

        private void AttractionScheduleNew_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            //Timer delayed = new Timer();
            //delayed.Interval = 50;
            //delayed.Tick += new EventHandler(delayed_Tick);
            //delayed.Start();

            if (statusCellColorMap == null || !statusCellColorMap.Any())
            {
                LookupValuesList lookUpList = new LookupValuesList(this.Utilities.ExecutionContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookUpValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookUpValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "ATTRACTION_SCHEDULE_BACKCOLORS"));
                List<LookupValuesDTO> lookUpValuesList = lookUpList.GetAllLookupValues(lookUpValuesSearchParams);
                if ((lookUpValuesList != null) && (lookUpValuesList.Any()))
                {
                    foreach (LookupValuesDTO lookupValueDTO in lookUpValuesList)
                    {
                        String backColor = lookupValueDTO.Description;
                        string[] RGB = backColor.Split(',');
                        Color statusBackColor = new Color();
                        statusBackColor = Color.FromArgb(Convert.ToInt32(RGB[0]), Convert.ToInt32(RGB[1]), Convert.ToInt32(RGB[2]));
                        if (!statusCellColorMap.ContainsKey(lookupValueDTO.LookupName))
                        {
                            statusCellColorMap.Add(lookupValueDTO.LookupValue, statusBackColor);
                        }
                    }
                }
            }

            foreach (String status in statusCellColorMap.Keys)
            {
                Color statusBackColor = statusCellColorMap[status];

                switch (status)
                {
                    case "OPEN":
                        break;
                    case "UNAVAILABLE":
                        lblStatusUnavailable.BackColor = GetBackPanelColor("UNAVAILABLE", "");
                        break;
                    case "BLOCKED":
                        lblStatusBlocked.BackColor = GetBackPanelColor("BLOCKED", ""); ;
                        break;
                    case "SOLDOUT":
                        lblStatusSoldOut.BackColor = GetBackPanelColor("SOLDOUT", ""); ;
                        break;
                    case "RACE_IN_PROGRESS":
                        lblStatusInUse.BackColor = GetBackPanelColor("RACE_IN_PROGRESS", ""); ;
                        break;
                    case "PARTY_RESERVATION":
                        lblStatusPartyReservation.BackColor = GetBackPanelColor("", "PARTY_RESERVATION"); ;
                        break;
                    case "RESCHEDULE_IN_PROGRESS":
                        lblStatusRescheduleInProgress.BackColor = GetBackPanelColor("RESCHEDULE_IN_PROGRESS", ""); ;
                        break;
                    default:
                        break;
                };
            }

            LoadScheduleHeader();
            // clone the existing attractions as existing schedules are at trx line and not grouped
            if (!isRescheduleSlot && existingAttractionBooking != null && existingAttractionBooking.Any())
            {
                foreach (AttractionBooking attractionBooking in existingAttractionBooking)
                {
                    if (!facilityMapNames.ContainsKey(attractionBooking.AttractionBookingDTO.FacilityMapId))
                    {
                        FacilityMapDTO facilityMapDTO = new FacilityMapBL(executionContext, attractionBooking.AttractionBookingDTO.FacilityMapId).FacilityMapDTO;
                        facilityMapNames.Add(attractionBooking.AttractionBookingDTO.FacilityMapId, facilityMapDTO.FacilityMapName);

                        FacilityMapListBL facilityMapListBL = new FacilityMapListBL(executionContext);
                        relatedFacilityMaps.Add(attractionBooking.AttractionBookingDTO.FacilityMapId,
                            facilityMapListBL.GetFacilityMapsForSameFacility(attractionBooking.AttractionBookingDTO.FacilityMapId));
                    }
                    AddBookingDTOToTreeNodesList(attractionBooking.AttractionBookingDTO);
                }
                LoadSummaryPanel();
            }

            dtpAttractionDate.Value = this.selectedDate;
            if (isBooking)
            {
                dtpAttractionDate.Enabled = false;
                btnNext.Visible = false;
                btnPrev.Visible = false;
            }

            LoadProductsButton();
            if (!isRescheduleSlot)
            {
                Control productPanel = flpProducts.Controls[":Product:" + selectedProduct.ProductId.ToString()];
                if (productPanel != null)
                {
                    Control productButton = productPanel.Controls["ProductButton:" + selectedProduct.ProductId.ToString()];
                    if (productButton != null)
                    {
                        ((Button)productButton).PerformClick();
                    }
                }
                //lblHeaderAttractionProduct.Text = selectedProduct.ProductName;
            }
            else
            {
                if (!facilityMapNames.ContainsKey(facilityMapId))
                {
                    FacilityMapDTO facilityMapDTO = new FacilityMapBL(executionContext, facilityMapId).FacilityMapDTO;
                    facilityMapNames.Add(facilityMapId, facilityMapDTO.FacilityMapName);
                }
                if (!relatedFacilityMaps.ContainsKey(facilityMapId))
                {
                    FacilityMapListBL facilityMapListBL = new FacilityMapListBL(executionContext);
                    relatedFacilityMaps.Add(facilityMapId, facilityMapListBL.GetFacilityMapsForSameFacility(facilityMapId));
                }

                //if (facilityMapNames.ContainsKey(facilityMapId))
                //    lblHeaderAttractionProduct.Text = "Reschedule slots for " + facilityMapNames[facilityMapId];
            }

            LoadReschedulePanel();

            Utilities.setLanguage(this);
            //lblErrorMessage.Text += " Total " + (this.Utilities.getServerTime() - this.firstTime).Seconds.ToString(); 
            log.LogMethodExit();
        }

        private void LoadScheduleHeader()
        {
            log.LogMethodEntry();

            flpScheduleHeader.SuspendLayout();
            flpScheduleHeader.Controls.Clear();
            int cumilativeWidth = 0;
            FlowLayoutPanel scheduleHeaderPanel = new FlowLayoutPanel();
            scheduleHeaderPanel.Width = flpScheduleHeader.Width;
            scheduleHeaderPanel.Height = flpScheduleHeader.Height;
            scheduleHeaderPanel.FlowDirection = FlowDirection.LeftToRight;
            scheduleHeaderPanel.BackColor = System.Drawing.Color.Black;
            scheduleHeaderPanel.Top = ehSchedules.Top;
            scheduleHeaderPanel.Left = flpProducts.Right;
            scheduleHeaderPanel.Margin = new Padding(0, 0, 0, 0);
            scheduleHeaderPanel.WrapContents = true;

            Label lblFacilityNameHeader = new Label();
            lblFacilityNameHeader.Height = scheduleHeaderPanel.Height;
            cumilativeWidth += lblFacilityNameHeader.Width = (int)(scheduleHeaderPanel.Width * 0.18);
            lblFacilityNameHeader.Text = "Facility Map";
            lblFacilityNameHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            lblFacilityNameHeader.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            lblFacilityNameHeader.ForeColor = System.Drawing.Color.Black;
            lblFacilityNameHeader.Font = new System.Drawing.Font("Arial", 10F, FontStyle.Bold);
            lblFacilityNameHeader.Margin = new Padding(1, 1, 1, 0);
            lblFacilityNameHeader.Padding = new Padding(0);
            lblFacilityNameHeader.TextAlign = ContentAlignment.MiddleCenter;
            lblFacilityNameHeader.Left = scheduleHeaderPanel.Left;
            lblFacilityNameHeader.Top = scheduleHeaderPanel.Top;
            scheduleHeaderPanel.Controls.Add(lblFacilityNameHeader);

            Label lblScheduleNameHeader = new Label();
            lblScheduleNameHeader.Height = scheduleHeaderPanel.Height;
            cumilativeWidth += lblScheduleNameHeader.Width = (int)(scheduleHeaderPanel.Width * 0.18);
            lblScheduleNameHeader.Text = "Schedule Name";
            lblScheduleNameHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            lblScheduleNameHeader.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            lblScheduleNameHeader.ForeColor = System.Drawing.Color.Black;
            lblScheduleNameHeader.Font = new System.Drawing.Font("Arial", 10F, FontStyle.Bold);
            lblScheduleNameHeader.Margin = new Padding(0, 1, 1, 0);
            lblScheduleNameHeader.TextAlign = ContentAlignment.MiddleCenter;
            lblScheduleNameHeader.Left = lblFacilityNameHeader.Right;
            lblScheduleNameHeader.Top = scheduleHeaderPanel.Top;
            scheduleHeaderPanel.Controls.Add(lblScheduleNameHeader);

            Label lblScheduleTimeHeader = new Label();
            lblScheduleTimeHeader.Height = scheduleHeaderPanel.Height;
            cumilativeWidth += lblScheduleTimeHeader.Width = (int)(scheduleHeaderPanel.Width * 0.10);
            lblScheduleTimeHeader.Text = "Time";
            lblScheduleTimeHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            lblScheduleTimeHeader.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            lblScheduleTimeHeader.ForeColor = System.Drawing.Color.Black;
            lblScheduleTimeHeader.Font = new System.Drawing.Font("Arial", 10F, FontStyle.Bold);
            lblScheduleTimeHeader.Margin = new Padding(0, 1, 1, 0);
            lblScheduleTimeHeader.TextAlign = ContentAlignment.MiddleCenter;
            lblScheduleTimeHeader.Left = lblScheduleNameHeader.Right;
            lblScheduleTimeHeader.Top = scheduleHeaderPanel.Top;
            scheduleHeaderPanel.Controls.Add(lblScheduleTimeHeader);

            Label lblPriceHeader = new Label();
            lblPriceHeader.Height = scheduleHeaderPanel.Height;
            cumilativeWidth += lblPriceHeader.Width = (int)(scheduleHeaderPanel.Width * 0.10);
            lblPriceHeader.Text = "Price";
            lblPriceHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            lblPriceHeader.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            lblPriceHeader.ForeColor = System.Drawing.Color.Black;
            lblPriceHeader.Font = new System.Drawing.Font("Arial", 10F, FontStyle.Bold);
            lblPriceHeader.Margin = new Padding(0, 1, 1, 0);
            lblPriceHeader.TextAlign = ContentAlignment.MiddleCenter;
            lblPriceHeader.Left = lblScheduleNameHeader.Right;
            lblPriceHeader.Top = scheduleHeaderPanel.Top;
            scheduleHeaderPanel.Controls.Add(lblPriceHeader);

            Label lblTotalUnitsHeader = new Label();
            lblTotalUnitsHeader.Height = scheduleHeaderPanel.Height;
            cumilativeWidth += lblTotalUnitsHeader.Width = (int)(scheduleHeaderPanel.Width * 0.10);
            lblTotalUnitsHeader.Text = "Total Units";
            lblTotalUnitsHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            lblTotalUnitsHeader.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            lblTotalUnitsHeader.ForeColor = System.Drawing.Color.Black;
            lblTotalUnitsHeader.Font = new System.Drawing.Font("Arial", 10F, FontStyle.Bold);
            lblTotalUnitsHeader.Margin = new Padding(0, 1, 1, 0);
            lblTotalUnitsHeader.TextAlign = ContentAlignment.MiddleCenter;
            lblTotalUnitsHeader.Left = lblPriceHeader.Right;
            lblTotalUnitsHeader.Top = scheduleHeaderPanel.Top;
            scheduleHeaderPanel.Controls.Add(lblTotalUnitsHeader);

            Label lblDesiredUnitsHeader = new Label();
            lblDesiredUnitsHeader.Height = scheduleHeaderPanel.Height;
            cumilativeWidth += lblDesiredUnitsHeader.Width = (int)(scheduleHeaderPanel.Width * 0.10);
            lblDesiredUnitsHeader.Text = "Desired Units";
            lblDesiredUnitsHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            lblDesiredUnitsHeader.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            lblDesiredUnitsHeader.ForeColor = System.Drawing.Color.Black;
            lblDesiredUnitsHeader.Font = new System.Drawing.Font("Arial", 10F, FontStyle.Bold);
            lblDesiredUnitsHeader.Margin = new Padding(0, 1, 1, 0);
            lblDesiredUnitsHeader.TextAlign = ContentAlignment.MiddleCenter;
            lblDesiredUnitsHeader.Left = lblTotalUnitsHeader.Right;
            lblDesiredUnitsHeader.Top = scheduleHeaderPanel.Top;
            scheduleHeaderPanel.Controls.Add(lblDesiredUnitsHeader);

            Label lblBookedUnitsHeader = new Label();
            lblBookedUnitsHeader.Height = scheduleHeaderPanel.Height;
            cumilativeWidth += lblBookedUnitsHeader.Width = (int)(scheduleHeaderPanel.Width * 0.10);
            lblBookedUnitsHeader.Text = "Booked Units";
            lblBookedUnitsHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            lblBookedUnitsHeader.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            lblBookedUnitsHeader.ForeColor = System.Drawing.Color.Black;
            lblBookedUnitsHeader.Font = new System.Drawing.Font("Arial", 10F, FontStyle.Bold);
            lblBookedUnitsHeader.Margin = new Padding(0, 1, 1, 0);
            lblBookedUnitsHeader.TextAlign = ContentAlignment.MiddleCenter;
            lblBookedUnitsHeader.Left = lblDesiredUnitsHeader.Right;
            lblBookedUnitsHeader.Top = scheduleHeaderPanel.Top;
            scheduleHeaderPanel.Controls.Add(lblBookedUnitsHeader);

            Label lblAvailableUnitsHeader = new Label();
            lblAvailableUnitsHeader.Height = scheduleHeaderPanel.Height;
            lblAvailableUnitsHeader.Width = (scheduleHeaderPanel.Width - cumilativeWidth - 9);
            lblAvailableUnitsHeader.Text = "Available Units";
            lblAvailableUnitsHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            lblAvailableUnitsHeader.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            lblAvailableUnitsHeader.ForeColor = System.Drawing.Color.Black;
            lblAvailableUnitsHeader.Font = new System.Drawing.Font("Arial", 10F, FontStyle.Bold);
            lblAvailableUnitsHeader.Margin = new Padding(0, 1, 1, 0);
            lblAvailableUnitsHeader.TextAlign = ContentAlignment.MiddleCenter;
            lblAvailableUnitsHeader.Left = lblBookedUnitsHeader.Right;
            lblAvailableUnitsHeader.Top = scheduleHeaderPanel.Top;
            scheduleHeaderPanel.Controls.Add(lblAvailableUnitsHeader);

            flpScheduleHeader.Controls.Add(scheduleHeaderPanel);
            flpScheduleHeader.ResumeLayout(true);

            log.LogMethodExit();
        }
        private void delayed_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            ((sender) as Timer).Stop();
            RefreshView();

            log.LogMethodExit();
        }

        private void ProductButton_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Cursor = Cursors.WaitCursor;
            flpProducts.SuspendLayout();
            Button selectedProductButton = (Button)sender;

            if (selectedProduct != null)
            {
                if (selectedProduct.ProductId == (int)selectedProductButton.Tag)
                {
                    this.Cursor = Cursors.Default;
                    // same product has been clicked
                    return;
                }
                // check if another product button is selected. Reset the image of that button
                Control productPanel = flpProducts.Controls[":Product:" + selectedProduct.ProductId.ToString()];
                if (productPanel != null)
                {
                    Control productButton = productPanel.Controls["ProductButton:" + selectedProduct.ProductId.ToString()];
                    if (productButton != null)
                    {
                        Button tempButton = ((Button)productButton);
                        tempButton.BackgroundImageLayout = ImageLayout.Stretch;
                        tempButton.BackColor = Color.Transparent;
                        tempButton.ForeColor = Color.Black;
                        tempButton.BackgroundImage = global::Parafait_POS.Properties.Resources.Attraction;
                        tempButton.BackgroundImage.Tag = tempButton.BackgroundImage;
                    }
                }
            }

            selectedProductButton.BackgroundImageLayout = ImageLayout.Stretch;
            selectedProductButton.BackgroundImage = global::Parafait_POS.Properties.Resources.ProductPressed;
            selectedProductButton.ForeColor = Color.White;
            selectedProduct = productsList.Where(x => x.ProductId == (int)selectedProductButton.Tag).ToList<ProductsDTO>()[0];
            lblHeaderAttractionProduct.Text = selectedProduct.ProductName;
            selectedFacilityMap = -1;
            flpProducts.ResumeLayout(true);
            ProductButtonClickEventHandler();

            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private void ProductButton_MouseUp(object sender, MouseEventArgs e)
        {
        }

        private void ProductButton_MouseDown(object sender, MouseEventArgs e)
        {
        }

        private void chkShowPast_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.SuspendLayout();
            RefreshView();
            this.ResumeLayout(true);
            log.LogMethodExit();
        }

        private void cmbAttractionFacility_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (cmbAttractionFacility != null && cmbAttractionFacility.SelectedValue != null)
            {
                //flpSchedules.SuspendLayout();
                selectedFacilityMap = ((int)cmbAttractionFacility.SelectedValue);
                FilterSchedulePerFacilityAndTimeSlotSelection();
                //flpSchedules.ResumeLayout(true);
            }
            log.LogMethodExit();
        }

        private void dtpAttractionDate_ValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            selectedDate = dtpAttractionDate.Value;
            RefreshView();
            log.LogMethodExit();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            dtpAttractionDate.Value = dtpAttractionDate.Value.AddDays(-1);
            //dtpAttractionDate.Refresh();
            log.LogMethodExit();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            dtpAttractionDate.Value = dtpAttractionDate.Value.AddDays(1);
            //dtpAttractionDate.Refresh();
            log.LogMethodExit();
        }

        private void btnPrev_MouseDown(object sender, MouseEventArgs e)
        {
            this.btnPrev.BackgroundImage = global::Parafait_POS.Properties.Resources.R_Backward_Btn_Hover;
        }

        private void btnPrev_MouseUp(object sender, MouseEventArgs e)
        {
            this.btnPrev.BackgroundImage = global::Parafait_POS.Properties.Resources.R_Backward_Btn;
        }

        private void btnNext_MouseDown(object sender, MouseEventArgs e)
        {
            this.btnNext.BackgroundImage = global::Parafait_POS.Properties.Resources.R_Forward_Btn_Hover;
        }

        private void btnNext_MouseUp(object sender, MouseEventArgs e)
        {
            this.btnNext.BackgroundImage = global::Parafait_POS.Properties.Resources.R_Forward_Btn;
        }

        private void flpScheduleLine_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            String senderName = "";
            if (sender as usrCtrlAttractionScheduleDetails != null)
            {
                senderName = ((usrCtrlAttractionScheduleDetails)sender).Name;
            }
            else
            {
                senderName = ((System.Windows.Controls.Label)sender).Name;
            }
            int startIndex = senderName.IndexOf("DTOID");
            int counter = 0;

            if (startIndex >= 0)
            {
                //int.TryParse(senderName.Substring(startIndex + 7), out counter);
                if (selectedScheduleUsrCtrl != null)
                {
                    counter = selectedScheduleUsrCtrl.TabIndex;
                    selectedScheduleUsrCtrl.Background = System.Windows.Media.Brushes.SlateGray;
                    //selectedScheduleUsrCtrl.SetControlColor(counter % 2 == 0 ? System.Drawing.Color.White : System.Drawing.Color.Azure);
                    selectedScheduleUsrCtrl.SetControlColor(selectedScheduleUsrCtrl.PrevColor);
                }
                selectedScheduleUsrCtrl = (startIndex == 0) ? (usrCtrlAttractionScheduleDetails)sender : (usrCtrlAttractionScheduleDetails)((System.Windows.Controls.WrapPanel)((System.Windows.Controls.Label)sender).Parent).Parent;
            }
            else
            {
                POSUtils.ParafaitMessageBox(Utilities.MessageUtils.getMessage(2224));
                return;
            }

            if (selectedScheduleUsrCtrl == null || selectedProduct == null)
            {
                POSUtils.ParafaitMessageBox(Utilities.MessageUtils.getMessage(2224));
                return;
            }

            selectedScheduleUsrCtrl.PrevColor = selectedScheduleUsrCtrl.BackColor;
            selectedScheduleUsrCtrl.Background = System.Windows.Media.Brushes.Red;
            selectedScheduleUsrCtrl.SetControlColor(System.Windows.Media.Colors.Green);

            ScheduleDetailsDTO scheduleDTO = (ScheduleDetailsDTO)selectedScheduleUsrCtrl.Tag;
            // get total booked quantity for product
            int totalBookedQty = GetTotalBookedQuantity(scheduleDTO.ProductId);
            // get total booked quantity for this slot
            int totalBookedQtyinCurrentSchedule = GetTotalBookedQuantityForCurrentBooking(scheduleDTO, -1, true);
            int quantityToBeBooked = totalBookedQtyinCurrentSchedule == 0 ? quantityMap[scheduleDTO.ProductId] - totalBookedQty : totalBookedQtyinCurrentSchedule;
            int delta = 0;
            if (isRescheduleSlot)
            {
                if (sourceDASDTO == null)
                {
                    if (dayAttractionSchedulesList.FirstOrDefault(x => x.DayAttractionScheduleId != -1 && x.DayAttractionScheduleId == scheduleDTO.DayAttractionScheduleId &&
                        x.ScheduleDateTime == scheduleDTO.ScheduleFromDate && relatedFacilityMaps[scheduleDTO.FacilityMapId].Contains(x.FacilityMapId)) != null)
                    {
                        sourceDASDTO = dayAttractionSchedulesList.FirstOrDefault(x => x.DayAttractionScheduleId != -1 && x.DayAttractionScheduleId == scheduleDTO.DayAttractionScheduleId
                            && x.ScheduleDateTime == scheduleDTO.ScheduleFromDate && relatedFacilityMaps[scheduleDTO.FacilityMapId].Contains(x.FacilityMapId));

                        if (sourceDASDTO != null && sourceDASDTO.FacilityMapId != selectedFacilityMap)
                        {
                            String message = MessageContainerList.GetMessage(executionContext, 2448);
                            lblErrorMessage.Text = message;
                            POSUtils.ParafaitMessageBox(message); //New Message Box UI - 05-Mar-2016
                            ResetRescheduleAction(false);
                            return;
                        }
                    }
                    else
                    {
                        sourceDASDTO = new DayAttractionScheduleDTO(scheduleDTO.DayAttractionScheduleId, scheduleDTO.ScheduleId, scheduleDTO.FacilityMapId,
                            scheduleDTO.ScheduleFromDate.Date, scheduleDTO.ScheduleFromDate, DayAttractionScheduleDTO.ScheduleStatusEnumToString(DayAttractionScheduleDTO.ScheduleStatusEnum.OPEN),
                            "", true, DayAttractionScheduleDTO.SourceEnumToString(isBooking ? DayAttractionScheduleDTO.SourceEnum.RESERVATION : DayAttractionScheduleDTO.SourceEnum.WALK_IN), true,
                            DateTime.MinValue, "", scheduleDTO.ScheduleName, scheduleDTO.ScheduleToDate, scheduleDTO.AttractionPlayId, scheduleDTO.AttractionPlayName,
                            scheduleDTO.ScheduleFromTime, scheduleDTO.ScheduleToTime, "");
                    }

                    sourceScheduleDetailsDTO = scheduleDTO;

                    if (sourceDASDTO.ScheduleStatus != DayAttractionScheduleDTO.ScheduleStatusEnumToString(DayAttractionScheduleDTO.ScheduleStatusEnum.OPEN) &&
                        sourceDASDTO.ScheduleStatus != DayAttractionScheduleDTO.ScheduleStatusEnumToString(DayAttractionScheduleDTO.ScheduleStatusEnum.BLOCKED))
                    {
                        String message = MessageContainerList.GetMessage(executionContext, 2449);
                        lblErrorMessage.Text = message;
                        POSUtils.ParafaitMessageBox(message); //New Message Box UI - 05-Mar-2016
                        ResetRescheduleAction(false);
                        return;
                    }

                    Control moveBookings1 = flpReschedule.Controls["btnMoveBookingsButton"];
                    moveBookings1.Enabled = true;
                    Control moveSchedules1 = flpReschedule.Controls["btnMoveScheduleButton"];
                    moveSchedules1.Enabled = true;

                    if (flpReschedule.Controls["btnBlockScheduleButton"] != null)
                    {
                        Control blockButton = flpReschedule.Controls["btnBlockScheduleButton"];
                        blockButton.Text = sourceDASDTO != null && sourceDASDTO.DayAttractionScheduleId > -1 && sourceDASDTO.ScheduleStatus == "BLOCKED" ?
                                                        MessageContainerList.GetMessage(this.Utilities.ExecutionContext, "Unblock Schedule")
                                                        : MessageContainerList.GetMessage(this.Utilities.ExecutionContext, "Block Schedule");

                        if (sourceDASDTO != null && sourceDASDTO.DayAttractionScheduleId > -1 && sourceDASDTO.ScheduleStatus == "BLOCKED")
                        {
                            Control moveBookings = flpReschedule.Controls["btnMoveBookingsButton"];
                            moveBookings.Enabled = false;
                            Control moveSchedules = flpReschedule.Controls["btnMoveScheduleButton"];
                            moveSchedules.Enabled = false;
                            Control reserveBookings = flpReschedule.Controls["btnReserveScheduleButton"];
                            reserveBookings.Enabled = false;
                        }
                        else if (sourceDASDTO != null && sourceDASDTO.DayAttractionScheduleId == -1)
                        {
                            Control moveBookings = flpReschedule.Controls["btnMoveBookingsButton"];
                            moveBookings.Enabled = false;
                            Control moveSchedules = flpReschedule.Controls["btnMoveScheduleButton"];
                            moveSchedules.Enabled = false;
                        }
                    }

                    if (flpReschedule.Controls["btnReserveScheduleButton"] != null)
                    {
                        Control blockButton = flpReschedule.Controls["btnReserveScheduleButton"];
                        blockButton.Text = sourceDASDTO != null && sourceDASDTO.DayAttractionScheduleId > -1 && sourceDASDTO.ScheduleStatus == "OPEN" && scheduleDTO.BookedUnits == 0 ?
                                                        MessageContainerList.GetMessage(this.Utilities.ExecutionContext, "Release Schedule")
                                                        : MessageContainerList.GetMessage(this.Utilities.ExecutionContext, "Reserve Schedule");

                        if (sourceDASDTO != null && sourceDASDTO.DayAttractionScheduleId > -1 && scheduleDTO.BookedUnits > 0)
                        {
                            Control reserveBookings = flpReschedule.Controls["btnReserveScheduleButton"];
                            reserveBookings.Enabled = false;
                        }
                    }

                    flpReschedule.Visible = true;
                    flpReschedule.BringToFront();
                }
                else if (sourceDASDTO != null && targetDASDTO == null)
                {
                    if (dayAttractionSchedulesList.FirstOrDefault(x => x.DayAttractionScheduleId != -1 && x.DayAttractionScheduleId == scheduleDTO.DayAttractionScheduleId &&
                        x.ScheduleDateTime == scheduleDTO.ScheduleFromDate && relatedFacilityMaps[scheduleDTO.FacilityMapId].Contains(x.FacilityMapId)) != null)
                    {
                        targetDASDTO = dayAttractionSchedulesList.FirstOrDefault(x => x.DayAttractionScheduleId != -1 && x.DayAttractionScheduleId == scheduleDTO.DayAttractionScheduleId
                            && x.ScheduleDateTime == scheduleDTO.ScheduleFromDate && relatedFacilityMaps[scheduleDTO.FacilityMapId].Contains(x.FacilityMapId));
                    }
                    else
                    {
                        targetDASDTO = new DayAttractionScheduleDTO(scheduleDTO.DayAttractionScheduleId, scheduleDTO.ScheduleId, scheduleDTO.FacilityMapId,
                            scheduleDTO.ScheduleFromDate.Date, scheduleDTO.ScheduleFromDate, DayAttractionScheduleDTO.ScheduleStatusEnumToString(DayAttractionScheduleDTO.ScheduleStatusEnum.OPEN),
                            "", true, sourceDASDTO.Source, true, DateTime.MinValue, "", scheduleDTO.ScheduleName, scheduleDTO.ScheduleToDate, scheduleDTO.AttractionPlayId,
                            scheduleDTO.AttractionPlayName, scheduleDTO.ScheduleFromTime, scheduleDTO.ScheduleToTime, "");
                    }

                    targetScheduleDetailsDTO = scheduleDTO;

                    if (targetDASDTO.ScheduleStatus != DayAttractionScheduleDTO.ScheduleStatusEnumToString(DayAttractionScheduleDTO.ScheduleStatusEnum.OPEN))
                    {
                        String message = MessageContainerList.GetMessage(executionContext, 2449);
                        lblErrorMessage.Text = message;
                        POSUtils.ParafaitMessageBox(message); //New Message Box UI - 05-Mar-2016
                        ResetRescheduleAction(false);
                        return;
                    }

                    if (rescheduleAction == AttractionBookingDTO.RescheduleActionEnum.MOVE_SCHEDULES)
                    {
                        this.Cursor = Cursors.WaitCursor;
                        try
                        {
                            RescheduleAttractionBookingsBL rescheduleAttractionBookingsBL = new RescheduleAttractionBookingsBL(this.Utilities.ExecutionContext);
                            List<ScheduleDetailsDTO> impactedSchedules = rescheduleAttractionBookingsBL.MoveAttractionSchedulesImpactedSlots(scheduleDTO.ScheduleFromDate, scheduleDTO.FacilityMapId,
                                scheduleDTO.ScheduleId, sourceDASDTO.AttractionScheduleId);
                            LoadSummaryForReschedule(impactedSchedules);
                        }
                        catch (Exception ex)
                        {
                            POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext, ex.Message));
                            lblErrorMessage.Text = MessageContainerList.GetMessage(Utilities.ExecutionContext, ex.Message) + MessageContainerList.GetMessage(Utilities.ExecutionContext, " Choose a different slot.");
                            targetDASDTO = null;
                            targetScheduleDetailsDTO = null;
                            this.Cursor = Cursors.Default;
                            ResetRescheduleAction(false);
                            return;
                        }
                        this.Cursor = Cursors.Default;
                        lblErrorMessage.Text = MessageContainerList.GetMessage(Utilities.ExecutionContext, 2777);
                    }
                    else if (rescheduleAction == AttractionBookingDTO.RescheduleActionEnum.MOVE_BOOKINGS)
                    {
                        if (targetDASDTO.FacilityMapId != sourceDASDTO.FacilityMapId)
                        {
                            if (targetDASDTO.DayAttractionScheduleId == -1)
                            {
                                targetDASDTO.FacilityMapId = sourceDASDTO.FacilityMapId;
                            }
                            else
                            {
                                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext, 2778));
                                ResetRescheduleAction(false);
                                return;
                            }
                        }

                        if (targetScheduleDetailsDTO.AvailableUnits < sourceScheduleDetailsDTO.BookedUnits)
                        {
                            String message = MessageContainerList.GetMessage(executionContext, 2);
                            lblErrorMessage.Text = message;
                            POSUtils.ParafaitMessageBox(message); //New Message Box UI - 05-Mar-2016
                            ResetRescheduleAction(false);
                            return;
                        }

                        List<ScheduleDetailsDTO> schedulesList = new List<ScheduleDetailsDTO>();
                        schedulesList.Add(targetScheduleDetailsDTO);
                        LoadSummaryForReschedule(schedulesList);
                        lblErrorMessage.Text = MessageContainerList.GetMessage(Utilities.ExecutionContext, 2777);
                    }
                    else
                    {
                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext, 2779));
                        return;
                    }
                }
                else
                {
                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext, 2779));
                    return;
                }
            }
            else
            {
                int userQty = (int)NumberPadForm.ShowNumberPadForm(MessageContainerList.GetMessage(executionContext, "Enter Desired Units"), quantityToBeBooked == 0 ? "0" : quantityToBeBooked.ToString(), Utilities);
                if (userQty < 0 || userQty > 999)
                {
                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1)); //New Message Box UI - 05-Mar-2016
                    return;
                }

                delta = userQty - totalBookedQtyinCurrentSchedule;
                if (delta > scheduleDTO.AvailableUnits)
                {
                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 2)); //New Message Box UI - 05-Mar-2016
                    return;
                }

                if ((delta + totalBookedQty) > quantityMap[scheduleDTO.ProductId])
                {
                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1)); //New Message Box UI - 05-Mar-2016
                    return;
                }

                if (!CreateNewBookingSlot(scheduleDTO, userQty))
                {
                    POSUtils.ParafaitMessageBox(Utilities.MessageUtils.getMessage(1)); //New Message Box UI - 05-Mar-2016
                    return;
                }

                Control productButtonFLP = flpProducts.Controls[":Product:" + scheduleDTO.ProductId];
                if (productButtonFLP != null)
                {
                    if (productButtonFLP.Controls["LabelBooked:" + scheduleDTO.ProductId] != null)
                    {
                        Control ctn = productButtonFLP.Controls["LabelBooked:" + scheduleDTO.ProductId];

                        //int bookedQty = this.GetTotalBookedQuantity(scheduleDTO.ProductId);
                        int totalQty = quantityMap[scheduleDTO.ProductId];

                        ctn.Text = (delta + totalBookedQty).ToString() + "/" + totalQty;
                    }
                }

                // Reduce available units
                scheduleDTO.AvailableUnits = scheduleDTO.AvailableUnits != null && scheduleDTO.AvailableUnits > 0 ? scheduleDTO.AvailableUnits - delta : scheduleDTO.AvailableUnits;
                scheduleDTO.BookedUnits += delta;

                LoadSummaryPanel();
            }

            usrCtrlAttractionScheduleDetails scheduleLine = FindChild<usrCtrlAttractionScheduleDetails>(flpSchedules, "DTOID" + scheduleDTO.ScheduleId + "FacMpId" + scheduleDTO.FacilityMapId);
            if (scheduleLine != null)
            {
                System.Windows.Controls.Label ctn = FindChild<System.Windows.Controls.Label>(scheduleLine, "DesiredUnitsDTOID" + scheduleDTO.ScheduleId);
                if (ctn != null)
                {
                    int currentUnits = 0;
                    int.TryParse(ctn.Content.ToString(), out currentUnits);
                    ctn.Content = (currentUnits + delta).ToString();
                }
                // Reduce available units
                System.Windows.Controls.Label ctnAvlUnits = FindChild<System.Windows.Controls.Label>(scheduleLine, "AvailableUnitsDTOID" + scheduleDTO.ScheduleId);
                if (ctnAvlUnits != null)
                {
                    ctnAvlUnits.Content = scheduleDTO.AvailableUnits.ToString();
                }
            }

            log.LogMethodExit();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.DialogResult = DialogResult.Cancel;
            this.Close();
            log.LogMethodExit();
        }

        private string bw_DoMoveBookingsWork(ExecutionContext executionContext, DayAttractionScheduleDTO sourceDASDTO, DayAttractionScheduleDTO targetDASDTO)
        {
            try
            {
                String message = MessageContainerList.GetMessage(this.Utilities.ExecutionContext, "Move Booking") + " from " + sourceDASDTO.ScheduleDateTime + " to " + targetDASDTO.ScheduleDateTime + " completed successfully ";
                RescheduleAttractionBookingsBL rescheduleAttractionBookingsBL = new RescheduleAttractionBookingsBL(executionContext, sourceDASDTO, targetDASDTO);
                rescheduleAttractionBookingsBL.MoveAttractionBookings();
                return message;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return "Move failed " + ex.Message;
            }
            finally
            {
                lock (reschduleLock)
                {
                    rescheduleInProgress = false;
                }
            }
        }

        private string bw_DoMoveSchedulesWork(ExecutionContext executionContext, DayAttractionScheduleDTO sourceDASDTO, DayAttractionScheduleDTO targetDASDTO)
        {
            try
            {
                String message = MessageContainerList.GetMessage(this.Utilities.ExecutionContext, "Move Schedule") + " from " + sourceDASDTO.ScheduleDateTime + " to " + targetDASDTO.ScheduleDateTime + " completed successfully ";
                RescheduleAttractionBookingsBL rescheduleAttractionBookingsBL = new RescheduleAttractionBookingsBL(executionContext, sourceDASDTO, targetDASDTO);
                rescheduleAttractionBookingsBL.MoveAttractionSchedules();
                return message;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return "Move failed " + ex.Message;
            }
            finally
            {
                lock (reschduleLock)
                {
                    rescheduleInProgress = false;
                }
            }

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            String exeDir = System.IO.Path.GetDirectoryName(Environment.CommandLine.Replace("\"", ""));
            if (isRescheduleSlot)
            {
                if (rescheduleAction == AttractionBookingDTO.RescheduleActionEnum.MOVE_BOOKINGS)
                {
                    if (sourceDASDTO != null && targetDASDTO != null)
                    {
                        rescheduleInProgress = true;
                        lblReschduleInProgress.Text = MessageContainerList.GetMessage(this.Utilities.ExecutionContext, "Reschduling in progress. Please wait.");
                        flpReschduleInProgress.Visible = true;

                        flpReschduleInProgress.BringToFront();
                        Application.DoEvents();

                        Task<string> task = Task<string>.Factory.StartNew(() =>
                        {
                            try
                            {
                                String returnMessage = bw_DoMoveBookingsWork(executionContext, sourceDASDTO, targetDASDTO);
                                return returnMessage;
                            }
                            catch (Exception ex)
                            {
                                return "Error: " + ex.Message;
                            }
                        });
                        Task UITask = task.ContinueWith((ret) =>
                        {
                            lblErrorMessage.Text = ret.Result;
                        }, TaskScheduler.FromCurrentSynchronizationContext());

                        flpReschduleInProgress.BringToFront();
                        Application.DoEvents();

                    }
                    else
                    {
                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext, 2780));
                        return;
                    }
                }
                else if (rescheduleAction == AttractionBookingDTO.RescheduleActionEnum.MOVE_SCHEDULES)
                {
                    if (sourceDASDTO != null && targetDASDTO != null)
                    {
                        rescheduleInProgress = true;
                        if (File.Exists(exeDir + "\\Resources\\Processing_icon.gif"))
                        {
                            picWorkInProgress.Image = Image.FromFile(exeDir + "\\Resources\\Processing_icon.gif");
                        }
                        else
                        {
                            picWorkInProgress.Image = Properties.Resources.Processing_Icon;
                        }
                        lblReschduleInProgress.Text = MessageContainerList.GetMessage(this.Utilities.ExecutionContext, "Reschduling in progress. Please wait.");
                        flpReschduleInProgress.Visible = true;
                        Task<string> task = Task<string>.Factory.StartNew(() =>
                        {
                            try
                            {
                                String returnMessage = bw_DoMoveSchedulesWork(executionContext, sourceDASDTO, targetDASDTO);
                                return returnMessage;
                            }
                            catch (Exception ex)
                            {
                                return "Error: " + ex.Message;
                            }
                        });
                        Task UITask = task.ContinueWith((ret) =>
                        {
                            lblErrorMessage.Text = ret.Result;
                        }, TaskScheduler.FromCurrentSynchronizationContext());

                        flpReschduleInProgress.BringToFront();
                        Application.DoEvents();
                    }
                    else
                    {
                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext, 2780));
                        return;
                    }
                }
                else if (rescheduleAction == AttractionBookingDTO.RescheduleActionEnum.BLOCK_SCHEDULE)
                {
                    if (sourceDASDTO != null)
                    {
                        // it is already blocked, mark the slot as inactive
                        if (sourceDASDTO.DayAttractionScheduleId > -1 && sourceDASDTO.Blocked &&
                            sourceDASDTO.ScheduleStatus == DayAttractionScheduleDTO.ScheduleStatusEnumToString(DayAttractionScheduleDTO.ScheduleStatusEnum.BLOCKED))
                        {
                            sourceDASDTO.IsActive = false;
                        }
                        else
                        {
                            sourceDASDTO.Blocked = true;
                            sourceDASDTO.ScheduleStatus = DayAttractionScheduleDTO.ScheduleStatusEnumToString(DayAttractionScheduleDTO.ScheduleStatusEnum.BLOCKED);
                        }
                        DayAttractionScheduleBL dayAttractionScheduleBL = new DayAttractionScheduleBL(Utilities.ExecutionContext, sourceDASDTO);
                        dayAttractionScheduleBL.Save();
                        lblErrorMessage.Text = sourceDASDTO.ScheduleDateTime + " " + AttractionBookingDTO.RescheduleActionEnumToString(rescheduleAction) + " completed successfully ";
                    }
                    else
                    {
                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext, 2781));
                        return;
                    }
                }
                else if (rescheduleAction == AttractionBookingDTO.RescheduleActionEnum.RESERVE_SCHEDULE)
                {
                    if (sourceDASDTO != null)
                    {
                        if (sourceDASDTO.DayAttractionScheduleId > -1 &&
                            sourceDASDTO.ScheduleStatus == DayAttractionScheduleDTO.ScheduleStatusEnumToString(DayAttractionScheduleDTO.ScheduleStatusEnum.OPEN))
                        {
                            sourceDASDTO.IsActive = false;
                        }
                        else
                        {
                            sourceDASDTO.FacilityMapId = sourceScheduleDetailsDTO.FacilityMapId;
                        }
                        DayAttractionScheduleBL dayAttractionScheduleBL = new DayAttractionScheduleBL(Utilities.ExecutionContext, sourceDASDTO);
                        dayAttractionScheduleBL.Save();
                        lblErrorMessage.Text = sourceDASDTO.ScheduleDateTime + " " + AttractionBookingDTO.RescheduleActionEnumToString(rescheduleAction) + " successfully.";
                    }
                    else
                    {
                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext, 2781));
                        return;
                    }
                }

                bool breakThread = false;
                while (!breakThread)
                {
                    System.Threading.Thread.Sleep(100);
                    lock (reschduleLock)
                    {
                        if (!rescheduleInProgress)
                        {
                            breakThread = true;
                            break;
                        }
                    }

                }

                flpReschduleInProgress.Visible = false;
                ResetRescheduleAction(true);
                return;
            }

            // perform validations - total booked quantity should be equal to or less than the input quantity
            foreach (int key in quantityMap.Keys)
            {
                if (GetTotalBookedQuantity(key) != quantityMap[key])
                {
                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 2225, this.productsList.Where(x => x.ProductId == key).ToList<ProductsDTO>()[0].ProductName));
                    return;
                }
            }
            // do not allow elapsed schedules to be selected for non booking
            List<AttractionBookingDTO> elapsedList = selectedAttractionBookings.Where(x => x.ScheduleFromDate < Utilities.getServerTime().AddMinutes(POSStatic.ATTRACTION_BOOKING_GRACE_PERIOD)).ToList();
            if (elapsedList.Any() && !isBooking)
            {
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 2226));
                return;
            }
            if (isBooking && isAttractionsWithinReservationPeriodOnly)
            {
                // do not allow elapsed schedules or future schedule to be selected for booking
                elapsedList = selectedAttractionBookings.Where(x => (x.ScheduleFromDate >= this.selectedDate
                                                                     && x.ScheduleToDate <= this.selectedToDateTime) == false).ToList();
                if (elapsedList != null && elapsedList.Any())
                {
                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 2648, this.selectedDate.ToString(Utilities.ParafaitEnv.DATETIME_FORMAT), this.selectedToDateTime.ToString(Utilities.ParafaitEnv.DATETIME_FORMAT)));//"Please select schedules within in " + this.selectedDate.ToString(Utilities.ParafaitEnv.DATETIME_FORMAT) + " and " + this.selectedToDateTime.ToString(Utilities.ParafaitEnv.DATETIME_FORMAT)                    
                    return;
                }
            }

            // concurrency check
            foreach (AttractionBookingDTO atbDTO in selectedAttractionBookings)
            {
                if (atbDTO.BookedUnits > 0)
                {
                    int totalBookedUnits = (new FacilityMapBL(executionContext, atbDTO.FacilityMapId)).GetTotalBookedUnitsForAttraction(atbDTO.ScheduleFromDate, atbDTO.ScheduleToDate);

                    // for reschedules, remove the quantity from existing list as this is already included
                    if (existingAttractionBooking != null && existingAttractionBooking.Any())
                    {
                        List<AttractionBooking> existingATB = existingAttractionBooking.Where(x => (x.AttractionBookingDTO.AttractionScheduleId == atbDTO.AttractionScheduleId)
                                                                                                        && (x.AttractionBookingDTO.FacilityMapId == atbDTO.FacilityMapId)
                                                                                                        && (x.AttractionBookingDTO.ScheduleFromDate.Date == atbDTO.ScheduleFromDate.Date)
                                                                                                        && (x.AttractionBookingDTO.ScheduleFromTime == atbDTO.ScheduleFromTime)
                                                                                                            ).ToList();
                        if (existingATB != null && existingATB.Any())
                        {
                            totalBookedUnits = totalBookedUnits - existingATB.Sum(x => x.AttractionBookingDTO.BookedUnits);
                        }
                    }

                    if (atbDTO.AvailableUnits < totalBookedUnits + atbDTO.BookedUnits)
                    {
                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 326, atbDTO.BookedUnits,
                            (atbDTO.AvailableUnits - totalBookedUnits).ToString() + "(" + atbDTO.AvailableUnits.ToString() + ")"));
                        return;
                    }

                    // set up the source before calling validation
                    if (isBooking)
                        atbDTO.Source = AttractionBookingDTO.SourceEnum.RESERVATION;

                    AttractionBooking atbForValidation = new AttractionBooking(executionContext, atbDTO);
                    try
                    {
                        atbForValidation.ValidateDayAttractionBooking(null);
                    }
                    catch (RowNotInTableException ex)
                    {
                        // do nothing valid scenario
                    }
                    catch (EntityExpiredException ex)
                    {
                        // this will come in scenario where an extity might have been booked from web but was not confirmed
                        // in such a scenario, let ATB handle it
                        // do nothing valid scenario
                    }
                    catch (Exception ex)
                    {
                        POSUtils.ParafaitMessageBox("Slot " + atbDTO.ScheduleFromDate + " cannot be booked. \n" + ex.Message);
                        return;
                    }
                }

            }
            this.DialogResult = DialogResult.OK;
            this.Close();
            log.LogMethodExit();
        }

        private void btnOk_MouseDown(object sender, MouseEventArgs e)
        {
            this.btnOk.BackgroundImage = global::Parafait_POS.Properties.Resources.normal1;
        }

        private void btnOk_MouseUp(object sender, MouseEventArgs e)
        {
            this.btnOk.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
        }

        private void btnCancel_MouseDown(object sender, MouseEventArgs e)
        {
            this.btnCancel.BackgroundImage = global::Parafait_POS.Properties.Resources.normal1;
        }

        private void btnCancel_MouseUp(object sender, MouseEventArgs e)
        {
            this.btnCancel.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
        }

        private void btnClear_MouseDown(object sender, MouseEventArgs e)
        {
            ((Button)sender).BackgroundImage = global::Parafait_POS.Properties.Resources.Delete_Icon_Hover;
        }

        private void btnClear_MouseUp(object sender, MouseEventArgs e)
        {
            ((Button)sender).BackgroundImage = global::Parafait_POS.Properties.Resources.Delete_Icon_Normal;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Button selectedClearButton = (Button)sender;
            AttractionBookingDTO attractionDTO = (AttractionBookingDTO)selectedClearButton.Tag;

            int quantity = attractionDTO.BookedUnits;
            int totalBooked = this.GetTotalBookedQuantity(attractionDTO.AttractionProductId);

            selectedAttractionBookings.Remove(attractionDTO);
            LoadSummaryPanel();

            DateTime scheduleDateTime = attractionDTO.ScheduleFromDate;
            if (scheduleDateTime.Hour < ParafaitDefaultContainerList.GetParafaitDefault<int>(Utilities.ExecutionContext, "BUSINESS_DAY_START_TIME"))
            {
                scheduleDateTime = scheduleDateTime.AddDays(-1).AddHours(ParafaitDefaultContainerList.GetParafaitDefault<int>(Utilities.ExecutionContext, "BUSINESS_DAY_START_TIME"));
            }
            // refresh only if this is for a different date or product
            if (facilityMapDTOList.Where(x => x.FacilityMapId == attractionDTO.FacilityMapId).ToList().Any() && selectedDate.Date == scheduleDateTime.Date)
            {
                usrCtrlAttractionScheduleDetails scheduleLine = FindChild<usrCtrlAttractionScheduleDetails>(flpSchedules, "DTOID" + attractionDTO.AttractionScheduleId + "FacMpId" + attractionDTO.FacilityMapId);
                if (scheduleLine != null)
                {
                    //ScheduleLineCell ctn = FindChild<ScheduleLineCell>(scheduleLine,

                    ScheduleDetailsDTO scheduleDetailsDTO = (ScheduleDetailsDTO)scheduleLine.Tag;
                    scheduleDetailsDTO.AvailableUnits += quantity;

                    System.Windows.Controls.Label ctn = FindChild<System.Windows.Controls.Label>(scheduleLine, "DesiredUnitsDTOID" + attractionDTO.AttractionScheduleId);
                    if (ctn != null)
                    {
                        int currentUnits = 0;
                        int.TryParse(ctn.Content.ToString(), out currentUnits);
                        ctn.Content = (currentUnits - quantity).ToString();
                    }

                    System.Windows.Controls.Label ctnAvlUnit = FindChild<System.Windows.Controls.Label>(scheduleLine, "AvailableUnitsDTOID" + attractionDTO.AttractionScheduleId);
                    if (ctnAvlUnit != null)
                    {
                        int currentUnits = 0;
                        int.TryParse(ctnAvlUnit.Content.ToString(), out currentUnits);
                        ctnAvlUnit.Content = (currentUnits + quantity).ToString();
                    }
                }
            }
            else
            {
                //RefreshView();
            }

            Control productButtonFLP = flpProducts.Controls[":Product:" + attractionDTO.AttractionProductId];
            if (productButtonFLP != null)
            {
                if (productButtonFLP.Controls["LabelBooked:" + attractionDTO.AttractionProductId] != null)
                {
                    Control ctn = productButtonFLP.Controls["LabelBooked:" + attractionDTO.AttractionProductId];
                    ctn.Text = ((quantity * -1) + totalBooked).ToString() + "/" + quantityMap[attractionDTO.AttractionProductId];
                }
            }

            log.LogMethodExit();
            return;
        }

        private void btnScheduleEditButton_MouseDown(object sender, MouseEventArgs e)
        {
            ((Button)sender).BackgroundImage = global::Parafait_POS.Properties.Resources.Edit_Icon_Hover;
        }
        private void btnScheduleEditButton_MouseUp(object sender, MouseEventArgs e)
        {
            ((Button)sender).BackgroundImage = global::Parafait_POS.Properties.Resources.Edit_Icon_Normal;
        }
        private void btnScheduleEditButton_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Button selectedEditButton = (Button)sender;
            AttractionBookingDTO attractionBookingDTO = (AttractionBookingDTO)selectedEditButton.Tag;

            // trigger schedule with the given details
            if (selectedProduct != productsList.Where(x => x.ProductId == attractionBookingDTO.AttractionProductId).ToList<ProductsDTO>()[0])
            {
                Control productPanel1 = flpProducts.Controls[":Product:" + selectedProduct.ProductId.ToString()];
                if (productPanel1 != null)
                {
                    Control productButton = productPanel1.Controls["ProductButton:" + selectedProduct.ProductId.ToString()];
                    if (productButton != null)
                    {
                        Button tempButton = ((Button)productButton);
                        tempButton.BackgroundImageLayout = ImageLayout.Stretch;
                        tempButton.BackColor = Color.Transparent;
                        tempButton.ForeColor = Color.Black;
                        tempButton.BackgroundImage = global::Parafait_POS.Properties.Resources.Attraction;
                        tempButton.BackgroundImageLayout = ImageLayout.Stretch;
                        tempButton.BackgroundImage.Tag = tempButton.BackgroundImage;
                    }
                }

                //selectedProduct = productsList.Where(x => x.ProductId == attractionBookingDTO.AttractionProductId).ToList<ProductsDTO>()[0];
                //lblHeaderAttractionProduct.Text = selectedProduct.ProductName;
                //Control productPanel = flpProducts.Controls[":Product:" + selectedProduct.ProductId.ToString()];
                Control productPanel = flpProducts.Controls[":Product:" + attractionBookingDTO.AttractionProductId.ToString()];
                if (productPanel != null)
                {
                    //Control productButton = productPanel.Controls["ProductButton:" + selectedProduct.ProductId.ToString()];
                    Control productButton = productPanel.Controls["ProductButton:" + attractionBookingDTO.AttractionProductId.ToString()];
                    if (productButton != null)
                    {
                        Button tempButton = ((Button)productButton);
                        tempButton.BackgroundImageLayout = ImageLayout.Stretch;
                        tempButton.BackgroundImage = global::Parafait_POS.Properties.Resources.ProductPressed;
                        tempButton.BackgroundImageLayout = ImageLayout.Stretch;
                        tempButton.ForeColor = Color.White;
                        // Nitin : check this
                        tempButton.PerformClick();
                    }
                }
            }

            // set the schedule date, this triggers the data value changed event and the schedules grid is loaded
            if (attractionBookingDTO.ScheduleFromDate.Hour < ParafaitDefaultContainerList.GetParafaitDefault<int>(Utilities.ExecutionContext, "BUSINESS_DAY_START_TIME"))
            {
                selectedDate = dtpAttractionDate.Value = attractionBookingDTO.ScheduleFromDate.Date.AddDays(-1).AddHours(ParafaitDefaultContainerList.GetParafaitDefault<int>(Utilities.ExecutionContext, "BUSINESS_DAY_START_TIME"));
            }
            else
            {
                selectedDate = dtpAttractionDate.Value = attractionBookingDTO.ScheduleFromDate.Date.AddHours(ParafaitDefaultContainerList.GetParafaitDefault<int>(Utilities.ExecutionContext, "BUSINESS_DAY_START_TIME"));
            }

            // highlight the selected schedule line
            if (flpSchedules != null && flpSchedules.Children.Count > 0)
            {
                usrCtrlAttractionScheduleDetails AttractionScheduleDetails = FindChild<usrCtrlAttractionScheduleDetails>(flpSchedules, "DTOID" + attractionBookingDTO.AttractionScheduleId.ToString() + "FacMpId" + attractionBookingDTO.FacilityMapId.ToString());
                if (AttractionScheduleDetails != null)
                {
                    //flpSchedules.AutoScrollPosition = new Point(AttractionScheduleDetails.Left, AttractionScheduleDetails.Top);
                    flpScheduleLine_Click(AttractionScheduleDetails, new EventArgs());
                }
            }

            log.LogMethodExit();
            return;
        }

        private void btnSeatBookingButton_MouseDown(object sender, MouseEventArgs e)
        {
            ((Button)sender).BackgroundImage = global::Parafait_POS.Properties.Resources.Seat_Icon_Hover;
        }
        private void btnSeatBookingButton_MouseUp(object sender, MouseEventArgs e)
        {
            ((Button)sender).BackgroundImage = global::Parafait_POS.Properties.Resources.Seat_Icon_Normal;
        }

        private void btnSeatBookingButton_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            AttractionBookingDTO attractionBookingDTO = (AttractionBookingDTO)((Button)sender).Tag;
            List<int> selectedSeats = new List<int>();
            List<int> currentSeats = new List<int>();
            if (attractionBookingDTO.AttractionBookingSeatsDTOList != null && attractionBookingDTO.AttractionBookingSeatsDTOList.Any())
            {
                foreach (AttractionBookingSeatsDTO seat in attractionBookingDTO.AttractionBookingSeatsDTOList)
                {
                    if (seat.SeatId > -1)
                        selectedSeats.Add(seat.SeatId);
                }
            }

            // add selected seats from other products for same slot to show
            List<AttractionBookingDTO> otherProductsSeats = selectedAttractionBookings.Where(x => x.FacilityMapId == attractionBookingDTO.FacilityMapId
                                                                                            && x.ScheduleFromDate == attractionBookingDTO.ScheduleFromDate
                                                                                            && x.ScheduleToDate == attractionBookingDTO.ScheduleToDate
                                                                                            && x.AttractionProductId != attractionBookingDTO.AttractionProductId)
                                                                                            .ToList();
            foreach (AttractionBookingDTO tempDTO in otherProductsSeats)
            {
                foreach (AttractionBookingSeatsDTO seat in tempDTO.AttractionBookingSeatsDTOList)
                {
                    if (seat.SeatId > -1)
                        currentSeats.Add(seat.SeatId);
                }
            }

            using (AttractionSeatBooking asb = new AttractionSeatBooking(attractionBookingDTO.AttractionScheduleId,
                                                                         attractionBookingDTO.FacilityMapId,
                                                                         attractionBookingDTO.ScheduleFromDate,
                                                                         attractionBookingDTO.ScheduleToDate,
                                                                         attractionBookingDTO.BookedUnits,
                                                                         selectedSeats,
                                                                         currentSeats))
            {
                if (asb.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (asb.GetSelectedSeatIds != null && asb.GetSelectedSeatIds.Count > 0)
                    {
                        for (int i = 0; i < asb.GetSelectedSeatIds.Count; i++)
                        {
                            int seat = asb.GetSelectedSeatIds[i];
                            String seatName = String.Empty;
                            try
                            {
                                seatName = asb.GetSelectedSeatNames[i];
                            }
                            catch
                            { }

                            if (selectedSeats.Count > 0)
                            {
                                if (!attractionBookingDTO.AttractionBookingSeatsDTOList.Where(x => x.SeatId == seat).ToList().Any())
                                {
                                    AttractionBookingSeatsDTO seatsDTO = new AttractionBookingSeatsDTO();
                                    seatsDTO.SeatId = seat;
                                    seatsDTO.SeatName = seatName;
                                    attractionBookingDTO.AttractionBookingSeatsDTOList.Add(seatsDTO);
                                }
                                else
                                {
                                    selectedSeats.Remove(seat);
                                }
                            }
                            else
                            {
                                AttractionBookingSeatsDTO seatsDTO = new AttractionBookingSeatsDTO();
                                seatsDTO.SeatId = seat;
                                seatsDTO.SeatName = seatName;
                                attractionBookingDTO.AttractionBookingSeatsDTOList.Add(seatsDTO);
                            }
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        private void ProductButtonClickEventHandler()
        {
            log.LogMethodEntry();
            this.SuspendLayout();
            LoadScheduleGrid();
            this.ResumeLayout(true);
            log.LogMethodExit();
        }

        private void LoadProductsButton()
        {
            POSUtils.logPOSDebug("PB1:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            log.LogMethodEntry();
            flpProducts.Controls.Clear();
            flpProducts.SuspendLayout();

            FlowLayoutPanel productHeaderPanel = new FlowLayoutPanel();
            productHeaderPanel.FlowDirection = FlowDirection.LeftToRight;
            productHeaderPanel.Width = flpProducts.Width;
            productHeaderPanel.Height = 35;
            productHeaderPanel.BackColor = System.Drawing.Color.SlateGray;
            productHeaderPanel.Margin = new Padding(0);

            Label lblAttractionProduct = new Label();
            lblAttractionProduct.Text = "Attraction Product";
            lblAttractionProduct.BackColor = System.Drawing.Color.Transparent;
            lblAttractionProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            lblAttractionProduct.ForeColor = System.Drawing.Color.Black;
            lblAttractionProduct.Size = new System.Drawing.Size(100, 35);
            lblAttractionProduct.Font = new System.Drawing.Font("Arial", 9F, FontStyle.Bold);
            lblAttractionProduct.Margin = new Padding(5, 2, 2, 2);
            lblAttractionProduct.TextAlign = ContentAlignment.MiddleCenter;
            lblAttractionProduct.Left = productHeaderPanel.Left;

            Label lblQuantity = new Label();
            lblQuantity.Text = "Units Booked";
            lblQuantity.BackColor = System.Drawing.Color.Transparent;
            lblQuantity.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            lblQuantity.ForeColor = System.Drawing.Color.Black;
            lblQuantity.Size = new System.Drawing.Size(75, 35);
            lblQuantity.Font = new System.Drawing.Font("Arial", 9F, FontStyle.Bold);
            lblQuantity.Margin = new Padding(17, 2, 2, 2);
            lblQuantity.TextAlign = ContentAlignment.MiddleCenter;
            lblQuantity.Left = lblAttractionProduct.Right;

            POSUtils.logPOSDebug("PB2:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            productHeaderPanel.Controls.Add(lblAttractionProduct);
            productHeaderPanel.Controls.Add(lblQuantity);
            flpProducts.Controls.Add(productHeaderPanel);
            POSUtils.logPOSDebug("PB3:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            int productButtonY = productHeaderPanel.Bottom; ;
            // Step 1 - Create Product Buttons
            for (int i = 0; i < productsList.Count; i++)
            {
                POSUtils.logPOSDebug("PB1:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                ProductsDTO productsDTO = productsList[i];
                FlowLayoutPanel productPanel = new FlowLayoutPanel();
                productPanel.FlowDirection = FlowDirection.LeftToRight;
                productPanel.Width = flpProducts.Width;
                productPanel.Height = 80;
                productPanel.Name = ":Product:" + productsDTO.ProductId.ToString();
                productPanel.BackColor = System.Drawing.Color.Transparent;
                productPanel.Top = productButtonY;
                productButtonY += productPanel.Height;

                Button ProductButton = new Button();
                ProductButton.Click += new EventHandler(ProductButton_Click);
                ProductButton.MouseDown += ProductButton_MouseDown;
                ProductButton.MouseUp += ProductButton_MouseUp;
                ProductButton.Name = "ProductButton:" + productsDTO.ProductId.ToString();
                ProductButton.Text = productsDTO.ProductName.Replace("&", "&&");
                ProductButton.Tag = productsDTO.ProductId;
                ProductButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                ProductButton.FlatAppearance.BorderColor = System.Drawing.Color.White;
                ProductButton.FlatAppearance.BorderSize = 0;
                ProductButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
                ProductButton.FlatAppearance.MouseDownBackColor = ProductButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
                ProductButton.BackColor = Color.Transparent;
                ProductButton.Size = new System.Drawing.Size(100, 79);
                ProductButton.BackgroundImage = (selectedProduct != null && selectedProduct.ProductId == productsDTO.ProductId) ?
                                                  global::Parafait_POS.Properties.Resources.ProductPressed
                                                : global::Parafait_POS.Properties.Resources.Attraction;
                ProductButton.BackgroundImageLayout = ImageLayout.Stretch;
                ProductButton.BackgroundImage.Tag = ProductButton.BackgroundImage;
                ProductButton.ForeColor = System.Drawing.Color.Black;
                ProductButton.Font = new System.Drawing.Font("Tahoma", 9F);
                ProductButton.Top = productPanel.Top;
                ProductButton.Left = productPanel.Left;

                // Create the ToolTip and associate with the Form container.
                ToolTip toolTip = new ToolTip();
                // Set up the delays for the ToolTip.
                toolTip.AutoPopDelay = 5000;
                toolTip.InitialDelay = 1000;
                toolTip.ReshowDelay = 500;
                // Force the ToolTip text to be displayed whether or not the form is active.
                toolTip.ShowAlways = true;
                // Set up the ToolTip text for the Button
                toolTip.SetToolTip(ProductButton, productsDTO.Description.ToString());

                int bookedQty = this.GetTotalBookedQuantity(productsDTO.ProductId);
                int totalQty = quantityMap[productsDTO.ProductId];

                Label lblBookedForProduct = new Label();
                lblBookedForProduct.Name = "LabelBooked:" + productsDTO.ProductId.ToString();
                lblBookedForProduct.Text = bookedQty + "/" + totalQty;
                lblBookedForProduct.Tag = productsDTO.ProductId;
                lblBookedForProduct.BackColor = (totalQty - bookedQty == 0) ? System.Drawing.Color.LightGreen : System.Drawing.Color.LightSlateGray;
                lblBookedForProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                lblBookedForProduct.ForeColor = System.Drawing.Color.Black;
                lblBookedForProduct.Size = new System.Drawing.Size(75, 85);
                lblBookedForProduct.Font = new System.Drawing.Font("Arial", 11F, FontStyle.Bold);
                lblBookedForProduct.Margin = new Padding(15, 2, 0, 10);
                lblBookedForProduct.TextAlign = ContentAlignment.MiddleCenter;
                lblBookedForProduct.Left = ProductButton.Right;

                productPanel.Controls.Add(ProductButton);
                productPanel.Controls.Add(lblBookedForProduct);
                flpProducts.Controls.Add(productPanel);
                POSUtils.logPOSDebug("PB2:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            }

            flpProducts.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
            flpProducts.ResumeLayout(true);
            log.LogMethodExit();
        }

        private void LoadSummaryPanel()
        {
            log.LogMethodEntry();
            flpSummary.Controls.Clear();
            flpSummary.SuspendLayout();
            flpSummary.FlowDirection = FlowDirection.TopDown;
            flpSummary.BackColor = Color.Transparent;
            flpSummary.BorderStyle = BorderStyle.FixedSingle;
            int defaultHeight = 12;
            int panelHeight = 26;

            DateTime currentDateSelection = DateTime.MinValue;
            int currentProductSelection = -1;
            int currentFacilitySelection = -1;
            // step 1 sort the selected slots
            selectedAttractionBookings = selectedAttractionBookings.OrderBy(x => x.ScheduleFromDate.Date)
                                                                                   .ThenBy(x => x.FacilityMapId)
                                                                                   .ThenBy(x => x.AttractionProductId)
                                                                                   .ThenBy(x => x.ScheduleFromTime)
                                                                                   .ToList();

            FlowLayoutPanel datePanel = null;
            FlowLayoutPanel detailsPanel = null;
            FlowLayoutPanel facilityMapPanel = null;

            int facilityMapCounter = 0;

            foreach (AttractionBookingDTO attractionBookingDTO in selectedAttractionBookings)
            {
                // ignore if this does not have any quantity
                if (attractionBookingDTO.BookedUnits <= 0)
                    continue;

                int datePanelY = 0;
                if (currentDateSelection.Date != attractionBookingDTO.ScheduleFromDate.Date)
                {
                    facilityMapCounter = 0;
                    if (facilityMapPanel != null)
                    {
                        detailsPanel.Height += facilityMapPanel.Height;
                        detailsPanel.Controls.Add(facilityMapPanel);
                    }
                    if (detailsPanel != null)
                    {
                        datePanel.Height += detailsPanel.Height;
                        datePanel.Controls.Add(detailsPanel);
                        datePanelY = datePanel.Bottom;
                    }
                    if (datePanel != null)
                    {
                        flpSummary.Controls.Add(datePanel);
                        datePanel = null;
                        detailsPanel = null;
                        facilityMapPanel = null;
                    }

                    // date has changed, reset the counters
                    currentDateSelection = attractionBookingDTO.ScheduleFromDate;
                    currentProductSelection = -1;
                    currentFacilitySelection = -1;

                    // Create a new flp
                    datePanel = new FlowLayoutPanel();
                    datePanel.Width = flpSummary.Width - 2;
                    datePanel.FlowDirection = FlowDirection.TopDown;
                    datePanel.Name = "DatePanel:" + currentDateSelection.Date.ToString("MM/dd/yyyy");
                    datePanel.Margin = new Padding(0);
                    datePanel.Top = datePanelY + 5;
                    datePanel.Height = defaultHeight;
                    datePanel.WrapContents = false;
                    datePanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;

                    detailsPanel = new FlowLayoutPanel();
                    detailsPanel.Width = flpSummary.Width - 2;
                    detailsPanel.FlowDirection = FlowDirection.TopDown;
                    detailsPanel.Name = "DatePlusDetails:" + currentDateSelection.Date.ToString("MM/dd/yyyy");
                    detailsPanel.Margin = new Padding(0);
                    detailsPanel.Height = defaultHeight;

                    FlowLayoutPanel dateHeader = new FlowLayoutPanel();
                    dateHeader.Width = (datePanel.Width);
                    dateHeader.Height = panelHeight;
                    dateHeader.FlowDirection = FlowDirection.LeftToRight;
                    dateHeader.Margin = new Padding(0, 0, 0, 0);
                    dateHeader.Left = datePanel.Left;
                    dateHeader.Top = datePanel.Top;
                    dateHeader.BorderStyle = BorderStyle.None;
                    dateHeader.BackColor = Color.LightSteelBlue;

                    Label lblDate = new Label();
                    lblDate.Height = dateHeader.Height;
                    lblDate.Width = (int)(dateHeader.Width * 0.8);
                    lblDate.Text = currentDateSelection.Date.ToString("dddd, dd MMMM yyyy");
                    lblDate.BackColor = System.Drawing.Color.Transparent;
                    lblDate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    lblDate.ForeColor = System.Drawing.Color.Black;
                    lblDate.Font = new System.Drawing.Font("Arial", 9F, FontStyle.Bold);
                    lblDate.Margin = new Padding(5, 0, 0, 0);
                    lblDate.TextAlign = ContentAlignment.MiddleLeft;
                    lblDate.BorderStyle = BorderStyle.None;
                    lblDate.Left = dateHeader.Left + dateHeader.Width;
                    lblDate.Top = dateHeader.Top;
                    dateHeader.Controls.Add(lblDate);

                    datePanel.Controls.Add(dateHeader);
                    detailsPanel.Left = datePanel.Left;
                    detailsPanel.Top = dateHeader.Bottom;
                }

                if (currentFacilitySelection != attractionBookingDTO.FacilityMapId)
                {
                    facilityMapCounter++;
                    int facilityY = 0;
                    if (facilityMapPanel != null)
                    {
                        detailsPanel.Height += facilityMapPanel.Height;
                        detailsPanel.Controls.Add(facilityMapPanel);
                        facilityY = facilityMapPanel.Bottom;
                    }

                    currentProductSelection = -1;
                    currentFacilitySelection = attractionBookingDTO.FacilityMapId;

                    facilityMapPanel = new FlowLayoutPanel();
                    facilityMapPanel.Width = detailsPanel.Width;
                    facilityMapPanel.FlowDirection = FlowDirection.TopDown;
                    facilityMapPanel.Left = detailsPanel.Left;
                    facilityMapPanel.Top = facilityY;
                    facilityMapPanel.Name = "FacilityPlusDetails:" + currentDateSelection.Date.ToString("MM/dd/yyyy") + ":" + currentFacilitySelection;
                    facilityMapPanel.Margin = new Padding(0);
                    facilityMapPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
                    facilityMapPanel.Height = defaultHeight;
                    facilityMapPanel.BackColor = facilityMapCounter % 2 == 0 ? Color.Ivory : Color.LightSkyBlue;

                    FlowLayoutPanel facilityMapheader = new FlowLayoutPanel();
                    facilityMapheader.Width = facilityMapPanel.Width;
                    facilityMapheader.Height = panelHeight;
                    facilityMapheader.FlowDirection = FlowDirection.LeftToRight;
                    facilityMapheader.Margin = new Padding(0);
                    facilityMapheader.Left = facilityMapPanel.Left;
                    facilityMapheader.Top = facilityMapPanel.Top;
                    facilityMapheader.BorderStyle = BorderStyle.None;

                    Label lblSpacing = new Label();
                    lblSpacing.Height = facilityMapheader.Height;
                    lblSpacing.Width = 5;
                    lblSpacing.Text = "";
                    lblSpacing.BackColor = System.Drawing.Color.Transparent;
                    lblSpacing.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    lblSpacing.ForeColor = System.Drawing.Color.Black;
                    lblSpacing.Font = new System.Drawing.Font("Arial", 11F, FontStyle.Bold);
                    lblSpacing.Margin = new Padding(0, 0, 0, 0);
                    lblSpacing.TextAlign = ContentAlignment.MiddleLeft;
                    lblSpacing.BorderStyle = BorderStyle.None;
                    lblSpacing.Left = facilityMapheader.Right;
                    lblSpacing.Top = facilityMapheader.Top;
                    facilityMapheader.Controls.Add(lblSpacing);

                    Label lblFacilityName = new Label();
                    lblFacilityName.Height = facilityMapheader.Height;
                    lblFacilityName.Width = (int)(facilityMapheader.Width * 0.80);
                    lblFacilityName.Text = facilityMapNames[currentFacilitySelection];
                    lblFacilityName.BackColor = System.Drawing.Color.Transparent;
                    lblFacilityName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    lblFacilityName.ForeColor = System.Drawing.Color.Black;
                    lblFacilityName.Font = new System.Drawing.Font("Arial", 9F, FontStyle.Bold);
                    lblFacilityName.Margin = new Padding(5, 0, 0, 0);
                    lblFacilityName.TextAlign = ContentAlignment.MiddleLeft;
                    lblFacilityName.BorderStyle = BorderStyle.None;
                    lblFacilityName.Left = lblSpacing.Right;
                    lblFacilityName.Top = facilityMapheader.Top;
                    facilityMapheader.Controls.Add(lblFacilityName);

                    facilityMapPanel.Height += facilityMapheader.Height;
                    facilityMapPanel.Controls.Add(facilityMapheader);
                }

                if (currentProductSelection != attractionBookingDTO.AttractionProductId)
                {
                    currentProductSelection = attractionBookingDTO.AttractionProductId;

                    FlowLayoutPanel productHeader = new FlowLayoutPanel();
                    productHeader.Width = facilityMapPanel.Width;
                    productHeader.Height = panelHeight;
                    productHeader.FlowDirection = FlowDirection.LeftToRight;
                    productHeader.Margin = new Padding(0);
                    productHeader.Left = facilityMapPanel.Left;
                    productHeader.Top = facilityMapPanel.Top;
                    productHeader.BorderStyle = BorderStyle.None;

                    Label lblSpacing = new Label();
                    lblSpacing.Height = productHeader.Height;
                    lblSpacing.Width = 10;
                    lblSpacing.Text = "";
                    lblSpacing.BackColor = System.Drawing.Color.Transparent;
                    lblSpacing.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    lblSpacing.ForeColor = System.Drawing.Color.Black;
                    lblSpacing.Font = new System.Drawing.Font("Arial", 9F, FontStyle.Bold);
                    lblSpacing.Margin = new Padding(0, 0, 0, 0);
                    lblSpacing.TextAlign = ContentAlignment.MiddleLeft;
                    lblSpacing.BorderStyle = BorderStyle.None;
                    lblSpacing.Left = productHeader.Right;
                    lblSpacing.Top = productHeader.Top;
                    productHeader.Controls.Add(lblSpacing);

                    Label lblProductName = new Label();
                    lblProductName.Height = productHeader.Height;
                    lblProductName.Width = (int)(productHeader.Width * 0.80);
                    lblProductName.Text = (productsList.Where(x => x.ProductId == attractionBookingDTO.AttractionProductId).ToList<ProductsDTO>())[0].ProductName;
                    lblProductName.BackColor = System.Drawing.Color.Transparent;
                    lblProductName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    lblProductName.ForeColor = System.Drawing.Color.Black;
                    lblProductName.Font = new System.Drawing.Font("Arial", 9F, FontStyle.Regular);
                    lblProductName.Margin = new Padding(5, 0, 0, 0);
                    lblProductName.TextAlign = ContentAlignment.MiddleLeft;
                    lblProductName.BorderStyle = BorderStyle.None;
                    lblProductName.Left = lblSpacing.Right;
                    lblProductName.Top = productHeader.Top;
                    productHeader.Controls.Add(lblProductName);

                    facilityMapPanel.Height += productHeader.Height;
                    facilityMapPanel.Controls.Add(productHeader);
                }

                {
                    FlowLayoutPanel scheduleDetailsPanel = new FlowLayoutPanel();
                    scheduleDetailsPanel.Width = facilityMapPanel.Width;
                    scheduleDetailsPanel.Height = 32;
                    scheduleDetailsPanel.FlowDirection = FlowDirection.LeftToRight;
                    scheduleDetailsPanel.Margin = new Padding(0);
                    scheduleDetailsPanel.Left = facilityMapPanel.Left;
                    scheduleDetailsPanel.BorderStyle = BorderStyle.None;
                    scheduleDetailsPanel.Name = "FacilityPanel : " + currentDateSelection.Date.ToString("MM/dd/yyyy") + ":" + attractionBookingDTO.FacilityMapId + ":" + attractionBookingDTO.AttractionScheduleId;

                    Label lblSpacing = new Label();
                    lblSpacing.Height = scheduleDetailsPanel.Height;
                    lblSpacing.Width = 15;
                    lblSpacing.Text = "";
                    lblSpacing.BackColor = System.Drawing.Color.Transparent;
                    lblSpacing.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    lblSpacing.ForeColor = System.Drawing.Color.Black;
                    lblSpacing.Font = new System.Drawing.Font("Arial", 9F, FontStyle.Regular);
                    lblSpacing.Margin = new Padding(0, 0, 0, 0);
                    lblSpacing.TextAlign = ContentAlignment.MiddleLeft;
                    lblSpacing.BorderStyle = BorderStyle.None;
                    lblSpacing.Left = scheduleDetailsPanel.Right;
                    lblSpacing.Top = scheduleDetailsPanel.Top;
                    scheduleDetailsPanel.Controls.Add(lblSpacing);

                    //bookingDTO
                    Label lblScheduleDetails = new Label();
                    lblScheduleDetails.Height = scheduleDetailsPanel.Height;
                    lblScheduleDetails.Width = (int)(scheduleDetailsPanel.Width * 0.45);
                    lblScheduleDetails.AutoSize = false;
                    int hours = Decimal.ToInt32(attractionBookingDTO.ScheduleFromTime);
                    int minutes = (int)((attractionBookingDTO.ScheduleFromTime - hours) * 100);
                    DateTime fromTime = this.selectedDate.Date.AddMinutes(hours * 60 + minutes);
                    hours = Decimal.ToInt32(attractionBookingDTO.ScheduleToTime);
                    minutes = (int)((attractionBookingDTO.ScheduleToTime - hours) * 100);
                    DateTime toTime = this.selectedDate.Date.AddMinutes(hours * 60 + minutes);

                    lblScheduleDetails.Text = "Time : " + fromTime.ToString("hh:mm tt") + "\nUnits : " + attractionBookingDTO.BookedUnits;
                    lblScheduleDetails.BackColor = System.Drawing.Color.Transparent;
                    lblScheduleDetails.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    lblScheduleDetails.ForeColor = System.Drawing.Color.Black;
                    lblScheduleDetails.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                    lblScheduleDetails.Margin = new Padding(5, 0, 0, 0);
                    lblScheduleDetails.TextAlign = ContentAlignment.MiddleLeft;
                    lblScheduleDetails.BorderStyle = BorderStyle.None;
                    lblScheduleDetails.Left = lblSpacing.Right;
                    scheduleDetailsPanel.Controls.Add(lblScheduleDetails);

                    // Allow edit only if the schedule is not in the past
                    if ((attractionBookingDTO.BookingId == -1) ||
                        (attractionBookingDTO.BookingId != -1 && fromTime > Utilities.getServerTime().AddMinutes(POSStatic.ATTRACTION_BOOKING_GRACE_PERIOD)))
                    {
                        Button btnScheduleEditButton = new Button();
                        btnScheduleEditButton.Click += new EventHandler(btnScheduleEditButton_Click);
                        btnScheduleEditButton.MouseDown += new System.Windows.Forms.MouseEventHandler(btnScheduleEditButton_MouseDown);
                        btnScheduleEditButton.MouseUp += new System.Windows.Forms.MouseEventHandler(btnScheduleEditButton_MouseUp);
                        btnScheduleEditButton.Name = "ScheduleEditButton:" + currentDateSelection.Date.ToString("MM/dd/yyyy") + ":" + attractionBookingDTO.Identifier;
                        btnScheduleEditButton.TextAlign = ContentAlignment.BottomCenter;
                        btnScheduleEditButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                        btnScheduleEditButton.FlatAppearance.BorderColor = System.Drawing.Color.White;
                        btnScheduleEditButton.FlatAppearance.BorderSize = 0;
                        btnScheduleEditButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
                        btnScheduleEditButton.BackColor = Color.Transparent;
                        btnScheduleEditButton.BackgroundImage = global::Parafait_POS.Properties.Resources.Edit_Icon_Normal;
                        btnScheduleEditButton.BackgroundImageLayout = ImageLayout.Stretch;
                        btnScheduleEditButton.ForeColor = System.Drawing.Color.Black;
                        btnScheduleEditButton.Size = new System.Drawing.Size(30, 30);
                        btnScheduleEditButton.Text = "";
                        btnScheduleEditButton.Font = new System.Drawing.Font("Tahoma", 10F);
                        btnScheduleEditButton.Margin = new Padding(2, 0, 0, 0);
                        btnScheduleEditButton.Left = lblScheduleDetails.Right;
                        btnScheduleEditButton.Top = lblSpacing.Top;
                        btnScheduleEditButton.Tag = attractionBookingDTO;
                        if (!isRescheduleSlot)
                            scheduleDetailsPanel.Controls.Add(btnScheduleEditButton);

                        Button btnScheduleDeleteButton = new Button();
                        btnScheduleDeleteButton.Click += new EventHandler(btnClear_Click);
                        btnScheduleDeleteButton.MouseDown += new System.Windows.Forms.MouseEventHandler(btnClear_MouseDown);
                        btnScheduleDeleteButton.MouseUp += new System.Windows.Forms.MouseEventHandler(btnClear_MouseUp);
                        btnScheduleDeleteButton.Name = "ScheduleDeleteButton:" + currentDateSelection.Date.ToString("MM/dd/yyyy") + ":" + attractionBookingDTO.Identifier;
                        btnScheduleDeleteButton.Text = "";
                        btnScheduleDeleteButton.TextAlign = ContentAlignment.BottomCenter;
                        btnScheduleDeleteButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                        btnScheduleDeleteButton.FlatAppearance.BorderColor = System.Drawing.Color.White;
                        btnScheduleDeleteButton.FlatAppearance.BorderSize = 0;
                        btnScheduleDeleteButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
                        btnScheduleDeleteButton.BackgroundImageLayout = ImageLayout.Stretch;
                        btnScheduleDeleteButton.BackColor = Color.Transparent;
                        btnScheduleDeleteButton.BackgroundImage = Properties.Resources.Delete_Icon_Normal;
                        btnScheduleDeleteButton.ForeColor = System.Drawing.Color.Black;
                        btnScheduleDeleteButton.Size = new System.Drawing.Size(30, 30);
                        btnScheduleDeleteButton.Font = new System.Drawing.Font("Tahoma", 9F);
                        btnScheduleDeleteButton.Margin = new Padding(9, 0, 0, 0);
                        btnScheduleDeleteButton.Left = btnScheduleEditButton.Right;
                        btnScheduleDeleteButton.Top = lblScheduleDetails.Top;
                        btnScheduleDeleteButton.Tag = attractionBookingDTO;
                        scheduleDetailsPanel.Controls.Add(btnScheduleDeleteButton);

                        Button btnSeatBookingButton = new Button();
                        btnSeatBookingButton.Click += new EventHandler(btnSeatBookingButton_Click);
                        btnSeatBookingButton.MouseDown += new System.Windows.Forms.MouseEventHandler(btnSeatBookingButton_MouseDown);
                        btnSeatBookingButton.MouseUp += new System.Windows.Forms.MouseEventHandler(btnSeatBookingButton_MouseUp);
                        btnSeatBookingButton.Name = "BookSeatsButton:" + currentDateSelection.Date.ToString("MM/dd/yyyy") + ":" + attractionBookingDTO.Identifier;
                        btnSeatBookingButton.Tag = attractionBookingDTO;
                        btnSeatBookingButton.Text = "";
                        btnSeatBookingButton.TextAlign = ContentAlignment.BottomCenter;
                        btnSeatBookingButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                        btnSeatBookingButton.FlatAppearance.BorderColor = System.Drawing.Color.White;
                        btnSeatBookingButton.FlatAppearance.BorderSize = 0;
                        btnSeatBookingButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
                        btnSeatBookingButton.BackgroundImageLayout = ImageLayout.Stretch;
                        btnSeatBookingButton.BackColor = Color.Transparent;
                        btnSeatBookingButton.BackgroundImage = Properties.Resources.Seat_Icon_Normal;
                        btnSeatBookingButton.ForeColor = System.Drawing.Color.Black;
                        btnSeatBookingButton.Size = new System.Drawing.Size(30, 30);
                        btnSeatBookingButton.Font = new System.Drawing.Font("Tahoma", 9F);
                        btnSeatBookingButton.Margin = new Padding(9, 0, 0, 0);
                        btnSeatBookingButton.Left = btnScheduleDeleteButton.Right;
                        btnSeatBookingButton.Top = lblScheduleDetails.Top;
                        scheduleDetailsPanel.Controls.Add(btnSeatBookingButton);
                    }

                    facilityMapPanel.Height += scheduleDetailsPanel.Height;
                    facilityMapPanel.Controls.Add(scheduleDetailsPanel);
                }
            }

            if (facilityMapPanel != null)
            {
                detailsPanel.Height += facilityMapPanel.Height;
                detailsPanel.Controls.Add(facilityMapPanel);
            }
            if (detailsPanel != null)
            {
                datePanel.Height += detailsPanel.Height;
                datePanel.Controls.Add(detailsPanel);
            }
            if (datePanel != null)
            {
                flpSummary.Controls.Add(datePanel);
            }

            flpSummary.ResumeLayout(true);
            log.LogMethodExit();
        }

        private void LoadScheduleGrid()
        {
            log.LogMethodEntry();
            Cursor = Cursors.WaitCursor;
            try
            {
                //flpSchedules.SuspendLayout();
                flpSchedules.Children.Clear();
                if (scrollViewer != null)
                {
                    scrollViewer.Content = null;
                }
                else
                {
                    scrollViewer = new System.Windows.Controls.ScrollViewer();
                }

                String remarks = "";
                pageNumber = 0;

                flpSchedules.Background = System.Windows.Media.Brushes.Black;
                flpSchedules.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                flpSchedules.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                scrollViewer.Width = flpSchedules.Width;
                scrollViewer.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                scrollViewer.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                scrollViewer.HorizontalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto;
                scrollViewer.VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Hidden;
                scrollViewer.Content = flpSchedules;
                scrollViewer.ScrollChanged += new System.Windows.Controls.ScrollChangedEventHandler(scrollViewer_Scroll);
                //scrollViewer.Content = new TableLayoutButton("Hello", System.Windows.Media.Brushes.Aqua);

                DateTime beforeTime = Utilities.getServerTime();
                DateTime currentTime = Utilities.getServerTime();
                int currentFacilityMapSelection = selectedFacilityMap;// set as this is reset when the facility maps are added
                List<ScheduleDetailsViewDTO> schedules = GetEligibleSchedules(selectedDate,
                    Convert.ToInt32(String.IsNullOrEmpty(ParafaitDefaultContainerList.GetParafaitDefault(this.Utilities.ExecutionContext, "BUSINESS_DAY_START_TIME")) ? "0" : ParafaitDefaultContainerList.GetParafaitDefault(this.Utilities.ExecutionContext, "BUSINESS_DAY_START_TIME"))
                    , 24, this.facilityMapId, selectedProduct.ProductId);

                if (schedules != null && schedules.Any())
                {
                    schedules = schedules.OrderBy(x => x.ScheduleDetailsDTO.ScheduleFromDate).ToList();
                    this.facilityMapDTOList = GetAttractionFacilityMaps(selectedProduct.ProductId);

                    var facilityIdList = schedules.Select(x => x.ScheduleDetailsDTO.FacilityMapId).Distinct();
                    foreach (int tempFMId in facilityIdList)
                    {
                        if (!facilityMapNames.ContainsKey(tempFMId))
                        {
                            FacilityMapBL facilityMapBL = new FacilityMapBL(executionContext, tempFMId);
                            facilityMapNames.Add(tempFMId, facilityMapBL.FacilityMapDTO.FacilityMapName);
                        }

                        if (!relatedFacilityMaps.ContainsKey(tempFMId) && tempFMId > -1)
                        {
                            FacilityMapListBL facilityMapListBL = new FacilityMapListBL(executionContext);
                            relatedFacilityMaps.Add(tempFMId, facilityMapListBL.GetFacilityMapsForSameFacility(tempFMId));
                        }
                    }

                    // default first is selected instead of all
                    //if (currentFacilityMapSelection == -1 && this.facilityMapDTOList.Count > 2)
                    //    currentFacilityMapSelection = facilityMapDTOList[1].FacilityMapId;

                    cmbAttractionFacility.DisplayMember = "FacilityMapName";
                    cmbAttractionFacility.ValueMember = "facilityMapId";
                    cmbAttractionFacility.DataSource = facilityMapDTOList;
                    cmbAttractionFacility.SelectedValue = facilityMapDTOList.Where(x => x.FacilityMapId == currentFacilityMapSelection).ToList().Any() ? currentFacilityMapSelection : this.facilityMapId;

                    int counter = 0;
                    currentTime = Utilities.getServerTime();
                    foreach (ScheduleDetailsViewDTO scheduleDetailsViewDTO in schedules)
                    {
                        string[] RGB = scheduleDetailsViewDTO.BackColor.Split(',');
                        Color tempColor = Color.FromArgb(Convert.ToInt32(RGB[0]), Convert.ToInt32(RGB[1]), Convert.ToInt32(RGB[2]));
                        System.Windows.Media.Color backColor = System.Windows.Media.Color.FromArgb(tempColor.A, tempColor.R, tempColor.G, tempColor.B);

                        int totalUnits = 0;
                        totalUnits = scheduleDetailsViewDTO.ScheduleDetailsDTO.TotalUnits == null ? 0 : Convert.ToInt32(scheduleDetailsViewDTO.ScheduleDetailsDTO.TotalUnits);
                        int desiredUnits = GetTotalBookedQuantityForCurrentBooking(scheduleDetailsViewDTO.ScheduleDetailsDTO);

                        // to avoid double counting in a reschedule scenario
                        if (existingAttractionBooking != null && existingAttractionBooking.Any())
                        {
                            List<AttractionBooking> existingATB = existingAttractionBooking.Where(x => (x.AttractionBookingDTO.AttractionScheduleId == scheduleDetailsViewDTO.ScheduleDetailsDTO.ScheduleId)
                                                                                                            && (x.AttractionBookingDTO.FacilityMapId == scheduleDetailsViewDTO.ScheduleDetailsDTO.FacilityMapId)
                                                                                                            && (x.AttractionBookingDTO.ScheduleFromDate.Date == scheduleDetailsViewDTO.ScheduleDetailsDTO.ScheduleFromDate.Date)
                                                                                                            && (x.AttractionBookingDTO.ScheduleFromTime == scheduleDetailsViewDTO.ScheduleDetailsDTO.ScheduleFromTime)
                                                                                                                ).ToList();
                            if (existingATB != null && existingATB.Any())
                            {
                                scheduleDetailsViewDTO.ScheduleDetailsDTO.BookedUnits = scheduleDetailsViewDTO.ScheduleDetailsDTO.BookedUnits - existingATB.Sum(x => x.AttractionBookingDTO.BookedUnits);
                            }
                        }

                        scheduleDetailsViewDTO.ScheduleDetailsDTO.AvailableUnits = totalUnits < 0 ? 0 : totalUnits - scheduleDetailsViewDTO.ScheduleDetailsDTO.BookedUnits - desiredUnits;

                        System.Windows.Input.MouseButtonEventHandler clickAction = null;
                        if (!scheduleDetailsViewDTO.PastSchedule)
                        {
                            clickAction = flpScheduleLine_Click;
                        }

                        usrCtrlAttractionScheduleDetails usrCtrlAttractionScheduleDetails = new usrCtrlAttractionScheduleDetails(executionContext, Utilities, scheduleDetailsViewDTO.ScheduleDetailsDTO,
                            desiredUnits, backColor, selectedDate, (int)flpSchedules.Width, scheduleDetailsViewDTO.PastSchedule, clickAction);
                        //usrCtrlAttractionScheduleDetails.SuspendLayout();

                        usrCtrlAttractionScheduleDetails.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        usrCtrlAttractionScheduleDetails.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                        usrCtrlAttractionScheduleDetails.Width = (int)flpSchedules.Width;
                        usrCtrlAttractionScheduleDetails.Tag = scheduleDetailsViewDTO.ScheduleDetailsDTO;
                        usrCtrlAttractionScheduleDetails.Name = "DTOID" + scheduleDetailsViewDTO.ScheduleDetailsDTO.ScheduleId.ToString() + "FacMpId" + scheduleDetailsViewDTO.ScheduleDetailsDTO.FacilityMapId.ToString();
                        flpSchedules.Children.Add(usrCtrlAttractionScheduleDetails);
                        //usrCtrlAttractionScheduleDetails.ResumeLayout(false);
                        counter++;
                    }


                    //add a tail to show bottom margin
                    //System.Windows.Controls.WrapPanel scheduleLinePanelTail = new System.Windows.Controls.WrapPanel();
                    //scheduleLinePanelTail.Width = (int)flpSchedules.Width;
                    //scheduleLinePanelTail.Height = 32;
                    //scheduleLinePanelTail.FlowDirection = System.Windows.FlowDirection.LeftToRight;
                    //scheduleLinePanelTail.Background = System.Windows.Media.Brushes.Red;


                    //System.Windows.Controls.Label lblTail = new System.Windows.Controls.Label();
                    //lblTail.Height = scheduleLinePanelTail.Height;
                    //lblTail.Width = scheduleLinePanelTail.Width;
                    //lblTail.Background = System.Windows.Media.Brushes.Blue;
                    //lblTail.Margin = new System.Windows.Thickness(1, 1, 0, 1);
                    //lblTail.Content = "";
                    //scheduleLinePanelTail.Children.Add(lblTail);
                    //flpSchedules.Children.Add(scheduleLinePanelTail);
                    //flpSchedules.Margin = new System.Windows.Thickness(1, 1, 0, 1);


                }
                if (facilityMapDTOList != null)
                    selectedFacilityMap = facilityMapDTOList.Where(x => x.FacilityMapId == currentFacilityMapSelection).ToList().Any() ? currentFacilityMapSelection : -1;

                if (!String.IsNullOrEmpty(remarks))
                    lblErrorMessage.Text = remarks;

                FilterSchedulePerFacilityAndTimeSlotSelection();
                scrollViewer_Scroll(this, null);
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
                lblErrorMessage.Text = "Encountered an error while building schedules";
            }

            //flpSchedules.ResumeLayout(true);
            //lblErrorMessage.Text = (firstTime - beforeTime).Seconds.ToString() + ":" + (currentTime - beforeTime).Seconds.ToString() + ":" + (Utilities.getServerTime() - currentTime).Seconds.ToString();
            Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private List<ScheduleDetailsDTO> RemoveSchedulesOutOfRange(List<ScheduleDetailsDTO> schedules, decimal fromTime, decimal toTime)
        {
            log.LogMethodEntry(schedules, fromTime, toTime);
            List<ScheduleDetailsDTO> scheduleList = null;
            if (schedules != null && schedules.Any())
            {
               scheduleList = schedules.Where(sch => sch.ScheduleFromTime >= fromTime && sch.ScheduleToTime <= toTime).ToList();
            }
            log.LogMethodExit(scheduleList);
            return scheduleList;
        }

        private void RefreshView()
        {
            log.LogMethodEntry();
            // To do, refresh the booked units counts here
            this.SuspendLayout();
            LoadScheduleGrid();
            this.ResumeLayout(true);
            log.LogMethodExit();
        }

        private void FilterSchedulePerFacilityAndTimeSlotSelection()
        {
            log.LogMethodEntry();
            if (flpSchedules != null && flpSchedules.Children.Count > 0)
            {
                int startPosition = pageNumber * slotsPerPage;
                int endPosition = startPosition + slotsPerPage;
                int counter = 0;
                int height = 1;
                for (int i = 0; i < flpSchedules.Children.Count; i++)
                {
                    if (!(flpSchedules.Children[i] is usrCtrlAttractionScheduleDetails))
                        continue;

                    usrCtrlAttractionScheduleDetails AttractionScheduleDetails = (usrCtrlAttractionScheduleDetails)flpSchedules.Children[i];
                    if (selectedFacilityMap < 0)
                    {
                        if (!AttractionScheduleDetails.IsVisible)
                        {
                            if (counter >= startPosition && counter <= endPosition)
                            {
                                //counter++;
                                AttractionScheduleDetails.Visibility = System.Windows.Visibility.Visible;
                                height += 32;
                            }
                            else
                            {
                                AttractionScheduleDetails.Visibility = System.Windows.Visibility.Collapsed;
                            }
                        }
                        else
                        {
                            height += 32;
                        }
                    }
                    else
                    {
                        ScheduleDetailsDTO scheduleDetailsDTO = (ScheduleDetailsDTO)AttractionScheduleDetails.Tag;
                        if (scheduleDetailsDTO.FacilityMapId == selectedFacilityMap)
                        {
                            if (counter >= startPosition && counter <= endPosition)
                            {
                                //counter++;
                                AttractionScheduleDetails.Visibility = System.Windows.Visibility.Visible;
                                height += 32;
                            }
                            else
                            {
                                AttractionScheduleDetails.Visibility = System.Windows.Visibility.Collapsed;
                            }
                        }
                        else
                            AttractionScheduleDetails.Visibility = System.Windows.Visibility.Collapsed;
                    }
                }
                flpSchedules.Height = height;
                scrollViewer.Height = height;

                // setting this here so that it is set from all calling functions
                ehSchedules.Child = scrollViewer;
                vsbSchedule.ScrollViewer = scrollViewer;

                // doing this to force a redraw of the layout, Else the schedules do not appear
                if(this.Width < Screen.PrimaryScreen.WorkingArea.Width)
                    this.Width = Screen.PrimaryScreen.WorkingArea.Width;
                else
                    this.Width -= 1;
                //this.OnResize(null);
            }
            log.LogMethodExit();
        }

        private void scrollViewer_Scroll(object sender, System.Windows.Controls.ScrollChangedEventArgs e)
        {
            // doing this to force a redraw of the layout, Else the schedules do not appear
            if (this.Width < Screen.PrimaryScreen.WorkingArea.Width)
                this.Width = Screen.PrimaryScreen.WorkingArea.Width;
            else
                this.Width -= 1;
        }

        private void AttractionScheduleNew_Resize(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            int Height = (int)(Screen.PrimaryScreen.WorkingArea.Height * 0.95);
            int Width = (int)(Screen.PrimaryScreen.WorkingArea.Width * 0.98);

            flpProducts.Width = !isRescheduleSlot ? 225 : 0;
            flpProducts.Height = Height - lblErrorMessage.Height;

            panelRight.Width = 250;
            panelRight.Height = Height - flpScreenHeader.Height;

            flpScreenHeader.Width = Width - flpProducts.Width;
            flpScheduleHeader.Width = (Width - panelRight.Width - flpProducts.Width - vsbSchedule.Width);
            ehSchedules.Width = Width - panelRight.Width - flpProducts.Width - vsbSchedule.Width;
            if(scrollViewer != null)
            {
                scrollViewer.Width = ehSchedules.Width;
                if (scrollViewer.Content != null)
                {
                    ((System.Windows.Controls.WrapPanel)scrollViewer.Content).Width = scrollViewer.Width;
                    foreach(System.Windows.UIElement element in ((System.Windows.Controls.WrapPanel)scrollViewer.Content).Children)
                    {
                        ((usrCtrlAttractionScheduleDetails)element).Width = scrollViewer.Width;
                    }
                }
            }

            panelRight.Top = flpScreenHeader.Bottom;
            panelButtons.Width = flpSummary.Width = panelRight.Width;
            flpLegends.Height = lblLegend.Height + lblStatusBlocked.Height * 6;
            lblLegend.Width = lblStatusBlocked.Width = lblStatusInUse.Width = lblStatusPartyReservation.Width = lblStatusRescheduleInProgress.Width
                = lblStatusSoldOut.Width = lblStatusUnavailable.Width = flpLegends.Width;
            flpSummary.Height = panelRight.Height- flpLegends.Height - panelButtons.Height;
            flpLegends.Top = flpSummary.Bottom;
            panelButtons.Top = flpLegends.Bottom;
            lblErrorMessage.Top = flpProducts.Bottom;
            lblErrorMessage.Left = flpProducts.Left;
            lblErrorMessage.Width = flpProducts.Width + (int)ehSchedules.Width + vsbSchedule.Width;

            flpScheduleHeader.Height = flpScreenHeader.Height;
            flpScheduleHeader.Top = flpScreenHeader.Bottom;

            ehSchedules.Top = flpScheduleHeader.Bottom;
            ehSchedules.Height = (int)(Height) - flpScreenHeader.Height - flpScheduleHeader.Height - lblErrorMessage.Height;
            vsbSchedule.Height = (int)ehSchedules.Height + flpScheduleHeader.Height;
            vsbSchedule.Left = ehSchedules.Right;
            vsbSchedule.Top = flpScheduleHeader.Top;
            panelRight.Left = vsbSchedule.Right;
            flpScreenHeader.Left = flpScheduleHeader.Left = flpProducts.Right;
            ehSchedules.Left = flpProducts.Right;

            log.LogMethodExit();
        }

        private void AddBookingDTOToTreeNodesList(AttractionBookingDTO attractionBookingDTO)
        {
            log.LogMethodEntry(attractionBookingDTO);
            List<AttractionBookingDTO> existingBookingDTO = this.selectedAttractionBookings.Where(x => (x.AttractionScheduleId == attractionBookingDTO.AttractionScheduleId)
                                                                                            && (x.FacilityMapId == attractionBookingDTO.FacilityMapId)
                                                                                            && (x.ScheduleFromDate.Date == attractionBookingDTO.ScheduleFromDate.Date)
                                                                                            && (x.ScheduleFromTime == attractionBookingDTO.ScheduleFromTime)
                                                                                            && (x.AttractionProductId == attractionBookingDTO.AttractionProductId)
                                                                                                ).ToList();
            if (existingBookingDTO.Any())
            {
                existingBookingDTO[0].BookedUnits += attractionBookingDTO.BookedUnits;
                if (attractionBookingDTO.AttractionBookingSeatsDTOList.Any())
                {
                    existingBookingDTO[0].AttractionBookingSeatsDTOList.AddRange(attractionBookingDTO.AttractionBookingSeatsDTOList);
                }
            }
            else
            {
                AttractionBookingDTO selectedattractionBookingDTO = new AttractionBookingDTO();
                selectedattractionBookingDTO.AttractionProductId = attractionBookingDTO.AttractionProductId;
                selectedattractionBookingDTO.FacilityMapId = attractionBookingDTO.FacilityMapId;
                selectedattractionBookingDTO.AttractionPlayId = attractionBookingDTO.AttractionPlayId;
                selectedattractionBookingDTO.AttractionPlayName = attractionBookingDTO.AttractionPlayName;
                selectedattractionBookingDTO.AttractionScheduleId = attractionBookingDTO.AttractionScheduleId;
                selectedattractionBookingDTO.AttractionScheduleName = attractionBookingDTO.AttractionScheduleName;
                selectedattractionBookingDTO.ScheduleFromDate = attractionBookingDTO.ScheduleFromDate;
                selectedattractionBookingDTO.ScheduleToDate = attractionBookingDTO.ScheduleToDate;
                selectedattractionBookingDTO.BookedUnits = attractionBookingDTO.BookedUnits;
                selectedattractionBookingDTO.ScheduleFromTime = attractionBookingDTO.ScheduleFromTime;
                selectedattractionBookingDTO.ScheduleToTime = attractionBookingDTO.ScheduleToTime;
                selectedattractionBookingDTO.AvailableUnits = attractionBookingDTO.AvailableUnits;

                if (attractionBookingDTO.AttractionBookingSeatsDTOList.Any())
                {
                    selectedattractionBookingDTO.AttractionBookingSeatsDTOList.AddRange(attractionBookingDTO.AttractionBookingSeatsDTOList);
                }
                selectedAttractionBookings.Add(selectedattractionBookingDTO);
            }
            log.LogMethodExit();
        }

        private int GetTotalBookedQuantity(int productId)
        {
            log.LogMethodEntry(productId);
            int quantity = 0;

            List<AttractionBookingDTO> bookingDTOs = selectedAttractionBookings.Where(x => x.AttractionProductId == productId).ToList();
            if (bookingDTOs.Any())
            {
                quantity = bookingDTOs.Sum(x => x.BookedUnits);
            }

            log.LogMethodExit("booked quantity for product:" + productId + ":" + quantity);
            return quantity;
        }

        private int GetTotalBookedQuantityForCurrentBooking(ScheduleDetailsDTO scheduleDetailsDTO, int bookingId = -1, bool checkForProduct = false)
        {
            log.LogMethodEntry(scheduleDetailsDTO, bookingId, checkForProduct);
            int quantity = 0;

            // Get total booked units from temp list. DB list is gor seperately
            List<AttractionBookingDTO> existingBooking = selectedAttractionBookings.Where(x => (x.AttractionScheduleId == scheduleDetailsDTO.ScheduleId)
                                                                                            && (x.FacilityMapId == scheduleDetailsDTO.FacilityMapId)
                                                                                            && (x.ScheduleFromDate.Date == scheduleDetailsDTO.ScheduleFromDate.Date)
                                                                                            && (x.ScheduleFromTime == scheduleDetailsDTO.ScheduleFromTime)
                                                                                                ).ToList();
            if (existingBooking.Any())
            {
                if (checkForProduct)
                {
                    existingBooking = existingBooking.Where(x => (x.AttractionProductId == scheduleDetailsDTO.ProductId)).ToList();
                }

                if (bookingId != -1)
                {
                    existingBooking = existingBooking.Where(x => (x.BookingId == bookingId)).ToList();
                }
                quantity = existingBooking.Sum(x => x.BookedUnits);
            }

            log.LogMethodExit();
            return quantity;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams CP = base.CreateParams;
                CP.ExStyle = CP.ExStyle | 0x02000000;
                return CP;
            }
        }

        private bool CreateNewBookingSlot(ScheduleDetailsDTO scheduleDetailsDTO, int qty)
        {
            log.LogMethodEntry(scheduleDetailsDTO, qty);

            if (qty > 999 || qty < -999)
                return false;

            List<AttractionBookingDTO> existingBooking = selectedAttractionBookings.Where(x => (x.AttractionScheduleId == scheduleDetailsDTO.ScheduleId)
                                                                                            && (x.AttractionProductId == scheduleDetailsDTO.ProductId)
                                                                                            && (x.FacilityMapId == scheduleDetailsDTO.FacilityMapId)
                                                                                            && (x.ScheduleFromDate.Date == scheduleDetailsDTO.ScheduleFromDate.Date)
                                                                                            && (x.ScheduleFromTime == scheduleDetailsDTO.ScheduleFromTime)
                                                                                                ).ToList();

            if (existingBooking.Any())
            {
                // first check if the sum total is more or less
                int currentBookedQuantity = existingBooking.Sum(x => x.BookedUnits);
                if (currentBookedQuantity == qty)
                {
                    // amount is same no, changes needed
                    return true;
                }
                else if (currentBookedQuantity < qty)
                {
                    // user has added quantity
                    AttractionBookingDTO currentAttractionBooking = existingBooking[0];
                    {
                        currentAttractionBooking.BookedUnits += (qty - currentBookedQuantity);
                    }
                }
                else if (currentBookedQuantity > qty)
                {
                    // user had chosen to resuce the quantity, so need to reduce from existing dto
                    int delta = currentBookedQuantity - qty;

                    // get new dto and reduce from them
                    foreach (AttractionBookingDTO currentAttractionBooking in existingBooking)
                    {
                        if (currentAttractionBooking.BookedUnits >= delta)
                        {
                            currentAttractionBooking.BookedUnits -= delta;
                            delta = 0;
                        }
                        else
                        {
                            delta -= currentAttractionBooking.BookedUnits;
                            currentAttractionBooking.BookedUnits = 0;
                        }

                        if (delta == 0)
                            break;
                    }

                    if (delta > 0)
                    {
                        POSUtils.ParafaitMessageBox("Not able to assign seats. Please cancel and try again.");
                    }
                }
            }
            else
            {
                AttractionBookingDTO selectedattractionBookingDTO = new AttractionBookingDTO();
                selectedattractionBookingDTO.AttractionProductId = scheduleDetailsDTO.ProductId;
                selectedattractionBookingDTO.FacilityMapId = scheduleDetailsDTO.FacilityMapId;
                selectedattractionBookingDTO.AttractionPlayId = scheduleDetailsDTO.AttractionPlayId;
                selectedattractionBookingDTO.AttractionPlayName = scheduleDetailsDTO.AttractionPlayName;
                selectedattractionBookingDTO.AttractionScheduleId = scheduleDetailsDTO.ScheduleId;
                selectedattractionBookingDTO.AttractionScheduleName = scheduleDetailsDTO.ScheduleName;
                selectedattractionBookingDTO.ScheduleFromDate = scheduleDetailsDTO.ScheduleFromDate;
                selectedattractionBookingDTO.ScheduleToDate = scheduleDetailsDTO.ScheduleToDate;
                selectedattractionBookingDTO.BookedUnits = qty;
                selectedattractionBookingDTO.Price = scheduleDetailsDTO.Price == null ? 0 : Convert.ToDouble(scheduleDetailsDTO.Price.ToString());
                selectedattractionBookingDTO.PromotionId = scheduleDetailsDTO.PromotionId;
                selectedattractionBookingDTO.AvailableUnits = scheduleDetailsDTO.TotalUnits;
                selectedattractionBookingDTO.ScheduleFromTime = scheduleDetailsDTO.ScheduleFromTime;
                selectedattractionBookingDTO.ScheduleToTime = scheduleDetailsDTO.ScheduleToTime;

                if (scheduleDetailsDTO.ExpiryDate != null)
                    selectedattractionBookingDTO.ExpiryDate = Convert.ToDateTime(scheduleDetailsDTO.ExpiryDate.ToString());

                selectedAttractionBookings.Add(selectedattractionBookingDTO);
            }

            log.LogMethodExit();
            return true;
        }

        private List<ScheduleDetailsViewDTO> GetEligibleSchedules(DateTime scheduleDate, decimal fromTime, decimal toTime, int facilityMapId, int productId = -1, int masterScheduleId = -1)
        {
            log.LogMethodEntry(scheduleDate, fromTime, toTime, facilityMapId, productId, masterScheduleId);

            if(attractionBookingScheduleBL == null)
                attractionBookingScheduleBL = new AttractionBookingSchedulesBL(Utilities.ExecutionContext);

            //List<ScheduleDetailsDTO> scheduleDetailsDTOList = attractionBookingScheduleBL.GetAttractionBookingSchedules(scheduleDate.Date, productId != -1?productId.ToString():"", 
            //    facilityMapId, null, fromTime, toTime, true);

            //if (scheduleDetailsDTOList != null && productId > -1)
            //{
            //    foreach (ScheduleDetailsDTO scheduleDetailsDTO in scheduleDetailsDTOList)
            //        scheduleDetailsDTO.ProductId = productId;
            //}

            //if (chkShowPast != null && chkShowPast.Checked == false)
            //{
            //    scheduleDetailsDTOList = scheduleDetailsDTOList.Where(x => x.ScheduleFromDate >= (this.isBooking ? scheduleDate : Utilities.getServerTime().AddMinutes(POSStatic.ATTRACTION_BOOKING_GRACE_PERIOD))).ToList();
            //}

            //log.LogMethodExit(scheduleDetailsDTOList);
            //return scheduleDetailsDTOList;

            List<ScheduleDetailsViewDTO> scheduleDetailsViewDTOList = attractionBookingScheduleBL.BuildScheduleDetailsForView(scheduleDate, productId != -1 ? productId.ToString() : "",
                facilityMapId, null, fromTime, toTime, this.selectedToDateTime, chkShowPast != null ? chkShowPast.Checked : false, true, true, -1, this.isBooking, this.isRescheduleSlot, 0, 200);

            dayAttractionSchedulesList = new List<DayAttractionScheduleDTO>();
            foreach (ScheduleDetailsViewDTO scheduleDetailsViewDTO in scheduleDetailsViewDTOList)
            {
                if (scheduleDetailsViewDTO.DayAttractionScheduleDTO != null)
                    dayAttractionSchedulesList.Add(scheduleDetailsViewDTO.DayAttractionScheduleDTO);
            }
            log.LogMethodExit(scheduleDetailsViewDTOList);
            return scheduleDetailsViewDTOList;
        }

        private List<AttractionBookingDTO> GetBookedQuantityForDay(DateTime scheduleDate, decimal fromTime, decimal toTime, List<FacilityMapDTO> facilityMapDTO, int productId, int bookingId, int masterScheduleId = -1)
        {
            log.LogMethodEntry(scheduleDate, fromTime, toTime, facilityMapDTO, productId, bookingId, masterScheduleId);
            List<AttractionBookingDTO> bookedUnitMap = new List<AttractionBookingDTO>();
            List<int> facilityMapId = new List<int>();
            int hours = Decimal.ToInt32(fromTime);
            int minutes = (int)((fromTime - hours) * 100);
            DateTime fromDateTime = scheduleDate.Date.AddMinutes(hours * 60 + minutes);
            hours = Decimal.ToInt32(toTime);
            minutes = (int)((toTime - hours) * 100);
            DateTime toDateTime = scheduleDate.Date.AddMinutes(hours * 60 + minutes);

            foreach (FacilityMapDTO facilityMap in facilityMapDTO)
            {
                if (facilityMap.FacilityMapId == -1)
                    continue;

                facilityMapId.Add(facilityMap.FacilityMapId);
            }

            AttractionBookingList attractionBooking = new AttractionBookingList(executionContext);
            bookedUnitMap = attractionBooking.GetTotalBookedUnitsForAttractionBySchedule(facilityMapId, fromDateTime, toDateTime, productId, bookingId);

            log.LogMethodExit(bookedUnitMap);
            return bookedUnitMap;
        }

        private List<DayAttractionScheduleDTO> GetDayAttractionSchedules(List<FacilityMapDTO> facilityMapDTOList, DateTime startDateTime, DateTime endDateTime )
        {
            // if multiple bookings are allowed, then do not prevent from being booked
            log.LogMethodEntry(facilityMapDTOList, startDateTime, endDateTime);
            List<DayAttractionScheduleDTO> dayAttractionScheduleDTOs = new List<DayAttractionScheduleDTO>();
            DayAttractionScheduleListBL dayAttractionScheduleListBL = new DayAttractionScheduleListBL(executionContext);
            List<KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>(DayAttractionScheduleDTO.SearchByParameters.SCHEDULE_FROM_DATE_TIME, startDateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
            searchParameters.Add(new KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>(DayAttractionScheduleDTO.SearchByParameters.SCHEDULE_TO_DATE_TIME, endDateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
            searchParameters.Add(new KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>(DayAttractionScheduleDTO.SearchByParameters.IS_ACTIVE, "Y"));
            List<DayAttractionScheduleDTO> dayAttractionScheduleDTOList = dayAttractionScheduleListBL.GetAllDayAttractionScheduleList(searchParameters, null);
            if (dayAttractionScheduleDTOList != null && dayAttractionScheduleDTOList.Any())
            {
                dayAttractionScheduleDTOs.AddRange(dayAttractionScheduleDTOList);
            }
            log.LogMethodExit(dayAttractionScheduleDTOs);
            return dayAttractionScheduleDTOs;
        }
        private List<FacilityMapDTO> GetAttractionFacilityMaps(int productId)
        {
            log.LogMethodEntry(productId);
            FacilityMapListBL facilityMapListBL = new FacilityMapListBL(Utilities.ExecutionContext);
            List<KeyValuePair<FacilityMapDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<FacilityMapDTO.SearchByParameters, string>>();
            searchParams.Add(new KeyValuePair<FacilityMapDTO.SearchByParameters, string>(FacilityMapDTO.SearchByParameters.SITE_ID, Utilities.ExecutionContext.GetSiteId().ToString()));
            searchParams.Add(new KeyValuePair<FacilityMapDTO.SearchByParameters, string>(FacilityMapDTO.SearchByParameters.IS_ACTIVE, "1"));
            if (productId > -1)
            {
                searchParams.Add(new KeyValuePair<FacilityMapDTO.SearchByParameters, string>(FacilityMapDTO.SearchByParameters.HAVING_PRODUCT_TYPES_IN, ProductTypeValues.ATTRACTION));
                searchParams.Add(new KeyValuePair<FacilityMapDTO.SearchByParameters, string>(FacilityMapDTO.SearchByParameters.ALLOWED_PRODUCT_ID, productId.ToString()));
            }
            else if(productId == -1 && this.facilityMapId > -1)
            {
                searchParams.Add(new KeyValuePair<FacilityMapDTO.SearchByParameters, string>(FacilityMapDTO.SearchByParameters.FACILITY_MAP_ID, this.facilityMapId.ToString()));
            }

            List<FacilityMapDTO> facilityMapDTOList = facilityMapListBL.GetFacilityMapDTOList(searchParams);
            if (facilityMapDTOList == null || facilityMapDTOList.Count == 0)
            {
                facilityMapDTOList = new List<FacilityMapDTO>();
            }
            FacilityMapDTO facilityMapDTO = new FacilityMapDTO()
            {
                FacilityMapName = "-All -",
                FacilityMapId = -1
            };

            facilityMapDTOList = facilityMapDTOList.OrderBy(fac => fac.FacilityMapName).ToList();

            // do not add all if a facility is sent
            if (this.facilityMapId == -1)
                facilityMapDTOList.Insert(0, facilityMapDTO);

            log.LogMethodExit();
            return facilityMapDTOList;
        }

        public List<AttractionBooking> ShowSchedules(ref String message)
        {
            log.LogMethodEntry();
            try
            {
                this.StartPosition = FormStartPosition.CenterParent;
                this.Text = "Schedule Attraction";
                DialogResult dr = this.ShowDialog();
                if (dr == DialogResult.Cancel)
                {
                    log.LogMethodExit();
                    return new List<AttractionBooking>();
                }
                else
                {
                    List<AttractionBooking> attractionBookings = new List<AttractionBooking>();
                    selectedAttractionBookings = selectedAttractionBookings.Where(x => (x.BookingId == -1 && x.BookedUnits > 0) ||
                                                                                      (x.BookingId > -1)).ToList();
                    // convert from the tree node to the selected attraction bookings
                    foreach (AttractionBookingDTO selectedTreeNodeScheduleDTO in selectedAttractionBookings)
                    {
                        if (isBooking || isEvent)
                            selectedTreeNodeScheduleDTO.Source = AttractionBookingDTO.SourceEnum.RESERVATION;
                        attractionBookings.Add(new AttractionBooking(executionContext, selectedTreeNodeScheduleDTO));
                    }
                    log.LogMethodExit();
                    return attractionBookings;
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return new List<AttractionBooking>();
            }
        }

        /// <summary>
        /// Method to check if valid schedules exists fro given date and time. 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="reservationCode"></param>
        /// <returns>List<AttractionBooking></returns>
        public List<AttractionBooking> ShowSchedulesForNonPOSScreens(Dictionary<int, int> quantityMap, List<AttractionBooking> existingAttractionSchedules, DateTime selectedFromDate,
            DateTime? selectedToDateTime,
            int facilityId, bool isBooking, CustomerDTO customerDTO, int bookingId, ref string message)
        {
            log.LogMethodEntry(quantityMap, existingAttractionSchedules, selectedDate, selectedToDateTime, facilityId, isBooking, customerDTO, bookingId, message);

            List<ProductsDTO> productsList = new List<ProductsDTO>();
            List<AttractionBooking> atbList = new List<AttractionBooking>();

            try
            {
                MasterScheduleList masterScheduleList = new MasterScheduleList(Utilities.ExecutionContext);
                this.masterScheduleBLList = masterScheduleList.GetAllMasterScheduleBLList();

                for (int i = 0; i < quantityMap.Keys.Count; i++)
                {
                    int product = quantityMap.Keys.ElementAt(i);
                    Products products = new Products(product);
                    productsList.Add(products.GetProductsDTO);
                }

                this.productsList = productsList;
                this.quantityMap = quantityMap;
                this.selectedAttractionBookings = new List<AttractionBookingDTO>();
                this.existingAttractionBooking = existingAttractionSchedules;
                
                this.isBooking = isBooking;
                if (this.isBooking)
                {
                    isAttractionsWithinReservationPeriodOnly = ParafaitDefaultContainerList.GetParafaitDefault<bool>(this.executionContext, "ATTRACTIONS_WITHIN_RESERVATION_PERIOD_ONLY", false);
                }
                if (this.isBooking)
                {
                    if (isAttractionsWithinReservationPeriodOnly)
                    {
                        this.selectedDate = selectedFromDate;
                    }
                    else
                    {
                        this.selectedDate = selectedFromDate.Date.AddHours(ParafaitDefaultContainerList.GetParafaitDefault<int>(this.executionContext, "BUSINESS_DAY_START_TIME", 6));
                        selectedToDateTime = null;
                    }
                }
                else
                {
                    this.selectedDate = selectedFromDate.Date;
                }
                this.selectedToDateTime = (selectedToDateTime == null ? DateTime.MinValue : (DateTime)selectedToDateTime);
                this.facilityMapId = facilityId;
                //this.PrimaryCard = card;
                this.trxCustomerDTO = customerDTO;
                this.bookingId = bookingId;
                facilityMapNames = new Dictionary<int, string>();
                selectedProduct = productsList[0];

                if (!SchedulesExist())
                {
                    return null;
                }

                InitlializeScreen();

                if (isBooking)
                {
                    dtpAttractionDate.Enabled = false;
                    btnNext.Visible = false;
                    btnPrev.Visible = false;
                }
                atbList = this.ShowSchedules(ref message);

            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            log.LogMethodExit();
            return atbList;
        }

        /// <summary>
        /// Method to check if valid schedules exists fro given date and time. 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="reservationCode"></param>
        /// <returns>List<AttractionBooking></returns>
        public List<AttractionBooking> ShowSchedulesForRescheduleSlot(bool isRescheduleSlot,
            List<AttractionBooking> existingAttractionSchedules, DateTime selectedDate, int facilityMapId, ref string message)
        {
            try
            {
                MasterScheduleList masterScheduleList = new MasterScheduleList(Utilities.ExecutionContext);
                if(this.masterScheduleBLList == null || !this.masterScheduleBLList.Any())
                    this.masterScheduleBLList = masterScheduleList.GetAllMasterScheduleBLList();
                this.isRescheduleSlot = isRescheduleSlot;
                this.selectedAttractionBookings = new List<AttractionBookingDTO>();
                this.existingAttractionBooking = existingAttractionSchedules;

                this.selectedDate = selectedDate;
                selectedFacilityMap = this.facilityMapId = facilityMapId;
                this.bookingId = -1;
                selectedProduct = new ProductsDTO();
                productsList = new List<ProductsDTO>();
                facilityMapNames = new Dictionary<int, string>();

                quantityMap = new Dictionary<int, int>();
                if (existingAttractionSchedules != null)
                {
                    quantityMap.Add(-1, existingAttractionSchedules.Sum(x => x.AttractionBookingDTO.BookedUnits));
                }

                InitlializeScreen();

                chkShowPast.Checked = false;
                //if (!SchedulesExist())
                //{
                //    return null;
                //}
                if (isBooking)
                {
                    cmbAttractionFacility.Enabled = false;
                }

                this.StartPosition = FormStartPosition.CenterParent;
                this.Text = "Reschedule Attraction Bookings and Schedule ";
                lblHeaderAttractionProduct.Height += 10;
                DialogResult dr = this.ShowDialog();
                if (dr == DialogResult.Cancel)
                {
                    log.LogMethodExit();
                    return new List<AttractionBooking>();
                }
                else
                {
                    List<AttractionBooking> attractionBookings = new List<AttractionBooking>();
                    selectedAttractionBookings = selectedAttractionBookings.Where(x => (x.BookingId == -1 && x.BookedUnits > 0) ||
                                                                                      (x.BookingId > -1)).ToList();
                    // convert from the tree node to the selected attraction bookings
                    foreach (AttractionBookingDTO selectedTreeNodeScheduleDTO in selectedAttractionBookings)
                    {
                        if (isBooking || isEvent)
                            selectedTreeNodeScheduleDTO.Source = AttractionBookingDTO.SourceEnum.RESERVATION;
                        attractionBookings.Add(new AttractionBooking(executionContext, selectedTreeNodeScheduleDTO));
                    }
                    log.LogMethodExit();
                    return attractionBookings;
                }

            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            log.LogMethodExit();
            return new List<AttractionBooking>();
        }

        /// <summary>
        /// Method to check if valid schedules exists fro given date and time. 
        /// </summary>
        /// <returns>true if schedules exists</returns>
        public bool SchedulesExist()
        {
            log.LogMethodEntry();

            List<ScheduleDetailsViewDTO> schedules = GetEligibleSchedules(selectedDate,
                Convert.ToInt32(String.IsNullOrEmpty(ParafaitDefaultContainerList.GetParafaitDefault(this.Utilities.ExecutionContext, "BUSINESS_DAY_START_TIME")) ? "0" : ParafaitDefaultContainerList.GetParafaitDefault(this.Utilities.ExecutionContext, "BUSINESS_DAY_START_TIME"))
                , 24, this.facilityMapId, selectedProduct.ProductId);
            if (schedules != null && schedules.Any())
                return true;
            else
                return false;
        }

        private void LoadReschedulePanel()
        {
            log.LogMethodEntry();
            flpReschedule.Controls.Clear();
            flpReschedule.SuspendLayout();
            flpReschedule.FlowDirection = FlowDirection.TopDown;
            flpReschedule.BackColor = Color.Transparent;
            flpReschedule.BorderStyle = BorderStyle.FixedSingle;
            flpReschedule.Location = new System.Drawing.Point((int)(flpReschedule.Parent.Size.Width / 2) - 250, (int)(flpReschedule.Parent.Size.Height / 2) - 200);

            flpReschduleInProgress.Location = new System.Drawing.Point((int)(flpReschedule.Parent.Size.Width / 2) - 250, (int)(flpReschedule.Parent.Size.Height / 2) - 200);

            Label lblHeader = new Label();
            lblHeader.Width = flpReschedule.Width;
            lblHeader.Text = MessageContainerList.GetMessage(this.Utilities.ExecutionContext, "What do you want to do with this schedule?");
            lblHeader.BackColor = System.Drawing.Color.Transparent;
            lblHeader.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            lblHeader.ForeColor = System.Drawing.Color.Black;
            lblHeader.Font = new System.Drawing.Font("Arial", 12F, FontStyle.Bold);
            lblHeader.Margin = new Padding(10, 20, 20, 10);
            lblHeader.Padding = new Padding(0);
            lblHeader.TextAlign = ContentAlignment.MiddleCenter;
            flpReschedule.Controls.Add(lblHeader);

            Button btnMoveBookingsButton = new Button();
            btnMoveBookingsButton.BackColor = System.Drawing.Color.Transparent;
            btnMoveBookingsButton.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            btnMoveBookingsButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            btnMoveBookingsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnMoveBookingsButton.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            btnMoveBookingsButton.ForeColor = System.Drawing.Color.White;
            btnMoveBookingsButton.Margin = new System.Windows.Forms.Padding(100, 3, 100, 3);
            btnMoveBookingsButton.Name = "btnMoveBookingsButton";
            btnMoveBookingsButton.Size = new System.Drawing.Size(200, 45);
            btnMoveBookingsButton.Location = new System.Drawing.Point((int)(flpReschedule.Size.Width/2)-100, 50);
            btnMoveBookingsButton.TabIndex = 5;
            btnMoveBookingsButton.Text = MessageContainerList.GetMessage(this.Utilities.ExecutionContext, "Move Booking");
            btnMoveBookingsButton.UseVisualStyleBackColor = false;
            btnMoveBookingsButton.Click += new EventHandler(btnMoveBookingsButton_Click);
            btnMoveBookingsButton.TextAlign = ContentAlignment.MiddleCenter;
            btnMoveBookingsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnMoveBookingsButton.FlatAppearance.BorderColor = System.Drawing.Color.White;
            btnMoveBookingsButton.FlatAppearance.BorderSize = 0;
            btnMoveBookingsButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;

            Button btnMoveScheduleButton = new Button();
            btnMoveScheduleButton.BackColor = System.Drawing.Color.Transparent;
            btnMoveScheduleButton.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            btnMoveScheduleButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            btnMoveScheduleButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnMoveScheduleButton.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            btnMoveScheduleButton.ForeColor = System.Drawing.Color.White;
            btnMoveScheduleButton.Margin = new System.Windows.Forms.Padding(100, 3, 100, 3);
            btnMoveScheduleButton.Name = "btnMoveScheduleButton";
            btnMoveScheduleButton.Size = new System.Drawing.Size(200, 45);
            btnMoveBookingsButton.Location = new System.Drawing.Point((int)(flpReschedule.Size.Width / 2) - 100, 110);
            btnMoveBookingsButton.TabIndex = 5;
            btnMoveScheduleButton.Text = MessageContainerList.GetMessage(this.Utilities.ExecutionContext, "Move Schedule");
            btnMoveScheduleButton.UseVisualStyleBackColor = false;
            btnMoveScheduleButton.Click += new EventHandler(btnMoveScheduleButton_Click);
            btnMoveScheduleButton.TextAlign = ContentAlignment.MiddleCenter;
            btnMoveScheduleButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnMoveScheduleButton.FlatAppearance.BorderColor = System.Drawing.Color.White;
            btnMoveScheduleButton.FlatAppearance.BorderSize = 0;
            btnMoveScheduleButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;

            Button btnBlockScheduleButton = new Button();
            btnBlockScheduleButton.BackColor = System.Drawing.Color.Transparent;
            btnBlockScheduleButton.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            btnBlockScheduleButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            btnBlockScheduleButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnBlockScheduleButton.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            btnBlockScheduleButton.ForeColor = System.Drawing.Color.White;
            btnBlockScheduleButton.Margin = new System.Windows.Forms.Padding(100, 3, 100, 3);
            btnBlockScheduleButton.Name = "btnBlockScheduleButton";
            btnBlockScheduleButton.Size = new System.Drawing.Size(200, 45);
            btnBlockScheduleButton.Location = new System.Drawing.Point((int)(flpReschedule.Size.Width / 2) - 100, 170);
            btnBlockScheduleButton.TabIndex = 5;
            btnBlockScheduleButton.Text = MessageContainerList.GetMessage(this.Utilities.ExecutionContext, "Block Schedule");
            btnBlockScheduleButton.UseVisualStyleBackColor = false;
            btnBlockScheduleButton.Click += new EventHandler(btnBlockScheduleButton_Click);
            btnBlockScheduleButton.TextAlign = ContentAlignment.MiddleCenter;
            btnBlockScheduleButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnBlockScheduleButton.FlatAppearance.BorderColor = System.Drawing.Color.White;
            btnBlockScheduleButton.FlatAppearance.BorderSize = 0;
            btnBlockScheduleButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;

            Button btnReserveScheduleButton = new Button();
            btnReserveScheduleButton.BackColor = System.Drawing.Color.Transparent;
            btnReserveScheduleButton.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            btnReserveScheduleButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            btnReserveScheduleButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnReserveScheduleButton.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            btnReserveScheduleButton.ForeColor = System.Drawing.Color.White;
            btnReserveScheduleButton.Margin = new System.Windows.Forms.Padding(100, 3, 100, 3);
            btnReserveScheduleButton.Name = "btnReserveScheduleButton";
            btnReserveScheduleButton.Size = new System.Drawing.Size(200, 45);
            btnReserveScheduleButton.Location = new System.Drawing.Point((int)(flpReschedule.Size.Width / 2) - 100, 230);
            btnReserveScheduleButton.TabIndex = 5;
            btnReserveScheduleButton.Text = MessageContainerList.GetMessage(this.Utilities.ExecutionContext, "Reserve Schedule");
            btnReserveScheduleButton.UseVisualStyleBackColor = false;
            btnReserveScheduleButton.Click += new EventHandler(btnReserveScheduleButton_Click);
            btnReserveScheduleButton.TextAlign = ContentAlignment.MiddleCenter;
            btnReserveScheduleButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnReserveScheduleButton.FlatAppearance.BorderColor = System.Drawing.Color.White;
            btnReserveScheduleButton.FlatAppearance.BorderSize = 0;
            btnReserveScheduleButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;

            Button btnCancelRescheduleButton = new Button();
            btnCancelRescheduleButton.BackColor = System.Drawing.Color.Transparent;
            btnCancelRescheduleButton.BackgroundImage = global::Parafait_POS.Properties.Resources.normal1;
            btnCancelRescheduleButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            btnCancelRescheduleButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnCancelRescheduleButton.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            btnCancelRescheduleButton.ForeColor = System.Drawing.Color.White;
            btnCancelRescheduleButton.Margin = new System.Windows.Forms.Padding(100, 3, 100, 3);
            btnCancelRescheduleButton.Name = "btnCancelRescheduleButton";
            btnCancelRescheduleButton.Size = new System.Drawing.Size(200, 45);
            btnCancelRescheduleButton.Location = new System.Drawing.Point((int)(flpReschedule.Size.Width / 2) - 100, 290);
            btnCancelRescheduleButton.TabIndex = 5;
            btnCancelRescheduleButton.Text = MessageContainerList.GetMessage(this.Utilities.ExecutionContext, "Cancel");
            btnCancelRescheduleButton.UseVisualStyleBackColor = false;
            btnCancelRescheduleButton.Click += new EventHandler(btnCancelRescheduleButton_Click);
            btnCancelRescheduleButton.TextAlign = ContentAlignment.MiddleCenter;
            btnCancelRescheduleButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnCancelRescheduleButton.FlatAppearance.BorderColor = System.Drawing.Color.White;
            btnCancelRescheduleButton.FlatAppearance.BorderSize = 0;
            btnCancelRescheduleButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;


            flpReschedule.Controls.Add(btnMoveBookingsButton);
            flpReschedule.Controls.Add(btnMoveScheduleButton);
            flpReschedule.Controls.Add(btnBlockScheduleButton);
            flpReschedule.Controls.Add(btnReserveScheduleButton);
            flpReschedule.Controls.Add(btnCancelRescheduleButton);

            flpReschedule.ResumeLayout(true);
            log.LogMethodExit();
        }

        private void btnMoveBookingsButton_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            
            flpReschedule.Visible = false;

            if (sourceScheduleDetailsDTO.BookedUnits == null || sourceScheduleDetailsDTO.BookedUnits <= 0)
            {
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext,2782));
                sourceDASDTO = null;
                sourceScheduleDetailsDTO = null;
                return;
            }

            rescheduleAction = AttractionBookingDTO.RescheduleActionEnum.MOVE_BOOKINGS;

            List<ScheduleDetailsDTO> scheduleList = new List<ScheduleDetailsDTO>();
            LoadSummaryForReschedule(scheduleList);
            lblErrorMessage.Text = MessageContainerList.GetMessage(Utilities.ExecutionContext,2783);
            log.LogMethodExit();
        }

        private void btnMoveScheduleButton_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            rescheduleAction = AttractionBookingDTO.RescheduleActionEnum.MOVE_SCHEDULES;
            flpReschedule.Visible = false;
            List<ScheduleDetailsDTO> scheduleList = new List<ScheduleDetailsDTO>();
            LoadSummaryForReschedule(scheduleList);

            lblErrorMessage.Text = MessageContainerList.GetMessage(Utilities.ExecutionContext, 2784);
            log.LogMethodExit();
        }

        private void btnBlockScheduleButton_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            flpReschedule.Visible = false;

            if(sourceScheduleDetailsDTO.DayAttractionScheduleId != -1)
            {
                if (this.dayAttractionSchedulesList.FirstOrDefault(x => x.DayAttractionScheduleId == sourceScheduleDetailsDTO.DayAttractionScheduleId).ScheduleStatus
                    != DayAttractionScheduleDTO.ScheduleStatusEnumToString(DayAttractionScheduleDTO.ScheduleStatusEnum.BLOCKED))
                {
                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext, 2809));
                    sourceDASDTO = null;
                    sourceScheduleDetailsDTO = null;
                    return;
                }
            }

            rescheduleAction = AttractionBookingDTO.RescheduleActionEnum.BLOCK_SCHEDULE;

            List<ScheduleDetailsDTO> scheduleList = new List<ScheduleDetailsDTO>();
            //scheduleList.Add(sourceScheduleDetailsDTO);
            LoadSummaryForReschedule(scheduleList);

            using (GenericDataEntry trxRemarks = new GenericDataEntry(1))
            {
                trxRemarks.Text = POSStatic.MessageUtils.getMessage(201);
                trxRemarks.DataEntryObjects[0].mandatory = false;
                trxRemarks.DataEntryObjects[0].label = " Remarks";
                trxRemarks.DataEntryObjects[0].data = "";
                if (trxRemarks.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    sourceDASDTO.Remarks = this.executionContext.GetUserId() + ":" + trxRemarks.DataEntryObjects[0].data;
                }
            }

            lblErrorMessage.Text = MessageContainerList.GetMessage(Utilities.ExecutionContext, 2777);
            log.LogMethodExit();
        }

        private void btnReserveScheduleButton_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            flpReschedule.Visible = false;

            if (sourceScheduleDetailsDTO.DayAttractionScheduleId != -1)
            {
                if (this.dayAttractionSchedulesList.FirstOrDefault(x => x.DayAttractionScheduleId == sourceScheduleDetailsDTO.DayAttractionScheduleId).ScheduleStatus
                    != DayAttractionScheduleDTO.ScheduleStatusEnumToString(DayAttractionScheduleDTO.ScheduleStatusEnum.OPEN))
                {
                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext, 2810));
                    sourceDASDTO = null;
                    sourceScheduleDetailsDTO = null;
                    return;
                }
            }

            rescheduleAction = AttractionBookingDTO.RescheduleActionEnum.RESERVE_SCHEDULE;
            List<ScheduleDetailsDTO> scheduleList = new List<ScheduleDetailsDTO>();
            //scheduleList.Add(sourceScheduleDetailsDTO);
            LoadSummaryForReschedule(scheduleList);

            using (GenericDataEntry trxRemarks = new GenericDataEntry(1))
            {
                trxRemarks.Text = POSStatic.MessageUtils.getMessage(201);
                trxRemarks.DataEntryObjects[0].mandatory = false;
                trxRemarks.DataEntryObjects[0].label = " Remarks";
                trxRemarks.DataEntryObjects[0].data = "";
                if (trxRemarks.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    sourceDASDTO.Remarks = this.executionContext.GetUserId() + ":" + trxRemarks.DataEntryObjects[0].data;
                }
            }

            lblErrorMessage.Text = MessageContainerList.GetMessage(Utilities.ExecutionContext, 2777);
            log.LogMethodExit();
        }

        private void btnCancelRescheduleButton_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetRescheduleAction(false);
            log.LogMethodExit();
        }

        private void btnClearAction_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetRescheduleAction(true);
            log.LogMethodExit();
        }

        private void ResetRescheduleAction(bool refreshSchedules)
        {
            log.LogMethodEntry();
            this.SuspendLayout();
            rescheduleAction = AttractionBookingDTO.RescheduleActionEnum.NONE;
            flpReschedule.Visible = false;
            sourceDASDTO = null;
            sourceScheduleDetailsDTO = null;
            targetDASDTO = null;
            targetScheduleDetailsDTO = null;
            lblErrorMessage.Text = "";
            flpSummary.Controls.Clear();

            Control moveBookings = flpReschedule.Controls["btnMoveBookingsButton"];
            moveBookings.Enabled = true;
            Control moveSchedules = flpReschedule.Controls["btnMoveScheduleButton"];
            moveSchedules.Enabled = true;
            Control reserveBookings = flpReschedule.Controls["btnReserveScheduleButton"];
            reserveBookings.Enabled = true;

            if (refreshSchedules)
                LoadScheduleGrid();

            this.ResumeLayout(true);
            log.LogMethodExit();
        }
        private void LoadSummaryForReschedule(List<ScheduleDetailsDTO> impactedScheduleDTOList)
        {
            log.LogMethodEntry(impactedScheduleDTOList);

            flpSummary.Controls.Clear();
            flpSummary.SuspendLayout();
            flpSummary.FlowDirection = FlowDirection.TopDown;
            flpSummary.BackColor = Color.Transparent;
            flpSummary.BorderStyle = BorderStyle.FixedSingle;
            flpSummary.AutoScroll = true;

            FlowLayoutPanel impactedSlotPanel = new FlowLayoutPanel();
            impactedSlotPanel.Width = (int)(flpSummary.Width);
            impactedSlotPanel.Height = 32;
            impactedSlotPanel.FlowDirection = FlowDirection.LeftToRight;
            impactedSlotPanel.Margin = new Padding(0, 0, 0, 0);
            impactedSlotPanel.Left = flpSummary.Left;
            impactedSlotPanel.Top = flpSummary.Top;
            impactedSlotPanel.BorderStyle = BorderStyle.FixedSingle;
            impactedSlotPanel.BackColor = Color.LightSteelBlue;

            Label lblAction = new Label();
            lblAction.Height = 32;
            lblAction.Width = impactedSlotPanel.Width;
            lblAction.Text = MessageContainerList.GetMessage(Utilities.ExecutionContext, AttractionBookingDTO.RescheduleActionEnumToString(rescheduleAction));
            lblAction.BackColor = System.Drawing.Color.Transparent;
            lblAction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            lblAction.ForeColor = System.Drawing.Color.Black;
            lblAction.Font = new System.Drawing.Font("Arial", 9F, FontStyle.Bold);
            lblAction.Margin = new Padding(5, 0, 0, 0);
            lblAction.TextAlign = ContentAlignment.MiddleLeft;
            lblAction.BorderStyle = BorderStyle.None;
            lblAction.Anchor = AnchorStyles.None;
            impactedSlotPanel.Height += lblAction.Height;
            impactedSlotPanel.Controls.Add(lblAction);

            ScheduleDetailsDTO tempSchDTO = null;
            usrCtrlAttractionScheduleDetails line = FindChild<usrCtrlAttractionScheduleDetails>(flpSchedules, ":DTOID:" + sourceScheduleDetailsDTO.ScheduleId.ToString() + ":" + sourceScheduleDetailsDTO.FacilityMapId.ToString());
            if (line != null)
            {
                usrCtrlAttractionScheduleDetails lineControl = line;
                lineControl.SetControlColor(System.Windows.Media.Colors.SandyBrown);
                tempSchDTO = (ScheduleDetailsDTO)lineControl.Tag;
            }

            Label lblSourceSlot = new Label();
            lblSourceSlot.Height = 32;
            lblSourceSlot.Width = impactedSlotPanel.Width;
            lblSourceSlot.Text = " " + (tempSchDTO != null ? (!String.IsNullOrEmpty(tempSchDTO.FacilityMapName) ? tempSchDTO.FacilityMapName : tempSchDTO.ScheduleName) + " - " + tempSchDTO.ScheduleFromDate.ToString("hh:mm tt") :
                sourceScheduleDetailsDTO.ScheduleName + " - " + sourceScheduleDetailsDTO.ScheduleFromDate.ToString("hh:mm tt"));
            lblSourceSlot.BackColor = System.Drawing.Color.SandyBrown;
            lblSourceSlot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            lblSourceSlot.ForeColor = System.Drawing.Color.Black;
            lblSourceSlot.Font = new System.Drawing.Font("Arial", 9F, FontStyle.Bold);
            lblSourceSlot.Margin = new Padding(0, 0, 0, 0);
            lblSourceSlot.TextAlign = ContentAlignment.MiddleLeft;
            lblSourceSlot.BorderStyle = BorderStyle.None;
            lblSourceSlot.Anchor = AnchorStyles.None;
            impactedSlotPanel.Height += lblSourceSlot.Height;
            impactedSlotPanel.Controls.Add(lblSourceSlot);

            if (impactedScheduleDTOList.Any())
            {
                Label lblImpactedSlots = new Label();
                lblImpactedSlots.Height = 32;
                lblImpactedSlots.Width = impactedSlotPanel.Width;
                lblImpactedSlots.Text = "  Impacted Schedules";
                lblImpactedSlots.BackColor = System.Drawing.Color.Transparent;
                lblImpactedSlots.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                lblImpactedSlots.ForeColor = System.Drawing.Color.Black;
                lblImpactedSlots.Font = new System.Drawing.Font("Arial", 9F, FontStyle.Bold);
                lblImpactedSlots.Margin = new Padding(0, 0, 0, 0);
                lblImpactedSlots.TextAlign = ContentAlignment.MiddleLeft;
                lblImpactedSlots.BorderStyle = BorderStyle.None;
                lblImpactedSlots.Anchor = AnchorStyles.None;
                impactedSlotPanel.Height += lblImpactedSlots.Height;
                impactedSlotPanel.Controls.Add(lblImpactedSlots);
            }
            foreach (ScheduleDetailsDTO dasDTO in impactedScheduleDTOList)
            {
                ScheduleDetailsDTO tmpImpactedDTO = null;
                usrCtrlAttractionScheduleDetails impactedline = FindChild<usrCtrlAttractionScheduleDetails>(flpSchedules,":DTOID:" + dasDTO.ScheduleId.ToString() + ":" + dasDTO.FacilityMapId.ToString());
                if (impactedline != null)
                {
                    usrCtrlAttractionScheduleDetails lineControl = impactedline;
                    lineControl.SetControlColor(System.Windows.Media.Colors.Green);
                    tmpImpactedDTO = (ScheduleDetailsDTO)lineControl.Tag;
                }

                Label lblScheduleSlot = new Label();
                lblScheduleSlot.Height = 32;
                lblScheduleSlot.Width = impactedSlotPanel.Width;
                lblScheduleSlot.Text = "  " + (tmpImpactedDTO != null ? (!String.IsNullOrEmpty(tmpImpactedDTO.FacilityMapName) ? tmpImpactedDTO.FacilityMapName : tmpImpactedDTO.ScheduleName) + " - " + tmpImpactedDTO.ScheduleFromDate.ToString("hh:mm tt") :
                    dasDTO.ScheduleName + " - " +dasDTO.ScheduleFromDate.ToString("hh:mm tt"));
                lblScheduleSlot.BackColor = System.Drawing.Color.Green;
                lblScheduleSlot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                lblScheduleSlot.ForeColor = System.Drawing.Color.Black;
                lblScheduleSlot.Font = new System.Drawing.Font("Arial", 9F, FontStyle.Bold);
                lblScheduleSlot.Margin = new Padding(0, 0, 0, 0);
                lblScheduleSlot.TextAlign = ContentAlignment.MiddleLeft;
                lblScheduleSlot.BorderStyle = BorderStyle.None;
                lblScheduleSlot.Anchor = AnchorStyles.None;
                impactedSlotPanel.Height += lblScheduleSlot.Height;
                impactedSlotPanel.Controls.Add(lblScheduleSlot);

                
            }

            Button btnClearAction = new Button();
            btnClearAction.BackColor = System.Drawing.Color.Transparent;
            btnClearAction.BackgroundImage = global::Parafait_POS.Properties.Resources.Delete_Icon_Normal;
            btnClearAction.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            btnClearAction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnClearAction.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            btnClearAction.ForeColor = System.Drawing.Color.White;
            btnClearAction.Margin = new System.Windows.Forms.Padding((int)(impactedSlotPanel.Size.Width / 2) - 15, 3, 0, 3);
            btnClearAction.Name = "btnClearAction";
            btnClearAction.Size = new System.Drawing.Size(30,30);
            btnClearAction.TabIndex = 5;
            btnClearAction.Text = MessageContainerList.GetMessage(this.Utilities.ExecutionContext, "Clear Action");
            btnClearAction.UseVisualStyleBackColor = false;
            btnClearAction.Click += new EventHandler(btnClearAction_Click);
            btnClearAction.TextAlign = ContentAlignment.MiddleCenter;
            btnClearAction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnClearAction.FlatAppearance.BorderColor = System.Drawing.Color.White;
            btnClearAction.FlatAppearance.BorderSize = 0;
            btnClearAction.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            impactedSlotPanel.Height += btnClearAction.Height;
            impactedSlotPanel.Controls.Add(btnClearAction);

            if (flpSummary.Height < impactedSlotPanel.Height)
                flpSummary.Height = impactedSlotPanel.Height + 10;
            flpSummary.Controls.Add(impactedSlotPanel);
            flpSummary.ResumeLayout(true);
            log.LogMethodExit();
        }

        private Color GetBackPanelColor(String status, String origin)
        {
            String colorLabel = "";
            if (!String.IsNullOrEmpty(status))
            {
                switch (status.ToUpper())
                {
                    case "OPEN":
                        colorLabel = "OPEN";
                        break;
                    case "UNAVAILABLE":
                        colorLabel = "UNAVAILABLE";
                        break;
                    case "BLOCKED":
                        colorLabel = "BLOCKED";
                        break;
                    case "SOLDOUT":
                        colorLabel = "SOLDOUT";
                        break;
                    case "RACE_IN_PROGRESS":
                    case "RACING":
                    case "FINISHED":
                    case "ABORTED":
                    case "CLOSED":
                        colorLabel = "RACE_IN_PROGRESS";
                        break;
                    case "RESCHEDULE_IN_PROGRESS":
                    case "RESCHEDULE":
                        colorLabel = "RESCHEDULE_IN_PROGRESS";
                        break;
                    default:
                        colorLabel = "OPEN";
                        break;
                };
            }
            else
            {
                switch (origin)
                {
                    case "PARTY_RESERVATION":
                    case "RESERVATION":
                        colorLabel = "PARTY_RESERVATION";
                        break;
                    case "DIFFERENT_FACILITY":
                        colorLabel = "UNAVAILABLE";
                        break;
                    default:
                        colorLabel = "OPEN";
                        break;
                };
            }

            if (statusCellColorMap.ContainsKey(colorLabel))
                return statusCellColorMap[colorLabel];
            else
                return Color.White;
        }

        public static T FindChild<T>(System.Windows.DependencyObject parent, string childName) where T : System.Windows.DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = System.Windows.Media.VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = System.Windows.Media.VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as System.Windows.FrameworkElement;
                    // If the child's name is set for search
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }
    }
 }
