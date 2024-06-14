/********************************************************************************************
 * Project Name - ConcurrentManager
 * Description  - ConcLib to send message
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.70.02     03-Nov-2019      Guru S A       Waiver phase 2 changes
 *2.90.0      23-Jul-2020      Jinto Thomas   Modified sendemail(),sendSms() methods, Checking messagingclient is 
 *                                             set or not. if set calling SendEMailSynchronous/SendSMSSynchronous with client information
 *2.100.0     15-Sep-2020      Nitin Pai      Push Notification: Send Push Notification Message                                             
 *********************************************************************************************/
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Messaging;
using System.Text.RegularExpressions;
using System.IO;
using System.ComponentModel;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Communication;
using System.Globalization;
using System.Diagnostics;
using Semnox.Parafait.Customer;

namespace Semnox.Parafait.ConcurrentManager
{
    public partial class ConcLib
    {
        private static Dictionary<int, MessagingClientDTO> messagingClientDictionary = new Dictionary<int, MessagingClientDTO>();
        public string SendMessage(int RequestId, string LogFileName)
        {
            log.LogMethodEntry(RequestId, LogFileName);
            
            // Uncomment to Debug the service
            //Debugger.Launch();
            try
            {
                _requestId = RequestId;
                _logFileName = LogFileName;

                MessageQueue messageQueue = MessageQueueUtils.GetMessageQueue(RequestId);

                System.Threading.ThreadStart thr = delegate
                {
                    while (true)
                    {
                        System.Messaging.Message msg = messageQueue.Receive();

                        msg.Formatter = new XmlMessageFormatter(new String[] { "System.String,mscorlib" });
                        QueueMessage = msg.Body.ToString();
                        if (QueueMessage.Equals("SHUTDOWN"))
                            break;
                    }
                };

                System.Threading.Thread th = new System.Threading.Thread(new System.Threading.ThreadStart(thr));
                th.Start();

                int loopCount = 6;
                
                while (QueueMessage.Equals("SHUTDOWN") == false)
                {
                    System.Windows.Forms.Application.DoEvents();

                    if (loopCount++ < 6)
                    {
                        Thread.Sleep(5 * 1000);
                        continue;
                    }
                    loopCount = 0;

                    DataTable dtCleanUpOldWaiverAttachments = _utilities.executeDataTable(@"select * 
                                                                                        from MessagingRequests 
                                                                                        where SendDate is not null
                                                                                         and attachFile is not null
                                                                                         and Reference = 'Waiver'
                                                                                         and SendDate < DATEADD(MINUTE, -30, getdate())
                                                                                         and SendDate >= DATEADD(hour, -4, getdate())
                                                                                         and isnull(Status, 'x') = 'Success' 
                                                                                        order by Id asc");
                    if(dtCleanUpOldWaiverAttachments.Rows.Count > 0)
                    {
                        foreach (DataRow oldMsgRequestEntry in dtCleanUpOldWaiverAttachments.Rows)
                        {
                            string attachmentFileName = string.Empty;
                            try
                            {
                                attachmentFileName = oldMsgRequestEntry["attachFile"].ToString();
                                if (string.IsNullOrEmpty(attachmentFileName) == false)
                                {
                                    log.LogVariableState("Attachment file being deleted is" , attachmentFileName);
                                    FileInfo file = new FileInfo(attachmentFileName);
                                    if (file.Exists)
                                    { 
                                        file.Delete();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {                                
                                log.Error("Unable to delete file: "+ attachmentFileName, ex);
                            }
                        }
                    }

                    MessagingRequestListBL messagingRequestListBL = new MessagingRequestListBL(_utilities.ExecutionContext);
                    List<KeyValuePair<MessagingRequestDTO.SearchByParameters, string>> searchParametrs = new List<KeyValuePair<MessagingRequestDTO.SearchByParameters, string>>();
                    searchParametrs.Add(new KeyValuePair<MessagingRequestDTO.SearchByParameters, string>(MessagingRequestDTO.SearchByParameters.ATTEMPT_LESS_THAN, "4"));
                    searchParametrs.Add(new KeyValuePair<MessagingRequestDTO.SearchByParameters, string>(MessagingRequestDTO.SearchByParameters.STATUS_NOT_EQ, "Success,Sending"));
                    searchParametrs.Add(new KeyValuePair<MessagingRequestDTO.SearchByParameters, string>(MessagingRequestDTO.SearchByParameters.SEND_DATE, ""));
                    List<MessagingRequestDTO> messagingRequestDTOList = messagingRequestListBL.GetAllMessagingRequestList(searchParametrs, 0, 100);
                    if(messagingRequestDTOList != null && messagingRequestDTOList.Any())
                    {
                        foreach (var messagingRequestDTO in messagingRequestDTOList)
                        {
                            messagingRequestDTO.Status = "Sending";
                        }

                        MessagingRequestListBL tempMessagingRequestListBL = new MessagingRequestListBL(_utilities.ExecutionContext, messagingRequestDTOList);
                        tempMessagingRequestListBL.SaveMessagingRequest();

                        messagingRequestDTOList = messagingRequestDTOList.OrderBy(x => x.Id).ToList();
                        foreach (var messagingRequestDTO in messagingRequestDTOList)
                        {
                            try
                            {
                                if(messagingRequestDTO.MessagingClientId == -1)
                                {
                                    MessagingClientListBL messagingClientListBL = new MessagingClientListBL(_utilities.ExecutionContext);
                                    List<KeyValuePair<MessagingClientDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<MessagingClientDTO.SearchByParameters, string>>();
                                    searchParameters.Add(new KeyValuePair<MessagingClientDTO.SearchByParameters, string>(MessagingClientDTO.SearchByParameters.IS_ACTIVE, "1"));

                                    searchParameters.Add(new KeyValuePair<MessagingClientDTO.SearchByParameters, string>(MessagingClientDTO.SearchByParameters.MESSAGING_CHANNEL_CODE, messagingRequestDTO.MessageType));
                                    List<MessagingClientDTO> messagingClientDTOList = messagingClientListBL.GetMessagingClientDTOList(searchParameters);
                                    if (messagingClientDTOList != null && messagingClientDTOList.Any())
                                    {
                                        messagingRequestDTO.MessagingClientId = messagingClientDTOList[0].ClientId;
                                    }
                                    else
                                    {
                                        throw new ValidationException("Messaging client not setup.");
                                    }
                                }
                                
                                MessagingClientBL messagingClient = MessagingClientFactory.GetInstance(_utilities.ExecutionContext).GetMessageClient(messagingRequestDTO.MessagingClientId);

                                if (messagingClient != null)
                                {
                                    bool whatsAppEnabled;
                                    bool OptOutWhatsApp;
                                    if (messagingRequestDTO.MessageType == "W")
                                    {
                                        CustomerBL customerBL = new CustomerBL(_executionContext, messagingRequestDTO.CustomerId);
                                        int index = customerBL.CustomerDTO.ContactDTOList.FindIndex(x => x.Attribute1 == messagingRequestDTO.ToMobile);
                                        whatsAppEnabled = (index == -1 ? false : customerBL.CustomerDTO.ContactDTOList[index].WhatsAppEnabled);
                                        OptOutWhatsApp = customerBL.CustomerDTO.ProfileDTO.OptOutWhatsApp;
                                        if (whatsAppEnabled)
                                        {
                                            messagingClient.Send(messagingRequestDTO);
                                            if(!messagingRequestDTO.Status.Equals("Success"))
                                            {
                                                whatsAppEnabled = false;
                                            }
                                           
                                            MessagingRequestBL messagingRequestBL = new MessagingRequestBL(_utilities.ExecutionContext, messagingRequestDTO);
                                            messagingRequestBL.Save();
                                            if (!whatsAppEnabled)
                                            {
                                                customerBL.CustomerDTO.ContactDTOList[index].WhatsAppEnabled = false;
                                                try
                                                {
                                                    customerBL.Save(null);
                                                }
                                                catch (Exception ex)
                                                {
                                                    log.Error(ex);
                                                    throw;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            messagingClient.Send(messagingRequestDTO);
                                            if (messagingRequestDTO.Status.Equals("Success"))
                                            {
                                                whatsAppEnabled = true;
                                            }
                                            MessagingRequestBL messagingRequestBL = new MessagingRequestBL(_utilities.ExecutionContext, messagingRequestDTO);
                                            messagingRequestBL.Save();

                                            if (whatsAppEnabled)
                                            {
                                                customerBL.CustomerDTO.ContactDTOList[index].WhatsAppEnabled = true;
                                                try
                                                {
                                                    customerBL.Save(null);
                                                }
                                                catch (Exception ex)
                                                {
                                                    log.Error(ex);
                                                    throw;
                                                }
                                            }
                                                                                  
                                        }
                                    }
                                    else
                                    {
                                        messagingClient.Send(messagingRequestDTO);
                                        MessagingRequestBL messagingRequestBL = new MessagingRequestBL(_utilities.ExecutionContext, messagingRequestDTO);
                                        messagingRequestBL.Save();
                                    }
                                    
                                }
                                else
                                {
                                    throw new Exception("Wrong messaging client set up");
                                }
                                
                            }
                            catch (Exception ex)
                            {
                                log.Error("Error occured while processing MessagingRequests", ex);
                                log.LogMethodExit(ex.Message);
                                writeToLog(ex.Message);
                                _utilities.EventLog.logEvent("SendMessage", 'E', "Error occured while processing MessagingRequest: " + ex.Message, "SendMessageErrroLog", "SendMessage", 0);
                                messagingRequestDTO.Status = "Error";
                                if (messagingRequestDTO.Attempts == null)
                                {
                                    messagingRequestDTO.Attempts = 0;
                                }
                                messagingRequestDTO.Attempts++;
                                messagingRequestDTO.StatusMessage = ex.Message.Substring(0, Math.Min(ex.Message.Length, 500));
                                MessagingRequestBL messagingRequestBL = new MessagingRequestBL(_utilities.ExecutionContext, messagingRequestDTO);
                                messagingRequestBL.Save();
                                //updateRequestRecord(messagingRequestDTO.Id, false, ex.Message);
                            }
                        }
                    }
                    
                }

                while (bgEmailList.Count > 0)
                    Thread.Sleep(500);

                while (bgPhoneList.Count > 0)
                    Thread.Sleep(500);

                while (bgDeviceList.Count > 0)
                    Thread.Sleep(500);

                log.LogMethodExit("Success");
                return "Success";
            }
            catch (Exception ex)
            {
                log.Error("Error occured when reading from Message Queue or when selecting values from MessagingRequests", ex);
                log.LogMethodExit(ex.Message);
                writeToLog(ex.Message);
                _utilities.EventLog.logEvent("SendMessage", 'E', "Error occured when reading from Message Queue or when selecting values from MessagingRequests: " + ex.Message, "SendMessageErrroLog", "SendMessage", 0);
                return ex.Message;
            }
        }

       
        

        //void bw_DoNotificationWork(object sender, DoWorkEventArgs e)
        //{
        //    log.LogMethodEntry(sender, e);

        //    while (bgDeviceList.Count >= BATCHSIZE)
        //    {
        //        Thread.Sleep(1000);
        //    }
        //    BackgroundWorker bg = sender as BackgroundWorker;

        //    lock (bgDeviceList)
        //    {
        //        bgDeviceList.Add(bg);
        //    }

        //    object token = "";
        //    try
        //    {
        //        token = (e.Argument as object[])[0].ToString();
        //        string result = sendNotification(e.Argument);
        //        updateRequestRecord((int)((e.Argument as object[])[2]), true, result);
        //        e.Result = result;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("Sending Notification Unsuccessful!", ex);
        //        e.Result = ex.Message;
        //        _utilities.EventLog.logEvent("SendMessage", 'A', "Error occured while sending Notification [" + token.ToString() + "]: " + ex.Message, "SendMessageErrroLog", "SendNotification", 0);

        //        updateRequestRecord((int)((e.Argument as object[])[2]), false, ex.Message);
        //    }

        //    bgDeviceList.Remove(bg);

        //    log.LogMethodExit(null);
        //}

        //string sendNotification(object o)
        //{
        //    log.LogMethodEntry(o);

        //    object[] param = o as object[];
        //    string returnValueNew = string.Empty;

        //    string toPhone = param[0].ToString();
        //    MessagingRequestDTO messagingRequestDTO = (MessagingRequestDTO)param[1];
        //    AppNotifications appNotifications = new AppNotifications(_utilities.ExecutionContext);
        //    returnValueNew = appNotifications.SendMessage(messagingRequestDTO);
        //    log.LogMethodExit(returnValueNew);
        //    return returnValueNew;
        //}

        //void _sendMessage(MessagingRequestDTO messagingRequestDTO)
        //{
        //    log.LogMethodEntry(messagingRequestDTO);

        //    RegexUtilities regex = new RegexUtilities();
        //    switch (messagingRequestDTO.MessageType)
        //    {
        //        case "E":
        //            {
        //                try
        //                {
        //                    string email = messagingRequestDTO.ToEmail;
        //                    if (regex.IsValidEmail(email) == false)
        //                    {
        //                        string status = "EML INV CUST" + messagingRequestDTO.CustomerId;
        //                        writeToLog(status);
        //                        updateAttemptStatus(messagingRequestDTO.Id, status);
        //                        //continue;
        //                    }
        //                    //else
        //                    //    // updateAttemptStatus(dr["id"]); //code before -07/07/2015
        //                    //    updateAttemptStatus(messagingRequestDTO.Id, "Sending"); //success also is passed to this function (in the case of email)
        //                    // Once the records where status<>'Success' are retrieved, update the status field to 'Success'.
        //                    //this is to avoid the following situation:- in case if the mail is being sent (but process is not completed), 
        //                    //and the status has not got updated as 'Success'-the next process
        //                    //may fetch the same records again and the mail sending will be attempted for the same records.This results in 
        //                    // sending of mails multiple times for same records. -07/07/2015
        //                    // Updated to sending to handle the issue
        //                    else
        //                    {
        //                        BackgroundWorker bg = new BackgroundWorker();
        //                        bg.DoWork += new DoWorkEventHandler(bw_DoWork);
        //                        //bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
        //                        bg.RunWorkerAsync(new object[] { email, messagingRequestDTO.Subject, messagingRequestDTO.Body, messagingRequestDTO.Id,
        //                                            messagingRequestDTO.AttachFile, messagingRequestDTO.Cc, messagingRequestDTO.Bcc, messagingRequestDTO.MessagingClientId });
        //                    }
                            
        //                }
        //                catch (Exception ex)
        //                {
        //                    writeToLog("Send email error" + ex.Message);
        //                }

        //                //Thread.Sleep(100); //the line was commented on 09/07/2015
        //                //Thread.Sleep(15000); // the line was added on 09/072015
        //                Thread.Sleep(100); //the line was commented on 09/07/2015
        //                break;

        //            }
        //        case "S":
        //            {
        //                try
        //                {
        //                    string phone = messagingRequestDTO.ToMobile;
        //                    if (string.IsNullOrEmpty(phone.Trim()) || phone.Length < 8)
        //                    {
        //                        string status = "PH INV CUST" + messagingRequestDTO.CustomerId;
        //                        writeToLog(status);
        //                        updateAttemptStatus(messagingRequestDTO.Id, status);
        //                        //continue;
        //                    }
        //                    //else
        //                    //    updateAttemptStatus(messagingRequestDTO.Id);
        //                    else
        //                    {
        //                        BackgroundWorker bg = new BackgroundWorker();
        //                        bg.DoWork += new DoWorkEventHandler(bw_DoSMSWork);
        //                        //bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
        //                        bg.RunWorkerAsync(new object[] { phone, messagingRequestDTO.Body, messagingRequestDTO.Id, messagingRequestDTO.MessagingClientId });
        //                    }
                            
        //                }
        //                catch (Exception ex)
        //                {
        //                    writeToLog("Send sms error" + ex.Message);
        //                }

        //                Thread.Sleep(100);
        //                break;

        //            }
        //        case "A":
        //            {
        //                try
        //                {
        //                    string token = messagingRequestDTO.ToDevice;
        //                    if (string.IsNullOrEmpty(token))
        //                    {
        //                        string status = "TKN INV CUSTID" + messagingRequestDTO.CustomerId;
        //                        writeToLog(status);
        //                        updateAttemptStatus(messagingRequestDTO.Id, status);
        //                        //continue;
        //                    }
        //                    //else
        //                    //    updateAttemptStatus(messagingRequestDTO.Id, "Sending");
        //                    else
        //                    {
        //                        BackgroundWorker bg = new BackgroundWorker();
        //                        bg.DoWork += new DoWorkEventHandler(bw_DoNotificationWork);
        //                        //bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
        //                        bg.RunWorkerAsync(new object[] { token, messagingRequestDTO, messagingRequestDTO.Id, messagingRequestDTO.MessagingClientId });
        //                    }
                           
        //                }
        //                catch (Exception ex)
        //                {
        //                    writeToLog("Send notification error" + ex.Message);
        //                }

        //                // Wait time
        //                Thread.Sleep(100);
        //                break;

        //            }
        //    }

        //    log.LogMethodExit();
        //}
    }
}
