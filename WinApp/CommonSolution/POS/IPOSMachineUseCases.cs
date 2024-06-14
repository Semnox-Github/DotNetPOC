/********************************************************************************************
 * Project Name - POS  
 * Description  - IPOSMachineDataService class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Aug-2020       Girish Kundar             Created : POS UI Redesign with REST API
 0.1         10-Nov-2020       Vikas Dwivedi             Modified as per the new Standards
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Semnox.Parafait.POS
{
    public interface IPOSMachineUseCases
    {
        Task<List<POSMachineDTO>> GetPOSMachines(List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>> parameters, bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null);
        Task<string> SavePOSMachines(List<POSMachineDTO> posMachineDTOList);
        Task<POSMachineContainerDTOCollection> GetPOSMachineContainerDTOCollection(int siteId, string hash, bool rebuildCache);
    }
}
