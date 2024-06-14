/********************************************************************************************
 * Project Name - Products
 * Description  - TaxContainer class to get the List of Tax.
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.150.0      18-Jan-2021      Prajwal S                 Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Semnox.Parafait.Product
{
    public class TaxContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Dictionary<int, TaxContainerDTO> taxIdTaxContainerDTODictionary = new Dictionary<int, TaxContainerDTO>();
        private readonly List<TaxDTO> taxDTOList;
        private readonly TaxContainerDTOCollection taxContainerDTOCollection;
        private readonly DateTime? taxModuleLastUpdateTime;
        private readonly int siteId;
        
        /// <summary>
        /// Default Container Constructor
        /// </summary>
        /// <param name="siteId"></param>
        public TaxContainer(int siteId) : this(siteId, GetTaxDTOList(siteId), GetTaxModuleLastUpdateTime(siteId))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with parameters siteId, taxDTOList, taxModuleLastUpdateTime
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="taxDTOList"></param>
        /// <param name="taxModuleLastUpdateTime"></param>
        public TaxContainer(int siteId, List<TaxDTO> taxDTOList, DateTime? taxModuleLastUpdateTime)
        {
            log.LogMethodEntry(siteId, taxDTOList, taxModuleLastUpdateTime);
            this.siteId = siteId;
            this.taxDTOList = taxDTOList;
            this.taxModuleLastUpdateTime = taxModuleLastUpdateTime;
            List<TaxContainerDTO> taxContainerDTOList = new List<TaxContainerDTO>();

            Dictionary<int, TaxStructureDTO> taxStructureIdTaxStructureDTODictionary = GetTaxStructureIdTaxStructureDTODictionary(taxDTOList);
            

            foreach (TaxDTO taxDTO in taxDTOList)
            {
                if (taxIdTaxContainerDTODictionary.ContainsKey(taxDTO.TaxId))
                {
                    continue;
                }
                TaxContainerDTO taxContainerDTO = GetTaxContainerDTO(taxDTO, taxStructureIdTaxStructureDTODictionary);

                taxContainerDTOList.Add(taxContainerDTO);
                taxIdTaxContainerDTODictionary.Add(taxContainerDTO.TaxId, taxContainerDTO);
            }
            taxContainerDTOCollection = new TaxContainerDTOCollection(taxContainerDTOList);
            log.LogMethodExit();
        }

        private static TaxContainerDTO GetTaxContainerDTO(TaxDTO taxDTO, Dictionary<int, TaxStructureDTO> taxStructureIdTaxStructureDTODictionary)
        {
            TaxContainerDTO taxContainerDTO = new TaxContainerDTO(taxDTO.TaxId, taxDTO.TaxName, taxDTO.TaxPercentage, taxDTO.Attribute1, taxDTO.Attribute2, taxDTO.Attribute3, taxDTO.Attribute4, taxDTO.Attribute5, taxDTO.Guid);
            if (taxDTO.TaxStructureDTOList != null && taxDTO.TaxStructureDTOList.Any())
            {
                taxContainerDTO.TaxStructureContainerDTOList = new List<TaxStructureContainerDTO>();
                foreach (TaxStructureDTO taxStructureDTO in taxDTO.TaxStructureDTOList)
                {
                    //int level = GetTaxStructureIdLevel(taxStructureDTO.TaxStructureId, taxStructureIdTaxStructureDTODictionary);
                    decimal cumulativePercenatge = GetTaxStructureCumulativePercenatge(taxStructureDTO.TaxStructureId, taxStructureIdTaxStructureDTODictionary);
                    //decimal effectivePercenatge = cumulativePercenatge / (decimal)Math.Pow(100, level);
                    TaxStructureContainerDTO taxStructureContainerDTO = new TaxStructureContainerDTO(taxStructureDTO.TaxStructureId, taxStructureDTO.TaxId, taxStructureDTO.StructureName, taxStructureDTO.Percentage, taxStructureDTO.ParentStructureId,
                                                                                                     cumulativePercenatge, taxStructureDTO.Guid);
                    taxContainerDTO.TaxStructureContainerDTOList.Add(taxStructureContainerDTO);
                }
            }

            return taxContainerDTO;
        }


        private static int GetTaxStructureIdLevel(int taxStructureId, Dictionary<int, TaxStructureDTO> taxStructureIdTaxStructureDTODictionary)
        {
            log.LogMethodEntry(taxStructureId, taxStructureIdTaxStructureDTODictionary);
            int result = 0;
            if(taxStructureIdTaxStructureDTODictionary.ContainsKey(taxStructureId) &&
                taxStructureIdTaxStructureDTODictionary[taxStructureId].ParentStructureId >= 0)
            {
                int parentLevel = GetTaxStructureIdLevel(taxStructureIdTaxStructureDTODictionary[taxStructureId].ParentStructureId, taxStructureIdTaxStructureDTODictionary);
                result = parentLevel + 1;
            }
            log.LogMethodExit(result);
            return result;
        }

        private static decimal GetTaxStructureCumulativePercenatge(int taxStructureId, Dictionary<int, TaxStructureDTO> taxStructureIdTaxStructureDTODictionary)
        {
            log.LogMethodEntry(taxStructureId, taxStructureIdTaxStructureDTODictionary);
            decimal result = 0;
            if(taxStructureIdTaxStructureDTODictionary.ContainsKey(taxStructureId) == false)
            {
                log.LogMethodExit(result, "Unable to find a tax structure id : " + taxStructureId);
                return result;
            }
            TaxStructureDTO taxStructureDTO = taxStructureIdTaxStructureDTODictionary[taxStructureId];
            if (taxStructureDTO.ParentStructureId >= 0)
            {
                decimal parentCumulativePercenatge = GetTaxStructureCumulativePercenatge(taxStructureIdTaxStructureDTODictionary[taxStructureId].ParentStructureId, taxStructureIdTaxStructureDTODictionary);
                result = Math.Round(parentCumulativePercenatge * (decimal)taxStructureDTO.Percentage / 100m, 4);
            }
            else
            {
                result = (decimal)taxStructureDTO.Percentage;
            }
            log.LogMethodExit(result);
            return result;
        }

        private static Dictionary<int, TaxStructureDTO> GetTaxStructureIdTaxStructureDTODictionary(List<TaxDTO> taxDTOList)
        {
            log.LogMethodEntry(taxDTOList);
            Dictionary<int, TaxStructureDTO> result = new Dictionary<int, TaxStructureDTO>();
            foreach (var taxDTO in taxDTOList)
            {
                if (taxDTO.TaxStructureDTOList == null)
                {
                    continue;
                }
                foreach (var taxStructureDTO in taxDTO.TaxStructureDTOList)
                {
                    if (result.ContainsKey(taxStructureDTO.TaxStructureId))
                    {
                        continue;
                    }
                    result.Add(taxStructureDTO.TaxStructureId, taxStructureDTO);
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Get the latest update time of Tax table from DB.
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        private static DateTime? GetTaxModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            DateTime? result = null;
            try
            {
                TaxList taxListBL = new TaxList();
                result = taxListBL.GetTaxModuleLastUpdateTime(siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the Tax max last update date.", ex);
                result = null;
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Get all the active Tax records for the given siteId.
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        private static List<TaxDTO> GetTaxDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            List<TaxDTO> taxDTOList = null;
            try
            {
                TaxList taxListBL = new TaxList();
                List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>> searchParameters = new List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>>();
                searchParameters.Add(new KeyValuePair<TaxDTO.SearchByTaxParameters, string>(TaxDTO.SearchByTaxParameters.SITE_ID, siteId.ToString()));
                searchParameters.Add(new KeyValuePair<TaxDTO.SearchByTaxParameters, string>(TaxDTO.SearchByTaxParameters.ACTIVE_FLAG, "1"));
                taxDTOList = taxListBL.GetAllTaxes(searchParameters, true, true);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the Tax.", ex);
            }

            if (taxDTOList == null)
            {
                taxDTOList = new List<TaxDTO>();
            }
            log.LogMethodExit(taxDTOList);
            return taxDTOList;
        }

        /// <summary>
        /// Returns taxContainerDTOCollection.
        /// </summary>
        /// <returns></returns>
        public TaxContainerDTOCollection GetTaxContainerDTOCollection()
        {
            log.LogMethodEntry();
            log.LogMethodExit(taxContainerDTOCollection);
            return taxContainerDTOCollection;
        }

        public TaxContainerDTO GetTaxContainerDTO(int taxId)
        {
            log.LogMethodEntry(taxId);
            if (taxIdTaxContainerDTODictionary.ContainsKey(taxId) == false)
            {
                string errorMessage = "Tax with tax Id :" + taxId + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            TaxContainerDTO result = taxIdTaxContainerDTODictionary[taxId]; ;
            log.LogMethodExit(result);
            return result;
        }


        /// <summary>
        /// Refresh the container if there is any update in Db.
        /// </summary>
        /// <returns></returns>
        public TaxContainer Refresh()
        {
            log.LogMethodEntry();
            TaxList taxListBL = new TaxList();
            DateTime? updateTime = taxListBL.GetTaxModuleLastUpdateTime(siteId);
            if (taxModuleLastUpdateTime.HasValue
                && taxModuleLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in Tax since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            TaxContainer result = new TaxContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
