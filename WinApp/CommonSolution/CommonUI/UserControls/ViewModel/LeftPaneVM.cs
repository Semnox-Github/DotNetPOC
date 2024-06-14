/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - Left Pane View Model
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.100.0     25-Sep-2020   Raja Uthanda            Created for POS UI Redesign 
 *2.110.0     25-Nov-2020   Raja Uthanda            modified for multi user mode 
 *2.140.0     25-Nov-2020   Raja Uthanda            Modified for parent-child menu UI.
 ********************************************************************************************/
using System.Linq;
using System.Windows;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Semnox.Core.Utilities;

namespace Semnox.Parafait.CommonUI
{
    public class LeftPaneVM : ViewModelBase
    {
        #region Members
        private const string navigationClickedText = "NavigationClicked";

        private bool hideScroll;
        private bool applyColorCode;
        private bool multiScreenMode;
        private bool fromHeaderClick;
        private bool addButtonVisiblity;
        private bool removeButtonVisiblity;        

        private string screenName;
        private string searchText;
        private string moduleName;
        private string selectedMenuItem;

        private ColorCode colorCode;
        private Visibility searchVisibility;

        private ICommand actionsCommand;
        private ICommand menuSelectedCommand;
        private ICommand addButtonClickedCommand;
        private ICommand removeButtonClickedCommand;

        private ExpanderMenuItem selectedExpanderMenu;
        private LeftPaneUserControl leftPaneUserControl;

        private ObservableCollection<string> menuItems;
        private ObservableCollection<string> backupMenuItems;        
        private ObservableCollection<ExpanderMenuItem> expanderMenus;
        private ObservableCollection<ExpanderMenuItem> backupexpanderMenus;

