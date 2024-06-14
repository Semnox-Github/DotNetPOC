/*
 *  * Project Name - NumberPad
 * Description  - NumberPad
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *****************************************************************************************************************
*2.90.0       23-Jun-2020      Raghuveera     Variable refund changes to enable -ve amount entry
*2.130.7      13-Apr-2022      Guru S A       Payment mode OTP validation changes
*2.130.11     13-Oct-2022      Vignesh Bhat   Tender Amount UI enhancement to support AMOUNT_FORMAT configuration   
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Semnox.Core.Utilities
{
    public class NumberPad
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private System.Windows.Forms.TextBox textBoxNumPadDisplay;
        private System.Windows.Forms.TextBox textBoxNumPadDisplayHidden;
        private System.Windows.Forms.Panel panelNumpadDisplay;
        private System.Windows.Forms.Panel panelNumPad;
        private System.Windows.Forms.Button RefButton;

        private System.Windows.Forms.Button OKButton;

        public bool NewEntry = true;

        public delegate void ReceiveAction();
        ReceiveAction receiveAction;

        public delegate void KeyAction();
        KeyAction keyAction;

        public double ReturnNumber;
        public string ReturnNumberString;

        static string decimalStr = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;
        char decimalChar = decimalStr[0];
        static string AMOUNT_FORMAT;
        static int _RoundingPrecision = 2;

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

        public NumberPad(string AmountFormat, int RoundingPrecision = 2)
        {
            log.LogMethodEntry(AmountFormat, RoundingPrecision);
            AMOUNT_FORMAT = AmountFormat;
            _RoundingPrecision = RoundingPrecision;

            panelNumPad = new System.Windows.Forms.Panel();
            panelNumpadDisplay = new System.Windows.Forms.Panel();

            textBoxNumPadDisplay = new System.Windows.Forms.TextBox();
            textBoxNumPadDisplay.BackColor = System.Drawing.Color.Gainsboro;
            textBoxNumPadDisplay.Font = new System.Drawing.Font("Arial", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            textBoxNumPadDisplay.BorderStyle = BorderStyle.None;
            textBoxNumPadDisplay.Location = new System.Drawing.Point(1, 3);
            textBoxNumPadDisplay.Name = "textBoxNumPadDisplay";
            textBoxNumPadDisplay.Size = new System.Drawing.Size(250, 35);
            textBoxNumPadDisplay.Text = 0.ToString(AMOUNT_FORMAT);
            textBoxNumPadDisplay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            textBoxNumPadDisplay.TabStop = false;
            textBoxNumPadDisplay.ReadOnly = true;

            textBoxNumPadDisplayHidden = new System.Windows.Forms.TextBox();

            panelNumpadDisplay.BackColor = System.Drawing.Color.Gray;
            panelNumpadDisplay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panelNumpadDisplay.Controls.Add(textBoxNumPadDisplay);
            panelNumpadDisplay.Location = new System.Drawing.Point(4, 3);
            panelNumpadDisplay.Name = "panelNumpadDisplay";
            panelNumpadDisplay.Size = new System.Drawing.Size(255, 45);

            // panelNumPad
            // 
            panelNumPad.Controls.Add(panelNumpadDisplay);
            panelNumPad.Location = new System.Drawing.Point(1, 0);
            panelNumPad.Name = "panelNumPad";
            panelNumPad.Size = new System.Drawing.Size(265, 312);
            panelNumPad.BackColor = System.Drawing.Color.Gray;
            panelNumPad.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            RefButton = new Button();

            RefButton.BackColor = System.Drawing.Color.White;
            RefButton.Font = new System.Drawing.Font("Arial", 22F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            RefButton.ForeColor = System.Drawing.Color.White;
            RefButton.Location = new System.Drawing.Point(3, 50);
            RefButton.Name = "buttonNumPad";
            RefButton.Size = new System.Drawing.Size(65, 65);
            RefButton.Text = "Ref";
            RefButton.UseVisualStyleBackColor = false;

            for (int i = 1; i <= 14; i++)
            {
                Button buttonNumPad = new Button();

                buttonNumPad.FlatStyle = FlatStyle.Flat;
                buttonNumPad.FlatAppearance.BorderSize = 0;
                buttonNumPad.FlatAppearance.MouseOverBackColor =
                    buttonNumPad.FlatAppearance.MouseDownBackColor =
                    buttonNumPad.BackColor = System.Drawing.Color.Transparent;
                buttonNumPad.BackgroundImage = KeyPads.KeyPadResources._0;
                buttonNumPad.BackgroundImageLayout = ImageLayout.Stretch;

                buttonNumPad.Font = RefButton.Font;
                buttonNumPad.ForeColor = RefButton.ForeColor;
                buttonNumPad.Size = RefButton.Size;
                buttonNumPad.UseVisualStyleBackColor = RefButton.UseVisualStyleBackColor;
                buttonNumPad.Name = RefButton.Name + i.ToString();
                int row, col;
                row = Math.DivRem(i - 1, 4, out col);
                buttonNumPad.Location = new System.Drawing.Point(RefButton.Location.X + col * (RefButton.Size.Width - 1), RefButton.Location.Y + row * (RefButton.Size.Height - 1));

                switch (i)
                {
                    case 1: buttonNumPad.BackgroundImage = KeyPads.KeyPadResources._1; break;
                    case 2: buttonNumPad.BackgroundImage = KeyPads.KeyPadResources._2; break;
                    case 3: buttonNumPad.BackgroundImage = KeyPads.KeyPadResources._3; break;
                    case 4: buttonNumPad.BackgroundImage = KeyPads.KeyPadResources.clear; break;
                    case 5: buttonNumPad.BackgroundImage = KeyPads.KeyPadResources._4; break;
                    case 6: buttonNumPad.BackgroundImage = KeyPads.KeyPadResources._5; break;
                    case 7: buttonNumPad.BackgroundImage = KeyPads.KeyPadResources._6; break;
                    case 8: buttonNumPad.BackgroundImage = KeyPads.KeyPadResources.back; break;
                    case 9: buttonNumPad.BackgroundImage = KeyPads.KeyPadResources._7; break;
                    case 10: buttonNumPad.BackgroundImage = KeyPads.KeyPadResources._8; break;
                    case 11: buttonNumPad.BackgroundImage = KeyPads.KeyPadResources._9; break;
                    case 12: buttonNumPad.BackgroundImage = KeyPads.KeyPadResources.dot; break;
                    case 13: buttonNumPad.BackgroundImage = KeyPads.KeyPadResources._0; break;
                    case 14: buttonNumPad.BackgroundImage = KeyPads.KeyPadResources.ok; break;
                }

                switch (i)
                {
                    case 4: buttonNumPad.Name = RefButton.Name + "Cancel";
                        break;
                    case 8: buttonNumPad.Name = RefButton.Name + "BackSpace";
                        break;
                    case 12: buttonNumPad.Name = RefButton.Name + "Dot";
                        buttonNumPad.Text = decimalStr;
                        break;
                    case 13: buttonNumPad.Name = RefButton.Name + "0";
                        buttonNumPad.Width = buttonNumPad.Size.Width * 2 - 1;
                        break;
                    case 14: buttonNumPad.Width = buttonNumPad.Size.Width * 2;
                        buttonNumPad.Name = RefButton.Name + "OK";
                        buttonNumPad.Location = new System.Drawing.Point(buttonNumPad.Location.X + RefButton.Width - 1, buttonNumPad.Location.Y);
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
                        buttonNumPad.Name = RefButton.Name + name;
                        break;
                }
                buttonNumPad.Click += new EventHandler(buttonNumPad_Click);
                buttonNumPad.MouseDown += buttonNumPad_MouseDown;
                buttonNumPad.MouseUp += buttonNumPad_MouseUp;
                panelNumPad.Controls.Add(buttonNumPad);
            }

            NewEntry = true;
            OKButton.TabIndex = 0;
            log.LogMethodExit(null);
        }

        void buttonNumPad_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Button buttonNumPad = (Button)sender;
            String Action = buttonNumPad.Name.Substring(RefButton.Name.Length);
            switch (Action)
            {
                case "1": buttonNumPad.BackgroundImage = KeyPads.KeyPadResources._1; break;
                case "2": buttonNumPad.BackgroundImage = KeyPads.KeyPadResources._2; break;
                case "3": buttonNumPad.BackgroundImage = KeyPads.KeyPadResources._3; break;
                case "Cancel": buttonNumPad.BackgroundImage = KeyPads.KeyPadResources.clear; break;
                case "4": buttonNumPad.BackgroundImage = KeyPads.KeyPadResources._4; break;
                case "5": buttonNumPad.BackgroundImage = KeyPads.KeyPadResources._5; break;
                case "6": buttonNumPad.BackgroundImage = KeyPads.KeyPadResources._6; break;
                case "BackSpace": buttonNumPad.BackgroundImage = KeyPads.KeyPadResources.back; break;
                case "7": buttonNumPad.BackgroundImage = KeyPads.KeyPadResources._7; break;
                case "8": buttonNumPad.BackgroundImage = KeyPads.KeyPadResources._8; break;
                case "9": buttonNumPad.BackgroundImage = KeyPads.KeyPadResources._9; break;
                case "Dot": buttonNumPad.BackgroundImage = KeyPads.KeyPadResources.dot; break;
                case "0": buttonNumPad.BackgroundImage = KeyPads.KeyPadResources._0; break;
                case "OK": buttonNumPad.BackgroundImage = KeyPads.KeyPadResources.ok; break;
            }
            log.LogMethodExit(null);
        }

        void buttonNumPad_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Button buttonNumPad = (Button)sender;
            String Action = buttonNumPad.Name.Substring(RefButton.Name.Length);
            switch (Action)
            {
                case "1": buttonNumPad.BackgroundImage = KeyPads.KeyPadResources._1_pressed; break;
                case "2": buttonNumPad.BackgroundImage = KeyPads.KeyPadResources._2_pressed; break;
                case "3": buttonNumPad.BackgroundImage = KeyPads.KeyPadResources._3_pressed; break;
                case "Cancel": buttonNumPad.BackgroundImage = KeyPads.KeyPadResources.clear_pressed; break;
                case "4": buttonNumPad.BackgroundImage = KeyPads.KeyPadResources._4_pressed; break;
                case "5": buttonNumPad.BackgroundImage = KeyPads.KeyPadResources._5_pressed; break;
                case "6": buttonNumPad.BackgroundImage = KeyPads.KeyPadResources._6_pressed; break;
                case "BackSpace": buttonNumPad.BackgroundImage = KeyPads.KeyPadResources.back_pressed; break;
                case "7": buttonNumPad.BackgroundImage = KeyPads.KeyPadResources._7_pressed; break;
                case "8": buttonNumPad.BackgroundImage = KeyPads.KeyPadResources._8_pressed; break;
                case "9": buttonNumPad.BackgroundImage = KeyPads.KeyPadResources._9_pressed; break;
                case "Dot": buttonNumPad.BackgroundImage = KeyPads.KeyPadResources.dot_pressed; break;
                case "0": buttonNumPad.BackgroundImage = KeyPads.KeyPadResources._0_pressed; break;
                case "OK": buttonNumPad.BackgroundImage = KeyPads.KeyPadResources.ok_pressed; break;
            }
            log.LogMethodExit(null);
        }

        void buttonNumPad_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Button buttonNumPad = (Button)sender;
            String Action = buttonNumPad.Name.Substring(RefButton.Name.Length);
            handleaction(Action);
            OKButton.Focus();

            if (keyAction != null)
            {
                try
                {
                    ReturnNumberString = textBoxNumPadDisplay.Text;
                    ReturnNumber = Math.Round(Convert.ToDouble(textBoxNumPadDisplay.Text), _RoundingPrecision + 2, MidpointRounding.AwayFromZero);
                }
                catch(Exception ex)
                {
                    log.Error("Error occured while performing a mathematical function such as round", ex);
                }
                keyAction.Invoke();
            }
            log.LogMethodExit(null);
        }

        public void handleaction(string Action)
        {
            log.LogMethodEntry(Action);
            bool OKPressed = false;

            switch (Action)
            {
                case "Cancel": NewEntry = true; break;
                case "BackSpace":
                    if (!NewEntry)
                    {
                        textBoxNumPadDisplayHidden.Text = textBoxNumPadDisplayHidden.Text.Substring(0, textBoxNumPadDisplayHidden.Text.Length - 1);
                        if (textBoxNumPadDisplayHidden.Text == "")
                            NewEntry = true;
                    }
                    break;
                case "Dot":
                    if (NewEntry)
                    {
                        textBoxNumPadDisplayHidden.Text = "";
                        NewEntry = false;
                    }
                    if (!textBoxNumPadDisplayHidden.Text.Contains(decimalStr))
                    {
                        textBoxNumPadDisplayHidden.AppendText(decimalStr);
                    }
                    break;
                case "OK":
                    if (textBoxNumPadDisplayHidden.Text == decimalStr)
                    {
                        ReturnNumber = 0;
                        ReturnNumberString = String.Empty;
                    }
                    else
                        try
                        {
                            ReturnNumberString = textBoxNumPadDisplayHidden.Text;
                            ReturnNumber = Math.Round(Convert.ToDouble(textBoxNumPadDisplayHidden.Text), _RoundingPrecision + 2, MidpointRounding.AwayFromZero);
                        }
                        catch (Exception ex)
                        {
                            log.Error("Error occured while performing a mathematical function such as round", ex);
                        }
                    textBoxNumPadDisplayHidden.Text = string.Format("{0:" + AMOUNT_FORMAT + "}", ReturnNumber);
                    NewEntry = true;
                    OKPressed = true;
                    receiveAction.Invoke();
                    break;
                case "N":
                    textBoxNumPadDisplayHidden.Text = "-" + string.Format("{0:" + AMOUNT_FORMAT + "}", ReturnNumber);
                    NewEntry = true;
                    break;
                default:
                    if (NewEntry)
                    {
                        if (textBoxNumPadDisplayHidden.Text.Contains("-"))
                        {
                            textBoxNumPadDisplayHidden.Text = "-";
                        }
                        else
                        {
                            textBoxNumPadDisplayHidden.Text = "";
                        }
                    }
                    textBoxNumPadDisplayHidden.AppendText(Action);
                    NewEntry = false;
                    break;
            }

            if (OKPressed)
            {
                OKPressed = false;
            }
            else if (NewEntry)
            {
                if (textBoxNumPadDisplayHidden.Text.Contains("-"))
                {
                    textBoxNumPadDisplayHidden.Text = "-" + 0.ToString(AMOUNT_FORMAT);
                }
                else
                {
                    textBoxNumPadDisplayHidden.Text = 0.ToString(AMOUNT_FORMAT);
                }
            }
            textBoxNumPadDisplay.Text = ConvertToFormattedAmount(textBoxNumPadDisplayHidden.Text, AMOUNT_FORMAT);
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
                    else if (Key.Equals('N'))
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
 
        /// <summary>
        /// ConvertToFormattedAmount - Converts amount to amount format
        /// </summary>
        public static string ConvertToFormattedAmount(string amount, string amountFormat)
        {
            log.LogMethodEntry(amount, amountFormat);
            decimal parsedAmount = 0;
            string formattedAmount;
            if (decimal.TryParse(amount, out parsedAmount) == false)
            {
                formattedAmount = amount;
            }
            else
            {
                formattedAmount = parsedAmount.ToString(amountFormat).Trim();
            }
            log.LogMethodExit(formattedAmount);
            return formattedAmount;
        }
    }
}

