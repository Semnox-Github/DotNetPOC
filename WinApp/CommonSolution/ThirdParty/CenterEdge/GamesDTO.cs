/********************************************************************************************
 * Project Name - CenterEdge  
 * Description  - GamesDTO class This would hold the games that are configured in the system. 
 *                        This would return the active games in the Parafait system. 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Sep-2020       Girish Kundar             Created : CenterEdge  REST API
 ********************************************************************************************/

namespace Semnox.Parafait.ThirdParty.CenterEdge
{
    public class GameDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int gameId;
        private string gameName;
        private bool virtualPlayEnabledGame;

        public GameDTO()
        {
            log.LogMethodEntry();
            gameId = -1;
            gameName = string.Empty;
            virtualPlayEnabledGame = true;
            log.LogMethodExit();
        }

        public GameDTO(int id, string name ,bool virtualPlayEnabled)
        {
            log.LogMethodEntry(id ,name, virtualPlayEnabled);
            this.gameId = id;
            this.gameName = name;
            this.virtualPlayEnabled = virtualPlayEnabled;
            log.LogMethodExit();
        }
        public int id { get { return gameId; } set { gameId = value; } }
        public string name { get { return gameName; } set { gameName = value; } }
        public bool virtualPlayEnabled { get { return virtualPlayEnabledGame; } set { virtualPlayEnabledGame = value; } }
    }
}
