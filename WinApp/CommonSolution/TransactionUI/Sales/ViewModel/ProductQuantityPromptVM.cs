using Semnox.Core.Utilities;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;
using Semnox.Parafait.TransactionUI.Sales.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Semnox.Parafait.TransactionUI.Sales
{

    public class ProductQuantityOptionDTO
    {
        private string productName;
        private int minimumQuantity;
        private int childProductId;
        private int maximumQuantity;
        private bool quantityPrompt;
        private int productQuantity;
        public string ProductName
        {
            get { return productName; }
            set { productName = value; }
        }
        public int ProductQuantity
        {
            get { return productQuantity; }
            set { productQuantity = value; }
        }
        public int MinimumQuantity
        {
            get { return minimumQuantity; }
            set { minimumQuantity = value; }
        }
        public int ChildProductId
        {
            get { return childProductId; }
            set { childProductId = value; }
        }
        public int MaximumQuantity
        {
            get { return maximumQuantity; }
            set { maximumQuantity = value; }
        }

        public bool QuantityPrompt
        {
            get { return quantityPrompt; }
            set { quantityPrompt = value; }
        }
        public ProductQuantityOptionDTO(int childProductId, string productName, int minimumQuantity, int maximumQuantity, bool quantityPrompt, int productQuantity)
        {
            this.childProductId = childProductId;
            this.productName = productName;
            this.maximumQuantity = maximumQuantity;
            this.minimumQuantity = minimumQuantity;
            this.quantityPrompt = quantityPrompt;
            this.productQuantity = productQuantity;
        }
    }
    public class ProductQuantityPromptVM : BaseWindowViewModel
    {
        private ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private DisplayTagsVM displayTagsVM;
        private const string VOLUNTEER_ISSUANCE = "V";
        private ProductQuantityPromptView productQuantityPrompt;
        private ObservableCollection<ProductQuantityOptionDTO> productQuantityOptionDTOList;
        private ICommand closeCommand;
        private ICommand loadedCommand;
        private ICommand navigationClickCommand;
        private ICommand optionButtionClicked;
        private Dictionary<int, int> productIdQuantityDictionary;
        private List<ComboProductDTO> comboProductDTOList;
        // Lables 
        private string labelStatus;
        private string labelOptionContent;
        private string moduleName;

        #region Properties
        /// <summary>
        /// DisplayTagsVM
        /// </summary>
        public DisplayTagsVM DisplayTagsVM
        {
            get
            {
                return displayTagsVM;
            }
            set
            {
                displayTagsVM = value;
            }
        }
        public ObservableCollection<ProductQuantityOptionDTO> ProductList
        {
            get
            {
                return productQuantityOptionDTOList;
            }
            set
            {
                productQuantityOptionDTOList = value;
            }
        }
        /// <summary>
        /// ModuleName
        /// </summary>
        public string ModuleName
        {
            get
            {
                return moduleName;
            }
            set
            {
                moduleName = value;
            }
        }
        /// <summary>
        /// ModuleName
        /// </summary>
        public Dictionary<int, int> ProductIdQuantityDictionary
        {
            get
            {
                return productIdQuantityDictionary;
            }
            set
            {
                productIdQuantityDictionary = value;
            }
        }
        public string LabelOptionContent
        {
            get
            {
                return labelOptionContent;
            }
            set
            {
                log.LogMethodEntry(value);
                if (value != null)
                {
                    SetProperty(ref labelOptionContent, value);
                }
            }
        }

        /// <summary>
        /// CloseCommand
        /// </summary>
        public ICommand CloseCommand
        {
            get
            {
                return closeCommand;
            }
            set
            {
                SetProperty(ref closeCommand, value);
            }
        }

        /// <summary>
        /// LoadedCommand
        /// </summary>
        public ICommand LoadedCommand
        {
            get
            {
                return loadedCommand;
            }
            set
            {
                SetProperty(ref loadedCommand, value);
            }
        }
        /// <summary>
        /// OptionButtonClicked
        /// </summary>
        public ICommand OptionButtonClicked
        {
            get
            {
                return optionButtionClicked;
            }
            set
            {
                SetProperty(ref optionButtionClicked, value);
            }
        }
        /// <summary>
        /// tagCommands
        /// </summary>

            #endregion Properties

            #region Constructor
            /// <summary>
            /// NotificationTagsVM
            /// </summary>
            /// <param name="executionContext"></param>
        public ProductQuantityPromptVM(ExecutionContext executionContext, List<ComboProductDTO> comboProductDTOList)
        {
            log.LogMethodEntry(comboProductDTOList);
            this.ExecutionContext = executionContext;
            this.comboProductDTOList = comboProductDTOList;
            productIdQuantityDictionary = new Dictionary<int, int>();
            FooterVM = new FooterVM(ExecutionContext)
            {
                Message = string.Empty,
                MessageType = MessageType.None,
                HideSideBarVisibility = Visibility.Collapsed
            };
            InitializeCommands();
            LoadProductQuantityRows(comboProductDTOList);
            SetDisplayTagsVM();
            log.LogMethodExit();
        }
        #endregion Constructor

        #region Methods

        private void SetDisplayTagsVM()
        {
            SetFooterContent(string.Empty, MessageType.None);
            if (DisplayTagsVM == null)
            {
                DisplayTagsVM = new DisplayTagsVM();
            }
            DisplayTagsVM.DisplayTags = new ObservableCollection<ObservableCollection<DisplayTag>>()
                                    {
                                      new ObservableCollection<DisplayTag>()
                                      {
                                          new DisplayTag()
                                          {
                                               Text = MessageContainerList.GetMessage(ExecutionContext,  4238),
                                               TextSize = TextSize.Medium,
                                               FontWeight = System.Windows.FontWeights.Bold
                                          }
                                      }
                                    };

        }

        private void ShowPopUp()
        {
            log.LogMethodEntry();
            PerformClose();
            log.LogMethodExit();
        }

        private void PerformClose()
        {
            if (productQuantityPrompt != null)
            {
                productQuantityPrompt.Close();
            }
        }
        private void InitializeCommands()
        {
            LoadedCommand = new DelegateCommand(OnLoaded);
            CloseCommand = new DelegateCommand(Close);
            OptionButtonClicked = new DelegateCommand(OnPrintOptionClick);
        }

        private void LoadProductQuantityRows(List<ComboProductDTO> comboProductDTOList)
        {
            log.LogMethodEntry(comboProductDTOList);
            List<ProductQuantityOptionDTO> productQuantityOptionDTOList = new List<ProductQuantityOptionDTO>();
            if (comboProductDTOList != null
                 && comboProductDTOList.Any())
            {
                foreach (ComboProductDTO comboProductContainerDTO in comboProductDTOList)
                {
                    Products productsBL = new Products(ExecutionContext, comboProductContainerDTO.ChildProductId);
                    int minQty = comboProductContainerDTO.Quantity.HasValue ? (Convert.ToInt32(comboProductContainerDTO.Quantity))
                        : Convert.ToInt32(productsBL.GetProductsDTO.MinimumQuantity);
                    int maxQty = comboProductContainerDTO.MaximumQuantity.HasValue ? (Convert.ToInt32(comboProductContainerDTO.MaximumQuantity))
                        : (productsBL.GetProductsDTO.MaximumQuantity.HasValue ? Convert.ToInt32(productsBL.GetProductsDTO.MaximumQuantity) : 0);
                    ProductQuantityOptionDTO productQuantityOptionDTO = new ProductQuantityOptionDTO(
                                                                         comboProductContainerDTO.ChildProductId,
                                                                         comboProductContainerDTO.ChildProductName,
                                                                         minQty,
                                                                         maxQty,
                                                                         productsBL.GetProductsDTO.QuantityPrompt == "Y" ? true : false, 
                                                                         Convert.ToInt32(minQty));

                    if (productQuantityOptionDTOList.Exists(x => x.ChildProductId == productQuantityOptionDTO.ChildProductId))
                    {
                        var existsProductQuantityOptionDTO = productQuantityOptionDTOList.Where(x => x.ChildProductId == productQuantityOptionDTO.ChildProductId).First();
                        existsProductQuantityOptionDTO.ProductQuantity += 1;
                    }
                    else
                    {
                        productQuantityOptionDTOList.Add(productQuantityOptionDTO);
                    }
                }
            }
            ProductList = new ObservableCollection<ProductQuantityOptionDTO>(productQuantityOptionDTOList);
            LabelOptionContent = MessageContainerList.GetMessage(ExecutionContext, "Please choose the quantity for products");
            log.LogMethodExit();
        }
        private void OnLoaded(object parameter)
        {
            log.LogMethodEntry();
            SetFooterContent(string.Empty,MessageType.None);
            productQuantityPrompt = parameter as ProductQuantityPromptView;
            log.LogMethodExit();
        }
        private void OnPrintOptionClick(object parameter)
        {
            log.LogMethodEntry();
            SetFooterContent(string.Empty, MessageType.None);
            productQuantityPrompt = parameter as ProductQuantityPromptView;
            try
            {
                if (productQuantityPrompt != null)
                {

                    foreach (ProductQuantityOptionDTO productQuantityOptionDTO in ProductList)
                    {
                        if(productQuantityOptionDTO.MaximumQuantity > 0 && productQuantityOptionDTO.ProductQuantity > productQuantityOptionDTO.MaximumQuantity)
                        {
                            throw new ValidationException(MessageContainerList.GetMessage(ExecutionContext, "Quantity cannot be more than max quantity"));
                        }
                        if (productQuantityOptionDTO.MinimumQuantity > 0 && productQuantityOptionDTO.ProductQuantity < productQuantityOptionDTO.MinimumQuantity)
                        {
                            throw new ValidationException(MessageContainerList.GetMessage(ExecutionContext, "Quantity cannot be less than mim quantity"));
                        }
                        if (productIdQuantityDictionary.ContainsKey(productQuantityOptionDTO.ChildProductId) == false)
                        {
                            productIdQuantityDictionary.Add(productQuantityOptionDTO.ChildProductId, productQuantityOptionDTO.ProductQuantity);
                        }
                        //else
                        //{
                        //    productIdQuantityDictionary[productQuantityOptionDTO.ChildProductId] = productQuantityOptionDTO.ProductQuantity;
                        //}
                    }
                }
                PerformClose();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                SetFooterContent(ex.Message,MessageType.Error);
            }
            log.LogMethodExit();
        }
        private void Close(object param)
        {
            log.LogMethodEntry(param);
            String ErrorMessage = String.Empty;
            productQuantityPrompt = param as ProductQuantityPromptView;
            try
            {
                if (productQuantityPrompt != null)
                {
                    if (productIdQuantityDictionary != null)
                    {
                        productIdQuantityDictionary.Clear();
                    }
                    productQuantityPrompt.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            };
        }
        private void NavigationClick(object param)
        {
            log.LogMethodEntry(param);
            String ErrorMessage = String.Empty;
            productQuantityPrompt = param as ProductQuantityPromptView;
            try
            {
                if (productQuantityPrompt != null)
                {
                    productQuantityPrompt.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            };
        }
        private void CloseAddWindow(string message)
        {
            if (productQuantityPrompt != null)
            {
                productQuantityPrompt.Close();
            }
        }

        #endregion Methods
    }
}
