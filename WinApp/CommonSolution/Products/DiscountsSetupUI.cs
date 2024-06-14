using Semnox.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.Discounts;
using Semnox.Parafait.Product;
using Semnox.Parafait.Category;
using Semnox.Parafait.SortableBindingList;

namespace Semnox.Parafait.Products
{
    /// <summary>
    /// DiscountsSetupUI Class.
    /// </summary>
    public partial class DiscountsSetupUI : Form
    {
        Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        string discountType = DiscountsBL.DISCOUNT_TYPE_TRANSACTION;
        Dictionary<int, ProductsDTO> productsDTODictionary;
        Dictionary<int, CategoryDTO> categoryDTODictionary;
        /// <summary>
        /// Constructor of DiscountsSetupUI class.
        /// </summary>
        /// <param name="utilities">Parafait Utilities</param>
        public DiscountsSetupUI(Utilities utilities)
        {
            log.Debug("Starts-DiscountsSetupUI() parameterized constructor.");
            InitializeComponent();
            this.utilities = utilities;
            utilities.setupDataGridProperties(ref dgvDiscountsDTOList);
            utilities.setupDataGridProperties(ref dgvDiscountPurchaseCriteriaDTOList);
            utilities.setupDataGridProperties(ref dgvDiscountedProductsDTOList);
            utilities.setupDataGridProperties(ref dgvDiscountedGamesDTOList);
            if (utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
            utilities.setLanguage(this);
            log.Debug("Ends-DiscountsSetupUI() parameterized constructor.");
        }

        private void DiscountsSetupUI_Load(object sender, EventArgs e)
        {
            log.Debug("Starts-tcDiscountsDTOList_Selected() Event.");
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
            tcDiscountsPurchaseCriteriaAndProducts.Appearance = TabAppearance.FlatButtons;
            tcDiscountsPurchaseCriteriaAndProducts.ItemSize = new Size(0, 1);
            tcDiscountsPurchaseCriteriaAndProducts.SizeMode = TabSizeMode.Fixed;
            RefreshData();
            log.Debug("Ends-tcDiscountsDTOList_Selected() Event.");
        }

        private void RefreshData()
        {
            log.Debug("Starts-RefreshData() method.");
            LoadProductsDTOList();
            LoadTransactionProfileList();
            LoadCategoryDTOList();
            LoadScheduleDTOList();
            LoadGamesDTOList();
            LoadDiscountsDTOList();
            log.Debug("Ends-RefreshData() Method");
        }

        private void LoadTransactionProfileList()
        {
            log.Debug("Starts-LoadTransactionProfileList() method.");
            TransactionProfileListBL transactionProfileListBL = new TransactionProfileListBL();
            List<KeyValuePair<TransactionProfileDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TransactionProfileDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<TransactionProfileDTO.SearchByParameters, string>(TransactionProfileDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            List<TransactionProfileDTO> transactionProfileDTOList = transactionProfileListBL.GetTransactionProfileDTOList(searchParameters);
            if(transactionProfileDTOList == null)
            {
                transactionProfileDTOList = new List<TransactionProfileDTO>();
            }
            transactionProfileDTOList.Insert(0, new TransactionProfileDTO());
            transactionProfileDTOList[0].ProfileName = "Select";

            dgvDiscountsDTOListTransactionProfileIdComboBoxColumn.DisplayMember = "ProfileName";
            dgvDiscountsDTOListTransactionProfileIdComboBoxColumn.ValueMember = "TransactionProfileId";
            dgvDiscountsDTOListTransactionProfileIdComboBoxColumn.DataSource = transactionProfileDTOList;

            log.Debug("Ends-LoadTransactionProfileList() Method");
        }

        private void LoadGamesDTOList()
        {
            log.Debug("Starts-LoadGamesDTOList() method.");
            GameList gameList = new GameList();
            List<GameDTO> gameDTOList; 
            List<KeyValuePair<GameDTO.SearchByGameParameters, string>> searchParameters = new List<KeyValuePair<GameDTO.SearchByGameParameters, string>>();
            searchParameters.Add(new KeyValuePair<GameDTO.SearchByGameParameters, string>(GameDTO.SearchByGameParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            gameDTOList = gameList.GetGameList(searchParameters);
            if (gameDTOList == null)
            {
                gameDTOList = new List<GameDTO>();
            }

            gameDTOList.Insert(0, new GameDTO());
            gameDTOList[0].GameId = -1;
            gameDTOList[0].GameName = "-All-";
            
            dgvDiscountedGamesDTOListGameIdComboBoxColumn.DisplayMember = "GameName";
            dgvDiscountedGamesDTOListGameIdComboBoxColumn.ValueMember = "GameId";
            dgvDiscountedGamesDTOListGameIdComboBoxColumn.DataSource = gameDTOList;

            
            dgvDiscountedGamesDTOListGameIdComboBoxColumn.DisplayMember = "GameName";
            dgvDiscountedGamesDTOListGameIdComboBoxColumn.ValueMember = "GameId";
            dgvDiscountedGamesDTOListGameIdComboBoxColumn.DataSource = gameDTOList;
            log.Debug("Ends-LoadGamesDTOList() Method");
        }

        private void LoadProductsDTOList()
        {
            log.Debug("Starts-LoadProductsDTOList() method.");
            Products products = new Products();
            List<ProductsDTO> productsDTOList;
            ProductsFilterParams productsFilterParams = new ProductsFilterParams();
            //productsFilterParams.IsActive = "Y";
            productsFilterParams.SiteId = machineUserContext.GetSiteId();
            productsDTOList = products.GetProductDTOList(productsFilterParams);
            productsDTODictionary = new Dictionary<int, ProductsDTO>();
            if (productsDTOList == null)
            {
                productsDTOList = new List<ProductsDTO>();
            }
            else
            {
                for (int i = 0; i < productsDTOList.Count; i++)
                {
                    productsDTODictionary.Add(productsDTOList[i].ProductId, productsDTOList[i]);
                }
            }
            productsDTOList.Insert(0, new ProductsDTO());
            productsDTOList[0].ProductId = -1;
            productsDTOList[0].ProductName = "-All-";


            
            dgvDiscountPurchaseCriteriaDTOListProductIdComboBoxColumn.DisplayMember = "ProductName";
            dgvDiscountPurchaseCriteriaDTOListProductIdComboBoxColumn.ValueMember = "ProductId";
            dgvDiscountPurchaseCriteriaDTOListProductIdComboBoxColumn.DataSource = productsDTOList;

            
            dgvDiscountedProductsDTOListProductIdComboBoxColumn.DisplayMember = "ProductName";
            dgvDiscountedProductsDTOListProductIdComboBoxColumn.ValueMember = "ProductId";
            dgvDiscountedProductsDTOListProductIdComboBoxColumn.DataSource = productsDTOList;

            log.Debug("Ends-LoadProductsDTOList() Method");
        }

        private void LoadCategoryDTOList()
        {
            log.Debug("Starts-LoadCategoryDTOList() method.");
            CategoryList categoryList = new CategoryList();
            List<CategoryDTO> categoryDTOList;
            List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>> searchParameters = new List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>>();
            //searchParameters.Add(new KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>(CategoryDTO.SearchByCategoryParameters.IS_ACTIVE, "Y"));
            searchParameters.Add(new KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>(CategoryDTO.SearchByCategoryParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            categoryDTOList = categoryList.GetAllCategory(searchParameters);
            categoryDTODictionary = new Dictionary<int, CategoryDTO>();
            if (categoryDTOList == null)
            {
                categoryDTOList = new List<CategoryDTO>();
            }
            else
            {
                for (int i = 0; i < categoryDTOList.Count; i++)
                {
                    categoryDTODictionary.Add(categoryDTOList[i].CategoryId, categoryDTOList[i]);
                }
            }
            categoryDTOList.Insert(0, new CategoryDTO());
            categoryDTOList[0].CategoryId = -1;
            categoryDTOList[0].Name = "-All-";
            
            dgvDiscountPurchaseCriteriaDTOListCategoryIdComboBoxColumn.DisplayMember = "Name";
            dgvDiscountPurchaseCriteriaDTOListCategoryIdComboBoxColumn.ValueMember = "CategoryId";
            dgvDiscountPurchaseCriteriaDTOListCategoryIdComboBoxColumn.DataSource = categoryDTOList;

            
            dgvDiscountedProductsDTOListCategoryIdComboBoxColumn.DisplayMember = "Name";
            dgvDiscountedProductsDTOListCategoryIdComboBoxColumn.ValueMember = "CategoryId";
            dgvDiscountedProductsDTOListCategoryIdComboBoxColumn.DataSource = categoryDTOList;

            log.Debug("Ends-LoadCategoryDTOList() Method");
        }

        private void LoadScheduleDTOList()
        {
            log.Debug("Starts-LoadScheduleDTOList() method.");
            ScheduleList ScheduleList = new ScheduleList();
            List<ScheduleDTO> ScheduleDTOList;
            List<KeyValuePair<ScheduleDTO.SearchByScheduleParameters, string>> searchParameters = new List<KeyValuePair<ScheduleDTO.SearchByScheduleParameters, string>>();
            //searchParameters.Add(new KeyValuePair<ScheduleDTO.SearchByScheduleParameters, string>(ScheduleDTO.SearchByScheduleParameters.IS_ACTIVE, "1"));
            searchParameters.Add(new KeyValuePair<ScheduleDTO.SearchByScheduleParameters, string>(ScheduleDTO.SearchByScheduleParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            ScheduleDTOList = ScheduleList.GetAllSchedule(searchParameters);
            if (ScheduleDTOList == null)
            {
                ScheduleDTOList = new List<ScheduleDTO>();
            }
            ScheduleDTOList.Insert(0, new ScheduleDTO());
            ScheduleDTOList[0].ScheduleId = -1;
            ScheduleDTOList[0].ScheduleName = "";
            
            dgvDiscountsDTOListScheduleIdComboBoxColumn.DisplayMember = "ScheduleName";
            dgvDiscountsDTOListScheduleIdComboBoxColumn.ValueMember = "ScheduleId";
            dgvDiscountsDTOListScheduleIdComboBoxColumn.DataSource = ScheduleDTOList;

            log.Debug("Ends-LoadScheduleDTOList() Method");
        }

        private void LoadDiscountsDTOList()
        {
            log.Debug("Starts-LoadDiscountsDTOList() method.");
            DiscountsListBL discountsListBL = new DiscountsListBL();
            List<KeyValuePair<DiscountsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<DiscountsDTO.SearchByParameters, string>>();
            if (chbShowActiveEntries.Checked)
            {
                searchParameters.Add(new KeyValuePair<DiscountsDTO.SearchByParameters, string>(DiscountsDTO.SearchByParameters.ACTIVE_FLAG, "Y"));
            }
            searchParameters.Add(new KeyValuePair<DiscountsDTO.SearchByParameters, string>(DiscountsDTO.SearchByParameters.DISCOUNT_NAME, txtName.Text));
            searchParameters.Add(new KeyValuePair<DiscountsDTO.SearchByParameters, string>(DiscountsDTO.SearchByParameters.DISCOUNT_TYPE, discountType));
            searchParameters.Add(new KeyValuePair<DiscountsDTO.SearchByParameters, string>(DiscountsDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            List<DiscountsDTO> discountsDTOList = discountsListBL.GetDiscountsDTOList(searchParameters, true, chbShowActiveEntries.Checked);
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
            log.Debug("Ends-LoadDiscountsDTOList() Method");
        }

        private void tcDiscountsDTOList_Selected(object sender, TabControlEventArgs e)
        {
            log.Debug("Starts-tcDiscountsDTOList_Selected() Event.");
            bool showColumns = true;
            if (tcDiscountsDTOList.SelectedTab == transactionDiscountsTab)
            {
                showColumns = true;
                discountType = DiscountsBL.DISCOUNT_TYPE_TRANSACTION;
                grpbxDiscountedProducts.Visible = true;
                tcDiscountsPurchaseCriteriaAndProducts.SelectedTab = tpProductPurchaseCriteriaAndDiscountedProducts;
            }
            else if (tcDiscountsDTOList.SelectedTab == gamePlayDiscountsTab)
            {
                showColumns = false;
                discountType = DiscountsBL.DISCOUNT_TYPE_GAMEPLAY;
                tcDiscountsPurchaseCriteriaAndProducts.SelectedTab = tpDiscountedGames;
            }
            else if (tcDiscountsDTOList.SelectedTab == loyaltyDiscountsTab)
            {
                showColumns = false;
                discountType = DiscountsBL.DISCOUNT_TYPE_LOYALTY;
                tcDiscountsPurchaseCriteriaAndProducts.SelectedTab = tpDiscountedGames;
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
            LoadDiscountsDTOList();
            log.Debug("Ends-tcDiscountsDTOList_Selected() Event.");
        }

        private void discountsDTOListBS_AddingNew(object sender, AddingNewEventArgs e)
        {
            log.Debug("Starts-discountsDTOListBS_AddingNew() Event.");
            DiscountsDTO discountsDTO = new DiscountsDTO();
            discountsDTO.DiscountType = discountType;
            if (discountType == DiscountsBL.DISCOUNT_TYPE_TRANSACTION)
            {
                discountsDTO.DisplayInPOS = "Y";
            }
            e.NewObject = discountsDTO;
            log.Debug("Ends-discountsDTOListBS_AddingNew() Event.");
        }

        private void discountsDTOListBS_CurrentChanged(object sender, EventArgs e)
        {
            log.Debug("Starts-discountsDTOListBS_CurrentChanged() Event.");
            bool disableDiscountedProductsDiscountPercentage = true;
            bool disableDiscountedProductsDiscountAmount = true;
            if (discountsDTOListBS.Current != null && discountsDTOListBS.Current is DiscountsDTO)
            {
                DiscountsDTO discountsDTO = discountsDTOListBS.Current as DiscountsDTO;
                SortableBindingList<DiscountedGamesDTO> discountedGamesDTOList;
                if (discountsDTO.DiscountedGamesDTOList != null)
                {
                    discountedGamesDTOList = new SortableBindingList<DiscountedGamesDTO>(discountsDTO.DiscountedGamesDTOList);
                }
                else
                {
                    discountedGamesDTOList = new SortableBindingList<DiscountedGamesDTO>();
                }
                discountedGamesDTOListBS.DataSource = discountedGamesDTOList;
                SortableBindingList<DiscountPurchaseCriteriaDTO> discountPurchaseCriteriaDTOList;
                if (discountsDTO.DiscountPurchaseCriteriaDTOList != null)
                {
                    discountPurchaseCriteriaDTOList = new SortableBindingList<DiscountPurchaseCriteriaDTO>(discountsDTO.DiscountPurchaseCriteriaDTOList);
                }
                else
                {
                    discountPurchaseCriteriaDTOList = new SortableBindingList<DiscountPurchaseCriteriaDTO>();
                }
                discountPurchaseCriteriaDTOListBS.DataSource = discountPurchaseCriteriaDTOList;
                if (discountsDTO.DiscountPercentage > 0)
                {
                    disableDiscountedProductsDiscountPercentage = false;
                }
                if (discountsDTO.DiscountAmount > 0)
                {
                    disableDiscountedProductsDiscountAmount = false;
                }
                SetDiscountedProductsDTOListBS();
            }
            dgvDiscountedProductsDTOListDiscountPercentageTextBoxColumn.ReadOnly = disableDiscountedProductsDiscountPercentage;
            dgvDiscountedProductsDTOListDiscountAmountTextBoxColumn.ReadOnly = disableDiscountedProductsDiscountAmount;
            log.Debug("Ends-discountsDTOListBS_CurrentChanged() Event.");
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnClose_Click() Event.");
            chbShowActiveEntries.Checked = true;
            txtName.ResetText();
            discountedGamesDTOListBS.DataSource = new SortableBindingList<DiscountedGamesDTO>();
            discountPurchaseCriteriaDTOListBS.DataSource = new SortableBindingList<DiscountPurchaseCriteriaDTO>();
            discountedProductsDTOListBS.DataSource = new SortableBindingList<DiscountedProductsDTO>();

            RefreshData();
            log.Debug("Ends-btnClose_Click() Event.");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnClose_Click() Event.");
            this.Close();
            log.Debug("Ends-btnClose_Click() Event.");
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnSearch_Click() Event.");
            LoadDiscountsDTOList();
            log.Debug("Ends-btnSearch_Click() Event.");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnSave_Click() Discounts.");
            dgvDiscountedGamesDTOList.EndEdit();
            dgvDiscountsDTOList.EndEdit();
            dgvDiscountPurchaseCriteriaDTOList.EndEdit();
            dgvDiscountedProductsDTOList.EndEdit();
            UpdateCurrentDiscountDTO();
            SortableBindingList<DiscountsDTO> discountsDTOSortableList = (SortableBindingList<DiscountsDTO>)discountsDTOListBS.DataSource;
            string message;
            DiscountsBL discountsBL;
            bool error = false;
            if (discountsDTOSortableList != null)
            {
                for (int i = 0; i < discountsDTOSortableList.Count; i++)
                {
                    message = ValidateDiscountsDTO(discountsDTOSortableList[i]);
                    if (string.IsNullOrEmpty(message))
                    {
                        if (discountsDTOSortableList[i].DiscountedGamesDTOList != null)
                        {
                            for (int j = 0; j < discountsDTOSortableList[i].DiscountedGamesDTOList.Count; j++)
                            {
                                message = ValidateDiscountedGamesDTO(discountsDTOSortableList[i].DiscountedGamesDTOList[j]);
                                if (string.IsNullOrEmpty(message) == false)
                                {
                                    discountsDTOListBS.Position = i;
                                    dgvDiscountsDTOList.Rows[i].Selected = true;
                                    dgvDiscountedGamesDTOList.Rows[j].Selected = true;
                                    MessageBox.Show(message);
                                    error = true;
                                    break;
                                }
                            }
                        }
                        if (discountsDTOSortableList[i].DiscountedProductsDTOList != null)
                        {
                            for (int j = 0; j < discountsDTOSortableList[i].DiscountedProductsDTOList.Count; j++)
                            {
                                message = ValidateDiscountedProductsDTO(discountsDTOSortableList[i].DiscountedProductsDTOList[j]);
                                if (string.IsNullOrEmpty(message) == false)
                                {
                                    discountsDTOListBS.Position = i;
                                    dgvDiscountsDTOList.Rows[i].Selected = true;
                                    dgvDiscountedProductsDTOList.Rows[j].Selected = true;
                                    MessageBox.Show(message);
                                    error = true;
                                    break;
                                }
                            }
                        }
                        if (discountsDTOSortableList[i].DiscountPurchaseCriteriaDTOList != null)
                        {
                            for (int j = 0; j < discountsDTOSortableList[i].DiscountPurchaseCriteriaDTOList.Count; j++)
                            {
                                message = ValidateDiscountPurchaseCriteriaDTO(discountsDTOSortableList[i].DiscountPurchaseCriteriaDTOList[j]);
                                if (string.IsNullOrEmpty(message) == false)
                                {
                                    discountsDTOListBS.Position = i;
                                    dgvDiscountsDTOList.Rows[i].Selected = true;
                                    dgvDiscountPurchaseCriteriaDTOList.Rows[j].Selected = true;
                                    MessageBox.Show(message);
                                    error = true;
                                    break;
                                }
                            }
                        }
                        if (error == false)
                        {
                            try
                            {
                                if(discountsDTOSortableList[i].AutomaticApply == "Y")
                                {
                                    if(discountsDTOSortableList[i].RemarksMandatory == "Y")
                                    {
                                        discountsDTOSortableList[i].RemarksMandatory = "N";
                                    }
                                    if(discountsDTOSortableList[i].ManagerApprovalRequired == "Y")
                                    {
                                        discountsDTOSortableList[i].ManagerApprovalRequired = "N";
                                    }
                                }
                                discountsBL = new DiscountsBL(discountsDTOSortableList[i]);
                                discountsBL.Save();
                            }
                            catch (ForeignKeyException ex)
                            {
                                error = true;
                                log.Error(ex.Message);
                                discountsDTOListBS.Position = i;
                                dgvDiscountsDTOList.Rows[i].Selected = true;
                                MessageBox.Show(utilities.MessageUtils.getMessage(1143));
                                break;
                            }
                            catch (InvalidDiscountAmountException ex)
                            {
                                error = true;
                                log.Error(ex.Message);
                                discountsDTOListBS.Position = i;
                                dgvDiscountsDTOList.Rows[i].Selected = true;
                                MessageBox.Show(utilities.MessageUtils.getMessage(1223));
                            }
                            catch (InvalidDiscountPercentageException ex)
                            {
                                error = true;
                                log.Error(ex.Message);
                                discountsDTOListBS.Position = i;
                                dgvDiscountsDTOList.Rows[i].Selected = true;
                                MessageBox.Show(utilities.MessageUtils.getMessage(1224));
                            }
                            catch (Exception)
                            {
                                error = true;
                                log.Error("Error while saving discounts.");
                                discountsDTOListBS.Position = i;
                                dgvDiscountsDTOList.Rows[i].Selected = true;
                                MessageBox.Show(utilities.MessageUtils.getMessage(718));
                                break;
                            }
                        }
                    }
                    else
                    {
                        error = true;
                        discountsDTOListBS.Position = i;
                        dgvDiscountsDTOList.Rows[i].Selected = true;
                        MessageBox.Show(message);
                        break;
                    }

                }
            }
            else
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(371));
            }
            if (!error)
            {
                btnSearch.PerformClick();
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
            log.Debug("Ends-btnSave_Click() Discounts.");
        }

        private void UpdateCurrentDiscountDTO()
        {
            if (discountsDTOListBS.Current != null && discountsDTOListBS.Current is DiscountsDTO)
            {
                DiscountsDTO discountsDTO = discountsDTOListBS.Current as DiscountsDTO;
                if (discountPurchaseCriteriaDTOListBS.DataSource != null && discountPurchaseCriteriaDTOListBS.DataSource is SortableBindingList<DiscountPurchaseCriteriaDTO>)
                {
                    SortableBindingList<DiscountPurchaseCriteriaDTO> discountPurchaseCriteriaDTOSBList =
                        discountPurchaseCriteriaDTOListBS.DataSource as SortableBindingList<DiscountPurchaseCriteriaDTO>;
                    if (discountPurchaseCriteriaDTOSBList.Count > 0)
                    {
                        discountsDTO.DiscountPurchaseCriteriaDTOList = new List<DiscountPurchaseCriteriaDTO>(discountPurchaseCriteriaDTOSBList);
                    }
                    else
                    {
                        discountsDTO.DiscountPurchaseCriteriaDTOList = null;
                    }
                }
            }
        }

        private string ValidateDiscountsDTO(DiscountsDTO discountsDTO)
        {
            log.Debug("Starts-ValidateDiscountsDTO(discountsDTO) method.");
            string returnValue = string.Empty;
            if (string.IsNullOrEmpty(discountsDTO.DiscountName))
            {
                return utilities.MessageUtils.getMessage(1144).Replace("&1", dgvDiscountsDTOListDiscountNameTextBoxColumn.HeaderText);
            }
            if (discountsDTO.DiscountPercentage < 0)
            {
                return utilities.MessageUtils.getMessage(1144).Replace("&1", dgvDiscountsDTOListDiscountPercentageTextBoxColumn.HeaderText);
            }
            if (discountsDTO.DiscountAmount < 0)
            {
                return utilities.MessageUtils.getMessage(1144).Replace("&1", dgvDiscountsDTOListDiscountAmountTextBoxColumn.HeaderText);
            }
            if (discountsDTO.MinimumCredits < 0)
            {
                return utilities.MessageUtils.getMessage(1144).Replace("&1", dgvDiscountsDTOListMinimumCreditsTextBoxColumn.HeaderText);
            }
            if (discountsDTO.MinimumSaleAmount < 0)
            {
                return utilities.MessageUtils.getMessage(1144).Replace("&1", dgvDiscountsDTOListMinimumSaleAmountTextBoxColumn.HeaderText);
            }
            if (discountsDTO.SortOrder < 0)
            {
                return utilities.MessageUtils.getMessage(1144).Replace("&1", dgvDiscountsDTOListSortOrderTextBoxColumn.HeaderText);
            }
            if (discountType == DiscountsBL.DISCOUNT_TYPE_TRANSACTION)
            {
                if (discountsDTO.VariableDiscounts == "N" &&
                (discountsDTO.DiscountAmount == null || discountsDTO.DiscountAmount == 0) &&
                (discountsDTO.DiscountPercentage == null || discountsDTO.DiscountPercentage == 0))
                {
                    return utilities.MessageUtils.getMessage(611);
                }
            }
            else
            {
                if (discountsDTO.DiscountPercentage == null || discountsDTO.DiscountPercentage == 0)
                {
                    return utilities.MessageUtils.getMessage(1144).Replace("&1", dgvDiscountsDTOListDiscountPercentageTextBoxColumn.HeaderText);
                }
            }
            if (discountsDTO.DiscountedProductsDTOList != null)
            {
                double discountedProductsAmount = 0;
                double discountAmount = (discountsDTO.DiscountAmount == null ? 0 : (double)discountsDTO.DiscountAmount);
                double discountPercentage = (discountsDTO.DiscountPercentage == null ? 0 : (double)discountsDTO.DiscountPercentage);
                double maxProductDiscountPercentage = 0;
                foreach (var discountedProductsDTO in discountsDTO.DiscountedProductsDTOList)
                {
                    if (discountedProductsDTO.IsActive == "Y" &&
                        discountedProductsDTO.Discounted == "Y")
                    {
                        if (discountedProductsDTO.DiscountAmount != null &&
                            discountedProductsDTO.DiscountAmount > 0)
                        {
                            discountedProductsAmount += (double)discountedProductsDTO.DiscountAmount;
                        }
                        if (discountedProductsDTO.DiscountPercentage != null &&
                           discountedProductsDTO.DiscountPercentage > maxProductDiscountPercentage)
                        {
                            maxProductDiscountPercentage = (double)discountedProductsDTO.DiscountPercentage;
                        }
                    }
                }
                if (discountedProductsAmount != 0 && discountedProductsAmount != discountAmount)
                {
                    return utilities.MessageUtils.getMessage(1223);
                }
                if (maxProductDiscountPercentage > discountPercentage)
                {
                    return utilities.MessageUtils.getMessage(1224);
                }
            }
            log.Debug("Ends-ValidateDiscountsDTO(discountsDTO) method.");
            return returnValue;
        }

        private string ValidateDiscountPurchaseCriteriaDTO(DiscountPurchaseCriteriaDTO discountPurchaseCriteriaDTO)
        {
            log.Debug("Starts-ValidateDiscountPurchaseCriteriaDTO(discountPurchaseCriteriaDTO) method.");
            string returnValue = string.Empty;
            if (discountPurchaseCriteriaDTO.ProductId == -1 && discountPurchaseCriteriaDTO.CategoryId == -1)
            {
                returnValue = utilities.MessageUtils.getMessage(1144).Replace("&1", dgvDiscountPurchaseCriteriaDTOListProductIdComboBoxColumn.HeaderText);
            }
            if (discountPurchaseCriteriaDTO.MinQuantity < 0)
            {
                returnValue = utilities.MessageUtils.getMessage(1144).Replace("&1", dgvDiscountPurchaseCriteriaDTOListMinQuantityTextBoxColumn.HeaderText);
            }
            log.Debug("Ends-ValidateDiscountPurchaseCriteriaDTO(discountPurchaseCriteriaDTO) method.");
            return returnValue;
        }

        private string ValidateDiscountedGamesDTO(DiscountedGamesDTO discountedGamesDTO)
        {
            log.Debug("Starts-ValidateDiscountedGamesDTO(discountedGamesDTO) method.");
            string returnValue = string.Empty;
            log.Debug("Ends-ValidateDiscountedGamesDTO(discountedGamesDTO) method.");
            return returnValue;
        }

        private string ValidateDiscountedProductsDTO(DiscountedProductsDTO discountedProductsDTO)
        {
            log.Debug("Starts-ValidateDiscountedProductsDTO(discountedProductsDTO) method.");
            string returnValue = string.Empty;
            if (discountedProductsDTO.Quantity < 0)
            {
                returnValue = utilities.MessageUtils.getMessage(1144).Replace("&1", dgvDiscountedProductsDTOListQuantityTextBoxColumn.HeaderText);
            }
            if (discountedProductsDTO.DiscountPercentage < 0 || discountedProductsDTO.DiscountPercentage > 100)
            {
                returnValue = utilities.MessageUtils.getMessage(1144).Replace("&1", dgvDiscountedProductsDTOListDiscountPercentageTextBoxColumn.HeaderText);
            }
            if (discountedProductsDTO.DiscountAmount < 0)
            {
                returnValue = utilities.MessageUtils.getMessage(1144).Replace("&1", dgvDiscountedProductsDTOListDiscountAmountTextBoxColumn.HeaderText);
            }
            log.Debug("Ends-ValidateDiscountedProductsDTO(discountedProductsDTO) method.");
            return returnValue;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnDelete_Click() Event.");

            if (dgvDiscountsDTOList.SelectedRows.Count <= 0 && dgvDiscountsDTOList.SelectedCells.Count <= 0)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(959));
                log.Debug("Ends-btnDelete_Click() event by \"No rows selected. Please select the rows you want to delete and press delete..\" message ");
                return;
            }
            bool rowsDeleted = false;
            bool confirmDelete = false;
            bool refreshFromDb = false;
            bool discountPurchaseCriteriaExists = false;

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
                    if (isActiveValue != null && isActiveValue.ToString().Equals("Y"))
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
                        string.Equals(row.Cells[dgvDiscountPurchaseCriteriaDTOListIsActiveCheckBoxColumn.Index].Value.ToString(), "Y"))
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
                            if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                            {
                                confirmDelete = true;
                                refreshFromDb = true;
                                SortableBindingList<DiscountPurchaseCriteriaDTO> discountPurchaseCriteriaDTOSortableList = (SortableBindingList<DiscountPurchaseCriteriaDTO>)discountPurchaseCriteriaDTOListBS.DataSource;
                                DiscountPurchaseCriteriaDTO discountPurchaseCriteriaDTO = discountPurchaseCriteriaDTOSortableList[row.Index];
                                discountPurchaseCriteriaDTO.IsActive = "N";
                                DiscountPurchaseCriteriaBL discountPurchaseCriteriaBL = new DiscountPurchaseCriteriaBL(discountPurchaseCriteriaDTO);
                                discountPurchaseCriteriaBL.Save();
                            }
                        }
                    }
                }
            }
            DiscountsDTO discountsDTO = null;
            if (discountPurchaseCriteriaExists == false)
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
                            try
                            {
                                discountsDTO.IsActive = "N";
                                DiscountsBL discountsBL = new DiscountsBL(discountsDTO);
                                discountsBL.Save();
                                confirmDelete = true;
                                refreshFromDb = true;
                            }
                            catch (ForeignKeyException ex)
                            {
                                log.Error(ex.Message);
                                MessageBox.Show(utilities.MessageUtils.getMessage(1143));
                            }
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
                btnSearch.PerformClick();
            }
            log.Debug("Ends-btnDelete_Click() Event.");
        }

        private void dgvDiscountsDTOList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.Debug("Starts-dgvDiscountsDTOList_DataError() Event.");
            if (e.ColumnIndex != dgvDiscountsDTOListScheduleIdComboBoxColumn.Index)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(1144).Replace("&1", dgvDiscountsDTOList.Columns[e.ColumnIndex].HeaderText));
            }
            e.Cancel = true;
            log.Debug("Ends-dgvDiscountsDTOList_DataError() Event.");
        }

