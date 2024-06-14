/********************************************************************************************
 * Project Name - Device.Futronic
 * Description  - Class for  of ftrNativeLib      
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019   Girish kundar  Modified : Added Logger Methods.
 ********************************************************************************************/
using System;
using System.Runtime.InteropServices;

namespace ftrAnsiSdkDemo
{
    public struct FTRSCAN_IMAGE_SIZE
    {
        public int nWidth;
        public int nHeight;
        public int nImageSize;
    };

    class ftrNativeLib
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public const int FTR_ERROR_SUCCESS = 0;
        public const int FTR_ERROR_EMPTY_FRAME = 4306;
        public const int FTR_ERROR_MOVABLE_FINGER = 0x20000001;
        public const int FTR_ERROR_NO_FRAME = 0x20000002;
        public const int FTR_ERROR_USER_CANCELED = 0x20000003;
        public const int FTR_ERROR_HARDWARE_INCOMPATIBLE = 0x20000004;
        public const int FTR_ERROR_FIRMWARE_INCOMPATIBLE = 0x20000005;
        public const int FTR_ERROR_INVALID_AUTHORIZATION_CODE = 0x20000006;
        public const int FTR_ERROR_ROLL_NOT_STARTED = 0x20000007;
        public const int FTR_ERROR_ROLL_PROGRESS_DATA = 0x20000008;
        public const int FTR_ERROR_ROLL_TIMEOUT = 0x20000009;
        public const int FTR_ERROR_ROLL_ABORTED = 0x2000000A;
        public const int FTR_ERROR_ROLL_ALREADY_STARTED = 0x2000000B;
        public const int FTR_ERROR_NO_MORE_ITEMS = 259;
        public const int FTR_ERROR_NOT_ENOUGH_MEMORY = 8;
        public const int FTR_ERROR_NO_SYSTEM_RESOURCES = 1450;
        public const int FTR_ERROR_TIMEOUT = 1460;
        public const int FTR_ERROR_NOT_READY = 21;
        public const int FTR_ERROR_BAD_CONFIGURATION = 1610;
        public const int FTR_ERROR_INVALID_PARAMETER = 87;
        public const int FTR_ERROR_CALL_NOT_IMPLEMENTED = 120;
        public const int FTR_ERROR_NOT_SUPPORTED = 50;
        public const int FTR_ERROR_WRITE_PROTECT = 19;
        public const int FTR_ERROR_MESSAGE_EXCEEDS_MAX_SIZE = 4336;

        public const int FTR_ANSISDK_ERROR_IMAGE_SIZE_NOT_SUP = 0x30000001;
        public const int FTR_ANSISDK_ERROR_EXTRACTION_UNSPEC = 0x30000002;
        public const int FTR_ANSISDK_ERROR_EXTRACTION_BAD_IMP = 0x30000003;
        public const int FTR_ANSISDK_ERROR_MATCH_NULL = 0x30000004;
        public const int FTR_ANSISDK_ERROR_MATCH_PARSE_PROBE = 0x30000005;
        public const int FTR_ANSISDK_ERROR_MATCH_PARSE_GALLERY = 0x30000006;
        public const int FTR_ANSISDK_ERROR_MORE_DATA = 0x30000007;


        private static class FtrAnsiSdk32
        {
            [DllImport("ftrScanAPI.dll", SetLastError = true)]
            public static extern IntPtr ftrScanOpenDevice();

            [DllImport("ftrScanAPI.dll", SetLastError = true)]
            public static extern void ftrScanCloseDevice(IntPtr hDevice);

            [DllImport("ftrScanAPI.dll", SetLastError = true)]
            public static extern bool ftrScanGetImageSize(IntPtr hDevice, ref FTRSCAN_IMAGE_SIZE pImageSize);

            [DllImport("ftrAnsiSdk.dll", SetLastError = true)]
            public static extern bool ftrAnsiSdkCaptureImage(IntPtr hDevice, byte[] pBuffer);

            [DllImport("ftrAnsiSdk.dll", SetLastError = true)]
            public static extern int ftrAnsiSdkGetMaxTemplateSize();

            [DllImport("ftrAnsiSdk.dll", SetLastError = true)]
            public static extern bool ftrAnsiSdkCreateTemplate(IntPtr hDevice, byte byFingerPosition, byte[] pOutImageBuffer, byte[] pOutTemplate, ref int pnOutTemplateSize);

