/********************************************************************************************
* Project Name - Parafait Queue Management
* Description  - FormQueueManagement - manage to the list of players
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
using System.Linq;
using System.Text;

namespace ParafaitQueueManagement
{
    public class PlayersList
    {
        Player[] listOfPlayers;
        int totalEntries;
        //  Utilities playerListUtil = new Utilities();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public PlayersList(int totalPlayers)
        {
            log.LogMethodEntry(totalPlayers);
            listOfPlayers = new Player[totalPlayers];
            totalEntries = 0;
            log.LogMethodExit();
        }
        public Player CheckLastPlayerOnMachine(int machineIdToSearch)
        {
            log.LogMethodEntry(machineIdToSearch);
            int lastEntryWaitTime = -1;
            Player returnPlayer = null;
            for (int i = 0; i < totalEntries; i++)
            {
                if (listOfPlayers[i].GameMachineAssigned == machineIdToSearch)
                {
                    if (listOfPlayers[i].WaitTime > lastEntryWaitTime)
                        returnPlayer = listOfPlayers[i];
                }
            }
            log.LogMethodExit(returnPlayer);
            return returnPlayer;
        }
        public int TotalEntries
        {
            get { return totalEntries; }
        }
        public int AddPlayer(Player playerPassed)
        {
            log.LogMethodEntry(playerPassed);
            if (totalEntries < listOfPlayers.Length)
            {
                listOfPlayers[totalEntries] = playerPassed;
                totalEntries++;
                log.LogMethodExit(totalEntries);
                return totalEntries;
            }
            log.LogMethodExit(-1);
            return -1;
        }
        public Player[] GetAllPlayers()
        {
            log.LogMethodEntry();
            log.LogMethodExit(listOfPlayers);
            return listOfPlayers;
        }
        public Player GetPlayer(int i)
        {
            log.LogMethodEntry(i);
            if (i < totalEntries)
            {
                log.LogMethodExit(listOfPlayers[i]);
                return listOfPlayers[i];
            }
            else
            {
                log.LogMethodExit(null);
                return null;
            }
        }
        public int FindPlayer(int playerIdentifier)
        {
            log.LogMethodEntry(playerIdentifier);
            int playerNumber = -1;
            bool playerFound = false;
            for (int i = 0; (i < totalEntries) && (playerFound == false); i++)
            {
                if (listOfPlayers[i].QueueEntryId == playerIdentifier)
                {
                    playerNumber = i;
                    playerFound = true;
                }
            }
            log.LogMethodExit(playerNumber);
            return playerNumber;
        }
        public int RemovePlayer(int playerIdentifier)
        {
            log.LogMethodEntry(playerIdentifier);
            int removalStatus = -1;
            int playerIndex = FindPlayer(playerIdentifier);
            if (playerIndex != -1)
                removalStatus = listOfPlayers[playerIndex].RemoveFromQueue();
            log.LogMethodExit(removalStatus);
            return removalStatus;
        }
        public int UpdateTeamName(int playerIdentifier, string teamNameToSet)
        {
            log.LogMethodEntry(playerIdentifier, teamNameToSet);
            int updateTeamNameStatus = -1;
            int playerIndex = FindPlayer(playerIdentifier);
            if (playerIndex != -1)
                updateTeamNameStatus = listOfPlayers[playerIndex].UpdateTeamName(teamNameToSet);
            log.LogMethodExit(updateTeamNameStatus);
            return updateTeamNameStatus;
        }
        public int AdvancePlayer(int playerIdentifier, int timeToAdvance)
        {
            log.LogMethodEntry(playerIdentifier, timeToAdvance);
            int playerIndex = FindPlayer(playerIdentifier);
            int advancePlayerStatus = -1;
            if (playerIndex != -1)
                advancePlayerStatus = listOfPlayers[playerIndex].AdvancePlayer(timeToAdvance);
            log.LogMethodExit(advancePlayerStatus);
            return advancePlayerStatus;
        }
        public int ShiftPlayer(int playerIdentifier, int newMachineId, string gameNamePassed)
        {
            log.LogMethodEntry(playerIdentifier, newMachineId, gameNamePassed);
            int playerIndex = FindPlayer(playerIdentifier);
            int shiftPlayerStatus = -1;
            if (playerIndex != -1)
                shiftPlayerStatus = listOfPlayers[playerIndex].ShiftPlayer(newMachineId, gameNamePassed, Common.Utilities.getServerTime());
            log.LogMethodExit(shiftPlayerStatus);
            return shiftPlayerStatus;
        }
        
        // This rountine takes care of printing the token
        // The complexity is handling the teams - if there is a team, when the token is printed, it should print the tokens for all the team members
        // But this need not happen if it's re-printing as token for the team has already been printed once. 
        public int PrintTokens(int playerIdentifier, bool printFlag)
        {
            log.LogMethodEntry(playerIdentifier, printFlag);
            int printStatus = -1;
            int playerIndex = FindPlayer(playerIdentifier);
            log.LogVariableState("playerImdex", playerIndex);
            if (playerIndex != -1)
            {
                if (listOfPlayers[playerIndex].PrintCount > 0)
                    listOfPlayers[playerIndex].PrintToken(printFlag);
                else
                {
                    if (listOfPlayers[playerIndex].PrintCount == 0)
                    {
                        if (string.Compare(listOfPlayers[playerIndex].TeamName, "") != 0)
                        {
                            for (int i = 0; i < totalEntries; i++)
                            {
                                if ((listOfPlayers[playerIndex].GameMachineAssigned == listOfPlayers[i].GameMachineAssigned) &&
                                        (string.Compare(listOfPlayers[playerIndex].TeamName, listOfPlayers[i].TeamName) == 0))
                                {
                                    if ((listOfPlayers[i].PrintCount == 0) || (listOfPlayers[playerIndex].QueueEntryId == listOfPlayers[i].QueueEntryId))
                                        listOfPlayers[i].PrintToken(printFlag);
                                }
                            }
                        }
                        else
                            listOfPlayers[playerIndex].PrintToken(printFlag);
                    }

                }
            }
            log.LogMethodExit(printStatus);
            return printStatus;
        }

        public void Activate(int playerIdentifier)
        {
            log.LogMethodEntry(playerIdentifier);
            int playerIndex = FindPlayer(playerIdentifier);
            log.LogVariableState("playerImdex", playerIndex);
            if (playerIndex != -1)
            {
                if (listOfPlayers[playerIndex].CustomerIntimated == false)
                {
                    if (string.Compare(listOfPlayers[playerIndex].TeamName, "") != 0)
                    {
                        for (int i = 0; i < totalEntries; i++)
                        {
                            if ((listOfPlayers[playerIndex].GameMachineAssigned == listOfPlayers[i].GameMachineAssigned) &&
                                    (string.Compare(listOfPlayers[playerIndex].TeamName, listOfPlayers[i].TeamName) == 0))
                            {
                                if ((listOfPlayers[i].CustomerIntimated == false) || (listOfPlayers[playerIndex].QueueEntryId == listOfPlayers[i].QueueEntryId))
                                    listOfPlayers[i].ActivatePlayer();
                            }
                        }
                    }
                    else
                        listOfPlayers[playerIndex].ActivatePlayer();
                }
            }
            log.LogMethodExit();
        }

        public void UndoPrintTokens(int playerIdentifier, bool customerIntimationFlag)
        {
            log.LogMethodEntry(playerIdentifier, customerIntimationFlag);
            int playerIndex = FindPlayer(playerIdentifier);
            log.LogVariableState("playerImdex", playerIndex);
            if (playerIndex != -1)
            {
                if (string.Compare(listOfPlayers[playerIndex].TeamName, "") != 0)
                {
                    for (int i = 0; i < totalEntries; i++)
                    {
                        if ((listOfPlayers[playerIndex].GameMachineAssigned == listOfPlayers[i].GameMachineAssigned) &&
                                (string.Compare(listOfPlayers[playerIndex].TeamName, listOfPlayers[i].TeamName) == 0))
                        {
                            if ((listOfPlayers[i].PrintCount == 0) || (listOfPlayers[playerIndex].QueueEntryId == listOfPlayers[i].QueueEntryId))
                                listOfPlayers[i].UndoPrintToken(customerIntimationFlag);
                        }
                    }
                }
                else
                    listOfPlayers[playerIndex].UndoPrintToken(customerIntimationFlag);
            }
            log.LogMethodExit();
        }
    }
}
