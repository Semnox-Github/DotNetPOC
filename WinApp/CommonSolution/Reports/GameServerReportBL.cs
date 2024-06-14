/********************************************************************************************
 * Project Name - Reports
 * Description  - Business Logic of GameServerReportBL for Reports module
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.80        23-Jun-2020   Vikas Dwivedi        Created
 *2.90        02-Jul-2020   Girish Kundar        Modified: Added methods to build report view
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Game;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParafaitServer
{
    public class GameServerReportBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor with ExecutionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public GameServerReportBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
    }

    public class GameServerReportListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<MachineDTO> machineDTOList;
        private string READER_PRICE_DISPLAY_FORMAT;
        public GameServerReportListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public List<MachineDTO> GetGameServerReport(bool restart, int masterid, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            Utilities utilities = new Utilities();
            List<MachineDTO> machineDTOResult = new List<MachineDTO>();
            GameServerReportDataHandler gameServerReportDataHandler = new GameServerReportDataHandler(sqlTransaction);
            if (restart && masterid > -1)  // restart for the single hub
            {
                Hub hubBL = new Hub(masterid);
                HubDTO hubDTO = hubBL.GetHubDTO; 
                hubDTO.RestartAP = true;
                hubBL.Save();
                RestartOnRefresh(masterid, utilities, sqlTransaction);
                machineDTOResult = GetGameServerReaderView(utilities, sqlTransaction);
            }
            else
            {
                machineDTOList = gameServerReportDataHandler.GetMachinesDetails(executionContext, sqlTransaction);
                machineDTOResult = GetGameServerReaderView(utilities, sqlTransaction);
            }
            log.LogMethodExit(machineDTOResult);
            return machineDTOResult;
        }

        private void RestartOnRefresh(int masterid, Utilities utilities, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(masterid, utilities, sqlTransaction);
            try
            {
                GameServerReportDataHandler gameServerReportDataHandler = new GameServerReportDataHandler(sqlTransaction);
                bool restartServer = Convert.ToBoolean(utilities.executeScalar(@"select isnull(RestartAP, 0) from masters where master_id = @masterId",
                                                        new SqlParameter("@masterId", masterid)));
                if (restartServer)
                {
                    List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>> searchParameters = new List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>>();
                    searchParameters.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.MASTER_ID, masterid.ToString()));
                    searchParameters.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.IS_ACTIVE, "Y"));
                    MachineList machineList = new MachineList(utilities.ExecutionContext);
                    List<MachineDTO> tempMachinesList = machineList.GetMachineList(searchParameters,false,sqlTransaction);
                    HashSet<int> hsOfPosMachineIdsBeforeSaving = new HashSet<int>();
                    HashSet<int> hsOfPosMachineIdsAfterSaving = new HashSet<int>();
                    foreach (MachineDTO m in tempMachinesList)
                    {
                        hsOfPosMachineIdsBeforeSaving.Add(m.MachineId);
                    }
                    if (machineDTOList == null)
                    {
                        machineDTOList =  gameServerReportDataHandler.GetMachinesDetails(executionContext, sqlTransaction);
                    }
                    foreach (MachineDTO mc in machineDTOList)
                    {
                        hsOfPosMachineIdsAfterSaving.Add(mc.MachineId);
                    }
                    List<MachineDTO> machinesList = machineDTOList;
                    HashSet<int> uniquePosMachineIds = new HashSet<int>(hsOfPosMachineIdsBeforeSaving);
                    uniquePosMachineIds.ExceptWith(hsOfPosMachineIdsAfterSaving);

                    machinesList.Clear();
                    machinesList = new List<MachineDTO>(tempMachinesList);

                    if (uniquePosMachineIds != null && uniquePosMachineIds.Count > 0)
                    {
                        foreach (int mh in uniquePosMachineIds)
                        {
                            machinesList.Add(tempMachinesList.Where(x => x.MachineId == mh).FirstOrDefault());
                        }
                    }
                    machineDTOList = machinesList;
                }
                else
                {
                    machineDTOList = gameServerReportDataHandler.GetMachinesDetails(executionContext, sqlTransaction);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                System.IO.File.AppendAllText(".\\logs\\ReaderView" + DateTime.Now.ToString("_yyyy-MM-dd") + ".log", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss :") + "Mainloop() table refresh catch:" + ex.ToString() + Environment.NewLine);
            }
            log.LogMethodExit();
        }

        public List<MachineDTO> GetGameServerReaderView(Utilities Utilities, SqlTransaction sqlTransaction)
        {
            try
            {
                log.LogMethodEntry(sqlTransaction);
                try
                {
                    READER_PRICE_DISPLAY_FORMAT = Utilities.getParafaitDefaults("READER_PRICE_DISPLAY_FORMAT");
                }
                catch
                {
                    READER_PRICE_DISPLAY_FORMAT = "##0.00";
                }
                if (machineDTOList != null && machineDTOList.Any())
                {
                    foreach (MachineDTO machineDTO in machineDTOList)
                    {
                        Hub hub = new Hub(machineDTO.MasterId);
                        string id;
                        if (hub.GetHubDTO.DirectMode == "Y")
                        {
                            id = (machineDTO.MachineAddress + "[" + machineDTO.MacAddress + "][" + (machineDTO.IPAddress != null ? machineDTO.IPAddress.ToString() : "") + "]");
                        }
                        else
                        {
                            id = machineDTO.MachineAddress;
                        }
                        Game game = new Game(machineDTO.GameId);
                        GameDTO gameDTO = game.GetGameDTO;
                        double machinePrice = gameDTO.PlayCredits != null ? (double)gameDTO.PlayCredits : game.GetGameProfileDTO.PlayCredits;
                        double machineVIPPrice = gameDTO.VipPlayCredits != null ? (double)gameDTO.VipPlayCredits : game.GetGameProfileDTO.VipPlayCredits;
                        machineDTO.HubName = hub.GetHubDTO.MasterName;
                        machineDTO.MachineAddress = id;
                        machineDTO.VipPrice = "Normal: " + machinePrice.ToString(READER_PRICE_DISPLAY_FORMAT) + ", VIP: " + machineVIPPrice.ToString(READER_PRICE_DISPLAY_FORMAT);
                        //machineDTO.CommunicationSuccessRate = (machineDTO == null || machineDTO.MachineCommunicationLogDTO == null) ? 0.0 : (machineDTO.MachineCommunicationLogDTO.CommunicationSuccessRatio / 100.0);
                        machineDTO.CommunicationSuccessRate = (machineDTO == null || machineDTO.MachineCommunicationLogDTO == null) ? 0.0 : (machineDTO.MachineCommunicationLogDTO.CommunicationSuccessRatio);
                    }
                }
              
            }

            catch (Exception ex)
            {
                log.Error(ex);
                System.IO.File.AppendAllText(".\\logs\\ReaderView" + DateTime.Now.ToString("_yyyy-MM-dd") + ".log", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss :") + "Mainloop() table refresh catch:" + ex.ToString() + Environment.NewLine);
            }
            log.LogMethodExit(machineDTOList);
            return machineDTOList;
        }

    }
}
