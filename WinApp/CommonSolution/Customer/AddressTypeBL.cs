/********************************************************************************************
 * Project Name - AddressType BL
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

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// Business logic for AddressType class.
    /// </summary>
    public class AddressTypeBL
    {
        private AddressTypeDTO addressTypeDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of AddressTypeBL class
        /// </summary>
        private AddressTypeBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the addressType id as the parameter
        /// Would fetch the addressType object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution Context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">SQL Transaction</param>
        public AddressTypeBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            AddressTypeDataHandler addressTypeDataHandler = new AddressTypeDataHandler(sqlTransaction);
            addressTypeDTO = addressTypeDataHandler.GetAddressTypeDTO(id);
            if (addressTypeDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "AddressType", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates AddressTypeBL object using the AddressTypeDTO
        /// </summary>
        /// <param name="executionContext">execution Context</param>
        /// <param name="addressTypeDTO">AddressTypeDTO object</param>
        public AddressTypeBL(ExecutionContext executionContext, AddressTypeDTO addressTypeDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, addressTypeDTO);
            this.addressTypeDTO = addressTypeDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the AddressType
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            AddressTypeDataHandler addressTypeDataHandler = new AddressTypeDataHandler(sqlTransaction);
            if (addressTypeDTO.Id < 0)
            {
                addressTypeDTO = addressTypeDataHandler.InsertAddressType(addressTypeDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                addressTypeDTO.AcceptChanges();
            }
            else
            {
                if (addressTypeDTO.IsChanged)
                {
                    addressTypeDTO = addressTypeDataHandler.UpdateAddressType(addressTypeDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    addressTypeDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public AddressTypeDTO AddressTypeDTO
        {
            get
            {
                return addressTypeDTO;
            }
        }

    }

    /// <summary>
    /// Manages the list of AddressType
    /// </summary>
    public class AddressTypeListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public AddressTypeListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the AddressType list
        /// </summary>
        public List<AddressTypeDTO> GetAddressTypeDTOList(List<KeyValuePair<AddressTypeDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            AddressTypeDataHandler addressTypeDataHandler = new AddressTypeDataHandler(sqlTransaction);
            List<AddressTypeDTO> returnValue = addressTypeDataHandler.GetAddressTypeDTOList(searchParameters);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        public DateTime? GetAddressTypeModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            AddressTypeDataHandler addressTypeDataHandler = new AddressTypeDataHandler(null);
            DateTime? result = addressTypeDataHandler.GetAddressTypeModuleLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
