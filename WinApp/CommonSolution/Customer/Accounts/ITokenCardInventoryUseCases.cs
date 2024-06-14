/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Interface for TokenCardInventoryUseCases
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.130.0     25-May-2021   Prashanth            Created for POS UI Redesign 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer.Accounts
{
    public interface ITokenCardInventoryUseCases
    {
        Task<List<TokenCardInventoryDTO>> GetAllTokenCardInventoryDTOsList(List<KeyValuePair<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string>> searchParameters, SqlTransaction sqlTransaction = null);
        Task<List<TokenCardInventoryDTO>> SaveCardInventory(List<TokenCardInventoryDTO> tokenCardInventoryDTOList);
        Task<List<TokenCardInventoryDTO>> UpdateCardInventory(List<TokenCardInventoryDTO> tokenCardInventoryDTOList);
    }
}
