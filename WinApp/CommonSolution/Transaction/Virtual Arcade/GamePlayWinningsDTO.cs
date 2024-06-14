/********************************************************************************************
 * Project Name - GamePlayWinnings DTO                                                                         
 * Description  - Dto to hold the customer winnings details for each level 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************* 
  *2.110.0     01-Feb-2021   Girish Kundar    Created : Virtual Arcade changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction.VirtualArcade
{
    /// <summary>
    /// This class hold the customer winnigs details like bonus,tockets, virtual points and product id
    /// </summary>
    public class GamePlayWinningsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string accountNumber;
        private string levelName;
        private string customerXP;
        private int? levelId;
        private List<GamePlayWinningDetailDTO> gamePlayWinningDetailDTOList;

        /// <summary>
        /// GamePlayWinningsDTO
        /// </summary>
        public GamePlayWinningsDTO()
        {
            log.LogMethodEntry();
            accountNumber = "";
            levelName = "";
            levelId = null;
            gamePlayWinningDetailDTOList = new List<GamePlayWinningDetailDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// GamePlayWinningsDTO
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <param name="levelName"></param>
        /// <param name="levelId"></param>
        /// <param name="customerXP"></param>
        public GamePlayWinningsDTO(string accountNumber,string levelName, int? levelId,string customerXP)
        {
            log.LogMethodEntry(accountNumber, levelName, levelId,customerXP);
            this.accountNumber = accountNumber;
            this.levelName = levelName;
            this.levelId = levelId;
            this.customerXP = customerXP;
            log.LogMethodExit();
        }

        /// <summary>
        /// AccountNumber
        /// </summary>
        public string AccountNumber { get { return accountNumber; } set { accountNumber = value; } }

        /// <summary>
        /// LevelName
        /// </summary>
        public string LevelName { get { return levelName; } set { levelName = value; } }
       /// <summary>
        /// LevelName
        /// </summary>
        public string CustomerXP { get { return customerXP; } set { customerXP = value; } }
        /// <summary>
        /// LevelId
        /// </summary>
        public int? LevelId { get { return levelId; } set { levelId = value; } }

        /// <summary>
        /// Winnigs
        /// </summary>
        public List<GamePlayWinningDetailDTO> Winnings { get { return gamePlayWinningDetailDTOList; } set { gamePlayWinningDetailDTOList = value; } }
    }
}
