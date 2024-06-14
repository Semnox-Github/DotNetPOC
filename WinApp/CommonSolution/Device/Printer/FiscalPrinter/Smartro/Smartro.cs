
/********************************************************************************************
 * Project Name - Device
 * Description  - KoreaFiscalization  Printer
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By            Remarks          
 *********************************************************************************************

*2.150.0      01-Dec-2021      Vidita Solution        Created: Korean Fiscalization
*2.150.0      01-Feb-2021      Girish Kundar          Modified: Using override method PrintReceipt
*2.130.10     22-May-2023      Sathyavathi            Modified: Externalized SMARTRO_TIME_OUT value
********************************************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.Device.Printer.FiscalPrinter.Smartro;
using Semnox.Parafait.Languages;
namespace Semnox.Parafait.Device.Printer.FiscalPrint.Smartro
{

    //   ****Smartro Mapping in to FiscalizationRequest Object ******//
    // 1. Transaction Amount mapped to FiscalizationRequest.PaymentInfo.Amount
    // 2. PrintOption        mapped to FiscalizationRequest.PaymentInfo.Description
    // 3. approvalId         mapped to FiscalizationRequest.PaymentInfo.reference
    // 4. approvalDate       mapped to FiscalizationRequest.PaymentInfo.moment
    // 5. CreditCardAuthorization     mapped to FiscalizationRequest.extReference 
    // 6. ExternalSourceReference      mapped to FiscalizationRequest.PaymentInfo.reference  
    // 7. CardNumber         mapped to FiscalizationRequest.PaymentInfo.description 
    // 8. CardCompanu        mapped to FiscalizationRequest.transactionLine.description
    //   ****Smartro Mapping in to FiscalizationRequest Object ******//
    public class Smartro : FiscalPrinter
    {
        private static Dictionary<string, string> SmartroErrorCodes = new Dictionary<string, string>();
        SmartroLib._ST_DATA_[] stTeleData =
          {
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_BCC,    "0001"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_DDC_CODE,   "0002"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_TELEGRAM_ETX,   "0003"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_FILLER1,    "0004"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_FILLER2,    "0005"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_TELEGRAM_FS,    "0006"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_MASTERKEY_IDX,  "0007"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_TELEGRAM_STX,   "0008"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_SAM_ID, "0009"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_SW_VERSION, "0010"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_VERSION_INFO,   "0011"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_WORKING_KEY,    "0012"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_FRANCHISE_INFO, "0013"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_FRANCHISE_NAME, "0014"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_FRANCHISE_ID,   "0015"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_FRANCHISE_TELEPHONE,    "0016"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_FRANCHISE_ADDRESS,  "0017"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_AVAILABLE_SCORES,   "0018"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_TRADE_UNIQUE_ID,    "0019"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_TRADE_UNIQUE_ID_13, "0020"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_TRADE_SEPARATE_CODE,    "0021"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_DEAL_DATE_TIME, "0022"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_AFTER_TRADE_BALANCE,    "0023"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_PAYMENT_DIVISION,   "0024"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_ADD_TAX_AMOUNT, "0025"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_MEACHINE_SERIAL_NUMBER, "0026"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_RANDOM_NUMBER,  "0027"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_ACCRUE_SCORES,  "0028"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_TERMINAL_NUMBER,    "0029"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_TERMINAL_VERSION,   "0030"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_PERSON_NAME,    "0031"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_REPRESENTATIVE_PERSON,  "0032"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_PURCHASE,   "0033"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_PURCHASE_NAME,  "0034"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_SALES_TIME, "0035"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_SALES_DATE, "0036"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_HEADER, "0037"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_MISS_PAGE_COUNT,    "0038"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_ISSUE,  "0039"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_ISSUE_NAME, "0040"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_OCCUR_SCORES,   "0041"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_BONUS_PURCHASE, "0042"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_BONUS_ISSUE,    "0043"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_BONUS_USED_SEPARATE,    "0044"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_BONUS_APPROVAL_ID,  "0045"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_BONUS_RESPONSE_CODE,    "0046"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_BONUS_INFO, "0047"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_BONUS_OUTPUT_MSG,   "0048"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_BONUS_SEPARATE, "0049"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_BONUS_CARD_NUMBER,  "0050"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_SERVICE_CHARGE, "0051"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_ADD_INFO_SEPARATE,  "0052"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_PASSWORD,   "0053"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_BUSINESS_NUMBER,    "0054"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_SIGN_IMAGE_DATA,    "0055"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_SIGNPAD_ID, "0056"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_GOOD_INFO,  "0057"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_SIGN_SET,   "0058"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_SIGN_IMAGE_INFO,    "0059"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_SERVER_TRADE_DATE,  "0060"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_SERVICE_SEPARATE,   "0061"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_SERVICE_TYPE,   "0062"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_TAX,    "0063"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_APPROVAL_AMOUNT,    "0064"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_APPROVAL_ID,    "0065"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_BUSINESS_DIVISION,  "0066"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_RECEIPT_PRINT_TYPE, "0067"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_SALE_DATA_POS_NO,   "0068"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_REQUEST_NUMBER, "0069"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_REQUEST_CODE,   "0070"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_BASE_TRADE_DATE,    "0071"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_EXPIRY_DATE,    "0072"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_RESPONSE_CODE,  "0073"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_IMAGE_DATA, "0074"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_PRINT_MSG,  "0075"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_SAVE_INDEX, "0076"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_IMAGE_NAME, "0077"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_BALANCE,    "0078"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_TELEGRAM_SIZE,  "0079"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_RECEIPT_TITLE,  "0080"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_INFO_CHANGE_WORK,   "0081"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_PAYMENT_LIST,   "0082"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_PAYMENT_TIME,   "0083"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_PAYMENT_DATE,   "0084"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_NORMAL_CHECK_RESPONSE_TIME, "0085"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_TOTAL_BUY_AMOUNT_AND,   "0086"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_WITHDRAWAL_AMOUNT,  "0087"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_OUTPUT_DATA,    "0088"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_OUTPUT_MSG, "0089"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_CASH_CANCEL_REASON, "0090"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_CARD_NUMBER,    "0091"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_COUPON_NUMBER,  "0092"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_ITEM_NAME,  "0093"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_INSTALLMENT_PERIOD_USED_CHECK,  "0094"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_INSTALLMENT_PERIOD, "0095"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_HASH_CODE,  "0096"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_CASH_APPROVAL_TYPE, "0097"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_DISPLAY_MSG_1,  "0098"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_DISPLAY_MSG_2,  "0099"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_DISPLAY_MSG_3,  "0100"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_DISPLAY_MSG,    "0101"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_STATE_CODE, "0102"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_OCCUR_SCORES_BEFORE,    "0103"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_CHECK_SEPARATE, "0104"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_CHECK_INFO, "0105"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_CHECK_DATE, "0106"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_CHECK_CODE, "0107"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_CHECK_AMOUNT,   "0108"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_CAT_PORT,   "0109"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_CAT_BPS,    "0110"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_SIGNPAD_TYPE,   "0111"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_SIGNPAD_PORT,   "0112"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_SIGNPAD_BPS,    "0113"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_OPERATION_METHOD,   "0114"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_LINKED_TRADE_DISPLAY,   "0115"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_BARCODE_GOODS_NUMBER,   "0116"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_CURRENCY_CODE,  "0117"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_STORE_CODE, "0118"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_BUSINESS_INFO_01,   "0119"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_BUSINESS_INFO_02,   "0120"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_BUSINESS_INFO_03,   "0121"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_BUSINESS_INFO_04,   "0122"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_BUSINESS_INFO_05,   "0123"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_BUSINESS_INFO_06,   "0124"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_BUSINESS_INFO_07,   "0125"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_BUSINESS_INFO_08,   "0126"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_BUSINESS_INFO_09,   "0127"  ),
            new SmartroLib._ST_DATA_( SmartroLib.MSG_ITME_ENUM.ITEM_BUSINESS_INFO_10,   "0128"  ),
            new SmartroLib._ST_DATA_( -1    ,                                       "END"),
        };
        private String m_strIP;
        private int m_nPort;
        private List<string> VCATArray;
        private Utilities Utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SmartroPaymentResponseDTO smartroPaymentResponseDTO;
        private IDisplayStatusUI statusDisplayUi;
        private string message;
        private const int TIME_OUT_ERROR_CODE = -104;
        private bool unAttended;

        public Smartro(Utilities utilities, bool unattended = false) : base(utilities)
        {
            log.LogMethodEntry(utilities, unattended);
            Utilities = utilities;
            this.unAttended = unattended;
            VCATArray = new List<string>();
            smartroPaymentResponseDTO = new SmartroPaymentResponseDTO();
            BuildErrorCodes();
            log.LogMethodExit();
        }

        private void BuildErrorCodes()
        {
            AddOrUpdate(SmartroErrorCodes, "00", MessageContainerList.GetMessage(utilities.ExecutionContext, "Normal"));
            AddOrUpdate(SmartroErrorCodes, "FE", MessageContainerList.GetMessage(utilities.ExecutionContext, 4245)); //"Card recognition failure (IC recognition/MS reading all failed)");
            AddOrUpdate(SmartroErrorCodes, "FF", MessageContainerList.GetMessage(utilities.ExecutionContext, 4246)); //"Transaction Failed (Application Processing Failed)");
            AddOrUpdate(SmartroErrorCodes, "CB", MessageContainerList.GetMessage(utilities.ExecutionContext, 4247)); //"FallBack Fail (unusual Fall back Case)");
            AddOrUpdate(SmartroErrorCodes, "F2", MessageContainerList.GetMessage(utilities.ExecutionContext, 4248)); //"No card application (Card Standby Time Out)");
            AddOrUpdate(SmartroErrorCodes, "F9", MessageContainerList.GetMessage(utilities.ExecutionContext, 4249)); //"Invalid Command (No Command ID)");
            AddOrUpdate(SmartroErrorCodes, "F8", MessageContainerList.GetMessage(utilities.ExecutionContext, 4250)); //"Invalid Data (Bad Input DATA)");
            AddOrUpdate(SmartroErrorCodes, "EA", MessageContainerList.GetMessage(utilities.ExecutionContext, 4251)); //"Input Timeout (Select Multiple Cards, Offline PinInput Standby)");
            AddOrUpdate(SmartroErrorCodes, "EB", MessageContainerList.GetMessage(utilities.ExecutionContext, 4252)); //"Reader equipment abnormality (integrity verification failure status)");
            AddOrUpdate(SmartroErrorCodes, "EC", MessageContainerList.GetMessage(utilities.ExecutionContext, 4253)); //"IC Card Removal Required");
            AddOrUpdate(SmartroErrorCodes, "ED", MessageContainerList.GetMessage(utilities.ExecutionContext, 4254)); //"Encryption key exchange request");
            AddOrUpdate(SmartroErrorCodes, "EE", MessageContainerList.GetMessage(utilities.ExecutionContext, 4255)); //"Encryption Pin processing failed");
            AddOrUpdate(SmartroErrorCodes, "S1", MessageContainerList.GetMessage(utilities.ExecutionContext, 4256)); //"Signature pad device error");
            AddOrUpdate(SmartroErrorCodes, "L1", MessageContainerList.GetMessage(utilities.ExecutionContext, 4257)); //"No config file");
            AddOrUpdate(SmartroErrorCodes, "E0", MessageContainerList.GetMessage(utilities.ExecutionContext, 4258)); //"Configuration error");
            AddOrUpdate(SmartroErrorCodes, "L2", MessageContainerList.GetMessage(utilities.ExecutionContext, 4259)); //"No authentication file");
            AddOrUpdate(SmartroErrorCodes, "L3", MessageContainerList.GetMessage(utilities.ExecutionContext, 4260)); //"Authentication error");
            AddOrUpdate(SmartroErrorCodes, "L0", MessageContainerList.GetMessage(utilities.ExecutionContext, 4261)); //"Config, no authentication files");
            AddOrUpdate(SmartroErrorCodes, "D0", MessageContainerList.GetMessage(utilities.ExecutionContext, 4262)); //"Device port not set");
            AddOrUpdate(SmartroErrorCodes, "D1", MessageContainerList.GetMessage(utilities.ExecutionContext, 4263)); // "Integrity Error");
            AddOrUpdate(SmartroErrorCodes, "W0", MessageContainerList.GetMessage(utilities.ExecutionContext, 4264)); //"Working Key Error");
            AddOrUpdate(SmartroErrorCodes, "OE", MessageContainerList.GetMessage(utilities.ExecutionContext, 4265)); //"OCX Error");
            AddOrUpdate(SmartroErrorCodes, "D9", MessageContainerList.GetMessage(utilities.ExecutionContext, 4266)); //"Full Text Error");
            AddOrUpdate(SmartroErrorCodes, "UC", MessageContainerList.GetMessage(utilities.ExecutionContext, 4267)); //"User Cancellation");
            AddOrUpdate(SmartroErrorCodes, "NR", MessageContainerList.GetMessage(utilities.ExecutionContext, 4268)); //"Not in initial state");
            AddOrUpdate(SmartroErrorCodes, "AE", MessageContainerList.GetMessage(utilities.ExecutionContext, 4269)); //"Already running");
            AddOrUpdate(SmartroErrorCodes, "VR", MessageContainerList.GetMessage(utilities.ExecutionContext, 4270)); //"VCAT re-run required");
            AddOrUpdate(SmartroErrorCodes, "IC", MessageContainerList.GetMessage(utilities.ExecutionContext, 4280)); //"Unable to initialize during server communication");
            AddOrUpdate(SmartroErrorCodes, "IR", MessageContainerList.GetMessage(utilities.ExecutionContext, 4281)); //"Invalid registered number");
        }

        static void AddOrUpdate(Dictionary<string, string> dict, string key, string value)
        {
            if (dict.ContainsKey(key))
            {
                dict[key] = value;
            }
            else
            {
                dict.Add(key, value);
            }
        }
        private void LOG(String strData)
        {
            log.LogMethodEntry(strData);
            //VCATArray.Add(strData);
            log.LogMethodExit(VCATArray);
        }
        private void LOGPRINT(int nIdx, String strTitle, String strData)
        {
            log.LogMethodEntry(nIdx, strTitle, strData);
            String strMSG = "";
            if (nIdx == SmartroLib.LOG_START)
            {
                strMSG = "================================================";
                LOG(strMSG);
                strMSG = String.Format("[{0}] [{1}]", strTitle, "START");
                log.Debug("LOG_START:" + strMSG);
            }
            else if (nIdx == SmartroLib.LOG_END)
            {
                strMSG = String.Format("[{0}] [{1}]", strTitle, "END");
                LOG(strMSG);
                log.Debug("LOG_END:" + strMSG);
                strMSG = "================================================";
            }
            else if (nIdx == SmartroLib.LOG_DATA)
            {
                strMSG = String.Format("[{0}] {1}", strTitle, strData);
                log.Debug("LOG_DATA:" + strMSG);
            }
            else if (nIdx == SmartroLib.LOG_RES_DATA)
            {
                strMSG = strData;
                log.Debug("LOG_RES_DATA:" + strMSG);
            }
            LOG(strMSG);
            log.LogMethodExit();
        }
        private void DATASet(int nIdx, String strValue)
        {
            log.LogMethodEntry(nIdx, strValue);
            String strKEY;
            strKEY = stTeleData[nIdx].strNumber;
            log.Debug("strKEY:" + strKEY);
            SmartroLib.SMTSetData(strKEY, strValue);
            log.LogMethodExit();
        }
        private String DATAGet(int nIdx)
        {
            log.LogMethodEntry(nIdx);
            int nRet;
            String strKEY;
            StringBuilder strRet = new StringBuilder(new String('\0', 4096));
            String strMSG = "";
            strKEY = stTeleData[nIdx].strNumber;
            nRet = SmartroLib.SMTGetData(1, strKEY, strRet);
            if (nRet > 0)
            {
                strMSG = String.Format("[{0}] <{1}>", strKEY, strRet);
                log.Debug("(nRet > 0 :" + strMSG);
            }

            if (nIdx == (int)SmartroLib.MSG_ITME_ENUM.ITEM_APPROVAL_ID)
            {
                log.Debug("Approval Number :" + strRet.ToString());
            }
            else if (nIdx == (int)SmartroLib.MSG_ITME_ENUM.ITEM_SALES_DATE)
            {
                log.Debug("TRADE DATE (YYYYMMDD) :" + strRet.ToString());
            }

            if (strMSG.Length > 0)
            {
                LOGPRINT(SmartroLib.LOG_RES_DATA, "", strMSG);
                log.Debug("(LOG_RES_DATA :" + strMSG);
            }
            log.LogMethodExit(strRet.ToString());
            return strRet.ToString();
        }
        public override bool PrintReceipt(FiscalizationRequest receiptRequest, ref string Message)
        {
            log.LogMethodEntry(receiptRequest);
            //if (OpenPort() == false)
            //{
            //    log.Error("Unable to connect to VCAT device");
            //    Message = "Unable to connect to VCAT device";
            //    return false;
            //}
            var smartroIP = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "FISCAL_DEVICE_TCP/IP_ADDRESS");
            var smartroPort = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "FISCAL_PRINTER_PORT_NUMBER");
            if (string.IsNullOrWhiteSpace(smartroIP) || string.IsNullOrWhiteSpace(smartroPort))
            {
                log.Error("Smartro IP and Port number is not set up");
                throw new Exception(MessageContainerList.GetMessage(Utilities.ExecutionContext, 4278));
            }
            log.Debug("IP Address : " + smartroIP);
            m_strIP = smartroIP;
            log.Debug("Port number  : " + smartroPort);
            m_nPort = Convert.ToInt32(smartroPort);

            bool fiscalizationResult = true;
            PaymentInfo paymentInfo = null;
            TransactionLine transactionLine = null;
            string installmentMonth = "00";
            decimal taxAmount = 0;
            List<PaymentInfo> payItemList = new List<PaymentInfo>();
            if (receiptRequest != null)
            {
                try
                {
                    if (receiptRequest.payments != null && receiptRequest.payments.Any())
                    {
                        paymentInfo = receiptRequest.payments.FirstOrDefault();
                        log.LogVariableState("paymentInfo", paymentInfo);
                        installmentMonth = paymentInfo.quantity.ToString();
                        log.LogVariableState("installtionMonth", installmentMonth);
                        if (installmentMonth.Length < 2)
                        {
                            installmentMonth = "0" + installmentMonth;
                            log.LogVariableState("installtionMonth after appending 0 : ", installmentMonth);
                        }
                    }
                    if (receiptRequest.transactionLines != null && receiptRequest.transactionLines.Any())
                    {
                        transactionLine = receiptRequest.transactionLines.FirstOrDefault();
                        log.LogVariableState("transactionLine", transactionLine);
                        taxAmount = Convert.ToDecimal(transactionLine.VATAmount);
                        log.LogVariableState("taxAmount", taxAmount);
                    }
                    switch (paymentInfo.paymentMode)
                    {
                        case "Cash":
                            {
                                if (receiptRequest.isReversal)
                                {
                                    CashReversal(paymentInfo.amount.ToString(), paymentInfo.reference, paymentInfo.description, receiptRequest.extReference, taxAmount);
                                    log.LogVariableState("smartroPaymentResponseDTO", smartroPaymentResponseDTO);
                                }
                                else
                                {
                                    CashPayment(paymentInfo.amount.ToString(), paymentInfo.description, taxAmount);
                                    log.LogVariableState("smartroPaymentResponseDTO", smartroPaymentResponseDTO);
                                }
                            }
                            break;
                        case "CreditCard":
                            {
                                if (receiptRequest.isReversal)
                                {
                                    CreditCardReversal(paymentInfo.amount.ToString(), paymentInfo.reference, paymentInfo.description, taxAmount, installmentMonth);
                                    log.LogVariableState("smartroPaymentResponseDTO", smartroPaymentResponseDTO);
                                }
                                else
                                {
                                    CreditPayment(paymentInfo.amount.ToString(), taxAmount, installmentMonth);
                                    log.LogVariableState("smartroPaymentResponseDTO", smartroPaymentResponseDTO);
                                }
                            }
                            break;
                        default:
                            {
                                fiscalizationResult = false;
                                Message = MessageContainerList.GetMessage(Utilities.ExecutionContext, 4282);
                            }
                            break;
                    }
                    PaymentInfo responseData = new PaymentInfo();
                    receiptRequest.extReference = smartroPaymentResponseDTO.ItemApprovalId;
                    responseData.reference = smartroPaymentResponseDTO.TradeUniqueId;
                    responseData.description = smartroPaymentResponseDTO.CardNumber;
                    payItemList.Add(responseData);
                    receiptRequest.payments = payItemList.ToArray();
                    if (transactionLine != null)
                    {
                        transactionLine.description = smartroPaymentResponseDTO.CardCompany;
                    }
                    log.LogVariableState("receiptRequest", receiptRequest);
                }
                catch (Exception ex)
                {
                    log.Error("Exception in PrintReceipt", ex);
                    fiscalizationResult = false;
                    Message = MessageContainerList.GetMessage(Utilities.ExecutionContext, ex.Message);
                    throw ex;
                }
            }
            log.LogMethodExit(fiscalizationResult);
            return fiscalizationResult;
        }

        public override bool IsConfirmationRequired(FiscalizationRequest receiptRequest)
        {
            log.LogMethodEntry(receiptRequest);
            if (receiptRequest != null)
            {
                if (receiptRequest.payments != null && receiptRequest.payments.Any())
                {
                    if (receiptRequest.payments[0].paymentMode == "Cash")
                    {
                        log.LogMethodExit(true);
                        return true;
                    }
                    else
                    {
                        log.LogMethodExit(false);
                        return false;
                    }
                }
            }
            log.LogMethodExit(false);
            return false;
        }

        //Cash payment
        private void CashPayment(string amount, string printOption, decimal taxAmount = 0)
        {
            log.LogMethodEntry(amount, printOption, taxAmount);
            int nRet;
            String strMSG = "";
            int nKeyType;
            LOGPRINT(SmartroLib.LOG_START, "CASH APPROVAL", strMSG);
            nKeyType = 1;  // KEY = NUMBER
            SmartroLib.InitData();
            log.Debug("CASH_PAY_ITEM_SERVICE_TYPE : " + SmartroDefaults.GetSmartroDefault("CASH_PAY_ITEM_SERVICE_TYPE"));
            log.Debug("CASH_PAY_ITEM_TRADE_SEPARATE_CODE : " + SmartroDefaults.GetSmartroDefault("CASH_PAY_ITEM_TRADE_SEPARATE_CODE"));
            log.Debug("CASH_PAY_ITEM_INSTALLMENT_PERIOD_USED_CHECK : " + SmartroDefaults.GetSmartroDefault("CASH_PAY_ITEM_INSTALLMENT_PERIOD_USED_CHECK"));
            log.Debug("CASH_PAY_ITEM_SIGN_SET : " + SmartroDefaults.GetSmartroDefault("CASH_PAY_ITEM_SIGN_SET"));
            log.Debug("SMARTRO_TIME_OUT : " + SmartroDefaults.GetSmartroDefault("SMARTRO_TIME_OUT"));
            log.Debug("IP Address : " + m_strIP);
            log.Debug("m_nPort : " + m_nPort);
            log.Debug("nKeyType : " + nKeyType);
            if (printOption.CompareTo("BUSINESS_PERSON") == 0)//BUSINESSPERSON
            {
                DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_SERVICE_TYPE, SmartroDefaults.GetSmartroDefault("CASH_PAY_ITEM_SERVICE_TYPE"));         // SERVICE TYPE
                DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_TRADE_SEPARATE_CODE, SmartroDefaults.GetSmartroDefault("CASH_PAY_ITEM_TRADE_SEPARATE_CODE"));
                DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_APPROVAL_AMOUNT, amount);
                DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_INSTALLMENT_PERIOD_USED_CHECK, SmartroDefaults.GetSmartroDefault("CASH_PAY_BUSINESSPERSON_ITEM_INSTALLMENT_PERIOD_USED_CHECK"));
                DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_SIGN_SET, SmartroDefaults.GetSmartroDefault("CASH_PAY_ITEM_SIGN_SET"));
            }
            else if (printOption.CompareTo("CUSTOMER") == 0)//CUSTOMER
            {
                DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_SERVICE_TYPE, SmartroDefaults.GetSmartroDefault("CASH_PAY_ITEM_SERVICE_TYPE"));         // SERVICE TYPE
                DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_TRADE_SEPARATE_CODE, SmartroDefaults.GetSmartroDefault("CASH_PAY_ITEM_TRADE_SEPARATE_CODE"));
                DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_APPROVAL_AMOUNT, amount);
                DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_INSTALLMENT_PERIOD_USED_CHECK, SmartroDefaults.GetSmartroDefault("CASH_PAY_CUSTOMER_ITEM_INSTALLMENT_PERIOD_USED_CHECK"));
                DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_SIGN_SET, SmartroDefaults.GetSmartroDefault("CASH_PAY_ITEM_SIGN_SET"));
            }
            else if (printOption.CompareTo("VOLUNTEER_ISSUANCE") == 0) //VOLUNTEER_ISSUANCE - No receipt
            {
                DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_SERVICE_TYPE, SmartroDefaults.GetSmartroDefault("CASH_PAY_VOLUNTEER_ISSUANCE_ITEM_SERVICE_TYPE"));         // SERVICE TYPE
                DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_TRADE_SEPARATE_CODE, SmartroDefaults.GetSmartroDefault("CASH_PAY_VOLUNTEER_ISSUANCE_ITEM_TRADE_SEPARATE_CODE"));
                DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_APPROVAL_AMOUNT, amount);
                DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_INSTALLMENT_PERIOD_USED_CHECK, SmartroDefaults.GetSmartroDefault("CASH_PAY_VOLUNTEER_ISSUANCE_ITEM_INSTALLMENT_PERIOD_USED_CHECK"));
                DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_SIGN_SET, SmartroDefaults.GetSmartroDefault("CASH_PAY_VOLUNTEER_ISSUANCE_ITEM_SIGN_SET"));
                DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_SIGN_IMAGE_DATA, SmartroDefaults.GetSmartroDefault("CASH_PAY_VOLUNTEER_ISSUANCE_ITEM_SIGN_IMAGE_DATA"));
            }
            else
            {
                log.Error("Invalid print option");
            }
            // Tax and Service charge set up
            DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_TAX, taxAmount.ToString());
            DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_SERVICE_CHARGE, "00");

            int timeOut = Convert.ToInt32(SmartroDefaults.GetSmartroDefault("SMARTRO_TIME_OUT"));
            message = MessageContainerList.GetMessage(utilities.ExecutionContext, 4237);
            try
            {
                statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, unAttended, null, message);
                statusDisplayUi.CancelClicked += StatusDisplayUi_CancelClicked;
                statusDisplayUi.EnableCancelButton(false);
                Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
                thr.Start();
                statusDisplayUi.DisplayText(message);
                nRet = SmartroLib.SMTTcpSendRcv(nKeyType, m_strIP, m_nPort, timeOut);
                if (nRet == TIME_OUT_ERROR_CODE)
                {
                    strMSG = String.Format("nRet = {0}", nRet);
                    LOGPRINT(SmartroLib.LOG_DATA, "CASH APPROVAL", strMSG);
                    log.Error("TIME_OUT_ERROR_CODE , Failed to process payment by VCAT");
                    log.LogMethodExit();
                    throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 4272));
                }
            }
            finally
            {
                statusDisplayUi.CloseStatusWindow();
            }
            strMSG = String.Format("nRet = {0}", nRet);
            LOGPRINT(SmartroLib.LOG_DATA, "CASH APPROVAL", strMSG);
            if (nRet > 0)
            {
                TradeParser(SmartroLib.TRADE_APP);
            }
            LOGPRINT(SmartroLib.LOG_END, "CASH APPROVAL", strMSG);
            if (nRet > 0)
            {
                string response = DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_RESPONSE_CODE);
                log.LogVariableState("response", response);
                if (response == "00")
                {
                    // Get Card or Phone number
                    smartroPaymentResponseDTO.CardNumber = DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_CARD_NUMBER);
                    log.Debug("VCATcardNumber" + smartroPaymentResponseDTO.CardNumber);

                    // Get Trade uniqieId
                    smartroPaymentResponseDTO.TradeUniqueId = DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_TRADE_UNIQUE_ID);
                    log.Debug("TradeUniqueId" + smartroPaymentResponseDTO.TradeUniqueId);

                    // Get Approaval Id
                    smartroPaymentResponseDTO.ItemApprovalId = DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_APPROVAL_ID);
                    log.Debug("ItemApprovalId" + smartroPaymentResponseDTO.ItemApprovalId);

                }
                else
                {
                    // Error Codes 
                    if (SmartroErrorCodes.ContainsKey(response))
                    {
                        string errorMessage = SmartroErrorCodes[response];
                        log.Error(errorMessage);
                        throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, errorMessage));
                    }
                    else
                    {
                        SmartroErrorCodes.Add(response, "Error while reading registered number");
                        string errorMessage = "Error while reading registered number";
                        log.Error(errorMessage);
                        throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 4276));
                    }
                }
            }
            else
            {
                log.Error("VCAT status < 0 , Failed to process payment by VCAT");
                log.LogMethodExit();
                throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 4273));
            }
            log.LogMethodExit();
        }

        private void ShowProcessing(string message)
        {

        }

        private void CloseProcessingUI()
        {
            log.LogMethodEntry();
            if (statusDisplayUi != null)
            {
                statusDisplayUi.CloseStatusWindow();
            }
            log.LogMethodExit();
        }
        //Credit payment
        private void CreditPayment(string amount, decimal taxAmount = 0, string installmentMonth = "00")
        {
            log.LogMethodEntry(amount, taxAmount, installmentMonth);
            try
            {
                int nRet;
                String strMSG = "";
                int nKeyType;
                LOGPRINT(SmartroLib.LOG_START, "CARD APPROVAL", strMSG);
                nKeyType = 1;  // KEY = NUMBER
                SmartroLib.InitData();
                log.Debug("CARD_PAY_ITEM_SERVICE_TYPE : " + SmartroDefaults.GetSmartroDefault("CARD_PAY_ITEM_SERVICE_TYPE"));
                log.Debug("CARD_PAY_ITEM_TRADE_SEPARATE_CODE : " + SmartroDefaults.GetSmartroDefault("CARD_PAY_ITEM_TRADE_SEPARATE_CODE"));
                log.Debug("CARD_PAY_ITEM_SIGN_SET : " + SmartroDefaults.GetSmartroDefault("CARD_PAY_ITEM_SIGN_SET"));
                log.Debug("SMARTRO_TIME_OUT : " + SmartroDefaults.GetSmartroDefault("SMARTRO_TIME_OUT"));
                DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_SERVICE_TYPE, SmartroDefaults.GetSmartroDefault("CARD_PAY_ITEM_SERVICE_TYPE"));         // SERVICE TYPE
                DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_TRADE_SEPARATE_CODE, SmartroDefaults.GetSmartroDefault("CARD_PAY_ITEM_TRADE_SEPARATE_CODE"));
                DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_APPROVAL_AMOUNT, amount);
                DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_SIGN_SET, SmartroDefaults.GetSmartroDefault("CARD_PAY_ITEM_SIGN_SET"));

                // Tax and Service charge set up
                DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_TAX, taxAmount.ToString());
                DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_SERVICE_CHARGE, "00");

                // Installation Month 
                DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_INSTALLMENT_PERIOD_USED_CHECK, installmentMonth);


                int timeOut = Convert.ToInt32(SmartroDefaults.GetSmartroDefault("SMARTRO_TIME_OUT"));
                message = MessageContainerList.GetMessage(utilities.ExecutionContext, 4237);
                string amountMsg = MessageContainerList.GetMessage(Utilities.ExecutionContext, 1839, ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "CURRENCY_SYMBOL") + amount);
                try
                {
                    statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, unAttended, amountMsg, "Smatro Payment");
                    log.Debug("unAttended : " + unAttended);
                    statusDisplayUi.CancelClicked += StatusDisplayUi_CancelClicked;
                    statusDisplayUi.EnableCancelButton(false);
                    Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
                    thr.Start();
                    statusDisplayUi.DisplayText(message);
                    Task<int> task = Task<int>.Factory.StartNew(() => { return SmartroLib.SMTTcpSendRcv(nKeyType, m_strIP, m_nPort, timeOut); });
                    while (task.IsCompleted == false)
                    {
                        Thread.Sleep(100);
                        Application.DoEvents();
                    }
                    nRet = task.Result;
                    if (nRet == TIME_OUT_ERROR_CODE)
                    {
                        strMSG = String.Format("nRet = {0}", nRet);
                        LOGPRINT(SmartroLib.LOG_DATA, "CARD APPROVAL", strMSG);
                        log.Error("TIME_OUT_ERROR_CODE , Failed to process payment by VCAT");
                        log.LogMethodExit();
                        throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 4272));
                    }
                }
                finally
                {
                    statusDisplayUi.CloseStatusWindow();
                }
                strMSG = String.Format("nRet = {0}", nRet);
                log.Debug("SMTTcpSendRcv : " + strMSG);
                LOGPRINT(SmartroLib.LOG_DATA, "CARD APPROVAL", strMSG);
                if (nRet > 0)
                {
                    log.Debug("nRet > 0 : " + nRet);
                    TradeParser(SmartroLib.TRADE_APP);
                }
                LOGPRINT(SmartroLib.LOG_END, "CARD APPROVAL", strMSG);
                if (nRet > 0)
                {
                    string response = DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_RESPONSE_CODE);
                    log.LogVariableState("response", response);
                    if (response == "00")
                    {
                        // Get Card or Phone number
                        smartroPaymentResponseDTO.CardNumber = DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_CARD_NUMBER);
                        log.Debug("VCATcardNumber" + smartroPaymentResponseDTO.CardNumber);

                        // Get Trade uniqieId
                        smartroPaymentResponseDTO.TradeUniqueId = DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_TRADE_UNIQUE_ID);
                        log.Debug("TradeUniqueId" + smartroPaymentResponseDTO.TradeUniqueId);

                        // Get Approaval Id
                        smartroPaymentResponseDTO.ItemApprovalId = DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_APPROVAL_ID);
                        log.Debug("ItemApprovalId" + smartroPaymentResponseDTO.ItemApprovalId);

                        // Get Card Company
                        string itemPurchaseName = DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_PURCHASE_NAME);
                        if (string.IsNullOrWhiteSpace(itemPurchaseName) == false && itemPurchaseName.Length > 4)
                        {
                            string cardCompany = itemPurchaseName.Substring(4, itemPurchaseName.Length - 4);
                            log.Debug("CardCompany" + cardCompany);
                            smartroPaymentResponseDTO.CardCompany = cardCompany;
                            log.Debug("CardCompany" + smartroPaymentResponseDTO.CardCompany);
                        }

                    }
                    else
                    {
                        // Error Codes 
                        if (SmartroErrorCodes.ContainsKey(response))
                        {
                            string errorMessage = SmartroErrorCodes[response];
                            log.Error(errorMessage);
                            throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, errorMessage));
                        }
                        else
                        {
                            SmartroErrorCodes.Add(response, "Error while reading credit card : " + response);
                            string errorMessage = "Error while reading credit card" + " : " + response;
                            log.Error(errorMessage);
                            throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 4277) + " : " + response);
                        }
                    }

                }
                else
                {
                    log.Error("VCAT status < 0 , Failed to process payment by VCAT");
                    log.LogMethodExit();
                    throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 4273));
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
            log.LogMethodExit();
        }

        private void StatusDisplayUi_CancelClicked(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (statusDisplayUi != null)
            {
                statusDisplayUi.DisplayText("Cancelling...");
            }
            //CancellProcess();
            log.LogMethodExit(null);
        }
        ////Cash payment reversal
        private void CashReversal(string amount, string approvalId, string approvalDate, string printOption, decimal taxAmount = 0)
        {
            log.LogMethodEntry(amount, approvalId, approvalDate, printOption, taxAmount);
            int nRet;
            String strMSG = "";
            String strTmp;
            int nKeyType;
            LOGPRINT(SmartroLib.LOG_START, "CASH CANCEL", strMSG);
            nKeyType = 1;  // KEY = NUMBER
            SmartroLib.InitData();
            log.Debug("CASH_CANCEL_ITEM_SERVICE_TYPE : " + SmartroDefaults.GetSmartroDefault("CASH_CANCEL_ITEM_SERVICE_TYPE"));
            log.Debug("CASH_CANCEL_ITEM_TRADE_SEPARATE_CODE : " + SmartroDefaults.GetSmartroDefault("CASH_CANCEL_ITEM_TRADE_SEPARATE_CODE"));
            log.Debug("CASH_CANCEL_ITEM_INSTALLMENT_PERIOD_USED_CHECK : " + SmartroDefaults.GetSmartroDefault("CASH_CANCEL_ITEM_INSTALLMENT_PERIOD_USED_CHECK"));
            log.Debug("CASH_CANCEL_ITEM_SIGN_SET : " + SmartroDefaults.GetSmartroDefault("CASH_CANCEL_ITEM_SIGN_SET"));
            log.Debug("CASH_CANCEL_ITEM_CASH_CANCEL_REASON : " + SmartroDefaults.GetSmartroDefault("CASH_CANCEL_ITEM_CASH_CANCEL_REASON"));
            log.Debug("SMARTRO_TIME_OUT : " + SmartroDefaults.GetSmartroDefault("SMARTRO_TIME_OUT"));

            strTmp = amount;
            DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_APPROVAL_AMOUNT, strTmp);
            strTmp = approvalId;
            if (strTmp.Length == 0)
            {
                log.Error("Approval number is empty");
                throw new Exception(MessageContainerList.GetMessage(Utilities.ExecutionContext, 4274));
            }
            else
            {
                DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_APPROVAL_ID, strTmp); // SERVICE TYPE
            }

            strTmp = approvalDate;
            if (strTmp.Length == 0)
            {
                log.Error("Approval date is empty");
                throw new Exception(MessageContainerList.GetMessage(Utilities.ExecutionContext, 4275));
            }
            else
            {
                DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_BASE_TRADE_DATE, strTmp);
            }
            if (string.IsNullOrWhiteSpace(printOption) == false)
            {
                if (printOption.CompareTo("BUSINESS_PERSON") == 0)//BUSINESSPERSON
                {
                    DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_SERVICE_TYPE, SmartroDefaults.GetSmartroDefault("CASH_CANCEL_ITEM_SERVICE_TYPE"));         // SERVICE TYPE
                    DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_TRADE_SEPARATE_CODE, SmartroDefaults.GetSmartroDefault("CASH_CANCEL_ITEM_TRADE_SEPARATE_CODE"));
                    DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_INSTALLMENT_PERIOD_USED_CHECK, SmartroDefaults.GetSmartroDefault("CASH_PAY_BUSINESSPERSON_ITEM_INSTALLMENT_PERIOD_USED_CHECK"));
                    DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_SIGN_SET, SmartroDefaults.GetSmartroDefault("CASH_CANCEL_ITEM_SIGN_SET"));
                }
                else if (printOption.CompareTo("CUSTOMER") == 0)//CUSTOMER
                {
                    DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_SERVICE_TYPE, SmartroDefaults.GetSmartroDefault("CASH_CANCEL_ITEM_SERVICE_TYPE"));         // SERVICE TYPE
                    DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_TRADE_SEPARATE_CODE, SmartroDefaults.GetSmartroDefault("CASH_CANCEL_ITEM_TRADE_SEPARATE_CODE"));
                    DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_INSTALLMENT_PERIOD_USED_CHECK, SmartroDefaults.GetSmartroDefault("CANCEL_CASH_PAY_CUSTOMER_ITEM_INSTALLMENT_PERIOD_USED_CHECK"));  // 00
                    DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_SIGN_SET, SmartroDefaults.GetSmartroDefault("CASH_CANCEL_ITEM_SIGN_SET"));
                }
                else if (printOption.CompareTo("VOLUNTEER_ISSUANCE") == 0) //VOLUNTEER_ISSUANCE - No receipt
                {
                    DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_SERVICE_TYPE, SmartroDefaults.GetSmartroDefault("CANCEL_CASH_PAY_VOLUNTEER_ISSUANCE_ITEM_SERVICE_TYPE"));         // SERVICE TYPE
                    DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_TRADE_SEPARATE_CODE, SmartroDefaults.GetSmartroDefault("CASH_CANCEL_ITEM_TRADE_SEPARATE_CODE"));
                    DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_INSTALLMENT_PERIOD_USED_CHECK, SmartroDefaults.GetSmartroDefault("CASH_PAY_VOLUNTEER_ISSUANCE_ITEM_INSTALLMENT_PERIOD_USED_CHECK"));
                    DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_SIGN_SET, SmartroDefaults.GetSmartroDefault("CASH_PAY_VOLUNTEER_ISSUANCE_ITEM_SIGN_SET"));
                    DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_SIGN_IMAGE_DATA, SmartroDefaults.GetSmartroDefault("CASH_PAY_VOLUNTEER_ISSUANCE_ITEM_SIGN_IMAGE_DATA"));
                }
                else
                {
                    log.Debug("Invalid cash payment option. Valid values are C,V,B");
                    throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 4273));
                }
            }
            else
            {
                log.Debug("Invalid cash payment option. Valid values are C,V,B");
                throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 4273));
            }
            DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_CASH_CANCEL_REASON, SmartroDefaults.GetSmartroDefault("CASH_CANCEL_ITEM_CASH_CANCEL_REASON"));

            // Tax and Service charge set up
            DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_TAX, taxAmount.ToString());
            DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_SERVICE_CHARGE, "00");

            message = MessageContainerList.GetMessage(utilities.ExecutionContext, 4237);
            try
            {
                statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, false, null, message);
                statusDisplayUi.CancelClicked += StatusDisplayUi_CancelClicked;
                statusDisplayUi.EnableCancelButton(false);
                Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
                thr.Start();
                statusDisplayUi.DisplayText(message);
                nRet = SmartroLib.SMTTcpSendRcv(nKeyType, m_strIP, m_nPort, int.Parse(SmartroDefaults.GetSmartroDefault("SMARTRO_TIME_OUT")));
                if (nRet == TIME_OUT_ERROR_CODE)
                {
                    strMSG = String.Format("nRet = {0}", nRet);
                    LOGPRINT(SmartroLib.LOG_DATA, "CASH CANCEL", strMSG);
                    log.Error("TIME_OUT_ERROR_CODE , Failed to process payment by VCAT");
                    log.LogMethodExit();
                    throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 4272));
                }
            }
            finally
            {
                statusDisplayUi.CloseStatusWindow();
            }
            strMSG = String.Format("nRet = {0}", nRet);
            LOGPRINT(SmartroLib.LOG_DATA, "CASH CANCEL", strMSG);
            if (nRet > 0)
            {
                TradeParser(SmartroLib.TRADE_APP_CAN);
            }
            LOGPRINT(SmartroLib.LOG_END, "CASH CANCEL", strMSG);
            if (nRet > 0)
            {

                string response = DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_RESPONSE_CODE);
                log.LogVariableState("response", response);
                if (response == "00")
                {
                    // Get Card or Phone number
                    smartroPaymentResponseDTO.CardNumber = DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_CARD_NUMBER);
                    log.Debug("VCATcardNumber" + smartroPaymentResponseDTO.CardNumber);

                    // Get Trade uniqieId
                    smartroPaymentResponseDTO.TradeUniqueId = DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_TRADE_UNIQUE_ID);
                    log.Debug("TradeUniqueId" + smartroPaymentResponseDTO.TradeUniqueId);

                    // Get Approaval Id
                    smartroPaymentResponseDTO.ItemApprovalId = DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_APPROVAL_ID);
                    log.Debug("ItemApprovalId" + smartroPaymentResponseDTO.ItemApprovalId);

                }
                else
                {
                    // Error Codes 
                    if (SmartroErrorCodes.ContainsKey(response))
                    {
                        string errorMessage = SmartroErrorCodes[response];
                        log.Error(errorMessage);
                        throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, errorMessage));
                    }
                    else
                    {
                        SmartroErrorCodes.Add(response, "Error while reading registered number");
                        string errorMessage = "Error while reading registered number";
                        log.Error(errorMessage);
                        throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 4276));
                    }
                }
            }
            else
            {
                log.Error("VCAT status < 0 , Failed to process payment by VCAT");
                log.LogMethodExit();
                throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 4273));
            }
            log.LogMethodExit();
        }
        ////Cash payment reversal
        private void CreditCardReversal(string amount, string approvalId, string approvalDate, decimal taxAmount = 0, string installmentMonth = "00")
        {
            log.LogMethodEntry(amount, approvalId, approvalDate, taxAmount, installmentMonth);
            int nRet;
            String strMSG = "";
            String strTmp;
            int nKeyType;
            LOGPRINT(SmartroLib.LOG_START, "CARD CANCEL", strMSG);
            nKeyType = 1;  // KEY = NUMBER
            SmartroLib.InitData();

            log.Debug("CARD_CANCEL_ITEM_SERVICE_TYPE : " + SmartroDefaults.GetSmartroDefault("CARD_CANCEL_ITEM_SERVICE_TYPE"));
            log.Debug("CARD_CANCEL_ITEM_TRADE_SEPARATE_CODE : " + SmartroDefaults.GetSmartroDefault("CARD_CANCEL_ITEM_TRADE_SEPARATE_CODE"));
            log.Debug("CARD_CANCEL_ITEM_SIGN_SET : " + SmartroDefaults.GetSmartroDefault("CARD_CANCEL_ITEM_SIGN_SET"));
            log.Debug("SMARTRO_TIME_OUT : " + SmartroDefaults.GetSmartroDefault("SMARTRO_TIME_OUT"));

            DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_SERVICE_TYPE, SmartroDefaults.GetSmartroDefault("CARD_CANCEL_ITEM_SERVICE_TYPE"));         // SERVICE TYPE
            DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_TRADE_SEPARATE_CODE, SmartroDefaults.GetSmartroDefault("CARD_CANCEL_ITEM_TRADE_SEPARATE_CODE"));
            DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_APPROVAL_AMOUNT, amount);
            strTmp = approvalId;
            if (strTmp.Length == 0)
            {
                log.Error("Approval Number cannot be empty");
                throw new Exception(MessageContainerList.GetMessage(Utilities.ExecutionContext, 4274));
            }
            else
            {
                DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_APPROVAL_ID, strTmp);
            }

            strTmp = approvalDate;
            if (strTmp.Length == 0)
            {
                log.Error("Approval date is empty");
                throw new Exception(MessageContainerList.GetMessage(Utilities.ExecutionContext, 4275));
            }
            else
            {
                DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_BASE_TRADE_DATE, strTmp);
            }

            DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_SIGN_SET, SmartroDefaults.GetSmartroDefault("CARD_CANCEL_ITEM_SIGN_SET"));

            // Tax and Service charge set up
            DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_TAX, taxAmount.ToString());
            DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_SERVICE_CHARGE, "00");

            // Installation Month 
            DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_INSTALLMENT_PERIOD_USED_CHECK, installmentMonth);

            message = MessageContainerList.GetMessage(utilities.ExecutionContext, 4237);
            try
            {
                statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, false, null, message);
                statusDisplayUi.CancelClicked += StatusDisplayUi_CancelClicked;
                statusDisplayUi.EnableCancelButton(false);
                Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
                thr.Start();
                statusDisplayUi.DisplayText(message);
                nRet = SmartroLib.SMTTcpSendRcv(nKeyType, m_strIP, m_nPort, int.Parse(SmartroDefaults.GetSmartroDefault("SMARTRO_TIME_OUT")));
                if (nRet == TIME_OUT_ERROR_CODE)
                {
                    strMSG = String.Format("nRet = {0}", nRet);
                    LOGPRINT(SmartroLib.LOG_DATA, "CARD CANCEL", strMSG);
                    log.Error("TIME_OUT_ERROR_CODE , Failed to process payment by VCAT");
                    log.LogMethodExit();
                    throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 4272));
                }
            }
            finally
            {
                statusDisplayUi.CloseStatusWindow();
            }
            log.Debug("nRet" + nRet);
            strMSG = String.Format("nRet = {0}", nRet);
            LOGPRINT(SmartroLib.LOG_DATA, "CARD CANCEL", strMSG);
            if (nRet > 0)
            {
                TradeParser(SmartroLib.TRADE_APP_CAN);
            }
            LOGPRINT(SmartroLib.LOG_END, "CARD CANCEL", strMSG);
            if (nRet > 0)
            {

                string response = DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_RESPONSE_CODE);
                log.LogVariableState("response", response);
                if (response == "00")
                {
                    // Get Card or Phone number
                    smartroPaymentResponseDTO.CardNumber = DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_CARD_NUMBER);
                    log.Debug("VCATcardNumber" + smartroPaymentResponseDTO.CardNumber);

                    // Get Trade uniqieId
                    smartroPaymentResponseDTO.TradeUniqueId = DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_TRADE_UNIQUE_ID);
                    log.Debug("TradeUniqueId" + smartroPaymentResponseDTO.TradeUniqueId);

                    // Get Approaval Id
                    smartroPaymentResponseDTO.ItemApprovalId = DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_APPROVAL_ID);
                    log.Debug("ItemApprovalId" + smartroPaymentResponseDTO.ItemApprovalId);

                    // Get Card Company
                    string itemPurchaseName = DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_PURCHASE_NAME);
                    if (string.IsNullOrWhiteSpace(itemPurchaseName) == false && itemPurchaseName.Length > 4)
                    {
                        string cardCompany = itemPurchaseName.Substring(4, itemPurchaseName.Length - 4);
                        log.Debug("CardCompany" + cardCompany);
                        smartroPaymentResponseDTO.CardCompany = cardCompany;
                        log.Debug("CardCompany" + smartroPaymentResponseDTO.CardCompany);
                    }

                }
                else
                {
                    // Error Codes 
                    if (SmartroErrorCodes.ContainsKey(response))
                    {
                        string errorMessage = SmartroErrorCodes[response];
                        log.Error(errorMessage);
                        throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, errorMessage));
                    }
                    else
                    {
                        SmartroErrorCodes.Add(response, "Error while reading credit card : " + response);
                        string errorMessage = "Error while reading credit card" + " : " + response;
                        log.Error(errorMessage);
                        throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 4277) + " : " + response);
                    }
                }

            }
            else
            {
                log.Error("VCAT status < 0 , Failed to process payment by VCAT");
                log.LogMethodExit();
                throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 4273));
            }
            log.LogMethodExit();
        }
        private void TradeParser(int nType)
        {
            log.LogMethodEntry(nType);
            DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_SERVICE_TYPE);  // SERVICE TYPE

            if (nType == SmartroLib.TRADE_LINK_CONFIRM)
            {
                DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_RESPONSE_CODE);
                DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_DISPLAY_MSG);
            }
            else if (nType == SmartroLib.TRADE_TRADE_INIT)
            {
                DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_RESPONSE_CODE);
                DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_DISPLAY_MSG);
            }
            else if (nType == SmartroLib.TRADE_VCAT_INFO)
            {
                DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_BUSINESS_NUMBER);
                DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_FRANCHISE_NAME);
                DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_REPRESENTATIVE_PERSON);
                DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_FRANCHISE_TELEPHONE);
                DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_FRANCHISE_ADDRESS);
                DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_TERMINAL_VERSION);
                DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_FILLER1); // Filler1
                                                                     ///////////////////////////////////////////////////
            }
            else
            {
                DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_TRADE_SEPARATE_CODE);
                DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_RESPONSE_CODE);
                DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_CARD_NUMBER);
                DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_DISPLAY_MSG);

                DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_APPROVAL_AMOUNT);
                DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_TAX);
                DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_SERVICE_CHARGE);
                DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_INSTALLMENT_PERIOD_USED_CHECK);
                DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_APPROVAL_ID);
                DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_SALES_DATE);
                DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_SALES_TIME);
                DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_TRADE_UNIQUE_ID);
                DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_FRANCHISE_ID);
                DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_TERMINAL_NUMBER);
                DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_ISSUE);
                DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_PURCHASE);
                DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_DDC_CODE);
                DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_RECEIPT_TITLE);
                DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_OUTPUT_MSG);

                DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_MASTERKEY_IDX); // MasterKey Index (2)
                DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_WORKING_KEY); // WorkingKey      (16)
                DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_SIGN_IMAGE_INFO);
                DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_SIGN_IMAGE_DATA);

                DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_PURCHASE_NAME);

                DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_FILLER1); // Filler1
                DATAGet((int)SmartroLib.MSG_ITME_ENUM.ITEM_FILLER2); // Filler2
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Port Open Method
        /// Method to open the port for the connected printer
        /// </summary>
        public override bool OpenPort()
        {
            try
            {
                int nRet;
                String strMSG = String.Empty;
                int nKeyType;
                LOGPRINT(SmartroLib.LOG_START, "TRADE INIT", strMSG);
                nKeyType = 1;
                var smartroIP = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "FISCAL_DEVICE_TCP/IP_ADDRESS");
                var smartroPort = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "FISCAL_PRINTER_PORT_NUMBER");
                if (string.IsNullOrWhiteSpace(smartroIP) || string.IsNullOrWhiteSpace(smartroPort))
                {
                    log.Error("Smartro IP and Port number is not set up");
                    throw new Exception(MessageContainerList.GetMessage(Utilities.ExecutionContext, 4278));
                }
                log.Debug("IP Address : " + smartroIP);
                m_strIP = smartroIP;
                log.Debug("Port number  : " + smartroPort);
                m_nPort = Convert.ToInt32(smartroPort);

                log.Debug("Calling InitData method");
                SmartroLib.InitData();
                log.Debug("Calling InitData method completed");

                log.Debug("INIT_ITEM_SERVICE_TYPE : " + SmartroDefaults.GetSmartroDefault("INIT_ITEM_SERVICE_TYPE"));
                log.Debug("SMARTRO_TIME_OUT : " + SmartroDefaults.GetSmartroDefault("SMARTRO_TIME_OUT"));
                string serviceType = SmartroDefaults.GetSmartroDefault("INIT_ITEM_SERVICE_TYPE");
                int timeout = Convert.ToInt32(SmartroDefaults.GetSmartroDefault("SMARTRO_TIME_OUT"));
                DATASet((int)SmartroLib.MSG_ITME_ENUM.ITEM_SERVICE_TYPE, serviceType);         // SERVICE TYPE
                nRet = SmartroLib.SMTTcpSendRcv(nKeyType, m_strIP, m_nPort, timeout);
                strMSG = String.Format("nRet = {0}", nRet);
                log.Debug("nRet SMTTcpSendRcv :" + strMSG);
                LOGPRINT(SmartroLib.LOG_DATA, "TRADE INIT", strMSG);
                if (nRet > 0)
                {
                    log.Debug("Calling TradeParser");
                    TradeParser(SmartroLib.TRADE_TRADE_INIT);
                }
                LOGPRINT(SmartroLib.LOG_END, "TRADE INIT", strMSG);

                if (nRet > 0)
                {
                    log.Debug("Open Port success");
                    log.Debug("Port opened Return value " + nRet.ToString());
                }
                else
                {
                    log.Debug("Open Port Failed");
                    log.Error("Port Open failed Return value " + nRet.ToString());
                    return false;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message + " Printer Initialization failed");
                log.LogMethodExit(false);
                return false;
            }
            VCATArray.Clear();
            log.LogMethodExit(true);
            return true;
        }
        //method Make payment
        private bool Initialize(ref string Message)
        {
            log.LogMethodEntry(Message);
            log.Debug("Getting status of printer by calling GetVariable F11");
            return true;
        }
        /// <summary>
        /// Close Method 
        /// </summary>
        public override void ClosePort()
        {
            log.LogMethodEntry();
            if (VCATArray != null)
                VCATArray.Clear();
            log.LogMethodExit(null);
        }
    }
}

