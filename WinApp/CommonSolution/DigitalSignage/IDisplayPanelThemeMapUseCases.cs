/********************************************************************************************
* Project Name - DigitalSignage
* Description  - Specification of the DisplayPanelThemeMap use cases. 
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.120.00   21-Apr-2021   Roshan Devadiga        Created 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.DigitalSignage
{
   public interface IDisplayPanelThemeMapUseCases
    {
        Task<List<DisplayPanelThemeMapDTO>> GetDisplayPanelThemeMaps(List<KeyValuePair<DisplayPanelThemeMapDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null);
        Task<string> SaveDisplayPanelThemeMaps(List<DisplayPanelThemeMapDTO> displayPanelThemeDTOList);

    }
}