            [DllImport("ftrAnsiSdk.dll", SetLastError = true)]
            public static extern bool ftrAnsiSdkCreateTemplateFromBuffer(IntPtr hDevice, byte byFingerPosition, byte[] pImageBuffer, int nWidth, int nHeight, byte[] pOutTemplate, ref int pnOutTemplateSize);

            [DllImport("ftrAnsiSdk.dll", SetLastError = true)]
            public static extern bool ftrAnsiSdkVerifyTemplate(IntPtr hDevice, byte byFingerPosition, byte[] pInTemplate, byte[] pOutImageBuffer, ref float pfOutResult);

            [DllImport("ftrAnsiSdk.dll", SetLastError = true)]
            public static extern bool ftrAnsiSdkMatchTemplates(byte[] pProbeTemplate, byte[] pGaleryTemplate, ref float pfOutResult);

            [DllImport("ftrAnsiSdk.dll", SetLastError = true)]
            public static extern bool ftrAnsiSdkConvertAnsiTemplateToIso(byte[] pTemplateANSI, byte[] pTemplateIso, ref int pnInOutTemplateSize);
        }



        private static class FtrAnsiSdk64
        {
            [DllImport("ftrScanAPI.dll", SetLastError = true)]
            public static extern IntPtr ftrScanOpenDevice();

            [DllImport("ftrScanAPI.dll", SetLastError = true)]
            public static extern void ftrScanCloseDevice(IntPtr hDevice);

            [DllImport("ftrScanAPI.dll", SetLastError = true)]
            public static extern bool ftrScanGetImageSize(IntPtr hDevice, ref FTRSCAN_IMAGE_SIZE pImageSize);

            [DllImport("ftrAnsiSdk.dll", SetLastError = true)]
            public static extern bool ftrAnsiSdkCaptureImage(IntPtr hDevice, byte[] pBuffer);

            [DllImport("ftrAnsiSdk.dll", SetLastError = true)]
            public static extern int ftrAnsiSdkGetMaxTemplateSize();

            [DllImport("ftrAnsiSdk.dll", SetLastError = true)]
            public static extern bool ftrAnsiSdkCreateTemplate(IntPtr hDevice, byte byFingerPosition, byte[] pOutImageBuffer, byte[] pOutTemplate, ref int pnOutTemplateSize);

            [DllImport("ftrAnsiSdk.dll", SetLastError = true)]
            public static extern bool ftrAnsiSdkCreateTemplateFromBuffer(IntPtr hDevice, byte byFingerPosition, byte[] pImageBuffer, int nWidth, int nHeight, byte[] pOutTemplate, ref int pnOutTemplateSize);

            [DllImport("ftrAnsiSdk.dll", SetLastError = true)]
            public static extern bool ftrAnsiSdkVerifyTemplate(IntPtr hDevice, byte byFingerPosition, byte[] pInTemplate, byte[] pOutImageBuffer, ref float pfOutResult);

            [DllImport("ftrAnsiSdk.dll", SetLastError = true)]
            public static extern bool ftrAnsiSdkMatchTemplates(byte[] pProbeTemplate, byte[] pGaleryTemplate, ref float pfOutResult);

            [DllImport("ftrAnsiSdk.dll", SetLastError = true)]
            public static extern bool ftrAnsiSdkConvertAnsiTemplateToIso(byte[] pTemplateANSI, byte[] pTemplateIso, ref int pnInOutTemplateSize);
        }


        public static IntPtr ftrScanOpenDevice()
        {
            log.LogMethodEntry();
            if (Environment.Is64BitProcess)
            {
                log.LogMethodExit();
                return FtrAnsiSdk64.ftrScanOpenDevice();
            }
            else
            {
                log.LogMethodExit();
                return FtrAnsiSdk32.ftrScanOpenDevice();
            }
        }
        public static void ftrScanCloseDevice(IntPtr hDevice)
        {
            log.LogMethodEntry(hDevice);
            if (Environment.Is64BitProcess)
            {
                log.LogMethodExit();
                FtrAnsiSdk64.ftrScanCloseDevice(hDevice);
            }
            else
            {
                log.LogMethodExit();
                FtrAnsiSdk32.ftrScanCloseDevice(hDevice);
            }
        }

