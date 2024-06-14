/********************************************************************************************
 * Project Name - Device.Printer
 * Description  - Class for  of FutronicFS84      
 *  
 **************
 **Version Log
 **************
 *Version       Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019   Girish kundar  Modified : Added Logger Methods.
 *2.80          09-Apr-2020   Indrajet Kumar Modified : Verify Method - added parameter UserFingerPrintDetailDTO - 
 *                                           to support FingerPrintRedaer Base Method 
 *                                           Added : Initialize & Default Constructor
 ********************************************************************************************/
using System;
using System.Threading;
using System.Collections.Generic;

namespace Semnox.Parafait.Device.Biometric.FutronicFS84
{
    public class FutronicFS84 : FingerPrintReader
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private FamComm m_commFam;
        private bool m_bPIV = true;
        uint m_uiParam1 = 0;
        uint m_uiParam2 = 0;
        byte m_nSensorState = 0;

        private static int[] FTR_MATCH_SCORE_VALUE = {
            37, //FTR_ANSISDK_MATCH_SCORE_LOW
            65, //FTR_ANSISDK_MATCH_SCORE_LOW_MEDIUM
            93, //FTR_ANSISDK_MATCH_SCORE_MEDIUM
            121,//FTR_ANSISDK_MATCH_SCORE_HIGH_MEDIUM
            146,//FTR_ANSISDK_MATCH_SCORE_HIGH
            189 //FTR_ANSISDK_MATCH_SCORE_VERY_HIGH
        };

        //public enum MATCH_SCORE : int
        //{
        //    MATCH_SCORE_LOW,
        //    MATCH_SCORE_LOW_MEDIUM,
        //    MATCH_SCORE_MEDIUM,
        //    MATCH_SCORE_HIGH_MEDIUM,
        //    MATCH_SCORE_HIGH,
        //    MATCH_SCORE_VERY_HIGH
        //}

        // set MatchScoreLevel to default
        int MatchScoreValue = FTR_MATCH_SCORE_VALUE[(int)MATCH_SCORE.MATCH_SCORE_MEDIUM];

        public FutronicFS84(string IPAddress, int PortNo)
        {
            log.LogMethodEntry("IPAddress", "PortNo");
            m_commFam = new FamComm();
            string[] bytes = IPAddress.Split('.');
            m_commFam.IPAddress = Array.ConvertAll(bytes, byte.Parse);
            m_commFam.PortNumber = PortNo;
            log.LogMethodExit();
        }

        public override void Initialize(int portNo, string portAddress, MessagePrint messagePrint, IntPtr liveFingerPrint, QualityProgress qualityProgress)
        {
            log.LogMethodEntry(portNo, portAddress , messagePrint, liveFingerPrint, qualityProgress);           
            log.LogMethodExit();
        }

        public FutronicFS84()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        public override void SetMatchScoreValue(MATCH_SCORE MatchScoreLevel)
        {
            log.LogMethodEntry(MatchScoreLevel);
            MatchScoreValue = FTR_MATCH_SCORE_VALUE[(int)MatchScoreLevel];
            log.LogMethodExit(MatchScoreValue);
        }

