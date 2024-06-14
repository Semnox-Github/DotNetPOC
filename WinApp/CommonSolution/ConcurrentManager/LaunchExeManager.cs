using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using Semnox.Core.Utilities;
using Semnox.Parafait.JobUtils;

namespace Semnox.Parafait.ConcurrentManager
{
    /// <summary>
    /// LaunchExeManager UI Class
    /// </summary>
    public partial class LaunchExeManager : Form
    {
        Utilities _Utilities;
        string connstring = "";

        /// <summary>
        /// Default constructor of LaunchExeManager
        /// </summary>
        public LaunchExeManager()
        {
            InitializeComponent();
            connstring = ConfigurationManager.ConnectionStrings["ParafaitUtils.Properties.Settings.ParafaitConnectionString"].ConnectionString;
            _Utilities = new Utilities(connstring);
            _Utilities.ParafaitEnv.User_Id = 3;
            _Utilities.ParafaitEnv.LoginID = "semnox";
            _Utilities.ParafaitEnv.Username = "Semnox";
            //_Utilities.ParafaitEnv.SetPOSMachine("", _Utilities.ParafaitEnv.POSMachine);
            _Utilities.ParafaitEnv.Initialize();
           // _Utilities.ParafaitEnv.SiteId = 1010;
           // _Utilities.ParafaitEnv.SiteName = "Smaash";
        }

        private void concurrentProgramBtn_Click(object sender, EventArgs e)
        {
            try
            {
                ConcurrentProgramUI conProUI = new ConcurrentProgramUI(_Utilities);
                conProUI.Show();
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                Convert.ToInt32(526598);
                ConcLib concLib = new ConcLib(_Utilities, _Utilities.ExecutionContext);
                concLib.MessagingTriggers(1, "MessageTest.log");

                //ConcurrentProgramsLaunchUI conProUI = new ConcurrentProgramsLaunchUI();
                //conProUI.Show();
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.ToString());
            }
        }


        ConcurrentProgramsLaunch cls;
        private void btn3_Click(object sender, EventArgs e)
        {
            cls = new ConcurrentProgramsLaunch(_Utilities);
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
   
            timer.Interval = 50 * 1000;
            timer.Tick += timer_Tick;
            timer.Start();

            cls.LoadConcurrentEngine();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            cls.CheckRequests();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //frmJobStatus cls = new frmJobStatus(_Utilities);
            //cls.ShowDialog();
            Convert.ToInt32(526598);
            ConcLib concLib = new ConcLib(_Utilities, _Utilities.ExecutionContext);
            concLib.SendMessage(1, "MessageTest.log");
        }       
    }
}
