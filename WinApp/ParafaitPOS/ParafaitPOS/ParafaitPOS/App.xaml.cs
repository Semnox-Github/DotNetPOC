/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Startup file
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.100.0     25-Sep-2020   Amitha Joy              Created for POS UI Redesign 
 ********************************************************************************************/
using Semnox.Parafait.CommonUI;
using Semnox.Core.Utilities;
using System; 
using System.Configuration; 
using System.IO; 
using System.Windows;
using Semnox.Parafait.Device;
using Semnox.Parafait.Communication;
using System.Windows.Interop;
using Semnox.Parafait.ViewContainer;
using System.Collections.Generic;
using System.Linq;

namespace ParafaitPOS
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static List<string> resourceFilePaths;
        public static ExecutionContext machineUserContext;
        public static bool inDesignMode = true;
        public static int SerialPortNumber;
        public static COMPort PoleDisplayPort;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string dictionaryXamlFileName = @"Styles\ControlsDictionary.xaml";
        private const string styleXamlFileName = @"Styles\Styles.xaml";
        // used when pages are called standalone
        public static void EnsureApplicationResources(List<string> resourcePaths = null)
        {
            if (Application.Current == null)
            {
                // create the Application object
                new Application
                {
                    ShutdownMode = ShutdownMode.OnExplicitShutdown
                };
            }
            inDesignMode = false;
            resourceFilePaths = resourcePaths;
            TranslateHelper.isDesignMode = App.inDesignMode;
            TranslateHelper.executioncontext = App.machineUserContext;
            PoleDisplay.executionContext = App.machineUserContext;
            PoleDisplay.SerialPortNumber = App.SerialPortNumber;
            PoleDisplay.PoleDisplayPort = App.PoleDisplayPort;
            LoadResources();
            Application.Current.Resources["RightPaneWidth"] = new GridLength(SystemParameters.PrimaryScreenWidth / 10);
            if (SystemParameters.PrimaryScreenWidth < 1000)
            {
                Application.Current.Resources["KeyBoard.Button.MinHeight"] = (double)40;
                Application.Current.Resources["CustomScroll.Large"] = new GridLength(36);
                Application.Current.Resources["CustomScroll.Large.Width"] = (double)36;
                Application.Current.Resources["CustomScroll.Medium"] = new GridLength(36);
                Application.Current.Resources["CustomScroll.Medium.Width"] = (double)36;
                Application.Current.Resources["FontSize.Large"] = (double)26;
                Application.Current.Resources["FontSize.Medium"] = (double)19;
                Application.Current.Resources["FontSize.Small"] = (double)15;
                Application.Current.Resources["Scrollbar.Margin.Arrow"] = new Thickness(8, 4, 8, 4);
            }
            SetFullScreen(machineUserContext);

        }
        private static void LoadResources()
        {
            log.LogMethodEntry();
            string appPath = System.AppDomain.CurrentDomain.BaseDirectory;
            log.Info(appPath);
            if (resourceFilePaths != null && resourceFilePaths.Any())
            {
                for (int i = 0; i < resourceFilePaths.Count; i++)
                {
                    AddResourcesToDictionary(resourceFilePaths[i]);
                }
            }
            else
            {
                string styleFilePath = appPath + styleXamlFileName;
                AddResourcesToDictionary(styleFilePath);
            }
            string controlDicPath = appPath + dictionaryXamlFileName;
            AddResourcesToDictionary(controlDicPath);
            log.LogMethodExit();
        }
        private static void AddResourcesToDictionary(string resourcePath)
        {
            log.LogMethodEntry(resourcePath);
            if(!string.IsNullOrEmpty(resourcePath))
            {
                StreamReader stream = new StreamReader(resourcePath);
                Application.Current.Resources.MergedDictionaries.Add(System.Windows.Markup.XamlReader.Load(stream.BaseStream) as ResourceDictionary);
            }
            log.LogMethodExit();
        }
        protected override void OnStartup(StartupEventArgs e)
        {

            if (machineUserContext == null)
            {
                System.Windows.Application.Current.Shutdown();
            }

            inDesignMode = false;
            App.SerialPortNumber = 2;

            Application.Current.Resources["RightPaneWidth"] = new GridLength(SystemParameters.PrimaryScreenWidth / 10);
            if (SystemParameters.PrimaryScreenWidth < 1000)
            {
                Application.Current.Resources["KeyBoard.Button.MinHeight"] = (double)40;
                Application.Current.Resources["CustomScroll.Large"] = new GridLength(36);
                Application.Current.Resources["CustomScroll.Large.Width"] = (double)36;
                Application.Current.Resources["CustomScroll.Medium"] = new GridLength(36);
                Application.Current.Resources["CustomScroll.Medium.Width"] = (double)36;
                Application.Current.Resources["FontSize.Large"] = (double)26;
                Application.Current.Resources["FontSize.Medium"] = (double)19;
                Application.Current.Resources["FontSize.Small"] = (double)15;
                Application.Current.Resources["Scrollbar.Margin.Arrow"] = new Thickness(8, 4, 8, 4);
            }

            // to be commented when the application runs in stand alone mode
            Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            //to be commented when the application runs in stand alone mode
            SetExecutionContext();
            TranslateHelper.isDesignMode = App.inDesignMode;
            TranslateHelper.executioncontext = App.machineUserContext;
            PoleDisplay.executionContext = App.machineUserContext;
            PoleDisplay.SerialPortNumber = App.SerialPortNumber;
            LoadResources();
            SetFullScreen(machineUserContext);

            base.OnStartup(e);
        }

        internal static void SetFullScreen(ExecutionContext machineusercontext)
        {
            bool full_screen = ParafaitDefaultViewContainerList.GetParafaitDefault(machineusercontext, "FULL_SCREEN_POS")=="Y" ? true:false;
            string appPath = System.AppDomain.CurrentDomain.BaseDirectory;
            string controlDicPath = appPath + dictionaryXamlFileName;
            StreamReader Controlsstream = new StreamReader(controlDicPath);
            var rd = System.Windows.Markup.XamlReader.Load(Controlsstream.BaseStream) as ResourceDictionary;
            // add the new key with value
            if (rd.Contains("Resizemode"))
            {
                rd["Resizemode"] = full_screen ? ResizeMode.NoResize : ResizeMode.CanResize;
            }
            else
            {
                rd.Add("Resizemode", full_screen ? ResizeMode.NoResize : ResizeMode.CanResize);
            }
            if (rd.Contains("WindowStyle"))
            {
                rd["WindowStyle"] = full_screen ? WindowStyle.None : WindowStyle.SingleBorderWindow;
            }
            else
            {
                rd.Add("WindowStyle", full_screen ? WindowStyle.None : WindowStyle.SingleBorderWindow);
            }
            if (!full_screen)
            {
                Application.Current.Resources["Redemption.MultiScreen.ActionButton.Width"] = (double)36;
            }
            Application.Current.Resources.MergedDictionaries.Add(rd);
        }
        public void SetExecutionContext()
        {
            Utilities utls = new Utilities();
            string ipAddress = "";
            try
            {
                ipAddress = System.Net.Dns.GetHostEntry(Environment.MachineName).AddressList[0].ToString();
            }
            catch { }

            utls.ParafaitEnv.SetPOSMachine(ipAddress, Environment.MachineName);
            utls.ParafaitEnv.Initialize();
            machineUserContext = utls.ExecutionContext;
            int site_id = Convert.ToInt32(ConfigurationManager.AppSettings["SITE_ID"]);
            machineUserContext.SetSiteId(site_id);
            machineUserContext.SetLanguageId(-1);
            utls.ParafaitEnv.SiteId = site_id;
            machineUserContext.POSMachineName = Environment.MachineName;
            if (utls.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utls.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            machineUserContext.SetIsCorporate(utls.ParafaitEnv.IsCorporate);
            machineUserContext.SetUserId("semnox");
            machineUserContext.SetUserPKId(3);
            //utls.ParafaitEnv.Initialize();
        }
        private void registerDevices(Window window,ExecutionContext executionContext, EventHandler CardScanCompleteEvent, EventHandler BarcodeScanCompleteEvent)
        {
            IntPtr handle = (new WindowInteropHelper(window)).Handle;
            POSDeviceCollection posDeviceCollection = new POSDeviceCollection(executionContext, handle);
            foreach (var cardReader in posDeviceCollection.GetDevices(DeviceType.CardReader))
            {
                cardReader.Register(CardScanCompleteEvent);
            }
            foreach (var barcodeReader in posDeviceCollection.GetDevices(DeviceType.BarcodeReader))
            {
                barcodeReader.Register(BarcodeScanCompleteEvent);
            }
        }
    }
}
