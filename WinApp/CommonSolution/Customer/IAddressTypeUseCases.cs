﻿/********************************************************************************************
* Project Name - Customer
* Description  - Specification of the IAddressTypeUseCases use cases. 
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.120.00   08-Jul-2021   Roshan Devadiga        Created 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer
{
   public interface IAddressTypeUseCases
    {
        Task<List<AddressTypeDTO>>  GetAddressTypes(List<KeyValuePair<AddressTypeDTO.SearchByParameters, string>> searchParameters);
        Task<AddressTypeContainerDTOCollection> GetAddressTypeContainerDTOCollection(int siteId, string hash, bool rebuildCache);

    }
}
