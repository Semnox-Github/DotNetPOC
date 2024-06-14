/********************************************************************************************
* Project Name - Customer
* Description  - AddressTypeContainer class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.120.00    08-Jul-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer
{
   public class AddressTypeContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly List<AddressTypeDTO> addressTypeDTOList;
        private readonly DateTime? addressTypeLastUpdateTime;
        private readonly int siteId;
        private readonly ConcurrentDictionary<int, AddressTypeDTO> addressTypeDTODictionary;

        private ExecutionContext executionContext = ExecutionContext.GetExecutionContext();

        internal AddressTypeContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            this.siteId = siteId;
            addressTypeDTODictionary = new ConcurrentDictionary<int, AddressTypeDTO>();
            addressTypeDTOList = new List<AddressTypeDTO>();
            AddressTypeListBL addressTypeListBL = new AddressTypeListBL(executionContext);
            addressTypeLastUpdateTime = addressTypeListBL.GetAddressTypeModuleLastUpdateTime(siteId);

            List<KeyValuePair<AddressTypeDTO.SearchByParameters, string>> SearchParameters = new List<KeyValuePair<AddressTypeDTO.SearchByParameters, string>>();
            SearchParameters.Add(new KeyValuePair<AddressTypeDTO.SearchByParameters, string>(AddressTypeDTO.SearchByParameters.IS_ACTIVE, "1"));
            SearchParameters.Add(new KeyValuePair<AddressTypeDTO.SearchByParameters, string>(AddressTypeDTO.SearchByParameters.SITE_ID, siteId.ToString()));
            addressTypeDTOList = addressTypeListBL.GetAddressTypeDTOList(SearchParameters);
            if (addressTypeDTOList != null && addressTypeDTOList.Any())
            {
                foreach (AddressTypeDTO addressTypeDTO in addressTypeDTOList)
                {
                    addressTypeDTODictionary[addressTypeDTO.Id] = addressTypeDTO;
                }
            }
            else
            {
                addressTypeDTOList = new List<AddressTypeDTO>();
                addressTypeDTODictionary = new ConcurrentDictionary<int, AddressTypeDTO>();
            }
            log.LogMethodExit();
        }
        public List<AddressTypeContainerDTO> GetAddressTypeContainerDTOList()
        {
            log.LogMethodEntry();
            List<AddressTypeContainerDTO> addressTypeViewDTOList = new List<AddressTypeContainerDTO>();
            foreach (AddressTypeDTO addressTypeDTO in addressTypeDTOList)
            {

                AddressTypeContainerDTO addressTypeViewDTO = new AddressTypeContainerDTO(addressTypeDTO.Id, addressTypeDTO.Name, addressTypeDTO.Description);

                addressTypeViewDTOList.Add(addressTypeViewDTO);
            }
            log.LogMethodExit(addressTypeViewDTOList);
            return addressTypeViewDTOList;
        }
        public AddressTypeContainer Refresh()
        {
            log.LogMethodEntry();
            AddressTypeListBL addressTypeListBL = new AddressTypeListBL(executionContext);
            DateTime? updateTime = addressTypeListBL.GetAddressTypeModuleLastUpdateTime(siteId);
            if (addressTypeLastUpdateTime.HasValue
                && addressTypeLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in AddressType since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            AddressTypeContainer result = new AddressTypeContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
