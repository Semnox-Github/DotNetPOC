/********************************************************************************************
 * Project Name - DiscountsSetupUI
 * Description  - UI for discounts setup
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70      23-Apr-2019   Guru S A       updates due to renamed classes for maint schedule module
 *2.70.2    3-Aug-2019    Girish Kundar  added LogMethodEntry() and LogMethodExit()
 *2.80      28-Jun-2020   Deeksha        Modified to Make Product module read only in Windows Management Studio.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Category;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Discounts;
using Semnox.Parafait.Product;
using Semnox.Parafait.Game;

namespace Semnox.Parafait.DiscountSetup
{
    /// <summary>
    /// DiscountsSetupUI Class.
    /// </summary>
    public partial class DiscountsSetupUI : Form
    {
        private Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext = ExecutionContext.GetExecutionContext();
        private string discountType = DiscountsBL.DISCOUNT_TYPE_TRANSACTION;
        private Dictionary<int, ProductsDTO> productsDTODictionary;
        private Dictionary<int, CategoryDTO> categoryDTODictionary;
        private Dictionary<int, ProductGroupDTO> productGroupDTODictionary;
        private Dictionary<int, GameDTO> gameDTODictionary;
        private List<ProductsDTO> productsDTOList;
        private List<CategoryDTO> categoryDTOList;
        private List<ProductGroupDTO> productGroupDTOList;
        private List<GameDTO> gameDTOList;
        private ManagementStudioSwitch managementStudioSwitch;
        /// <summary>
        /// Constructor of DiscountsSetupUI class.
        /// </summary>
        /// <param name="utilities">Parafait Utilities</param>
        public DiscountsSetupUI(Utilities utilities)
        {
            log.LogMethodEntry(utilities);
            InitializeComponent();
            this.utilities = utilities;
            utilities.setupDataGridProperties(ref dgvDiscountsDTOList);
            utilities.setupDataGridProperties(ref dgvDiscountPurchaseCriteriaDTOList);
            utilities.setupDataGridProperties(ref dgvDiscountedProductsDTOList);
            utilities.setupDataGridProperties(ref dgvDiscountedGamesDTOList);
            dgvDiscountedProductsDTOList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgvDiscountedGamesDTOList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            if (utilities.ParafaitEnv.IsCorporate)
            {
                executionContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                executionContext.SetSiteId(-1);
            }
            executionContext.SetUserId(utilities.ParafaitEnv.LoginID);
            utilities.setLanguage(this);
            dgvDiscountsDTOListDiscountPercentageTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();
            dgvDiscountsDTOListDiscountAmountTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();
            dgvDiscountedProductsDTOListDiscountPercentageTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();
            dgvDiscountedProductsDTOListDiscountAmountTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();
            dgvDiscountedProductsDTOListDiscountedPriceTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();

            dgvDiscountPurchaseCriteriaDTOListProductIdButtonColumn.Text = "...";
            dgvDiscountPurchaseCriteriaDTOListProductIdButtonColumn.UseColumnTextForButtonValue = true;
            dgvDiscountPurchaseCriteriaDTOListProductIdButtonColumn.Width = 30;

            dgvDiscountPurchaseCriteriaDTOListCategoryIdButtonColumn.Text = "...";
            dgvDiscountPurchaseCriteriaDTOListCategoryIdButtonColumn.UseColumnTextForButtonValue = true;
            dgvDiscountPurchaseCriteriaDTOListCategoryIdButtonColumn.Width = 30;

            dgvDiscountPurchaseCriteriaDTOListProductGroupIdButtonColumn.Text = "...";
            dgvDiscountPurchaseCriteriaDTOListProductGroupIdButtonColumn.UseColumnTextForButtonValue = true;
            dgvDiscountPurchaseCriteriaDTOListProductGroupIdButtonColumn.Width = 30;

            dgvDiscountedProductsDTOListProductIdButtonColumn.Text = "...";
            dgvDiscountedProductsDTOListProductIdButtonColumn.UseColumnTextForButtonValue = true;
            dgvDiscountedProductsDTOListProductIdButtonColumn.Width = 30;

            dgvDiscountedProductsDTOListCategoryIdButtonColumn.Text = "...";
            dgvDiscountedProductsDTOListCategoryIdButtonColumn.UseColumnTextForButtonValue = true;
            dgvDiscountedProductsDTOListCategoryIdButtonColumn.Width = 30;

            dgvDiscountedProductsDTOListProductGroupIdButtonColumn.Text = "...";
            dgvDiscountedProductsDTOListProductGroupIdButtonColumn.UseColumnTextForButtonValue = true;
            dgvDiscountedProductsDTOListProductGroupIdButtonColumn.Width = 30;

            dgvDiscountedGamesDTOListGameIdButtonColumn.Text = "...";
            dgvDiscountedGamesDTOListGameIdButtonColumn.UseColumnTextForButtonValue = true;
            dgvDiscountedGamesDTOListGameIdButtonColumn.Width = 30;

            grpDiscountedGames.Visible = false;
            tlpDiscountsPurchaseCriteriaAndProducts.Visible = true;
            managementStudioSwitch = new ManagementStudioSwitch(executionContext);
            log.LogMethodExit();
        }

        private async void DiscountsSetupUI_Load(object sender, EventArgs e)
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
            dgvDiscountsDTOListCouponsButtonColumn.Text = "...";
            dgvDiscountsDTOListCouponsButtonColumn.UseColumnTextForButtonValue = true;
            dgvDiscountsDTOListScheduleIdComboBoxColumn.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            OnDataLoadStart();
            await RefreshData();
            OnDataLoadComplete();
            log.LogMethodExit();
        }

        private void OnDataLoadStart()
        {
            log.LogMethodEntry();
            DisableControls();
            lblStatus.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 1448);
            log.LogMethodExit();
        }

        private void OnDataLoadComplete()
        {
            log.LogMethodEntry();
            lblStatus.Text = "";
            EnableControls();
            UpdateUIElements();
            log.LogMethodExit();
        }

        private List<Control> GetControlsToBeDisabledOnDataLoad()
        {
           
            return new List<Control>()
            {
                //tcDiscountsDTOList,
                btnSearch,
                chbShowActiveEntries,
                txtName,
                lnkClearDiscountedProducts,
                lnkClearDiscountedProducts,
                lnkSelectAll,
                btnSave,
                btnRefresh,
                btnDelete,
                btnClose,
                btnSchedule,
                btnPublishToSite,
                lnkClearDiscountedGameplayGames,
                transactionDiscountsTab,
                gamePlayDiscountsTab,
                loyaltyDiscountsTab
            };
        }

        private void DisableControls()
        {
            log.LogMethodEntry();
            foreach (var control in GetControlsToBeDisabledOnDataLoad())
            {
                control.Enabled = false;
            }
            RefreshControls();
            log.LogMethodExit();
        }

        private void RefreshControls()
        {
            log.LogMethodEntry();
            foreach (var control in GetControlsToBeDisabledOnDataLoad())
            {
                control.Refresh();
            }
            lblStatus.Refresh();
            log.LogMethodExit();
        }

        private void EnableControls()
        {
            log.LogMethodEntry();
            foreach (var control in GetControlsToBeDisabledOnDataLoad())
            {
                control.Enabled = true;
            }
            RefreshControls();
            log.LogMethodExit();
        }

        private async Task RefreshData()
        {
            log.LogMethodEntry();
            Task[] taskList;
            Task<List<TransactionProfileDTO>> getTransactionProfileDTOListTask = Task<List<TransactionProfileDTO>>.Factory.StartNew(() => { return GetTransactionProfileDTOList(); });
            Task<List<ProductsDTO>> getProductsDTOListTask = Task<List<ProductsDTO>>.Factory.StartNew(() => { return GetProductsDTOList(); });
            Task<List<CategoryDTO>> getCategoryDTOListTask = Task<List<CategoryDTO>>.Factory.StartNew(() => { return GetCategoryDTOList(); });
            Task<List<GameDTO>> getGameDTOListTask = Task<List<GameDTO>>.Factory.StartNew(() => { return GetGameDTOList(); });
            Task<List<ProductGroupDTO>> getProductGroupDTOListTask = Task<List<ProductGroupDTO>>.Factory.StartNew(() => { return GetProductGroupDTOList(); });
            Task<List<ScheduleCalendarDTO>> getScheduleDTOListTask = Task<List<Semnox.Core.GenericUtilities.ScheduleCalendarDTO>>.Factory.StartNew(() => { return GetScheduleDTOList(); });
            Task<List<DiscountsDTO>> getDiscountsDTOListTask = GetDiscountsDTOList(txtName.Text, discountType, executionContext.GetSiteId(), chbShowActiveEntries.Checked);
            taskList = new Task[] { getTransactionProfileDTOListTask,
                                    getProductsDTOListTask,
                                    getCategoryDTOListTask,
                                    getProductGroupDTOListTask,
                                    getScheduleDTOListTask,
                                    getDiscountsDTOListTask };

            await Task.WhenAll(taskList);

            SetTransactionProfileList(getTransactionProfileDTOListTask.Result);
            SetProductsDTOList(getProductsDTOListTask.Result);
            SetCategoryDTOList(getCategoryDTOListTask.Result);
            SetGameDTOList(getGameDTOListTask.Result);
            SetProductGroupDTOList(getProductGroupDTOListTask.Result);
            SetScheduleDTOList(getScheduleDTOListTask.Result);
            SetDiscountsDTOList(getDiscountsDTOListTask.Result);
            log.LogMethodExit();
        }

        private void SetTransactionProfileList(List<TransactionProfileDTO> transactionProfileDTOList)
        {
            log.LogMethodEntry(transactionProfileDTOList);
            dgvDiscountsDTOListTransactionProfileIdComboBoxColumn.DisplayMember = "ProfileName";
            dgvDiscountsDTOListTransactionProfileIdComboBoxColumn.ValueMember = "TransactionProfileId";
            dgvDiscountsDTOListTransactionProfileIdComboBoxColumn.DataSource = transactionProfileDTOList;
            log.LogMethodExit();
        }

        private void SetGameDTOList(List<GameDTO> gameDTOList)
        {
            log.LogMethodEntry(gameDTOList);
            BindingSource bs = new BindingSource();
            bs.DataSource = gameDTOList;
            dgvDiscountedGamesDTOListGameIdComboBoxColumn.DisplayMember = "GameName";
            dgvDiscountedGamesDTOListGameIdComboBoxColumn.ValueMember = "GameId";
            dgvDiscountedGamesDTOListGameIdComboBoxColumn.DataSource = bs;
            log.LogMethodExit();
        }

        List<TransactionProfileDTO> GetTransactionProfileDTOList()
        {
            log.LogMethodEntry();
            TransactionProfileListBL transactionProfileListBL = new TransactionProfileListBL(executionContext);
            List<KeyValuePair<TransactionProfileDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TransactionProfileDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<TransactionProfileDTO.SearchByParameters, string>(TransactionProfileDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<TransactionProfileDTO> transactionProfileDTOList = transactionProfileListBL.GetTransactionProfileDTOList(searchParameters);
            if (transactionProfileDTOList == null)
            {
                transactionProfileDTOList = new List<TransactionProfileDTO>();
            }
            transactionProfileDTOList.Insert(0, new TransactionProfileDTO());
            transactionProfileDTOList[0].ProfileName = "Select";
            log.LogMethodExit(transactionProfileDTOList);
            return transactionProfileDTOList;
        }

        private void SetProductsDTOList(List<ProductsDTO> productsDTOList)
        {
            log.LogMethodEntry(productsDTOList);
            BindingSource bs = new BindingSource();
            bs.DataSource = productsDTOList;
            dgvDiscountPurchaseCriteriaDTOListProductIdComboBoxColumn.DisplayMember = "ProductName";
            dgvDiscountPurchaseCriteriaDTOListProductIdComboBoxColumn.ValueMember = "ProductId";
            dgvDiscountPurchaseCriteriaDTOListProductIdComboBoxColumn.DataSource = bs;

            bs = new BindingSource();
            bs.DataSource = productsDTOList;
            dgvDiscountedProductsDTOListProductIdComboBoxColumn.DisplayMember = "ProductName";
            dgvDiscountedProductsDTOListProductIdComboBoxColumn.ValueMember = "ProductId";
            dgvDiscountedProductsDTOListProductIdComboBoxColumn.DataSource = productsDTOList;

            log.LogMethodExit();
        }

        private void SetProductGroupDTOList(List<ProductGroupDTO> productGroupDTOList)
        {
            log.LogMethodEntry(productGroupDTOList);

            BindingSource bs = new BindingSource();
            bs.DataSource = productGroupDTOList;
            dgvDiscountPurchaseCriteriaDTOListProductGroupIdComboBoxColumn.DisplayMember = "Name";
            dgvDiscountPurchaseCriteriaDTOListProductGroupIdComboBoxColumn.ValueMember = "Id";
            dgvDiscountPurchaseCriteriaDTOListProductGroupIdComboBoxColumn.DataSource = bs;

            bs = new BindingSource();
            bs.DataSource = productGroupDTOList;
            dgvDiscountedProductsDTOListProductGroupIdComboBoxColumn.DisplayMember = "Name";
            dgvDiscountedProductsDTOListProductGroupIdComboBoxColumn.ValueMember = "Id";
            dgvDiscountedProductsDTOListProductGroupIdComboBoxColumn.DataSource = bs;

            log.LogMethodExit();
        }

        private List<ProductsDTO> GetProductsDTOList()
        {
            log.LogMethodEntry();
            Products products = new Products();
            List<ProductsDTO> productList;
            ProductsFilterParams productsFilterParams = new ProductsFilterParams();
            productsFilterParams.IsActive = false;
            productsFilterParams.SiteId = executionContext.GetSiteId();
            productList = products.GetProductDTOList(productsFilterParams);
            productsDTODictionary = new Dictionary<int, ProductsDTO>();
            if (productList == null)
            {
                productList = new List<ProductsDTO>();
            }
            else
            {
                for (int i = 0; i < productList.Count; i++)
                {
                    if(productList[i].ActiveFlag == false)
                    {
                        productList[i].ProductName = productList[i].ProductName + " (" + MessageContainerList.GetMessage(utilities.ExecutionContext, "Inactive") + ")";
                    }
                    productsDTODictionary.Add(productList[i].ProductId, productList[i]);
                }
            }
            productsDTOList = productList;
            productList.Insert(0, new ProductsDTO());
            productList[0].ProductId = -1;
            productList[0].ProductName = "-All-";
            productList[0].ActiveFlag = true;
            log.LogMethodExit(productList);
            return productList;
        }

        private List<ProductGroupDTO> GetProductGroupDTOList()
        {
            log.LogMethodEntry();
            using(UnitOfWork unitOfWork = new UnitOfWork())
            {
                ProductGroupListBL productGroupListBL = new ProductGroupListBL(executionContext, unitOfWork);
                SearchParameterList<ProductGroupDTO.SearchByParameters> searchParemeters = new SearchParameterList<ProductGroupDTO.SearchByParameters>();
                searchParemeters.Add(ProductGroupDTO.SearchByParameters.SITE_ID, executionContext.SiteId);
                productGroupDTOList = productGroupListBL.GetProductGroupDTOList(searchParemeters);
            }
            productGroupDTODictionary = new Dictionary<int, ProductGroupDTO>();
            if (productGroupDTOList == null)
            {
                productGroupDTOList = new List<ProductGroupDTO>();
            }
            else
            {
                for (int i = 0; i < productGroupDTOList.Count; i++)
                {
                    if (productGroupDTOList[i].IsActive == false)
                    {
                        productGroupDTOList[i].Name = productGroupDTOList[i].Name + " (" + MessageContainerList.GetMessage(utilities.ExecutionContext, "Inactive") + ")";
                    }
                    productGroupDTODictionary.Add(productGroupDTOList[i].Id, productGroupDTOList[i]);
                }
            }
            productGroupDTOList.Insert(0, new ProductGroupDTO());
            productGroupDTOList[0].Id = -1;
            productGroupDTOList[0].Name = "-All-";
            productGroupDTOList[0].IsActive = true;
            log.LogMethodExit(productGroupDTOList);
            return productGroupDTOList;
        }

        private void SetCategoryDTOList(List<CategoryDTO> categoryDTOList)
        {
            log.LogMethodEntry(categoryDTOList);
            BindingSource bs = new BindingSource();
            bs.DataSource = categoryDTOList;
            dgvDiscountPurchaseCriteriaDTOListCategoryIdComboBoxColumn.DisplayMember = "Name";
            dgvDiscountPurchaseCriteriaDTOListCategoryIdComboBoxColumn.ValueMember = "CategoryId";
            dgvDiscountPurchaseCriteriaDTOListCategoryIdComboBoxColumn.DataSource = bs;

            bs = new BindingSource();
            bs.DataSource = categoryDTOList;
            dgvDiscountedProductsDTOListCategoryIdComboBoxColumn.DisplayMember = "Name";
            dgvDiscountedProductsDTOListCategoryIdComboBoxColumn.ValueMember = "CategoryId";
            dgvDiscountedProductsDTOListCategoryIdComboBoxColumn.DataSource = bs;

            log.LogMethodExit();
        }

        private List<CategoryDTO> GetCategoryDTOList()
        {
            log.LogMethodEntry();
            CategoryList categoryList = new CategoryList(executionContext);
            List<CategoryDTO> categories;
            List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>> searchParameters = new List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>>();
            //searchParameters.Add(new KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>(CategoryDTO.SearchByCategoryParameters.IS_ACTIVE, "Y"));
            searchParameters.Add(new KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>(CategoryDTO.SearchByCategoryParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            categories = categoryList.GetAllCategory(searchParameters);
            categoryDTODictionary = new Dictionary<int, CategoryDTO>();
            if (categories == null)
            {
                categories = new List<CategoryDTO>();
            }
            else
            {
                for (int i = 0; i < categories.Count; i++)
                {
                    if (categories[i].IsActive != true)
                    {
                        categories[i].Name = categories[i].Name + " (" + MessageContainerList.GetMessage(utilities.ExecutionContext, "Inactive") + ")";
                    }
                    categoryDTODictionary.Add(categories[i].CategoryId, categories[i]);
                }
            }
            categoryDTOList = categories;
            categories.Insert(0, new CategoryDTO());
            categories[0].CategoryId = -1;
            categories[0].Name = "-All-";
            categories[0].IsActive = true;
            log.LogMethodExit(categories);
            return categories;
        }

        private List<GameDTO> GetGameDTOList()
        {
            log.LogMethodEntry();
            GameList gameList = new GameList(executionContext);
            List<GameDTO> list;
            List<KeyValuePair<GameDTO.SearchByGameParameters, string>> searchParameters = new List<KeyValuePair<GameDTO.SearchByGameParameters, string>>();
            //searchParameters.Add(new KeyValuePair<GameDTO.SearchByGameParameters, string>(GameDTO.SearchByGameParameters.IS_ACTIVE, "Y"));
            searchParameters.Add(new KeyValuePair<GameDTO.SearchByGameParameters, string>(GameDTO.SearchByGameParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            list = gameList.GetGameList(searchParameters, false);
            gameDTODictionary = new Dictionary<int, GameDTO>();
            if (list == null)
            {
                list = new List<GameDTO>();
            }
            else
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].IsActive != true)
                    {
                        list[i].GameName = list[i].GameName + " (" + MessageContainerList.GetMessage(utilities.ExecutionContext, "Inactive") + ")";
                    }
                    gameDTODictionary.Add(list[i].GameId, list[i]);
                }
            }
            gameDTOList = list;
            list.Insert(0, new GameDTO());
            list[0].GameId = -1;
            list[0].GameName = "-All-";
            list[0].IsActive = true;
            log.LogMethodExit(list);
            return list;
        }

        private async Task LoadScheduleDTOList()
        {
            log.LogMethodEntry();
            List<Semnox.Core.GenericUtilities.ScheduleCalendarDTO> ScheduleDTOList = await Task<List<Semnox.Core.GenericUtilities.ScheduleCalendarDTO>>.Factory.StartNew(() => { return GetScheduleDTOList(); });
            SetScheduleDTOList(ScheduleDTOList);
            log.LogMethodExit();
        }

        private void SetScheduleDTOList(List<ScheduleCalendarDTO> ScheduleDTOList)
        {
            log.LogMethodEntry(ScheduleDTOList);
            dgvDiscountsDTOListScheduleIdComboBoxColumn.DisplayMember = "ScheduleName";
            dgvDiscountsDTOListScheduleIdComboBoxColumn.ValueMember = "ScheduleId";
            dgvDiscountsDTOListScheduleIdComboBoxColumn.DataSource = ScheduleDTOList;
            log.LogMethodExit();
        }

        private List<Semnox.Core.GenericUtilities.ScheduleCalendarDTO> GetScheduleDTOList()
        {
            log.LogMethodEntry();
            Semnox.Core.GenericUtilities.ScheduleCalendarListBL ScheduleList = new Semnox.Core.GenericUtilities.ScheduleCalendarListBL(executionContext);
            List<Semnox.Core.GenericUtilities.ScheduleCalendarDTO> ScheduleDTOList;
            List<KeyValuePair<Semnox.Core.GenericUtilities.ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>> searchParameters = new List<KeyValuePair<Semnox.Core.GenericUtilities.ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>>();
            //searchParameters.Add(new KeyValuePair<ScheduleDTO.SearchByScheduleParameters, string>(ScheduleDTO.SearchByScheduleParameters.IS_ACTIVE, "1"));
            searchParameters.Add(new KeyValuePair<Semnox.Core.GenericUtilities.ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>(Semnox.Core.GenericUtilities.ScheduleCalendarDTO.SearchByScheduleCalendarParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            ScheduleDTOList = ScheduleList.GetAllSchedule(searchParameters);
            if (ScheduleDTOList == null)
            {
                ScheduleDTOList = new List<Semnox.Core.GenericUtilities.ScheduleCalendarDTO>();
            }
            ScheduleDTOList.Insert(0, new Semnox.Core.GenericUtilities.ScheduleCalendarDTO());
            ScheduleDTOList[0].ScheduleId = -1;
            ScheduleDTOList[0].ScheduleName = "";
            log.LogMethodExit(ScheduleDTOList);
            return ScheduleDTOList;
        }

        private async Task LoadDiscountsDTOList()
        {
            log.LogMethodEntry();
            List<DiscountsDTO> discountsDTOList = await GetDiscountsDTOList(txtName.Text, discountType, executionContext.GetSiteId(), chbShowActiveEntries.Checked); ;
            SetDiscountsDTOList(discountsDTOList);
            log.LogMethodExit();
        }

        private void SetDiscountsDTOList(List<DiscountsDTO> discountsDTOList)
        {
            log.LogMethodEntry(discountsDTOList);
            SortableBindingList<DiscountsDTO> discountsDTOSortableList;
            if (discountsDTOList != null)
            {
                discountsDTOSortableList = new SortableBindingList<DiscountsDTO>(discountsDTOList);
            }
            else
            {
                discountsDTOSortableList = new SortableBindingList<DiscountsDTO>();
            }
            discountsDTOListBS.DataSource = discountsDTOSortableList;
            log.LogMethodExit();
        }

        private async Task<List<DiscountsDTO>> GetDiscountsDTOList(string discountName, 
                                                       string discountType, 
                                                       int siteId, 
                                                       bool activeEntriesOnly)
        {
            log.LogMethodEntry(discountName, discountType, siteId, activeEntriesOnly);
            IDiscountUseCases discountUseCases = DiscountUseCaseFactory.GetDiscountUseCases(executionContext, Guid.NewGuid().ToString()); 
            List<DiscountsDTO> discountsDTOList = await discountUseCases.GetDiscountsDTOList(isActive: activeEntriesOnly? "1": null,
                                                                                             discountType: discountType,
                                                                                             siteId:siteId,
                                                                                             discountName:discountName,
                                                                                             loadChildRecords:true,
                                                                                             loadActiveChildRecords: activeEntriesOnly);
            log.LogMethodExit(discountsDTOList);
            return discountsDTOList;
        }

        private async void tcDiscountsDTOList_Selected(object sender, TabControlEventArgs e)
        {
            log.LogMethodEntry();
            bool showColumns = true;
            if (tcDiscountsDTOList.SelectedTab == transactionDiscountsTab)
            {
                showColumns = true;
                discountType = DiscountsBL.DISCOUNT_TYPE_TRANSACTION;
                grpDiscountedGames.Visible = false;
                tlpDiscountsPurchaseCriteriaAndProducts.Visible = true;
            }
            else if (tcDiscountsDTOList.SelectedTab == gamePlayDiscountsTab)
            {
                showColumns = false;
                discountType = DiscountsBL.DISCOUNT_TYPE_GAMEPLAY;
                grpDiscountedGames.Visible = true;
                tlpDiscountsPurchaseCriteriaAndProducts.Visible = false;
            }
            else if (tcDiscountsDTOList.SelectedTab == loyaltyDiscountsTab)
            {
                showColumns = false;
                discountType = DiscountsBL.DISCOUNT_TYPE_LOYALTY;
                grpDiscountedGames.Visible = true;
                tlpDiscountsPurchaseCriteriaAndProducts.Visible = false;
            }
            dgvDiscountsDTOListAutomaticApplyCheckBoxColumn.Visible = showColumns;
            dgvDiscountsDTOListDiscountAmountTextBoxColumn.Visible = showColumns;
            dgvDiscountsDTOListMinimumSaleAmountTextBoxColumn.Visible = showColumns;
            dgvDiscountsDTOListDisplayInPOSCheckBoxColumn.Visible = showColumns;
            dgvDiscountsDTOListSortOrderTextBoxColumn.Visible = showColumns;
            dgvDiscountsDTOListManagerApprovalRequiredCheckBoxColumn.Visible = showColumns;
            dgvDiscountsDTOListVariableDiscountsCheckBoxColumn.Visible = showColumns;
            dgvDiscountsDTOListRemarksMandatoryCheckBoxColumn.Visible = showColumns;
            dgvDiscountsDTOListCouponMandatoryCheckBoxColumn.Visible = showColumns;
            dgvDiscountsDTOListCouponsButtonColumn.Visible = showColumns;
            dgvDiscountsDTOListTransactionProfileIdComboBoxColumn.Visible = showColumns;
            AllowMultipleApplication.Visible = showColumns;
            DiscountCriteriaLines.Visible = showColumns;
            dgvDiscountsDTOListApplicationLimitTextBoxColumn.Visible = showColumns;
            OnDataLoadStart();
            await LoadDiscountsDTOList();
            OnDataLoadComplete();
            log.LogMethodExit();
        }

        private void discountsDTOListBS_AddingNew(object sender, AddingNewEventArgs e)
        {
            log.LogMethodEntry();
            DiscountsDTO discountsDTO = new DiscountsDTO();
            discountsDTO.DiscountType = discountType;
            if (discountType == DiscountsBL.DISCOUNT_TYPE_TRANSACTION)
            {
                discountsDTO.DisplayInPOS = "Y";
            }
            e.NewObject = discountsDTO;
            log.LogMethodExit();
        }

        private void discountsDTOListBS_CurrentChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            bool disableDiscountedProductsDiscountPercentage = true;
            bool disableDiscountedProductsDiscountAmount = true;
            bool disableDiscountedProductsDiscountedPrice = true;
            if (discountsDTOListBS.Current != null && discountsDTOListBS.Current is DiscountsDTO)
            {
                DiscountsDTO discountsDTO = discountsDTOListBS.Current as DiscountsDTO;
                discountedGamesDTOListBS.DataSource = discountsDTO.DiscountedGamesDTOList;
                discountPurchaseCriteriaDTOListBS.DataSource = discountsDTO.DiscountPurchaseCriteriaDTOList;
                if (discountsDTO.DiscountPercentage > 0)
                {
                    disableDiscountedProductsDiscountPercentage = false;
                }
                if (discountsDTO.DiscountAmount > 0)
                {
                    disableDiscountedProductsDiscountAmount = false;
                }

                if ((discountsDTO.DiscountPercentage.HasValue == false || discountsDTO.DiscountPercentage.Value == 0) &&
                    (discountsDTO.DiscountAmount.HasValue == false || discountsDTO.DiscountAmount.Value == 0))
                {
                    disableDiscountedProductsDiscountAmount = false;
                    disableDiscountedProductsDiscountedPrice = false;
                }
                SetDiscountedProductsDTOListBS();
            }
            dgvDiscountedProductsDTOListDiscountPercentageTextBoxColumn.ReadOnly = disableDiscountedProductsDiscountPercentage;
            dgvDiscountedProductsDTOListDiscountAmountTextBoxColumn.ReadOnly = disableDiscountedProductsDiscountAmount;
            dgvDiscountedProductsDTOListDiscountedPriceTextBoxColumn.ReadOnly = disableDiscountedProductsDiscountedPrice;
            log.LogMethodExit();
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            chbShowActiveEntries.Checked = true;
            txtName.ResetText();
            discountedGamesDTOListBS.DataSource = new SortableBindingList<DiscountedGamesDTO>();
            discountPurchaseCriteriaDTOListBS.DataSource = new SortableBindingList<DiscountPurchaseCriteriaDTO>();
            discountedProductsDTOListBS.DataSource = new SortableBindingList<DiscountedProductsDTO>();
            OnDataLoadStart();
            await RefreshData();
            OnDataLoadComplete();
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Close();
            log.LogMethodExit();
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            OnDataLoadStart();
            await LoadDiscountsDTOList();
            OnDataLoadComplete();
            log.LogMethodExit();
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                OnDataLoadStart();
                dgvDiscountedGamesDTOList.EndEdit();
                dgvDiscountsDTOList.EndEdit();
                dgvDiscountPurchaseCriteriaDTOList.EndEdit();
                dgvDiscountedProductsDTOList.EndEdit();
                SortableBindingList<DiscountsDTO> discountsDTOSortableList = (SortableBindingList<DiscountsDTO>)discountsDTOListBS.DataSource;
                bool error = false;
                if (discountsDTOSortableList == null)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(371));
                    log.LogMethodExit("discountsDTOSortableList == null");
                    return;
                }
                for (int i = 0; i < discountsDTOSortableList.Count; i++)
                {
                    try
                    {
                        using(UnitOfWork unitOfWork = new UnitOfWork())
                        {
                            unitOfWork.Begin();
                            DiscountsBL discountsBL = new DiscountsBL(executionContext, discountsDTOSortableList[i], unitOfWork);
                            discountsBL.Save();
                            unitOfWork.Commit();
                        }
                    }
                    catch (Semnox.Core.Utilities.ForeignKeyException ex)
                    {
                        error = true;
                        log.Error(ex.Message);
                        discountsDTOListBS.Position = i;
                        dgvDiscountsDTOList.Rows[i].Selected = true;
                        MessageBox.Show(utilities.MessageUtils.getMessage(1143));
                        break;
                    }
                    catch (ValidationException ex)
                    {
                        error = true;
                        log.Error(ex.Message);
                        discountsDTOListBS.Position = i;
                        dgvDiscountsDTOList.Rows[i].Selected = true;
                        MessageBox.Show(ex.GetAllValidationErrorMessages());
                    }
                    catch (Exception ex)
                    {
                        error = true;
                        log.Error("Error while saving discounts.");
                        discountsDTOListBS.Position = i;
                        dgvDiscountsDTOList.Rows[i].Selected = true;
                        MessageBox.Show(ex.Message);
                        break;
                    }
                }

                if (!error)
                {
                    await LoadDiscountsDTOList();
                }
                else
                {
                    dgvDiscountsDTOList.Update();
                    dgvDiscountsDTOList.Refresh();
                    dgvDiscountPurchaseCriteriaDTOList.Update();
                    dgvDiscountPurchaseCriteriaDTOList.Refresh();
                    dgvDiscountedGamesDTOList.Update();
                    dgvDiscountedGamesDTOList.Refresh();
                    dgvDiscountedProductsDTOList.Update();
                    dgvDiscountedProductsDTOList.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                OnDataLoadComplete();
            }
            
            log.LogMethodExit();
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                OnDataLoadStart();
                if (dgvDiscountsDTOList.SelectedRows.Count <= 0 &&
                dgvDiscountsDTOList.SelectedCells.Count <= 0)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(959));
                    log.Debug("Ends-btnDelete_Click() event by \"No rows selected. Please select the rows you want to delete and press delete..\" message ");
                    return;
                }
                bool rowsDeleted = false;
                bool confirmDelete = false;
                bool refreshFromDb = false;
                bool discountPurchaseCriteriaExists = false;
                bool discountedProductsExists = false;
                bool discountedGamesExists = false;
                if (dgvDiscountPurchaseCriteriaDTOList.SelectedCells.Count > 0)
                {
                    foreach (DataGridViewCell cell in dgvDiscountPurchaseCriteriaDTOList.SelectedCells)
                    {
                        dgvDiscountPurchaseCriteriaDTOList.Rows[cell.RowIndex].Selected = true;
                    }
                }
                if (dgvDiscountPurchaseCriteriaDTOList.SelectedRows.Count == 0 &&
                   dgvDiscountPurchaseCriteriaDTOList.RowCount > 0)
                {
                    for (int i = dgvDiscountPurchaseCriteriaDTOList.RowCount - 1; i >= 0; i--)
                    {
                        object isActiveValue = dgvDiscountPurchaseCriteriaDTOList.Rows[i].Cells[dgvDiscountPurchaseCriteriaDTOListIsActiveCheckBoxColumn.Index].Value;
                        if (isActiveValue != null && isActiveValue.Equals(true))
                        {
                            dgvDiscountPurchaseCriteriaDTOList.Rows[i].Selected = true;
                            break;
                        }
                    }
                }
                if (dgvDiscountPurchaseCriteriaDTOList.SelectedRows.Count > 0)
                {
                    foreach (DataGridViewRow row in dgvDiscountPurchaseCriteriaDTOList.SelectedRows)
                    {
                        if (row.Cells[dgvDiscountPurchaseCriteriaDTOListIsActiveCheckBoxColumn.Index].Value != null &&
                            bool.Equals(row.Cells[dgvDiscountPurchaseCriteriaDTOListIsActiveCheckBoxColumn.Index].Value, true))
                        {
                            discountPurchaseCriteriaExists = true;
                            if (row.Cells[dgvDiscountPurchaseCriteriaDTOListCriteriaIdTextBoxColumn.Index].Value != null &&
                                Convert.ToInt32(row.Cells[dgvDiscountPurchaseCriteriaDTOListCriteriaIdTextBoxColumn.Index].Value.ToString()) < 0)
                            {
                                dgvDiscountPurchaseCriteriaDTOList.Rows.RemoveAt(row.Index);
                                rowsDeleted = true;
                            }
                            else
                            {
                                if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactivation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                                {
                                    confirmDelete = true;
                                    refreshFromDb = true;
                                    List<DiscountPurchaseCriteriaDTO> discountPurchaseCriteriaDTOSortableList = (List<DiscountPurchaseCriteriaDTO>)discountPurchaseCriteriaDTOListBS.DataSource;
                                    DiscountPurchaseCriteriaDTO discountPurchaseCriteriaDTO = discountPurchaseCriteriaDTOSortableList[row.Index];
                                    discountPurchaseCriteriaDTO.IsActive = false;
                                }
                            }
                        }
                    }
                }
                if (discountPurchaseCriteriaExists == false)
                {
                    if (dgvDiscountedProductsDTOList.SelectedCells.Count > 0)
                    {
                        foreach (DataGridViewCell cell in dgvDiscountedProductsDTOList.SelectedCells)
                        {
                            dgvDiscountedProductsDTOList.Rows[cell.RowIndex].Selected = true;
                        }
                    }
                    if (dgvDiscountedProductsDTOList.SelectedRows.Count == 0 &&
                       dgvDiscountedProductsDTOList.RowCount > 0)
                    {
                        for (int i = dgvDiscountedProductsDTOList.RowCount - 1; i >= 0; i--)
                        {
                            object isActiveValue = dgvDiscountedProductsDTOList.Rows[i].Cells[dgvDiscountedProductsDTOListIsActiveCheckBoxColumn.Index].Value;
                            if (isActiveValue != null && isActiveValue.Equals(true))
                            {
                                dgvDiscountedProductsDTOList.Rows[i].Selected = true;
                                break;
                            }
                        }
                    }
                    if (dgvDiscountedProductsDTOList.SelectedRows.Count > 0)
                    {
                        foreach (DataGridViewRow row in dgvDiscountedProductsDTOList.SelectedRows)
                        {
                            if (row.Cells[dgvDiscountedProductsDTOListIsActiveCheckBoxColumn.Index].Value != null &&
                                bool.Equals(row.Cells[dgvDiscountedProductsDTOListIsActiveCheckBoxColumn.Index].Value, true))
                            {
                                discountedProductsExists = true;
                                if (row.Cells[dgvDiscountedProductsDTOListIdTextBoxColumn.Index].Value != null &&
                                    Convert.ToInt32(row.Cells[dgvDiscountedProductsDTOListIdTextBoxColumn.Index].Value.ToString()) < 0)
                                {
                                    dgvDiscountedProductsDTOList.Rows.RemoveAt(row.Index);
                                    rowsDeleted = true;
                                }
                                else
                                {
                                    if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                                    {
                                        confirmDelete = true;
                                        refreshFromDb = true;
                                        List<DiscountedProductsDTO> discountedProductsDTOSortableList = (List<DiscountedProductsDTO>)discountedProductsDTOListBS.DataSource;
                                        DiscountedProductsDTO discountedProductsDTO = discountedProductsDTOSortableList[row.Index];
                                        discountedProductsDTO.IsActive = false;
                                    }
                                }
                            }
                        }
                    }
                }
                if (discountPurchaseCriteriaExists == false && discountedProductsExists == false)
                {
                    if (dgvDiscountedGamesDTOList.SelectedCells.Count > 0)
                    {
                        foreach (DataGridViewCell cell in dgvDiscountedGamesDTOList.SelectedCells)
                        {
                            dgvDiscountedGamesDTOList.Rows[cell.RowIndex].Selected = true;
                        }
                    }
                    if (dgvDiscountedGamesDTOList.SelectedRows.Count == 0 &&
                       dgvDiscountedGamesDTOList.RowCount > 0)
                    {
                        for (int i = dgvDiscountedGamesDTOList.RowCount - 1; i >= 0; i--)
                        {
                            object isActiveValue = dgvDiscountedGamesDTOList.Rows[i].Cells[dgvDiscountedGamesDTOListIsActiveCheckBoxColumn.Index].Value;
                            if (isActiveValue != null && isActiveValue.Equals(true))
                            {
                                dgvDiscountedGamesDTOList.Rows[i].Selected = true;
                                break;
                            }
                        }
                    }
                    if (dgvDiscountedGamesDTOList.SelectedRows.Count > 0)
                    {
                        foreach (DataGridViewRow row in dgvDiscountedGamesDTOList.SelectedRows)
                        {
                            if (row.Cells[dgvDiscountedGamesDTOListIsActiveCheckBoxColumn.Index].Value != null &&
                                bool.Equals(row.Cells[dgvDiscountedGamesDTOListIsActiveCheckBoxColumn.Index].Value, true))
                            {
                                discountedGamesExists = true;
                                if (row.Cells[dgvDiscountedGamesDTOListIdTextBoxColumn.Index].Value != null &&
                                    Convert.ToInt32(row.Cells[dgvDiscountedGamesDTOListIdTextBoxColumn.Index].Value.ToString()) < 0)
                                {
                                    dgvDiscountedGamesDTOList.Rows.RemoveAt(row.Index);
                                    rowsDeleted = true;
                                }
                                else
                                {
                                    if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                                    {
                                        confirmDelete = true;
                                        refreshFromDb = true;
                                        List<DiscountedGamesDTO> discountedGamesDTOSortableList = (List<DiscountedGamesDTO>)discountedGamesDTOListBS.DataSource;
                                        DiscountedGamesDTO discountedGamesDTO = discountedGamesDTOSortableList[row.Index];
                                        discountedGamesDTO.IsActive = false;
                                    }
                                }
                            }
                        }
                    }
                }

                DiscountsDTO discountsDTO = null;
                if (discountPurchaseCriteriaExists == false &&
                    discountedProductsExists == false &&
                    discountedGamesExists == false)
                {
                    if (discountsDTOListBS.Current != null && discountsDTOListBS.Current is DiscountsDTO)
                    {
                        discountsDTO = discountsDTOListBS.Current as DiscountsDTO;
                        if (discountsDTO.DiscountId < 0)
                        {
                            SortableBindingList<DiscountsDTO> discountsDTOSortableList = (SortableBindingList<DiscountsDTO>)discountsDTOListBS.DataSource;
                            discountsDTOSortableList.Remove(discountsDTO);
                            rowsDeleted = true;
                        }
                        else
                        {
                            if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                            {
                                discountsDTO.IsActive = false;
                                confirmDelete = true;
                                refreshFromDb = true;
                            }
                        }
                    }
                }
                if (rowsDeleted)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(957));
                }
                if (refreshFromDb)
                {
                    if (discountsDTOListBS.Current != null && discountsDTOListBS.Current is DiscountsDTO)
                    {
                        discountsDTO = discountsDTOListBS.Current as DiscountsDTO;
                        using (UnitOfWork unitOfWork = new UnitOfWork())
                        {
                            unitOfWork.Begin();
                            DiscountsBL discountsBL = new DiscountsBL(executionContext, discountsDTO, unitOfWork);
                            discountsBL.Save();
                            unitOfWork.Commit();
                        }
                        await LoadDiscountsDTOList();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while deleting the record", ex);
                MessageBox.Show(ex.Message);
            }
            finally
            {
                OnDataLoadComplete();
            }
            log.LogMethodExit();
        }

        private void dgvDiscountsDTOList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            if (e.ColumnIndex != dgvDiscountsDTOListScheduleIdComboBoxColumn.Index)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(1144).Replace("&1", dgvDiscountsDTOList.Columns[e.ColumnIndex].HeaderText));
            }
            e.Cancel = true;
            log.LogMethodExit();
        }

        private void dgvDiscountPurchaseCriteriaDTOList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            MessageBox.Show(utilities.MessageUtils.getMessage(1144).Replace("&1", dgvDiscountPurchaseCriteriaDTOList.Columns[e.ColumnIndex].HeaderText));
            e.Cancel = true;
            log.LogMethodExit();
        }

        private void dgvDiscountedProductsDTOList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            MessageBox.Show(utilities.MessageUtils.getMessage(1144).Replace("&1", dgvDiscountedProductsDTOList.Columns[e.ColumnIndex].HeaderText));
            e.Cancel = true;
            log.LogMethodExit();
        }

        private void dgvDiscountedGamesDTOList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            MessageBox.Show(utilities.MessageUtils.getMessage(1144).Replace("&1", dgvDiscountedGamesDTOList.Columns[e.ColumnIndex].HeaderText));
            e.Cancel = true;
            log.LogMethodExit();
        }



        private async void lnkClearDiscountedProducts_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                OnDataLoadStart();
                if (discountsDTOListBS.Current != null &&
                discountsDTOListBS.Current is DiscountsDTO)
                {
                    DiscountsDTO discountsDTO = discountsDTOListBS.Current as DiscountsDTO;
                    if (discountsDTO.DiscountId != -1)
                    {
                        using (UnitOfWork unitOfWork = new UnitOfWork())
                        {
                            unitOfWork.Begin();
                            DiscountsBL discountsBL = new DiscountsBL(executionContext, discountsDTO, unitOfWork);
                            discountsBL.InactivateDiscountedProducts();
                            unitOfWork.Commit();
                        }
                        await LoadDiscountsDTOList();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                OnDataLoadComplete();
            }
            log.LogMethodExit();
        }

        private async void lnkSelectAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                OnDataLoadStart();
                if (discountsDTOListBS.Current != null &&
                discountsDTOListBS.Current is DiscountsDTO)
                {
                    DiscountsDTO discountsDTO = discountsDTOListBS.Current as DiscountsDTO;
                    if (discountsDTO.DiscountId != -1)
                    {
                        using (UnitOfWork unitOfWork = new UnitOfWork())
                        {
                            unitOfWork.Begin();
                            DiscountsBL discountsBL = new DiscountsBL(executionContext, discountsDTO, unitOfWork);
                            discountsBL.ClearDiscountedProducts();
                            unitOfWork.Commit();
                        }
                        await LoadDiscountsDTOList();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                OnDataLoadComplete();
            }
            log.LogMethodExit();
        }

        private void SetDiscountedProductsDTOListBS()
        {
            log.LogMethodEntry();
            if (discountsDTOListBS.Current != null && discountsDTOListBS.Current is DiscountsDTO)
            {
                DiscountsDTO discountsDTO = discountsDTOListBS.Current as DiscountsDTO;
                discountedProductsDTOListBS.DataSource = discountsDTO.DiscountedProductsDTOList;
            }
            log.LogMethodExit();
        }


        private async void lnkClearDiscountedGameplayGames_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                OnDataLoadStart();
                if (discountsDTOListBS.Current != null &&
                discountsDTOListBS.Current is DiscountsDTO)
                {
                    DiscountsDTO discountsDTO = discountsDTOListBS.Current as DiscountsDTO;
                    if (discountsDTO.DiscountId != -1)
                    {
                        using (UnitOfWork unitOfWork = new UnitOfWork())
                        {
                            unitOfWork.Begin();
                            DiscountsBL discountsBL = new DiscountsBL(executionContext, discountsDTO, unitOfWork);
                            discountsBL.InactivateDiscountedGames();
                            unitOfWork.Commit();
                        }
                        await LoadDiscountsDTOList();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                OnDataLoadComplete();
            }
            log.LogMethodExit();
        }

        private void dgvDiscountsDTOList_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            log.LogMethodEntry();
            if (e.ColumnIndex == dgvDiscountsDTOListDiscountPercentageTextBoxColumn.Index)
            {
                double s;
                if (e.FormattedValue != null && string.IsNullOrWhiteSpace(e.FormattedValue.ToString()) == false)
                {
                    if (double.TryParse(e.FormattedValue.ToString(), out s))
                    {
                        if (s > 100)
                        {
                            MessageBox.Show(utilities.MessageUtils.getMessage(563));
                            e.Cancel = true;
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        private void dgvDiscountsDTOList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvDiscountsDTOListMinimumCreditsTextBoxColumn.Index == e.ColumnIndex)
                {
                    if (dgvDiscountsDTOList.Rows[e.RowIndex].Cells[dgvDiscountsDTOListMinimumCreditsTextBoxColumn.Index].Value != null &&
                        !string.IsNullOrEmpty(dgvDiscountsDTOList.Rows[e.RowIndex].Cells[dgvDiscountsDTOListMinimumCreditsTextBoxColumn.Index].Value.ToString()) &&
                        dgvDiscountsDTOList.Rows[e.RowIndex].Cells[dgvDiscountsDTOListVariableDiscountsCheckBoxColumn.Index].Value.ToString() == "N")
                    {
                        if (dgvDiscountsDTOList.Rows[e.RowIndex].Cells[dgvDiscountsDTOListAutomaticApplyCheckBoxColumn.Index].Value.ToString() != "Y")
                            dgvDiscountsDTOList.Rows[e.RowIndex].Cells[dgvDiscountsDTOListAutomaticApplyCheckBoxColumn.Index].Value = "Y";
                        dgvDiscountsDTOList.Rows[e.RowIndex].Cells[dgvDiscountsDTOListAutomaticApplyCheckBoxColumn.Index].ReadOnly = true;
                    }
                    else if (dgvDiscountsDTOList.Rows[e.RowIndex].Cells[dgvDiscountsDTOListVariableDiscountsCheckBoxColumn.Index].Value.ToString() == "Y")
                    {
                        dgvDiscountsDTOList.Rows[e.RowIndex].Cells[dgvDiscountsDTOListAutomaticApplyCheckBoxColumn.Index].ReadOnly = true;
                    }
                    else
                    {
                        dgvDiscountsDTOList.Rows[e.RowIndex].Cells[dgvDiscountsDTOListAutomaticApplyCheckBoxColumn.Index].ReadOnly = false;
                    }
                }
            }
            log.LogMethodExit();
        }

        private void dgvDiscountsDTOList_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            if (e.ColumnIndex == dgvDiscountsDTOListAutomaticApplyCheckBoxColumn.Index)
            {
                if (dgvDiscountsDTOList.Rows[e.RowIndex].Cells[dgvDiscountsDTOListMinimumCreditsTextBoxColumn.Index].Value != null &&
                    !string.IsNullOrEmpty(dgvDiscountsDTOList.Rows[e.RowIndex].Cells[dgvDiscountsDTOListMinimumCreditsTextBoxColumn.Index].Value.ToString()) &&
                    dgvDiscountsDTOList.Rows[e.RowIndex].Cells[dgvDiscountsDTOListVariableDiscountsCheckBoxColumn.Index].Value.ToString() == "N")
                {
                    if (dgvDiscountsDTOList.Rows[e.RowIndex].Cells[dgvDiscountsDTOListAutomaticApplyCheckBoxColumn.Index].Value.ToString() != "Y")
                        dgvDiscountsDTOList.Rows[e.RowIndex].Cells[dgvDiscountsDTOListAutomaticApplyCheckBoxColumn.Index].Value = "Y";
                    dgvDiscountsDTOList.Rows[e.RowIndex].Cells[dgvDiscountsDTOListAutomaticApplyCheckBoxColumn.Index].ReadOnly = true;
                }
                else if (dgvDiscountsDTOList.Rows[e.RowIndex].Cells[dgvDiscountsDTOListVariableDiscountsCheckBoxColumn.Index].Value.ToString() == "Y")
                {
                    dgvDiscountsDTOList.Rows[e.RowIndex].Cells[dgvDiscountsDTOListAutomaticApplyCheckBoxColumn.Index].ReadOnly = true;
                }
                else
                {
                    dgvDiscountsDTOList.Rows[e.RowIndex].Cells[dgvDiscountsDTOListAutomaticApplyCheckBoxColumn.Index].ReadOnly = false;
                }
            }

            if (e.ColumnIndex == dgvDiscountsDTOListDiscountAmountTextBoxColumn.Index)
            {
                if (dgvDiscountsDTOList.Rows[e.RowIndex].Cells[dgvDiscountsDTOListVariableDiscountsCheckBoxColumn.Index].Value.ToString() == "Y")
                {
                    dgvDiscountsDTOList.Rows[e.RowIndex].Cells[dgvDiscountsDTOListDiscountAmountTextBoxColumn.Index].ReadOnly = true;
                }
            }

            if (e.ColumnIndex == dgvDiscountsDTOListDiscountPercentageTextBoxColumn.Index)
            {
                if (dgvDiscountsDTOList.Rows[e.RowIndex].Cells[dgvDiscountsDTOListVariableDiscountsCheckBoxColumn.Index].Value.ToString() == "Y")
                {
                    dgvDiscountsDTOList.Rows[e.RowIndex].Cells[dgvDiscountsDTOListDiscountPercentageTextBoxColumn.Index].ReadOnly = true;
                }
            }
            log.LogMethodExit();
        }

        private void ShowDiscountCouponsPopup(int index)
        {
            log.LogMethodEntry(index);
            if (discountsDTOListBS.DataSource != null && discountsDTOListBS.DataSource is SortableBindingList<DiscountsDTO>)
            {
                SortableBindingList<DiscountsDTO> discountsDTOList = discountsDTOListBS.DataSource as SortableBindingList<DiscountsDTO>;
                if (index >= 0 && index < discountsDTOList.Count)
                {
                    DiscountsDTO discountsDTO = discountsDTOList[index];
                    if (discountsDTO.DiscountId != -1)
                    {
                        DiscountCouponsUI discountCouponsUI = new DiscountCouponsUI(utilities, discountsDTO.DiscountId);
                        discountCouponsUI.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(673), utilities.MessageUtils.getMessage("Discount Save"));
                    }
                }
            }
            log.LogMethodExit();
        }

        private void dgvDiscountsDTOList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (e.ColumnIndex == dgvDiscountsDTOListCouponsButtonColumn.Index)
                {
                    if (dgvDiscountsDTOList[dgvDiscountsDTOListCouponMandatoryCheckBoxColumn.Index, e.RowIndex].Value.ToString() == "Y")
                    {
                        ShowDiscountCouponsPopup(e.RowIndex);
                    }
                }
                else if (e.ColumnIndex == dgvDiscountsDTOListVariableDiscountsCheckBoxColumn.Index)
                {
                    if (e.RowIndex > -1)
                    {
                        if (dgvDiscountsDTOList[dgvDiscountsDTOListVariableDiscountsCheckBoxColumn.Index, e.RowIndex].Value.ToString() == "N")
                        {
                            dgvDiscountsDTOList[dgvDiscountsDTOListDiscountPercentageTextBoxColumn.Index, e.RowIndex].Value = null;
                            dgvDiscountsDTOList[dgvDiscountsDTOListDiscountAmountTextBoxColumn.Index, e.RowIndex].Value = null;
                            dgvDiscountsDTOList.Rows[e.RowIndex].Cells[dgvDiscountsDTOListAutomaticApplyCheckBoxColumn.Index].Value = "N";
                            dgvDiscountsDTOList[dgvDiscountsDTOListVariableDiscountsCheckBoxColumn.Index, e.RowIndex].Value = "Y";

                            dgvDiscountsDTOList.Rows[e.RowIndex].Cells[dgvDiscountsDTOListDiscountPercentageTextBoxColumn.Index].ReadOnly = true;
                            dgvDiscountsDTOList.Rows[e.RowIndex].Cells[dgvDiscountsDTOListDiscountAmountTextBoxColumn.Index].ReadOnly = true;
                            dgvDiscountsDTOList.Rows[e.RowIndex].Cells[dgvDiscountsDTOListAutomaticApplyCheckBoxColumn.Index].ReadOnly = true;

                        }
                        else
                        {
                            dgvDiscountsDTOList.Rows[e.RowIndex].Cells[dgvDiscountsDTOListAutomaticApplyCheckBoxColumn.Index].ReadOnly = false;
                            dgvDiscountsDTOList.Rows[e.RowIndex].Cells[dgvDiscountsDTOListDiscountPercentageTextBoxColumn.Index].ReadOnly = false;
                            dgvDiscountsDTOList.Rows[e.RowIndex].Cells[dgvDiscountsDTOListDiscountAmountTextBoxColumn.Index].ReadOnly = false;
                            dgvDiscountsDTOList[dgvDiscountsDTOListVariableDiscountsCheckBoxColumn.Index, e.RowIndex].Value = "N";
                        }
                    }
                }
                else if (e.ColumnIndex == dgvDiscountsDTOListCouponMandatoryCheckBoxColumn.Index)
                {
                    if (e.RowIndex > -1)
                    {
                        if (dgvDiscountsDTOList[dgvDiscountsDTOListCouponMandatoryCheckBoxColumn.Index, e.RowIndex].Value.ToString() == "N")
                        {
                            dgvDiscountsDTOList[dgvDiscountsDTOListCouponMandatoryCheckBoxColumn.Index, e.RowIndex].Value = "Y";
                            if (Convert.ToInt32(dgvDiscountsDTOList[dgvDiscountsDTOListDiscountIdTextBoxColumn.Index, e.RowIndex].Value) != -1)
                            {
                                ShowDiscountCouponsPopup(e.RowIndex);
                            }
                        }
                        else
                        {
                            dgvDiscountsDTOList[dgvDiscountsDTOListCouponMandatoryCheckBoxColumn.Index, e.RowIndex].Value = "N";
                        }
                    }
                }
            }
            catch { }
            log.LogMethodExit();
        }

        private async void btnSchedule_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (discountsDTOListBS.Current != null && discountsDTOListBS.Current is DiscountsDTO)
            {
                OnDataLoadStart();
                DiscountsDTO discountsDTO = discountsDTOListBS.Current as DiscountsDTO;
                try
                {
                    using (UnitOfWork unitOfWork = new UnitOfWork())
                    {
                        unitOfWork.Begin();
                        DiscountsBL discountsBL = new DiscountsBL(executionContext, discountsDTO, unitOfWork);
                        discountsBL.Save();
                        unitOfWork.Commit();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    dgvDiscountsDTOList.Rows[discountsDTOListBS.Position].Selected = true;
                    MessageBox.Show(ex.Message);
                    return;
                }
                try
                {
                    DiscountScheduleUI discountScheduleUI = new DiscountScheduleUI(utilities, discountsDTO);
                    discountScheduleUI.ShowDialog();
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
                await LoadScheduleDTOList();
                await LoadDiscountsDTOList();
                OnDataLoadComplete();
                dgvDiscountsDTOList.Update();
                dgvDiscountsDTOList.Refresh();
            }
            log.LogMethodExit();
        }

        private void btnPublishToSite_Click(object sender, EventArgs e)//Starts: Modification on 14-Jul-2016 for adding publish to site feature 
        {
            log.LogMethodEntry();
            if (discountsDTOListBS.Current != null && discountsDTOListBS.Current is DiscountsDTO)
            {
                DiscountsDTO discountsDTO = discountsDTOListBS.Current as DiscountsDTO;
                try
                {
                    if (discountsDTO != null)
                    {
                        if (discountsDTO.DiscountId > 0)
                        {
                            Publish.PublishUI publishUI = new Publish.PublishUI(utilities, discountsDTO.DiscountId, "Discounts", discountsDTO.DiscountName);
                            publishUI.ShowDialog();
                        }
                        log.LogMethodExit();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, utilities.MessageUtils.getMessage("Discount Publish"));
                    log.Fatal("Ends-btnPublishToSite_Click() Event with exception. Exception: " + ex.ToString());
                }
            }
            log.LogMethodExit();
        }

        private void dgvDiscountPurchaseCriteriaDTOList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                object value = dgvDiscountPurchaseCriteriaDTOList.Rows[e.RowIndex].Cells[dgvDiscountPurchaseCriteriaDTOListProductIdComboBoxColumn.Index].Value;
                if (value != null && Convert.ToInt32(value) != -1)
                {
                    dgvDiscountPurchaseCriteriaDTOList.Rows[e.RowIndex].Cells[dgvDiscountPurchaseCriteriaDTOListCategoryIdButtonColumn.Index].ReadOnly = true;
                }
                else
                {
                    dgvDiscountPurchaseCriteriaDTOList.Rows[e.RowIndex].Cells[dgvDiscountPurchaseCriteriaDTOListCategoryIdButtonColumn.Index].ReadOnly = false;
                }
                value = dgvDiscountPurchaseCriteriaDTOList.Rows[e.RowIndex].Cells[dgvDiscountPurchaseCriteriaDTOListCategoryIdComboBoxColumn.Index].Value;
                if (value != null && Convert.ToInt32(value) != -1)
                {
                    dgvDiscountPurchaseCriteriaDTOList.Rows[e.RowIndex].Cells[dgvDiscountPurchaseCriteriaDTOListProductIdButtonColumn.Index].ReadOnly = true;
                }
                else
                {
                    dgvDiscountPurchaseCriteriaDTOList.Rows[e.RowIndex].Cells[dgvDiscountPurchaseCriteriaDTOListProductIdButtonColumn.Index].ReadOnly = false;
                }
            }
            log.LogMethodExit();
        }

        private void dgvDiscountPurchaseCriteriaDTOList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            log.LogMethodEntry();
            if (dgvDiscountPurchaseCriteriaDTOList.RowCount > 0)
            {
                for (int i = 0; i < dgvDiscountPurchaseCriteriaDTOList.RowCount; i++)
                {
                    object value = dgvDiscountPurchaseCriteriaDTOList.Rows[i].Cells[dgvDiscountPurchaseCriteriaDTOListProductIdComboBoxColumn.Index].Value;
                    if (value != null && Convert.ToInt32(value) != -1)
                    {
                        dgvDiscountPurchaseCriteriaDTOList.Rows[i].Cells[dgvDiscountPurchaseCriteriaDTOListCategoryIdButtonColumn.Index].ReadOnly = true;
                    }
                    else
                    {
                        dgvDiscountPurchaseCriteriaDTOList.Rows[i].Cells[dgvDiscountPurchaseCriteriaDTOListCategoryIdButtonColumn.Index].ReadOnly = false;
                    }
                    value = dgvDiscountPurchaseCriteriaDTOList.Rows[i].Cells[dgvDiscountPurchaseCriteriaDTOListCategoryIdComboBoxColumn.Index].Value;
                    if (value != null && Convert.ToInt32(value) != -1)
                    {
                        dgvDiscountPurchaseCriteriaDTOList.Rows[i].Cells[dgvDiscountPurchaseCriteriaDTOListProductIdButtonColumn.Index].ReadOnly = true;
                    }
                    else
                    {
                        dgvDiscountPurchaseCriteriaDTOList.Rows[i].Cells[dgvDiscountPurchaseCriteriaDTOListProductIdButtonColumn.Index].ReadOnly = false;
                    }
                }
            }
            log.LogMethodExit();
        }

        private void DiscountsSetupUI_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Dispose();
            log.LogMethodExit();
        }

        private void dgvDiscountedGamesDTOList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.RowIndex < 0 ||
                (e.ColumnIndex != dgvDiscountedGamesDTOListGameIdButtonColumn.Index))
            {
                log.LogMethodExit(null, "No Valid Column or row Selected");
                return;
            }
            if (dgvDiscountedGamesDTOList.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly)
            {
                log.LogMethodExit(null, "Non editable");
                return;
            }
            try
            {
                DataGridViewRow curRow = dgvDiscountedGamesDTOList.Rows[e.RowIndex];
                List<DiscountedGamesDTO> list = (dgvDiscountedGamesDTOList.DataSource as BindingSource).DataSource as List<DiscountedGamesDTO>;
                if (curRow.IsNewRow)
                {
                    DataGridViewCell cell = dgvDiscountedGamesDTOList.CurrentCell;
                    dgvDiscountedGamesDTOList.CurrentCell = curRow.Cells[dgvDiscountedGamesDTOListDiscountedCheckBoxColumn.Index];
                    bool value = dgvDiscountedGamesDTOList.BeginEdit(true);
                    dgvDiscountedGamesDTOList.NotifyCurrentCellDirty(true);
                }
                if (e.ColumnIndex == dgvDiscountedGamesDTOListGameIdButtonColumn.Index)
                {
                    GameDTO selectedGameDTO = null;
                    List<EntityPropertyDefintion> entityPropertyDefintionList = new List<EntityPropertyDefintion>()
                        {
                            new EntityPropertyDefintion("GameId", MessageContainerList.GetMessage(utilities.ExecutionContext,"Id"), false),
                            new EntityPropertyDefintion("GameName", MessageContainerList.GetMessage(utilities.ExecutionContext,"Name"), true),
                        };
                    using (GenericEntitySelectionUI<GameDTO> genericEntitySelectionUI = new GenericEntitySelectionUI<GameDTO>(utilities, MessageContainerList.GetMessage(utilities.ExecutionContext, "Games"), entityPropertyDefintionList, gameDTOList))
                    {
                        if (genericEntitySelectionUI.ShowDialog() == DialogResult.OK)
                        {
                            selectedGameDTO = genericEntitySelectionUI.SelectedValue;
                        }
                    }
                    if (selectedGameDTO != null && list.Count > e.RowIndex)
                    {
                        list[e.RowIndex].GameId = selectedGameDTO.GameId;
                    }
                }
                dgvDiscountedGamesDTOList.Refresh();
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while selecting game", ex);
            }
            log.LogMethodExit();
        }

        private void dgvDiscountPurchaseCriteriaDTOList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.RowIndex < 0 ||
                (e.ColumnIndex != dgvDiscountPurchaseCriteriaDTOListProductIdButtonColumn.Index &&
                e.ColumnIndex != dgvDiscountPurchaseCriteriaDTOListCategoryIdButtonColumn.Index &&
                e.ColumnIndex != dgvDiscountPurchaseCriteriaDTOListProductGroupIdButtonColumn.Index))
            {
                log.LogMethodExit(null, "No Valid Column or row Selected");
                return;
            } 
            
            try
            {
                OnDataLoadStart();
                DataGridViewRow curRow = dgvDiscountPurchaseCriteriaDTOList.Rows[e.RowIndex];
                List<DiscountPurchaseCriteriaDTO> list = (dgvDiscountPurchaseCriteriaDTOList.DataSource as BindingSource).DataSource as List<DiscountPurchaseCriteriaDTO>;
                if (curRow.IsNewRow)
                {
                    DataGridViewCell cell = dgvDiscountPurchaseCriteriaDTOList.CurrentCell;
                    dgvDiscountPurchaseCriteriaDTOList.CurrentCell = curRow.Cells[dgvDiscountPurchaseCriteriaDTOListMinQuantityTextBoxColumn.Index];
                    bool value = dgvDiscountPurchaseCriteriaDTOList.BeginEdit(true);
                    dgvDiscountPurchaseCriteriaDTOList.NotifyCurrentCellDirty(true);
                }
                if (e.ColumnIndex == dgvDiscountPurchaseCriteriaDTOListProductIdButtonColumn.Index)
                {
                    ProductsDTO selectedProductsDTO = null;
                    List<EntityPropertyDefintion> entityPropertyDefintionList = new List<EntityPropertyDefintion>()
                        {
                            new EntityPropertyDefintion("ProductId", MessageContainerList.GetMessage(utilities.ExecutionContext,"Id"), false),
                            new EntityPropertyDefintion("ProductName", MessageContainerList.GetMessage(utilities.ExecutionContext,"Name"), true),
                            new EntityPropertyDefintion("Description", MessageContainerList.GetMessage(utilities.ExecutionContext,"Description"), true),
                            new EntityPropertyDefintion("ActiveFlag", MessageContainerList.GetMessage(utilities.ExecutionContext,"Active?"), true),
                        };
                    using (GenericEntitySelectionUI<ProductsDTO> genericEntitySelectionUI = new GenericEntitySelectionUI<ProductsDTO>(utilities, MessageContainerList.GetMessage(utilities.ExecutionContext, "Products"), entityPropertyDefintionList, productsDTOList))
                    {
                        if (genericEntitySelectionUI.ShowDialog() == DialogResult.OK)
                        {
                            selectedProductsDTO = genericEntitySelectionUI.SelectedValue;
                        }
                    }
                    if (selectedProductsDTO != null && list.Count > e.RowIndex)
                    {
                        list[e.RowIndex].ProductId = selectedProductsDTO.ProductId;
                    }
                }
                else if (e.ColumnIndex == dgvDiscountPurchaseCriteriaDTOListProductGroupIdButtonColumn.Index)
                {
                    ProductGroupDTO selectedProductGroupDTO = null;
                    List<EntityPropertyDefintion> entityPropertyDefintionList = new List<EntityPropertyDefintion>()
                        {
                            new EntityPropertyDefintion("Id", MessageContainerList.GetMessage(utilities.ExecutionContext,"Id"), false),
                            new EntityPropertyDefintion("Name", MessageContainerList.GetMessage(utilities.ExecutionContext,"Name"), true),
                            new EntityPropertyDefintion("IsActive", MessageContainerList.GetMessage(utilities.ExecutionContext,"Active?"), true),
                        };
                    using (GenericEntitySelectionUI<ProductGroupDTO> genericEntitySelectionUI = new GenericEntitySelectionUI<ProductGroupDTO>(utilities, MessageContainerList.GetMessage(utilities.ExecutionContext, "Product Group"), entityPropertyDefintionList, productGroupDTOList))
                    {
                        if (genericEntitySelectionUI.ShowDialog() == DialogResult.OK)
                        {
                            selectedProductGroupDTO = genericEntitySelectionUI.SelectedValue;
                        }
                    }
                    if (selectedProductGroupDTO != null && list.Count > e.RowIndex)
                    {
                        list[e.RowIndex].ProductGroupId = selectedProductGroupDTO.Id;
                    }
                }
                else
                {
                    CategoryDTO selectedCategoryDTO = null;
                    List<EntityPropertyDefintion> entityPropertyDefintionList = new List<EntityPropertyDefintion>()
                        {
                            new EntityPropertyDefintion("CategoryId", MessageContainerList.GetMessage(utilities.ExecutionContext,"Id"), false),
                            new EntityPropertyDefintion("Name", MessageContainerList.GetMessage(utilities.ExecutionContext,"Name"), true),
                            new EntityPropertyDefintion("IsActive", MessageContainerList.GetMessage(utilities.ExecutionContext,"Active?"), true,null,false),
                        };
                    using (GenericEntitySelectionUI<CategoryDTO> genericEntitySelectionUI = new GenericEntitySelectionUI<CategoryDTO>(utilities, MessageContainerList.GetMessage(utilities.ExecutionContext, "Category List"), entityPropertyDefintionList, categoryDTOList))
                    {
                        if (genericEntitySelectionUI.ShowDialog() == DialogResult.OK)
                        {
                            selectedCategoryDTO = genericEntitySelectionUI.SelectedValue;
                        }
                    }
                    if (selectedCategoryDTO != null && list.Count > e.RowIndex)
                    {
                        list[e.RowIndex].CategoryId = selectedCategoryDTO.CategoryId;
                    }
                }
                dgvDiscountPurchaseCriteriaDTOList.Refresh();
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while selecting product and category", ex);
            }
            finally
            {
                OnDataLoadComplete();
            }
            log.LogMethodExit();
        }

        private void dgvDiscountedProductsDTOList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.RowIndex < 0 ||
                (e.ColumnIndex != dgvDiscountedProductsDTOListProductIdButtonColumn.Index &&
                e.ColumnIndex != dgvDiscountedProductsDTOListCategoryIdButtonColumn.Index &&
                e.ColumnIndex != dgvDiscountedProductsDTOListProductGroupIdButtonColumn.Index))
            {
                log.LogMethodExit(null, "No Valid Column or row Selected");
                return;
            }
            if (dgvDiscountedProductsDTOList.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly)
            {
                log.LogMethodExit(null, "Non editable");
                return;
            }
            try
            {
                DataGridViewRow curRow = dgvDiscountedProductsDTOList.Rows[e.RowIndex];
                List<DiscountedProductsDTO> list = (dgvDiscountedProductsDTOList.DataSource as BindingSource).DataSource as List<DiscountedProductsDTO>;
                if (curRow.IsNewRow)
                {
                    DataGridViewCell cell = dgvDiscountedProductsDTOList.CurrentCell;
                    dgvDiscountedProductsDTOList.CurrentCell = curRow.Cells[dgvDiscountedProductsDTOListQuantityTextBoxColumn.Index];
                    bool value = dgvDiscountedProductsDTOList.BeginEdit(true);
                    dgvDiscountedProductsDTOList.NotifyCurrentCellDirty(true);
                }
                if (e.ColumnIndex == dgvDiscountedProductsDTOListProductIdButtonColumn.Index)
                {
                    ProductsDTO selectedProductsDTO = null;
                    List<EntityPropertyDefintion> entityPropertyDefintionList = new List<EntityPropertyDefintion>()
                        {
                            new EntityPropertyDefintion("ProductId", MessageContainerList.GetMessage(utilities.ExecutionContext,"Id"), false),
                            new EntityPropertyDefintion("ProductName", MessageContainerList.GetMessage(utilities.ExecutionContext,"Name"), true),
                            new EntityPropertyDefintion("Description", MessageContainerList.GetMessage(utilities.ExecutionContext,"Description"), true),
                            new EntityPropertyDefintion("ActiveFlag", MessageContainerList.GetMessage(utilities.ExecutionContext,"Active?"), true),
                        };
                    using (GenericEntitySelectionUI<ProductsDTO> genericEntitySelectionUI = new GenericEntitySelectionUI<ProductsDTO>(utilities, MessageContainerList.GetMessage(utilities.ExecutionContext, "Products"), entityPropertyDefintionList, productsDTOList))
                    {
                        if (genericEntitySelectionUI.ShowDialog() == DialogResult.OK)
                        {
                            selectedProductsDTO = genericEntitySelectionUI.SelectedValue;
                        }
                    }
                    if (selectedProductsDTO != null && list.Count > e.RowIndex)
                    {
                        list[e.RowIndex].ProductId = selectedProductsDTO.ProductId;
                    }
                }
                else if (e.ColumnIndex == dgvDiscountedProductsDTOListProductGroupIdButtonColumn.Index)
                {
                    ProductGroupDTO selectedProductGroupDTO = null;
                    List<EntityPropertyDefintion> entityPropertyDefintionList = new List<EntityPropertyDefintion>()
                        {
                            new EntityPropertyDefintion("Id", MessageContainerList.GetMessage(utilities.ExecutionContext,"Id"), false),
                            new EntityPropertyDefintion("Name", MessageContainerList.GetMessage(utilities.ExecutionContext,"Name"), true),
                            new EntityPropertyDefintion("IsActive", MessageContainerList.GetMessage(utilities.ExecutionContext,"Active?"), true),
                        };
                    using (GenericEntitySelectionUI<ProductGroupDTO> genericEntitySelectionUI = new GenericEntitySelectionUI<ProductGroupDTO>(utilities, MessageContainerList.GetMessage(utilities.ExecutionContext, "Product Group"), entityPropertyDefintionList, productGroupDTOList))
                    {
                        if (genericEntitySelectionUI.ShowDialog() == DialogResult.OK)
                        {
                            selectedProductGroupDTO = genericEntitySelectionUI.SelectedValue;
                        }
                    }
                    if (selectedProductGroupDTO != null && list.Count > e.RowIndex)
                    {
                        list[e.RowIndex].ProductGroupId = selectedProductGroupDTO.Id;
                    }
                }
                else
                {
                    CategoryDTO selectedCategoryDTO = null;
                    List<EntityPropertyDefintion> entityPropertyDefintionList = new List<EntityPropertyDefintion>()
                        {
                            new EntityPropertyDefintion("CategoryId", MessageContainerList.GetMessage(utilities.ExecutionContext,"Id"), false),
                            new EntityPropertyDefintion("Name", MessageContainerList.GetMessage(utilities.ExecutionContext,"Name"), true),
                            new EntityPropertyDefintion("IsActive", MessageContainerList.GetMessage(utilities.ExecutionContext,"Active?"), true,null,false),
                        };
                    using (GenericEntitySelectionUI<CategoryDTO> genericEntitySelectionUI = new GenericEntitySelectionUI<CategoryDTO>(utilities, MessageContainerList.GetMessage(utilities.ExecutionContext, "Category List"), entityPropertyDefintionList, categoryDTOList))
                    {
                        if (genericEntitySelectionUI.ShowDialog() == DialogResult.OK)
                        {
                            selectedCategoryDTO = genericEntitySelectionUI.SelectedValue;
                        }
                    }
                    if (selectedCategoryDTO != null && list.Count > e.RowIndex)
                    {
                        list[e.RowIndex].CategoryId = selectedCategoryDTO.CategoryId;
                    }
                }
                dgvDiscountedProductsDTOList.Refresh();
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while selecting product and category", ex);
            }
            log.LogMethodExit();
        }

        private void UpdateUIElements()
        {
            log.LogMethodEntry();
            if (managementStudioSwitch.EnablProductModule)
            {
                dgvDiscountsDTOList.AllowUserToAddRows = true;
                dgvDiscountsDTOList.ReadOnly = false;
                dgvDiscountPurchaseCriteriaDTOList.AllowUserToAddRows = true;
                dgvDiscountPurchaseCriteriaDTOList.ReadOnly = false;
                dgvDiscountedProductsDTOList.AllowUserToAddRows = true;
                dgvDiscountedProductsDTOList.ReadOnly = false;
                dgvDiscountedGamesDTOList.AllowUserToAddRows = true;
                dgvDiscountedGamesDTOList.ReadOnly = false;
                btnSave.Enabled = true;
                btnDelete.Enabled = true;
                btnPublishToSite.Enabled = true;
                lnkClearDiscountedProducts.Enabled = true;
                lnkSelectAll.Enabled = true;
                lnkClearDiscountedGameplayGames.Enabled = true;
            }
            else
            {
                dgvDiscountsDTOList.AllowUserToAddRows = false;
                dgvDiscountsDTOList.ReadOnly = true;
                dgvDiscountPurchaseCriteriaDTOList.AllowUserToAddRows = false;
                dgvDiscountPurchaseCriteriaDTOList.ReadOnly = true;
                dgvDiscountedProductsDTOList.AllowUserToAddRows = false;
                dgvDiscountedProductsDTOList.ReadOnly = true;
                dgvDiscountedGamesDTOList.AllowUserToAddRows = false;
                dgvDiscountedGamesDTOList.ReadOnly = true;
                btnDelete.Enabled = false;
                btnSave.Enabled = false;
                btnPublishToSite.Enabled = false;
                lnkClearDiscountedProducts.Enabled = false;
                lnkSelectAll.Enabled = false;
                lnkClearDiscountedGameplayGames.Enabled = false;
            }
            log.LogMethodExit();
        }
    }
}
