/********************************************************************************************
 * Project Name - Customer
 * Description  - CustomerRelationshipTypeContainer class 
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By        Remarks          
 *********************************************************************************************
 2.130.0      31-Aug-2021       Mushahid Faizan    Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Semnox.Parafait.Customer
{
    public class CustomerRelationshipTypeContainer 
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly List<CustomerRelationshipTypeDTO> customerRelationshipTypeDTOList;
        private readonly DateTime? customerRelationshipTypeModuleLastUpdateTime;
        private readonly int siteId;
        private readonly Dictionary<int, CustomerRelationshipTypeDTO> customerRelationshipTypeIdCustomerRelationshipTypeDTODictionary = new Dictionary<int, CustomerRelationshipTypeDTO>();
        private readonly CustomerRelationshipTypeContainerDTOCollection customerRelationshipTypeContainerDTOCollection;
        private readonly Dictionary<int, CustomerRelationshipTypeContainerDTO> customerRelationshipTypeIdcustomerRelationshipTypeContainerDTODictionary = new Dictionary<int, CustomerRelationshipTypeContainerDTO>();

        private ExecutionContext executionContext = ExecutionContext.GetExecutionContext();


        public CustomerRelationshipTypeContainer(int siteId) : this(siteId, GetCustomerRelationshipTypeDTOList(siteId), GetCustomerRelationshipTypeModuleLastUpdateTime(siteId))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }

        public CustomerRelationshipTypeContainer(int siteId, List<CustomerRelationshipTypeDTO> customerRelationshipTypeDTOList, DateTime? customerRelationshipTypeModuleLastUpdateTime)
        {
            log.LogMethodEntry(siteId);
            this.siteId = siteId;
            this.customerRelationshipTypeDTOList = customerRelationshipTypeDTOList;
            this.customerRelationshipTypeModuleLastUpdateTime = customerRelationshipTypeModuleLastUpdateTime;
            foreach (var customerRelationshipTypeDTO in customerRelationshipTypeDTOList)
            {
                if (customerRelationshipTypeIdCustomerRelationshipTypeDTODictionary.ContainsKey(customerRelationshipTypeDTO.Id))
                {
                    continue;
                }
                customerRelationshipTypeIdCustomerRelationshipTypeDTODictionary.Add(customerRelationshipTypeDTO.Id, customerRelationshipTypeDTO);
            }
            List<CustomerRelationshipTypeContainerDTO> customerRelationshipTypeContainerDTOList = new List<CustomerRelationshipTypeContainerDTO>();
            foreach (CustomerRelationshipTypeDTO customerRelationshipTypeDTO in customerRelationshipTypeDTOList)
            {
                if (customerRelationshipTypeIdcustomerRelationshipTypeContainerDTODictionary.ContainsKey(customerRelationshipTypeDTO.Id))
                {
                    continue;
                }
                CustomerRelationshipTypeContainerDTO customerRelationshipTypeContainerDTO = new CustomerRelationshipTypeContainerDTO(customerRelationshipTypeDTO.Id, customerRelationshipTypeDTO.Name, customerRelationshipTypeDTO.Description);
                customerRelationshipTypeContainerDTOList.Add(customerRelationshipTypeContainerDTO);
                customerRelationshipTypeIdcustomerRelationshipTypeContainerDTODictionary.Add(customerRelationshipTypeDTO.Id, customerRelationshipTypeContainerDTO);
            }
            customerRelationshipTypeContainerDTOCollection = new CustomerRelationshipTypeContainerDTOCollection(customerRelationshipTypeContainerDTOList);
            log.LogMethodExit();
        }


        private static List<CustomerRelationshipTypeDTO> GetCustomerRelationshipTypeDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            List<CustomerRelationshipTypeDTO> customerRelationshipTypeDTOList = null;
            try
            {
                CustomerRelationshipTypeListBL customerRelationshipTypeList = new CustomerRelationshipTypeListBL();

                List<KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string>(CustomerRelationshipTypeDTO.SearchByParameters.IS_ACTIVE, "1"));
                searchParameters.Add(new KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string>(CustomerRelationshipTypeDTO.SearchByParameters.SITE_ID, siteId.ToString()));
                customerRelationshipTypeDTOList = customerRelationshipTypeList.GetCustomerRelationshipTypeDTOList(searchParameters);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the customerRelationshipType.", ex);
            }

            if (customerRelationshipTypeDTOList == null)
            {
                customerRelationshipTypeDTOList = new List<CustomerRelationshipTypeDTO>();
            }
            log.LogMethodExit(customerRelationshipTypeDTOList);
            return customerRelationshipTypeDTOList;
        }

        private static DateTime? GetCustomerRelationshipTypeModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            DateTime? result = null;
            try
            {
                CustomerRelationshipTypeListBL customerRelationshipTypeList = new CustomerRelationshipTypeListBL();
                result = customerRelationshipTypeList.GetCustomerRelationshipTypeLastUpdateTime(siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the CustomerRelationshipType max last update date.", ex);
                result = null;
            }
            log.LogMethodExit(result);
            return result;
        }


        public CustomerRelationshipTypeContainerDTO GetCustomerRelationshipTypeContainerDTO(int customerRelationshipTypeId)
        {
            log.LogMethodEntry(customerRelationshipTypeId);
            if (customerRelationshipTypeIdcustomerRelationshipTypeContainerDTODictionary.ContainsKey(customerRelationshipTypeId) == false)
            {
                string errorMessage = "customerRelationshipType with customerRelationshipType Id :" + customerRelationshipTypeId + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            CustomerRelationshipTypeContainerDTO result = customerRelationshipTypeIdcustomerRelationshipTypeContainerDTODictionary[customerRelationshipTypeId]; ;
            log.LogMethodExit(result);
            return result;
        }

        public CustomerRelationshipTypeContainerDTOCollection GetCustomerRelationshipTypeContainerDTOCollection()
        {
            log.LogMethodEntry();
            log.LogMethodExit(customerRelationshipTypeContainerDTOCollection);
            return customerRelationshipTypeContainerDTOCollection;
        }
        //internal CustomerRelationshipTypeContainer(int siteId)
        //{
        //    log.LogMethodEntry(siteId);
        //    this.siteId = siteId;
        //    customerRelationshipTypeIdCustomerRelationshipTypeDTODictionary = new ConcurrentDictionary<int, CustomerRelationshipTypeDTO>();
        //    customerRelationshipTypeDTOList = new List<CustomerRelationshipTypeDTO>();
        //    CustomerRelationshipTypeListBL customerRelationshipTypeList = new CustomerRelationshipTypeListBL(executionContext);
        //    customerRelationshipTypeModuleLastUpdateTime = customerRelationshipTypeList.GetCustomerRelationshipTypeLastUpdateTime(siteId);

        //    List<KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string>>();
        //    searchParameters.Add(new KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string>(CustomerRelationshipTypeDTO.SearchByParameters.IS_ACTIVE, "1"));
        //    searchParameters.Add(new KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string>(CustomerRelationshipTypeDTO.SearchByParameters.SITE_ID, siteId.ToString()));
        //    customerRelationshipTypeDTOList = customerRelationshipTypeList.GetCustomerRelationshipTypeDTOList(searchParameters);

        //    if (customerRelationshipTypeDTOList != null && customerRelationshipTypeDTOList.Any())
        //    {
        //        foreach (CustomerRelationshipTypeDTO customerRelationshipTypeDTO in customerRelationshipTypeDTOList)
        //        {
        //            customerRelationshipTypeIdCustomerRelationshipTypeDTODictionary[customerRelationshipTypeDTO.Id] = customerRelationshipTypeDTO;
        //        }
        //    }
        //    else
        //    {
        //        customerRelationshipTypeDTOList = new List<CustomerRelationshipTypeDTO>();
        //        customerRelationshipTypeIdCustomerRelationshipTypeDTODictionary = new ConcurrentDictionary<int, CustomerRelationshipTypeDTO>();
        //    }
        //    log.LogMethodExit();
        //}
        public List<CustomerRelationshipTypeContainerDTO> GetCustomerRelationshipTypeContainerDTOList()
        {
            log.LogMethodEntry();
            List<CustomerRelationshipTypeContainerDTO> customerRelationshipTypeContainerDTOList = new List<CustomerRelationshipTypeContainerDTO>();
            foreach (CustomerRelationshipTypeDTO customerRelationshipTypeDTO in customerRelationshipTypeDTOList)
            {

                CustomerRelationshipTypeContainerDTO customerRelationshipTypeContainerDTO = new CustomerRelationshipTypeContainerDTO(customerRelationshipTypeDTO.Id, customerRelationshipTypeDTO.Name, customerRelationshipTypeDTO.Description);

                customerRelationshipTypeContainerDTOList.Add(customerRelationshipTypeContainerDTO);
            }
            log.LogMethodExit(customerRelationshipTypeContainerDTOList);
            return customerRelationshipTypeContainerDTOList;
        }
        public CustomerRelationshipTypeContainer Refresh()
        {
            log.LogMethodEntry();
            CustomerRelationshipTypeListBL customerRelationshipTypeList = new CustomerRelationshipTypeListBL();
            DateTime? updateTime = customerRelationshipTypeList.GetCustomerRelationshipTypeLastUpdateTime(siteId);
            if (customerRelationshipTypeModuleLastUpdateTime.HasValue
                && customerRelationshipTypeModuleLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in CustomerRelationshipType since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            CustomerRelationshipTypeContainer result = new CustomerRelationshipTypeContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
