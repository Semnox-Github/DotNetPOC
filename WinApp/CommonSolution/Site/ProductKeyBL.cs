/********************************************************************************************
 * Project Name - ProductKeyBL
 * Description  - Business logic file for  ProductKey
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.2.0      21-May-2019   Divya                   Created 
 *            13-Jun-2019   Mushahid Faizan         Modified Save() in ProductKeyListBL class.
 *            29-Aug-2019   Mushahid Faizan         Added GetExpiryDate() and GetNoOfLicensedPOSMachines() methods.
 *2.70.3      21-Apr-2020   Girish Kundar           Modified : Added method to remove spaces in all the Keys            
 *2.110       20-Dec-2020   Nitin Pai               Dashboard Changes - Dashboard License Validation has moved from HQ f HQ to HQ
 *2.120       09-Mar-2020   Girish Kundar          Modified : Licence count feature added for DashBoard(WMS changes)
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace Semnox.Parafait.Site
{
    /// <summary>
    /// Business logic for ProductKey class.
    /// </summary>
    public class ProductKeyBL
    {
        private ProductKeyDTO productKeyDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        KeyManagement keyManagementObj;

        /// <summary>
        /// Parameterized constructor of ProductKeyBL class
        /// </summary>
        public ProductKeyBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.productKeyDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates ProductKeyBL object using the ProductKeyDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="productKeyDTO">ProductKeyDTO object</param>
        public ProductKeyBL(ExecutionContext executionContext, ProductKeyDTO productKeyDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, productKeyDTO);
            this.productKeyDTO = productKeyDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates ProductKeyBL object using the 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="id"></param>
        /// <param name="sqlTransaction"></param>
        public ProductKeyBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            ProductKeyDataHandler productKeyDataHandler = new ProductKeyDataHandler(sqlTransaction);
            productKeyDTO = productKeyDataHandler.GetProductKeyDTO(id);
            log.LogMethodExit();
        }
        /// <summary>
        /// Saves the ProductKey
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ProductKeyDataHandler productKeyDataHandler = new ProductKeyDataHandler(sqlTransaction);
            Validate();
            if (productKeyDTO.Id < 0)
            {
                productKeyDTO = productKeyDataHandler.Insert(productKeyDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                productKeyDTO.AcceptChanges();
            }
            else if (productKeyDTO.IsChanged)
            {
                productKeyDTO = productKeyDataHandler.Update(productKeyDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                productKeyDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }
        public void SaveAuthKey(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ProductKeyDataHandler productKeyDataHandler = new ProductKeyDataHandler(sqlTransaction);
            if (productKeyDTO.IsChanged && productKeyDTO.Id > 0)
            {
                productKeyDTO = productKeyDataHandler.Update(productKeyDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                productKeyDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }
        private void Validate(SqlTransaction sqlTransaction = null)
        {
            List<ValidationError> validationErrorList = new List<ValidationError>();
            string siteKey = string.Empty;
            string featureKey = string.Empty;
            string existFeatureKey = string.Empty;
            string convertedFeatureKey = string.Empty;
            DateTime expiryDate = DateTime.MinValue;
            DBUtils dBUtils = new DBUtils();
            MessageUtils messageUtils = new MessageUtils(dBUtils);
            ParafaitEnv env = new ParafaitEnv(dBUtils)
            {
                SiteId = executionContext.GetSiteId(),
                IsCorporate = executionContext.GetIsCorporate(),
                LanguageId = executionContext.GetLanguageId(),
                LoginID = executionContext.GetUserId()
            };
            KeyManagement keyManagementObj = new KeyManagement(dBUtils, env);
            if (string.IsNullOrEmpty(productKeyDTO.SiteKey.ToString()))
            {
                throw new ValidationException(messageUtils.getMessage("Enter Site Key"));
            }
            if (productKeyDTO.LicenseKey.Length < 18)
            {
                throw new ValidationException(messageUtils.getMessage("Enter valid License Key"));
            }
            string convertedSiteKey = Encoding.UTF8.GetString(productKeyDTO.SiteKey, 0, productKeyDTO.SiteKey.Length).Replace("\0", string.Empty);
            string convertedLicenseKey = Encoding.UTF8.GetString(productKeyDTO.LicenseKey, 0, productKeyDTO.LicenseKey.Length).Replace("\0", string.Empty);
            if (productKeyDTO != null && productKeyDTO.FeatureKey != null && productKeyDTO.FeatureKey.Length > 0)
            {
                convertedFeatureKey = Encoding.UTF8.GetString(productKeyDTO.FeatureKey, 0, productKeyDTO.FeatureKey.Length).Replace("\0", string.Empty);
            }
            string licKey = Encryption.Decrypt(convertedLicenseKey);
            if (string.IsNullOrEmpty(licKey))
            {
                log.Error(productKeyDTO.LicenseKey.ToString());
                throw new ValidationException(messageUtils.getMessage("Please enter valid License Key"));
            }

            keyManagementObj.DecodeKey(licKey, ref siteKey, ref expiryDate);
            if (siteKey != null && siteKey != convertedSiteKey.ToString())
            {
                string errorMessage = "Invalid Key. Please enter valid License Key";
                throw new ValidationException(messageUtils.getMessage(errorMessage));
            }

            if (string.IsNullOrEmpty(productKeyDTO.NoOfPOSMachinesLicensed) == false)
            {
                string noOfLicensedPOSMachinesCode = Encryption.Decrypt(productKeyDTO.NoOfPOSMachinesLicensed);
                if (String.IsNullOrEmpty(noOfLicensedPOSMachinesCode))
                {
                    log.Error(noOfLicensedPOSMachinesCode);
                    throw new ValidationException(messageUtils.getMessage("Invalid Licensed POS key"));
                }
            }
            ProductKeyBL productKey = new ProductKeyBL(executionContext, Convert.ToInt32(productKeyDTO.Id));
            ProductKeyDTO productKeyDto = productKey.productKeyDTO;
            if (productKeyDTO != null && productKeyDTO.FeatureKey != null && productKeyDTO.FeatureKey.Length > 0)
            {
                featureKey = Encoding.UTF8.GetString(productKeyDTO.FeatureKey, 0, productKeyDTO.FeatureKey.Length).Replace("\0", string.Empty);
            }
            if (productKeyDto != null && productKeyDto.FeatureKey != null && productKeyDto.FeatureKey.Length > 0)
            {
                existFeatureKey = Encoding.UTF8.GetString(productKeyDto.FeatureKey, 0, productKeyDto.FeatureKey.Length).Replace("\0", string.Empty);
            }

            if (string.Equals(featureKey, existFeatureKey))
            {
                log.Info("No changes to feature key");
            }
            else
            {
                if (string.IsNullOrEmpty(convertedFeatureKey) == false)
                {
                    string featuresDecodedKey = Encryption.Decrypt(convertedFeatureKey);
                    if (string.IsNullOrEmpty(featuresDecodedKey))
                    {
                        log.Error(productKeyDTO.FeatureKey.ToString());
                        throw new ValidationException(messageUtils.getMessage("Invalid Feature Key"));
                    }
                    //featuresDecodedKey = Encryption.Encrypt(featuresDecodedKey);
                    productKeyDTO.FeatureKey = Encoding.UTF8.GetBytes(featuresDecodedKey);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///  Returns Expiry Date
        /// </summary>
        /// <returns></returns>
        public string GetExpiryDate()
        {
            DBUtils dBUtils = new DBUtils();
            ParafaitEnv env = new ParafaitEnv(dBUtils);

            string siteKey = string.Empty, licenseKey = string.Empty;
            string date = string.Empty;
            DateTime expiryDate = DateTime.MinValue;
            keyManagementObj = new KeyManagement(dBUtils, env);
            keyManagementObj.ReadKeysFromDB(executionContext.GetSiteId(), ref siteKey, ref licenseKey);
            if (!string.IsNullOrEmpty(siteKey)) // key found
            {
                licenseKey = Encryption.Decrypt(licenseKey);
                keyManagementObj.DecodeKey(licenseKey, ref siteKey, ref expiryDate);
                if (expiryDate == DateTime.MaxValue)
                    date = "Never";
                else
                    date = expiryDate.ToString(env.DATE_FORMAT);
            }
            return date;
        }
        /// <summary>
        /// Returns no of Licensed Pos Machines
        /// </summary>
        /// <returns></returns>
        public int GetNoOfLicensedPOSMachines()
        {
            DBUtils dBUtils = new DBUtils();
            ParafaitEnv env = new ParafaitEnv(dBUtils);
            string siteKey = string.Empty, licenseKey = string.Empty;

            keyManagementObj = new KeyManagement(dBUtils, env);
            keyManagementObj.ReadKeysFromDB(executionContext.GetSiteId(), ref siteKey, ref licenseKey);
            keyManagementObj.SetProductKeys();
            int noOfLicensedMachine = (keyManagementObj.LicensedNumberOfPOSMachines);
            return noOfLicensedMachine;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ProductKeyDTO ProductKeyDTO
        {
            get
            {
                return productKeyDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of ProductKey
    /// </summary>
    public class ProductKeyListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<ProductKeyDTO> productKeyDTOList;
        private KeyManagement keyManagementObj;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public ProductKeyListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor having ExecutionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public ProductKeyListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.productKeyDTOList = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="productKeyDTOList"></param>
        public ProductKeyListBL(ExecutionContext executionContext, List<ProductKeyDTO> productKeyDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, productKeyDTOList);
            this.productKeyDTOList = productKeyDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the ProductKeyListBL List
        /// </summary>
        public List<ProductKeyDTO> GetProductKeyDTOList(List<KeyValuePair<ProductKeyDTO.SearchByParameters, string>> searchParameters, bool loadFeatures = false,
                                                        SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);

            DBUtils dBUtils = new DBUtils();
            ParafaitEnv env = new ParafaitEnv(dBUtils);
            if (loadFeatures)
            {
                env.SiteId = executionContext.GetSiteId();
                env.IsCorporate = executionContext.GetIsCorporate();
                env.LanguageId = executionContext.GetLanguageId();
                env.LoginID = executionContext.GetUserId();
                keyManagementObj = new KeyManagement(dBUtils, env);
            }

            ProductKeyDataHandler productKeyDataHandler = new ProductKeyDataHandler(sqlTransaction);
            List<ProductKeyDTO> productKeyDTOList = productKeyDataHandler.GetAllProductKeyDTOList(searchParameters);
            if (productKeyDTOList != null && productKeyDTOList.Count != 0)
            {
                foreach (ProductKeyDTO productKeyDTO in productKeyDTOList)
                {
                    if (loadFeatures)
                    {
                        for (int i = 0; i < keyManagementObj.licensedFeatures.Length; i++)
                        {
                            if (keyManagementObj.FeatureValid(executionContext.GetSiteId(), keyManagementObj.licensedFeatures[i].ToString()))
                            {
                                if (keyManagementObj.IsLicenceCountTypeFeature(keyManagementObj.licensedFeatures[i].ToString()))
                                {
                                    int licenceCount = keyManagementObj.GetLicenceCount(executionContext.GetSiteId(), keyManagementObj.licensedFeatures[i].ToString());
                                    productKeyDTO.AddOnFeatures.Add(new KeyValuePair<string, int>(keyManagementObj.licensedFeatures[i].ToString(), licenceCount));
                                }
                                else
                                {
                                    productKeyDTO.AddOnFeatures.Add(new KeyValuePair<string, int>(keyManagementObj.licensedFeatures[i].ToString(), 1));
                                }
                            }
                            else
                            {
                                productKeyDTO.AddOnFeatures.Add(new KeyValuePair<string, int>(keyManagementObj.licensedFeatures[i].ToString(), 0));
                            }
                        }
                    }
                    RemoveSpacesInKeys(productKeyDTO);
                }
            }
            log.LogMethodExit(productKeyDTOList);
            return productKeyDTOList;
        }



        private void RemoveSpacesInKeys(ProductKeyDTO productKeyDTO)
        {
            log.LogMethodEntry();
            byte[] buffer;
            string siteKey = string.Empty, licenseKey = string.Empty, featureKey = string.Empty;
            buffer = (byte[])productKeyDTO.SiteKey;
            for (int i = 0; i < buffer.Length; i++)
            {
                if (buffer[i] != 0)
                    siteKey += (char)buffer[i];
            }
            productKeyDTO.SiteKey = Encoding.ASCII.GetBytes(siteKey);

            buffer = (byte[])productKeyDTO.LicenseKey;
            for (int i = 0; i < buffer.Length; i++)
            {
                if (buffer[i] != 0)
                    licenseKey += (char)buffer[i];
            }
            productKeyDTO.LicenseKey = Encoding.ASCII.GetBytes(licenseKey);

            buffer = (byte[])productKeyDTO.FeatureKey;
            for (int i = 0; i < buffer.Length; i++)
            {
                if (buffer[i] != 0)
                    featureKey += (char)buffer[i];
            }
            productKeyDTO.FeatureKey = Encoding.ASCII.GetBytes(featureKey);
            log.LogMethodExit();
        }

        /// <summary>
        /// List Save method
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(bool authKeyUpdate)
        {
            log.LogMethodEntry();
            if (productKeyDTOList != null && productKeyDTOList.Count > 0)
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (ProductKeyDTO productKeyDTO in productKeyDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            ProductKeyBL productKeyBL = new ProductKeyBL(executionContext, productKeyDTO);
                            if (authKeyUpdate)
                            {
                                productKeyBL.SaveAuthKey(parafaitDBTrx.SQLTrx);
                            }
                            else
                            {
                                productKeyBL.Save(parafaitDBTrx.SQLTrx);
                            }
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw;
                        }
                        catch (SqlException sqlEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(sqlEx);
                            log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                            throw new ValidationException("Please enter Authkey to proceed");
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw;
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// method to validate license
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public bool ValidateLicenseCount(string featureName, int numberOfLicences)
        {
            DBUtils dBUtils = new DBUtils();
            ParafaitEnv env = new ParafaitEnv(dBUtils)
            {
                SiteId = executionContext.GetSiteId(),
                IsCorporate = executionContext.GetIsCorporate(),
                LanguageId = executionContext.GetLanguageId(),
                LoginID = executionContext.GetUserId()
            };

            KeyManagement keyManagement = new KeyManagement(dBUtils, env);
            if (keyManagement.FeatureValid(featureName, numberOfLicences))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}