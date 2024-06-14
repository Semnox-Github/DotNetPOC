/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - Custom combo box
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.100.0     25-Sep-2020   Raja Uthanda            Created for POS UI Redesign 
 *2.100.0     23-Sep-2021   Raja Uthanda            Modified for errorstate
 ********************************************************************************************/
using System;
using System.Linq;
using System.Windows;
using System.Reflection;
using System.Collections;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls.Primitives;

using Semnox.Core.Utilities;

namespace Semnox.Parafait.CommonUI
{
    public class CustomComboBox : ComboBox
    {

        #region Members
        private int searchItemsCount;

        private bool isFromTextChanged;
        private bool isFromToggleClicked;

        private TextBox textBox;
        private List<object> itemSource;
        private CustomDataGridUserControl customDataGridUserControl;

        public static readonly DependencyProperty CustomDataGridVMDependencyProperty = DependencyProperty.Register("CustomDataGridVM", typeof(CustomDataGridVM), typeof(CustomComboBox), new PropertyMetadata(null));
        public static readonly DependencyProperty ExecutionContextDependencyProperty = DependencyProperty.Register("ExecutionContext", typeof(ExecutionContext), typeof(CustomComboBox), new PropertyMetadata(null));
        public static readonly DependencyProperty DisplayMemberPathCollectionDependencyProperty = DependencyProperty.Register("DisplayMemberPathCollection", typeof(List<string>), typeof(CustomComboBox), new PropertyMetadata(null));
        public static readonly DependencyProperty ErrorStateDependencyProperty = DependencyProperty.Register("ErrorState", typeof(bool), typeof(CustomComboBox), new FrameworkPropertyMetadata() { DefaultValue = false, BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
        public static readonly DependencyProperty EditableItemsSourceDependencyProperty = DependencyProperty.Register("EditableItemsSource", typeof(IEnumerable), typeof(CustomComboBox), new PropertyMetadata(null, OnEditableItemsSourceChanged));
        public static readonly DependencyProperty ErrorTextVisibleDependencyProperty = DependencyProperty.Register("ErrorTextVisible", typeof(bool), typeof(CustomComboBox), new PropertyMetadata(true));
        public static readonly DependencyProperty HeadingDependencyProperty = DependencyProperty.Register("Heading", typeof(string), typeof(CustomComboBox), new PropertyMetadata(""));
        public static readonly DependencyProperty IsDefaultValueDependencyProperty = DependencyProperty.Register("IsDefaultValue", typeof(bool), typeof(CustomComboBox), new PropertyMetadata(true));
        public static readonly DependencyProperty IsMandatoryDependencyProperty = DependencyProperty.Register("IsMandatory", typeof(bool), typeof(CustomComboBox), new PropertyMetadata(false));
        public static readonly DependencyProperty DefaultValueDependencyProperty = DependencyProperty.Register("DefaultValue", typeof(string), typeof(CustomComboBox), new PropertyMetadata(TranslateHelper.TranslateMessage("Select")));
        public static readonly DependencyProperty SizeDependencyProperty = DependencyProperty.Register("Size", typeof(Size), typeof(CustomComboBox), new PropertyMetadata(Size.Small));
        public static readonly DependencyProperty HideBackgroundDependencyProperty = DependencyProperty.Register("HideBackground", typeof(bool), typeof(CustomComboBox), new PropertyMetadata(false));
        private static readonly logging.Logger log = new logging.Logger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties   
        public bool ErrorState
        {
            get
            {
                return (bool)GetValue(ErrorStateDependencyProperty);
            }
            set
            {
                SetValue(ErrorStateDependencyProperty, value);
            }
        }
        public ExecutionContext ExecutionContext
        {
            get
            {
                return (ExecutionContext)GetValue(ExecutionContextDependencyProperty);
            }
            set
            {
                SetValue(ExecutionContextDependencyProperty, value);
            }
        }
        public CustomDataGridVM CustomDataGridVM
        {
            get
            {
                return (CustomDataGridVM)GetValue(CustomDataGridVMDependencyProperty);
            }
            set
            {
                SetValue(CustomDataGridVMDependencyProperty, value);
            }
        }
        public List<string> DisplayMemberPathCollection
        {
            get
            {
                return (List<string>)GetValue(DisplayMemberPathCollectionDependencyProperty);
            }
            set
            {
                SetValue(DisplayMemberPathCollectionDependencyProperty, value);
            }
        }
        public IEnumerable EditableItemsSource
        {
            get
            {
                return (IEnumerable)GetValue(EditableItemsSourceDependencyProperty);
            }
            set
            {
                SetValue(EditableItemsSourceDependencyProperty, value);
            }
        }
        public bool ErrorTextVisible
        {
            get
            {
                return (bool)GetValue(ErrorTextVisibleDependencyProperty);
            }
            set
            {
                SetValue(ErrorTextVisibleDependencyProperty, value);
            }
        }
        internal TextBox TextBox
        {
            get
            {
                return textBox;
            }
        }
        public bool HideBackground
        {
            get { return (bool)GetValue(HideBackgroundDependencyProperty); }
            set { SetValue(HideBackgroundDependencyProperty, value); }
        }
        internal bool IsDefaultValue
        {
            get { return (bool)GetValue(IsDefaultValueDependencyProperty); }
            private set { SetValue(IsDefaultValueDependencyProperty, value); }
        }
        public string DefaultValue
        {
            get { return (string)GetValue(DefaultValueDependencyProperty); }
            set { SetValue(DefaultValueDependencyProperty, value); }
        }
        public string Heading
        {
            get { return (string)GetValue(HeadingDependencyProperty); }
            set { SetValue(HeadingDependencyProperty, value); }
        }
        public Size Size
        {
            get { return (Size)GetValue(SizeDependencyProperty); }
            set { SetValue(SizeDependencyProperty, value); }
        }
        public bool IsMandatory
        {
            get { return (bool)GetValue(IsMandatoryDependencyProperty); }
            set { SetValue(IsMandatoryDependencyProperty, value); }
        }
        internal Popup Popup
        {
            get
            {
                if (Template != null)
                {
                    return this.Template.FindName("PART_Popup", this) as Popup;
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        #region Methods
        private void FindParent(Visual myVisual)
        {
            log.LogMethodEntry(myVisual);
            if (ExecutionContext == null)
            {
                if (myVisual == null)
                {
                    return;
                }
                object visual = VisualTreeHelper.GetParent(myVisual);
                if (visual is Window)
                {
                    Window parent = visual as Window;
                    if (parent.DataContext != null && parent.DataContext is ViewModelBase)
                    {
                        ExecutionContext = (parent.DataContext as ViewModelBase).ExecutionContext;
                    }
                }
                else
                {
                    FindParent(visual as Visual);
                }
            }
            log.LogMethodExit();
        }
        protected override void OnDropDownOpened(EventArgs e)
        {
            log.LogMethodEntry(e);
            FindParent(this);
            if (DisplayMemberPathCollection != null && ExecutionContext != null)
            {
                if (CustomDataGridVM == null)
                {
                    CustomDataGridVM = new CustomDataGridVM(ExecutionContext)
                    {
                        IsComboAndSearchVisible = false,
                        IsMultiScreenRowTwo = true,
                        MultiScreenMode = true,
                        SelectOption = SelectOption.ManualSelectionOnly
                    };
                }
                if (!isFromTextChanged)
                {
                    customDataGridUserControl = this.Template.FindName("DisplayMemberPathCollectionDataGrid", this) as CustomDataGridUserControl;
                    if (customDataGridUserControl != null && customDataGridUserControl.Visibility == Visibility.Visible)
                    {
                        customDataGridUserControl.SelectionChanged -= OnCustomDataGridUserControlSelectionChanged;
                        customDataGridUserControl.SelectionChanged += OnCustomDataGridUserControlSelectionChanged;
                        Dictionary<string, CustomDataGridColumnElement> headers = new Dictionary<string, CustomDataGridColumnElement>();
                        foreach (string propertyName in DisplayMemberPathCollection)
                        {
                            headers.Add(propertyName, new CustomDataGridColumnElement() { Heading = propertyName });
                        }
                        CustomDataGridVM.CollectionToBeRendered = new ObservableCollection<object>(this.itemSource);
                        CustomDataGridVM.HeaderCollection = headers;
                    }
                }
            }
            base.OnDropDownOpened(e);
            log.LogMethodExit();
        }
        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            log.LogMethodEntry(e);
            if (!IsEditable)
            {
                IsDefaultValue = SelectedItem != null ? false : true;
            }
            if (SelectedItem != null && ErrorState)
            {
                SetErrorState(false);
            }
            base.OnSelectionChanged(e);
            log.LogMethodExit();
        }
        private static void OnEditableItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            log.LogMethodEntry(d, e);
            CustomComboBox comboBox = d as CustomComboBox;
            if (comboBox != null)
            {
                comboBox.IsEditableItemsSourceChanged();
            }
            log.LogMethodExit();
        }
        private void OnCustomDataGridUserControlSelectionChanged(object sender, RoutedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (CustomDataGridVM.SelectedItem != null && !isFromTextChanged && CustomDataGridVM.SelectedItem != this.SelectedItem)
            {
                this.SelectedItem = CustomDataGridVM.SelectedItem;
                this.IsDropDownOpen = false;
            }
            log.LogMethodExit();
        }
        private void IsEditableItemsSourceChanged()
        {
            log.LogMethodEntry();
            ItemsSource = EditableItemsSource;
            itemSource = ItemsSource != null ? ItemsSource.Cast<object>().ToList() : new List<object>();
            log.LogMethodExit();
        }
        private void OnToggleButtonChecked(object sender, RoutedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (textBox != null)
            {
                isFromToggleClicked = true;
            }
            if (!IsDropDownOpen)
            {
                IsDropDownOpen = true;
            }
            if (!isFromTextChanged && itemSource != null && IsEditable)
            {
                ItemsSource = EditableItemsSource != null ? EditableItemsSource : itemSource;
            }
            isFromToggleClicked = false;
        }
        private void OnToggleButtonUnChecked(object sender, RoutedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (IsDropDownOpen)
            {
                IsDropDownOpen = false;
            }
            log.LogMethodExit();
        }
        private void OnComboboxGotFocus(object sender, RoutedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (IsEditable && !IsDropDownOpen && !(e.OriginalSource is ToggleButton))
            {
                IsDropDownOpen = true;
            }
            if (textBox != null && textBox.Text != null && textBox.Text == DefaultValue)
            {
                textBox.Text = string.Empty;
            }
            log.LogMethodExit();
        }
        private void OnComboboxLostFocus(object sender, RoutedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (textBox != null && !string.IsNullOrEmpty(textBox.Text) && !this.IsFocused
                 && !this.IsKeyboardFocused && !this.textBox.IsKeyboardFocused)
            {
                if (itemSource != null && itemSource.Count > 0)
                {
                    object searchedItem;
                    if (!string.IsNullOrEmpty(this.DisplayMemberPath))
                    {
                        searchedItem = itemSource.FirstOrDefault(item =>
                       (item.GetType().GetProperty(this.DisplayMemberPath).GetValue(item) as string).ToLower()
                            == textBox.Text.ToLower());
                    }
                    else
                    {
                        searchedItem = itemSource.FirstOrDefault(item =>
                        (item is string && (item as string).ToLower() == textBox.Text.ToLower())
                            || (item.ToString().ToLower() == textBox.Text.ToLower()));
                    }
                    if (searchedItem != null)
                    {
                        SelectedItem = searchedItem;
                    }
                    if (DisplayMemberPathCollection != null && CustomDataGridVM != null && textBox.Text == string.Empty)
                    {
                        CustomDataGridVM.SelectedItem = null;
                    }
                }
            }
            ValidString();
            log.LogMethodExit();
        }
        private void OpenOrCloseDropDown(int count = 0, bool select = false)
        {
            log.LogMethodEntry(count);
            if (count > 0 || (select && !IsDropDownOpen))
            {
                if (!IsDropDownOpen)
                {
                    OpenDropDown();
                    if (!select)
                    {
                        SetCaretIndex();
                    }
                }
            }
            else if (IsDropDownOpen)
            {
                IsDropDownOpen = false;
            }
            log.LogMethodExit();
        }
        private bool OpenDropDown()
        {
            log.LogMethodEntry();
            bool isOpened = false;
            if (!this.IsDropDownOpen)
            {
                isFromTextChanged = true;
                IsDropDownOpen = true;
                isFromTextChanged = false;
                isOpened = true;
            }
            log.LogMethodExit();
            return isOpened;
        }
        private void SetCaretIndex()
        {
            log.LogMethodEntry();
            this.textBox.SelectionLength = 0;
            if (this.textBox.Text.Length > 0)
            {
                this.textBox.CaretIndex = this.textBox.Text.Length;
            }
            log.LogMethodExit();
        }
        private void OnComboBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            textBox = e.OriginalSource as TextBox;
            if (itemSource == null && this.ItemsSource != null)
            {
                itemSource = this.ItemsSource.Cast<object>().ToList();
            }
            if (textBox != null && !isFromToggleClicked && textBox.IsFocused)
            {
                string searchText = textBox.Text;
                this.Text = textBox.Text;
                if (itemSource != null)
                {
                    searchItemsCount = 0;
                    if (SelectedItem != null)
                    {
                        SelectedItem = null;
                    }
                    if (DisplayMemberPathCollection != null)
                    {
                        List<object> searchedItems = new List<object>();
                        foreach (string property in DisplayMemberPathCollection)
                        {
                            if (!string.IsNullOrEmpty(property))
                            {
                                searchedItems.AddRange(itemSource.Where(o => o.GetType() != null && o.GetType().GetProperty(property) != null
                                    && o.GetType().GetProperty(property).GetValue(o) != null
                                    && o.GetType().GetProperty(property).GetValue(o).ToString().ToLower().Contains(searchText.ToLower())).ToList());
                            }
                        }
                        CustomDataGridVM.CollectionToBeRendered = new ObservableCollection<object>(searchedItems.Distinct());
                        searchItemsCount = CustomDataGridVM.UICollectionToBeRendered.Count;
                        OpenDropDown();
                        SetCaretIndex();
                    }
                    else
                    {
                        bool select = true;
                        List<object> searchedList = null;
                        string smallText = searchText.ToLower();
                        if (!string.IsNullOrWhiteSpace(DisplayMemberPath))
                        {
                            searchedList = itemSource.Where(item => item != null && item.GetType() != null && item.GetType().GetProperty(DisplayMemberPath) != null
                            && (item.GetType().GetProperty(DisplayMemberPath).GetValue(item) != null) &&
                            (item.GetType().GetProperty(DisplayMemberPath).GetValue(item)).ToString().ToLower()
                           .Contains(smallText)).ToList();
                        }
                        else if (itemSource.All(item => item is string))
                        {
                            searchedList = itemSource.Where(item => item != null && (item as string).ToLower().Contains(searchText.ToLower())).ToList();
                        }
                        else if (itemSource.All(item => item is int) || itemSource.All(item => item is int?))
                        {
                            searchedList = itemSource.Where(item =>
                            {
                                int result;
                                if (item != null && int.TryParse(item.ToString(), out result) && result.ToString().ToLower().Contains(smallText))
                                {
                                    return true;
                                }
                                return false;
                            }).ToList();
                        }
                        else if (itemSource.All(item => item is float) || itemSource.All(item => item is float?))
                        {
                            searchedList = itemSource.Where(item =>
                            {
                                float result;
                                if (item != null && float.TryParse(item.ToString(), out result) && result.ToString().ToLower().Contains(smallText))
                                {
                                    return true;
                                }
                                return false;
                            }).ToList();
                        }
                        else if (itemSource.All(item => item is double) || itemSource.All(item => item is double?))
                        {
                            searchedList = itemSource.Where(item => 
                            {
                                double result;
                                if (item != null && double.TryParse(item.ToString(), out result) && result.ToString().ToLower().Contains(smallText))
                                {
                                    return true;
                                }
                                return false;
                            }).ToList();
                        }
                        else if (itemSource.All(item => item is byte) || itemSource.All(item => item is byte?))
                        {
                            searchedList = itemSource.Where(item =>
                            {
                                byte result;
                                if (item != null && byte.TryParse(item.ToString(), out result) && result.ToString().ToLower().Contains(smallText))
                                {
                                    return true;
                                }
                                return false;
                            }).ToList();
                        }
                        if (searchedList != null)
                        {
                            ItemsSource = searchedList;
                            select = false;
                            searchItemsCount = searchedList.Count;
                        }
                        OpenOrCloseDropDown(searchItemsCount, select);
                    }
                    ValidString();
                    if (IsTextSearchEnabled)
                    {
                        IsTextSearchEnabled = false;
                    }
                }
            }
            log.LogMethodExit();
        }
        private void ValidString()
        {
            log.LogMethodEntry();
            bool errorState = false;
            if (IsEditable && SelectedItem == null && ((textBox != null && !string.IsNullOrEmpty(textBox.Text)) || !string.IsNullOrEmpty(Text)))
            {
                if ((textBox.IsFocused && searchItemsCount == 0) || !textBox.IsFocused)
                {
                    errorState = true;
                }
            }
            SetErrorState(errorState);
            log.LogMethodExit();
        }
        private void SetErrorState(bool errorState)
        {
            log.LogMethodEntry(errorState);
            if (ErrorState != errorState)
            {
                PropertyInfo propertyInfo = GetPropertyInfo(CustomComboBox.ErrorStateDependencyProperty);
                if (propertyInfo != null && propertyInfo.GetSetMethod() != null)
                {
                    propertyInfo.SetValue(this.GetBindingExpression(CustomComboBox.ErrorStateDependencyProperty).DataItem, errorState);
                }
                else
                {
                    ErrorState = errorState;
                }
            }
            log.LogMethodExit();
        }
        private PropertyInfo GetPropertyInfo(DependencyProperty dependencyProperty)
        {
            log.LogMethodEntry(dependencyProperty);
            PropertyInfo propertyInfo = null;
            BindingExpression bindingExpression = this.GetBindingExpression(dependencyProperty);
            if (bindingExpression != null)
            {
                string[] splits = bindingExpression.ParentBinding.Path.Path.Split('.');
                Type type = bindingExpression.DataItem.GetType();
                foreach (string split in splits)
                {
                    propertyInfo = type.GetProperty(split);
                    if (propertyInfo != null)
                    {
                        type = propertyInfo.PropertyType;
                    }
                }
            }
            log.LogMethodExit();
            return propertyInfo;
        }
        #endregion

        #region Constructors
        public CustomComboBox()
        {
            log.LogMethodEntry();
            this.AddHandler(TextBoxBase.TextChangedEvent, new TextChangedEventHandler(OnComboBoxTextChanged));
            this.AddHandler(TextBoxBase.LostFocusEvent, new RoutedEventHandler(OnComboboxLostFocus));
            this.AddHandler(TextBoxBase.GotFocusEvent, new RoutedEventHandler(OnComboboxGotFocus));
            this.AddHandler(ToggleButton.CheckedEvent, new RoutedEventHandler(OnToggleButtonChecked));
            this.AddHandler(ToggleButton.UncheckedEvent, new RoutedEventHandler(OnToggleButtonUnChecked));
            log.LogMethodExit();
        }
        #endregion
    }
}
