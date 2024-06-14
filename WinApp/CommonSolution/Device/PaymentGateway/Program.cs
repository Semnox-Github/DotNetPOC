//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using System.Configuration;

//namespace Semnox.Parafait.Device.PaymentGateway
//{
//    static class Program
//    {
//        /// <summary>
//        /// The main entry point for the application.
//        /// </summary>
//        [STAThread]
//        static void Main()
//        {
//            Utilities _Utilities;
//            string connstring = "";
//            connstring = ConfigurationManager.ConnectionStrings["ParafaitConnectionString"].ConnectionString;
//            _Utilities = new Utilities(connstring);
//            _Utilities.ParafaitEnv.User_Id = 6;
//            _Utilities.ParafaitEnv.LoginID = "Semnox";
//            _Utilities.ParafaitEnv.SiteId = 1010;


//            Application.EnableVisualStyles();
//            Application.SetCompatibleTextRenderingDefault(false);
//            Application.Run(new Main());
//        }
//    }
//}
