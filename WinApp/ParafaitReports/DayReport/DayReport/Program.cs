using System;
using System.Windows.Forms;

namespace DayReport
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //var si = new System.Diagnostics.ProcessStartInfo();
                //si.CreateNoWindow = true;
                //si.FileName = "test.cmd";
                //System.Diagnostics.Process.Start(si);              
                Application.Run(new frmMain());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.StackTrace);
            }
        }
    }
}
