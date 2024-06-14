/********************************************************************************************
 * Project Name -KeyManagement Class
 * Description  -Business Class for key Management
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.50.0     14-Dec-2018    Guru S A         Application security changes
 *2.110.0    20-Dec-2020    Nitin Pai        Dashboard License Validation has moved from Parafait Central to Client HQ.
 *                                           Added New method to validate the number of licenses also
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows.Forms;
using Semnox.Parafait;
//using Semnox.Parafait.EncryptionUtils;
using Semnox.Core.Utilities;

namespace Semnox.Core.Utilities
{
    public class KeyManagement
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string[] licensedFeatures = { "Loyalty Management",
                                             "Internet Reporting",
                                             "Bio-Metric POS Login",
                                             "Inventory Management",
                                             "Attraction Ticketing",
                                             "CheckIn - CheckOut",
                                             "Gateway",
                                             "Analytics Dashboard",
                                             "Sales Dashboard",
                                            };

        DBUtils Utilities;
        ParafaitEnv env;
        MessageUtils msgUtils;
        private int licensedNumberOfPOSMachines;
        private string licensedNumberOfPOSMachinesKey;
        private string licensedFeaturesKey;
        private string authKey;
        private string licenseKey;

        public string LicensedNumberOfPOSMachinesKey
        { get { return licensedNumberOfPOSMachinesKey; } }
        public string LicensedFeaturesKey
        { get { return licensedFeaturesKey; } }
        public string AuthKey { get { return authKey; } }
        public string LicenseKey { get { return licenseKey; } }
        public int LicensedNumberOfPOSMachines { get { return licensedNumberOfPOSMachines; } }

        public KeyManagement(DBUtils dbUtilities, ParafaitEnv env)
        {
            log.LogMethodEntry();
            this.Utilities = dbUtilities;
            this.env = env;
            this.msgUtils = new MessageUtils(this.Utilities, env);
            this.licensedNumberOfPOSMachines = 0;
            log.LogMethodExit();
        }

        public string GenerateKey(string SiteKey, DateTime ExpiryDate)
        {
            log.LogMethodEntry(ExpiryDate);
            string key;
            int hash = 0;

            byte[] array;
            array = new byte[20];

            key = SiteKey.Trim();

            for (int i = 0; i < key.Length; i++)
            {
                array[i] = Convert.ToByte(key[i]);
            }

            for (int i = key.Length; i < 12; i++) // right pad 0123... upto 12 bytes
            {
                array[i] = Convert.ToByte('0' + i - key.Length);
            }

            for (int i = 0; i < key.Length; i++) // swap a-z positions
            {
                array[i] = Convert.ToByte(90 - (array[i] - 65));
            }

            for (int i = 0; i < 12; i++) // get a hash number for encoding date
            {
                hash = hash + array[i];
            }
            hash = hash % 10;

            if (ExpiryDate == DateTime.MaxValue)
                key = "000000";
            else
                key = ExpiryDate.ToString("ddMMyy");

            for (int i = 0; i < key.Length; i++) // add date
            {
                array[12 + i] = Convert.ToByte(key[i] + hash + 17);
            }

            string LicenseKey = "";

            for (int i = 0; i < 12; i++)
            {
                if (i % 2 == 0)   // even positions contain bytes from the end in reverse order. 
                                  // i.e, 0, 2, 4, 6, 8, 10th bytes are 17, 16, 15, 14, 13, 12 respectively
                    LicenseKey += Convert.ToChar(array[17 - i / 2]);
                else // odd ones are in place
                    LicenseKey += Convert.ToChar(array[i]);
            }

            for (int i = 0; i < 11; i += 2) // fill the last 12-17 positions with 0, 2, 4, 6, 8, 10
            {
                LicenseKey += Convert.ToChar(array[i]);
            }

            log.LogMethodExit();
            return (LicenseKey);
        }

        public void DecodeKey(string LicenseKey, ref string SiteKey, ref DateTime ExpiryDate)
        {
            log.LogMethodEntry();
            byte[] array;
            array = new byte[20];
            int hash = 0;

            try
            {
                for (int i = 12; i < 18; i += 1) // fill the last 17-12 positions with 0, 2, 4, 6, 8, 10 respectively
                {
                    array[i] = Convert.ToByte(LicenseKey[(17 - i) * 2]);
                }

                for (int i = 0; i < 12; i++)
                {
                    if (i % 2 == 0)   // even positions filled with 12-17 positions 
                        array[i] = Convert.ToByte(LicenseKey[i / 2 + 12]);
                    else // odd ones are in place
                        array[i] = Convert.ToByte(LicenseKey[i]);
                }

                for (int i = 0; i < 12; i++) // get a hash number for encoding date
                {
                    hash = hash + array[i];
                }
                hash = hash % 10;

                int length = 0;

                for (int i = 0; i < 12; i++)
                {
                    if (array[i] >= 65 && array[i] <= 90)
                        length++;
                }

                for (int i = 0; i < length; i++) // swap a-z positions
                {
                    array[i] = Convert.ToByte((90 - array[i]) + 65);
                }

                string date = "";
                for (int i = 12; i < 18; i++)
                {
                    array[i] = Convert.ToByte(array[i] - hash - 17);
                    date += Convert.ToChar(array[i]);
                }

                if (date == "000000")
                    ExpiryDate = DateTime.MaxValue;
                else
                    ExpiryDate = DateTime.ParseExact(date.Substring(2, 2) + "-" + date.Substring(0, 2) + "-" + DateTime.Now.Year.ToString().Substring(0, 2) + date.Substring(4, 2), "MM-dd-yyyy", null);

                SiteKey = "";

                for (int i = 0; i < length; i++)
                {
                    SiteKey += Convert.ToChar(array[i]);
                }
            }
            catch (Exception ex)
            {
                SiteKey = "";
                ExpiryDate = DateTime.MinValue;
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        public void ReadKeysFromDB(ref string SiteKey, ref string LicenseKey)
        {
            log.LogMethodEntry();
            int siteId;
            if (env.IsCorporate)
                siteId = env.SiteId;
            else
                siteId = -1;

            log.LogVariableState("siteId", siteId);
            ReadKeysFromDB(siteId, ref SiteKey, ref LicenseKey);
            log.LogMethodExit();
        }

        public void ReadKeysFromDB(int siteId, ref string SiteKey, ref string LicenseKey)
        {
            log.LogMethodEntry(siteId);
            using (SqlConnection cnn = Utilities.createConnection())
            {
                using (SqlCommand cmd = new SqlCommand("", cnn))
                {
                    cmd.CommandText = "select * from ProductKey where site_id = @site_id or @site_id = -1 order by site_id";
                    cmd.Parameters.AddWithValue("@site_id", siteId);

                    string siteKey = "", licenseKey = "";

                    SqlDataReader sqr;
                    sqr = cmd.ExecuteReader();

                    if (sqr.HasRows)
                    {
                        byte[] buffer;
                        sqr.Read();

                        buffer = (byte[])sqr["SiteKey"];
                        for (int i = 0; i < buffer.Length; i++)
                        {
                            if (buffer[i] != 0)
                                siteKey += (char)buffer[i];
                        }
                        SiteKey = siteKey;

                        buffer = (byte[])sqr["LicenseKey"];
                        for (int i = 0; i < buffer.Length; i++)
                        {
                            if (buffer[i] != 0)
                                licenseKey += (char)buffer[i];
                        }
                        LicenseKey = licenseKey;
                    }
                    else
                    {
                        SiteKey = "";
                        LicenseKey = "";
                    }
                    sqr.Close();
                    sqr.Dispose();
                    cmd.Dispose();
                }
            }
            log.LogMethodExit();
        }

        public bool FeatureValid(string featureName)
        {
            log.LogMethodEntry(featureName);
            int siteId;
            if (env.IsCorporate)
                siteId = env.SiteId;
            else
                siteId = -1;

            log.LogVariableState("siteId", siteId);
            bool retVal = FeatureValid(siteId, featureName);
            log.LogMethodExit(retVal);
            return retVal;
        }

        public bool FeatureValid(string featureName, int numberOfLicenses)
        {
            log.LogMethodEntry(featureName);
            int siteId;
            if (env.IsCorporate)
                siteId = env.SiteId;
            else
                siteId = -1;

            log.LogVariableState("siteId", siteId);
            bool retVal = FeatureValid(siteId, featureName, numberOfLicenses);
            log.LogMethodExit(retVal);
            return retVal;
        }

        public bool IsLicenceCountTypeFeature(string featureName)
        {
            log.LogMethodEntry(featureName);
            if (String.IsNullOrWhiteSpace(featureName) == false
               && (featureName == "Analytics Dashboard" || featureName == "Sales Dashboard"))
            {
                return true;
            }
            log.LogMethodExit();
            return false;
        }

        public int GetLicenceCount(int siteId, string featureName)
        {
            log.LogMethodEntry(featureName);
            int result = 0;
            string siteKey = "";
            string dummy = "";
            string featureKey = "";

            ReadKeysFromDB(siteId, ref siteKey, ref dummy);
            if (siteKey == "")
            {
                log.LogMethodExit("siteKey is empty");
                result = 0;
            }
            if (String.IsNullOrWhiteSpace(featureName) == false
                 && (featureName == "Analytics Dashboard" || featureName == "Sales Dashboard"))
            {
                using (SqlConnection cnn = Utilities.createConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("", cnn))
                    {
                        cmd.CommandText = "select FeatureKey from productKey where site_id = @site_id or @site_id = -1 order by site_id";
                        cmd.Parameters.AddWithValue("@site_id", siteId);

                        SqlDataReader sqr;
                        sqr = cmd.ExecuteReader();

                        if (sqr.HasRows)
                        {
                            byte[] buffer = new byte[2000];
                            sqr.Read();

                            try
                            {
                                buffer = (byte[])sqr["FeatureKey"];
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                            }
                            for (int i = 0; i < buffer.Length; i++)
                            {
                                if (buffer[i] != 0)
                                    featureKey += (char)buffer[i];
                            }

                            int start = featureKey.IndexOf(siteKey + featureName) + (siteKey + featureName).Length;
                            int stop = featureKey.IndexOf(siteKey, start);
                            result = Convert.ToInt32(featureKey.Substring(start, (stop == -1 ? featureKey.Length : stop) - start));
                        }
                        sqr.Close();
                        sqr.Dispose();
                        cmd.Dispose();
                        log.LogMethodExit(result);
                        return result;
                    }
                }
            }
            log.LogMethodExit();
            return result;
        }

        public bool FeatureValid(int siteId, string featureName, int numberOfLicenses = -1)
        {
            log.LogMethodEntry(siteId, featureName);
            string siteKey = "";
            string dummy = "";
            string featureKey = "";
            bool retValue = false;

            ReadKeysFromDB(siteId, ref siteKey, ref dummy);
            if (siteKey == "")
            {
                log.LogMethodExit("siteKey is empty");
                return false;
            }

            bool found = false;
            for (int i = 0; i < licensedFeatures.Length; i++)
            {
                if (licensedFeatures[i] == featureName)
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                log.LogMethodExit(true);
                return true;
            }

            using (SqlConnection cnn = Utilities.createConnection())
            {
                using (SqlCommand cmd = new SqlCommand("", cnn))
                {
                    cmd.CommandText = "select FeatureKey from productKey where site_id = @site_id or @site_id = -1 order by site_id";
                    cmd.Parameters.AddWithValue("@site_id", siteId);

                    SqlDataReader sqr;
                    sqr = cmd.ExecuteReader();

                    if (sqr.HasRows)
                    {
                        byte[] buffer = new byte[2000];
                        sqr.Read();
                        try
                        {
                            buffer = (byte[])sqr["FeatureKey"];
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            retValue = false;
                        }
                        for (int i = 0; i < buffer.Length; i++)
                        {
                            if (buffer[i] != 0)
                                featureKey += (char)buffer[i];
                        }
                        if (string.IsNullOrWhiteSpace(featureKey) == false)
                        {
                            if (featureKey.Contains(siteKey + featureName + "Y"))
                            {
                                retValue = true;
                            }
                            else if (featureKey.Contains(siteKey + featureName + "N"))
                            {
                                retValue = false;
                            }
                            else
                            {
                                int start = featureKey.IndexOf(siteKey + featureName) + (siteKey + featureName).Length;
                                int stop = featureKey.IndexOf(siteKey, start);
                                try
                                {
                                    int licenseCount = Convert.ToInt32(featureKey.Substring(start, (stop == -1 ? featureKey.Length : stop) - start));
                                    if (licenseCount > 0)
                                        retValue = true;
                                }
                                catch(Exception ex)
                                {
                                    log.Error("Licence feature not found in the Key");
                                    retValue = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        retValue = false;
                    }

                    sqr.Close();
                    sqr.Dispose();
                    cmd.Dispose();
                    log.LogMethodExit(retValue);
                    return retValue;
                }
            }
        }

        public bool validateLicense(ref string message)
        {
            log.LogMethodEntry();
            string DBsiteKey = "", DBlicenseKey = "";
            string DecodeSiteKey = "";
            DateTime expiryDate;

            expiryDate = DateTime.MinValue;

            ReadKeysFromDB(ref DBsiteKey, ref DBlicenseKey);

            if (DBsiteKey != "")
            {
                DBlicenseKey = Encryption.Decrypt(DBlicenseKey);
                KeyManagement km = new KeyManagement(Utilities, env);
                km.DecodeKey(DBlicenseKey, ref DecodeSiteKey, ref expiryDate);

                if (DecodeSiteKey != DBsiteKey)
                {
                    message = "Invalid Site Key in License Key";
                    log.LogMethodExit("DecodeSiteKey != DBsiteKey");
                    return false;
                }

                if (DBsiteKey == "SEMNOX" && expiryDate == DateTime.MaxValue)
                {
                    message = msgUtils.getMessage(319);
                    log.LogMethodExit("SEMNOX key");
                    return false;
                }

                DateTime serverDate, maxTrxDate;

                using (SqlConnection cnn = Utilities.createConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("", cnn))
                    {
                        cmd.CommandText = "select getdate()";
                        serverDate = Convert.ToDateTime(cmd.ExecuteScalar());
                        cmd.CommandText = "select isnull(max(LastUpdateTime), getdate()) from trx_header";
                        maxTrxDate = Convert.ToDateTime(cmd.ExecuteScalar());

                        if ((expiryDate < serverDate.Date) || (expiryDate < maxTrxDate && maxTrxDate > serverDate)) // check for max trx date as well in case server time is moved back after key expiry
                        {
                            message = msgUtils.getMessage(320, expiryDate.ToString("dd-MMM-yyyy"));
                            log.LogMethodExit("Expiry date");
                            return false;
                        }
                        else
                        {
                            if (expiryDate == serverDate.Date)
                            {
                                message = msgUtils.getMessage(321);
                            }
                            else if (expiryDate < serverDate.Date.AddDays(14))
                            {
                                message = msgUtils.getMessage(322, expiryDate.ToString("dd-MMM-yyyy"));
                            }
                        }
                        log.LogMethodExit(true);
                        return true;
                    }
                }
            }
            else
            {
                message = msgUtils.getMessage(323);
                log.LogMethodExit(false);
                return false;
            }
        }

        private void GetNoOfLicensedPOSMachines()
        {
            log.LogMethodEntry();
            int siteId;
            if (env.IsCorporate)
                siteId = env.SiteId;
            else
                siteId = -1;
            string siteKey = "";
            string dummy = "";
            log.LogVariableState("siteId", siteId);
            ReadKeysFromDB(siteId, ref siteKey, ref dummy);
            if (siteKey == "")
            {
                licensedNumberOfPOSMachines = 0;
                log.LogMethodExit("siteKey == ''");
                return;
            }
            try
            {
                var noOfPOSMachinesLicensedCode = Utilities.executeScalar("select NoOfPOSMachinesLicensed from productKey where (site_id = @site_id or @site_id = -1)",
                                                                           new SqlParameter[] { new SqlParameter("@site_id", siteId) });
                if (noOfPOSMachinesLicensedCode != null)
                {
                    string codeValue = Encryption.Decrypt(noOfPOSMachinesLicensedCode.ToString());
                    string[] codeValueArray = codeValue.Split('|');
                    if (codeValueArray[0] == siteKey)
                    {
                        licensedNumberOfPOSMachines = Convert.ToInt32(codeValueArray[1]);
                    }
                }
                else
                {
                    log.Error("null code");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                licensedNumberOfPOSMachines = 0;
            }
            log.LogMethodExit();
        }

        public void ValidateLicensedPOSMachines(int machineCount)
        {
            log.LogMethodEntry(machineCount);
            if (LicensedNumberOfPOSMachines == 0)
            { GetNoOfLicensedPOSMachines(); }
            if (machineCount > LicensedNumberOfPOSMachines)
            {
                List<ValidationError> validationErrorList = new List<ValidationError>();
                validationErrorList.Add(new ValidationError("NoOfLicensedPOSMachines", "NoOfPOSMachines", msgUtils.getMessage("Number of registered POS Machines is more than the number of licensed POS Machines")));
                throw new ValidationException("ValidateLicensedPOSMachines", validationErrorList);
            }
            log.LogMethodExit();
        }

        public void SetProductKeys()
        {
            log.LogMethodEntry();
            int siteId;
            if (env.IsCorporate)
                siteId = env.SiteId;
            else
                siteId = -1;
            DataTable keyTable = Utilities.executeDataTable(@"select featureKey,NoOfPOSMachinesLicensed, licenseKey, AuthKey
                                                         from ProductKey where site_id = @site_id or @site_id = -1",
                                                           new SqlParameter("@site_id", siteId));
            if (keyTable != null && keyTable.Rows.Count > 0)
            {
                licensedNumberOfPOSMachinesKey = keyTable.Rows[0]["NoOfPOSMachinesLicensed"].ToString();
                licensedFeaturesKey = Encryption.Encrypt(ByteToString(keyTable.Rows[0]["featureKey"]));
                licenseKey = ByteToString(keyTable.Rows[0]["licenseKey"]).TrimEnd('\0');
                authKey = ByteToString(keyTable.Rows[0]["AuthKey"]).TrimEnd('\0');
                GetNoOfLicensedPOSMachines();
            }
            log.LogMethodExit();
        }

        private string ByteToString(object columnValue)
        {
            log.LogMethodEntry();
            string columnString = System.Text.Encoding.Default.GetString(columnValue as byte[]);
            log.LogMethodExit();
            return columnString;
        }
    }
}
