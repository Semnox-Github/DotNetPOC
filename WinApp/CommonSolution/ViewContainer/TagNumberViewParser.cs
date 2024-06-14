/********************************************************************************************
 * Project Name - ViewContainer
 * Description  - CashdrawerViewContainer holds the parafait default values for a given siteId, userId and POSMachineId
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.140.00    27-Jul-2021       Girish Kundar            Created : Multicash drawer enhancement
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Printer.Cashdrawers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// 
    /// </summary>
    public class TagNumberViewParser
    {
        private static readonly Semnox.Parafait.logging.Logger log =
        new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        private readonly TagNumberViewLengthList tagNumberViewLengthList;
        private readonly ScannedTagNumberViewTrimmer scannedTagNumberViewTrimmer;
        private readonly bool reverseTagNumber;

        public TagNumberViewParser(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            tagNumberViewLengthList = new TagNumberViewLengthList(executionContext);
            scannedTagNumberViewTrimmer = new ScannedTagNumberViewTrimmer(executionContext);
            reverseTagNumber = ParafaitDefaultViewContainerList.GetParafaitDefault(executionContext,
                "REVERSE_DESKTOP_CARD_NUMBER",
                false);
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="tagNumberViewLengthList"></param>
        /// <param name="scannedTagNumberViewTrimmer"></param>
        /// <param name="reverseTagNumber"></param>
        public TagNumberViewParser(ExecutionContext executionContext,
            TagNumberViewLengthList tagNumberViewLengthList,
            ScannedTagNumberViewTrimmer scannedTagNumberViewTrimmer,
            bool reverseTagNumber)
        {
            log.LogMethodEntry(executionContext, tagNumberViewLengthList, scannedTagNumberViewTrimmer, reverseTagNumber);
            this.executionContext = executionContext;
            this.tagNumberViewLengthList = tagNumberViewLengthList;
            this.scannedTagNumberViewTrimmer = scannedTagNumberViewTrimmer;
            this.reverseTagNumber = reverseTagNumber;
            log.LogMethodExit();
        }
        public bool TryParse(string scannedTagNumberString, out TagNumberView tagNumber)
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
        private bool IsValid(string scannedTagNumberString)
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
                string errorMessage = MessageViewContainerList.GetMessage(executionContext, 2204);
                log.LogMethodExit(errorMessage);
                return errorMessage;
            }
            if (reverseTagNumber) intermediateValue = GetReversedTagNumberString(intermediateValue);
            if (scannedTagNumberViewTrimmer.CanBeTrimmedToValidTagNumberString(intermediateValue) == false)
            {
                string errorMessage = MessageViewContainerList.GetMessage(executionContext, 195, scannedTagNumberString.Length,
                    "(" + tagNumberViewLengthList + ")");
                log.LogMethodExit(errorMessage);
                return errorMessage;
            }

            intermediateValue = scannedTagNumberViewTrimmer.Trim(intermediateValue);

            if (TagNumberStringContainsOnlyZeros(intermediateValue))
            {
                string errorMessage = MessageViewContainerList.GetMessage(executionContext, 2205);
                log.LogMethodExit(errorMessage);
                return errorMessage;
            }
            log.LogMethodExit(string.Empty);
            return string.Empty;
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
        private bool TagNumberStringContainsOnlyZeros(string scannedTagNumberValue)
        {
            log.LogMethodEntry(scannedTagNumberValue);
            bool result = false;
            int nearestValidTagNumberLength =
                tagNumberViewLengthList.GetNearestTagNumberLength(scannedTagNumberValue.Length);
            if (System.Text.RegularExpressions.Regex.Matches(scannedTagNumberValue, "0").Count >=
                nearestValidTagNumberLength)
                result = true;

            log.LogMethodExit(result);
            return result;
        }
        private TagNumberView GetDefaultTagNumber()
        {
            log.LogMethodEntry();
            TagNumberView tagNumberView = new TagNumberView(executionContext,
                new string(Enumerable.Repeat('F', tagNumberViewLengthList.MinimumValidTagNumberLength).ToArray()));
            log.LogMethodExit(tagNumberView);
            return tagNumberView;
        }
        /// <summary>
        /// Parses the give tag number string
        /// </summary>.
        /// <param name="scannedTagNumberString"></param>
        /// <returns></returns>
        internal TagNumberView Parse(string scannedTagNumberString)
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
            intermediateValue = scannedTagNumberViewTrimmer.Trim(intermediateValue).ToUpper();
            TagNumberView result = new TagNumberView(executionContext, intermediateValue, tagNumberViewLengthList);
            log.LogMethodExit(result);
            return result;
        }
    }
 }
