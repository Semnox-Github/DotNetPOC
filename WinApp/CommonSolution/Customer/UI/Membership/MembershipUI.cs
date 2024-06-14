/********************************************************************************************
 * Project Name - Customer.Membership
 * Description  - MembershipUI
 * 
 **************
 **Version Log
 **************
 *Version       Date             Modified By        Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019      Girish kundar      Modified :Removed Unused name space's.
 *2.80.0        17-Feb-2019      Deeksha            Modified to Make Cards module as
 *                                                  read only in Windows Management Studio.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.PriceList;
using Semnox.Parafait.Product;
using Semnox.Parafait.Publish;

namespace Semnox.Parafait.Customer.Membership
{
    public partial class MembershipUI : Form
    {
        private Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        private SortableBindingList<MembershipDTO> membershipSortableBindingList;
        private List<MembershipRuleDTO> membershipRuleListOnDisplay;
        private List<PriceListDTO> priceListOnDisplay;
        private List<ProductsDTO> rewardProductList;
        private List<LookupValuesDTO> memberFunctionLookupValuesDTO;
        private List<KeyValuePair<string, string>> attributeList;
        private List<KeyValuePair<string, string>> unitWindowValues;
        private DataGridView dgvfocused = null;
        int UIProfile = 1;
        private ManagementStudioSwitch managementStudioSwitch;

        public MembershipUI(Utilities _Utilities)
        {
            log.LogMethodEntry(_Utilities);
            InitializeComponent();
            utilities = _Utilities;
            utilities.setupDataGridProperties(ref dgvMembership);
            utilities.setupDataGridProperties(ref dgvMembershipRewards);
            if (utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            machineUserContext.SetIsCorporate(utilities.ExecutionContext.GetIsCorporate());
            machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
            utilities.setLanguage(this);
            LoadAll();
            PopulateMembership();
            dgvMembership.DataError += new DataGridViewDataErrorEventHandler(dgvMembership_ComboDataError);
            dgvMembershipRewards.DataError += new DataGridViewDataErrorEventHandler(dgvMembershipRewards_ComboDataError);
            managementStudioSwitch = new ManagementStudioSwitch(machineUserContext);
            UpdateUIElements();
            log.LogMethodExit();
        }
        private void LoadAll()
        {
            log.LogMethodEntry();
            LoadMembershipRule();
            LoadPriceList();
            LoadRewardProductList();
            LoadAttributeList();
            LoadMembershipFunctions();
            LoadUnitValues();
            LoadBaseMembership();
            log.LogMethodExit();
        }
        private void LoadMembershipRule()
        {
            log.LogMethodEntry();
            MembershipRulesList membershipRuleList = new MembershipRulesList(machineUserContext);
            List<KeyValuePair<MembershipRuleDTO.SearchByParameters, string>> membershipRuleSearchParams = new List<KeyValuePair<MembershipRuleDTO.SearchByParameters, string>>();
            membershipRuleSearchParams.Add(new KeyValuePair<MembershipRuleDTO.SearchByParameters, string>(MembershipRuleDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            membershipRuleSearchParams.Add(new KeyValuePair<MembershipRuleDTO.SearchByParameters, string>(MembershipRuleDTO.SearchByParameters.ISACTIVE, "1"));
            membershipRuleListOnDisplay = membershipRuleList.GetAllMembershipRule(membershipRuleSearchParams);
            if (membershipRuleListOnDisplay == null)
            {
                membershipRuleListOnDisplay = new List<MembershipRuleDTO>();
            }
            membershipRuleListOnDisplay.Insert(0, new MembershipRuleDTO());
            membershipRuleListOnDisplay[0].RuleName = "<SELECT>";
            membershipRuleIDDataGridViewTextBoxColumn.DataSource = membershipRuleListOnDisplay;
            membershipRuleIDDataGridViewTextBoxColumn.ValueMember = "MembershipRuleID";
            membershipRuleIDDataGridViewTextBoxColumn.DisplayMember = "RuleName";
            log.LogMethodExit();
        }
        private void LoadPriceList()
        {
            log.LogMethodEntry();
            PriceListList priceList = new PriceListList(machineUserContext);
            List<KeyValuePair<PriceListDTO.SearchByPriceListParameters, string>> priceSearchParams = new List<KeyValuePair<PriceListDTO.SearchByPriceListParameters, string>>();
            priceSearchParams.Add(new KeyValuePair<PriceListDTO.SearchByPriceListParameters, string>(PriceListDTO.SearchByPriceListParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            priceListOnDisplay = priceList.GetAllPriceList(priceSearchParams);
            if (priceListOnDisplay == null)
            {
                priceListOnDisplay = new List<PriceListDTO>();
            }
            priceListOnDisplay.Insert(0, new PriceListDTO());
            priceListOnDisplay[0].PriceListName = "<SELECT>";
            priceListIdDataGridViewTextBoxColumn.DataSource = priceListOnDisplay;
            priceListIdDataGridViewTextBoxColumn.ValueMember = "PriceListId";
            priceListIdDataGridViewTextBoxColumn.DisplayMember = "PriceListName";
            log.LogMethodExit();
        }
        private void LoadRewardProductList()
        {
            log.LogMethodEntry();
            Products products = new Products();
            ProductsFilterParams productsFilterParams = new ProductsFilterParams();
            productsFilterParams.SiteId = machineUserContext.GetSiteId();
            productsFilterParams.IsActive = true;
            rewardProductList = products.GetRewardProductDTOList(productsFilterParams);
            if (rewardProductList == null)
            {
                rewardProductList = new List<ProductsDTO>();
            }
            rewardProductList.Insert(0, new ProductsDTO());
            rewardProductList[0].ProductName = "<SELECT>";
            rewardProductIDDataGridViewTextBoxColumn.DataSource = rewardProductList;
            rewardProductIDDataGridViewTextBoxColumn.ValueMember = "ProductId";
            rewardProductIDDataGridViewTextBoxColumn.DisplayMember = "ProductName";
            log.LogMethodExit();
        }

        private void LoadAttributeList()
        {
            log.LogMethodEntry();
            attributeList = new List<KeyValuePair<string, string>>();
            attributeList.Add(new KeyValuePair<string, string>("", "<SELECT>"));
            attributeList.Add(new KeyValuePair<string, string>("L", utilities.MessageUtils.getMessage("Loyalty Points")));
            attributeList.Add(new KeyValuePair<string, string>("T", utilities.MessageUtils.getMessage("Tickets")));
            rewardAttributeDataGridViewTextBoxColumn.DataSource = attributeList;
            rewardAttributeDataGridViewTextBoxColumn.ValueMember = "Key";
            rewardAttributeDataGridViewTextBoxColumn.DisplayMember = "Value";
            log.LogMethodExit();
        }
        private void LoadMembershipFunctions()
        {
            log.LogMethodEntry();
            try
            {
                LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "MEMBERSHIP_REWARD_FUNCTION"));
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                memberFunctionLookupValuesDTO = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                if (memberFunctionLookupValuesDTO == null)
                {
                    memberFunctionLookupValuesDTO = new List<LookupValuesDTO>();
                }
                BindingSource bindingSource = new BindingSource();
                memberFunctionLookupValuesDTO.Insert(0, new LookupValuesDTO());
                memberFunctionLookupValuesDTO[0].LookupValueId = -1;
                memberFunctionLookupValuesDTO[0].Description = utilities.MessageUtils.getMessage("<Select>");
                rewardFunctionDataGridViewTextBoxColumn.DataSource = memberFunctionLookupValuesDTO;
                rewardFunctionDataGridViewTextBoxColumn.ValueMember = "LookupValue";
                rewardFunctionDataGridViewTextBoxColumn.DisplayMember = "Description";
            }
            catch (Exception e)
            {
                log.Error("Exception:" + e.ToString());
            }
            log.LogMethodExit();
        }
        private void LoadUnitValues()
        {
            log.LogMethodEntry();
            unitWindowValues = new List<KeyValuePair<string, string>>();
            unitWindowValues.Add(new KeyValuePair<string, string>(string.Empty, "<SELECT>"));
            unitWindowValues.Add(new KeyValuePair<string, string>("D", "Days"));
            unitWindowValues.Add(new KeyValuePair<string, string>("M", "Months"));
            unitWindowValues.Add(new KeyValuePair<string, string>("Y", "Years"));
            unitOfRewardFunctionPeriodDataGridViewTextBoxColumn.DataSource = new BindingSource(unitWindowValues, null);
            unitOfRewardFunctionPeriodDataGridViewTextBoxColumn.ValueMember = "Key";
            unitOfRewardFunctionPeriodDataGridViewTextBoxColumn.DisplayMember = "Value";
            unitOfRewardFrequencyDataGridViewTextBoxColumn.DataSource = new BindingSource(unitWindowValues, null);
            unitOfRewardFrequencyDataGridViewTextBoxColumn.ValueMember = "Key";
            unitOfRewardFrequencyDataGridViewTextBoxColumn.DisplayMember = "Value";
            log.LogMethodExit();
        }
        private void LoadBaseMembership()
        {
            log.LogMethodEntry();
            MembershipsList membershipList = new MembershipsList(machineUserContext);
            List<MembershipDTO> membershipDTOList;
            List<KeyValuePair<MembershipDTO.SearchByParameters, string>> searchByParameters = new List<KeyValuePair<MembershipDTO.SearchByParameters, string>>();
            searchByParameters.Add(new KeyValuePair<MembershipDTO.SearchByParameters, string>(MembershipDTO.SearchByParameters.IS_ACTIVE, "1"));
            searchByParameters.Add(new KeyValuePair<MembershipDTO.SearchByParameters, string>(MembershipDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            membershipDTOList = membershipList.GetAllMembership(searchByParameters, machineUserContext.GetSiteId());
            if (membershipDTOList == null)
            {
                membershipDTOList = new List<MembershipDTO>();
            }
            membershipDTOList.Insert(0, new MembershipDTO());
            membershipDTOList[0].MembershipName = "<SELECT>";
            baseMembershipIDDataGridViewTextBoxColumn.DataSource = membershipDTOList;
            baseMembershipIDDataGridViewTextBoxColumn.ValueMember = "MembershipID";
            baseMembershipIDDataGridViewTextBoxColumn.DisplayMember = "MembershipName";
            log.LogMethodExit();
        }
        private void PopulateMembership()
        {
            log.LogMethodEntry();
            MembershipsList membershipList = new MembershipsList(machineUserContext);
            List<KeyValuePair<MembershipDTO.SearchByParameters, string>> membershipSearchParams = new List<KeyValuePair<MembershipDTO.SearchByParameters, string>>();
            //membershipSearchParams.Add(new KeyValuePair<MembershipDTO.SearchByMembershipParameters, string>(MembershipDTO.SearchByMembershipParameters.ACTIVE_FLAG, "1"));
            membershipSearchParams.Add(new KeyValuePair<MembershipDTO.SearchByParameters, string>(MembershipDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            List<MembershipDTO> membershipDTOList = membershipList.GetAllMembership(membershipSearchParams, machineUserContext.GetSiteId(),true);
            membershipSortableBindingList = new SortableBindingList<MembershipDTO>();
            if (membershipDTOList != null)
            {
                membershipSortableBindingList = new SortableBindingList<MembershipDTO>(membershipDTOList);
                foreach (var membershipDTO in membershipDTOList)
                {
                    if (membershipDTO.MembershipRewardsDTOList != null)
                    {
                        membershipDTO.MembershipRewardsDTOList = new SortableBindingList<MembershipRewardsDTO>(membershipDTO.MembershipRewardsDTOList);
                    }
                    else
                    {
                        membershipDTO.MembershipRewardsDTOList = new SortableBindingList<MembershipRewardsDTO>();
                    }
                }
            }
            membershipDTOBindingSource.DataSource = membershipSortableBindingList;
            log.LogMethodExit();
        }
        private void Membership_CurrentChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SortableBindingList<MembershipRewardsDTO> sortableMembershipRewardsDTOList = null;
            if (membershipDTOBindingSource.Current != null && membershipDTOBindingSource.Current is MembershipDTO)
            {
                if ((membershipDTOBindingSource.Current as MembershipDTO).MembershipRewardsDTOList != null)
                {
                    sortableMembershipRewardsDTOList = (membershipDTOBindingSource.Current as MembershipDTO).MembershipRewardsDTOList as SortableBindingList<MembershipRewardsDTO>;
                }
                else
                {
                    sortableMembershipRewardsDTOList = new SortableBindingList<MembershipRewardsDTO>();
                }
            }
            else
            {
                sortableMembershipRewardsDTOList = new SortableBindingList<MembershipRewardsDTO>();
            }
            membershipRewardsDTOBindingSource.DataSource = sortableMembershipRewardsDTOList;
            log.LogMethodExit();
        }
        private void dgvMembership_ComboDataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            if (e.ColumnIndex == dgvMembership.Columns["membershipRuleIDDataGridViewTextBoxColumn"].Index)
            {
                if (membershipRuleListOnDisplay != null)
                    dgvMembership.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = membershipRuleListOnDisplay[0].MembershipRuleID;
            }
            else if (e.ColumnIndex == dgvMembership.Columns["priceListIdDataGridViewTextBoxColumn"].Index)
            {
                if (priceListOnDisplay != null)
                    dgvMembership.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = priceListOnDisplay[0].PriceListId;
            }
            log.LogMethodExit();
        }
        private void dgvMembershipRewards_ComboDataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            if (e.ColumnIndex == dgvMembershipRewards.Columns["rewardProductIDDataGridViewTextBoxColumn"].Index)
            {
                if (rewardProductList != null)
                    dgvMembershipRewards.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = rewardProductList[0].ProductId;
            }
            else if (e.ColumnIndex == dgvMembershipRewards.Columns["rewardAttributeDataGridViewTextBoxColumn"].Index)
            {
                if (attributeList != null)
                    dgvMembershipRewards.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = attributeList[0].Key;
            }
            else if (e.ColumnIndex == dgvMembershipRewards.Columns["rewardFunctionDataGridViewTextBoxColumn"].Index)
            {
                if (memberFunctionLookupValuesDTO != null)
                    dgvMembershipRewards.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = memberFunctionLookupValuesDTO[0].LookupValue;
            }
            else if (e.ColumnIndex == dgvMembershipRewards.Columns["unitOfRewardFunctionPeriodDataGridViewTextBoxColumn"].Index || e.ColumnIndex == dgvMembershipRewards.Columns["unitOfRewardFrequencyDataGridViewTextBoxColumn"].Index)
            {
                if (unitWindowValues != null)
                    dgvMembershipRewards.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = unitWindowValues[0].Key;
            }
            log.LogMethodExit();
        }

        private void MembershipUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (utilities.ParafaitEnv.IsCorporate && utilities.ParafaitEnv.IsMasterSite)
            {
                btnPublishToSite.Visible = true;
            }
            else
            {
                btnPublishToSite.Visible = false;
            }
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Close();
            log.LogMethodExit();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            LoadAll();
            PopulateMembership();
            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                log.LogMethodEntry(sender, e);
                dgvMembershipRewards.EndEdit();
                dgvMembership.EndEdit();
                if (membershipSortableBindingList != null && membershipSortableBindingList.Count > 0)
                {
                    foreach (MembershipDTO membershipDTO in membershipSortableBindingList)
                    {
                        if (!Validate(membershipDTO))
                        {
                            return;
                        }
                        else
                        {
                            using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                            {
                                try
                                {
                                    parafaitDBTrx.BeginTransaction();
                                    MembershipBL membership = new MembershipBL(machineUserContext,membershipDTO );
                                    membership.Save(parafaitDBTrx.SQLTrx);
                                    parafaitDBTrx.EndTransaction();
                                }
                                catch (Exception ex)
                                {
                                    parafaitDBTrx.RollBack();
                                    MessageBox.Show(utilities.MessageUtils.getMessage("Save Error") + " " + ex.Message, utilities.MessageUtils.getMessage("Save Error"));
                                    return;
                                }
                            }
                        }
                    }
                    MessageBox.Show(utilities.MessageUtils.getMessage(122));
                    PopulateMembership();
                }
                else
                    MessageBox.Show(utilities.MessageUtils.getMessage(371));
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage("Save Error") + " " + ex.Message, "Save Error");
                log.Error("Error in save. Exception:" + ex.ToString());
            }
        }
        private bool Validate(MembershipDTO membershipDTO)
        {
            log.LogMethodEntry(membershipDTO);
            if (string.IsNullOrEmpty(membershipDTO.MembershipName))
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(1144, utilities.MessageUtils.getMessage("Membership Name")));
                log.LogMethodExit(false, utilities.MessageUtils.getMessage(1144,  utilities.MessageUtils.getMessage("Membership Name")));
                return false;
            }
            if (membershipDTO.MembershipRuleID == -1)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(1144, utilities.MessageUtils.getMessage("Membership Rule")));
                log.LogMethodExit(false, utilities.MessageUtils.getMessage(1144,utilities.MessageUtils.getMessage("Membership Rule")));
                return false;
            }

            if (membershipDTO.MembershipRewardsDTOList != null && membershipDTO.MembershipRewardsDTOList.Count > 0)
            {
                foreach (MembershipRewardsDTO membershipRewardsDTO in membershipDTO.MembershipRewardsDTOList)
                {
                    if (string.IsNullOrEmpty(membershipRewardsDTO.RewardName))
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(1144,utilities.MessageUtils.getMessage("Membership Reward Name")));
                        log.LogMethodExit(false, utilities.MessageUtils.getMessage(1144, utilities.MessageUtils.getMessage("Membership Reward Name")));
                        return false;
                    }
                    if (membershipRewardsDTO.RewardProductID == -1 && string.IsNullOrEmpty(membershipRewardsDTO.RewardAttribute))
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(1484));
                        log.LogMethodExit(false, utilities.MessageUtils.getMessage(1484));
                        return false;
                    }
                    if (membershipRewardsDTO.RewardProductID != -1 && !string.IsNullOrEmpty(membershipRewardsDTO.RewardAttribute))
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(1485));
                        log.LogMethodExit(false, utilities.MessageUtils.getMessage(1485));
                        return false;
                    }
                    if(!string.IsNullOrEmpty(membershipRewardsDTO.RewardAttribute))
                    {
                        if(membershipRewardsDTO.RewardAttributePercent == 0)
                        {
                            MessageBox.Show(utilities.MessageUtils.getMessage(1486));
                            log.LogMethodExit(false, utilities.MessageUtils.getMessage(1486));
                            return false;
                        }
                        if (membershipRewardsDTO.RewardFunction == null)
                        {
                            MessageBox.Show(utilities.MessageUtils.getMessage(1487));
                            log.LogMethodExit(false, utilities.MessageUtils.getMessage(1487));
                            return false;
                        }
                        else
                        {
                            if (membershipRewardsDTO.RewardFunction == "TOPRD" &&
                                (membershipRewardsDTO.RewardFunctionPeriod == 0 || membershipRewardsDTO.UnitOfRewardFunctionPeriod == null))
                            {
                                MessageBox.Show(utilities.MessageUtils.getMessage(1488));
                                log.LogMethodExit(false, utilities.MessageUtils.getMessage(1488));
                                return false;
                            }
                        }
                    }
                    if(membershipRewardsDTO.RewardFrequency != 0 && membershipRewardsDTO.UnitOfRewardFrequency == null)
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(1489));
                        log.LogMethodExit(false, utilities.MessageUtils.getMessage(1489));
                        return false;
                    }
                    if(membershipRewardsDTO.RewardProductID != -1 && string.IsNullOrEmpty(membershipRewardsDTO.RewardAttribute))
                    {
                        if (membershipRewardsDTO.RewardFrequency == 0)
                            membershipRewardsDTO.UnitOfRewardFrequency = null;
                        membershipRewardsDTO.RewardAttributePercent = 0;
                        membershipRewardsDTO.RewardFunction = null;
                        membershipRewardsDTO.RewardFunctionPeriod = 0;
                        membershipRewardsDTO.UnitOfRewardFunctionPeriod = null;
                    }
                }
            }

            log.LogMethodExit(true);
            return true;
        }

        private void membershipDTOBindingSource_AddingNew(object sender, AddingNewEventArgs e)
        {
            log.LogMethodEntry();
            MembershipDTO membershipDTO = new MembershipDTO();
            membershipDTO.MembershipRewardsDTOList = new SortableBindingList<MembershipRewardsDTO>();
            e.NewObject = membershipDTO;
            log.LogMethodExit();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgvfocused != null)
            {
                if (this.dgvfocused.SelectedRows.Count <= 0 && this.dgvfocused.SelectedCells.Count <= 0)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(959));
                    log.Debug("Ends-btnDelete_Click event by \"No rows selected. Please select the rows you want to delete and press delete..\" message ");
                    return;
                }
                bool rowsDeleted = false;
                bool confirmDelete = false;
                if (this.dgvfocused.SelectedCells.Count > 0)
                {
                    foreach (DataGridViewCell cell in this.dgvfocused.SelectedCells)
                    {
                        dgvfocused.Rows[cell.RowIndex].Selected = true;
                    }
                }
                foreach (DataGridViewRow dgvSelectedRow in this.dgvfocused.SelectedRows)
                {
                    if (dgvSelectedRow.Cells[0].Value == null)
                    {
                        return;
                    }
                    if (Convert.ToInt32(dgvSelectedRow.Cells[0].Value.ToString()) <= 0)
                    {
                        dgvfocused.Rows.RemoveAt(dgvSelectedRow.Index);
                        rowsDeleted = true;
                    }
                    else
                    {
                        if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                        {
                            confirmDelete = true;
                            BindingSource focussedGridDataListDTOBS = (BindingSource)dgvfocused.DataSource;
                            switch (dgvfocused.Name)
                            {
                                case "dgvMembership":
                                    var membershipDTOList = (SortableBindingList<MembershipDTO>)focussedGridDataListDTOBS.DataSource;
                                    MembershipDTO membershipDTO = membershipDTOList[dgvSelectedRow.Index];
                                    membershipDTO.IsActive = false;
                                    MembershipBL membership = new MembershipBL(machineUserContext,membershipDTO);
                                    membership.Save();
                                    break;
                                case "dgvMembershipRewards":
                                    var membershipRewardsDTOList = (SortableBindingList<MembershipRewardsDTO>)focussedGridDataListDTOBS.DataSource;
                                    MembershipRewardsDTO membershipRewardsDTO = membershipRewardsDTOList[dgvSelectedRow.Index];
                                    membershipRewardsDTO.IsActive = false;
                                    MembershipRewardsBL membershipRewards = new MembershipRewardsBL(machineUserContext, membershipRewardsDTO);
                                    membershipRewards.Save();
                                    break;

                            }
                        }
                    }
                }
                if (rowsDeleted == true)
                    MessageBox.Show(utilities.MessageUtils.getMessage(957));
                PopulateMembership();
            }
            log.LogMethodExit();
        }

        private void dgvMembership_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            dgvfocused = (DataGridView)sender;
            log.LogMethodExit();
        }

        private void dgvMembershipRewards_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            dgvfocused = (DataGridView)sender;
            log.LogMethodExit();
        }

        private void btnExclusionRules_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (membershipDTOBindingSource.Current != null && membershipDTOBindingSource.Current is MembershipDTO)
                {
                    if ((membershipDTOBindingSource.Current as MembershipDTO) != null)
                    {
                        //Parafait.Card.frmMembershipRules frm = new Card.frmMembershipRules((membershipDTOBindingSource.Current as MembershipDTO).MembershipID);
                        //frm.ShowDialog();  
                        if (Application.OpenForms.Count > 0)
                        {
                            foreach (Form openForm in Application.OpenForms)
                            {
                                if (openForm.Name == "frmMembershipRules")
                                {
                                    openForm.Close();
                                    break;
                                }
                            }
                        }
                        Type type = Type.GetType("Parafait.Card.frmMembershipRules,Parafait");

                        //for this form I have an overloaded constructor, accepting a 'User' object
                        ConstructorInfo ci = type.GetConstructor(new Type[1] { typeof(object) });
                        object[] argVals = new object[] { (membershipDTOBindingSource.Current as MembershipDTO).MembershipID };
                        Form frmMembershipRules = (Form)ci.Invoke(argVals);
                        frmMembershipRules.Location = new System.Drawing.Point(this.Location.X, this.Location.Y + 25);
                        frmMembershipRules.StartPosition = this.StartPosition;
                        //frmMembershipRules.MdiParent = this.ParentForm;
                        //frmMembershipRules.Width = this.Width;
                        frmMembershipRules.ShowDialog();
                        log.LogMethodExit();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(utilities.MessageUtils.getMessage(1490));
            }
        }

        private void dgvMembershipRewards_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (this.dgvMembershipRewards.CurrentCellAddress.X == rewardProductIDDataGridViewTextBoxColumn.DisplayIndex)
            {
                if (e.Control is DataGridViewComboBoxEditingControl)
                {
                    ((ComboBox)e.Control).DropDownStyle = ComboBoxStyle.DropDown;
                    ((ComboBox)e.Control).AutoCompleteSource = AutoCompleteSource.ListItems;
                    ((ComboBox)e.Control).AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
                }

            }
            log.LogMethodExit();
        }

        private void btnMembershipRule_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if(Application.OpenForms.Count > 0)
                {
                    foreach (Form openForm in Application.OpenForms)
                    {
                        if (openForm.Name == "MembershipRuleUI")
                        {
                            openForm.Close();
                            break;
                        }
                    }
                }
                MembershipRuleUI membershipRuleUI = new MembershipRuleUI(utilities);
                membershipRuleUI.Location = new System.Drawing.Point(this.Location.X, this.Location.Y + 25);
                //membershipRuleUI.StartPosition = this.StartPosition;
                //membershipRuleUI.MdiParent = this.ParentForm;
                membershipRuleUI.Width = this.Width;
                setupVisuals(membershipRuleUI);
                membershipRuleUI.ShowDialog();
            }
            catch (Exception e1)
            { MessageBox.Show(e1.ToString()); }
            log.LogMethodExit();
        }

        private void btnPublishToSite_Click(object sender, EventArgs e)//Starts: Modification on 14-Jul-2016 for adding publish to site feature 
        {
            log.LogMethodEntry(sender, e);
            if (membershipDTOBindingSource.Current != null && membershipDTOBindingSource.Current is MembershipDTO)
            {
                MembershipDTO membershipDTO = membershipDTOBindingSource.Current as MembershipDTO;
                try
                {
                    if (membershipDTO != null)
                    {
                        if (membershipDTO.MembershipID > 0)
                        {
                            PublishUI publishUI = new PublishUI(utilities, membershipDTO.MembershipID, "Membership", membershipDTO.MembershipName);
                            publishUI.ShowDialog();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, utilities.MessageUtils.getMessage("Membership Publish"));
                    log.Error(ex);
                }
            }
            log.LogMethodExit();
        }

        public void setupVisuals(Control c)
        {
            log.LogMethodEntry(c);
            string type = c.GetType().ToString().ToLower();
            System.Drawing.Color SkinColor = System.Drawing.Color.White;
            if (c.HasChildren)
            {
                c.BackColor = SkinColor;
                foreach (Control cc in c.Controls)
                {
                    setupVisuals(cc);
                }
            }
            if (type.Contains("radiobutton"))
            {
                ;
            }
            else if (type.Contains("forms.button"))
            {
                setupButtonVisuals((Button)c);
            }
            else if (type.Contains("tabpage"))
            {
                TabPage tp = (TabPage)c;
                tp.BackColor = SkinColor;
            }
            else if (type == "system.windows.forms.datagridview")
            {
                DataGridView dg = (DataGridView)c;
                dg.BackgroundColor = SkinColor;
            }
            log.LogMethodExit();
        }

        public void setupButtonVisuals(Button b)
        {
            log.LogMethodEntry(b);
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseDownBackColor =
            b.FlatAppearance.MouseOverBackColor =
            b.BackColor = System.Drawing.Color.Transparent;
            b.Font = new System.Drawing.Font("arial", 8.5f);
            b.ForeColor = System.Drawing.Color.Black;
            if (b.Width < 100)
                b.Width = 90;
            b.Height = 25;
            b.BackgroundImageLayout = ImageLayout.Stretch;
            if (UIProfile == 2)
                b.BackgroundImage = Properties.Resources.ButtonNav2Normal;
            else
                b.BackgroundImage = Properties.Resources.normal3;

            b.MouseDown += b_MouseDown;
            b.MouseUp += b_MouseUp;
            log.LogMethodExit();
        }

        void b_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Button b = sender as Button;
            if (UIProfile == 2)
                b.BackgroundImage = Properties.Resources.ButtonNav2Normal;
            else
                b.BackgroundImage = Properties.Resources.normal3;
            b.ForeColor = System.Drawing.Color.Black;
            log.LogMethodExit();
        }

        void b_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Button b = sender as Button;
            if (UIProfile == 2)
                b.BackgroundImage = Properties.Resources.ButtonNav2Pressed;
            else
                b.BackgroundImage = Properties.Resources.pressed3;
            b.ForeColor = System.Drawing.Color.White;
            log.LogMethodExit();
        }

        private void UpdateUIElements()
        {
            log.LogMethodEntry();
            if (managementStudioSwitch.EnableCardsModule)
            {
                dgvMembership.AllowUserToAddRows = true;
                dgvMembership.ReadOnly = false;
                dgvMembershipRewards.AllowUserToAddRows = true;
                dgvMembershipRewards.ReadOnly = false;
                btnSave.Enabled = true;
                btnDelete.Enabled = true;
            }
            else
            {
                dgvMembership.AllowUserToAddRows = false;
                dgvMembership.ReadOnly = true;
                dgvMembershipRewards.AllowUserToAddRows = false;
                dgvMembershipRewards.ReadOnly = true;
                btnSave.Enabled = false;
                btnDelete.Enabled = false;
            }
            log.LogMethodExit();
        }
    }
}
