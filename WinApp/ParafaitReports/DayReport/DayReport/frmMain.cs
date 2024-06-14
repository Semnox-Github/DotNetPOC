using Semnox.Core.Utilities;
using System;
using System.Configuration;
using System.Threading;
using System.Windows.Forms;

namespace DayReport
{
    public partial class frmMain : Form
    {
        Utilities utilities;
        string time;        
        DayReport dayRpt;
        string connstring = "";
        public frmMain()
        {
            InitializeComponent();
            this.Height = Convert.ToInt16(System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height - 40);
        }

        private const int CS_NOCLOSE = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle = cp.ClassStyle | CS_NOCLOSE;
                return cp;
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.Show();
            this.Activate();
            this.Refresh();
            bool status = false;
            int i = 10;
            if (!System.IO.Directory.Exists( ".\\Log"))
            {
                System.IO.Directory.CreateDirectory(".\\Log");
            }
            if (!System.IO.File.Exists(".\\Log\\Log.txt"))
            {
                System.IO.File.WriteAllText(".\\Log\\Log.txt", DateTime.Now.ToString() + ":Server started." + Environment.NewLine);
            }
            else
            {
                System.IO.File.AppendAllText(".\\Log\\Log.txt", DateTime.Now.ToString() + ":Server started." + Environment.NewLine);
            }
            writeLog(DateTime.Now.ToString() + ":Server started.");
            connstring = ConfigurationManager.ConnectionStrings["ParafaitConnectionString"].ConnectionString;
            while (i > 0)
            {
                try
                {
                    utilities = new Utilities(connstring);
                    status = true;
                    break;
                }
                catch
                {
                    writeLog(DateTime.Now.ToString() + ":Error in database connection...");
                    System.IO.File.AppendAllText(".\\Log\\Log.txt", DateTime.Now.ToString() + ":Error in database connection..." + Environment.NewLine);
                    status = false;                                                                      
                }               
                i--;
                Thread.Sleep(6000); 
            }
            if (!status)
            {
                writeLog(DateTime.Now.ToString() + ":Application restarting...");
                System.IO.File.AppendAllText(".\\Log\\Log.txt", DateTime.Now.ToString() + ":Application restarting..." + Environment.NewLine);
                Thread.Sleep(3000);
                Application.Restart();
            }
            time = utilities.getParafaitDefaults("BUSINESS_DAY_START_TIME").PadLeft(2,'0')+":00";
            dayRpt = new DayReport();
            writeLog(DateTime.Now.ToString() + ":Waiting for the schedule at " + time + " O'Clock.");
            System.IO.File.AppendAllText(".\\Log\\Log.txt", DateTime.Now.ToString() + ":Waiting for the schedule at " + time + " O'Clock." + Environment.NewLine);
            if (dayRpt.error)
            {
                writeLog(DateTime.Now.ToString() + ":Error in initializing... Please restart the application .");
                System.IO.File.AppendAllText(".\\Log\\Log.txt", DateTime.Now.ToString() + ":Error in initializing... Please restart the application ." + Environment.NewLine);
            }
            else
            {
                PollTimer.Interval = 1000;
                PollTimer.Start();
            }           
        }
     

        public void writeLog(string inputString)
        {           
            logText.AppendText(inputString + System.Environment.NewLine);
            logText.Refresh();
        }      

        private void buttonUpload_Click(object sender, EventArgs e)
        {
            PollTimer.Stop();
            writeLog(DateTime.Now.ToString()+":Switched to manual Sales report generation...");
            System.IO.File.AppendAllText(".\\Log\\Log.txt", DateTime.Now.ToString() + ":Switched to manual Sales report generation..." + Environment.NewLine);
            frmReportGenerator rptgenarator = new frmReportGenerator();
            rptgenarator.ShowDialog();
            if (!string.IsNullOrEmpty(rptgenarator.Messages))
            {
                string[] messages;
                messages = rptgenarator.Messages.Split('*');
                for (int i = 0; i < messages.Length; i++)
                {
                    if (!string.IsNullOrEmpty(messages[i]))
                    {
                        writeLog(messages[i]);
                    }
                }
            }
            writeLog(DateTime.Now.ToString() + ":Switched back to automated Sales report generation...");
            System.IO.File.AppendAllText(".\\Log\\Log.txt", DateTime.Now.ToString() + ":Switched back to automated Sales report generation..." + Environment.NewLine);
            PollTimer.Start();
        }

       

        private void btnShutDown_Click(object sender, EventArgs e)
        {
            writeLog(DateTime.Now.ToString() + ":Server shutting down.");
            System.IO.File.AppendAllText(".\\Log\\Log.txt", DateTime.Now.ToString() + ":Server shutting down." + Environment.NewLine);
            Thread.Sleep(5000);
            this.Close();
        }       

        private void PollTimer_Tick(object sender, EventArgs e)
        {            
            if (DateTime.Now.ToString("HH:mm").Equals(time))
            {
                PollTimer.Stop();
                writeLog(DateTime.Now.ToString() + ":File generation and FTP pocess is started.");
                System.IO.File.AppendAllText(".\\Log\\Log.txt", DateTime.Now.ToString() + ":File generation and FTP pocess is started." + Environment.NewLine);
                Thread.Sleep(3000);
                if (!dayRpt.processData())
                {
                    writeLog(DateTime.Now.ToString() + ":Error in report generation...");
                    System.IO.File.AppendAllText(".\\Log\\Log.txt", DateTime.Now.ToString() + ":Error in report generation..." + Environment.NewLine);                    
                }
                else
                {
                    writeLog(DateTime.Now.ToString() + ":File generation and FTP pocess is completed.");
                    System.IO.File.AppendAllText(".\\Log\\Log.txt", DateTime.Now.ToString() + ":File generation and FTP pocess is completed." + Environment.NewLine);
                }
                Thread.Sleep(60000);
               
                PollTimer.Start();
                writeLog(DateTime.Now.ToString() + ":Waiting for the next schedule at "+time+" O'Clock...");
                System.IO.File.AppendAllText(".\\Log\\Log.txt", DateTime.Now.ToString() + ":Waiting for the next schedule at " + time + " O'Clock..." + Environment.NewLine);
            }        
            
        }
       
    }
}
