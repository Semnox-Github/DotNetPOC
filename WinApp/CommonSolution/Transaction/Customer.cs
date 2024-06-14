using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
//using Semnox.Parafait.EncryptionUtils;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Transaction
{
    public class Customer1
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public int customer_id = -1;
        public string Title;
        public string first_name;
        public string middle_name;
        public string last_name;
        public string address1;
        public string address2;
        public string address3;
        public string city;
        public string state;
        public string country;
        public string pin;
        public string email;
        public DateTime birth_date;
        public DateTime anniversary;
        public char gender = 'N';
        public string contact_phone1;
        public string contact_phone2;
        public string notes;
        public int CustomDataSetId = -1;
        public string Company = "";
        public string Designation = "";
        public string PhotoFileName = "";
        public string IDProofFileName = "";
        public string UniqueID = "";
        public string Username;
        public string FBUserId;
        public string FBAccessToken;
        public string TWAccessToken;
        public string TWAccessSecret;
        public string Password;
        public string TaxCode;
        public char RightHanded = 'N';
        public char TeamUser = 'N';

        public char Verified = 'N';
        public object VerificationRecordId = -1;

        public string ExternalSystemRef;

        // Added below two fields on 08-Sep-2015 for Web Service requirement
        public DateTime last_Updated_Date;
        public string last_Updated_User;

        //added on 20-Feb-2017 to save customerDTO coupons details
        public DataTable CustomerCuponsDT { get; set; }
        public string WeChatAccessToken;
        //end

        public Utilities Utilities;
        public Customer1(Utilities ParafaitUtilities)
        {
            log.LogMethodEntry(ParafaitUtilities);

            Utilities = ParafaitUtilities;

            log.LogMethodExit(null);
        }

        public void getDetails(int in_customer_id, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(in_customer_id, SQLTrx);

            string encryptedPassPhrase = Utilities.getParafaitDefaults("CUSTOMER_ENCRYPTION_PASS_PHRASE");
            string passPhrase = encryptedPassPhrase;

            string CommandText = @"select customer_id,customer_name,city,state,country,pin,gender,notes,Title,company,middle_name,last_name,last_updated_date,last_updated_user,
                                    Designation,PhotoFileName,ExternalSystemRef,CustomDataSetId,RightHanded,TeamUser,Verified,Username,Password,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE(@PassphraseEnteredByUser, address1Encrypted)) as address1,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE(@PassphraseEnteredByUser, address2Encrypted)) as address2,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE(@PassphraseEnteredByUser, address3Encrypted)) as address3,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE(@PassphraseEnteredByUser, emailEncrypted)) as email,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE(@PassphraseEnteredByUser, WeChatAccessTokenEncrypted)) as WeChatAccessToken,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE(@PassphraseEnteredByUser, IDProofFileNameEncrypted)) as IDProofFileName,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE (@PassphraseEnteredByUser,birthDateEncrypted),121) as birth_date,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE (@PassphraseEnteredByUser,anniversaryEncrypted),121) as anniversary,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE (@PassphraseEnteredByUser,contactPhone1Encrypted)) as contact_phone1 ,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE (@PassphraseEnteredByUser,contactPhone2Encrypted)) as contact_phone2,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE (@PassphraseEnteredByUser,FBUserIdEncrypted)) as FBUserId,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE (@PassphraseEnteredByUser,FBAccessTokenEncrypted)) as FBAccessToken,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE (@PassphraseEnteredByUser,TWAccessSecretEncrypted)) as TWAccessSecret,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE (@PassphraseEnteredByUser,TWAccessTokenEncrypted)) as TWAccessToken,
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE (@PassphraseEnteredByUser,TaxCodeEncrypted)) as TaxCode, 
                                    convert(nvarchar(max), DECRYPTBYPASSPHRASE (@PassphraseEnteredByUser,UniqueIDEncrypted)) as Unique_ID
                                    from customers  where customer_id = @customer_id";
	    DataTable DT = Utilities.executeDataTable(CommandText, SQLTrx, new SqlParameter("@customer_id", in_customer_id), new SqlParameter("@PassphraseEnteredByUser", passPhrase));

            log.LogVariableState("@customer_id", in_customer_id);

            if (DT.Rows.Count > 0)
            {
                customer_id = Convert.ToInt32(DT.Rows[0]["customer_id"]);
                Title = DT.Rows[0]["Title"].ToString();
                first_name = DT.Rows[0]["customer_name"].ToString();
                middle_name = DT.Rows[0]["middle_name"].ToString();
                last_name = DT.Rows[0]["last_name"].ToString();
                address1 = DT.Rows[0]["address1"].ToString();
                address2 = DT.Rows[0]["address2"].ToString();
                address3 = DT.Rows[0]["address3"].ToString();
                city = DT.Rows[0]["city"].ToString();
                state = DT.Rows[0]["state"].ToString();
                country = DT.Rows[0]["country"].ToString();
                pin = DT.Rows[0]["pin"].ToString();
                email = DT.Rows[0]["email"].ToString();
                notes = DT.Rows[0]["notes"].ToString();
                Company = DT.Rows[0]["Company"].ToString();
                Designation = DT.Rows[0]["Designation"].ToString();
                PhotoFileName = DT.Rows[0]["PhotoFileName"].ToString();
                IDProofFileName = DT.Rows[0]["IDProofFileName"].ToString();
                ExternalSystemRef = DT.Rows[0]["ExternalSystemRef"].ToString();
                WeChatAccessToken = DT.Rows[0]["WeChatAccessToken"].ToString(); //Added for merkle integartion

                if(DT.Rows[0]["TaxCode"] != DBNull.Value)
                {
                    TaxCode = Convert.ToString(DT.Rows[0]["TaxCode"]);
                }
                if (DT.Rows[0]["birth_date"] != DBNull.Value)
                   birth_date = Convert.ToDateTime(DT.Rows[0]["birth_date"]);

                if (DT.Rows[0]["anniversary"] != DBNull.Value)
                    anniversary = Convert.ToDateTime(DT.Rows[0]["anniversary"]);

                gender = DT.Rows[0]["gender"] == DBNull.Value ? 'N': DT.Rows[0]["gender"].ToString()[0];

                contact_phone1 = DT.Rows[0]["contact_phone1"].ToString();
                contact_phone2 = DT.Rows[0]["contact_phone2"].ToString();

                if (DT.Rows[0]["CustomDataSetId"] != DBNull.Value)
                    CustomDataSetId = Convert.ToInt32(DT.Rows[0]["CustomDataSetId"]);

                UniqueID = DT.Rows[0]["Unique_ID"].ToString();
                Username = DT.Rows[0]["Username"].ToString();
                Password = DT.Rows[0]["Password"].ToString();
                FBUserId = DT.Rows[0]["FBUserId"].ToString();
                FBAccessToken = DT.Rows[0]["FBAccessToken"].ToString();
                TWAccessToken = DT.Rows[0]["TWAccessToken"].ToString();
                TWAccessSecret = DT.Rows[0]["TWAccessSecret"].ToString();
                RightHanded = DT.Rows[0]["RightHanded"].ToString() == "N" ? 'N' : 'Y';
                TeamUser = DT.Rows[0]["TeamUser"].ToString() == "Y" ? 'Y' : 'N';

                Verified = DT.Rows[0]["Verified"].ToString() == "Y" ? 'Y' : 'N';

                //added below two lines on 08-Sep-2015 for web service requirement
                last_Updated_Date = (DateTime)DT.Rows[0]["last_updated_date"];
                last_Updated_User = DT.Rows[0]["last_updated_user"].ToString();
            }

            log.LogMethodExit(null);
        }

        public void saveDetails(SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(SQLTrx);

            SqlCommand cmd = Utilities.getCommand(SQLTrx);

            cmd.CommandText = "select 1 from customers where customer_id = @customer_id";
            cmd.Parameters.AddWithValue("@customer_id", customer_id);

            log.LogVariableState("@customer_id", customer_id);

            cmd.Parameters.AddWithValue("@first_name", first_name);

            if (string.IsNullOrEmpty(middle_name))
            {
                cmd.Parameters.AddWithValue("@middle_name", DBNull.Value);
                log.LogVariableState("@middle_name", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@middle_name", middle_name);
                log.LogVariableState("@middle_name", middle_name);
            }

            if (string.IsNullOrEmpty(Title))
            {
                cmd.Parameters.AddWithValue("@Title", DBNull.Value);
                log.LogVariableState("@Title", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@Title", Title);
                log.LogVariableState("@Title", Title);
            }

            if (string.IsNullOrEmpty(TaxCode))
            {
                cmd.Parameters.AddWithValue("@TaxCode", DBNull.Value);
                log.LogVariableState("@TaxCode", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@TaxCode", TaxCode);
                log.LogVariableState("@TaxCode", TaxCode);
            }

            if (string.IsNullOrEmpty(last_name))
            {
                cmd.Parameters.AddWithValue("@last_name", DBNull.Value);
                log.LogVariableState("@last_name", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@last_name", last_name);
                log.LogVariableState("@last_name", last_name);
            }

            if (string.IsNullOrEmpty(address1))
            {
                cmd.Parameters.AddWithValue("@address1", DBNull.Value);
                log.LogVariableState("@address1", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@address1", address1);
                log.LogVariableState("@address1", address1);
            }

            if (string.IsNullOrEmpty(address2))
            {
                cmd.Parameters.AddWithValue("@address2", DBNull.Value);
                log.LogVariableState("@address2", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@address2", address2);
                log.LogVariableState("@address2", address2);
            }

            if (string.IsNullOrEmpty(address3))
            {
                cmd.Parameters.AddWithValue("@address3", DBNull.Value);
                log.LogVariableState("@address3", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@address3", address3);
                log.LogVariableState("@address3", address3);
            }

            if (string.IsNullOrEmpty(city))
            {
                cmd.Parameters.AddWithValue("@city", DBNull.Value);
                log.LogVariableState("@city", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@city", city);
                log.LogVariableState("@city", city);
            }

            if (string.IsNullOrEmpty(state))
            {
                cmd.Parameters.AddWithValue("@state", DBNull.Value);
                log.LogVariableState("@state", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@state", state);
                log.LogVariableState("@state", state);
            }

            if (string.IsNullOrEmpty(country))
            {
                cmd.Parameters.AddWithValue("@country", DBNull.Value);
                log.LogVariableState("@country", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@country", country);
                log.LogVariableState("@country", country);
            }

            if (string.IsNullOrEmpty(pin))
            {
                cmd.Parameters.AddWithValue("@pin", DBNull.Value);
                log.LogVariableState("@pin", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@pin", pin);
                log.LogVariableState("@pin", pin);
            }

            if (string.IsNullOrEmpty(email))
            {
                cmd.Parameters.AddWithValue("@email", DBNull.Value);
                log.LogVariableState("@email", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@email", email);
                log.LogVariableState("@email", email);
            }

            if (birth_date != DateTime.MinValue)
            {
                cmd.Parameters.AddWithValue("@birth_date", birth_date);
                log.LogVariableState("@birth_date", birth_date);
            }
            else
            {
                cmd.Parameters.AddWithValue("@birth_date", DBNull.Value);
                log.LogVariableState("@birth_date", DBNull.Value);
            }

            if (anniversary != DateTime.MinValue)
            {
                cmd.Parameters.AddWithValue("@anniversary", anniversary);
                log.LogVariableState("@anniversary", anniversary);
            }
            else
            {
                cmd.Parameters.AddWithValue("@anniversary", DBNull.Value);
                log.LogVariableState("@anniversary", DBNull.Value);
            }

            cmd.Parameters.AddWithValue("@gender", gender);
            log.LogVariableState("@gender", gender);


            if (string.IsNullOrEmpty(notes))
            {
                cmd.Parameters.AddWithValue("@notes", DBNull.Value);
                log.LogVariableState("@notes", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@notes", notes);
                log.LogVariableState("@notes", notes);
            }

            if (string.IsNullOrEmpty(contact_phone1))
            {
                cmd.Parameters.AddWithValue("@contact_phone1", DBNull.Value);
                log.LogVariableState("@contact_phone1", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@contact_phone1", contact_phone1);
                log.LogVariableState("@contact_phone1", contact_phone1);
            }

            if (string.IsNullOrEmpty(contact_phone2))
            {
                cmd.Parameters.AddWithValue("@contact_phone2", DBNull.Value);
                log.LogVariableState("@contact_phone2", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@contact_phone2", contact_phone2);
                log.LogVariableState("@contact_phone2", contact_phone2);
            }

            if (string.IsNullOrEmpty(Company))
            {
                cmd.Parameters.AddWithValue("@Company", DBNull.Value);
                log.LogVariableState("@Company", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@Company", Company);
                log.LogVariableState("@Company", Company);
            }

            if (string.IsNullOrEmpty(Designation))
            {
                cmd.Parameters.AddWithValue("@Designation", DBNull.Value);
                log.LogVariableState("@Designation", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@Designation", Designation);
                log.LogVariableState("@Designation", Designation);
            }

            if (string.IsNullOrEmpty(UniqueID))
            {
                cmd.Parameters.AddWithValue("@UniqueID", DBNull.Value);
                log.LogVariableState("@UniqueID", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@UniqueID", UniqueID);
                log.LogVariableState("@UniqueID", UniqueID);
            }

            if (string.IsNullOrEmpty(PhotoFileName))
            {
                cmd.Parameters.AddWithValue("@PhotoFileName", DBNull.Value);
                log.LogVariableState("@PhotoFileName", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@PhotoFileName", PhotoFileName);
                log.LogVariableState("@PhotoFileName", PhotoFileName);
            }

            if (string.IsNullOrEmpty(IDProofFileName))
            {
                cmd.Parameters.AddWithValue("@IDProofFileName", DBNull.Value);
                log.LogVariableState("@IDProofFileName", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@IDProofFileName", IDProofFileName);
                log.LogVariableState("@IDProofFileName", IDProofFileName);
            }

            if (string.IsNullOrEmpty(Username))
            {
                cmd.Parameters.AddWithValue("@Username", DBNull.Value);
                log.LogVariableState("@Username", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@Username", Username);
                log.LogVariableState("@Username", Username);
            }

            if (string.IsNullOrEmpty(FBAccessToken))
            {
                cmd.Parameters.AddWithValue("@FBAccessToken", DBNull.Value);
                log.LogVariableState("@FBAccessToken", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@FBAccessToken", FBAccessToken);
                log.LogVariableState("@FBAccessToken", FBAccessToken);
            }

            if (string.IsNullOrEmpty(FBUserId))
            {
                cmd.Parameters.AddWithValue("@FBUserId", DBNull.Value);
                log.LogVariableState("@FBUserId", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@FBUserId", FBUserId);
                log.LogVariableState("@FBUserId", FBUserId);
            }

            if (string.IsNullOrEmpty(TWAccessSecret))
            {
                cmd.Parameters.AddWithValue("@TWAccessSecret", DBNull.Value);
                log.LogVariableState("@TWAccessSecret", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@TWAccessSecret", TWAccessSecret);
                log.LogVariableState("@TWAccessSecret", TWAccessSecret);
            }

            if (string.IsNullOrEmpty(TWAccessToken))
            {
                cmd.Parameters.AddWithValue("@TWAccessToken", DBNull.Value);
                log.LogVariableState("@TWAccessToken", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@TWAccessToken", TWAccessToken);
                log.LogVariableState("@TWAccessToken", TWAccessToken);
            }

            cmd.Parameters.AddWithValue("@RightHanded", RightHanded);
            cmd.Parameters.AddWithValue("@TeamUser", TeamUser);
            cmd.Parameters.AddWithValue("@Verified", Verified);

            log.LogVariableState("@RightHanded", RightHanded);
            log.LogVariableState("@TeamUser", TeamUser);
            log.LogVariableState("@Verified", Verified);

            cmd.Parameters.AddWithValue("@user", Utilities.ParafaitEnv.LoginID);

            log.LogVariableState("@user", Utilities.ParafaitEnv.LoginID);

            if (string.IsNullOrEmpty(Password))
            {
                cmd.Parameters.AddWithValue("@Password", DBNull.Value);
                log.LogVariableState("@Password", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@Password", Password);
                log.LogVariableState("@Password", "Password");
            }

            if (string.IsNullOrEmpty(ExternalSystemRef))
            {
                cmd.Parameters.AddWithValue("@ExternalSystemRef", DBNull.Value);
                log.LogVariableState("@ExternalSystemRef", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ExternalSystemRef", ExternalSystemRef);
                log.LogVariableState("@ExternalSystemRef", ExternalSystemRef);
            }

            if (CustomDataSetId != -1)
            {
                cmd.Parameters.AddWithValue("@CustomDataSetId", CustomDataSetId);
                log.LogVariableState("@CustomDataSetId", CustomDataSetId);
            }
            else
            {
                cmd.Parameters.AddWithValue("@CustomDataSetId", DBNull.Value);
                log.LogVariableState("@CustomDataSetId", DBNull.Value);
            }

            if (string.IsNullOrEmpty(WeChatAccessToken))
            {
                cmd.Parameters.AddWithValue("@weChatAccessToken", DBNull.Value);
                log.LogVariableState("@weChatAccessToken", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@weChatAccessToken", WeChatAccessToken);
                log.LogVariableState("@weChatAccessToken", WeChatAccessToken);
            }
            string encryptedPassPhrase = Utilities.getParafaitDefaults("CUSTOMER_ENCRYPTION_PASS_PHRASE");
            string passPhrase = encryptedPassPhrase;

            if (string.IsNullOrEmpty(passPhrase))
                cmd.Parameters.AddWithValue("@PassphraseEnteredByUser", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@PassphraseEnteredByUser", passPhrase);

            if (cmd.ExecuteScalar() == null) // new customerDTO
            {
                cmd.CommandText = "insert into customers (" +
                                    "customer_name, " +
                                    "Title, " +
                                    "middle_name, " +
                                    "last_name, " +
                                    "address1Encrypted, " +
                                    "address2Encrypted, " +
                                    "address3Encrypted, " +
                                    "city, " +
                                    "state, " +
                                    "country, " +
                                    "pin, " +
                                    "emailEncrypted, " +
                                    "birthDateEncrypted, " +
                                    "anniversaryEncrypted, " +
                                    "gender, " +
                                    "notes, " +
                                    "CustomDataSetId, " +
                                    "contactPhone1Encrypted, " +
                                    "contactPhone2Encrypted, " +
                                    "Company, " +
                                    "Designation, " +
                                    "UniqueIDEncrypted, " +
                                    "PhotoFileName, " +
                                    "IDProofFileNameEncrypted, " +
                                    "Username, " +
                                    "FBUserIdEncrypted, " +
                                    "FBAccessTokenEncrypted, " +
                                    "TWAccessTokenEncrypted, " +
                                    "TWAccessSecretEncrypted, " +
                                    "RightHanded, " +
                                    "TeamUser, " +
                                    "Password, " +
                                    "last_updated_date, " +
                                    "last_updated_user, " +
                                    "Verified, " +
                                    "ExternalSystemRef, " +
                                    "WeChatAccessTokenEncrypted, " +
                                    "TaxCodeEncrypted " +

                          ") values (" +
                                    "@first_name, " +
                                    "@Title, " +
                                    "@middle_name, " +
                                    "@last_name, " +
                                    "EncryptByPassPhrase(@PassphraseEnteredByUser, @address1 ), " +
                                    "EncryptByPassPhrase(@PassphraseEnteredByUser, @address2 ), " +
                                    "EncryptByPassPhrase(@PassphraseEnteredByUser, @address3 ), " +
                                    "@city, " +
                                    "@state, " +
                                    "@country, " +
                                    "@pin, " +
                                    "EncryptByPassPhrase(@PassphraseEnteredByUser, @email ), " +
                                    "EncryptByPassPhrase(@PassphraseEnteredByUser, convert(nvarchar(max), @birth_date,121) ), " +
                                    "EncryptByPassPhrase(@PassphraseEnteredByUser, convert(nvarchar(max), @anniversary,121) ), " +
                                    "@gender, " +
                                    "@notes, " +
                                    "@CustomDataSetId, " +
                                    "EncryptByPassPhrase(@PassphraseEnteredByUser, @contact_phone1 ), " +
                                    "EncryptByPassPhrase(@PassphraseEnteredByUser, @contact_phone2 ), " +
                                    "@Company, " +
                                    "@Designation, " +
                                    "EncryptByPassPhrase(@PassphraseEnteredByUser, @UniqueID), " +
                                    "@PhotoFileName, " +
                                    "EncryptByPassPhrase(@PassphraseEnteredByUser, @IDProofFileName ), " +
                                    "@Username, " +
                                    "EncryptByPassPhrase(@PassphraseEnteredByUser, @FBUserId ), " +
                                    "EncryptByPassPhrase(@PassphraseEnteredByUser, @FBAccessToken ), " +
                                    "EncryptByPassPhrase(@PassphraseEnteredByUser, @TWAccessToken ), " +
                                    "EncryptByPassPhrase(@PassphraseEnteredByUser, @TWAccessSecret ), " +
                                    "@RightHanded, " +
                                    "@TeamUser, " +
                                    "@Password, " +
                                    "getdate(), " +
                                    "@user, " +
                                    "@Verified, " +
                                   "@ExternalSystemRef, " +
                                   "EncryptByPassPhrase(@PassphraseEnteredByUser, @weChatAccessToken ), " +
                                   "EncryptByPassPhrase(@PassphraseEnteredByUser, @TaxCode )"+
                                   ") ; select @@identity;";

                customer_id = Convert.ToInt32(cmd.ExecuteScalar());               
            }
            else
            {
                cmd.CommandText = "update customers set " +
                                    "customer_name = @first_name, " +
                                    "Title = @Title, " +
                                    "middle_name = @middle_name, " +
                                    "last_name = @last_name, " +
                                    "city = @city, " +
                                    "state = @state, " +
                                    "country = @country, " +
                                    "pin = @pin, " +
                                    "gender = @gender, " +
                                    "notes = @notes, " +
                                    "CustomDataSetId = @CustomDataSetId, " +
                                    "Company = @Company, " +
                                    "Designation = @Designation, " +
                                    "PhotoFileName = @PhotoFileName, " +
                                    "IDProofFileName = @IDProofFileName, " +
                                    "Username = @Username, " +
                                    "RightHanded = @RightHanded, " +
                                    "TeamUser = @TeamUser, " +
                                    "Password = @Password, " +
                                    "last_updated_date = getdate(), " +
                                    "last_updated_user = @user, " +
                                    "Verified = @Verified, " +
                                    "ExternalSystemRef = @ExternalSystemRef, " +
                                    "address1Encrypted = EncryptByPassPhrase(@PassphraseEnteredByUser, @address1 ),  " +
                                    "address2Encrypted = EncryptByPassPhrase(@PassphraseEnteredByUser, @address2 ),  " +
                                    "address3Encrypted = EncryptByPassPhrase(@PassphraseEnteredByUser, @address3 ),  " +
                                    "emailEncrypted = EncryptByPassPhrase(@PassphraseEnteredByUser, @email ),  " +
                                    "birthDateEncrypted = EncryptByPassPhrase(@PassphraseEnteredByUser,convert(nvarchar(max), @birth_date,121) ),  " +
                                    "anniversaryEncrypted = EncryptByPassPhrase(@PassphraseEnteredByUser, convert(nvarchar(max), @anniversary,121) ),  " +
                                    "contactPhone1Encrypted = EncryptByPassPhrase(@PassphraseEnteredByUser, @contact_phone1 ),  " +
                                    "contactPhone2Encrypted = EncryptByPassPhrase(@PassphraseEnteredByUser, @contact_phone2 ),  " +
                                    "FBUserIdEncrypted = EncryptByPassPhrase(@PassphraseEnteredByUser, @FBUserId ),  " +
                                    "FBAccessTokenEncrypted = EncryptByPassPhrase(@PassphraseEnteredByUser, @FBAccessToken ),  " +
                                    "TWAccessSecretEncrypted = EncryptByPassPhrase(@PassphraseEnteredByUser, @TWAccessSecret ),  " +
                                    "TWAccessTokenEncrypted = EncryptByPassPhrase(@PassphraseEnteredByUser,@TWAccessToken ),  " +
                                    "WeChatAccessTokenEncrypted = EncryptByPassPhrase(@PassphraseEnteredByUser, @WeChatAccessToken ),  " +
                                    "IDProofFileNameEncrypted = EncryptByPassPhrase(@PassphraseEnteredByUser, @IDProofFileName ), " +
                                    "TaxCodeEncrypted = EncryptByPassPhrase(@PassphraseEnteredByUser, @TaxCode ), " +
                                    "UniqueIDEncrypted = EncryptByPassPhrase(@PassphraseEnteredByUser, @UniqueID) " +
                                    "where customer_id = @customer_id";

                cmd.ExecuteNonQuery();
            }

            if (VerificationRecordId.Equals(-1) == false)
            {
                cmd.CommandText = @"update CustomerVerification 
                                    set customerId = @customerId, LastUpdateDate = getdate(), LastUpdateUser = @user 
                                    where Id = @id";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@customerId", customer_id);
                cmd.Parameters.AddWithValue("@id", VerificationRecordId);
                cmd.Parameters.AddWithValue("@user", Utilities.ParafaitEnv.LoginID);
                cmd.ExecuteNonQuery();

                log.LogVariableState("@customerId", customer_id);
                log.LogVariableState("@id", VerificationRecordId);
                log.LogVariableState("@user", Utilities.ParafaitEnv.LoginID);
            }

            log.LogMethodExit(null);
        }        
    }
}
