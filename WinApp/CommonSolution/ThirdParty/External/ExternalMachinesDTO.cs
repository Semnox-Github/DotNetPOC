/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - Created to hold game Play Business Logic .
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.130.7    17-june-2022   M S Shreyas           Created : External  REST API.
 ***************************************************************************************************/
using System;

namespace Semnox.Parafait.ThirdParty.External
{
    
    public class ExternalMachinesDTO
    {
        public int MachineId { get; set; }
        public string MachineName { get; set; }
        public string MachineAddress { get; set; }
        public bool IsVirtualGame { get; set; }
        public string TicketAllowed { get; set; }
        public string TimerMachine { get; set; }
        public int TimerInterval { get; set; }
        public int NumberOfCoins { get; set; }
        public string TicketMode { get; set; }
        public string ExternalMachineReference { get; set; }
        public int GameId { get; set; }
        public string GameName { get; set; }
        public string HubName { get; set; }
        public decimal Price { get; set; }
        public string MachineCharacteristics { get; set; }

        
        public ExternalMachinesDTO()
        {
            MachineId = -1;
            MachineName = String.Empty;
            MachineAddress = String.Empty;
            IsVirtualGame = false;
            TicketAllowed = String.Empty;
            TimerMachine = String.Empty;
            TimerInterval = -1;
            NumberOfCoins = -1;
            TicketMode = String.Empty;
            ExternalMachineReference = String.Empty;
            GameId = -1;
            GameName = String.Empty;
            HubName = String.Empty;
            Price = 0;
            MachineCharacteristics = String.Empty;
        }

        public ExternalMachinesDTO(int MachineId, string MachineName, string MachineAddress, bool IsVirtualGame, string TicketAllowed,
            string TimerMachine,int TimerInterval, int NumberOfCoins, string TicketMode, string ExternalMachineReference, int gameId, string GameName,
            string HubName, decimal Price, string MachineCharacteristics)

        {
            this.MachineId = MachineId;
            this.MachineName = MachineName;
            this.MachineAddress = MachineAddress;
            this.IsVirtualGame = IsVirtualGame;
            this.TicketAllowed = TicketAllowed;
            this.TimerMachine = TimerMachine;
            this.TimerInterval = TimerInterval;
            this.NumberOfCoins = NumberOfCoins;
            this.TicketMode = TicketMode;
            this.ExternalMachineReference = ExternalMachineReference;
            this.GameId = gameId;
            this.GameName = GameName;
            this.HubName = HubName;
            this.Price = Price;
            this.MachineCharacteristics = MachineCharacteristics;
        }
    }


}