        public static bool ftrScanGetImageSize(IntPtr hDevice, ref FTRSCAN_IMAGE_SIZE pImageSize)
        {
            log.LogMethodEntry(hDevice, pImageSize);
            bool returnValue;
            if (Environment.Is64BitProcess)
            {
                returnValue = FtrAnsiSdk64.ftrScanGetImageSize(hDevice, ref pImageSize);
                log.LogMethodExit(returnValue);
                return returnValue;
            }
            else
            {
                returnValue = FtrAnsiSdk32.ftrScanGetImageSize(hDevice, ref pImageSize);
                log.LogMethodExit(returnValue);
                return returnValue;
            }
        }
        public static bool ftrAnsiSdkCaptureImage(IntPtr hDevice, byte[] pBuffer)
        {
            log.LogMethodEntry(hDevice, pBuffer);
            bool returnValue;
            if (Environment.Is64BitProcess)
            {
                returnValue = FtrAnsiSdk64.ftrAnsiSdkCaptureImage(hDevice, pBuffer);
                log.LogMethodExit(returnValue);
                return returnValue;
            }
            else
            {
                returnValue = FtrAnsiSdk32.ftrAnsiSdkCaptureImage(hDevice, pBuffer);
                log.LogMethodExit(returnValue);
                return returnValue;
            }
        }
        public static int ftrAnsiSdkGetMaxTemplateSize()
        {
            log.LogMethodEntry();
            int returnValue;
            if (Environment.Is64BitProcess)
            {
                returnValue = FtrAnsiSdk64.ftrAnsiSdkGetMaxTemplateSize();
                log.LogMethodExit(returnValue);
                return returnValue;
            }
            else
            {
                returnValue = FtrAnsiSdk32.ftrAnsiSdkGetMaxTemplateSize();
                log.LogMethodExit(returnValue);
                return returnValue;
            }
        }


        public static bool ftrAnsiSdkCreateTemplate(IntPtr hDevice, byte byFingerPosition, byte[] pOutImageBuffer, byte[] pOutTemplate, ref int pnOutTemplateSize)
        {
            log.LogMethodEntry(hDevice, "byFingerPosition", "pOutImageBuffer", pOutTemplate, pnOutTemplateSize);
            bool returnValue;
            if (Environment.Is64BitProcess)
            {
                returnValue = FtrAnsiSdk32.ftrAnsiSdkCreateTemplate(hDevice, byFingerPosition, pOutImageBuffer, pOutTemplate, ref pnOutTemplateSize);
                log.LogMethodExit(returnValue);
                return returnValue;
            }
            else
            {
                returnValue = FtrAnsiSdk32.ftrAnsiSdkCreateTemplate(hDevice, byFingerPosition, pOutImageBuffer, pOutTemplate, ref pnOutTemplateSize);
                log.LogMethodExit(returnValue);
                return returnValue;
            }
        }
        public static bool ftrAnsiSdkCreateTemplateFromBuffer(IntPtr hDevice, byte byFingerPosition, byte[] pImageBuffer, int nWidth, int nHeight, byte[] pOutTemplate, ref int pnOutTemplateSize)
        {
            log.LogMethodEntry(hDevice, "byFingerPosition"," pImageBuffer", nWidth, nHeight, pOutTemplate, pnOutTemplateSize);
            bool returnValue;
            if (Environment.Is64BitProcess)
            {
                returnValue = FtrAnsiSdk64.ftrAnsiSdkCreateTemplateFromBuffer(hDevice, byFingerPosition, pImageBuffer, nWidth, nHeight, pOutTemplate, ref pnOutTemplateSize);
                log.LogMethodExit(returnValue);
                return returnValue;
            }
            else
            {
                returnValue = FtrAnsiSdk32.ftrAnsiSdkCreateTemplateFromBuffer(hDevice, byFingerPosition, pImageBuffer, nWidth, nHeight, pOutTemplate, ref pnOutTemplateSize);
                log.LogMethodExit(returnValue);
                return returnValue;
            }
        }

