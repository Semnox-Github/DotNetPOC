using Semnox.Core.Utilities;
using Semnox.Parafait.CommonUI;
using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using Semnox.Parafait.POSUI.StaticMenu;
using Semnox.Parafait.Product;

namespace ParafaitPOS
{
    public class SalesVM : BaseWindowViewModel
    {
        #region Members
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool showSecondActionGroup;
        private bool showLineActionButtons;
        private bool showLineBorder;
        private bool showLineActionSecondButtons;

        private ProductMenuUserControlVM productMenuUserControlVM;
        private TransactionItem selectedTransactionItem;
        private ObservableCollection<ProductDTO> products;
        private ObservableCollection<TransactionItem> transactionCollection;
        private SalesView salesView;

        private ICommand actionNavigationCommand;
        private ICommand transactionitemClickedCommand;
        private ICommand itemClickedCommand;
        private ICommand seatClickedCommand;
        private ICommand loadedCommand;
        #endregion

        #region Properties
        public TransactionItem SelectedTransactionItem
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedTransactionItem);
                return selectedTransactionItem;
            }
            set
            {
                log.LogMethodEntry(selectedTransactionItem, value);
                SetProperty(ref selectedTransactionItem, value);
                log.LogMethodExit(selectedTransactionItem);
            }
        }
        public ICommand SeatClickedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(seatClickedCommand);
                return seatClickedCommand;
            }
            set
            {
                log.LogMethodEntry(seatClickedCommand, value);
                SetProperty(ref seatClickedCommand, value);
                log.LogMethodExit(seatClickedCommand);
            }
        }
        public ICommand LoadedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(loadedCommand);
                return loadedCommand;
            }
            set
            {
                log.LogMethodEntry(loadedCommand, value);
                SetProperty(ref loadedCommand, value);
                log.LogMethodExit(loadedCommand);
            }
        }
        public ObservableCollection<TransactionItem> TransactionCollection
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(transactionCollection);
                return transactionCollection;
            }
            set
            {
                log.LogMethodEntry(transactionCollection, value);
                SetProperty(ref transactionCollection, value);
                log.LogMethodExit(transactionCollection);
            }
        }
        public ObservableCollection<ProductDTO> Products
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(products);
                return products;
            }
            set
            {
                log.LogMethodEntry(products, value);
                SetProperty(ref products, value);
                log.LogMethodExit(products);
            }
        }
        public bool ShowLineActionButtons
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(showLineActionButtons);
                return showLineActionButtons;
            }
            set
            {
                log.LogMethodEntry(showLineActionButtons, value);
                SetProperty(ref showLineActionButtons, value);
                log.LogMethodExit(showLineActionButtons);
            }
        }
        public bool ShowLineBorder
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(showLineBorder);
                return showLineBorder;
            }
            set
            {
                log.LogMethodEntry(showLineBorder, value);
                SetProperty(ref showLineBorder, value);
                log.LogMethodExit(showLineBorder);
            }
        }
        public bool ShowLineActionSecondButtons
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(showLineActionSecondButtons);
                return showLineActionSecondButtons;
            }
            set
            {
                log.LogMethodEntry(showLineActionSecondButtons, value);
                SetProperty(ref showLineActionSecondButtons, value);
                log.LogMethodExit(showLineActionSecondButtons);
            }
        }
        public bool ShowSecondActionGroup
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(showSecondActionGroup);
                return showSecondActionGroup;
            }
            set
            {
                log.LogMethodEntry(showSecondActionGroup, value);
                SetProperty(ref showSecondActionGroup, value);
                log.LogMethodExit(showSecondActionGroup);
            }
        }

        public ProductMenuUserControlVM ProductMenuUserControlVM
        {
            get
            {
                return productMenuUserControlVM;
            }
        }

        public ICommand ItemClickedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(itemClickedCommand);
                return itemClickedCommand;
            }
            set
            {
                log.LogMethodEntry(itemClickedCommand, value);
                SetProperty(ref itemClickedCommand, value);
                log.LogMethodExit(itemClickedCommand);
            }
        }
        public ICommand TransactionItemClickedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(transactionitemClickedCommand);
                return transactionitemClickedCommand;
            }
            set
            {
                log.LogMethodEntry(transactionitemClickedCommand, value);
                SetProperty(ref transactionitemClickedCommand, value);
                log.LogMethodExit(transactionitemClickedCommand);
            }
        }
        public ICommand ActionNavigationCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(actionNavigationCommand);
                return actionNavigationCommand;
            }
            set
            {
                log.LogMethodEntry(actionNavigationCommand, value);
                SetProperty(ref actionNavigationCommand, value);
                log.LogMethodExit(actionNavigationCommand);
            }
        }
        #endregion

        #region Methods
        private void OnLeftPaneMenuSelected(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (LeftPaneVM.SelectedMenuItem.ToLower() == "food")
            {

            }
            log.LogMethodExit();
        }
        private void OnLoaded(object parameter)
        {
            salesView = parameter as SalesView;
        }
        private void OnSeatClicked(object parameter)
        {
            TransactionItem transactionItem = parameter as TransactionItem;
            if(transactionItem == TransactionCollection[0] && transactionItem.Selected)
            {
                return;
            }
            if(transactionItem.Selected)
            {
                SelectedTransactionItem.Selected = false;
                TransactionCollection[0].Selected = true;
                SelectedTransactionItem = TransactionCollection[0];
            }
            else
            {   
                SelectedTransactionItem = transactionItem;
                foreach (TransactionItem item in TransactionCollection)
                {
                    item.Selected = false;
                }
                SelectedTransactionItem.Selected = true;
            }
        }
        private void OnItemClicked(object parameter)
        {
            ProductDTO productDTO = parameter as ProductDTO;
            SelectedTransactionItem.Products.Add(productDTO);
            ContentPresenter itemcontainer = salesView.TransactionItemsControl.ItemContainerGenerator.ContainerFromIndex(TransactionCollection.IndexOf(selectedTransactionItem)) as ContentPresenter;
            if (itemcontainer != null)
            {
                ItemsControl itemsControl = FindVisualChild<ItemsControl>(itemcontainer);
                if (itemsControl != null)
                {
                    ContentPresenter contentPresenter = itemsControl.ItemContainerGenerator.ContainerFromIndex(selectedTransactionItem.Products.Count - 1) as ContentPresenter;
                    if (contentPresenter != null)
                    {
                        contentPresenter.BringIntoView();
                    }
                }
            }

        }
        private ChildControl FindVisualChild<ChildControl>(DependencyObject DependencyObj) where ChildControl : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(DependencyObj); i++)
            {
                DependencyObject Child = VisualTreeHelper.GetChild(DependencyObj, i);

                if (Child != null && Child is ChildControl)
                {
                    return (ChildControl)Child;
                }
                else
                {
                    ChildControl ChildOfChild = FindVisualChild<ChildControl>(Child);

                    if (ChildOfChild != null)
                    {
                        return ChildOfChild;
                    }
                }
            }
            return null;
        }
        private void OnTransactionItemClicked(object parameter)
        {   
            if (ShowLineActionButtons)
            {
                ShowLineBorder = true;
            }
            else
            {
                ShowLineBorder = false;
            }
            if (showLineActionButtons)
            {
                ShowLineActionSecondButtons = !showLineActionButtons;
            }
            ShowLineActionButtons = !showLineActionButtons;
            (parameter as ProductDTO).Selected = !(parameter as ProductDTO).Selected;
        }
        private void OnActionNavigationClicked(object parameter)
        {
            if (showLineActionButtons)
            {
                ShowLineActionSecondButtons = !showLineActionSecondButtons;
            }
            else
            {
                ShowSecondActionGroup = !showSecondActionGroup;
            }
        }
        private void OnLeftPaneNavigationClicked(object parameter)
        {
            (parameter as Window).Close();
        }
        #endregion

        #region Constructor
        public SalesVM(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.ExecutionContext = executionContext;
            productMenuUserControlVM = new ProductMenuUserControlVM(executionContext, ProductMenuType.ORDER_SALE);
            LeftPaneVM = new LeftPaneVM(this.ExecutionContext)
            {
                ModuleName = "Sales",
                MenuItems = new ObservableCollection<string>()
                {
                    "Food",
                    "Drinks"
                },
                SelectedMenuItem = "Food"
            };
            FooterVM = new FooterVM(this.ExecutionContext)
            {
                MessageType = MessageType.None,
                Message = string.Empty,
                HideSideBarVisibility = Visibility.Collapsed
            };
            DisplayTagsVM = new DisplayTagsVM()
            {
                DisplayTags = new ObservableCollection<ObservableCollection<DisplayTag>>()
                 {
                     new ObservableCollection<DisplayTag>()
                     {
                         new DisplayTag(){ Text = "Orders"},
                         new DisplayTag(){ Text = "10", FontWeight = FontWeights.Bold, TextSize = TextSize.Medium},
                     },
                     new ObservableCollection<DisplayTag>()
                     {
                         new DisplayTag(){ Text = "Sale Amount"},
                         new DisplayTag(){ Text = "20", FontWeight = FontWeights.Bold, TextSize = TextSize.Medium},
                     },
                     new ObservableCollection<DisplayTag>()
                     {
                         new DisplayTag(){ Text = "Clocked hrs"},
                         new DisplayTag(){ Text = "7 hrs", FontWeight = FontWeights.Bold, TextSize = TextSize.Medium},
                     }
                 }
            };
            ObservableCollection<ProductDTO> productCollection = new ObservableCollection<ProductDTO>()
            {
                new ProductDTO() { Color = Colors.Blue, Name = "Mexicana Chicken", Size = ProductTileSize.Large },
                new ProductDTO() { Color = Colors.Red, Name = "Red Pepper Chicken", Size = ProductTileSize.Large },
                new ProductDTO() { Color = Colors.Orange, Name = "Chicken BBQ", Size = ProductTileSize.Small },
                new ProductDTO() { Color = Colors.Orange, Name = "Chicken sdj", Size = ProductTileSize.Small },
                new ProductDTO() { Color = Colors.Gray, Name = "Cheeky Chicken", Size = ProductTileSize.Small },
                new ProductDTO() { Color = Colors.Yellow, Name = "Chicken Shawarma", Size = ProductTileSize.Small },
                new ProductDTO() { Color = Colors.Orange, Name = "Chicken saldk", Size = ProductTileSize.Small },
                new ProductDTO() { Color = Colors.Orange, Name = "Chicken klo", Size = ProductTileSize.Small },
                new ProductDTO() { Color = Colors.Gray, Name = "Cheeky pw9q", Size = ProductTileSize.Small },
                new ProductDTO() { Color = Colors.Yellow, Name = "Chicken owe9", Size = ProductTileSize.Small },
                new ProductDTO() { Index = 0, Name = "Spicy Chicken Ranch", Size = ProductTileSize.Medium },
                new ProductDTO() { Index = 1, Name = "Chicken Roasted", Size = ProductTileSize.Medium },
            };
            for (int i = 0; i < productCollection.Count; i++)
            {
                if (productCollection[i].Index >= 0 && productCollection[i].Index < productCollection.Count)
                {
                    productCollection.Move(productCollection.IndexOf(productCollection[i]), productCollection[i].Index);
                }
            }
            Products = productCollection;
            transactionCollection = new ObservableCollection < TransactionItem >()
            {
                new TransactionItem()   { SeatName = "Shared Order", Selected = true, Products = new ObservableCollection<ProductDTO>() },
                new TransactionItem()   { SeatName = "Seat1", Selected = false, Products = new ObservableCollection<ProductDTO>() },
                new TransactionItem()   { SeatName = "Seat2", Selected = false, Products = new ObservableCollection<ProductDTO>() },
                new TransactionItem()   { SeatName = "Seat3", Selected = false, Products = new ObservableCollection<ProductDTO>() },
                new TransactionItem()   { SeatName = "Seat4", Selected = false, Products = new ObservableCollection<ProductDTO>() },
            };
            selectedTransactionItem = transactionCollection[0];
            LeftPaneMenuSelectedCommand = new DelegateCommand(OnLeftPaneMenuSelected);
            LeftPaneNavigationClickedCommand = new DelegateCommand(OnLeftPaneNavigationClicked);
            actionNavigationCommand = new DelegateCommand(OnActionNavigationClicked);
            transactionitemClickedCommand = new DelegateCommand(OnTransactionItemClicked);
            itemClickedCommand = new DelegateCommand(OnItemClicked);
            loadedCommand = new DelegateCommand(OnLoaded);
            seatClickedCommand = new DelegateCommand(OnSeatClicked);
            log.LogMethodExit();
        }
        #endregion
    }

    public enum ProductTileSize
    {
        Small,
        Medium,
        Large
    }
    public class TransactionItem : ViewModelBase
    {
        private string seatName;
        private bool selected;
        private ObservableCollection<ProductDTO> products;

        public string SeatName
        {
            get
            {
                return seatName;
            }
            set
            {
                SetProperty(ref seatName, value);
            }
        }
        public bool Selected
        {
            get
            {
                return selected;
            }
            set
            {
                SetProperty(ref selected, value);
            }
        }
        public ObservableCollection<ProductDTO> Products
        {
            get
            {
                return products;
            }
            set
            {
                SetProperty(ref products, value);
            }
        }

    }
    public class ProductDTO : ViewModelBase
    {
        private int index = -1;
        private bool selected;
        private string name;
        private Color color;
        private ProductTileSize size;

        public bool Selected
        {
            get
            {
                return selected;
            }
            set
            {
                SetProperty(ref selected, value);
            }
        }
        public int Index
        {
            get
            {
                return index;
            }
            set
            {
                SetProperty(ref index, value);
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                SetProperty(ref name, value);
            }
        }
        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                SetProperty(ref color, value);
            }
        }
        public ProductTileSize Size
        {
            get
            {
                return size;
            }
            set
            {
                SetProperty(ref size, value);
            }
        }
    }
}
