
/********************************************************************************************
 * Project Name - Device
 * Description  - ICashdrawerUseCases
 * 
 **************
 **Version Log
 **************
 *Version      Date             Modified By    Remarks          
 *********************************************************************************************
 *2.130.0     11-Aug-2021      Girish Kundar     Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Printer.Cashdrawers
{
    /// <summary>
    /// ICashdrawerUseCases
    /// </summary>
    public interface ICashdrawerUseCases
    {
        Task<List<CashdrawerDTO>> GetCashdrawers(List<KeyValuePair<CashdrawerDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null);
        Task<List<CashdrawerDTO>> SaveCashdrawers(List<CashdrawerDTO> cashdrawerDTOList);
        Task<CashdrawerContainerDTOCollection> GetCashdrawerContainerDTOCollection(int siteId, string hash, bool rebuildCache);

    }
}
