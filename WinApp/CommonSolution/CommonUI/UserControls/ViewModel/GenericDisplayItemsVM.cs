/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - Generic Display Item View Model
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.110.0     25-Nov-2020   Raja Uthanda            Modified for multi screen
 *2.130.0     26-May-2021   Raja Uthanda            Modified for multi screen 
 ********************************************************************************************/
using System;
using System.Linq;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Semnox.Core.Utilities;
using Semnox.Parafait.ViewContainer;

namespace Semnox.Parafait.CommonUI
{
    public class GenericDisplayItemsVM : ViewModelBase
    {
        #region Members        
        private bool multiScreenMode;
        private bool showDisabledItems;
        private bool ismultiScreenRowTwo;

        private int column;
        private double itemHeight;

        private ButtonType buttonType;
        private MultiScreenItemBackground multiScreenItemBackground;

        private object selectedItem;
        private GenericDisplayItemsUserControl genericDisplayItemsUserControl;

        private ObservableCollection<object> displayItemModels;
        private Dictionary<string, string> propertyAndValueCollection;
        private ObservableCollection<GenericDisplayItemModel> backupDisplayItemModels;
        private ObservableCollection<ObservableCollection<GenericDisplayItemModel>> currentDisplayItemModels;

        private static readonly logging.Logger log = new logging.Logger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
        public bool ShowDisabledItems
        {
            get { return showDisabledItems; }
            set { SetProperty(ref showDisabledItems, value); }
        }
        public MultiScreenItemBackground MultiScreenItemBackground
        {
            get { return multiScreenItemBackground; }
            set
            {
                SetProperty(ref multiScreenItemBackground, value);
            }
        }
        public ButtonType ButtonType
        {
            get { return buttonType; }
            set
            {
                SetProperty(ref buttonType, value);
            }
        }
        public bool IsMultiScreenRowTwo
        {
            get { return ismultiScreenRowTwo; }
            set
            {
                SetProperty(ref ismultiScreenRowTwo, value);
            }
        }
        public bool MultiScreenMode
        {
            get { return multiScreenMode; }
            set
            {
                SetProperty(ref multiScreenMode, value);
            }
        }
        public Dictionary<string, string> PropertyAndValueCollection
        {
            get { return propertyAndValueCollection; }
            set { SetProperty(ref propertyAndValueCollection, value); }
        }
        public object SelectedItem
        {
            get { return selectedItem; }
            set
            {
                SetProperty(ref selectedItem, value);
            }
        }
        public ObservableCollection<ObservableCollection<GenericDisplayItemModel>> CurrentDisplayItemModels
        {
            get { return currentDisplayItemModels; }
            set
            {
                SetProperty(ref currentDisplayItemModels, value);
            }
        }
        public ObservableCollection<GenericDisplayItemModel> BackupDisplayItemModels
        {
            get { return backupDisplayItemModels; }
            set
            {
                SetProperty(ref backupDisplayItemModels, value);
            }
        }
        public ObservableCollection<object> DisplayItemModels
        {
            get { return displayItemModels; }
            set
            {
                SetProperty(ref displayItemModels, value);
            }
        }
        public ICommand SizeChangedCommand { get; private set; }
        public ICommand LoadedCommand { get; private set; }
        public ICommand ItemClickedCommand { get; private set; }
        public ICommand ItemOfferOrInfoClickedCommand { get; private set; }
        #endregion

