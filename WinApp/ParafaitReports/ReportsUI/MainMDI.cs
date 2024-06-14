/********************************************************************************************
* Project Name - Parafait Report
* Description  - MainMDI 
* 
**************
**Version Log
**************
*Version     Date             Modified By          Remarks          
*********************************************************************************************
 * 2.70        1-Jul-2019      Lakshminarayana     Modified to add support for ULC cards 
 * 2.80       23-Aug-2019      Jinto Thomas        Added logger into methods
 * 2.80       18-Sep-2019      Dakshakh raj        Modified : Added logs                           
 * 2.110      02-Feb-2021      Laster Menezes      commented registerDevices() method
 * 2.155.1    09-Nov-2023      Rakshith Shetty     Uncommented registerDevices() method
********************************************************************************************/
using System;
using System.Threading;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
//using ReportsLibrary;
using Semnox.Parafait.Device;
using Semnox.Parafait.Reports;

namespace Semnox.Parafait.Report.Reports
{
    /// <summary>
    /// MainMDI method
    /// </summary>
    public partial class MainMDI : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// machineUserContext initialization
        /// </summary>
        /// 
        public static Semnox.Core.Utilities.ExecutionContext machineUserContext = Semnox.Core.Utilities.ExecutionContext.GetExecutionContext();
        /// <summary>
        /// MainMDI method
        /// </summary>
        public MainMDI(string mode, string LoginId, string Password, string siteId)
        {
            log.LogMethodEntry(mode, LoginId, siteId);

            InitializeComponent();

            showSplash();
            Common.initEnv();
            Common.ParafaitEnv.IsClientServer = true;
            Common.SetSiteCulture();


            if (mode == "U") // online mode run from command line. requires authentication
            {
                Common.ParafaitEnv.Initialize();

                registerDevices();

                if (!Authenticate.User())
                    Environment.Exit(0);
            }
            else
            {
                string decryptPwd = Encryption.Decrypt(Password);
                Security sec = new Security(Common.Utilities);

                try
                {
                    Security.User user = sec.Login(LoginId, decryptPwd);

                    Common.ParafaitEnv.Username = user.UserName;
                    Common.ParafaitEnv.LoginID = user.LoginId;
                    Common.ParafaitEnv.Role = user.RoleName;
                    Common.ParafaitEnv.UserCardNumber = user.CardNumber;
                    Common.ParafaitEnv.Manager_Flag = user.ManagerFlag ? "Y" : "N";
                    Common.ParafaitEnv.User_Id = user.UserId;
                    Common.ParafaitEnv.Password = Password;
                    Common.ParafaitEnv.RoleId = user.RoleId;
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    MessageBox.Show(ex.Message, Common.Utilities.MessageUtils.getMessage("Get user details"));
                    Environment.Exit(0);
                }

                if (siteId == "-1")
                {
                    Common.ParafaitEnv.SiteId = -1;
                    Common.ParafaitEnv.IsCorporate = false;
                }
                else
                {
                    Common.ParafaitEnv.SiteId = Convert.ToInt32(siteId);
                    Common.ParafaitEnv.IsCorporate = true;
                }

                Common.ParafaitEnv.Initialize();
            }

            Common.Utilities.setLanguage();
            Common.Utilities.setLanguage(this);
            Common.LoadMessages();
            if (Common.GetSiteId() != -1)
                Common.Utilities.ParafaitEnv.IsCorporate = true;

            if (Common.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(Common.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            machineUserContext.SetUserId(Common.ParafaitEnv.LoginID);
            log.LogMethodExit();
        }

        //Begin: Modification for fixing Card Reader Login Problem on 28-Sep-2016
        private void CardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (e is DeviceScannedEventArgs)
            {
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;
                TagNumberParser tagNumberParser = new TagNumberParser(Common.Utilities.ExecutionContext);
                if (tagNumberParser.IsValid(checkScannedEvent.Message) == false)
                {
                    log.LogMethodExit("tagNumberParser.IsValid == false");
                    return;
                }
                TagNumber tagNumber = tagNumberParser.Parse(checkScannedEvent.Message);
                string CardNumber = tagNumber.Value;
            }
            log.LogMethodExit();
        }

        private bool registerDevices()
        {
            log.LogMethodEntry();
            int deviceAddress = 0;
            int ACR1252UIndex = 0;
            bool response = false;

            EventHandler CardScanCompleteEvent = new EventHandler(CardScanCompleteEventHandle);

            DeviceClass mifareReader = null;

            response = true;
            try
            {
                mifareReader = new ACR122U(deviceAddress);
            }
            catch
            {
                try
                {
                    string serialNumber = Common.Utilities.getParafaitDefaults("CARD_READER_SERIAL_NUMBER").Trim();
                    if (string.IsNullOrEmpty(serialNumber))
                    {
                        mifareReader = new ACR1252U(ACR1252UIndex);
                    }
                    else
                    {
                        mifareReader = new ACR1252U(serialNumber.Split('|')[0]);
                    }
                    ACR1252UIndex++;
                }
                catch
                {
                    try
                    {
                        mifareReader = new MIBlack(deviceAddress);
                    }
                    catch
                    {
                        response = false;
                    }
                }
            }

            if (response)
            {
                mifareReader.Register(CardScanCompleteEvent);
                ReportsCommon.ReaderDevice = mifareReader;
            }
            else
                response = registerUSBDevice();

            Common.Utilities.getMifareCustomerKey();

            log.LogMethodExit(response);
            return response;
        }

        private bool registerUSBDevice()
        {
            log.LogMethodEntry();
            string USBReaderVID = Common.Utilities.getParafaitDefaults("USB_READER_VID");
            string USBReaderPID = Common.Utilities.getParafaitDefaults("USB_READER_PID");
            string USBReaderOptionalString = Common.Utilities.getParafaitDefaults("USB_READER_OPT_STRING");

            EventHandler currEventHandler = new EventHandler(CardScanCompleteEventHandle);
            USBDevice simpleCardListener;

            if (IntPtr.Size == 4) //32 bit
                simpleCardListener = new KeyboardWedge32();
            else
                simpleCardListener = new KeyboardWedge64();

            log.LogVariableState("simpleCardListener", simpleCardListener);
            bool flag = simpleCardListener.InitializeUSBReader(this, USBReaderVID, USBReaderPID, USBReaderOptionalString);
            if (simpleCardListener.isOpen)
            {
                simpleCardListener.Register(currEventHandler);
                ReportsCommon.ReaderDevice = simpleCardListener;
                log.LogMethodExit(true);
                return true;
            }
            else
            {
                log.LogMethodExit(false);
                return false;
            }
        }

        private void showSplash()
        {
            log.LogMethodEntry();
            try
            {
                ThreadStart starter = delegate
                {
                    SplashScreen s = new SplashScreen();
                    s.Show();
                    Thread.Sleep(2000);
                    s.CloseSplash();
                };

                new Thread(starter).Start();
                Thread.Sleep(1000);
            }
            catch { }
            log.LogMethodExit();
        }

        private void MainMDI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (Common.Utilities.getParafaitDefaults("FULL_SCREEN_POS").Equals("Y"))
            {
                this.WindowState = FormWindowState.Normal;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                this.Bounds = Screen.PrimaryScreen.Bounds;
                this.WindowState = FormWindowState.Maximized;
            }

            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 10;
            timer.Tick += timer_Tick;
            timer.Start();
            this.Activate();
            log.LogMethodExit();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            (sender as System.Windows.Forms.Timer).Stop();
            ReportsCommon.openForm(this, "RunReports", null, true, false);
            log.LogMethodExit();
        }
    }
}
