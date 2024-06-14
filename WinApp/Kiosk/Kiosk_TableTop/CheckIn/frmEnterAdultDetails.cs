/********************************************************************************************
* Project Name - Parafait_Kiosk 
* Description  - Handles Playground Entry menu
* 
**************
**Version Log
**************
*Version      Date             Modified By        Remarks          
*********************************************************************************************
*2.150.3.0   28-Apr-2023      Vignesh Bhat        Created: TableTop Kiosk Changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Languages;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Product;
using System.Linq;
using System.Globalization;
using System.Threading;

namespace Parafait_Kiosk
{
    public partial class frmEnterAdultDetails : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Utilities utilities = KioskStatic.Utilities;
        private CustomerDTO parentCustomerDTO;
        private List<CustomCustomerDTO> customCustomerDTOList = new List<CustomCustomerDTO>();
        private SortableBindingList<CustomCheckInDetailDTO> customCheckInDetailDTOList = new SortableBindingList<CustomCheckInDetailDTO>();
        private Font savTimeOutFont;
        private Font TimeOutFont;
        private const string WARNING = "WARNING";
        private const string ERROR = "ERROR";
        private int adultQtySelectedInQtyScreen;
        private List<CustomCustomerDTO> selectedRelationList;
        private bool isCustomerProfileExist;
        private string stringGuestOrAdult;
        private string stringGuestOrAdultInitCap;
        private ProductsContainerDTO productsContainerDTO;
        private List<Semnox.Parafait.Transaction.Transaction.TransactionLine> trxLines;
        private PurchaseProductDTO purchaseProductDTO;
        private VirtualWindowsKeyboardController virtualKeyboardController;
        private VirtualKeyboardController customKeyboardController;
        public TextBox CurrentActiveTextBox;
        private int dOBFirstColumnIndex;
        public KioskTransaction GetKioskTransaction { get { return kioskTransaction; } }

        internal List<Semnox.Parafait.Transaction.Transaction.TransactionLine> TrxLines { get { return trxLines; } }
        internal List<CustomCustomerDTO> SelectedRelationList { get { return selectedRelationList; } }

        public frmEnterAdultDetails(KioskTransaction kioskTransaction, List<Semnox.Parafait.Transaction.Transaction.TransactionLine> inTrxLines, PurchaseProductDTO purchaseProd, List<CustomCustomerDTO> inSelectedList)
        {
            log.LogMethodEntry("kioskTransaction", inTrxLines, purchaseProd, inSelectedList);
            InitializeComponent();
            this.kioskTransaction = kioskTransaction;
            this.trxLines = inTrxLines;
            this.purchaseProductDTO = purchaseProd;
            this.selectedRelationList = inSelectedList;
            btnProceed.Enabled = false;

            try
            {
                //checkin checkout feature currently does not work without parent customer details
                if (purchaseProductDTO.ParentCustomerId == -1)
                {
                    string errMsg = "ERROR: ParentCustomerId is null";
                    log.Error(errMsg);
                    KioskStatic.logToFile(errMsg);
                    log.LogMethodExit();
                    return;
                }

                ProductQtyMappingDTO productQtyMapping = purchaseProductDTO.ProductQtyMappingDTOs.Where(p => p.ProductsContainerDTO.ProductId == trxLines[0].ProductID).FirstOrDefault();
                if (productQtyMapping == null)
                {
                    string errMsg = "Error: Could not retrieve product details from Purchase Product in Select Child screen";
                    log.Error(errMsg);
                    log.Debug(productQtyMapping);
                    KioskStatic.logToFile(errMsg);
                    return;
                }
                this.productsContainerDTO = productQtyMapping.ProductsContainerDTO;

                KioskStatic.setDefaultFont(this);
                savTimeOutFont = lblTimeRemaining.Font;
                TimeOutFont = lblTimeRemaining.Font = new System.Drawing.Font(lblTimeRemaining.Font.FontFamily, 50, FontStyle.Bold);
                SetKioskTimerTickValue(60);
                //ResetKioskTimer();
                lblTimeRemaining.Text = GetKioskTimerTickValue().ToString("#0");

                InitializeKeyboard();
                isCustomerProfileExist = (productsContainerDTO.CustomerProfilingGroupId != -1) ? true : false;
                adultQtySelectedInQtyScreen = trxLines.Count;

                int msgNumber = isCustomerProfileExist ? 4342 : 4119;
                stringGuestOrAdult = MessageContainerList.GetMessage(utilities.ExecutionContext, msgNumber);
                CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
                TextInfo textInfo = cultureInfo.TextInfo;
                stringGuestOrAdultInitCap = textInfo.ToTitleCase(stringGuestOrAdult);
                //lblAdultAdded.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 4289, stringGuestOrAdultInitCap, customCheckInDetailDTOList.Count.ToString(), adultQtySelectedInQtyScreen.ToString());

                int checkInFacilityId = productsContainerDTO.CheckInFacilityId;
                lblGridFooterMsg.Text = "";

                List<FacilityContainerDTO> facilityContainerDTOList = FacilityContainerList.GetFacilityContainerDTOCollection(utilities.ExecutionContext.GetSiteId()).FacilitysContainerDTOList;
                string checkInFacilityName = facilityContainerDTOList.Where(f => f.FacilityId == checkInFacilityId).FirstOrDefault().FacilityName;
                lblGreeting.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 4290, stringGuestOrAdult);
                lblPackageHeader.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Package") + ":";//Package Name Header
                string productName = KioskHelper.GetProductName(productsContainerDTO.ProductId);
                lblPackage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, productName);//Product Name for which child is being selected
                lblFacilityHeader.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Playground") + ":";//facility Name Header
                lblFacility.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, checkInFacilityName);//facility name
                panelAgeCriteria.Visible = (productsContainerDTO.CustomerProfilingGroupId != -1) ? true : false;
                lblAgeCriteriaHeader.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 4812) + " :";//Age Criteria
                if (productsContainerDTO.AgeLowerLimit > KioskStatic.AGE_LOWER_LIMIT && productsContainerDTO.AgeUpperLimit < KioskStatic.AGE_UPPER_LIMIT)
                {
                    lblAgeCriteria.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 4777, productsContainerDTO.AgeLowerLimit, productsContainerDTO.AgeUpperLimit);//1 to 13 yrs
                }
                else if (productsContainerDTO.AgeLowerLimit > KioskStatic.AGE_LOWER_LIMIT && productsContainerDTO.AgeUpperLimit == KioskStatic.AGE_UPPER_LIMIT)
                {
                    //lblAgeCriteria.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 4779, productsContainerDTO.AgeLowerLimit);//above 1 yrs

                    lblAgeCriteria.Text = (productsContainerDTO.AgeLowerLimit == 1) ?
                        MessageContainerList.GetMessage(utilities.ExecutionContext, 5174, productsContainerDTO.AgeLowerLimit) //Above 1 yr
                        : MessageContainerList.GetMessage(utilities.ExecutionContext, 4779, productsContainerDTO.AgeLowerLimit); //Above 2 yrs
                }
                else if (productsContainerDTO.AgeLowerLimit == KioskStatic.AGE_LOWER_LIMIT && productsContainerDTO.AgeUpperLimit < KioskStatic.AGE_UPPER_LIMIT)
                {
                    lblAgeCriteria.Text = (productsContainerDTO.AgeLowerLimit > 1) ?
                        MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4778, productsContainerDTO.AgeUpperLimit) //below 18 yrs
                        : MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 5172, productsContainerDTO.AgeUpperLimit); //below 1 yr
                }
                lblGuestCountHeader.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 4467, stringGuestOrAdultInitCap); //"Adult Selected :"
                lblGuestCount.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 4776, (selectedRelationList == null) ? 0 : selectedRelationList.Count, adultQtySelectedInQtyScreen); //"Quantity:&1"
                dgvEnterAdultDetails.Columns["LinkRelationdgvBtn"].HeaderText = "";
                dgvEnterAdultDetails.Columns["LinkRelationdgvBtn"].DefaultCellStyle.NullValue = MessageContainerList.GetMessage(utilities.ExecutionContext, 4117); // "Click to link"
                lblAdultAdded.Visible = false;
                lblGridFooterMsg.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4350); //(*) Marks field are mandatory to fill up
                lblScreenDetails.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 4789);

                SetStyle(ControlStyles.UserPaint, true);
                SetStyle(ControlStyles.AllPaintingInWmPaint, true);
                SetStyle(ControlStyles.DoubleBuffer, true);
                KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
                txtMessage.Text = "";
                DisplaybtnCancel(false);
                DisplaybtnPrev(true);
                SetFont();
                SetCustomImages();
                SetCustomizedFontColors();
                utilities.setLanguage(this);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected error while executing frmEnterAdultDetails(): " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void frmEnterAdultDetails_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                CustomCustomerDTO customCustomerDTO = new CustomCustomerDTO(purchaseProductDTO.ParentCustomerId);
                this.parentCustomerDTO = customCustomerDTO.GetCustomerDTO(purchaseProductDTO.ParentCustomerId);

                GetColumnsToDisplay(); 
                dgvEnterAdultDetails.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(CustomCheckInDetailDTOBindingSource_DataError);// Attach an event handler for the DataError event.
                customCheckInDetailDTOBindingSource.DataSource = new SortableBindingList<CustomCheckInDetailDTO>(customCheckInDetailDTOList); // Bind the BindingSource to the DataGridView
                dgvEnterAdultDetails.DataSource = customCheckInDetailDTOBindingSource;  // control's DataSource.
                nameDataGridViewTextBoxColumn.DefaultCellStyle.NullValue = MessageContainerList.GetMessage(utilities.ExecutionContext, 4384, stringGuestOrAdultInitCap);
                dateOfBirthDataGridViewTextBoxColumn.DefaultCellStyle.NullValue = KioskStatic.DateMonthFormat;
                dateOfBirthDataGridViewTextBoxColumn.DefaultCellStyle.Format = KioskStatic.DateMonthFormat;
                dgvEnterAdultDetails.AllowUserToAddRows = false;
                ClearDGV();
                if (selectedRelationList == null)
                {
                    selectedRelationList = new List<CustomCustomerDTO>(purchaseProductDTO.ParentCustomerId);
                }
                int diffCount = adultQtySelectedInQtyScreen - ((selectedRelationList == null) ? 0 : selectedRelationList.Count);
                if (selectedRelationList != null && selectedRelationList.Any())
                {
                    AutoPopulateAdultDetails(selectedRelationList);
                }
                for (int i = 0; i < diffCount; i++)
                {
                    InsertNewCustomCheckInDetailDTO(null);
                }
                UpdateBindDataSource();
                UpdateCheckInGuestsCount();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                displayMessageLine(ex.Message, ERROR);
                KioskStatic.logToFile("Unexpected error while frmEnterAdultDetails_Load(): " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            StopKioskTimer(); 
            DialogResult = DialogResult.No;
            Close(); 
            log.LogMethodExit();
        }

        private void btnProceed_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();
            try
            {
                if (!ValidateAgeCreteriaOfAllProducts())
                    return;

                AttachCheckInDetailDTOToTrxLines();
                DisposeKeyboardObject();
                StopKioskTimer();
                using (frmAdultSummary frm = new frmAdultSummary(kioskTransaction, trxLines, purchaseProductDTO))
                {
                    DialogResult dresult = frm.ShowDialog();
                    kioskTransaction = frm.GetKioskTransaction;
                    if (dresult != System.Windows.Forms.DialogResult.No) // back button pressed
                    {
                        DialogResult = dresult;
                        this.Close();
                        log.LogMethodExit();
                        return;
                    }
                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error while processing frmEnterAdultDetails btnProceed_Click:" + ex.Message);
            }
            finally
            {
                StartKioskTimer();
            }
            log.LogMethodExit();
        }

        private void dgvEnterAdultDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();

            if (e.RowIndex < 0)
                return;

            dgvEnterAdultDetails.EndEdit();
            try
            {
                if (dgvEnterAdultDetails.Rows[e.RowIndex].ReadOnly)
                    return;

                if (dgvEnterAdultDetails.Columns[e.ColumnIndex].Name == "LinkRelationdgvBtn")
                {
                    //if customer is already linked, ignore the click to link relation button
                    if (customCheckInDetailDTOList[e.RowIndex].CustomerId > -1)
                    {
                        string msg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4811); //Relation is already linked
                        displayMessageLine(msg, WARNING);
                        return;
                    }

                    if (string.IsNullOrEmpty(customCheckInDetailDTOList[e.RowIndex].Name)
                        || (customCheckInDetailDTOList[e.RowIndex].DateOfBirth == null || customCheckInDetailDTOList[e.RowIndex].DateOfBirth == DateTime.MinValue))
                    {
                        string msg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4473); //Required fields are not entered
                        frmOKMsg.ShowUserMessage(msg);
                        displayMessageLine(msg, WARNING);
                        return;
                    }

                    if (!ValidateDateOfBirth(e.RowIndex))
                        return;

                    txtMessage.Text = "";
                    CustomCustomerDTO linkedRelatedCustomerDTO = LinkRelationToParentCustomer(customCheckInDetailDTOList[e.RowIndex]);
                    if (linkedRelatedCustomerDTO != null)
                    {
                        if (selectedRelationList == null)
                        {
                            selectedRelationList = new List<CustomCustomerDTO>(purchaseProductDTO.ParentCustomerId);
                        }
                        selectedRelationList.Add(linkedRelatedCustomerDTO);
                        customCheckInDetailDTOList.RemoveAt(e.RowIndex);
                        CustomCheckInDetailDTO newCheckInDetailDTO = new CustomCheckInDetailDTO();
                        newCheckInDetailDTO.Name = linkedRelatedCustomerDTO.RelatedCustomerName;
                        newCheckInDetailDTO.DateOfBirth = linkedRelatedCustomerDTO.DOB;
                        newCheckInDetailDTO.Age = linkedRelatedCustomerDTO.Age;
                        newCheckInDetailDTO.CustomerId = linkedRelatedCustomerDTO.RelatedCustomerId;
                        customCheckInDetailDTOList.Insert(e.RowIndex, newCheckInDetailDTO);
                        dgvEnterAdultDetails.Rows[e.RowIndex].Cells["LinkRelationdgvBtn"].Value = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4474); //Linked
                        dgvEnterAdultDetails.Rows[e.RowIndex].Cells["LinkRelationdgvBtn"].Style.SelectionBackColor = Color.Green;
                        dgvEnterAdultDetails.Rows[e.RowIndex].Cells["LinkRelationdgvBtn"].Style.SelectionForeColor = Color.White;
                        dgvEnterAdultDetails.Rows[e.RowIndex].Cells["LinkRelationdgvBtn"].Style.BackColor = Color.Green;
                        dgvEnterAdultDetails.Rows[e.RowIndex].Cells["LinkRelationdgvBtn"].Style.ForeColor = Color.White;
                        //DateTime dateOfBirth = Convert.ToDateTime(newCheckInDetailDTO.DateOfBirth);
                        //dgvEnterAdultDetails.Rows[e.RowIndex].Cells["relativeDayOfDOB"].Value = dateOfBirth.Day.ToString();
                        //dgvEnterAdultDetails.Rows[e.RowIndex].Cells["relativeMonthOfDOB"].Value = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(dateOfBirth.Month);
                        //dgvEnterAdultDetails.Rows[e.RowIndex].Cells["relativeYearOfDOB"].Value = dateOfBirth.Year.ToString();
                        dgvEnterAdultDetails.Rows[e.RowIndex].Cells["dateOfBirthDataGridViewTextBoxColumn"].Value = newCheckInDetailDTO.DateOfBirth;
                        UpdateBindDataSource();
                        dgvEnterAdultDetails.EndEdit();
                        dgvEnterAdultDetails.Refresh();
                    }
                }
                else if (dgvEnterAdultDetails.Columns[e.ColumnIndex].Name == "nameDataGridViewTextBoxColumn")
                {
                    if (dgvEnterAdultDetails.Rows[e.RowIndex].Cells["nameDataGridViewTextBoxColumn"].Value != null
                          && dgvEnterAdultDetails.Rows[e.RowIndex].Cells["nameDataGridViewTextBoxColumn"].Value.ToString() == MessageContainerList.GetMessage(utilities.ExecutionContext, 4384, stringGuestOrAdultInitCap))
                    {
                        dgvEnterAdultDetails.Rows[e.RowIndex].Cells["nameDataGridViewTextBoxColumn"].Value = string.Empty;
                    }
                }
                else if (dgvEnterAdultDetails.Columns[e.ColumnIndex].Name == "deleteDataGridViewImageColumn")
                {
                    if (!string.IsNullOrEmpty(dgvEnterAdultDetails.CurrentRow.Cells["nameDataGridViewTextBoxColumn"].Value.ToString()))
                    {
                        CustomCheckInDetailDTO customCheckInDetailDTO = new CustomCheckInDetailDTO();
                        customCheckInDetailDTO.Name = MessageContainerList.GetMessage(utilities.ExecutionContext, 4384, stringGuestOrAdultInitCap);
                        customCheckInDetailDTO.DateOfBirth = null;
                        customCheckInDetailDTOList.RemoveAt(dgvEnterAdultDetails.CurrentRow.Index);
                        customCheckInDetailDTOList.Add(customCheckInDetailDTO);
                        UpdateBindDataSource();

                        if (customCheckInDetailDTOList[dgvEnterAdultDetails.CurrentRow.Index].CustomerId != -1)
                        {
                            int index = selectedRelationList.IndexOf(selectedRelationList.Where(l => l.RelatedCustomerId == customCheckInDetailDTOList[dgvEnterAdultDetails.CurrentRow.Index].CustomerId).FirstOrDefault());
                            selectedRelationList.RemoveAt(index);
                        }
                    }
                }
                else if (dgvEnterAdultDetails.Columns[e.ColumnIndex].Name == "dateOfBirthDataGridViewTextBoxColumn")
                {
                    if (customCheckInDetailDTOList[e.RowIndex].CustomerId > 0)
                    {
                        dgvEnterAdultDetails.CurrentCell.ReadOnly = true;
                    }
                    else
                    {
                        StopKioskTimer();
                        DisposeKeyboardObject();
                        DateTime doB = customCheckInDetailDTOList[e.RowIndex].DateOfBirth == null ?
                            DateTime.MinValue : Convert.ToDateTime(customCheckInDetailDTOList[e.RowIndex].DateOfBirth);
                        DateTime newDOB = KioskHelper.LaunchCalendar(defaultDateTimeToShow: doB, enableDaySelection: KioskStatic.enableDaySelection, enableMonthSelection: KioskStatic.enableMonthSelection,
                            enableYearSelection: KioskStatic.enableYearSelection, disableTill: DateTime.MinValue, showTimePicker: false, popupAlerts: frmOKMsg.ShowUserMessage);
                        if (newDOB != null && newDOB != DateTime.MinValue)
                        {
                            dgvEnterAdultDetails.Rows[e.RowIndex].Cells["dateOfBirthDataGridViewTextBoxColumn"].Value = newDOB.Date.ToString();
                        }
                        string dateMonthformat = KioskStatic.DateMonthFormat;
                        DateTime dateofBirthValue = DateTime.MinValue;
                        try
                        {
                            dateofBirthValue = KioskHelper.GetFormatedDateValue(newDOB);
                            if (dateofBirthValue != null && dateofBirthValue != Convert.ToDateTime(customCheckInDetailDTOList[dgvEnterAdultDetails.CurrentRow.Index].DateOfBirth))
                            {
                                dgvEnterAdultDetails.Rows[dgvEnterAdultDetails.CurrentRow.Index].Cells["dateOfBirthDataGridViewTextBoxColumn"].Value = dateofBirthValue.Date.ToString(KioskStatic.DateMonthFormat);
                                customCheckInDetailDTOList[dgvEnterAdultDetails.CurrentRow.Index].DateOfBirth = dateofBirthValue;
                                customCheckInDetailDTOList[dgvEnterAdultDetails.CurrentRow.Index].Age = KioskHelper.GetAge(dateofBirthValue.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.CurrentCulture;
                                dateofBirthValue = Convert.ToDateTime(newDOB, provider);
                                if (dateofBirthValue != null && dateofBirthValue != Convert.ToDateTime(customCheckInDetailDTOList[dgvEnterAdultDetails.CurrentRow.Index].DateOfBirth))
                                {
                                    dgvEnterAdultDetails.Rows[dgvEnterAdultDetails.CurrentRow.Index].Cells["dateOfBirthDataGridViewTextBoxColumn"].Value = dateofBirthValue.Date.ToString(KioskStatic.DateMonthFormat);
                                    customCheckInDetailDTOList[dgvEnterAdultDetails.CurrentRow.Index].DateOfBirth = dateofBirthValue;
                                    customCheckInDetailDTOList[dgvEnterAdultDetails.CurrentRow.Index].Age = KioskHelper.GetAge(dateofBirthValue.ToString());
                                }
                            }
                            catch (Exception exp)
                            {
                                displayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext, 449, dateMonthformat, Convert.ToDateTime("23-Feb-1982").ToString(dateMonthformat)), ERROR);
                                log.Error(exp);
                                log.Error(MessageContainerList.GetMessage(utilities.ExecutionContext, 449, dateMonthformat, Convert.ToDateTime("23-Feb-1982").ToString(dateMonthformat)));
                            }
                            log.Error(ex);
                        }
                        StartKioskTimer();
                    }
                }
                UpdateCheckInGuestsCount();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected error while processing deleteDataGridViewImageColumn of dgvEnterAdultDetails: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void CustomCheckInDetailDTOBindingSource_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();
            e.Cancel = true;
            log.LogMethodExit(null);
        }

        private void dgvEnterAdultDetails_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                if (e.Control is TextBox)
                {
                    CurrentActiveTextBox = e.Control as TextBox;

                    Font font = new Font(e.CellStyle.Font.FontFamily, e.CellStyle.Font.Size);
                    e.CellStyle = dgvEnterAdultDetails.DefaultCellStyle;
                    e.CellStyle.Font = font;

                }
                else if (e.Control is ComboBox)
                {
                    ComboBox cb = e.Control as ComboBox;
                    if (cb != null)
                    {
                        cb.IntegralHeight = false;
                        cb.MaxDropDownItems = 10;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error while setting cell style in dgvCustomCustomerDTOList_EditingControlShowing() : " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void dgvEnterAdultDetails_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (customCheckInDetailDTOBindingSource.DataSource is SortableBindingList<CustomCheckInDetailDTO>)
                {
                    customCheckInDetailDTOList = (SortableBindingList<CustomCheckInDetailDTO>)customCheckInDetailDTOBindingSource.DataSource;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error while dgvEnterAdultDetails_DataBindingComplete() : " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void dgvEnterAdultDetails_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();

            if (e.RowIndex < 0)
                return;

            try
            {
                var addImage = Properties.Resources.Add;
                var deleteImage = Properties.Resources.Delete;

                if (dgvEnterAdultDetails.Columns[e.ColumnIndex].Name == "deleteDataGridViewImageColumn")
                {
                    if ((dgvEnterAdultDetails.Rows[e.RowIndex].Cells["nameDataGridViewTextBoxColumn"].Value != null)
                          && (dgvEnterAdultDetails.Rows[e.RowIndex].Cells["nameDataGridViewTextBoxColumn"].Value.ToString() != MessageContainerList.GetMessage(utilities.ExecutionContext, 4384, stringGuestOrAdultInitCap))
                          && !string.IsNullOrWhiteSpace(dgvEnterAdultDetails.Rows[e.RowIndex].Cells["nameDataGridViewTextBoxColumn"].Value.ToString()))
                    {
                        e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                        var w = deleteImage.Width;
                        var h = deleteImage.Height;
                        var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                        var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;

                        e.Graphics.DrawImage(deleteImage, new Rectangle(x, y, w, h));
                        e.Handled = true;
                    }
                    else
                    {
                        e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                        var w = addImage.Width;
                        var h = addImage.Height;
                        var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                        var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;

                        e.Graphics.DrawImage(addImage, new Rectangle(x, y, w, h));
                        e.Handled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error while dgvEnterAdultDetails_CellPainting(): " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void dgvEnterAdultDetails_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();

            if (e.RowIndex < 0)
                return;

            dgvEnterAdultDetails.EndEdit();
            try
            {
                if (dgvEnterAdultDetails.Columns[e.ColumnIndex].Name == "nameDataGridViewTextBoxColumn")
                {
                    if ((dgvEnterAdultDetails.CurrentRow.Cells["nameDataGridViewTextBoxColumn"].Value) != null)
                    {
                        if (string.IsNullOrWhiteSpace(dgvEnterAdultDetails.CurrentRow.Cells["nameDataGridViewTextBoxColumn"].Value.ToString()))
                        {
                            customCheckInDetailDTOList[e.RowIndex].Name = MessageContainerList.GetMessage(utilities.ExecutionContext, 4384, stringGuestOrAdultInitCap);
                        }
                        dgvEnterAdultDetails.CurrentCell.Style.BackColor = Color.White;
                        UpdateCheckInGuestsCount();
                    }
                }
                //else if ((dgvEnterAdultDetails.Columns[e.ColumnIndex].Name == "relativeDayOfDOB")
                //        || (dgvEnterAdultDetails.Columns[e.ColumnIndex].Name == "relativeMonthOfDOB")
                //        || (dgvEnterAdultDetails.Columns[e.ColumnIndex].Name == "relativeYearOfDOB"))
                //{
                //    int selectedCustomerId = Convert.ToInt32(dgvEnterAdultDetails.CurrentRow.Cells["customerIdDataGridViewTextBoxColumn"].Value);
                //    if (selectedCustomerId != -1)
                //    {
                //        dgvEnterAdultDetails.CurrentRow.ReadOnly = true;
                //    }
                //    UpdateCheckInDetailDTODOB(e.RowIndex);
                //}
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("dgvEnterAdultDetails_CellLeave: Unexpected error while handling wrong Date Of Birth : " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void HighlightErrorField(int rowIndex)
        {
            log.LogMethodEntry(rowIndex);
            ResetKioskTimer();

            try
            {
                //dgvEnterAdultDetails.Rows[rowIndex].Cells["relativeDayOfDOB"].Style.SelectionBackColor = Color.Red;
                //dgvEnterAdultDetails.Rows[rowIndex].Cells["relativeMonthOfDOB"].Style.SelectionBackColor = Color.Red;
                //dgvEnterAdultDetails.Rows[rowIndex].Cells["relativeYearOfDOB"].Style.SelectionBackColor = Color.Red;
                dgvEnterAdultDetails.Rows[rowIndex].Cells["dateOfBirthDataGridViewTextBoxColumn"].Style.SelectionBackColor = Color.Red;
                string msg = MessageContainerList.GetMessage(utilities.ExecutionContext, 4288);
                frmOKMsg.ShowUserMessage(msg);
                log.Error(msg);
                displayMessageLine(msg, ERROR);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("HighlightErrorField(): Unexpected error while handling wrong Date Of Birth: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void DownButtonClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }

        private void UpButtonClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }
        private void LeftButtonClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }

        private void RightButtonClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }

        private void UpdateCheckInDetailDTODOB(int rowIndex)
        {
            log.LogMethodEntry(rowIndex);
            ResetKioskTimer();
            try
            {
                string day = dgvEnterAdultDetails.Rows[rowIndex].Cells["relativeDayOfDOB"].EditedFormattedValue.ToString();
                string month = dgvEnterAdultDetails.Rows[rowIndex].Cells["relativeMonthOfDOB"].EditedFormattedValue.ToString();
                string year = dgvEnterAdultDetails.Rows[rowIndex].Cells["relativeYearOfDOB"].EditedFormattedValue.ToString();

                if ((!string.IsNullOrEmpty(day) && !(day.Equals(relativeDayOfDOB.Items[0])))
                     && (!(string.IsNullOrEmpty(month)) && !(month.Equals(relativeMonthOfDOB.Items[0])))
                     && (!(string.IsNullOrEmpty(year)) && !(year.Equals(relativeYearOfDOB.Items[0]))))
                {
                    string monthNumber = DateTime.ParseExact(month, "MMM", CultureInfo.CurrentCulture).Month.ToString();
                    DateTime doB = new DateTime(Convert.ToInt32(year), Convert.ToInt32(monthNumber), Convert.ToInt32(day));
                    //string dateOfBirth = doB.ToString(KioskStatic.DateMonthFormat);
                    customCheckInDetailDTOList[rowIndex].DateOfBirth = doB;
                    customCheckInDetailDTOList[rowIndex].Age = KioskHelper.GetAge(doB.ToString(KioskStatic.DateMonthFormat)); 
                    UpdateBindDataSource();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("UpdateCheckInDetailDTODOB(): Unexpected error while handling wrong Date Of Birth : " + ex.Message);
            }
            log.LogMethodExit();
        }

        private bool ValidateDateOfBirth(int rowIndex)
        {
            log.LogMethodEntry(rowIndex);
            ResetKioskTimer();
            bool status = false;
            try
            {
                string dateOfBirth = customCheckInDetailDTOList[rowIndex].DateOfBirth.ToString();
                if (customCheckInDetailDTOList[rowIndex].DateOfBirth != null && customCheckInDetailDTOList[rowIndex].DateOfBirth != DateTime.MinValue)
                {
                    decimal age = KioskHelper.GetAge(dateOfBirth);
                    if (age >= productsContainerDTO.AgeLowerLimit && age <= productsContainerDTO.AgeUpperLimit)
                    {
                        dgvEnterAdultDetails.Rows[rowIndex].Cells["dateOfBirthDataGridViewTextBoxColumn"].Style.SelectionBackColor = this.dgvEnterAdultDetails.DefaultCellStyle.SelectionBackColor;
                        //customCheckInDetailDTOList[rowIndex].DateOfBirth = dateOfBirth;
                        status = true;
                        UpdateBindDataSource();
                    }
                }

                if (status == false)
                    HighlightErrorField(rowIndex);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("ValidateDateOfBirth(): Unexpected error while handling wrong Date Of Birth : " + ex.Message);
            }
            log.LogMethodExit(status);
            return status;
        }

        private void AutoPopulateAdultDetails(List<CustomCustomerDTO> adultDTOList)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                if (adultDTOList == null || adultDTOList.Count == 0)
                    return;

                int i = 0;
                foreach (CustomCustomerDTO adultDTO in adultDTOList)
                {
                    CustomCheckInDetailDTO adultCheckInDetailDTO = new CustomCheckInDetailDTO();
                    adultCheckInDetailDTO.Name = adultDTO.RelatedCustomerName;
                    adultCheckInDetailDTO.DateOfBirth = adultDTO.DOB;
                    adultCheckInDetailDTO.Age = adultDTO.Age;
                    adultCheckInDetailDTO.CustomerId = adultDTO.RelatedCustomerId;
                    InsertNewCustomCheckInDetailDTO(adultCheckInDetailDTO);

                    dgvEnterAdultDetails.Rows[i].Cells["dateOfBirthDataGridViewTextBoxColumn"].Value = selectedRelationList[i].DOB;

                    //since disable option is not there, just remove text on the button and handle in cell click action
                    dgvEnterAdultDetails.Rows[i].Cells["LinkRelationdgvBtn"].Value = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4474); //Linked
                    dgvEnterAdultDetails.Rows[i].Cells["LinkRelationdgvBtn"].Style.SelectionBackColor = Color.Green;
                    dgvEnterAdultDetails.Rows[i].Cells["LinkRelationdgvBtn"].Style.SelectionForeColor = Color.White;
                    dgvEnterAdultDetails.Rows[i].Cells["LinkRelationdgvBtn"].Style.BackColor = Color.Green;
                    dgvEnterAdultDetails.Rows[i].Cells["LinkRelationdgvBtn"].Style.ForeColor = Color.White;

                    dgvEnterAdultDetails.Rows[i].ReadOnly = true;
                    i++;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error while AutoPopulateChildDetails():" + ex);
            }
            log.LogMethodExit();
        }

        private void AttachCheckInDetailDTOToTrxLines()
        {
            log.LogMethodEntry();
            ResetKioskTimer();

            for (int i = 0; i < customCheckInDetailDTOList.Count; i++)
            {
                trxLines[i].LineCheckInDetailDTO = customCheckInDetailDTOList[i].CheckInDetailDTO;
            }
            log.LogMethodExit();
        }

        private bool ValidateAgeCreteriaOfAllProducts()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            bool status = false;
            try
            {
                if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(utilities.ExecutionContext, "IGNORE_CUSTOMER_BIRTH_YEAR"))
                {
                    //When Ignore birth year is set, check-in feature is expected to work like phase-1.
                    //Date of Birth validation in that case is to be skipped
                    return true;
                }

                if (productsContainerDTO.CustomerProfilingGroupId == -1)
                {
                    //If there is no customer profile set, Date of Birth validation is to be skipped
                    return true;
                }

                for (int i = 0; i < customCheckInDetailDTOList.Count; i++)
                {
                    status = ValidateDateOfBirth(i);

                    if (!status)
                        break;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected error while ValidateAgeCreteriaOfAllProducts() in enter adult screen:" + ex.Message);
            }
            log.LogMethodExit(status);
            return status;
        }

        private void InsertNewCustomCheckInDetailDTO(CustomCheckInDetailDTO inAdultDTO = null)
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
                CustomCheckInDetailDTO customCheckInDetailDTO = new CustomCheckInDetailDTO();
                if (inAdultDTO != null)
                {
                    customCheckInDetailDTO = inAdultDTO;
                }
                else
                {
                    customCheckInDetailDTO.Name = MessageContainerList.GetMessage(utilities.ExecutionContext, 4384, stringGuestOrAdultInitCap);
                }
                customCheckInDetailDTOList.Add(customCheckInDetailDTO);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected error while InsertNewCustomCheckInDetailDTO() in frmEnterAdultDetails: " + ex);
            }
            log.LogMethodExit();
        }

        private void UpdateCheckInGuestsCount()
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();

                int selectedGuestCount = customCheckInDetailDTOList.Count(k => !string.IsNullOrWhiteSpace(k.Name)
                        && k.Name != MessageContainerList.GetMessage(utilities.ExecutionContext, 4384, stringGuestOrAdultInitCap));
                lblGuestCount.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 4776, selectedGuestCount, adultQtySelectedInQtyScreen); //"Quantity:&1"
                btnProceed.Enabled = (selectedGuestCount < adultQtySelectedInQtyScreen) ? false : true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected error while UpdateCheckInGuestsCount() in adult screen : " + ex);
            }
            log.LogMethodExit();
        }

        private void UpdateBindDataSource()
        {
            log.LogMethodEntry();

            if (dgvEnterAdultDetails.Rows.Count < 0)
                return;

            try
            {
                ResetKioskTimer();
                //dateOfBirthDataGridViewTextBoxColumn.DefaultCellStyle.NullValue = KioskStatic.DateMonthFormat;
                dgvEnterAdultDetails.Rows[customCheckInDetailDTOList.Count - 1].Selected = true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected error while dgvEnterAdultDetails_UpdateBindDataSource() : " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void ClearDGV()
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
                customCheckInDetailDTOBindingSource.DataSource = new SortableBindingList<CustomCheckInDetailDTO>();
                dgvEnterAdultDetails.DataSource = customCheckInDetailDTOBindingSource;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected error while dgvEnterAdultDetails_ClearData() : " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void GetColumnsToDisplay()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            //KioskStatic.logToFile("Fetching Lookup values for dgvEnterAdultDetails column display fields");
            try
            {
                LookupsContainerDTO CheckinFieldsLookupValuesList = LookupsContainerList.GetLookupsContainerDTO(-1, "KIOSK_CHECKIN_DISPLAY_FIELDS_CONFIG");
                if (CheckinFieldsLookupValuesList != null)
                {
                    for (int i = 0; i < CheckinFieldsLookupValuesList.LookupValuesContainerDTOList.Count; i++)
                    {
                        switch (CheckinFieldsLookupValuesList.LookupValuesContainerDTOList[i].LookupValue)
                        {
                            case "Name":
                                nameDataGridViewTextBoxColumn.Visible = (CheckinFieldsLookupValuesList.LookupValuesContainerDTOList[i].Description == "Y") ? true : false;
                                nameDataGridViewTextBoxColumn.HeaderText += "*";
                                break;

                            case "VehicleNumber":
                                vehicleNumberDataGridViewTextBoxColumn.Visible = (CheckinFieldsLookupValuesList.LookupValuesContainerDTOList[i].Description == "Y") ? true : false;
                                //vehicleNumberDataGridViewTextBoxColumn.HeaderText += "*";
                                break;

                            case "VehicleModel":
                                vehicleModelDataGridViewTextBoxColumn.Visible = (CheckinFieldsLookupValuesList.LookupValuesContainerDTOList[i].Description == "Y") ? true : false;
                                //vehicleNumberDataGridViewTextBoxColumn.HeaderText += "*";
                                break;

                            case "VehicleColour":
                                vehicleColorDataGridViewTextBoxColumn.Visible = (CheckinFieldsLookupValuesList.LookupValuesContainerDTOList[i].Description == "Y") ? true : false;
                                //vehicleColorDataGridViewTextBoxColumn.HeaderText += "*";
                                break;

                            case "DateOfBirth":
                                bool value = (CheckinFieldsLookupValuesList.LookupValuesContainerDTOList[i].Description == "Y") ? true : false;
                                dateOfBirthDataGridViewTextBoxColumn.Visible = value;
                                if ((ParafaitDefaultContainerList.GetParafaitDefault<bool>(utilities.ExecutionContext, "IGNORE_CUSTOMER_BIRTH_YEAR", false) == false)
                                    && (productsContainerDTO.CustomerProfilingGroupId > -1))
                                {
                                    dateOfBirthDataGridViewTextBoxColumn.HeaderText += "*";
                                }
                                break;

                            case "Age":
                                ageDataGridViewTextBoxColumn.Visible = (CheckinFieldsLookupValuesList.LookupValuesContainerDTOList[i].Description == "Y") ? true : false;
                                //ageDataGridViewTextBoxColumn.HeaderText += "*";
                                break;

                            case "SpecialNeeds":
                                specialNeedsDataGridViewTextBoxColumn.Visible = (CheckinFieldsLookupValuesList.LookupValuesContainerDTOList[i].Description == "Y") ? true : false;
                                //specialNeedsDataGridViewTextBoxColumn.HeaderText += "*";
                                break;

                            case "Allergies":
                                allergiesDataGridViewTextBoxColumn.Visible = (CheckinFieldsLookupValuesList.LookupValuesContainerDTOList[i].Description == "Y") ? true : false;
                                //allergiesDataGridViewTextBoxColumn.HeaderText += "*";
                                break;

                            default:
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected error Fetching Lookup values for dgvEnterAdultDetails column display fields: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private CustomCustomerDTO LinkRelationToParentCustomer(CustomCheckInDetailDTO customCheckInDetailDTO)
        {
            log.LogMethodEntry();
            StopKioskTimer();

            CustomCustomerDTO linkedRelatedCustomerDTO = null;
            try
            {
                DisposeKeyboardObject();
                using (frmAddCustomerRelation frm = new frmAddCustomerRelation(parentCustomerDTO.Id
                         , (parentCustomerDTO.FirstName + " " + parentCustomerDTO.LastName), parentCustomerDTO.CardNumber
                         , customCheckInDetailDTO.Name, customCheckInDetailDTO.DateOfBirth.ToString()))
                {
                    DialogResult dr = frm.ShowDialog();
                    if (dr == DialogResult.OK)
                    {
                        linkedRelatedCustomerDTO = frm.LinkedCustomerDTO;
                        frm.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error while UpdateCheckInGuestsCount() in child screen: " + ex.Message);
            }
            finally
            {
                StartKioskTimer();
            }
            log.LogMethodExit(linkedRelatedCustomerDTO);
            return linkedRelatedCustomerDTO;
        }

        private void SortDOBFields()
        {
            log.LogMethodEntry();
            ResetKioskTimer();

            try
            {
                //Convert date format into array of characters and remove duplicate characters
                var uniqueCharArray = KioskStatic.DateMonthFormat.ToLower().ToCharArray().Distinct().ToArray();
                var resultString = new String(uniqueCharArray.Where(Char.IsLetter).ToArray());

                int displayIndex = relativeDayOfDOB.DisplayIndex;
                switch (resultString)
                {
                    case "dmy":
                        relativeDayOfDOB.DisplayIndex = displayIndex;
                        relativeMonthOfDOB.DisplayIndex = displayIndex + 1;
                        relativeYearOfDOB.DisplayIndex = displayIndex + 2;
                        dOBFirstColumnIndex = dgvEnterAdultDetails.Columns["relativeDayOfDOB"].Index;
                        break;

                    case "mdy":
                        relativeDayOfDOB.DisplayIndex = displayIndex + 1;
                        relativeMonthOfDOB.DisplayIndex = displayIndex;
                        relativeYearOfDOB.DisplayIndex = displayIndex + 2;
                        dOBFirstColumnIndex = dgvEnterAdultDetails.Columns["relativeMonthOfDOB"].Index;
                        break;

                    case "ymd":
                        relativeDayOfDOB.DisplayIndex = displayIndex + 2;
                        relativeMonthOfDOB.DisplayIndex = displayIndex + 1;
                        relativeYearOfDOB.DisplayIndex = displayIndex;
                        dOBFirstColumnIndex = dgvEnterAdultDetails.Columns["relativeYearOfDOB"].Index;
                        break;

                    default:
                        relativeDayOfDOB.DisplayIndex = displayIndex;
                        relativeMonthOfDOB.DisplayIndex = displayIndex + 1;
                        relativeYearOfDOB.DisplayIndex = displayIndex + 2;
                        dOBFirstColumnIndex = dgvEnterAdultDetails.Columns["relativeDayOfDOB"].Index;
                        break;
                }
            }
            catch (Exception ex)
            {
                string msg = "ERROR: Failed to sort the DOB fields in Enter Child Details()";
                log.Error(msg + ex);
                KioskStatic.logToFile(msg + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetVirtualKeyboard()
        {
            log.LogMethodEntry();
            try
            {
                virtualKeyboardController = new VirtualWindowsKeyboardController(panelEnterAdultDetails.Top);
                bool popupOnScreenKeyBoard = true;
                virtualKeyboardController.Initialize(this, new List<Control>() { btnShowKeypad }, popupOnScreenKeyBoard);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error Initializing Windows Keyboard in  Add Relation screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetCustomKeyboard()
        {
            log.LogMethodEntry();
            try
            {
                customKeyboardController = new VirtualKeyboardController(panelEnterAdultDetails.Top);
                bool showKeyboardOnTextboxEntry = true;
                customKeyboardController.Initialize(this, new List<Control>() { btnShowKeypad }, showKeyboardOnTextboxEntry,null, lblScreenDetails.Font.FontFamily.Name);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error Initializing Custom Keyboard in  Add Relation screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void btnShowKeyPad_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }

        private void InitializeKeyboard()
        {
            log.LogMethodEntry();
            try
            {
                bool isWindowsKeyboardEnabled = ParafaitDefaultContainerList.GetParafaitDefault<bool>(KioskStatic.Utilities.ExecutionContext, "ENABLE_WINDOWS_KEYBOARD", false);
                if (isWindowsKeyboardEnabled)
                {
                    SetVirtualKeyboard();
                }
                else
                {
                    SetCustomKeyboard();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error Initializing keyboard in  Add Relation screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void DisposeKeyboardObject()
        {
            log.LogMethodEntry();
            try
            {
                bool isWindowsKeyboardEnabled = ParafaitDefaultContainerList.GetParafaitDefault<bool>(KioskStatic.Utilities.ExecutionContext, "ENABLE_WINDOWS_KEYBOARD", false);
                if (isWindowsKeyboardEnabled)
                {
                    virtualKeyboardController.Dispose();
                }
                else
                {
                    customKeyboardController.Dispose();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error Initializing keyboard in  Add Relation screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        public override void KioskTimer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            int tickSecondsRemaining = GetKioskTimerSecondsValue();
            tickSecondsRemaining--;
            setKioskTimerSecondsValue(tickSecondsRemaining);
            if (tickSecondsRemaining <= 60)
            {
                lblTimeRemaining.Font = TimeOutFont;
                lblTimeRemaining.Text = tickSecondsRemaining.ToString("#0");
            }
            else
            {
                lblTimeRemaining.Font = savTimeOutFont;
                lblTimeRemaining.Text = (tickSecondsRemaining / 60).ToString() + ":" + (tickSecondsRemaining % 60).ToString().PadLeft(2, '0');
            }

            if (tickSecondsRemaining == 10)
            {
                if (TimeOut.AbortTimeOut(this))
                {
                    ResetKioskTimer();
                }
                else
                    tickSecondsRemaining = 0;
            }

            if (tickSecondsRemaining <= 0)
            {
                displayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext, 457), WARNING);
                Application.DoEvents();
                base.CloseForms();
                Dispose();
            }
            log.LogMethodExit();
        }

        void displayMessageLine(string message, string msgType)
        {
            log.LogMethodEntry(message, msgType);
            ResetKioskTimer();
            txtMessage.Text = message;
            log.LogMethodExit();
        }

        private void SetCustomImages()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            KioskStatic.logToFile("Setting customized background images for Enter Adult Details screen");
            try
            {
                this.BackgroundImage = ThemeManager.GetBackgroundImage(ThemeManager.CurrentThemeImages.EnterAdultBackgroundImage);
                this.btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
                this.panelEnterAdultDetails.BackgroundImage = ThemeManager.CurrentThemeImages.KioskActivityTableImage;
                this.btnPrev.BackgroundImage = btnProceed.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
                this.lblTimeRemaining.BackgroundImage = ThemeManager.CurrentThemeImages.TimerBoxSmall;
            }
            catch (Exception ex)
            {
                log.Error("Error while Setting Customized background images for dgvEnterAdultDetails", ex);
                KioskStatic.logToFile("Error while setting customized background images for dgvEnterAdultDetails: " + ex);
            }
            log.LogMethodExit();
        }

        private void SetFont()
        {
            log.LogMethodEntry();
            ResetKioskTimer();

            try
            {
                dgvEnterAdultDetails.Font = new System.Drawing.Font(lblGreeting.Font.FontFamily, 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                dgvEnterAdultDetails.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font(lblGreeting.Font.FontFamily, 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                dgvEnterAdultDetails.DefaultCellStyle.Font =
                    dgvEnterAdultDetails.RowTemplate.DefaultCellStyle.Font =
                    dgvEnterAdultDetails.RowHeadersDefaultCellStyle.Font =
                    dgvEnterAdultDetails.RowsDefaultCellStyle.Font = new System.Drawing.Font(lblGreeting.Font.FontFamily, 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                lblGreeting.Font = new Font(lblGreeting.Font.FontFamily, 38F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                lblGuestCountHeader.Font = new Font(lblGreeting.Font.FontFamily, 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                lblAgeCriteriaHeader.Font = new Font(lblGreeting.Font.FontFamily, 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                lblPackageHeader.Font = new Font(lblGreeting.Font.FontFamily, 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                lblFacilityHeader.Font = new Font(lblGreeting.Font.FontFamily, 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                lblGuestCount.Font = new Font(lblGreeting.Font.FontFamily, 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                lblAgeCriteria.Font = new Font(lblGreeting.Font.FontFamily, 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                lblPackage.Font = new Font(lblGreeting.Font.FontFamily, 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                lblFacility.Font = new Font(lblGreeting.Font.FontFamily, 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                lblScreenDetails.Font = new Font(lblGreeting.Font.FontFamily, 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                lblGridFooterMsg.Font = new Font(lblGreeting.Font.FontFamily, 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                //dgvEnterAdultDetails.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.Highlight;
                //dgvEnterAdultDetails.ColumnHeadersDefaultCellStyle.ForeColor = SystemColors.HighlightText;
                dgvEnterAdultDetails.DefaultCellStyle.SelectionBackColor = this.dgvEnterAdultDetails.DefaultCellStyle.BackColor;
                dgvEnterAdultDetails.DefaultCellStyle.SelectionForeColor = this.dgvEnterAdultDetails.DefaultCellStyle.ForeColor;
            }
            catch (Exception ex)
            {
                string msg = "Unexpected error while Setting Customized background images for dgvEnterAdultDetails: ";
                log.Error(msg, ex);
                KioskStatic.logToFile(msg + ex);
            }
            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements for Enter Adult Details screen");
            try
            {
                this.lblGreeting.ForeColor = KioskStatic.CurrentTheme.EnterAdultDetailsGreetingLblTextForeColor;
                this.lblGridFooterMsg.ForeColor = KioskStatic.CurrentTheme.LblGridFooterMsgTextForeColor;
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.EnterAdultDetailsFooterTextMsgTextForeColor;//Footer text message
                this.btnProceed.ForeColor = KioskStatic.CurrentTheme.EnterAdultDetailsProceedButtonTextForeColor;
                this.btnPrev.ForeColor = KioskStatic.CurrentTheme.EnterAdultDetailsBackButtonTextForeColor;
                this.btnHome.ForeColor = KioskStatic.CurrentTheme.EnterAdultDetailsHomeButtonTextForeColor;//Footer text message
                this.lblTimeRemaining.ForeColor = KioskStatic.CurrentTheme.EnterAdultDetailsTimeRemainingLblTextForeColor;
                this.lblGuestCountHeader.ForeColor =
                    this.lblPackageHeader.ForeColor =
                    this.lblFacilityHeader.ForeColor =
                    this.lblAgeCriteriaHeader.ForeColor =
                    this.lblGuestCountHeader.ForeColor = KioskStatic.CurrentTheme.PackageDetailsLblHeaderTextForeColor;

                this.lblPackage.ForeColor =
                    this.lblFacility.ForeColor =
                    this.lblAgeCriteria.ForeColor = KioskStatic.CurrentTheme.PackageDetailsLblTextForeColor;
                this.bigVerticalScrollAdultDetails.InitializeScrollBar(ThemeManager.CurrentThemeImages.ScrollDownEnabled, ThemeManager.CurrentThemeImages.ScrollDownDisabled, ThemeManager.CurrentThemeImages.ScrollUpEnabled, ThemeManager.CurrentThemeImages.ScrollUpDisabled);
                this.bigHorizontalScrollAdultDetails.InitializeScrollBar(ThemeManager.CurrentThemeImages.ScrollLeftEnabled, ThemeManager.CurrentThemeImages.ScrollLeftDisabled, ThemeManager.CurrentThemeImages.ScrollRightEnabled, ThemeManager.CurrentThemeImages.ScrollRightDisabled);

                this.lblGuestCount.ForeColor = KioskStatic.CurrentTheme.GuestCountLblTextForeColor;
                this.lblScreenDetails.ForeColor = KioskStatic.CurrentTheme.ScreenDetailsLblTextForeColor;

                this.dgvEnterAdultDetails.ColumnHeadersDefaultCellStyle.ForeColor = KioskStatic.CurrentTheme.EnterAdultDetailsGridHeaderTextForeColor;

                this.dgvEnterAdultDetails.ForeColor = KioskStatic.CurrentTheme.EnterAdultDetailsGridTextForeColor;
                this.dgvEnterAdultDetails.DefaultCellStyle.ForeColor = KioskStatic.CurrentTheme.EnterAdultDetailsGridTextForeColor;
                this.dgvEnterAdultDetails.DefaultCellStyle.SelectionForeColor = KioskStatic.CurrentTheme.EnterAdultDetailsGridTextForeColor;
                this.dgvEnterAdultDetails.RowsDefaultCellStyle.ForeColor = KioskStatic.CurrentTheme.EnterAdultDetailsGridTextForeColor;
                this.dgvEnterAdultDetails.RowsDefaultCellStyle.SelectionForeColor = KioskStatic.CurrentTheme.EnterAdultDetailsGridTextForeColor;
            }
            catch (Exception ex)
            {
                log.Error("Error while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements for frmEnterChildDetails: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void frmEnterAdultDetails_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                StopKioskTimer();
                DisposeKeyboardObject();
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing frmEnterAdultDetails_FormClosed()", ex);
            }
            //Cursor.Hide();

            log.Info(this.Name + ": Form closed");
            log.LogMethodExit();
        }
    }
}
