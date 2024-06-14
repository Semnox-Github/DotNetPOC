/********************************************************************************************
 * Project Name - DPL Main CLass
 * Description  - DPL 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *1.20.1      27-May-2021   Deeksha              Modified for Concurrent Program AWS changes 
 *2.120.4     12-Nov-2021    Deeksha             Modified to set siteId in context for HQ environment                                                                            
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.JobUtils;
using System;
using System.Configuration;
using System.Diagnostics;

namespace Semnox.Parafait.Inventory
{
    class Program
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string COMPLETE_PHASE = "Complete";
        private const string NORMAL_STATUS = "Normal";
        private const string ERROR_STATUS = "Error";
        private const string PROGRAM_NAME = "DPL Program";
        private const string RUNNING_PHASE = "Running";
        private const string PROGRAM_EXECUTABLE_NAME = "DPPHost.exe";

        static void Main(string[] args)
        {
            //Debugger.Launch();
            string connstring = ConfigurationManager.ConnectionStrings["ParafaitConnectionString"].ConnectionString;
            Utilities utilities = new Utilities(connstring);
            ConcurrentRequestsDTO concurrentRequestsDTO = null;
            try
            {
                ExecutionContext basicExecutionContext = ConcurrentProgramHelper.CreateBasicExecutionContext(utilities);
                string guid = ConcurrentProgramHelper.GetGuid(args);
                if (string.IsNullOrWhiteSpace(guid))
                {
                    log.Error("No guid found for the program DPL Program");
                    return;
                }
                int currentProcessID = Process.GetCurrentProcess().Id;
                concurrentRequestsDTO = ConcurrentProgramHelper.GetConcurrentRequestDTO(basicExecutionContext, PROGRAM_EXECUTABLE_NAME, guid);
                if (concurrentRequestsDTO != null)
                {
                    DPL dplObject = new DPL(basicExecutionContext);
                    utilities.ParafaitEnv.User_Id = dplObject.UserId;
                    utilities.ParafaitEnv.Username = dplObject.UserName;
                    utilities.ParafaitEnv.LoginID = dplObject.UserLoginId;
                    utilities.ParafaitEnv.SetPOSMachine("", utilities.ParafaitEnv.POSMachine);
                    utilities.ParafaitEnv.Initialize();
                    SetContextSiteId(utilities, basicExecutionContext);
                    //_Utilities.ParafaitEnv.IsCorporate = true;
                    DPLProcessor dplProcessor = new DPLProcessor(utilities);
                    ConcurrentProgramHelper.UpdateConcurrentRequest(utilities.ExecutionContext, concurrentRequestsDTO.RequestId, NORMAL_STATUS, RUNNING_PHASE);
                    dplProcessor.DPLProcessFile();
                    ConcurrentProgramHelper.UpdateConcurrentRequest(utilities.ExecutionContext, concurrentRequestsDTO.RequestId, NORMAL_STATUS, COMPLETE_PHASE);
                }
            }
            catch (Exception ex)
            {
                if (concurrentRequestsDTO != null && utilities != null)
                {
                    ConcurrentProgramHelper.UpdateConcurrentRequest(utilities.ExecutionContext, concurrentRequestsDTO.RequestId, ERROR_STATUS, COMPLETE_PHASE);
                }
                log.Error(ex);
            }
        }
        private static void SetContextSiteId(Utilities utilities, ExecutionContext basicExecutionContext)
        {
            log.LogMethodEntry(utilities, basicExecutionContext);

            if (basicExecutionContext.IsCorporate)
            {
                log.Info("Running in HQ");
                utilities.ParafaitEnv.IsCorporate = true;
            }
            else
            {
                log.Info("Running in Local Site");
            }
            utilities.ExecutionContext.SetIsCorporate(utilities.ParafaitEnv.IsCorporate);
            if (utilities.ParafaitEnv.IsCorporate)
            {
                utilities.ParafaitEnv.SiteId = basicExecutionContext.GetSiteId();
                utilities.ExecutionContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                utilities.ExecutionContext.SetSiteId(-1);
            }
            utilities.ExecutionContext.SetUserId(utilities.ParafaitEnv.LoginID);
            utilities.ExecutionContext.SetMachineId(utilities.ParafaitEnv.POSMachineId);

            log.LogMethodExit(utilities.ExecutionContext);
        }
    }
}
