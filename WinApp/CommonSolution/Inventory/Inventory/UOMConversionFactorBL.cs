/********************************************************************************************
 * Project Name - Inventory
 * Description  - BL object of UOMConversionFactor
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By         Remarks          
 *********************************************************************************************
 *2.100.0       24-Jul-2020   Deeksha             Created for Recipe Management enhancement.
 *2.120.0       11-May-2021   Mushahid Faizan     Modified for Web Inventory Enhancements
 *2.150.0       13-Dec-2022   Abhishek            Modified:Validate() as a part of Web Inventory Redesign. 
 *********************************************************************************************/
using System;
using System.Linq;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// UOM Conversion Factor BL
    /// </summary>
    public class UOMConversionFactorBL
    {
        private UOMConversionFactorDTO uomConversionFactorDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private UOMConversionFactorBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates UOMConversionFactorBL object using the UOMConversionFactorDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="uOMConversionFactorDTO">uOMConversionFactorDTO DTO object</param>
        public UOMConversionFactorBL(ExecutionContext executionContext, UOMConversionFactorDTO uomConversionFactorDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, uomConversionFactorDTO);
            this.uomConversionFactorDTO = uomConversionFactorDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the UOMConversionFactor  id as the parameter
        /// Would fetch the UOMConversionFactor object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="recipeEstimationDetailId">id -PromotionRule </param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public UOMConversionFactorBL(ExecutionContext executionContext, int UOMId,
                                  SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, UOMId, sqlTransaction);
            UOMConversionFactorDataHandler UOMConversionFactorDataHandler = new UOMConversionFactorDataHandler(sqlTransaction);
            uomConversionFactorDTO = UOMConversionFactorDataHandler.GetUOMConversionFactorId(UOMId);
            if (uomConversionFactorDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "UOMConversionFactorDTO", UOMId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the UOMConversionFactor DTO
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (uomConversionFactorDTO.IsChanged == false
                && uomConversionFactorDTO.UOMConversionFactorId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            UOMConversionFactorDataHandler uOMConversionFactorDataHandler = new UOMConversionFactorDataHandler(sqlTransaction);
            Validate(sqlTransaction);
            if (uomConversionFactorDTO.UOMConversionFactorId < 0)
            {
                uomConversionFactorDTO = uOMConversionFactorDataHandler.Insert(uomConversionFactorDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                uomConversionFactorDTO.AcceptChanges();
            }
            else if (uomConversionFactorDTO.IsChanged)
            {
                uomConversionFactorDTO = uOMConversionFactorDataHandler.Update(uomConversionFactorDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                uomConversionFactorDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the uomConversionFactorDTO 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        private void Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (uomConversionFactorDTO.UOMId <= -1 || uomConversionFactorDTO.BaseUOMId <= -1)
            {
                log.Error("Please select a valid entry from the dropdown list");
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1837, MessageContainerList.GetMessage(executionContext, "UOM"));
                throw new ValidationException(errorMessage);
            }
            if (uomConversionFactorDTO.UOMId > -1 && uomConversionFactorDTO.BaseUOMId > -1
                  && uomConversionFactorDTO.BaseUOMId == uomConversionFactorDTO.UOMId)
            {
                log.Error("Base UOM and Conversion UOM cannot be same");
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2773, MessageContainerList.GetMessage(executionContext, "UOM"));
                throw new ValidationException(errorMessage);
            }
            List<UOMConversionFactorDTO> uomConversionFactorDTOList = new List<UOMConversionFactorDTO>();
            UOMConversionFactorListBL uomConversionFactorListBL = new UOMConversionFactorListBL(executionContext);
            List<KeyValuePair<UOMConversionFactorDTO.SearchByParameters, string>> uomSearchParams = new List<KeyValuePair<UOMConversionFactorDTO.SearchByParameters, string>>();
            uomSearchParams.Add(new KeyValuePair<UOMConversionFactorDTO.SearchByParameters, string>(UOMConversionFactorDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            uomConversionFactorDTOList = uomConversionFactorListBL.GetUOMConversionFactorDTOList(uomSearchParams, sqlTransaction);
            if (uomConversionFactorDTO.UOMConversionFactorId <= -1 && (uomConversionFactorDTOList.Exists(x => x.BaseUOMId == uomConversionFactorDTO.BaseUOMId
                                    & x.UOMConversionFactorId > -1 & x.UOMId == uomConversionFactorDTO.UOMId)))
            {
                log.Debug("You cannot insert the duplicate record");
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1872, MessageContainerList.GetMessage(executionContext, "UOM"));
                throw new ValidationException(errorMessage);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public UOMConversionFactorDTO UOMConversionFactorDTO
        {
            get
            {
                return uomConversionFactorDTO;
            }
        }
    }

    /// <summary>
    /// UOM Conversion Factor List BL
    /// </summary>
    public class UOMConversionFactorListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<UOMConversionFactorDTO> uOMConversionFactorDTOList = new List<UOMConversionFactorDTO>();

        /// <summary>
        /// Parameterized constructor of UOMConversionFactorBL
        /// </summary>
        /// <param name="executionContext">executionContext object</param>
        public UOMConversionFactorListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="promotionRuleDTOList">UOMConversionFactor DTO List as parameter </param>
        public UOMConversionFactorListBL(ExecutionContext executionContext, List<UOMConversionFactorDTO> uOMConversionFactorDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, uOMConversionFactorDTOList);
            this.uOMConversionFactorDTOList = uOMConversionFactorDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Returns the Get the UOMConversionFactor DTO list based on the search parameter.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>The List of UOMConversionFactorDTO </returns>
        public List<UOMConversionFactorDTO> GetUOMConversionFactorDTOList(List<KeyValuePair<UOMConversionFactorDTO.SearchByParameters, string>> searchParameters,
                                                              SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            UOMConversionFactorDataHandler uOMConversionFactorDataHandler = new UOMConversionFactorDataHandler(sqlTransaction);
            List<UOMConversionFactorDTO> uOMConversionFactorDTOList = uOMConversionFactorDataHandler.GetUOMConversionFactorDTOList(searchParameters);
            log.LogMethodExit(uOMConversionFactorDTOList);
            return uOMConversionFactorDTOList;
        }

        /// <summary>
        /// Gets the UOMConversionFactorDTO List for UOMIdList
        /// </summary>
        /// <param name="UOMIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of UOMConversionFactorDTO</returns>
        public List<UOMConversionFactorDTO> GetUOMDTOListOfUoms(List<int> UOMIdList, bool activeRecords = true,
                                                                SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(UOMIdList, activeRecords, sqlTransaction);
            UOMConversionFactorDataHandler uOMConversionFactorDataHandler = new UOMConversionFactorDataHandler(sqlTransaction);
            List<UOMConversionFactorDTO> uomConversionFactorDTOList = uOMConversionFactorDataHandler.GetUOMConversionFactorDTOListOfUOM(UOMIdList, activeRecords);
            log.LogMethodExit(uomConversionFactorDTOList);
            return uomConversionFactorDTOList;
        }
        
        /// <summary>
        /// Gets the UOMConversionFactorDTO List for UOMIdList
        /// </summary>
        /// <param name="UOMIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of UOMConversionFactorDTO</returns>
        public List<UOMConversionFactorDTO> GetUOMConversionFactorDTOList(List<int> UOMIdList, bool activeRecords = true,
                                                                SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(UOMIdList, activeRecords, sqlTransaction);
            UOMConversionFactorDataHandler uOMConversionFactorDataHandler = new UOMConversionFactorDataHandler(sqlTransaction);
            List<UOMConversionFactorDTO> uomConversionFactorDTOList = uOMConversionFactorDataHandler.GetUOMConversionFactorDTOList(UOMIdList, activeRecords);
            log.LogMethodExit(uomConversionFactorDTOList);
            return uomConversionFactorDTOList;
        }

        /// <summary>
        /// Saves the  list of UOMConversionFactorDTOList DTO.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (uOMConversionFactorDTOList == null ||
                uOMConversionFactorDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < uOMConversionFactorDTOList.Count; i++)
            {
                UOMConversionFactorDTO uOMConversionFactorDTO = uOMConversionFactorDTOList[i];
                if (uOMConversionFactorDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    UOMConversionFactorBL uOMConversionFactorBL = new UOMConversionFactorBL(executionContext, uOMConversionFactorDTO);
                    uOMConversionFactorBL.Save(sqlTransaction);
                }
                catch (SqlException ex)
                {
                    log.Error(ex);
                    if (ex.Number == 2601)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1872));
                    }
                    else if (ex.Number == 547)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                    }
                    else
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, ex.Message));
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving UOMConversionFactorDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("UOMConversionFactorDTO", uOMConversionFactorDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

        public DateTime? GetUOMConversionModuleLastUpdateTime(int siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(siteId);
            UOMConversionFactorDataHandler uomConversionDataHandler = new UOMConversionFactorDataHandler(sqlTransaction);
            DateTime? result = uomConversionDataHandler.GetUOMConversionModuleLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }

        public double GetUOMConversionFactor(UOMDTO uomDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            double conversionFactor = 0;
            List<KeyValuePair<UOMConversionFactorDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<UOMConversionFactorDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<UOMConversionFactorDTO.SearchByParameters, string>(UOMConversionFactorDTO.SearchByParameters.BASE_UOM_ID, uomDTO.UOMId.ToString()));
            searchParameters.Add(new KeyValuePair<UOMConversionFactorDTO.SearchByParameters, string>(UOMConversionFactorDTO.SearchByParameters.IS_ACTIVE, "1"));
            uOMConversionFactorDTOList = GetUOMConversionFactorDTOList(searchParameters);
            if(uOMConversionFactorDTOList!=null && uOMConversionFactorDTOList.Any())
            {
                conversionFactor = uOMConversionFactorDTOList[0].ConversionFactor;
            }
            return conversionFactor;
        }
    }
}
