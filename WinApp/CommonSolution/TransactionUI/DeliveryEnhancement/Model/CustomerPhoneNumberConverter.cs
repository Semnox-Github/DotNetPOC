/********************************************************************************************
 * Project Name - Transaction UI
 * Description  - CustomerNameConverter 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.140.0     01-Jul-2021    Fiona                  Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Transaction;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Semnox.Parafait.TransactionUI
{
    public class CustomerPhoneNumberConverter : IValueConverter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            log.LogMethodEntry();
            string customerPhoneNumber = "-";
            IEnumerable<object> parameterList = (IEnumerable<object>)parameter;
            if (parameterList == null)
            {
                log.LogMethodExit(customerPhoneNumber);
                return customerPhoneNumber;
            }
            ExecutionContext executionContext = parameterList.ElementAt(0) as ExecutionContext;
            List<TransactionDTO> searchedDeliveries = parameterList.ElementAt(1) as List<TransactionDTO>;
            int CustomerId = (int)value;// searchedDeliveries.Find(x=>x.TransctionOrderDispensingDTO.DeliveryContactId==);
            int deliveryContactId = -1;
            if (searchedDeliveries!=null && searchedDeliveries.Count>0)
            {
                deliveryContactId = searchedDeliveries.Find(x => x.CustomerId == CustomerId).TransctionOrderDispensingDTO.DeliveryContactId;
            }
            List<CustomerDTO> customerDTOList = new List<CustomerDTO>();
            
            

            //if(CustomeId>-1 && customerDTOList!=null && customerDTOList.Count!=0)
            //{
            //    customerPhoneNumber = customerDTOList.Find(x => x.Id == CustomeId).PhoneNumber;
            //}
            ICustomerUseCases customerUseCases = CustomerUseCaseFactory.GetCustomerUseCases(executionContext);
            List<KeyValuePair<CustomerSearchByParameters, string>> searchParams = new List<KeyValuePair<CustomerSearchByParameters, string>>();
            searchParams.Add(new KeyValuePair<CustomerSearchByParameters, string>(CustomerSearchByParameters.CUSTOMER_ID, CustomerId.ToString()));
            searchParams.Add(new KeyValuePair<CustomerSearchByParameters, string>(CustomerSearchByParameters.ISACTIVE, "1"));
        
            try
            {
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<List<CustomerDTO>> task = customerUseCases.GetCustomerDTOList(searchParams, true, true);
                    task.Wait();
                    customerDTOList = task.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }

            if (CustomerId > -1 && customerDTOList != null && customerDTOList.Count != 0)
            {
                if(deliveryContactId == -1)
                {
                    customerPhoneNumber = customerDTOList.Find(x => x.Id == CustomerId).PhoneNumber;
                }
                else
                {
                    ProfileDTO profileDTO = customerDTOList.Find(x => x.Id == CustomerId).ProfileDTO;
                    if (profileDTO.ContactDTOList != null && profileDTO.ContactDTOList.Count > 0)
                    {
                        ContactDTO contactDTO = profileDTO.ContactDTOList.Where((x) => x.ContactType == ContactType.PHONE && x.IsActive && x.Id == deliveryContactId).OrderByDescending((x) => x.LastUpdateDate).FirstOrDefault();
                        if (contactDTO != null)
                        {
                            customerPhoneNumber = contactDTO.Attribute1;
                        }
                    }
                }
               
            }

            log.LogMethodEntry(customerPhoneNumber);
            return customerPhoneNumber;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
