/********************************************************************************************
 * Project Name - ModifierSetDetails BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 *********************************************************************************************
 *2.40       17-Sep-2018      Indhu               Created 
 *2.60       18-Feb-2019      Muhammed Mehraj     Modified Added SaveUpdateModifierSetDetailsList()
 *2.60       26-Apr-2019      Akshay G            modified SaveUpdateModifierSetDetailsList(),Save() and added GetModifierSetDetailsList()
 *2.70       28-Jun-2019      Akshay G            Added DeleteModifierSetDetails() method
  *2.110.00    27-Nov-2020     Abhishek            Modified : Modified to 3 Tier Standard 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Product
{
    public class ModifierSetDetailsBL
    {
        private ModifierSetDetailsDTO modifierSetDetailsDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        private ModifierSetDetailsBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        ///Constructor will fetch the ModifierSetDetails DTO based on the modifierSetDetails id passed 
        /// </summary>
        /// <param name="modifierSetDetailId">ModifierSetDetails id</param>
        public ModifierSetDetailsBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            ModifierSetDetailsDataHandler modifierSetDetailsDataHandler = new ModifierSetDetailsDataHandler(sqlTransaction);
            modifierSetDetailsDTO = modifierSetDetailsDataHandler.GetModifierSetDetailsDTO(id);
            if (modifierSetDetailsDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, " Modifier Set Details ", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit(modifierSetDetailsDTO);
        }

        /// <summary>
        /// Creates modifierSetDetails object using the ModifierSetDetailsDTO
        /// </summary>
        /// <param name="modifierSetDetail">ModifierSetDetailsDTO object</param>
        public ModifierSetDetailsBL(ExecutionContext executionContext, ModifierSetDetailsDTO modifierSetDetailsDTO)
              : this(executionContext)
        {
            log.LogMethodEntry(modifierSetDetailsDTO);
            this.modifierSetDetailsDTO = modifierSetDetailsDTO;
            log.LogMethodExit();
        }


        /// Saves the modifierSetDetails record
        /// Checks if the ModifierSetDetailsId is not less 0
        ///     If it is less than 0, then inserts
        ///     else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
         
            ModifierSetDetailsDataHandler modifierSetDetailsDataHandler = new ModifierSetDetailsDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate(sqlTransaction);
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (modifierSetDetailsDTO.ModifierSetDetailId < 0)
            {
                modifierSetDetailsDTO = modifierSetDetailsDataHandler.Insert(modifierSetDetailsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                modifierSetDetailsDTO.AcceptChanges();
            }
            else
            {
                if (modifierSetDetailsDTO.IsChanged == true)
                {
                    modifierSetDetailsDTO = modifierSetDetailsDataHandler.Update(modifierSetDetailsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    modifierSetDetailsDTO.AcceptChanges();
                }
            }
            if (modifierSetDetailsDTO.ParentModifierDTOList != null && modifierSetDetailsDTO.ParentModifierDTOList.Count > 0)
            {
                foreach (ParentModifierDTO parentModifierDTO in modifierSetDetailsDTO.ParentModifierDTOList)
                {
                    ParentModifierBL parentModifierBL = new ParentModifierBL(executionContext, parentModifierDTO);
                    parentModifierBL.Save(sqlTransaction);
                }
            }
            log.LogMethodExit();
        }



        /// <summary>
        /// Delete ModifierSetDetails details based on id
        /// </summary>
        /// <param name="id">id</param>        
        public void Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {
                ModifierSetDetailsDataHandler modifierSetDetailsDataHandler = new ModifierSetDetailsDataHandler(sqlTransaction);
                modifierSetDetailsDataHandler.Delete(modifierSetDetailsDTO.ModifierSetDetailId);
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
        /// Gets the ModifierSetDetailDTO
        /// </summary>
        public ModifierSetDetailsDTO getModifierSetDetailDTO { get { return modifierSetDetailsDTO; } }
    }

    /// <summary>
    /// Manages the list of modifierSetDetails
    /// </summary>
    public class ModifierSetDetailsList
    {
        private List<ModifierSetDetailsDTO> modifierSetDetailsDTOList;
        private ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Default constructor
        /// </summary>
        public ModifierSetDetailsList(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// parameterized Constructor with executionContext and modifierSetDetailsDTOList
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="modifierSetDetailsDTOList"></param>
        public ModifierSetDetailsList(ExecutionContext executionContext, List<ModifierSetDetailsDTO> modifierSetDetailsDTOList)
           : this(executionContext)
            {
            log.LogMethodEntry(executionContext, modifierSetDetailsDTOList);
            this.modifierSetDetailsDTOList = modifierSetDetailsDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the modifierSetDetails list
        /// </summary>
        public List<ModifierSetDetailsDTO> GetModifierSetDetailList(List<KeyValuePair<ModifierSetDetailsDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ModifierSetDetailsDataHandler modifierSetDetailsDataHandler = new ModifierSetDetailsDataHandler(sqlTransaction);
            List<ModifierSetDetailsDTO> modifierSetDetailsDTOList = modifierSetDetailsDataHandler.GetModifierSetDetails(searchParameters);
            log.LogMethodExit(modifierSetDetailsDTOList);
            return modifierSetDetailsDTOList;
        }

        /// <summary>
        /// Gets the modifierSetDetails List along with child based on loadChildRecords
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="loadChildRecords"></param>
        /// <returns></returns>
        public List<ModifierSetDetailsDTO> GetModifierSetDetailsList(List<KeyValuePair<ModifierSetDetailsDTO.SearchByParameters, string>> searchParameters, bool loadChildRecords = false)
        {
            log.LogMethodEntry(searchParameters, loadChildRecords);
            ModifierSetDetailsDataHandler modifierSetDetailsDataHandler = new ModifierSetDetailsDataHandler();
            modifierSetDetailsDTOList = modifierSetDetailsDataHandler.GetModifierSetDetails(searchParameters);
            if (loadChildRecords && modifierSetDetailsDTOList != null && modifierSetDetailsDTOList.Count != 0)
            {
                foreach (ModifierSetDetailsDTO modifierSetDetailsDTO in modifierSetDetailsDTOList)
                {
                    List<KeyValuePair<ParentModifierDTO.SearchByParameters, string>> searchParentModifierParameters = new List<KeyValuePair<ParentModifierDTO.SearchByParameters, string>>();
                    searchParentModifierParameters.Add(new KeyValuePair<ParentModifierDTO.SearchByParameters, string>(ParentModifierDTO.SearchByParameters.SITE_ID, modifierSetDetailsDTO.Site_Id.ToString()));
                    searchParentModifierParameters.Add(new KeyValuePair<ParentModifierDTO.SearchByParameters, string>(ParentModifierDTO.SearchByParameters.MODIFIERID, modifierSetDetailsDTO.ModifierSetDetailId.ToString()));
                    ParentModifierList parentModifierList = new ParentModifierList(executionContext);
                    List<ParentModifierDTO> parentModifierDTOList = parentModifierList.GetParentModifierDTOList(searchParentModifierParameters);
                    if (parentModifierDTOList != null && parentModifierDTOList.Count > 0)
                    {
                        modifierSetDetailsDTO.ParentModifierDTOList = new List<ParentModifierDTO>(parentModifierDTOList);
                        modifierSetDetailsDTO.AcceptChanges();
                    }
                }
            }
            log.LogMethodExit(modifierSetDetailsDTOList);
            return modifierSetDetailsDTOList;
        }

        /// <summary>
        /// Insert and update Modifiersetdetail object
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            try
            {
                log.LogMethodEntry(sqlTransaction);
                if (modifierSetDetailsDTOList != null)
                {
                    foreach (ModifierSetDetailsDTO modifierSetDetailsDTO in modifierSetDetailsDTOList)
                    {
                        List<ValidationError> validationErrors = Validate(modifierSetDetailsDTO, sqlTransaction);
                        if (validationErrors.Any())
                        {
                            string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                            log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                            throw new ValidationException(message, validationErrors);
                        }
                        ModifierSetDetailsBL modifierSetDetailsBL = new ModifierSetDetailsBL(executionContext, modifierSetDetailsDTO);
                        modifierSetDetailsBL.Save(sqlTransaction);
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
        /// Gets the ModifierSetDetailsDTO List modifierSetIdList List
        /// </summary>
        /// <param name="modifierSetIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of ModifierSetDetailsDTO</returns>
        public List<ModifierSetDetailsDTO> GetModifierSetDetailsDTOListForModifierSet(List<int> modifierSetIdList, bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(modifierSetIdList, activeRecords, sqlTransaction);
            ModifierSetDetailsDataHandler modifierSetDetailsDataHandler = new ModifierSetDetailsDataHandler(sqlTransaction);
            List<ModifierSetDetailsDTO> modifierSetDetailsDTOList = modifierSetDetailsDataHandler.GetModifierSetDetailsDTOList(modifierSetIdList, activeRecords);
            log.LogMethodExit(modifierSetDetailsDTOList);
            return modifierSetDetailsDTOList;
        }

        /// <summary>
        /// Validates the MembershipExclusionRuleDTO 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        public List<ValidationError> Validate(ModifierSetDetailsDTO modifierSetDetailsDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            log.LogMethodEntry(sqlTransaction);
            List<KeyValuePair<ModifierSetDetailsDTO.SearchByParameters, string>> searchParameter = new List<KeyValuePair<ModifierSetDetailsDTO.SearchByParameters, string>>();
            searchParameter.Add(new KeyValuePair<ModifierSetDetailsDTO.SearchByParameters, string>(ModifierSetDetailsDTO.SearchByParameters.ISACTIVE, "1"));
            searchParameter.Add(new KeyValuePair<ModifierSetDetailsDTO.SearchByParameters, string>(ModifierSetDetailsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<ModifierSetDetailsDTO> modifierSetDetailsDTOList = GetModifierSetDetailsList(searchParameter);
            if (modifierSetDetailsDTO.ModifierSetDetailId > -1 && modifierSetDetailsDTO.IsActive == false)
            {
                if (modifierSetDetailsDTOList != null && modifierSetDetailsDTOList.Any(x => x.ParentId == modifierSetDetailsDTO.ModifierSetDetailId))
                {
                    log.Debug("Active parent modifier exists for this Modifier Set Details");
                    validationErrorList.Add(new ValidationError("MOdifierSetDetails", "ParentModifierId", MessageContainerList.GetMessage(executionContext, 4750)));
                }
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }
    }
}
