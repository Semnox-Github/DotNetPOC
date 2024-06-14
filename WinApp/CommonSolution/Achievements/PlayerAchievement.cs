/********************************************************************************************
 * Project Name - Achievement
 * Description  - Player Achievement
 * 
 **************
 **Version Log
 **************
 *Version       Date            Modified By             Remarks          
 *********************************************************************************************
 *2.70.2        13-Aug-2019     Deeksha                 Added logger methods.
 *2.90.0        29-May-2020     Dakashakh raj           Modified :Ability to handle multiple projects. 
 ********************************************************************************************/
using Semnox.Parafait.CardCore;
using Semnox.Parafait.Customer;
using System.Collections.Generic;
using System.ComponentModel;

namespace Semnox.Parafait.Achievements
{
    public class PlayerAchievement : CustomerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private double score;
        private double points;
        private double historicalPoints;
        private string status;
        private string errorMessage;
        private string projectName;

        private List<AchievementLevelExtended> achievementLevelExtendedList;
        private List<CardCoreDTO> cardList;
     
         /// <summary>
        /// Default constructor
        /// </summary>
        public PlayerAchievement()
            : base()
        {
            log.LogMethodEntry();
            this.projectName = "";
            this.status = "";
            this.score = 0;
            this.points = 0;
            this.historicalPoints = 0;
            this.cardList = new List<CardCoreDTO>();
            this.achievementLevelExtendedList = new List<AchievementLevelExtended>();
            this.errorMessage = "";
            log.LogMethodExit();
         }

        /// <summary>
        /// Get/Set method of the Status field
        /// </summary>
        [DisplayName("ProjectName")]
        public string ProjectName { get { return projectName; } set { projectName = value; } }

        /// <summary>
        /// Get/Set method of the Status field
        /// </summary>
        [DisplayName("Status")]
        public string Status { get { return status; } set { status = value; } }


        /// <summary>
        /// Get/Set method of the ErrorMessage field
        /// </summary>
        [DisplayName("ErrorMessage")]
        public string ErrorMessage { get { return errorMessage; } set { errorMessage = value; } }


        /// <summary>
        /// Get/Set method of the Score field
        /// </summary>
        [DisplayName("Score")]
        public double Score { get { return score; } set { score = value; } }



        /// <summary>
        /// Get/Set method of the Points field
        /// </summary>
        [DisplayName("Points")]
        public double Points { get { return points; } set { points = value; } }


        /// <summary>
        /// Get/Set method of the Points field
        /// </summary>
        [DisplayName("HistoricalPoints")]
        public double HistoricalPoints { get { return historicalPoints; } set { historicalPoints = value; } }


        /// <summary>
        /// Get/Set method of the CardList field
        /// </summary>
        [DisplayName("CardList")]
        public List<CardCoreDTO> CardList { get { return cardList; } set { cardList = value; } }


        /// <summary>
        /// Get/Set method of the AchievementLevelExtendedList field
        /// </summary>
        [DisplayName("AchievementLevelExtendedList")]
        public List<AchievementLevelExtended> AchievementLevelList { get { return achievementLevelExtendedList; } set { achievementLevelExtendedList = value; } }

    }
}
