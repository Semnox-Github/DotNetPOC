/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - view model for key board window
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.100.0     25-Sep-2020   Raja Uthanda            Created for POS UI Redesign 
 *2.110.0     14-Dec-2020   Raja Uthanda            Modified for POS UI redemption 
 ********************************************************************************************/

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Semnox.Parafait.CommonUI
{
    public enum ColorCode
    {
        User1 = 0,
        User2 = 1,
        User3 = 2,
        User4 = 3,
        User5 = 4,
        User6 = 5,
        User7 = 6,
        User8 = 7
    }

    public class KeyboardVM : ViewModelBase
    {
        #region Members
        private bool multiScreenMode;
        private bool capsLockFlag;
        private bool shiftFlag;
        private bool firstTime;

        private string selectedText;

        private int caretIndex;

        private ColorCode colorCode;

        private Window keyboardWindow;
        private Control selectedTextBox;

        private ICommand buttonClickedCommand;

        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Properties
        public ColorCode ColorCode
        {
            get
            {
                log.LogMethodEntry(colorCode);
                log.LogMethodExit(colorCode);
                return colorCode;
            }
            set
            {
                log.LogMethodEntry(colorCode, value);
                SetProperty(ref colorCode, value);
                log.LogMethodExit(colorCode);
            }
        }

        public bool MultiScreenMode
        {
            get
            {
                log.LogMethodEntry(multiScreenMode);
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

        public Window KeyboardWindow
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(keyboardWindow);
                return keyboardWindow;
            }
            set
            {
                log.LogMethodEntry(keyboardWindow, value);
                SetProperty(ref keyboardWindow, value);
                log.LogMethodExit(keyboardWindow);
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
            set
            {
                log.LogMethodEntry(value);
                buttonClickedCommand = value;
                log.LogMethodExit(buttonClickedCommand);
            }
        }

        public string KeyboardText
        {
            get
            {
                log.LogMethodEntry();
                return GetOrSetText(string.Empty, true);
            }
            set
            {
                log.LogMethodEntry();
                GetOrSetText(value, false);
                log.LogMethodExit();
            }

        }

        public Control CurrentTextBox
        {
            get
            {
                log.LogMethodEntry();
                return selectedTextBox;
            }
            set
            {
                log.LogMethodEntry(value);
                selectedTextBox = value;
            }
        }

        public bool CapsLockFlag
        {
            get
            {
                log.LogMethodEntry();
                return capsLockFlag;
            }
            set
            {
                log.LogMethodEntry(capsLockFlag, value);
                SetProperty(ref capsLockFlag, value);
                log.LogMethodExit(capsLockFlag);
            }
        }

        public bool ShiftFlag
        {
            get
            {
                log.LogMethodEntry();
                return shiftFlag;
            }
            set
            {
                log.LogMethodEntry(value);
                shiftFlag = value;
            }
        }
        #endregion

        #region Methods
        private string GetOrSetText(string value, bool isGet)
        {
            log.LogMethodEntry(isGet);          
            if (CurrentTextBox is CustomTextBox)
            {
                CustomTextBox customTextBox = CurrentTextBox as CustomTextBox;
                if (isGet)
                {
                    value = customTextBox.Text;
                }
                else
                {
                    customTextBox.Text = value;
                }
            }
            else if (CurrentTextBox is CustomComboBox)
            {
                CustomComboBox comboBox = CurrentTextBox as CustomComboBox;
                if (isGet)
                {
                    value = comboBox.Text;
                }
                else
                {
                    comboBox.Text = value;
                }
            }
            else if (CurrentTextBox is CustomSearchTextBox)
            {
                CustomSearchTextBox searchTextBox = CurrentTextBox as CustomSearchTextBox;
                if (isGet)
                {
                    value = searchTextBox.Text;
                }
                else
                {
                    searchTextBox.Text = value;
                }
            }
            else if (CurrentTextBox is CustomTextBoxDatePicker)
            {
                CustomTextBoxDatePicker datePicker = CurrentTextBox as CustomTextBoxDatePicker;
                if (isGet)
                {
                    value = datePicker.Text;
                }
                else
                {
                    datePicker.Text = value;
                }
            }
            else if (CurrentTextBox is CustomButtonTextBox)
            {
                CustomButtonTextBox buttonTextBox = CurrentTextBox as CustomButtonTextBox;
                if (isGet)
                {
                    value = buttonTextBox.Text;
                }
                else
                {
                    buttonTextBox.Text = value;
                }
            }
            log.LogMethodExit();
            return value;
        }
        private int GetOrSetCaretIndex(bool assign = false, int caretIndex = -1)
        {
            log.LogMethodEntry();
            if (CurrentTextBox is CustomTextBox)
            {
                CustomTextBox customTextBox = CurrentTextBox as CustomTextBox;
                if (customTextBox != null)
                {
                    if (assign)
                    {
                        customTextBox.CaretIndex = caretIndex;
                    }
                    else
                    {
                        caretIndex = customTextBox.CaretIndex;
                        selectedText = customTextBox.SelectedText;
                    }
                }
            }
            else if (CurrentTextBox is CustomButtonTextBox)
            {
                CustomButtonTextBox customTextBox = CurrentTextBox as CustomButtonTextBox;
                if (customTextBox != null)
                {
                    if (assign)
                    {
                        customTextBox.CaretIndex = caretIndex;
                    }
                    else
                    {
                        caretIndex = customTextBox.CaretIndex;
                        selectedText = customTextBox.SelectedText;
                    }
                }
            }
            else if (CurrentTextBox is CustomSearchTextBox)
            {
                CustomSearchTextBox customSearchTextBox = CurrentTextBox as CustomSearchTextBox;
                if (customSearchTextBox != null)
                {
                    if (assign)
                    {
                        customSearchTextBox.CaretIndex = caretIndex;
                    }
                    else
                    {
                        caretIndex = customSearchTextBox.CaretIndex;
                        selectedText = customSearchTextBox.SelectedText;
                    }
                }
            }
            else if (CurrentTextBox is CustomTextBoxDatePicker)
            {
                CustomTextBoxDatePicker customTextBoxDatePicker = CurrentTextBox as CustomTextBoxDatePicker;
                if (customTextBoxDatePicker != null)
                {
                    if (assign)
                    {
                        customTextBoxDatePicker.CaretIndex = caretIndex;
                    }
                    else
                    {
                        caretIndex = customTextBoxDatePicker.CaretIndex;
                        selectedText = customTextBoxDatePicker.SelectedText;
                    }
                }
            }
            else if (CurrentTextBox is CustomComboBox)
            {
                CustomComboBox customComboBox = CurrentTextBox as CustomComboBox;
                if (customComboBox != null && customComboBox.TextBox != null)
                {
                    if (assign)
                    {
                        customComboBox.TextBox.CaretIndex = caretIndex;
                    }
                    else
                    {
                        caretIndex = customComboBox.TextBox.CaretIndex;
                        selectedText = customComboBox.TextBox.SelectedText;
                    }
                }
            }
            if (caretIndex == -1 && !assign)
            {
                caretIndex = KeyboardText.Length;
            }
            log.LogMethodExit();
            return caretIndex;
        }
        internal void OnButtonClicked(object param)
        {
            log.LogMethodEntry(param);

            Button keyButton = param as Button;

            if (keyButton != null)
            {
                caretIndex = GetOrSetCaretIndex();

                switch (keyButton.Name)
                {
                    case "btncmd1":
                        InsertText("1");
                        break;
                    case "btncmd2":
                        InsertText("2");
                        break;
                    case "btncmd3":
                        InsertText("3");
                        break;
                    case "btncmd4":
                        InsertText("4");
                        break;
                    case "btncmd5":
                        InsertText("5");
                        break;
                    case "btncmd6":
                        InsertText("6");
                        break;
                    case "btncmd7":
                        InsertText("7");
                        break;
                    case "btncmd8":
                        InsertText("8");
                        break;
                    case "btncmd9":
                        InsertText("9");
                        break;
                    case "btncmd0":
                        InsertText("0");
                        break;


                    case "btncmdQ":
                        KeyBoardInputCharacters('Q');
                        break;
                    case "btncmdW":
                        KeyBoardInputCharacters('W');
                        break;
                    case "btncmdE":
                        KeyBoardInputCharacters('E');
                        break;
                    case "btncmdR":
                        KeyBoardInputCharacters('R');
                        break;
                    case "btncmdT":
                        KeyBoardInputCharacters('T');
                        break;
                    case "btncmdY":
                        KeyBoardInputCharacters('Y');
                        break;
                    case "btncmdU":
                        KeyBoardInputCharacters('U');
                        break;
                    case "btncmdI":
                        KeyBoardInputCharacters('I');
                        break;
                    case "btncmdO":
                        KeyBoardInputCharacters('O');
                        break;
                    case "btncmdP":
                        KeyBoardInputCharacters('P');
                        break;
                    case "btncmdBackspace":
                        if (!string.IsNullOrEmpty(KeyboardText))
                        {
                            ClearTextOnBackSpace(selectedText);
                        }
                        break;
                    case "btncmdA":
                        KeyBoardInputCharacters('A');
                        break;
                    case "btncmdS":
                        KeyBoardInputCharacters('S');
                        break;
                    case "btncmdD":
                        KeyBoardInputCharacters('D');
                        break;
                    case "btncmdF":
                        KeyBoardInputCharacters('F');
                        break;
                    case "btncmdG":
                        KeyBoardInputCharacters('G');
                        break;
                    case "btncmdH":
                        KeyBoardInputCharacters('H');
                        break;
                    case "btncmdJ":
                        KeyBoardInputCharacters('J');
                        break;
                    case "btncmdK":
                        KeyBoardInputCharacters('K');
                        break;
                    case "btncmdL":
                        KeyBoardInputCharacters('L');
                        break;

                    case "btncmdSlash":
                        InsertText("/");
                        break;
                    case "btncmdDot":
                        InsertText(".");
                        break;
                    case "btncmdColon":
                        InsertText(":");
                        break;



                    case "btncmdAT"://Fourth Row
                        InsertText("@");
                        break;
                    case "btncmdDotCom":
                        InsertText(capsLockFlag ? ".COM" : ".com");
                        break;
                    case "btncmdZ":
                        KeyBoardInputCharacters('Z');
                        break;
                    case "btncmdX":
                        KeyBoardInputCharacters('X');
                        break;
                    case "btncmdC":
                        KeyBoardInputCharacters('C');
                        break;
                    case "btncmdV":
                        KeyBoardInputCharacters('V');
                        break;
                    case "btncmdB":
                        KeyBoardInputCharacters('B');
                        break;
                    case "btncmdN":
                        KeyBoardInputCharacters('N');
                        break;
                    case "btncmdM":
                        KeyBoardInputCharacters('M');
                        break;

                    case "btncmdPlus":
                        InsertText("+");
                        break;
                    case "btncmdHypen":
                        InsertText("-");
                        break;
                    case "btncmdUnderScore":
                        InsertText("_");
                        break;



                    case "btncmdCapsLock": //Last row
                        CapsLockFlag = !capsLockFlag;
                        break;


                    case "btncmdSpace":
                        InsertText(" ");
                        break;

                    case "btncmdOk":
                    case "btncmdClose":
                        if (keyboardWindow != null)
                        {
                            keyboardWindow.Close();
                            keyboardWindow = null;
                        }
                        break;
                }

                if (!CurrentTextBox.IsKeyboardFocused && keyButton.Name != "btncmdOk" && keyButton.Name != "btncmdClose")
                {
                    if (capsLockFlag && firstTime && keyButton.Name != "btncmdBackspace")
                    {
                        CapsLockFlag = !capsLockFlag;
                        firstTime = false;
                    }
                    if (CurrentTextBox is CustomComboBox)
                    {
                        if (!(CurrentTextBox as CustomComboBox).TextBox.IsKeyboardFocused)
                        {
                            Keyboard.Focus((CurrentTextBox as CustomComboBox).TextBox);
                        }
                    }
                    else
                    {
                        Keyboard.Focus(CurrentTextBox);
                    }
                    if (keyButton.Name == "btncmdBackspace")
                    {
                        if (caretIndex > 0)
                        {
                            caretIndex -= 1;
                        }
                    }
                    else if (caretIndex == KeyboardText.Length - 1)
                    {
                        caretIndex = KeyboardText.Length;
                    }
                    else if (keyButton.Name == "btncmdDotCom")
                    {
                        caretIndex += 4;
                    }
                    else
                    {
                        caretIndex += 1;
                    }
                    GetOrSetCaretIndex(true, caretIndex);
                }

                caretIndex = -1;
            }
            log.LogMethodExit();
        }
        private void InsertText(string text)
        {
            log.LogMethodEntry(text);
            KeyboardText = KeyboardText.Insert(caretIndex, text);
            log.LogMethodExit();
        }
        private void ClearTextOnBackSpace(string selectedText)
        {
            log.LogMethodEntry(caretIndex, selectedText);
            if (!string.IsNullOrEmpty(selectedText) && KeyboardText.Contains(selectedText))
            {
                KeyboardText = KeyboardText.Remove(KeyboardText.IndexOf(selectedText), selectedText.Length);
            }
            else
            {
                if (caretIndex > 0)
                {
                    int backupIndex = caretIndex - 1;
                    KeyboardText = KeyboardText.Remove(backupIndex, 1);
                }
                else
                {
                    KeyboardText = KeyboardText.Substring(0, KeyboardText.Length - 1);
                }
            }
            this.selectedText = string.Empty;
            log.LogMethodExit();
        }
        private void KeyBoardInputCharacters(char input)
        {
            log.LogMethodEntry();
            string pressedChar = input.ToString();
            if ((CapsLockFlag && ShiftFlag) || (!CapsLockFlag && !ShiftFlag))
            {
                pressedChar = char.ToLower(input).ToString();
            }
            if (ShiftFlag)
            {
                ShiftFlag = false;
            }
            InsertText(pressedChar);
            log.LogMethodExit();
        }
        #endregion

        #region Constructor
        public KeyboardVM()
        {
            log.LogMethodEntry();
            buttonClickedCommand = new DelegateCommand(OnButtonClicked);

            capsLockFlag = true;
            shiftFlag = false;
            firstTime = true;
            multiScreenMode = false;
            selectedTextBox = null;

            caretIndex = -1;
            selectedText = string.Empty;
            log.LogMethodExit();
        }
        #endregion

    }
}