        public override byte[] Scan()
        {
            log.LogMethodEntry();
            byte nRet;
            nRet = m_commFam.PrepareConnection();
            if (nRet != 0)
            {
                log.Error("Exception Occurred at CaptureTemplate() method ");
                throw new ApplicationException(m_commFam.ErrorMessage);
            }

            try
            {
                int ticks = Environment.TickCount + 10000; //10 secs
                while (Environment.TickCount < ticks)
                {
                    nRet = m_commFam.FamIsFingerPresent();
                    if (nRet == 0)
                        break;
                    else if (nRet != FamDefs.RET_NO_IMAGE)
                    {
                        log.Error(m_commFam.ErrorMessage);
                        throw new ApplicationException(m_commFam.ErrorMessage);
                    }
                    Thread.Sleep(50);
                }

                if (ticks < Environment.TickCount)
                {
                    log.Error("Exception Occurred at CaptureTemplate()-Timeout!  ");
                    throw new ApplicationException("Timeout!");
                }
                else
                {
                    uint nContrast = 0;
                    uint nBrightness = 0;
                    nRet = m_commFam.FamCaptureImage(m_bPIV, ref nContrast, ref nBrightness);
                    if (nRet == 0)
                    {
                        //nRet = m_commFam.FamProcessImage35();
                        nRet = m_commFam.FamProcessImageANSI();
                        if (nRet == 0)
                        {
                            uint nTemplateLength = 0;
                            //if (m_commFam.FamDownloadSample35(ref nTemplateLength) == 0)
                            if (m_commFam.FamDownloadSampleANSI(ref nTemplateLength) == 0)
                            {
                                // fs84 ANSI fix
                                m_commFam.FamDownloadedTemplate[10] = 0x00;
                                m_commFam.FamDownloadedTemplate[11] = 0x4d;
                                log.LogMethodExit(m_commFam.FamDownloadedTemplate);
                                return m_commFam.FamDownloadedTemplate;
                            }
                            else
                            {
                                log.Error("Exception Occurred at CaptureTemplate()  ");
                                throw new ApplicationException(m_commFam.ErrorMessage);
                            }
                        }
                        else
                        {
                            log.Error("Exception Occurred at CaptureTemplate()  ");
                            throw new ApplicationException(m_commFam.ErrorMessage);
                        }
                    }
                    else
                    {
                        log.Error("Exception Occurred at CaptureTemplate()  ");
                        throw new ApplicationException(m_commFam.ErrorMessage);
                    }
                }
            }
            finally
            {
                log.LogMethodExit("Finally block");
                m_commFam.CloseConnection();
            }
        }

        /// <summary>
        /// Verify the fingerprint Template and return the bool value.
        /// </summary>
        /// <param name="userFingerPrintDetailDTOList"></param>
        /// <param name="refTemplate"></param>
        /// <returns></returns>
        public override bool Verify(List<UserFingerPrintDetailDTO> userFingerPrintDetailDTOList, byte[] refTemplate)
        {
            log.LogMethodEntry(userFingerPrintDetailDTOList, refTemplate);
            float score = -1;
            bool value = false;
            foreach (UserFingerPrintDetailDTO userFingerPrintDetailDTO in userFingerPrintDetailDTOList)
            {
                userFingerPrintDetailDTO.FPTemplate[10] = refTemplate[10] = 0x00;
                userFingerPrintDetailDTO.FPTemplate[11] = refTemplate[11] = 0x4d;
                bool match = ftrNativeLib.ftrAnsiSdkMatchTemplates(userFingerPrintDetailDTO.FPTemplate, refTemplate, ref score);

                if (!match)
                {
                    log.Error("ANSI template match error");
                    throw new ApplicationException("ANSI template match error");
                }
                if (score >= MatchScoreValue)
                {
                    log.LogMethodExit(true);                    
                    value = true;
                }
                else
                {
                    log.LogMethodExit(false);
                    value = false;
                }
            }
            return value;                        

            // SDK format verification
            //FutronicSdkBase m_Operation = new FutronicIdentification();
            //bool result = false;
            //int nResult;
            //FtrIdentifyRecord rgRecords = new FtrIdentifyRecord();

            //((FutronicIdentification)m_Operation).BaseTemplate = Template;
            //rgRecords.KeyValue = new byte[] { 0x01 };
            //rgRecords.Template = refTemplate;

            //nResult = ((FutronicIdentification)m_Operation).Identification(rgRecords, ref result);
            //if (nResult == FutronicSdkBase.RETCODE_OK)
            //{
            //    return result;
            //}
            //else
            //{
            //    throw new ApplicationException(FutronicSdkBase.SdkRetCode2Message(nResult));
            //}
        }

