/********************************************************************************************
 * Project Name - Object Translations
 * Description  - A high level structure created to classify the object translations 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        30-Dec-2016   Raghuveera          Created 
 *2.60        04-Mar-2019   Akshay Gulaganji    Modified Constructors [added executionContext], Save() method 
 *                                                and added one constructor ObjectTranslations(objectTranslations,executionContext)
 *2.70.2        26-Jul-2019   Dakshakh raj        Modified : Save() method Insert/Update method returns DTO.
 *2.80        03-Mar-2020     Mushahid Faizan   Modified : 3 tier Changes for REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Bussiness logic of saving  Object Translations
    /// </summary>
    public class ObjectTranslations
    {
        private ObjectTranslationsDTO objectTranslations;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public ObjectTranslations(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with ObjectTranslationsDTO and executionContext
        /// </summary>
        /// <param name="objectTranslations"></param>
        /// <param name="executionContext"></param>
        public ObjectTranslations(ExecutionContext executionContext, ObjectTranslationsDTO objectTranslations) : this(executionContext)
        {
            log.LogMethodEntry(objectTranslations, executionContext);
            this.objectTranslations = objectTranslations;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the object transilation id as the parameter
        /// Would fetch the object translation object from the database based on the id passed. 
        /// </summary>
        /// <param name="objectTranslationsId">objectTranslationsId</param>
        public ObjectTranslations(ExecutionContext executionContext, int objectTranslationsId) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, objectTranslationsId);
            ObjectTranslationsDataHandler objectTranslationsDataHandler = new ObjectTranslationsDataHandler();
            objectTranslations = objectTranslationsDataHandler.GetObjectTranslations(objectTranslationsId);
            log.LogMethodExit(objectTranslations);
        }


        /// <summary>
        /// Saves the objectTranslationsDTO.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (objectTranslations.IsChanged == false &&
                objectTranslations.Id > -1)
            {
                log.LogMethodExit(null, "No Changes to save");
                return;
            }
            ObjectTranslationsDataHandler objectTranslationsDataHandler = new ObjectTranslationsDataHandler();
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (objectTranslations.Id < 0)
            {
                objectTranslations = objectTranslationsDataHandler.InsertObjectTranslations(objectTranslations, executionContext.GetUserId(), executionContext.GetSiteId());
                objectTranslations.AcceptChanges();
            }
            else if (objectTranslations.IsChanged)
            {
                objectTranslations = objectTranslationsDataHandler.UpdateObjectTranslations(objectTranslations, executionContext.GetUserId(), executionContext.GetSiteId());
                objectTranslations.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates WaiverDTO
        /// </summary>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            ValidationError validationError = null;

            // Validation Logic here

            // validate Child list
            return validationErrorList;
        }
        ///// <summary>
        ///// Saves the object transilation
        ///// Checks if the ObjectTranslations id is not less than or equal to 0
        /////     If it is less than or equal to 0, then inserts
        /////     else updates
        ///// </summary>
        //public void Save()
        //{
        //    log.LogMethodEntry();
        //    ObjectTranslationsDataHandler objectTranslationsDataHandler = new ObjectTranslationsDataHandler();
        //    if (objectTranslations.Id < 0)
        //    {
        //        int objectTranslationsId = objectTranslationsDataHandler.InsertObjectTranslations(objectTranslations, executionContext.GetUserId(), executionContext.GetSiteId());
        //        objectTranslations.Id = objectTranslationsId;
        //    }
        //    else
        //    {
        //        if (objectTranslations.IsChanged == true)
        //        {
        //            objectTranslationsDataHandler.UpdateObjectTranslations(objectTranslations, executionContext.GetUserId(), executionContext.GetSiteId());
        //            objectTranslations.AcceptChanges();
        //        }
        //    }
        //    log.LogMethodExit();
        //}

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ObjectTranslationsDTO GetObjectTranslations { get { return objectTranslations; } }
    }

    /// <summary>
    /// Manages the list of object transilation
    /// </summary>
    public class ObjectTranslationsList
    {
        private List<ObjectTranslationsDTO> objectTranslationsDTOList = new List<ObjectTranslationsDTO>();
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// No Parameter constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public ObjectTranslationsList()
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }


        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public ObjectTranslationsList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parametrized constructors with 2 params
        /// </summary>
        /// <param name="objectTranslationsDTOList"></param>
        /// <param name="executionContext"></param>
        public ObjectTranslationsList(ExecutionContext executionContext, List<ObjectTranslationsDTO> objectTranslationsDTOList) : this(executionContext)
        {
            log.LogMethodEntry(objectTranslationsDTOList, executionContext);
            this.objectTranslationsDTOList = objectTranslationsDTOList;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the object Translations
        /// </summary>
        public ObjectTranslationsDTO GetObjectTranslations(ExecutionContext executionContext, int objectTranslationsId)
        {
            log.LogMethodEntry(executionContext, objectTranslationsId);
            ObjectTranslationsDataHandler objectTranslationsDataHandler = new ObjectTranslationsDataHandler();
            log.LogMethodExit(objectTranslationsDataHandler.GetObjectTranslations(objectTranslationsId));
            return objectTranslationsDataHandler.GetObjectTranslations(objectTranslationsId);
        }
        /// <summary>
        /// Returns the object Translations
        /// </summary>
        public List<ObjectTranslationsDTO> GetAllObjectTranslations(List<KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            ObjectTranslationsDataHandler objectTranslationsDataHandler = new ObjectTranslationsDataHandler();
            this.objectTranslationsDTOList = objectTranslationsDataHandler.GetObjectTranslationsList(searchParameters);
            log.LogMethodExit(objectTranslationsDTOList);
            return objectTranslationsDTOList;
        }


        /// <summary>
        /// Saves the objectTranslationsDTO List
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (objectTranslationsDTOList == null ||
                objectTranslationsDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < objectTranslationsDTOList.Count; i++)
            {
                var objectTranslationsDTO = objectTranslationsDTOList[i];
                if (objectTranslationsDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    ObjectTranslations objectTranslationsBL = new ObjectTranslations(executionContext, objectTranslationsDTO);
                    objectTranslationsBL.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving objectTranslationsDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("objectTranslationsDTO", objectTranslationsDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

        public DateTime? GetObjectTranslationsModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            ObjectTranslationsDataHandler objectTranslationsDataHandler = new ObjectTranslationsDataHandler(null);
            DateTime? result = objectTranslationsDataHandler.GetObjectTranslationsModuleLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
