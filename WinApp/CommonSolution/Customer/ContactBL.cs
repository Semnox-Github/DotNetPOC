/********************************************************************************************
 * Project Name - Contact BL
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
 *2.130.7     23-Apr-2022      Nitin Pai           Add DBSyncEntries for a customer outside of SQL transaction
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.DBSynch;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// Business logic for Contact class.
    /// </summary>
    public class ContactBL
    {
        private ContactDTO contactDTO;
        private string passPhrase;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ParafaitDefaultsListBL parafaitDefaultsListBL = new ParafaitDefaultsListBL(null);

        /// <summary>
        /// Parameterized constructor of ContactBL class
        /// </summary>
        private ContactBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            passPhrase = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CUSTOMER_ENCRYPTION_PASS_PHRASE");
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the contact id as the parameter
        /// Would fetch the contact object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">optional sql transaction</param>
        public ContactBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            ContactDataHandler contactDataHandler = new ContactDataHandler(passPhrase, sqlTransaction);
            contactDTO = contactDataHandler.GetContactDTO(id);
            if (contactDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "contact", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates ContactBL object using the ContactDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="contactDTO">ContactDTO object</param>
        public ContactBL(ExecutionContext executionContext, ContactDTO contactDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, contactDTO);
            this.contactDTO = contactDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the Contact
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ContactDataHandler contactDataHandler = new ContactDataHandler(passPhrase, sqlTransaction);
            if (contactDTO.Id < 0)
            {
                contactDTO = contactDataHandler.InsertContact(contactDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                contactDTO.AcceptChanges();
                CreateRoamingData(sqlTransaction);
            }
            else
            {
                if (contactDTO.IsChanged)
                {
                    contactDTO = contactDataHandler.UpdateContact(contactDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    contactDTO.AcceptChanges();
                    CreateRoamingData(sqlTransaction);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Create Roaming Data
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        internal void CreateRoamingData(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ContactDTO savedContactDTO = (new ContactBL(executionContext, contactDTO.Id, sqlTransaction)).ContactDTO;
            DBSynchLogService dBSynchLogService = new DBSynchLogService(executionContext, "Contact", savedContactDTO.Guid, savedContactDTO.SiteId);
            dBSynchLogService.CreateRoamingDataForCustomer(sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ContactDTO ContactDTO
        {
            get
            {
                return contactDTO;
            }
        }

        private int PhoneNumberWidth
        {
            get
            {
                return ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "CUSTOMER_PHONE_NUMBER_WIDTH");
            }

        }

        /// <summary>
        /// Validates the Contact, throws ValidationException if any fields are not valid
        /// </summary>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            if (contactDTO.IsActive)
            {
                if (contactDTO.ContactType == ContactType.EMAIL)
                {
                    if (string.IsNullOrWhiteSpace(contactDTO.Attribute1))
                    {
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 572);
                        validationErrorList.Add(new ValidationError("Contact", "Attribute1", errorMessage));
                    }
                    else if (!System.Text.RegularExpressions.Regex.IsMatch(contactDTO.Attribute1.Trim(), @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$"))// Changes made for the domain name size like .com ,.comm .ukcom etc
                    {
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 572);
                        validationErrorList.Add(new ValidationError("Contact", "Attribute1", errorMessage));
                    }
                }
                else if (contactDTO.ContactType == ContactType.PHONE)
                {
                    if (string.IsNullOrWhiteSpace(contactDTO.Attribute1))
                    {
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 785);
                        validationErrorList.Add(new ValidationError("Contact", "Attribute1", errorMessage));
                    }
                    else if (!System.Text.RegularExpressions.Regex.IsMatch(contactDTO.Attribute1, @"^[0-9]+$"))
                    {
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 785);
                        validationErrorList.Add(new ValidationError("Contact", "Attribute1", errorMessage));
                    }
                    else if (PhoneNumberWidth > 0)
                    {
                        if (contactDTO.Attribute1.Length != PhoneNumberWidth)
                        {
                            string errorMessage = MessageContainerList.GetMessage(executionContext, 785);
                            validationErrorList.Add(new ValidationError("Contact", "Attribute1", errorMessage));
                        }

                        for (int i = 0; i < 10; i++)
                        {
                            string match = new string(i.ToString()[0], PhoneNumberWidth);
                            if (match.Equals(contactDTO.Attribute1))
                            {
                                string errorMessage = MessageContainerList.GetMessage(executionContext, 785);
                                validationErrorList.Add(new ValidationError("Contact", "Attribute1", errorMessage));
                            }
                        }
                    }
                }
                else if (contactDTO.ContactType == ContactType.FACEBOOK)
                {
                    if (string.IsNullOrWhiteSpace(contactDTO.Attribute1))
                    {
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "FB UserId"));
                        validationErrorList.Add(new ValidationError("Contact", "Attribute1", errorMessage));
                    }
                    if (string.IsNullOrWhiteSpace(contactDTO.Attribute2))
                    {
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "FB Token"));
                        validationErrorList.Add(new ValidationError("Contact", "Attribute2", errorMessage));
                    }
                }
                else if (contactDTO.ContactType == ContactType.TWITTER)
                {
                    if (string.IsNullOrWhiteSpace(contactDTO.Attribute1))
                    {
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "TW Token"));
                        validationErrorList.Add(new ValidationError("Contact", "Attribute1", errorMessage));
                    }
                    if (string.IsNullOrWhiteSpace(contactDTO.Attribute2))
                    {
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "TW Secret"));
                        validationErrorList.Add(new ValidationError("Contact", "Attribute2", errorMessage));
                    }
                }
                else if (contactDTO.ContactType == ContactType.WECHAT)
                {
                    if (string.IsNullOrWhiteSpace(contactDTO.Attribute1))
                    {
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Wechat access token"));
                        validationErrorList.Add(new ValidationError("Contact", "Attribute1", errorMessage));
                    }
                }
                else
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Contact Type"));
                    validationErrorList.Add(new ValidationError("Contact", "ContactType", errorMessage));
                }
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

    }
        /// <summary>
        /// Manages the list of Contact
        /// </summary>
        public class ContactListBL
        {
            private string passPhrase;
            private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            private readonly ExecutionContext executionContext;

            /// <summary>
            /// Parameterized constructor
            /// </summary>
            /// <param name="executionContext">execution context</param>
            public ContactListBL(ExecutionContext executionContext)
            {
                log.LogMethodEntry(executionContext);
                this.executionContext = executionContext;
                passPhrase = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CUSTOMER_ENCRYPTION_PASS_PHRASE");
                log.LogMethodExit();
            }

            /// <summary>
            /// Returns the Contact list
            /// </summary>
            public List<ContactDTO> GetContactDTOList(List<KeyValuePair<ContactDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
            {
                log.LogMethodEntry(searchParameters, sqlTransaction);
                ContactDataHandler contactDataHandler = new ContactDataHandler(passPhrase, sqlTransaction);
                List<ContactDTO> returnValue = contactDataHandler.GetContactDTOList(searchParameters);
                log.LogMethodExit(returnValue);
                return returnValue;
            }

            /// <summary>
            /// Returns the Contact list
            /// </summary>
            public List<ContactDTO> GetContactDTOList(List<int> profileIdList, bool activeChildRecords, SqlTransaction sqlTransaction = null)
            {
                log.LogMethodEntry(profileIdList, activeChildRecords, sqlTransaction);
                ContactDataHandler contactDataHandler = new ContactDataHandler(passPhrase, sqlTransaction);
                List<ContactDTO> returnValue = contactDataHandler.GetContactDTOList(profileIdList, activeChildRecords);
                log.LogMethodExit(returnValue);
                return returnValue;
            }

        /// <summary>
        /// Gets the ContactDTO List for AddressList
        /// </summary>
        /// <param name="addressIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of ContactDTO</returns>
        public List<ContactDTO> GetAddressContactDTOList(List<int> addressIdList,
                                                         bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(addressIdList, activeRecords, sqlTransaction);
            ContactDataHandler contactDataHandler = new ContactDataHandler(passPhrase, sqlTransaction);
            List<ContactDTO> contactDTOList = contactDataHandler.GetAddressContactDTOList(addressIdList, activeRecords);
            log.LogMethodExit(contactDTOList);
            return contactDTOList;
        }
    }

    }

