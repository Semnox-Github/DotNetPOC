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
    
    public class ExternalLeaderBoardDTO
    {
        public string Name { get; set; }
        public string Photo { get; set; }
        public int Rank { get; set; }
        public string Game { get; set; }
        public string LevelName { get; set; }
        public decimal Score { get; set; }


        public ExternalLeaderBoardDTO()
        {

            Name = String.Empty;
            Photo = String.Empty;
            Rank = -1;
            Game = String.Empty;
            LevelName = String.Empty;
            Score = 0;
        }

        public ExternalLeaderBoardDTO(string Name, string Photo, int Rank, string Game, string LevelName, decimal Score)
        {

            this.Name = Name;
            this.Photo = Photo;
            this.Rank = Rank;
            this.Game = Game;
            this.LevelName = LevelName;
            this.Score = Score;
        }


    }
}
