/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - User Control VM for Custom Data grid 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.120.0     24-Mar-2021   Raja Uthanda            Created for POS UI Redesign 
 ********************************************************************************************/
using System;
using System.Linq;
using System.Windows;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls.Primitives;

using Semnox.Core.Utilities;
using Semnox.Parafait.ViewContainer;

namespace Semnox.Parafait.CommonUI
{
    public enum SelectOption
    {
        None,
        CheckBox,
        Delete,
        ManualSelectionOnly
    }
    public class CustomDataGridVM : ViewModelBase
    {
        #region Members        
        private bool selectAll;
        private bool isReadOnly;
        private bool showHeader;
        private bool showRowDetails;
        private bool multiScreenMode;
        private bool showSearchTextBox;
        private bool ismultiScreenRowTwo;
        private bool enableInternalSorting;
        private bool showchildHeaderColumns;
        private bool isComboAndSearchVisible;
        private bool dataGridRowEnableWorksInReverse;

        private SelectOption selectOption;
        private ScrollBarVisibility verticalScrollBarVisibility;
        private ScrollBarVisibility horizontalScrollBarVisibility;
        private MultiScreenItemBackground multiScreenItemBackground;
        private DataGridRowDetailsVisibilityMode rowDetailsVisibilityMode;

        private string searchText;
        private string rowDetailsProperty;
        private string childDataGridHeader;
        private string dataGridRowEnableProperty;
        private string searchTextBoxDefaultValue;
        private string dataGridRowReadOnlyProperty;

        private object selectedItem;
        private DataGridCell currentCell;
        private ComboGroupVM comboGroupVM;
        private object parentSelectedItem;
        private DataGridColumn sortedColoumn;
        private DataGridRow comboBoxSelectedRow;
        private CustomDataGridVM parentDataGridVM;
        private CustomDataGridVM childCustomDataGridVM;
        private CustomDataGridUserControl customDataGrid;
        private CustomDataGridButtonModel buttonClickedModel;
        private CustomDataGridButtonModel childButtonClickedModel;
        private CustomDataGridComboBoxSelectionModel selectedCustomComboBoxModel;

        private List<object> selectedGroup;
        private ObservableCollection<object> selectedItems;
        private ObservableCollection<string> searchProperties;
        private ObservableCollection<object> collectionToBeRendered;
        private ObservableCollection<object> uiCollectionToBeRendered;
        private Dictionary<string, CustomDataGridColumnElement> headerCollection;
        private Dictionary<string, CustomDataGridColumnElement> rowDetailsHeaderCollection;
        private static readonly logging.Logger log = new logging.Logger(MethodBase.GetCurrentMethod().DeclaringType);

        private ICommand loadedCommand;
        private ICommand deleteCommand;
        private ICommand searchCommand;
        private ICommand actionsCommand;
        private ICommand selectAllCommand;
        private ICommand buttonClickedCommand;
        private ICommand checkBoxClickedCommand;
        private ICommand selectionChangedCommand;
        private ICommand radioButtonClickedCommand;
        private ICommand comboSelectionChangedCommand;
        private ICommand checkBoxandGridRowLoadedCommand;
        #endregion

