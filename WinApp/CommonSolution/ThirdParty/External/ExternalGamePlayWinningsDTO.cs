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
using System.Collections.Generic;

namespace Semnox.Parafait.ThirdParty.External
{
    
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Winnings
    {
        public string Type { get; set; }
        public decimal Value { get; set; }

        public Winnings()
        {
            Type = string.Empty;
            Value = 0;
        }
        public Winnings(string Type, decimal Value)
        {
            this.Type = Type;
            this.Value = Value;
        }

    }

    public class ExternalGamePlayWinningsDTO
    {
        public string CardNumber { get; set; }
        public string LevelName { get; set; }
        public string CustomerXP { get; set; }
        public int? LevelId { get; set; }
        public List<Winnings> Winnings { get; set; }


        public ExternalGamePlayWinningsDTO()
        {
            CardNumber =String.Empty;
            LevelName = String.Empty;
            CustomerXP = String.Empty;
            LevelId = -1;
            Winnings = new List<Winnings>();
        }


        public ExternalGamePlayWinningsDTO(string CardNumber, string LevelName, string CustomerXP, int? LevelId, List<Winnings> Winnings)
        {
            this.CardNumber = CardNumber;
            this.LevelName = LevelName;
            this.CustomerXP = CustomerXP;
            this.LevelId = LevelId;
            this.Winnings = Winnings;
        }
    }

}