        #region Methods
        private void OnItemClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                GenericDisplayItemModel genericDisplayItemModel = parameter as GenericDisplayItemModel;
                if (genericDisplayItemModel != null)
                {
                    if (backupDisplayItemModels != null && backupDisplayItemModels.Count > 0)
                    {
                        selectedItem = genericDisplayItemModel;
                    }
                    else
                    {
                        selectedItem = displayItemModels.FirstOrDefault(
                        d => d.GetType().GetProperty(PropertyAndValueCollection.Keys.FirstOrDefault()).GetValue(d).ToString().ToLower() ==
                        genericDisplayItemModel.Heading.ToLower());
                    }
                }
            }
            if (genericDisplayItemsUserControl != null)
            {
                genericDisplayItemsUserControl.RaiseItemClickedEvent();
            }
            log.LogMethodExit();
        }
        private void OnItemOfferOrInfoClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                GenericDisplayItemModel genericDisplayItemModel = parameter as GenericDisplayItemModel;
                if (genericDisplayItemModel != null)
                {
                    selectedItem = genericDisplayItemModel;
                }
            }
            if (genericDisplayItemsUserControl != null)
            {
                genericDisplayItemsUserControl.RaiseOfferOrInfoClickedEvent();
            }
            log.LogMethodExit();
        }
        private void OnLoaded(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                GenericDisplayItemsUserControl displayItemsUserControl = parameter as GenericDisplayItemsUserControl;
                if (displayItemsUserControl != null)
                {
                    genericDisplayItemsUserControl = displayItemsUserControl;
                }
            }
            SetUIDisplayItemModels();
            log.LogMethodExit();
        }
        private void FindItemsPanel(Visual visual)
        {
            log.LogMethodEntry(visual);
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(visual); i++)
            {
                Visual child = VisualTreeHelper.GetChild(visual, i) as Visual;
                if (child != null)
                {
                    if (child is GenericDisplayItemControl)
                    {
                        GenericDisplayItemControl displayItemControl = child as GenericDisplayItemControl;
                        itemHeight = displayItemControl.ActualHeight + displayItemControl.Margin.Bottom;
                        break;
                    }
                    else
                    {
                        FindItemsPanel(child);
                        break;
                    }
                }
            }
            log.LogMethodExit();
        }
        public void SetUIDisplayItemModels()
        {
            log.LogMethodEntry();
            if (backupDisplayItemModels != null && backupDisplayItemModels.Count > 0)
            {
                SetCurrentItems(backupDisplayItemModels.ToList());
            }
            else if (displayItemModels != null && displayItemModels.Count > 0)
            {
                List<GenericDisplayItemModel> itemModels = new List<GenericDisplayItemModel>();
                foreach (object obj in displayItemModels)
                {
                    GenericDisplayItemModel displayItemModel = GetUIDisplayItemModels(obj);
                    if (displayItemModel != null)
                    {
                        itemModels.Add(displayItemModel);
                    }
                }
                SetCurrentItems(itemModels);
            }
            log.LogMethodExit();
        }
        private void FindAncestor(Visual myVisual)
        {
            log.LogMethodEntry(myVisual);
            if (myVisual == null)
            {
                return;
            }
            object visual = VisualTreeHelper.GetParent(myVisual);
            if (visual is GenericDisplayItemsUserControl)
            {
                genericDisplayItemsUserControl = visual as GenericDisplayItemsUserControl;
                if (genericDisplayItemsUserControl != null)
                {
                    OnLoaded(genericDisplayItemsUserControl);
                }
            }
            else
            {
                FindAncestor(visual as Visual);
            }

            log.LogMethodExit();
        }
        internal void SetCurrentItems(List<GenericDisplayItemModel> itemModels)
        {
            log.LogMethodEntry();
            CurrentDisplayItemModels = new ObservableCollection<ObservableCollection<GenericDisplayItemModel>>();
            if (itemModels != null && itemModels.Any())
            {
                if(!showDisabledItems)
                {
                    itemModels = itemModels.Where(t => t.IsEnabled).ToList();
                }
                column = 2;
                if (genericDisplayItemsUserControl != null && genericDisplayItemsUserControl.ContentAreaItemsControl != null
                    && genericDisplayItemsUserControl.ContentAreaItemsControl.Template != null)
                {
                    CustomScrollViewer scrollViewer = (CustomScrollViewer)genericDisplayItemsUserControl.ContentAreaItemsControl.
                        Template.FindName("scvItems", genericDisplayItemsUserControl.ContentAreaItemsControl);
                    if (scrollViewer != null)
                    {
                        column = (int)Math.Floor(scrollViewer.ViewportWidth / (190 + 8));
                    }
                }
                if (column > 0)
                {
                    for (int i = 0; i < itemModels.Count; i++)
                    {
                        ObservableCollection<GenericDisplayItemModel> currentList = new ObservableCollection<GenericDisplayItemModel>(
                            itemModels.Skip(i).Take(column));
                        CurrentDisplayItemModels.Add(currentList);
                        i += currentList.Count - 1;
                    }
                }
            }
            if (genericDisplayItemsUserControl != null)
            {
                genericDisplayItemsUserControl.RaiseContentRenderedEvent();
            }
            log.LogMethodExit();
        }
        private GenericDisplayItemModel GetUIDisplayItemModels(object model)
        {
            log.LogMethodEntry(model);
            List<string> keyList = new List<string>(propertyAndValueCollection.Keys);
            List<string> valueList = new List<string>(propertyAndValueCollection.Values);
            string heading = model.GetType().GetProperty(keyList[0]).GetValue(model).ToString();
            GenericDisplayItemModel displayItemModel = new GenericDisplayItemModel()
            {
                Heading = heading,
                ButtonType = buttonType,
                Key = heading
            };
            for (int j = 1; j < keyList.Count; j++)
            {
                if (keyList[j].Contains("."))
                {
                    List<string> collection = keyList[j].Split('.').ToList();
                    if (collection != null && collection.Count > 1)
                    {
                        model = model.GetType().GetProperty(collection[0]).GetValue(model);
                        keyList[j] = collection[1];
                    }
                }
                if (model != null)
                {
                    object value = model.GetType().GetProperty(keyList[j]).GetValue(model);
                    if (value != null)
                    {
                        if (value is decimal || value is double || value is float || value is int
                            || value is decimal? || value is double? || value is float? || value is int?)
                        {
                            int intValue = Convert.ToInt32(value);
                            value = intValue <= 0 ? 0.ToString() :
                                intValue.ToString(ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "NUMBER_FORMAT"));
                        }
                        displayItemModel.ItemsSource.Add(new DiscountItem(value.ToString() + " " + valueList[j], string.Empty));
                    }
                }
            }
            log.LogMethodExit(displayItemModel);
            return displayItemModel;
        }
        private void OnSizeChanged(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                CustomScrollViewer scrollViewer = parameter as CustomScrollViewer;
                if (scrollViewer != null)
                {
                    scrollViewer.UpdateLayout();
                    int calculatedColumn = (int)scrollViewer.ViewportWidth / (190 + 8);
                    if (calculatedColumn != column)
                    {
                        SetUIDisplayItemModels();
                    }
                }
            }
            log.LogMethodExit();
        }
        #endregion

        #region Constructors
        public GenericDisplayItemsVM(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ExecutionContext = executionContext;

            buttonType = ButtonType.None;
            multiScreenItemBackground = MultiScreenItemBackground.Grey;

            selectedItem = null;
            column = 2;
            displayItemModels = new ObservableCollection<object>();
            propertyAndValueCollection = new Dictionary<string, string>();

            LoadedCommand = new DelegateCommand(OnLoaded);
            SizeChangedCommand = new DelegateCommand(OnSizeChanged);
            ItemClickedCommand = new DelegateCommand(OnItemClicked);
            ItemOfferOrInfoClickedCommand = new DelegateCommand(OnItemOfferOrInfoClicked);

            log.LogMethodExit();
        }
        #endregion
    }
}