        #region Properties
        public List<object> SelectedGroup
        {
            get { return selectedGroup; }
            set
            {
                if (!Equals(selectedGroup, value))
                {
                    selectedGroup = value;
                    OnPropertyChanged();
                }
            }
        }
        public string ChildDataGridHeader
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(childDataGridHeader);
                return childDataGridHeader;
            }
            set
            {
                log.LogMethodEntry(childDataGridHeader, value);
                SetProperty(ref childDataGridHeader, value);
                log.LogMethodExit(childDataGridHeader);
            }
        }
        public bool ShowChildHeaderColumns
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(showchildHeaderColumns);
                return showchildHeaderColumns;
            }
            set
            {
                log.LogMethodEntry(showchildHeaderColumns, value);
                SetProperty(ref showchildHeaderColumns, value);
                log.LogMethodExit(showchildHeaderColumns);
            }
        }
        public bool ShowHeader
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(showHeader);
                return showHeader;
            }
            set
            {
                log.LogMethodEntry(showHeader, value);
                SetProperty(ref showHeader, value);
                log.LogMethodExit(showHeader);
            }
        }
        public DataGridRowDetailsVisibilityMode RowDetailsVisibilityMode
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(rowDetailsVisibilityMode);
                return rowDetailsVisibilityMode;
            }
            set
            {
                log.LogMethodEntry(rowDetailsVisibilityMode, value);
                SetProperty(ref rowDetailsVisibilityMode, value);
                log.LogMethodExit(rowDetailsVisibilityMode);
            }
        }
        public bool EnableInternalSorting
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(enableInternalSorting);
                return enableInternalSorting;
            }
            set
            {
                log.LogMethodEntry(enableInternalSorting, value);
                SetProperty(ref enableInternalSorting, value);
                log.LogMethodExit(enableInternalSorting);
            }
        }
        public CustomDataGridButtonModel ButtonClickedModel
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(buttonClickedModel);
                return buttonClickedModel;
            }
        }
        public DataGridColumn SortedColoumn
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(sortedColoumn);
                return sortedColoumn;
            }
            set
            {
                log.LogMethodEntry();
                log.LogMethodExit(sortedColoumn);
                SetProperty(ref sortedColoumn, value);
            }
        }
        public CustomDataGridVM SelectedChildCustomDataGridVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(childCustomDataGridVM);
                return childCustomDataGridVM;
            }
            private set
            {
                log.LogMethodEntry(childCustomDataGridVM, value);
                SetProperty(ref childCustomDataGridVM, value);
                log.LogMethodExit(childCustomDataGridVM);
            }
        }
        public CustomDataGridComboBoxSelectionModel SelectedCustomComboBoxModel
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedCustomComboBoxModel);
                return selectedCustomComboBoxModel;
            }
        }
        public string DataGridRowReadOnlyProperty
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(dataGridRowReadOnlyProperty);
                return dataGridRowReadOnlyProperty;
            }
            set
            {
                log.LogMethodEntry(dataGridRowReadOnlyProperty, value);
                SetProperty(ref dataGridRowReadOnlyProperty, value);
                log.LogMethodExit(dataGridRowReadOnlyProperty);
            }
        }
        public string RowDetailsProperty
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(rowDetailsProperty);
                return rowDetailsProperty;
            }
            set
            {
                log.LogMethodEntry(rowDetailsProperty, value);
                SetProperty(ref rowDetailsProperty, value);
                log.LogMethodExit(rowDetailsProperty);
            }
        }
        public bool ShowRowDetails
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(showRowDetails);
                return showRowDetails;
            }
            set
            {
                log.LogMethodEntry(showRowDetails, value);
                SetProperty(ref showRowDetails, value);
                log.LogMethodExit(showRowDetails);
            }
        }
        public Dictionary<string, CustomDataGridColumnElement> RowDetailsHeaderCollection
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(rowDetailsHeaderCollection);
                return rowDetailsHeaderCollection;
            }
            set
            {
                log.LogMethodEntry(rowDetailsHeaderCollection, value);
                SetProperty(ref rowDetailsHeaderCollection, value);
                log.LogMethodExit(rowDetailsHeaderCollection);
            }
        }
        public ScrollBarVisibility HorizontalScrollBarVisibility
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(horizontalScrollBarVisibility);
                return horizontalScrollBarVisibility;
            }
            set
            {
                log.LogMethodEntry(horizontalScrollBarVisibility, value);
                SetProperty(ref horizontalScrollBarVisibility, value);
                log.LogMethodExit(horizontalScrollBarVisibility);
            }
        }
        public ScrollBarVisibility VerticalScrollBarVisibility
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(verticalScrollBarVisibility);
                return verticalScrollBarVisibility;
            }
            set
            {
                log.LogMethodEntry(verticalScrollBarVisibility, value);
                SetProperty(ref verticalScrollBarVisibility, value);
                log.LogMethodExit(verticalScrollBarVisibility);
            }
        }
        public MultiScreenItemBackground MultiScreenItemBackground
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(multiScreenItemBackground);
                return multiScreenItemBackground;
            }
            set
            {
                log.LogMethodEntry(multiScreenItemBackground, value);
                SetProperty(ref multiScreenItemBackground, value);
                log.LogMethodExit(multiScreenItemBackground);
            }
        }
        public bool IsMultiScreenRowTwo
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(ismultiScreenRowTwo);
                return ismultiScreenRowTwo;
            }
            set
            {
                log.LogMethodEntry(ismultiScreenRowTwo, value);
                SetProperty(ref ismultiScreenRowTwo, value);
                ResetColumns();
                log.LogMethodExit(ismultiScreenRowTwo);
            }
        }
        public bool MultiScreenMode
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(multiScreenMode);
                return multiScreenMode;
            }
            set
            {
                log.LogMethodEntry(multiScreenMode, value);
                SetProperty(ref multiScreenMode, value);
                log.LogMethodExit(multiScreenMode);
            }
        }
        public bool ShowSearchTextBox
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(showSearchTextBox);
                return showSearchTextBox;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref showSearchTextBox, value);
                log.LogMethodExit(showSearchTextBox);
            }
        }
        public bool IsComboAndSearchVisible
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(isComboAndSearchVisible);
                return isComboAndSearchVisible;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref isComboAndSearchVisible, value);
                log.LogMethodExit(isComboAndSearchVisible);
            }
        }
        public bool DataGridRowEnableWorksInReverse
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(dataGridRowEnableWorksInReverse);
                return dataGridRowEnableWorksInReverse;
            }
            set
            {
                log.LogMethodEntry(dataGridRowEnableWorksInReverse, value);
                SetProperty(ref dataGridRowEnableWorksInReverse, value);
                log.LogMethodExit(dataGridRowEnableWorksInReverse);
            }
        }
        public string DataGridRowEnableProperty
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(dataGridRowEnableProperty);
                return dataGridRowEnableProperty;
            }
            set
            {
                log.LogMethodEntry(dataGridRowEnableProperty, value);
                SetProperty(ref dataGridRowEnableProperty, value);
                log.LogMethodExit(dataGridRowEnableProperty);
            }
        }
        public bool IsReadOnly
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(isReadOnly);
                return isReadOnly;
            }
            set
            {
                log.LogMethodEntry(isReadOnly, value);
                SetProperty(ref isReadOnly, value);
                log.LogMethodExit(isReadOnly);
            }
        }
        public SelectOption SelectOption
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectOption);
                return selectOption;
            }
            set
            {
                log.LogMethodEntry(selectOption, value);
                SetProperty(ref selectOption, value);
                log.LogMethodExit(selectOption);
            }
        }
        public ComboGroupVM ComboGroupVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(comboGroupVM);
                return comboGroupVM;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref comboGroupVM, value);
            }
        }
        public string SearchTextBoxDefaultValue
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(searchTextBoxDefaultValue);
                return searchTextBoxDefaultValue;
            }
            set
            {
                log.LogMethodEntry(searchTextBoxDefaultValue, value);
                SetProperty(ref searchTextBoxDefaultValue, value);
                log.LogMethodExit(searchTextBoxDefaultValue);
            }
        }
        public string SearchText
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(searchText);
                return searchText;
            }
            set
            {
                log.LogMethodEntry(searchText, value);
                SetProperty(ref searchText, value);
                if (isComboAndSearchVisible)
                {
                    UpdateUICollection();
                }
                log.LogMethodExit(searchText);
            }
        }
        public ObservableCollection<string> SearchProperties
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(searchProperties);
                return searchProperties;
            }
            set
            {
                log.LogMethodEntry(searchProperties, value);
                SetProperty(ref searchProperties, value);
                log.LogMethodExit(searchProperties);
            }
        }
        public ObservableCollection<object> CollectionToBeRendered
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(collectionToBeRendered);
                return collectionToBeRendered;
            }
            set
            {
                log.LogMethodEntry(collectionToBeRendered, value);
                SetProperty(ref collectionToBeRendered, value);
                UpdateUI();
                log.LogMethodExit(collectionToBeRendered);
            }
        }
        public ObservableCollection<object> UICollectionToBeRendered
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(uiCollectionToBeRendered);
                return uiCollectionToBeRendered;
            }
            set
            {
                log.LogMethodEntry(uiCollectionToBeRendered, value);
                SetProperty(ref uiCollectionToBeRendered, value);
                log.LogMethodExit(uiCollectionToBeRendered);
            }
        }
        public Dictionary<string, CustomDataGridColumnElement> HeaderCollection
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(headerCollection);
                return headerCollection;
            }
            set
            {
                log.LogMethodEntry(headerCollection, value);
                SetProperty(ref headerCollection, value);
                ResetColumns();
                log.LogMethodExit(headerCollection);
            }
        }
        public bool SelectAll
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectAll);
                return selectAll;
            }
            private set
            {
                log.LogMethodEntry(selectAll, value);
                SetProperty(ref selectAll, value);
                log.LogMethodExit(selectAll);
            }
        }
        public ObservableCollection<object> SelectedItems
        {
            get
            { return selectedItems; }
            set
            {
                if (!Equals(selectedItems, value))
                {
                    UpdateSelectedItems(false);
                    selectedItems = value;
                    OnPropertyChanged();
                    SelectAll = false;
                    UpdateSelectedItems(true);
                }
            }
        }
        public object SelectedItem
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedItem);
                return selectedItem;
            }
            set
            {
                log.LogMethodEntry(selectedItem, value);
                SetProperty(ref selectedItem, value);
                log.LogMethodExit(selectedItem);
            }
        }
        public ICommand DeleteCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(deleteCommand);
                return deleteCommand;
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
        public ICommand SelectionChangedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectionChangedCommand);
                return selectionChangedCommand;
            }
        }
        public ICommand ButtonClickedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(buttonClickedCommand);
                return buttonClickedCommand;
            }
        }
        public ICommand SelectAllCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectAllCommand);
                return selectAllCommand;
            }
        }
        public ICommand ActionsCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(actionsCommand);
                return actionsCommand;
            }
        }
        public ICommand CheckBoxandGridRowLoadedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(checkBoxandGridRowLoadedCommand);
                return checkBoxandGridRowLoadedCommand;
            }
        }
        public ICommand CheckBoxClickedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(checkBoxClickedCommand);
                return checkBoxClickedCommand;
            }
        }
        public ICommand RadioButtonClickedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(radioButtonClickedCommand);
                return radioButtonClickedCommand;
            }
        }
        public ICommand SearchCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(searchCommand);
                return searchCommand;
            }
        }
        public ICommand ComboSelectionChangedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(comboSelectionChangedCommand);
                return comboSelectionChangedCommand;
            }
        }
        #endregion

        #region Methods
        private void OnLoaded(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                customDataGrid = parameter as CustomDataGridUserControl;
                customDataGrid.dataGrid.Sorting -= OnDataGridSorting;
                customDataGrid.dataGrid.Sorting += OnDataGridSorting;
                customDataGrid.dataGrid.LoadingRowDetails += OnLoadingRowDetails;
                customDataGrid.dataGrid.PreviewMouseUp += OnDataGridPreviewMouseUp;
                SetColumns();
                UpdateSelectedItems(true);
                RaiseSelectionChangedEvent();
            }
            log.LogMethodExit();
        }
        private void RaiseSelectedItemsChangedEvent()
        {
            log.LogMethodEntry();
            if (SelectOption == SelectOption.CheckBox && customDataGrid != null)
            {
                customDataGrid.RaiseSelectedItemsChangedEvent();
            }
            log.LogMethodExit();
        }
        private void RaiseSelectionChangedEvent()
        {
            log.LogMethodEntry();
            if (SelectOption != SelectOption.CheckBox && customDataGrid != null)
            {
                customDataGrid.RaiseSelectionChangedEvent();
            }
            log.LogMethodExit();
        }
        private void OnDataGridPreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (rowDetailsVisibilityMode == DataGridRowDetailsVisibilityMode.Visible)
            {
                FrameworkElement frameworkElement = e.OriginalSource as FrameworkElement;
                if (frameworkElement != null)
                {
                    Button button = frameworkElement.TemplatedParent as Button;
                    if (button != null && button.Command != null && uiCollectionToBeRendered.Contains(button.DataContext))
                    {
                        DataGridRow dataGridRow = customDataGrid.dataGrid.ItemContainerGenerator.ContainerFromItem(button.DataContext) as DataGridRow;
                        if (dataGridRow != null)
                        {
                            DataGridCellsPresenter cellsPresenter = GetVisualChild(dataGridRow);
                            if (cellsPresenter != null)
                            {
                                DataGridCell dataGridCell = cellsPresenter.ItemContainerGenerator.ContainerFromItem(button.DataContext) as DataGridCell;
                                if (dataGridCell != null && dataGridCell.Column != null)
                                {
                                    customDataGrid.dataGrid.CurrentColumn = dataGridCell.Column;
                                    button.Command.Execute(button.DataContext);
                                    e.Handled = true;
                                }
                            }
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
        private childItem FindVisualChild<childItem>(DependencyObject obj) where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                {
                    return (childItem)child;
                }
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null)
                    {
                        return childOfChild;
                    }
                }
            }
            return null;
        }
        private void OnLoadingRowDetails(object sender, DataGridRowDetailsEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (rowDetailsVisibilityMode == DataGridRowDetailsVisibilityMode.Visible && e.DetailsElement != null)
            {
                CustomDataGridUserControl dataGridUserControl = e.DetailsElement.FindName("ChildCustomDataGridUserControl") as CustomDataGridUserControl;
                if (dataGridUserControl != null)
                {
                    CustomDataGridVM customDataGrid = SetRowDetailsCustomDataGridVM(e.Row.DataContext);
                    if (customDataGrid != null && !(dataGridUserControl.DataContext is CustomDataGridVM))
                    {
                        dataGridUserControl.DataContext = customDataGrid;
                        customDataGrid.OnLoaded(dataGridUserControl);
                    }
                }
            }
            log.LogMethodExit();
        }
        public void SortBasedOnColumn(int columnIndex, ListSortDirection? sortDirection)
        {
            log.LogMethodEntry(columnIndex, sortDirection);
            if (columnIndex > -1 && customDataGrid != null && customDataGrid.dataGrid != null
                && customDataGrid.dataGrid.Columns.Count > columnIndex)
            {
                DataGridColumn currentDataGridColumn = customDataGrid.dataGrid.Columns[columnIndex];
                if (currentDataGridColumn != null)
                {
                    if (currentDataGridColumn.SortDirection != sortDirection)
                    {
                        currentDataGridColumn.SortDirection = sortDirection;
                    }
                    if (enableInternalSorting)
                    {
                        OnDataGridSorting(customDataGrid.dataGrid, new DataGridSortingEventArgs(currentDataGridColumn));
                    }
                }
            }
            log.LogMethodExit();
        }
        private void OnDataGridSorting(object sender, DataGridSortingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            sortedColoumn = e.Column;
            if (sortedColoumn.CanUserSort && !string.IsNullOrWhiteSpace(sortedColoumn.SortMemberPath))
            {
                if (enableInternalSorting)
                {
                    int index = customDataGrid.dataGrid.Columns.IndexOf(e.Column);
                    if (index != -1)
                    {
                        List<KeyValuePair<string, CustomDataGridColumnElement>> header = headerCollection.ToList();
                        CustomDataGridColumnElement customDataGridColumn = header[index].Value;
                        List<object> searchedItems = uiCollectionToBeRendered.ToList();
                        if (customDataGridColumn != null && customDataGridColumn.Converter != null)
                        {
                            IValueConverter valueConverter = customDataGridColumn.Converter as IValueConverter;
                            SecondarySourceConverter secondarySourceConverter = null;
                            IMultiValueConverter multiValueConverter = null;
                            if (valueConverter == null)
                            {
                                secondarySourceConverter = customDataGridColumn.Converter as SecondarySourceConverter;
                                if (secondarySourceConverter == null)
                                {
                                    multiValueConverter = customDataGridColumn.Converter as IMultiValueConverter;
                                }
                            }
                            Dictionary<object, object> valuePairs = new Dictionary<object, object>();
                            bool isInstanceProperty = false;
                            List<object> sortedItems = new List<object>();
                            Type propertyType = null;
                            foreach (object item in searchedItems)
                            {
                                if (header[index].Key.Contains("."))
                                {
                                    isInstanceProperty = true;
                                    break;
                                }
                                object convertedObj = null;
                                if (valueConverter != null)
                                {
                                    propertyType = item.GetType().GetProperty(header[index].Key).GetType();
                                    convertedObj = valueConverter.Convert(item.GetType().GetProperty(header[index].Key).GetValue(item), null, customDataGridColumn.ConverterParameter, null);
                                }
                                else if (secondarySourceConverter != null)
                                {
                                    convertedObj = secondarySourceConverter.Convert(new object[] { item }, null, customDataGridColumn.ConverterParameter, null);
                                }
                                else if (multiValueConverter != null)
                                {
                                    convertedObj = multiValueConverter.Convert(new object[] { item.GetType().GetProperty(header[index].Key).GetValue(item) }, null, customDataGridColumn.ConverterParameter, null);
                                }
                                if (convertedObj != null)
                                {
                                    string convertedObjStringValue = convertedObj.ToString();
                                    int intValue;
                                    DateTime dateTimeValue;
                                    double doubleValue;
                                    if (Int32.TryParse(convertedObjStringValue, out intValue))
                                    {
                                        valuePairs.Add(item, intValue);
                                    }
                                    else if (Double.TryParse(convertedObjStringValue, out doubleValue))
                                    {
                                        valuePairs.Add(item, doubleValue);
                                    }
                                    else if (DateTime.TryParse(convertedObjStringValue, out dateTimeValue))
                                    {
                                        valuePairs.Add(item, dateTimeValue);
                                    }
                                    else
                                    {
                                        valuePairs.Add(item, convertedObjStringValue);
                                    }
                                }
                                else
                                {
                                    valuePairs.Add(item, convertedObj);
                                }
                            }
                            if (!isInstanceProperty)
                            {
                                bool ascending = false;
                                if (valuePairs.Any(x => x.Value is Int32) && !valuePairs.All(x => x.Value is Int32))
                                {
                                    valuePairs = valuePairs.ToDictionary(x => x.Key, y => y.Value.ToString() as object);
                                }
                                else if (valuePairs.Any(x => x.Value is Double) && !valuePairs.All(x => x.Value is Double))
                                {
                                    valuePairs = valuePairs.ToDictionary(x => x.Key, y => y.Value.ToString() as object);
                                }
                                if (e.Column.SortDirection == null || e.Column.SortDirection == ListSortDirection.Descending)
                                {
                                    sortedItems = valuePairs.OrderBy(x => x.Value).Select(k => k.Key).ToList();
                                    ascending = true;
                                }
                                else if (e.Column.SortDirection == ListSortDirection.Ascending)
                                {
                                    sortedItems = valuePairs.OrderByDescending(x => x.Value).Select(k => k.Key).ToList();
                                }
                                UICollectionToBeRendered = new ObservableCollection<object>(sortedItems);
                                e.Column.SortDirection = ascending ? ListSortDirection.Ascending : ListSortDirection.Descending;
                                e.Handled = true;
                            }
                            else if (customDataGrid.dataGrid.Items.Count > 0 && selectOption != SelectOption.ManualSelectionOnly)
                            {
                                SelectedItem = customDataGrid.dataGrid.Items[0];
                                customDataGrid.dataGrid.ScrollIntoView(selectedItem);
                            }
                        }
                    }
                }
                else
                {
                    e.Column.SortDirection = e.Column.SortDirection == null || e.Column.SortDirection == ListSortDirection.Descending ? ListSortDirection.Ascending : ListSortDirection.Descending;
                    e.Handled = true;
                }
                if (customDataGrid != null)
                {
                    customDataGrid.RaiseDataGridSortingEvent();
                }
            }
            log.LogMethodExit();
        }
        private void OnComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            CustomComboBox customComboBox = sender as CustomComboBox;
            if (customComboBox != null && customComboBox.DataContext != null)
            {
                BindingExpression bindingExpression = customComboBox.GetBindingExpression(CustomComboBox.SelectedItemProperty);
                if (bindingExpression != null && !string.IsNullOrEmpty(bindingExpression.ParentBinding.Path.Path))
                {
                    int columnIndex = this.headerCollection.Keys.ToList().IndexOf(this.headerCollection.Keys.FirstOrDefault(k => k == bindingExpression.ParentBinding.Path.Path));
                    comboBoxSelectedRow = this.customDataGrid.dataGrid.ItemContainerGenerator.ContainerFromItem(customComboBox.DataContext) as DataGridRow;
                    if (comboBoxSelectedRow != null)
                    {
                        this.selectedCustomComboBoxModel = new CustomDataGridComboBoxSelectionModel(columnIndex, customComboBox);
                        this.customDataGrid.RaiseComboBoxSelectionChangedEvent();
                    }
                }
            }
            log.LogMethodExit();
        }
        private void ResetColumns()
        {
            log.LogMethodEntry();
            if (customDataGrid != null && customDataGrid.dataGrid != null)
            {
                if(customDataGrid.dataGrid.Columns.Any())
                {
                    customDataGrid.dataGrid.Columns.Clear();
                }
                SetColumns();
            }
            log.LogMethodExit();
        }
        private void SetColumns()
        {
            log.LogMethodEntry();
            if (customDataGrid != null && customDataGrid.dataGrid.Columns.Count == 0)
            {
                foreach (KeyValuePair<string, CustomDataGridColumnElement> dataEntry in this.headerCollection)
                {
                    if (!string.IsNullOrEmpty(dataEntry.Key))
                    {
                        DataGridTemplateColumn templateColumn = new DataGridTemplateColumn();
                        templateColumn.Header = MessageViewContainerList.GetMessage(this.ExecutionContext, dataEntry.Key, null);
                        if (!string.IsNullOrEmpty(dataEntry.Value.Heading))
                        {
                            templateColumn.Header = MessageViewContainerList.GetMessage(this.ExecutionContext, dataEntry.Value.Heading, null);
                        }
                        DataTemplate dataTemplate = new DataTemplate();
                        FrameworkElementFactory factory = null;
                        if (templateColumn.CanUserSort != dataEntry.Value.CanUserSort)
                        {
                            templateColumn.CanUserSort = dataEntry.Value.CanUserSort;
                        }
                        if (templateColumn.CanUserSort)
                        {
                            templateColumn.SortMemberPath = dataEntry.Key;
                        }
                        Style style = new Style()
                        {
                            BasedOn = (Style)customDataGrid.Resources["CustomDataGridColumnHeader"],
                            TargetType = typeof(DataGridColumnHeader)
                        };
                        style.Setters.Add(new Setter(DataGridColumnHeader.HorizontalAlignmentProperty, dataEntry.Value.DataGridColumnHorizontalAlignment));
                        templateColumn.HeaderStyle = style;
                        switch (dataEntry.Value.Type)
                        {
                            case DataEntryType.TextBlock:
                                {
                                    templateColumn.IsReadOnly = true;
                                    factory = new FrameworkElementFactory(typeof(CustomTextBlock));
                                    if (dataEntry.Value.Converter != null)
                                    {
                                        if (dataEntry.Value.Converter is IMultiValueConverter)
                                        {
                                            MultiBinding textBlockBinding = new MultiBinding();
                                            {
                                                textBlockBinding.Bindings.Add(new Binding(dataEntry.Key));
                                            }
                                            textBlockBinding.Converter = dataEntry.Value.Converter as IMultiValueConverter;
                                            if (dataEntry.Value.ConverterParameter != null)
                                            {
                                                textBlockBinding.ConverterParameter = dataEntry.Value.ConverterParameter;
                                            }
                                            factory.SetBinding(CustomTextBlock.TextProperty, textBlockBinding);
                                        }
                                        else
                                        {
                                            Binding textBlockBinding = new Binding(dataEntry.Key);
                                            textBlockBinding.Converter = dataEntry.Value.Converter as IValueConverter;
                                            if (dataEntry.Value.ConverterParameter != null)
                                            {
                                                textBlockBinding.ConverterParameter = dataEntry.Value.ConverterParameter;
                                            }
                                            factory.SetBinding(CustomTextBlock.TextProperty, textBlockBinding);
                                        }
                                    }
                                    else if (dataEntry.Value.Properties != null && dataEntry.Value.Properties.Count > 0)
                                    {
                                        dataEntry.Value.ConverterParameter = dataEntry.Value;
                                        dataEntry.Value.Converter = new MathConverter();
                                        MultiBinding textBlockBinding = new MultiBinding();
                                        textBlockBinding.Bindings.Add(new Binding());
                                        textBlockBinding.ConverterParameter = dataEntry.Value.ConverterParameter;
                                        textBlockBinding.Converter = dataEntry.Value.Converter as IMultiValueConverter;
                                        factory.SetBinding(CustomTextBlock.TextProperty, textBlockBinding);
                                    }
                                    else if (dataEntry.Value.SecondarySource != null)
                                    {
                                        dataEntry.Value.ConverterParameter = new ObservableCollection<object>
                                        {
                                            collectionToBeRendered,
                                            dataEntry.Value,
                                            dataEntry.Key
                                        };
                                        dataEntry.Value.Converter = new SecondarySourceConverter();
                                        MultiBinding textBlockBinding = new MultiBinding();
                                        textBlockBinding.Bindings.Add(new Binding());
                                        textBlockBinding.ConverterParameter = dataEntry.Value.ConverterParameter;
                                        textBlockBinding.Converter = dataEntry.Value.Converter as IMultiValueConverter;
                                        factory.SetBinding(CustomTextBlock.TextProperty, textBlockBinding);
                                    }
                                    else if (!string.IsNullOrEmpty(dataEntry.Value.ChildOrSecondarySourcePropertyName)
                                        && !string.IsNullOrWhiteSpace(dataEntry.Value.ChildOrSecondarySourcePropertyName))
                                    {
                                        dataEntry.Value.ConverterParameter = dataEntry.Value;
                                        dataEntry.Value.Converter = new SumConverter();
                                        Binding textBlockBinding = new Binding(dataEntry.Key);
                                        if (!string.IsNullOrEmpty(dataEntry.Value.DataGridColumnStringFormat)
                                            && !string.IsNullOrWhiteSpace(dataEntry.Value.DataGridColumnStringFormat))
                                        {
                                            textBlockBinding.StringFormat = dataEntry.Value.DataGridColumnStringFormat;
                                        }
                                        textBlockBinding.ConverterParameter = dataEntry.Value.ConverterParameter;
                                        textBlockBinding.Converter = dataEntry.Value.Converter as IValueConverter;
                                        factory.SetBinding(CustomTextBlock.TextProperty, textBlockBinding);
                                    }
                                    else
                                    {
                                        dataEntry.Value.ConverterParameter = dataEntry.Value;
                                        dataEntry.Value.Converter = new CheckDateMinConverter();
                                        Binding textBlockBinding = new Binding(dataEntry.Key);
                                        if (!string.IsNullOrEmpty(dataEntry.Value.DataGridColumnStringFormat)
                                            && !string.IsNullOrWhiteSpace(dataEntry.Value.DataGridColumnStringFormat))
                                        {
                                            textBlockBinding.StringFormat = dataEntry.Value.DataGridColumnStringFormat;
                                        }
                                        textBlockBinding.ConverterParameter = dataEntry.Value.ConverterParameter;
                                        textBlockBinding.Converter = dataEntry.Value.Converter as IValueConverter;
                                        factory.SetBinding(CustomTextBlock.TextProperty, textBlockBinding);
                                    }
                                    factory.AddHandler(CustomTextBlock.MouseUpEvent, new MouseButtonEventHandler((obj, e) => OnLargeContentMouseUp(obj, e)));
                                    factory.SetValue(CustomTextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
                                    factory.SetValue(CustomTextBlock.HorizontalAlignmentProperty, dataEntry.Value.DataGridColumnHorizontalAlignment);
                                    factory.SetValue(CustomTextBlock.FontWeightProperty, dataEntry.Value.FontWeight);
                                    factory.SetValue(CustomTextBlock.TextTrimmingProperty, TextTrimming.CharacterEllipsis);
                                    factory.SetValue(CustomTextBlock.IsEnabledProperty, dataEntry.Value.IsEnable);
                                    if (ismultiScreenRowTwo)
                                    {
                                        factory.SetValue(CustomTextBlock.TextSizeDependencyProperty, TextSize.XSmall);
                                    }
                                }
                                break;
                            case DataEntryType.TextBox:
                                {
                                    factory = new FrameworkElementFactory(typeof(CustomTextBox));
                                    factory.SetBinding(CustomTextBox.TextProperty, new Binding(dataEntry.Key)
                                    { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
                                    factory.SetValue(CustomTextBox.MaxLengthProperty, dataEntry.Value.MaxLength > -1 ?
                                        dataEntry.Value.MaxLength : int.MaxValue);
                                    factory.SetValue(CustomTextBox.VerticalAlignmentProperty, VerticalAlignment.Center);
                                    factory.SetValue(CustomTextBox.SizeDependencyProperty, Size.DataGridSize);
                                    factory.SetValue(CustomTextBox.ErrorTextVisibleDependencyProperty, false);
                                    factory.SetValue(CustomTextBox.IsEnabledProperty, dataEntry.Value.IsEnable);
                                    factory.SetValue(CustomTextBox.FontWeightProperty, dataEntry.Value.FontWeight);
                                    factory.SetValue(CustomTextBox.NumberKeyboardTypeDependencyProperty, dataEntry.Value.NumberKeyboardType);
                                    factory.AddHandler(CustomTextBlock.PreviewMouseUpEvent, new MouseButtonEventHandler((obj, e) => OnLargeContentMouseUp(obj, e)));
                                    factory.SetValue(CustomTextBox.IsReadOnlyProperty, dataEntry.Value.IsReadOnly ? dataEntry.Value.IsReadOnly : isReadOnly);
                                }
                                break;
                            case DataEntryType.ComboBox:
                                {
                                    factory = new FrameworkElementFactory(typeof(CustomComboBox));
                                    factory.AddHandler(CustomComboBox.SelectionChangedEvent, new SelectionChangedEventHandler((obj, e) => OnComboBoxSelectionChanged(obj, e)));
                                    if (dataEntry.Value.Converter != null)
                                    {
                                        if (dataEntry.Value.Converter is IMultiValueConverter)
                                        {
                                            MultiBinding comboBoxBinding = new MultiBinding()
                                            {
                                                Mode = BindingMode.TwoWay,
                                                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                                            };
                                            comboBoxBinding.Bindings.Add(new Binding(dataEntry.Key));
                                            comboBoxBinding.Converter = dataEntry.Value.Converter as IMultiValueConverter;
                                            if (dataEntry.Value.ConverterParameter != null)
                                            {
                                                comboBoxBinding.ConverterParameter = dataEntry.Value.ConverterParameter;
                                            }
                                            factory.SetBinding(CustomComboBox.SelectedItemProperty, comboBoxBinding);
                                        }
                                        else
                                        {
                                            Binding comboBoxBinding = new Binding(dataEntry.Key)
                                            {
                                                Mode = BindingMode.TwoWay,
                                                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                                            };
                                            comboBoxBinding.Converter = dataEntry.Value.Converter as IValueConverter;
                                            if (dataEntry.Value.ConverterParameter != null)
                                            {
                                                comboBoxBinding.ConverterParameter = dataEntry.Value.ConverterParameter;
                                            }
                                            factory.SetBinding(CustomComboBox.SelectedItemProperty, comboBoxBinding);
                                        }
                                    }
                                    else
                                    {
                                        factory.SetBinding(CustomComboBox.SelectedItemProperty, new Binding(dataEntry.Key)
                                        {
                                            Mode = BindingMode.TwoWay,
                                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                                        });
                                    }
                                    factory.SetBinding(CustomComboBox.ItemsSourceProperty, new Binding("Options")
                                    {
                                        Source = dataEntry.Value,
                                    });
                                    factory.SetValue(CustomComboBox.DisplayMemberPathProperty, dataEntry.Value.DisplayMemberPath);
                                    factory.SetValue(CustomComboBox.IsEditableProperty, true);
                                    factory.SetValue(CustomComboBox.VerticalAlignmentProperty, VerticalAlignment.Center);
                                    factory.SetValue(CustomComboBox.SizeDependencyProperty, Size.DataGridSize);
                                    factory.SetValue(CustomComboBox.ErrorTextVisibleDependencyProperty, false);
                                    factory.SetValue(CustomComboBox.FontWeightProperty, dataEntry.Value.FontWeight);
                                    if (!dataEntry.Value.IsEnable)
                                    {
                                        factory.SetValue(CustomComboBox.IsEnabledProperty, dataEntry.Value.IsEnable);
                                    }
                                    else if (dataEntry.Value.IsReadOnly)
                                    {
                                        factory.SetValue(CustomComboBox.IsEnabledProperty, !dataEntry.Value.IsReadOnly);
                                    }
                                    else
                                    {
                                        factory.SetValue(CustomComboBox.IsEnabledProperty, !isReadOnly);
                                    }
                                }
                                break;
                            case DataEntryType.CheckBox:
                                {
                                    string propertyName = dataEntry.Key;
                                    if (!string.IsNullOrWhiteSpace(dataEntry.Value.SourcePropertyName) && collectionToBeRendered != null && collectionToBeRendered.Any()
                                        && collectionToBeRendered[0].GetType().GetProperty(propertyName) == null &&
                                        collectionToBeRendered[0].GetType().GetProperty(dataEntry.Value.SourcePropertyName) != null)
                                    {
                                        propertyName = dataEntry.Value.SourcePropertyName;
                                    }
                                    factory = new FrameworkElementFactory(typeof(CustomCheckBox));
                                    Binding checkBoxBinding = new Binding(propertyName) { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
                                    if (dataEntry.Value.Converter != null)
                                    {
                                        if (dataEntry.Value.Converter is IMultiValueConverter)
                                        {
                                            MultiBinding multiCheckBoxBinding = new MultiBinding();
                                            multiCheckBoxBinding.Bindings.Add(checkBoxBinding);
                                            multiCheckBoxBinding.Converter = dataEntry.Value.Converter as IMultiValueConverter;
                                            if (dataEntry.Value.ConverterParameter != null)
                                            {
                                                multiCheckBoxBinding.ConverterParameter = dataEntry.Value.ConverterParameter;
                                            }
                                            factory.SetBinding(CustomCheckBox.IsCheckedProperty, multiCheckBoxBinding);
                                        }
                                        else
                                        {
                                            checkBoxBinding.Converter = dataEntry.Value.Converter as IValueConverter;
                                            if (dataEntry.Value.ConverterParameter != null)
                                            {
                                                checkBoxBinding.ConverterParameter = dataEntry.Value.ConverterParameter;
                                            }
                                            factory.SetBinding(CustomCheckBox.IsCheckedProperty, checkBoxBinding);
                                        }
                                    }
                                    else
                                    {
                                        factory.SetBinding(CustomCheckBox.IsCheckedProperty, checkBoxBinding);
                                    }
                                    factory.SetValue(CustomCheckBox.VerticalAlignmentProperty, VerticalAlignment.Center);
                                    factory.SetValue(CustomCheckBox.HorizontalAlignmentProperty, dataEntry.Value.DataGridColumnHorizontalAlignment);
                                    factory.SetValue(CustomCheckBox.ErrorTextVisibleDependencyProperty, false);
                                    factory.SetValue(CustomCheckBox.FontWeightProperty, dataEntry.Value.FontWeight);
                                    factory.SetValue(CustomCheckBox.SizeDependencyProperty, Size.DataGridSize);
                                    if (!dataEntry.Value.IsEnable)
                                    {
                                        factory.SetValue(CustomCheckBox.IsEnabledProperty, dataEntry.Value.IsEnable);
                                    }
                                    else if (dataEntry.Value.IsReadOnly)
                                    {
                                        factory.SetValue(CustomCheckBox.IsEnabledProperty, !dataEntry.Value.IsReadOnly);
                                    }
                                    else
                                    {
                                        factory.SetValue(CustomCheckBox.IsEnabledProperty, !isReadOnly);
                                    }
                                }
                                break;
                            case DataEntryType.RadioButton:
                                {
                                    FrameworkElementFactory radioButtonfactory = new FrameworkElementFactory(typeof(RadioButton));
                                    radioButtonfactory.SetBinding(RadioButton.IsCheckedProperty, new Binding(dataEntry.Key)
                                    { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
                                    radioButtonfactory.SetBinding(RadioButton.CommandProperty, new Binding("RadioButtonClickedCommand") { Source = this });
                                    radioButtonfactory.SetBinding(RadioButton.CommandParameterProperty, new Binding()
                                    { RelativeSource = new RelativeSource() { Mode = RelativeSourceMode.Self } });
                                    factory = new FrameworkElementFactory(typeof(Viewbox));
                                    factory.SetValue(Viewbox.VerticalAlignmentProperty, VerticalAlignment.Center);
                                    factory.SetValue(Viewbox.HorizontalAlignmentProperty, dataEntry.Value.DataGridColumnHorizontalAlignment);
                                    factory.SetValue(Viewbox.WidthProperty, (double)44);
                                    factory.SetValue(Viewbox.HeightProperty, (double)44);
                                    factory.AppendChild(radioButtonfactory);
                                }
                                break;
                            case DataEntryType.DatePicker:
                                {
                                    factory = new FrameworkElementFactory(typeof(CustomTextBoxDatePicker));
                                    Binding datePickerBinding = new Binding(dataEntry.Key) { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
                                    if (!string.IsNullOrEmpty(dataEntry.Value.DataGridColumnStringFormat)
                                        && !string.IsNullOrWhiteSpace(dataEntry.Value.DataGridColumnStringFormat))
                                    {
                                        datePickerBinding.StringFormat = dataEntry.Value.DataGridColumnStringFormat;
                                    }
                                    factory.SetBinding(CustomTextBoxDatePicker.TextProperty, datePickerBinding);
                                    factory.SetValue(CustomTextBoxDatePicker.VerticalAlignmentProperty, VerticalAlignment.Center);
                                    factory.SetValue(CustomTextBoxDatePicker.SizeDependencyProperty, Size.DataGridSize);
                                    factory.SetValue(CustomTextBoxDatePicker.ErrorTextVisibleDependencyProperty, false);
                                    factory.SetValue(CustomTextBoxDatePicker.IsEnabledProperty, dataEntry.Value.IsEnable);
                                    factory.SetValue(CustomTextBoxDatePicker.FontWeightProperty, dataEntry.Value.FontWeight);
                                    if (dataEntry.Value.IsReadOnly)
                                    {
                                        factory.SetValue(CustomTextBoxDatePicker.IsReadOnlyProperty, dataEntry.Value.IsReadOnly);
                                    }
                                    else
                                    {
                                        factory.SetValue(CustomTextBoxDatePicker.IsReadOnlyProperty, isReadOnly);
                                    }

                                }
                                break;
                            case DataEntryType.Button:
                                {
                                    templateColumn.Header = string.Empty;
                                    switch (dataEntry.Value.ActionButtonType)
                                    {
                                        case DataGridButtonType.Content:
                                            {
                                                factory = new FrameworkElementFactory(typeof(CustomActionButton));
                                                Binding contentBinding = new Binding(dataEntry.Key);
                                                if (!string.IsNullOrEmpty(dataEntry.Value.Heading))
                                                {
                                                    factory.SetValue(CustomActionButton.ContentProperty, dataEntry.Value.Heading);
                                                }
                                                else
                                                {
                                                    factory.SetValue(CustomActionButton.ContentProperty, dataEntry.Key);
                                                }
                                                factory.SetValue(CustomActionButton.ActionStyleDependencyProperty, ActionStyle.Active);
                                                factory.SetValue(CustomActionButton.IsEnabledProperty, dataEntry.Value.IsEnable);
                                                factory.SetValue(CustomActionButton.PaddingProperty, dataEntry.Value.ContentButtonPadding);
                                                factory.SetValue(CustomActionButton.TextSizeDependencyProperty, dataEntry.Value.ContentButtonTextSize);
                                                factory.SetValue(CustomActionButton.TextTrimmingDependencyProperty, dataEntry.Value.ContentButtonTextTrimming);
                                                factory.SetValue(CustomActionButton.HeightProperty, (double)Application.Current.Resources["CustomDataGrid.ActionButton.Width"]);
                                                factory.SetValue(CustomActionButton.CommandParameterProperty, new Binding());
                                                factory.SetValue(CustomActionButton.CommandProperty, ButtonClickedCommand);
                                            }
                                            break;
                                        case DataGridButtonType.Add:
                                            factory = SetCustomActionButtonStyle(dataEntry.Value.IsEnable, this.customDataGrid.FindResource("AddButtonStyle"));
                                            break;
                                        case DataGridButtonType.Remove:
                                            factory = SetCustomActionButtonStyle(dataEntry.Value.IsEnable, this.customDataGrid.FindResource("RemoveButtonStyle"));
                                            break;
                                        case DataGridButtonType.More:
                                            factory = SetCustomActionButtonStyle(dataEntry.Value.IsEnable, this.customDataGrid.FindResource("MoreButtonStyle"));
                                            break;
                                        case DataGridButtonType.Edit:
                                            factory = SetCustomActionButtonStyle(dataEntry.Value.IsEnable, this.customDataGrid.FindResource("EditButtonStyle"));
                                            break;
                                        case DataGridButtonType.Custom:
                                            if (!string.IsNullOrWhiteSpace(dataEntry.Value.StyleName))
                                            {
                                                factory = SetCustomActionButtonStyle(dataEntry.Value.IsEnable, Application.Current.FindResource(dataEntry.Value.StyleName));
                                            }
                                            else if (dataEntry.Value.Converter != null)
                                            {
                                                factory = SetCustomActionButtonStyle(dataEntry.Value.IsEnable);
                                                if (dataEntry.Value.Converter is IValueConverter)
                                                {
                                                    Binding binding = new Binding(dataEntry.Key) { Converter = dataEntry.Value.Converter as IValueConverter };
                                                    if (dataEntry.Value.ConverterParameter != null)
                                                    {
                                                        binding.ConverterParameter = dataEntry.Value.ConverterParameter;
                                                    }
                                                    factory.SetBinding(Button.StyleProperty, binding);
                                                }
                                            }
                                            break;
                                    }

                                }
                                break;
                        }
                        dataTemplate.VisualTree = factory;
                        templateColumn.CellTemplate = dataTemplate;
                        if (dataEntry.Value.DataGridColumnFixedSize > 1)
                        {
                            templateColumn.Width = new DataGridLength(dataEntry.Value.DataGridColumnFixedSize, DataGridLengthUnitType.Pixel);
                        }
                        else
                        {
                            templateColumn.Width = new DataGridLength(1, dataEntry.Value.DataGridColumnLengthUnitType);
                        }
                        customDataGrid.dataGrid.Columns.Add(templateColumn);
                        if (sortedColoumn != null && sortedColoumn.Header != null && templateColumn != null &&
                           sortedColoumn.Header.ToString() == templateColumn.Header.ToString())
                        {
                            sortedColoumn = templateColumn;
                        }
                    }
                    if (comboGroupVM != null && comboGroupVM.ComboList != null && comboGroupVM.ComboList.Count > 0)
                    {
                        OnComboSelectionChanged(null);
                    }
                }
            }
            if (UICollectionToBeRendered != null && UICollectionToBeRendered.Any())
            {
                if (selectOption == SelectOption.None && selectedItem == null && selectOption != SelectOption.ManualSelectionOnly)
                {
                    SelectedItem = UICollectionToBeRendered.First();
                    customDataGrid.dataGrid.ScrollIntoView(selectedItem);
                }
            }
            log.LogMethodExit();
        }
        private FrameworkElementFactory SetCustomActionButtonStyle(bool isEnable, object style = null)
        {
            log.LogMethodEntry(isEnable, style);
            FrameworkElementFactory factory = new FrameworkElementFactory(typeof(Button));
            factory.SetValue(Button.IsEnabledProperty, isEnable);
            if (style != null)
            {
                factory.SetValue(Button.StyleProperty, style);
            }
            factory.SetValue(Button.CommandParameterProperty, new Binding());
            factory.SetValue(Button.CommandProperty, ButtonClickedCommand);
            return factory;
        }
        private void OnLargeContentMouseUp(object sender, MouseButtonEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            CustomTextBlock customTextBlock = sender as CustomTextBlock;
            CustomTextBox customTextBox = sender as CustomTextBox;
            double actualWidth = 0;
            string text = string.Empty;
            Typeface typeface = null;
            FormattedText formattedText = null;
            if (customTextBlock != null)
            {
                actualWidth = customTextBlock.ActualWidth;
                text = customTextBlock.Text;
                typeface = new Typeface(customTextBlock.FontFamily, customTextBlock.FontStyle, customTextBlock.FontWeight, customTextBlock.FontStretch);
                formattedText = new FormattedText(customTextBlock.Text, System.Threading.Thread.CurrentThread.CurrentCulture, customTextBlock.FlowDirection,
                   typeface, customTextBlock.FontSize, customTextBlock.Foreground);
            }
            else if (customTextBox != null && (customTextBox.IsReadOnly || !customTextBox.IsEnabled))
            {
                actualWidth = customTextBox.ActualWidth;
                text = customTextBox.Text;
                typeface = new Typeface(customTextBox.FontFamily, customTextBox.FontStyle, customTextBox.FontWeight, customTextBox.FontStretch);
                formattedText = new FormattedText(customTextBox.Text, System.Threading.Thread.CurrentThread.CurrentCulture, customTextBox.FlowDirection,
                   typeface, customTextBox.FontSize, customTextBox.Foreground);
            }
            if (formattedText != null && formattedText.Width > actualWidth)
            {
                GenericMessagePopupView messagePopupView = new GenericMessagePopupView();
                GenericMessagePopupVM messagePopupVM = new GenericMessagePopupVM(ExecutionContext);
                if (this.customDataGrid.dataGrid.CurrentCell != null && this.customDataGrid.dataGrid.CurrentCell.Column != null)
                {
                    int index = this.customDataGrid.dataGrid.CurrentCell.Column.DisplayIndex;
                    if (index > -1 && headerCollection.Count > index)
                    {
                        string heading = headerCollection.Values.ToList()[index].Heading;
                        messagePopupVM.Heading = string.IsNullOrEmpty(heading) ? headerCollection.Keys.ToList()[index] : heading;
                    }
                }
                if (string.IsNullOrEmpty(messagePopupVM.Heading))
                {
                    messagePopupVM.Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Full Content");
                }
                messagePopupVM.MessageButtonsType = MessageButtonsType.OK;
                messagePopupVM.CancelButtonText = MessageViewContainerList.GetMessage(ExecutionContext, "OK");
                messagePopupVM.Content = text;
                messagePopupView.DataContext = messagePopupVM;
                messagePopupView.ShowDialog();
            }
            log.LogMethodExit();
        }
        private CustomDataGridVM SetRowDetailsCustomDataGridVM(object item)
        {
            log.LogMethodEntry(item);
            CustomDataGridVM customDataGridVM = null;
            if (showRowDetails)
            {
                object childInstance = item.GetType().GetProperty(rowDetailsProperty).GetValue(item);
                List<object> childCollection = new List<object>();
                if (childInstance != null)
                {
                    if (childInstance is IEnumerable<object>)
                    {
                        childCollection = (item.GetType().GetProperty(rowDetailsProperty).GetValue(item) as IEnumerable<object>).ToList();
                    }
                    else
                    {
                        childCollection = new List<object>() { childInstance };
                    }
                }
                Dictionary<string, CustomDataGridColumnElement> keyValues = new Dictionary<string, CustomDataGridColumnElement>();
                foreach (KeyValuePair<string, CustomDataGridColumnElement> keyValuePair in rowDetailsHeaderCollection)
                {
                    keyValues.Add(keyValuePair.Key, (CustomDataGridColumnElement)keyValuePair.Value.Clone());
                }
                customDataGridVM = new CustomDataGridVM(ExecutionContext)
                {
                    ShowRowDetails = false,
                    parentDataGridVM = this,
                    IsReadOnly = this.isReadOnly,
                    HeaderCollection = keyValues,
                    IsComboAndSearchVisible = false,
                    ShowHeader = showchildHeaderColumns,
                    VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                    RowDetailsVisibilityMode = this.rowDetailsVisibilityMode,
                    CollectionToBeRendered = new ObservableCollection<object>(childCollection),
                };
            }
            log.LogMethodExit();
            return customDataGridVM;
        }
        private void RefreshSelectedItems()
        {
            log.LogMethodEntry();
            List<object> items = new List<object>(selectedItems);
            foreach (object data in items)
            {
                DataGridRow dataGridRow = customDataGrid.dataGrid.ItemContainerGenerator.ContainerFromItem(data) as DataGridRow;
                if (dataGridRow != null)
                {
                    if (!customDataGrid.dataGrid.SelectedItems.Contains(data))
                    {
                        customDataGrid.dataGrid.SelectedItems.Add(data);
                    }
                }
            }
            log.LogMethodExit();
        }
        private void OnSelectionChanged(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (customDataGrid == null)
            {
                customDataGrid = parameter as CustomDataGridUserControl;
            }
            if (selectOption == SelectOption.None)
            {
                if (selectedItem != null)
                {
                    customDataGrid.dataGrid.UpdateLayout();
                    customDataGrid.dataGrid.ScrollIntoView(selectedItem);
                    if (showRowDetails && rowDetailsVisibilityMode == DataGridRowDetailsVisibilityMode.VisibleWhenSelected && parentSelectedItem != selectedItem)
                    {
                        parentSelectedItem = selectedItem;
                        SelectedChildCustomDataGridVM = SetRowDetailsCustomDataGridVM(selectedItem);
                    }
                }
            }
            else if (selectOption == SelectOption.CheckBox)
            {
                RefreshSelectedItems();
            }
            SetChildDataGridVM();
            RaiseSelectionChangedEvent();
            log.LogMethodExit();
        }
        private void SetChildDataGridVM()
        {
            log.LogMethodEntry();
            if (parentDataGridVM != null && rowDetailsVisibilityMode == DataGridRowDetailsVisibilityMode.Visible)
            {
                parentDataGridVM.childCustomDataGridVM = this;
            }
            log.LogMethodExit();
        }
        private static FrameworkElement GetTemplateChildByName(DependencyObject parent, string name)
        {
            int childnum = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childnum; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is FrameworkElement &&

                ((FrameworkElement)child).Name == name)
                {
                    return child as FrameworkElement;
                }
                else
                {
                    var s = GetTemplateChildByName(child, name);
                    if (s != null)
                        return s;
                }
            }
            return null;
        }
        private bool GetRowEnableValue(bool result)
        {
            log.LogMethodEntry();
            if (dataGridRowEnableWorksInReverse)
            {
                result = !result;
            }
            log.LogMethodExit();
            return result;
        }
        private void OnCheckBoxAndDataGridRowLoaded(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                CustomCheckBox checkBox = parameter as CustomCheckBox;
                DataGridRow gridRow = parameter as DataGridRow;
                if (checkBox != null && checkBox.DataContext != null && selectOption == SelectOption.CheckBox)
                {
                    if (!string.IsNullOrEmpty(dataGridRowEnableProperty))
                    {
                        object obj = checkBox.DataContext.GetType().GetProperty(dataGridRowEnableProperty).GetValue(checkBox.DataContext);
                        bool result;
                        if (obj != null && bool.TryParse(obj.ToString(), out result))
                        {
                            checkBox.IsEnabled = GetRowEnableValue(result);
                            if (checkBox.IsEnabled && selectAll)
                            {
                                AddToSelectedItems(checkBox);
                            }
                        }
                    }
                }
                else if (gridRow != null && gridRow.DataContext != null)
                {
                    if (!string.IsNullOrEmpty(dataGridRowEnableProperty))
                    {
                        PropertyInfo property = gridRow.DataContext.GetType().GetProperty(dataGridRowEnableProperty);
                        if (property != null)
                        {
                            object obj = property.GetValue(gridRow.DataContext);
                            bool result;
                            if (obj != null && bool.TryParse(obj.ToString(), out result))
                            {
                                gridRow.IsEnabled = GetRowEnableValue(result);
                            }
                        }
                    }
                    if (gridRow.IsEnabled)
                    {
                        if (selectAll)
                        {
                            gridRow.IsSelected = selectAll;
                        }
                        if (selectedItems.Contains(gridRow.DataContext) && selectOption == SelectOption.CheckBox)
                        {
                            gridRow.IsSelected = true;
                        }
                        if (gridRow.IsSelected)
                        {
                            checkBox = GetTemplateChildByName(gridRow, "chkDataGridRow") as CustomCheckBox;
                            if (checkBox != null)
                            {
                                AddToSelectedItems(checkBox);
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(dataGridRowReadOnlyProperty))
                    {
                        object obj = gridRow.DataContext.GetType().GetProperty(dataGridRowReadOnlyProperty).GetValue(gridRow.DataContext);
                        bool result;
                        if (obj != null && bool.TryParse(obj.ToString(), out result))
                        {
                            result = GetRowEnableValue(result);
                            foreach (KeyValuePair<string, CustomDataGridColumnElement> column in headerCollection)
                            {
                                int index = headerCollection.Keys.ToList().IndexOf(column.Key);
                                if (index > -1 && column.Value.Type != DataEntryType.Button)
                                {
                                    FrameworkElement customControl = GetCustomControls(gridRow, index);
                                    if (customControl != null)
                                    {
                                        if (customControl is CustomTextBox)
                                        {
                                            (customControl as CustomTextBox).IsReadOnly = !result;
                                        }
                                        else if (customControl is CustomComboBox)
                                        {
                                            (customControl as CustomComboBox).IsReadOnly = !result;
                                            currentCell.IsEnabled = result;
                                        }
                                        else if (customControl is CustomCheckBox)
                                        {
                                            (customControl as CustomCheckBox).IsEnabled = result;
                                            currentCell.IsEnabled = result;
                                        }
                                        else if (customControl is CustomRadioButton)
                                        {
                                            (customControl as CustomRadioButton).IsEnabled = result;
                                            currentCell.IsEnabled = result;
                                        }
                                        else if (customControl is CustomTextBoxDatePicker)
                                        {
                                            (customControl as CustomTextBoxDatePicker).IsReadOnly = !result;
                                        }
                                    }
                                    currentCell = null;
                                }
                            }
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
        private void AddToSelectedItems(CustomCheckBox checkBox)
        {
            log.LogMethodEntry();
            checkBox.IsChecked = true;
            if (!selectedItems.Contains(checkBox.DataContext))
            {
                selectedItems.Add(checkBox.DataContext);
            }
            if (!customDataGrid.dataGrid.SelectedItems.Contains(checkBox.DataContext))
            {
                customDataGrid.dataGrid.SelectedItems.Add(checkBox.DataContext);
            }
            log.LogMethodExit();
        }
        public bool SetEnableForIndividualCell(int columnIndex, bool isReadOnly)
        {
            log.LogMethodEntry(columnIndex, isReadOnly);
            bool result = false;
            FrameworkElement frameworkElement = this.GetCustomControls(comboBoxSelectedRow, columnIndex);
            if (frameworkElement is CustomTextBox)
            {
                (frameworkElement as CustomTextBox).IsReadOnly = isReadOnly;
            }
            else if (frameworkElement is CustomCheckBox)
            {
                (frameworkElement as CustomCheckBox).IsEnabled = !isReadOnly;
                currentCell.IsEnabled = !isReadOnly;
            }
            else if (frameworkElement is CustomComboBox)
            {
                (frameworkElement as CustomComboBox).IsEnabled = !isReadOnly;
                currentCell.IsEnabled = !isReadOnly;
            }
            else if (frameworkElement is CustomTextBoxDatePicker)
            {
                (frameworkElement as CustomTextBoxDatePicker).IsReadOnly = isReadOnly;
            }
            else if (frameworkElement is CustomRadioButton)
            {
                (frameworkElement as CustomRadioButton).IsEnabled = !isReadOnly;
                currentCell.IsEnabled = !isReadOnly;
            }
            log.LogMethodExit();
            return result;
        }
        public FrameworkElement GetCustomControls(DataGridRow row, int columnIndex)
        {
            log.LogMethodEntry(row, columnIndex);
            if (row == null)
            {
                return null;
            }
            DataGridCellsPresenter presenter = GetVisualChild(row);
            if (presenter == null)
            {
                return null;
            }
            FrameworkElement child = null;
            currentCell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex);
            if (currentCell == null && customDataGrid != null && customDataGrid.dataGrid != null && row != null
                && customDataGrid.dataGrid.Columns.Count > 0)
            {
                // now try to bring into view and retreive the cell
                customDataGrid.dataGrid.ScrollIntoView(row, customDataGrid.dataGrid.Columns[columnIndex]);
                currentCell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex);
            }
            if (currentCell != null)
            {
                child = GetVisualCustomControls(currentCell);
            }
            log.LogMethodExit();
            return child;
        }
        private FrameworkElement GetVisualCustomControls(Visual parent)
        {
            log.LogMethodEntry(parent);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            FrameworkElement child = null;
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as FrameworkElement;
                if (child != null && (child is CustomTextBox || child is CustomComboBox || child is CustomCheckBox || child is CustomTextBoxDatePicker || child is CustomRadioButton))
                {
                    break;
                }
                else
                {
                    child = GetVisualCustomControls(v);
                }
            }
            log.LogMethodExit();
            return child;
        }
        public DataGridCellsPresenter GetVisualChild(Visual parent)
        {
            log.LogMethodEntry(parent);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            DataGridCellsPresenter child = null;
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as DataGridCellsPresenter;
                if (child == null)
                {
                    child = GetVisualChild(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            log.LogMethodExit();
            return child;
        }
        private void OnButtonClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (customDataGrid != null)
            {
                buttonClickedModel = new CustomDataGridButtonModel(customDataGrid.dataGrid.CurrentCell.Column.DisplayIndex, parameter);
                if (parentDataGridVM != null)
                {
                    parentDataGridVM.childButtonClickedModel = buttonClickedModel;
                }
                else if (childButtonClickedModel != null)
                {
                    buttonClickedModel = childButtonClickedModel;
                }
                customDataGrid.RaiseButtonClickedEvent();
                buttonClickedModel = null;
                if (parentDataGridVM != null && parentDataGridVM.childButtonClickedModel != null)
                {
                    parentDataGridVM.childButtonClickedModel = null;
                }
                else if (childButtonClickedModel != null)
                {
                    childButtonClickedModel = null;
                }
            }
            log.LogMethodExit();
        }
        private void OnRadioButtonClicked(object parameter)
        {
            log.LogMethodEntry(parameter);

            log.LogMethodExit();
        }
        private void OnSelectAll(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                selectedItems.Clear();
                customDataGrid.dataGrid.UnselectAll();
                customDataGrid.dataGrid.SelectedItems.Clear();
                for (int i = 0; i < uiCollectionToBeRendered.Count; i++)
                {
                    bool result = selectAll;
                    bool execute = false;
                    if (!string.IsNullOrEmpty(dataGridRowEnableProperty))
                    {
                        object obj = uiCollectionToBeRendered[i].GetType().GetProperty(dataGridRowEnableProperty).GetValue(uiCollectionToBeRendered[i]);
                        if (obj != null && bool.TryParse(obj.ToString(), out result))
                        {
                            execute = true;
                        }
                    }
                    if ((!string.IsNullOrWhiteSpace(dataGridRowEnableProperty) && execute) || string.IsNullOrWhiteSpace(dataGridRowEnableProperty))
                    {
                        bool isChecked = !string.IsNullOrWhiteSpace(dataGridRowEnableProperty) && execute ? GetRowEnableValue(result)
                                : selectAll;
                        DataGridRow dataGridRow = customDataGrid.dataGrid.ItemContainerGenerator.ContainerFromItem(uiCollectionToBeRendered[i]) as DataGridRow;
                        if (dataGridRow != null)
                        {
                            if (isChecked && !selectAll)
                            {
                                isChecked = selectAll;
                            }
                            CustomCheckBox rowCheckBox = GetTemplateChildByName(dataGridRow, "chkDataGridRow") as CustomCheckBox;
                            if (rowCheckBox != null)
                            {
                                rowCheckBox.IsChecked = isChecked;
                            }
                            dataGridRow.IsSelected = isChecked;
                            if (isChecked && !customDataGrid.dataGrid.SelectedItems.Contains(uiCollectionToBeRendered[i]))
                            {
                                customDataGrid.dataGrid.SelectedItems.Add(uiCollectionToBeRendered[i]);
                            }
                        }
                        if (isChecked && selectAll && !selectedItems.Contains(uiCollectionToBeRendered[i]))
                        {
                            selectedItems.Add(uiCollectionToBeRendered[i]);
                        }
                    }
                }
                RaiseSelectedItemsChangedEvent();
            }
            log.LogMethodExit();
        }
        private void UpdateUI()
        {
            log.LogMethodEntry();
            SelectAll = false;
            UICollectionToBeRendered = new ObservableCollection<object>(this.collectionToBeRendered);
            if (selectOption == SelectOption.CheckBox && selectedItems != null && selectedItems.Any())
            {
                selectedItems.Clear();
                if (customDataGrid != null && customDataGrid.dataGrid != null && customDataGrid.dataGrid.SelectedItems != null)
                {
                    customDataGrid.dataGrid.SelectedItems.Clear();
                }
            }
            childCustomDataGridVM = null;
            log.LogMethodExit();
        }
        private void UpdateSelectedItems(bool select)
        {
            log.LogMethodEntry();
            if (selectOption == SelectOption.CheckBox && selectedItems != null && customDataGrid != null && customDataGrid.dataGrid != null)
            {
                customDataGrid.dataGrid.UnselectAll();
                customDataGrid.dataGrid.SelectedItems.Clear();
                for (int i = 0; i < selectedItems.Count; i++)
                {
                    DataGridRow dataGridRow = customDataGrid.dataGrid.ItemContainerGenerator.ContainerFromItem(selectedItems[i]) as DataGridRow;
                    if (dataGridRow != null)
                    {
                        CustomCheckBox rowCheckBox = GetTemplateChildByName(dataGridRow, "chkDataGridRow") as CustomCheckBox;
                        dataGridRow.IsSelected = select;
                        if (rowCheckBox != null)
                        {
                            rowCheckBox.IsChecked = select;
                        }
                        if (select)
                        {
                            customDataGrid.dataGrid.SelectedItems.Add(selectedItems[i]);
                        }
                    }
                }
                if (select)
                {
                    RaiseSelectedItemsChangedEvent();
                }
            }
            log.LogMethodExit();
        }
        private void OnDeleteClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                Button button = parameter as Button;
                if (button != null)
                {
                    customDataGrid.RaiseDeleteAllClickedEvent();
                }
                else
                {
                    selectedItem = parameter;
                    customDataGrid.RaiseDeleteClickedEvent();
                }
            }
            log.LogMethodExit();
        }
        private void OnSearchClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (customDataGrid != null)
            {
                customDataGrid.RaiseSearchClickedEvent();
            }
            log.LogMethodExit();
        }
        private void OnComboSelectionChanged(object parameter)
        {
            log.LogMethodEntry(parameter);
            List<object> searchedItems = collectionToBeRendered.ToList();
            foreach (ComboBoxField comboField in ComboGroupVM.ComboList)
            {
                if (comboField != null && !string.IsNullOrEmpty(comboField.SelectedItem))
                {
                    if (comboField.SelectedItem.ToLower() == MessageViewContainerList.GetMessage(ExecutionContext, "All", null).ToLower())
                    {
                        continue;
                    }
                    else if (searchedItems.Count > 0 && searchedItems[0].GetType() != null && searchedItems[0].GetType().GetProperty(comboField.Header) != null)
                    {
                        searchedItems = searchedItems.Where(o => o.GetType() != null && o.GetType().GetProperty(comboField.Header) != null
                    && o.GetType().GetProperty(comboField.Header).GetValue(o) != null
                    && o.GetType().GetProperty(comboField.Header).GetValue(o).ToString().ToLower() == comboField.SelectedItem.ToLower()).ToList();
                    }
                    else if (!string.IsNullOrEmpty(comboField.PropertyName))
                    {
                        CustomDataGridColumnElement customDataGridColumn = headerCollection.Values.Where(x => x.Heading.ToLower() == comboField.Header.ToLower()).FirstOrDefault();
                        if (customDataGridColumn != null && customDataGridColumn.Converter != null)
                        {
                            List<object> filteredList = new List<object>();
                            foreach (object item in searchedItems)
                            {
                                if (customDataGridColumn.Converter is IValueConverter)
                                {
                                    object convertedObj = (customDataGridColumn.Converter as IValueConverter).Convert(item.GetType().GetProperty(comboField.PropertyName).
                                        GetValue(item), null, customDataGridColumn.ConverterParameter, null);
                                    if (convertedObj != null && convertedObj.ToString().ToLower() == comboField.SelectedItem.ToLower())
                                    {
                                        filteredList.Add(item);
                                    }
                                }
                                else if (customDataGridColumn.Converter is IMultiValueConverter)
                                {
                                    if (customDataGridColumn.Converter is SecondarySourceConverter)
                                    {
                                        object convertedObj = (customDataGridColumn.Converter as IMultiValueConverter).Convert(
                                           new object[] { item }, null, customDataGridColumn.ConverterParameter, null);
                                        if (convertedObj != null && convertedObj.ToString().ToLower() == comboField.SelectedItem.ToLower())
                                        {
                                            filteredList.Add(item);
                                        }
                                    }
                                    else
                                    {
                                        object convertedObj = (customDataGridColumn.Converter as IMultiValueConverter).Convert(
                                           new object[] { item.GetType().GetProperty(comboField.PropertyName).GetValue(item) }, null, customDataGridColumn.ConverterParameter, null);
                                        if (convertedObj != null && convertedObj.ToString().ToLower() == comboField.SelectedItem.ToLower())
                                        {
                                            filteredList.Add(item);
                                        }
                                    }
                                }
                            }
                            searchedItems = filteredList;
                        }
                        else
                        {
                            searchedItems = searchedItems.Where(o => o.GetType() != null && o.GetType().GetProperty(comboField.PropertyName) != null
                && o.GetType().GetProperty(comboField.PropertyName).GetValue(o) != null
                && o.GetType().GetProperty(comboField.PropertyName).GetValue(o).ToString().ToLower() == comboField.SelectedItem.ToLower()).ToList();
                        }
                    }
                }
            }
            UICollectionToBeRendered = new ObservableCollection<object>(searchedItems);
            if (UICollectionToBeRendered.Count > 0 && selectOption == SelectOption.None)
            {
                SelectedItem = UICollectionToBeRendered[0];
                customDataGrid.dataGrid.ScrollIntoView(selectedItem);
            }
            log.LogMethodExit();
        }
        private void UpdateUICollection()
        {
            log.LogMethodEntry();
            List<object> searchedItems = new List<object>();
            foreach (string property in searchProperties)
            {
                if (!string.IsNullOrEmpty(property))
                {
                    searchedItems.AddRange(collectionToBeRendered.Where(o => o.GetType() != null && o.GetType().GetProperty(property) != null
                        && o.GetType().GetProperty(property).GetValue(o) != null
                        && o.GetType().GetProperty(property).GetValue(o).ToString().ToLower().Contains(searchText.ToLower())).ToList());
                }
            }
            UICollectionToBeRendered = new ObservableCollection<object>(searchedItems.Distinct());
            log.LogMethodExit();
        }
        private void OnActionsClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                DataGridRow datagridRow = parameter as DataGridRow;
                CustomCheckBox checkBox = parameter as CustomCheckBox;
                if (selectOption == SelectOption.CheckBox)
                {
                    if (checkBox == null && datagridRow != null)
                    {
                        checkBox = GetTemplateChildByName(datagridRow, "chkDataGridRow") as CustomCheckBox;
                    }
                    if (checkBox != null)
                    {
                        bool boxChecked = (bool)checkBox.IsChecked;
                        if ((datagridRow != null && !boxChecked) || (datagridRow == null && boxChecked))
                        {
                            if (datagridRow != null)
                            {
                                checkBox.IsChecked = true;
                            }
                            if (!selectedItems.Contains(checkBox.DataContext))
                            {
                                selectedItems.Add(checkBox.DataContext);
                                customDataGrid.dataGrid.SelectedItems.Add(checkBox.DataContext);
                                RaiseSelectedItemsChangedEvent();
                            }
                        }
                        else if (selectedItems.Contains(checkBox.DataContext) && ((datagridRow != null && boxChecked) || (datagridRow == null && !boxChecked)))
                        {
                            if (datagridRow != null)
                            {
                                checkBox.IsChecked = false;
                            }
                            selectedItems.Remove(checkBox.DataContext);
                            customDataGrid.dataGrid.SelectedItems.Remove(checkBox.DataContext);
                            RefreshSelectedItems();
                            RaiseSelectedItemsChangedEvent();
                        }
                    }
                    SelectedItem = null;
                }
                if (datagridRow != null && customDataGrid != null && parentDataGridVM == null)
                {
                    customDataGrid.RaiseDataGridRowMouseUpEvent();
                }
            }
            log.LogMethodExit();
        }
        private void InitalizeCommands()
        {
            log.LogMethodEntry();
            PropertyChanged += OnPropertyChanged;
            loadedCommand = new DelegateCommand(OnLoaded);
            deleteCommand = new DelegateCommand(OnDeleteClicked);
            selectionChangedCommand = new DelegateCommand(OnSelectionChanged);
            checkBoxandGridRowLoadedCommand = new DelegateCommand(OnCheckBoxAndDataGridRowLoaded);
            actionsCommand = new DelegateCommand(OnActionsClicked);
            selectAllCommand = new DelegateCommand(OnSelectAll);
            radioButtonClickedCommand = new DelegateCommand(OnRadioButtonClicked);
            searchCommand = new DelegateCommand(OnSearchClicked);
            comboSelectionChangedCommand = new DelegateCommand(OnComboSelectionChanged);
            buttonClickedCommand = new DelegateCommand(OnButtonClicked);
            log.LogMethodExit();
        }
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (!string.IsNullOrEmpty(e.PropertyName))
            {
                switch (e.PropertyName)
                {
                    case "RowDetailsHeaderCollection":
                    case "ShowRowDetails":
                        {
                            SetRowDetailsCollection();
                        }
                        break;
                }
            }
            log.LogMethodExit();
        }
        private void SetRowDetailsCollection()
        {
            log.LogMethodEntry();
            if (customDataGrid != null && customDataGrid.dataGrid != null && !string.IsNullOrWhiteSpace(rowDetailsProperty)
                            && uiCollectionToBeRendered != null && rowDetailsVisibilityMode == DataGridRowDetailsVisibilityMode.Visible)
            {
                foreach (object item in uiCollectionToBeRendered)
                {
                    if (item != null)
                    {
                        DataGridRow dataGridRow = customDataGrid.dataGrid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                        if (dataGridRow != null)
                        {
                            DataGridDetailsPresenter contentPresenter = FindVisualChild<DataGridDetailsPresenter>(dataGridRow);
                            if (contentPresenter != null && contentPresenter.ContentTemplate != null)
                            {
                                contentPresenter.ApplyTemplate();
                                CustomDataGridUserControl dataGridUserControl = contentPresenter.ContentTemplate.FindName("ChildCustomDataGridUserControl", contentPresenter) as CustomDataGridUserControl;
                                if (dataGridUserControl != null)
                                {
                                    CustomDataGridVM childcustomDataGridVM = SetRowDetailsCustomDataGridVM(item);
                                    if (childcustomDataGridVM != null)
                                    {
                                        dataGridUserControl.DataContext = childcustomDataGridVM;
                                        dataGridUserControl.dataGrid.Columns.Clear();
                                        childcustomDataGridVM.OnLoaded(dataGridUserControl);
                                        SetChildDataGridVM();
                                    }
                                }
                            }
                        }
                    }
                }
                if (customDataGrid.dataGrid.Columns != null && customDataGrid.dataGrid.Columns.Any())
                {
                    customDataGrid.dataGrid.ScrollIntoView(uiCollectionToBeRendered[0], customDataGrid.dataGrid.Columns[0]);
                }
            }
            log.LogMethodExit();
        }
        private void SetDefaulValues()
        {
            log.LogMethodEntry();
            if (this.collectionToBeRendered == null)
            {
                this.collectionToBeRendered = new ObservableCollection<object>();
            }
            if (this.headerCollection == null)
            {
                this.headerCollection = new Dictionary<string, CustomDataGridColumnElement>();
            }
            if (this.searchProperties == null)
            {
                this.searchProperties = new ObservableCollection<string>();
            }
            if (this.selectedItems == null)
            {
                this.selectedItems = new ObservableCollection<object>();
            }
            if (this.uiCollectionToBeRendered == null)
            {
                this.uiCollectionToBeRendered = new ObservableCollection<object>();
            }
            log.LogMethodExit();
        }
        internal void Clear()
        {
            log.LogMethodEntry();
            IsReadOnly = false;
            multiScreenItemBackground = MultiScreenItemBackground.Grey;
            ismultiScreenRowTwo = false;
            multiScreenMode = false;
            if (CollectionToBeRendered != null)
            {
                CollectionToBeRendered.Clear();
            }
            if (UICollectionToBeRendered != null)
            {
                UICollectionToBeRendered.Clear();
            }
            if (HeaderCollection != null)
            {
                HeaderCollection.Clear();
            }
            if (customDataGrid != null)
            {
                customDataGrid.dataGrid.Columns.Clear();
            }
            log.LogMethodExit();
        }
        #endregion

        #region Constructor
        public CustomDataGridVM(ExecutionContext executionContext)
        {
            log.LogMethodEntry();

            ExecutionContext = executionContext;

            selectAll = false;
            isReadOnly = false;
            showHeader = true;
            multiScreenMode = false;
            showSearchTextBox = false;
            ismultiScreenRowTwo = false;
            enableInternalSorting = true;
            showchildHeaderColumns = true;
            isComboAndSearchVisible = true;
            dataGridRowEnableWorksInReverse = false;

            searchTextBoxDefaultValue = MessageViewContainerList.GetMessage(this.ExecutionContext, "Enter", null);
            childDataGridHeader = string.Empty;
            dataGridRowEnableProperty = string.Empty;

            selectOption = SelectOption.None;
            verticalScrollBarVisibility = ScrollBarVisibility.Visible;
            horizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            multiScreenItemBackground = MultiScreenItemBackground.Grey;
            rowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.VisibleWhenSelected;

            SetDefaulValues();
            InitalizeCommands();

            log.LogMethodExit();
        }

        public CustomDataGridVM(ExecutionContext executionContext, ObservableCollection<object> collectionToBeRendered, Dictionary<string, CustomDataGridColumnElement> headerCollection,
            ObservableCollection<string> searchProperties, string searchText, bool selectAll, SelectOption selectOption, bool isReadOnly, bool stretch, bool isComboAndSearchVisible,
            bool showSearchTextBox, MultiScreenItemBackground multiScreenItemBackground, string checkBoxRowEnableProperty, bool dataGridRowEnableWorksInReverse,
            ScrollBarVisibility verticalScrollBarVisibility, bool enableInternalSorting, DataGridRowDetailsVisibilityMode rowDetailsVisibilityMode, bool showHeader
            , ScrollBarVisibility horizontalScrollBarVisibility = ScrollBarVisibility.Auto)
        {
            log.LogMethodEntry();

            ExecutionContext = executionContext;

            this.dataGridRowEnableProperty = checkBoxRowEnableProperty;
            this.dataGridRowEnableWorksInReverse = dataGridRowEnableWorksInReverse;
            this.collectionToBeRendered = collectionToBeRendered;
            this.headerCollection = headerCollection;
            this.searchProperties = searchProperties;
            this.enableInternalSorting = enableInternalSorting;
            this.rowDetailsVisibilityMode = rowDetailsVisibilityMode;
            if (this.collectionToBeRendered != null)
            {
                this.uiCollectionToBeRendered = new ObservableCollection<object>(this.collectionToBeRendered);
            }
            this.selectAll = selectAll;
            this.isReadOnly = isReadOnly;
            this.selectOption = selectOption;
            this.showSearchTextBox = showSearchTextBox;
            this.isComboAndSearchVisible = isComboAndSearchVisible;
            this.multiScreenItemBackground = multiScreenItemBackground;
            this.verticalScrollBarVisibility = verticalScrollBarVisibility;
            this.horizontalScrollBarVisibility = horizontalScrollBarVisibility;
            this.showHeader = showHeader;

            this.searchText = searchText;

            SetDefaulValues();
            InitalizeCommands();

            log.LogMethodExit();
        }
        #endregion
    }
}
