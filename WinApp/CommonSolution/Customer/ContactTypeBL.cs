/********************************************************************************************
 * Project Name - ContactType BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        06-Feb-2017      Lakshminarayana     Created 
 *2.70.2      19-Jul-2019      Girish Kundar       Modified : Save() method. Now Insert/Update method returns the DTO instead of Id. 
 *2.90        21-May-2020      Girish Kundar       Modified : Made default constructor as Private 
 ********************************************************************************************/

using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// Business logic for ContactType class.
    /// </summary>
    public class ContactTypeBL
    {
        private ContactTypeDTO contactTypeDTO;
        private readonly ExecutionContext executionContext;
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of ContactTypeBL class
        /// </summary>
        private ContactTypeBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the contactType id as the parameter
        /// Would fetch the contactType object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public ContactTypeBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            ContactTypeDataHandler contactTypeDataHandler = new ContactTypeDataHandler(sqlTransaction);
            contactTypeDTO = contactTypeDataHandler.GetContactTypeDTO(id);
            if (contactTypeDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "ContactType", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates ContactTypeBL object using the ContactTypeDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="contactTypeDTO">ContactTypeDTO object</param>
        public ContactTypeBL(ExecutionContext executionContext, ContactTypeDTO contactTypeDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, contactTypeDTO);
            this.contactTypeDTO = contactTypeDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the ContactType
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ContactTypeDataHandler contactTypeDataHandler = new ContactTypeDataHandler(sqlTransaction);
            if (contactTypeDTO.Id < 0)
            {
                contactTypeDTO = contactTypeDataHandler.InsertContactType(contactTypeDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                contactTypeDTO.AcceptChanges();
            }
            else
            {
                if (contactTypeDTO.IsChanged)
                {
                    contactTypeDTO = contactTypeDataHandler.UpdateContactType(contactTypeDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    contactTypeDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ContactTypeDTO ContactTypeDTO
        {
            get
            {
                return contactTypeDTO;
            }
        }

    }

    /// <summary>
    /// Manages the list of ContactType
    /// </summary>
    public class ContactTypeListBL
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public ContactTypeListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the ContactType list
        /// </summary>
        public List<ContactTypeDTO> GetContactTypeDTOList(List<KeyValuePair<ContactTypeDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ContactTypeDataHandler contactTypeDataHandler = new ContactTypeDataHandler(sqlTransaction);
            List<ContactTypeDTO> returnValue = contactTypeDataHandler.GetContactTypeDTOList(searchParameters);
            log.LogMethodExit(returnValue);
            return returnValue;
        }
    }
}
