/********************************************************************************************
* Project Name - Game
* Description  - GameContainerDTOCollection class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.110.0        09-Nov-2020       Mushahid Faizan         Created : POS UI Redesign with REST API
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Game
{
    public class GameContainerDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<GameContainerDTO> gameContainerDTOList;
        private string hash;

        public GameContainerDTOCollection()
        {
            log.LogMethodEntry();
            gameContainerDTOList = new List<GameContainerDTO>();
            log.LogMethodExit();
        }

        public GameContainerDTOCollection(List<GameContainerDTO> gameContainerDTOList)
        {
            log.LogMethodEntry(GameContainerDTOList);
            this.gameContainerDTOList = gameContainerDTOList;
            if (gameContainerDTOList == null)
            {
                gameContainerDTOList = new List<GameContainerDTO>();
            }
            hash = new DtoListHash(gameContainerDTOList);
            log.LogMethodExit();
        }

        public List<GameContainerDTO> GameContainerDTOList
        {
            get
            {
                return gameContainerDTOList;
            }

            set
            {
                gameContainerDTOList = value;
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

