using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Semnox.Parafait.logging;

namespace Semnox.Core.Utilities.KeyPads.Kiosk
{
    public static class NumberPadForm
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static frmNumberPad FormNumPad;
        static Utilities Utilities;
        static Form _parent;

        public static double ShowNumberPadForm(string FormText, char firstKey, Utilities inUtilities, Form ParentForm = null)
        {
            log.LogMethodEntry(FormText, firstKey, inUtilities, ParentForm);
            Utilities = inUtilities;
            _parent = ParentForm;

            initialize(FormText);
            FormNumPad.GetKey(firstKey);
            
            DialogResult DR = FormNumPad.ShowDialog(ParentForm);
            log.LogMethodExit(FormNumPad.ReturnNumber);
            return FormNumPad.ReturnNumber;
        }

        public static double ShowNumberPadForm(string FormText, string firstString, Utilities inUtilities, Form ParentForm = null)
        {
            log.LogMethodEntry(FormText, firstString, inUtilities, ParentForm);
            Utilities = inUtilities;
            _parent = ParentForm;

            initialize(FormText);
            FormNumPad.handleaction(firstString);
            FormNumPad.NewEntry = true;
                        
            DialogResult DR = FormNumPad.ShowDialog(ParentForm);
            log.LogMethodExit(FormNumPad.ReturnNumber);
            return FormNumPad.ReturnNumber;
        }

        static void initialize(string FormText)
        {
            log.LogMethodEntry(FormText);
            FormNumPad = new frmNumberPad();
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

            FormNumPad.FormBorderStyle = FormBorderStyle.None;
            FormNumPad.SizeGripStyle = SizeGripStyle.Hide;
            FormNumPad.MinimizeBox = FormNumPad.MaximizeBox = false;

            FormNumPad.Init(Utilities.ParafaitEnv.AMOUNT_FORMAT, Utilities.ParafaitEnv.RoundingPrecision);
            FormNumPad.setReceiveAction = EventnumPadOKReceived;
                      
            FormNumPad.KeyPreview = true;

            FormNumPad.KeyPress += new KeyPressEventHandler(FormNumPad_KeyPress);
            FormNumPad.FormClosing += new FormClosingEventHandler(FormNumPad_FormClosing);
            log.LogMethodExit(null);
        }

        static void FormNumPad_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (FormNumPad.DialogResult == DialogResult.Cancel)
                FormNumPad.ReturnNumber = -1;
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
                FormNumPad.GetKey(e.KeyChar);
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
