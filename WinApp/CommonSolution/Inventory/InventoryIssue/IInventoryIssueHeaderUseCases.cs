/********************************************************************************************
* Project Name - Inventory
* Description  - IInventoryIssueHeaderUseCases class 
*  
**************
**Version Log
**************
*Version     Date               Modified By               Remarks          
*********************************************************************************************
*2.110.0         28-Dec-2020    Prajwal S               Created : POS UI Redesign with REST API
*2.110.1     01-Mar-2021      Mushahid Faizan          Modified : Web Inventory UI Redesign with REST API
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;

namespace Semnox.Parafait.Inventory
{
    public interface IInventoryIssueHeaderUseCases
    {
        Task<List<InventoryIssueHeaderDTO>> GetInventoryIssueHeaders(List<KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>>
                          searchParameters, int currentPage = 0, int pageSize = 0, bool buildChildRecords = false, bool loadActiveChild = false, SqlTransaction sqlTransaction = null);
        Task<List<InventoryIssueHeaderDTO>> SaveInventoryIssueHeaders(List<InventoryIssueHeaderDTO> inventoryIssueHeadersDTOList);
        Task<int> GetInventoryIssueCount(List<KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>>
                          searchParameters, SqlTransaction sqlTransaction = null);
        Task<InventoryIssueHeaderDTO> AddInventoryIssueLines(int issueId, List<InventoryIssueLinesDTO> inventoryIssueLineDTOList);
        Task<InventoryIssueHeaderDTO> UpdateInventoryIssueLines(int issueId, List<InventoryIssueLinesDTO> inventoryIssueLineDTOList);
        //  Task<InventoryIssueHeaderDTO> RemoveInventoryIssueLines(int issueId, List<InventoryIssueLinesDTO> inventoryIssueLineDTOList);
        Task<InventoryIssueHeaderDTO> UpdateIssueStatus(int issueId, string issueStatus);

        Task<MemoryStream> PrintIssues(int issueId, string reportKey, string timeStamp, DateTime? fromDate,
                         DateTime? toDate, string outputFormat);
    }
}
