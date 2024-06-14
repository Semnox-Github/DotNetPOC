/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - Keyboard helper file
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.100.0     25-Sep-2020   Siba Maharana            Created for POS UI Redesign 
 *2.110.0     26-Nov-2020   Raja Uthanda             Modified to handle numeric up down and multiscreen 
 *2.120.0     05-Apr-2021   Raja Uthanda             Handle numeric up down for validation decimal
 *2.120.0     17-Aug-2021   Raja Uthanda             Handle custom button textbox modification
 ********************************************************************************************/
using System;
using System.Windows;
using Microsoft.Win32;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Controls.Primitives;

namespace Semnox.Parafait.CommonUI
{
    public enum NumericUpDownAssigning
    {
        Replace = 0,
        Add = 1
    }
    public enum KeyBoardType
    {
        ALPHANUMERIC = 0,
        NUMERIC = 1,
        BOTH = 2
    }
    public class KeyboardHelper
    {
        #region Members
        private bool oskOpened;
        private bool fromCloseWindow;
        private bool multiScreenMode;
        private bool isKeyboardClicked;
        private bool isKeyboardWindowTop;
        private bool showStandardKeyboard;
        private bool fromParentCloseEvent;
        private bool virtualbuttonclicked;
        private bool showKeyboardOnTextboxEntry;

        private double comboBoxMaxDropDownHeight;

        private ColorCode colorCode;
        private KeyBoardType keyboardType;
        private NumericUpDownAssigning numericUpDownAssigning;

        private Window parentWindow;
        private KeyboardVM keyboardVM;
        private TextBox numbericTextBox;
        private Process keyBoardProcess;
        private KeyboardView keyboardWindow;
        public Control currentActiveControl;
        public delegate void NumberpadClosed();
        public delegate void KeypadMouseDown();
        private NumberKeyboardVM numberKeyboardVM;
        private NumberKeyboardView numberKeyboardView;
        public event NumberpadClosed NumberpadClosedEvent;        
        public event KeypadMouseDown KeypadMouseDownEvent;

        private List<Control> exclusionList;
        private List<Control> showVirtualKeyboardButtonList;
        
        private static readonly logging.Logger log = new logging.Logger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion Members

