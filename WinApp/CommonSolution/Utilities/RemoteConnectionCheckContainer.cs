/********************************************************************************************
 * Project Name - GenericUtilities
 * Description  - RemoteConnectionCheckContainer used to hold the latest ping time of the server 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Aug-2020       Amitha Joy                Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Timers;


namespace Semnox.Core.Utilities
{
    public enum ConnectionStatus
    {
        Online = 0,
        Offline = 1
    }

    public sealed class RemoteConnectionCheckContainer
    {
        private static readonly Parafait.logging.Logger log = new Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DateTime? lastCommunicatedTime = null;
        private static readonly object locker = new object();
        private static Timer refreshTimer;
        private static RemoteConnectionCheckContainer instance = new RemoteConnectionCheckContainer();

        private RemoteConnectionCheckContainer()
        {
            log.LogMethodEntry();
            refreshTimer = new Timer(1 * 60 * 1000);
            refreshTimer.Elapsed += OnRefreshTimer;
            using (NoSynchronizationContextScope.Enter())
            {
                Task<bool> canCommunicateWithServerTask = CanCommunicateWithServer();
                canCommunicateWithServerTask.Wait();
                if (canCommunicateWithServerTask.Result)
                {
                    LastCommunicatedTime = DateTime.Now;
                }
            }
            refreshTimer.Start();
            log.LogMethodExit();
        }

        private async void OnRefreshTimer(object sender, ElapsedEventArgs e)
        {
            log.LogMethodEntry();
            bool canCommunicateWithServer = await CanCommunicateWithServer();
            if (canCommunicateWithServer)
            {
                LastCommunicatedTime = DateTime.Now;
            }
            log.LogMethodExit();
        }

        public void ThrowIfNoConnection()
        {
            log.LogMethodEntry();
            ConnectionStatus status = GetStatus();
            if (status == ConnectionStatus.Offline)
            {
                string errorMessage = "Unable to connect to server. please try again.";
                log.LogMethodExit(null, "Throwing WebApiException - " + errorMessage);
                throw new WebApiException(errorMessage, new WebApiResponse(System.Net.HttpStatusCode.GatewayTimeout, string.Empty));
            }
            log.LogMethodExit();

        }

        public static RemoteConnectionCheckContainer GetInstance
        {
            get
            {
                return instance;
            }
        }

        private async Task<bool> CanCommunicateWithServer()
        {
            //log.LogMethodEntry();
            bool result = true;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                RemoteConnectionCheckService remoteConnectionCheckService = new RemoteConnectionCheckService();
                DateTime? serverTime = await remoteConnectionCheckService.GetServerTime();
                result = serverTime.HasValue && serverTime.Value != DateTime.MinValue;
            }
            //log.LogMethodExit(result);
            return result;
        }

        public ConnectionStatus GetStatus()
        {
            ConnectionStatus result = ConnectionStatus.Offline;
            DateTime? dateTime = LastCommunicatedTime;
            if (dateTime.HasValue && dateTime.Value.AddMinutes(2) > DateTime.Now)
            {
                result = ConnectionStatus.Online;
            }
            return result;
        }

        private DateTime? LastCommunicatedTime
        {
            get
            {
                lock(locker)
                {
                    return lastCommunicatedTime;
                }
            }
            set
            {
                lock(locker)
                {
                    lastCommunicatedTime = value;
                }
            }
        }
    }
}


