/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Redemption - view model for number keyboard
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.120.0     06-Apr-2021   Raja Uthanda            Created for POS UI Redesign 
 ********************************************************************************************/
using System;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using System.Windows.Controls;

namespace Semnox.Parafait.CommonUI
{
    public enum NumberKeyboardType
    {
        Positive = 1,
        Negative = 2,
        Both = 3,
    }
    public class NumberKeyboardVM : ViewModelBase
    {
        #region Members    
        private bool firstTime;
        private bool dotButtonEnabled;

        private string numberText;

        private ButtonClickType buttonClickType;
        private ICommand closeCommand;
        private ICommand buttonClickedCommand;

        public NumberKeyboardType keyboardType;

        private TextBox textBox;
        private CustomTextBox currentCustomTextBox;
        private NumberKeyboardView numberKeyboardView;
        private static readonly logging.Logger log = new logging.Logger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties 
        public ButtonClickType ButtonClickType
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(dotButtonEnabled);
                return buttonClickType;
            }
        }
        public NumberKeyboardType KeyboardType
        {
            get { return keyboardType; }
            set { SetProperty(ref keyboardType, value); }
        }
        public bool FirstTime
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(firstTime);
                return firstTime;
            }
            set
            {
                log.LogMethodEntry(firstTime, value);
                SetProperty(ref firstTime, value);
                log.LogMethodExit(firstTime);
            }
        }
        public CustomTextBox CurrentCustomTextBox
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(currentCustomTextBox);
                return currentCustomTextBox;
            }
            set
            {
                log.LogMethodEntry(currentCustomTextBox, value);
                SetProperty(ref currentCustomTextBox, value);
                log.LogMethodExit(currentCustomTextBox);
            }
        }
        public bool DotButtonEnabled
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(dotButtonEnabled);
                return dotButtonEnabled;
            }
            set
            {
                log.LogMethodEntry(dotButtonEnabled, value);
                SetProperty(ref dotButtonEnabled, value);
                log.LogMethodExit(dotButtonEnabled);
            }
        }

        public string NumberText
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(numberText);
                return numberText;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref numberText, value);
                PerformAddingNegativeTextValidation(true);
                CheckDigit();
                log.LogMethodExit(numberText);
            }
        }
        public ICommand CloseCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(closeCommand);
                return closeCommand;
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

        public NumberKeyboardView NumberKeyboardView
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(numberKeyboardView);
                return numberKeyboardView;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref numberKeyboardView, value);
                if (numberKeyboardView != null)
                {
                    textBox = numberKeyboardView.txtNum;
                }
                log.LogMethodExit(numberKeyboardView);
            }
        }
        #endregion

        #region Methods
        private void PerformClose()
        {
            log.LogMethodEntry();
            if (NumberKeyboardView != null)
            {
                NumberKeyboardView.Close();
                NumberKeyboardView = null;
            }
            log.LogMethodExit();
        }
        private void CheckDigit()
        {
            log.LogMethodEntry();
            if (!string.IsNullOrEmpty(numberText) && numberText.Any(c => !char.IsDigit(c)))
            {
                bool changed = false;
                string text = numberText;
                for (int i = 0; i < text.Length; i++)
                {
                    char c = text[i];
                    if(c == '-' && i == 0)
                    {
                        continue;
                    }
                    bool removeDot = c == '.' && dotButtonEnabled ? false : true;
                    if (c == '.' && !removeDot && numberText.Count(d => d == '.') > 1 && textBox != null)
                    {
                        int index = textBox.CaretIndex;
                        text = text.Remove(index - 1, 1);
                        changed = true;
                    }
                    if (!char.IsDigit(c) && removeDot)
                    {
                        text = text.Remove(i, 1);
                        changed = true;
                    }
                }
                if (changed)
                {
                    NumberText = text;
                }
            }
            log.LogMethodExit();
        }
        private void OnClosed(object param)
        {
            log.LogMethodEntry(param);
            buttonClickType = ButtonClickType.Cancel;
            PerformClose();
            log.LogMethodExit();
        }
        private void OnButtonClicked(object param)
        {
            log.LogMethodEntry(param);
            if (param != null)
            {
                Button button = param as Button;
                if (button != null)
                {
                    if (firstTime && button.Name != "btncmdOk")
                    {
                        NumberText = string.Empty;
                        firstTime = false;
                    }
                    int backupCaretIndex = 0;
                    if (textBox != null && textBox.CaretIndex != textBox.Text.Length)
                    {
                        backupCaretIndex = textBox.CaretIndex;
                    }
                    switch (button.Name)
                    {
                        case "btncmdCancel":
                            NumberText = string.Empty;
                            break;
                        case "btncmdBackSpace":
                            if (!string.IsNullOrEmpty(NumberText))
                            {
                                if (backupCaretIndex > 0)
                                {
                                    backupCaretIndex -= 1;
                                    NumberText = NumberText.Remove(backupCaretIndex, 1);
                                }
                                else
                                {
                                    NumberText = NumberText.Remove(NumberText.Length - 1);
                                }
                            }
                            break;
                        case "btncmdOk":
                            {
                                buttonClickType = ButtonClickType.Ok;
                                if(!string.IsNullOrWhiteSpace(numberText) && (numberText == "." || numberText == "-." || numberText == "-"))
                                {
                                    NumberText = string.Empty;
                                }
                                PerformClose();
                            }
                            break;
                        case "btncmdDot":
                            if (!NumberText.Contains("."))
                            {
                                CheckByteValue(button.Content.ToString(), ref backupCaretIndex);
                            }
                            break;
                        case "btncmdNegative":
                            AddORRemoveNegativeText(button.Content.ToString());
                            break;
                        default:
                            CheckByteValue(button.Content.ToString(), ref backupCaretIndex);
                            break;
                    }
                    if (textBox != null)
                    {
                        if (backupCaretIndex > 0)
                        {
                            textBox.CaretIndex = backupCaretIndex;
                        }
                        else
                        {
                            textBox.CaretIndex = textBox.Text.Length;
                        }
                        textBox.Focus();
                    }
                }
            }
            log.LogMethodExit();
        }
        private void AddORRemoveNegativeText(string content)
        {
            log.LogMethodEntry(content);
            switch (keyboardType)
            {
                case NumberKeyboardType.Negative:
                    AddNegativeText(content);
                    break;
                case NumberKeyboardType.Both:
                    if(NumberText.Contains("-"))
                    {
                        NumberText = NumberText.Remove(0, 1);
                    }
                    else
                    {
                        AddNegativeText(content);
                    }
                    break;
            }
            log.LogMethodExit();
        }
        private void PerformAddingNegativeTextValidation(bool fromPropertyChange = false)
        {
            log.LogMethodEntry(fromPropertyChange);
            if (!string.IsNullOrWhiteSpace(numberText))
            {
                switch(keyboardType)
                {
                    case NumberKeyboardType.Negative:
                        AddNegativeText("-");
                        break;
                    case NumberKeyboardType.Both:
                        if(fromPropertyChange && NumberText.Contains("-") && NumberText.Count(c => c == '-') == 1)
                        {
                            int negativeIndex = NumberText.IndexOf('-');
                            if(negativeIndex > 0)
                            {
                                NumberText = NumberText.Remove(negativeIndex, 1);
                                AddNegativeText("-");
                            }
                        }
                        break;
                }
                
            }
            log.LogMethodExit();
        }
        private void AddNegativeText(string content)
        {
            log.LogMethodEntry(content);
            if (keyboardType != NumberKeyboardType.Positive && !NumberText.Contains("-"))
            {
                NumberText = NumberText.Insert(0, content);
            }
            log.LogMethodExit();
        }
        private void CheckByteValue(string content, ref int backupCaretIndex)
        {
            log.LogMethodEntry(content, backupCaretIndex);
            if (currentCustomTextBox != null && currentCustomTextBox.IsByteDataType)
            {
                if(numberText.Length < 3)
                {
                    SetNumberText(content, ref backupCaretIndex);
                }
                byte convertedValue = 0;
                bool isByte = byte.TryParse(numberText, out convertedValue);
                if (!isByte)
                {
                    NumberText = byte.MaxValue.ToString();
                }
            }
            else
            {
                SetNumberText(content, ref backupCaretIndex);
            }
            log.LogMethodExit();
        }
        private void SetNumberText(string content, ref int backupCaretIndex)
        {
            log.LogMethodEntry(content, backupCaretIndex);
            if (backupCaretIndex > 0)
            {
                NumberText = NumberText.Insert(backupCaretIndex, content);
                backupCaretIndex += 1;
            }
            else
            {
                NumberText += content;
            }
            PerformAddingNegativeTextValidation();
            log.LogMethodExit();
        }        
        #endregion

        #region Constructors
        public NumberKeyboardVM()
        {
            log.LogMethodEntry();
            dotButtonEnabled = true;
            firstTime = true;
            numberText = string.Empty;
            keyboardType = NumberKeyboardType.Positive;
            buttonClickedCommand = new DelegateCommand(OnButtonClicked);
            closeCommand = new DelegateCommand(OnClosed);
            buttonClickType = ButtonClickType.Cancel;
            log.LogMethodExit();
        }
        #endregion
    }
}