        #region Constants
        private const int SC_CLOSE = 0xF060;
        private const int SWP_NOZORDER = 0x4;
        private const int SWP_HIDEWINDOW = 0x80;
        private const int SWP_SHOWWINDOW = 0x40;
        private const int SWP_NOCOPYBITS = 0x100;
        [DllImport("user32.dll", EntryPoint = "SetWindowPos", SetLastError = true)]
        private static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, int flags);
        [DllImport("kernel32.dll")]
        private static extern bool Wow64EnableWow64FsRedirection(bool set);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string className, string windowTitle);
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);
        #endregion

        #region Properties
        public NumericUpDownAssigning NumericUpDownAssigning
        {
            get { return numericUpDownAssigning; }
            set { numericUpDownAssigning = value; }
        }
        public ColorCode ColorCode
        {
            get { return colorCode; }
            set { colorCode = value; }
        }
        public bool MultiScreenMode
        {
            get { return multiScreenMode; }
            set { multiScreenMode = value; }
        }
        public KeyboardView KeyboardView { get { return keyboardWindow; } }
        public KeyboardVM KeyboardVM { get { return keyboardVM; } }
        public NumberKeyboardView NumberKeyboardView { get { return numberKeyboardView; } }
        public NumberKeyboardVM NumberKeyboardVM { get { return numberKeyboardVM; } }
        public KeyBoardType KeyboardType
        {
            get { return keyboardType; }
            set { keyboardType = value; }
        }
        #endregion Properties

        #region Constructor
        public KeyboardHelper()
        {
            log.LogMethodEntry();
            keyboardType = KeyBoardType.ALPHANUMERIC;
            numericUpDownAssigning = NumericUpDownAssigning.Replace;
            if (keyboardType == KeyBoardType.ALPHANUMERIC || keyboardType == KeyBoardType.BOTH)
            {
                keyboardWindow = new KeyboardView();
                keyboardWindow.Closed += OnKeyboardWindowClosed;
                keyboardVM = new KeyboardVM()
                {
                    KeyboardWindow = keyboardWindow,
                    MultiScreenMode = this.MultiScreenMode,
                    ColorCode = this.ColorCode
                };
                keyboardWindow.DataContext = keyboardVM;
                keyboardWindow = null;
            }
            if (keyboardType == KeyBoardType.NUMERIC || keyboardType == KeyBoardType.BOTH)
            {
                numberKeyboardVM = new NumberKeyboardVM();
                numberKeyboardView = new NumberKeyboardView()
                {
                    DataContext = numberKeyboardVM
                };
                numberKeyboardView.PreviewMouseDown += OnKeypadMouseDown;
                numberKeyboardVM.NumberKeyboardView = numberKeyboardView;
            }
            log.LogMethodExit();
        }
        #endregion Constructor

        #region Methods
        internal void CloseWindows()
        {
            log.LogMethodEntry();
            fromCloseWindow = true;
            CloseNumberWindow();
            CloseAlphaWindow();
            Kill();
            log.LogMethodExit();
        }
        private void CloseAlphaWindow()
        {
            log.LogMethodEntry();
            if (keyboardWindow != null)
            {
                keyboardWindow.Close();
                keyboardWindow = null;
            }
            log.LogMethodExit();
        }
        private void CloseNumberWindow()
        {
            log.LogMethodEntry();
            if (numberKeyboardView != null)
            {
                numberKeyboardView.Close();
                numberKeyboardView = null;
            }
            log.LogMethodExit();
        }
        public void ShowKeyBoard(Visual window, List<Control> showVirtualKeyboardButtonList, bool showKeyboardOnTextboxEntry = true, List<Control> exclusionList = null, bool showStandardKeyboard = false)
        {
            log.LogMethodEntry(window, showVirtualKeyboardButtonList, showKeyboardOnTextboxEntry, exclusionList);
            if (window is Window)
            {
                this.parentWindow = window as Window;
                if (parentWindow != null)
                {
                    parentWindow.Closed += OnParentWindowClosed;
                }
            }
            isKeyboardClicked = false;

            this.showKeyboardOnTextboxEntry = showKeyboardOnTextboxEntry;
            this.showStandardKeyboard = showStandardKeyboard;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(window); i++)
            {
                Visual childVisual = (Visual)VisualTreeHelper.GetChild(window, i);
                string type = childVisual.GetType().ToString().ToLower();

                if ((type.EndsWith("label")))
                {

                }
                else if (type.EndsWith("button"))
                {
                    Button btn = childVisual as Button;
                    this.showVirtualKeyboardButtonList = showVirtualKeyboardButtonList;
                    if (this.showVirtualKeyboardButtonList == null)
                    {
                        this.showVirtualKeyboardButtonList = new List<Control>();
                    }
                    if (this.showVirtualKeyboardButtonList != null && this.showVirtualKeyboardButtonList.Contains(btn))
                    {
                        btn.Click -= OnVirtualKeyboardClicked;
                        btn.Click += OnVirtualKeyboardClicked;
                    }
                }
                else if (type.EndsWith("customactionbutton"))
                {
                    CustomActionButton button = childVisual as CustomActionButton;
                    this.showVirtualKeyboardButtonList = showVirtualKeyboardButtonList;
                    if (this.showVirtualKeyboardButtonList == null)
                    {
                        this.showVirtualKeyboardButtonList = new List<Control>();
                    }
                    if (this.showVirtualKeyboardButtonList != null && this.showVirtualKeyboardButtonList.Contains(button))
                    {
                        button.Click += OnVirtualKeyboardClicked;
                    }
                }
                else if (type.EndsWith("customtextbox"))
                {
                    CustomTextBox textbox = childVisual as CustomTextBox;
                    this.exclusionList = exclusionList;
                    if (this.exclusionList == null)
                    {
                        this.exclusionList = new List<Control>();
                        textbox.GotFocus += new RoutedEventHandler(OnTextboxGotFocus);
                        textbox.LostFocus += new RoutedEventHandler(OnTextboxLostFocus);
                    }
                    if (this.exclusionList != null && this.exclusionList.Contains(textbox))
                    {
                        textbox.LostFocus += new RoutedEventHandler(OnTextboxLostFocus);
                    }
                    else
                    {
                        textbox.GotFocus -= new RoutedEventHandler(OnTextboxGotFocus);
                        textbox.LostFocus -= new RoutedEventHandler(OnTextboxLostFocus);
                        textbox.GotFocus += new RoutedEventHandler(OnTextboxGotFocus);
                        textbox.LostFocus += new RoutedEventHandler(OnTextboxLostFocus);
                        textbox.PreviewMouseDown += OnTextboxMouseDown;
                    }
                }
                else if (type.EndsWith("customsearchtextbox"))
                {
                    CustomSearchTextBox searchTextBox = childVisual as CustomSearchTextBox;

                    this.exclusionList = exclusionList;
                    if (this.exclusionList != null && this.exclusionList.Contains(searchTextBox))
                    {
                        searchTextBox.LostFocus += new RoutedEventHandler(OnTextboxLostFocus);
                    }
                    else
                    {
                        searchTextBox.GotFocus += new RoutedEventHandler(OnTextboxGotFocus);
                        searchTextBox.LostFocus += new RoutedEventHandler(OnTextboxLostFocus);
                    }
                }
                else if(type.EndsWith("custombuttontextbox"))
                {
                    CustomButtonTextBox buttonTextBox = childVisual as CustomButtonTextBox;
                    buttonTextBox.GotFocus -= new RoutedEventHandler(OnTextboxGotFocus);
                    buttonTextBox.LostFocus -= new RoutedEventHandler(OnTextboxLostFocus);
                    buttonTextBox.GotFocus += new RoutedEventHandler(OnTextboxGotFocus);
                    buttonTextBox.LostFocus += new RoutedEventHandler(OnTextboxLostFocus);
                    buttonTextBox.PreviewMouseDown += OnTextboxMouseDown;
                }
                else if (type.EndsWith("customcombobox"))
                {
                    CustomComboBox customComboBox = childVisual as CustomComboBox;

                    this.exclusionList = exclusionList;

                    if (this.exclusionList != null && this.exclusionList.Contains(customComboBox))
                    {
                        customComboBox.LostFocus += new RoutedEventHandler(OnTextboxLostFocus);
                    }
                    else
                    {
                        if (customComboBox.IsEditable == true)
                        {
                            comboBoxMaxDropDownHeight = customComboBox.MaxDropDownHeight;
                            customComboBox.GotFocus += new RoutedEventHandler(OnTextboxGotFocus);
                            customComboBox.LostFocus += new RoutedEventHandler(OnTextboxLostFocus);
                            customComboBox.DropDownOpened += CustomComboBox_DropDownOpened;
                            customComboBox.DropDownClosed += OnCustomComboBoxDropDownClosed;

                        }

                    }

                }
                else if (type.EndsWith("customtextboxdatepicker"))
                {
                    CustomTextBoxDatePicker textBoxDatePicker = childVisual as CustomTextBoxDatePicker;
                    this.exclusionList = exclusionList;
                    if (this.exclusionList != null && this.exclusionList.Contains(textBoxDatePicker))
                    {
                        textBoxDatePicker.LostFocus += new RoutedEventHandler(OnTextboxLostFocus);
                    }
                    else
                    {
                        textBoxDatePicker.GotFocus += new RoutedEventHandler(OnTextboxGotFocus);
                        textBoxDatePicker.LostFocus += new RoutedEventHandler(OnTextboxLostFocus);
                    }
                }
                else if (type.EndsWith("customnumericupdown"))
                {
                    CustomNumericUpDown customNumericUpDown = childVisual as CustomNumericUpDown;
                    this.exclusionList = exclusionList;

                    if (customNumericUpDown != null && customNumericUpDown.TextBox != null)
                    {
                        customNumericUpDown.TextBox.PreviewMouseDown += OnNumericUpDownMouseDown;
                    }
                }
                else
                {
                    if (!type.EndsWith("CustomDataGridUserControl".ToLower()) && childVisual is FrameworkElement)
                    {
                        ((FrameworkElement)childVisual).ApplyTemplate();
                    }
                    ShowKeyBoard(childVisual, showVirtualKeyboardButtonList, showKeyboardOnTextboxEntry, exclusionList, this.showStandardKeyboard);
                }
            }
            log.LogMethodExit();
        }
        private void OnTextboxMouseDown(object sender, MouseButtonEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            currentActiveControl = sender as Control;
            if (currentActiveControl != null && currentActiveControl.IsEnabled)
            {
                CustomTextBox customTextBox = currentActiveControl as CustomTextBox;
                if (customTextBox != null)
                {
                    if((customTextBox.ValidationType == ValidationType.DecimalOnly || customTextBox.ValidationType == ValidationType.NumberOnly
                    || customTextBox.IsIntDataType || customTextBox.IsDecimalDataType || customTextBox.IsByteDataType) && !customTextBox.IsReadOnly)
                    { 
                        if (numberKeyboardView != null)
                        {
                            ShowNumberKeyboard();
                        }
                        else
                        {
                            this.OnNumericUpDownMouseDown(currentActiveControl, null);
                        }
                    }
                }
                else if(showKeyboardOnTextboxEntry)
                {
                    CustomButtonTextBox customButtonTextBox = currentActiveControl as CustomButtonTextBox;
                    if(customButtonTextBox != null && !customButtonTextBox.IsReadOnly && customButtonTextBox.IsFocused && customButtonTextBox.ActionButton != null)
                    {
                        Point point = e.GetPosition(customButtonTextBox.ActionButton);
                        if(point.X < 0 || point.Y < 0)
                        {
                            OnTextboxGotFocus(customButtonTextBox,e);
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
        private void OnParentWindowClosed(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Kill();
            if (numberKeyboardView != null)
            {
                fromParentCloseEvent = true;
                numberKeyboardView.Close();
                fromParentCloseEvent = false;
            }
            log.LogMethodExit();
        }
        internal void OnNumericUpDownMouseDown(object sender, MouseButtonEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            numbericTextBox = sender as TextBox;
            if (numbericTextBox != null)
            {
                if (numbericTextBox.TemplatedParent != null)
                {
                    CustomNumericUpDown numericUpDown = numbericTextBox.TemplatedParent as CustomNumericUpDown;
                    if (numericUpDown != null)
                    {
                        EnableOrDisableNumericUpDownControl(numericUpDown, false);
                        currentActiveControl = numericUpDown;
                    }
                }
                if (numberKeyboardVM == null)
                {
                    numberKeyboardVM = new NumberKeyboardVM();
                }
                if (numberKeyboardView == null)
                {
                    numberKeyboardView = new NumberKeyboardView();
                    numberKeyboardView.PreviewMouseDown += OnKeypadMouseDown;
                    numberKeyboardView.DataContext = numberKeyboardVM;
                }
                ShowNumberKeyboard();
            }
            log.LogMethodExit();
        }
        private void ShowNumberKeyboard()
        {
            log.LogMethodEntry();
            string numberText = string.Empty;
            numberKeyboardVM.NumberKeyboardView = numberKeyboardView;
            if (currentActiveControl != null)
            {
                if (currentActiveControl is CustomNumericUpDown)
                {
                    numberKeyboardVM.DotButtonEnabled = false;
                    numberKeyboardVM.KeyboardType = (currentActiveControl as CustomNumericUpDown).NumberKeyboardType;
                }
                else
                {
                    CustomTextBox customTextBox = currentActiveControl as CustomTextBox;
                    if (customTextBox != null)
                    {
                        if (customTextBox.IsIntDataType || customTextBox.IsByteDataType || customTextBox.ValidationType == ValidationType.NumberOnly)
                        {
                            if(customTextBox.IsByteDataType)
                            {
                                numberKeyboardVM.CurrentCustomTextBox = customTextBox;
                            }
                            numberText = customTextBox.Text.ToString();
                            numberKeyboardVM.DotButtonEnabled = false;
                        }
                        else if (customTextBox.IsDecimalDataType || customTextBox.ValidationType==ValidationType.DecimalOnly)
                        {
                            numberText = customTextBox.Text.ToString();
                            numberKeyboardVM.DotButtonEnabled = true;
                        }
                        numberKeyboardVM.KeyboardType = customTextBox.NumberKeyboardType;
                    }
                }
            }
            numberKeyboardVM.NumberText = numberText;
            if (numericUpDownAssigning == NumericUpDownAssigning.Replace)
            {
                numberKeyboardVM.FirstTime = true;
            }
            numberKeyboardView.Closed += OnnumberKeyboardViewClosed;
            CloseAlphaWindow();
            if (parentWindow != null)
            {
                numberKeyboardView.Owner = parentWindow;
            }
            else
            {
                numberKeyboardView.Topmost = true;
            }
            numberKeyboardView.Show();
            SetNumberKeyboardPosition();
            log.LogMethodExit();
        }
        private void SetNumberKeyboardPosition()
        {
            log.LogMethodEntry();
            if (currentActiveControl != null && numberKeyboardView != null)
            {
                double left = 0;
                double top = 0;
                Point virtualPoint = new Point(0, currentActiveControl.ActualHeight);
                Point p = currentActiveControl.PointToScreen(virtualPoint);
                if (p.X + currentActiveControl.ActualWidth + 5 + numberKeyboardView.Width + 5 <= SystemParameters.WorkArea.Width)
                {
                    left = p.X + currentActiveControl.ActualWidth + 5;
                }
                else if (p.X + currentActiveControl.ActualWidth + numberKeyboardView.Width <= SystemParameters.WorkArea.Width + 10)
                {
                    left = p.X + currentActiveControl.ActualWidth - 10;
                }
                else if (p.X - numberKeyboardView.Width - 5 > 0)
                {
                    left = p.X - numberKeyboardView.Width - 5;
                }
                top = (p.Y - (currentActiveControl.ActualHeight / 2)) - (numberKeyboardView.Height / 2);
                if (top < 0)
                {
                    top = 0;
                }
                else if (top > 0 && top + numberKeyboardView.Height > SystemParameters.WorkArea.Height)
                {
                    top = SystemParameters.WorkArea.Height - numberKeyboardView.Height;
                }
                if (parentWindow != null)
                {
                    if (parentWindow.Width < SystemParameters.WorkArea.Width
                    && left + numberKeyboardView.Width > parentWindow.Width)
                    {
                        if (left + numberKeyboardView.Width <= (parentWindow.Left + parentWindow.Width))
                        {
                            if (parentWindow.Width < 500)
                            {
                                left = parentWindow.Left;
                            }
                        }
                        else
                        {
                            int diff = (int)(parentWindow.Width - (left + numberKeyboardView.Width));
                            if (diff < 0)
                            {
                                diff *= -1;
                            }
                            if (diff < 5)
                            {
                                left -= 5;
                            }
                            else
                            {
                                left = parentWindow.Left;
                            }
                        }
                    }
                    if (numberKeyboardView.Width > parentWindow.Width)
                    {
                        numberKeyboardView.Width = parentWindow.Width - 2;
                    }
                    if (parentWindow.Height < SystemParameters.WorkArea.Height
                    && top + numberKeyboardView.Height > parentWindow.Height)
                    {
                        top = parentWindow.Top;
                    }
                    if (numberKeyboardView.Height > parentWindow.Height)
                    {
                        numberKeyboardView.Height = parentWindow.Height - 2;
                    }
                }
                numberKeyboardView.Left = left;
                numberKeyboardView.Top = top;
            }
            log.LogMethodExit();
        }
        private void EnableOrDisableNumericUpDownControl(CustomNumericUpDown customNumericUpDown, bool isEnable)
        {
            log.LogMethodEntry(customNumericUpDown, isEnable);
            if (customNumericUpDown.IncreaseButton != null)
            {
                customNumericUpDown.IncreaseButton.IsEnabled = isEnable;
            }
            if (customNumericUpDown.DecreaseButton != null)
            {
                customNumericUpDown.DecreaseButton.IsEnabled = isEnable;
            }
            log.LogMethodExit();
        }
        private void OnnumberKeyboardViewClosed(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (currentActiveControl != null && !fromCloseWindow && numberKeyboardVM != null)
            {
                CustomNumericUpDown customNumericUpDown = currentActiveControl as CustomNumericUpDown;
                if (customNumericUpDown != null)
                {
                    EnableOrDisableNumericUpDownControl(customNumericUpDown, true);
                    if (numberKeyboardVM.ButtonClickType == ButtonClickType.Ok)
                    {
                        if (!fromParentCloseEvent && numbericTextBox != null)
                        {
                            if (numericUpDownAssigning == NumericUpDownAssigning.Add)
                            {
                                if (!string.IsNullOrEmpty(numberKeyboardVM.NumberText)
                                    && Int32.Parse(numberKeyboardVM.NumberText) <= customNumericUpDown.MaxValue)
                                {
                                    numbericTextBox.Text = (customNumericUpDown.Value + Int32.Parse(numberKeyboardVM.NumberText)).ToString();
                                }
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(numberKeyboardVM.NumberText))
                                {
                                    numberKeyboardVM.NumberText = "0";
                                }
                                numbericTextBox.Text = numberKeyboardVM.NumberText;
                            }
                        }
                    }
                }
                else if (numberKeyboardVM.ButtonClickType == ButtonClickType.Ok && currentActiveControl != null && currentActiveControl is CustomTextBox)
                {
                    CustomTextBox customTextBox = currentActiveControl as CustomTextBox;
                    if (customTextBox.ValidationType == ValidationType.DecimalOnly || customTextBox.ValidationType == ValidationType.NumberOnly
                    || customTextBox.IsIntDataType || customTextBox.IsDecimalDataType || customTextBox.IsByteDataType)
                    {
                        customTextBox.Text = numberKeyboardVM.NumberText;
                        if (customTextBox.Text.Length > 0)
                        {
                            customTextBox.CaretIndex = customTextBox.Text.Length;
                        }
                    }
                }
                if (NumberpadClosedEvent != null)
                {
                    NumberpadClosedEvent.Invoke();
                }
            }
            currentActiveControl = null;
            numbericTextBox = null;
            numberKeyboardView = null;
            fromCloseWindow = false;
            log.LogMethodExit();
        }
        private void OnKeyboardWindowClosed(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            keyboardWindow = null;
            keyboardVM = null;
            log.LogMethodExit();
        }
        private void OnCustomComboBoxDropDownClosed(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            CustomComboBox customComboBox = sender as CustomComboBox;
            if (customComboBox != null && customComboBox.Popup != null)
            {
                customComboBox.Popup.PlacementTarget = null;
                customComboBox.Popup.Placement = PlacementMode.Bottom;
                customComboBox.MaxDropDownHeight = comboBoxMaxDropDownHeight;
            }

            if (keyboardWindow != null)
            {
                System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(.0001)
                };
                timer.Start();
                timer.Tick += (sender1, args) =>
                {
                    timer.Stop();
                    if(KeyboardView != null)
                    {
                        Point keyboardWindowMousePosition = Mouse.GetPosition(keyboardWindow);
                        VisualTreeHelper.HitTest(keyboardWindow, KeyboardFilterCallback, ResultCallback,
                            new PointHitTestParameters(keyboardWindowMousePosition));
                    }
                    if (!isKeyboardClicked)
                    {
                        CheckParentWindowClicked();
                    }
                    isKeyboardClicked = false;
                };
            }
            else
            {
                CheckParentWindowClicked();
            }
            log.LogMethodExit();
        }
        private void CheckParentWindowClicked()
        {
            log.LogMethodEntry();
            if (parentWindow != null)
            {
                try
                {
                    Point parentWindowMousePostion = Mouse.GetPosition(this.parentWindow);
                    Point point = currentActiveControl.TranslatePoint(new Point(), this.parentWindow);
                    Rect rect = new Rect(point, currentActiveControl.RenderSize);
                    if (!rect.Contains(parentWindowMousePostion))
                    {
                        VisualTreeHelper.HitTest(this.parentWindow, this.ControlFilterCallback, 
                            this.ResultCallback, new PointHitTestParameters(parentWindowMousePostion));
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
            log.LogMethodExit();
        }
        private void CustomComboBox_DropDownOpened(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetComboBoxPopupPosition(sender as CustomComboBox);
            log.LogMethodExit();
        }
        private void SetComboBoxPopupPosition(CustomComboBox customComboBox)
        {
            log.LogMethodEntry(customComboBox);
            if (customComboBox != null && customComboBox.Popup != null
            && this.parentWindow != null && keyboardWindow != null)
            {
                Point virtualPoint = new Point(0, customComboBox.ActualHeight);

                Point p = customComboBox.PointToScreen(virtualPoint);

                customComboBox.Popup.PlacementTarget = customComboBox;

                Point controlPosition = customComboBox.TransformToAncestor(this.parentWindow).Transform(new Point(0, 0));

                double bottomDiff = keyboardWindow.Top - (controlPosition.Y + customComboBox.ActualHeight);
                double topDiff = controlPosition.Y - 5;

                if (isKeyboardWindowTop)
                {
                    bottomDiff = SystemParameters.PrimaryScreenHeight - p.Y - 5;
                    topDiff = controlPosition.Y - (keyboardWindow.Top + keyboardWindow.Height);
                }

                if (bottomDiff > topDiff)
                {
                    customComboBox.Popup.Placement = PlacementMode.Bottom;
                    customComboBox.MaxDropDownHeight = bottomDiff;
                }
                else
                {
                    customComboBox.Popup.Placement = PlacementMode.Top;
                    customComboBox.MaxDropDownHeight = topDiff;
                }
                if (customComboBox.MaxDropDownHeight > comboBoxMaxDropDownHeight)
                {
                    customComboBox.MaxDropDownHeight = comboBoxMaxDropDownHeight;
                }
            }
            log.LogMethodExit();
        }
        private HitTestFilterBehavior ControlFilterCallback(DependencyObject o)
        {
            log.LogMethodEntry(o);
            var c = o as Control;
            if ((c != null) && !(o is Window) && !(o is CustomScrollViewer))
            {
                if (c.Focusable)
                {
                    c.Focus();
                    return HitTestFilterBehavior.Stop;
                }
            }
            log.LogMethodExit();
            return HitTestFilterBehavior.Continue;
        }
        private HitTestFilterBehavior KeyboardFilterCallback(DependencyObject o)
        {
            log.LogMethodEntry(o);
            var c = o as Control;
            if ((c != null) && !(o is KeyboardView))
            {
                isKeyboardClicked = true;
                keyboardVM.OnButtonClicked(c);
                return HitTestFilterBehavior.Stop;
            }
            log.LogMethodExit();
            return HitTestFilterBehavior.Continue;
        }
        private HitTestResultBehavior ResultCallback(HitTestResult r)
        {
            return HitTestResultBehavior.Continue;
        }
        private void IsCustomComboBox()
        {
            log.LogMethodEntry();
            if (currentActiveControl != null && currentActiveControl is CustomComboBox)
            {
                SetComboBoxPopupPosition(currentActiveControl as CustomComboBox);
            }
            log.LogMethodExit();
        }
        private Point GetWindowPosition(double width, double height)
        {
            log.LogMethodEntry();
            Point position = new Point();
            if (currentActiveControl != null)
            {
                Point p = currentActiveControl.PointToScreen(new Point(0, currentActiveControl.ActualHeight));
                position.X = (SystemParameters.WorkArea.Width - width) / 2;
                isKeyboardWindowTop = false;
                double windowHeight = SystemParameters.WorkArea.Height - height;
                if (p.Y >= windowHeight)
                {
                    position.Y = 20;
                    isKeyboardWindowTop = true;
                }
                else
                {
                    position.Y = windowHeight;
                }
            }
            log.LogMethodExit(position);
            return position;
        }
        private void SnycronizeChilds()
        {
            log.LogMethodEntry();
            try
            {
                if (keyboardWindow != null)
                {
                    Point poistion = GetWindowPosition(keyboardWindow.Width, keyboardWindow.Height);
                    keyboardWindow.Left = poistion.X;
                    keyboardWindow.Top = poistion.Y;
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
        #endregion Methods

        #region Events
        private void OnVirtualKeyboardClicked(object sender, RoutedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                CustomTextBox customTextBox = null;
                if (currentActiveControl != null && currentActiveControl.IsEnabled)
                {
                    customTextBox = currentActiveControl as CustomTextBox;
                    if (customTextBox != null && (customTextBox.ValidationType == ValidationType.DecimalOnly || customTextBox.ValidationType == 
                        ValidationType.NumberOnly || customTextBox.IsIntDataType || customTextBox.IsDecimalDataType ||
                        customTextBox.IsByteDataType) && !customTextBox.IsReadOnly)
                    {
                        if (numberKeyboardView != null)
                        {
                            ShowNumberKeyboard();
                        }
                        else
                        {
                            this.OnNumericUpDownMouseDown(currentActiveControl, null);
                        }
                    }
                    else
                    {
                        bool isReadOnly = customTextBox != null ? customTextBox.IsReadOnly : false;
                        if (!isReadOnly)
                        {
                            if (ShowOnScreenKeyboard())
                            {
                                if (!showKeyboardOnTextboxEntry)
                                {
                                    virtualbuttonclicked = true;
                                }
                                Keyboard.Focus(currentActiveControl);
                                return;
                            }
                            else
                            {
                                if (keyboardWindow == null)
                                {
                                    keyboardWindow = new KeyboardView();
                                    keyboardWindow.Closed += OnKeyboardWindowClosed;
                                    keyboardWindow.PreviewMouseDown += OnKeypadMouseDown;
                                    keyboardVM = new KeyboardVM()
                                    {
                                        KeyboardWindow = keyboardWindow,
                                        MultiScreenMode = this.MultiScreenMode,
                                        ColorCode = this.ColorCode
                                    };
                                    keyboardWindow.DataContext = keyboardVM;
                                }
                                if (keyboardWindow != null)
                                {
                                    if (keyboardWindow.IsVisible)
                                    {
                                        keyboardWindow.Hide();
                                        virtualbuttonclicked = false;
                                    }
                                    else
                                    if (currentActiveControl is CustomTextBox || currentActiveControl is CustomComboBox || currentActiveControl is CustomSearchTextBox ||
                                        currentActiveControl is CustomTextBoxDatePicker || currentActiveControl is CustomButtonTextBox)
                                    {
                                        SetAlphaParentOrTopMost();
                                        if (!this.showKeyboardOnTextboxEntry)
                                        {
                                            virtualbuttonclicked = true;
                                        }
                                        Keyboard.Focus(currentActiveControl);
                                    }
                                }
                            }
                        }
                    }
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
        private void OnTextboxLostFocus(object sender, RoutedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Control textLostControl = sender as Control;
            if(!virtualbuttonclicked)
            {
                HideOnScreenKeyboard();
                CloseAlphaWindow();
            }
            CloseNumberWindow();
            log.LogMethodExit();
        }
        private void OnKeypadMouseDown(object sender, MouseButtonEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (KeypadMouseDownEvent != null)
            {
                KeypadMouseDownEvent.Invoke();
            }
            log.LogMethodExit();
        }
        private void SetAlphaParentOrTopMost()
        {
            log.LogMethodExit();
            SnycronizeChilds();
            keyboardVM.CurrentTextBox = currentActiveControl;
            keyboardWindow.ShowInTaskbar = false;
            if (parentWindow != null)
            {
                keyboardWindow.Owner = parentWindow;
            }
            else
            {
                keyboardWindow.Topmost = true;
            }
            CloseNumberWindow();
            keyboardWindow.Show();
            log.LogMethodExit();
        }

        internal void HideOnScreenKeyboard()
        {
            log.LogMethodEntry();
            try
            {
                if (keyBoardProcess != null)
                {
                    oskOpened = false;
                    uint WM_SYSCOMMAND = 0x0112;
                    IntPtr iHandle = FindWindow("OSKMainClass", null);
                    if (iHandle != IntPtr.Zero)
                    {
                        int n = SendMessage(iHandle, WM_SYSCOMMAND, SC_CLOSE, 0);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void Kill()
        {
            log.LogMethodEntry();
            try
            {
                if (keyBoardProcess != null)
                {
                    oskOpened = false;
                    HideOnScreenKeyboard();
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private bool ShowOnScreenKeyboard()
        {
            log.LogMethodEntry();
            oskOpened = false;
            if (parentWindow != null)
            {
                if (keyBoardProcess == null)
                {
                    keyBoardProcess = new Process();
                    keyBoardProcess.StartInfo = new ProcessStartInfo();
                    keyBoardProcess.StartInfo.FileName = "osk.exe";
                }
                if (showStandardKeyboard)
                {
                    try
                    {
                        Wow64EnableWow64FsRedirection(false);
                        keyBoardProcess.Start();
                        oskOpened = true;
                        Wow64EnableWow64FsRedirection(true);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        if (!oskOpened)
                        {
                            try
                            {
                                keyBoardProcess.Start();
                                oskOpened = true;
                            }
                            catch (Exception innerEx)
                            {
                                log.Error(innerEx);
                            }
                        }

                    }
                    finally
                    {
                        SetOSKKey();
                    }
                }
            }
            log.LogMethodExit(oskOpened);
            return oskOpened;
        }
        private void SetOSKKey(bool reset = false)
        {
            log.LogMethodEntry();
            if (keyBoardProcess != null && currentActiveControl != null)
            {
                Rect rect = new Rect(0, 0, 930, 300);
                if (!reset)
                {
                    Point position = GetWindowPosition(rect.Width, rect.Height);
                    rect = new Rect(position.X, position.Y, rect.Width, rect.Height);
                }
                RegistryKey myKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Osk", true);
                if (myKey != null)
                {
                    myKey.SetValue("WindowLeft", 0, RegistryValueKind.DWord);
                    myKey.SetValue("WindowTop", 0, RegistryValueKind.DWord);
                    myKey.SetValue("WindowLeft", rect.X, RegistryValueKind.DWord);
                    myKey.SetValue("WindowTop", rect.Y, RegistryValueKind.DWord);
                    myKey.SetValue("WindowWidth", rect.Width, RegistryValueKind.DWord);
                    myKey.SetValue("WindowHeight", rect.Height, RegistryValueKind.DWord);
                }
            }
            log.LogMethodExit();
        }
        private void OnTextboxGotFocus(object sender, RoutedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            currentActiveControl = sender as Control;
            if (currentActiveControl != null && currentActiveControl.IsEnabled)
            {
                CustomTextBox customTextBox = currentActiveControl as CustomTextBox;
                if (customTextBox != null && (customTextBox.ValidationType == ValidationType.DecimalOnly || customTextBox.ValidationType
                    == ValidationType.NumberOnly || customTextBox.IsIntDataType || customTextBox.IsDecimalDataType || customTextBox.IsByteDataType)
                    && !customTextBox.IsReadOnly)
                {
                    if (numberKeyboardView != null)
                    {
                        ShowNumberKeyboard();
                    }
                    else
                    {
                        OnNumericUpDownMouseDown(currentActiveControl, null);
                    }
                }
                else
                {
                    bool isReadOnly = customTextBox != null ? customTextBox.IsReadOnly : false;
                    CustomButtonTextBox customButtonTextBox = currentActiveControl as CustomButtonTextBox;
                    if(customButtonTextBox != null && e.OriginalSource != null && e.OriginalSource == customButtonTextBox.ActionButton)
                    {
                        return;
                    }
                    if(!isReadOnly)
                    {
                        if (showKeyboardOnTextboxEntry && ShowOnScreenKeyboard())
                        {
                            log.LogMethodExit("On screen keyboard.");
                            return;
                        }
                        else if (showStandardKeyboard && virtualbuttonclicked)
                        {
                            Wow64EnableWow64FsRedirection(false);
                            SetOSKKey();
                            Wow64EnableWow64FsRedirection(true);
                        }
                        if (keyboardWindow == null)
                        {
                            FrameworkElement ct = currentActiveControl;
                            while (true)
                            {
                                if (ct is Window)
                                {
                                    ((Window)ct).LocationChanged += new EventHandler(KeyboardVMLocationChanged);
                                    ((Window)ct).Deactivated += new EventHandler(KeyboardVMDeactivated);
                                    break;
                                }
                                if (ct != null && ct.Parent != null)
                                {
                                    ct = (FrameworkElement)ct.Parent;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            if (this.showKeyboardOnTextboxEntry)
                            {
                                keyboardWindow = new KeyboardView();
                                keyboardWindow.PreviewMouseDown += OnKeypadMouseDown;
                                keyboardWindow.Closed += OnKeyboardWindowClosed;
                                keyboardVM = new KeyboardVM()
                                {
                                    KeyboardWindow = keyboardWindow,
                                    MultiScreenMode = this.MultiScreenMode,
                                    ColorCode = this.ColorCode
                                };
                                keyboardWindow.DataContext = keyboardVM;
                                SetAlphaParentOrTopMost();
                                IsCustomComboBox();
                            }
                        }
                        else
                        {
                            if (this.showKeyboardOnTextboxEntry)
                            {
                                SetAlphaParentOrTopMost();
                                IsCustomComboBox();
                            }
                            else
                            {
                                keyboardVM.CurrentTextBox = currentActiveControl;
                                SnycronizeChilds();
                            }
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
        private void KeyboardVMDeactivated(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (keyboardWindow != null)
            {
                keyboardWindow.Topmost = false;
            }
            log.LogMethodExit();
        }
        private void KeyboardVMLocationChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SnycronizeChilds();
            log.LogMethodExit();
        }
        #endregion Events
    }
}