        private void dgvDiscountPurchaseCriteriaDTOList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.Debug("Starts-dgvDiscountPurchaseCriteriaDTOList_DataError() Event.");
            MessageBox.Show(utilities.MessageUtils.getMessage(1144).Replace("&1", dgvDiscountPurchaseCriteriaDTOList.Columns[e.ColumnIndex].HeaderText));
            e.Cancel = true;
            log.Debug("Ends-dgvDiscountPurchaseCriteriaDTOList_DataError() Event.");
        }

        private void dgvDiscountedProductsDTOList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.Debug("Starts-dgvDiscountedProductsDTOList_DataError() Event.");
            MessageBox.Show(utilities.MessageUtils.getMessage(1144).Replace("&1", dgvDiscountedProductsDTOList.Columns[e.ColumnIndex].HeaderText));
            e.Cancel = true;
            log.Debug("Ends-dgvDiscountedProductsDTOList_DataError() Event.");
        }

        private void dgvDiscountedGamesDTOList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.Debug("Starts-dgvDiscountedGamesDTOList_DataError() Event.");
            MessageBox.Show(utilities.MessageUtils.getMessage(1144).Replace("&1", dgvDiscountedGamesDTOList.Columns[e.ColumnIndex].HeaderText));
            e.Cancel = true;
            log.Debug("Ends-dgvDiscountedGamesDTOList_DataError() Event.");
        }

        private void lnkPopulateDiscountedProducts_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.Debug("Starts-lnkPopulateDiscountedProducts_LinkClicked() Event.");
            if (discountsDTOListBS.Current != null && discountsDTOListBS.Current is DiscountsDTO)
            {
                DiscountsDTO discountsDTO = discountsDTOListBS.Current as DiscountsDTO;
                if (discountsDTO.DiscountId != -1)
                {
                    DiscountsBL discountsBL = new DiscountsBL(discountsDTO);
                    discountsBL.PopulateDiscountedProducts();
                    btnSearch.PerformClick();
                }
                else
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(1134), utilities.MessageUtils.getMessage("Discount Save"));
                }
            }
            log.Debug("Ends-lnkPopulateDiscountedProducts_LinkClicked() Event.");
        }



        private void lnkClearDiscountedProducts_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.Debug("Starts-lnkPopulateDiscountedProducts_LinkClicked() Event.");
            if (discountsDTOListBS.Current != null &&
                discountsDTOListBS.Current is DiscountsDTO)
            {
                DiscountsDTO discountsDTO = discountsDTOListBS.Current as DiscountsDTO;
                if (discountsDTO.DiscountId != -1)
                {
                    DiscountsBL discountsBL = new DiscountsBL(discountsDTO);
                    discountsBL.InactivateDiscountedProducts();
                    btnSearch.PerformClick();
                }
                else
                {
                    discountedProductsDTOListBS.DataSource = new SortableBindingList<DiscountedProductsDTO>();
                }
            }
            log.Debug("Ends-lnkPopulateDiscountedProducts_LinkClicked() Event.");
        }

        private void lnkSelectAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.Debug("Starts-lnkPopulateDiscountedProducts_LinkClicked() Event.");
            if (discountsDTOListBS.Current != null &&
                discountsDTOListBS.Current is DiscountsDTO)
            {
                DiscountsDTO discountsDTO = discountsDTOListBS.Current as DiscountsDTO;
                if (discountsDTO.DiscountId != -1)
                {
                    DiscountsBL discountsBL = new DiscountsBL(discountsDTO);
                    discountsBL.ClearDiscountedProducts();
                    btnSearch.PerformClick();
                }
            }
            log.Debug("Ends-lnkPopulateDiscountedProducts_LinkClicked() Event.");
        }

        private void btnProductSearch_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnProductSearch_Click_LinkClicked() Event.");
            SetDiscountedProductsDTOListBS();
            log.Debug("Ends-btnProductSearch_Click_LinkClicked() Event.");
        }

        private void SetDiscountedProductsDTOListBS()
        {
            log.Debug("Starts-UpdateCurrentDiscountsDTO() method.");
            if (discountsDTOListBS.Current != null && discountsDTOListBS.Current is DiscountsDTO)
            {
                DiscountsDTO discountsDTO = discountsDTOListBS.Current as DiscountsDTO;
                List<DiscountedProductsDTO> discountedProductsDTOList = null;
                if (discountsDTO.DiscountedProductsDTOList != null)
                {
                    discountedProductsDTOList = discountsDTO.DiscountedProductsDTOList.FindAll(
                    delegate (DiscountedProductsDTO discountedProductsDTO)
                    {
                        bool returnValue = false;
                        if (string.IsNullOrWhiteSpace(txtProduct.Text))
                        {
                            returnValue = true;
                        }
                        else
                        {
                            string productName = (discountedProductsDTO.ProductId != -1) ? (productsDTODictionary.ContainsKey(discountedProductsDTO.ProductId) ? productsDTODictionary[discountedProductsDTO.ProductId].ProductName : "") : "";
                            string categoryName = (discountedProductsDTO.CategoryId != -1) ? (categoryDTODictionary.ContainsKey(discountedProductsDTO.CategoryId) ? categoryDTODictionary[discountedProductsDTO.CategoryId].Name : "") : "";
                            returnValue = (string.IsNullOrEmpty(productName) == false &&
                                           productName.IndexOf(txtProduct.Text, StringComparison.OrdinalIgnoreCase) >= 0) ||
                                           (string.IsNullOrEmpty(categoryName) == false &&
                                           categoryName.IndexOf(txtProduct.Text, StringComparison.OrdinalIgnoreCase) >= 0);
                        }
                        return returnValue;
                    });
                }


                SortableBindingList<DiscountedProductsDTO> discountedProductsDTOSortableBindingList;
                if (discountedProductsDTOList != null)
                {
                    discountedProductsDTOSortableBindingList = new SortableBindingList<DiscountedProductsDTO>(discountedProductsDTOList);
                }
                else
                {
                    discountedProductsDTOSortableBindingList = new SortableBindingList<DiscountedProductsDTO>();
                }
                discountedProductsDTOListBS.DataSource = discountedProductsDTOSortableBindingList;
            }
            log.Debug("Ends-UpdateCurrentDiscountsDTO() method.");
        }

        private void lnkPopulateGames_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.Debug("Starts-lnkPopulateGames_LinkClicked() Event.");
            if (discountsDTOListBS.Current != null && discountsDTOListBS.Current is DiscountsDTO)
            {
                DiscountsDTO discountsDTO = discountsDTOListBS.Current as DiscountsDTO;
                if (discountsDTO.DiscountId != 0)
                {
                    DiscountsBL discountsBL = new DiscountsBL(discountsDTO);
                    discountsBL.PopulateDiscountedGames();
                    btnSearch.PerformClick();
                }
                else
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(1134), utilities.MessageUtils.getMessage("Discount Save"));
                }
            }
            log.Debug("Ends-lnkPopulateGames_LinkClicked() Event.");
        }

        private void lnkClearDiscountedGameplayGames_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.Debug("Starts-lnkClearDiscountedGameplayGames_LinkClicked() Event.");
            if (discountsDTOListBS.Current != null && discountsDTOListBS.Current is DiscountsDTO)
            {
                DiscountsDTO discountsDTO = discountsDTOListBS.Current as DiscountsDTO;
                if (discountsDTO.DiscountId != -1)
                {
                    DiscountsBL discountsBL = new DiscountsBL(discountsDTO);
                    discountsBL.InactivateDiscountedGames();
                    btnSearch.PerformClick();
                }
                else
                {
                    discountedGamesDTOListBS.DataSource = new SortableBindingList<DiscountedGamesDTO>();
                }
            }

            log.Debug("Ends-lnkClearDiscountedGameplayGames_LinkClicked() Event.");
        }

        private void dgvDiscountsDTOList_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            log.Debug("Starts-dgvDiscountsDTOList_CellValidating() Event.");
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
            log.Debug("Ends-dgvDiscountsDTOList_CellValidating() Event.");
        }

        private void dgvDiscountsDTOList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            log.Debug("Starts-dgvDiscountsDTOList_CellValueChanged() Event.");
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
            log.Debug("Ends-dgvDiscountsDTOList_CellValueChanged() Event.");
        }

        private void dgvDiscountsDTOList_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            log.Debug("Starts-dgvDiscountsDTOList_CellEnter() Event.");
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
            log.Debug("Ends-dgvDiscountsDTOList_CellEnter() Event.");
        }

        private void ShowDiscountCouponsPopup(int index)
        {
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
        }

        private void dgvDiscountsDTOList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.Debug("Starts-dgvDiscountsDTOList_CellContentClick() Event.");
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
            log.Debug("Ends-dgvDiscountsDTOList_CellContentClick() Event.");
        }

        private void btnSchedule_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnSchedule_Click() Event.");
            if (discountsDTOListBS.Current != null && discountsDTOListBS.Current is DiscountsDTO)
            {
                DiscountsDTO discountsDTO = discountsDTOListBS.Current as DiscountsDTO;
                string message = ValidateDiscountsDTO(discountsDTO);
                if (string.IsNullOrEmpty(message))
                {
                    try
                    {
                        DiscountsBL discountsBL = new DiscountsBL(discountsDTO);
                        discountsBL.Save();
                    }
                    catch (ForeignKeyException ex)
                    {
                        log.Error(ex.Message);
                        dgvDiscountsDTOList.Rows[discountsDTOListBS.Position].Selected = true;
                        MessageBox.Show(utilities.MessageUtils.getMessage(1143));
                        return;
                    }
                    catch (Exception)
                    {
                        log.Error("Error while saving Discounts.");
                        dgvDiscountsDTOList.Rows[discountsDTOListBS.Position].Selected = true;
                        MessageBox.Show(utilities.MessageUtils.getMessage(718));
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
                    LoadScheduleDTOList();
                    dgvDiscountsDTOList.Update();
                    dgvDiscountsDTOList.Refresh();
                }
                else
                {
                    MessageBox.Show(message);
                }
            }
            log.Debug("Ends-btnSchedule_Click() Event.");
        }

        private void btnPublishToSite_Click(object sender, EventArgs e)//Starts: Modification on 14-Jul-2016 for adding publish to site feature 
        {
            log.Debug("Starts-btnPublishToSite_Click() Event.");
            if (discountsDTOListBS.Current != null && discountsDTOListBS.Current is DiscountsDTO)
            {
                DiscountsDTO discountsDTO = discountsDTOListBS.Current as DiscountsDTO;
                try
                {
                    if (discountsDTO != null)
                    {
                        if (discountsDTO.DiscountId > 0)
                        {
                            Core.Publish.PublishUI publishUI = new Core.Publish.PublishUI(utilities, discountsDTO.DiscountId, "Discounts", discountsDTO.DiscountName);
                            publishUI.ShowDialog();
                        }
                        log.Debug("Ends-btnPublishToSite_Click() Event.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, utilities.MessageUtils.getMessage("Discount Publish"));
                    log.Fatal("Ends-btnPublishToSite_Click() Event with exception. Exception: " + ex.ToString());
                }
            }
            log.Debug("Ends-btnPublishToSite_Click() Event.");
        }

        private void dgvDiscountPurchaseCriteriaDTOList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            log.Debug("Starts-dgvDiscountPurchaseCriteriaDTOList_CellValueChanged() Event.");
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (e.ColumnIndex == dgvDiscountPurchaseCriteriaDTOListProductIdComboBoxColumn.Index)
                {
                    object value = dgvDiscountPurchaseCriteriaDTOList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                    if (value != null && Convert.ToInt32(value) != -1)
                    {
                        dgvDiscountPurchaseCriteriaDTOList.Rows[e.RowIndex].Cells[dgvDiscountPurchaseCriteriaDTOListCategoryIdComboBoxColumn.Index].ReadOnly = true;
                    }
                    else
                    {
                        dgvDiscountPurchaseCriteriaDTOList.Rows[e.RowIndex].Cells[dgvDiscountPurchaseCriteriaDTOListCategoryIdComboBoxColumn.Index].ReadOnly = false;
                    }
                }
                if (e.ColumnIndex == dgvDiscountPurchaseCriteriaDTOListCategoryIdComboBoxColumn.Index)
                {
                    object value = dgvDiscountPurchaseCriteriaDTOList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                    if (value != null && Convert.ToInt32(value) != -1)
                    {
                        dgvDiscountPurchaseCriteriaDTOList.Rows[e.RowIndex].Cells[dgvDiscountPurchaseCriteriaDTOListProductIdComboBoxColumn.Index].ReadOnly = true;
                    }
                    else
                    {
                        dgvDiscountPurchaseCriteriaDTOList.Rows[e.RowIndex].Cells[dgvDiscountPurchaseCriteriaDTOListProductIdComboBoxColumn.Index].ReadOnly = false;
                    }
                }
            }
            log.Debug("Ends-dgvDiscountPurchaseCriteriaDTOList_CellValueChanged() Event.");
        }

        private void dgvDiscountPurchaseCriteriaDTOList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            log.Debug("Starts-dgvDiscountPurchaseCriteriaDTOList_DataBindingComplete() Event.");
            if(dgvDiscountPurchaseCriteriaDTOList.RowCount > 0)
            {
                for (int i = 0; i < dgvDiscountPurchaseCriteriaDTOList.RowCount; i++)
                {
                    object value = dgvDiscountPurchaseCriteriaDTOList.Rows[i].Cells[dgvDiscountPurchaseCriteriaDTOListProductIdComboBoxColumn.Index].Value;
                    if (value != null && Convert.ToInt32(value) != -1)
                    {
                        dgvDiscountPurchaseCriteriaDTOList.Rows[i].Cells[dgvDiscountPurchaseCriteriaDTOListCategoryIdComboBoxColumn.Index].ReadOnly = true;
                    }
                    else
                    {
                        dgvDiscountPurchaseCriteriaDTOList.Rows[i].Cells[dgvDiscountPurchaseCriteriaDTOListCategoryIdComboBoxColumn.Index].ReadOnly = false;
                    }
                    value = dgvDiscountPurchaseCriteriaDTOList.Rows[i].Cells[dgvDiscountPurchaseCriteriaDTOListCategoryIdComboBoxColumn.Index].Value;
                    if (value != null && Convert.ToInt32(value) != -1)
                    {
                        dgvDiscountPurchaseCriteriaDTOList.Rows[i].Cells[dgvDiscountPurchaseCriteriaDTOListProductIdComboBoxColumn.Index].ReadOnly = true;
                    }
                    else
                    {
                        dgvDiscountPurchaseCriteriaDTOList.Rows[i].Cells[dgvDiscountPurchaseCriteriaDTOListProductIdComboBoxColumn.Index].ReadOnly = false;
                    }
                }
            }
            log.Debug("Ends-dgvDiscountPurchaseCriteriaDTOList_DataBindingComplete() Event.");
        }
    }
}
