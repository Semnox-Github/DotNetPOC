/********************************************************************************************
 * Project Name - Device
 * Description  - KoreaFiscalization  Printer
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By    Remarks          
 *********************************************************************************************

*2.150.0      01-Dec-2021     Girish Kundar  Created : Korean Fiscalization
********************************************************************************************/
using System;
using System.Runtime.InteropServices;
using System.Text;
//=========================================================================================
// *********Important********* 
// ********Do Not Modify*****
//=========================================================================================

namespace Semnox.Parafait.Device.Printer.FiscalPrint.Smartro
{
    /// <summary>
    /// SmartroLib
    /// </summary>
    public class SmartroLib
    {
        //=========================================================================================
        // DLL Function Call
        //=========================================================================================
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Function : Initialize used Memory
        // Parm : 
        // Return : Success(1), Failure(None)
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [DllImport("SmtSndRcvVCATDLL.dll", EntryPoint = "InitData", CharSet = CharSet.Ansi)]
        public static extern int InitData();

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // KEY = NAME or NUMBER
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Function : Set Data
        // Parm : BYTE * strKey		: KEY
        //        BYTE * strValue	: DATA
        // Return : Success(1), Failure(0 below)
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [DllImport("SmtSndRcvVCATDLL.dll", EntryPoint = "SMTSetData", CharSet = CharSet.Ansi)]
        public static extern int SMTSetData(String strKey, String strValue);

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Function : Get Data
        // Parm : int nCount		: Response Count(Default : 1)
        //		  BYTE * strKey		: KEY
        //        BYTE * strReturn	: Data For the key
        // Return : Success(Response Data Length), Failure(0 below)
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [DllImport("SmtSndRcvVCATDLL.dll", EntryPoint = "SMTGetData", CharSet = CharSet.Ansi)]
        public static extern int SMTGetData(int nCount, String strKey, StringBuilder strReturn);

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Function : Get Data
        // Parm : int nCount		: Response Count(Default : 1)
        //		  BYTE * strKey		: KEY
        // Return : Success(Data For the key), Failure(NULL)
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [DllImport("SmtSndRcvVCATDLL.dll", EntryPoint = "SMTGetDataRet", CharSet = CharSet.Ansi)]
        public static extern StringBuilder SMTGetDataRet(int nCount, String strKey);

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Function : State Get Data
        // Parm : BYTE * strReturn	: State message code
        // Return : Success(Response Data Length), Failure(0 below)
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [DllImport("SmtSndRcvVCATDLL.dll", EntryPoint = "SMTStateGetData", CharSet = CharSet.Ansi)]
        public static extern int SMTStateGetData(StringBuilder strReturn);

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Function : State Get Data
        // Parm : 
        // Return : Success(Data For the key), Failure(NULL)
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [DllImport("SmtSndRcvVCATDLL.dll", EntryPoint = "SMTStateGetDataRet", CharSet = CharSet.Ansi)]
        public static extern StringBuilder SMTStateGetDataRet();

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Function : Send Data (ASYNC)
        // Parm : int nKeyType		: 1: NUMBER, 2:NAME
        //		  BYTE * szIP		: Connect IP Address
        //		  int nPort			: Connect PORT
        //		  int iTimeout		: Timeout
        //		  HWND hwnd			: ASYNC EVENT WINDOWS ID
        // Return : Success(1 more), Failure(0 below)
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [DllImport("SmtSndRcvVCATDLL.dll", EntryPoint = "SMTTcpSend", CharSet = CharSet.Ansi)]
        public static extern int SMTTcpSend(int nKeyType, String szIP, int nPort, int iTimeout, IntPtr hWnd);

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Function : Send & Rcv (SYNC)
        // Parm : int nKeyType		: 1: NUMBER, 2:NAME
        //		  BYTE * szIP		: Connect IP Address
        //		  int nPort			: Connect PORT
        //		  int iTimeout		: Timeout
        // Return : Success(Count(1 over)), Failure(0 below)
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [DllImport("SmtSndRcvVCATDLL.dll", EntryPoint = "SMTTcpSendRcv", CharSet = CharSet.Ansi)]
        public static extern int SMTTcpSendRcv(int nKeyType, String szIP, int nPort, int iTimeout);

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Function : Error Code(ASYNC 시 사용)
        // Parm : 
        // Return : Success(Count(1 over)), Failure(0 이하)
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [DllImport("SmtSndRcvVCATDLL.dll", EntryPoint = "GetExitErrorCode", CharSet = CharSet.Ansi)]
        public static extern int GetExitErrorCode();

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Function : 서명 암호화
        // Parm : BYTE * ucaWorkKey		: Working Key
        //		  BYTE * ucIndex		: Master Key Index
        //        BYTE * ucaBmpPath		: BMP Full Path
        //		  BYTE * ucpHashData	: Hash Data
        //		  BYTE * ucpEncSignData	: Encrypt Sign Data
        // Return : Success(Encrypt Sign Data Length), Failure(0 below)
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [DllImport("SmtSndRcvVCATDLL.dll", EntryPoint = "BmpToEncSign", CharSet = CharSet.Ansi)]
        public static extern int BmpToEncSign(String ucaWorkKey, String ucIndex, String ucaBmpPath, StringBuilder ucpHashData, StringBuilder ucpEncSignData);

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Function : BASE 64 Decode
        // Parm : BYTE * strSource		: Encoding Data
        //		  int iLength			: Encoding Data Length
        //        BYTE * strDecodeData	: Decode Data
        // Return : Success(1), Failure(0 below)
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [DllImport("SmtSndRcvVCATDLL.dll", EntryPoint = "DecodeBASE64", CharSet = CharSet.Ansi)]
        public static extern int DecodeBASE64(String strSource, int iLength, StringBuilder strDecodeData);

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Function : BASE 64 Decode
        // Parm : BYTE * strSource		: Encoding Data
        //		  int iLength			: Encoding Data Length
        //        BYTE * strFilePath	: BMP FULL PATH
        // Return : Success(1), Failure( 0 below)
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [DllImport("SmtSndRcvVCATDLL.dll", EntryPoint = "DecodeBASE64Bmp", CharSet = CharSet.Ansi)]
        public static extern int DecodeBASE64Bmp(String strSource, int iLength, String strFilePath);

