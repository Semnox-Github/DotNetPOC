using System;
using System.Windows.Forms;
using System.Configuration;
using Semnox.Parafait.Currency;
using Semnox.Core.Utilities;
namespace CurrencyUI
{
    public partial class Form1 : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Utilities _Utilities;
        string connstring = "";

        public Form1()
        {
            log.LogMethodEntry();
            InitializeComponent();

            connstring = ConfigurationManager.ConnectionStrings["ParafaitUtils.Properties.Settings.ParafaitConnectionString"].ConnectionString;
            _Utilities = new Utilities(connstring);
            _Utilities.ParafaitEnv.User_Id = 6;
            _Utilities.ParafaitEnv.LoginID = "Semnox Test";
            _Utilities.ParafaitEnv.Username = "Semnox";
            _Utilities.ParafaitEnv.SiteId = 1010;
            log.LogMethodExit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            frmCurrency frm = new frmCurrency(_Utilities);
            frm.ShowDialog();
            log.LogMethodExit();
        }
    }
}
