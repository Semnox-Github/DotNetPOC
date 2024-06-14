/********************************************************************************************
 * Project Name - PrizeMachineFilter DTO
 * Description  - Data object of PrizeMachineFilter
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *1.00        16-May-2017   Lakshminarayana          Created 
 *2.70.2        01-Aug-2019   Deeksha                  Modified:Added log() methods.
 *2.120.0      24-Feb-2021   Mushahid Faizan           Handled Execution Context.
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using Semnox.Parafait.Game;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Inventory;

namespace Semnox.Parafait.Redemption
{
    /// <summary>
    /// Buissness object class has methods to calculte prize stoking information
    /// </summary>
    public class PrizeMachineInfoBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext machineUserContext;
        private Dictionary<int, ProductDTO> products;

        private const string DISPENSING_CATEGORY = "Dispensing category";
        private const string REDEMPTION_TICKETS = "Redemption Tickets";
        private const string SINGLE_VALUE_PRIZES = "Single value Prizes";
        private const string MULTI_VALUE_PRIZES = "Multi-Value Prizes";
        private const string PRIZE_EVERY_TIME = "Prize Every Time";
        private const string STANDARD_PUSHER = "Standard Pusher";
        private const string TICKET_PUSHER = "Ticket Pusher";
        public PrizeMachineInfoBL()
        {
            log.LogMethodEntry();
            machineUserContext = ExecutionContext.GetExecutionContext();
            products = new Dictionary<int, ProductDTO>();
            log.LogMethodExit();
        }

        private string ValidatePrizeMachineFilterDTO(PrizeMachineFilterDTO prizeMachineFilterDTO)
        {
            log.LogMethodEntry(prizeMachineFilterDTO);
            string message = string.Empty;
            if(prizeMachineFilterDTO == null)
            {
                message = "'prizeMachineFilterDTO' is null.";
            }
            if(prizeMachineFilterDTO.StartOfPeriod == DateTime.MinValue)
            {
                message = "'StartOfPeriod' should be greater than min value.";
            }
            if(prizeMachineFilterDTO.EndOfPeriod == DateTime.MinValue)
            {
                message = "'EndOfPeriod' should be greater than min value.";
            }
            if(prizeMachineFilterDTO.StartOfPeriod > prizeMachineFilterDTO.EndOfPeriod)
            {
                message = "'EndOfPeriod' should be greater than 'StartOfPeriod'.";
            }
            if(string.IsNullOrWhiteSpace(prizeMachineFilterDTO.DispenseCategory) == false)
            {
                if((string.Equals(prizeMachineFilterDTO.DispenseCategory, REDEMPTION_TICKETS) ||
                    string.Equals(prizeMachineFilterDTO.DispenseCategory, SINGLE_VALUE_PRIZES) ||
                    string.Equals(prizeMachineFilterDTO.DispenseCategory, MULTI_VALUE_PRIZES) ||
                    string.Equals(prizeMachineFilterDTO.DispenseCategory, PRIZE_EVERY_TIME) ||
                    string.Equals(prizeMachineFilterDTO.DispenseCategory, STANDARD_PUSHER)) == false)
                {
                    message = "'DispenseCategory' is not valid.";
                }
            }
            log.LogMethodExit(message);
            return message;
        }


        /// <summary>
        /// Returns list of PrizeMachineInfoDTOs based on the prizeMachineFilterDTO.
        /// </summary>
        /// <param name="prizeMachineFilterDTO">prizeMachineFilterDTO</param>
        /// <returns>prizeMachineInfoDTOList</returns>
        public List<PrizeMachineInfoDTO> GetPrizeMachineInfoDTOList(PrizeMachineFilterDTO prizeMachineFilterDTO)
        {
            log.LogMethodEntry(prizeMachineFilterDTO);
            List<PrizeMachineInfoDTO> prizeMachineInfoDTOList = new List<PrizeMachineInfoDTO>();
            string validationError = ValidatePrizeMachineFilterDTO(prizeMachineFilterDTO);
            if(string.IsNullOrEmpty(validationError) == false)
            {
                throw new InvalidAurgumentException(validationError);
            }
            if((string.IsNullOrWhiteSpace(prizeMachineFilterDTO.DispenseCategory) &&  (prizeMachineFilterDTO.MachineList == null || prizeMachineFilterDTO.MachineList.Count == 0)) ||
                string.Equals(prizeMachineFilterDTO.DispenseCategory, REDEMPTION_TICKETS))
            {
                PrizeMachineInfoDTO prizeMachineInfoDTO = GetPrizeMachineInfoDTOForRedemption(prizeMachineFilterDTO);
                if(prizeMachineInfoDTO != null)
                {
                    prizeMachineInfoDTOList.Add(prizeMachineInfoDTO);
                }
            }
            if((string.IsNullOrWhiteSpace(prizeMachineFilterDTO.DispenseCategory) && (prizeMachineFilterDTO.MachineList == null || prizeMachineFilterDTO.MachineList.Count == 0)) ||
               string.Equals(prizeMachineFilterDTO.DispenseCategory, STANDARD_PUSHER))
            {
                PrizeMachineInfoDTO prizeMachineInfoDTO = GetPrizeMachineInfoForStandardPusher(prizeMachineFilterDTO);
                if(prizeMachineInfoDTO != null)
                {
                    prizeMachineInfoDTOList.Add(prizeMachineInfoDTO);
                }
            }
            if(prizeMachineFilterDTO.MachineList != null && prizeMachineFilterDTO.MachineList.Count > 0)
            {
                foreach(var item in prizeMachineFilterDTO.MachineList)
                {
                    PrizeMachineInfoDTO prizeMachineInfoDTO = GetPrizeMachineInfoForExternalMachineReference(item, prizeMachineFilterDTO.StartOfPeriod, prizeMachineFilterDTO.EndOfPeriod);
                    if(prizeMachineInfoDTO != null)
                    {
                        prizeMachineInfoDTOList.Add(prizeMachineInfoDTO);
                    }
                }
            }
            else
            {
                CompareInfo compareInfo = CultureInfo.InvariantCulture.CompareInfo;
                List<MachineDTO> machineDTOList = new List<MachineDTO>();
                if(string.IsNullOrWhiteSpace(prizeMachineFilterDTO.DispenseCategory) == false)
                {
                    if(compareInfo.Compare(prizeMachineFilterDTO.DispenseCategory, SINGLE_VALUE_PRIZES, CompareOptions.IgnoreCase) == 0 ||
                        compareInfo.Compare(prizeMachineFilterDTO.DispenseCategory, MULTI_VALUE_PRIZES, CompareOptions.IgnoreCase) == 0 ||
                        compareInfo.Compare(prizeMachineFilterDTO.DispenseCategory, PRIZE_EVERY_TIME, CompareOptions.IgnoreCase) == 0)
                    {
                        UpdatePrizeMachineInfoDTOListForDispenseCategory(prizeMachineInfoDTOList, prizeMachineFilterDTO.DispenseCategory, prizeMachineFilterDTO.StartOfPeriod, prizeMachineFilterDTO.EndOfPeriod);
                    }
                }
                else
                {
                    UpdatePrizeMachineInfoDTOListForDispenseCategory(prizeMachineInfoDTOList, SINGLE_VALUE_PRIZES, prizeMachineFilterDTO.StartOfPeriod, prizeMachineFilterDTO.EndOfPeriod);
                    UpdatePrizeMachineInfoDTOListForDispenseCategory(prizeMachineInfoDTOList, MULTI_VALUE_PRIZES, prizeMachineFilterDTO.StartOfPeriod, prizeMachineFilterDTO.EndOfPeriod);
                    UpdatePrizeMachineInfoDTOListForDispenseCategory(prizeMachineInfoDTOList, PRIZE_EVERY_TIME, prizeMachineFilterDTO.StartOfPeriod, prizeMachineFilterDTO.EndOfPeriod);
                } 
            }
            log.LogMethodExit(prizeMachineInfoDTOList);
            return prizeMachineInfoDTOList;
        }

        private void UpdatePrizeMachineInfoDTOListForDispenseCategory(List<PrizeMachineInfoDTO> prizeMachineInfoDTOList, string dispenceCategory, DateTime fromDate, DateTime toDate)
        {
            List<MachineDTO>  machineDTOList = GetMachinesOfDispenceCategory(dispenceCategory);
            if(machineDTOList != null && machineDTOList.Count > 0)
            {
                foreach(var machineDTO in machineDTOList)
                {
                    PrizeMachineInfoDTO prizeMachineInfoDTO = new PrizeMachineInfoDTO();
                    prizeMachineInfoDTO.DispenseCategory = dispenceCategory;
                    prizeMachineInfoDTO.MachineRef = machineDTO.ExternalMachineReference;
                    UpdatePrizeMachineInfoDTOForLocation(machineDTO.InventoryLocationId, prizeMachineInfoDTO, fromDate, toDate);
                    prizeMachineInfoDTO.Status = "VALID";
                    prizeMachineInfoDTOList.Add(prizeMachineInfoDTO);
                }
            }
        }

        private PrizeMachineInfoDTO GetPrizeMachineInfoDTOForRedemption(PrizeMachineFilterDTO prizeMachineFilterDTO)
        {
            log.LogMethodEntry(prizeMachineFilterDTO);
            PrizeMachineInfoDTO prizeMachineInfoDTO = new PrizeMachineInfoDTO();
            prizeMachineInfoDTO.Status = "VALID";
            prizeMachineInfoDTO.DispenseCategory = REDEMPTION_TICKETS;
            RedemptionListBL redemptionBL = new RedemptionListBL();
            List<KeyValuePair<RedemptionDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<RedemptionDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.FROM_REDEMPTION_DATE, prizeMachineFilterDTO.StartOfPeriod.ToString("yyyy/MM/dd HH:mm:ss")));
            searchParameters.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.TO_REDEMPTION_DATE, prizeMachineFilterDTO.EndOfPeriod.ToString("yyyy/MM/dd HH:mm:ss")));
            searchParameters.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.SITE_ID, Convert.ToString(machineUserContext.GetSiteId())));
            searchParameters.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.FETCH_GIFT_REDEMPTIONS_ONLY, "Y"));
            List<RedemptionDTO> redemptionDTOList = redemptionBL.GetRedemptionDTOList(searchParameters);
            int totalNoOfTickets = 0;
            double totalCostOfGifts = 0;
            if((redemptionDTOList != null) && (redemptionDTOList.Any()))
            {
                foreach(var redemptionDTO in redemptionDTOList)
                {
                    RedemptionGiftsListBL redemptionGiftsListBL = new RedemptionGiftsListBL(machineUserContext);
                    List<KeyValuePair<RedemptionGiftsDTO.SearchByParameters, string>> searchRedemptionGiftsParameters = new List<KeyValuePair<RedemptionGiftsDTO.SearchByParameters, string>>();
                    searchRedemptionGiftsParameters.Add(new KeyValuePair<RedemptionGiftsDTO.SearchByParameters, string>(RedemptionGiftsDTO.SearchByParameters.REDEMPTION_ID, Convert.ToString(redemptionDTO.RedemptionId)));
                    searchRedemptionGiftsParameters.Add(new KeyValuePair<RedemptionGiftsDTO.SearchByParameters, string>(RedemptionGiftsDTO.SearchByParameters.SITE_ID, Convert.ToString(machineUserContext.GetSiteId())));
                    List<RedemptionGiftsDTO> redemptionGiftsDTOList = redemptionGiftsListBL.GetRedemptionGiftsDTOList(searchRedemptionGiftsParameters);
                    if (redemptionGiftsDTOList != null)
                    {
                        foreach (var redemptionGiftsDTO in redemptionGiftsDTOList)
                        {
                            if (redemptionGiftsDTO.Tickets != null)
                            {
                                if (redemptionGiftsDTO.Tickets >= 0)
                                {
                                    totalNoOfTickets += (int)redemptionGiftsDTO.Tickets;
                                    if (redemptionGiftsDTO.ProductId >= 0)
                                    {
                                        ProductDTO productDTO = GetProductDTO(redemptionGiftsDTO.ProductId);
                                        if (productDTO != null)
                                        {
                                            totalCostOfGifts += productDTO.Cost;
                                        }
                                    }
                                }
                                else
                                {
                                    totalNoOfTickets += (int)redemptionGiftsDTO.Tickets;
                                    if (redemptionGiftsDTO.ProductId >= 0)
                                    {
                                        ProductDTO productDTO = GetProductDTO(redemptionGiftsDTO.ProductId);
                                        if (productDTO != null)
                                        {
                                            totalCostOfGifts -= productDTO.Cost;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                prizeMachineInfoDTO.TicketCount = totalNoOfTickets;
                prizeMachineInfoDTO.StockValue = Convert.ToDecimal(totalCostOfGifts);
            }
            log.LogMethodExit(prizeMachineInfoDTO);
            return prizeMachineInfoDTO;
        }

        private PrizeMachineInfoDTO GetPrizeMachineInfoForExternalMachineReference(string externalMachineReference, DateTime fromDate, DateTime toDate)
        {
            log.LogMethodEntry(externalMachineReference, fromDate, toDate);
            PrizeMachineInfoDTO prizeMachineInfoDTO = new PrizeMachineInfoDTO();
            prizeMachineInfoDTO.MachineRef = externalMachineReference;
            prizeMachineInfoDTO.Status = "INVALIDMACHINE";
            MachineList machineListBL = new MachineList();
            List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>> searchByMachineParameters = new List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>>();
            searchByMachineParameters.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.EXTERNAL_MACHINE_REFERENCE, externalMachineReference));
            List<MachineDTO> machineDTOList = machineListBL.GetMachineList(searchByMachineParameters);
            if(machineDTOList != null && machineDTOList.Count > 0)
            {
                MachineDTO machineDTO = machineDTOList[0];
                if(machineDTO.CustomDataSetId >= 0)
                {
                    CustomAttributesListBL customAttributesListBL = new CustomAttributesListBL(machineUserContext);
                    List<KeyValuePair<CustomAttributesDTO.SearchByParameters, string>> searchByCustomAttributesParameters = new List<KeyValuePair<CustomAttributesDTO.SearchByParameters, string>>();
                    searchByCustomAttributesParameters.Add(new KeyValuePair<CustomAttributesDTO.SearchByParameters, string>(CustomAttributesDTO.SearchByParameters.NAME, DISPENSING_CATEGORY));
                    List<CustomAttributesDTO> customAttributesDTOList = customAttributesListBL.GetCustomAttributesDTOList(searchByCustomAttributesParameters);
                    int dispenseCategoryAttributeId = -1;
                    CompareInfo compareInfo = CultureInfo.InvariantCulture.CompareInfo;
                    if(customAttributesDTOList != null && customAttributesDTOList.Count > 0)
                    {
                        if(customAttributesDTOList.Count == 1)
                        {
                            dispenseCategoryAttributeId = customAttributesDTOList[0].CustomAttributeId;
                        }
                        else
                        {
                            foreach(var item in customAttributesDTOList)
                            {
                                if(compareInfo.Compare(item.Name, DISPENSING_CATEGORY, CompareOptions.IgnoreCase) == 0)
                                {
                                    dispenseCategoryAttributeId = item.CustomAttributeId;
                                }
                            }
                        }
                    }
                    if(dispenseCategoryAttributeId != -1)
                    {

                        CustomAttributeValueListListBL customAttributeValueListListBL = new CustomAttributeValueListListBL(machineUserContext);
                        List<KeyValuePair<CustomAttributeValueListDTO.SearchByParameters, string>> searchByCustomAttributeValueListParameters = new List<KeyValuePair<CustomAttributeValueListDTO.SearchByParameters, string>>();
                        searchByCustomAttributeValueListParameters.Add(new KeyValuePair<CustomAttributeValueListDTO.SearchByParameters, string>(CustomAttributeValueListDTO.SearchByParameters.CUSTOM_ATTRIBUTE_ID, dispenseCategoryAttributeId.ToString()));
                        List<CustomAttributeValueListDTO> customAttributeValueListDTOList = customAttributeValueListListBL.GetCustomAttributeValueListDTOList(searchByCustomAttributeValueListParameters);
                        if(customAttributeValueListDTOList != null && customAttributeValueListDTOList.Count > 0)
                        {
                            CustomDataListBL customDataListBL = new CustomDataListBL(machineUserContext);
                            List<KeyValuePair<CustomDataDTO.SearchByParameters, string>> searchByCustomDataParameters = new List<KeyValuePair<CustomDataDTO.SearchByParameters, string>>();
                            searchByCustomDataParameters.Add(new KeyValuePair<CustomDataDTO.SearchByParameters, string>(CustomDataDTO.SearchByParameters.CUSTOM_DATA_SET_ID, machineDTO.CustomDataSetId.ToString()));
                            searchByCustomDataParameters.Add(new KeyValuePair<CustomDataDTO.SearchByParameters, string>(CustomDataDTO.SearchByParameters.CUSTOM_ATTRIBUTE_ID, dispenseCategoryAttributeId.ToString()));
                            List<CustomDataDTO> customDataDTOList = customDataListBL.GetCustomDataDTOList(searchByCustomDataParameters);
                            if(customDataDTOList != null && customDataDTOList.Count == 1)
                            {
                                foreach(var item in customAttributeValueListDTOList)
                                {
                                    if(customDataDTOList[0].ValueId == item.ValueId)
                                    {
                                        prizeMachineInfoDTO.DispenseCategory = item.Value;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        prizeMachineInfoDTO.Status = "INVALIDCONFIG";
                    }

                    if(string.IsNullOrEmpty(prizeMachineInfoDTO.DispenseCategory) == false &&
                       compareInfo.Compare(prizeMachineInfoDTO.DispenseCategory, STANDARD_PUSHER, CompareOptions.IgnoreCase) != 0 &&
                       compareInfo.Compare(prizeMachineInfoDTO.DispenseCategory, TICKET_PUSHER, CompareOptions.IgnoreCase) != 0)
                    {
                        UpdatePrizeMachineInfoDTOForLocation(machineDTO.InventoryLocationId, prizeMachineInfoDTO, fromDate, toDate);
                        prizeMachineInfoDTO.Status = "VALID";
                    }
                    else
                    {
                        prizeMachineInfoDTO.Status = "INVALIDCONFIG";
                    }
                }
                else
                {
                    prizeMachineInfoDTO.Status = "INVALIDCONFIG";
                }
            }

            log.LogMethodExit(prizeMachineInfoDTO);
            return prizeMachineInfoDTO;
        }

        private PrizeMachineInfoDTO GetPrizeMachineInfoForStandardPusher(PrizeMachineFilterDTO prizeMachineFilterDTO)
        {
            log.LogMethodEntry(prizeMachineFilterDTO);
            PrizeMachineInfoDTO prizeMachineInfoDTO = new PrizeMachineInfoDTO();
            prizeMachineInfoDTO.DispenseCategory = STANDARD_PUSHER;
            prizeMachineInfoDTO.Status = "INVALID";
            List<int> locations = new List<int>();
            List<MachineDTO> machineDTOList = new List<MachineDTO>();
            List<MachineDTO> standardPusherMachineDTOList = GetMachinesOfDispenceCategory(STANDARD_PUSHER);
            List<MachineDTO> ticketPusherMachineDTOList = GetMachinesOfDispenceCategory(TICKET_PUSHER);
            if(standardPusherMachineDTOList != null && standardPusherMachineDTOList.Count > 0)
            {
                machineDTOList.AddRange(standardPusherMachineDTOList);
            }
            if(ticketPusherMachineDTOList != null && ticketPusherMachineDTOList.Count > 0)
            {
                machineDTOList.AddRange(ticketPusherMachineDTOList);
            }
            if(machineDTOList != null && machineDTOList.Count > 0)
            {
                foreach(var machineDTO in machineDTOList)
                {
                    if(locations.IndexOf(machineDTO.InventoryLocationId) < 0)
                    {
                        locations.Add(machineDTO.InventoryLocationId);
                    }
                }
                if(locations.Count > 0)
                {
                    foreach(var inventoryLocationId in locations)
                    {
                        UpdatePrizeMachineInfoDTOForLocation(inventoryLocationId, prizeMachineInfoDTO, prizeMachineFilterDTO.StartOfPeriod, prizeMachineFilterDTO.EndOfPeriod);
                    }
                }
                prizeMachineInfoDTO.MachineCount = machineDTOList.Count;
                prizeMachineInfoDTO.Status = "VALID";
            }
            log.LogMethodExit(prizeMachineInfoDTO);
            return prizeMachineInfoDTO;
        }

        private void UpdatePrizeMachineInfoDTOForLocation(int inventoryLocationId, PrizeMachineInfoDTO prizeMachineInfoDTO, DateTime fromDate, DateTime toDate)
        {
            log.LogMethodEntry(inventoryLocationId, prizeMachineInfoDTO, fromDate, toDate);
            decimal totalNoOfPrizes = 0;
            decimal totalCost = 0;
            if(inventoryLocationId >= 0 && prizeMachineInfoDTO != null)
            {
                InventoryAdjustmentsList inventoryAdjustmentsListBL = new InventoryAdjustmentsList(machineUserContext);
                List<KeyValuePair<InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters, string>> searchByInventoryAdjustmentsParameters = new List<KeyValuePair<InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters, string>>();
                searchByInventoryAdjustmentsParameters.Add(new KeyValuePair<InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters, string>(InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters.FROM_TIMESTAMP, fromDate.ToString("yyyy/MM/dd HH:mm:ss")));
                searchByInventoryAdjustmentsParameters.Add(new KeyValuePair<InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters, string>(InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters.TO_TIMESTAMP, toDate.ToString("yyyy/MM/dd HH:mm:ss")));
                searchByInventoryAdjustmentsParameters.Add(new KeyValuePair<InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters, string>(InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters.TO_LOCATION_ID, inventoryLocationId.ToString()));
                searchByInventoryAdjustmentsParameters.Add(new KeyValuePair<InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters, string>(InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters.ADJUSTMENT_TYPE, "Transfer"));
                List<InventoryAdjustmentsDTO> transferredToLocationInventoryAdjustmentsDTOList = inventoryAdjustmentsListBL.GetAllInventoryAdjustments(searchByInventoryAdjustmentsParameters);
                if(transferredToLocationInventoryAdjustmentsDTOList != null &&
                    transferredToLocationInventoryAdjustmentsDTOList.Count > 0)
                {
                    foreach(var inventoryAdjustmentsDTO in transferredToLocationInventoryAdjustmentsDTOList)
                    {
                        ProductDTO productDTO = GetProductDTO(inventoryAdjustmentsDTO.ProductId);
                        totalNoOfPrizes += (decimal)inventoryAdjustmentsDTO.AdjustmentQuantity;
                        if(productDTO != null)
                        {
                            totalCost += (decimal)inventoryAdjustmentsDTO.AdjustmentQuantity * (decimal)productDTO.Cost;
                        }
                    }
                }
                searchByInventoryAdjustmentsParameters = new List<KeyValuePair<InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters, string>>();
                searchByInventoryAdjustmentsParameters.Add(new KeyValuePair<InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters, string>(InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters.FROM_TIMESTAMP, fromDate.ToString("yyyy/MM/dd HH:mm:ss")));
                searchByInventoryAdjustmentsParameters.Add(new KeyValuePair<InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters, string>(InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters.TO_TIMESTAMP, toDate.ToString("yyyy/MM/dd HH:mm:ss")));
                searchByInventoryAdjustmentsParameters.Add(new KeyValuePair<InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters, string>(InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters.FROM_LOCATION_ID, inventoryLocationId.ToString()));
                searchByInventoryAdjustmentsParameters.Add(new KeyValuePair<InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters, string>(InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters.ADJUSTMENT_TYPE, "Adjustment"));
                List<InventoryAdjustmentsDTO> adjustedFromLocationInventoryAdjustmentsDTOList = inventoryAdjustmentsListBL.GetAllInventoryAdjustments(searchByInventoryAdjustmentsParameters);
                if(adjustedFromLocationInventoryAdjustmentsDTOList != null &&
                    adjustedFromLocationInventoryAdjustmentsDTOList.Count > 0)
                {
                    foreach(var inventoryAdjustmentsDTO in adjustedFromLocationInventoryAdjustmentsDTOList)
                    {
                        ProductDTO productDTO = GetProductDTO(inventoryAdjustmentsDTO.ProductId);
                        totalNoOfPrizes += (decimal)inventoryAdjustmentsDTO.AdjustmentQuantity;
                        if(productDTO != null)
                        {
                            totalCost += (decimal)inventoryAdjustmentsDTO.AdjustmentQuantity * (decimal)productDTO.Cost;
                        }
                    }
                }


                searchByInventoryAdjustmentsParameters = new List<KeyValuePair<InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters, string>>();
                searchByInventoryAdjustmentsParameters.Add(new KeyValuePair<InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters, string>(InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters.FROM_TIMESTAMP, fromDate.ToString("yyyy/MM/dd HH:mm:ss")));
                searchByInventoryAdjustmentsParameters.Add(new KeyValuePair<InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters, string>(InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters.TO_TIMESTAMP, toDate.ToString("yyyy/MM/dd HH:mm:ss")));
                searchByInventoryAdjustmentsParameters.Add(new KeyValuePair<InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters, string>(InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters.FROM_LOCATION_ID, inventoryLocationId.ToString()));
                searchByInventoryAdjustmentsParameters.Add(new KeyValuePair<InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters, string>(InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters.ADJUSTMENT_TYPE, "Transfer"));
                List<InventoryAdjustmentsDTO> transferredFromLocationInventoryAdjustmentsDTOList = inventoryAdjustmentsListBL.GetAllInventoryAdjustments(searchByInventoryAdjustmentsParameters);
                if(transferredFromLocationInventoryAdjustmentsDTOList != null &&
                    transferredFromLocationInventoryAdjustmentsDTOList.Count > 0)
                {
                    foreach(var inventoryAdjustmentsDTO in transferredFromLocationInventoryAdjustmentsDTOList)
                    {
                        ProductDTO productDTO = GetProductDTO(inventoryAdjustmentsDTO.ProductId);
                        totalNoOfPrizes -= (decimal)inventoryAdjustmentsDTO.AdjustmentQuantity;
                        if(productDTO != null)
                        {
                            totalCost -= (decimal)inventoryAdjustmentsDTO.AdjustmentQuantity * (decimal)productDTO.Cost;
                        }
                    }
                }
                prizeMachineInfoDTO.StockQuantity = prizeMachineInfoDTO.StockQuantity + totalNoOfPrizes;
                prizeMachineInfoDTO.StockValue = prizeMachineInfoDTO.StockValue + totalCost;
            }
            log.LogMethodExit();
        }

        private List<MachineDTO> GetMachinesOfDispenceCategory(string dispenseCategory)
        {
            log.LogMethodEntry(dispenseCategory);
            List<MachineDTO> machineList = new List<MachineDTO>();
            CompareInfo compareInfo = CultureInfo.InvariantCulture.CompareInfo;
            CustomAttributesListBL customAttributesListBL = new CustomAttributesListBL(machineUserContext);
            List<KeyValuePair<CustomAttributesDTO.SearchByParameters, string>> searchCustomAttributesParameters = new List<KeyValuePair<CustomAttributesDTO.SearchByParameters, string>>();
            searchCustomAttributesParameters.Add(new KeyValuePair<CustomAttributesDTO.SearchByParameters, string>(CustomAttributesDTO.SearchByParameters.NAME, DISPENSING_CATEGORY));
            List<CustomAttributesDTO> customAttributesDTOList = customAttributesListBL.GetCustomAttributesDTOList(searchCustomAttributesParameters);
            if(customAttributesDTOList != null && customAttributesDTOList.Count > 0)
            {
                CustomAttributesDTO customAttributesDTO = null;
                if(customAttributesDTOList.Count == 1)
                {
                    customAttributesDTO = customAttributesDTOList[0];
                }
                else
                {
                    foreach(var item in customAttributesDTOList)
                    {
                        if(compareInfo.Compare(item.Name, DISPENSING_CATEGORY, CompareOptions.IgnoreCase) == 0)
                        {
                            customAttributesDTO = item;
                            break;
                        }
                    }
                }
                if(customAttributesDTO != null && customAttributesDTO.CustomAttributeId >= 0)
                {
                    int dispensingCategoryAttributeId = customAttributesDTO.CustomAttributeId;
                    CustomAttributeValueListListBL customAttributeValueListListBL = new CustomAttributeValueListListBL(machineUserContext);
                    List<KeyValuePair<CustomAttributeValueListDTO.SearchByParameters, string>> searchCustomAttributeValueListParameters = new List<KeyValuePair<CustomAttributeValueListDTO.SearchByParameters, string>>();
                    searchCustomAttributeValueListParameters.Add(new KeyValuePair<CustomAttributeValueListDTO.SearchByParameters, string>(CustomAttributeValueListDTO.SearchByParameters.CUSTOM_ATTRIBUTE_ID, dispensingCategoryAttributeId.ToString()));
                    searchCustomAttributeValueListParameters.Add(new KeyValuePair<CustomAttributeValueListDTO.SearchByParameters, string>(CustomAttributeValueListDTO.SearchByParameters.VALUE, dispenseCategory));
                    List<CustomAttributeValueListDTO> customAttributeValueListDTOList = customAttributeValueListListBL.GetCustomAttributeValueListDTOList(searchCustomAttributeValueListParameters);
                    if(customAttributeValueListDTOList != null && customAttributeValueListDTOList.Count > 0)
                    {
                        CustomAttributeValueListDTO customAttributeValueListDTO = null;
                        if(customAttributeValueListDTOList.Count == 1)
                        {
                            customAttributeValueListDTO = customAttributeValueListDTOList[0];
                        }
                        else
                        {
                            foreach(var item in customAttributeValueListDTOList)
                            {

                                if(compareInfo.Compare(item.Value, dispenseCategory, CompareOptions.IgnoreCase) == 0)
                                {
                                    customAttributeValueListDTO = item;
                                    break;
                                }
                            }
                        }
                        if(customAttributeValueListDTO != null && customAttributeValueListDTO.ValueId >= 0)
                        {
                            int dispencingCategoryValueId = customAttributeValueListDTO.ValueId;
                            CustomDataListBL customDataListBL = new CustomDataListBL(machineUserContext);
                            List<KeyValuePair<CustomDataDTO.SearchByParameters, string>> searchCustomDataParameters = new List<KeyValuePair<CustomDataDTO.SearchByParameters, string>>();
                            searchCustomDataParameters.Add(new KeyValuePair<CustomDataDTO.SearchByParameters, string>(CustomDataDTO.SearchByParameters.CUSTOM_ATTRIBUTE_ID, dispensingCategoryAttributeId.ToString()));
                            searchCustomDataParameters.Add(new KeyValuePair<CustomDataDTO.SearchByParameters, string>(CustomDataDTO.SearchByParameters.VALUE_ID, dispencingCategoryValueId.ToString()));
                            List<CustomDataDTO> customDataDTOList = customDataListBL.GetCustomDataDTOList(searchCustomDataParameters);
                            if(customDataDTOList != null && customDataDTOList.Count > 0)
                            {

                                MachineList machineListBL = new MachineList();
                                foreach(var customDataDTO in customDataDTOList)
                                {
                                    List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>> searchByMachineParameters = new List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>>();
                                    searchByMachineParameters.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.CUSTOM_DATA_SET_ID, customDataDTO.CustomDataSetId.ToString()));
                                    List<MachineDTO> machineDTOSearchResult = machineListBL.GetMachineList(searchByMachineParameters);
                                    if(machineDTOSearchResult != null && machineDTOSearchResult.Count > 0)
                                    {
                                        if(machineDTOSearchResult[0].InventoryLocationId >= 0)
                                        {
                                            machineList.Add(machineDTOSearchResult[0]);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            log.LogMethodExit(machineList);
            return machineList;
        }

        private ProductDTO GetProductDTO(int productId)
        {
            log.LogMethodEntry(productId);
            ProductDTO productDTO = null;
            if(productId >= 0)
            {
                if(products.ContainsKey(productId))
                {
                    productDTO = products[productId];
                }
                else
                {
                    ProductList productListBL = new ProductList();
                    List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> searchParameters = new List<KeyValuePair<ProductDTO.SearchByProductParameters, string>>();
                    searchParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.PRODUCT_ID, Convert.ToString(productId)));
                    List<ProductDTO> productDTOList = productListBL.GetAllProducts(searchParameters);
                    if(productDTOList != null)
                    {
                        if(productDTOList.Count > 0)
                        {
                            productDTO = productDTOList[0];
                        }
                    }
                    products.Add(productId, productDTO);
                }
            }
            log.LogMethodExit(productDTO);
            return productDTO;
        }
    }

    /// <summary>
    /// Represents invalid argument error that occur during application execution. 
    /// </summary>
    public class InvalidAurgumentException : Exception
    {
        /// <summary>
        /// Default constructor of InvalidAurgumentException.
        /// </summary>
        public InvalidAurgumentException()
        {
        }

        /// <summary>
        /// Initializes a new instance of InvalidAurgumentException class with a specified error message.
        /// </summary>
        /// <param name="message">message</param>
        public InvalidAurgumentException(string message)
        : base(message)
        {
        }
        /// <summary>
        /// Initializes a new instance of InvalidAurgumentException class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="inner">inner exception</param>
        public InvalidAurgumentException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