        //=========================================================================================
        // Define
        //=========================================================================================
        public const int LOG_START = 1;
        public const int LOG_END = 2;
        public const int LOG_DATA = 3;
        public const int LOG_RES_DATA = 4;

        // Main Proc Event
        public const int EVENT_ON_COMPLETE = 0x400 + 10;
        public const int EVENT_ON_EXIT = 0x400 + 12;
        public const int EVEMT_ON_STATE = 0x400 + 14;

        public const int TRADE_APP = 1;	// TRADE APPROVAL
        public const int TRADE_APP_CAN = 2;	// TRADE APPROVAL
        public const int TRADE_VCAT_INFO = 3;	// VCAT INFO
        public const int TRADE_LINK_CONFIRM = 4;	// LINKED CONFRIM
        public const int TRADE_TRADE_INIT = 5;	// TRADE INIT

        ////////////////////////////////////////
        // ITEM LIST
        ////////////////////////////////////////
        public enum MSG_ITME_ENUM
        {
            ITEM_BCC,									/* "BCC" */
            ITEM_DDC_CODE,								/* "DDC 코드" */
            ITEM_TELEGRAM_ETX,							/* "ETX" */
            ITEM_FILLER1,								/* "FILLER1" */
            ITEM_FILLER2,								/* "FILLER2" */
            ITEM_TELEGRAM_FS,							/* "FS" */
            ITEM_MASTERKEY_IDX,						    /* "MASTERKEY INDEX" */
            ITEM_TELEGRAM_STX,							/* "STX" */
            ITEM_SAM_ID,								/* "SAM ID" */
            ITEM_SW_VERSION,							/* "S/W 버전" */
            ITEM_VERSION_INFO,							/* "Version 정보" */
            ITEM_WORKING_KEY,							/* "WORKING KEY" */
            ITEM_FRANCHISE_INFO,						/* "가맹점 정보" */
            ITEM_FRANCHISE_NAME,						/* "가맹점명" */
            ITEM_FRANCHISE_ID,							/* "가맹점번호" */
            ITEM_FRANCHISE_TELEPHONE,					/* "가맹점전화" */
            ITEM_FRANCHISE_ADDRESS,					    /* "가맹점주소" */
            ITEM_AVAILABLE_SCORES,						/* "가용점수" */
            ITEM_TRADE_UNIQUE_ID,						/* "거래고유번호" */
            ITEM_TRADE_UNIQUE_ID_13,					/* "거래고유번호" */
            ITEM_TRADE_SEPARATE_CODE,					/* "거래구분코드" */
            ITEM_DEAL_DATE_TIME,						/* "거래일시" */
            ITEM_AFTER_TRADE_BALANCE,					/* "거래후잔액" */
            ITEM_PAYMENT_DIVISION,						/* "결제구분" */
            ITEM_ADD_TAX_AMOUNT,						/* "과세금액" */
            ITEM_MEACHINE_SERIAL_NUMBER,				/* "기기 일련번호" */
            ITEM_RANDOM_NUMBER,						    /* "난수" */
            ITEM_ACCRUE_SCORES,						    /* "누적점수" */
            ITEM_TERMINAL_NUMBER,						/* "단말기 번호" */
            ITEM_TERMINAL_VERSION,						/* "단말기버젼" */
            ITEM_PERSON_NAME,							/* "담당자" */
            ITEM_REPRESENTATIVE_PERSON,				    /* "대표자명" */
            ITEM_PURCHASE,								/* "매입사" */
            ITEM_PURCHASE_NAME,						    /* "매입사명" */
            ITEM_SALES_TIME,							/* "매출시간" */
            ITEM_SALES_DATE,							/* "매출일자" */
            ITEM_HEADER,								/* "머리말" */
            ITEM_MISS_PAGE_COUNT,						/* "미출력 페이지수" */
            ITEM_ISSUE,								    /* "발급사" */
            ITEM_ISSUE_NAME,							/* "발급사명" */
            ITEM_OCCUR_SCORES,							/* "발생점수" */
            ITEM_BONUS_PURCHASE,						/* "보너스 매입사" */
            ITEM_BONUS_ISSUE,							/* "보너스 발급사" */
            ITEM_BONUS_USED_SEPARATE,					/* "보너스 사용구분" */
            ITEM_BONUS_APPROVAL_ID,					    /* "보너스 승인번호" */
            ITEM_BONUS_RESPONSE_CODE,					/* "보너스 응답코드" */
            ITEM_BONUS_INFO,							/* "보너스 정보" */
            ITEM_BONUS_OUTPUT_MSG,						/* "보너스 출력메세지" */
            ITEM_BONUS_SEPARATE,						/* "보너스구분" */
            ITEM_BONUS_CARD_NUMBER,					    /* "보너스카드번호" */
            ITEM_SERVICE_CHARGE,						/* "봉사료" */
            ITEM_ADD_INFO_SEPARATE,					    /* "부가정보 구분" */
            ITEM_PASSWORD,								/* "비밀번호" */
            ITEM_BUSINESS_NUMBER,						/* "사업자번호" */
            ITEM_SIGN_IMAGE_DATA,						/* "사인이미지데이터" */
            ITEM_SIGNPAD_ID,							/* "사인패드기기정보" */
            ITEM_GOOD_INFO,							    /* "상품정보" */
            ITEM_SIGN_SET,								/* "서명여부" */
            ITEM_SIGN_IMAGE_INFO,						/* "서명이미지정보" */
            ITEM_SERVER_TRADE_DATE,					    /* "서버거래일시" */
            ITEM_SERVICE_SEPARATE,						/* "서비스 구분" */
            ITEM_SERVICE_TYPE,							/* "서비스 유형" */
            ITEM_TAX,									/* "세금" */
            ITEM_APPROVAL_AMOUNT,						/* "승인금액" */
            ITEM_APPROVAL_ID,							/* "승인번호" */
            ITEM_BUSINESS_DIVISION,					    /* "업무구분" */
            ITEM_RECEIPT_PRINT_TYPE,					/* "영수증인쇄유형" */
            ITEM_SALE_DATA_POS_NO,						/* "영업일자 및 POS No" */
            ITEM_REQUEST_NUMBER,						/* "요청번호" */
            ITEM_REQUEST_CODE,							/* "요청코드" */
            ITEM_BASE_TRADE_DATE,						/* "원거래일자" */
            ITEM_EXPIRY_DATE,							/* "유효기간" */
            ITEM_RESPONSE_CODE,						    /* "응답코드" */
            ITEM_IMAGE_DATA,							/* "이미지 데이터" */
            ITEM_PRINT_MSG,							    /* "인쇄메세지" */
            ITEM_SAVE_INDEX,							/* "저장 된 INDEX" */
            ITEM_IMAGE_NAME,							/* "이미지 이름" */
            ITEM_BALANCE,								/* "잔액" */
            ITEM_TELEGRAM_SIZE,						    /* "전문 Size" */
            ITEM_RECEIPT_TITLE,						    /* "전표타이틀" */
            ITEM_INFO_CHANGE_WORK,						/* "정보변경업무" */
            ITEM_PAYMENT_LIST,							/* "정산내역" */
            ITEM_PAYMENT_TIME,							/* "정산시간" */
            ITEM_PAYMENT_DATE,							/* "정산일자 " */
            ITEM_NORMAL_CHECK_RESPONSE_TIME,			/* "정상수표 응답시간" */
            ITEM_TOTAL_BUY_AMOUNT_AND,					/* "총 매출 금액외" */
            ITEM_WITHDRAWAL_AMOUNT,					    /* "출금가능액" */
            ITEM_OUTPUT_DATA,							/* "출력데이터" */
            ITEM_OUTPUT_MSG,							/* "출력메세지" */
            ITEM_CASH_CANCEL_REASON,					/* "취소 사유" */
            ITEM_CARD_NUMBER,							/* "카드번호" */
            ITEM_COUPON_NUMBER,						    /* "쿠폰번호" */
            ITEM_ITEM_NAME,							    /* "품목명" */
            ITEM_INSTALLMENT_PERIOD_USED_CHECK,		    /* "할부개월 및 사용구분" */
            ITEM_INSTALLMENT_PERIOD,					/* "할부개월" */
            ITEM_HASH_CODE,							    /* "해쉬코드" */
            ITEM_CASH_APPROVAL_TYPE,					/* "현금승인유형" */
            ITEM_DISPLAY_MSG_1,						    /* "화면메세지1" */
            ITEM_DISPLAY_MSG_2,						    /* "화면메세지2" */
            ITEM_DISPLAY_MSG_3,						    /* "화면메세지3" */
            ITEM_DISPLAY_MSG,							/* "화면메세지" */
            ITEM_STATE_CODE,							/* "상태 코드" */
            ITEM_OCCUR_SCORES_BEFORE,					/* "발생전점수" */
            ITEM_CHECK_SEPARATE,						/* "수표구분" */
            ITEM_CHECK_INFO,							/* "수표정보" */
            ITEM_CHECK_DATE,							/* "수표발생일자" */
            ITEM_CHECK_CODE,							/* "권종코드" */
            ITEM_CHECK_AMOUNT,							/* "수표금액" */
            ITEM_CAT_PORT,								/* "단말기포트" */
            ITEM_CAT_BPS,								/* "단말기속도" */
            ITEM_SIGNPAD_TYPE,							/* "사인패드 유형" */
            ITEM_SIGNPAD_PORT,							/* "사인패드포트" */
            ITEM_SIGNPAD_BPS,							/* "사인패드속도" */
            ITEM_OPERATION_METHOD,						/* "운영방식" */
            ITEM_LINKED_TRADE_DISPLAY,					/* "연동승인거래화면" */
            ITEM_BARCODE_GOODS_NUMBER,					/* "바코드번호" */
            ITEM_CURRENCY_CODE,						    /* "통화코드" */
            ITEM_STORE_CODE,							/* "점포코드" */
            ITEM_BUSINESS_INFO_01,						/* "사업자#1" */
            ITEM_BUSINESS_INFO_02,						/* "사업자#2" */
            ITEM_BUSINESS_INFO_03,						/* "사업자#3" */
            ITEM_BUSINESS_INFO_04,						/* "사업자#4" */
            ITEM_BUSINESS_INFO_05,						/* "사업자#5" */
            ITEM_BUSINESS_INFO_06,						/* "사업자#6" */
            ITEM_BUSINESS_INFO_07,						/* "사업자#7" */
            ITEM_BUSINESS_INFO_08,						/* "사업자#8" */
            ITEM_BUSINESS_INFO_09,						/* "사업자#9" */
            ITEM_BUSINESS_INFO_10,						/* "사업자#10" */
        };

        public struct _ST_DATA__
        {
            public int nID1;
            public String strNumber1;
        }

        public struct _ST_DATA_
        {
            public int nID1;
            public String strNumber1;

            public _ST_DATA_(int nID, String strNumber)
            {
                this.nID1 = nID;
                this.strNumber1 = strNumber;
            }

            public _ST_DATA_(MSG_ITME_ENUM nID, String strNumber)
            {
                this.nID1 = (int)nID;
                this.strNumber1 = strNumber;
            }

            public int nID
            {
                get
                {
                    return this.nID1;
                }
                set
                {
                    this.nID1 = value;
                }
            }

            public String strNumber
            {
                get
                {
                    return this.strNumber1;
                }
                set
                {
                    this.strNumber1 = value;
                }
            }
        };
    }
}
