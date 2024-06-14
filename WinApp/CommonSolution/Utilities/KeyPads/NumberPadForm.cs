/*
 *  * Project Name - NumberPadForm
 * Description  - NumberPadForm
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *****************************************************************************************************************
*2.90.0       23-Jun-2020      Raghuveera Variable refund changes to enable -ve amount entry
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Semnox.Core.Utilities
{
    public static class NumberPadForm
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static NumberPad numPad;
        static Form FormNumPad;
        static Utilities Utilities;
        static Form _parent;
        public static DialogResult dialogResult;

        public static double ShowNumberPadForm(string FormText, char firstKey, Utilities inUtilities, Form ParentForm = null)
        {
            log.LogMethodEntry(FormText, firstKey, inUtilities, ParentForm);
            Utilities = inUtilities;
            _parent = ParentForm;

            initialize(FormText);
            numPad.GetKey(firstKey);
            dialogResult = FormNumPad.ShowDialog(ParentForm);
            log.LogMethodExit(numPad.ReturnNumber);
            return numPad.ReturnNumber;
        }

        public static double ShowNumberPadForm(string FormText, string firstString, Utilities inUtilities, Form ParentForm = null)
        {
            log.LogMethodEntry(FormText, firstString, inUtilities, ParentForm);
            Utilities = inUtilities;
            _parent = ParentForm;

            initialize(FormText);
            numPad.handleaction(firstString);
            numPad.NewEntry = true;
                        
            dialogResult = FormNumPad.ShowDialog(ParentForm);
            log.LogMethodExit(numPad.ReturnNumber);
            return numPad.ReturnNumber;
        }

        static void initialize(string FormText)
        {
            log.LogMethodEntry(FormText);
            FormNumPad = new Form();
            FormNumPad.Name = "FormNumPad";
            FormNumPad.Text = FormText;
            if (_parent != null)
            {
                FormNumPad.StartPosition = FormStartPosition.Manual;
                FormNumPad.Location = new System.Drawing.Point(_parent.Location.X + _parent.Width / 2 - FormNumPad.Width / 2,
                                        _parent.Location.Y + _parent.Height / 2 - FormNumPad.Height / 2);
            }
            else
                FormNumPad.StartPosition = FormStartPosition.CenterParent;

            FormNumPad.FormBorderStyle = FormBorderStyle.FixedDialog;
            FormNumPad.SizeGripStyle = SizeGripStyle.Hide;
            FormNumPad.MinimizeBox = FormNumPad.MaximizeBox = false;

            numPad = new NumberPad(Utilities.ParafaitEnv.AMOUNT_FORMAT, Utilities.ParafaitEnv.RoundingPrecision);
            Panel NumberPadVarPanel = numPad.NumPadPanel();
            FormNumPad.Size = NumberPadVarPanel.Size;
            FormNumPad.Width = FormNumPad.Width + 15;
            FormNumPad.Height = FormNumPad.Height + 35;
            NumberPadVarPanel.Location = new System.Drawing.Point(FormNumPad.DisplayRectangle.Width / 2 - NumberPadVarPanel.Width / 2, FormNumPad.DisplayRectangle.Height / 2 - NumberPadVarPanel.Height / 2);
            FormNumPad.Controls.Add(NumberPadVarPanel);
            numPad.setReceiveAction = EventnumPadOKReceived;
                      
            FormNumPad.KeyPreview = true;

            FormNumPad.KeyPress += new KeyPressEventHandler(FormNumPad_KeyPress);
            FormNumPad.FormClosing += new FormClosingEventHandler(FormNumPad_FormClosing);
            log.LogMethodExit(null);
        }

        static void FormNumPad_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (FormNumPad.DialogResult == DialogResult.Cancel)
                numPad.ReturnNumber = -1;
            log.LogMethodExit(null);
        }

        static void FormNumPad_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.KeyChar == (char)Keys.Escape)
            {
                FormNumPad.DialogResult = DialogResult.Cancel;
                FormNumPad.Close();
            }
            else
                numPad.GetKey(e.KeyChar);
            log.LogMethodExit(null);
        }

        private static void EventnumPadOKReceived()
        {
            log.LogMethodEntry();
            FormNumPad.DialogResult = DialogResult.OK;
            FormNumPad.Close();
            log.LogMethodExit(null);
        }
    }
}
