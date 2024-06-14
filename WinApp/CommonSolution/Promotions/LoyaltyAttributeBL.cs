/********************************************************************************************
 * Project Name - Loyalty Attribute BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.70.2      13-Nov-2019      Deeksha        Created 
 *2.70.3      06-Feb-2020      Girish Kundar  Modified : As per the 3 tier standard
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Parafait.Promotions
{
    public class LoyaltyAttributeBL
    {
        private LoyaltyAttributesDTO loyaltyAttributesDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of LoyaltyAttributeBL class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        private LoyaltyAttributeBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the LoyaltyAttribute id as the parameter
        /// Would fetch the LoyaltyAttribute object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="sqlTransaction">SqlTransaction</param>
        /// <param name="id">Id</param>
        public LoyaltyAttributeBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            LoyaltyAttributeDataHandler loyaltyAttributeDataHandler = new LoyaltyAttributeDataHandler(sqlTransaction);
            loyaltyAttributesDTO = loyaltyAttributeDataHandler.GetLoyaltyAttributes(id);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates LoyaltyAttributeBL object using the LoyaltyAttributeDTO
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="loyaltyAttributeDTO">loyaltyAttributeDTO object</param>
        public LoyaltyAttributeBL(ExecutionContext executionContext, LoyaltyAttributesDTO loyaltyAttributesDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, loyaltyAttributesDTO);
            this.loyaltyAttributesDTO = loyaltyAttributesDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// get LoyaltyAttributesDTO Object
        /// </summary>
        public LoyaltyAttributesDTO GetLoyaltyAttributesDTO
        {
            get { return loyaltyAttributesDTO; }
        }

        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            string errorMessage = string.Empty;
            if (loyaltyAttributesDTO == null)
            {
                errorMessage = MessageContainerList.GetMessage(executionContext, 2397, (MessageContainerList.GetMessage(executionContext, "loyaltyAttributesDTO"))); //Cannot proceed loyaltyAttributes record is Empty.
                validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext, "Attraction"), MessageContainerList.GetMessage(executionContext, "loyaltyAttributesDTO"), errorMessage));
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Saves the LoyaltyAttribute
        /// Checks if the LoyaltyAttributeId is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            LoyaltyAttributeDataHandler loyaltyAttributeDataHandler = new LoyaltyAttributeDataHandler(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            validationErrorList = Validate();
            if(validationErrorList.Count > 0)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Validation Error"), validationErrorList);
            }
            if (loyaltyAttributesDTO.LoyaltyAttributeId < 0)
            {
                loyaltyAttributesDTO = loyaltyAttributeDataHandler.Insert(loyaltyAttributesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                loyaltyAttributesDTO.AcceptChanges();
            }
            else
            {
                if (loyaltyAttributesDTO.IsChanged)
                {
                    loyaltyAttributesDTO = loyaltyAttributeDataHandler.Update(loyaltyAttributesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    loyaltyAttributesDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of LoyaltyAttribute
    /// </summary>
    public class LoyaltyAttributeListBL
    {
        private List<LoyaltyAttributesDTO> loyaltyAttributesDTOList;
        private ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public LoyaltyAttributeListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.loyaltyAttributesDTOList = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="loyaltyAttributesDTO"></param>
        /// <param name="executionContext"></param>
        public LoyaltyAttributeListBL(ExecutionContext executionContext, List<LoyaltyAttributesDTO> loyaltyAttributesDTOList)
        {
            log.LogMethodEntry(loyaltyAttributesDTOList, executionContext);
            this.executionContext = executionContext;
            this.loyaltyAttributesDTOList = loyaltyAttributesDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Save or update records with inner collections
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            try
            {
                log.LogMethodEntry();
                if (loyaltyAttributesDTOList != null)
                {
                    foreach (LoyaltyAttributesDTO loyaltyAttributesDTO in loyaltyAttributesDTOList)
                    {
                        LoyaltyAttributeBL loyaltyAttributeBL = new LoyaltyAttributeBL(executionContext, loyaltyAttributesDTO);
                        loyaltyAttributeBL.Save(sqlTransaction);
                    }
                }

                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Returns the LoyaltyAttributes  List
        /// </summary>
        public List<LoyaltyAttributesDTO> GetAllLoyaltyAttributesList(List<KeyValuePair<LoyaltyAttributesDTO.SearchByParameters, string>> searchParameters,SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            LoyaltyAttributeDataHandler loyaltyAttributeDataHandler = new LoyaltyAttributeDataHandler(sqlTransaction);
            List<LoyaltyAttributesDTO> loyaltyAttributesList = loyaltyAttributeDataHandler.GetLoyaltyAttributesDTOList(searchParameters, sqlTransaction);
            log.LogMethodExit(loyaltyAttributesList);
            return loyaltyAttributesList;
        }
        public DateTime? GetLoyaltyAttributeLastUpdateTime(int siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(siteId, sqlTransaction);
            LoyaltyAttributeDataHandler loyaltyAttributeDataHandler = new LoyaltyAttributeDataHandler(sqlTransaction);
            DateTime? result = loyaltyAttributeDataHandler.GetLoyaltyAttributeLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}


