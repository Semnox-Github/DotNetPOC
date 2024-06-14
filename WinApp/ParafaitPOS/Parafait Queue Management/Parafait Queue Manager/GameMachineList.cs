/********************************************************************************************
* Project Name - Parafait Queue Management
* Description  - GameMachineList 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
* 2.80        13-Sep-2019      Jinto Thomas         Added logger for methods
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ParafaitQueueManagement
{
    public class GameMachineList
    {
        GameMachine[] listOfGameMachines;
        List<string> gameNameList = new List<string>();
        //Utilities parafaitUtility = new Utilities();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public GameMachineList()
        {
            log.LogMethodEntry();
         //   SqlCommand cmdMachines = Utilities.getCommand();
            //cmdMachines.CommandText = 
          //  SqlDataAdapter daMachines = new SqlDataAdapter(cmdMachines);
            DataTable dtMachines =Common.Utilities.executeDataTable(@"select  m.machine_id machineId, 
                                                m.machine_name machineName, 
                                                g.game_name gameName, 
                                                m.active_flag activeFlag,
                                                isnull(m.serialnumber, m.machine_id) sortOrderCol
                                           from game_profile gp, 
                                                games g, 
                                                machines m
                                          where gp.game_profile_id = g.game_profile_id
                                            and g.game_id = m.game_id
                                            and dbo.GetMachineProfileValue(m.machine_id, 'QUEUE_SETUP_REQUIRED') = '1'
                                          order by m.serialnumber, m.machine_id");
          //  daMachines.Fill(dtMachines);
            listOfGameMachines = new GameMachine[dtMachines.Rows.Count];
            for (int i = 0; i < dtMachines.Rows.Count; i++)
            {
                listOfGameMachines[i] = new GameMachine(Convert.ToInt32(dtMachines.Rows[i]["machineId"]), dtMachines.Rows[i]["machineName"].ToString(), dtMachines.Rows[i]["gameName"].ToString(), dtMachines.Rows[i]["activeFlag"].ToString(), dtMachines.Rows[i]["sortOrderCol"].ToString());
                if (gameNameList.Any(listOfGameMachines[i].GameNameOfMachine.Contains) == false)
                    gameNameList.Add(listOfGameMachines[i].GameNameOfMachine);
            }
            log.LogMethodExit();
        }
        public void refreshGameMachineList()
        {
            log.LogMethodEntry();
            //SqlCommand cmdMachines = Utilities.getCommand();
            //cmdMachines.CommandText = ;
          //  SqlDataAdapter daMachines = new SqlDataAdapter(cmdMachines);
            DataTable dtMachines = Common.Utilities.executeDataTable(@"select  m.machine_id machineId, 
                                                m.machine_name machineName, 
                                                g.game_name gameName, 
                                                m.active_flag activeFlag,
                                                isnull(m.serialnumber, m.machine_id) sortOrderCol
                                           from game_profile gp, 
                                                games g, 
                                                machines m
                                          where gp.game_profile_id = g.game_profile_id
                                            and g.game_id = m.game_id
                                            and dbo.GetMachineProfileValue(m.machine_id, 'QUEUE_SETUP_REQUIRED') = '1'
                                          order by m.serialnumber, m.machine_id");
           // daMachines.Fill(dtMachines);
            for (int i = 0; i < dtMachines.Rows.Count; i++)
            {
                GameMachine currentGameMachine = IdentifyGameMachine(Convert.ToInt32(dtMachines.Rows[i]["machineId"]));
                currentGameMachine.ActiveFlag = dtMachines.Rows[i]["activeFlag"].ToString();
                currentGameMachine.GameNameOfMachine = dtMachines.Rows[i]["gameName"].ToString();
                currentGameMachine.LaneName = dtMachines.Rows[i]["machineName"].ToString();
                int sortOrderColValue = -1;
                bool parseSortOrderStatus = int.TryParse(dtMachines.Rows[i]["sortOrderCol"].ToString(), out sortOrderColValue);
                if (parseSortOrderStatus)
                    currentGameMachine.SerialNumber = sortOrderColValue;
                else
                    currentGameMachine.SerialNumber = -1;
            }
            log.LogMethodExit();
        }
        public void ClearPlayers()
        {
            log.LogMethodEntry();
            for (int i = 0; i < listOfGameMachines.Length; i++)
                listOfGameMachines[i].ClearCurrentPlayer();
            log.LogMethodExit();
        }
        public GameMachine IdentifyGameMachine(int machineIdPassed)
        {
            log.LogMethodEntry(machineIdPassed);
            for (int i = 0; i < listOfGameMachines.Length; i++)
            {
                if (listOfGameMachines[i].GameMachineId == machineIdPassed)
                    return listOfGameMachines[i];
            }
            log.LogMethodExit(null);
            return null;
        }
        public GameMachine IdentifyGameMachineThruSerial(int laneSerialNumber)
        {
            log.LogMethodEntry(laneSerialNumber);
            for (int i = 0; i < listOfGameMachines.Length; i++)
            {
                if (listOfGameMachines[i].SerialNumber == laneSerialNumber)
                    return listOfGameMachines[i];
            }
            log.LogMethodExit(null);
            return null;

        }
        public int TotalEntries
        {
            get { return listOfGameMachines.Length; }
        }
        public GameMachine RetrieveGameMachine(int indexOfGM)
        {
            if (indexOfGM < listOfGameMachines.Length)
                return listOfGameMachines[indexOfGM];
            else
                return null;
        }
        public GameMachine[] GetAllGameMachines()
        {
            return listOfGameMachines.OrderBy(x => x.SerialNumber).ToArray();
        }
        public string[] GetAllGameMachinesName()
        {
            return gameNameList.ToArray();
        }
    }

}
