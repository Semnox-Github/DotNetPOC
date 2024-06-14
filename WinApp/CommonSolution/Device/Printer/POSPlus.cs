/********************************************************************************************
 * Project Name - POSPlus
 * Description  - This is the core class which contains all the function of POSPlus device.
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        06-May-2016   Raghuveera          Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
 using Semnox.Core.Utilities;
using System.Net.Sockets;
using System.IO;

namespace Semnox.Parafait.Device.Printer
{
    public class POSPlus
    {
        POSPlusRequest pOSPlusRequest;        
        public string ResponseCode;
        //public string Method;
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Utilities utilities;
        string ipAddress;
        int portNo;
        TcpClient tcpclnt = new TcpClient();
       
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="_utilities"> utilities.</param>
        public POSPlus(Utilities _utilities)
        {
            log.LogMethodEntry(_utilities);
            utilities = _utilities;
            portNo = Convert.ToInt32(utilities.getParafaitDefaults("FISCAL_PRINTER_PORT_NUMBER"));
            ipAddress = utilities.getParafaitDefaults("FISCAL_DEVICE_TCP/IP_ADDRESS");
            tcpclnt.Client.ReceiveTimeout = 20000;
            tcpclnt.Connect(ipAddress, portNo);
            log.LogMethodExit(null);
        }
        ~POSPlus()
        {
            log.LogMethodEntry();
            tcpclnt.Close();
            log.LogMethodExit(null);
        }
        /// <summary>
        /// Posting the commands to the device 
        /// </summary>
        /// <returns>control code in string format </returns>
        public string PostTransaction(POSPlusRequest _POSPlusRequest, ref string message)
        {
            log.LogMethodEntry(_POSPlusRequest, message);
            string commandString = "";
            string response = "";
            string[] responseArray;
            try
            {
                pOSPlusRequest = _POSPlusRequest;
                if (pOSPlusRequest != null)
                {
                    response = SendCommand("Open " + pOSPlusRequest.SerialNo.ToString() + "\r\n", ref message);
                    if (!string.IsNullOrEmpty(response))
                    {
                        responseArray = response.Split(' ');
                        if (responseArray.Length > 0)
                        {
                            if (!GetOpenCommandResponse(responseArray[0], ref message))
                            {
                                log.LogVariableState("message", message);
                                log.LogMethodExit("");
                                return "";
                            }
                        }
                        else
                        {
                            message += "POSPlus invalid response!!..";
                            log.LogVariableState("message", message);
                            log.LogMethodExit("");
                            return "";
                        }
                        commandString = CreateRequestString(ref message);
                        if (!string.IsNullOrEmpty(commandString))
                        {
                            commandString = "kd " + commandString+"\n\r";
                            response = SendCommand(commandString, ref message);
                            if (!string.IsNullOrEmpty(response))
                            {
                                string returnvalue= GetControlCode(response, ref message);
                                log.LogVariableState("message", message);
                                log.LogMethodExit(returnvalue);
                                return (returnvalue);
                            }
                        }
                    }
                }
                else
                {
                    message = "POSPlus request object is null.";
                }
                log.LogVariableState("message", message);
                log.LogMethodExit("");
                return "";
            }
            catch (Exception e)
            {
                log.Error("Error occured because of Post transaction", e);
                message = e.ToString();
                log.LogVariableState("message", message);
                log.LogMethodExit("");
                return "";
            }
     
        }

        private string GetControlCode(string ResponseString, ref string message)
        {
            log.LogMethodEntry(ResponseString, message);
            string ControlCode = "";
            string[] responseArray = ResponseString.Split(' ');
            if (responseArray.Length > 0)
            {
                ResponseCode = responseArray[0];
                ControlCode = responseArray[1];
                GenerateResponse(ResponseCode, ref message);
            }
            log.LogVariableState("message", message);
            log.LogMethodExit(ControlCode);
            return ControlCode;
        }
        private void GenerateResponse(string ResponseCode, ref string message)
        {
            log.LogMethodEntry(ResponseCode, message);
            switch (ResponseCode)
            {
                case "-3": message = "POSPlus unknown command"; break;
                case "-2": message = "POSPlus CRC error"; break;
                case "-1": message = "POSPlus wrong length"; break;
                case "0": message = "OK"; break;
                case "1": message = "POSPlus wrong number of arguments"; break;
                case "2": message = "POSPlus wrong date/time range"; break;
                case "3": message = "POSPlus wrong format of organisation number"; break;
                case "4": message = "POSPlus wrong format of cash register id"; break;
                case "5": message = "POSPlus wrong format of serial number"; break;
                case "6":
                case "7": message = "POSPlus type of receipt not defined"; break;
                case "8": message = "POSPlus wrong format of return amount"; break;
                case "9": message = "POSPlus wrong format of sales amount"; break;
                case "10":
                case "11":
                case "12":
                case "13": message = "POSPlus wrong format of vat"; break;
                case "14":
                case "15":
                case "16":
                case "17":
                case "18":
                case "19": message = "POSPlus internal error in the control unit"; break;
                case "20": message = "POSPlus power fail abort"; break;
                case "21": message = "POSPlus relationship between sales amount and return amount is wrong"; break;
                case "22": message = "POSPlus field is present after field CRC"; break;
                case "23": message = "POSPlus error in an internal counter"; break;
                case "24": message = "POSPlus internal log is full"; break;
            }
            log.LogVariableState("message", message);
            log.LogMethodExit(null);
        }
        private bool GetOpenCommandResponse(string responseCode, ref string message)
        {
            log.LogMethodEntry(responseCode, message);
            switch (responseCode)
            {
                case "100":
                    {
                        message = "successful";
                        log.LogVariableState("message", message);
                        log.LogMethodExit(true);
                        return true;
                    }
                case "-100":
                    {
                        message = "POSPlus internal server error";
                        log.LogVariableState("message", message);
                        log.LogMethodExit(false);
                        return false;
                    }
                case "-99":
                    {
                        message = "POSPlus device is unavailable";
                        log.LogVariableState("message", message);
                        log.LogMethodExit(false);
                        return false;
                    }
                case "-98":
                    {
                        message = "POSPlus incorrect command";
                        log.LogVariableState("message", message);
                        log.LogMethodExit(false);
                        return false;
                    }
                case "-97":
                    {
                        message = "POSPlus previous request in work";
                        log.LogVariableState("message", message);
                        log.LogMethodExit(false);
                        return false;
                    }
                default:
                    {
                        log.LogVariableState("message", message);
                        log.LogMethodExit(false);
                        return false;
                    }
            }
        }

        private string CreateRequestString(ref string message)
        {
            log.LogMethodEntry(message);
            string requestString = "";

            StringBuilder requestStringBuild = new StringBuilder("");
            try
            {
                if (pOSPlusRequest.TransactionDate.Equals(DateTime.MinValue))
                {
                    message = "POSPlus: Transaction date is not set.";
                    log.LogVariableState("message", message);
                    log.LogMethodExit(requestString);
                    return requestString;
                }
                else
                {
                    requestStringBuild.Append(pOSPlusRequest.TransactionDate.ToString("yyyyMMddHHmm"));
                }
                if (pOSPlusRequest.OrganizationNumber <= 0)
                {
                    message = "POSPlus: Organization number is not set.";
                    log.LogVariableState("message", message);
                    log.LogMethodExit(requestString);
                    return requestString;
                }
                else
                {
                    requestStringBuild.Append(" " + pOSPlusRequest.OrganizationNumber);
                }
                if (string.IsNullOrEmpty(pOSPlusRequest.CashRegisterId))
                {
                    message = "POSPlus: Cash register id is not set.";
                    log.LogVariableState("message", message);
                    log.LogMethodExit(requestString);
                    return requestString;
                }
                else
                {
                    requestStringBuild.Append(" " + pOSPlusRequest.OrganizationNumber);
                }
                if (string.IsNullOrEmpty(pOSPlusRequest.CashRegisterId))
                {
                    message = "POSPlus: Cash register id is not set.";
                    log.LogVariableState("message", message);
                    log.LogMethodExit(requestString);
                    return requestString;
                }
                else
                {
                    requestStringBuild.Append(" " + pOSPlusRequest.CashRegisterId);
                }
                if (string.IsNullOrEmpty(pOSPlusRequest.SerialNo))
                {
                    message = "POSPlus: Serial no is not set.";
                    log.LogVariableState("message", message);
                    log.LogMethodExit(requestString);
                    return requestString;
                }
                else
                {
                    requestStringBuild.Append(" " + pOSPlusRequest.SerialNo);
                }
                if (string.IsNullOrEmpty(pOSPlusRequest.Type))
                {
                    message = "POSPlus: Type is not set.";
                    log.LogVariableState("message", message);
                    log.LogMethodExit(requestString);
                    return requestString;
                }
                else
                {
                    requestStringBuild.Append(" " + pOSPlusRequest.Type);
                }
                if (pOSPlusRequest.ReturnAmount >= 0 || pOSPlusRequest.SaleAmount >= 0)
                {
                    requestStringBuild.Append(" " + pOSPlusRequest.ReturnAmount.ToString("0.00").Replace('.', ','));
                    requestStringBuild.Append(" " + pOSPlusRequest.SaleAmount.ToString("0.00").Replace('.', ','));
                }
                else
                {
                    message = "POSPlus: Amount is not set.";
                    log.LogVariableState("message", message);
                    log.LogMethodExit(requestString); 
                    return requestString;
                }
                for (int i = 0; i < 4; i++)
                {
                    requestStringBuild.Append(" " + pOSPlusRequest.VATPercentage[i].ToString("0.00").Replace('.', ',') + ";" + pOSPlusRequest.VATAmount[i].ToString("0.00").Replace('.', ','));
                }

                if (string.IsNullOrEmpty(pOSPlusRequest.CRC))
                {
                    requestStringBuild.Append(" " + "0000");
                }
                else
                {
                    requestStringBuild.Append(" " + pOSPlusRequest.CRC);
                }
                requestString = requestStringBuild.ToString();
            }
            catch (Exception ex)
            {
                log.Error("Error occured because of create  request  string", ex);
                message = ex.ToString();
                requestString = "";
            }
            log.LogVariableState("message", message);
            log.LogMethodExit(requestString);
            return requestString;
        }
        private bool OpenCommand(long serialNo)
        {
            log.LogMethodEntry(serialNo);
            bool status = false;
            log.LogMethodExit(status);
            return status;
        }

        private string SendCommand(string requestString, ref string message)
        {
            log.LogMethodEntry(requestString, message);
            string responseString = "";
            try
            {                
                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] requestByteArray = asen.GetBytes(requestString);
                Stream stm = tcpclnt.GetStream();
                stm.Write(requestByteArray, 0, requestByteArray.Length);

                byte[] responseArray = new byte[2048];
                int k = stm.Read(responseArray, 0, 2048);

                for (int i = 0; i < k; i++)
                    responseString += Convert.ToChar(responseArray[i]).ToString();

                log.LogVariableState("message", message);
                log.LogMethodExit(responseString);
                return responseString;
            }

            catch (Exception e)
            {
                log.Error("Error occured during sending the command", e);
                message = e.Message;
                log.LogVariableState("message", message);
                log.LogMethodExit(""); 
                return "";
            }
        }
    }
}
