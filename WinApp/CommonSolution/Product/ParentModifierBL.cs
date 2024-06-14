/********************************************************************************************
 * Project Name - ParentModifierBL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By           Remarks          
 *********************************************************************************************
 *2.6.0       18-Feb-2019      Mehraj/Guru S A        3 tier class creation
 *2.70        28-Jun-2019      Akshay Gulaganji       added DeleteParentModifier() method
  *2.110.00    30-Nov-2020      Abhishek                Modified : Modified to 3 Tier Standard 
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Product
{
    public class ParentModifierBL
    {
        private ParentModifierDTO parentModifierDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor of ParentModifierBL class
        /// </summary>
        private ParentModifierBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the parentModifier id as the parameter
        /// Would fetch the parentModifier object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public ParentModifierBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            ParentModifierDataHandler parentModifierDataHandler = new ParentModifierDataHandler(sqlTransaction);
            this.parentModifierDTO = parentModifierDataHandler.GetParentModifierDTO(id);
            if (parentModifierDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, " Parent Modifier", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///  Constructor with the parentModifier DTO as the parameter
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="parentModifierDTO"></param>
        public ParentModifierBL(ExecutionContext executionContext, ParentModifierDTO parentModifierDTO)
              : this(executionContext)
        {
            log.LogMethodEntry(executionContext, parentModifierDTO);
            this.parentModifierDTO = parentModifierDTO;
            log.LogMethodExit();
        }


        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ParentModifierDTO ParentModifierDTO
        {
            get
            {
                return parentModifierDTO;
            }
        }

        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (parentModifierDTO.IsChanged == false
               && parentModifierDTO.Id > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            ParentModifierDataHandler parentModifierDataHandler = new ParentModifierDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate(sqlTransaction);
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (parentModifierDTO.Id < 0 && parentModifierDTO.Price != null)
            {
                parentModifierDTO = parentModifierDataHandler.Insert(parentModifierDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                parentModifierDTO.AcceptChanges();
            }
            else
            {
                if (parentModifierDTO.IsChanged == true && parentModifierDTO.Id > -1)
                {
                    parentModifierDTO = parentModifierDataHandler.Update(parentModifierDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    parentModifierDTO.AcceptChanges();
                }
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
        /// Delete ParentModifier details based on id
        /// </summary>
        /// <param name="id">id</param>        
        public void Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {
                ParentModifierDataHandler parentModifierBL = new ParentModifierDataHandler(sqlTransaction);
                parentModifierBL.Delete(parentModifierDTO.Id);
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
    }

    public class ParentModifierList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<ParentModifierDTO> parentModifiersList;

        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public ParentModifierList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with List<ParentModifierDTO> as parameter
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="productModifiersList"></param>
        public ParentModifierList(List<ParentModifierDTO> parentModifiers, ExecutionContext executionContext)
         : this(executionContext)
        {
            log.LogMethodEntry(parentModifiers, executionContext);
            this.parentModifiersList = parentModifiers;
            log.LogMethodExit();
        }

        /// <summary>
        /// SaveUpdateParentModifiersList
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {
                if (parentModifiersList != null)
                {
                    foreach (ParentModifierDTO parentModifierDTO in parentModifiersList)
                    {
                        ParentModifierBL parentModifierBL = new ParentModifierBL(executionContext, parentModifierDTO);
                        parentModifierBL.Save(sqlTransaction);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the ParentModifier list
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<ParentModifierDTO> GetParentModifierDTOList(List<KeyValuePair<ParentModifierDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ParentModifierDataHandler parentModifieeDataHandler = new ParentModifierDataHandler(sqlTransaction);
            List<ParentModifierDTO> parentModifierDTOList = parentModifieeDataHandler.GetParentModifierDTOList(searchParameters);
            log.LogMethodExit(parentModifierDTOList);
            return parentModifierDTOList;
        }
    }
}
