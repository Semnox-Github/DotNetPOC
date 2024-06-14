using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public static class Utils
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static readonly Dictionary<string, string> SCR200ReturnCodes = new Dictionary<string, string>
        {
            {"00", "Approved"},
            {"76", "Declined"},
            {"V0", "Protocol Version Mismatch"},
            {"V1", "Currency Mismatch"},
            {"V2", "Completion amount did not match authorised amount and AuthCompleteSame was set to 1"},
            {"V3", "Completion amount exceed authorised amount"},
            {"V4", "Invalid prompt ID"},
            {"V5", "Transmit message failed"},
            {"V6", "Card read error. Retry..."},
            {"V7", "Message is too long"},
            {"V8", "Maximum authorisation amount exceeded"},
            {"V9", "Unable to display message"},
            {"VA", "Busy"},
            {"VB", "Card read timeout"},
            {"VC", "Self-test failed"},
            {"VD", "Hardware Failure"},
            {"VE", "Not initialized (CFG+SETD is not done)"},
            {"VF", "Transaction Reference or Sequence Number Error"},
            {"VG", "Invalid object"},
            {"VH", "Invalid action"},
            {"VJ", "More than max outstanding voids / completions outstanding (8 outstanding transactions not settled with DPS internet servers)"},
            {"VK", "Invalid message (fields are malformed)"},
            {"VL", "Configuration update needed (communications with DPS host required)"},
            {"VM", "Stored Value Card Presented"},
            {"VN", "Non-Stored Value Card Presented"},
            {"VP", "CRC Error"},
            {"VQ", "Invalid Amount"},
            {"VR", "Invalid Merchant Reference"},
            {"VS", "Unsupported Baud Rate"},
            {"VT", "Unsupported Maximum Message Size"},
            {"VV", "ICC response data exceeds maximum message length"},
            {"VW", "Transaction cancelled"},
            {"DP", " SCR200 SERIAL COMMUNICATIONS: Version: 1.6.34 / v1.2.8.13 Firmware DRAFT"},
            {"VX", "Access to restricted card data prohibited"},
            {"VY", "Requested byte offset is larger than receipt, or line count would exceed buffer size"},
            {"VZ", "Cannot execute command – TXEN is not enabled"},
            {"W0", "Removal detected – financial transactions disabled (applies to SCR200 and SKP removal)"},
            {"W1", "Feature is disabled. This message is received when you attempt an operation which has not been enabled at DPS for this device"},
            {"W2", "ICC Declined. The ICC Card locally declined a transaction"},
            {"W3", "Data not available (e.g. When using GETT to retrieve a token that has not been retrieved, or is not configured on the DPS host)"},
            {"W5", "Stored Value Card state is unknown. The card needs to be represented to confirm the correct card state (with an SVE command)."},
            {"W6", "Stored Value Transaction could not be settled – POS should reverse the transaction with an SVR"},
            {"W7", "A previous stored value transaction failed due to power failure"},
            {"W8", "Wrong card presented (during an SVE)"},
            {"W9", "No error to recover from (during an SVE). This means that no value was deducted from the card"},
            {"WA", "Either a slot was provided where no multi-merchant facility is configured, or no slot was provided when a multi-merchant facility is configured"},
            {"WB", "Card is expired"},
            {"WC", "Transaction is declined because a PIN is required (but no PED is present)"},
            {"WD", "Invalid Card"},
            {"WE", "Card not allowed (the card was not represented in the card prefix table)"},
            {"WF", "Offline conditions exceeded."},
            {"WG", "No PIN pad (a PIN could not be entered)"},
            {"WH", "Security error. The device was tampered or grids are not enabled"},
            {"WI", "DeviceId does not match the required DeviceIdPrefix configured at the host"},
            {"WJ", "Transaction was offline, operation not allowed"},
            {"WK", "Root key not ready"},
            {"WL", "Session expired"},
            {"WM", "Session failed"},
            {"WN", "Insufficient storage space"},
            {"WO", "Wrong sequence"},
            {"WP", "Unknown MIFARE card"},
            {"WQ", "EMV Terminal Configuration UiCapability must be 0 for this function"},
            {"U9", "Timeout (No Response from Host)"}
        };
    
        public static string getReturnMessage(string returnCode)
        {
            log.LogMethodEntry(returnCode);
            if (SCR200ReturnCodes.ContainsKey(returnCode))
            {
                string returnvalue = SCR200ReturnCodes[returnCode];
                log.LogMethodExit(returnvalue);
                return returnvalue;
            }
            else
            {
                string returnvalue= "Unknown Return Code: " + returnCode;
                log.LogMethodExit(returnvalue);
                return returnvalue;
            }   
        }

        public static readonly Dictionary<string, string> SCR200TxnStates = new Dictionary<string, string>
        {
            {"0", "Idle/Ready"},
            {"1", "Authorization in progress"},
            {"2", "Authorization done"},
            {"3", "Reversal in progress"},
            {"4", "Complete in progress"},
            {"5", "Stored Value Purchase in progress"},
            {"6", "Stored Value Refund in progress"},
            {"7", "Reversal Completed"},
            {"8", "Complete Completed"},
            {"9", "Authorization Failed"},
            {"10", "Stored Value Purchase Complete"},
            {"11", "Stored Value Refund Complete"},
            {"12", "Cash Info Transaction Recorded"}
        };

        public static string getTxnState(string TxnCode)
        {
            log.LogMethodEntry(TxnCode);
            if (SCR200TxnStates.ContainsKey(TxnCode))
            {
                string returnvalue= SCR200TxnStates[TxnCode];
                log.LogMethodExit(returnvalue);
                return returnvalue;
            }
          
            else
            {
                log.LogMethodExit("");
                return "";
            }
        }

        public static bool IsReachableUrl(string url)
        {
            log.LogMethodEntry(url);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = 15000;
            request.Method = "HEAD"; // As per Lasse's comment
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    bool returnvalue = response.StatusCode == HttpStatusCode.OK;
                    log.LogMethodExit(returnvalue);
                    return returnvalue;
                }
            }
            catch(Exception ex)
            {
                log.Error("Error occured while processing the http request and responses",ex);
                log.LogMethodExit(false);
                return false;
            }
        }
    }
}
