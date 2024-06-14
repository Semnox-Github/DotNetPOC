/********************************************************************************************
 * Project Name - GenericUtilities                                                                          
 * Description  - Represents a tag number parser. Parses a scanned tag number string
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 ********************************************************************************************* 
 *2.50.0      14-June-2019   Lakshminarayana      Created
 *2.70.2        12-Aug-2019    Deeksha              Added logger methods.
 ********************************************************************************************/
using System;
using System.Linq;
using System.Collections.Generic;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Represents a tag number parser. Parses a scanned tag number string
    /// </summary>
    public class TagNumberParser
    {
        private static readonly Semnox.Parafait.logging.Logger log =
            new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        private readonly TagNumberLengthList tagNumberLengthList;
        private readonly ScannedTagNumberTrimmer scannedTagNumberTrimmer;
        private readonly bool reverseTagNumber;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public TagNumberParser(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            tagNumberLengthList = new TagNumberLengthList(executionContext);
            scannedTagNumberTrimmer = new ScannedTagNumberTrimmer(executionContext);
            reverseTagNumber = ParafaitDefaultContainerList.GetParafaitDefault(executionContext,
                "REVERSE_DESKTOP_CARD_NUMBER",
                false);
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="tagNumberLengthList"></param>
        /// <param name="scannedTagNumberTrimmer"></param>
        /// <param name="reverseTagNumber"></param>
        public TagNumberParser(ExecutionContext executionContext,
            TagNumberLengthList tagNumberLengthList,
            ScannedTagNumberTrimmer scannedTagNumberTrimmer,
            bool reverseTagNumber)
        {
            log.LogMethodEntry(executionContext, tagNumberLengthList, scannedTagNumberTrimmer, reverseTagNumber);
            this.executionContext = executionContext;
            this.tagNumberLengthList = tagNumberLengthList;
            this.scannedTagNumberTrimmer = scannedTagNumberTrimmer;
            this.reverseTagNumber = reverseTagNumber;
            log.LogMethodExit();
        }


        public bool TryParse(string scannedTagNumberString, out TagNumber tagNumber)
        {
            log.LogMethodEntry(scannedTagNumberString);
            try
            {
                if (IsValid(scannedTagNumberString) == false)
                {
                    tagNumber = GetDefaultTagNumber();
                    log.LogMethodExit(false);
                    return false;
                }
                tagNumber = Parse(scannedTagNumberString);
            }
            catch (Exception ex)
            {
                tagNumber = GetDefaultTagNumber();
                log.Error("Error occurred while parsing the scanned tag number", ex);
                log.LogMethodExit(false);
                return false;
            }
            log.LogMethodExit(true);
            return true;
        }

        private TagNumber GetDefaultTagNumber()
        {
            log.LogMethodEntry();
            TagNumber tagNumber = new TagNumber(executionContext,
                new string(Enumerable.Repeat('F', tagNumberLengthList.MinimumValidTagNumberLength).ToArray()));
            log.LogMethodExit(tagNumber);
            return tagNumber;
        }

        /// <summary>
        /// Parses the give tag number string
        /// </summary>.
        /// <param name="scannedTagNumberString"></param>
        /// <returns></returns>
        public TagNumber Parse(string scannedTagNumberString)
        {
            log.LogMethodEntry(scannedTagNumberString);
            string errorMessage = Validate(scannedTagNumberString);
            if (string.IsNullOrWhiteSpace(errorMessage) == false)
            {
                log.LogMethodExit(null, "Throwing Exception -" + errorMessage);
                throw new Exception(errorMessage);
            }
            string intermediateValue = RemoveWhiteSpaces(scannedTagNumberString);
            if (reverseTagNumber) intermediateValue = GetReversedTagNumberString(intermediateValue);
            intermediateValue = scannedTagNumberTrimmer.Trim(intermediateValue).ToUpper();
            TagNumber result = new TagNumber(executionContext, intermediateValue, tagNumberLengthList);
            log.LogMethodExit(result);
            return result;
        }
        
        private bool TagNumberStringContainsOnlyZeros(string scannedTagNumberValue)
        {
            log.LogMethodEntry(scannedTagNumberValue);
            bool result = false;
            int nearestValidTagNumberLength =
                tagNumberLengthList.GetNearestTagNumberLength(scannedTagNumberValue.Length);
            if (System.Text.RegularExpressions.Regex.Matches(scannedTagNumberValue, "0").Count >=
                nearestValidTagNumberLength)
                result = true;

            log.LogMethodExit(result);
            return result;
        }

        private static string RemoveWhiteSpaces(string scannedTagNumber)
        {
            log.LogMethodEntry(scannedTagNumber);
            string returnvalue = string.IsNullOrWhiteSpace(scannedTagNumber)
                ? string.Empty
                : System.Text.RegularExpressions.Regex.Replace(scannedTagNumber, @"\W+", string.Empty);
            log.LogMethodExit(returnvalue);
            return returnvalue;
        }

        private static string GetReversedTagNumberString(string scannedTagNumberValue)
        {
            log.LogMethodEntry(scannedTagNumberValue);
            if (string.IsNullOrWhiteSpace(scannedTagNumberValue))
            {
                log.LogMethodExit(scannedTagNumberValue);
                return scannedTagNumberValue;
            }
            char[] array = scannedTagNumberValue.ToCharArray();
            int length = scannedTagNumberValue.Length;
            for (int i = 0; i < length / 2; i += 2)
            {
                char x = array[i];
                char y = array[i + 1];
                array[i] = array[length - i - 2];
                array[i + 1] = array[length - i - 1];
                array[length - i - 2] = x;
                array[length - i - 1] = y;
            }

            string result = new string(array);
            log.LogMethodExit(result);
            return result;
        }

        public bool IsValid(string scannedTagNumberString)
        {
            log.LogMethodEntry(scannedTagNumberString);
            bool returnValue = string.IsNullOrWhiteSpace(Validate(scannedTagNumberString));
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        public string Validate(string scannedTagNumberString)
        {
            log.LogMethodEntry(scannedTagNumberString);
            string intermediateValue = RemoveWhiteSpaces(scannedTagNumberString);

            if (string.IsNullOrWhiteSpace(intermediateValue))
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2204);
                log.LogMethodExit(errorMessage);
                return errorMessage;
            }
            if (reverseTagNumber) intermediateValue = GetReversedTagNumberString(intermediateValue);
            if (scannedTagNumberTrimmer.CanBeTrimmedToValidTagNumberString(intermediateValue) == false)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 195, scannedTagNumberString.Length,
                    "(" + tagNumberLengthList + ")");
                log.LogMethodExit(errorMessage);
                return errorMessage;
            }

            intermediateValue = scannedTagNumberTrimmer.Trim(intermediateValue);

            if (TagNumberStringContainsOnlyZeros(intermediateValue))
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2205);
                log.LogMethodExit(errorMessage);
                return errorMessage;
            }
            log.LogMethodExit(string.Empty);
            return  string.Empty;
        }

        /// <summary>
        /// Check if device is applicable to receive encrypted tags (QR Code)
        /// </summary>
        /// <param name="readerDevice">device instance</param>
        /// <param name="tagLength">length of encrypted tag received</param>
        /// <returns>true or false</returns>
        public bool IsTagDecryptApplicable(DeviceClass readerDevice, int tagLength)
        {
            log.LogMethodEntry(tagLength);
            if (readerDevice.DeviceDefinition != null
                && readerDevice.DeviceDefinition.EnableTagDecryption)
            {
                HashSet<int> excludedTagLengths = GetExcludedTagNumberLengthListFromString(readerDevice.DeviceDefinition.ExcludeDecryptionForTagLength);
                if (excludedTagLengths.Contains(tagLength))
                {
                    log.LogMethodExit(false);
                    return false;
                }
                log.LogMethodExit(true);
                return true;
            }
            else
            {
                log.LogMethodExit(false);
                return false;
            }
        }

        /// <summary>
        /// Decrypt the encrypted QR code and return decrypted string
        /// This will have QR type, Site, Tag Number and UTC
        /// </summary>
        /// <param name="readerDevice">Device Instance</param>
        /// <param name="tagNumber">Tag Number received by Listener</param>
        /// <returns></returns>
        public string GetDecryptedTagData(DeviceClass readerDevice, string tagNumber)
        {
            log.LogMethodEntry(readerDevice, tagNumber);
            if (IsTagDecryptApplicable(readerDevice, tagNumber.Length))
            {
                string strParafaitKey = Encryption.GetParafaitKeys("ParafaitEncryption");
                string decryptedTagNumber = Encryption.Decrypt(tagNumber, strParafaitKey);
                log.LogMethodExit(decryptedTagNumber);
                return decryptedTagNumber;
            }
            else
            {
                log.LogMethodExit("Tag Number cannot be decrypted. Invalid Tag for decryption");
                throw (new ValidationException("Tag Number cannot be decrypted"));
            }
        }

        /// <summary>
        /// Decrypt QR code based tag number
        /// </summary>
        /// <param name="decryptedTagNumber">Decrypted Tag code</param>
        /// <param name="siteId">Current site id</param>
        /// <returns>decrypted tag number</returns>
        public string ValidateDecryptedTag(string decryptedTagNumber, int siteId)
        {
            //T|XXXXXXXX|Site|YYYYMMDDHHMMSS
            log.LogMethodEntry(decryptedTagNumber, siteId);
            string[] decryptedTagNumberList = decryptedTagNumber.Split(new char[] { '|' });
            if (decryptedTagNumberList.Length <= 1)
                throw new ValidationException("QR Code is invalid!");
            if (decryptedTagNumberList[0] != "T")
            {
                log.LogMethodExit("QR Transaction Type: " + decryptedTagNumber.Substring(0, 1));
                throw new ValidationException("QR Code needs to be of Transaction type!");
            }
            int accountValidityThreshold = Int32.Parse(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "THRESHOLD_MINS_FOR_ACCOUNT_QR_CODE_VALIDATION"));
            string tagNumber = decryptedTagNumberList[1];
            string tagSiteId = decryptedTagNumberList[2];
            if (Convert.ToInt32(tagSiteId) != siteId)
            {
                log.LogMethodExit("QR Code site id: " + tagSiteId + " does not match current site: " + siteId.ToString());
                throw new ValidationException("QR Code does not belong to current site!");
            }
            DateTime utcDateTime = DateTime.ParseExact(decryptedTagNumberList[3], "yyyyMMddHHmmss", null);
            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
            DateTime currentLocalTime = lookupValuesList.GetServerDateTime();
            DateTime decryptedTime = utcDateTime.ToLocalTime();
            TimeSpan timeDiff = currentLocalTime - decryptedTime;
            if (timeDiff.TotalMinutes < -accountValidityThreshold 
                || timeDiff.TotalMinutes > accountValidityThreshold) //5 minute tolerance
            {
                log.LogMethodExit("QR Code time validation failed. Total Difference with current time: " + Convert.ToInt32(timeDiff.TotalMinutes) + " minutes.");
                throw new ValidationException("QR Code time validation failed. Total Difference with current time: " + Convert.ToInt32(timeDiff.TotalMinutes) + " minutes.");
            }
            return tagNumber;
        }

        protected HashSet<int> GetExcludedTagNumberLengthListFromString(string pipeSeparatedTagNumberLength)
        {
            log.LogMethodEntry(pipeSeparatedTagNumberLength);
            HashSet<int> result = new HashSet<int>();
            string[] tagNumberLengthStringList =
                pipeSeparatedTagNumberLength.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string tagNumberLengthString in tagNumberLengthStringList)
            {
                int tagNumberLength;
                if (int.TryParse(tagNumberLengthString, out tagNumberLength) && tagNumberLength > 0)
                {
                    result.Add(tagNumberLength);
                }
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}