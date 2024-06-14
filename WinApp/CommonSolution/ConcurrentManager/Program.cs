using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.ConcurrentManager
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] argv)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                
                    //Application.Run(new LaunchExeManager());
            }
            catch (Exception ex)
            {
                MessageBox.Show("FATAL ERROR. Application will Close.\n" + ex.Message, "Concurrent Manager");
                MessageBox.Show(ex.StackTrace, "Semnox.Parafait.ConcurrentManager");
            }
        }
    }
}