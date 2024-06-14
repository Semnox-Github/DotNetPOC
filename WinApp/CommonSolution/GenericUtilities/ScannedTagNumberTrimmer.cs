/********************************************************************************************
 * Project Name - GenericUtilities
 * Description  - Business logic
 *
 **************
 **Version Log
 **************
 *Version     Date             Modified By            Remarks
 *********************************************************************************************
 *1.00        07-07-2019       Lakshminarayana Rao     Created
 ********************************************************************************************/

using Semnox.Core.Utilities;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Scanned tag number string trimmer.
    /// </summary>
    public class ScannedTagNumberTrimmer
    {
        private static readonly Semnox.Parafait.logging.Logger log =
            new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly int leftTrimValue;
        private readonly int rightTrimValue;
        private readonly TagNumberLengthList tagNumberLengthList;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public ScannedTagNumberTrimmer(ExecutionContext executionContext)
        {
            leftTrimValue = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "LEFT_TRIM_CARD_NUMBER", 0);
            rightTrimValue = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "RIGHT_TRIM_CARD_NUMBER", 0);
            tagNumberLengthList = new TagNumberLengthList(executionContext);
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="leftTrimValue"></param>
        /// <param name="rightTrimValue"></param>
        /// <param name="tagNumberLengthList"></param>
        public ScannedTagNumberTrimmer(int leftTrimValue, 
                                       int rightTrimValue,
                                       TagNumberLengthList tagNumberLengthList)
        {
            this.leftTrimValue = leftTrimValue;
            this.rightTrimValue = rightTrimValue;
            this.tagNumberLengthList = tagNumberLengthList;
        }

        /// <summary>
        /// Given tag number string can be trimmed to a valid tag number
        /// </summary>
        /// <param name="scannedTagNumberValue"></param>
        /// <returns></returns>
        public bool CanBeTrimmedToValidTagNumberString(string scannedTagNumberValue)
        {
            log.LogMethodEntry(scannedTagNumberValue, tagNumberLengthList);
            if (scannedTagNumberValue.Length < tagNumberLengthList.MinimumValidTagNumberLength)
            {
                log.LogMethodExit(false, "Scanned tag number length is less than the minimum valid tag number length");
                return false;
            }

            int nearestValidTagNumberLength = tagNumberLengthList.GetNearestTagNumberLength(scannedTagNumberValue.Length);
            if ((scannedTagNumberValue.Length - leftTrimValue - rightTrimValue) > nearestValidTagNumberLength)
            {
                log.LogMethodExit(false, "Scanned tag number can't be trimmed to a valid tag number");
                return false;
            }

            log.LogMethodExit(true);
            return true;
        }

        /// <summary>
        /// Trims the tag number based on the left trim and right trim configuration
        /// </summary>
        /// <param name="scannedTagNumber"></param>
        /// <returns></returns>
        public string Trim(string scannedTagNumber)
        {
            log.LogMethodEntry(scannedTagNumber);

            if (tagNumberLengthList.Contains(scannedTagNumber.Length))
            {
                log.LogMethodExit(scannedTagNumber);
                return scannedTagNumber;
            }

            int nearestValidTagNumberLength = tagNumberLengthList.GetNearestTagNumberLength(scannedTagNumber.Length);

            int intermediateLeftTrimValue = leftTrimValue;
            if (scannedTagNumber.Length - intermediateLeftTrimValue < nearestValidTagNumberLength)
            {
                intermediateLeftTrimValue = scannedTagNumber.Length - nearestValidTagNumberLength;
            }

            int intermediateRightTrimValue = rightTrimValue;
            if (scannedTagNumber.Length - intermediateLeftTrimValue == nearestValidTagNumberLength)
            {
                intermediateRightTrimValue = 0;
            }
            else if (scannedTagNumber.Length - intermediateLeftTrimValue - intermediateRightTrimValue <
                     nearestValidTagNumberLength)
            {
                intermediateRightTrimValue =
                    scannedTagNumber.Length - intermediateLeftTrimValue - nearestValidTagNumberLength;
            }

            string result = scannedTagNumber.Substring(intermediateLeftTrimValue,
                scannedTagNumber.Length - intermediateLeftTrimValue - intermediateRightTrimValue);
            log.LogMethodExit(result);
            return result;
        }
    }
}