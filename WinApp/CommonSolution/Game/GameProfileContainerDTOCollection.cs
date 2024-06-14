/********************************************************************************************
* Project Name - Game
* Description  - GameProfileContainerDTOCollection class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.110.0      10-Dec-2020      Prajwal S                 Created : POS UI Redesign with REST API
********************************************************************************************/
using System.Collections.Generic;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Game;

namespace Semnox.Parafait.Game
{
        public class GameProfileContainerDTOCollection
        {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<GameProfileContainerDTO> gameProfileContainerDTOList;
        private string hash;

        public GameProfileContainerDTOCollection()
        {
            log.LogMethodEntry();
            gameProfileContainerDTOList = new List<GameProfileContainerDTO>();
            log.LogMethodExit();
        }

        public GameProfileContainerDTOCollection(List<GameProfileContainerDTO> gameProfileContainerDTOList)
        {
            log.LogMethodEntry(gameProfileContainerDTOList);
            this.gameProfileContainerDTOList = gameProfileContainerDTOList;
            if (gameProfileContainerDTOList == null)
            {
                gameProfileContainerDTOList = new List<GameProfileContainerDTO>();
            }
            hash = new DtoListHash(gameProfileContainerDTOList);
            log.LogMethodExit();
        }

        public List<GameProfileContainerDTO> GameProfileContainerDTOList
        {
            get
            {
                return gameProfileContainerDTOList;
            }

            set
            {
                gameProfileContainerDTOList = value;
            }
        }

        public string Hash
        {
            get
            {
                return hash;
            }
            set
            {
                hash = value;
            }
        }

    }
}
