/********************************************************************************************
* Project Name - frmPOSFingerPrintLogin
* Description  - This form will verify the FingerPrint Login and it will display only when POSFingerPrintAuthentication is Enable.
* 
**************
**Version Log
**************
*Version     Date             Modified By              Remarks          
*********************************************************************************************
*2.80       25-Feb-2020       Indrajeet Kumar          Created 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Semnox.Parafait.Device.Biometric;
using Semnox.Core.Utilities;
using Semnox.Parafait.User;
using Semnox.Parafait.Languages;

namespace Parafait_POS
{
    public partial class frmPOSFingerPrintLogin : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext = ExecutionContext.GetExecutionContext();
        private FingerPrintReader fingerPrintReader = null;
        private byte[] fingerTemplate = null; // Hold the Template                
        private ParafaitDefaultsDTO parafaitDefaultsDTO = null;
        UserFingerPrintDetailDTO userFingerPrintDetailDTO = null;        
        UsersDTO usersDTO = null;

        public frmPOSFingerPrintLogin()
        {
            log.LogMethodEntry();
            InitializeComponent();
            EventHandler fingerprintScanCompleteEvent = new EventHandler(FingerPrintScanCompleteEventHandle);

            ParafaitDefaultsListBL parafaitDefaultsListBL = new ParafaitDefaultsListBL(executionContext);
            List<KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>> searchParafaitDefaultsParam = new List<KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>>();
            searchParafaitDefaultsParam.Add(new KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>(ParafaitDefaultsDTO.SearchByParameters.DEFAULT_VALUE_NAME, "BIOMETRIC_DEVICE_TYPE"));
            List<ParafaitDefaultsDTO> parafaitDefaultsDTOList = parafaitDefaultsListBL.GetParafaitDefaultsDTOList(searchParafaitDefaultsParam);
            if (parafaitDefaultsDTOList != null && parafaitDefaultsDTOList.Count > 0)
            {
                parafaitDefaultsDTO = parafaitDefaultsDTOList[0];
                BiometricFactory.GetInstance().Initialize();
                fingerPrintReader = BiometricFactory.GetInstance().GetBiometricDeviceType(parafaitDefaultsDTO.DefaultValue);

                log.Debug("fingerPrintReader Before Intialization :" + fingerPrintReader);
                fingerPrintReader.Initialize(-1, string.Empty, DisplayText, this.pbPrint.Handle, QualityProgressBar);

                //// -------------- Event Registration ------------
                log.Debug("Start - Event Registration");
                fingerPrintReader.Register(fingerprintScanCompleteEvent);
                //fingerTemplate = fingerPrintReader.Scan();                
                log.Debug("End - Event Registration");
                //// -------------- Event Registration ------------

                log.Debug("fingerPrintReader After Intialization : " + fingerPrintReader);
                log.Debug("Picture Box Handle : " + this.pbPrint.Handle);
            }
            else
            {
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 2820), this.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            log.LogMethodExit();
        }

        private void DisplayText(int status)
        {
            log.LogMethodEntry(status);
            try
            {
                if (txtMsg.InvokeRequired)
                {
                    MessagePrint messagePrint = new MessagePrint(DisplayText);
                    Invoke(messagePrint, new object[] { status });
                }
                else
                {
                    string fpStatusMessage = string.Empty;
                    switch (status)
                    {
                        case 0: fpStatusMessage = "No finger"; break;
                        case 1: fpStatusMessage = "Move finger up"; break;
                        case 2: fpStatusMessage = "Move finger down"; break;
                        case 3: fpStatusMessage = "Move finger left"; break;
                        case 4: fpStatusMessage = "Move finger right"; break;
                        case 5: fpStatusMessage = "Press finger harder"; break;
                        case 6: fpStatusMessage = "Latent"; break;
                        case 7: fpStatusMessage = "Remove your finger"; break;
                        case 8: fpStatusMessage = "Finger accepted"; break;
                        case 9: fpStatusMessage = "Finger was detected"; break;
                        case 10: fpStatusMessage = "Finger is misplaced"; break;
                        case 11: fpStatusMessage = "Don't remove your finger"; break;
                        default: fpStatusMessage = "No finger"; break;
                    }
                    txtMsg.Text = fpStatusMessage;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                POSUtils.ParafaitMessageBox(ex.Message, "Error");
                throw ex;
            }
            log.LogMethodExit();
        }

        private void QualityProgressBar(byte quality)
        {
            log.LogMethodEntry(quality);
            try
            {
                if (pbStatusBar.InvokeRequired)
                {
                    QualityProgress qualityProgress = new QualityProgress(QualityProgressBar);
                    Invoke(qualityProgress, new object[] { quality });
                }
                else
                {
                    this.pbStatusBar.Value = quality < this.pbStatusBar.Maximum ? quality : this.pbStatusBar.Maximum;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                POSUtils.ParafaitMessageBox(ex.Message, "Error");
                throw ex;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// If PA-DSS is not set and match with each FingerPrint
        /// </summary>
        private void Identify()
        {
            log.LogMethodEntry();
            UserFingerPrintDetailBL userFingerPrintDetailBL = new UserFingerPrintDetailBL(executionContext);

            //Based on POSStatic LastXDaysLoginUserList - create UserFingerPrintDetailDTO List.
            List<UserFingerPrintDetailDTO> lastXdaysUserFPrintDetailDTOList = userFingerPrintDetailBL.CreateUsersFPDetailsList(POSStatic.GetLastXDaysUserList());
            if (lastXdaysUserFPrintDetailDTOList != null && lastXdaysUserFPrintDetailDTOList.Any())
            {
                userFingerPrintDetailDTO = fingerPrintReader.Identify(lastXdaysUserFPrintDetailDTOList, fingerTemplate);
                if (userFingerPrintDetailDTO != null)
                {
                    usersDTO = POSStatic.LastXDaysLoginUserList.FirstOrDefault(s => s.UserId == userFingerPrintDetailDTO.UserId);
                    log.Debug("Identify - LastXDaysLoginUserList UsersDTO : " + usersDTO);
                }
            }

            if (userFingerPrintDetailDTO == null && usersDTO == null)
            {                
                //Based on POSStatic LocalUserList - create UserFingerPrintDetailDTO List.
                List<UserFingerPrintDetailDTO> localUserFPDetailDTOList = userFingerPrintDetailBL.CreateUsersFPDetailsList(POSStatic.GetLocalUserList());
                if (localUserFPDetailDTOList != null && localUserFPDetailDTOList.Any())
                {
                   
                    userFingerPrintDetailDTO = fingerPrintReader.Identify(localUserFPDetailDTOList, fingerTemplate);
                    log.Debug("Identify - Local userFingerPrintDetailDTO Value : " + userFingerPrintDetailDTO);                    
                    if (userFingerPrintDetailDTO != null)
                    {
                        usersDTO = POSStatic.LocalUserList.FirstOrDefault(s => s.UserId == userFingerPrintDetailDTO.UserId);
                        log.Debug("Identify - Local UsersDTO : " + usersDTO);
                    }
                }
            }

            if (userFingerPrintDetailDTO == null && usersDTO == null)
            {           
                //Based on POSStatic GlobalUserList - create UserFingerPrintDetailDTO List.
                List<UserFingerPrintDetailDTO> globalUserFPDetailDTOList = userFingerPrintDetailBL.CreateUsersFPDetailsList(POSStatic.GetGlobalUserList());
                if (globalUserFPDetailDTOList != null && globalUserFPDetailDTOList.Any())
                {
                    userFingerPrintDetailDTO = fingerPrintReader.Identify(globalUserFPDetailDTOList, fingerTemplate);
                    log.Debug("Identify - Global userFingerPrintDetailDTO Value : " + userFingerPrintDetailDTO);
                    if (userFingerPrintDetailDTO == null)
                    {
                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 2821), "Alert");
                    }
                    else if (userFingerPrintDetailDTO != null)
                    {
                        usersDTO = POSStatic.GlobalUserList.FirstOrDefault(s => s.UserId == userFingerPrintDetailDTO.UserId);
                        log.Debug("Identify - Global UsersDTO : " + usersDTO);
                    }
                }
            }

            if (usersDTO != null)
            {
                try
                {
                    Authenticate.FingerSwiped(usersDTO);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    POSUtils.ParafaitMessageBox(ex.Message);
                }
            }
            else
            {
                POSUtils.ParafaitMessageBox(POSStatic.MessageUtils.getMessage(1951), "Alert");
                fingerPrintReader.Scan();
            }
            log.LogMethodExit();
        }        

        /// <summary>
        /// Below event is used to Switch the Screen from FingerPrint Login Screen to LoginId & Password Screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lnkLoginUsername_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry();
            if (fingerPrintReader != null)
            {
                fingerPrintReader.Dispose();
                fingerPrintReader = null;
            }
            this.Dispose();
            this.Close();
            Authenticate.User();
            log.LogMethodExit();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (fingerPrintReader != null)
            {
                fingerPrintReader.Dispose();
                fingerPrintReader = null;
            }
            this.Dispose();
            this.Close();
           // Authenticate.User();
            log.LogMethodExit();
        }                

        private void FingerPrintScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                log.Debug("Start - FingerPrintScanCompleteEventHandle");
                if (e is MorphoScannedEventArgs)
                {
                    log.Debug("Start - Evenet e" + e);
                    MorphoScannedEventArgs checkScannedEvent = e as MorphoScannedEventArgs;
                    fingerTemplate = checkScannedEvent.fingerPrintTemplate;
                    if (fingerTemplate != null)
                        Identify();
                    log.Debug("checkScannedEvent Value" + checkScannedEvent);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                POSUtils.ParafaitMessageBox(ex.Message, "Error");
                throw ex;
            }
            log.LogMethodExit();
        }

        private void frmPOSFingerPrintLogin_Shown(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                fingerTemplate = fingerPrintReader.Scan();
            }
            catch (Exception ex)
            {
                POSUtils.ParafaitMessageBox(ex.Message, "Error");
            }            
            log.LogMethodExit();
        }
    }
}
