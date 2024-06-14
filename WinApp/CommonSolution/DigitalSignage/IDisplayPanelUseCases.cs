/********************************************************************************************
* Project Name - DigitalSignage
* Description  - Specification of the DisplayPanel use cases. 
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.140.00   21-Apr-2021   Roshan Devadiga        Created 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.DigitalSignage
{
   public  interface IDisplayPanelUseCases
    {
        Task<List<DisplayPanelDTO>> GetDisplayPanels(List<KeyValuePair<DisplayPanelDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null);
        Task<string> SaveDisplayPanels(List<DisplayPanelDTO> displayPanelDTOList);
        Task<string> SaveStartPCs(List<DisplayPanelDTO> displayPanelDTOList);
        Task<string> SaveShutdownPCs(List<DisplayPanelDTO> displayPanelDTOList);
        /// <summary>
        /// GetDisplayPanelContainerDTOCollection
        /// </summary>
        /// <param name="siteId">siteId</param>
        /// <param name="hash">hash</param>
        /// <param name="rebuildCache">rebuildCache</param>
        Task<DisplayPanelContainerDTOCollection> GetDisplayPanelContainerDTOCollection(int siteId, string hash, bool rebuildCache);
   }
}
