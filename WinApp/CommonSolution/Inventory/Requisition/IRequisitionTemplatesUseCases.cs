/********************************************************************************************
 * Project Name - Inventory
 * Description  - IRequisitionTemplatesUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0    11-Dec-2020         Abhishek                 Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Semnox.Parafait.Inventory.Requisition
{
    public interface IRequisitionTemplatesUseCases
    {
        Task<List<RequisitionTemplatesDTO>> GetRequisitionTemplates(List<KeyValuePair<RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters, string>> searchParameters, 
                                                   bool loadChildRecords = false, bool activeChildRecords = true, int currentPage = 0, int pageSize = 0,
                                                   SqlTransaction sqlTransaction = null);
        Task<string> SaveRequisitionTemplates(List<RequisitionTemplatesDTO> requisitionTemplatesDTOList);
        Task<int> GetRequisitionTemplateCount(List<KeyValuePair<RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters, string>> searchParameters,
                                                   SqlTransaction sqlTransaction = null);
    }
}
