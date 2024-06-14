/********************************************************************************************
 * Project Name - Profile BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        06-Feb-2017      Lakshminarayana     Created 
 *2.60        21-May-2019      Mehraj              Added GetProfileImageBase64() and GetIdImageBase64()
 *2.70.2      19-Jul-2019      Girish Kundar       Modified : Save() method. Now Insert/Update method returns the DTO instead of Id.
 *2.70.3      10-Mar-2020      Lakshminarayana     Updated to change the customer authentication process
 *2.90        26-Jun-2020      Indrajeet Kumar     Customer authentication by password policy option.
 *2.130.7     23-Apr-2022      Nitin Pai           Add DBSyncEntries for a customer outside of SQL transaction
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.DBSynch;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// Business logic for Profile class.
    /// </summary>
    public class ProfileBL
    {
        private ProfileDTO profileDTO;
        private readonly ExecutionContext executionContext;
        private string passPhrase;
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of ProfileBL class
        /// </summary>
        private ProfileBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            passPhrase = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CUSTOMER_ENCRYPTION_PASS_PHRASE");
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the profile id as the parameter
        /// Would fetch the profile object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="loadChildRecords">whether to load the child records</param>
        /// <param name="activeChildRecords">whether to load only active child records</param>
        /// <param name="sqlTransaction">sql transaction</param>
        public ProfileBL(ExecutionContext executionContext, int id, bool loadChildRecords = true, 
            bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, loadChildRecords, activeChildRecords, sqlTransaction);
            ProfileDataHandler profileDataHandler = new ProfileDataHandler(passPhrase, sqlTransaction);
            profileDTO = profileDataHandler.GetProfileDTO(id);
            if (profileDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "Profile", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (loadChildRecords && profileDTO != null)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }
         
        /// <summary>
        /// Builds the complex profile DTO structure
        /// </summary>
        /// <param name="profileDTO">Profile dto</param>
        /// <param name="activeChildRecords">whether to load only active child records</param>
        /// <param name="sqlTransaction">optional sql transaction</param>
        private void Build(bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry("profileDTO", activeChildRecords, sqlTransaction);
            if (profileDTO != null && profileDTO.Id != -1)
            {
                AddressListBL addressListBL = new AddressListBL(executionContext);
                List<KeyValuePair<AddressDTO.SearchByParameters, string>> searchAddressParams = new List<KeyValuePair<AddressDTO.SearchByParameters, string>>();
                searchAddressParams.Add(new KeyValuePair<AddressDTO.SearchByParameters, string>(AddressDTO.SearchByParameters.PROFILE_ID, profileDTO.Id.ToString()));
                if(activeChildRecords)
                {
                    searchAddressParams.Add(new KeyValuePair<AddressDTO.SearchByParameters, string>(AddressDTO.SearchByParameters.IS_ACTIVE, "1"));
                }
                profileDTO.AddressDTOList = addressListBL.GetAddressDTOList(searchAddressParams, sqlTransaction, true, true);

                ContactListBL contactListBL = new ContactListBL(executionContext);
                List<KeyValuePair<ContactDTO.SearchByParameters, string>> searchContactParams = new List<KeyValuePair<ContactDTO.SearchByParameters, string>>();
                searchContactParams.Add(new KeyValuePair<ContactDTO.SearchByParameters, string>(ContactDTO.SearchByParameters.PROFILE_ID, profileDTO.Id.ToString()));
                if (activeChildRecords)
                {
                    searchContactParams.Add(new KeyValuePair<ContactDTO.SearchByParameters, string>(ContactDTO.SearchByParameters.IS_ACTIVE, "1"));
                }
                profileDTO.ContactDTOList = contactListBL.GetContactDTOList(searchContactParams, sqlTransaction);

                ProfileContentHistoryListBL profileContentHistoryListBL = new ProfileContentHistoryListBL();
                List<KeyValuePair<ProfileContentHistoryDTO.SearchByParameters, string>> searchContentHistoryParams = new List<KeyValuePair<ProfileContentHistoryDTO.SearchByParameters, string>>();
                searchContentHistoryParams.Add(new KeyValuePair<ProfileContentHistoryDTO.SearchByParameters, string>(ProfileContentHistoryDTO.SearchByParameters.PROFILE_ID, profileDTO.Id.ToString()));
                if (activeChildRecords)
                {
                    searchContentHistoryParams.Add(new KeyValuePair<ProfileContentHistoryDTO.SearchByParameters, string>(ProfileContentHistoryDTO.SearchByParameters.IS_ACTIVE, "1"));
                }
                profileDTO.ProfileContentHistoryDTOList = profileContentHistoryListBL.GetAllProfileContentHistory(searchContentHistoryParams, sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates ProfileBL object using the ProfileDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="profileDTO">ProfileDTO object</param>
        public ProfileBL(ExecutionContext executionContext, ProfileDTO profileDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, "profileDTO");
            this.profileDTO = profileDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the Profile
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ProfileDataHandler profileDataHandler = new ProfileDataHandler(passPhrase, sqlTransaction);
            if (profileDTO.Id < 0)
            {
                profileDTO = profileDataHandler.InsertProfile(profileDTO, executionContext.GetUserId(), executionContext.GetSiteId());
				profileDTO.AcceptChanges();
                CreateRoamingData(sqlTransaction);
	    }
	    else
            {
                if (profileDTO.IsChanged)
                {                    
                    profileDTO = profileDataHandler.UpdateProfile(profileDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    profileDTO.AcceptChanges();
                    CreateRoamingData(sqlTransaction);
                }
            }


            if (profileDTO.ContactDTOList != null)
            {
                foreach (var contactDTO in profileDTO.ContactDTOList)
                {
                    if(contactDTO.IsChanged)
                    {
                        contactDTO.ProfileId = profileDTO.Id;
                        ContactBL contactBL = new ContactBL(executionContext, contactDTO);
                        contactBL.Save(sqlTransaction);
                    }
                    
                }
            }

            if(profileDTO.AddressDTOList != null)
            {
                foreach (var addressDTO in profileDTO.AddressDTOList)
                {
                    if (addressDTO.IsChanged)
                    {
                        addressDTO.ProfileId = profileDTO.Id;
                        AddressBL addressBL = new AddressBL(executionContext, addressDTO);
                        addressBL.Save(sqlTransaction);
                    }
                }
            }

            if (profileDTO.ProfileContentHistoryDTOList != null)
            {
                foreach (var profileContentHistoryDTO in profileDTO.ProfileContentHistoryDTOList)
                {
                    if (profileContentHistoryDTO.IsChanged)
                    {
                        profileContentHistoryDTO.ProfileId = profileDTO.Id;
                        ProfileContentHistoryBL profileContentHistoryBL = new ProfileContentHistoryBL(executionContext, profileContentHistoryDTO);
                        profileContentHistoryBL.Save(sqlTransaction);
                    }
                }
            }           


            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the Profile Security Attributes
        /// Performs only update
        /// </summary>
        public void SaveSecurityAttributes(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ProfileDataHandler profileDataHandler = new ProfileDataHandler(passPhrase, sqlTransaction);
            if (profileDTO.IsChanged)
            {
                profileDTO = profileDataHandler.UpdateProfileSecurityAttributes(profileDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                profileDTO.AcceptChanges();
                CreateRoamingData(sqlTransaction);
            }
            log.LogMethodExit();
        }

        internal void CreateRoamingData(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ProfileDTO savedProfileDTO = (new ProfileBL(executionContext, profileDTO.Id, false, false, sqlTransaction)).ProfileDTO;
            DBSynchLogService dBSynchLogService = new DBSynchLogService(executionContext, "Profile", savedProfileDTO.Guid, savedProfileDTO.SiteId);
            dBSynchLogService.CreateRoamingDataForCustomer(sqlTransaction);
            log.LogMethodExit();
		}

		/// <summary>
		/// Gets the DTO
		/// </summary>
		public ProfileDTO ProfileDTO
        {
            get
            {
                return profileDTO;
            }
        }


        /// <summary>
        /// Validates the Profile, throws ValidationException if any fields are not valid
        /// </summary>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            ProfileDataHandler profileDataHandler = new ProfileDataHandler(passPhrase, sqlTransaction);
            if (profileDTO != null)
            {
                if (profileDTO.IsActive == false && profileDataHandler.GetProfileReferenceCount(profileDTO.Id) > 0)
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 1143);
                    validationErrorList.Add(new ValidationError("Profile", "", errorMessage));
                }
                if (Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "OPT_IN_PROMOTIONS") == "M" &&
                   !profileDTO.OptInPromotions )
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 249, MessageContainerList.GetMessage(executionContext, "Promotion Opt-in"));
                    validationErrorList.Add(new ValidationError("Profile", "OptInPromotions", errorMessage));
                }
                if ( Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "OPT_IN_PROMOTIONS_MODE") == "M"
                    && string.IsNullOrEmpty(profileDTO.OptInPromotionsMode))
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 1717);
                    validationErrorList.Add(new ValidationError("Profile", "OptInPromotionsMode", errorMessage));
                }
                if (Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "TERMS_AND_CONDITIONS") == "M" &&
                   !profileDTO.PolicyTermsAccepted)
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 249, MessageContainerList.GetMessage(executionContext, "Terms and Conditions"));
                    validationErrorList.Add(new ValidationError("Profile", "Terms and Conditions", errorMessage));
                }
                if (profileDTO.AddressDTOList != null)
                {
                    for (int i = 0; i < profileDTO.AddressDTOList.Count; i++)
                    {
                        var addressDTO = profileDTO.AddressDTOList[i];
                        AddressBL addressBL = new AddressBL(executionContext, addressDTO);
                        List<ValidationError> validationErrors = addressBL.Validate(sqlTransaction);
                        foreach (var validationError in validationErrors)
                        {
                            validationError.RecordIndex = i;
                        }
                        validationErrorList.AddRange(validationErrors);
                    }
                }
                if (profileDTO.ContactDTOList != null)
                {
                    for (int i = 0; i < profileDTO.ContactDTOList.Count; i++)
                    {
                        var contactDTO = profileDTO.ContactDTOList[i];
                        ContactBL contactBL = new ContactBL(executionContext, contactDTO);
                        List<ValidationError> validationErrors = contactBL.Validate(sqlTransaction);
                        foreach (var validationError in validationErrors)
                        {
                            validationError.RecordIndex = i;
                        }
                        validationErrorList.AddRange(validationErrors);
                    }
                }
                if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AUTO_GENERATE_NICKNAME") == "Y" &&
                   string.IsNullOrEmpty(profileDTO.NickName))
                {
                    NicknameBL nicknameBL = new NicknameBL(executionContext);
                    profileDTO.NickName = nicknameBL.GetNickName();
                }
                else if ((ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AUTO_GENERATE_NICKNAME") == "N" ||
                   ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AUTO_GENERATE_NICKNAME") == "") &&
                  string.IsNullOrEmpty(profileDTO.NickName) && ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "NICKNAME") == "M")
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 249, MessageContainerList.GetMessage(executionContext, "Nickname"));
                    validationErrorList.Add(new ValidationError("Profile", "Nickname", errorMessage));
                }
                bool excludeWords = NickNameDTO.excludedWordsList.Contains(profileDTO.NickName);
                if (excludeWords)
                {
                    log.Error("The NickName is not valid, please register the customer again");
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 5176);
                    validationErrorList.Add(new ValidationError("Profile", "NickName", errorMessage));
                }
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Returns the profile image
        /// </summary>
        /// <returns>profile image</returns>
        public Image GetProfileImage()
        {
            log.LogMethodEntry();
            Image profileImage = null;
            if(string.IsNullOrWhiteSpace(profileDTO.PhotoURL) == false)
            {
                ProfileDataHandler profileDataHandler = new ProfileDataHandler(passPhrase, null);
                object file = profileDataHandler.GetFile(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "IMAGE_DIRECTORY") + "\\" + ProfileDTO.PhotoURL);
                if (file != DBNull.Value)
                {
                    byte[] b = file as byte[];
                    if(b != null)
                    {
                        try
                        {
                            b = Encryption.Decrypt(b);
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            b = file as byte[];
                        }
                        using (System.IO.MemoryStream ms = new System.IO.MemoryStream(b))
                        {
                            profileImage = System.Drawing.Image.FromStream(ms);
                        }
                    }
                }
            }
            log.LogMethodExit("profileImage");
            return profileImage;
        }

        /// <summary>
        /// Returns base64 ProfileImage of Customer
        /// </summary>
        /// <returns></returns>
        public string GetProfileImageBase64()
        {
            log.LogMethodEntry();
            string profileImage = string.Empty;
            if (string.IsNullOrWhiteSpace(profileDTO.PhotoURL))
            {
                log.LogMethodExit(profileImage, "PhotoURL is empty");
                return profileImage;
            }
            ProfileDataHandler profileDataHandler = new ProfileDataHandler(passPhrase, null);
            object file = profileDataHandler.GetFile(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "IMAGE_DIRECTORY") + "\\" + ProfileDTO.PhotoURL);
            profileImage = GetBase64String(file);
            log.LogMethodExit("profileImage");
            return profileImage;
        }
        /// <summary>
        /// Return base64 IdProofFile of customer
        /// </summary>
        ///<returns></returns>
        public string GetIdImageBase64()
        {
            log.LogMethodEntry();
            string idImage = string.Empty;
            if (string.IsNullOrWhiteSpace(profileDTO.IdProofFileURL))
            {
                log.LogMethodExit(idImage, "IdProofFileURL is empty");
                return idImage;
            }
            ProfileDataHandler profileDataHandler = new ProfileDataHandler(passPhrase, null);
            object file = profileDataHandler.GetFile(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "IMAGE_DIRECTORY") + "\\" + ProfileDTO.IdProofFileURL);
            idImage = GetBase64String(file);
            log.LogMethodExit("idImage");
            return idImage;
        }
        /// <summary>
        /// This method will retrun the image to base64 string
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private string GetBase64String(object file)
        {
            log.LogMethodEntry("file");
            string result = string.Empty;
            if (file == DBNull.Value)
            {
                log.LogMethodExit(result, "file is empty");
                return result;
            }
            byte[] bytes = file as byte[];
            if(bytes == null)
            {
                log.LogMethodExit(result, "file is empty");
                return result;
            }
            try
            {
                bytes = Encryption.Decrypt(bytes);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                bytes = file as byte[];
            }
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes))
            {
                result = System.Convert.ToBase64String(ms.ToArray());
            }
            log.LogMethodExit("result");
            return result;
        }

        /// <summary>
        /// Copies the Id Proof file to a temporary folder and returns the url 
        /// </summary>
        /// <returns></returns>
        public string CopyIdProofFileToTempDirectory()
        {
            log.LogMethodEntry();
            string temporaryFilePath = string.Empty;
            if(string.IsNullOrWhiteSpace(profileDTO.IdProofFileURL) == false)
            {
                ProfileDataHandler profileDataHandler = new ProfileDataHandler(passPhrase, null);
                object file = profileDataHandler.GetFile(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "IMAGE_DIRECTORY") + "\\" + profileDTO.IdProofFileURL);
                if (file != null && file != DBNull.Value)
                {
                    byte[] bytes = file as byte[];
                    if (bytes != null)
                    {
                        try
                        {
                            bytes = Encryption.Decrypt(bytes);
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            bytes = file as byte[];
                        }
                        string extension = profileDTO.IdProofFileURL;
                        try
                        {
                            extension = (new System.IO.FileInfo(extension)).Extension;
                        }
                        catch(Exception ex)
                        {
                            log.Error("Error occured while calculating file extension. IdProofFileURL: " + profileDTO.IdProofFileURL, ex);
                        }
                        string tempFile = System.IO.Path.GetTempPath() + "ProfileIdProof" + Guid.NewGuid().ToString() + extension;
                        using (System.IO.FileStream fileStream = new System.IO.FileStream(tempFile, System.IO.FileMode.Create, System.IO.FileAccess.Write))
                        {
                            fileStream.Write(bytes, 0, bytes.Length);
                        }
                        temporaryFilePath = tempFile;
                    }
                }
            }
            log.LogMethodExit(temporaryFilePath);
            return temporaryFilePath;
        }

        /// <summary>
        /// Saves the id proof file
        /// </summary>
        /// <param name="fileName">file name</param>
        /// <param name="file">file to be uploaded</param>
        public void SaveIdProofFile(string fileName, byte[] file)
        {
            log.LogMethodEntry(fileName, "file");
            if(string.IsNullOrWhiteSpace(fileName) == false && file != null)
            {
                string encryptedString = Encryption.Encrypt(file);
                file = Convert.FromBase64String(encryptedString);
                ProfileDataHandler profileDataHandler = new ProfileDataHandler(passPhrase, null);
                profileDataHandler.SaveFile(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "IMAGE_DIRECTORY") + "\\" + fileName, file);
                profileDTO.IdProofFileURL = fileName;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the customer photo
        /// </summary>
        /// <param name="fileName">file name</param>
        /// <param name="file">file to be uploaded</param>
        public void SaveProfilePhoto(string fileName, byte[] file)
        {
            log.LogMethodEntry(fileName, "file");
            if (string.IsNullOrWhiteSpace(fileName) == false && file != null)
            {
                string encryptedString = Encryption.Encrypt(file);
                file = Convert.FromBase64String(encryptedString);
                ProfileDataHandler profileDataHandler = new ProfileDataHandler(passPhrase, null);
                profileDataHandler.SaveFile(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "IMAGE_DIRECTORY") + "\\" + fileName, file);
                profileDTO.PhotoURL = fileName;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Authenticates the profile using the password
        /// </summary>
        /// <param name="password"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public bool Authenticate(string password, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry("password", sqlTransaction);
            ProfileDataHandler profileDataHandler = new ProfileDataHandler(passPhrase, sqlTransaction);
            string profilePassword = profileDataHandler.GetPassword(profileDTO.Id);
            bool result = false;
            //if (string.IsNullOrWhiteSpace(encryptedPassword) == false)
            //{
            //    result = encryptedPassword.Equals(Encryption.Encrypt(password));
            //}

            // Added Customer Security to verify the password-Option Security Policy Enhancement - 24 June 2020
            CustomerSecurity customerSecurity = new CustomerSecurity(executionContext);
            result = customerSecurity.Login(profileDTO.Id, password, profilePassword);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Updates the profile password
        /// </summary>
        /// <param name="password">new password</param>
        /// <param name="sqlTransaction">sql transaction</param>
        public void UpdatePassword(string password, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry("password", sqlTransaction);
            ProfileDataHandler profileDataHandler = new ProfileDataHandler(passPhrase, sqlTransaction);

            CustomerSecurity customerSecurity = new CustomerSecurity(executionContext);
            customerSecurity.ValidatePassword(ProfileDTO.UserName, password, ProfileDTO.Id);
            
            profileDataHandler.SetPassword(ProfileDTO.Id, Encryption.Encrypt(password), executionContext.GetUserId());

            if (customerSecurity.IsPasswordHistoryEnabled())
            {
                CustomerPasswordHistoryDTO customerPasswordHistoryDTO = new CustomerPasswordHistoryDTO();
                customerPasswordHistoryDTO.Password = Encryption.Encrypt(password);
                customerPasswordHistoryDTO.ProfileId = ProfileDTO.Id;
                CustomerPasswordHistoryBL customerPasswordHistoryBL = new CustomerPasswordHistoryBL(executionContext, customerPasswordHistoryDTO);
                customerPasswordHistoryBL.Save(sqlTransaction);
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of Profile
    /// </summary>
    public class ProfileListBL
    {
        private string passPhrase;
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public ProfileListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            passPhrase = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CUSTOMER_ENCRYPTION_PASS_PHRASE");
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the Profile list
        /// </summary>
        public List<ProfileDTO> GetProfileDTOList(List<KeyValuePair<ProfileDTO.SearchByParameters, string>> searchParameters,
            bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, loadChildRecords, activeChildRecords, sqlTransaction);
            ProfileDataHandler profileDataHandler = new ProfileDataHandler(passPhrase, sqlTransaction);
            List<ProfileDTO> profileDTOList = profileDataHandler.GetProfileDTOList(searchParameters);
            if (loadChildRecords)
            {
                if (profileDTOList != null && profileDTOList.Count > 0)
                {
                    Build(profileDTOList, activeChildRecords, sqlTransaction);
                }
            }
            log.LogMethodExit(profileDTOList);
            return profileDTOList;
        }

        /// <summary>
        /// Returns the Profile list
        /// </summary>
        public List<ProfileDTO> GetProfileDTOList(List<int> profileIdList,
            bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(profileIdList, loadChildRecords, activeChildRecords, sqlTransaction);
            ProfileDataHandler profileDataHandler = new ProfileDataHandler(passPhrase, sqlTransaction);
            List<ProfileDTO> profileDTOList = profileDataHandler.GetProfileDTOList(profileIdList);
            if (loadChildRecords)
            {
                if (profileDTOList != null && profileDTOList.Count > 0)
                {
                    Build(profileDTOList, activeChildRecords, sqlTransaction);
                }
            }
            log.LogMethodExit(profileDTOList);
            return profileDTOList;
        }

        private void Build(List<ProfileDTO> profileDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(profileDTOList, activeChildRecords, sqlTransaction);
            if (profileDTOList != null && profileDTOList.Count > 0)
            {
                Dictionary<int, ProfileDTO> profileDTODictionary = new Dictionary<int, ProfileDTO>();
                List<int> profileIdList = new List<int>();
                for (int i = 0; i < profileDTOList.Count; i++)
                {
                    profileDTODictionary.Add(profileDTOList[i].Id, profileDTOList[i]);
                    profileIdList.Add(profileDTOList[i].Id);
                }
                
                AddressListBL addressListBL = new AddressListBL(executionContext);
                List<AddressDTO> addressDTOList = addressListBL.GetAddressDTOList(profileIdList, activeChildRecords, sqlTransaction);
                if (addressDTOList != null)
                {
                    foreach (var addressDTO in addressDTOList)
                    {
                        if (profileDTODictionary.ContainsKey(addressDTO.ProfileId))
                        {
                            if (profileDTODictionary[addressDTO.ProfileId].AddressDTOList == null)
                            {
                                profileDTODictionary[addressDTO.ProfileId].AddressDTOList = new List<AddressDTO>();
                            }
                            profileDTODictionary[addressDTO.ProfileId].AddressDTOList.Add(addressDTO);
                        }
                    }
                }

                ContactListBL contactListBL = new ContactListBL(executionContext);
                List<ContactDTO> contactDTOList = contactListBL.GetContactDTOList(profileIdList, activeChildRecords, sqlTransaction);
                if (contactDTOList != null)
                {
                    foreach (var contactDTO in contactDTOList)
                    {
                        if (profileDTODictionary.ContainsKey(contactDTO.ProfileId))
                        {
                            if (profileDTODictionary[contactDTO.ProfileId].ContactDTOList == null)
                            {
                                profileDTODictionary[contactDTO.ProfileId].ContactDTOList = new List<ContactDTO>();
                            }
                            profileDTODictionary[contactDTO.ProfileId].ContactDTOList.Add(contactDTO);
                        }
                    }
                }

                ProfileContentHistoryListBL profileContentHistoryListBL = new ProfileContentHistoryListBL();
                List<ProfileContentHistoryDTO> profileContentHistoryDTOList = profileContentHistoryListBL.GetAllProfileContentHistory(profileIdList, activeChildRecords, sqlTransaction);
                if (profileContentHistoryDTOList != null)
                {
                    foreach (var profileContentHistoryDTO in profileContentHistoryDTOList)
                    {
                        if (profileDTODictionary.ContainsKey(profileContentHistoryDTO.ProfileId))
                        {
                            if (profileDTODictionary[profileContentHistoryDTO.ProfileId].ProfileContentHistoryDTOList == null)
                            {
                                profileDTODictionary[profileContentHistoryDTO.ProfileId].ProfileContentHistoryDTOList = new List<ProfileContentHistoryDTO>();
                            }
                            profileDTODictionary[profileContentHistoryDTO.ProfileId].ProfileContentHistoryDTOList.Add(profileContentHistoryDTO);
                        }
                    }
                }
            }
            log.LogMethodEntry();
        }
    }
}
