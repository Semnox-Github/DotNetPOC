/********************************************************************************************
 * Project Name - Product
 * Description  - ProductCalendarContainer class to get the data
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.0      20-July-2021      Prajwal S               Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// Class holds the available Product values.
    /// </summary>
    public class ProductCalendarContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DateTimeRange dateTimeRange;
        private readonly Dictionary<int, ProductCalendarContainerDTO> productIdProductCalendarDTODictionary = new Dictionary<int, ProductCalendarContainerDTO>();
        private ProductCalendarContainerDTOCollection productCalendarContainerDTOCollection;
        private TimeSpan increment;
        private int siteId;

        public ProductCalendarContainer(int siteId, List<ProductsDTO> ProductsDTOList, DateTimeRange dateTimeRange, TimeSpan increment)
        {
            log.LogMethodEntry(ProductsDTOList, dateTimeRange, increment);
            this.dateTimeRange = dateTimeRange;
            this.increment = increment;
            this.siteId = siteId;
            List<ProductCalendarContainerDTO> productCalendarContainerDTOList = new List<ProductCalendarContainerDTO>();
            foreach (var productsDTO in ProductsDTOList)
            {
                if (productIdProductCalendarDTODictionary.ContainsKey(productsDTO.ProductId) ||
                    productsDTO.ActiveFlag == false)
                {
                    continue;
                }

                ProductCalendarContainerDTO productCalendarDTO = GetProductCalendarContainerDTO(productsDTO);
                productCalendarContainerDTOList.Add(productCalendarDTO);
                productIdProductCalendarDTODictionary.Add(productsDTO.ProductId, productCalendarDTO);
            }
            productCalendarContainerDTOCollection = new ProductCalendarContainerDTOCollection(productCalendarContainerDTOList);
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the Product available
        /// </summary>
        /// <param name="productsDTO">ProductsDTO</param>
        /// <returns></returns>
        private ProductCalendarContainerDTO GetProductCalendarContainerDTO(ProductsDTO productsDTO)
        {
            log.LogMethodEntry(productsDTO);
            if (productsDTO.ProductsCalenderDTOList == null ||
                productsDTO.ProductsCalenderDTOList.Any() == false)
            {
                ProductCalendarDetailContainerDTO defaultProductCalendarDetailDTO;
                ProductCalendarContainerDTO defaultProductCalendarDTO;

                List<ProductCalendarDetailContainerDTO> defaultProductCalendarDetailDTOList = new List<ProductCalendarDetailContainerDTO>();
                if(productsDTO.StartDate != DateTime.MinValue && productsDTO.ExpiryDate != DateTime.MinValue && productsDTO.StartDate >= productsDTO.ExpiryDate)
                {
                    log.Error("Product start date is greater than or equal to expiry date. ProductId " + productsDTO.ProductId);
                    defaultProductCalendarDetailDTO = new ProductCalendarDetailContainerDTO(dateTimeRange.StartDateTime, dateTimeRange.EndDateTime, false);
                    defaultProductCalendarDetailDTOList.Add(defaultProductCalendarDetailDTO);
                }
                else if((productsDTO.StartDate != DateTime.MinValue && dateTimeRange.EndDateTime <= productsDTO.StartDate) || (productsDTO.ExpiryDate != DateTime.MinValue && dateTimeRange.EndDateTime >= productsDTO.ExpiryDate))
                {
                    defaultProductCalendarDetailDTO = new ProductCalendarDetailContainerDTO(dateTimeRange.StartDateTime, dateTimeRange.EndDateTime, false);
                    defaultProductCalendarDetailDTOList.Add(defaultProductCalendarDetailDTO);
                }
                else if(dateTimeRange.Contains(productsDTO.StartDate) && dateTimeRange.Contains(productsDTO.ExpiryDate))
                {
                    defaultProductCalendarDetailDTO = new ProductCalendarDetailContainerDTO(dateTimeRange.StartDateTime, productsDTO.StartDate, false);
                    defaultProductCalendarDetailDTOList.Add(defaultProductCalendarDetailDTO);
                    defaultProductCalendarDetailDTO = new ProductCalendarDetailContainerDTO(productsDTO.StartDate, productsDTO.ExpiryDate, true);
                    defaultProductCalendarDetailDTOList.Add(defaultProductCalendarDetailDTO);
                    defaultProductCalendarDetailDTO = new ProductCalendarDetailContainerDTO(productsDTO.ExpiryDate, dateTimeRange.EndDateTime, false);
                    defaultProductCalendarDetailDTOList.Add(defaultProductCalendarDetailDTO);
                }
                else if (dateTimeRange.Contains(productsDTO.StartDate) == false && dateTimeRange.Contains(productsDTO.ExpiryDate))
                {
                    defaultProductCalendarDetailDTO = new ProductCalendarDetailContainerDTO(dateTimeRange.StartDateTime, productsDTO.ExpiryDate, true);
                    defaultProductCalendarDetailDTOList.Add(defaultProductCalendarDetailDTO);
                    defaultProductCalendarDetailDTO = new ProductCalendarDetailContainerDTO(productsDTO.ExpiryDate, dateTimeRange.EndDateTime, false);
                    defaultProductCalendarDetailDTOList.Add(defaultProductCalendarDetailDTO);
                }
                else if(dateTimeRange.Contains(productsDTO.StartDate) && dateTimeRange.Contains(productsDTO.ExpiryDate) == false)
                {
                    defaultProductCalendarDetailDTO = new ProductCalendarDetailContainerDTO(dateTimeRange.StartDateTime, productsDTO.StartDate, false);
                    defaultProductCalendarDetailDTOList.Add(defaultProductCalendarDetailDTO);
                    defaultProductCalendarDetailDTO = new ProductCalendarDetailContainerDTO(productsDTO.StartDate, dateTimeRange.EndDateTime, true);
                    defaultProductCalendarDetailDTOList.Add(defaultProductCalendarDetailDTO);
                }
                else
                {
                    defaultProductCalendarDetailDTO = new ProductCalendarDetailContainerDTO(dateTimeRange.StartDateTime, dateTimeRange.EndDateTime, true);
                    defaultProductCalendarDetailDTOList.Add(defaultProductCalendarDetailDTO);
                }
                defaultProductCalendarDTO = new ProductCalendarContainerDTO(productsDTO.ProductId, productsDTO.Guid, defaultProductCalendarDetailDTOList);
                return defaultProductCalendarDTO;
            }
            ProductCalendarCalculator productCalendarCalculator = new ProductCalendarCalculator(productsDTO.ProductsCalenderDTOList);
            List<ProductCalendarDetailContainerDTO> productCalendarDetailDTOList = new List<ProductCalendarDetailContainerDTO>();
            List<DateTime> dateTimesInRange = dateTimeRange.GetDateTimesInRange(increment).ToList();
            List<bool> availabilityInRange = new List<bool>();
            foreach (var value in dateTimesInRange)
            {
                if ((productsDTO.ExpiryDate == DateTime.MinValue ||
                    value <= productsDTO.ExpiryDate) &&
                    (productsDTO.StartDate == DateTime.MinValue ||
                    value >= productsDTO.StartDate))
                {
                    availabilityInRange.Add(productCalendarCalculator.IsProductAvailableOn(value));
                }
                else
                {
                    availabilityInRange.Add(false);
                }
            }
            DateTime startDateTime = dateTimesInRange[0];
            DateTime endDateTime;
            bool available = availabilityInRange[0];
            for (int i = 1; i < dateTimesInRange.Count; i++)
            {
                if (availabilityInRange[i] != available)
                {
                    ProductCalendarDetailContainerDTO ProductCalendarDetailDTO = new ProductCalendarDetailContainerDTO(startDateTime, dateTimesInRange[i], available);
                    productCalendarDetailDTOList.Add(ProductCalendarDetailDTO);
                    startDateTime = dateTimesInRange[i];
                    available = availabilityInRange[i];
                }
            }
            endDateTime = dateTimesInRange.Last();
            ProductCalendarDetailContainerDTO lastProductCalendarDetailDTO = new ProductCalendarDetailContainerDTO(startDateTime, endDateTime, available);
            productCalendarDetailDTOList.Add(lastProductCalendarDetailDTO);
            ProductCalendarContainerDTO result = new ProductCalendarContainerDTO(productsDTO.ProductId, productsDTO.Guid, productCalendarDetailDTOList);
            log.LogMethodExit(result);
            return result;
        }

        public ProductCalendarContainerDTOCollection GetProductCalendarDTOCollection()
        {
            log.LogMethodEntry();
            log.LogMethodExit(productCalendarContainerDTOCollection);
            return productCalendarContainerDTOCollection;
        }

        public bool IsProductAvailable(int productId, DateTime dateTime)
        {
            log.LogMethodEntry(productId, dateTime);
            if(productIdProductCalendarDTODictionary.ContainsKey(productId) == false)
            {
                log.LogMethodExit(false, "Unable to find productCalendarDetailDTO");
                return false;
            }
            ProductCalendarContainerDTO productCalendarContainerDTO = GetProductCalendarContainerDTO(productId);
            foreach (var productCalendarDetailContainerDTO in productCalendarContainerDTO.ProductCalendarDetailContainerDTOList)
            {
                if (dateTime >= productCalendarDetailContainerDTO.StartDateTime && dateTime < productCalendarDetailContainerDTO.EndDateTime)
                {
                    log.LogMethodExit(productCalendarDetailContainerDTO.Available);
                    return productCalendarDetailContainerDTO.Available;
                }
            }
            log.LogMethodExit(false, "Unable to find productCalendarDetailDTO");
            return false;
        }

        public ProductCalendarContainerDTO GetProductCalendarContainerDTO(int productId)
        {
            log.LogMethodEntry(productId);
            if (productIdProductCalendarDTODictionary.ContainsKey(productId) == false)
            {
                string message = MessageContainerList.GetMessage(siteId, -1, 2196, "Product", productId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new Exception(message);
            }
            ProductCalendarContainerDTO result = productIdProductCalendarDTODictionary[productId];
            log.LogMethodExit(result);
            return result;
        }
    }
}