        private static readonly logging.Logger log = new logging.Logger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
        public string NavigationClickedText
        {
            get { return navigationClickedText;  }
        }
        public bool HideScroll
        {
            get { return hideScroll; }
            set
            {
                log.LogMethodEntry(hideScroll, value);
                SetProperty(ref hideScroll, value);
                log.LogMethodExit(hideScroll);
            }
        }
        public bool ApplyColorCode
        {
            get { return applyColorCode; }
            set
            {
                log.LogMethodEntry(applyColorCode, value);
                SetProperty(ref applyColorCode, value);
                log.LogMethodExit(applyColorCode);
            }
        }
        public ColorCode ColorCode
        {
            get { return colorCode; }
            set
            {
                log.LogMethodEntry(colorCode, value);
                SetProperty(ref colorCode, value);
                log.LogMethodExit(colorCode);
            }
        }
        public bool MultiScreenMode
        {
            get { return multiScreenMode; }
            set
            {
                log.LogMethodEntry(multiScreenMode, value);
                SetProperty(ref multiScreenMode, value);
                log.LogMethodExit(multiScreenMode);
            }
        }
        public bool RemoveButtonVisiblity
        {
            get { return removeButtonVisiblity; }
            set
            {
                log.LogMethodEntry(removeButtonVisiblity, value);
                SetProperty(ref removeButtonVisiblity, value);
                log.LogMethodExit(removeButtonVisiblity);
            }
        }
        public bool AddButtonVisiblity
        {
            get { return addButtonVisiblity; }
            set
            {
                log.LogMethodEntry(addButtonVisiblity, value);
                SetProperty(ref addButtonVisiblity, value);
                log.LogMethodExit(addButtonVisiblity);
            }
        }
        public string ScreenName
        {
            get { return screenName; }
            set
            {
                log.LogMethodEntry(screenName, value);
                SetProperty(ref screenName, value);
                log.LogMethodExit(screenName);
            }
        }
        public string SearchText
        {
            get { return searchText; }
            set
            {
                log.LogMethodEntry(searchText,value);
                SetProperty(ref searchText, value);
                log.LogMethodExit(searchText);
            }
        }
        public string SelectedMenuItem
        {
            get {   return selectedMenuItem; }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref selectedMenuItem, value);
            }
        }
        public string ModuleName
        {
            get {   return moduleName; }
            set
            {   
                if (value != null)
                {
                    log.LogMethodEntry(moduleName, value);
                    SetProperty(ref moduleName, value);
                    log.LogMethodExit(moduleName);
                }
            }
        }
        public Visibility SearchVisibility
        {
            get { return searchVisibility; }
            set
            {
                log.LogMethodEntry(searchVisibility, value);
                SetProperty(ref searchVisibility, value);
                log.LogMethodExit(searchVisibility);
            }
        }
        public ObservableCollection<string> MenuItems
        {
            get { return menuItems; }
            set
            {
                log.LogMethodEntry(menuItems, value);
                SetProperty(ref menuItems, value);
                log.LogMethodExit(menuItems);
            }
        }
        public ObservableCollection<ExpanderMenuItem> ExpanderMenuItems
        {
            get {  return expanderMenus; }
            set
            {
                log.LogMethodEntry(expanderMenus, value);
                SetProperty(ref expanderMenus, value);
                log.LogMethodExit(expanderMenus);
            }
        }
        public ExpanderMenuItem SelectedExpanderMenu
        {
            get {   return selectedExpanderMenu; }
            set
            {
                log.LogMethodEntry(selectedExpanderMenu, value);
                SetProperty(ref selectedExpanderMenu, value);
                log.LogMethodExit(selectedExpanderMenu);
            }
        }
        public ICommand ActionsCommand
        {
            get { return actionsCommand; }
        }        
        public ICommand MenuSelectedCommand
        {
            get { return menuSelectedCommand; }
        }
        public ICommand AddButtonClickedCommand
        {
            get { return addButtonClickedCommand; }
        }
        public ICommand RemoveButtonClickedCommand
        {
            get { return removeButtonClickedCommand; }
        }
        #endregion

        #region Constructors
        public LeftPaneVM(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);

            ExecutionContext = executionContext;
            InitializeCommands();

            hideScroll = false;
            multiScreenMode = false;
            fromHeaderClick = false;
            addButtonVisiblity = false;
            removeButtonVisiblity = false;            

            searchText = string.Empty;
            moduleName = string.Empty;
            screenName = string.Empty;
            selectedMenuItem = string.Empty;

            selectedExpanderMenu = null;
            
            searchVisibility = Visibility.Visible;

            menuItems = new ObservableCollection<string>();
            expanderMenus = new ObservableCollection<ExpanderMenuItem>();

            log.LogMethodExit();
        }
        #endregion

        #region Methods
        public void RaiseCanExecuteChanged()
        {
            log.LogMethodEntry();
            (MenuSelectedCommand as DelegateCommand).RaiseCanExecuteChanged();
            (AddButtonClickedCommand as DelegateCommand).RaiseCanExecuteChanged();
            (RemoveButtonClickedCommand as DelegateCommand).RaiseCanExecuteChanged();
            log.LogMethodExit();
        }
        private void InitializeCommands()
        {
            log.LogMethodEntry();
            PropertyChanged += OnPropertyChanged;
            actionsCommand = new DelegateCommand(OnActions, ButtonEnable);
            menuSelectedCommand = new DelegateCommand(OnMenuSelected, ButtonEnable);
            addButtonClickedCommand = new DelegateCommand(OnAddCliked, ButtonEnable);
            removeButtonClickedCommand = new DelegateCommand(OnRemoveCliked, ButtonEnable);
            log.LogMethodExit();
        }      
        private void OnActions(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                string actionsText = parameter as string;
                TreeViewItem treeViewItem = parameter as TreeViewItem;
                LeftPaneUserControl userControl = parameter as LeftPaneUserControl;
                if (treeViewItem != null)
                {
                    ExpanderMenuItem expanderMenuItems = treeViewItem.DataContext as ExpanderMenuItem;
                    if (treeViewItem.HasItems && expanderMenuItems != null)
                    {
                        expanderMenuItems.IsExpanded = !expanderMenuItems.IsExpanded;
                        if (selectedExpanderMenu != null && selectedMenuItem != null)
                        {
                            string oldSelected = selectedMenuItem;
                            selectedMenuItem = string.Empty;
                            fromHeaderClick = true;
                            SelectedMenuItem = oldSelected;
                            fromHeaderClick = false;
                            treeViewItem.BringIntoView();
                        }
                    }
                    else if (treeViewItem.IsSelected)
                    {
                        ItemsControl control = GetSelectedParentTreeView(treeViewItem);
                        if (control != null && control.DataContext != null)
                        {
                            selectedExpanderMenu = control.DataContext as ExpanderMenuItem;
                        }
                        if (treeViewItem.DataContext != null)
                        {
                            selectedMenuItem = treeViewItem.DataContext.ToString();
                        }
                        RaiseExpanderSelectionChangedEvent();
                    }
                }
                else if(!string.IsNullOrEmpty(actionsText))
                {
                    switch(actionsText)
                    {
                        case navigationClickedText:
                            if (leftPaneUserControl != null)
                            {
                                leftPaneUserControl.RaiseNavigationClickEvent();
                            }
                            break;
                    }
                }
                else if (userControl != null)
                {
                    leftPaneUserControl = userControl;
                    PerformSelection();
                }
            }
            log.LogMethodExit();
        }
        private void RaiseExpanderSelectionChangedEvent()
        {
            log.LogMethodEntry();
            if (selectedExpanderMenu != null && selectedMenuItem != null && leftPaneUserControl != null)
            {
                leftPaneUserControl.RaiseMenuSelectedEvent();
            }
            log.LogMethodExit();
        }
        public ItemsControl GetSelectedParentTreeView(TreeViewItem item)
        {
            log.LogMethodEntry(item);
            DependencyObject parent = null;
            if (item != null)
            {
                parent = VisualTreeHelper.GetParent(item);
                while (!(parent is TreeViewItem))
                {
                    parent = VisualTreeHelper.GetParent(parent);
                }
            }
            log.LogMethodExit(parent);
            return parent as ItemsControl;
        }
        private void ClearMenuItems(bool clearExpandMenu)
        {
            log.LogMethodEntry(clearExpandMenu);
            if(clearExpandMenu && expanderMenus != null && expanderMenus.Any())
            {
                ExpanderMenuItems.Clear();
            }
            else if (!clearExpandMenu && menuItems != null && menuItems.Any())
            {
                MenuItems.Clear();
            }
            log.LogMethodExit();
        }
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if(!string.IsNullOrEmpty(e.PropertyName))
            {   
                switch(e.PropertyName)
                {
                    case "SearchText":
                        if (searchVisibility == Visibility.Visible)
                        {
                            PerformSearch();
                        }
                        break;
                    case "MenuItems":
                        if (backupMenuItems == null)
                        {
                            backupMenuItems = new ObservableCollection<string>(menuItems);
                        }
                        ClearMenuItems(true);
                        break;
                    case "ExpanderMenuItems":
                        if (backupexpanderMenus == null)
                        {
                            backupexpanderMenus = new ObservableCollection<ExpanderMenuItem>(expanderMenus);
                        }
                        ClearMenuItems(false);
                        break;
                    case "SelectedExpanderMenu":
                    case "SelectedMenuItem":
                        PerformSelection();
                        break;
                }
            }
            log.LogMethodExit();
        }
        private void PerformSelection()
        {
            log.LogMethodEntry();
            if (expanderMenus != null && expanderMenus.Any() && selectedMenuItem != null && selectedExpanderMenu != null && selectedExpanderMenu.SubMenus != null
                && selectedExpanderMenu.SubMenus.Any() && leftPaneUserControl != null && leftPaneUserControl.ExpanderTreeView != null)
            {
                PerformChildSelection(selectedExpanderMenu, false);
            }
            log.LogMethodExit();
        }
        private void PerformChildSelection(ExpanderMenuItem expanderMenu, bool fromSearch)
        {
            log.LogMethodEntry(expanderMenu, fromSearch);
            if(expanderMenu != null && expanderMenu.SubMenus != null)
            {
                string selectedItem = expanderMenu.SubMenus.FirstOrDefault(s => s.ToLower() == selectedMenuItem.ToLower());
                if(selectedItem != null)
                {
                    if (!fromSearch && !fromHeaderClick && !selectedExpanderMenu.IsExpanded)
                    {
                        selectedExpanderMenu.IsExpanded = true;
                    }
                    TreeViewItem parentTreeView = leftPaneUserControl.ExpanderTreeView.ItemContainerGenerator.ContainerFromItem(expanderMenu) as TreeViewItem;
                    if (parentTreeView != null)
                    {
                        TreeViewItem chidTreeViewItem = parentTreeView.ItemContainerGenerator.ContainerFromItem(selectedItem) as TreeViewItem;
                        if (chidTreeViewItem == null)
                        {
                            parentTreeView.UpdateLayout();
                            chidTreeViewItem = parentTreeView.ItemContainerGenerator.ContainerFromItem(selectedItem) as TreeViewItem;
                        }
                        if (chidTreeViewItem != null)
                        {
                            chidTreeViewItem.IsSelected = true;
                            if (!fromHeaderClick && !fromSearch)
                            {
                                chidTreeViewItem.BringIntoView();
                                RaiseExpanderSelectionChangedEvent();
                            }
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
        private void PerformSearch()
        {
            log.LogMethodEntry();
            if (searchText == null)
            {
                searchText = string.Empty;
            }
            string smallText = searchText.ToLower();
            if (backupMenuItems != null && backupMenuItems.Any())
            {
                List<string> tempList = backupMenuItems.Where(g => g != null && g.ToLower().Contains(smallText)).ToList();
                MenuItems = new ObservableCollection<string>(tempList);
            }
            else if(backupexpanderMenus != null && backupexpanderMenus.Any())
            {
                List<ExpanderMenuItem> possibleCollection = backupexpanderMenus.Where(g => (g.ParentHeader != null && g.ParentHeader.ToLower().Contains(smallText))
                || (g.SubMenus != null && g.SubMenus.Any(s => s != null && s.ToLower().Contains(smallText)))).ToList();
                List<ExpanderMenuItem> searchedList = new List<ExpanderMenuItem>();
                if (possibleCollection != null && possibleCollection.Any())
                {
                    bool expandParent = true;
                    foreach(ExpanderMenuItem expander in possibleCollection)
                    {
                        if((expander.ParentHeader != null && expander.ParentHeader.ToLower().Contains(smallText)) || !(expander.SubMenus != null && expander.SubMenus.Any(s => s != null && s.ToLower().Contains(smallText))))
                        {
                            if(!expander.IsExpanded)
                            {
                                expander.IsExpanded = expandParent;
                            }
                            searchedList.Add(expander);
                        }
                        else if(expander.SubMenus != null && expander.SubMenus.Any(s => s != null && s.ToLower().Contains(smallText)))
                        {
                            searchedList.Add(new ExpanderMenuItem(expander.ParentHeader, 
                                new ObservableCollection<string>(expander.SubMenus.Where(s => s != null && s.ToLower().Contains(smallText))), expandParent));
                        }
                    }
                }
                if(searchedList == null)
                {
                    searchedList = new List<ExpanderMenuItem>();
                }
                ExpanderMenuItems = new ObservableCollection<ExpanderMenuItem>(searchedList);
                if (selectedExpanderMenu != null && selectedMenuItem != null && expanderMenus != null && expanderMenus.Any())
                {
                    PerformChildSelection(expanderMenus.FirstOrDefault(e => e.ParentHeader.ToLower() == selectedExpanderMenu.ParentHeader.ToLower()), true);
                }
            }
            log.LogMethodExit();
        }
        private void OnMenuSelected(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                LeftPaneUserControl leftPaneUserControl = parameter as LeftPaneUserControl;
                if (leftPaneUserControl != null)
                {
                    leftPaneUserControl.SelectedItem = SelectedMenuItem;
                    leftPaneUserControl.RaiseMenuSelectedEvent();
                }
            }
            log.LogMethodExit();
        }
        private void OnAddCliked(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                LeftPaneUserControl leftPaneUserControl = parameter as LeftPaneUserControl;
                if (leftPaneUserControl != null)
                {
                    leftPaneUserControl.RaiseAddButtonClickedEvent();
                }
            }
            log.LogMethodExit();
        }
        private void OnRemoveCliked(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                LeftPaneUserControl leftPaneUserControl = parameter as LeftPaneUserControl;
                if (leftPaneUserControl != null)
                {
                    leftPaneUserControl.RaiseRemoveButtonClickedEvent();
                }
            }
            log.LogMethodExit();
        }
        #endregion
    }
}
