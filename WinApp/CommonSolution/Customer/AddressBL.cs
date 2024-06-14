/********************************************************************************************
 * Project Name - Address BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        06-Feb-2017      Lakshminarayana     Created 
 *2.70.2      19-Jul-2019      Girish Kundar    Modified : Save() method. Now Insert/Update method returns the DTO instead of Id. 
 *2.90        21-May-2020      Girish Kundar       Modified : Made default constructor as Private 
 *2.140.0    14-Sep-2021       Prajwal S           Modified : Added Build child and address validation.
 *2.130.7     23-Apr-2022      Nitin Pai           Add DBSyncEntries for a customer outside of SQL transaction
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.DBSynch;
using System.Linq;
using System.Text.RegularExpressions;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// Business logic for Address class.
    /// </summary>
    public class AddressBL
    {
        private AddressDTO addressDTO;
        private readonly ExecutionContext executionContext;
        private string passPhrase;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of AddressBL class
        /// </summary>
        private AddressBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            passPhrase = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CUSTOMER_ENCRYPTION_PASS_PHRASE");
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the address id as the parameter
        /// Would fetch the address object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">SQL Transaction</param>
        public AddressBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null, bool loadChildRecords = false, bool activeChildRecords = false)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            AddressDataHandler addressDataHandler = new AddressDataHandler(passPhrase, sqlTransaction);
            addressDTO = addressDataHandler.GetAddressDTO(id);
            if (addressDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "Address", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates AddressBL object using the AddressDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="addressDTO">AddressDTO object</param>
        public AddressBL(ExecutionContext executionContext, AddressDTO addressDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, addressDTO);
            this.addressDTO = addressDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the Address
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            AddressDataHandler addressDataHandler = new AddressDataHandler(passPhrase, sqlTransaction);
            if (addressDTO.Id < 0)
            {
                addressDTO = addressDataHandler.InsertAddress(addressDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                addressDTO.LastUpdateDate = DateTime.Now;
                CreateRoamingData(sqlTransaction);
                if (addressDTO.ContactDTOList != null && addressDTO.ContactDTOList.Count > 0)
                {
                    foreach (ContactDTO contactDTO in addressDTO.ContactDTOList)
                    {
                        if (contactDTO.AddressId == -1)
                        {
                            contactDTO.AddressId = addressDTO.Id;
                        }
                        ContactBL contactBL = new ContactBL(executionContext, contactDTO);
                        contactBL.Save(sqlTransaction);
                    }
                }
                addressDTO.AcceptChanges();
            }
            else
            {
                if (addressDTO.IsChanged)
                {
                    addressDTO = addressDataHandler.UpdateAddress(addressDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    addressDTO.LastUpdateDate = DateTime.Now;
                    CreateRoamingData(sqlTransaction);
                    if (addressDTO.ContactDTOList != null && addressDTO.ContactDTOList.Count > 0)
                    {
                        foreach (ContactDTO contactDTO in addressDTO.ContactDTOList)
                        {
                            if (contactDTO.AddressId == -1)
                            {
                                contactDTO.AddressId = addressDTO.Id;
                            }
                            ContactBL contactBL = new ContactBL(executionContext, contactDTO);
                            contactBL.Save(sqlTransaction);
                        }
                    }
                    addressDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        internal void CreateRoamingData(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            AddressDTO savedAddressDTO = (new AddressBL(executionContext, addressDTO.Id, sqlTransaction)).AddressDTO;
            DBSynchLogService dBSynchLogService = new DBSynchLogService(executionContext, "Address", savedAddressDTO.Guid, savedAddressDTO.SiteId);
            dBSynchLogService.CreateRoamingDataForCustomer(sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public AddressDTO AddressDTO
        {
            get
            {
                return addressDTO;
            }
        }

        /// <summary>
        /// Validates the Address, throws ValidationException if any fields are not valid
        /// </summary>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            List<ValidationError> validationErrorList = new List<ValidationError>();
            return validationErrorList;
        }

        /// <summary>
        /// Validates the Address, throws ValidationException if any fields are not valid
        /// </summary>
        public List<ValidationError> ValidateAddress(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            ValidationError validationError = null;
            if (addressDTO.IsActive)
            {
                if (Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ADDRESS_TYPE") == "M" &&
                         addressDTO.AddressType == AddressType.NONE)
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 249, MessageContainerList.GetMessage(executionContext, "Address Type"));
                    validationError = new ValidationError("Address", "AddressType", errorMessage);
                    validationErrorList.Add(validationError);
                }
                validationError = ValidateStringField("Address", "ADDRESS1", "Line1", addressDTO.Line1, "Line 1");
                if (validationError != null)
                {
                    validationErrorList.Add(validationError);
                }
                validationError = ValidateStringField("Address", "ADDRESS2", "Line2", addressDTO.Line2, "Line 2");
                if (validationError != null)
                {
                    validationErrorList.Add(validationError);
                }
                validationError = ValidateStringField("Address", "ADDRESS3", "Line3", addressDTO.Line3, "Line 3");
                if (validationError != null)
                {
                    validationErrorList.Add(validationError);
                }
                validationError = ValidateStringField("Address", "CITY", "City", addressDTO.City, "City");
                if (validationError != null)
                {
                    validationErrorList.Add(validationError);
                }
                validationError = ValidateStringField("Address", "PIN", "PostalCode", addressDTO.PostalCode, "Postal Code");
                if (validationError != null)
                {
                    validationErrorList.Add(validationError);
                }
                validationError = ValidateForeignKeyField("Address", "COUNTRY", "CountryId", addressDTO.CountryId, "Country");
                if (validationError != null)
                {
                    validationErrorList.Add(validationError);
                }
                validationError = ValidateForeignKeyField("Address", "STATE", "StateId", addressDTO.StateId, "State");
                if (validationError != null)
                {
                    validationErrorList.Add(validationError);
                }
            }
            log.LogMethodExit(validationErrorList);

            return validationErrorList;
        }
        private ValidationError ValidateStringField(string entity, string defaultValueName, string attributeName, string attributeValue, string displayName)
        {
            log.LogMethodEntry(entity, defaultValueName, attributeName, attributeValue, displayName);
            ValidationError validationError = null;
            string specialChars = @"[-+=@]";
            if (Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, defaultValueName) == "M")
            {
                if (string.IsNullOrWhiteSpace(attributeValue))
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 249, MessageContainerList.GetMessage(executionContext, displayName));
                    validationError = new ValidationError(entity, attributeName, errorMessage);
                }
            }
            if (string.IsNullOrEmpty(attributeValue) == false && attributeName == "FirstName" && Regex.IsMatch(attributeValue.Substring(0, 1), specialChars))
            {
                validationError = new ValidationError(entity, attributeName, MessageContainerList.GetMessage(executionContext, 2265, MessageContainerList.GetMessage(executionContext, "First Name"), specialChars));
            }
            log.LogMethodExit(validationError);
            return validationError;
        }

        private ValidationError ValidateForeignKeyField(string entity, string defaultValueName, string attributeName, int attributeValue, string displayName)
        {
            log.LogMethodEntry(entity, defaultValueName, attributeName, attributeValue, displayName);
            ValidationError validationError = null;
            if (Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, defaultValueName) == "M")
            {
                if (attributeValue == -1)
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 249, MessageContainerList.GetMessage(executionContext, displayName));
                    validationError = new ValidationError(entity, attributeName, errorMessage);
                }
            }
            log.LogMethodExit(validationError);
            return validationError;
        }

        /// <summary>
        /// Builds the child records for Address object.
        /// </summary>
        /// <param name="activeChildRecords">activeChildRecords holds either true or false</param>
        /// <param name="sqlTransaction"></param>
        private void Build(bool activeChildRecords, SqlTransaction sqlTransaction)    //added build
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);
            ContactListBL contactListBL = new ContactListBL(executionContext);
            List<ContactDTO> contactDTOList = contactListBL.GetAddressContactDTOList(new List<int>() { addressDTO.Id }, false, sqlTransaction);
            if (contactDTOList.Count > 0)
            {
                addressDTO.ContactDTOList = contactDTOList;
            }
        }
    }

    /// <summary>
    /// Manages the list of Address
    /// </summary>
    public class AddressListBL
    {
        private string passPhrase;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public AddressListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            passPhrase = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CUSTOMER_ENCRYPTION_PASS_PHRASE");
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the Address list
        /// </summary>
        public List<AddressDTO> GetAddressDTOList(List<KeyValuePair<AddressDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null, bool loadChildRecords = false, bool activeChildRecords = false)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            AddressDataHandler addressDataHandler = new AddressDataHandler(passPhrase, sqlTransaction);
            List<AddressDTO> returnValue = addressDataHandler.GetAddressDTOList(searchParameters);
            if (returnValue != null && returnValue.Any() && loadChildRecords)
            {
                Build(returnValue, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Returns the Address list
        /// </summary>
        public List<AddressDTO> GetAddressDTOList(List<int> profileIdList, bool activeChildRecords, SqlTransaction sqlTransaction = null, bool loadChildRecords = true)
        {
            log.LogMethodEntry(profileIdList, activeChildRecords, sqlTransaction);
            AddressDataHandler addressDataHandler = new AddressDataHandler(passPhrase, sqlTransaction);
            List<AddressDTO> returnValue = addressDataHandler.GetAddressDTOList(profileIdList, activeChildRecords);
            if (returnValue != null && returnValue.Any() && loadChildRecords)
            {
                Build(returnValue, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Builds the List of Address object based on the list of Address id.
        /// </summary>
        /// <param name="addressDTOList"></param>
        /// <param name="activeChildRecords"></param>
        /// <param name="sqlTransaction"></param>
        private void Build(List<AddressDTO> addressDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(addressDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, AddressDTO> addressDTOIdMap = new Dictionary<int, AddressDTO>();
            List<int> addressIdList = new List<int>();
            for (int i = 0; i < addressDTOList.Count; i++)
            {
                if (addressDTOIdMap.ContainsKey(addressDTOList[i].Id))
                {
                    continue;
                }
                addressDTOIdMap.Add(addressDTOList[i].Id, addressDTOList[i]);
                addressIdList.Add(addressDTOList[i].Id);
            }

            ContactListBL contactListBL = new ContactListBL(executionContext);
            List<ContactDTO> contactDTOList = contactListBL.GetAddressContactDTOList(addressIdList, activeChildRecords, sqlTransaction);
            if (contactDTOList != null && contactDTOList.Any())
            {
                for (int i = 0; i < contactDTOList.Count; i++)
                {
                    if (addressDTOIdMap.ContainsKey(contactDTOList[i].AddressId) == false)
                    {
                        continue;
                    }
                    AddressDTO addressDTO = addressDTOIdMap[contactDTOList[i].AddressId];
                    if (addressDTO.ContactDTOList == null)
                    {
                        addressDTO.ContactDTOList = new List<ContactDTO>();
                    }
                    addressDTO.ContactDTOList.Add(contactDTOList[i]);
                }
            }
            log.LogMethodExit();
        }

    }

}
