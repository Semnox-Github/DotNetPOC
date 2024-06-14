/********************************************************************************************
 * Project Name - Profile Data Handler
 * Description  - Data handler of the Profile class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        06-Feb-2017   Lakshminarayana     Created 
 *2.70.2      19-Jul-2019    Girish Kundar       Modified :Structure of data Handler - insert /Update methods
 *                                                          Fix for SQL Injection Issue  
 *2.70.2      09-Oct-2019    Akshay Gulaganji    ClubSpeed interface phase-1 enhancement changes - Added ExternalSystemReference
 *2.70.3      10-Mar-2020    Lakshminarayana    Updated to change the customer authentication process
 *2.90        24-Jun-2020    Indrajeet Kumar    Modified - Insert & Update Query to enhance - password option- security policy
  ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    ///  Profile Data Handler - Handles insert, update and select of  Profile objects
    /// </summary>
    public class ProfileDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string passPhrase;
        private SqlTransaction sqlTransaction;
        private static readonly string PROFILE_SELECT_QUERY = @"SELECT Profile.Id, Profile.ProfileTypeId, Profile.Title, Profile.FirstName,Profile.NickName, Profile.MiddleName, Profile.LastName,
                                                                Profile.OptInPromotions, Profile.OptInPromotionsMode, Profile.OptInLastUpdatedDate, Profile.PolicyTermsAccepted,
                                                                CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,Profile.DateOfBirth)) AS DateOfBirth, 
                                                                CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,Profile.Anniversary)) AS Anniversary,
                                                                CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,Profile.UniqueId)) AS UniqueId,
                                                                Profile.Gender, Profile.Notes,Profile.PhotoURL, Profile.RightHanded, Profile.TeamUser, Profile.IdProofFileURL, 
                                                                CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,TaxCode)) AS TaxCode, ProfileType.Name AS ProfileType,
                                                                Profile.LastLoginTime,Profile.Company, Profile.Designation, Profile.IsActive, Profile.CreatedBy, Profile.CreationDate, 
                                                                CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,Profile.Username)) AS Username,
                                                                Profile.LastUpdatedBy, Profile.LastUpdateDate, Profile.Guid, Profile.SynchStatus, Profile.site_id, Profile.MasterEntityId, Profile.ExternalSystemReference,
                                                                Profile.UserStatus, Profile.PasswordChangeDate, Profile.InvalidAccessAttempts, Profile.LockedOutTime, Profile.PasswordChangeOnNextLogin,
                                                                Profile.OptOutWhatsApp
                                                                FROM Profile
                                                                LEFT OUTER JOIN ProfileType ON Profile.ProfileTypeId = ProfileType.Id ";
        private static readonly Dictionary<ProfileDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ProfileDTO.SearchByParameters, string>
            {
                {ProfileDTO.SearchByParameters.ID, "Profile.Id"},
                {ProfileDTO.SearchByParameters.ID_LIST, "Profile.Id"},
                {ProfileDTO.SearchByParameters.PROFILE_TYPE_ID, "Profile.ProfileTypeId"},
                {ProfileDTO.SearchByParameters.PROFILE_TYPE, "ProfileType.Name"},
                {ProfileDTO.SearchByParameters.FIRST_NAME, "Profile.FirstName"},
                {ProfileDTO.SearchByParameters.LAST_NAME, "Profile.LastName"},
                {ProfileDTO.SearchByParameters.NICK_NAME, "Profile.NickName"},
                {ProfileDTO.SearchByParameters.OPT_IN_PROMOTIONS, "Profile.OptInPromotions"},
                {ProfileDTO.SearchByParameters.POLICY_TERMS_ACCEPTED, "Profile.PolicyTermsAccepted"},
                {ProfileDTO.SearchByParameters.IS_ACTIVE,"Profile.IsActive"},
                {ProfileDTO.SearchByParameters.MASTER_ENTITY_ID,"Profile.MasterEntityId"},
                {ProfileDTO.SearchByParameters.SITE_ID, "Profile.site_id"},
                { ProfileDTO.SearchByParameters.EXTERNAL_SYSTEM_REFERENCE,"Profile.ExternalSystemReference" },
                { ProfileDTO.SearchByParameters.LAST_UPDATE_FROM_DATE,"Profile.LastUpdateDate" },
                { ProfileDTO.SearchByParameters.LAST_UPDATE_TO_DATE,"Profile.LastUpdateDate" },
                { ProfileDTO.SearchByParameters.OPT_OUT_WHATSAPP,"Profile.OptOutWhatsApp" }
            };
        private DataAccessHandler dataAccessHandler;

        /// <summary>
        /// Default constructor of ProfileDataHandler class
        /// </summary>
        public ProfileDataHandler(string passPhrase, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry("passPhrase", sqlTransaction);
            this.passPhrase = passPhrase;
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating Profile Record.
        /// </summary>
        /// <param name="profileDTO">ProfileDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(ProfileDTO profileDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(profileDTO, loginId, siteId);
            profileDTO.Title = profileDTO.Title != null ? profileDTO.Title.Trim() : string.Empty;
            profileDTO.FirstName = profileDTO.FirstName != null ? profileDTO.FirstName.Trim() : string.Empty;
            profileDTO.MiddleName = profileDTO.MiddleName != null ? profileDTO.MiddleName.Trim() : string.Empty;
            profileDTO.LastName = profileDTO.LastName != null ? profileDTO.LastName.Trim() : string.Empty;
            profileDTO.Notes = profileDTO.Notes != null ? profileDTO.Notes.Trim() : string.Empty;
            profileDTO.UniqueIdentifier = profileDTO.UniqueIdentifier != null ? profileDTO.UniqueIdentifier.Trim() : string.Empty;
            profileDTO.UserName = profileDTO.UserName != null ? profileDTO.UserName.Trim() : string.Empty;
            profileDTO.ExternalSystemReference = profileDTO.ExternalSystemReference != null ? profileDTO.ExternalSystemReference.Trim() : string.Empty;
            profileDTO.TaxCode = profileDTO.TaxCode != null ? profileDTO.TaxCode.Trim() : string.Empty;
            profileDTO.Company = profileDTO.Company != null ? profileDTO.Company.Trim() : string.Empty;
            profileDTO.Designation = profileDTO.Designation != null ? profileDTO.Designation.Trim() : string.Empty;
            profileDTO.UserName = profileDTO.UserName != null ? profileDTO.UserName.Trim() : string.Empty;

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", profileDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProfileType", profileDTO.ProfileType.ToString()));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Title", profileDTO.Title));
            parameters.Add(dataAccessHandler.GetSQLParameter("@FirstName", profileDTO.FirstName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MiddleName", profileDTO.MiddleName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastName", profileDTO.LastName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@NickName", profileDTO.NickName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Notes", profileDTO.Notes));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DateOfBirth", profileDTO.DateOfBirth.HasValue ? profileDTO.DateOfBirth.Value.Date.ToString("yyyy/MM/dd HH:mm:ss") : null));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Gender", profileDTO.Gender));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Anniversary", profileDTO.Anniversary.HasValue ? profileDTO.Anniversary.Value.Date.ToString("yyyy/MM/dd HH:mm:ss") : null));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PhotoURL", profileDTO.PhotoURL));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RightHanded", profileDTO.RightHanded));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TeamUser", profileDTO.TeamUser));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UniqueIdentifier", profileDTO.UniqueIdentifier));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IdProofFileURL", profileDTO.IdProofFileURL));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TaxCode", profileDTO.TaxCode));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Company", profileDTO.Company));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Designation", profileDTO.Designation));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UserName", profileDTO.UserName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastLoginTime", profileDTO.LastLoginTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@OptInPromotions", profileDTO.OptInPromotions == true ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@OptInPromotionsMode", profileDTO.OptInPromotionsMode));
            parameters.Add(dataAccessHandler.GetSQLParameter("@OptInLastUpdatedDate", profileDTO.OptInLastUpdatedDate, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PolicyTermsAccepted", profileDTO.PolicyTermsAccepted == true ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PassPhrase", passPhrase));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", profileDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", profileDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ExternalSystemReference", profileDTO.ExternalSystemReference));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UserStatus", string.IsNullOrEmpty(profileDTO.UserStatus) ?DBNull.Value : (object)profileDTO.UserStatus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PasswordChangeDate", profileDTO.PasswordChangeDate == DateTime.MinValue ? DBNull.Value : (object)profileDTO.PasswordChangeDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@InvalidAccessAttempts", profileDTO.InvalidAccessAttempts == -1 ? DBNull.Value : (object)profileDTO.InvalidAccessAttempts));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LockedOutTime", profileDTO.LockedOutTime == DateTime.MinValue ? DBNull.Value : (object)profileDTO.LockedOutTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PasswordChangeOnNextLogin", profileDTO.PasswordChangeOnNextLogin));
            parameters.Add(dataAccessHandler.GetSQLParameter("@OptOutWhatsApp", profileDTO.OptOutWhatsApp));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the Profile record to the database
        /// </summary>
        /// <param name="profileDTO">ProfileDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns ProfileDTO</returns>
        public ProfileDTO InsertProfile(ProfileDTO profileDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(profileDTO, loginId, siteId);
            string query = @"INSERT INTO Profile 
                                        ( 
                                            ProfileTypeId,
                                            Title,
                                            FirstName,
                                            MiddleName,
                                            LastName,
                                            Notes,
                                            DateOfBirth,
                                            Gender,
                                            Anniversary,
                                            PhotoURL,
                                            RightHanded,
                                            TeamUser,
                                            UniqueId,
                                            IdProofFileURL,
                                            TaxCode,
                                            Company,
                                            Designation,
                                            UserName,
                                            LastLoginTime,
                                            OptInPromotions,
                                            OptInPromotionsMode,
                                            OptInLastUpdatedDate,
                                            PolicyTermsAccepted,
                                            IsActive,
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdatedBy,
                                            LastUpdateDate,
                                            site_id,
                                            MasterEntityId,
                                            ExternalSystemReference,
                                            UserStatus,
                                            PasswordChangeDate,
                                            InvalidAccessAttempts,
                                            LockedOutTime,
                                            PasswordChangeOnNextLogin,
                                            OptOutWhatsApp,
                                            HashDateOfBirth,
                                            HashAnniversary,
                                            HashUniqueId,
                                            HashTaxCode,
                                            HashUserName,
                                            NickName
                                        ) 
                                VALUES 
                                        (
                                            (SELECT Id FROM ProfileType WHERE Name = @ProfileType),
                                            @Title,
                                            @FirstName,
                                            @MiddleName,
                                            @LastName,
                                            @Notes,
                                            ENCRYPTBYPASSPHRASE(@PassPhrase, @DateOfBirth),
                                            @Gender,
                                            ENCRYPTBYPASSPHRASE(@PassPhrase, @Anniversary),
                                            @PhotoURL,
                                            @RightHanded,
                                            @TeamUser,
                                            ENCRYPTBYPASSPHRASE(@PassPhrase, @UniqueIdentifier),
                                            @IdProofFileURL,
                                            ENCRYPTBYPASSPHRASE(@PassPhrase, @TaxCode),
                                            @Company,
                                            @Designation,
                                            ENCRYPTBYPASSPHRASE(@PassPhrase, @UserName),
                                            @LastLoginTime,
                                            @OptInPromotions,
                                            @OptInPromotionsMode,
                                            case when @OptInPromotions = 'Y' then getdate() else null end ,
                                            @PolicyTermsAccepted,
                                            @IsActive,
                                            @CreatedBy,
                                            GETDATE(),
                                            @LastUpdatedBy,
                                            GETDATE(),
                                            @site_id,
                                            @MasterEntityId,
                                            @ExternalSystemReference,
                                            @UserStatus,
                                            @PasswordChangeDate,
                                            @InvalidAccessAttempts,
                                            @LockedOutTime,
                                            @PasswordChangeOnNextLogin,
                                            @OptOutWhatsApp,
                                            CASE WHEN @DateOfBirth IS NOT NULL THEN 
                                            hashbytes('SHA2_256', upper(Convert(nvarchar(100), convert(datetime,@DateOfBirth,120), 120)))
                                            ELSE NULL END,
                                            CASE WHEN @Anniversary IS NOT NULL THEN 
                                            hashbytes('SHA2_256', upper(Convert(nvarchar(100), convert(datetime,@Anniversary,120), 120)))
                                            ELSE NULL END, 
                                            hashbytes('SHA2_256',convert(nvarchar(max), upper(@UniqueIdentifier))),
                                            hashbytes('SHA2_256',convert(nvarchar(max), upper(@TaxCode))),
                                            hashbytes('SHA2_256',convert(nvarchar(max), upper(@UserName))),
                                            @NickName
                                        ) SELECT * FROM Profile WHERE Id  = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(profileDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshProfileDTO(profileDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting profileDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(profileDTO);
            return profileDTO;
        }

        /// <summary>
        /// Updates the Profile record
        /// </summary>
        /// <param name="profileDTO">ProfileDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the ProfileDTO</returns>
        public ProfileDTO UpdateProfile(ProfileDTO profileDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(profileDTO, loginId, siteId);
            string query = @"UPDATE Profile 
                             SET ProfileTypeId=(SELECT Id FROM ProfileType WHERE Name = @ProfileType),
                                 Title=@Title,
                                 FirstName=@FirstName,
                                 MiddleName=@MiddleName,
                                 LastName=@LastName,  
                                 Notes=@Notes,
                                 DateOfBirth=ENCRYPTBYPASSPHRASE(@PassPhrase, @DateOfBirth),   
                                 Gender=@Gender,
                                 Anniversary=ENCRYPTBYPASSPHRASE(@PassPhrase, @Anniversary),
                                 PhotoURL = @PhotoURL,
                                 RightHanded = @RightHanded,
                                 TeamUser = @TeamUser,
                                 UniqueId = ENCRYPTBYPASSPHRASE(@PassPhrase, @UniqueIdentifier),
                                 IdProofFileURL = @IdProofFileURL,
                                 TaxCode = ENCRYPTBYPASSPHRASE(@PassPhrase, @TaxCode),
                                 Company = @Company,
                                 Designation = @Designation,
                                 UserName = ENCRYPTBYPASSPHRASE(@PassPhrase, @UserName),
                                 LastLoginTime=@LastLoginTime,
                                 OptInPromotions = @OptInPromotions,
                                 OptInPromotionsMode = @OptInPromotionsMode,
								 OptInLastUpdatedDate = case when @OptInPromotions <> isnull(OptInPromotions,'N') or @OptInPromotionsMode <>  isnull(OptInPromotionsMode,'') then getdate() else OptInLastUpdatedDate end ,
                                 PolicyTermsAccepted = @PolicyTermsAccepted,
                                 IsActive = @IsActive,
                                 LastUpdatedBy = @LastUpdatedBy,
                                 LastUpdateDate=GETDATE(),
                                 MasterEntityId=@MasterEntityId,
                                 ExternalSystemReference=@ExternalSystemReference,
                                 OptOutWhatsApp = @OptOutWhatsApp,
                                 HashDateOfBirth = CASE WHEN @DateOfBirth IS NOT NULL THEN 
                                                       hashbytes('SHA2_256', upper(Convert(nvarchar(100), convert(datetime,@DateOfBirth,120), 120)))
                                                  ELSE NULL END,
                                 HashAnniversary = CASE WHEN @Anniversary IS NOT NULL THEN 
                                                       hashbytes('SHA2_256', upper(Convert(nvarchar(100), convert(datetime,@Anniversary,120), 120)))
                                                  ELSE NULL END, 
                                 HashUniqueId = hashbytes('SHA2_256',convert(nvarchar(max), upper(@UniqueIdentifier))),
                                 HashTaxCode = hashbytes('SHA2_256',convert(nvarchar(max), upper(@TaxCode))),
                                 HashUserName = hashbytes('SHA2_256',convert(nvarchar(max), upper(@UserName))),
                                 NickName = @NickName
                             WHERE Id = @Id 
                           SELECT * FROM Profile WHERE Id  = @Id ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(profileDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshProfileDTO(profileDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating profileDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(profileDTO);
            return profileDTO;
        }

        /// <summary>
        /// Updates the Profile record
        /// </summary>
        /// <param name="profileDTO">ProfileDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the ProfileDTO</returns>
        public ProfileDTO UpdateProfileSecurityAttributes(ProfileDTO profileDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(profileDTO, loginId, siteId);
            string query = @"UPDATE Profile 
                             SET UserStatus =@UserStatus,
                                 PasswordChangeDate=@PasswordChangeDate,
                                 InvalidAccessAttempts=@InvalidAccessAttempts,
                                 LockedOutTime=@LockedOutTime,
                                 PasswordChangeOnNextLogin=@PasswordChangeOnNextLogin
                             WHERE Id = @Id 
                           SELECT * FROM Profile WHERE Id  = @Id ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(profileDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshProfileDTO(profileDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating profileDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(profileDTO);
            return profileDTO;
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="profileDTO">ProfileDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshProfileDTO(ProfileDTO profileDTO, DataTable dt)
        {
            log.LogMethodEntry(profileDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                profileDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                profileDTO.LastUpdateDate = dataRow["LastupdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdateDate"]);
                profileDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                profileDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                profileDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                profileDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                profileDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
        }

        /// <summary>
        /// Checks whether profile is in use.
        /// <param name="id">Profile Id</param>
        /// </summary>
        /// <returns>Returns refrenceCount</returns>
        public int GetProfileReferenceCount(int id)
        {
            log.LogMethodEntry(id);
            int refrenceCount = 0;
            string query = @"SELECT 
                            (
                            SELECT COUNT(1) 
                            FROM Customers
                            WHERE ProfileId = @ProfileId
                            AND IsActive = 1 
                            )
                            AS ReferenceCount";
            SqlParameter parameter = new SqlParameter("@ProfileId", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { dataAccessHandler.GetSQLParameter("@ProfileId", id, true) }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                refrenceCount = Convert.ToInt32(dataTable.Rows[0]["ReferenceCount"]);
            }
            log.LogMethodExit(refrenceCount);
            return refrenceCount;
        }

        /// <summary>
        /// Returns file 
        /// <param name="fileUrl">file url</param>
        /// </summary>
        /// <returns>Returns refrenceCount</returns>
        public object GetFile(string fileUrl)
        {
            log.LogMethodEntry(fileUrl);
            object file = null;
            string query = @"exec ReadBinaryDataFromFile @FileUrl";
            file = dataAccessHandler.executeScalar(query, new SqlParameter[] { dataAccessHandler.GetSQLParameter("@FileUrl", fileUrl) }, sqlTransaction);
            log.LogMethodExit("file");
            return file;
        }

        /// <summary>
        /// Save the file to destination url
        /// </summary>
        /// <param name="destinationFileUrl">destination url</param>
        /// <param name="file">file to be saved</param>
        /// <returns></returns>
        public void SaveFile(string destinationFileUrl, byte[] file)
        {
            log.LogMethodEntry(destinationFileUrl, "file");
            try
            {
                string query = @"exec SaveBinaryDataToFile @bytes, @FileName";
                dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { dataAccessHandler.GetSQLParameter("@bytes", file), dataAccessHandler.GetSQLParameter("@FileName", destinationFileUrl) }, sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while saving the file", ex);
                throw ex;
            }
            log.LogMethodExit();
        }

        private ProfileType GetProfileType(string profileTypeString)
        {
            log.LogMethodEntry(profileTypeString);
            ProfileType profileType = ProfileType.NONE;
            try
            {
                profileType = (ProfileType)Enum.Parse(typeof(ProfileType), profileTypeString, true);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while parsing the profile type", ex);
                throw ex;
            }
            log.LogMethodExit(profileType);
            return profileType;
        }

        /// <summary>
        /// Converts the Data row object to ProfileDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns ProfileDTO</returns>
        private ProfileDTO GetProfileDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ProfileDTO profileDTO = new ProfileDTO(Convert.ToInt32(dataRow["Id"]),
                                            dataRow["ProfileTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ProfileTypeId"]),
                                            dataRow["ProfileType"] == DBNull.Value ? GetProfileType("NONE") : GetProfileType(dataRow["ProfileType"].ToString()),
                                            dataRow["Title"] == DBNull.Value ? string.Empty : dataRow["Title"].ToString().Trim(),
                                            dataRow["FirstName"] == DBNull.Value ? string.Empty : dataRow["FirstName"].ToString().Trim(),
                                            dataRow["MiddleName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["MiddleName"]).Trim(),
                                            dataRow["LastName"] == DBNull.Value ? string.Empty : dataRow["LastName"].ToString().Trim(),
                                            dataRow["Notes"] == DBNull.Value ? string.Empty : dataRow["Notes"].ToString().Trim(),
                                            dataRow["DateOfBirth"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["DateOfBirth"]).Date,
                                            dataRow["Gender"] == DBNull.Value ? "N" : dataRow["Gender"].ToString(),
                                            dataRow["Anniversary"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["Anniversary"]).Date,
                                            dataRow["PhotoURL"] == DBNull.Value ? string.Empty : dataRow["PhotoURL"].ToString(),
                                            dataRow["RightHanded"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["RightHanded"]),
                                            dataRow["TeamUser"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["TeamUser"]),
                                            dataRow["UniqueId"] == DBNull.Value ? string.Empty : dataRow["UniqueId"].ToString().Trim(),
                                            dataRow["IdProofFileURL"] == DBNull.Value ? string.Empty : dataRow["IdProofFileURL"].ToString(),
                                            dataRow["TaxCode"] == DBNull.Value ? string.Empty : dataRow["TaxCode"].ToString().Trim(),
                                            dataRow["Company"] == DBNull.Value ? string.Empty : dataRow["Company"].ToString().Trim(),
                                            dataRow["Designation"] == DBNull.Value ? string.Empty : dataRow["Designation"].ToString().Trim(),
                                            dataRow["Username"] == DBNull.Value ? string.Empty : dataRow["Username"].ToString().Trim(),
                                            string.Empty,
                                            dataRow["LastLoginTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["LastLoginTime"]),
                                            dataRow["OptInPromotions"] == DBNull.Value ? false : dataRow["OptInPromotions"].ToString() == "Y" ? true : false,
                                            dataRow["OptInPromotionsMode"] == DBNull.Value ? string.Empty : dataRow["OptInPromotionsMode"].ToString(),
                                            dataRow["OptInLastUpdatedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["OptInLastUpdatedDate"]),
                                            dataRow["PolicyTermsAccepted"] == DBNull.Value ? false : dataRow["PolicyTermsAccepted"].ToString() == "Y" ? true : false,
                                            dataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["IsActive"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString(),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString(),
                                            dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString(),
                                            dataRow["ExternalSystemReference"].ToString(),
                                            dataRow["UserStatus"] == DBNull.Value ? string.Empty : dataRow["UserStatus"].ToString(),
                                            dataRow["PasswordChangeDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["PasswordChangeDate"]),
                                            dataRow["InvalidAccessAttempts"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["InvalidAccessAttempts"]),
                                            dataRow["LockedOutTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["LockedOutTime"]),
                                            dataRow["PasswordChangeOnNextLogin"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["PasswordChangeOnNextLogin"]),
                                            dataRow["OptOutWhatsApp"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["OptOutWhatsApp"]),
                                            dataRow["NickName"] == DBNull.Value ? string.Empty : dataRow["NickName"].ToString()
                                            );
            log.LogMethodExit(profileDTO);
            return profileDTO;
        }

        /// <summary>
        /// Gets the Profile data of passed Profile Id
        /// </summary>
        /// <param name="profileId">integer type parameter</param>
        /// <returns>Returns ProfileDTO</returns>
        public ProfileDTO GetProfileDTO(int profileId)
        {
            log.LogMethodEntry(profileId);
            ProfileDTO returnValue = null;
            string query = PROFILE_SELECT_QUERY + @" WHERE Profile.Id = @Id";
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { dataAccessHandler.GetSQLParameter("@Id", profileId, true), dataAccessHandler.GetSQLParameter("@PassPhrase", passPhrase) }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetProfileDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Gets the ProfileDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ProfileDTO matching the search criteria</returns>
        public List<ProfileDTO> GetProfileDTOList(List<KeyValuePair<ProfileDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<ProfileDTO> list = null;
            int count = 0;
            string selectQuery = PROFILE_SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<ProfileDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == ProfileDTO.SearchByParameters.ID ||
                            searchParameter.Key == ProfileDTO.SearchByParameters.PROFILE_TYPE_ID ||
                            searchParameter.Key == ProfileDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ProfileDTO.SearchByParameters.PROFILE_TYPE) //enum
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'NONE') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == ProfileDTO.SearchByParameters.ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == ProfileDTO.SearchByParameters.OPT_IN_PROMOTIONS)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == ProfileDTO.SearchByParameters.POLICY_TERMS_ACCEPTED)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == ProfileDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ProfileDTO.SearchByParameters.IS_ACTIVE || searchParameter.Key == ProfileDTO.SearchByParameters.OPT_OUT_WHATSAPP)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == ProfileDTO.SearchByParameters.FIRST_NAME || searchParameter.Key == ProfileDTO.SearchByParameters.LAST_NAME)
                        {
                            query.Append(joiner + "CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase," + DBSearchParameters[searchParameter.Key] + "))" + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), (searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ProfileDTO.SearchByParameters.LAST_UPDATE_FROM_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == ProfileDTO.SearchByParameters.LAST_UPDATE_TO_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) < " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        count++;
                    }
                    else
                    {
                        log.LogMethodExit(null, "throwing exception");
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                selectQuery = selectQuery + query;
            }
            parameters.Add(dataAccessHandler.GetSQLParameter("@PassPhrase", passPhrase));
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<ProfileDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ProfileDTO profileDTO = GetProfileDTO(dataRow);
                    list.Add(profileDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// Gets the CustomDataSetDTO List for CustomDataSet Id List
        /// </summary>
        /// <param name="customDataSetIdList">integer list parameter</param>
        /// <returns>Returns List of CustomDataSetDTO</returns>
        public List<ProfileDTO> GetProfileDTOList(List<int> profileIdList)
        {
            log.LogMethodEntry(profileIdList);
            List<ProfileDTO> list = null;
            string query = PROFILE_SELECT_QUERY + @" INNER JOIN @ProfileIdList List ON Profile.Id = List.Id";
            DataTable table = dataAccessHandler.BatchSelect(query, "@ProfileIdList", profileIdList, new []{ dataAccessHandler.GetSQLParameter("@PassPhrase", passPhrase)}, sqlTransaction);
            if (table.Rows.Count > 0)
            {
                list = new List<ProfileDTO>();
                foreach (DataRow dataRow in table.Rows)
                {
                    ProfileDTO profileDTO = GetProfileDTO(dataRow);
                    list.Add(profileDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// Returns the password of the profile
        /// </summary>
        /// <param name="profileId"></param>
        /// <returns></returns>
        public string GetPassword(int profileId)
        {
            log.LogMethodEntry(profileId);
            string result = string.Empty;
            string query = @"SELECT Password 
                            FROM Profile 
                            WHERE Id = @Id";
            object queryResult = dataAccessHandler.executeScalar(query, new[] { dataAccessHandler.GetSQLParameter("@Id", profileId) }, sqlTransaction);
            if (queryResult != null && queryResult != DBNull.Value)
            {
                result = Convert.ToString(queryResult);
            }
            log.LogMethodExit("result");
            return result;
        }

        /// <summary>
        /// Updates the password for the given profile
        /// </summary>
        /// <param name="profileId"></param>
        /// <param name="password"></param>
        /// <param name="loginId"></param>
        /// <returns></returns>
        public int SetPassword(int profileId, string password, string loginId)
        {
            log.LogMethodEntry(profileId, "password", loginId);
            string query = @"UPDATE Profile 
                             SET Password=@Password,
                                 PasswordChangeOnNextLogin = 0,
                                 LastUpdatedBy=@LastUpdatedBy,
                                 LastUpdateDate=GETDATE()
                             WHERE Id = @Id ";
            SqlParameter[] parameters = new SqlParameter[]
            {
                dataAccessHandler.GetSQLParameter("@Id", profileId, true),
                dataAccessHandler.GetSQLParameter("@Password", password),
                dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId)
            };
            int noOfRowsUpdated = dataAccessHandler.executeUpdateQuery(query, parameters, sqlTransaction);
            log.LogMethodExit(noOfRowsUpdated);
            return noOfRowsUpdated;
        }

    }
}
