using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Messaging;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.ConcurrentManager
{
    public partial class ConcLib
    {
        public string WindowsEventLogger(int RequestId, string LogFileName)
        {
            log.LogMethodEntry(RequestId, LogFileName);

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

                int loopCount = 3;
                while (QueueMessage.Equals("SHUTDOWN") == false)
                {
                    System.Windows.Forms.Application.DoEvents();

                    if (loopCount++ < 3)
                    {
                        Thread.Sleep(5 * 1000);
                        continue;
                    }
                    loopCount = 0;

                    DateTime timeStamp = DateTime.Now;
                    DateTime lastSynchTime;
                    object oLastSynchTime = _utilities.executeScalar("select max(Data) from EventLog where Source = 'WindowsLogger' and Type = 'D'");
                    if (oLastSynchTime == DBNull.Value)
                    {
                        EventLog ev = new EventLog(_utilities);
                        lastSynchTime = DateTime.Now.AddDays(-1);
                        ev.logEvent("WindowsLogger", 'D', lastSynchTime.ToString("yyyy-MM-dd HH:mm:ss.ffff"), "WindowsEventLogger Synch time", "Logging", 0);
                    }
                    else
                    {
                        IFormatProvider culture = System.Globalization.CultureInfo.CurrentCulture;
                        lastSynchTime = DateTime.ParseExact(oLastSynchTime.ToString(), "yyyy-MM-dd HH:mm:ss.ffff", culture);
                    }

                    DataTable dtEvents = _utilities.executeDataTable(@"select * 
                                                                        from EventLog 
                                                                        where TimeStamp > @fromTime 
                                                                        and TimeStamp <= @toTimeStamp 
                                                                        and Category not in ('INACTIVETERMINAL')
                                                                        order by Timestamp",
                                                                        new SqlParameter("@fromTime", lastSynchTime),
                                                                        new SqlParameter("@toTimeStamp", timeStamp));

                    log.LogVariableState("@fromTime", lastSynchTime);
                    log.LogVariableState("@toTimeStamp", timeStamp);

                    using (WindowsEventLogger winLogger = new WindowsEventLogger())
                    {
                        foreach (DataRow dr in dtEvents.Rows)
                        {
                            System.Diagnostics.EventLogEntryType logType = System.Diagnostics.EventLogEntryType.Information;
                            switch (dr["Type"].ToString())
                            {
                                case "D":
                                case "I":
                                case "M": logType = System.Diagnostics.EventLogEntryType.Information; break;
                                case "E": logType = System.Diagnostics.EventLogEntryType.Error; break;
                                case "W": logType = System.Diagnostics.EventLogEntryType.Warning; break;
                                case "S": logType = System.Diagnostics.EventLogEntryType.SuccessAudit; break;
                                case "F": logType = System.Diagnostics.EventLogEntryType.FailureAudit; break;
                                default: logType = System.Diagnostics.EventLogEntryType.Information; break;
                            }
                            winLogger.WriteEvent(dr["Source"].ToString(),
                                            dr["Data"].ToString() + " - " + dr["Description"].ToString() + Environment.NewLine +
                                            Convert.ToDateTime(dr["TimeStamp"]).ToString("yyyy-MM-dd HH:mm:ss") + Environment.NewLine +
                                            "Computer: " + dr["Computer"].ToString() + Environment.NewLine +
                                            "User: " + dr["Username"].ToString() + Environment.NewLine +
                                            "Category: " + dr["Category"].ToString(),
                                            logType, Convert.ToInt32(dr["EventLogId"]));
                            writeToLog("Logging " + logType.ToString() + " event: " + dr["Data"].ToString());
                        }
                    }

                    _utilities.executeNonQuery("update EventLog set Data = @toTimeStamp where Source = 'WindowsLogger' and Type = 'D'",
                                                new SqlParameter("@toTimeStamp", timeStamp.ToString("yyyy-MM-dd HH:mm:ss.ffff")));

                    log.LogVariableState("@toTimeStamp", timeStamp.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                }

                log.LogMethodExit("Success");
                return "Success";
            }
            catch (Exception ex)
            {
                log.Error("Error occured when reading from Message Queue or selecting values from EventLog", ex);
                log.LogMethodExit(ex.Message);
                return ex.Message;
            }
        }
    }
}
