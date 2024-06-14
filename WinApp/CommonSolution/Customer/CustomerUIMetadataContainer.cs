/********************************************************************************************
* Project Name - Customer
* Description  - CustomerUIMetadataContainer class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.120.00    09-Jul-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
2.140.00    14-Sep-2021       Prajwal S              Modified : Added Customer Field Value field.
********************************************************************************************/
using Semnox.Core.GenericUtilities;
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
    public class CustomerUIMetadataContainer: BaseContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly List<CustomerUIMetadataDTO> customerUIMetadataDTOList;
        private readonly DateTime? customerUIMetadataLastUpdateTime;
        private readonly int siteId;
        private readonly ConcurrentDictionary<string, CustomerUIMetadataDTO> customerUIMetadataDTODictionary;

        private ExecutionContext executionContext = ExecutionContext.GetExecutionContext();

        internal CustomerUIMetadataContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            this.siteId = siteId;
            customerUIMetadataDTODictionary = new ConcurrentDictionary<string, CustomerUIMetadataDTO>();
            customerUIMetadataDTOList = new List<CustomerUIMetadataDTO>();
            CustomerUIMetadataBL customerUIMetadataBL = new CustomerUIMetadataBL(executionContext);
            customerUIMetadataLastUpdateTime = customerUIMetadataBL.GetCustomerUIMetadataLastUpdateTime(siteId);

            customerUIMetadataDTOList = customerUIMetadataBL.GetCustomerUIMetadataDTOList(siteId);
            if (customerUIMetadataDTOList != null && customerUIMetadataDTOList.Any())
            {
                foreach (CustomerUIMetadataDTO customerUIMetadataDTO in customerUIMetadataDTOList)
                {
                    customerUIMetadataDTODictionary[customerUIMetadataDTO.CustomerFieldName] = customerUIMetadataDTO;
                }
            }
            else
            {
                customerUIMetadataDTOList = new List<CustomerUIMetadataDTO>();
                customerUIMetadataDTODictionary = new ConcurrentDictionary<string, CustomerUIMetadataDTO>();
            }
            log.LogMethodExit();
        }
        public List<CustomerUIMetadataContainerDTO> GetCustomerUIMetadataContainerDTOList()
        {
            log.LogMethodEntry();
            List<CustomerUIMetadataContainerDTO> customerUIMetadataContainerDTOList = new List<CustomerUIMetadataContainerDTO>();
            foreach (CustomerUIMetadataDTO customerUIMetadataDTO in customerUIMetadataDTOList)
            {

                CustomerUIMetadataContainerDTO customerUIMetadataContainerDTO = new CustomerUIMetadataContainerDTO(customerUIMetadataDTO.CustomerFieldOrder, customerUIMetadataDTO.CustomerFieldName,
                                                                                customerUIMetadataDTO.EntityFieldCaption, customerUIMetadataDTO.EntityFieldName,customerUIMetadataDTO.CustomerFieldValue, customerUIMetadataDTO.CustomAttributeFlag,
                                                                                customerUIMetadataDTO.CustomAttributeId, customerUIMetadataDTO.CustomerFieldType,customerUIMetadataDTO.ValidationType,
                                                                                customerUIMetadataDTO.FieldLength, customerUIMetadataDTO.DisplayFormat, customerUIMetadataDTO.CustomerFieldValues);
                if(customerUIMetadataDTO.CustomAttributesDTO != null)
                {
                    customerUIMetadataContainerDTO.CustomAttributesContainerDTO = new CustomAttributesContainerDTO(customerUIMetadataDTO.CustomAttributesDTO.CustomAttributeId, customerUIMetadataDTO.CustomAttributesDTO.Name,
                                                                                 customerUIMetadataDTO.CustomAttributesDTO.Sequence, customerUIMetadataDTO.CustomAttributesDTO.Type, customerUIMetadataDTO.CustomAttributesDTO.Applicability, customerUIMetadataDTO.CustomAttributesDTO.Access);
                    if(customerUIMetadataDTO.CustomAttributesDTO.CustomAttributeValueListDTOList != null && customerUIMetadataDTO.CustomAttributesDTO.CustomAttributeValueListDTOList.Any())
                    {
                        foreach(CustomAttributeValueListDTO customAttributeValueListDTO in customerUIMetadataDTO.CustomAttributesDTO.CustomAttributeValueListDTOList)
                        {
                            customerUIMetadataContainerDTO.CustomAttributesContainerDTO.CustomAttributeValueListContainerDTOList.Add(new CustomAttributeValueListContainerDTO(customAttributeValueListDTO.ValueId, customAttributeValueListDTO.Value, customAttributeValueListDTO.CustomAttributeId, customAttributeValueListDTO.IsDefault));
                        }
                    }
                }

                customerUIMetadataContainerDTOList.Add(customerUIMetadataContainerDTO);
            }
            log.LogMethodExit(customerUIMetadataContainerDTOList);
            return customerUIMetadataContainerDTOList;
        }
        public CustomerUIMetadataContainer Refresh()
        {
            log.LogMethodEntry();
            CustomerUIMetadataBL customerUIMetadataBL = new CustomerUIMetadataBL(executionContext);
            DateTime? updateTime = customerUIMetadataBL.GetCustomerUIMetadataLastUpdateTime(siteId);
            if (customerUIMetadataLastUpdateTime.HasValue
                && customerUIMetadataLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in CustomerUIMetadata since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            CustomerUIMetadataContainer result = new CustomerUIMetadataContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
