/********************************************************************************************
* Project Name - Parafait_Kiosk
* Description  - Handles Playground Entry menu
* 
**************
**Version Log
**************
*Version       Date             Modified By        Remarks          
*********************************************************************************************
 *2.150.0.0    20-Sep-2021      Sathyavathi        Created for Check-In feature Phase-2
 *2.150.0.0    02-Dec-2022      Sathyavathi        Check-In feature Phase-2 Additional features
 *2.150.1      22-Feb-2023      Sathyavathi        Kiosk Cart Enhancements
 *2.155.0      22-Jun-2023      Sathyavathi        Attraction Sale in Kiosk - Calendar changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Transaction;
using System.Linq;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Product;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using System.Globalization;
using System.Threading;

namespace Parafait_Kiosk
{
    public partial class frmSelectAdult : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Utilities utilities = KioskStatic.Utilities;
        private List<CustomCustomerDTO> customCustomerDTOList = new List<CustomCustomerDTO>();
        private List<CustomCustomerDTO> selectedAdultList = new List<CustomCustomerDTO>(); //needed to auto populate the selected list in Child Entry screen
        private CustomerDTO parentCustomerDTO;
        private const string WARNING = "WARNING";
        private const string ERROR = "ERROR";
        private int adultQtySelectedInQtyScreen;
        private ProductsContainerDTO productsContainerDTO;
        private List<Semnox.Parafait.Transaction.Transaction.TransactionLine> trxLines;
        private PurchaseProductDTO purchaseProductDTO;

        internal List<Semnox.Parafait.Transaction.Transaction.TransactionLine> TrxLines { get { return trxLines; } }
        public KioskTransaction GetKioskTransaction { get { return kioskTransaction; } }

        public frmSelectAdult(KioskTransaction kioskTransaction, List<Semnox.Parafait.Transaction.Transaction.TransactionLine> inTrxLines, PurchaseProductDTO purchaseProd)
        {
            log.LogMethodEntry("kioskTransaction", inTrxLines, purchaseProd);
            InitializeComponent();
            this.kioskTransaction = kioskTransaction;
            this.trxLines = inTrxLines;
            this.purchaseProductDTO = purchaseProd;
            KioskStatic.setDefaultFont(this);

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
            txtMessage.Text = "";

            btnProceed.Enabled = false;
            try
            {
                if(trxLines != null)
                {
                    adultQtySelectedInQtyScreen = trxLines.Count;
                }
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
                    string errMsg = "ERROR: Could not retrieve product details from Purchase Product in Select Child screen";
                    log.Error(errMsg);
                    log.Debug(productQtyMapping);
                    KioskStatic.logToFile(errMsg);
                    return;
                }
                this.productsContainerDTO = productQtyMapping.ProductsContainerDTO;
                lblSiteName.Text = KioskStatic.SiteHeading;
                List<FacilityContainerDTO> facilityContainerDTOList = FacilityContainerList.GetFacilityContainerDTOCollection(utilities.ExecutionContext.SiteId).FacilitysContainerDTOList;
                FacilityContainerDTO facilityContainerDTO = facilityContainerDTOList.Where(f => f.FacilityId == productsContainerDTO.CheckInFacilityId).FirstOrDefault();
                string checkInFacilityName = facilityContainerDTO == null ? "" : facilityContainerDTO.FacilityName;

                bool isCustomerProfileExist = (productsContainerDTO.CustomerProfilingGroupId != -1) ? true : false;
                string appendGuestOrAdultString = MessageContainerList.GetMessage(utilities.ExecutionContext, isCustomerProfileExist ? 4342 : 4119);
                CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
                TextInfo textInfo = cultureInfo.TextInfo;
                appendGuestOrAdultString = textInfo.ToTitleCase(appendGuestOrAdultString);
                lblGreeting.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 4284, appendGuestOrAdultString);
                btnSkip.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 4469); //"Skip"
                lblMemberDetails.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 4346);
                lblYourFamily.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 4285);
                lblPackageHeader.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Package") + ":";//Package Name Header
                string productName = KioskHelper.GetProductName(productsContainerDTO.ProductId);
                lblPackage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, productName);//Product Name for which child is being selected
                lblFacilityHeader.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Playground") + ":";//facility Name Header
                lblFacility.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, checkInFacilityName);//facility name
                panelAgeCriteria.Visible = (productsContainerDTO.CustomerProfilingGroupId != -1) ? true : false;
                lblAgeCriteriaHeader.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 4812) + ":";//Age Criteria
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
                lblGuestCountHeader.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 4467, appendGuestOrAdultString); //"Adult Selected: "
                lblGuestCount.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 4776, (selectedAdultList == null) ? 0 : selectedAdultList.Count, adultQtySelectedInQtyScreen); //"Quantity:&1"
                lblScreenDetails.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 4788);

                panelNoRelation.Visible = false;
                SetFont();
                SetCustomImages();
                SetCustomizedFontColors();
                DisplaybtnCancel(false);
                DisplaybtnPrev(true);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error while frmSelectAdult(): " + ex.Message);
            }

            CustomCustomerDTO customCustomerDTO = new CustomCustomerDTO(purchaseProductDTO.ParentCustomerId);
            parentCustomerDTO = customCustomerDTO.GetCustomerDTO(purchaseProductDTO.ParentCustomerId);
            if (parentCustomerDTO == null)
            {
                string errMsg = "ERROR: parentCustomerDTO can not be null";
                log.Error(errMsg);
                KioskStatic.logToFile(errMsg);
                log.LogMethodExit();
                return;
            }
            GetLinkedRelationsMatchingAgeCriteria();
            if (customCustomerDTOList == null || customCustomerDTOList.Count == 0)
            {
                decimal parentAge = KioskHelper.GetAge(parentCustomerDTO.DateOfBirth.ToString());
                if (parentAge < productsContainerDTO.AgeLowerLimit || parentAge > productsContainerDTO.AgeUpperLimit)
                {
                    StopKioskTimer();
                    throw new RelationsNotExistException();
                }
                panelYourFamily.Visible = false;
                panelNoRelation.Visible = true;
                lblNoRelationMsg1.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 4400);
                lblNoRelationMsg2.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 4401);
                lblNoRelationMsg3.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 4402);
            }
            utilities.setLanguage(this);
            log.LogMethodExit();
        }

        private void frmSelectAdult_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();

            try
            {
                memberDateofBirth.DefaultCellStyle = KioskHelper.gridViewDateOfBirthCellStyle();
                int rowId = dgvMemberDetails.Rows.Add();
                DataGridViewRow newRow = dgvMemberDetails.Rows[rowId];
                newRow.Cells["MemberNameDataGridViewTextBoxColumn"].Value = (parentCustomerDTO.FirstName + parentCustomerDTO.LastName);
                newRow.Cells["CardNumberdataGridViewTextBoxColumn"].Value = parentCustomerDTO.CardNumber ?? "";
                newRow.Cells["memberDateofBirth"].Value = (parentCustomerDTO.DateOfBirth == null) ? "" 
                    : parentCustomerDTO.DateOfBirth.Value.ToString(KioskStatic.DateMonthFormat);

                //Logic to enable/Disable member selection based on age criteria
                if (parentCustomerDTO.DateOfBirth != null)
                {
                    newRow.Cells["editMemberBirthYear"].Value = MessageContainerList.GetMessage(utilities.ExecutionContext, 4134); // "Edit DOB"
                    if (!IsAgeCriteriaMatches(parentCustomerDTO.DateOfBirth.ToString()))
                    {
                        (dgvMemberDetails.Rows[rowId].Cells["MemberSelectDataGridViewTextBoxColumn"] as DataGridViewImageCell).Value = (System.Drawing.Image)Parafait_Kiosk.Properties.Resources.Check_Box_Empty;
                        int removeIndex = selectedAdultList.IndexOf(selectedAdultList.Where(k => k.RelatedCustomerId == parentCustomerDTO.Id).FirstOrDefault());
                        if (removeIndex > -1)
                        {
                            selectedAdultList.RemoveAt(removeIndex);
                        }
                        dgvMemberDetails.Rows[rowId].DefaultCellStyle.ForeColor = SystemColors.InactiveCaption;
                        dgvMemberDetails.Rows[rowId].DefaultCellStyle.SelectionForeColor = SystemColors.InactiveCaption;
                    }
                    else
                    {
                        this.dgvMemberDetails.Rows[rowId].DefaultCellStyle.SelectionBackColor = this.dgvYourFamily.DefaultCellStyle.BackColor;
                        this.dgvMemberDetails.Rows[rowId].DefaultCellStyle.SelectionForeColor = this.dgvYourFamily.DefaultCellStyle.ForeColor;
                    }
                }
                dgvYourFamily.Columns["editRelativeBirthYear"].DefaultCellStyle.NullValue = MessageContainerList.GetMessage(utilities.ExecutionContext, 4134); // "Edit DOB"

                this.dgvMemberDetails.ClearSelection();
                dgvMemberDetails.ReadOnly = true;
                dOBDataGridViewTextBoxColumn.DefaultCellStyle.Format = KioskStatic.DateMonthFormat;
                dgvYourFamily.DataSource = customCustomerDTOBindingSource;  // control's DataSource.
                customCustomerDTOBindingSource.DataSource = customCustomerDTOList;  // Bind the BindingSource to the DataGridView
            }
            catch (Exception ex)
            {
                log.Error(ex);
                displayMessageLine(ex.Message, ERROR);
                KioskStatic.logToFile("Error while frmSelectAdult_Load(): " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            StopKioskTimer();
            DialogResult = DialogResult.None;
            try
            {
                if (selectedAdultList.Any())
                {
                    string screen = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "quantity");
                    bool isShowCartInKiosk = ParafaitDefaultContainerList.GetParafaitDefault<bool>(KioskStatic.Utilities.ExecutionContext, "SHOW_CART_IN_KIOSK", false);
                    string msg = (isShowCartInKiosk == false) ? MessageContainerList.GetMessage(utilities.ExecutionContext, 4287, screen) //"This will abondon the transaction and take you back to the quantity screen"
                                    : MessageContainerList.GetMessage(utilities.ExecutionContext, 4850, screen) //"This action will remove the product from the Cart and take you back to the quantity screen"
                                    + Environment.NewLine
                                    + MessageContainerList.GetMessage(utilities.ExecutionContext, 4291); //"Are you sure you want to go back?"
                    using (frmYesNo frmOK = new frmYesNo(msg))
                    {
                        DialogResult dr = frmOK.ShowDialog();
                        if (dr != DialogResult.Yes)
                        {
                            log.LogMethodExit();
                            return;
                        }
                    }
                }
                DialogResult = DialogResult.No;
                Close();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                displayMessageLine(ex.Message, ERROR);
                KioskStatic.logToFile("Error in btnBack_Click: " + ex.Message);
            }
            finally
            {
                StartKioskTimer();
            }
            log.LogMethodExit();
        }

        private void btnSkip_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            StopKioskTimer();

            try
            {
                using (frmEnterAdultDetails frm = new frmEnterAdultDetails(kioskTransaction, trxLines, purchaseProductDTO, selectedAdultList))
                {
                    DialogResult dr = frm.ShowDialog();
                    if (dr == System.Windows.Forms.DialogResult.No) // back button pressed
                    {
                        if (frm.SelectedRelationList != null && frm.SelectedRelationList.Any())
                        {
                            this.selectedAdultList = frm.SelectedRelationList;
                            GetLinkedRelationsMatchingAgeCriteria();
                            customCustomerDTOBindingSource.DataSource = null;
                            customCustomerDTOBindingSource.DataSource = customCustomerDTOList;
                            dgvYourFamily.SuspendLayout();
                            dgvYourFamily.Refresh();
                            dgvYourFamily.ResumeLayout();
                            EnableCheckboxForSelectedRelations(selectedAdultList);
                            UpdateCheckInGuestsCount();
                        }
                        else
                        {
                            DisableCheckboxForSelectedChild();
                        }
                    }
                    else
                    {
                        DialogResult = dr;
                        Close();
                        log.LogMethodExit();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected error in btnSkip_Click(): " + ex.Message);
            }

            StartKioskTimer();
            log.LogMethodExit();
        }

        private void btnProceed_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            StopKioskTimer();

            try
            {
                if (selectedAdultList.Count < adultQtySelectedInQtyScreen)
                {
                    using (frmEnterAdultDetails frm = new frmEnterAdultDetails(kioskTransaction, trxLines, purchaseProductDTO, selectedAdultList))
                    {
                        DialogResult dr = frm.ShowDialog();
                        kioskTransaction = frm.GetKioskTransaction;
                        if (dr == System.Windows.Forms.DialogResult.No) // back button pressed
                        {
                            if (frm.SelectedRelationList != null && frm.SelectedRelationList.Any())
                            {
                                this.selectedAdultList = frm.SelectedRelationList;
                                GetLinkedRelationsMatchingAgeCriteria();
                                customCustomerDTOBindingSource.DataSource = null;
                                customCustomerDTOBindingSource.DataSource = customCustomerDTOList;
                                dgvYourFamily.SuspendLayout();
                                dgvYourFamily.Refresh();
                                dgvYourFamily.ResumeLayout();
                                EnableCheckboxForSelectedRelations(selectedAdultList);
                            }
                            else
                            {
                                DisableCheckboxForSelectedChild();
                            }
                        }
                        else
                        {
                            DialogResult = dr;
                            Close();
                            log.LogMethodExit();
                            return;
                        }
                    }
                }
                else
                {
                    AttachCheckInDetailDTOToTrxLines();
                    StopKioskTimer();
                    using (frmAdultSummary frmas = new frmAdultSummary(kioskTransaction, trxLines, purchaseProductDTO))
                    {
                        DialogResult dr = frmas.ShowDialog();
                        kioskTransaction = frmas.GetKioskTransaction;
                        if (dr != System.Windows.Forms.DialogResult.No) // back button pressed
                        {
                            DialogResult = dr;
                            Close();
                            log.LogMethodExit();
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected error in btnSkip_Click(): " + ex.Message);
            }
            finally
            {
                StartKioskTimer();
            }
            log.LogMethodExit();
        }

        private void dgvMemberDetails_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();

            if (e.RowIndex < 0)
                return;

            try
            {
                int index = dgvMemberDetails.CurrentRow.Index;

                if (dgvMemberDetails.Columns[e.ColumnIndex].Name == "editMemberBirthYear")
                {               
                    string text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Edit")
                        + " " + MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Member")
                        + " " + dgvMemberDetails.Columns["memberDateofBirth"].HeaderText;
                    bool status = EditBirthYear(parentCustomerDTO, text);
                    if (status == false)
                    {
                        log.LogMethodExit();
                        return;
                    }
                    dgvMemberDetails.Rows[0].Cells["memberDateofBirth"].Value = parentCustomerDTO.DateOfBirth.Value.ToString(KioskStatic.DateMonthFormat);
                    dgvMemberDetails.Refresh();
                    if (!IsAgeCriteriaMatches(parentCustomerDTO.DateOfBirth.ToString()))
                    {
                        (dgvMemberDetails.CurrentRow.Cells["MemberSelectDataGridViewTextBoxColumn"] as DataGridViewImageCell).Value = (System.Drawing.Image)Parafait_Kiosk.Properties.Resources.Check_Box_Empty;
                        int removeIndex = selectedAdultList.IndexOf(selectedAdultList.Where(k => k.ParentCustomerId == -1 && k.RelatedCustomerId == -1).FirstOrDefault());
                        if (removeIndex > -1)
                        {
                            selectedAdultList.RemoveAt(removeIndex);
                        }
                        dgvMemberDetails.Rows[index].DefaultCellStyle.ForeColor = SystemColors.InactiveCaption;
                        dgvMemberDetails.Rows[index].DefaultCellStyle.SelectionForeColor = SystemColors.InactiveCaption;
                    }
                    else
                    {
                        this.dgvMemberDetails.Rows[index].DefaultCellStyle.SelectionBackColor = this.dgvYourFamily.DefaultCellStyle.BackColor;
                        this.dgvMemberDetails.Rows[index].DefaultCellStyle.SelectionForeColor = KioskStatic.CurrentTheme.SelectAdultMemberDetailsGridInfoTextForeColor;// this.dgvYourFamily.DefaultCellStyle.ForeColor;
                    }
                    ControlSkipAndEnableButtons();
                    log.LogMethodExit();
                    return;
                }

                Bitmap x = (System.Drawing.Bitmap)(dgvMemberDetails.CurrentRow.Cells["MemberSelectDataGridViewTextBoxColumn"] as DataGridViewImageCell).Value;
                Bitmap y = (System.Drawing.Bitmap)Parafait_Kiosk.Properties.Resources.Check_Box_Ticked;
                int editIndex = -1;
                if (CompareImages(x, y))
                {
                    (dgvMemberDetails.CurrentRow.Cells["MemberSelectDataGridViewTextBoxColumn"] as DataGridViewImageCell).Value = (System.Drawing.Image)Parafait_Kiosk.Properties.Resources.Check_Box_Empty;
                    editIndex = selectedAdultList.IndexOf(selectedAdultList.Where(k => k.RelatedCustomerId == -1).FirstOrDefault());//it is parent, not related customer, so RelatedCustomerId = -1

                    if (editIndex > -1)
                    {
                        selectedAdultList.RemoveAt(editIndex);
                    }
                }
                else
                {
                    if (selectedAdultList.Count < adultQtySelectedInQtyScreen)
                    {
                        if (IsAgeCriteriaMatches(parentCustomerDTO.DateOfBirth.ToString()))
                        {
                            (dgvMemberDetails.CurrentRow.Cells["MemberSelectDataGridViewTextBoxColumn"] as DataGridViewImageCell).Value = (System.Drawing.Image)Parafait_Kiosk.Properties.Resources.Check_Box_Ticked;
                            CustomCustomerDTO customCustomerDTO = new CustomCustomerDTO(parentCustomerDTO);
                            selectedAdultList.Add(customCustomerDTO);
                            txtMessage.Text = "";
                        }
                        else
                        {
                            string errMsg = MessageContainerList.GetMessage(utilities.ExecutionContext, 4288);
                            frmOKMsg.ShowUserMessage(errMsg);
                            txtMessage.Text = errMsg;
                            return;
                        }
                    }
                    else
                    {
                        (dgvMemberDetails.CurrentRow.Cells["MemberSelectDataGridViewTextBoxColumn"] as DataGridViewImageCell).Value = (System.Drawing.Image)Parafait_Kiosk.Properties.Resources.Check_Box_Empty;
                        string warningMsg = MessageContainerList.GetMessage(utilities.ExecutionContext, 4286, selectedAdultList.Count, adultQtySelectedInQtyScreen);
                        txtMessage.Text = warningMsg;
                        frmOKMsg.ShowUserMessage(warningMsg);
                    }
                }
                UpdateCheckInGuestsCount();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected error while processing click event of dgvMemberDetails: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void dgvYourFamily_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();

            if (e.RowIndex < 0)
                return;

            try
            {
                int index = dgvYourFamily.CurrentRow.Index;

                if (dgvYourFamily.Columns[e.ColumnIndex].Name == "editRelativeBirthYear")
                {
                    string text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Edit")
                        + " " + MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Family")
                        + " " + dgvYourFamily.Columns["dOBDataGridViewTextBoxColumn"].HeaderText;
                    bool status = EditBirthYear(customCustomerDTOList[index].CustomerRelationshipDTO.RelatedCustomerDTO, text);
                    if (status == false)
                    {
                        log.LogMethodExit();
                        return;
                    }
                    dgvYourFamily.EndEdit();
                    dgvYourFamily.Refresh();
                    if (!IsAgeCriteriaMatches(customCustomerDTOList[index].CustomerRelationshipDTO.RelatedCustomerDTO.DateOfBirth.ToString()))
                    {
                        (dgvYourFamily.CurrentRow.Cells["checkboxDataGridViewImageColumn"] as DataGridViewImageCell).Value = (System.Drawing.Image)Parafait_Kiosk.Properties.Resources.Check_Box_Empty;
                        int removeIndex = selectedAdultList.IndexOf(selectedAdultList.Where(k => k.RelatedCustomerId == customCustomerDTOList[index].RelatedCustomerId).FirstOrDefault());
                        if (removeIndex > -1)
                        {
                            selectedAdultList.RemoveAt(removeIndex);
                        }
                        dgvYourFamily.Rows[index].DefaultCellStyle.ForeColor = SystemColors.InactiveCaption;
                        dgvYourFamily.Rows[index].DefaultCellStyle.SelectionForeColor = SystemColors.InactiveCaption;
                    }
                    else
                    {
                        this.dgvYourFamily.Rows[index].DefaultCellStyle.SelectionBackColor = this.dgvYourFamily.DefaultCellStyle.BackColor;
                        this.dgvYourFamily.Rows[index].DefaultCellStyle.SelectionForeColor = KioskStatic.CurrentTheme.SelectAdultYourFamilyGridInfoTextForeColor;// this.dgvYourFamily.DefaultCellStyle.ForeColor;
                    }
                    UpdateCheckInGuestsCount();
                    log.LogMethodExit();
                    return;
                }

                Bitmap x = (System.Drawing.Bitmap)(dgvYourFamily.CurrentRow.Cells["checkboxDataGridViewImageColumn"] as DataGridViewImageCell).Value;
                Bitmap y = (System.Drawing.Bitmap)Parafait_Kiosk.Properties.Resources.Check_Box_Ticked;
                int editIndex = -1;
                int age = customCustomerDTOList[index].Age;

                if (CompareImages(x, y))
                {
                    (dgvYourFamily.CurrentRow.Cells["checkboxDataGridViewImageColumn"] as DataGridViewImageCell).Value = (System.Drawing.Image)Parafait_Kiosk.Properties.Resources.Check_Box_Empty;
                    editIndex = selectedAdultList.IndexOf(selectedAdultList.Where(k => k.RelatedCustomerId == customCustomerDTOList[index].RelatedCustomerId).FirstOrDefault());
                    if (editIndex > -1)
                    {
                        selectedAdultList.RemoveAt(editIndex);
                        txtMessage.Text = "";
                    }
                }
                else
                {
                    if (selectedAdultList.Count < adultQtySelectedInQtyScreen)
                    {
                        if (IsAgeCriteriaMatches(customCustomerDTOList[index].CustomerRelationshipDTO.RelatedCustomerDTO.DateOfBirth.ToString()))
                        {
                            (dgvYourFamily.CurrentRow.Cells["checkboxDataGridViewImageColumn"] as DataGridViewImageCell).Value = (System.Drawing.Image)Parafait_Kiosk.Properties.Resources.Check_Box_Ticked;
                            selectedAdultList.Add(customCustomerDTOList[index]);
                            txtMessage.Text = "";
                        }
                        else
                        {
                            string errMsg = MessageContainerList.GetMessage(utilities.ExecutionContext, 4288);
                            frmOKMsg.ShowUserMessage(errMsg);
                            txtMessage.Text = errMsg;
                            return;
                        }
                    }
                    else
                    {
                        (dgvYourFamily.CurrentRow.Cells["checkboxDataGridViewImageColumn"] as DataGridViewImageCell).Value = (System.Drawing.Image)Parafait_Kiosk.Properties.Resources.Check_Box_Empty;
                        string warningMsg = MessageContainerList.GetMessage(utilities.ExecutionContext, 4286, selectedAdultList.Count, adultQtySelectedInQtyScreen);
                        txtMessage.Text = warningMsg;
                        frmOKMsg.ShowUserMessage(warningMsg);
                    }
                }
                UpdateCheckInGuestsCount();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected error while processing dgvSelectAdult click event: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void dgvMemberDetails_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (dgvMemberDetails.Columns[e.ColumnIndex].Name == "CardNumberdataGridViewTextBoxColumn")
                {
                    if (e.Value != null)
                    {
                        e.Value = KioskHelper.GetMaskedCardNumber(e.Value.ToString());
                        e.FormattingApplied = true;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error while dgvMemberDetails_CellFormatting in adult select screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        //private void dgvSelectAdult_verticalScrollBar_Click(object sender, EventArgs e)
        //{
        //    log.LogMethodEntry();
        //    ResetKioskTimer();
        //    log.LogMethodExit();
        //}

        private bool IsAgeCriteriaMatches(string doB)
        {
            log.LogMethodEntry(doB);
            ResetKioskTimer();
            bool status = false;
            try
            {
                decimal age = KioskHelper.GetAge(doB);
                if (age >= productsContainerDTO.AgeLowerLimit && age <= productsContainerDTO.AgeUpperLimit)
                {
                    status = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                displayMessageLine(ex.Message, ERROR);
                KioskStatic.logToFile("Error in IsAdultRelationsExist() while getting linked relations: " + ex.Message);
            }
            log.LogMethodExit(status);
            return status;
        }

        private List<CustomerRelationshipDTO> GetLinkedRelationsMatchingAgeCriteria()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            List<CustomerRelationshipDTO> relationDTOList = null;
            try
            {
                customCustomerDTOList.Clear(); ;
                relationDTOList = KioskHelper.GetLinkedRelationsFilteredByAge(purchaseProductDTO.ParentCustomerId, productsContainerDTO.AgeLowerLimit, productsContainerDTO.AgeUpperLimit);
                if (relationDTOList != null && relationDTOList.Any())
                {
                    foreach (CustomerRelationshipDTO customerRelationshipDTO in relationDTOList)
                    {
                        CustomCustomerDTO customCustomerDTO = new CustomCustomerDTO(customerRelationshipDTO);
                        customCustomerDTOList.Add(customCustomerDTO);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                displayMessageLine(ex.Message, ERROR);
                KioskStatic.logToFile("Error in GetLinkedRelationsMatchingAgeCriteria(): " + ex.Message);
            }
            log.LogMethodExit(relationDTOList);
            return relationDTOList;
        }

        private void ControlSkipAndEnableButtons()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                btnProceed.Enabled = selectedAdultList.Any() ? true : false;
                btnSkip.Enabled = !btnProceed.Enabled;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected error in ControlSkipAndEnableeButtons of select adult screen:: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void AttachCheckInDetailDTOToTrxLines()
        {
            log.LogMethodEntry();
            ResetKioskTimer();

            try
            {
                for (int i = 0; i < selectedAdultList.Count; i++)
                {
                    CustomCheckInDetailDTO adultCheckInDetailDTO = new CustomCheckInDetailDTO();
                    adultCheckInDetailDTO.Name = selectedAdultList[i].RelatedCustomerName;
                    adultCheckInDetailDTO.DateOfBirth = selectedAdultList[i].DOB;
                    adultCheckInDetailDTO.Age = selectedAdultList[i].Age;
                    adultCheckInDetailDTO.CustomerId = selectedAdultList[i].RelatedCustomerId;
                    trxLines[i].LineCheckInDetailDTO = adultCheckInDetailDTO.CheckInDetailDTO;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected error in AttachCheckInDetailDTOToTrxLines() of select adult screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void EnableCheckboxForSelectedRelations(List<CustomCustomerDTO> newlyAddedChildren)
        {
            log.LogMethodEntry(newlyAddedChildren);
            StopKioskTimer();
            try
            {
                foreach (CustomCustomerDTO newlyAddedChild in newlyAddedChildren)
                {
                    int index = -1;
                    index = customCustomerDTOList.IndexOf(customCustomerDTOList.Where(k => k.RelatedCustomerId == newlyAddedChild.RelatedCustomerId).FirstOrDefault());
                    if (index > -1)
                    {
                        (dgvYourFamily.Rows[index].Cells["checkboxDataGridViewImageColumn"] as DataGridViewImageCell).Value = (System.Drawing.Image)Parafait_Kiosk.Properties.Resources.Check_Box_Ticked;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                displayMessageLine(ex.Message, ERROR);
                KioskStatic.logToFile("Error while EnableCheckboxForNewlyLinkedChild() in select adult screen: " + ex.Message);
            }
            finally
            {
                StartKioskTimer();
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

        private void DisableCheckboxForSelectedChild()
        {
            log.LogMethodEntry();
            StopKioskTimer();
            try
            {
                for (int i = 0; i < customCustomerDTOList.Count; i++)
                {
                    (dgvYourFamily.Rows[i].Cells["checkboxDataGridViewImageColumn"] as DataGridViewImageCell).Value = (System.Drawing.Image)Parafait_Kiosk.Properties.Resources.Check_Box_Empty;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                displayMessageLine(ex.Message, ERROR);
                KioskStatic.logToFile("Error while EnableCheckboxForNewlyLinkedChild() in select child screen: " + ex.Message);
            }
            finally
            {
                StartKioskTimer();
            }
            log.LogMethodExit();
        }

        private bool EditBirthYear(CustomerDTO customerDTO, string controlText)
        {
            log.LogMethodEntry(customerDTO);
            ResetKioskTimer();
            bool status = false;

            try
            {
                bool allowEditBirthDayAndMonth = ParafaitDefaultContainerList.GetParafaitDefault<bool>(KioskStatic.Utilities.ExecutionContext, "ALLOW_EDIT_BIRTH_DAY_AND_MONTH", true);
                if (allowEditBirthDayAndMonth == false)
                {
                    frmOKMsg.ShowUserMessage(MessageContainerList.GetMessage(utilities.ExecutionContext, 5159)); //'Please note, you are allowed to edit year only'
                }
                DateTime doB = customerDTO.DateOfBirth == null ? DateTime.MinValue : Convert.ToDateTime(customerDTO.DateOfBirth);
                DateTime newDOB = KioskHelper.LaunchCalendar(defaultDateTimeToShow: doB, enableDaySelection: (allowEditBirthDayAndMonth && KioskStatic.enableDaySelection),
                    enableMonthSelection: (allowEditBirthDayAndMonth && KioskStatic.enableMonthSelection), enableYearSelection: KioskStatic.enableYearSelection, disableTill: DateTime.MinValue, showTimePicker: false, popupAlerts: frmOKMsg.ShowUserMessage);
                if (newDOB != null && newDOB != DateTime.MinValue)
                {
                    customerDTO.DateOfBirth = newDOB;
                }
                //Save Customer
                SqlConnection sqlConnection = KioskStatic.Utilities.createConnection();
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                try
                {
                    CustomerBL customerBL = new CustomerBL(KioskStatic.Utilities.ExecutionContext, customerDTO);
                    customerBL.Save(sqlTransaction);
                    sqlTransaction.Commit();
                    status = true;
                }
                catch (Exception ex)
                {
                    sqlTransaction.Rollback();
                    log.Error(ex);
                    //Unable to change the date of birth. Please try from Register menu of Home Screen
                    string errMsg = MessageContainerList.GetMessage(utilities.ExecutionContext, 4800);
                    frmOKMsg.ShowUserMessage(errMsg);
                    KioskStatic.logToFile("Error in EditDateOfBirth() of Select child: " + errMsg + ex.Message);
                    customerDTO.DateOfBirth = doB;
                }
                finally
                {
                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                displayMessageLine(ex.Message, ERROR);
                frmOKMsg.ShowUserMessage(ex.Message);
                KioskStatic.logToFile("Error in EditDateOfBirth() of Select child: " + ex.Message);
            }
            finally
            {
                StartKioskTimer();
            }
            log.LogMethodExit();
            return status;
        }

        private void UpdateCheckInGuestsCount()
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
                lblGuestCount.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 4776, selectedAdultList.Count, adultQtySelectedInQtyScreen); //"Quantity:&1"
                btnProceed.Enabled = selectedAdultList.Any() ? true : false;
                btnSkip.Enabled = !btnProceed.Enabled;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error while UpdateCheckInGuestsCount() in child screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private static bool CompareImages(Bitmap image1, Bitmap image2)
        {
            log.LogMethodEntry(image1, image2);

            try
            {
                if (image1.Width == image2.Width && image1.Height == image2.Height)
                {
                    for (int i = 0; i < image1.Width; i++)
                    {
                        for (int j = 0; j < image1.Height; j++)
                        {
                            if (image1.GetPixel(i, j) != image2.GetPixel(i, j))
                            {
                                log.LogMethodExit();
                                return false;
                            }
                        }
                    }
                    log.LogMethodExit();
                    return true;
                }
                else
                {
                    log.LogMethodExit();
                    return false;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected error in CompareImages() of select adult screen:: " + ex);
                log.LogMethodExit();
                return false;
            }
        }

        void displayMessageLine(string message, string msgType)
        {
            log.LogMethodEntry(message, msgType);
            ResetKioskTimer();
            txtMessage.Text = message;
            log.LogMethodExit();
        }

        public override void KioskTimer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            int tickSecondsRemaining = GetKioskTimerSecondsValue();
            tickSecondsRemaining--;
            setKioskTimerSecondsValue(tickSecondsRemaining);
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

        private void SetFont()
        {
            log.LogMethodEntry();
            ResetKioskTimer();

            try
            {
                dgvMemberDetails.ColumnHeadersDefaultCellStyle.Font =
                    dgvYourFamily.ColumnHeadersDefaultCellStyle.Font = new Font(lblGreeting.Font.FontFamily, 21F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                dgvMemberDetails.DefaultCellStyle.Font =
                    dgvYourFamily.DefaultCellStyle.Font =
                    dgvMemberDetails.RowHeadersDefaultCellStyle.Font =
                    dgvYourFamily.RowHeadersDefaultCellStyle.Font =
                    dgvMemberDetails.RowsDefaultCellStyle.Font =
                    dgvYourFamily.RowsDefaultCellStyle.Font =
                    dgvMemberDetails.RowTemplate.DefaultCellStyle.Font =
                    dgvYourFamily.RowTemplate.DefaultCellStyle.Font =
                    new System.Drawing.Font(lblGreeting.Font.FontFamily, 21F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

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

                lblMemberDetails.Font = new Font(lblGreeting.Font.FontFamily, 28F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                lblYourFamily.Font = new Font(lblGreeting.Font.FontFamily, 28F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                //this.dgvMemberDetails.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.Highlight;
                //this.dgvYourFamily.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.Highlight;
                //this.dgvMemberDetails.ColumnHeadersDefaultCellStyle.ForeColor = SystemColors.HighlightText;
                //this.dgvYourFamily.ColumnHeadersDefaultCellStyle.ForeColor = SystemColors.HighlightText;
                this.dgvYourFamily.DefaultCellStyle.SelectionBackColor = this.dgvYourFamily.DefaultCellStyle.BackColor;
                this.dgvYourFamily.DefaultCellStyle.SelectionForeColor = this.dgvYourFamily.DefaultCellStyle.ForeColor;
                this.dgvMemberDetails.DefaultCellStyle.SelectionBackColor = this.dgvMemberDetails.DefaultCellStyle.BackColor;
                this.dgvMemberDetails.DefaultCellStyle.SelectionForeColor = this.dgvMemberDetails.DefaultCellStyle.ForeColor;
                this.bigVerticalScrollViewFamily.InitializeScrollBar(ThemeManager.CurrentThemeImages.ScrollDownEnabled, ThemeManager.CurrentThemeImages.ScrollDownDisabled, ThemeManager.CurrentThemeImages.ScrollUpEnabled, ThemeManager.CurrentThemeImages.ScrollUpDisabled);
                //this.verticalScrollBarViewMemberDetails.InitializeScrollBar(KioskStatic.CurrentTheme.ScrollDownEnabled, KioskStatic.CurrentTheme.ScrollDownDisabled, KioskStatic.CurrentTheme.ScrollUpEnabled, KioskStatic.CurrentTheme.ScrollUpDisabled);

                memberDateofBirth.DefaultCellStyle = KioskHelper.gridViewDateOfBirthCellStyle();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected error in SetFont() of select adult screen: " + ex.Message);
            }

            log.LogMethodExit();
        }

        private void SetCustomImages()
        {
            log.LogMethodEntry();
            ResetKioskTimer();

            try
            {
                this.BackgroundImage = ThemeManager.GetBackgroundImageTwo(ThemeManager.CurrentThemeImages.SelectAdultBackgroundImage);
                this.btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
                this.panelMemberDetails.BackgroundImage =
                    this.panelYourFamily.BackgroundImage =
                    this.panelNoRelation.BackgroundImage = ThemeManager.CurrentThemeImages.KioskActivityTableImage;
                this.btnPrev.BackgroundImage = btnSkip.BackgroundImage = btnProceed.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
            }
            catch (Exception ex)
            {
                string msg = "Error while Setting Customized background images for frmSelectAdult: ";
                log.Error(msg, ex);
                KioskStatic.logToFile(msg + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            ResetKioskTimer();

            KioskStatic.logToFile("Setting customized font colors for the UI elements of frmSelectAdult");
            try
            {
                this.lblGreeting.ForeColor = KioskStatic.CurrentTheme.SelectAdultGreetingLblTxtForeColor;
                this.lblYourFamily.ForeColor = KioskStatic.CurrentTheme.SelectAdultYourfamilyLblTextForeColor;

                this.dgvYourFamily.ForeColor = KioskStatic.CurrentTheme.SelectAdultYourFamilyGridInfoTextForeColor;
                this.dgvMemberDetails.ForeColor = KioskStatic.CurrentTheme.SelectAdultMemberDetailsGridInfoTextForeColor;

                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.SelectAdultFooterTxtMsgTextForeColor;//Footer text message
                this.lblSiteName.ForeColor = KioskStatic.CurrentTheme.SiteHeadingForeColor;
                this.btnProceed.ForeColor = KioskStatic.CurrentTheme.SelectAdultProceedButtonTextForeColor;
                this.btnSkip.ForeColor = KioskStatic.CurrentTheme.SelectAdultSkipButtonTextForeColor;
                this.btnPrev.ForeColor = KioskStatic.CurrentTheme.SelectAdultBackButtonTextForeColor;
                this.btnHome.ForeColor = KioskStatic.CurrentTheme.SelectAdultHomeButtonTextForeColor;//Footer text message
                this.lblMemberDetails.ForeColor = KioskStatic.CurrentTheme.SelectAdultMemberDetailsLblTextForeColor;
                this.lblGridFooterMsg.ForeColor = KioskStatic.CurrentTheme.SelectAdultGridFooterMsgLblTextForeColor;
                this.lblNoRelationMsg1.ForeColor = KioskStatic.CurrentTheme.SelectAdultNoRelationMsg1LblTextForeColor;
                this.lblNoRelationMsg2.ForeColor = KioskStatic.CurrentTheme.SelectAdultNoRelationMsg2LblTextForeColor;
                this.lblNoRelationMsg3.ForeColor = KioskStatic.CurrentTheme.SelectAdultNoRelationMsg3LblTextForeColor;

                this.lblGuestCountHeader.ForeColor =
                    this.lblPackageHeader.ForeColor =
                    this.lblFacilityHeader.ForeColor =
                    this.lblAgeCriteriaHeader.ForeColor =
                     this.lblGuestCountHeader.ForeColor = KioskStatic.CurrentTheme.PackageDetailsLblHeaderTextForeColor;

                this.lblGuestCount.ForeColor =
                this.lblPackage.ForeColor =
                this.lblFacility.ForeColor =
                this.lblAgeCriteria.ForeColor = KioskStatic.CurrentTheme.PackageDetailsLblTextForeColor;

                this.lblGuestCount.ForeColor = KioskStatic.CurrentTheme.GuestCountLblTextForeColor;
                this.lblScreenDetails.ForeColor = KioskStatic.CurrentTheme.ScreenDetailsLblTextForeColor;

                this.dgvYourFamily.ColumnHeadersDefaultCellStyle.ForeColor = KioskStatic.CurrentTheme.SelectAdultGridHeaderTextForeColor;
                this.dgvMemberDetails.ColumnHeadersDefaultCellStyle.ForeColor = KioskStatic.CurrentTheme.SelectAdultGridHeaderTextForeColor;

                this.dgvYourFamily.RowsDefaultCellStyle.ForeColor = KioskStatic.CurrentTheme.SelectAdultYourFamilyGridInfoTextForeColor;
                this.dgvYourFamily.RowsDefaultCellStyle.SelectionForeColor = KioskStatic.CurrentTheme.SelectAdultYourFamilyGridInfoTextForeColor;
                
                this.dgvMemberDetails.RowsDefaultCellStyle.ForeColor = KioskStatic.CurrentTheme.SelectAdultMemberDetailsGridInfoTextForeColor;
                this.dgvMemberDetails.RowsDefaultCellStyle.SelectionForeColor = KioskStatic.CurrentTheme.SelectAdultMemberDetailsGridInfoTextForeColor;

                this.dgvYourFamily.DefaultCellStyle.SelectionForeColor = KioskStatic.CurrentTheme.SelectAdultYourFamilyGridInfoTextForeColor;
                this.dgvMemberDetails.DefaultCellStyle.SelectionForeColor = KioskStatic.CurrentTheme.SelectAdultMemberDetailsGridInfoTextForeColor;
            }
            catch (Exception ex)
            {
                string msg = "Error while setting customized font colors for the UI elements of frmSelectAdult: ";
                log.Error(msg, ex);
                KioskStatic.logToFile(msg + ex.Message);
            }
            log.LogMethodExit();
        }

        public class RelationsNotExistException : Exception
        {
            public RelationsNotExistException()
            {
            }

            public RelationsNotExistException(string message) : base(message)
            {
            }

            public RelationsNotExistException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected RelationsNotExistException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }

        private void frmSelectAdult_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                StopKioskTimer();
            }
            catch (Exception ex)
            {
                log.Error("Unexpected error occurred while executing frmSelectAdult_FormClosed()", ex);
            }
            //Cursor.Hide();

            log.Info(this.Name + ": Form closed");
            log.LogMethodExit();
        }
    }
}








