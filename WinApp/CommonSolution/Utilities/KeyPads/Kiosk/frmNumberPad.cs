/********************************************************************************************
 * Project Name - KeyPads.Kiosk 
 * Description  -frmNumberPad
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 ********************************************************************************************
 *2.150.1     22-Feb-2023      Guru S A          Kiosk Cart Enhancements
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Semnox.Parafait.logging;

namespace Semnox.Core.Utilities.KeyPads.Kiosk
{
    public partial class frmNumberPad : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int kioskSecondsRemaining;
        private int kioskTimerTick = 10;
        public frmNumberPad()
        {
            log.LogMethodEntry();
            InitializeComponent();

            kioskTimer.Interval = 1000;
            kioskTimer.Enabled = true;
            kioskTimer.Tick += KioskTimer_Tick;
            ResetKioskTimer(); 
            this.FormClosed += delegate
            {
                kioskTimer.Stop();
            };
            StopKioskTimer();
            log.LogMethodExit(null);
        }

        private System.Windows.Forms.Button OKButton;

        public bool NewEntry = true;

        public delegate void ReceiveAction();
        ReceiveAction receiveAction;

        public delegate void KeyAction();
        KeyAction keyAction;

        public double ReturnNumber;

        static string decimalStr = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;
        char decimalChar = decimalStr[0];
        static string AMOUNT_FORMAT;
        static int _RoundingPrecision = 2;

        string ButtonNamePrefix = "buttonNumPad";
        public ReceiveAction setReceiveAction
        {
            get
            {
                return receiveAction;
            }
            set
            {
                receiveAction = value;
            }
        }

        public KeyAction setKeyAction
        {
            get
            {
                return keyAction;
            }
            set
            {
                keyAction = value;
            }
        }

        public System.Windows.Forms.Panel NumPadPanel()
        {
            log.LogMethodEntry();
            log.LogMethodExit(panelNumPad);
            return panelNumPad;
        }

        public void Init(string AmountFormat, int RoundingPrecision = 2, int displayValue = 0)
        {
            log.LogMethodEntry(AmountFormat, RoundingPrecision, displayValue);
            try
            {
                StartKioskTimer();
                ResetKioskTimer();
                AMOUNT_FORMAT = AmountFormat;
                _RoundingPrecision = RoundingPrecision;

                textBoxNumPadDisplay.Text = displayValue.ToString(AMOUNT_FORMAT);
                textBoxNumPadDisplay.TabStop = false;

                for (int i = 1; i <= 14; i++)
                {
                    Button buttonNumPad = flpKeys.Controls[i - 1] as Button;

                    buttonNumPad.Name = ButtonNamePrefix + i.ToString();

                    switch (i)
                    {
                        case 4:
                            buttonNumPad.Name = ButtonNamePrefix + "Cancel";
                            break;
                        case 8:
                            buttonNumPad.Name = ButtonNamePrefix + "BackSpace";
                            break;
                        case 12:
                            buttonNumPad.Name = ButtonNamePrefix + "Dot";
                            buttonNumPad.Text = decimalStr;
                            break;
                        case 13:
                            buttonNumPad.Name = ButtonNamePrefix + "0";
                            break;
                        case 14:
                            buttonNumPad.Name = ButtonNamePrefix + "OK";
                            OKButton = buttonNumPad;
                            break;
                        default:
                            string name = "";
                            if (i < 4)
                                name = i.ToString();
                            else if (i < 8)
                                name = (i - 1).ToString();
                            else if (i < 12)
                                name = (i - 2).ToString();
                            buttonNumPad.Name = ButtonNamePrefix + name;
                            break;
                    }
                    buttonNumPad.Click += new EventHandler(buttonNumPad_Click);
                    buttonNumPad.MouseDown += buttonNumPad_MouseDown;
                    buttonNumPad.MouseUp += buttonNumPad_MouseUp;
                }

                NewEntry = true;
                OKButton.TabIndex = 0;
            }
            finally
            {
                ResetKioskTimer();
            }
            log.LogMethodExit(null);
        }

        void buttonNumPad_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Button buttonNumPad = (Button)sender;
            String Action = buttonNumPad.Name.Substring(ButtonNamePrefix.Length);
            switch (Action)
            {
                case "1": buttonNumPad.BackgroundImage = Resource.keypad_button1; break;
                case "2": buttonNumPad.BackgroundImage = Resource.keypad_button1; break;
                case "3": buttonNumPad.BackgroundImage = Resource.keypad_button1; break;
                case "Cancel": buttonNumPad.BackgroundImage = Resource.keypad_button2; break;
                case "4": buttonNumPad.BackgroundImage = Resource.keypad_button1; break;
                case "5": buttonNumPad.BackgroundImage = Resource.keypad_button1; break;
                case "6": buttonNumPad.BackgroundImage = Resource.keypad_button1; break;
                case "BackSpace": buttonNumPad.BackgroundImage = Resource.back_space; break;
                case "7": buttonNumPad.BackgroundImage = Resource.keypad_button1; break;
                case "8": buttonNumPad.BackgroundImage = Resource.keypad_button1; break;
                case "9": buttonNumPad.BackgroundImage = Resource.keypad_button1; break;
                case "Dot": buttonNumPad.BackgroundImage = Resource.keypad_button1; break;
                case "0": buttonNumPad.BackgroundImage = Resource.keypad_button3; break;
                case "OK": buttonNumPad.BackgroundImage = Resource.keypad_button3; break;
            }
            log.LogMethodExit(null);
        }

        void buttonNumPad_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                ResetKioskTimer();
                Button buttonNumPad = (Button)sender;
                String Action = buttonNumPad.Name.Substring(ButtonNamePrefix.Length);
                switch (Action)
                {
                    case "1": buttonNumPad.BackgroundImage = Resource.keypad_button1_pressed; break;
                    case "2": buttonNumPad.BackgroundImage = Resource.keypad_button1_pressed; break;
                    case "3": buttonNumPad.BackgroundImage = Resource.keypad_button1_pressed; break;
                    case "Cancel": buttonNumPad.BackgroundImage = Resource.keypad_button2_pressed; break;
                    case "4": buttonNumPad.BackgroundImage = Resource.keypad_button1_pressed; break;
                    case "5": buttonNumPad.BackgroundImage = Resource.keypad_button1_pressed; break;
                    case "6": buttonNumPad.BackgroundImage = Resource.keypad_button1_pressed; break;
                    case "BackSpace": buttonNumPad.BackgroundImage = Resource.back_space_pressed; break;
                    case "7": buttonNumPad.BackgroundImage = Resource.keypad_button1_pressed; break;
                    case "8": buttonNumPad.BackgroundImage = Resource.keypad_button1_pressed; break;
                    case "9": buttonNumPad.BackgroundImage = Resource.keypad_button1_pressed; break;
                    case "Dot": buttonNumPad.BackgroundImage = Resource.keypad_button1_pressed; break;
                    case "0": buttonNumPad.BackgroundImage = Resource.keypad_button3_pressed; break;
                    case "OK": buttonNumPad.BackgroundImage = Resource.keypad_button3_pressed; break;
                }
            }
            finally
            {
                ResetKioskTimer();
            }
            log.LogMethodExit(null);
        }

        void buttonNumPad_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                ResetKioskTimer();
                Button buttonNumPad = (Button)sender;
                String Action = buttonNumPad.Name.Substring(ButtonNamePrefix.Length);
                handleaction(Action);
                OKButton.Focus();

                if (keyAction != null)
                {
                    try
                    {
                        ReturnNumber = Math.Round(Convert.ToDouble(textBoxNumPadDisplay.Text), _RoundingPrecision + 2, MidpointRounding.AwayFromZero);
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error occured while performing mathematical function such as round", ex);
                    }
                    keyAction.Invoke();
                }
            }
            finally
            {
                ResetKioskTimer();
            }
            log.LogMethodExit(null);
        }

        public void handleaction(string Action)
        {
            log.LogMethodEntry(Action);
            try
            {
                ResetKioskTimer();
                bool OKPressed = false;
                switch (Action)
                {
                    case "Cancel": NewEntry = true; break;
                    case "BackSpace":
                        if (!NewEntry)
                        {
                            textBoxNumPadDisplay.Text = textBoxNumPadDisplay.Text.Substring(0, textBoxNumPadDisplay.Text.Length - 1);
                            if (textBoxNumPadDisplay.Text == "")
                                NewEntry = true;
                        }
                        break;
                    case "Dot":
                        if (NewEntry)
                        {
                            textBoxNumPadDisplay.Text = "";
                            NewEntry = false;
                        }
                        if (!textBoxNumPadDisplay.Text.Contains(decimalStr)) textBoxNumPadDisplay.Text += decimalStr; break;
                    case "OK":
                        if (textBoxNumPadDisplay.Text == decimalStr)
                            ReturnNumber = 0;
                        else
                            try
                            {
                                ReturnNumber = Math.Round(Convert.ToDouble(textBoxNumPadDisplay.Text), _RoundingPrecision + 2, MidpointRounding.AwayFromZero);
                            }
                            catch (Exception ex)
                            {
                                log.Error("Error occured while performing mathematical function such as round", ex);
                            }
                        textBoxNumPadDisplay.Text = string.Format("{0:" + AMOUNT_FORMAT + "}", ReturnNumber);
                        NewEntry = true;
                        OKPressed = true;
                        receiveAction.Invoke();
                        break;
                    default:
                        if (NewEntry)
                        {
                            textBoxNumPadDisplay.Text = "";
                        }
                        textBoxNumPadDisplay.Text += Action;
                        NewEntry = false;
                        break;
                }
                if (OKPressed)
                    OKPressed = false;
                else if (NewEntry)
                    textBoxNumPadDisplay.Text = 0.ToString(AMOUNT_FORMAT);
            }
            finally
            {
                ResetKioskTimer();
            }
            log.LogMethodExit(null);
        }

        public void GetKey(Char Key)
        {
            log.LogMethodEntry(Key);
            string Action;
            switch ((Keys)Key)
            {
                case Keys.Escape: Action = "Cancel"; break;
                case Keys.Enter: Action = "OK"; break;
                case Keys.Back: Action = "BackSpace"; break;
                default:
                    if (Key == decimalChar)
                    {
                        Action = "Dot";
                    }
                    else if (Key >= '0' && Key <= '9')
                    {
                        Action = Key.ToString();
                    }
                    else
                    {
                        Action = "XX";
                    }
                    break;
            }
            if (Action != "XX")
                handleaction(Action);
            OKButton.Focus();
            log.LogMethodExit(null);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            DialogResult = DialogResult.Cancel;
            Close();
            log.LogMethodExit(null);
        }

        private void btnCloseX_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            DialogResult = DialogResult.Cancel;
            Close();
            log.LogMethodExit(null);
        }
        private void KioskTimer_Tick(object sender, EventArgs e)
        {
            //log.LogMethodEntry(sender, e);
            if (this == ActiveForm)
            {
                if (kioskSecondsRemaining <= 0)
                {
                    this.Close();
                }
                kioskSecondsRemaining--;
            }
            else
            {
                ResetKioskTimer();
            }
            // log.LogMethodExit();
        }
        private void StopKioskTimer()
        {
            log.LogMethodEntry();
            kioskTimer.Stop();
            log.LogMethodExit();
        }
        private void StartKioskTimer()
        {
            log.LogMethodEntry();
            kioskTimer.Start();
            log.LogMethodExit();
        }
        private void ResetKioskTimer()
        {
            log.LogMethodEntry();
            try
            {
                kioskSecondsRemaining = kioskTimerTick;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(kioskSecondsRemaining);
        }
    }
}
