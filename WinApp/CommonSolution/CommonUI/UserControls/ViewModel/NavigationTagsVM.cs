/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common -view model for navigation user control
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.130.0     25-Sep-2020   Raja Uthanda            Created for POS UI Redesign 
 ********************************************************************************************/
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Collections.ObjectModel;

namespace Semnox.Parafait.CommonUI
{
    public class NavigationTagsVM : ViewModelBase
    {
        #region Members
        private bool showPreviousNavigation;
        private NavigationTag selectedNavigationTag;
        private NavigationUserControl navigationUserControl;

        private ObservableCollection<NavigationTag> navigationTags;
        private ObservableCollection<NavigationTag> previousNavigationTags;
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ICommand loadedCommand;
        private ICommand clickedCommand;
        private ICommand backNavigationCommand;
        #endregion

        #region Properties 
        public ObservableCollection<NavigationTag> PreviousNavigationTags
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(previousNavigationTags);
                return previousNavigationTags;
            }
            set
            {
                log.LogMethodEntry(previousNavigationTags, value);
                SetProperty(ref previousNavigationTags, value);
                log.LogMethodExit(previousNavigationTags);
            }
        }
        public bool ShowPreviousNavigation
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(showPreviousNavigation);
                return showPreviousNavigation;
            }
            set
            {
                log.LogMethodEntry(showPreviousNavigation, value);
                SetProperty(ref showPreviousNavigation, value);
                log.LogMethodExit(showPreviousNavigation);
            }
        }
        public NavigationTag SelectedNavigationTag
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedNavigationTag);
                return selectedNavigationTag;
            }
        }
        public ObservableCollection<NavigationTag> NavigationTags
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(navigationTags);
                return navigationTags;
            }
            set
            {
                log.LogMethodEntry(navigationTags, value);
                SetProperty(ref navigationTags, value);
                if(navigationTags == null)
                {
                    navigationTags = new ObservableCollection<NavigationTag>();
                }
                navigationTags.CollectionChanged -= OnNavigationTagsCollectionChanged;
                navigationTags.CollectionChanged += OnNavigationTagsCollectionChanged;
                ScrollToEnd();
                log.LogMethodExit(navigationTags);
            }
        }
        public ICommand BackNavigationCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(backNavigationCommand);
                return backNavigationCommand;
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
        }
        public ICommand ClickedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(clickedCommand);
                return clickedCommand;
            }
        }
        #endregion

        #region Methods
        private void OnLoaded(object parameter)
        {
            log.LogMethodEntry(parameter);
            if(parameter != null)
            {
                navigationUserControl = parameter as NavigationUserControl;
                if(navigationUserControl != null && navigationUserControl.ContentItemsControl != null)
                {
                    navigationUserControl.ContentItemsControl.SizeChanged += OnContentItemsControlSizeChanged;
                }
                ScrollToEnd();
            }
            log.LogMethodExit();
        }
        private void OnContentItemsControlSizeChanged(object sender, SizeChangedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (navigationUserControl != null && navigationUserControl.scvContentArea != null && navigationUserControl.ContentItemsControl != null)
            {
                PreviousNavigationTags.Clear();
                for (int i = 0; i < navigationUserControl.ContentItemsControl.Items.Count - 1; i++)
                {
                    ContentPresenter contentPresenter = navigationUserControl.ContentItemsControl.ItemContainerGenerator.ContainerFromIndex(i) as ContentPresenter;
                    if (contentPresenter != null && contentPresenter.ContentTemplate != null)
                    {
                        CustomTextBlock panel = contentPresenter.ContentTemplate.FindName("tbText", contentPresenter) as CustomTextBlock;
                        if (panel != null && !IsUserVisible(panel, navigationUserControl.scvContentArea) && !previousNavigationTags.Contains(panel.DataContext))
                        {
                            PreviousNavigationTags.Add(panel.DataContext as NavigationTag);
                        }
                    }
                }
                if(previousNavigationTags.Count > 0)
                {
                    if(!showPreviousNavigation)
                    {
                        ShowPreviousNavigation = true;
                    }
                }
                else if (showPreviousNavigation)
                {
                    ShowPreviousNavigation = false;                    
                }
            }
            log.LogMethodExit();
        }
        private bool IsUserVisible(FrameworkElement element, FrameworkElement container)
        {
            log.LogMethodEntry(element, container);
            if (!element.IsVisible)
            {
                return false;
            }
            Rect bounds = element.TransformToAncestor(container).TransformBounds(new Rect(0.0, 0.0, element.ActualWidth, element.ActualHeight));
            Rect rect = new Rect(0.0, 0.0, container.ActualWidth, container.ActualHeight);
            log.LogMethodExit();
            return rect.Contains(bounds);
        }
        private void ScrollToEnd()
        {
            log.LogMethodEntry();
            if (navigationUserControl != null)
            {
                if(navigationUserControl.scvContentArea != null)
                {
                    navigationUserControl.scvContentArea.ScrollToRightEnd();
                }
                if(navigationUserControl.scvPopup != null)
                {
                    navigationUserControl.scvPopup.ScrollToTop();
                }
            }
            log.LogMethodExit();
        }
        private void OnNavigationTagClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            if(parameter != null)
            {
                selectedNavigationTag = parameter as NavigationTag;
                if(selectedNavigationTag != null)
                {
                    int index = navigationTags.IndexOf(selectedNavigationTag);
                    if (index != navigationTags.Count - 1)
                    {                        
                        for(int i = index + 1; i < navigationTags.Count;i++)
                        {
                            NavigationTags.RemoveAt(i);
                            i--;
                        }
                    }
                    RaiseNavigationTagClickedEvent();
                }
            }
            log.LogMethodExit();
        }
        private void RaiseNavigationTagClickedEvent()
        {
            log.LogMethodEntry();
            if (navigationUserControl != null && selectedNavigationTag != null)
            {
                if (navigationUserControl.PreviousNavigationPopup != null)
                {
                    navigationUserControl.PreviousNavigationPopup.IsOpen = false;
                }
                navigationUserControl.RaiseNavigationTagClickedEvent();
            }
            selectedNavigationTag = null;
            log.LogMethodExit();
        }
        private void OnBackNavigationClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            if(navigationTags != null && navigationTags.Count > 1)
            {
                NavigationTags.Remove(navigationTags.Last());
                if(navigationTags.Count > 0)
                { 
                    selectedNavigationTag = navigationTags.Last();
                    RaiseNavigationTagClickedEvent();
                }
            }
            log.LogMethodExit();
        }
        private void InitalizeCommands()
        {
            log.LogMethodEntry();
            loadedCommand = new DelegateCommand(OnLoaded);
            clickedCommand = new DelegateCommand(OnNavigationTagClicked);
            backNavigationCommand = new DelegateCommand(OnBackNavigationClicked);
            log.LogMethodExit();
        }
        private void OnNavigationTagsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            log.LogMethodEntry(sender,e);
            ScrollToEnd();
            log.LogMethodExit();
        }
        #endregion

        #region Constructor
        public NavigationTagsVM()
        {
            log.LogMethodEntry();

            navigationTags = new ObservableCollection<NavigationTag>();
            previousNavigationTags = new ObservableCollection<NavigationTag>();
            showPreviousNavigation = false;

            InitalizeCommands();
            log.LogMethodExit();
        }
        #endregion
    }
}
