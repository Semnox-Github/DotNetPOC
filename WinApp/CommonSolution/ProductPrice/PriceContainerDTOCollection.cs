/********************************************************************************************
 * Project Name - ProductPrice
 * Description  - Represents a collection of PriceContainerDTO and a hash of the same collection
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.130       18-Aug-2021   Lakshminarayana         Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.ProductPrice
{
    /// <summary>
    /// Represents a collection of PriceContainerDTO and a hash of the same collection
    /// </summary>
    public class PriceContainerDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<PriceContainerDTO> priceContainerDTOList;
        private Dictionary<int, PriceContainerDTO> priceContainerDTODictionary = new Dictionary<int, PriceContainerDTO>();
        private string hash;

        /// <summary>
        /// Default constructor
        /// </summary>
        public PriceContainerDTOCollection()
        {
            log.LogMethodEntry();
            priceContainerDTOList = new List<PriceContainerDTO>();
            priceContainerDTODictionary = new Dictionary<int, PriceContainerDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="priceContainerDTOList"></param>
        public PriceContainerDTOCollection(List<PriceContainerDTO> priceContainerDTOList)
        {
            log.LogMethodEntry(priceContainerDTOList);
            this.priceContainerDTOList = priceContainerDTOList;
            if (priceContainerDTOList == null)
            {
                priceContainerDTOList = new List<PriceContainerDTO>();
            }
            PopulatePriceContainerDTODictionary();
            hash = new DtoListHash(priceContainerDTOList);
            log.LogMethodExit();
        }

        private void PopulatePriceContainerDTODictionary()
        {
            log.LogMethodEntry(priceContainerDTOList);
            if(priceContainerDTOList != null)
            {
                foreach (var priceContainerDTO in priceContainerDTOList)
                {
                    if (priceContainerDTODictionary.ContainsKey(priceContainerDTO.ProductId))
                    {
                        continue;
                    }
                    priceContainerDTODictionary.Add(priceContainerDTO.ProductId, priceContainerDTO);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the matching price container dto
        /// </summary>
        public PriceContainerDTO GetPriceContainerDTO(int productId)
        {
            log.LogMethodEntry(productId);
            if (priceContainerDTODictionary.ContainsKey(productId) == false)
            {
                string errorMessage = string.Format("Unable to find price container DTO for productId:{0} ", productId);
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            PriceContainerDTO result = priceContainerDTODictionary[productId];
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the matching price container dto
        /// </summary>
        public PriceContainerDetailDTO GetPriceContainerDetailDTO(int productId, DateTime dateTime)
        {
            log.LogMethodEntry(productId);
            PriceContainerDTO priceContainerDTO = GetPriceContainerDTO(productId);
            PriceContainerDetailDTO result = null;
            foreach (var priceContainerDetailDTO in priceContainerDTO.PriceContainerDetailDTOList)
            {
                if (dateTime >= priceContainerDetailDTO.StartDateTime && dateTime < priceContainerDetailDTO.EndDateTime)
                {
                    result = priceContainerDetailDTO;
                    break;
                }
            }
            if (result == null)
            {
                string errorMessage = string.Format("Unable to find price container detail DTO for productId: {0} on datetime {1}", productId, dateTime.ToString(CultureInfo.InvariantCulture));
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Get/Set price container dto list
        /// </summary>
        public List<PriceContainerDTO> PriceContainerDTOList
        {
            get
            {
                return priceContainerDTOList;
            }

            set
            {
                priceContainerDTOList = value;
                PopulatePriceContainerDTODictionary();
            }
        }

        /// <summary>
        /// Get/Set method of hash
        /// </summary>
        public string Hash
        {
            get
            {
                return hash;
            }
            set
            {
                hash = value;
            }
        }

    }
}
