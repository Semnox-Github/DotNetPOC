/********************************************************************************************
 * Project Name - Communication
 * Description  - Specification of Message entity use cases  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      17-Nov-2020      Lakshminarayana           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System.Threading.Tasks;

namespace Semnox.Parafait.Languages
{
    public interface IMessageUseCases
    {
        Task<MessageContainerDTOCollection> GetMessageContainerDTOCollection(int siteId, int languageId, string hash, bool rebuildCache);
    }
}
