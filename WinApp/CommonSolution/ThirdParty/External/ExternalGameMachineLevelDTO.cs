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
    public class ExternalGameMachineLevelDTO
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

        public ExternalGameMachineLevelDTO()
        {
            GameMachineLevelId = -1;
            MachineId = -1;
            LevelName = String.Empty;
            LevelCharacteristics = String.Empty;
            QualifyingScore = null;
            ScoreToVPRatio = null;
            ScoreToXPRatio = null;
            TranslationFileName = String.Empty;
            ImageFileName = String.Empty;
            AutoLoadEntitlement = false;
            EntitlementType = String.Empty;
        }

        public ExternalGameMachineLevelDTO(int GameMachineLevelId, int MachineId, string LevelName, string LevelCharacteristics,
            decimal? QualifyingScore, decimal? ScoreToVPRatio, decimal? ScoreToXPRatio, string TranslationFileName, string ImageFileName,
            bool AutoLoadEntitlement, string EntitlementType)
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