        public static bool ftrAnsiSdkVerifyTemplate(IntPtr hDevice, byte byFingerPosition, byte[] pInTemplate, byte[] pOutImageBuffer, ref float pfOutResult)
        {
            log.LogMethodEntry(hDevice, byFingerPosition, pInTemplate, pOutImageBuffer, pfOutResult);
            bool returnValue;
            if (Environment.Is64BitProcess)
            {
                returnValue = FtrAnsiSdk64.ftrAnsiSdkVerifyTemplate(hDevice, byFingerPosition, pInTemplate, pOutImageBuffer, ref pfOutResult);
                log.LogMethodExit(returnValue);
                return returnValue;
            }
            else
            {
                returnValue = FtrAnsiSdk32.ftrAnsiSdkVerifyTemplate(hDevice, byFingerPosition, pInTemplate, pOutImageBuffer, ref pfOutResult);
                log.LogMethodExit(returnValue);
                return returnValue;
            }
        }

        public static bool ftrAnsiSdkMatchTemplates(byte[] pProbeTemplate, byte[] pGaleryTemplate, ref float pfOutResult)
        {

            log.LogMethodEntry(pProbeTemplate, pGaleryTemplate, pfOutResult);
            bool returnValue;
            if (Environment.Is64BitProcess)
            {
                returnValue = FtrAnsiSdk64.ftrAnsiSdkMatchTemplates(pProbeTemplate, pGaleryTemplate, ref pfOutResult);
                log.LogMethodExit(returnValue);
                return returnValue;
            }
            else
            {
                returnValue = FtrAnsiSdk32.ftrAnsiSdkMatchTemplates(pProbeTemplate, pGaleryTemplate, ref pfOutResult);
                log.LogMethodExit(returnValue);
                return returnValue;
            }
        }
        public static bool ftrAnsiSdkConvertAnsiTemplateToIso(byte[] pTemplateANSI, byte[] pTemplateIso, ref int pnInOutTemplateSize)
        {
            log.LogMethodEntry(pTemplateANSI, pTemplateIso, pnInOutTemplateSize);
            bool returnValue;
            if (Environment.Is64BitProcess)
            {
                returnValue = FtrAnsiSdk64.ftrAnsiSdkConvertAnsiTemplateToIso(pTemplateANSI, pTemplateIso, ref pnInOutTemplateSize);
                log.LogMethodExit(returnValue);
                return returnValue;
            }
            else
            {
                returnValue = FtrAnsiSdk32.ftrAnsiSdkConvertAnsiTemplateToIso(pTemplateANSI, pTemplateIso, ref pnInOutTemplateSize);
                log.LogMethodExit(returnValue);
                return returnValue;
            }

        }
        public static string GetErrorMessage(int nError)
        {
            log.LogMethodEntry(nError);
            string stError = null;
            switch (nError)
            {
                case FTR_ERROR_SUCCESS:
                    stError = "OK";
                    break;
                case FTR_ERROR_EMPTY_FRAME: // ERROR_EMPTY
                    stError = "- Empty frame -";
                    break;
                case FTR_ERROR_MOVABLE_FINGER:
                    stError = "- Movable finger -";
                    break;
                case FTR_ERROR_NO_FRAME:
                    stError = "- No frame -";
                    break;
                case FTR_ERROR_USER_CANCELED:
                    stError = "- User canceled -";
                    break;
                case FTR_ERROR_HARDWARE_INCOMPATIBLE:
                    stError = "- Incompatible hardware -";
                    break;
                case FTR_ERROR_FIRMWARE_INCOMPATIBLE:
                    stError = "- Incompatible firmware -";
                    break;
                case FTR_ERROR_INVALID_AUTHORIZATION_CODE:
                    stError = "- Invalid authorization code -";
                    break;

                case FTR_ANSISDK_ERROR_IMAGE_SIZE_NOT_SUP:
                    stError = "- Image size is not supported -";
                    break;
                case FTR_ANSISDK_ERROR_EXTRACTION_UNSPEC:
                    stError = "- Unspecified extraction error -";
                    break;
                case FTR_ANSISDK_ERROR_EXTRACTION_BAD_IMP:
                    stError = "- Incorrect impression type -";
                    break;
                case FTR_ANSISDK_ERROR_MATCH_NULL:
                case FTR_ANSISDK_ERROR_MATCH_PARSE_PROBE:
                case FTR_ANSISDK_ERROR_MATCH_PARSE_GALLERY:
                    stError = "- Incorrect parameter -";
                    break;
                default:
                    stError = string.Format("Unknown return code - %d", nError);
                    break;
            }
            log.LogMethodExit(stError);
            return stError;
        }
    }
}
