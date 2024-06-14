/********************************************************************************************
 * Project Name - ModifierSet BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 *********************************************************************************************
 *2.40        17-Sep-2018      Indhu               Created 
 *2.60        18-Feb-2019      Muhammed Mehraj     Modified Added  SaveUpdateModifierSetList()
 *2.60        26-Apr-2019      Akshay G            modified 
 *2.70        28-Jun-2019      Nagesh Badiger      Added DeleteModifierSet() and DeleteModifierSetList() methods
 *                             Akshay G            modified DeleteModifierSet() and DeleteModifierSetList() methods
 *2.110.00    26-Nov-2020      Abhishek            Modified : Modified to 3 Tier Standard
 ********************************************************************************************/
using System;
using System.Linq;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Product
{
    public class ModifierSetBL
    {
        private ModifierSetDTO modifierSetDTO;
        private PurchasedModifierSet purchasedModifierSet;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// parameterized constructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        private ModifierSetBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        ///Constructor will fetch the ModifierSet DTO based on the modifierSet id passed 
        /// </summary>
        /// <param name="modifierSetId">ModifierSetDTO id</param>
        /// <param name="activeChildRecords">Only active records are loaded</param>
        /// <param name="loadChildRecords">Loads the child records</param>
        public ModifierSetBL(int modifierSetId, ExecutionContext executionContext, bool loadChildRecords = false, bool activeChildRecords = false,
                             SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(modifierSetId, executionContext, loadChildRecords, activeChildRecords, sqlTransaction);
            ModifierSetDataHandler modifierSetDataHandler = new ModifierSetDataHandler(sqlTransaction);
            modifierSetDTO = modifierSetDataHandler.GetModifierSetDTO(modifierSetId);
            if (modifierSetDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, " Modifier Set", modifierSetId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (modifierSetDTO != null && loadChildRecords)
            {
                ModifierSetDetailsList modifierSetDetailsList = new ModifierSetDetailsList(executionContext);
                List<KeyValuePair<ModifierSetDetailsDTO.SearchByParameters, string>> searchAddressParams = new List<KeyValuePair<ModifierSetDetailsDTO.SearchByParameters, string>>();
                searchAddressParams.Add(new KeyValuePair<ModifierSetDetailsDTO.SearchByParameters, string>(ModifierSetDetailsDTO.SearchByParameters.MODIFIER_SET_ID, modifierSetDTO.ModifierSetId.ToString()));
                if (activeChildRecords)
                {
                    searchAddressParams.Add(new KeyValuePair<ModifierSetDetailsDTO.SearchByParameters, string>(ModifierSetDetailsDTO.SearchByParameters.ISACTIVE, "Y"));
                }
                this.modifierSetDTO.ModifierSetDetailsDTO = modifierSetDetailsList.GetModifierSetDetailList(searchAddressParams, sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates modifierSet object using the ModifierSetDTO
        /// </summary>
        /// <param name="modifierSet">ModifierSetDTO object</param>
        public ModifierSetBL(ExecutionContext executionContext, ModifierSetDTO modifierSetDTO)
            : this(executionContext)
        {
            log.LogMethodEntry();
            this.modifierSetDTO = modifierSetDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the modifierSet record
        /// Checks if the ModifierSetDTOId is not less 0
        ///     If it is less than 0, then inserts
        ///     else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (modifierSetDTO.IsChangedRecursive == false
               && modifierSetDTO.ModifierSetId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            ModifierSetDataHandler modifierSetDataHandler = new ModifierSetDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate(sqlTransaction);
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (modifierSetDTO.ModifierSetId < 0)
            {
                modifierSetDTO = modifierSetDataHandler.Insert(modifierSetDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                modifierSetDTO.AcceptChanges();
            }
            else
            {
                if (modifierSetDTO.IsChanged == true)
                {
                    modifierSetDTO = modifierSetDataHandler.Update(modifierSetDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    modifierSetDTO.AcceptChanges();
                }
            }
            if (modifierSetDTO.ModifierSetDetailsDTO != null && modifierSetDTO.ModifierSetDetailsDTO.Count > 0)
            {
                foreach (ModifierSetDetailsDTO modifierSetDetailsDTO in modifierSetDTO.ModifierSetDetailsDTO)
                {
                    modifierSetDetailsDTO.ModifierSetId = modifierSetDTO.ModifierSetId;

                }
                ModifierSetDetailsList modifierSetDetailsList = new ModifierSetDetailsList(executionContext, modifierSetDTO.ModifierSetDetailsDTO);
                modifierSetDetailsList.Save(sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the ModifierSet and ModifierSet details based on ModifierSetId
        /// </summary>
        /// <param name="ModifierSetId"></param>        
        public void Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {
                ModifierSetDataHandler modifierSetDataHandler = new ModifierSetDataHandler(sqlTransaction);
                modifierSetDataHandler.Delete(modifierSetDTO.ModifierSetId);
            }
            catch (ValidationException valEx)
            {
                log.Error(valEx);
                log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                throw;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the MembershipExclusionRuleDTO 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ModifierSetDTO GetModifierSetDTO { get { return modifierSetDTO; } }

        /// <summary>
        /// Gets the Purchased Modifier Set
        /// </summary>
        /// <param name="modifierSetDTO"></param>
        /// <returns></returns>
        public void PurchasedModifierSet(ModifierSetDTO modifierSetDTO)
        {
            log.LogMethodEntry(modifierSetDTO);
            purchasedModifierSet = new PurchasedModifierSet();
            purchasedModifierSet.ModifierSetId = modifierSetDTO.ModifierSetId;
            purchasedModifierSet.ParentModifierSetId = modifierSetDTO.ParentModifierSetId;
            purchasedModifierSet.SetName = modifierSetDTO.SetName;
            purchasedModifierSet.MinQuantity = modifierSetDTO.MinQuantity;
            purchasedModifierSet.MaxQuantity = modifierSetDTO.MaxQuantity;
            purchasedModifierSet.FreeQuantity = modifierSetDTO.FreeQuantity;
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public PurchasedModifierSet GetPurchasedModifierSet { get { return purchasedModifierSet; } }
    }

    /// <summary>
    /// Manages the list of modifierSet
    /// </summary>
    public class ModifierSetDTOList
    {
        private List<ModifierSetDTO> modifierSetDTOList;
        private ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor with executionContext
        /// </summary>
        public ModifierSetDTOList(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parmeterised constructor with 2 params
        /// </summary>
        /// <param name="modifierSets"></param>
        /// <param name="executionContext"></param>
        public ModifierSetDTOList(ExecutionContext executionContext, List<ModifierSetDTO> modifierSetDTOList)
         : this(executionContext)
        {
            log.LogMethodEntry(executionContext, modifierSetDTOList);
            this.modifierSetDTOList = modifierSetDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the modifierSet list
        /// </summary>
        public List<ModifierSetDTO> GetAllModifierSetDTOList(List<KeyValuePair<ModifierSetDTO.SearchByParameters, string>> searchParameters,
                                                           bool loadChildRecords = false, bool loadActiveChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            ModifierSetDataHandler modifierSetDataHandler = new ModifierSetDataHandler(sqlTransaction);
            modifierSetDTOList = modifierSetDataHandler.GetModifierSetList(searchParameters);
            if (loadChildRecords && modifierSetDTOList != null && modifierSetDTOList.Count != 0)
            {
                foreach (ModifierSetDTO modifierSetDTO in modifierSetDTOList)
                {
                    ModifierSetDetailsList modifierSetDetailsBL = new ModifierSetDetailsList(executionContext);
                    List<KeyValuePair<ModifierSetDetailsDTO.SearchByParameters, string>> searchByModifierSetDetailsParameters = new List<KeyValuePair<ModifierSetDetailsDTO.SearchByParameters, string>>();
                    searchByModifierSetDetailsParameters.Add(new KeyValuePair<ModifierSetDetailsDTO.SearchByParameters, string>(ModifierSetDetailsDTO.SearchByParameters.SITE_ID, modifierSetDTO.Site_Id.ToString()));
                    searchByModifierSetDetailsParameters.Add(new KeyValuePair<ModifierSetDetailsDTO.SearchByParameters, string>(ModifierSetDetailsDTO.SearchByParameters.MODIFIER_SET_ID, modifierSetDTO.ModifierSetId.ToString()));
                    if (loadActiveChildRecords)
                    {
                        searchByModifierSetDetailsParameters.Add(new KeyValuePair<ModifierSetDetailsDTO.SearchByParameters, string>(ModifierSetDetailsDTO.SearchByParameters.ISACTIVE, "1"));
                    }
                    List<ModifierSetDetailsDTO> modifierSetDetailsDTOList = modifierSetDetailsBL.GetModifierSetDetailsList(searchByModifierSetDetailsParameters, loadChildRecords);
                    if (modifierSetDetailsDTOList != null)
                    {
                        modifierSetDTO.ModifierSetDetailsDTO = new List<ModifierSetDetailsDTO>(modifierSetDetailsDTOList);
                        modifierSetDTO.AcceptChanges();
                    }
                }
            }
            log.LogMethodExit(modifierSetDTOList);
            return modifierSetDTOList;
        }

        /// <summary>
        /// Insert and update Modifierset  and Modifiersetdetails collection
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            try
            {
                log.LogMethodEntry(sqlTransaction);
                if (modifierSetDTOList != null)
                {
                    foreach (ModifierSetDTO modifierSetDTO in modifierSetDTOList)
                    {
                        List<ValidationError> validationErrors = Validate(modifierSetDTO, sqlTransaction);
                        if (validationErrors.Any())
                        {
                            string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                            log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                            throw new ValidationException(message, validationErrors);
                        }
                        ModifierSetBL modifierSetBL = new ModifierSetBL(executionContext, modifierSetDTO);
                        modifierSetBL.Save(sqlTransaction);
                    }
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Delete the Modifier SetList
        /// </summary>
        public void Delete()
        {
            log.LogMethodEntry();
            if (modifierSetDTOList != null && modifierSetDTOList.Count > 0)
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (ModifierSetDTO modifierSetDTO in modifierSetDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            if (modifierSetDTO.ModifierSetDetailsDTO != null && modifierSetDTO.ModifierSetDetailsDTO.Count != 0)
                            {
                                foreach (ModifierSetDetailsDTO modifierSetDetailsDTO in modifierSetDTO.ModifierSetDetailsDTO)
                                {
                                    foreach (ParentModifierDTO parentModifierDTO in modifierSetDetailsDTO.ParentModifierDTOList)
                                    {
                                        if (parentModifierDTO.IsChanged)
                                        {
                                            ParentModifierBL parentModifierBL = new ParentModifierBL(executionContext, parentModifierDTO);
                                            parentModifierBL.Delete(parafaitDBTrx.SQLTrx);
                                        }
                                    }
                                    if (modifierSetDetailsDTO.IsChanged && modifierSetDetailsDTO.IsActive == false)
                                    {
                                        ModifierSetDetailsBL modifierSetDetailsBL = new ModifierSetDetailsBL(executionContext, modifierSetDetailsDTO);
                                        modifierSetDetailsBL.Delete(parafaitDBTrx.SQLTrx);
                                    }
                                }
                            }
                            if (modifierSetDTO.IsChanged && modifierSetDTO.IsActive == false)
                            {
                                ModifierSetBL modifierSetBL = new ModifierSetBL(executionContext, modifierSetDTO);
                                modifierSetBL.Delete(parafaitDBTrx.SQLTrx);
                            }
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            log.Error(valEx);
                            parafaitDBTrx.RollBack();
                            log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                            throw;
                        }
                        catch (SqlException sqlEx)
                        {
                            log.Error(sqlEx);
                            parafaitDBTrx.RollBack();
                            log.LogMethodExit(null, "Throwing SQL Exception : " + sqlEx.Message);
                            if (sqlEx.Number == 547)
                            {
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869)); //Unable to delete this record.Please check the reference record first.
                            }
                            else
                            {
                                throw;
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex.Message);
                            parafaitDBTrx.RollBack();
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw;
                        }
                    }
                }
                log.LogMethodExit();
            }
        }

        public DateTime? GetModifierSetModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            ModifierSetDataHandler modifierSetDataHandler = new ModifierSetDataHandler(null);
            DateTime? result = modifierSetDataHandler.GetModifierSetModuleLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the ModifierSetDTO List for product Id List
        /// </summary>
        /// <param name="modifierSetIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of ProductDTO</returns>
        public List<ModifierSetDTO> GetModifierSetDTOListForModifierSet(List<int> modifierSetIdList, bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(modifierSetIdList, activeRecords, sqlTransaction);
            ModifierSetDataHandler modifierSetDataHandler = new ModifierSetDataHandler(sqlTransaction);
            List<ModifierSetDTO> modifierSetDTOList = modifierSetDataHandler.GetModifierSetDTOList(modifierSetIdList, activeRecords);
            Build(modifierSetDTOList, activeRecords, sqlTransaction);
            log.LogMethodExit(modifierSetDTOList);
            return modifierSetDTOList;
        }

        /// <summary>
        /// Validates the MembershipExclusionRuleDTO 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        public List<ValidationError> Validate(ModifierSetDTO modifierSetDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            List<KeyValuePair<ModifierSetDTO.SearchByParameters, string>> searchParameter = new List<KeyValuePair<ModifierSetDTO.SearchByParameters, string>>();
            searchParameter.Add(new KeyValuePair<ModifierSetDTO.SearchByParameters, string>(ModifierSetDTO.SearchByParameters.ISACTIVE, "1"));
            searchParameter.Add(new KeyValuePair<ModifierSetDTO.SearchByParameters, string>(ModifierSetDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<ModifierSetDTO> modifierSetDTOList = GetAllModifierSetDTOList(searchParameter);
            if (modifierSetDTO.ModifierSetId > -1 && modifierSetDTO.IsActive == false)
            {
                if (modifierSetDTOList != null && modifierSetDTOList.Any(x => x.ParentModifierSetId == modifierSetDTO.ModifierSetId))
                {
                    log.Debug("Unable to delete. This is a Parent Modifier.");
                    validationErrorList.Add(new ValidationError("MOdifierSet", "ParentModifierId", MessageContainerList.GetMessage(executionContext, 4749)));
                }
            }
            if (modifierSetDTO.ModifierSetId < 0 && modifierSetDTOList != null && modifierSetDTOList.Any(x => x.SetName == modifierSetDTO.SetName))
            {
                log.Debug("Duplicate Set Name");
                validationErrorList.Add(new ValidationError("MOdifierSet", "SetName", MessageContainerList.GetMessage(executionContext, 2608, "Set")));
            }
            if (modifierSetDTO.ModifierSetId > -1 && modifierSetDTOList != null && modifierSetDTOList.Any(x => x.SetName == modifierSetDTO.SetName && x.ModifierSetId != modifierSetDTO.ModifierSetId))
            {
                log.Debug("Duplicate Set Name");
                validationErrorList.Add(new ValidationError("MOdifierSet", "SetName", MessageContainerList.GetMessage(executionContext, 2608, "Set")));
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        // <summary>
        /// Builds the List of ModifierSetDetails object based on the list of ModifierSet id.
        /// </summary>
        /// <param name="modifierSetDTOList"></param>
        /// <param name="activeChildRecords"></param>
        /// <param name="sqlTransaction"></param>
        private void Build(List<ModifierSetDTO> modifierSetDTOs, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(modifierSetDTOs, activeChildRecords, sqlTransaction);
            Dictionary<int, ModifierSetDTO> modifierSetDTOProductsIdMap = new Dictionary<int, ModifierSetDTO>();
            List<int> modifierSetIdList = new List<int>();
            for (int i = 0; i < modifierSetDTOs.Count; i++)
            {
                if (modifierSetDTOProductsIdMap.ContainsKey(modifierSetDTOs[i].ModifierSetId))
                {
                    continue;
                }
                modifierSetDTOProductsIdMap.Add(modifierSetDTOs[i].ModifierSetId, modifierSetDTOs[i]);
                modifierSetIdList.Add(modifierSetDTOs[i].ModifierSetId);
            }
            ModifierSetDetailsList modifierSetDetailsList = new ModifierSetDetailsList(executionContext);
            List<ModifierSetDetailsDTO> modifierSetDetailsDTOLists = modifierSetDetailsList.GetModifierSetDetailsDTOListForModifierSet(modifierSetIdList, activeChildRecords, sqlTransaction);
            if (modifierSetDetailsDTOLists != null && modifierSetDetailsDTOLists.Any())
            {
                foreach (var modifierSetDetailsDTO in modifierSetDetailsDTOLists)
                {

                    if (modifierSetDTOProductsIdMap.ContainsKey(modifierSetDetailsDTO.ModifierSetId) == false)
                    {
                        continue;
                    }
                    ModifierSetDTO modifierSetDTO = modifierSetDTOProductsIdMap[modifierSetDetailsDTO.ModifierSetId];
                    if (modifierSetDTO.ModifierSetDetailsDTO == null)
                    {
                        modifierSetDTO.ModifierSetDetailsDTO = new List<ModifierSetDetailsDTO>();
                    }
                    modifierSetDTO.ModifierSetDetailsDTO.Add(modifierSetDetailsDTO);
                }
            }
        }
    }
}
