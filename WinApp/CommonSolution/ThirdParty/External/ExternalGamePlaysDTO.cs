/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - Created to hold the customer game play level results details .
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.130.7    07-Apr-2022   Vignesh Bhat             Created : External  REST API.
 ***************************************************************************************************/
using System;

namespace Semnox.Parafait.ThirdParty.External
{
    
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ExternalGamePlaysDTO
    {
        public int GameplayId { get; set; }
        public int MachineId { get; set; }
        public string CardNumber { get; set; }
        public DateTime PlayDate { get; set; }
        public bool CommitGamePlay { get; set; }

        public ExternalGamePlaysDTO()
        {
            GameplayId = -1;
            MachineId = -1;
            CardNumber = String.Empty;
            CommitGamePlay = false;
        }

        public ExternalGamePlaysDTO(int GameplayId, int MachineId, string CardNumber, DateTime PlayDate, bool commitGamePlay)
        {
            this.GameplayId = GameplayId;
            this.MachineId = MachineId;
            this.CardNumber = CardNumber;
            this.PlayDate = PlayDate;
            this.CommitGamePlay = commitGamePlay;
        }
    }

    public class ExternalGamePlayRequest
    {
        public string MachineReference { get; set; }
        public int MachineId { get; set; }
        public string CardNumber { get; set; }
        public DateTime PlayDate { get; set; }
        public bool CommitGamePlay { get; set; }

        public ExternalGamePlayRequest()
        {
            MachineReference = string.Empty;
            MachineId = -1;
            CardNumber = String.Empty;
            CommitGamePlay = false;
        }
        public ExternalGamePlayRequest(int MachineId, string machineReference, string CardNumber, DateTime PlayDate, bool commitGamePlay)
        {
            this.MachineId = MachineId;
            this.MachineReference = machineReference;
            this.CardNumber = CardNumber;
            this.PlayDate = PlayDate;
            this.CommitGamePlay = commitGamePlay;
        }
    }

}
