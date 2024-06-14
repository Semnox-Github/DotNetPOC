/******************************************************************************************************
 * Project Name - POS
 * Description  - POS Booking
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By    Remarks          
 ****************************************************************************************************** 
 *2.6.0       11-Apr-2019      Archana        Include/Exclude for redeemable products changes
 *2.70        26-Mar-2019      Guru S A       Booking phase 2 enhancement changes 
 *2.150.0     22-Apr-2021      Abhishek       Modified : POS UI Redesign
********************************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading;
using System.Drawing.Printing;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using Semnox.Parafait.Discounts;
using Semnox.Parafait.Device.Lockers;
using Semnox.Parafait.Customer;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Product;

namespace Parafait_POS
{
    public partial class POS
    {
        Dictionary<string, Image> btnImageDictionary = new Dictionary<string, Image>();
        
        private void initializeUIComponents()
        {
            log.Debug("Starts-initializeUIComponents()");//Modified for Adding logger feature on 29-Feb-2016
            if (ParafaitEnv.LanguageCode != null && ParafaitEnv.LanguageCode.StartsWith("en", StringComparison.CurrentCultureIgnoreCase) == false)
            {
                tabControlCardAction.Font = new Font(tabControlCardAction.Font, FontStyle.Regular);
                tabControlSelection.Font = new Font(tabControlSelection.Font, FontStyle.Regular);
            }

            if (Utilities.getParafaitDefaults("ENABLE_PRODUCTS_IN_POS") == "N")
            {
                tabControlSelection.TabPages.RemoveByKey("tabPageProducts");
                tabControlSelection.TabPages.RemoveByKey("tabPageDiscounts");
                tabControlCardAction.TabPages.RemoveByKey("tabPageTrx");
                tabControlCardAction.TabPages.RemoveByKey("tabPageMyTrx");
                tabControlCardAction.TabPages.RemoveByKey("tpBookings");
            }
            else
            {
                initializeDisplayGroupDropDown();
                initializeProductTab();
                if (Utilities.getParafaitDefaults("ENABLE_DISCOUNTS_IN_POS") == "N")
                {
                    tabControlSelection.TabPages.RemoveByKey("tabPageDiscounts");
                }
                else
                {
                    InitializeDiscountTab();
                }

                if (Utilities.getParafaitDefaults("ENABLE_MY_TRANSACTIONS_IN_POS") == "N")
                {
                    tabControlCardAction.TabPages.RemoveByKey("tabPageMyTrx");
                }
                else
                {
                    SetupMyTransactionsTab();
                }

                if (Utilities.getParafaitDefaults("ENABLE_BOOKINGS_IN_POS") == "N")
                {
                    tabControlCardAction.TabPages.Remove(tpBookings);
                }
            }

            if (Utilities.getParafaitDefaults("ENABLE_TASKS_IN_POS") == "N")
                tabControlSelection.TabPages.RemoveByKey("tabPageFunctions");
            else
            {
                initializeTasksTab();
                tabControlSelection.TabPages["tabPageFunctions"].Text = "Card Tasks";
            }

            if (Utilities.getParafaitDefaults("ENABLE_MANUAL_PRODUCT_SEARCH_IN_POS") == "Y")
            {
                panelProductSearch.Visible = true;
            }
            else
            {
                panelProductSearch.Visible = false;
                tabControlProducts.Height += panelProductSearch.Height;
            }

            //Modified 02/2019 for BearCat - Show Recipe
            bool ENABLE_REFUND_IN_POS = Utilities.getParafaitDefaults("ENABLE_REFUND_IN_POS").Equals("Y");
            bool ENABLE_PRODUCT_DETAILS_IN_POS = Utilities.getParafaitDefaults("ENABLE_PRODUCT_DETAILS_IN_POS").Equals("Y");            
            if (!ENABLE_REFUND_IN_POS && !ENABLE_PRODUCT_DETAILS_IN_POS)
            {
                panelProductButtons.Visible = false;
                panelProductSearch.Location = new Point(panelProductSearch.Location.X, panelProductSearch.Location.Y + btnRefundCard.Height);
                tabControlProducts.Height += btnRefundCard.Height;
            }
            else if (ENABLE_REFUND_IN_POS && ENABLE_PRODUCT_DETAILS_IN_POS)
            {
                panelProductButtons.Visible = true;
                btnRefundCard.Visible = true;
                btnProductDetails.Visible = true;                
                panelProductButtons.Width = tabControlProducts.Width;
            }
            else if (ENABLE_PRODUCT_DETAILS_IN_POS)
            {
                panelProductButtons.Visible = true;
                btnRefundCard.Visible = false;
                btnProductDetails.Visible = true;
                panelProductButtons.Width = tabControlProducts.Width;
            }
            else if (ENABLE_REFUND_IN_POS)
            {
                panelProductButtons.Visible = true;
                btnRefundCard.Visible = true;
                btnProductDetails.Visible = false;
                panelProductButtons.Width = tabControlProducts.Width;
            }

            buttonSaveTransaction.Enabled = (Utilities.getParafaitDefaults("DISABLE_TRX_COMPLETE") != "Y");
            btnPlaceOrder.Enabled = (Utilities.getParafaitDefaults("DISABLE_ORDER_SUSPEND") != "Y");

            enablePaymentLinkButton = TransactionPaymentLink.ISPaymentLinkEnbled(Utilities.ExecutionContext);
            btnSendPaymentLink.Enabled = enablePaymentLinkButton;

            if (!panelProductSearch.Visible && !panelProductButtons.Visible)
            {
                tabControlProducts.Height = tabPageProducts.Height;
            }

            if (Utilities.getParafaitDefaults("ENABLE_REDEMPTION_IN_POS") == "N")
            {
                tabControlSelection.TabPages.RemoveByKey("tabPageRedeem");
            }
            else
            {
                ToolTip toolTip = new ToolTip();

                toolTip.AutoPopDelay = 5000;
                toolTip.InitialDelay = 1000;
                toolTip.ReshowDelay = 500;
                toolTip.ShowAlways = true;

                toolTip.SetToolTip(pbRedeem, MessageUtils.getMessage("Redeem Tickets for Gifts"));
            }

            panelCardSwipe.Location = new Point(panelAmounts.Location.X, panelCardSwipe.Location.Y);

            Utilities.setupDataGridProperties(ref dataGridViewCardDetails);

            Utilities.setupDataGridProperties(ref dataGridViewPurchases);
            CommonFuncs.setupDataGridProperties(dataGridViewPurchases);
            // May 20 2016 begin
            if (tabControlCardAction.TabPages.Contains(tabPageActivities))
            {
                dataGridViewPurchases.Columns["dcBtnCardActivityTrxPrint"].DefaultCellStyle.NullValue = null;
            }
            // May 20 2016 end

            Utilities.setupDataGridProperties(ref dataGridViewGamePlay);
            CommonFuncs.setupDataGridProperties(dataGridViewGamePlay);

            Utilities.setupDataGridProperties(ref dgvCardGames);
            CommonFuncs.setupDataGridProperties(dgvCardGames);

            Utilities.setupDataGridProperties(ref dgvCardDiscounts);
            CommonFuncs.setupDataGridProperties(dgvCardDiscounts);

            Utilities.setupDataGridProperties(ref dgvCreditPlus);
            CommonFuncs.setupDataGridProperties(dgvCreditPlus);

            createCardDetailsGrid();

            dataGridViewTransaction.GridColor = Color.Gray;
            Quantity.DefaultCellStyle.Format = "N" + Utilities.ParafaitEnv.POS_QUANTITY_DECIMALS.ToString();
            nudQuantity.DecimalPlaces = Utilities.ParafaitEnv.POS_QUANTITY_DECIMALS;

            labelCardStatus.Text = "";
            labelCardNo.Text = "";

            initializePaymodeDropDown();

            setBackgroundColor();
            buttonSaveTransaction.Text = MessageUtils.getMessage("New");

            //Fetch Default Pay mode from Site Set up than POS Configuration file
            //try
            //{
            //    cmbPaymentMode.SelectedIndex = Properties.Settings.Default.DefaultPayMode;
            //}
            //catch
            //{
            try
            {
                cmbPaymentMode.SelectedIndex = Utilities.ParafaitEnv.DEFAULT_PAY_MODE;
            }
            catch
            {
                cmbPaymentMode.SelectedIndex = 0;
            }
            //}

            //02-Jan-2019:: Payment Mode cannot be changed in POS going forward. It will show value as defined in Management Studio configuration
            //if (ParafaitEnv.Manager_Flag == "Y")
            //    cmbPaymentMode.Enabled = true;
            //else
            cmbPaymentMode.Enabled = false;

            //StaticDataExchange.ClearPaymentData();

            rdAmount.Checked = true;

            if (Utilities.getParafaitDefaults("ENABLE_CARD_DETAILS_IN_POS") == "N") // if card details not required, print customer logo in that region
            {
                if (tabControlCardAction.TabPages.ContainsKey("tabPageCardInfo"))
                {
                    tabControlCardAction.TabPages.RemoveByKey("tabPageCardInfo");
                }
                if (tabControlCardAction.TabPages.ContainsKey("tabPageActivities"))
                {
                    tabControlCardAction.TabPages.RemoveByKey("tabPageActivities");
                }
                if (tabControlCardAction.TabPages.ContainsKey("tabPageCardCustomer"))
                {
                    tabControlCardAction.TabPages.RemoveByKey("tabPageCardCustomer");
                }
                try
                {
                    foreach (Control c in panelCardSwipe.Controls)
                        c.Visible = false;
                    PictureBox pbCompanyLogo = new PictureBox();
                    if (ParafaitEnv.CompanyLogo == null)
                    {
                        pbCompanyLogo.Image = Properties.Resources.paraLogo;
                    }
                    else
                    {
                        pbCompanyLogo.Image = ParafaitEnv.CompanyLogo;
                    }
                    pbCompanyLogo.BackColor = Color.Transparent;
                    pbCompanyLogo.Height = panelCardSwipe.Height - 15;
                    pbCompanyLogo.Width = panelCardSwipe.Width - 2;
                    pbCompanyLogo.SizeMode = PictureBoxSizeMode.Zoom;
                    pbCompanyLogo.Location = new Point(1, 5 + panelCardSwipe.Height / 2 - pbCompanyLogo.Height / 2);

                    panelCardSwipe.Controls.Add(pbCompanyLogo);
                }
                catch (Exception ex)
                {
                    POSUtils.ParafaitMessageBox(ex.Message, "Logo Display Error");
                }
            }

            initializeCustomerInfo();

            //Modified 02/2019 for BearCat - Show Recipe
            InitializeProductInquiryPanel();

            POSUtils.initializePaymentModeDT();

            loadLaunchApps();

            dgvTrxSummary.BorderStyle = BorderStyle.None;
            dgvTrxSummary.Columns["ProductQuantity"].DefaultCellStyle = Utilities.gridViewNumericCellStyle();
            dgvTrxSummary.Columns["Amount"].DefaultCellStyle = Utilities.gridViewAmountCellStyle();
            dgvTrxSummary.Columns["Amount"].HeaderCell.Style.Alignment =
                dgvTrxSummary.Columns["ProductQuantity"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvTrxSummary.Columns["ProductQuantity"].SortMode =
                dgvTrxSummary.Columns["Amount"].SortMode = DataGridViewColumnSortMode.NotSortable;

            lblAlerts.Text = "";
            if (lblAlerts.Height <= 30)
                lblAlerts.Font = new Font(lblAlerts.Font.FontFamily, 8, (lblAlerts.Font.Bold ? FontStyle.Bold : FontStyle.Regular)); // reduce font to fit in one line
            else if (lblAlerts.Height < 40)
                lblAlerts.Font = new Font(lblAlerts.Font.FontFamily, 10, (lblAlerts.Font.Bold ? FontStyle.Bold : FontStyle.Regular)); // reduce font to fit in one line

            if (POSStatic.AUTO_DEBITCARD_PAYMENT_POS)
            {
                btnPayment.Enabled = false;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                tabControlSelection.TabPages.Remove(tabPageSystem);
                btnTasks.Visible = btnLaunchApps.Visible = btnShowNumPad.Visible = false;
                //btnLookupCustomer.Enabled =
                btnPlaceOrder.Enabled =
                btnPrint.Enabled = false;
            }

            initializeTrxProfiles();
            initializeTableLayout();

            if (POSStatic.ParafaitEnv.MIFARE_CARD)
                dataGridViewGamePlay.Columns[0].Visible = false;

            if (POSStatic.ALLOW_MANUAL_CARD_IN_POS)
            {
                this.labelCardNumber.Font = new Font(labelCardNumber.Font, FontStyle.Underline);
                this.labelCardNumber.Click += new System.EventHandler(this.labelCardNumber_Click);
            }
            TagNumberLengthList tagNumberLengthList = new TagNumberLengthList(POSStatic.Utilities.ExecutionContext);
            if (this.tagNumberLengthList.MaximumValidTagNumberLength > 10)
            {
                labelCardNo.Font = new Font(labelCardNo.Font.FontFamily, labelCardNo.Font.Size - 2, labelCardNo.Font.Style);
            }
            if (tabControlCardAction.TabPages.Contains(tpBookings))
                getAttractionFacilities();

            //if (POSStatic.LOCKER_LOCK_MAKE.Equals(ParafaitLockCardHandlerDTO.LockerMake.NONE.ToString()))
                //lockerUtilityToolStripMenuItem.Enabled = false;

            log.Debug("Ends-initializeUIComponents()");//Modified for Adding logger feature on 29-Feb-2016
        }

        void initializeTrxProfiles()
        {
            log.Debug("Starts-initializeTrxProfiles()");//Added for logger function on 29-Feb-2016
            DataTable dt = Utilities.executeDataTable(@"select * from trxProfiles where active = 'Y' order by ProfileName");
            if (dt.Rows.Count == 0)
            {
                flpTrxProfiles.Visible = false;
                dgvTrxSummary.Width = dataGridViewTransaction.Width;
                log.Info("Ends-initializeTrxProfiles() as dt.Rows.Count == 0");//Added for logger function on 29-Feb-2016
                return;
            }

            foreach (DataRow dr in dt.Rows)
            {
                Button btnTrxProfile = new Button(); // place holder button
                btnTrxProfile.Tag = dr["TrxProfileId"];
                btnTrxProfile.Text = dr["ProfileName"].ToString();
                btnTrxProfile.Visible = false;
                flpTrxProfiles.Controls.Add(btnTrxProfile);
            }

            btnTrxProfileDefault.Click += new EventHandler(btnTrxProfile_Click);
            btnTrxProfileDefault.Tag = -1;
            log.Debug("Ends-initializeTrxProfiles()");//Added for logger function on 29-Feb-2016
        }

        void initializeTableLayout()
        {
            log.Debug("Starts-initializeTableLayout()");//Added for logger function on 29-Feb-2016
            DataTable dtF = Utilities.executeDataTable(@"select facilityName, facilityId 
                                                            from CheckInFacility c
                                                            where active_flag = 'Y'
                                                            and (FacilityId = @facilityId or @facilityId = -1)
                                                            and (exists (select 1 
                                                                            from FacilityPOSAssignment fpa 
                                                                            where fpa.FacilityId = c.FacilityId
                                                            and fpa.POSMachineId = @POSMachineId))
                                                            and exists (select 1 
                                                                        from FacilityTables f
                                                                        where f.FacilityId = c.FacilityId
                                                                        and f.active = 'Y')
                                                            order by 1",
                                                            new SqlParameter("@facilityId", -1),
                                                            new SqlParameter("@POSMachineId", ParafaitEnv.POSMachineId));
            if (dtF.Rows.Count == 0)
            {
                tcOrderView.TabPages.RemoveByKey("tpOrderTableView");
                log.Info("Ends-initializeTableLayout() as tF.Rows.Count == 0");//Added for logger function on 29-Feb-2016
                return;
            }

            foreach (DataRow dr in dtF.Rows)
            {
                RadioButton rb = new RadioButton();
                rb.Text = dr["facilityName"].ToString();
                rb.Tag = dr["facilityId"];
                rb.Font = new System.Drawing.Font(rb.Font.FontFamily, 8, FontStyle.Regular);
                rb.AutoSize = true;
                flpFacilities.Controls.Add(rb);
                rb.CheckedChanged += new EventHandler(rb_CheckedChanged);
            }

            ((RadioButton)(flpFacilities.Controls[0])).Checked = true;

            if (dtF.Rows.Count == 1)
            {
                flpFacilities.Visible = false;
                panelTables.Height += flpFacilities.Height;
            }
            log.Debug("Ends-initializeTableLayout()");//Added for logger function on 29-Feb-2016
        }

        public bool IsStateExist()
        {
            DataTable dt = Utilities.executeDataTable(@"SELECT StateId FROM state WHERE CountryId = @countryId",
                                                                new SqlParameter("@countryId", Utilities.getParafaitDefaults("STATE_LOOKUP_FOR_COUNTRY")));
            if (dt != null && dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //CustomerDetailUI customerDetailUI = null;
        private CustomerRegistrationUI customerRegistrationUI;
        void initializeCustomerInfo()
        {
            log.LogMethodEntry();
            customerRegistrationUI = new CustomerRegistrationUI(Utilities,displayMessageLine, ()=>{ lastTrxActivityTime = DateTime.Now;});
            //customerDetailUI = new CustomerDetailUI(Utilities, POSUtils.ParafaitMessageBox, ParafaitDefaultContainerList.GetParafaitDefault<bool>(Utilities.ExecutionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD"));
            //customerDetailUI.CustomerContactInfoEntered += customerDetailUI_CustomerContactInfoEntered;
            //customerDetailUI.UniqueIdentifierValidating += txtUniqueId_Validating;
            //panelCustomer.Controls.Add(customerDetailUI);
            //Panel CustomerDetailsView = this.Controls["CustomerDetailsView"] as Panel;
            //if (CustomerDetailsView == null)
            //{
            //    CustomerDetailsView = new Panel();
            //    CustomerDetailsView.BackColor = panelCustomer.BackColor;
            //    CustomerDetailsView.Name = "CustomerDetailsView";
            //    CustomerDetailsView.Size = new System.Drawing.Size(panelCustomer.Width, panelCustomer.Height);
            //    CustomerDetailsView.Left = (this.Width - CustomerDetailsView.Width) / 2;
            //    CustomerDetailsView.Top = tabControlCardAction.Top + tabControlCardAction.ItemSize.Height - 5;
            //    CustomerDetailsView.Font = tabControlCardAction.Font;
            //    CustomerDetailsView.BorderStyle = BorderStyle.FixedSingle;
            //    CustomerDetailsView.Visible = false;
            //    //panelCustomer.Left = (CustomerDetailsView.Width - panelCustomer.Width) / 2;
            //    this.Controls.Add(CustomerDetailsView);
            //    CustomerDetailsView.Controls.Add(panelCustomer);
            //    tabControlCardAction.TabPages.Remove(tabPageCardCustomer);
            //}
            log.LogMethodExit();
        }

        void initializeDisplayGroupDropDown()
        {
            log.Debug("Starts-initializeDisplayGroupDropDown()");//Added for logger function on 29-Feb-2016
            cmbDisplayGroups = new ListBox();
            cmbDisplayGroups.Font = new Font("Arial", 14.0F);
            cmbDisplayGroups.SelectionMode = SelectionMode.One;
            cmbDisplayGroups.SelectedIndexChanged += new EventHandler(cmbDisplayGroups_SelectedIndexChanged);
            cmbDisplayGroups.LostFocus += new EventHandler(cmbDisplayGroups_LostFocus);
            cmbDisplayGroups.HorizontalScrollbar = false;
            cmbDisplayGroups.ScrollAlwaysVisible = false;
            tabPageProducts.Controls.Add(cmbDisplayGroups);
            log.Debug("Ends-initializeDisplayGroupDropDown()");//Added for logger function on 29-Feb-2016
        }

        void cmbDisplayGroups_LostFocus(object sender, EventArgs e)
        {
            log.Debug("Starts-cmbDisplayGroups_LostFocus()");//Added for logger function on 29-Feb-2016
            if (!btnDisplayGroupDropDown.Focused)
                cmbDisplayGroups.Visible = false;

            log.Debug("Ends-cmbDisplayGroups_LostFocus()");//Added for logger function on 29-Feb-2016
        }

        void cmbDisplayGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("Starts-cmbDisplayGroups_SelectedIndexChanged()");//Added for logger function on 29-Feb-2016
            tabControlProducts.SelectedIndex = cmbDisplayGroups.SelectedIndex;
            cmbDisplayGroups.Visible = false;
            log.Debug("Ends-cmbDisplayGroups_SelectedIndexChanged()");//Added for logger function on 29-Feb-2016
        }

        void initializePaymodeDropDown()
        {
            log.Debug("Starts-initializePaymodeDropDown()");//Added for logger function on 29-Feb-2016
            SqlCommand cmd = Utilities.getCommand();
            cmd.CommandText = "select top 1 datavalues from parafait_defaults pd, defaults_datatype dd " +
                              "where default_value_name = 'DEFAULT_PAY_MODE' " +
                              "and active_flag='Y'" +
                              "and pd.datatype_id = dd.datatype_id";
            string values = cmd.ExecuteScalar().ToString();
            string value, display;
            int pos;
            DataTable dtValues = new DataTable();
            dtValues.Columns.Add("Value");
            dtValues.Columns.Add("Display");

            while (1 == 1)
            {
                pos = values.IndexOf('|');
                if (pos < 0)
                    break;
                value = values.Substring(0, pos);
                values = values.Substring(pos + 1);
                pos = values.IndexOf('|');
                if (pos >= 0)
                {
                    display = values.Substring(0, pos);
                    values = values.Substring(pos + 1);
                }
                else
                {
                    display = values;
                }

                dtValues.Rows.Add(new object[] { value, display });

                if (pos < 0)
                    break;
            }

            cmbPaymentMode.DataSource = dtValues;
            cmbPaymentMode.ValueMember = "Value";
            cmbPaymentMode.DisplayMember = "Display";
            cmbPaymentMode.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPaymentMode.SelectedIndex = -1;

            cmbPaymentMode.SelectedIndexChanged += new EventHandler(cmbPaymentMode_SelectedIndexChanged);
            log.Debug("Ends-initializePaymodeDropDown()");//Added for logger function on 29-Feb-2016
        }

        private void setBackgroundColor()
        {
            log.Debug("Starts-setBackgroundColor()");//Added for logger function on 29-Feb-2016

            POSBackColor = ParafaitEnv.POS_SKIN_COLOR_USER;

            this.BackColor = POSBackColor;
            flowLayoutPanelCardProducts.BackColor = POSBackColor;
            flowLayoutPanelDiscounts.BackColor = POSBackColor;
            flowLayoutPanelFunctions.BackColor = POSBackColor;
            panelAmounts.BackColor = POSBackColor;
            panelCardSwipe.BackColor = POSBackColor;
            tabPageProductGroups.BackColor = POSBackColor;
            tabPageDiscounts.BackColor = POSBackColor;
            tabPageRedeem.BackColor = POSBackColor;
            tabPageFunctions.BackColor = POSBackColor;
            tabPageSystem.BackColor = POSBackColor;
            //tabPageCardCustomer.BackColor = POSBackColor;
            tabPageActivities.BackColor = POSBackColor;
            tabPageProducts.BackColor = POSBackColor;
            tabPageTrx.BackColor = POSBackColor;
            tabPageCardInfo.BackColor = POSBackColor;
            tpBookings.BackColor = POSBackColor;

            foreach (TabPage tp in tabControlProducts.TabPages)
                tp.BackColor = POSBackColor;

            foreach (TabPage tp in tcAttractionBookings.TabPages)
                tp.BackColor = POSBackColor;

            dgvAllReservations.BackgroundColor
                = dgvBookings.BackgroundColor
                = dgvAttractionSchedules.BackgroundColor =
            dataGridViewTransaction.BackgroundColor = POSBackColor;
            dataGridViewPurchases.BackgroundColor = POSBackColor;
            dataGridViewGamePlay.BackgroundColor = POSBackColor;
            dataGridViewCardDetails.BackgroundColor = POSBackColor;
            dgvCardGames.BackgroundColor = POSBackColor;
            dgvCardDiscounts.BackgroundColor = POSBackColor;
            dgvCreditPlus.BackgroundColor = POSBackColor;
            textBoxCustomerInfo.BackColor = POSBackColor;
            labelCardStatus.BackColor = labelCardNo.BackColor = POSBackColor;

            splitContainerPOS.Panel1.BackColor = POSBackColor;
            splitContainerPOS.Panel2.BackColor = POSBackColor;

            dgvTrxSummary.BackgroundColor =
                dgvTrxSummary.GridColor =
            dgvTrxSummary.DefaultCellStyle.BackColor = dgvTrxSummary.DefaultCellStyle.SelectionBackColor = Color.DarkGray;
            dgvTrxSummary.DefaultCellStyle.ForeColor = dgvTrxSummary.DefaultCellStyle.SelectionForeColor = Color.Black;

            log.Debug("Ends-setBackgroundColor()");//Added for logger function on 29-Feb-2016
        }

        private void initializeProductTab()
        {
            log.Debug("Starts-initializeProductTab()");//Added for logger function on 29-Feb-2016
            if (tabControlSelection.TabPages["tabPageProducts"] == null)
            {
                log.Info("Ends-initializeProductTab() as tabPageProducts is null");//Added for logger function on 29-Feb-2016
                return;
            }
            SqlCommand Productcmd = Utilities.getCommand();
            DateTime serverTime = Utilities.getServerTime();

            double cardButtonSize = 1;
            try
            {
                cardButtonSize = Convert.ToDouble(Utilities.getParafaitDefaults("CARD_PRODUCT_BUTTON_SIZE")) / 100.0;
            }
            //catch { }            
            catch(Exception ex)
            {
                log.Fatal("Ends-initializeProductTab() cardProductButtonSize due to exception " + ex.Message);//Added for logger function on 29-Feb-2016
            }           

            double noncardButtonSize = 1;
            try
            {
                noncardButtonSize = Convert.ToDouble(Utilities.getParafaitDefaults("NON-CARD_PRODUCT_BUTTON_SIZE")) / 100.0;
            }
            //catch { }
            catch (Exception ex)
            {
                log.Fatal("Ends-initializeProductTab() noncardProductButtonSize due to exception " + ex.Message);//Added for logger function on 29-Feb-2016
            }

            double comboButtonSize = 1;
            try
            {
                comboButtonSize = Convert.ToDouble(Utilities.getParafaitDefaults("COMBO_PRODUCT_BUTTON_SIZE")) / 100.0;
            }
            //catch { }           
            catch (Exception ex)
            {
                log.Fatal("Ends-initializeProductTab() comboProductButtonSize due to exception " + ex.Message);//Added for logger function on 29-Feb-2016
            }

            //Modified 02/2019 for BearCat - 86-68 - updated query to LOJ with productAvailabilityStatus to get status and qty
            Productcmd.CommandText = @"select p.product_id, p.product_name as product_name, 
                                             isnull(case p.description when '' then null else p.description end, p.product_name) as description, 
                                             pt.product_type, 
                                            -- isnull(case display_group when '' then null else display_group end, 'Others') display_group, 
                                             isnull(case pdf.displayGroup when '' then null else pdf.displayGroup end, 'Others') display_group,  
                                             isnull(invP.ImageFileName, p.ImageFileName) ImageFileName, p.ButtonColor, p.TextColor, p.Font,
                                             pdf.ButtonColor DispGroupButtonColor, pdf.TextColor DispGroupTextColor, pdf.Font DispGroupFont
                                       from products p 
                                         left outer join product invP 
                                         on p.product_id = invP.ManualProductId
                                        left outer join ProductsDisplayGroup pdg
                                         on pdg.ProductId = p.product_id
                                         left outer join ProductDisplayGroupFormat pdf
                                           on pdf.Id = pdg.DisplayGroupId,  
                                        -- on pdf.displayGroup = isnull(case display_group when '' then null else display_group end, 'Others'),
                                         product_type pt 
                                     where p.product_type_id = pt.product_type_id and
                                     p.active_flag = 'Y' and
                                     p.DisplayInPOS = 'Y'
                                     and ISNULL(invP.IsSellable,'Y') = 'Y'
                                     and (p.POSTypeId = @Counter or @Counter = -1 or p.POSTypeId is null)
                                     and (p.expiryDate >= getdate() or p.expiryDate is null)
                                     and (p.StartDate <= getdate() or p.StartDate is null)
                                     and not exists (select 1 
                                                     from POSProductExclusions e 
                                                     where e.POSMachineId = @POSMachine
                                                     and e.ProductDisplayGroupFormatId = pdf.Id)
                                     and not exists (select 1 
                                                       from UserRoleDisplayGroupExclusions urdge , 
                                                            users u
                                                      where urdge.ProductDisplayGroupId = pdf.Id
                                                        and urdge.role_id = u.role_id
                                                        and u.loginId = @loginId )
                                     and (not exists (select 1
                                                     from ProductCalendar pc
                                                     where pc.product_id = p.product_id)
                                           or exists (select 1 from 
                                                        (select top 1 date, day, -- select in the order of specific date, day of month, weekday, every day. if there are multiple slots on same day, take the one which is in current hour
                                                                case when @nowHour between isnull(FromTime, @nowHour) and isnull(case ToTime when 0 then 24 else ToTime end, @nowHour) then 0 else 1 end sort, 
                                                                FromTime, ToTime, ShowHide  
                                                        from ProductCalendar pc 
                                                         where pc.product_id = p.product_id 
                                                         and (Date = @today -- specific day
                                                            or Day = @DayNumber -- day number 1001 - 1031
                                                            or Day = @weekDay -- week day 0-6
                                                            or Day = -1) -- everyday
                                                         order by 1 desc, 2 desc, 3) inView 
                                                         where (ShowHide = 'Y' 
                                                                and (@nowHour >= isnull(FromTime, 0) and @nowHour <= case isnull(ToTime, 0) when 0 then 24 else ToTime end))
                                                            or (ShowHide = 'N'
                                                                and (@nowHour < isnull(FromTime, 0) or @nowHour > case isnull(ToTime, 0) when 0 then 24 else ToTime end))))
                                        order by isnull(pdf.sortOrder,(select top 1 SortOrder from ProductDisplayGroupFormat where DisplayGroup = 'Others')), display_group, sort_order,
                                        case product_type 
                                            when 'CARDSALE' then 0
                                            when 'NEW' then 1
                                            when 'RECHARGE' then 2 
                                            when 'VARIABLECARD' then 3 
                                            when 'GAMETIME' then 4
                                            when 'CHECK-IN' then 5
                                            when 'CHECK-OUT' then 6 
                                            else 7 end";
            Productcmd.Parameters.AddWithValue("@POSMachine", Utilities.ParafaitEnv.POSMachineId);
            Productcmd.Parameters.AddWithValue("@Counter", Utilities.ParafaitEnv.POSTypeId);
            Productcmd.Parameters.AddWithValue("@today", serverTime.Date);
            Productcmd.Parameters.AddWithValue("@nowHour", serverTime.Hour + serverTime.Minute / 100.0);
            //Productcmd.Parameters.Add("@nowHour", SqlDbType.Float).Value = (serverTime.Hour + serverTime.Minute / 100.0);
            Productcmd.Parameters.AddWithValue("@DayNumber", serverTime.Day + 1000); // day of month stored as 1000 + day of month
            Productcmd.Parameters.AddWithValue("@loginId", Utilities.ParafaitEnv.LoginID);

            int dayofweek = -1;
            switch (serverTime.DayOfWeek)
            {
                case DayOfWeek.Sunday: dayofweek = 0; break;
                case DayOfWeek.Monday: dayofweek = 1; break;
                case DayOfWeek.Tuesday: dayofweek = 2; break;
                case DayOfWeek.Wednesday: dayofweek = 3; break;
                case DayOfWeek.Thursday: dayofweek = 4; break;
                case DayOfWeek.Friday: dayofweek = 5; break;
                case DayOfWeek.Saturday: dayofweek = 6; break;
                default: break;
            }
            Productcmd.Parameters.AddWithValue("@weekDay", dayofweek);

            DataTable ProductTbl = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(Productcmd);
            da.Fill(ProductTbl);
            bool refreshProductTab = true;
            if (POSStatic.DTPOSProductList.Rows.Count > 0)
            {
                if (AreDataTablesSame(POSStatic.DTPOSProductList, ProductTbl))
                {
                    tabControlProducts = POSStatic.POSProductTab;
                    refreshProductTab = false;
                }
                else
                {
                    POSStatic.DTPOSProductList = new DataTable();
                    POSStatic.DTPOSProductList = ProductTbl.Copy();
                }
            }
            else
            {
                POSStatic.DTPOSProductList = ProductTbl.Copy();
            }
            if (refreshProductTab)
            {
                tabControlProducts.TabPages.Clear();
                cmbDisplayGroups.Items.Clear();
                cmbDisplayGroups.Visible = false;
            }
            string prev_display_group = "@!@##$#";
            TabPage ProductTab;
            FlowLayoutPanel flpProducts = null;

            FlowLayoutPanel flpMainMenu = new FlowLayoutPanel();
            flpMainMenu.Dock = DockStyle.Fill;
            flpMainMenu.AutoScroll = true;
            flpMainMenu.BackColor = Color.Transparent;
            TabPage MainMenuTab = new TabPage("MainMenu");
            MainMenuTab.BackColor = POSBackColor;
            MainMenuTab.Text = "Main Menu";
            MainMenuTab.Controls.Add(flpMainMenu);
            if (refreshProductTab && POSStatic.SHOW_DISPLAY_GROUP_BUTTONS)
                tabControlProducts.TabPages.Add(MainMenuTab);


            if (refreshProductTab)
            {
                for (int i = 0; i < ProductTbl.Rows.Count; i++)
                {
                    Button ProductButton = new Button();
                    ProductButton.Click += new EventHandler(ProductButton_Click);
                    ProductButton.MouseDown += ProductButton_MouseDown;
                    ProductButton.MouseUp += ProductButton_MouseUp;
                    ProductButton.Name = "ProductButton" + i.ToString();
                    ProductButton.Text = ProductTbl.Rows[i]["product_name"].ToString().Replace("&", "&&");
                    ProductButton.Tag = ProductTbl.Rows[i]["product_id"];
                    ProductButton.Font = SampleButtonCardProduct.Font;
                    ProductButton.ForeColor = SampleButtonCardProduct.ForeColor;
                    ProductButton.Size = SampleButtonCardProduct.Size;
                    ProductButton.FlatStyle = SampleButtonCardProduct.FlatStyle;
                    ProductButton.FlatAppearance.BorderColor = SampleButtonCardProduct.FlatAppearance.BorderColor;
                    ProductButton.FlatAppearance.BorderSize = SampleButtonCardProduct.FlatAppearance.BorderSize;
                    ProductButton.FlatAppearance.MouseDownBackColor = ProductButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
                    ProductButton.BackgroundImageLayout = ImageLayout.Zoom;
                    ProductButton.BackColor = Color.Transparent;

                    // Create the ToolTip and associate with the Form container.
                    ToolTip toolTip = new ToolTip();

                    // Set up the delays for the ToolTip.
                    toolTip.AutoPopDelay = 5000;
                    toolTip.InitialDelay = 1000;
                    toolTip.ReshowDelay = 500;
                    // Force the ToolTip text to be displayed whether or not the form is active.
                    toolTip.ShowAlways = true;

                    // Set up the ToolTip text for the Button
                    toolTip.SetToolTip(ProductButton, ProductTbl.Rows[i]["description"].ToString());

                    string productType = ProductTbl.Rows[i]["product_type"].ToString();
                    if (productType == "ATTRACTION")
                    {
                        ProductButton.BackgroundImage = sampleButtonAttraction.BackgroundImage;
                        ProductButton.ForeColor = sampleButtonAttraction.ForeColor;
                        ProductButton.Size = sampleButtonAttraction.Size;
                        ProductButton.Font = sampleButtonAttraction.Font;
                    }
                    else if (productType == "MANUAL" || productType == ProductTypeValues.SERVICECHARGE || productType == ProductTypeValues.GRATUITY)
                    {
                        ProductButton.BackgroundImage = SampleButtonOtherProduct.BackgroundImage;
                        ProductButton.ForeColor = SampleButtonOtherProduct.ForeColor;
                        ProductButton.Size = SampleButtonOtherProduct.Size;
                        ProductButton.Font = SampleButtonOtherProduct.Font;

                        if (ProductTbl.Rows[i]["ImageFileName"] != DBNull.Value)
                        {
                            try
                            {
                                object o = Utilities.executeScalar("exec ReadBinaryDataFromFile @FileName",
                                                                   new SqlParameter("@FileName", POSStatic.imageFolder + "\\" + ProductTbl.Rows[i]["ImageFileName"].ToString()));
                                if (o != null && o != DBNull.Value)
                                    ProductButton.BackgroundImage = Utilities.ConvertToImage(o);
                            }
                            // catch { }                        
                            catch (Exception ex)
                            {
                                log.Fatal("Ends-initializeProductTab() due to MANUAL product exception " + ex.Message);//Added for logger function on 29-Feb-2016
                            }
                        }
                    }
                    else if (productType == "CARDSALE")
                    {
                        ProductButton.BackgroundImage = sampleButtonCardSaleProduct.BackgroundImage;
                        ProductButton.ForeColor = sampleButtonCardSaleProduct.ForeColor;
                        ProductButton.ContextMenuStrip = ctxProductButtonContextMenu;
                    }
                    else if (productType == "NEW")
                    {
                        ProductButton.BackgroundImage = SampleButtonCardProduct.BackgroundImage;
                        ProductButton.ForeColor = SampleButtonCardProduct.ForeColor;
                    }
                    else if (productType == "RECHARGE")
                    {
                        ProductButton.BackgroundImage = btnSampleProductRecharge.BackgroundImage;
                        ProductButton.ForeColor = btnSampleProductRecharge.ForeColor;
                        ProductButton.ContextMenuStrip = ctxProductButtonContextMenu;
                    }
                    else if (productType == "VARIABLECARD")
                    {
                        ProductButton.BackgroundImage = btnSampleVariableRecharge.BackgroundImage;
                        ProductButton.ForeColor = btnSampleVariableRecharge.ForeColor;
                        ProductButton.ContextMenuStrip = ctxProductButtonContextMenu;
                    }
                    else if (productType == "GAMETIME")
                    {
                        ProductButton.BackgroundImage = SampleButtonGameTime.BackgroundImage;
                        ProductButton.ForeColor = SampleButtonGameTime.ForeColor;
                        ProductButton.ContextMenuStrip = ctxProductButtonContextMenu;
                    }
                    else if (productType.StartsWith("CHECK"))
                    {
                        ProductButton.BackgroundImage = btnSampleCheckInCheckOut.BackgroundImage;
                        ProductButton.ForeColor = btnSampleCheckInCheckOut.ForeColor;
                    }
                    else if (productType == "COMBO")
                    {
                        ProductButton.BackgroundImage = btnSampleCombo.BackgroundImage;
                        ProductButton.ForeColor = btnSampleCombo.ForeColor;
                        ProductButton.Size = btnSampleCombo.Size;
                        ProductButton.Font = btnSampleCombo.Font;
                    }
                    else if (productType == "LOCKER" || productType == "LOCKER_RETURN")
                    {
                        ProductButton.BackgroundImage = SampleButtonGameTime.BackgroundImage;
                        ProductButton.ForeColor = SampleButtonGameTime.ForeColor;
                        ProductButton.ContextMenuStrip = ctxProductButtonContextMenu;
                    }
                    //Begin: Added to display the Rental Products in POS on Nov-27-2015//
                    else if (productType == "RENTAL" || productType == "RENTAL_RETURN")
                    {
                        ProductButton.BackgroundImage = SampleButtonGameTime.BackgroundImage;
                        ProductButton.ForeColor = SampleButtonGameTime.ForeColor;
                        ProductButton.ContextMenuStrip = ctxProductButtonContextMenu;
                    }
                    //End: Added to display the Rental Products in POS on Nov-27-2015//
                    //Begin: Added to display the Booking Products in POS on Nov-27-2015//
                    else if (productType == "BOOKINGS")
                    {
                        ProductButton.BackgroundImage = SampleButtonGameTime.BackgroundImage;
                        ProductButton.ForeColor = SampleButtonGameTime.ForeColor;
                        ProductButton.Size = SampleButtonGameTime.Size;
                        ProductButton.Font = SampleButtonGameTime.Font;
                    }
                    //End: Added to display the Booking Products in POS on Nov-27-2015//
                    else if (productType == "VOUCHER")
                    {
                        ProductButton.BackgroundImage = SampleButtonVoucher.BackgroundImage;
                        ProductButton.ForeColor = SampleButtonVoucher.ForeColor;
                        ProductButton.ContextMenuStrip = ctxProductButtonContextMenu;
                    }
                    else
                    {
                        continue;
                    }

                    if (ProductTbl.Rows[i]["ButtonColor"].ToString().Trim() != "")
                    {
                        if (btnImageDictionary.ContainsKey(ProductTbl.Rows[i]["ButtonColor"].ToString().Trim()))
                            ProductButton.BackgroundImage = btnImageDictionary[ProductTbl.Rows[i]["ButtonColor"].ToString().Trim()];
                        else
                        {
                            setProductButtonColor(ProductButton, ProductTbl.Rows[i]["ButtonColor"].ToString());
                            btnImageDictionary.Add(ProductTbl.Rows[i]["ButtonColor"].ToString(), ProductButton.BackgroundImage);
                        }
                        //setProductButtonColor(ProductButton, ProductTbl.Rows[i]["ButtonColor"].ToString());
                    }

                    if (ProductTbl.Rows[i]["TextColor"].ToString().Trim() != "")
                    {
                        setProductButtonTextColor(ProductButton, ProductTbl.Rows[i]["TextColor"].ToString());
                    }
                    else if (ProductTbl.Rows[i]["ButtonColor"].ToString().Trim() != "")
                    {
                        Color bColor = POSUtils.getColor(ProductTbl.Rows[i]["ButtonColor"].ToString().Trim());
                        if (bColor.ToArgb() != 0)
                            ProductButton.ForeColor = Color.FromArgb(255 - bColor.R, 255 - bColor.G, 255 - bColor.B);
                    }

                    ProductButton.BackgroundImage.Tag = ProductButton.BackgroundImage;

                    if (ProductTbl.Rows[i]["Font"].ToString() != "")
                    {
                        setProductButtonFont(ProductButton, ProductTbl.Rows[i]["Font"].ToString());
                    }

                    if (productType == "MANUAL")
                    {
                        Size s = new Size((int)(ProductButton.Width * noncardButtonSize), (int)(ProductButton.Height * noncardButtonSize));
                        ProductButton.Size = s;
                    }
                    else if (productType == "COMBO")
                    {
                        Size s = new Size((int)(ProductButton.Width * comboButtonSize), (int)(ProductButton.Height * comboButtonSize));
                        ProductButton.Size = s;
                    }
                    else
                    {
                        Size s = new Size((int)(ProductButton.Width * cardButtonSize), (int)(ProductButton.Height * cardButtonSize));
                        ProductButton.Size = s;
                    }

                    //Modified 02/2019 for BearCat - 86-68
                    //Commenting out the code to show remaining quantity on product screen. Can be added back if needed.
                    //if (productType == "MANUAL" || productType == "COMBO")
                    //{
                    //    string Available = ProductTbl.Rows[i]["Available"].ToString();
                    //    string AvailableQty = ProductTbl.Rows[i]["AvailableQty"].ToString();
                    //    DateTime UnavailableTill = Convert.ToDateTime(ProductTbl.Rows[i]["UnavailableTill"].ToString());
                    //    if (Available.Equals("N") && (UnavailableTill > Utilities.getServerTime()))
                    //    {
                    //        Size sAvl = new Size((int)((ProductButton.Width) / 3), (int)((ProductButton.Height) / 3));
                    //        ProductButton.Image = new Bitmap(Properties.Resources.qty_1, sAvl);
                    //        ProductButton.ImageAlign = ContentAlignment.BottomRight;
                    //        ProductButton.TextAlign = ContentAlignment.TopCenter;
                    //        //ProductButton.ForeColor = Color.FromArgb(255, 0, 0);
                    //    }
                    //}

                    string dispGroup = ProductTbl.Rows[i]["display_group"].ToString();
                    if (dispGroup == "Others")
                        dispGroup = MessageUtils.getMessage("Others");
                    if (dispGroup != prev_display_group)
                    {
                        prev_display_group = dispGroup;

                        ProductTab = new TabPage(prev_display_group);
                        ProductTab.Name = prev_display_group;
                        ProductTab.BackColor = POSBackColor;
                        ProductTab.Text = prev_display_group.Replace("&", "&&");
                        tabControlProducts.TabPages.Add(ProductTab);
                        flpProducts = new FlowLayoutPanel();
                        flpProducts.Dock = DockStyle.Fill;
                        flpProducts.AutoScroll = true;
                        flpProducts.BackColor = Color.Transparent;

                        ProductTab.Controls.Add(flpProducts);

                        cmbDisplayGroups.Items.Add(prev_display_group);
                        int width = (int)(cmbDisplayGroups.CreateGraphics().MeasureString(prev_display_group, cmbDisplayGroups.Font).Width) + 20;
                        if (width > cmbDisplayGroups.Width)
                            cmbDisplayGroups.Width = width;

                        Button MainMenuButton = new Button();

                        MainMenuButton.Click += new EventHandler(MainMenuButton_Click);
                        MainMenuButton.MouseDown += ProductButton_MouseDown;
                        MainMenuButton.MouseUp += ProductButton_MouseUp;
                        MainMenuButton.Name = "MainMenu" + dispGroup;
                        MainMenuButton.Text = ProductTab.Text;
                        MainMenuButton.Tag = dispGroup;
                        MainMenuButton.Font = sampleButtonMainMenu.Font;
                        MainMenuButton.Size = sampleButtonMainMenu.Size;
                        MainMenuButton.FlatStyle = sampleButtonMainMenu.FlatStyle;
                        MainMenuButton.FlatAppearance.BorderColor = sampleButtonMainMenu.FlatAppearance.BorderColor;
                        MainMenuButton.FlatAppearance.BorderSize = sampleButtonMainMenu.FlatAppearance.BorderSize;
                        MainMenuButton.FlatAppearance.MouseDownBackColor = MainMenuButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
                        MainMenuButton.BackgroundImageLayout = ImageLayout.Zoom;
                        MainMenuButton.BackColor = Color.Transparent;
                        MainMenuButton.BackgroundImage = sampleButtonMainMenu.BackgroundImage;
                        MainMenuButton.ForeColor = sampleButtonMainMenu.ForeColor;

                        if (ProductTbl.Rows[i]["DispGroupButtonColor"].ToString().Trim() != "")
                        {
                            setProductButtonColor(MainMenuButton, ProductTbl.Rows[i]["DispGroupButtonColor"].ToString());
                        }
                        MainMenuButton.BackgroundImage.Tag = MainMenuButton.BackgroundImage;

                        if (ProductTbl.Rows[i]["DispGroupTextColor"].ToString().Trim() != "")
                        {
                            Color foreColor = POSUtils.getColor(ProductTbl.Rows[i]["DispGroupTextColor"].ToString());
                            if (foreColor.ToArgb() != 0)
                                MainMenuButton.ForeColor = foreColor;
                        }

                        if (ProductTbl.Rows[i]["DispGroupFont"].ToString() != "")
                        {
                            try
                            {
                                MainMenuButton.Font = CustomFontConverter.ConvertStringToFont(Utilities.ExecutionContext, ProductTbl.Rows[i]["DispGroupFont"].ToString());
                            }
                            //catch { }                       
                            catch (Exception ex)
                            {
                                log.Fatal("Ends-initializeProductTab() due to DispGroupFont exception " + ex.Message);//Added for logger function on 29-Feb-2016
                            }
                        }

                        flpMainMenu.Controls.Add(MainMenuButton);
                    }
                    flpProducts.Controls.Add(ProductButton);
                }
            }

            if (tabControlProducts.SelectedTab != null)
            {
                lblTabText.Text = tabControlProducts.SelectedTab.Text;
            }

            tabControlProducts.Refresh();
            if (refreshProductTab)
                POSStatic.POSProductTab = tabControlProducts;
            log.Debug("Ends-initializeProductTab()");//Added for logger function on 29-Feb-2016
        }

        private void MainMenuButton_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-MainMenuButton_Click()");//Added for logger function on 29-Feb-2016
            TabPage tp = tabControlProducts.TabPages[(sender as Button).Tag.ToString()];
            if (tp != null)
                tabControlProducts.SelectedTab = tp;

            log.Debug("Ends-MainMenuButton_Click()");//Added for logger function on 29-Feb-2016
        }

        void ProductButton_MouseUp(object sender, MouseEventArgs e)
        {
            log.Debug("Starts-ProductButton_MouseUp()");//Added for logger function on 29-Feb-2016
            try
            {
                Button b = (Button)sender;
                b.BackgroundImage = (Image)b.BackgroundImage.Tag;
            }
            //catch { }            
            catch (Exception exp)
            {
                log.Fatal("Ends-ProductButton_MouseUp() due to exception " + exp.Message);//Added for logger function on 29-Feb-2016
            }

            log.Debug("Ends-ProductButton_MouseUp()");//Added for logger function on 29-Feb-2016
        }

        void ProductButton_MouseDown(object sender, MouseEventArgs e)
        {
            log.Debug("Starts-ProductButton_MouseDown()");//Added for logger function on 29-Feb-2016
            try
            {
                Button b = (Button)sender;
                Image savImg = b.BackgroundImage.Tag as Image;
                b.BackgroundImage = POSUtils.pressedButtonImage((Bitmap)savImg);
                b.BackgroundImage.Tag = savImg;
            }
            //catch { }            
            catch (Exception exp)
            {
                log.Fatal("Ends-ProductButton_MouseDown() due to exception " + exp.Message);//Added for logger function on 29-Feb-2016
            }
            log.Debug("Ends-ProductButton_MouseDown()");//Added for logger function on 29-Feb-2016
        }

        bool setProductButtonColor(Button button, string bgcolor)
        {
            log.Debug("Starts-setProductButtonColor(button," + bgcolor + ")");//Added for logger function on 29-Feb-2016
            Color BGcolor = POSUtils.getColor(bgcolor);
            if (BGcolor.ToArgb() == 0)
            {
                log.Info("Ends-setProductButtonColor(button," + bgcolor + ") as BGcolor.ToArgb() == 0");//Added for logger function on 29-Feb-2016
                return false;
            }

            button.BackgroundImage = POSUtils.ChangeColor((Bitmap)button.BackgroundImage, BGcolor);
            log.Debug("Ends-setProductButtonColor(button," + bgcolor + ")");//Added for logger function on 29-Feb-2016
            return true;
        }

        bool setProductButtonTextColor(Button button, string fcolor)
        {
            log.Debug("Starts-setProductButtonTextColor(button," + fcolor + ")");//Added for logger function on 29-Feb-2016
            Color foreColor = POSUtils.getColor(fcolor);
            if (foreColor.ToArgb() == 0)
            {
                log.Info("Ends-setProductButtonTextColor(button," + fcolor + ") as foreColor.ToArgb() == 0");//Added for logger function on 29-Feb-2016
                return false;
            }

            button.ForeColor = foreColor;
            log.Debug("Ends-setProductButtonTextColor(button," + fcolor + ")");//Added for logger function on 29-Feb-2016
            return true;
        }

        bool setProductButtonFont(Button button, string font)
        {
            log.Debug("Starts-setProductButtonFont(button," + font + ")");//Added for logger function on 29-Feb-2016
            try
            {
                if (string.IsNullOrEmpty(font))
                    button.Font = new Font("Tahoma", 9);
                else
                {
                    button.Font = CustomFontConverter.ConvertStringToFont(Utilities.ExecutionContext, font);
                }
            }
            //catch { }            
            catch (Exception exp)
            {
                log.Fatal("Ends-setProductButtonFont(button," + font + ") due to exception " + exp.Message);//Added for logger function on 29-Feb-2016
            }
            
            log.Debug("Ends-setProductButtonFont(button," + font + ")");//Added for logger function on 29-Feb-2016
            return true;
        }

        private bool AreDataTablesSame(DataTable existingProductDT, DataTable newProductDT)
        {
            if (existingProductDT.Rows.Count != newProductDT.Rows.Count || existingProductDT.Columns.Count != newProductDT.Columns.Count)
                return false;


            for (int i = 0; i < existingProductDT.Rows.Count; i++)
            {
                for (int c = 0; c < existingProductDT.Columns.Count; c++)
                {
                    if (!Equals(existingProductDT.Rows[i][c], newProductDT.Rows[i][c]))
                        return false;
                }
            }
            return true;
        }

        private void DisposeControls(Control disposeControlObj)
        {
            log.LogMethodEntry();
            if (disposeControlObj.Controls.Count == 0)
            {
                if (!disposeControlObj.IsDisposed)
                    disposeControlObj.Dispose();
            }
            while (disposeControlObj.Controls.Count > 0)
            {
                if (disposeControlObj.Controls[0].Controls != null)
                {
                    DisposeControls(disposeControlObj.Controls[0]);
                }
                if (!disposeControlObj.IsDisposed)
                    disposeControlObj.Dispose();
            }
            log.LogMethodExit();
        }

        private void InitializeDiscountTab()
        {
            log.LogMethodEntry();
            flowLayoutPanelDiscounts.Controls.Clear();
            IEnumerable<DiscountContainerDTO> discountContainerDTOList =
                DiscountContainerList.GetTransactionDiscountsBLList(Utilities.ExecutionContext);

            DateTime currentTime = ServerDateTime.Now;
            int i = 0;
            foreach (DiscountContainerDTO discountContainerDTO in discountContainerDTOList)
            {
                i++;
                if (DiscountContainerList.IsDiscountAvailable(Utilities.ExecutionContext,discountContainerDTO.DiscountId, currentTime) == false)
                {
                    continue;
                }

                if (discountContainerDTO.DisplayInPOS != "Y")
                {
                    continue;
                }

                Button discountButton = new Button();
                discountButton.Click += new EventHandler(DiscountButton_Click);
                discountButton.Name = "DiscountButton" + i.ToString();
                discountButton.Text = discountContainerDTO.DiscountName;
                discountButton.Tag = discountContainerDTO;
                discountButton.Font = SampleButtonDiscount.Font;
                discountButton.BackColor = Color.Transparent;
                discountButton.ForeColor = SampleButtonDiscount.ForeColor;
                discountButton.Size = SampleButtonDiscount.Size;
                discountButton.FlatStyle = SampleButtonDiscount.FlatStyle;
                discountButton.FlatAppearance.BorderSize = 0;
                discountButton.FlatAppearance.MouseDownBackColor = SampleButtonDiscount.FlatAppearance.MouseDownBackColor;
                discountButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
                discountButton.BackgroundImage = SampleButtonDiscount.BackgroundImage;
                discountButton.BackgroundImageLayout = SampleButtonDiscount.BackgroundImageLayout;
                discountButton.TextAlign = SampleButtonDiscount.TextAlign;
                if (discountContainerDTO.AutomaticApply == "Y")
                    discountButton.Enabled = false;
                flowLayoutPanelDiscounts.Controls.Add(discountButton);
            }
            flowLayoutPanelDiscounts.Refresh();
            log.LogMethodExit();
        }

        private void initializeTasksTab()
        {
            log.Debug("Starts-initializeTasksTab()");//Added for logger function on 29-Feb-2016
            SqlCommand TaskCmd = Utilities.getCommand();
            TaskCmd.CommandText = "Select * from task_type";
            DataTable TaskTbl = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(TaskCmd);
            da.Fill(TaskTbl);

            System.Windows.Forms.Control.ControlCollection tasks = flowLayoutPanelFunctions.Controls;
            for (int i = 0; i < TaskTbl.Rows.Count; i++)
            {
                for (int j = 0; j < tasks.Count; j++)
                {
                    if (tasks[j].Tag != null && tasks[j].Tag.ToString() == TaskTbl.Rows[i]["task_type"].ToString())
                    {
                        if (TaskTbl.Rows[i]["display_in_pos"].ToString() != "Y")
                        {
                            tasks[j].Visible = false;
                        }
                        else
                        {
                            if (tasks[j].Text.Contains("Tickets"))
                                tasks[j].Text = tasks[j].Text.Replace("Tickets", POSStatic.TicketTermVariant);
                            else if (tasks[j].Text.Contains("Ticket"))
                                tasks[j].Text = tasks[j].Text.Replace("Ticket", POSStatic.TicketTermVariant.TrimEnd('s'));
                        }
                    }
                }

                if (btnRefundCard.Tag != null && btnRefundCard.Tag.ToString() == TaskTbl.Rows[i]["task_type"].ToString())
                {
                    if (TaskTbl.Rows[i]["display_in_pos"].ToString() != "Y")
                    {
                        btnRefundCard.Visible = false;
                        btnRefundCardTask.Visible = false;
                    }
                    else if (Utilities.getParafaitDefaults("HIDE_REFUND_IN_PRODUCT_TAB").Equals("Y"))
                    {
                        btnRefundCard.Visible = false;
                    }
                    else
                        btnRefundCardTask.Visible = false;
                }

                if (!btnRefundCard.Visible)
                    tabControlProducts.Height = tabPageProducts.Height;

                if (btnLoadBonus.Tag != null && btnLoadBonus.Tag.ToString() == TaskTbl.Rows[i]["task_type"].ToString())
                {
                    if (TaskTbl.Rows[i]["display_in_pos"].ToString() != "Y")
                    {
                        dataGridViewGamePlay.Columns["ReverseGamePlay"].Visible = false;
                    }
                }
            }

            flowLayoutPanelFunctions.Refresh();
            log.Debug("Ends-initializeTasksTab()");//Added for logger function on 29-Feb-2016
        }

        //Modified 02/2019 for BearCat - Show Recipe
        ProductDetailsUserControl productDetailsUI = null;
        void InitializeProductInquiryPanel()
        {
            log.LogMethodEntry();
            productDetailsUI = new ProductDetailsUserControl(Utilities);
            productDetailsUI.Location = new System.Drawing.Point(0, 20);
            productDetailsUI.Width = panelProductDetails.Width;
            productDetailsUI.BackColor = System.Drawing.Color.SlateGray;

            panelProductDetails.Controls.Add(productDetailsUI);
            Panel productDetailsPanel = this.Controls["ProductDetailsView"] as Panel;
            if (productDetailsPanel == null)
            {
                productDetailsPanel = new Panel();
                productDetailsPanel.BackColor = panelProductDetails.BackColor;
                productDetailsPanel.Name = "ProductDetailsView";
                productDetailsPanel.Size = new System.Drawing.Size(panelProductDetails.Width, panelProductDetails.Height);
                productDetailsPanel.Left = (this.Width - productDetailsPanel.Width) / 2;
                productDetailsPanel.Top = tabControlCardAction.Top + tabControlCardAction.ItemSize.Height - 5;
                productDetailsPanel.Font = panelProductDetails.Font;
                productDetailsPanel.BorderStyle = BorderStyle.FixedSingle;
                productDetailsPanel.Visible = false;
                this.Controls.Add(productDetailsPanel);
                productDetailsPanel.Controls.Add(panelProductDetails);
            }
            log.LogMethodExit();
        }
    }
}
