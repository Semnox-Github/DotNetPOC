/********************************************************************************************
 * Project Name - Inventory
 * Description  - IRequisitionUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0    10-Dec-2020         Mushahid Faizan                 Created 
 ********************************************************************************************/
using Semnox.Parafait.Communication;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;

namespace Semnox.Parafait.Inventory.Requisition
{
    public interface IRequisitionUseCases
    {
        Task<List<RequisitionDTO>> GetRequisitions(List<KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>> searchParameters,
                                                   bool loadChildRecords = false, bool activeChildRecords = true, int currentPage = 0, int pageSize = 0,
                                                   SqlTransaction sqlTransaction = null);
        Task<List<RequisitionDTO>> SaveRequisitions(List<RequisitionDTO> requisitionDTOList);
        Task<int> GetRequisitionCount(List<KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>> searchParameters,
                                                   SqlTransaction sqlTransaction = null);
        Task<RequisitionDTO> UpdateRequisitionStatus(int requisitionId, string requsitionStatus);
        Task<RequisitionDTO> AddRequisitionLines(int requisitionId, List<RequisitionLinesDTO> requisitionLineDTOList);
        Task<RequisitionDTO> UpdateRequisitionLines(int requisitionId, List<RequisitionLinesDTO> requisitionLineDTOList);

        Task<MemoryStream> PrintRequisitions(int requisitionId, string reportKey, string timeStamp, DateTime? fromDate,
                             DateTime? toDate, string outputFormat);

        Task<MessagingRequestDTO> SendRequisitionEmail(int requisitionId, List<MessagingRequestDTO> messagingRequestDTOList);
    }
}
