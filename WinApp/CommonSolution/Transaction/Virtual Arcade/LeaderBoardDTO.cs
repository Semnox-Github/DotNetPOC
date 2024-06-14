/********************************************************************************************
 * Project Name - LeaderBoard DTO                                                                         
 * Description  - Dto to hold the customer level results for each level to display
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************* 
  *2.110.0     01-Feb-2021   Girish Kundar    Created : Virtual Arcade changes
 ********************************************************************************************/
using System.Collections.Generic;

namespace Semnox.Parafait.Transaction.VirtualArcade
{
    /// <summary>
    /// LeaderBoardDTO
    /// </summary>
    public class LeaderBoardDTO 
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string customerName;
        private string customerPhoto;
        private int rank;
        private string gameName;
        private string levelName;
        private decimal score;

        /// <summary>
        /// Default constructor
        /// </summary>
        public LeaderBoardDTO()
        {
            log.LogMethodEntry();
            this.customerName = string.Empty;
            this.customerPhoto = string.Empty;
            this.rank = 0;
            this.gameName = string.Empty;
            this.levelName = string.Empty;
            this.score = 0;
            log.LogMethodExit();
        }

        public LeaderBoardDTO(string customerName, string customerPhoto, int rank,string gameName,string levelName,decimal score)
        {
            log.LogMethodEntry();
            this.customerName = customerName;
            this.customerPhoto = customerPhoto;
            this.rank = rank;
            this.gameName = gameName;
            this.levelName = levelName;
            this.score = score;
            log.LogMethodExit();
        }
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get { return customerName; } set { customerName = value; } }
        /// <summary>
        /// Photo
        /// </summary>
        public string Photo { get { return customerPhoto; } set { customerPhoto = value; } }
        /// <summary>
        /// Rank
        /// </summary>
        public int Rank { get { return rank; } set { rank = value; } }
        /// <summary>
        /// Game
        /// </summary>
        public string Game { get { return gameName; } set { gameName = value; } }
        /// <summary>
        /// LevelName
        /// </summary>
        public string LevelName { get { return levelName; } set { levelName = value; } }
        /// <summary>
        /// Score
        /// </summary>
        public decimal Score { get { return score; } set { score = value; } }
    }
}
