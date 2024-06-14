/********************************************************************************************
 * Project Name - TableAttributeDetailsUtils  
 * Description  - ITableAttributeDetailsUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.140.0      13-Sep-2021      Fiona           Created 
 ********************************************************************************************/
using Semnox.Parafait.TableAttributeSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.TableAttributeDetailsUtils
{
    public interface ITableAttributeDetailsUseCases
    {
        Task<List<KeyValuePair<string, string>>> GetSQLDataList(string sqlSource, string sqlDisplayMember, string sqlValueMember);
        Task<List<TableAttributeDetailsDTO>> GetTableAttributeDetailsDTOList(EnabledAttributesDTO.TableWithEnabledAttributes enabledTableName, string recordGuid);

    }
}
