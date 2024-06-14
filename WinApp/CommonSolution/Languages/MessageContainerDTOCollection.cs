/********************************************************************************************
 * Project Name - Utilities 
 * Description  - Data object of MessageContainerDTOCollection
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 2.110.0      17-Nov-2020      Lakshminarayana           Created : POS UI Redesign with REST API
 ********************************************************************************************/

using System.Collections.Generic;
using System.Linq;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Languages
{
    public class MessageContainerDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<MessageContainerDTO> messageContainerDTOList;
        private string hash;

        public MessageContainerDTOCollection()
        {
            log.LogMethodEntry();
            messageContainerDTOList = new List<MessageContainerDTO>();
            log.LogMethodExit();
        }

        public MessageContainerDTOCollection(List<MessageContainerDTO> messageContainerDTOList)
        {
            log.LogMethodEntry(messageContainerDTOList);
            this.messageContainerDTOList = messageContainerDTOList;
            if (this.messageContainerDTOList == null)
            {
                this.messageContainerDTOList = new List<MessageContainerDTO>();
            }
            hash = new DtoListHash(messageContainerDTOList);
            log.LogMethodExit();
        }

        public List<MessageContainerDTO> MessageContainerDTOList
        {
            get
            {
                return messageContainerDTOList;
            }

            set
            {
                messageContainerDTOList = value;
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