        public override void LedState(uint LedNoMask, bool OnOff)
        {
            log.LogMethodEntry(LedNoMask, OnOff);
            byte nRet;
            nRet = m_commFam.PrepareConnection();
            if (nRet != 0)
            {
                log.Error(m_commFam.ErrorMessage);
                throw new ApplicationException(m_commFam.ErrorMessage);
            }

            try
            {
                LedNoMask = LedNoMask << 3;

                if (OnOff)
                {
                    m_uiParam1 = (m_uiParam1 & 0xffffffe7) | LedNoMask;
                    m_uiParam2 = (m_uiParam2 | 0x00000018) & ~LedNoMask;
                }
                else
                {
                    m_uiParam1 = m_uiParam1 & ~LedNoMask;
                    m_uiParam2 = m_uiParam2 | LedNoMask;
                }
                m_commFam.FamPeripherialControl(m_uiParam1, m_uiParam2, ref m_nSensorState);
            }
            finally
            {
                m_commFam.CloseConnection();
            }
            log.LogMethodExit();
        }

        public override void LedBuzzerState(uint LedNoMask, bool LedOnOff, bool BuzzerOnOff)
        {
            log.LogMethodEntry(LedNoMask, LedOnOff, BuzzerOnOff);
            byte nRet;
            nRet = m_commFam.PrepareConnection();
            if (nRet != 0)
            {
                log.Error(m_commFam.ErrorMessage);
                throw new ApplicationException(m_commFam.ErrorMessage);
            }

            try
            {
                uint buzzerMask = 0x00000001;

                if (BuzzerOnOff)
                {
                    m_uiParam1 = m_uiParam1 | buzzerMask;
                    m_uiParam2 = m_uiParam2 & ~buzzerMask;
                }
                else
                {
                    m_uiParam2 = m_uiParam2 | buzzerMask;
                    m_uiParam1 = m_uiParam1 & ~buzzerMask;
                }

                LedNoMask = LedNoMask << 3;

                if (LedOnOff)
                {
                    m_uiParam1 = (m_uiParam1 & 0xffffffe7) | LedNoMask;
                    m_uiParam2 = (m_uiParam2 | 0x00000018) & ~LedNoMask;
                }
                else
                {
                    m_uiParam1 = m_uiParam1 & ~LedNoMask;
                    m_uiParam2 = m_uiParam2 | LedNoMask;
                }
                m_commFam.FamPeripherialControl(m_uiParam1, m_uiParam2, ref m_nSensorState);
            }
            finally
            {
                m_commFam.CloseConnection();
            }
            log.LogMethodExit();
        }

        public override void LockState(bool OnOff)
        {
            log.LogMethodEntry(OnOff);
            byte nRet;
            nRet = m_commFam.PrepareConnection();
            if (nRet != 0)
            {
                log.Error(m_commFam.ErrorMessage);
                throw new ApplicationException(m_commFam.ErrorMessage);
            }

            try
            {
                uint lockMask = 0x00000004;

                if (OnOff)
                {
                    m_uiParam1 = m_uiParam1 | lockMask;
                    m_uiParam2 = m_uiParam2 & ~lockMask;
                }
                else
                {
                    m_uiParam2 = m_uiParam2 | lockMask;
                    m_uiParam1 = m_uiParam1 & ~lockMask;
                }
                m_commFam.FamPeripherialControl(m_uiParam1, m_uiParam2, ref m_nSensorState);
            }
            finally
            {
                m_commFam.CloseConnection();
            }
            log.LogMethodExit();
        }

        public override void Buzzer(int Interval = 100)
        {
            log.LogMethodEntry(Interval);
            byte nRet;
            nRet = m_commFam.PrepareConnection();
            if (nRet != 0)
            {
                log.Error(m_commFam.ErrorMessage);
                throw new ApplicationException(m_commFam.ErrorMessage);
            }

            try
            {
                uint buzzerMask = 0x00000001;

                m_uiParam1 = m_uiParam1 | buzzerMask;
                m_uiParam2 = m_uiParam2 & ~buzzerMask;
                m_commFam.FamPeripherialControl(m_uiParam1, m_uiParam2, ref m_nSensorState);

                System.Threading.Thread.Sleep(Interval);

                m_uiParam2 = m_uiParam2 | buzzerMask;
                m_uiParam1 = m_uiParam1 & ~buzzerMask;

                m_commFam.FamPeripherialControl(m_uiParam1, m_uiParam2, ref m_nSensorState);
            }
            finally
            {
                m_commFam.CloseConnection();
            }
            log.LogMethodExit();
        }
    }
}
