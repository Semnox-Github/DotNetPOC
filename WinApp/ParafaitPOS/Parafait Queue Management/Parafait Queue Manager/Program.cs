using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ParafaitQueueManagement
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string [] argv)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new FormQueueMgmt());
            try
            {
                if (argv.Length > 0)
                {
                    if (argv[0] == "M") //Called from Management Studio
                    {
                        Application.Run(new FormQueueMgmt("M",argv[1],argv[2]));
                    }
                }
                else
                {
                    Application.Run(new FormQueueMgmt("U","",""));  //Called from command Line(needs authentication)
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
