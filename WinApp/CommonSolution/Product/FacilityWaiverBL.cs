/********************************************************************************************
 * Project Name - Facility Waiver  BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        26-Sep-2019      Deeksha        Created 
 *2.70.2        17-Oct-2019      Dakshakh       Modified : Waiver-Phase-2 Enhancement, Added Validation messages 
 *2.80          10-May-2020      Girish Kundar  Modified: REST API Changes merge from WMS  
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Parafait.Product
{
    public class FacilityWaiverBL
    {
        private FacilityWaiverDTO facilityWaiverDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        /// <summary>
        /// Constructor with the FacilityWaiverBL id as the parameter
        /// Would fetch the FacilityWaiverBL object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="sqlTransaction">SqlTransaction</param>
        /// <param name="id">Id</param>
        public FacilityWaiverBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            this.executionContext = executionContext;
            FacilityWaiversDataHandler facilityWaiversDataHandler = new FacilityWaiversDataHandler(sqlTransaction);
            facilityWaiverDTO = facilityWaiversDataHandler.GetFacilityWaiverDTO(id);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates FacilityWaiverBL object using the FacilityWaiverDTO
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="facilityWaiverDTO">FacilityWaiverDTO object</param>
        public FacilityWaiverBL(ExecutionContext executionContext, FacilityWaiverDTO facilityWaiverDTO)
        {
            log.LogMethodEntry(executionContext, facilityWaiverDTO);
            this.executionContext = executionContext;
            this.facilityWaiverDTO = facilityWaiverDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// get FacilityWaiverDTO Object
        /// </summary>
        public FacilityWaiverDTO GetFacilityWaiverDTO
        {
            get { return facilityWaiverDTO; }
        }

        /// <summary>
        /// Validate method to validate facilityWaiverDTO data
        /// </summary>
        /// <returns></returns>
        public List<ValidationError> Validate()
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            string errorMessage = string.Empty;
            if (facilityWaiverDTO == null)
            {
                errorMessage = MessageContainerList.GetMessage(executionContext, 2330, (MessageContainerList.GetMessage(executionContext, "facilityWaiverDTO"))); //Cannot proceed facilityWaiverDTO record is Empty.
                validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext, "Customer"), MessageContainerList.GetMessage(executionContext, "facilityWaiverDTO"), errorMessage));
            }
            if (facilityWaiverDTO.WaiverSetId < 0)
            {
                errorMessage = MessageContainerList.GetMessage(executionContext, 2331, (MessageContainerList.GetMessage(executionContext, "facilityWaiverDTO"))); //Select Waiver Set before proceeding
                validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext, "Customer"), MessageContainerList.GetMessage(executionContext, "facilityWaiverDTO"), errorMessage));
            }
            DateTime fromDateValidation = DateTime.Now;
            if (facilityWaiverDTO.EffectiveFrom != null && DateTime.TryParse(((DateTime)facilityWaiverDTO.EffectiveFrom).ToString("yyyy-MM-dd HH:mm:ss"), out fromDateValidation) == false)
            {
                errorMessage = MessageContainerList.GetMessage(executionContext, 2332, (MessageContainerList.GetMessage(executionContext, "facilityWaiverDTO"))); //Select Valid EffectiveFrom Date before proceeding
                validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext, "Customer"), MessageContainerList.GetMessage(executionContext, "facilityWaiverDTO"), errorMessage));
            }
            DateTime toDateValidation = DateTime.Now;
            if (facilityWaiverDTO.EffectiveTo != null && (DateTime.TryParse(((DateTime)facilityWaiverDTO.EffectiveTo).ToString("yyyy-MM-dd HH:mm:ss"), out toDateValidation) == false
                                                            || facilityWaiverDTO.EffectiveTo < facilityWaiverDTO.EffectiveFrom))
            {
                errorMessage = MessageContainerList.GetMessage(executionContext, 2333, (MessageContainerList.GetMessage(executionContext, "facilityWaiverDTO"))); //Select Valid EffectiveTo Date before proceeding
                validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext, "Customer"), MessageContainerList.GetMessage(executionContext, "facilityWaiverDTO"), errorMessage));
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Saves the CustomerSignedWaiver
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            FacilityWaiversDataHandler facilityWaiversDataHandler = new FacilityWaiversDataHandler(sqlTransaction);
            Validate();
            if (facilityWaiverDTO.FacilityWaiverId < 0)
            {
                facilityWaiverDTO = facilityWaiversDataHandler.InsertFacilityWaiver(facilityWaiverDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                facilityWaiverDTO.AcceptChanges();
            }
            else
            {
                if (facilityWaiverDTO.IsChanged)
                {
                    facilityWaiverDTO = facilityWaiversDataHandler.UpdateFacilityWaiver(facilityWaiverDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    facilityWaiverDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Called when facility is deleted - this is the child list so it should be deleted
        /// </summary>
        /// <param name="sqlTransaction"></param>
        internal void Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {
                FacilityWaiversDataHandler facilityWaiversDataHandler = new FacilityWaiversDataHandler(sqlTransaction);
                facilityWaiversDataHandler.Delete(facilityWaiverDTO.FacilityWaiverId);
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }
    }
    public class FacilityWaiverListBL
    {
        private List<FacilityWaiverDTO> facilityWaiverDTOList;
        private ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public FacilityWaiverListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.facilityWaiverDTOList = null;
            log.LogMethodExit();
        }


        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="facilityWaiverDTOList">facilityWaiverDTOList</param>
        /// <param name="executionContext">executionContext</param>
        public FacilityWaiverListBL(ExecutionContext executionContext, List<FacilityWaiverDTO> facilityWaiverDTOList)
        {
            log.LogMethodEntry(facilityWaiverDTOList, executionContext);
            this.executionContext = executionContext;
            this.facilityWaiverDTOList = facilityWaiverDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Save or update records with inner collections
        /// </summary>
        //public void SaveFacilityWaiver()
        //{
        //    try
        //    {
        //        log.LogMethodEntry();
        //        if (facilityWaiverDTOList != null)
        //        {
        //            foreach (FacilityWaiverDTO facilityWaiverDTO in facilityWaiverDTOList)
        //            {
        //                FacilityWaiverBL facilityWaiverBL = new FacilityWaiverBL(executionContext, facilityWaiverDTO);
        //                facilityWaiverBL.Save();
        //            }
        //        }

        //        log.LogMethodExit();
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex.Message);
        //        log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
        //        throw;
        //    }
        //}
        public void Delete(SqlTransaction sqlTransaction = null)
        {
            try
            {
                log.LogMethodEntry(sqlTransaction);
                if (facilityWaiverDTOList != null)
                {
                    foreach (FacilityWaiverDTO facilityWaiverDTO in facilityWaiverDTOList)
                    {
                        FacilityWaiverBL facilityWaiverBL = new FacilityWaiverBL(executionContext, facilityWaiverDTO);
                        facilityWaiverBL.Delete(sqlTransaction);
                    }
                }

                log.LogMethodExit();
            }
            catch (SqlException sqlEx)
            {
                log.Error(sqlEx);
                log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                if (sqlEx.Number == 547)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Returns the Facility Waiver DTO List
        /// </summary>
        public List<FacilityWaiverDTO> GetAllFacilityWaiverList(List<KeyValuePair<FacilityWaiverDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            FacilityWaiversDataHandler facilityWaiversDataHandler = new FacilityWaiversDataHandler(sqlTransaction);
            List<FacilityWaiverDTO> facilityWaiverList = facilityWaiversDataHandler.GetAllFacilityWaiverList(searchParameters);
            log.LogMethodExit(facilityWaiverList);
            return facilityWaiverList;
        }
    }
}

