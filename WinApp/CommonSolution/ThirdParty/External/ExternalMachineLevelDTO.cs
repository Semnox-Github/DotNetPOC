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
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ExternalMachineLevelDTO
    {
        public int GameMachineLevelId { get; set; }
        public int MachineId { get; set; }
        public string LevelName { get; set; }
        public string LevelCharacteristics { get; set; }
        public decimal? QualifyingScore { get; set; }
        public decimal? ScoreToVPRatio { get; set; }
        public decimal? ScoreToXPRatio { get; set; }
        public string TranslationFileName { get; set; }
        public string ImageFileName { get; set; }
        public bool AutoLoadEntitlement { get; set; }
        public string EntitlementType { get; set; }
        public ExternalMachineLevelDTO()
        {
            GameMachineLevelId = -1;
            MachineId = -1;
            LevelName = string.Empty;
            LevelCharacteristics = string.Empty;
            QualifyingScore = -1;
            EntitlementType = string.Empty;
            ImageFileName = string.Empty;
            TranslationFileName = string.Empty;
            ScoreToXPRatio = -1;
            ScoreToVPRatio = -1;
            AutoLoadEntitlement = false;

        }
        public ExternalMachineLevelDTO(int GameMachineLevelId, int MachineId,string LevelName,string LevelCharacteristics, decimal? QualifyingScore, decimal? ScoreToVPRatio,
            decimal? ScoreToXPRatio,string TranslationFileName,string ImageFileName,bool AutoLoadEntitlement,string EntitlementType)
        {
            this.GameMachineLevelId = GameMachineLevelId;
            this.MachineId = MachineId;
            this.LevelName = LevelName;
            this.LevelCharacteristics = LevelCharacteristics;
            this.QualifyingScore = QualifyingScore;
            this.ScoreToVPRatio = ScoreToVPRatio;
            this.ScoreToXPRatio = ScoreToXPRatio;
            this.TranslationFileName = TranslationFileName;
            this.ImageFileName = ImageFileName;
            this.AutoLoadEntitlement = AutoLoadEntitlement;
            this.EntitlementType = EntitlementType;
        }
    }
}
