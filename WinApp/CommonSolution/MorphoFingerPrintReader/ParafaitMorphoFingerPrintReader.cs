/********************************************************************************************
* Project Name - ParafaitMorphoFingerPrintReader
* Description  - 
* 
**************
**Version Log
**************
*Version     Date             Modified By              Remarks          
*********************************************************************************************
*2.80       26-Feb-2020       Indrajeet Kumar          Created 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using Sagem.MorphoKit;
using Sagem.MorphoKit.Imaging;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using Semnox.Parafait.User;
using System.Threading;

namespace Semnox.Parafait.Device.Biometric.Morpho
{
    public enum AcquisitionDeviceEnum
    {
        SingleFingerDevice,
        MultiFingerDevice
    }
    public class ParafaitMorphoFingerPrintReader : FingerPrintReader
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const int matchingThreshold = 3500;
        protected SynchronizationContext synContext;
        protected List<EventHandler> callBackList = new List<EventHandler>();
        protected event EventHandler morphoInputHandler;
        public EventHandler MorphoInputHandler
        {
            get { return morphoInputHandler; }
        }

        public ParafaitMorphoFingerPrintReader()
        {
            log.LogMethodEntry();
            acquisitionDeviceType = AcquisitionDeviceEnum.SingleFingerDevice;
            acquisitionDevice = new AcquisitionDevice();
            acquisitionParameters = new MorphoMSOAcquisitionParameters();
            SetFingerParameters();
            log.LogMethodExit();
        }

        /// <summary>
        /// Intializes the the ParafaitMorphoFingerPrintReader
        /// </summary>        
        public override void Initialize(int portno, string portaddress, MessagePrint messagePrint, IntPtr liveFingerPrint, QualityProgress qualityProgress)
        {
            log.LogMethodEntry();
            SetFingerEvent(messagePrint.Invoke);
            SetImageHandle(liveFingerPrint);
            SetQualityEvent(qualityProgress.Invoke);
            SetFingerParameters();
            log.LogMethodExit();
        }        

        private void DisplayStatusMessage(int status, string caption)
        {
            log.LogMethodEntry(status, caption);                            
            switch (status)
            {
                case -1:
                    throw new Exception("Unknown acquisition status");                                                
                case -19:
                    throw new Exception("The specified time was reached before the acquisition completes");
                case -26:
                    throw new Exception("Acquisition had been cancelled by user");                        
                case -46:
                    throw new Exception("The acquisition device had detected a false finger(only for acquisition devices with FFD feature");                       
                case -47:
                    throw new Exception("The acquired finger can be too moist or the acquisition device is wet");                        
                default:
                    throw new Exception("The acquisition failed due to an internal error");                        
            }                                             
        }

        #region IAcquisitionStrategy Members       

        /// <summary>
        /// Scan Method will capture the FingerPrint Template from Device
        /// </summary>
        public override byte[] Scan()
        {
            log.LogMethodEntry();
            try
            {
                log.Debug("Start-acquisitionDevice : " + acquisitionDevice);
                acquisitionDevice.AcquisitionMode = AcquisitionMode.ENROLL;
                acquisitionDevice.TimeOut = acquisitionParameters.Timeout;
                acquisitionDevice.FakeFingerMode = acquisitionParameters.FakeFingerMode;
                acquisitionDevice.ForceFingerOnTop = (acquisitionParameters as MorphoMSOAcquisitionParameters).ForceFingerOnTop;
                log.Debug("Current Device Serial : " + currentDevice);
                IAcquisitionResult acqresult = acquisitionDevice.Acquire(currentDevice);
                if (acqresult.Status == 0)
                {
                    ICoder coder = new Coder();
                    ICoderResult enrollResult = coder.Enroll(
                        acqresult.ImageBuffer,
                        acqresult.Width, acqresult.Height, 2);

                    log.Debug("Start - FireMorphoReadCompleteEvent");
                    log.Debug("Template : " + enrollResult.Template);
                    FireFingerPrintScanCompleteEvent(enrollResult.Template);
                    log.Debug("End - FireMorphoReadCompleteEvent");
                    fpTemplate = enrollResult.Template;
                }
                else
                {                    
                    DisplayStatusMessage(acqresult.Status, "Scan");
                }
                log.Debug("FingerPrint Template : " + fpTemplate);
                log.LogMethodExit(fpTemplate);
                return fpTemplate;
            }
            catch (Exception exc)
            {
                log.Error(exc.Message);
                throw exc;
            }
        }

        /// <summary>
        /// Verify Method, which take the imput as 2 parameter - UserFingerPrintDetailDTO List and refTemplate.
        /// </summary>
        /// <param name = "Template" ></ param >
        /// < param name= "refTemplate" ></ param >
        /// < returns > bool </ returns >
        public override bool Verify(List<UserFingerPrintDetailDTO> userFingerPrintDetailDTOList, byte[] refTemplate)
        {
            log.LogMethodEntry(userFingerPrintDetailDTOList, refTemplate);
            try
            {
                log.Debug("userFingerPrintDetailDTOList Count in Morpho Class : " + userFingerPrintDetailDTOList.Count);
                log.Debug("userFingerPrintDetailDTOList Template in Morpho Class : " + refTemplate);
                int score = -1;
                bool value = false;
                foreach (UserFingerPrintDetailDTO userFingerPrintDetailDTO in userFingerPrintDetailDTOList)
                {
                    IAuthenticator authenticator = new Authenticator();
                    score = authenticator.Authenticate(userFingerPrintDetailDTO.FPTemplate, refTemplate);

                    log.Debug("Before Matching Score value : " + score);
                    if (score >= matchingThreshold)
                    {
                        log.Debug("After Matching Score value : " + score);
                        value = true;
                        break;
                    }
                    else
                    {
                        value = false;
                        continue;
                    }
                }
                log.LogMethodExit(value);
                log.Debug("Value : " + value);
                return value;
            }
            catch (Exception exc)
            {
                log.Error(exc.Message);
                throw exc;
            }
        }

        /// <summary>
        /// Identify Method, which take the imput as 2 parameter - UserFingerPrintDetailDTO List and refTemplate.
        /// </summary>
        /// <param name = "userFingerPrintDetailDTOList" ></ param >
        /// < param name= "scanedTemplate" ></ param >
        /// < returns ></ returns >
        public override UserFingerPrintDetailDTO Identify(List<UserFingerPrintDetailDTO> userFingerPrintDetailDTOList, byte[] scanedTemplate)
        {
            log.LogMethodEntry(userFingerPrintDetailDTOList, scanedTemplate);
            try
            {
                UserFingerPrintDetailDTO userFingerPrintDetail = null;
                log.Debug("userFingerPrintDetailDTOList Count in Morpho Class : " + userFingerPrintDetailDTOList.Count);
                log.Debug("userFingerPrintDetailDTOList Template in Morpho Class : " + scanedTemplate);
                int score = -1;
                foreach (UserFingerPrintDetailDTO userFingerPrintDetailDTO in userFingerPrintDetailDTOList)
                {
                    IAuthenticator authenticator = new Authenticator();
                    log.Debug("authenticator Value" + authenticator);
                    score = authenticator.Authenticate(userFingerPrintDetailDTO.FPTemplate, scanedTemplate);
                    log.Debug("Before Matching Score value Condition : " + score);
                    if (score >= matchingThreshold)
                    {
                        log.Debug("After Matching Score value : " + score);
                        userFingerPrintDetail = userFingerPrintDetailDTO;
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
                log.LogMethodExit(userFingerPrintDetail);
                log.Debug("userFingerPrintDetailDTO Value : " + userFingerPrintDetail);
                return userFingerPrintDetail;
            }
            catch (Exception exc)
            {
                log.Error(exc.Message);
                throw exc;
            }
        }

        public override void Dispose()
        {
            log.LogMethodEntry();
            acquisitionDevice.CancelAcquisition();
            log.LogMethodExit();
        }

        public void SwitchOnBacklight()
        {
            MessageBox.Show("Not availabe yet.", "SwitchOnBackLight", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void SwitchOffBacklight()
        {
            MessageBox.Show("Not availabe yet.", "SwitchOffBackLight", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void SetacquisitionParameters(IAcquisitionParameters parameters)
        {
            log.LogMethodEntry(parameters);
            if (AcquisitionDeviceEnum.SingleFingerDevice != parameters.AcquisitionDeviceType)
            {
                acquisitionParameters = new MorphoMSOAcquisitionParameters();
                acquisitionParameters = parameters;
            }
            else
            {
                acquisitionParameters = parameters;
            }
            log.LogMethodExit();
        }

        public string GetDeviceInformation(String value)
        {
            log.LogMethodEntry(value);
            IAcquisitionDeviceDescriptor descriptor = acquisitionDevice.GetDescriptor(value);
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}\n{1}\n{2}\n", descriptor.ProductDescriptor, descriptor.SensorDescriptor,
                descriptor.SoftwareDescriptor);
            log.LogMethodExit(sb.ToString());
            return sb.ToString();
        }

        public IList<String> DevicesList
        {
            get
            {
                IList<string> list = new List<string>();
                log.Debug("Empty Device List : " + list);
                int numberOfDevices = acquisitionDevice.GetNumberOfDevices();
                log.Debug("Total Device List : " + numberOfDevices);
                if (numberOfDevices == 0)
                {
                    return list;
                }
                IAcquisitionDeviceInfo[] deviceInfos = acquisitionDevice.EnumerateDevices();
                log.Debug("Device Info : " + deviceInfos);
                foreach (IAcquisitionDeviceInfo info in deviceInfos)
                { list.Add(info.SerialNumber); }
                log.Debug("Device List : " + list);
                return list;
            }
        }
        public string GetDevice()
        {
            log.LogMethodEntry();
            try
            {
                IList<string> device_list = DevicesList;
                log.LogMethodEntry("List of FingerPrint Device : " + device_list);
                if (device_list.Count == 0)
                {
                    return null;
                }

                return device_list.FirstOrDefault();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// Set the FingerPrint Device MorphoMSOAcquisitionParameters.
        /// </summary>
        private void SetFingerParameters()
        {
            log.LogMethodEntry();
            MorphoMSOAcquisitionParameters morphoMSOAcquisitionParameters = new MorphoMSOAcquisitionParameters();
            log.Debug("Start to get the Device");
            DeviceToUse = GetDevice();
            log.Debug("Device To Use : " + DeviceToUse);
            morphoMSOAcquisitionParameters.Timeout = 60;
            morphoMSOAcquisitionParameters.SaveImages = false;
            morphoMSOAcquisitionParameters.ForceFingerOnTop = false;
            morphoMSOAcquisitionParameters.Consolidation = false;
            FFDMode ffdmode = FFDMode.DEFAULT;
            try
            {
                if (Enum.IsDefined(typeof(FFDMode), 1))
                {
                    ffdmode = FFDMode.DEFAULT;
                }
            }
            catch (Exception) { }
            morphoMSOAcquisitionParameters.FakeFingerMode = ffdmode;
            morphoMSOAcquisitionParameters.SaveTemplates = true;
            SetacquisitionParameters(morphoMSOAcquisitionParameters);
            log.LogMethodExit(morphoMSOAcquisitionParameters);
        }
        
        public string DeviceToUse
        {
            get { return currentDevice; }
            set
            {
                if (String.IsNullOrEmpty(value))
                    throw new ArgumentNullException("Device serial number should not be null or empty.");
                currentDevice = value;
            }
        }

        public AcquisitionDeviceEnum AcquisitionDeviceType
        {
            get { return AcquisitionDeviceEnum.SingleFingerDevice; }
        }

        #endregion

        #region Implementation

        public void SetFingerEvent(FingerEventHandler ev)
        {
            acquisitionDevice.FingerEvent += ev;
        }

        public void SetEnrolmentEvent(EnrolmentEventHandler ev)
        {
            acquisitionDevice.EnrolmentEvent += ev;
        }

        public void SetQualityEvent(QualityEventHandler ev)
        {
            acquisitionDevice.QualityEvent += ev;
        }

        public void SetImageEvent(ImageEventHandler ev)
        {
            acquisitionDevice.ImageEvent += ev;
        }

        public void SetImageHandle(IntPtr handle)
        {
            acquisitionDevice.Display = handle;
        }

        #endregion

        private void SaveImage(byte[] data)
        {
            if (acquisitionParameters.SaveImages)
            {
                SaveFileDialog fileDlg = new SaveFileDialog();
                fileDlg.AddExtension = true;

                fileDlg.FileName = "fingerprint";
                fileDlg.Title = "Save picture";
                fileDlg.DefaultExt = "raw";
                fileDlg.Filter = "RAW Image (*.raw)|*.raw|All Files (*.*)|*.*";
                if (fileDlg.ShowDialog() == DialogResult.OK)
                {
                    FileStream fs = new FileStream(fileDlg.FileName, FileMode.Create);
                    fs.Write(data, 0, data.Length);
                    fs.Close();
                }
            }
        }

        /// <summary>
        /// SaveTemplate Method is used to save fingerprint Template to the system.
        /// </summary>
        /// <param name = "data" ></ param >
        private void SaveTemplate(byte[] data)
        {
            log.LogMethodEntry(data);
            if (acquisitionParameters.SaveTemplates)
            {
                SaveFileDialog fileDlg = new SaveFileDialog();
                fileDlg.AddExtension = true;

                fileDlg.FileName = "fingerprint";
                fileDlg.Title = "Save fingerprint template";
                fileDlg.DefaultExt = "cfv";
                fileDlg.Filter = "Morpho CFV Fingerprint Template (*.cfv)|*.cfv|All Files (*.*)|*.*";
                if (fileDlg.ShowDialog() == DialogResult.OK)
                {
                    FileStream fs = new FileStream(fileDlg.FileName, FileMode.Create);
                    fs.Write(data, 0, data.Length);
                    fs.Close();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Below Method set the FingerPrint Event which give the status of fingerprint.
        /// </summary>
        /// <param name = "status" ></ param >
        public void finger_device_FingerEvent(int status)
        {
            log.LogMethodEntry();
            FingerEventStatus l_e_FingerEventStatus = FingerEventStatus.UNKNOWN;
            if (Enum.IsDefined(typeof(FingerEventStatus), status))
                l_e_FingerEventStatus = (FingerEventStatus)status;
            string msg = "";
            switch (l_e_FingerEventStatus)
            {
                case FingerEventStatus.NO_FINGER_DETECTED: msg = "No finger"; break;
                case FingerEventStatus.MOVE_FINGER_UP: msg = "Move finger up"; break;
                case FingerEventStatus.MOVE_FINGER_DOWN: msg = "Move finger down"; break;
                case FingerEventStatus.MOVE_FINGER_LEFT: msg = "Move finger left"; break;
                case FingerEventStatus.MOVE_FINGER_RIGHT: msg = "Move finger right"; break;
                case FingerEventStatus.PRESS_FINGER_HARDER: msg = "Press finger harder"; break;
                case FingerEventStatus.LATENT: msg = "Latent"; break;
                case FingerEventStatus.REMOVE_FINGER: msg = "Remove your finger"; break;
                case FingerEventStatus.OK: msg = "Finger accepted"; break;
                case FingerEventStatus.FINGER_DETECTED: msg = "Finger was detected"; break;
                case FingerEventStatus.FINGER_MISPLACED: msg = "Finger is misplaced"; break;
                case FingerEventStatus.LIVE_OK: msg = "Don't remove your finger"; break;
                default: msg = "No finger"; break;
            }
            log.LogMethodExit(msg);
            //return msg;            
        }

        private IAcquisitionDevice acquisitionDevice;
        private string currentDevice;
        private AcquisitionDeviceEnum acquisitionDeviceType;
        private IAcquisitionParameters acquisitionParameters;
        private byte[] fpTemplate;
        //private UserFingerPrintDetailDTO userFingerPrintDetailDTO;
    }

    public interface IAcquisitionParameters
    {
        Int32 Timeout { get; set; }
        FFDMode FakeFingerMode { get; set; }
        Boolean SaveImages { get; set; }
        Boolean SaveTemplates { get; set; }
        AcquisitionDeviceEnum AcquisitionDeviceType { get; }
    }

    public class MorphoMSOAcquisitionParameters : IAcquisitionParameters
    {
        #region IAcquisitionParameters Membres

        public int Timeout
        {
            get { return _timeout; }
            set { _timeout = value; }
        }

        public FFDMode FakeFingerMode
        {
            get { return _ffdmode; }
            set { _ffdmode = value; }
        }

        public Boolean SaveImages
        {
            get
            { return _saveImages; }
            set
            { _saveImages = value; }
        }

        public Boolean SaveTemplates
        {
            get
            { return _saveTemplates; }
            set
            { _saveTemplates = value; }
        }

        public AcquisitionDeviceEnum AcquisitionDeviceType
        {
            get { return _acquisitionDeviceType; }
        }

        #endregion

        public MorphoMSOAcquisitionParameters()
        {
            _acquisitionDeviceType = AcquisitionDeviceEnum.SingleFingerDevice;
            _timeout = 60;
            _forceFingerOnTop = false;
            _consolidation = false;
            _saveImages = false;
            _saveTemplates = false;
            _imageSource = FingerImageSource.ImageFromDevice;
            _compute_nfiq2 = false;
        }
        public Boolean ForceFingerOnTop
        {
            get { return _forceFingerOnTop; }
            set { _forceFingerOnTop = value; }
        }

        public Boolean Consolidation
        {
            get { return _consolidation; }
            set { _consolidation = value; }
        }

        public enum FingerImageSource { ImageFromDevice, ImageFromDisk }
        public FingerImageSource ImageSource
        {
            get { return _imageSource; }
            set { _imageSource = value; }
        }
        public Boolean ComputeNfiq2
        {
            get { return _compute_nfiq2; }
            set { _compute_nfiq2 = value; }
        }

        private bool _forceFingerOnTop;
        private bool _consolidation;
        private bool _saveTemplates;
        private bool _saveImages;
        private int _timeout;
        private FFDMode _ffdmode;
        private AcquisitionDeviceEnum _acquisitionDeviceType;
        private FingerImageSource _imageSource;
        private bool _compute_nfiq2;
    }
}
