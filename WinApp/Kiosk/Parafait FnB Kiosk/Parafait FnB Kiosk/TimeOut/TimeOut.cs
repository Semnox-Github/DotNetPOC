using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Parafait_Kiosk;

namespace Parafait_FnB_Kiosk
{
    public static class TimeOut
    {
        public static frmTimeOut timeOutForm;
        public static bool AbortTimeOut(Form owner)
        {
            timeOutForm = new frmTimeOut();
            if (timeOutForm.ShowDialog(owner) == DialogResult.Abort)
                return true;
            else
                return false;
        }

        public static void Abort()
        {
            try
            {
                timeOutForm.CloseForm();
            }
            catch { }
        }
    }
}
